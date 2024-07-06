using DevExpress.CodeParser;
using Invoice.Core.Artefacts.Invoice;
using Invoice.Core.Base;
using Invoice.Core.Business.InvoiceConfiguration.Parameters;
using Invoice.Core.Business.InvoiceKind.BasicExpenses;
using Invoice.Core.Business.InvoiceKind.Financial;
using Invoice.Core.Business.Office;
using Invoice.Core.DBEntities;
using Invoice.Core.Domain.Contracts;
using Invoice.Core.InvoiceKind.BasicExpenses;
using Invoice.Core.InvoiceKind.Orders;
using Invoice.Core.UtileServices;
using Newtonsoft.Json;
using PlumbingProps.Config;
using PlumbingProps.CrossUtil;
using PlumbingProps.Wrapper;
using SIN.OperationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.Data.Filtering.Helpers.SubExprHelper.ThreadHoppingFiltering;

namespace Invoice.Core.Business.InvoiceKind.Orders
{
    public class AdapterInvoice(ContextCompany contextCompany) : BaseManager, IAdapterInvoice
    {
        private const int CODIGO_SIN_UNIDAD_MEDIDAD_SERVICIO = 58;
        private string CODIGO_SIN_PRODUCTO = "83131";
        private string CODIGO_ACTIVIDAD_ECONOMICA = "620000";
        private const int CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA = 1;
        private IHeaderFacturable GetHeader(IFacturable factura)
        {
            if (factura is not RequestCompraVenta)
            {
                throw new ArgumentException($"la factura no corresponde a compra venta {nameof(RequestCompraVenta)}");
            }

            FacturaHelper facturaHelper = new FacturaHelper();
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            RequestCompraVenta facturaCompraVenta = (factura as RequestCompraVenta)!;
            DBEntities.Office office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
            DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
            DBEntities.Cui cui = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
            DBEntities.Cufd cufd = repositoryFacturacion.SimpleSelect<DBEntities.Cufd>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
            DateTime fechaEmsion = DateTime.Now;

            ///TODO armamso los parametros de CUFD
            CUFDHeaderRequest parametrosCUF = new CUFDHeaderRequest
            {
                cafc = parameterManager.CAFC,
                CUFD = cufd.ValorCufd,
                CUFDCodigoControl = cufd.CodigoControl,
                FechaEmisionFactura = fechaEmsion,
                FechaHora = Convert.ToInt64(fechaEmsion.ToString("yyyyMMddHHmmssfff")),
                Modalidad = parameterManager.MODALIDAD_ELECTRONICA,
                NIT = Convert.ToInt64(company.Nit),
                NumeroFactura = facturaCompraVenta.numeroFactura,
                PuntoVenta = office.SinidPuntoVenta,
                Sucursal = Convert.ToInt64(office.SincodigoSucursal),
                TipoDocumentoSector = CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA,
                TipoEmision = Convert.ToInt32(parameterManager.SYSTEM_MODO! == "0" ? parameterManager.EMISION_LINEA : parameterManager.EMISION_NOLINEA),
                TipoFactura = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL)
            };

            ///TODO obtenemos la leyenda
            var resulLeyenda = parameterManager.ObtenerUnaLeyenda(Convert.ToInt32(CODIGO_ACTIVIDAD_ECONOMICA));
            if (resulLeyenda.State != ResponseType.Success)
            {
                throw new ArgumentException($"Error al obtener las leyendas: {resulLeyenda.Message}");
            }

