using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Iza.Core.Integration.Baneco
{
    /// <summary>
    /// Cliente del proxy qr-banco-economico. Expone solo lo que el proxy expone a un suscriptor:
    /// emitir un QR de cobro y listar los cobros de un dia. El proxy no tiene consulta de estado por
    /// QR, asi que verificar un cobro puntual es filtrar el reporte del dia por qrId.
    /// Iza nunca habla directo con Baneco ni maneja el token del banco: eso queda del lado del proxy.
    /// </summary>
    public sealed class QrBancoEconomicoClient
    {
        private const string CabeceraApiKey = "X-Api-Key";
        private const string CabeceraCuenta = "X-Baneco-Account";

        /// <summary>
        /// Una sola instancia por proceso. Crear un <see cref="HttpClient"/> por llamada agota los
        /// puertos efimeros bajo carga: cada socket queda en TIME_WAIT y el pool no se reutiliza.
        /// </summary>
        private static readonly HttpClient Http = ConstruirHttpClient();

        private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

        private readonly QrBancoEconomicoOptions _opciones;

        public QrBancoEconomicoClient(QrBancoEconomicoOptions opciones)
        {
            _opciones = opciones ?? throw new ArgumentNullException(nameof(opciones));

            string? error = _opciones.Validar();
            if (error is not null) throw new QrBancoEconomicoException(error);
        }

        private static HttpClient ConstruirHttpClient()
        {
            var handler = new SocketsHttpHandler
            {
                // Renueva las conexiones para que un cambio de DNS del proxy se tome sin reiniciar Iza.
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                AutomaticDecompression = DecompressionMethods.All
            };

            return new HttpClient(handler);
        }

        /// <summary>
        /// Emite un QR de cobro para una venta contra el proxy. Devuelve el identificador del banco y la
        /// imagen en Base64.
        /// </summary>
        public async Task<(string QrId, string QrImageBase64)> GenerarQrAsync(
            string transactionId, string moneda, decimal monto, DateOnly fechaVencimiento,
            string? descripcion, string? codigoSucursal, CancellationToken ct = default)
        {
            // De un solo uso y de monto fijo. Es lo que hace que el cobro sea verificable por qrId: con
            // modifyAmount = true el pagador podria alterar el importe, y con singleUse = false un mismo
            // codigo acumularia cobros de varias ventas y el qrId dejaria de identificar a una sola.
            var cuerpo = new GenerarQrRequest(
                TransactionId: transactionId,
                Currency: moneda,
                Amount: monto,
                Description: descripcion,
                DueDate: fechaVencimiento,
                SingleUse: true,
                ModifyAmount: false,
                BranchCode: string.IsNullOrWhiteSpace(codigoSucursal) ? null : codigoSucursal);

            using var solicitud = Solicitud(HttpMethod.Post, "api/baneco/qrs");
            // Se serializa primero para que la solicitud lleve Content-Length. JsonContent.Create
            // escribe en streaming y sale chunked, que algunos balanceadores y WAF rechazan.
            solicitud.Content = new StringContent(JsonSerializer.Serialize(cuerpo, Json), Encoding.UTF8, "application/json");

            GenerarQrResponse respuesta = await EnviarAsync<GenerarQrResponse>(solicitud, "generar el QR", ct);

            if (respuesta.ResponseCode != 0)
                throw new QrBancoEconomicoException($"Baneco rechazo la emision del QR: {Mensaje(respuesta.Message)}");

            if (string.IsNullOrWhiteSpace(respuesta.QrId) || string.IsNullOrWhiteSpace(respuesta.QrImage))
                throw new QrBancoEconomicoException("El proxy respondio sin qrId o sin imagen del QR.");

            return (respuesta.QrId!, respuesta.QrImage!);
        }

        /// <summary>
        /// Lista los QR cobrados de una fecha. El proxy ya los filtra a los emitidos por este
        /// suscriptor, de modo que la respuesta no expone la operativa de otros canales de la cuenta.
        /// </summary>
        public async Task<IReadOnlyList<QrPagadoResponseAdapter>> ObtenerQrsPagadosAsync(DateOnly fecha, CancellationToken ct = default)
        {
            using var solicitud = Solicitud(HttpMethod.Get, $"api/baneco/qrs/paid/{fecha:yyyy-MM-dd}");

            List<QrPagadoResponse>? pagos = await EnviarAsync<List<QrPagadoResponse>>(solicitud, "consultar los QR pagados", ct);

            var resultado = new List<QrPagadoResponseAdapter>(pagos?.Count ?? 0);
            foreach (QrPagadoResponse pago in pagos ?? [])
                resultado.Add(new QrPagadoResponseAdapter(pago));

            return resultado;
        }

        private HttpRequestMessage Solicitud(HttpMethod metodo, string rutaRelativa)
        {
            var solicitud = new HttpRequestMessage(metodo, _opciones.Ruta(rutaRelativa));
            solicitud.Headers.Add(CabeceraApiKey, _opciones.ApiKey);

            if (!string.IsNullOrWhiteSpace(_opciones.CuentaBaneco))
                solicitud.Headers.Add(CabeceraCuenta, _opciones.CuentaBaneco);

            return solicitud;
        }

        private async Task<T> EnviarAsync<T>(HttpRequestMessage solicitud, string accion, CancellationToken ct)
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
            timeout.CancelAfter(TimeSpan.FromSeconds(_opciones.TimeoutSegundos));

            HttpResponseMessage respuesta;
            try
            {
                respuesta = await Http.SendAsync(solicitud, HttpCompletionOption.ResponseContentRead, timeout.Token);
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                throw new QrBancoEconomicoException(
                    $"El proxy QR no respondio en {_opciones.TimeoutSegundos} s al {accion}.");
            }
            catch (HttpRequestException ex)
            {
                // El mensaje del socket no menciona la API Key: es seguro propagarlo al operador.
                throw new QrBancoEconomicoException($"No se pudo contactar al proxy QR para {accion}: {ex.Message}", ex);
            }

            using (respuesta)
            {
                string cuerpo = await respuesta.Content.ReadAsStringAsync(ct);

                if (!respuesta.IsSuccessStatusCode)
                    throw new QrBancoEconomicoException(DescribirError(respuesta.StatusCode, cuerpo, accion));

                try
                {
                    T? datos = JsonSerializer.Deserialize<T>(cuerpo, Json);
                    if (datos is null)
                        throw new QrBancoEconomicoException($"El proxy QR devolvio una respuesta vacia al {accion}.");

                    return datos;
                }
                catch (JsonException ex)
                {
                    throw new QrBancoEconomicoException($"El proxy QR devolvio una respuesta ilegible al {accion}.", ex);
                }
            }
        }

        /// <summary>
        /// Traduce el estado HTTP a una causa concreta. Sin esto, un 401 por API Key vencida y un 409
        /// por cuenta de ahorro faltante llegarian al cajero como el mismo "error de servicio".
        /// </summary>
        private static string DescribirError(HttpStatusCode estado, string cuerpo, string accion)
        {
            string detalle = ExtraerMensaje(cuerpo);

            string causa = estado switch
            {
                HttpStatusCode.Unauthorized =>
                    "la API Key de Iza es invalida, fue revocada o corresponde a otro ambiente",
                HttpStatusCode.Forbidden =>
                    "la API Key de Iza no tiene los alcances 'qr:write'/'qr:read' o la cuenta no le fue concedida",
                HttpStatusCode.Conflict =>
                    "el proxy rechazo la operacion por conflicto (transactionId repetido o suscriptor sin cuenta de ahorro cargada)",
                HttpStatusCode.TooManyRequests =>
                    "se supero la cuota por minuto del suscriptor; reintente en unos segundos",
                HttpStatusCode.BadGateway =>
                    "Baneco no respondio correctamente al proxy",
                _ => $"el proxy respondio {(int)estado}"
            };

            return string.IsNullOrWhiteSpace(detalle)
                ? $"No se pudo {accion}: {causa}."
                : $"No se pudo {accion}: {causa}. Detalle: {detalle}";
        }

        private static string ExtraerMensaje(string cuerpo)
        {
            if (string.IsNullOrWhiteSpace(cuerpo)) return string.Empty;

            try
            {
                ApiResultResponse? resultado = JsonSerializer.Deserialize<ApiResultResponse>(cuerpo, Json);
                if (!string.IsNullOrWhiteSpace(resultado?.Message)) return resultado!.Message!;
            }
            catch (JsonException)
            {
                // El proxy tambien devuelve texto plano y ValidationProblemDetails; se usa el cuerpo crudo.
            }

            string recortado = cuerpo.Trim();
            return recortado.Length > 300 ? recortado[..300] + "..." : recortado;
        }

        private static string Mensaje(string? mensaje) =>
            string.IsNullOrWhiteSpace(mensaje) ? "sin detalle" : mensaje!;

        /// <summary>Vista publica de un cobro; evita filtrar los tipos internos del cliente.</summary>
        public sealed class QrPagadoResponseAdapter
        {
            internal QrPagadoResponseAdapter(QrPagadoResponse origen)
            {
                QrId = origen.QrId;
                TransactionId = origen.TransactionId;
                FechaPago = origen.PaymentDate;
                HoraPago = origen.PaymentTime;
                Moneda = origen.Currency ?? string.Empty;
                Monto = origen.Amount;
                CodigoBancoOrigen = origen.SenderBankCode;
                NombrePagador = origen.SenderName;
                DocumentoPagador = origen.SenderDocumentId;
                CuentaPagador = origen.SenderAccount;
                Descripcion = origen.Description;
                CodigoSucursal = origen.BranchCode;
            }

            public string QrId { get; }
            public string? TransactionId { get; }
            public DateOnly FechaPago { get; }
            public string? HoraPago { get; }
            public string Moneda { get; }
            public decimal Monto { get; }
            public string? CodigoBancoOrigen { get; }
            public string? NombrePagador { get; }
            public string? DocumentoPagador { get; }
            public string? CuentaPagador { get; }
            public string? Descripcion { get; }
            public string? CodigoSucursal { get; }
        }
    }
}
