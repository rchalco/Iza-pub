using CoreAccesLayer.Wraper;
using Invoice.Core.Artefacts.Invoice;
using Invoice.Core.Base;
using Newtonsoft.Json;
using PlumbingProps.CrossUtil;
using PlumbingProps.Wrapper;
using SIN.CodesServices;
using Invoice.Core.Domain.Contracts;
using Invoice.Core.Business.InvoiceConfiguration.Parameters;
using Invoice.Core.Business.Office;
using Invoice.Core.Business.InvoiceKind.Financial;
using PlumbingProps.Config;
using System.Xml;
using Invoice.Core.Business.InvoiceKind.BasicExpenses;
using Invoice.Core.Business.InvoiceKind.Orders;
using Invoice.Core.UtileServices;
using System.Reflection.Metadata;
using Invoice.Core.Domain;
using Invoice.Core.Business.InvoiceKind.CurrencyExchange;

namespace Invoice.Core.Engines
{
    public class EngineInvoiceSender(ContextCompany contextCompany) : BaseManager
    {
        Dictionary<Type, string> configurationEnvelopXMLInvoice = new Dictionary<Type, string>();
        public string RSAKey = string.Empty;
        public string PublicKey = string.Empty;

        ParameterManager parameterManager = new ParameterManager(contextCompany);

        Dictionary<Type, string> ConfigurationEnvelopXMLInvoice
        {
            get
            {
                if (configurationEnvelopXMLInvoice.Count == 0)
                {
                    configurationEnvelopXMLInvoice.Add(typeof(RequestServicioBasico), "facturaElectronicaServicioBasico");
                    configurationEnvelopXMLInvoice.Add(typeof(RequestEntidadFinanciera), "facturaElectronicaEntidadFinanciera");
                    configurationEnvelopXMLInvoice.Add(typeof(RequestCompraVenta), "facturaElectronicaCompraVenta");
                    configurationEnvelopXMLInvoice.Add(typeof(RequestCambioMoneda), "facturaElectronicaMonedaExtranjera");

                }
                return configurationEnvelopXMLInvoice;
            }
        }

        private void setupKeys()
        {
            RSAKey = repositoryFacturacion.SimpleSelect<DBEntities.Parameter>(x => x.KeyName == "PATH_RSAKEY" && x.CompanyId == contextCompany.IdCompany).First()!.Value;
            PublicKey = repositoryFacturacion.SimpleSelect<DBEntities.Parameter>(x => x.KeyName == "PATH_PUBLICKEY" && x.CompanyId == contextCompany.IdCompany).First()!.Value;
        }