            IHeaderFacturable headerFacturable = new HeaderVentaCompra
            {
                codigoCliente = facturaCompraVenta.codigoCliente,
                codigoDocumentoSector = CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA,
                //codigoMetodoPago = parameterManager.METODO_PAGO_EFECTIVO,
                codigoMetodoPago = facturaCompraVenta.codigoMetodoPago,
                codigoMoneda = parameterManager.CODIGO_MONEDA_BS,
                codigoPuntoVenta = office.SinidPuntoVenta == 0 ? null : office.SinidPuntoVenta,
                codigoSucursal = Convert.ToInt32(office.SincodigoSucursal),
                codigoTipoDocumentoIdentidad = facturaCompraVenta.codigoTipoDocumentoIdentidad,//1 = CI
                complemento = facturaCompraVenta.complemento,
                cuf = facturaHelper.GenerarCUF(parametrosCUF),
                cufd = parametrosCUF.CUFD,
                direccion = office.Direccion,
                fechaEmision = parametrosCUF.FechaEmisionFactura.ToString(FacturaHelper.pFormatoDate),
                leyenda = resulLeyenda.Code,
                montoTotal = Math.Round(facturaCompraVenta.montoTotal, 2, MidpointRounding.AwayFromZero),
                montoTotalMoneda = Math.Round(facturaCompraVenta.montoTotal, 2, MidpointRounding.AwayFromZero),
                montoTotalSujetoIva = Math.Round(facturaCompraVenta.montoTotalSujetoIva, 2, MidpointRounding.AwayFromZero),
                municipio = facturaCompraVenta.municipio,
                nitEmisor = Convert.ToInt64(company.Nit),
                nombreRazonSocial = facturaCompraVenta.nombreRazonSocial,
                numeroDocumento = facturaCompraVenta.numeroDocumento,
                numeroFactura = facturaCompraVenta.numeroFactura,
                numeroTarjeta = facturaCompraVenta.numeroTarjeta,
                razonSocialEmisor = company.SocialReason,
                telefono = office.Telefono,
                tipoCambio = 1,
                usuario = "GAMATEK_SYSTEM_COMPRAVENTA",
                cafc = string.IsNullOrEmpty(parametrosCUF.cafc) ? null : parametrosCUF.cafc,
                codigoExcepcion = facturaCompraVenta.codigoExcepcion == 0 ? null : facturaCompraVenta.codigoExcepcion,
                descuentoAdicional = facturaCompraVenta.descuentoAdicional,
                montoGiftCard = facturaCompraVenta.montoGiftCard
            };
            MapHelper mapHelper = new MapHelper();
            return headerFacturable;
        }

        private List<IDetailFacturable> GetDetail(IFacturable factura)
        {
            if (factura is not RequestCompraVenta)
            {
                throw new ArgumentException($"la factura no corresponde a compra venta {nameof(RequestCompraVenta)}");
            }

            RequestCompraVenta facturaCompraVenta = (factura as RequestCompraVenta)!;
            List<IDetailFacturable> detailFacturables = new List<IDetailFacturable>();

            facturaCompraVenta.detalle.ForEach(detalle =>
            {
                IDetailFacturable detailFacturable = new DetailCompraVenta
                {
                    actividadEconomica = CODIGO_ACTIVIDAD_ECONOMICA,
                    cantidad = detalle.cantidad,
                    codigoProducto = CODIGO_SIN_PRODUCTO,
                    codigoProductoSin = Convert.ToInt32(CODIGO_SIN_PRODUCTO),
                    descripcion = detalle.descripcion,
                    montoDescuento = Math.Round(detalle.montoDescuento, 2, MidpointRounding.AwayFromZero),
                    precioUnitario = Math.Round(detalle.precioUnitario, 2, MidpointRounding.AwayFromZero),
                    subTotal = Math.Round(detalle.subTotal, 2, MidpointRounding.AwayFromZero),
                    unidadMedida = Convert.ToString(CODIGO_SIN_UNIDAD_MEDIDAD_SERVICIO),
                    numeroImei = detalle.numeroImei,
                    numeroSerie = detalle.numeroSerie
                };
                detailFacturables.Add(detailFacturable);
            });

            return detailFacturables;
        }

        public IInvoice GetInvoice(IFacturable factura)
        {
            IInvoice invoice = new FacturaCompraVenta();
            SetCodigos();
            invoice.header = GetHeader(factura);
            invoice.detail = GetDetail(factura);
            return invoice;
        }

