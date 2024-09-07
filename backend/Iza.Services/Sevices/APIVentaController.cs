using Iza.Core.Domain.Iventario;
using Iza.Core.Domain.Reportes;
using Iza.Core.Domain.Venta;
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
        [EnableCors()]
        public ResponseObject<AperturaCajaResponse> AperturaCaja(SaldoCajaDTO requestAperturaCaja)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.AperturaCaja(requestAperturaCaja);
        }

        [HttpPost("CierreCaja")]
        [EnableCors()]
        public ResponseObject<SaldoCajaDTO> CierreCaja(SaldoCajaDTO requestCierreCaja)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.CierreCaja(requestCierreCaja);
        }



        #endregion

        #region Ventas

        [HttpPost("ObtienePorAlmacen")]
        [EnableCors()]
        public ResponseQuery<ResulProductoPrecioVentaComplex> ObtienePorAlmacen(RequestSearchProductAlmacen requestSearchProductAlmacen)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.ObtienePorAlmacen(requestSearchProductAlmacen);
        }

        [HttpPost("RegistrarVentas")]
        [EnableCors()]
        public Response RegistrarVentas(RequestRegistroVenta requestRegistroVentas)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.RegistrarVentas(requestRegistroVentas);
        }



        #endregion

        #region Reportes Cola
        [HttpPost("GetDocumentPending")]
        [EnableCors()]
        public ResponseQuery<PrinterLineResponse> GetDocumentPending(PrinterLineRequest printerLineRequest)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.GetDocumentPending(printerLineRequest);
        }

        #endregion
    }


}