        public (ResponseObject<ResulInvoice> resulInvoice, DBEntities.Invoice invoice) GenerarFactura(IFacturable factura)
        {
            ResponseObject<ResulInvoice> objresult = new ResponseObject<ResulInvoice>
            {
                State = ResponseType.Success,
                Message = "FACTURA GENERADA EXITOSAMENTE",
                Data = new ResulInvoice()
            };
            DBEntities.Invoice invoice = null;
            try
            {
                ArgumentNullException.ThrowIfNull(factura);
                DBEntities.Office office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                DBEntities.Cufd cufd = repositoryFacturacion.SimpleSelect<DBEntities.Cufd>(x => x.IdOffice == contextCompany.IdOffice && x.EsRegistroActual).First();

                invoice = new DBEntities.Invoice
                {
                    CodeBatch = string.Empty,
                    DetailProcess = string.Empty,
                    FileNameCompress = string.Empty,
                    FolderContainer = string.Empty,
                    IdCufd = cufd.IdCufd,
                    IdEventoSignificativo = null,
                    IdOffice = office.IdOffice,
                    IndexFile = 0,
                    InvoiceNumber = "0",
                    InvoiceSign = "",
                    ProcessDate = DateTime.Now,
                    ProcessObject = JsonConvert.SerializeObject(factura),
                    RegistrationDate = DateTime.Now,
                    State = "GENERACION_INICIO",
                    TypeProcess = parameterManager.SYSTEM_MODO == "0" ? 1 : 2,
                    TypeObjectProcessed = factura.GetType().FullName,
                };
                Entity<DBEntities.Invoice> entityInvoice = new Entity<DBEntities.Invoice>
                {
                    EntityDB = invoice,
                    stateEntity = StateEntity.add
                };
                repositoryFacturacion.SaveObject(entityInvoice);
                repositoryFacturacion.Commit();
                invoice.InvoiceNumber = invoice.IdInvoice.ToString();
                factura.numeroFactura = invoice.IdInvoice;

                SetupAdaptersInvoice setupAdaptersInvoice = new SetupAdaptersInvoice(contextCompany);
                ///TODO Aca geramos la cebecera de la factura
                IAdapterInvoice adapterInvoice = setupAdaptersInvoice.AdaptersConfigurations[factura.GetType()];
                IInvoice invoiceGeenrated = adapterInvoice.GetInvoice(factura);
                string vXml = invoiceGeenrated.header.SerializarToXml();

                string vdetalle = string.Empty;
                #region Generacion del XML de los detalles
                ///TODO: aca debemos recorrer el detalle de los conceptos 
                invoiceGeenrated.detail.ForEach(detalleFactura =>
                {
                    string XMLDetalle = detalleFactura.SerializarToXml();
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(XMLDetalle);
                    vdetalle += $"<detalle>{xDoc.ChildNodes[1].InnerXml}</detalle>";
                });
                #endregion

                #region Fix de los XML para aceptacion de la informacion en el SIN
                vXml = vXml.Replace("<cabecera xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<cabecera>");
                vXml = vXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                vXml += vdetalle;
                vXml = $"<?xml version=\"1.0\" encoding=\"UTF-8\" standalone =\"yes\" ?><{ConfigurationEnvelopXMLInvoice[factura.GetType()]} xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{vXml}</{ConfigurationEnvelopXMLInvoice[factura.GetType()]}>";
                #endregion

                Signer sinSingManager = new Signer();
                FacturaHelper facturaHelper = new FacturaHelper();
                setupKeys();
                var resulFirma = sinSingManager.FirmarDocumentoXML(vXml, RSAKey, PublicKey);

                if (resulFirma.State != ResponseType.Success)
                {
                    objresult.State = ResponseType.Warning;
                    objresult.Message = $"Imposible continua con la generacion fallo en la firma: {resulFirma.Message}";
                    return (objresult, null);
                }
                ///TODO Seteamos la informacion para la factura                
                invoice.State = resulFirma.State != ResponseType.Success ? "GENERACION_ERROR_FIRMA" : "GENERACION_FIN";
                invoice.InvoiceSign = resulFirma.State != ResponseType.Success ? vXml : resulFirma.Data;
                invoice.InvoiceJson = JsonConvert.SerializeObject(invoiceGeenrated);
                invoice.InvoiceSha256 = facturaHelper.sha256_hash(invoice.InvoiceSign);
                invoice.CodigoSector = Convert.ToString(invoiceGeenrated.header.codigoDocumentoSector);
                invoice.Cuf = invoiceGeenrated.header.cuf;
                invoice.ProcessObject = JsonConvert.SerializeObject(factura);
                invoice.ModifyDate = DateTime.Now;

                ///TODO Generamos el archivo PDF de la factura
                string pathReports = ConfigManager.GetConfiguration().GetSection("Directories:InvoiceReports").Value!;
                if (!Directory.Exists(pathReports))
                {
                    Directory.CreateDirectory(pathReports);
                }
                var resulReport = adapterInvoice.GetReportInvoice(invoiceGeenrated, pathReports);
                invoice.PathInvoicePdf = resulReport.Item1;
                objresult.Message = resulReport.Item2;
                entityInvoice = new Entity<DBEntities.Invoice>
                {
                    EntityDB = invoice,
                    stateEntity = StateEntity.modify
                };
                repositoryFacturacion.SaveObject(entityInvoice);
                repositoryFacturacion.Commit();
                objresult.Data = new ResulInvoice
                {
                    cufd = invoice.Cuf,
                    mail = factura.mail,
                    nroFactura = invoice.IdInvoice,
                    reportePDF = resulReport.dataB64
                };
                ///No enviamos mail si el mail esta vacio o la factura esta fuera de linea y el evento no es 2 
                if (!string.IsNullOrEmpty(factura.mail) && !(invoice.TypeProcess == 2 && invoice.IdEventoSignificativo != 2))
                {
                    this.EnviarFacturaMail(invoice, factura.mail, factura.nombreRazonSocial, "EMITIÓ");
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, objresult);
            }

            return (objresult, invoice);
        }

