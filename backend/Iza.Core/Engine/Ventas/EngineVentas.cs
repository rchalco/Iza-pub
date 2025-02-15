using CoreAccesLayer.Implement.SQLServer;
using DevExpress.XtraReports;
using iText.Layout.Element;
using Iza.Core.Base;
using Iza.Core.Domain.General;
using Iza.Core.Domain.Iventario;
using Iza.Core.Domain.Reportes;
using Iza.Core.Domain.Venta;
using Iza.Core.Domain.Venta.Caja;
using Iza.Core.Reports;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Engine.Ventas
{
    public class EngineVentas: BaseManager
    {

        #region Cajas

        public ResponseObject<AperturaCajaResponse> AperturaCaja(SaldoCajaDTO requestAperturaCajae)
        {
            ParamOut poRespuesta = new ParamOut(0);
            ParamOut poLogRespuesta = new ParamOut("");
            poLogRespuesta.Size = 100;
            ResponseObject<AperturaCajaResponse> response = new ResponseObject<AperturaCajaResponse> { Message = "La caja se aperturo correctamente", State = ResponseType.Success };
            try
            {
                response.Data = new AperturaCajaResponse();
                ParamOut idFechaProceso = new ParamOut(0);
                ParamOut FechaProceso = new ParamOut(DateTime.Now);
                ParamOut idOperacionDiariaCaja = new ParamOut(0);
                //validamos que no exita una caja abierta en otra fecha 
                repositoryPub.CallProcedure<AperturaCajaResponse>("[ventas].[spAperturaCaja]",
                        requestAperturaCajae.idSesion,
                        requestAperturaCajae.idCaja,
                        requestAperturaCajae.idAlmacen,
                        requestAperturaCajae.SaldoInicial,
                        requestAperturaCajae.Observacion,
                        idOperacionDiariaCaja,
                        idFechaProceso,
                        FechaProceso,
                        poRespuesta,
                        poLogRespuesta);

                repositoryPub.Commit();

                if (Convert.ToInt32(poRespuesta.Valor) != 0)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }
                response.Message = poLogRespuesta.Valor.ToString();
                response.Data.idFechaProceso = Convert.ToInt64(idFechaProceso.Valor);
                response.Data.FechaProceso = Convert.ToDateTime(FechaProceso.Valor);
                response.Data.idOperacionDiaria = Convert.ToInt32(idOperacionDiariaCaja.Valor);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<ResponseObtieneCajerosCierre> ObtieneCajerosCierre(GeneralRequest1 requestObtieneCajerosCierre)
        {
            ResponseQuery<ResponseObtieneCajerosCierre> response = new ResponseQuery<ResponseObtieneCajerosCierre>
            {
                Message = "Reporte obtenido satisfactoriamente.",
                State = ResponseType.Success
            };
            try
            {
                ParamOut poRespuesta = new ParamOut(false);
                ParamOut poLogRespuesta = new ParamOut("");
                poLogRespuesta.Size = 100;

                response.ListEntities = repositoryPub.GetDataByProcedure<ResponseObtieneCajerosCierre>("ventas.spObtCajerosDeUnaFecha",
                    requestObtieneCajerosCierre.idSesion,
                    requestObtieneCajerosCierre.idFechaProceso,
                    poRespuesta,
                    poLogRespuesta);

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

        public ResponseQuery<ResponseSPObtPedidosPorFormasDePago> UltimasMovimientos(RequestSPObtPedidosPorFormasDePago requestSPObtPedidosPorFormasDePago)
        {
            ParamOut poRespuesta = new ParamOut(false);
            ParamOut poLogRespuesta = new ParamOut("");

            ResponseQuery<ResponseSPObtPedidosPorFormasDePago> response = new ResponseQuery<ResponseSPObtPedidosPorFormasDePago> { Message = "¨Ultimos Movimientos obtenidos correctamente", State = ResponseType.Success };
            try
            {
                requestSPObtPedidosPorFormasDePago.idEstado = 0;
                response.ListEntities = repositoryPub.GetDataByProcedure<ResponseSPObtPedidosPorFormasDePago>("ventas.spObtPedidosPorFormasDePago",
                    requestSPObtPedidosPorFormasDePago.idSesion,
                    requestSPObtPedidosPorFormasDePago.idFechaProceso,
                    requestSPObtPedidosPorFormasDePago.idAlmacen,
                    requestSPObtPedidosPorFormasDePago.idEstado,
                    requestSPObtPedidosPorFormasDePago.idOperacionDiaria,
                    poRespuesta, poLogRespuesta);
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


        public ResponseObject<SaldoCajaDTO> CierreCaja(SaldoCajaDTO requestAperturaCaja)
        {
            ParamOut poRespuesta = new ParamOut(false);
            ParamOut poLogRespuesta = new ParamOut("");
            ResponseObject<SaldoCajaDTO> response = new ResponseObject<SaldoCajaDTO> { Message = "La caja se cerro correctamente", State = ResponseType.Success };
            try
            {

                response.Data = repositoryPub.GetDataByProcedure<SaldoCajaDTO>("shFabula.spCierreCaja", requestAperturaCaja.idSesion, requestAperturaCaja.idCaja, requestAperturaCaja.SaldoUsuario, requestAperturaCaja.Observacion, poRespuesta, poLogRespuesta).FirstOrDefault();
                if (response.Data == null)
                {
                    response.State = ResponseType.Error;
                    response.Message = "Exisitio un error al cerrar la caja " + requestAperturaCaja.idOperacionDiariaCaja.ToString();
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

        public Response CerrarCaja(RequestSPCierreCajero requestSPCierreCajero)
        {
            ParamOut poRespuesta = new ParamOut(false);
            ParamOut poLogRespuesta = new ParamOut("");

            Response response = new Response { Message = "¨Cierre de Stock Almacen ejecutado correctamente", State = ResponseType.Success };
            try
            {
                requestSPCierreCajero.idEstado = 1;
                repositoryPub.CallProcedure<Response>("ventas.spCierreCajero",
                    requestSPCierreCajero.idSesion,
                    requestSPCierreCajero.idfechaproceso,
                    requestSPCierreCajero.idOperacionDiariaCaja,
                    requestSPCierreCajero.detalle,
                    requestSPCierreCajero.MontoApertura,
                    requestSPCierreCajero.MontoTotalCierre,
                    requestSPCierreCajero.idEstado,
                    requestSPCierreCajero.Observaciones,
                    poRespuesta,
                    poLogRespuesta
                    );
                repositoryPub.Commit();

                if ((bool)poRespuesta.Valor)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }
            }
            catch (Exception ex)
            {
                repositoryPub.Rollback();
                ProcessError(ex, response);
            }
            return response;
        }

        #endregion

        #region Ventas
        public ResponseQuery<ResulProductoPrecioVentaComplex> ObtienePorAlmacen(RequestSearchProductAlmacen requestSearchProductAlmacen)
        {
            ResponseQuery<ResulProductoPrecioVentaComplex> response = new ResponseQuery<ResulProductoPrecioVentaComplex> { Message = "Productos obtenidos correctamente", State = ResponseType.Success };
            try
            {
                response.ListEntities = new List<ResulProductoPrecioVentaComplex>();
                ParamOut paramOutRespuesta = new ParamOut(true);
                ParamOut paramOutLogRespuesta = new ParamOut("");
                paramOutLogRespuesta.Size = 100;

                var resulBD = repositoryPub.GetDataByProcedure<ResulProductoPrecioVenta>("[ventas].[spObtienePreciosAlmacen]",
                    requestSearchProductAlmacen.idSesion,
                    requestSearchProductAlmacen.idAlmacen,
                    "%",
                    paramOutRespuesta,
                    paramOutLogRespuesta);

                resulBD.ForEach(x =>
                {
                    ResulProductoPrecioVentaComplex resulProductoPrecioVentaComplex;
                    if (response.ListEntities.Any(y => y.categoria.Equals(x.categoria)))
                    {
                        resulProductoPrecioVentaComplex = response.ListEntities.First(y => y.categoria.Equals(x.categoria));
                    }
                    else
                    {
                        resulProductoPrecioVentaComplex = new ResulProductoPrecioVentaComplex
                        {
                            categoria = x.categoria,
                            etiquetaDerecha = x.etiquetaDerecha,
                            etiquetaIzquierda = x.etiquetaIzquierda,
                            slide = x.slide,
                            detalle = new List<ResulProductoPrecioVenta>()
                        };
                        response.ListEntities.Add(resulProductoPrecioVentaComplex);
                    }
                    resulProductoPrecioVentaComplex.detalle.Add(x);
                });

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public Response RegistrarVentas(RequestRegistroVenta requestRegistroVentas)
        {
            Response response = new Response { Message = "Venta registrada correctamente", State = ResponseType.Success };
            try
            {

                ParamOut paramOutRespuesta = new ParamOut(false);
                ParamOut paramOutidPedidoMaestro = new ParamOut(0);
                ParamOut paramOutLogRespuesta = new ParamOut("");
                paramOutLogRespuesta.Size = 100;

                /////TODO verificamos antes si existe en stock el producto 
                //var resulBDExistenciaStock = repositoryPub.GetDataByProcedure<ResulSPVerificaEnStock>("shFabula.spVerificaEnStock",
                //    requestRegistroVentas.idSesion,
                //    requestRegistroVentas.idFechaProceso,
                //    requestRegistroVentas.idAlmancen,
                //    requestRegistroVentas.detalleVentas,
                //    paramOutRespuesta,
                //    paramOutLogRespuesta);

                //if (resulBDExistenciaStock.Count > 0)
                //{
                //    response.State = ResponseType.Warning;
                //    response.Code = "SIN_STOCK";
                //    response.Message = "Existen producto no disponibles en el stock de la barra solictada!!!";
                //    response.ListEntities = resulBDExistenciaStock;
                //    return response;
                //}

                repositoryPub.CallProcedure<Response>("[ventas].[spAddPedido]",
                    requestRegistroVentas.idSesion,
                    requestRegistroVentas.idFechaProceso,
                    0, //@idMesero
                    requestRegistroVentas.idAlmancen,
                    requestRegistroVentas.idOperacionDiariaCaja,
                    0,//requestRegistroVentas.idAmabiente,
                    requestRegistroVentas.detalleVentas.Sum(x => x.cantidad * x.PrecioFinal),
                    requestRegistroVentas.detalleVentas,
                    requestRegistroVentas.formasDePago.Where(x => x.MontoCubierto > 0).ToList(),
                    2, //@estadoPedido
                    requestRegistroVentas.Observaciones, //@Observaciones AHORA ES vKey numero de la tarjeta
                    paramOutidPedidoMaestro, //@idPedidoMaestro
                    paramOutRespuesta,
                    paramOutLogRespuesta);

                if ((bool)paramOutRespuesta.Valor)
                {
                    response.Message = paramOutLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }

                repositoryPub.Commit();
                //Genera el archivo para llevarlo a la base de datos
                List<PedidoDTO> colPedidoDTO = new List<PedidoDTO>();
                requestRegistroVentas.detalleVentas.ForEach(x =>
                {
                    colPedidoDTO.Add(new PedidoDTO() { idProducto = x.idProducto, producto = x.nombreProducto, cantidad = x.cantidad.Value, precioUnitario = x.PrecioFinal.Value, idPedido = (int)paramOutidPedidoMaestro.Valor});
                });

                VoucherPedido reporte = new VoucherPedido();
                string nombreArchivo = "";
                string reporteBase64 = "";

                string formaPago = "";
                requestRegistroVentas.formasDePago.ForEach(x =>
                {
                    switch (x.idFormaPago)
                    {
                        case 1:
                            formaPago += formaPago == "" ? "EFECTIVO" : ",EFECTIVO";
                            break;
                        case 2:
                            formaPago += formaPago == "" ? "POS" : ",POS";
                            break;
                        case 3:
                            formaPago += formaPago == "" ? "TICKETS" : ",TICKETS";
                            break;
                        case 4:
                            formaPago += formaPago == "" ? "CORTESIA" : ",CORTESIA";
                            break;
                        default:
                            break;
                    }

                });
                reporte.xsFormaPago.Text = formaPago;
                reporte.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
                reporte.RollPaper = true;
                reporte.Margins.Left = 10;
                reporte.Margins.Right = 10;
                reporte.Margins.Bottom = 10;
                reporte.Margins.Bottom = 40;
                reporte.xrFecha.Text = DateTime.Now.ToString();
                reporte.xrUsuario.Text = requestRegistroVentas.usuario;

                reporte.DataSource = colPedidoDTO;
                string fileName = Path.Combine("c:\\tmp\\", "pedido_" + Guid.NewGuid() + ".pdf");
                reporte.ExportToPdf(fileName);
                nombreArchivo = fileName;
                reporteBase64 = Convert.ToBase64String(File.ReadAllBytes(fileName));

                response.Code = reporteBase64;
                repositoryPub.CallProcedure<Response>("reportes.spAddImpresion",
                    requestRegistroVentas.idSesion,
                    requestRegistroVentas.idOperacionDiariaCaja,
                    reporteBase64,
                    nombreArchivo = fileName);
                repositoryPub.Commit();

                if (Convert.ToInt32(paramOutRespuesta.Valor) != 0)
                {
                    response.State = ResponseType.Warning;
                    response.Message = Convert.ToString(paramOutLogRespuesta.Valor);
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseQuery<ResulObtieneFormasdePago> ObtieneFormasdePago(GeneralRequest1 requestSPObtFormasDePago)
        {
            ResponseQuery<ResulObtieneFormasdePago> response = new ResponseQuery<ResulObtieneFormasdePago> { Message = "Formas de pago obtenidos correctamente", State = ResponseType.Success };
            try
            {
                ParamOut paramOutRespuesta = new ParamOut(true);
                ParamOut paramOutLogRespuesta = new ParamOut("");
                paramOutLogRespuesta.Size = 100;

                response.ListEntities = repositoryPub.GetDataByProcedure<ResulObtieneFormasdePago>("ventas.spObtFormasDePago",
                    requestSPObtFormasDePago.idSesion,
                    requestSPObtFormasDePago.idFechaProceso,
                    paramOutRespuesta,
                    paramOutLogRespuesta);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<DetallePedidosDTO> DetallePedidoPorFormaPago(GeneralRequestRangoFecha requestGeneral)
        {
            ResponseQuery<DetallePedidosDTO> response = new ResponseQuery<DetallePedidosDTO> { Message = "Detalle de pedidos del dia obtenidos", State = ResponseType.Success };
            try
            {
                ParamOut paramOutRespuesta = new ParamOut(true);
                ParamOut paramOutLogRespuesta = new ParamOut("");
                paramOutLogRespuesta.Size = 100;

                response.ListEntities = repositoryPub.GetDataByProcedure<DetallePedidosDTO>("[reportes].[spObtPedidosPorFormaDePago]",
                    requestGeneral.idSesion,
                    requestGeneral.fechaProceso.Date,
                    requestGeneral.fechaProcesoFin.Date);
                //requestGeneral.idFechaProceso,
                //requestGeneral.fechaProceso.Date.AddDays(-1));
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<DetallePedidosDTO> ActualizaFormaPagoPedido(PedidoRequest requestPedido)
        {
            ParamOut poRespuesta = new ParamOut(false);
            ParamOut poLogRespuesta = new ParamOut("");
            ResponseQuery<DetallePedidosDTO> response = new ResponseQuery<DetallePedidosDTO> { Message = "Forma de pago actualizada", State = ResponseType.Success };
            try
            {

                repositoryPub.CallProcedure<Response>("[ventas].[spActualizaFormaDePago]", requestPedido.idSesion, requestPedido.idPedidoMaster, requestPedido.idFormaPago, poRespuesta);
                if ((bool)poRespuesta.Valor)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }
                repositoryPub.Commit();
                response.ListEntities = repositoryPub.GetDataByProcedure<DetallePedidosDTO>("[reportes].[spObtPedidosPorFormaDePago]",
                    requestPedido.idSesion,
                    requestPedido.idFechaProceso);

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
                repositoryPub.Rollback();
            }
            return response;
        }

        public ResponseQuery<DetallePedidosDTO> AnulaPedido(PedidoRequest requestPedido)
        {
            ParamOut poRespuesta = new ParamOut(false);
            ParamOut poLogRespuesta = new ParamOut("");
            ResponseQuery<DetallePedidosDTO> response = new ResponseQuery<DetallePedidosDTO> { Message = "Forma de pago actualizada", State = ResponseType.Success };
            try
            {

                repositoryPub.CallProcedure<Response>("[ventas].[spAnulaPedido]", requestPedido.idSesion, requestPedido.idPedidoMaster, poRespuesta);
                if ((bool)poRespuesta.Valor)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    repositoryPub.Rollback();
                    return response;
                }
                repositoryPub.Commit();
                response.ListEntities = repositoryPub.GetDataByProcedure<DetallePedidosDTO>("[reportes].[spObtPedidosPorFormaDePago]",
                    requestPedido.idSesion,
                    requestPedido.idFechaProceso);

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
                repositoryPub.Rollback();

            }
            return response;
        }

        public ResponseQuery<ResulProductoPrecioVenta> ObtieneProductosVenta(GeneralRequest1 request)
        {
            ResponseQuery<ResulProductoPrecioVenta> response = new ResponseQuery<ResulProductoPrecioVenta> { Message = "Venta registrada correctamente", State = ResponseType.Success };
            try
            {
                ParamOut paramOutRespuesta = new ParamOut(true);
                ParamOut paramOutLogRespuesta = new ParamOut("");
                paramOutLogRespuesta.Size = 100;
                response.ListEntities = repositoryPub.GetDataByProcedure<ResulProductoPrecioVenta>("inventario.spObtProductos", request.idSesion);

                if ((bool)paramOutRespuesta.Valor)
                {
                    response.Message = paramOutLogRespuesta.Valor.ToString();
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

        #endregion

        #region Reportes Cola
        public ResponseQuery<PrinterLineResponse> GetDocumentPending(PrinterLineRequest printerLineRequest)
        {
            ResponseQuery<PrinterLineResponse> response = new ResponseQuery<PrinterLineResponse> { Message = "Documentos obtenidos", State = ResponseType.Success };
            try
            {
                response.ListEntities = new List<PrinterLineResponse>();
                response.ListEntities = repositoryPub.GetDataByProcedure<PrinterLineResponse>("reportes.spObtImpresioneaPendientes",
                    printerLineRequest.PrinterId);
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
