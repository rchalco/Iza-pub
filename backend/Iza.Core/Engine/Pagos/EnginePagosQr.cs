using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Iza.Core.Domain.Pagos;
using Iza.Core.Integration.Baneco;
using PlumbingProps.Exceptions;
using PlumbingProps.Wrapper;

namespace Iza.Core.Engine.Pagos
{
    /// <summary>
    /// Cobros por QR de Banco Economico. Iza habla unicamente con el proxy qr-banco-economico: no
    /// conoce las credenciales del banco, ni su clave AES, ni la cuenta a acreditar. Ese aislamiento es
    /// el motivo de que exista el proxy.
    /// <para>
    /// No hereda de <c>BaseManager</c>: la operacion no toca DBPubIZA y abrir un repositorio por
    /// solicitud seria una conexion de mas a la base de produccion.
    /// </para>
    /// </summary>
    public class EnginePagosQr
    {
        private readonly QrBancoEconomicoOptions _opciones;

        public EnginePagosQr() : this(QrBancoEconomicoOptions.Actual) { }

        /// <summary>Sobrecarga para pruebas: permite inyectar una configuracion distinta a la del archivo.</summary>
        public EnginePagosQr(QrBancoEconomicoOptions opciones) => _opciones = opciones;

        /// <summary>
        /// Devuelve el QR de cobro vigente de la sucursal, emitiendolo contra el banco solo si no hay uno
        /// en cache o si el vigente esta por vencer. El QR dura <c>QrBancoEconomico:DiasVigencia</c> dias,
        /// es reutilizable y de monto editable.
        /// </summary>
        public async Task<ResponseObject<QrMensualDTO>> ObtieneQrMensual(RequestQrMensual request, CancellationToken ct = default)
        {
            var response = new ResponseObject<QrMensualDTO>
            {
                Message = "QR de cobro obtenido correctamente.",
                State = ResponseType.Success
            };

            try
            {
                request ??= new RequestQrMensual();

                string moneda = Preferido(request.moneda, _opciones.Moneda);
                string sucursal = Preferido(request.codigoSucursal, _opciones.CodigoSucursal);

                if (string.IsNullOrWhiteSpace(sucursal))
                {
                    // Sin sucursal todas las cajas compartirian una unica entrada de cache y el reporte de
                    // cobros no permitiria saber donde se pago.
                    response.State = ResponseType.Error;
                    response.Message = "No se indico la sucursal y no hay 'QrBancoEconomico:CodigoSucursal' configurada.";
                    return response;
                }

                var cliente = new QrBancoEconomicoClient(_opciones);
                string clave = QrMensualCache.Clave(sucursal, moneda);

                response.Data = await QrMensualCache.ObtenerOEmitirAsync(
                    clave,
                    request.forzarRenovacion,
                    () => EmitirAsync(cliente, sucursal, moneda, ct),
                    ct);

                if (response.Data.desdeCache)
                    response.Message = "QR de cobro vigente recuperado de cache.";
            }
            catch (QrBancoEconomicoException ex)
            {
                // Falla esperada de la integracion: el mensaje ya es accionable y no lleva secretos.
                response.State = ResponseType.Error;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.State = ResponseType.Error;
                response.Message = new ManagerException().ProcessException(ex);
            }

            return response;
        }

        /// <summary>
        /// Lista los cobros acreditados en una fecha sobre los QR emitidos por Iza. El banco reporta por
        /// dia; para un rango hay que iterar dia a dia.
        /// </summary>
        public async Task<ResponseQuery<QrPagadoDTO>> ObtieneQrsPagados(RequestQrsPagados request, CancellationToken ct = default)
        {
            var response = new ResponseQuery<QrPagadoDTO>
            {
                Message = "Reporte de QR pagados obtenido correctamente.",
                State = ResponseType.Success
            };

            try
            {
                request ??= new RequestQrsPagados();
                DateTime fecha = request.fecha == default ? DateTime.Today : request.fecha;

                if (fecha.Date > DateTime.Today)
                {
                    response.State = ResponseType.Error;
                    response.Message = "La fecha del reporte no puede ser futura.";
                    return response;
                }

                var cliente = new QrBancoEconomicoClient(_opciones);
                IReadOnlyList<QrBancoEconomicoClient.QrPagadoResponseAdapter> pagos =
                    await cliente.ObtenerQrsPagadosAsync(DateOnly.FromDateTime(fecha), ct);

                foreach (QrBancoEconomicoClient.QrPagadoResponseAdapter pago in pagos)
                {
                    response.ListEntities.Add(new QrPagadoDTO
                    {
                        qrId = pago.QrId,
                        transactionId = pago.TransactionId,
                        fechaPago = pago.FechaPago.ToDateTime(TimeOnly.MinValue),
                        horaPago = pago.HoraPago,
                        moneda = pago.Moneda,
                        monto = pago.Monto,
                        codigoBancoOrigen = pago.CodigoBancoOrigen,
                        nombrePagador = pago.NombrePagador,
                        documentoPagador = pago.DocumentoPagador,
                        cuentaPagador = pago.CuentaPagador,
                        descripcion = pago.Descripcion,
                        codigoSucursal = pago.CodigoSucursal
                    });
                }

                if (response.ListEntities.Count == 0)
                {
                    response.State = ResponseType.NoData;
                    response.Message = $"No hay cobros por QR registrados el {fecha:dd/MM/yyyy}.";
                }
            }
            catch (QrBancoEconomicoException ex)
            {
                response.State = ResponseType.Error;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.State = ResponseType.Error;
                response.Message = new ManagerException().ProcessException(ex);
            }

            return response;
        }

        private async Task<QrMensualDTO> EmitirAsync(
            QrBancoEconomicoClient cliente, string sucursal, string moneda, CancellationToken ct)
        {
            DateTime emision = DateTime.Today;
            DateTime vencimiento = emision.AddDays(_opciones.DiasVigencia);
            DateTime renovacion = vencimiento.AddDays(-_opciones.DiasMargenRenovacion);

            string transactionId = NuevoTransactionId(sucursal, emision);
            string descripcion = $"Cobro QR Iza {sucursal} {emision:MM/yyyy}";

            (string qrId, string imagen) = await cliente.GenerarQrAsync(
                transactionId, moneda, DateOnly.FromDateTime(vencimiento), descripcion, sucursal, ct);

            return new QrMensualDTO
            {
                qrId = qrId,
                qrImageBase64 = imagen,
                transactionId = transactionId,
                moneda = moneda,
                codigoSucursal = sucursal,
                fechaEmision = DateTime.Now,
                fechaVencimiento = vencimiento,
                fechaRenovacion = renovacion,
                montoEditable = true,
                desdeCache = false
            };
        }

        /// <summary>
        /// Identificador unico por suscriptor que exige el proxy. Lleva sufijo aleatorio porque una
        /// renovacion forzada el mismo dia y la misma sucursal produciria una colision (409) si solo
        /// dependiera de la fecha.
        /// </summary>
        private static string NuevoTransactionId(string sucursal, DateTime emision) =>
            $"IZA-{sucursal}-{emision:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        private static string Preferido(string? valor, string porDefecto) =>
            string.IsNullOrWhiteSpace(valor) ? porDefecto : valor.Trim();
    }
}
