using Invoice.Core.Artefacts.Invoice;
using Invoice.Core.Base;
using Invoice.Core.Business.InvoiceConfiguration.Parameters;
using Invoice.Core.Business.InvoiceKind.CurrencyExchange;
using Invoice.Core.Business.Office;
using Invoice.Core.DBEntities;
using Invoice.Core.Domain.Contracts;
using Invoice.Core.UtileServices;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using PlumbingProps.CrossUtil;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YKY.ModuloFacturacion.Reports;

namespace Invoice.Core.Business.InvoiceKind.Financial
{
    public class AdapterInvoice(ContextCompany contextCompany) : BaseManager, IAdapterInvoice
    {
        private const int CODIGO_SIN_UNIDAD_MEDIDAD_SERVICIO = 58;
        private string CODIGO_SIN_PRODUCTO = "49111";
        private string CODIGO_ACTIVIDAD_ECONOMICA = "451010";
        private const int CODIGO_SECTOR_FIANCIERO = 15;
        private IHeaderFacturable GetHeader(IFacturable factura)
        {
            if (factura is not RequestEntidadFinanciera)
            {
                throw new ArgumentException($"la factura no corresponde a entidad financiera {nameof(RequestEntidadFinanciera)}");
            }

            FacturaHelper facturaHelper = new FacturaHelper();
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            RequestEntidadFinanciera facturaFinanciera = (factura as RequestEntidadFinanciera)!;
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
                NumeroFactura = facturaFinanciera.numeroFactura,
                PuntoVenta = office.SinidPuntoVenta,
                Sucursal = Convert.ToInt64(office.SincodigoSucursal),
                TipoDocumentoSector = CODIGO_SECTOR_FIANCIERO,
                TipoEmision = Convert.ToInt32(parameterManager.SYSTEM_MODO! == "0" ? parameterManager.EMISION_LINEA : parameterManager.EMISION_NOLINEA),
                TipoFactura = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL)
            };

            ///TODO obtenemos la leyenda
            var resulLeyenda = parameterManager.ObtenerUnaLeyenda(Convert.ToInt32(CODIGO_ACTIVIDAD_ECONOMICA));

            if (resulLeyenda.State != ResponseType.Success)
            {
                throw new ArgumentException($"Error al obtener las leyendas: {resulLeyenda.Message}");
            }

            IHeaderFacturable headerFacturable = new HeaderEntidadFinanciera
            {
                codigoCliente = facturaFinanciera.codigoCliente,
                codigoDocumentoSector = CODIGO_SECTOR_FIANCIERO,
                codigoMetodoPago = facturaFinanciera.codigoMetodoPago,
                codigoMoneda = facturaFinanciera.codigoMoneda,
                codigoPuntoVenta = office.SinidPuntoVenta,
                codigoSucursal = Convert.ToInt32(office.SincodigoSucursal),
                codigoTipoDocumentoIdentidad = facturaFinanciera.codigoTipoDocumentoIdentidad,//1 = CI
                complemento = facturaFinanciera.complemento,
                cuf = facturaHelper.GenerarCUF(parametrosCUF),
                cufd = parametrosCUF.CUFD,
                direccion = office.Direccion,
                fechaEmision = parametrosCUF.FechaEmisionFactura.ToString(FacturaHelper.pFormatoDate),
                leyenda = resulLeyenda.Code,
                montoTotal = Math.Round(facturaFinanciera.montoTotal, 2, MidpointRounding.AwayFromZero),
                montoTotalMoneda = Math.Round(facturaFinanciera.montoTotalMoneda, 2, MidpointRounding.AwayFromZero),
                montoTotalSujetoIva = Math.Round(facturaFinanciera.montoTotalSujetoIva, 2, MidpointRounding.AwayFromZero),
                municipio = facturaFinanciera.municipio,
                nitEmisor = Convert.ToInt64(company.Nit),
                nombreRazonSocial = facturaFinanciera.nombreRazonSocial,
                numeroDocumento = facturaFinanciera.numeroDocumento,
                numeroFactura = facturaFinanciera.numeroFactura,
                numeroTarjeta = facturaFinanciera.numeroTarjeta,
                razonSocialEmisor = company.SocialReason,
                telefono = office.Telefono,
                tipoCambio = facturaFinanciera.tipoCambio,
                usuario = "GAMATEK_SYSTEM_FINANCIERO",
                cafc = string.IsNullOrEmpty(parametrosCUF.cafc) ? null : parametrosCUF.cafc,
                codigoExcepcion = facturaFinanciera.codigoExcepcion,
                descuentoAdicional = facturaFinanciera.descuentoAdicional,
                montoTotalArrendamientoFinanciero = facturaFinanciera.montoTotalArrendamientoFinanciero
            };

