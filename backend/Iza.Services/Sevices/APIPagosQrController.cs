using Iza.Core.Domain.Pagos;
using Iza.Core.Engine.Pagos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Wrapper;

namespace Iza.Services.Sevices
{
    /// <summary>
    /// Cobros por QR de Banco Economico. Toda la conversacion con el banco pasa por el proxy
    /// qr-banco-economico: Iza emite un QR por venta y verifica su cobro.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class APIPagosQrController : ControllerBase
    {
        /// <summary>
        /// Emite el QR de cobro de una venta. Es de un solo uso y de monto fijo: el pagador no puede
        /// alterar el importe ni reutilizar el codigo.
        /// </summary>
        [HttpPost("GeneraQrTransaccion")]
        [EnableCors()]
        public async Task<ResponseObject<QrTransaccionDTO>> GeneraQrTransaccion(RequestQrTransaccion requestQrTransaccion, CancellationToken ct)
        {
            EnginePagosQr mgrPagosQr = new EnginePagosQr();
            return await mgrPagosQr.GeneraQrTransaccion(requestQrTransaccion, ct);
        }

        /// <summary>
        /// Verifica si un QR puntual ya fue cobrado. Devuelve estado <c>NoData</c> mientras el pago no
        /// se acredite: es la respuesta normal mientras la caja espera, no un error.
        /// </summary>
        [HttpPost("ConsultaPagoQr")]
        [EnableCors()]
        public async Task<ResponseObject<QrPagadoDTO>> ConsultaPagoQr(RequestConsultaPagoQr requestConsultaPagoQr, CancellationToken ct)
        {
            EnginePagosQr mgrPagosQr = new EnginePagosQr();
            return await mgrPagosQr.ConsultaPagoQr(requestConsultaPagoQr, ct);
        }

        /// <summary>Lista los cobros por QR acreditados en una fecha. El banco reporta por dia, no por rango.</summary>
        [HttpPost("ObtieneQrsPagados")]
        [EnableCors()]
        public async Task<ResponseQuery<QrPagadoDTO>> ObtieneQrsPagados(RequestQrsPagados requestQrsPagados, CancellationToken ct)
        {
            EnginePagosQr mgrPagosQr = new EnginePagosQr();
            return await mgrPagosQr.ObtieneQrsPagados(requestQrsPagados, ct);
        }
    }
}
