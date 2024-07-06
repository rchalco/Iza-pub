using Invoice.Core.Artefacts.Invoice;
using Invoice.Core.Base;
using Invoice.Core.Business.Office;
using Invoice.Core.DBEntities;
using Newtonsoft.Json;
using PlumbingProps.CrossUtil;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Invoice.Core.Business.InvoiceConfiguration.Parameters;
using Invoice.Core.UtileServices;
using PlumbingProps.Config;
using Invoice.Core.Domain.Contracts;

namespace Invoice.Core.Engines
{
    public class EngineBatchInvoiceSender(ContextCompany contextCompany) : BaseManager
    {
        private const int FACTURA_ESTADO_SINCONEXION = 754;
        private const int FACTURA_ESTADO_PROCESADO = 751;
        private const int FACTURA_ESTADO_OBSERVADA = 755;
        private const int SIN_FACTURA_ESTADO_OBSERVADA = 904;
        private const int SIN_FACTURA_ESTADO_RECHAZADA = 902;

        private class FileInformationSIN
        {
            public int Index { get; set; }
            public string NameFile { get; set; }
            public string FileTARContainer { get; set; }
        }

        /// <summary>
        /// Registro de eventos, por defecto 1 corte de internet prametrizado en la tabla parameters
        /// </summary>
        /// <param name="idOficina"></param>
        /// <param name="tipoEvento"></param>
        /// <returns></returns>
        public ResponseObject<EventoSignificativo> RegistrarEventoSignificativo(int tipoEvento = -1)
        {
            var response = new ResponseObject<EventoSignificativo> { Message = "Se registro de forma correcta el evento significativo", State = ResponseType.Success };
            try
            {
                ///TODO verificamos que el evento significativo sea parametro o se debe extraer de la BD
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                if (tipoEvento == -1)
                {
                    tipoEvento = Convert.ToInt32(parameterManager.EVENTO_SIGNIFICATIVO);
                }

                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                using (SIN.OperationServices.ServicioFacturacionOperacionesClient client = sinEndPointConfiguration.ServicioFacturacionOperacionesClient())
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {

                    DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();

                    var invoices = repositoryFacturacion.GetDataByProcedure<DBEntities.Invoice>("GetFirstInvoiceOffline", contextCompany.IdOffice);
                    if (invoices.Count == 0)
                    {
                        response.State = ResponseType.Warning;
                        response.Message = $"Imposible generar el evento significativo ya que no se tiene ninga factura offline en estado GENERACION_FIN";
                        return response;
                    }

                    ///TODDO generamos un nuevo CUFD para el dia de hoy                    
                    var resulCUFD = parameterManager.CrearCUFD();
                    if (resulCUFD.State != ResponseType.Success)
                    {
                        response.State = resulCUFD.State;
                        response.Message = $"Imposible generar el evento significativo por error en la generacion del nuevo CUFD: {resulCUFD.Message}";
                        return response;
                    }
                    //ojoooooooooooooooooooo
                    DBEntities.Cufd currentCUFD = resulCUFD.Data;
                    DBEntities.Cufd lastCUFD = repositoryFacturacion.SimpleSelect<DBEntities.Cufd>(x => x.IdCufd == invoices.First().IdCufd).First();

                    ///TODO generamos el evento significativo

                    DBEntities.EventoSignificativo eventoSignificativo = new EventoSignificativo();

                    SIN.OperationServices.solicitudEventoSignificativo solicitudEventoSignificativo = new SIN.OperationServices.solicitudEventoSignificativo
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoMotivoEvento = tipoEvento,//sinParametrizacioManager.CORTE_INTERNET,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cufd = currentCUFD.ValorCufd,// el oficial es current
                        cufdEvento = lastCUFD.ValorCufd,
                        cuis = lastCUI.ValorCuis,
                        descripcion = $"EVENTO CORTE SERVICIO DE INTERNET O CAIDA DEL SIN  {DateTime.Now.ToString()}",
                        fechaHoraInicioEvento = invoices.First().RegistrationDate.AddMinutes(-5),
                        fechaHoraFinEvento = currentCUFD.FechaRegistro,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.registroEventoSignificativoAsync(new SIN.OperationServices.registroEventoSignificativo
                    {
                        SolicitudEventoSignificativo = solicitudEventoSignificativo
                    }).Result;

                    eventoSignificativo = new EventoSignificativo
                    {
                        CodEvento = resulService.RespuestaListaEventos.listaCodigos == null ? 0 : resulService.RespuestaListaEventos.listaCodigos.Last().codigoEvento,
                        CodEventoSignificativo = Convert.ToString(resulService.RespuestaListaEventos.codigoRecepcionEventoSignificativo),
                        CodigoControlCufd = currentCUFD.CodigoControl,
                        CodigoControlCufdevento = lastCUFD.CodigoControl,
                        CodPuntoVenta = lastOffice.SinidPuntoVenta,
                        CodSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        CufdEvento = lastCUFD.ValorCufd,
                        Cufd = currentCUFD.ValorCufd,
                        DescripcionEvento = solicitudEventoSignificativo.descripcion,
                        FechaHoraIni = solicitudEventoSignificativo.fechaHoraInicioEvento,
                        FechaHoraFin = solicitudEventoSignificativo.fechaHoraFinEvento,
                        Idcufd = Convert.ToInt32(currentCUFD.IdCufd),
                        Idcufdevento = Convert.ToInt32(lastCUFD.IdCufd),
                        IdOffice = contextCompany.IdOffice,
                        Procesado = false,
                        Request = JsonConvert.SerializeObject(solicitudEventoSignificativo),
                        Response = JsonConvert.SerializeObject(resulService)
                    };

                    if (!resulService.RespuestaListaEventos.transaccion)///TODO en caso de error al generar el evento verificamos que tengamos uno ya creado
                    {
                        ///TODO verificamos si ya tenemos un evento significativo asociado al cufd de la ultima factura emitida fuera de linea
                        SIN.OperationServices.solicitudConsultaEvento solicitudConsultaEvento = new SIN.OperationServices.solicitudConsultaEvento
                        {
                            codigoAmbiente = parameterManager.MOD_AMBIENTE,
                            codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                            codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                            codigoSistema = parameterManager.COD_SISTEMA,
                            codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                            cufd = currentCUFD.ValorCufd,
                            cuis = lastCUI.ValorCuis,
                            nit = parameterManager.EMISOR_NIT,
                            fechaEvento = invoices.First().RegistrationDate
                        };


                        ///Consultamos si tenemos ya un evento significativo 


                        var resulConsultaEventoSignificativo = client.consultaEventoSignificativoAsync(new SIN.OperationServices.consultaEventoSignificativo
                        {
                            SolicitudConsultaEvento = solicitudConsultaEvento
                        }).Result;


                        if (resulConsultaEventoSignificativo.RespuestaListaEventos.transaccion)//Existe un evento significativo
                        {
                            eventoSignificativo = new EventoSignificativo
                            {
                                CodEvento = resulConsultaEventoSignificativo.RespuestaListaEventos.listaCodigos.Last().codigoEvento,
                                CodEventoSignificativo = Convert.ToString(resulConsultaEventoSignificativo.RespuestaListaEventos.listaCodigos.Last().codigoRecepcionEventoSignificativo),
                                CodigoControlCufd = currentCUFD.CodigoControl,
                                CodigoControlCufdevento = lastCUFD.CodigoControl,
                                CodPuntoVenta = lastOffice.SinidPuntoVenta,
                                CodSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                                CufdEvento = lastCUFD.ValorCufd,
                                Cufd = currentCUFD.ValorCufd,
                                DescripcionEvento = resulConsultaEventoSignificativo.RespuestaListaEventos.listaCodigos.Last().descripcion,
                                FechaHoraIni = Convert.ToDateTime(resulConsultaEventoSignificativo.RespuestaListaEventos.listaCodigos.Last().fechaInicio),
                                FechaHoraFin = Convert.ToDateTime(resulConsultaEventoSignificativo.RespuestaListaEventos.listaCodigos.Last().fechaFin),
                                Idcufd = Convert.ToInt32(currentCUFD.IdCufd),
                                Idcufdevento = Convert.ToInt32(lastCUFD.IdCufd),
                                IdOffice = contextCompany.IdOffice,
                                Procesado = false,
                                Request = JsonConvert.SerializeObject(solicitudConsultaEvento),
                                Response = JsonConvert.SerializeObject(resulConsultaEventoSignificativo)
                            };
                        }
                        if (!resulConsultaEventoSignificativo.RespuestaListaEventos.transaccion)
                        {
                            response.State = ResponseType.Warning;
                            response.Message = "Error al generar el evento significativo, por favor revisar el detalle en la BD";
                        }
                    }

                    repositoryFacturacion.SaveObject<DBEntities.EventoSignificativo>(new CoreAccesLayer.Wraper.Entity<DBEntities.EventoSignificativo>
                    {
                        EntityDB = eventoSignificativo,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.add
                    });
                    repositoryFacturacion.Commit();
                    response.Data = eventoSignificativo;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// enviamos lote con corte de internet por defecto tipoEvento = 1 y con maximo de factura de int maxInvoice 
        /// </summary>
        /// <param name="tipoEvento"></param>
        /// <returns></returns>
        public Response RegisterBatchInvoice(int tipoEvento = -1, int maxInvoice = 500)
        {
            var response = new Response { Message = "Se termino de forma correcta el envio de Lotes, revise el LOG", State = ResponseType.Success };
            try
            {
                string _PATH_BATCHFILES = ConfigManager.GetConfiguration().GetSection("Directories:BathInvoices").Value!;
                ///TODO: obtenemos las facturas fuera de linea
                var resultInvoicing = repositoryFacturacion.SimpleSelect<DBEntities.Invoice>(x => x.TypeProcess == 2 && x.State == "GENERACION_FIN" && x.IdOffice == contextCompany.IdOffice).Take(maxInvoice).ToList();

                if (resultInvoicing.Count() == 0)
                {
                    response = new Response
                    {
                        Message = $"No se tiene facturas pendientes de envio para la oficina ${contextCompany.IdOffice}",
                        State = ResponseType.Warning
                    };
                    return response;
                }

                #region Generamos paquetes para cada factura                
                string codigoBatch = DateTime.Now.ToString("yyyyMMdd") + "GUID" + CustomGuid.GetGuid();

                ///Obtenemos el adaptador que corresponde

                ///TODO generamos los eventos significativos por cada oficina
                Dictionary<long, EventoSignificativo> eventosSignificativos = new Dictionary<long, EventoSignificativo>();
                resultInvoicing.ForEach(x =>
                {
                    if (!eventosSignificativos.Keys.Contains(x.IdOffice ?? 0))
                    {
                        var resulEventoSignificativo = RegistrarEventoSignificativo(tipoEvento);
                        if (resulEventoSignificativo.State == ResponseType.Success)
                        {
                            eventosSignificativos.Add(Convert.ToInt32(x.IdOffice), resulEventoSignificativo.Data);
                        }
                        else
                        {
                            ProcessError(new Exception(resulEventoSignificativo.Message));
                        }
                    }
                });

                if (eventosSignificativos.Count == 0)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se tienen eventos significativos validos para enviar el lote";
                    return response;
                }

                #endregion
                FacturaHelper facturaHelper = new FacturaHelper();
                ///TODO creamos los archivos XML por cada factura
                resultInvoicing.ForEach(facturaGenerada =>
                {
                    string mainPathInvoice = Path.Combine(_PATH_BATCHFILES, codigoBatch);
                    if (!Directory.Exists(mainPathInvoice))
                    {
                        Directory.CreateDirectory(mainPathInvoice);
                    }
                    string fileName = Path.Combine(mainPathInvoice, "FACTURA" + facturaGenerada.IdInvoice.ToString() + ".xml");
                    if (!string.IsNullOrEmpty(facturaGenerada.InvoiceSign))
                    {
                        facturaHelper.CrearArchivoXML(facturaGenerada.InvoiceSign, fileName);
                        facturaGenerada.State = "LOTE_ARCHIVO_GENERADO";
                        facturaGenerada.FileNameCompress = fileName;
                        facturaGenerada.FolderContainer = mainPathInvoice;
                        facturaGenerada.DetailProcess += "|LOTE_ARCHIVO_GENERADO";
                        DirectoryInfo directoryInfo = new DirectoryInfo(mainPathInvoice);
                        facturaGenerada.IndexFile = directoryInfo.EnumerateFiles().Count() - 1;
                        repositoryFacturacion.SaveObject<DBEntities.Invoice>(new CoreAccesLayer.Wraper.Entity<DBEntities.Invoice>
                        {
                            EntityDB = facturaGenerada,
                            stateEntity = CoreAccesLayer.Wraper.StateEntity.modify
                        });
                        repositoryFacturacion.Commit();
                    }

                });

                ///TODO creamos el archivo TAR
                Queue<(long, string)> fileProcesssQueue = new Queue<(long, string)>();

                resultInvoicing.ForEach(f =>
                {
                    string fileNameTGZ = Path.Combine(_PATH_BATCHFILES, f.FolderContainer + ".tar.gz");
                    string pathFacturas = Path.Combine(_PATH_BATCHFILES, f.FolderContainer ?? "");

                    if (!fileProcesssQueue.Contains((Convert.ToInt32(f.CodigoSector), fileNameTGZ)))
                    {
                        facturaHelper.CrearTarGZ(fileNameTGZ, pathFacturas);
                        fileProcesssQueue.Enqueue((Convert.ToInt32(f.CodigoSector), fileNameTGZ));
                    }
                });
                SetupAdaptersInvoice setupAdaptersInvoice = new SetupAdaptersInvoice(contextCompany);


                while (fileProcesssQueue.Count > 0)
                {
                    (long, string) fileTGZ = fileProcesssQueue.Dequeue();
                    string folderTGZ = fileTGZ.Item2.Replace(".tar.gz", "").Split('\\').Last();
                    var invoiceImportPROD = resultInvoicing.First(x => x.FolderContainer == Path.Combine(_PATH_BATCHFILES, folderTGZ));
                    Type type = Type.GetType(invoiceImportPROD.TypeObjectProcessed!);
                    IAdapterInvoice adapterInvoice = setupAdaptersInvoice.AdaptersConfigurations[type];
                    List<DBEntities.Invoice> setInvoices = resultInvoicing.Where(xx => xx.FolderContainer == Path.Combine(_PATH_BATCHFILES, folderTGZ)).ToList();
                    EventoSignificativo eventoSignificativo = eventosSignificativos[Convert.ToInt64(invoiceImportPROD.IdOffice)];
                    var responseSendBatch = adapterInvoice.SendBatchInvoices(setInvoices, eventoSignificativo, fileTGZ.Item2, folderTGZ, _PATH_BATCHFILES);
                }
                response.Message = $"EJECUCION DEL PROCESO DE LOTES DE FACTURA TERMINADA CORRECTAMENTE REVISE LA BD PARA VER LOS DETALLES";
                response.State = ResponseType.Success;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
                repositoryFacturacion.Rollback();
            }
            return response;
        }
    }
}
