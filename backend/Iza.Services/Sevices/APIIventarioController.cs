using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
using Iza.Core.Domain.Reportes;
using Iza.Core.Domain.Venta;
using Iza.Core.Domain.Venta.Caja;
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
    public class APIIventarioController : ControllerBase
    {
        [HttpPost("ObtenerProductosAlmacenCentral")]
        [EnableCors()]
        public ResponseQuery<AsignacionDTO> ObtenerProductosAlmacenCentral(RequestObtenerProductosAlmacenCentral requestObtenerProductosAlmacenCentral)
        {
            EngineInventarios engineInventario = new EngineInventarios();
            return engineInventario.ObtenerProductosAlmacenCentral(requestObtenerProductosAlmacenCentral);
        }

        [HttpPost("SolicitarAmbientes")]
        [EnableCors()]
        public ResponseQuery<AlmacenDTO> SolicitarAmbientes(GeneralRequest1 requestAperturaCaja)
        {
            EngineInventarios mgrInventario = new EngineInventarios();
            return mgrInventario.SolicitarAmbientes(requestAperturaCaja);
        }

        [HttpPost("ObtenerDashboardProductos")]
        [EnableCors()]
        public ResponseQuery<DashboardProductosDTO> ObtenerDashboardProductos(GeneralRequest1 generalRequest)
        {
            EngineInventarios mgrInventario = new EngineInventarios();
            return mgrInventario.ObtenerDashboardProductos(generalRequest);
        }

        [HttpPost("GrabaAsignacionProducto")]
        [EnableCors()]
        public ResponseObject<InventarioAsignacion> GrabaAsignacionProducto(InventarioAsignacion inventarioAsignacion)
        {
            EngineInventarios engineInventario = new EngineInventarios();
            return engineInventario.GrabaAsignacionProducto(inventarioAsignacion);
        }

        [HttpPost("SolicitarAmbientesCompleto")]
        [EnableCors()]
        public ResponseQuery<AlmacenDTO> SolicitarAmbientesCompleto(GeneralRequest1 requestAperturaCaja)
        {
            EngineInventarios mgrInventario = new EngineInventarios();
            return mgrInventario.SolicitarAmbientesCompleto(requestAperturaCaja);
        }


        [HttpPost("ProductosVendidosPorBarra")]
        [EnableCors()]
        public ResponseQuery<ProductosVendidosPorBarraDTO> ProductosVendidosPorBarra(GeneralRequestAlmacen request)
        {
            EngineInventarios mgInventario = new EngineInventarios();
            return mgInventario.ProductosVendidosPorBarra(request);
        }

        [HttpPost("IngredientesDeMenuGeneral")]
        [EnableCors()]
        public ResponseQuery<IngredientesDeMenuGeneralDTO> IngredientesDeMenuGeneral(GeneralRequest1 request)
        {
            EngineInventarios mgInventario = new EngineInventarios();
            return mgInventario.IngredientesDeMenuGeneral(request);
        }

        [HttpPost("BusquedaMenuGeneral")]
        [EnableCors()]
        public ResponseQuery<IngredientesDeMenuGeneralDTO> BusquedaMenuGeneral(GeneralRequestBusqueda request)
        {
            EngineInventarios mgInventario = new EngineInventarios();
            return mgInventario.BusquedaMenuGeneral(request);
        }


        [HttpPost("GrabarMenuGeneralCompleto")]
        [EnableCors()]
        public ResponseQuery<IngredientesDeMenuGeneralDTO> GrabarMenuGeneralCompleto(IngredientesDeMenuGeneralDTO request)
        {
            EngineInventarios mgInventario = new EngineInventarios();
            return mgInventario.GrabarMenuGeneralCompleto(request);
        }


        [HttpPost("ClasificadorPorTipo")]
        [EnableCors()]
        public ResponseQuery<ClasificadorDTO> ClasificadorPorTipo(GeneralRequest1 request)
        {
            EngineInventarios mgInventario = new EngineInventarios();
            return mgInventario.ClasificadorPorTipo(request);
        }

        [HttpPost("AperturaInventario")]
        [EnableCors()]
        public ResponseQuery<InventarioProducto> AperturaInventario(GeneralRequest1 requestGral)
        {
            EngineInventarios mgInventario = new EngineInventarios();
            return mgInventario.AperturaInventario(requestGral);
        }

        [HttpPost("CierreInventario")]
        [EnableCors()]
        public ResponseQuery<InventarioProducto> CierreInventario(GeneralRequest1 requestGral)
        {
            EngineInventarios mgInventario = new EngineInventarios();
            return mgInventario.CierreInventario(requestGral);
        }
    }
}
