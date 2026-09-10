using Iza.Core.Domain.Pagos;
using Iza.Core.Engine.Pagos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Wrapper;

namespace Iza.Services.Sevices
{
    /// <summary>
    /// Cobros por QR de Banco Economico. Toda la conversacion con el banco pasa por el proxy
    /// qr-banco-economico: Iza solo emite el QR del mes y consulta que se cobro.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class APIPagosQrController : ControllerBase
    {
        /// <summary>
        /// Devuelve el QR de cobro vigente de la sucursal. Se emite una vez por periodo y se sirve desde
        /// cache el resto del mes; <c>forzarRenovacion</c> emite uno nuevo aunque el vigente no haya vencido.
        /// </summary>
        [HttpPost("ObtieneQrMensual")]
        [EnableCors()]
        public async Task<ResponseObject<QrMensualDTO>> ObtieneQrMensual(RequestQrMensual requestQrMensual, CancellationToken ct)
        {
            EnginePagosQr mgrPagosQr = new EnginePagosQr();
            return await mgrPagosQr.ObtieneQrMensual(requestQrMensual, ct);
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
