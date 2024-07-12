using CoreAccesLayer.Implement.SQLServer;
using Iza.Core.Domain.Iventario;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Engine.Ventas
{
    internal class EngineInventario
    {
        //public ResponseObject<AsignacionDTO> AsignaProductos(AsignacionDTO inventarioAsignacion)
        //{

        //    ResponseObject<AsignacionDTO> response = new ResponseObject<AsignacionDTO> { Message = "Asignación se grabo correctamente", State = ResponseType.Success };
        //    try
        //    {
        //        response.Object = new InventarioAsignacion();
        //        List<typeDetailAsignacionDTO> coltypeDetailPedido = new List<typeDetailAsignacionDTO>();
        //        inventarioAsignacion.typeDetailAsignacion.ForEach(x =>
        //        {
        //            coltypeDetailPedido.Add(new typeDetailAsignacionDTO { idProducto = x.idProducto, cantidad = Convert.ToInt32(x.cantidad), PrecioFinal = 0 });
        //        });

        //        ParamOut poRespuesta = new ParamOut(false);
        //        ParamOut poLogRespuesta = new ParamOut("");
        //        poLogRespuesta.Size = 100;
        //        ////Verificar que exite cantidades.
        //        repositoryPub.CallProcedure<AsignaProductos>("inventario.spAsignacionInventario",
        //           inventarioAsignacion.idSesion,
        //           inventarioAsignacion.idfechaproceso,
        //           inventarioAsignacion.idAlmacenDesde,
        //           coltypeDetailPedido,
        //           poRespuesta, poLogRespuesta);

        //        if ((bool)poRespuesta.Valor)
        //        {
        //            response.Message = poLogRespuesta.Valor.ToString();
        //            response.State = ResponseType.Error;
        //            return response;
        //        }

        //        //SP grabar pedido
        //        repositoryFabula.CallProcedure<InventarioAsignacion>("shFabula.spAsignaProdAlmacen",
        //            inventarioAsignacion.idSesion,
        //            inventarioAsignacion.idfechaproceso,
        //            inventarioAsignacion.idAlmacenDesde,
        //            inventarioAsignacion.idAlmacenHasta,
        //            coltypeDetailPedido,
        //            "",
        //            poRespuesta, poLogRespuesta);
        //        repositoryFabula.Commit();

        //        if (response.Object == null)
        //        {
        //            response.Message = "Error al grabar el pedido";
        //            response.State = ResponseType.Error;
        //            return response;
        //        }

        //        if ((bool)poRespuesta.Valor)
        //        {
        //            response.Message = poLogRespuesta.Valor.ToString();
        //            response.State = ResponseType.Error;
        //            return response;
        //        }


        //        response.Object = inventarioAsignacion;

        //    }
        //    catch (Exception ex)
        //    {
        //        ProcessError(ex, response);
        //    }
        //    return response;
        //}

    }
}