        public (string fileName, string dataB64) GetReportInvoice(IInvoice invoice, string pathReports)
        {
            (string, string) resul = (string.Empty, string.Empty);
            if (invoice is not FacturaCompraVenta)
            {
                throw new ArgumentException($"la factura no corresponde a compra venta {nameof(FacturaCompraVenta)}");
            }
            DBEntities.Office office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
            DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
            FacturaCompraVenta facturaCompraVenta = (invoice as FacturaCompraVenta)!;
            ReportCompraVentaDTO reportCompraVentaDTO = new ReportCompraVentaDTO();
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            var resulLeyenda = parameterManager.ObtenerLeyenda3Leyendas(Convert.ToInt32(CODIGO_ACTIVIDAD_ECONOMICA));

            string linkQR = parameterManager.LINK_QR;
            linkQR = linkQR.Replace("valorNit", Convert.ToString(invoice.header.nitEmisor));
            linkQR = linkQR.Replace("valorCuf", Convert.ToString(invoice.header.cuf));
            linkQR = linkQR.Replace("valorNroFactura", Convert.ToString(invoice.header.numeroFactura));
            linkQR = linkQR.Replace("valorTamaño", "1");

            MapHelper mapHelper = new MapHelper();
            FacturaHelper facturaHelper = new FacturaHelper();
            reportCompraVentaDTO.QR = linkQR;// facturaHelper.GenerarQR(linkQR);

            reportCompraVentaDTO = mapHelper.MapObject<HeaderVentaCompra, ReportCompraVentaDTO>((facturaCompraVenta.header as HeaderVentaCompra)!);
            reportCompraVentaDTO.detalle = new List<CompraVentaDetalleDTO>();
            facturaCompraVenta.detail.ForEach(detail =>
            {
                DetailCompraVenta detailCompraVenta = (detail as DetailCompraVenta)!;
                CompraVentaDetalleDTO compraVentaDetalleDTO = mapHelper.MapObject<DetailCompraVenta, CompraVentaDetalleDTO>(detailCompraVenta);
                compraVentaDetalleDTO.unidadMedida = "UNIDAD (SERVICIOS)";
                reportCompraVentaDTO.detalle.Add(compraVentaDetalleDTO);
            });

            YKY.ModuloFacturacion.Reports.FacturaCompraVenta reporte = new YKY.ModuloFacturacion.Reports.FacturaCompraVenta();
            reportCompraVentaDTO.sucursalCasaMatriz = office.Name;
            reportCompraVentaDTO.puntoVenta = office.SinidPuntoVenta.ToString();
            reportCompraVentaDTO.montoliteral = facturaHelper.enletras(reportCompraVentaDTO.montoTotalSujetoIva.ToString());
            reportCompraVentaDTO.leyenda = resulLeyenda.Code;

            reporte.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            reporte.RollPaper = true;
            reporte.Margins.Left = 0;
            reporte.Margins.Right = 0;
            reporte.xrQRCode.Text = linkQR;
            reporte.DataSource = new List<ReportCompraVentaDTO>() { reportCompraVentaDTO };

            string fileName = Path.Combine(pathReports, "facturaCompraVenta_" + invoice.header.numeroFactura.ToString() + "_" + Guid.NewGuid() + ".pdf");
            reporte.ExportToPdf(fileName);
            resul.Item1 = fileName;
            resul.Item2 = Convert.ToBase64String(File.ReadAllBytes(fileName));
            return resul;
        }

        public Response SendFactura(DBEntities.Invoice invoice)
        {
            Response objresult = new Response { State = ResponseType.Success };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                using SIN.OrdersServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionCompraVentaClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    var resulcufd = repositoryFacturacion.SimpleSelect<DBEntities.Cufd>(x => x.IdCufd == invoice.IdCufd).First();
                    var resulcuis = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.IdCuis == resulcufd.IdCuis).First();
                    DBEntities.Office office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
                    FacturaHelper facturaHelper = new FacturaHelper();

                    invoice.State = "ENVIANDO";
                    invoice.ModifyDate = DateTime.Now;

