using Iza.Core.Domain.Venta.Caja;
using Iza.Core.Engine.Ventas;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Wrapper;

namespace Iza.Services.Sevices
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIVentaController : ControllerBase
    {


        #region Caja


        [HttpPost("AperturaCaja")]
        [EnableCors("MyPolicy")]
        public ResponseObject<AperturaCajaResponse> AperturaCaja(SaldoCajaDTO requestAperturaCaja)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.AperturaCaja(requestAperturaCaja);
        }

        [HttpPost("CierreCaja")]
        [EnableCors("MyPolicy")]
        public ResponseObject<SaldoCajaDTO> CierreCaja(SaldoCajaDTO requestCierreCaja)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.CierreCaja(requestCierreCaja);
        }

        #endregion
    }


}
