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
        [EnableCors("MyPolicy")]
        public ResponseObject<LoginResponse> LoginUsuario(LoginRequest requestLogin)
        {
            EngineSeguridad seguridadManager = new EngineSeguridad();
            return seguridadManager.Login(requestLogin);
        }

    }
}
