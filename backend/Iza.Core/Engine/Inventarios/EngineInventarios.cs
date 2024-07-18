using Iza.Core.Base;
using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
using Iza.Core.Domain.Reportes;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Engine.Inventarios
{
    public class EngineInventarios : BaseManager
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

        public ResponseQuery<AsignacionDTO> ObtenerProductosAlmacenCentral(RequestObtenerProductosAlmacenCentral requestObtenerProductosAlmacenCentral)
        {

            ResponseQuery<AsignacionDTO> response = new ResponseQuery<AsignacionDTO> { Message = "Se obtuvo el inventario de forma correcta", State = ResponseType.Success };
            try
            {
                response.ListEntities = new List<AsignacionDTO>();
                response.ListEntities = repositoryPub.GetDataByProcedure<AsignacionDTO>("[inventario].[spObtProductosDeCentral]", requestObtenerProductosAlmacenCentral.idSesion, requestObtenerProductosAlmacenCentral.idFechaProceso);

                if (response.ListEntities == null || response.ListEntities.Count == 0)
                {
                    response.Message = "No se cuenta con informacion del almacen central";
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

        public ResponseQuery<DashboardProductosDTO> ObtenerDashboardProductos(GeneralRequest1 generalRequest)
        {

            ResponseQuery<DashboardProductosDTO> response = new ResponseQuery<DashboardProductosDTO> { Message = "Se obtuvo el inventario de forma correcta", State = ResponseType.Success };
            try
            {
                response.ListEntities = new List<DashboardProductosDTO>();
                response.ListEntities = repositoryPub.GetDataByProcedure<DashboardProductosDTO>("[reportes].[spDashboardProductos]", generalRequest.idSesion, generalRequest.idFechaProceso);

                if (response.ListEntities == null || response.ListEntities.Count == 0)
                {
                    response.Message = "No se cuenta con informacion de los productos";
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
