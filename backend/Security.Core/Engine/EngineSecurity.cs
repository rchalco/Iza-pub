using CoreAccesLayer.Implement.SQLServer;
using PlumbingProps.Wrapper;
using Security.Core.Base;
using Security.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Engine
{
    public class EngineSecurity: BaseManager
    {
        public ResponseObject<LoginDTO> LoginUsuario(RequestLogin requestLogin)
        {

            ResponseObject<LoginDTO> response = new ResponseObject<LoginDTO> { Message = "¨Inicio de Sesión Correcto", State = ResponseType.Success };
            try
            {
                ///TODO:Encriptar el pass
                ///
                ParamOut poRespuesta = new ParamOut(false);
                ParamOut poLogRespuesta = new ParamOut("");
                poLogRespuesta.Size = 100;
                //response.Data = repositorySecurity.GetDataByProcedure<LoginDTO>("seguridad.spLogin", requestLogin.idEmpresa, requestLogin.usuario, requestLogin.password, requestLogin.version, poRespuesta, poLogRespuesta).FirstOrDefault();
                response.Data = repositorySecurity.GetDataByProcedure<LoginDTO>("seguridad.spLogin", requestLogin.idEmpresa, requestLogin.usuario, requestLogin.password, requestLogin.version, poRespuesta, poLogRespuesta).FirstOrDefault();


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

        public ResponseObject<LoginDTO> CambioContrasena(string Usuario, string Password, string PasswordNuevo)
        {

            ResponseObject<LoginDTO> response = new ResponseObject<LoginDTO> { Message = "¨Se realizo el cambio de contraseña", State = ResponseType.Success };
            try
            {

                ///TODO:Encriptar el pass

                //TUsuario ObjTUsuario = new TUsuario();
                //ObjTUsuario = repositoryFabula.SimpleSelect<TUsuario>(x => x.Usuario == Usuario).FirstOrDefault();
                //if (ObjTUsuario == null)
                //{
                //    response.State = ResponseType.Error;
                //    response.Message = "El Usuario no existe";
                //    return response;
                //}
                //if (ObjTUsuario.Pass != Password)
                //{
                //    response.State = ResponseType.Error;
                //    response.Message = "La contraseña es incorrecta";
                //    return response;
                //}
                //ObjTUsuario.Pass = PasswordNuevo;
                //Entity<TUsuario> entity = new Entity<TUsuario> { EntityDB = ObjTUsuario, stateEntity = StateEntity.modify };
                //repositoryFabula.SaveObject<TUsuario>(entity);
                //repositoryFabula.Commit();

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<MenuGeneralDTO> ObtieneMenuPorUsuario(RequestMenuUsuario requestGral)
        {
            ParamOut poRespuesta = new ParamOut(0);
            ParamOut poLogRespuesta = new ParamOut("");
            ResponseQuery<MenuGeneralDTO> response = new ResponseQuery<MenuGeneralDTO> { Message = "Menu Obtenido", State = ResponseType.Success };
            try
            {
                response.ListEntities = repositorySecurity.GetDataByProcedure<MenuGeneralDTO>("[seguridad].[spObtieneMenuPorRol]", requestGral.IdSesion, requestGral.IdRol, poRespuesta, poLogRespuesta);
                if (response.ListEntities == null)
                {
                    response.State = ResponseType.Error;
                    response.Message = "No exiten roles";
                }

                if (Convert.ToInt32(poRespuesta.Valor) != 0)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Warning;
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
