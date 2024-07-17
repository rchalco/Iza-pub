using Iza.Core.Base;
using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Engine.Inventarios
{
    public class EngineInventarios: BaseManager
    {

        public ResponseQuery<AlmacenDTO> SolicitarAmbientes(GeneralRequest1 pametros)
        {
            ResponseQuery<AlmacenDTO> response = new ResponseQuery<AlmacenDTO> { Message = "Barras obtenidas", State = ResponseType.Success };
            try
            {
                response.ListEntities = repositoryPub.GetDataByProcedure<AlmacenDTO>("[inventario].[spObtAlmacenesPuntosDeVenta]", pametros.idSesion, pametros.idFechaProceso);
                
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

    }
}
