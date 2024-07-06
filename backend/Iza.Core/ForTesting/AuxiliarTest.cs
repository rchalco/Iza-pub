using Iza.Core.Base;
using PlumbingProps.Wrapper;

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
                repositoryPub.CallProcedure<object>(spName, parametros);
                repositoryPub.Commit();
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