        public ResponseObject<ResulInvoice> GetInvoice(IFacturable invoice)
        {
            ResponseObject<ResulInvoice> objresult = new ResponseObject<ResulInvoice>
            {
                State = ResponseType.Success,
                Message = "FACTURA GENERADA EXITOSAMENTE"
            };
            try
            {
                ///TODO: realizamos las validacion para los tipos de NIT regulados
                if ((invoice.numeroDocumento == "99001" || invoice.numeroDocumento == "99002" || invoice.numeroDocumento == "99003")
                   && invoice.codigoTipoDocumentoIdentidad != 5)
                {
                    objresult.State = ResponseType.Warning;
                    objresult.Message = "Los nros de documentos 99001, 99002, 99003 deben ser facturados con tipo de documento NIT";
                    return objresult;
                }

                if (invoice.numeroDocumento == "99002" && !invoice.nombreRazonSocial.ToLower().Equals("control tributario"))
                {
                    objresult.State = ResponseType.Warning;
                    objresult.Message = "El NIT proporcionado 99002 es solo para el control tributario";
                    return objresult;
                }

                if (invoice.numeroDocumento == "99003" && !invoice.nombreRazonSocial.ToLower().Equals("ventas menores del dia"))
                {
                    objresult.State = ResponseType.Warning;
                    objresult.Message = "El NIT proporcionado 99003 es solo para ventas menores del dia";
                    return objresult;
                }

                if ((invoice.numeroDocumento == "99001" || invoice.numeroDocumento == "99002" || invoice.numeroDocumento == "99003"))
                {
                    invoice.codigoExcepcion = 1;
                }

                int modoGeneracion = Convert.ToInt32(parameterManager.SYSTEM_MODO);
                if (modoGeneracion == 0
                    && !((invoice.numeroDocumento == "99001" || invoice.numeroDocumento == "99002" || invoice.numeroDocumento == "99003"))
                    && invoice.codigoTipoDocumentoIdentidad == 5
                    && invoice.codigoExcepcion == 0)
                {
                    var resulVerifNIT = VerifyNIT(Convert.ToInt64(invoice.numeroDocumento));
                    if (resulVerifNIT.State != ResponseType.Success)
                    {
                        objresult.State = resulVerifNIT.State;
                        objresult.Code = "NIT_INVALIDO";
                        objresult.Message = resulVerifNIT.Message;
                        return objresult;
                    }

                }
                Response responseValidacion = new Response();
                /// Facturas fuera de linea
                if (modoGeneracion == 1)
                {
                    responseValidacion = validacionFacturaOffline(-1);
                    if (responseValidacion.State != ResponseType.Success)
                    {
                        objresult.State = responseValidacion.State;
                        objresult.Message = responseValidacion.Message;
                        return objresult;
                    }
                }
                SetupAdaptersInvoice setupAdaptersInvoice = new SetupAdaptersInvoice(contextCompany);
                IAdapterInvoice adapterInvoice = setupAdaptersInvoice.AdaptersConfigurations[invoice.GetType()];
                var resulGenerarFactura = GenerarFactura(invoice);
                objresult = resulGenerarFactura.resulInvoice;
                if (modoGeneracion == 0 && objresult.State == ResponseType.Success)
                {
                    var resulSend = adapterInvoice.SendFactura(resulGenerarFactura.invoice);
                    if (resulSend.State != ResponseType.Success)
                    {
                        objresult.State = resulSend.State;
                        objresult.Message = resulSend.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                ProcessError(ex, objresult);
            }

            return objresult;
        }

        public Response AnularFactura(int IdInvoice, int idMotivo = 1)
        {
            Response objresult = new Response
            {
                State = ResponseType.Success,
                Message = "FACTURA ANULADA EXITOSAMENTE"
            };
            try
            {
                if (repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == IdInvoice).Count == 0)
                {
                    objresult.Message = $"No existe la factura nro: {IdInvoice}";
                    objresult.State = ResponseType.Warning;
                    return objresult;
                }
                DBEntities.Invoice invoiceBD = repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == IdInvoice).First();
                Type type = Type.GetType(invoiceBD.TypeObjectProcessed)!;
                IFacturable invoice = Activator.CreateInstance(type) as IFacturable;
                SetupAdaptersInvoice setupAdaptersInvoice = new SetupAdaptersInvoice(contextCompany);
                IAdapterInvoice adapterInvoice = setupAdaptersInvoice.AdaptersConfigurations[invoice.GetType()];
                objresult = adapterInvoice.AnularFactura(invoiceBD.IdInvoice, idMotivo);
                //this.EnviarFacturaMail(invoiceBD.IdInvoice, "", "Cliente", "ANULÓ");

            }
            catch (Exception ex)
            {
                ProcessError(ex, objresult);
            }

            return objresult;
        }

