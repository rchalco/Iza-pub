using Iza.Core.Domain.Seguridad;
using Iza.Core.Engine.Seguridad;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Wrapper;

namespace Iza.Services.Sevices
{
    [Route("api/[controller]")]
    [ApiController]
    public class APISeguridadController : ControllerBase
    {

        [HttpPost("LoginUsuario")]
        [EnableCors()]
        public ResponseObject<LoginDTO> LoginUsuario(RequestLogin requestLogin)
        {
            EngineSeguridad seguridadManager = new EngineSeguridad();
            return seguridadManager.LoginUsuario(requestLogin);
        }

        [HttpPost("CambioContrasena")]
        [EnableCors()]
        public ResponseObject<LoginDTO> CambioContrasena(RequestLogin requestLogin)
        {
            EngineSeguridad seguridadManager = new EngineSeguridad();
            return seguridadManager.CambioContrasena(requestLogin.usuario, requestLogin.password, requestLogin.passwordNuevo);
        }

        [HttpPost("ObtieneMenuPorUsuario")]
        [EnableCors()]
        public ResponseQuery<MenuGeneralDTO> ObtieneMenuPorUsuario(RequestMenuUsuario requestParametros)
        {
            EngineSeguridad seguridadManager = new EngineSeguridad();
            return seguridadManager.ObtieneMenuPorUsuario(requestParametros);
        }

    }
}
