using CoreAccesLayer.Implement.SQLServer;
using Iza.Core.Base;
using Iza.Core.Domain.Seguridad;
using Iza.Core.Domain.Venta.Caja;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Engine.Seguridad
{
    public class EngineSeguridad : BaseManager
    {
        public ResponseObject<LoginResponse> Login(LoginRequest loginRequest)
        {

            //poLogRespuesta.Size = 100;
            ResponseObject<LoginResponse> response = new ResponseObject<LoginResponse> { Message = "La caja se aperturo correctamente", State = ResponseType.Success };
            try
            {
                ///TODO:Encriptar el pass
                ///
                ParamOut poRespuesta = new ParamOut(false);
                ParamOut poLogRespuesta = new ParamOut("");
                poLogRespuesta.Size = 100;
                response.Data = repositoryPub.GetDataByProcedure<LoginResponse>("seguridad.spLogin", loginRequest.idEmpresa, loginRequest.usuario, loginRequest.password, loginRequest.version, poRespuesta, poLogRespuesta).FirstOrDefault();


                if (response.Data == null)
                {
                    response.Message = "Error al realizar la consulta";
                    response.State = ResponseType.Error;
                    return response;
                }

                if ((bool)poRespuesta.Valor)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
    }
}
