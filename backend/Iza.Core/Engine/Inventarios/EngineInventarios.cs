using CoreAccesLayer.Implement.SQLServer;
using Iza.Core.Base;
using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
using Iza.Core.Domain.Reportes;
using Iza.Core.Domain.Types;
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

        public ResponseObject<InventarioAsignacion> GrabaAsignacionProducto(InventarioAsignacion inventarioAsignacion)
        {

            ResponseObject<InventarioAsignacion> response = new ResponseObject<InventarioAsignacion> { Message = "Asignación se grabo correctamente", State = ResponseType.Success };
            try
            {
                response.Data = new InventarioAsignacion();
                List<typeDetailAsignacion> coltypeDetailAsignacion = new List<typeDetailAsignacion>();
                inventarioAsignacion.detalleProductos.ForEach(x =>
                {
                    coltypeDetailAsignacion.Add(new typeDetailAsignacion { idProducto = x.idProducto, cantidad = Convert.ToInt32(x.cantidad) });
                });

                ParamOut poRespuesta = new ParamOut(false);
                ParamOut poLogRespuesta = new ParamOut("");
                poLogRespuesta.Size = 100;
                
                //SP grabar pedido
                repositoryPub.CallProcedure<InventarioAsignacion>("[inventario].[spAsignacionInventario]",
                    inventarioAsignacion.idSesion,
                    inventarioAsignacion.idfechaproceso,
                    inventarioAsignacion.idAlmacenDesde,
                    inventarioAsignacion.idAlmacenHasta,
                    "",
                    coltypeDetailAsignacion,
                    poRespuesta, poLogRespuesta);
                repositoryPub.Commit();

                if (response.Data == null)
                {
                    response.Message = "Error al grabar el pedido";
                    response.State = ResponseType.Error;
                    return response;
                }

                if ((bool)poRespuesta.Valor)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }


                response.Data = inventarioAsignacion;

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
                
    }
}
