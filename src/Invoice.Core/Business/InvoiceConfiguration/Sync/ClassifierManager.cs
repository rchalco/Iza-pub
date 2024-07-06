using System.ServiceModel.Channels;
using System.ServiceModel;
using Newtonsoft.Json;
using CoreAccesLayer.Wraper;
using Invoice.Core.Base;
using Invoice.Core.DBEntities;
using PlumbingProps.Wrapper;
using Invoice.Core.Business.Office;
using Invoice.Core.UtileServices;
using SIN.CodesServices;
using Invoice.Core.Business.InvoiceConfiguration.Parameters;
using SIN.SyncServices;

namespace Invoice.Core.Business.InvoiceConfiguration.Sync
{
    public class ClassifierManager(ContextCompany contextCompany) : BaseManager
    {

        public Response SincronizarActividades()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto: SincronizarActividades" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager sinParametrizacioManager = new ParameterManager(contextCompany);
                    DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();

                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = sinParametrizacioManager.MOD_AMBIENTE,
                        codigoPuntoVenta = Convert.ToInt32(lastOffice.SincodigoTipoPuntoVenta),
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = sinParametrizacioManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = sinParametrizacioManager.EMISOR_NIT
                    };
                    var resulService = client.sincronizarActividadesAsync(new SIN.SyncServices.sincronizarActividades
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaActividades.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "SincronizarActividades");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "SincronizarActividades",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }


        /// <summary>
        /// IVO
        /// </summary>
        /// <returns></returns>
        public Response SincronizarFechaHoraActual()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto: SincronizarFechaHoraActual" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();

                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };
                    var resulService = client.sincronizarFechaHoraAsync(new SIN.SyncServices.sincronizarFechaHora
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaFechaHora.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "SincronizarFechaHoraActual");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "SincronizarFechaHoraActual",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public Response SincronizarListaActividadesDocumentoSector()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto: SincronizarListaActividadesDocumentoSector" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();

                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };
                    var resulService = client.sincronizarListaActividadesDocumentoSectorAsync(new SIN.SyncServices.sincronizarListaActividadesDocumentoSector
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaActividadesDocumentoSector.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "SincronizarListaActividadesDocumentoSector");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "SincronizarListaActividadesDocumentoSector",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarListaLeyendasFactura
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarListaLeyendasFactura()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto: sincronizarListaLeyendasFactura" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };
                    var resulService = client.sincronizarListaLeyendasFacturaAsync(new SIN.SyncServices.sincronizarListaLeyendasFactura
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricasLeyendas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarListaLeyendasFactura");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarListaLeyendasFactura",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();


                        resulService.RespuestaListaParametricasLeyendas.listaLeyendas?.ToList().ForEach(leyenda =>
                        {
                            DBEntities.CaptionsInvoice listaLeyenda = new DBEntities.CaptionsInvoice
                            {
                                CodigoActividad = leyenda.codigoActividad,
                                DescripcionLeyenda = leyenda.descripcionLeyenda,
                                FechaRegistro = DateTime.Now,
                                FechaVigenciaHasta = null
                            };
                            Entity<DBEntities.CaptionsInvoice> entity = new Entity<DBEntities.CaptionsInvoice>
                            {
                                EntityDB = listaLeyenda,
                                stateEntity = StateEntity.add
                            };
                            repositoryFacturacion.SaveObject(entity);
                            repositoryFacturacion.Commit();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarListaMensajesServicios
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarListaMensajesServicios()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarListaMensajesServicios" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarListaMensajesServiciosAsync(new SIN.SyncServices.sincronizarListaMensajesServicios
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarListaMensajesServicios");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarListaMensajesServicios",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarListaProductosServicios
        /// </summary>        
        /// <returns></returns>
        public Response sincronizarListaProductosServicios()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarListaProductosServicios" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarListaProductosServiciosAsync(new SIN.SyncServices.sincronizarListaProductosServicios
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaProductos.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarListaProductosServicios");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarListaProductosServicios",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaEventosSignificativos
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaEventosSignificativos()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaEventosSignificativos" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaEventosSignificativosAsync(new SIN.SyncServices.sincronizarParametricaEventosSignificativos
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaEventosSignificativos");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaEventosSignificativos",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaMotivoAnulacion
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaMotivoAnulacion()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaMotivoAnulacion" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaMotivoAnulacionAsync(new SIN.SyncServices.sincronizarParametricaMotivoAnulacion
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaMotivoAnulacion");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaMotivoAnulacion",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaPaisOrigen
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaPaisOrigen()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaPaisOrigen" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaPaisOrigenAsync(new SIN.SyncServices.sincronizarParametricaPaisOrigen
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaPaisOrigen");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaPaisOrigen",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaTipoDocumentoIdentidad
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaTipoDocumentoIdentidad()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaTipoDocumentoIdentidad" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTipoDocumentoIdentidadAsync(new SIN.SyncServices.sincronizarParametricaTipoDocumentoIdentidad
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaTipoDocumentoIdentidad");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaTipoDocumentoIdentidad",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// SincronizarTipoDocumentoSector
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response SincronizarTipoDocumentoSector()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto SincronizarTipoDocumentoSector" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTipoDocumentoSectorAsync(new SIN.SyncServices.sincronizarParametricaTipoDocumentoSector
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "SincronizarTipoDocumentoSector");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "SincronizarTipoDocumentoSector",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaTipoEmision
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaTipoEmision()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaTipoEmision" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTipoEmisionAsync(new SIN.SyncServices.sincronizarParametricaTipoEmision
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaTipoEmision");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaTipoEmision",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaTipoHabitacion
        /// </summary>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaTipoHabitacion()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaTipoHabitacion" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTipoHabitacionAsync(new SIN.SyncServices.sincronizarParametricaTipoHabitacion
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaTipoHabitacion");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaTipoHabitacion",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaTipoMetodoPago
        /// </summary>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaTipoMetodoPago()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaTipoMetodoPago" };
            try
            {

                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTipoMetodoPagoAsync(new SIN.SyncServices.sincronizarParametricaTipoMetodoPago
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaTipoMetodoPago");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaTipoMetodoPago",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaTipoMoneda
        /// </summary>
        public Response sincronizarParametricaTipoMoneda()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaTipoMoneda" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTipoMonedaAsync(new SIN.SyncServices.sincronizarParametricaTipoMoneda
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaTipoMoneda");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaTipoMoneda",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaTipoPuntoVenta
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        /// 
        public Response sincronizarParametricaTipoPuntoVenta()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaTipoPuntoVenta" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTipoPuntoVentaAsync(new SIN.SyncServices.sincronizarParametricaTipoPuntoVenta
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaTipoPuntoVenta");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaTipoPuntoVenta",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaTipoFactura
        /// </summary>
        /// <returns></returns>
        public Response sincronizarParametricaTipoFactura()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaTipoFactura" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaTiposFacturaAsync(new SIN.SyncServices.sincronizarParametricaTiposFactura
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaTipoFactura");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaTipoFactura",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        /// <summary>
        /// sincronizarParametricaUnidadMedida
        /// </summary>
        public Response sincronizarParametricaUnidadMedida()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "Registro correcto sincronizarParametricaUnidadMedida" };
            try
            {

                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    ParameterManager parameterManager = new ParameterManager(contextCompany);
                    Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    SIN.SyncServices.solicitudSincronizacion solicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = parameterManager.EMISOR_NIT
                    };

                    var resulService = client.sincronizarParametricaUnidadMedidaAsync(new SIN.SyncServices.sincronizarParametricaUnidadMedida
                    {
                        SolicitudSincronizacion = solicitudSincronizacion
                    }).Result;

                    if (resulService.RespuestaListaParametricas.transaccion)
                    {
                        repositoryFacturacion.CallProcedure<object>("DarBajaClasificadores", "sincronizarParametricaUnidadMedida");
                        DBEntities.LogClassifierSin clasificadorSin = new DBEntities.LogClassifierSin
                        {
                            FechaRegistro = DateTime.Now,
                            Nemotico = "sincronizarParametricaUnidadMedida",
                            Request = JsonConvert.SerializeObject(solicitudSincronizacion),
                            Response = JsonConvert.SerializeObject(resulService)
                        };
                        Entity<DBEntities.LogClassifierSin> entity = new Entity<DBEntities.LogClassifierSin>
                        {
                            EntityDB = clasificadorSin,
                            stateEntity = StateEntity.add
                        };
                        repositoryFacturacion.SaveObject(entity);
                        repositoryFacturacion.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }
    }
}
