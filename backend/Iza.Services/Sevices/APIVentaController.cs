using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
using Iza.Core.Domain.Reportes;
using Iza.Core.Domain.Venta;
using Iza.Core.Domain.Venta.Caja;
using Iza.Core.Engine.Impresion;
using Iza.Core.Engine.Inventarios;
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

        [HttpPost("ObtieneFormasdePago")]
        [EnableCors()]
        public ResponseQuery<ResulObtieneFormasdePago> ObtieneFormasdePago(GeneralRequest1 requestSPObtFormasDePago)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.ObtieneFormasdePago(requestSPObtFormasDePago);
        }

        [HttpPost("DetallePedidoPorFormaPago")]
        [EnableCors()]
        public ResponseQuery<DetallePedidosDTO> DetallePedidoPorFormaPago(GeneralRequestRangoFecha requestGeneral)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.DetallePedidoPorFormaPago(requestGeneral);
        }

        [HttpPost("ActualizaFormaPagoPedido")]
        [EnableCors()]
        public ResponseQuery<DetallePedidosDTO> ActualizaFormaPagoPedido(PedidoRequest requestPedido)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.ActualizaFormaPagoPedido(requestPedido);
        }


        [HttpPost("AnulaPedido")]
        [EnableCors()]
        public ResponseQuery<DetallePedidosDTO> AnulaPedido(PedidoRequest requestPedido)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.AnulaPedido(requestPedido);
        }

        [HttpPost("ObtieneProductosVenta")]
        [EnableCors()]
        public ResponseQuery<ResulProductoPrecioVenta> ObtieneProductosVenta(GeneralRequest1 request)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.ObtieneProductosVenta(request);
        }
        [HttpPost("ObtieneCajerosCierre")]
        [EnableCors()]
        public ResponseQuery<ResponseObtieneCajerosCierre> ObtieneCajerosCierre(GeneralRequest1 requestObtieneCajerosCierre)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.ObtieneCajerosCierre(requestObtieneCajerosCierre);
        }

        [HttpPost("UltimasMovimientos")]
        [EnableCors()]
        public ResponseQuery<ResponseSPObtPedidosPorFormasDePago> UltimasMovimientos(RequestSPObtPedidosPorFormasDePago requestSPObtPedidosPorFormasDePago)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.UltimasMovimientos(requestSPObtPedidosPorFormasDePago);
        }


        [HttpPost("CerrarCaja")]
        [EnableCors()]
        public Response CerrarCaja(RequestSPCierreCajero requestSPCierreCajero)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.CerrarCaja(requestSPCierreCajero);
        }

        [HttpPost("ObtieneProdcutoInventario")]
        [EnableCors()]
        public ResponseQuery<ResulProductoPrecioVentaComplex> ObtieneProdcutoInventario(RequestSearchProductAlmacen requestSearchProductAlmacen)
        {
            EngineVentas mgrVentas = new EngineVentas();
            return mgrVentas.ObtieneProdcutoInventario(requestSearchProductAlmacen);
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

        [HttpPost("GenerarDocumento")]
        [EnableCors()]
        public IActionResult GenerarDocumento(DataDocumento dataDocumento)
        {
            EngineImpresion mangerPrinter = new EngineImpresion();
            var resulMgr = mangerPrinter.GenerarDocumento(dataDocumento);
            if (resulMgr.State == ResponseType.Success)
            {
                string fileName = resulMgr.Message;
                fileName = fileName.StartsWith("\\") ? "\\" + fileName : fileName;
                return new PhysicalFileResult(fileName, System.Net.Mime.MediaTypeNames.Application.Octet);
            }

            return Problem(resulMgr.Message);
        }


        #endregion
    }


}
