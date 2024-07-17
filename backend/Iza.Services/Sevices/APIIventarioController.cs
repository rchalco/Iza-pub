using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
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
        //[HttpPost("AsignaProductos")]
        //[EnableCors("MyPolicy")]
        //public Response AsignaProductos(MenuItemConfiguracion composicionItemMenu)
        //{
        //    EngineInventario mgrInventario = new EngineInventario();
        //    return mgrInventario.AsignaProductos(composicionItemMenu);
        //}

        [HttpPost("SolicitarAmbientes")]
        [EnableCors()]
        public ResponseQuery<AlmacenDTO> SolicitarAmbientes(GeneralRequest1 requestAperturaCaja)
        {
            EngineInventarios mgrInventario = new EngineInventarios();
            return mgrInventario.SolicitarAmbientes(requestAperturaCaja);
        }

       

    }
}
