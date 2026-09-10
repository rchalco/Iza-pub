using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Iza.Core.Domain.Pagos;

namespace Iza.Core.Integration.Baneco
{
    /// <summary>
    /// Guarda el QR vigente de cada sucursal para no pedir uno nuevo al banco en cada venta.
    /// <para>
    /// Es <b>estatica</b> a proposito: los engines de Iza se instancian por solicitud, asi que una cache
    /// de instancia no sobreviviria a la llamada y todas las ventas emitirian un QR distinto.
    /// </para>
    /// <para>
    /// Vive en la memoria del proceso. Con varias instancias de Iza.Services detras de un balanceador
    /// cada una emite su propio QR del mes; todos son validos y acreditan la misma cuenta, pero el
    /// cliente puede ver codigos distintos segun la instancia que lo atienda. Si eso molesta, el
    /// siguiente paso es persistir la entrada (tabla propia o Redis) en lugar de este diccionario.
    /// </para>
    /// </summary>
    public static class QrMensualCache
    {
        private static readonly ConcurrentDictionary<string, QrMensualDTO> Entradas = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Un cerrojo por clave. Sin el, una rafaga de cajeros al vencer el QR dispararia tantas
        /// emisiones contra Baneco como solicitudes concurrentes, y cada una devolveria un codigo distinto.
        /// </summary>
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> Cerrojos = new(StringComparer.OrdinalIgnoreCase);

        public static string Clave(string codigoSucursal, string moneda) => $"{codigoSucursal}|{moneda}";

        /// <summary>
        /// Devuelve el QR vigente de la clave, emitiendo uno nuevo solo si no hay o si el vigente entro
        /// en su margen de renovacion. <paramref name="emitir"/> se ejecuta a lo sumo una vez por clave
        /// y por renovacion.
        /// </summary>
        public static async Task<QrMensualDTO> ObtenerOEmitirAsync(
            string clave, bool forzarRenovacion, Func<Task<QrMensualDTO>> emitir, CancellationToken ct = default)
        {
            if (!forzarRenovacion && Entradas.TryGetValue(clave, out QrMensualDTO? vigente) && EstaVigente(vigente))
                return Copiar(vigente, desdeCache: true);

            SemaphoreSlim cerrojo = Cerrojos.GetOrAdd(clave, _ => new SemaphoreSlim(1, 1));
            await cerrojo.WaitAsync(ct);
            try
            {
                // Segunda verificacion: mientras se esperaba el cerrojo otra solicitud pudo emitirlo.
                // Una renovacion forzada siempre emite; es un acto deliberado del operador, no un fallo
                // de cache, y quedaria sin efecto si aqui reutilizara lo que acaba de reemplazar.
                if (!forzarRenovacion && Entradas.TryGetValue(clave, out QrMensualDTO? recien) && EstaVigente(recien))
                    return Copiar(recien, desdeCache: true);

                QrMensualDTO emitido = await emitir();
                Entradas[clave] = emitido;
                return Copiar(emitido, desdeCache: false);
            }
            finally
            {
                cerrojo.Release();
            }
        }

        /// <summary>Descarta la entrada. Util cuando el banco invalida un QR fuera de banda.</summary>
        public static void Invalidar(string clave) => Entradas.TryRemove(clave, out _);

        private static bool EstaVigente(QrMensualDTO entrada) =>
            !string.IsNullOrWhiteSpace(entrada.qrId) &&
            !string.IsNullOrWhiteSpace(entrada.qrImageBase64) &&
            DateTime.Today < entrada.fechaRenovacion.Date;

        /// <summary>
        /// Entrega una copia: el DTO viaja al controlador, que lo serializa, y <c>desdeCache</c> difiere
        /// entre quien lo emitio y quien lo reutiliza. Mutar la entrada compartida seria una condicion
        /// de carrera visible en la respuesta.
        /// </summary>
        private static QrMensualDTO Copiar(QrMensualDTO origen, bool desdeCache) => new()
        {
            qrId = origen.qrId,
            qrImageBase64 = origen.qrImageBase64,
            transactionId = origen.transactionId,
            moneda = origen.moneda,
            codigoSucursal = origen.codigoSucursal,
            fechaEmision = origen.fechaEmision,
            fechaVencimiento = origen.fechaVencimiento,
            fechaRenovacion = origen.fechaRenovacion,
            montoEditable = origen.montoEditable,
            desdeCache = desdeCache
        };
    }
}
