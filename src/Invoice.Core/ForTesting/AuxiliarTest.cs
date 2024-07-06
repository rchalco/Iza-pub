using CoreAccesLayer.Implement.SQLServer;
using Invoice.Core.Base;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.ForTesting
{
    public class AuxiliarTest : BaseManager
    {
        public Response EjecutarSP(string spName, params object[] parametros)
        {
            Response response = new Response
            {
                Message = "Procedimiento ejecutado exitosamente",
                State = ResponseType.Success,
            };

            try
            {
                repositoryFacturacion.CallProcedure<object>(spName, parametros);
                repositoryFacturacion.Commit();
            }
            catch (Exception ex)
            {
                response = new Response
                {
                    Message = "Error al  ejecutar el procedimiento:" + ex.Message,
                    State = ResponseType.Error,
                };
            }
            return response;

        }
    }
}
