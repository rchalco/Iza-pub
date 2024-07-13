using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Wrapper;
using Security.Core.Domain;
using Security.Core.Engine;

namespace Security.API.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class APISecurityController : ControllerBase
    {
        [HttpPost("LoginUsuario")]
        [EnableCors()]
        public ResponseObject<LoginDTO> LoginUsuario(RequestLogin requestLogin)
        {
            EngineSecurity seguridadManager = new EngineSecurity();
            return seguridadManager.LoginUsuario(requestLogin);
        }

        [HttpPost("CambioContrasena")]
        [EnableCors()]
        public ResponseObject<LoginDTO> CambioContrasena(RequestLogin requestLogin)
        {
            EngineSecurity seguridadManager = new EngineSecurity();
            return seguridadManager.CambioContrasena(requestLogin.usuario, requestLogin.password, requestLogin.passwordNuevo);
        }

        [HttpPost("ObtieneMenuPorUsuario")]
        [EnableCors()]
        public ResponseQuery<MenuGeneralDTO> ObtieneMenuPorUsuario(RequestMenuUsuario requestParametros)
        {
            EngineSecurity seguridadManager = new EngineSecurity();
            return seguridadManager.ObtieneMenuPorUsuario(requestParametros);
        }

        //[HttpPost("ObtieneMenuPorUsuario")]
        //[EnableCors()]
        //public ResponseQuery<MenuGeneralDTO> ObtieneMenuPorUsuario()
        //{
        //    EngineSecurity seguridadManager = new EngineSecurity();
        //    return seguridadManager.ObtieneMenuPorUsuario(new RequestParametrosGral());
        //}


    }
}
