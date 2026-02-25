using CoreAccesLayer.Implement.SQLServer;
using Iza.Core.Base;
using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
using Iza.Core.Domain.Reportes;
using Iza.Core.Domain.Types;
using Iza.Core.Domain.Venta;
using Iza.Core.Reports;
using NPOI.SS.Formula.Functions;
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

        public ResponseQuery<AlmacenDTO> SolicitarAmbientes(GeneralRequestAlmacen pametros)
        {
            ResponseQuery<AlmacenDTO> response = new ResponseQuery<AlmacenDTO> { Message = "Barras obtenidas", State = ResponseType.Success };
            try
            {
                response.ListEntities = repositoryPub.GetDataByProcedure<AlmacenDTO>("[inventario].[spObtAlmacenesPuntosDeVenta]", pametros.idSesion, pametros.idFechaProceso, pametros.idAlmacen);

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

                if (requestObtenerProductosAlmacenCentral.idAlmacen == 11) ///CENTRAL O PROVEEDOR
                    response.ListEntities = repositoryPub.GetDataByProcedure<AsignacionDTO>("[inventario].[spObtProductosDeCentral]", requestObtenerProductosAlmacenCentral.idSesion, requestObtenerProductosAlmacenCentral.idFechaProceso);
                else
                    response.ListEntities = repositoryPub.GetDataByProcedure<AsignacionDTO>("[inventario].[spObtProductosDeAlmacen]", requestObtenerProductosAlmacenCentral.idSesion, requestObtenerProductosAlmacenCentral.idFechaProceso, requestObtenerProductosAlmacenCentral.idAlmacen);

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
                //coltypeDetailAsignacion.Add(new typeDetailAsignacion { idProducto = x.idProducto, cantidad = Convert.ToInt32(x.cantidad), fechaDeVencimiento = x.fechaDeVencimiento.Year < 2000? DateTime.Now.AddDays(180): x.fechaDeVencimiento.Date, montoDeCompra = 0 });
                coltypeDetailAsignacion.Add(new typeDetailAsignacion { idProducto = x.idProducto, cantidad = Convert.ToInt32(x.cantidad), fechaDeVencimiento = x.fechaDeVencimiento.Year <= 2000 ? new DateTime(2000, 1, 1, 0, 0, 0) : x.fechaDeVencimiento.Date, montoDeCompra = 0 });
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

                AsignacionProductos reporte = new AsignacionProductos();
                string nombreArchivo = "";
                string reporteBase64 = "";
                reporte.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
                reporte.RollPaper = true;
                reporte.Margins.Left = 10;
                reporte.Margins.Right = 10;
                reporte.xrDestino.Text = inventarioAsignacion.destino;
                reporte.xrOrigen.Text = inventarioAsignacion.origen;
                reporte.xrUsuario.Text = inventarioAsignacion.observaciones;
                
                reporte.DataSource = inventarioAsignacion.detalleProductos;
                string fileName = Path.Combine("c:\\tmp\\", "asignacion_producto_" + Guid.NewGuid() + ".pdf");
                reporte.ExportToPdf(fileName);
                nombreArchivo = fileName;
                reporteBase64 = Convert.ToBase64String(File.ReadAllBytes(fileName));

                repositoryPub.CallProcedure<Response>("reportes.spAddImpresion",
                    inventarioAsignacion.idSesion,
                    inventarioAsignacion.idOperacionDiariaCaja,
                    reporteBase64,
                    nombreArchivo = fileName);
                repositoryPub.Commit();

               


                response.Data = inventarioAsignacion;

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<AlmacenDTO> SolicitarAmbientesCompleto(GeneralRequestAlmacen pametros)
        {
            ResponseQuery<AlmacenDTO> response = new ResponseQuery<AlmacenDTO> { Message = "Amacenes obtenidos", State = ResponseType.Success };
            try
            {
                response.ListEntities = repositoryPub.GetDataByProcedure<AlmacenDTO>("inventario.spObtAlmacenesPuntosDeVenta", pametros.idSesion, pametros.idFechaProceso, pametros.idAlmacen);

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<ProductosVendidosPorBarraDTO> ProductosVendidosPorBarra(GeneralRequestAlmacen request)
        {
            ResponseQuery<ProductosVendidosPorBarraDTO> response = new ResponseQuery<ProductosVendidosPorBarraDTO> { Message = "Formas de pago obtenidos correctamente", State = ResponseType.Success };
            try
            {
                ParamOut paramOutRespuesta = new ParamOut(true);
                ParamOut paramOutLogRespuesta = new ParamOut("");
                paramOutLogRespuesta.Size = 100;

                response.ListEntities = repositoryPub.GetDataByProcedure<ProductosVendidosPorBarraDTO>("reportes.spObtProductosVendidosPorBarra",
                    request.idSesion,
                    request.idFechaProceso,
                    request.idAlmacen);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<IngredientesDeMenuGeneralDTO> IngredientesDeMenuGeneral(GeneralRequest1 request)
        {

            ResponseQuery<IngredientesDeMenuGeneralDTO> response = new ResponseQuery<IngredientesDeMenuGeneralDTO> { Message = "Se obtuvo el menu general", State = ResponseType.Success };
            try
            {
                response.ListEntities = new List<IngredientesDeMenuGeneralDTO>();
                response.ListEntities = repositoryPub.GetDataByProcedure<IngredientesDeMenuGeneralDTO>("[ventas].[spObtIngredientesDeMenuGeneral]", request.idSesion);

                if (response.ListEntities == null || response.ListEntities.Count == 0)
                {
                    response.Message = "No se cuenta con informacion";
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

        public ResponseQuery<IngredientesDeMenuGeneralDTO> BusquedaMenuGeneral(GeneralRequestBusqueda request)
        {

            ResponseQuery<IngredientesDeMenuGeneralDTO> response = new ResponseQuery<IngredientesDeMenuGeneralDTO> { Message = "Se obtuvo el menu general", State = ResponseType.Success };
            try
            {
                response.ListEntities = new List<IngredientesDeMenuGeneralDTO>();
                response.ListEntities = repositoryPub.GetDataByProcedure<IngredientesDeMenuGeneralDTO>("[inventario].[spBuscarEnMenu]", request.idSesion, request.textoABuscar);

                if (response.ListEntities == null || response.ListEntities.Count == 0)
                {
                    response.Message = "No se cuenta con informacion";
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


        public ResponseQuery<IngredientesDeMenuGeneralDTO> GrabarMenuGeneralCompleto(IngredientesDeMenuGeneralDTO request)
        {

            ResponseQuery<IngredientesDeMenuGeneralDTO> response = new ResponseQuery<IngredientesDeMenuGeneralDTO> { Message = "Datos registrados correctamente", State = ResponseType.Success };
            try
            {
                //List<typeIngredientesDeMenu> typeMeduDetalle = new List<typeIngredientesDeMenu>();
                /////Grabar menu
                //if (request.detalle != null)
                //{
                //    request.detalle.ForEach(x =>
                //    {
                //        typeMeduDetalle.Add(new typeIngredientesDeMenu { idProducto = x.idProducto, medidaPorcentaje = 0, medidaUnitaria = x.medidaUnitaria, unidadDeMedida = x.unidaDeMedida });
                //    });
                //}
                ParamOut paramOutRespuesta = new ParamOut(true);
                repositoryPub.CallProcedure<Response>("[inventario].[spABMParamPrecio]", 
                    request.idSesion,
                    request.esParaMenu ? 1:0,
                    request.esProducto ? 1 : 0,
                    request.idPrecio,
                    request.idCategoria,
                    request.descripcionMenu,
                    request.marca,
                    request.embase,
                    request.precio,
                    request.idIngrediente,
                    request.descripcionMenu,
                    request.embaseXUnidades,
                    request.medidaUnitaria,
                    request.idProducto,
                    request.marca,
                    request.contenido,
                    request.precioUnitario,
                    request.precio,
                    0, //clasificador  no se usa
                    0, //cantidad
                    1, //orden despliegue
                    request.depliegueDerecha ? 1 : 0,
                    request.detalle, //typeMeduDetalle,
                    paramOutRespuesta
                    );

                ///respuesta
                if ((bool)paramOutRespuesta.Valor)
                {
                    response.Message = "Error al grabar.";
                    response.State = ResponseType.Error;
                    repositoryPub.Rollback();
                    return response;
                }
                repositoryPub.Commit();
                response.ListEntities = new List<IngredientesDeMenuGeneralDTO>();
                response.ListEntities = repositoryPub.GetDataByProcedure<IngredientesDeMenuGeneralDTO>("[ventas].[spObtIngredientesDeMenuGeneral]", request.idSesion);

                if (response.ListEntities == null || response.ListEntities.Count == 0)
                {
                    response.Message = "No se cuenta con informacion";
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

        public ResponseQuery<InventarioProducto> AperturaInventario(GeneralRequest1 requestGral)
        {

            ResponseQuery<InventarioProducto> response = new ResponseQuery<InventarioProducto> { Message = "Se aperturo el Inventario del dia", State = ResponseType.Success };
            try
            {
                //string fechaApertura1 = "";
                //ParamOut poFecha1 = new ParamOut(new DateTime());
                //poFecha1.Valor = DateTime.Now;
                //if (poFecha1.Valor != null)
                //    fechaApertura1 = Convert.ToDateTime(poFecha1.Valor).ToString("MM/dd/yyyy");
                //GrabaAsignacionProducto
                long id = 0;
                ParamOut poRespuesta = new ParamOut(false);
                ParamOut poLogRespuesta = new ParamOut("");
                ParamOut poFecha = new ParamOut(new DateTime());
                ParamOut poIdFecha = new ParamOut(id);

                poLogRespuesta.Size = 100;
                //response.ListEntities = new List<ResulProductoPrecioVenta>();

                response.ListEntities = repositoryPub.GetDataByProcedure<InventarioProducto>("inventario.spAperturaInventarioFechaProceso", requestGral.idSesion, poIdFecha, poFecha, poRespuesta, poLogRespuesta);
                //repositoryFabula.Commit();
                string fechaApertura = "";
                if (poFecha.Valor != null && response.ListEntities != null && response.ListEntities.Count>0)
                {
                    response.ListEntities[0].idFechaProceso = Convert.ToInt32(poIdFecha.Valor);
                    response.ListEntities[0].fechaProceso = Convert.ToDateTime(poFecha.Valor);
                }
                //response.Code = fechaApertura;
                if (response.ListEntities == null)
                {
                    response.State = ResponseType.Error;
                    response.Message = "No se realizo la apertur del Inventario";
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

        public ResponseQuery<InventarioProducto> CierreInventario(GeneralRequest1 requestGral)
        {

            ResponseQuery<InventarioProducto> response = new ResponseQuery<InventarioProducto> { Message = "Se realico el cierre del Inventario del dia", State = ResponseType.Success };
            try
            {
                long id = 0;
                ParamOut poRespuesta = new ParamOut(false);
                ParamOut poLogRespuesta = new ParamOut("");
                ParamOut poFecha = new ParamOut(new DateTime());
                ParamOut poIdFecha = new ParamOut(id);

                poLogRespuesta.Size = 100;
                //response.ListEntities = new List<ResulProductoPrecioVenta>();

                response.ListEntities = repositoryPub.GetDataByProcedure<InventarioProducto>("inventario.spCierreInventarioFechaProceso", requestGral.idSesion, requestGral.idFechaProceso, poRespuesta, poLogRespuesta);
                //repositoryFabula.Commit();

                if (response.ListEntities == null)
                {
                    response.State = ResponseType.Error;
                    response.Message = "No se realizo la apertura del Inventario";
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




        #region Clasificador

        public ResponseQuery<ClasificadorDTO> ClasificadorPorTipo(GeneralRequest1 requestGral)
        {

            ResponseQuery<ClasificadorDTO> response = new ResponseQuery<ClasificadorDTO> { Message = "Clasificador recuperado", State = ResponseType.Success };
            try
            {
                response.ListEntities = repositoryPub.GetDataByProcedure<ClasificadorDTO>("[ventas].[spObtClasificadorPorTipo]", requestGral.idFechaProceso);

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        #endregion

    }
}
