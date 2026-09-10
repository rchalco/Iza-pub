using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Cada venta emite su propio QR, de un solo uso y de monto fijo. Por eso el cobro se verifica por
    /// <c>qrId</c>: un pago acreditado sobre ese codigo pertenece a esa venta y a ninguna otra.
    /// </para>
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
        /// Emite un QR de cobro para una venta. Es de un solo uso y de monto fijo: el pagador no puede
        /// alterar el importe ni volver a usar el codigo.
        /// </summary>
        public async Task<ResponseObject<QrTransaccionDTO>> GeneraQrTransaccion(RequestQrTransaccion request, CancellationToken ct = default)
        {
            var response = new ResponseObject<QrTransaccionDTO>
            {
                Message = "QR de cobro generado correctamente.",
                State = ResponseType.Success
            };

            try
            {
                request ??= new RequestQrTransaccion();

                if (request.monto <= 0m)
                {
                    // Con monto fijo un QR en cero es incobrable: el pagador no puede corregirlo.
                    response.State = ResponseType.Error;
                    response.Message = "El monto del QR debe ser mayor a cero.";
                    return response;
                }

                string moneda = Preferido(request.moneda, _opciones.Moneda);
                string sucursal = Preferido(request.codigoSucursal, _opciones.CodigoSucursal);

                if (string.IsNullOrWhiteSpace(sucursal))
                {
                    // Sin sucursal el reporte de cobros no permite saber donde se pago.
                    response.State = ResponseType.Error;
                    response.Message = "No se indico la sucursal y no hay 'QrBancoEconomico:CodigoSucursal' configurada.";
                    return response;
                }

                var cliente = new QrBancoEconomicoClient(_opciones);

                DateTime emision = DateTime.Now;
                DateTime vencimiento = emision.Date.AddDays(_opciones.DiasVigencia);
                string transactionId = NuevoTransactionId(sucursal, emision);
                string descripcion = Preferido(
                    request.descripcion,
                    $"Cobro QR Iza {sucursal} {emision:dd/MM/yyyy HH:mm}");

                (string qrId, string imagen) = await cliente.GenerarQrAsync(
                    transactionId, moneda, request.monto, DateOnly.FromDateTime(vencimiento),
                    descripcion, sucursal, ct);

                response.Data = new QrTransaccionDTO
                {
                    qrId = qrId,
                    qrImageBase64 = imagen,
                    transactionId = transactionId,
                    monto = request.monto,
                    moneda = moneda,
                    codigoSucursal = sucursal,
                    descripcion = descripcion,
                    fechaEmision = emision,
                    fechaVencimiento = vencimiento,
                    usoUnico = true,
                    montoEditable = false
                };
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
        /// Verifica si un QR puntual ya fue cobrado. El proxy no expone consulta de estado por QR, asi
        /// que se filtra el reporte del dia por <c>qrId</c>; el filtro queda aqui y no en la caja para
        /// que ningun punto de venta reciba la operativa completa de la cuenta.
        /// </summary>
        public async Task<ResponseObject<QrPagadoDTO>> ConsultaPagoQr(RequestConsultaPagoQr request, CancellationToken ct = default)
        {
            var response = new ResponseObject<QrPagadoDTO>
            {
                Message = "Cobro por QR verificado correctamente.",
                State = ResponseType.Success
            };

            try
            {
                request ??= new RequestConsultaPagoQr();

                if (string.IsNullOrWhiteSpace(request.qrId))
                {
                    response.State = ResponseType.Error;
                    response.Message = "No se indico el qrId a verificar.";
                    return response;
                }

                DateTime fecha = request.fecha == default ? DateTime.Today : request.fecha;

                if (fecha.Date > DateTime.Today)
                {
                    response.State = ResponseType.Error;
                    response.Message = "La fecha de la consulta no puede ser futura.";
                    return response;
                }

                var cliente = new QrBancoEconomicoClient(_opciones);
                IReadOnlyList<QrBancoEconomicoClient.QrPagadoResponseAdapter> pagos =
                    await cliente.ObtenerQrsPagadosAsync(DateOnly.FromDateTime(fecha), ct);

                QrBancoEconomicoClient.QrPagadoResponseAdapter? pago = pagos.FirstOrDefault(
                    p => string.Equals(p.QrId, request.qrId.Trim(), StringComparison.OrdinalIgnoreCase));

                if (pago is null)
                {
                    // NoData es "todavia no pago", no un fallo: la caja lo usa para seguir esperando.
                    response.State = ResponseType.NoData;
                    response.Message = "El QR aun no registra el cobro.";
                    return response;
                }

                response.Data = Mapear(pago);
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
                    response.ListEntities.Add(Mapear(pago));
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

        private static QrPagadoDTO Mapear(QrBancoEconomicoClient.QrPagadoResponseAdapter pago) => new()
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
        };

        /// <summary>
        /// Identificador unico por suscriptor que exige el proxy. Lleva sufijo aleatorio porque dos
        /// cajas emitiendo en el mismo segundo producirian una colision (409) si solo dependiera de la
        /// marca de tiempo.
        /// </summary>
        private static string NuevoTransactionId(string sucursal, DateTime emision) =>
            $"IZA-{sucursal}-{emision:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        private static string Preferido(string? valor, string porDefecto) =>
            string.IsNullOrWhiteSpace(valor) ? porDefecto : valor.Trim();
    }
}