        public Response RevertirFactura(int IdInvoice)
        {
            Response objresult = new Response
            {
                State = ResponseType.Success,
                Message = "FACTURA REVERTIDA EXITOSAMENTE"
            };
            try
            {
                if (repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == IdInvoice).Count == 0)
                {
                    objresult.Message = $"No existe la factura nro: {IdInvoice}";
                    objresult.State = ResponseType.Warning;
                    return objresult;
                }
                DBEntities.Invoice invoiceBD = repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == IdInvoice).First();
                Type type = Type.GetType(invoiceBD.TypeObjectProcessed)!;
                IFacturable invoice = Activator.CreateInstance(type) as IFacturable;
                SetupAdaptersInvoice setupAdaptersInvoice = new SetupAdaptersInvoice(contextCompany);
                IAdapterInvoice adapterInvoice = setupAdaptersInvoice.AdaptersConfigurations[invoice.GetType()];
                objresult = adapterInvoice.RevertirFactura(invoiceBD.IdInvoice);
            }
            catch (Exception ex)
            {
                ProcessError(ex, objresult);
            }

            return objresult;
        }


        public async Task<Response> EnviarFacturaMail(DBEntities.Invoice invoice, string mail, string razonSocial, string operacion)
        {
            Response response = new Response { State = ResponseType.Success, Message = "mail enviado correctamente" };
            try
            {
                //MailHelper.smtPassword = "mikyches*123";
                //MailHelper.smtUser = "contactos@gamatek.net";
                //MailHelper.smtServer = "smtpout.secureserver.net";
                //MailHelper.smtPort = "587";


                MailHelper.smtPassword = "mikyches*123";
                MailHelper.smtUser = "jaimeavilaherbas333@gmail.com";
                MailHelper.smtServer = "smtp.gmail.com";
                MailHelper.smtPort = "587";


                ///TODO Generamos el archivo xml
                string directoryXML = ConfigManager.GetConfiguration().GetSection("Directories:InvoicesXMLDirectory").Value!;
                if (!Directory.Exists(directoryXML))
                {
                    Directory.CreateDirectory(directoryXML);
                }
                string _pathXML = Path.Combine(directoryXML, $@"factura{invoice.IdInvoice}.xml");
                File.WriteAllText(_pathXML, invoice.InvoiceSign ?? "");

                ///TODO convertimos de html a pdf
                System.Net.Mail.MailAddress mailAddressFrom = new System.Net.Mail.MailAddress(MailHelper.smtUser);
                string mailTemplatePath = ConfigManager.GetConfiguration().GetSection("Directories:MailTemplate").Value!;

                List<System.Net.Mail.MailAddress> mailAddresses = new List<System.Net.Mail.MailAddress>();
                Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
                files.Add("factura.pdf", File.ReadAllBytes(invoice.PathInvoicePdf ?? ""));
                files.Add("factura.xml", File.ReadAllBytes(_pathXML));
                mailAddresses.Add(new System.Net.Mail.MailAddress(mail));
                string contenidoMail = File.ReadAllText(mailTemplatePath);
                contenidoMail = contenidoMail.Replace("{nroFactura}", invoice.IdInvoice.ToString());
                contenidoMail = contenidoMail.Replace("{#CUFD}", invoice.Cuf);
                contenidoMail = contenidoMail.Replace("{#OPERACION}", operacion);
                contenidoMail = contenidoMail.Replace("{#nombreCliente}", razonSocial);
                MailHelper.SendMail(mailAddressFrom, mailAddresses, "Factura GAMATEK", contenidoMail, false, files);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public async Task<Response> EnviarFacturaMail(int idInvoice, string mail, string razonSocial, string operacion)
        {
            Response response = new Response { State = ResponseType.Success, Message = "mail enviado correctamente" };
            try
            {
                //MailHelper.smtPassword = "mikyches*123";
                //MailHelper.smtUser = "contactos@gamatek.net";
                //MailHelper.smtServer = "smtpout.secureserver.net";
                //MailHelper.smtPort = "25";

                MailHelper.smtPassword = "mikyches*123";
                MailHelper.smtUser = "jaimeavilaherbas333@gmail.com";
                MailHelper.smtServer = "smtp.gmail.com";
                MailHelper.smtPort = "587";


                DBEntities.Invoice invoice = repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == idInvoice).First();

                ///TODO Generamos el archivo xml
                string directoryXML = ConfigManager.GetConfiguration().GetSection("Directories:InvoicesXMLDirectory").Value!;
                if (!Directory.Exists(directoryXML))
                {
                    Directory.CreateDirectory(directoryXML);
                }
                string _pathXML = Path.Combine(directoryXML, $@"factura{invoice.IdInvoice}.xml");
                File.WriteAllText(_pathXML, invoice.InvoiceSign ?? "");

                ///TODO convertimos de html a pdf
                System.Net.Mail.MailAddress mailAddressFrom = new System.Net.Mail.MailAddress(MailHelper.smtUser);
                string mailTemplatePath = ConfigManager.GetConfiguration().GetSection("Directories:MailTemplate").Value!;

                List<System.Net.Mail.MailAddress> mailAddresses = new List<System.Net.Mail.MailAddress>();
                Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
                files.Add("factura.pdf", File.ReadAllBytes(invoice.PathInvoicePdf ?? ""));
                files.Add("factura.xml", File.ReadAllBytes(_pathXML));
                mailAddresses.Add(new System.Net.Mail.MailAddress(mail));
                string contenidoMail = File.ReadAllText(mailTemplatePath);
                contenidoMail = contenidoMail.Replace("{nroFactura}", invoice.IdInvoice.ToString());
                contenidoMail = contenidoMail.Replace("{#CUFD}", invoice.Cuf);
                contenidoMail = contenidoMail.Replace("{#OPERACION}", operacion);
                contenidoMail = contenidoMail.Replace("{#nombreCliente}", razonSocial);
                MailHelper.SendMail(mailAddressFrom, mailAddresses, "Factura GAMATEK", contenidoMail, false, files);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public async Task<Response> ObtnerPdfB64PorFactura(int idInvoice)
        {
            Response response = new Response { State = ResponseType.Success, Message = "archivo pdf obtenido correctamente" };
            try
            {
                DBEntities.Invoice invoice = repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.IdInvoice == idInvoice).First();
                response.Code = Convert.ToBase64String(File.ReadAllBytes(invoice.PathInvoicePdf));
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        private Response validacionFacturaOffline(int tipoEvento)
        {
            Response response = new Response { State = ResponseType.Success, Message = "validacion exitosa", Code = "00" };
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            if (tipoEvento == -1)
            {
                tipoEvento = Convert.ToInt32(parameterManager.EVENTO_SIGNIFICATIVO);
            }

            if (tipoEvento == 2)
            {
                response.Code = "01"; //enviar por mail factura
                return response;
            }
            else if (tipoEvento == 3)
            {
                response.Code = "00"; //no enviar por mail factura
                return response;
            }
            else if (tipoEvento == 4)
            {
                response.Code = "00"; //no enviar por mail factura
                return response;
            }
            else if (tipoEvento == 5)
            {
                //response.Message = "No se puede emitir factura, evento significativo 5:  Virus informático o falla de software. No valido para SERELCO";
                //response.State = ResponseType.Warning;
                //return response;
                response.Code = "00"; //no enviar por mail factura
                return response;
            }
            else if (tipoEvento == 6)
            {
                //response.Message = "No se puede emitir factura, evento significativo 6: Cambio de infraestructura de sistema o falla de hardware. No valido para SERELCO";
                //response.State = ResponseType.Warning;
                //return response;
                response.Code = "00"; //no enviar por mail factura
                return response;
            }
            else if (tipoEvento == 7)
            {
                //response.Message = "No se puede emitir factura, evento significativo 7: Corte de suministro de energía eléctrica. No valido para SERELCO";
                //response.State = ResponseType.Warning;
                //return response;
                response.Code = "00"; //no enviar por mail factura
                return response;
            }
            return response;
        }

        public Response VerifyNIT(long nit)
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "NIT Valido" };
            SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            using SIN.CodesServices.ServicioFacturacionCodigosClient client = sinEndPointConfiguration.ServicioFacturacionCodigosClient();
            using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
            {
                DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();

                SIN.CodesServices.solicitudVerificarNit _solicitudVerificarNit = new SIN.CodesServices.solicitudVerificarNit
                {
                    codigoAmbiente = parameterManager.MOD_AMBIENTE,
                    codigoModalidad = parameterManager.MODALIDAD_ELECTRONICA,
                    codigoSistema = company.SincodeSystem,
                    cuis = lastCUI.ValorCuis,
                    nit = Convert.ToInt64(company.Nit),
                    codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                    nitParaVerificacion = nit
                };

                var resulService = client.verificarNitAsync(new verificarNit
                {
                    SolicitudVerificarNit = _solicitudVerificarNit
                }).Result;

                if (!resulService.RespuestaVerificarNit.transaccion)
                {
                    responseObject.State = ResponseType.Warning;
                    responseObject.Message = "NIT no valido: ";
                    resulService.RespuestaVerificarNit?.mensajesList?.ToList().ForEach(x =>
                    {
                        responseObject.Message += x.descripcion;
                    });
                }
            }
            return responseObject;
        }
    }
}