                    repositoryFacturacion.SaveObject<DBEntities.Invoice>(new CoreAccesLayer.Wraper.Entity<DBEntities.Invoice>
                    {
                        EntityDB = invoice,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                    });
                    repositoryFacturacion.Commit();

                    ///TODO configuramos el cliente para que espere 3 minutos al SIN
                    client.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 3, 0);
                    client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 3, 0);
                    client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 3, 0);
                    client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 3, 0);

                    SIN.OrdersServices.solicitudRecepcionFactura solicitudRecepcionFactura = new SIN.OrdersServices.solicitudRecepcionFactura
                    {
                        archivo = facturaHelper.Zip(invoice.InvoiceSign!),
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoDocumentoSector = CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA,
                        codigoEmision = Convert.ToInt32(parameterManager.EMISION_LINEA),
                        codigoModalidad = parameterManager.MODALIDAD_ELECTRONICA,
                        codigoPuntoVenta = office.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = office.SinidPuntoVenta != 0,
                        codigoSistema = company.SincodeSystem,
                        codigoSucursal = Convert.ToInt32(office.SincodigoSucursal),
                        cufd = resulcufd.ValorCufd,
                        cuis = resulcuis.ValorCuis,
                        fechaEnvio = invoice.ProcessDate?.ToString(FacturaHelper.pFormatoDate),
                        hashArchivo = invoice.InvoiceSha256,
                        nit = Convert.ToInt64(company.Nit),
                        tipoFacturaDocumento = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL)
                    };

                    var resulService = client.recepcionFacturaAsync(new SIN.OrdersServices.recepcionFactura
                    {
                        SolicitudServicioRecepcionFactura = solicitudRecepcionFactura
                    }).Result;

                    objresult.Message = invoice.State = resulService.RespuestaServicioFacturacion.codigoEstado == 908 ? "ENVIADO" : "FALLO_ENVIO";
                    objresult.State = resulService.RespuestaServicioFacturacion.codigoEstado == 908 ? ResponseType.Success : ResponseType.Warning;
                    invoice.DetailProcess = resulService.RespuestaServicioFacturacion?.codigoDescripcion!;
                    invoice.RequestSendOnline = JsonConvert.SerializeObject(solicitudRecepcionFactura);
                    invoice.ResponseSendOnline = JsonConvert.SerializeObject(resulService);
                    invoice.ModifyDate = DateTime.Now;

                    if (resulService.RespuestaServicioFacturacion?.transaccion != true)
                    {

                        resulService.RespuestaServicioFacturacion?.mensajesList?.ToList().ForEach(x =>
                        {
                            objresult.Message += $" {x.codigo} - {x.descripcion}";
                        });
                    }

                    repositoryFacturacion.SaveObject<DBEntities.Invoice>(new CoreAccesLayer.Wraper.Entity<DBEntities.Invoice>
                    {
                        EntityDB = invoice,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                    });
                    repositoryFacturacion.Commit();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, objresult);
            }
            return objresult;
        }

        public Response AnularFactura(int idInvoice, int idMotivo = 1)
        {
            Response objresult = new Response { State = ResponseType.Success };
            try
            {
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                if (parameterManager.SYSTEM_MODO != "0")
                {
                    objresult.Message = "No se puede anular la factura, el sistema esta fuera de linea";
                    objresult.State = ResponseType.Warning;
                    return objresult;
                }
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                using SIN.OrdersServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionCompraVentaClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    var resulInvoice = repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == idInvoice);
                    if (resulInvoice.Count == 0)
                    {
                        objresult.State = ResponseType.Warning;
                        objresult.Message = "La factura no exist en la BD";
                        return objresult;
                    }

                    DBEntities.Invoice invoice = resulInvoice.First();

                    var resulcufd = repositoryFacturacion.SimpleSelect<DBEntities.Cufd>(x => x.IdOffice == invoice.IdOffice && x.EsRegistroActual).First();
                    var resulcuis = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.IdCuis == resulcufd.IdCuis).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == invoice.IdOffice).First();
                    DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();

                    SIN.OrdersServices.solicitudAnulacion solicitudAnulacion = new SIN.OrdersServices.solicitudAnulacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoDocumentoSector = CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA,
                        codigoEmision = Convert.ToInt32(parameterManager.EMISION_LINEA),
                        codigoModalidad = parameterManager.MODALIDAD_ELECTRONICA,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = company.SincodeSystem,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cufd = resulcufd.ValorCufd,
                        cuis = resulcuis.ValorCuis,
                        nit = Convert.ToInt64(company.Nit),
                        tipoFacturaDocumento = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL),
                        codigoMotivo = idMotivo,
                        cuf = invoice.Cuf
                    };
                    var resulService = client.anulacionFacturaAsync(new SIN.OrdersServices.anulacionFactura
                    {
                        SolicitudServicioAnulacionFactura = solicitudAnulacion
                    }).Result;

                    objresult.Message = resulService.RespuestaServicioFacturacion.transaccion ? "ANULADO" : "FALLO_ANULACION";
                    objresult.State = resulService.RespuestaServicioFacturacion.transaccion ? ResponseType.Success : ResponseType.Warning;

                    if (resulService.RespuestaServicioFacturacion?.transaccion != true)
                    {
                        resulService.RespuestaServicioFacturacion?.mensajesList?.ToList().ForEach(x =>
                        {
                            objresult.Message += $" {x.codigo} - {x.descripcion}";
                        });
                    }

                    invoice.DetailProcess = resulService.RespuestaServicioFacturacion?.codigoDescripcion ?? "";
                    invoice.RequestAnulacion = JsonConvert.SerializeObject(solicitudAnulacion);
                    invoice.ResponseAnulacion = JsonConvert.SerializeObject(resulService);
                    invoice.ModifyDate = DateTime.Now;
                    invoice.State = resulService.RespuestaServicioFacturacion?.transaccion == true ? "ANULADO" : "FALLO_ANULACION";

                    repositoryFacturacion.SaveObject<DBEntities.Invoice>(new CoreAccesLayer.Wraper.Entity<DBEntities.Invoice>
                    {
                        EntityDB = invoice,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                    });
                    repositoryFacturacion.Commit();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, objresult);
            }
            return objresult;
        }

        public Response SendBatchInvoices(List<DBEntities.Invoice> setInvoices, EventoSignificativo eventoSignificativo, string fileTGZ, string folderTGZ, string PATH_BATCHFILES)
        {
            Response response = new Response
            {
                Code = "00",
                Message = "Lote enviado correctamente",
                State = ResponseType.Success
            };
            DBEntities.Office resulOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == setInvoices.First().IdOffice).First();
            DBEntities.Cui resulCUIS = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == resulOffice.IdOffice).First();
            DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
            FacturaHelper facturaHelper = new FacturaHelper();
            string XMLSHA256 = facturaHelper.ConvertArryaBytesToSha256(File.ReadAllBytes(fileTGZ));
            string vTimestamp = DateTime.Now.ToString(FacturaHelper.pFormatoDate);
            int totalFacturas = setInvoices.Count;
            string codigoEstado = string.Empty;
            SetCodigos();
            SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            using SIN.OrdersServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionCompraVentaClient();
            using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
            {
                SIN.OrdersServices.solicitudRecepcionPaquete solicitudRecepcionMasiva = new SIN.OrdersServices.solicitudRecepcionPaquete
                {
                    codigoAmbiente = Convert.ToInt32(parameterManager.MOD_AMBIENTE),
                    codigoPuntoVenta = resulOffice.SinidPuntoVenta,
                    codigoSistema = company.SincodeSystem,
                    codigoSucursal = Convert.ToInt32(resulOffice.SincodigoSucursal),
                    nit = Convert.ToInt64(company.Nit),
                    codigoDocumentoSector = Convert.ToInt32(CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA),
                    codigoEmision = Convert.ToInt32(parameterManager.EMISION_NOLINEA),
                    codigoModalidad = Convert.ToInt32(parameterManager.MODALIDAD_ELECTRONICA),
                    cufd = eventoSignificativo.Cufd,
                    cuis = resulCUIS.ValorCuis,
                    tipoFacturaDocumento = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL),
                    archivo = File.ReadAllBytes(fileTGZ),
                    fechaEnvio = vTimestamp,
                    hashArchivo = XMLSHA256,
                    cantidadFacturas = totalFacturas,
                    codigoPuntoVentaSpecified = resulOffice.IdOffice != 0,
                    cafc = parameterManager.CAFC,
                    codigoEvento = Convert.ToInt64(eventoSignificativo.CodEventoSignificativo)
                };

                var resulEnvioPaquete = client.recepcionPaqueteFacturaAsync(new SIN.OrdersServices.recepcionPaqueteFactura
                {
                    SolicitudServicioRecepcionPaquete = solicitudRecepcionMasiva
                }).Result;

                ///TODO guardamos el log del envio de paquetes
                DBEntities.LogBatchProcess logBatchProcess = new DBEntities.LogBatchProcess
                {
                    CodigoDescripcionEnviado = resulEnvioPaquete.RespuestaServicioFacturacion.codigoDescripcion,
                    CodigoRecepcionEnviado = resulEnvioPaquete.RespuestaServicioFacturacion.codigoRecepcion ?? String.Empty,
                    EstadoEnviado = Convert.ToString(resulEnvioPaquete.RespuestaServicioFacturacion.codigoEstado),
                    FechaRegistro = DateTime.Now,
                    FileName = fileTGZ,
                    RequestEnviado = JsonConvert.SerializeObject(solicitudRecepcionMasiva),
                    ResponseEnviado = JsonConvert.SerializeObject(resulEnvioPaquete),
                    CodeBatch = folderTGZ,
                    IdEventoSignificativo = eventoSignificativo.IdEventoSignificativo,
                };

                repositoryFacturacion.SaveObject<DBEntities.LogBatchProcess>(
                    new CoreAccesLayer.Wraper.Entity<DBEntities.LogBatchProcess>
                    {
                        EntityDB = logBatchProcess,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.add
                    });
                repositoryFacturacion.Commit();

                ///TODO marcamos las factuas enviadas en el paquete
                setInvoices.Where(x => x.FolderContainer == Path.Combine(PATH_BATCHFILES, folderTGZ)).ToList().ForEach(x =>
                {
                    x.State = "TGZ_ENVIADO";
                    x.DetailProcess += "|" + resulEnvioPaquete.RespuestaServicioFacturacion.codigoEstado + ":" + resulEnvioPaquete.RespuestaServicioFacturacion.codigoRecepcion;
                    x.IdEventoSignificativo = eventoSignificativo.IdEventoSignificativo;
                    x.CodeBatch = folderTGZ;
                    repositoryFacturacion.SaveObject<DBEntities.Invoice>(
                        new CoreAccesLayer.Wraper.Entity<DBEntities.Invoice>
                        {
                            EntityDB = x,
                            stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                        });
                    repositoryFacturacion.Commit();
                });

                ///TODO validacion de las facturas
                SIN.OrdersServices.solicitudValidacionRecepcion solicitudServicioValidacionRecepcionPaquete = new SIN.OrdersServices.solicitudValidacionRecepcion
                {
                    codigoAmbiente = Convert.ToInt32(parameterManager.MOD_AMBIENTE),
                    codigoPuntoVenta = resulOffice.SinidPuntoVenta,
                    codigoSistema = parameterManager.COD_SISTEMA,
                    codigoSucursal = Convert.ToInt32(resulOffice.SincodigoSucursal),
                    nit = Convert.ToInt64(company.Nit),
                    codigoDocumentoSector = Convert.ToInt32(CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA),
                    codigoEmision = Convert.ToInt32(parameterManager.EMISION_NOLINEA),
                    codigoModalidad = Convert.ToInt32(parameterManager.MODALIDAD_ELECTRONICA),
                    cufd = eventoSignificativo.Cufd,
                    cuis = resulCUIS.ValorCuis,
                    tipoFacturaDocumento = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL),
                    codigoPuntoVentaSpecified = resulOffice.IdOffice != 0,
                    codigoRecepcion = resulEnvioPaquete.RespuestaServicioFacturacion.codigoRecepcion
                };
                System.Threading.Thread.Sleep(30000);
                var resulValidacion = client.validacionRecepcionPaqueteFacturaAsync(new SIN.OrdersServices.validacionRecepcionPaqueteFactura
                {
                    SolicitudServicioValidacionRecepcionPaquete = solicitudServicioValidacionRecepcionPaquete

                }).Result;
                response.State = ResponseType.Success;
                response.Message = resulValidacion.RespuestaServicioFacturacion.codigoEstado.ToString();
                resulValidacion.RespuestaServicioFacturacion.mensajesList?.ToList().ForEach(men =>
                {
                    response.Message += men.codigo + " " + men.descripcion;
                });

                ///TODO registramos el avance de validacion del paquete
                logBatchProcess.CodigoDescripcionValidado = resulValidacion.RespuestaServicioFacturacion.codigoDescripcion;
                logBatchProcess.CodigoRecepcionEnviadoValidado = resulValidacion.RespuestaServicioFacturacion.codigoRecepcion;
                logBatchProcess.EstadoValidado = Convert.ToString(resulValidacion.RespuestaServicioFacturacion.codigoEstado);
                logBatchProcess.FechaModificacion = DateTime.Now;
                logBatchProcess.RequestValidado = JsonConvert.SerializeObject(solicitudServicioValidacionRecepcionPaquete);
                logBatchProcess.ResponseValidado = JsonConvert.SerializeObject(resulValidacion);

                repositoryFacturacion.SaveObject<DBEntities.LogBatchProcess>(
                    new CoreAccesLayer.Wraper.Entity<DBEntities.LogBatchProcess>
                    {
                        EntityDB = logBatchProcess,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                    });
                repositoryFacturacion.Commit();

                ///TODO marcamos las factuas enviadas en el paquete
                setInvoices.Where(x => x.FolderContainer == Path.Combine(PATH_BATCHFILES, folderTGZ)).ToList().ForEach(x =>
                {
                    x.State = "TGZ_VALIDADO";
                    x.DetailProcess += "|" + resulValidacion.RespuestaServicioFacturacion.codigoEstado;
                    string observacion = "";

                    if (resulValidacion.RespuestaServicioFacturacion.codigoEstado == GlobalConstants.SIN_FACTURA_ESTADO_OBSERVADA &&
                        resulValidacion.RespuestaServicioFacturacion.mensajesList?.ToList().Any(validacion => validacion.numeroArchivo == Convert.ToInt16(x.IndexFile)) == true)
                    {
                        resulValidacion.RespuestaServicioFacturacion.mensajesList?
                        .ToList().Where(validacion => validacion.numeroArchivo == Convert.ToInt16(x.IndexFile)).ToList()
                        .ForEach(vMensajes =>
                        {
                            observacion += "|" + vMensajes.descripcion;
                        });
                        x.State = "OBSERVADA";
                    }
                    else if (resulValidacion.RespuestaServicioFacturacion.codigoEstado == GlobalConstants.SIN_FACTURA_ESTADO_RECHAZADA)
                    {
                        observacion += "|" + resulValidacion.RespuestaServicioFacturacion.codigoDescripcion;
                        resulValidacion.RespuestaServicioFacturacion.mensajesList?.ToList()
                        .ForEach(vMensajes =>
                        {
                            observacion += "|" + vMensajes.descripcion;
                        });
                        x.State = "RECHAZADA";
                    }
                    x.DetailProcess += observacion;
                    repositoryFacturacion.SaveObject<DBEntities.Invoice>(
                      new CoreAccesLayer.Wraper.Entity<DBEntities.Invoice>
                      {
                          EntityDB = x,
                          stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                      });
                    repositoryFacturacion.Commit();
                });
            }
            return response;
        }

        private void SetCodigos()
        {
            //CODIGO_SIN_PRODUCTO = repositoryFacturacion.SimpleSelect<DBEntities.Parameter>(x => x.KeyName == "CODIGO_SIN_PRODUCTO").First().Value;
            //CODIGO_ACTIVIDAD_ECONOMICA = repositoryFacturacion.SimpleSelect<DBEntities.Parameter>(x => x.KeyName == "CODIGO_ACTIVIDAD_ECONOMICA").First().Value;
            CODIGO_SIN_PRODUCTO = "83131";
            CODIGO_ACTIVIDAD_ECONOMICA = "620000";
        }

        public Response RevertirFactura(int idInvoice)
        {
            Response objresult = new Response { State = ResponseType.Success };
            try
            {
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                if (parameterManager.SYSTEM_MODO != "0")
                {
                    objresult.Message = "No se puede revertir la factura, el sistema esta fuera de linea";
                    objresult.State = ResponseType.Warning;
                    return objresult;
                }
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                using SIN.OrdersServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionCompraVentaClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    var resulInvoice = repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == idInvoice);
                    if (resulInvoice.Count == 0)
                    {
                        objresult.State = ResponseType.Warning;
                        objresult.Message = "La factura no exist en la BD";
                        return objresult;
                    }

                    DBEntities.Invoice invoice = resulInvoice.First();

                    var resulcufd = repositoryFacturacion.SimpleSelect<DBEntities.Cufd>(x => x.IdOffice == invoice.IdOffice && x.EsRegistroActual).First();
                    var resulcuis = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.IdCuis == resulcufd.IdCuis).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == invoice.IdOffice).First();
                    DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();

                    SIN.OrdersServices.solicitudReversionAnulacion solicitudAnulacion = new SIN.OrdersServices.solicitudReversionAnulacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoDocumentoSector = CODIGO_DOCUMENTO_SECTOR_COMPRA_VENTA,
                        codigoEmision = Convert.ToInt32(parameterManager.EMISION_LINEA),
                        codigoModalidad = parameterManager.MODALIDAD_ELECTRONICA,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = company.SincodeSystem,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cufd = resulcufd.ValorCufd,
                        cuis = resulcuis.ValorCuis,
                        nit = Convert.ToInt64(company.Nit),
                        tipoFacturaDocumento = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL),
                        cuf = invoice.Cuf
                    };
                    var resulService = client.reversionAnulacionFacturaAsync(new SIN.OrdersServices.reversionAnulacionFactura
                    { 
                        SolicitudServicioReversionAnulacionFactura = solicitudAnulacion
                    }).Result;

                    objresult.Message = resulService.RespuestaServicioFacturacion.transaccion ? "REVERTIDO" : "FALLO_REVERSION";
                    objresult.State = resulService.RespuestaServicioFacturacion.transaccion ? ResponseType.Success : ResponseType.Warning;

                    if (resulService.RespuestaServicioFacturacion?.transaccion != true)
                    {
                        resulService.RespuestaServicioFacturacion?.mensajesList?.ToList().ForEach(x =>
                        {
                            objresult.Message += $" {x.codigo} - {x.descripcion}";
                        });
                    }

                    invoice.DetailProcess = resulService.RespuestaServicioFacturacion?.codigoDescripcion ?? "";
                    invoice.RequestAnulacion = JsonConvert.SerializeObject(solicitudAnulacion);
                    invoice.ResponseAnulacion = JsonConvert.SerializeObject(resulService);
                    invoice.ModifyDate = DateTime.Now;
                    invoice.State = resulService.RespuestaServicioFacturacion?.transaccion == true ? "REVERTIDO" : "FALLO_REVERSION";

                    repositoryFacturacion.SaveObject<DBEntities.Invoice>(new CoreAccesLayer.Wraper.Entity<DBEntities.Invoice>
                    {
                        EntityDB = invoice,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                    });
                    repositoryFacturacion.Commit();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, objresult);
            }
            return objresult;
        }
    }
}