            return headerFacturable;
        }

        private List<IDetailFacturable> GetDetail(IFacturable factura)
        {
            if (factura is not RequestEntidadFinanciera)
            {
                throw new ArgumentException($"la factura no corresponde a entidad financiera {nameof(RequestEntidadFinanciera)}");
            }

            RequestEntidadFinanciera facturableServicioBasico = (factura as RequestEntidadFinanciera)!;
            IDetailFacturable detailFacturable = new DetailEntidadFinanciera
            {
                actividadEconomica = CODIGO_ACTIVIDAD_ECONOMICA,
                cantidad = facturableServicioBasico.detalle[0].cantidad,
                codigoProducto = CODIGO_SIN_PRODUCTO,
                codigoProductoSin = Convert.ToInt32(CODIGO_SIN_PRODUCTO),
                descripcion = facturableServicioBasico.detalle[0].descripcion,
                montoDescuento = Math.Round(facturableServicioBasico.detalle[0].montoDescuento, 2, MidpointRounding.AwayFromZero),
                precioUnitario = Math.Round(facturableServicioBasico.detalle[0].precioUnitario, 2, MidpointRounding.AwayFromZero),
                subTotal = Math.Round(facturableServicioBasico.detalle[0].subTotal, 2, MidpointRounding.AwayFromZero),
                unidadMedida = Convert.ToString(CODIGO_SIN_UNIDAD_MEDIDAD_SERVICIO)
            };
            List<IDetailFacturable> detailFacturables = new List<IDetailFacturable>() { detailFacturable };
            return detailFacturables;
        }

        public IInvoice GetInvoice(IFacturable factura)
        {
            IInvoice invoice = new FacturaEntidadFinanciera();
            SetCodigos();
            invoice.header = GetHeader(factura);
            invoice.detail = GetDetail(factura);
            return invoice;
        }

        public (string, string) GetReportInvoice(IInvoice invoice, string pathReports)
        {
            (string, string) resul = (string.Empty, string.Empty);
            if (invoice is not FacturaEntidadFinanciera)
            {
                throw new ArgumentException($"la factura no corresponde a compra venta {nameof(FacturaEntidadFinanciera)}");
            }
            DBEntities.Office office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
            DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
            FacturaEntidadFinanciera facturaEntidadFinanciera = (invoice as FacturaEntidadFinanciera)!;
            ReportEntidadFinancieraDTO reportEntidadFinancieraDTO = new ReportEntidadFinancieraDTO();
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            var resulLeyenda = parameterManager.ObtenerUnaLeyenda(Convert.ToInt32(CODIGO_ACTIVIDAD_ECONOMICA));

            string linkQR = parameterManager.LINK_QR;
            linkQR = linkQR.Replace("valorNit", Convert.ToString(invoice.header.nitEmisor));
            linkQR = linkQR.Replace("valorCuf", Convert.ToString(invoice.header.cuf));
            linkQR = linkQR.Replace("valorNroFactura", Convert.ToString(invoice.header.numeroFactura));
            linkQR = linkQR.Replace("valorTamaño", "2");

            MapHelper mapHelper = new MapHelper();
            FacturaHelper facturaHelper = new FacturaHelper();
            reportEntidadFinancieraDTO = mapHelper.MapObject<HeaderEntidadFinanciera, ReportEntidadFinancieraDTO>((facturaEntidadFinanciera.header as HeaderEntidadFinanciera)!);
            reportEntidadFinancieraDTO.detalle = new List<EntidadFinancieraDetalleDTO>();
            facturaEntidadFinanciera.detail.ForEach(detail =>
            {
                DetailEntidadFinanciera detailFinanciero = (detail as DetailEntidadFinanciera)!;
                EntidadFinancieraDetalleDTO entidadFinancieraDetalleDTO = mapHelper.MapObject<DetailEntidadFinanciera, EntidadFinancieraDetalleDTO>(detailFinanciero);
                entidadFinancieraDetalleDTO.unidadMedida = "UNIDAD (SERVICIOS)";
                reportEntidadFinancieraDTO.detalle.Add(entidadFinancieraDetalleDTO);
            });

            YKY.ModuloFacturacion.Reports.FacturaEntidadFinaciera reporte = new YKY.ModuloFacturacion.Reports.FacturaEntidadFinaciera();
            reportEntidadFinancieraDTO.sucursalCasaMatriz = office.Name;
            reportEntidadFinancieraDTO.puntoVenta = office.SinidPuntoVenta.ToString();
            reportEntidadFinancieraDTO.montoliteral = facturaHelper.enletras(reportEntidadFinancieraDTO.montoTotal.ToString());
            reportEntidadFinancieraDTO.leyenda = resulLeyenda.Code;

            reporte.xrQRCode.Text = linkQR;

            reporte.DataSource = new List<ReportEntidadFinancieraDTO>() { reportEntidadFinancieraDTO };

            string fileName = Path.Combine(pathReports, "facturaEntidadFinanciera_" + invoice.header.numeroFactura.ToString() + "_" + Guid.NewGuid() + ".pdf");
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
                using SIN.FinancialServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionFinancieroClient();
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

                    SIN.FinancialServices.solicitudRecepcionFactura solicitudRecepcionFactura = new SIN.FinancialServices.solicitudRecepcionFactura
                    {
                        archivo = facturaHelper.Zip(invoice.InvoiceSign!),
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoDocumentoSector = CODIGO_SECTOR_FIANCIERO,
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

                    var resulService = client.recepcionFacturaAsync(new SIN.FinancialServices.recepcionFactura
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
                using SIN.FinancialServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionFinancieroClient();
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

                    SIN.FinancialServices.solicitudAnulacion solicitudAnulacion = new SIN.FinancialServices.solicitudAnulacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoDocumentoSector = CODIGO_SECTOR_FIANCIERO,
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
                    var resulService = client.anulacionFacturaAsync(new SIN.FinancialServices.anulacionFactura
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
            using SIN.FinancialServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionFinancieroClient();
            using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
            {
                SIN.FinancialServices.solicitudRecepcionPaquete solicitudRecepcionMasiva = new SIN.FinancialServices.solicitudRecepcionPaquete
                {
                    codigoAmbiente = Convert.ToInt32(parameterManager.MOD_AMBIENTE),
                    codigoPuntoVenta = resulOffice.SinidPuntoVenta,
                    codigoSistema = company.SincodeSystem,
                    codigoSucursal = Convert.ToInt32(resulOffice.SincodigoSucursal),
                    nit = Convert.ToInt64(company.Nit),
                    codigoDocumentoSector = Convert.ToInt32(CODIGO_SECTOR_FIANCIERO),
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

                var resulEnvioPaquete = client.recepcionPaqueteFacturaAsync(new SIN.FinancialServices.recepcionPaqueteFactura
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
                SIN.FinancialServices.solicitudValidacionRecepcion solicitudServicioValidacionRecepcionPaquete = new SIN.FinancialServices.solicitudValidacionRecepcion
                {
                    codigoAmbiente = Convert.ToInt32(parameterManager.MOD_AMBIENTE),
                    codigoPuntoVenta = resulOffice.SinidPuntoVenta,
                    codigoSistema = parameterManager.COD_SISTEMA,
                    codigoSucursal = Convert.ToInt32(resulOffice.SincodigoSucursal),
                    nit = Convert.ToInt64(company.Nit),
                    codigoDocumentoSector = Convert.ToInt32(CODIGO_SECTOR_FIANCIERO),
                    codigoEmision = Convert.ToInt32(parameterManager.EMISION_NOLINEA),
                    codigoModalidad = Convert.ToInt32(parameterManager.MODALIDAD_ELECTRONICA),
                    cufd = eventoSignificativo.Cufd,
                    cuis = resulCUIS.ValorCuis,
                    tipoFacturaDocumento = Convert.ToInt32(parameterManager.TIPO_FACTURA_CREDFISCAL),
                    codigoPuntoVentaSpecified = resulOffice.IdOffice != 0,
                    codigoRecepcion = resulEnvioPaquete.RespuestaServicioFacturacion.codigoRecepcion
                };
                System.Threading.Thread.Sleep(30000);
                var resulValidacion = client.validacionRecepcionPaqueteFacturaAsync(new SIN.FinancialServices.validacionRecepcionPaqueteFactura
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
                using SIN.FinancialServices.ServicioFacturacionClient client = sinEndPointConfiguration.ServicioFacturacionFinancieroClient();
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

                    SIN.FinancialServices.solicitudReversionAnulacion solicitudAnulacion = new SIN.FinancialServices.solicitudReversionAnulacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoDocumentoSector = CODIGO_SECTOR_FIANCIERO,
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
                    var resulService = client.reversionAnulacionFacturaAsync(new SIN.FinancialServices.reversionAnulacionFactura
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
