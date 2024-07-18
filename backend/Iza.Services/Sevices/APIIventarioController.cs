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
        [EnableCors("MyPolicy")]
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

    }
}
