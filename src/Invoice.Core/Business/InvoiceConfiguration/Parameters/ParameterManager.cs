using CoreAccesLayer.Wraper;
using Invoice.Core.Base;
using Newtonsoft.Json;
using PlumbingProps.Wrapper;
using SIN.CodesServices;
using SIN.OperationServices;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Invoice.Core.Business.Office;
using Invoice.Core.UtileServices;
using NPOI.SS.Formula.Functions;
using SIN.SyncServices;
using System;

namespace Invoice.Core.Business.InvoiceConfiguration.Parameters
{
    public class ParameterManager(ContextCompany contextCompany) : BaseManager
    {

        private List<DBEntities.Parameter> _ParametersSystem = new List<DBEntities.Parameter>();
        public List<DBEntities.Parameter> ParametersSystem
        {
            get
            {
                if (_ParametersSystem == null || _ParametersSystem.Count() == 0)
                {
                    _ParametersSystem = repositoryFacturacion.SimpleSelect<DBEntities.Parameter>(x => x.CompanyId == contextCompany.IdCompany);
                }
                return _ParametersSystem ?? new List<DBEntities.Parameter>();
            }
        }

        private DBEntities.Company _Company;
        private DBEntities.Company Company
        {
            get
            {
                if (_Company == null)
                {
                    _Company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First()!;
                }
                return _Company;

            }
        }

        private DBEntities.Office _Office;
        private DBEntities.Office Office
        {
            get
            {
                if (_Office == null)
                {
                    _Office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First()!;
                }
                return _Office;

            }
        }


        public string COD_SISTEMA { get { return Company.SincodeSystem!; } }
        public int MODALIDAD_ELECTRONICA { get { return Convert.ToInt32(ParametersSystem?.Find(x => x.KeyName == "MODALIDAD_ELECTRONICA")!.Value!); } }
        public int MOD_AMBIENTE { get { return Convert.ToInt32(Company.SincodeEnviroment); } }
        public long EMISOR_NIT { get { return Convert.ToInt64(Company.Nit!); } }
        public int TIPO_PUNTOVENTA { get { return Convert.ToInt32(_Office.SincodigoTipoPuntoVenta!); } }
        public int METODO_PAGO_EFECTIVO { get { return Convert.ToInt32(ParametersSystem?.Find(x => x.KeyName == "METODO_PAGO_EFECTIVO")!.Value); } }
        public int CODIGO_MONEDA_BS { get { return Convert.ToInt32(ParametersSystem?.Find(x => x.KeyName == "CODIGO_MONEDA_BS")!.Value); } }
        public string DIRECCION { get { return Office.Direccion!; } }
        public string RAZON_SOCIAL { get { return Company.SocialReason!; } }
        public string TIPO_FACTURA_CREDFISCAL { get { return ParametersSystem?.Find(x => x.KeyName == "TIPO_FACTURA_CREDFISCAL")!.Value!; } }
        public string TIPO_FACTURA_NOCREDFISCAL { get { return ParametersSystem?.Find(x => x.KeyName == "TIPO_FACTURA_NOCREDFISCAL")!.Value!; } }
        public string TIPO_FACTURA_DOCAJUSTE { get { return ParametersSystem?.Find(x => x.KeyName == "TIPO_FACTURA_DOCAJUSTE")!.Value!; } }
        public string EMISION_LINEA { get { return ParametersSystem?.Find(x => x.KeyName == "EMISION_LINEA")!.Value!; } }
        public string EMISION_NOLINEA { get { return ParametersSystem?.Find(x => x.KeyName == "EMISION_NOLINEA")!.Value!; } }
        public string EMISION_MASIVA { get { return ParametersSystem?.Find(x => x.KeyName == "EMISION_MASIVA")!.Value!; } }
        public string LINK_QR { get { return ParametersSystem?.Find(x => x.KeyName == "LINK_QR")!.Value!; } }
        public string SYSTEM_MODO
        {
            get
            {
                return ParametersSystem?.Find(x => x.KeyName == "SYSTEM_MODO")!.Value!;
            }
        }
        public string EVENTO_SIGNIFICATIVO
        {
            get
            {
                return ParametersSystem?.Find(x => x.KeyName == "EVENTO_SIGNIFICATIVO")!.Value!;
            }
        }

        public string CAFC
        {
            get
            {
                return ParametersSystem?.Find(x => x.KeyName == "CAFC")!.Value!;
            }
        }

        public int CORTE_INTERNET { get { return 1; } }
        public string TELEFONO { get { return Office.Telefono!; } }


        public Response SincronizarCodigoCUISGlobal()
        {
            Response responseObject = new Response
            {
                State = ResponseType.Success,
                Message = "CUIS creado correctamente"
            };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                DBEntities.Office office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First()!;
                using ServicioFacturacionCodigosClient client = sinEndPointConfiguration.ServicioFacturacionCodigosClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    SIN.CodesServices.solicitudCuis _solicitudCuis = new solicitudCuis
                    {
                        codigoAmbiente = MOD_AMBIENTE,
                        codigoModalidad = MODALIDAD_ELECTRONICA,
                        codigoPuntoVenta = Convert.ToInt32(office.SinidPuntoVenta),
                        codigoPuntoVentaSpecified = Convert.ToInt32(office.SinidPuntoVenta) != 0,
                        codigoSistema = COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(office.SincodigoSucursal),
                        nit = EMISOR_NIT
                    };
                    var resulService = client.cuisAsync(new cuis
                    {
                        SolicitudCuis = _solicitudCuis
                    }).Result;

                    repositoryFacturacion.CallProcedure<Object>("DesactivarCUISGlobales", contextCompany.IdOffice);
                    DBEntities.Cui cui = new DBEntities.Cui
                    {
                        EsRegistroActual = true,
                        FechaRegistro = DateTime.Now,
                        FechaBaja = null,
                        IdOffice = contextCompany.IdOffice,
                        IdOfficeExternal = contextCompany.IdOfficeExternal,
                        Request = Newtonsoft.Json.JsonConvert.SerializeObject(_solicitudCuis),
                        Response = Newtonsoft.Json.JsonConvert.SerializeObject(resulService),
                        ValorCuis = resulService.RespuestaCuis.codigo
                    };
                    repositoryFacturacion.SaveObject<DBEntities.Cui>(new CoreAccesLayer.Wraper.Entity<DBEntities.Cui>
                    {
                        EntityDB = cui,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.add
                    });
                    repositoryFacturacion.Commit();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public ResponseObject<DBEntities.Cufd> CrearCUFD()
        {
            ResponseObject<DBEntities.Cufd> responseObject = new ResponseObject<DBEntities.Cufd> { State = ResponseType.Success, Message = "CUFD generado exitosamente" };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionCodigosClient client = sinEndPointConfiguration.ServicioFacturacionCodigosClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Office lastOffice = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();

                    solicitudCufd _solicitudCufd = new solicitudCufd
                    {
                        codigoAmbiente = MOD_AMBIENTE,
                        codigoModalidad = MODALIDAD_ELECTRONICA,
                        codigoSistema = COD_SISTEMA,
                        cuis = lastCUI.ValorCuis,
                        nit = EMISOR_NIT,
                        codigoPuntoVenta = lastOffice.SinidPuntoVenta,
                        codigoPuntoVentaSpecified = lastOffice.SinidPuntoVenta != 0,
                        codigoSucursal = Convert.ToInt32(lastOffice.SincodigoSucursal),
                    };

                    var resulService = client.cufdAsync(new cufd
                    {
                        SolicitudCufd = _solicitudCufd
                    }).Result;

                    DBEntities.Cufd _Cufd = new DBEntities.Cufd
                    {
                        IdOffice = contextCompany.IdOffice,
                        CodigoControl = resulService.RespuestaCufd.codigoControl ?? string.Empty,
                        CuisRequest = _solicitudCufd.cuis ?? string.Empty,
                        EsRegistroActual = !string.IsNullOrEmpty(resulService.RespuestaCufd.codigoControl),
                        FechaRegistro = DateTime.Now,
                        Request = JsonConvert.SerializeObject(_solicitudCufd),
                        Response = JsonConvert.SerializeObject(resulService),
                        IdCuis = lastCUI.IdCuis,
                        ValorCufd = resulService.RespuestaCufd.codigo ?? string.Empty
                    };
                    if (!string.IsNullOrEmpty(_Cufd.ValorCufd))
                    {
                        repositoryFacturacion.CallProcedure<Object>("DesactivarCUFDS", contextCompany.IdOffice);
                    }
                    else
                    {
                        responseObject.State = ResponseType.Warning;
                        responseObject.Message = "Error al genrar CUFD, por favor revisar el detalle en la BD";
                    }
                    repositoryFacturacion.SaveObject<DBEntities.Cufd>(new CoreAccesLayer.Wraper.Entity<DBEntities.Cufd>
                    {
                        EntityDB = _Cufd,
                        stateEntity = CoreAccesLayer.Wraper.StateEntity.add
                    });
                    repositoryFacturacion.Commit();
                    responseObject.Data = _Cufd;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public Response VerifyCUFDCurrentDate()
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "existe un CUFD para esta oficina" };
            try
            {
                var resul = repositoryFacturacion.SimpleSelect<DBEntities.Cufd>(x => x.IdOffice == contextCompany.IdOffice
                && x.FechaRegistro >= DateTime.Now.Date
                && x.EsRegistroActual == true);
                if (resul.Count == 0)
                {
                    responseObject.State = ResponseType.Warning;
                    responseObject.Message = "La oficina no cuenta con un CUF valido para la fecha actual";
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public ResponseQuery<DBEntities.CaptionsInvoice> ObtenerLeyendas()
        {
            ResponseQuery<DBEntities.CaptionsInvoice> responseObject = new ResponseQuery<DBEntities.CaptionsInvoice>
            {
                State = ResponseType.Success,
                Message = "Lista de leyendas obtenidas correctamente"
            };
            try
            {
                var resul = repositoryFacturacion.GetDataByProcedure<DBEntities.CaptionsInvoice>("obtener_leyendas");
                if (resul.Count == 0)
                {
                    responseObject.State = ResponseType.Warning;
                    responseObject.Message = "No se tiene leyendas disponibles";
                    return responseObject;
                }
                responseObject.ListEntities = resul;
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public Response ObtenerLeyenda3Leyendas(int codigoActividad)
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "existe un CUFD para esta oficina" };
            try
            {
                var resul = repositoryFacturacion.GetDataByProcedure<DBEntities.CaptionsInvoice>("ObtenerLeyendasByCodigoActividad", codigoActividad);
                if (resul.Count == 0)
                {
                    responseObject.State = ResponseType.Warning;
                    responseObject.Message = "No se tiene leyendas disponibles";
                    return responseObject;
                }
                for (int i = 0; i < 3; i++)
                {
                    Random random = new Random();
                    int index = random.Next(resul.Count);
                    responseObject.Code += resul[index].DescripcionLeyenda + "\n";
                }

            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public Response ObtenerUnaLeyenda(int codigoActividad)
        {
            Response responseObject = new Response { State = ResponseType.Success, Message = "existe un CUFD para esta oficina" };
            try
            {
                var resul = repositoryFacturacion.GetDataByProcedure<DBEntities.CaptionsInvoice>("ObtenerLeyendasByCodigoActividad", codigoActividad);
                if (resul.Count == 0)
                {
                    responseObject.State = ResponseType.Warning;
                    responseObject.Message = "No se tiene leyendas disponibles";
                    return responseObject;
                }
                Random random = new Random();
                int index = random.Next(resul.Count);
                responseObject.Code += resul[index].DescripcionLeyenda;

            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public Response ParametrizarProductos()
        {
            Response responseObject = new Response
            {
                State = ResponseType.Success,
                Message = "Productos parametrizados correctamente"
            };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual
                                                                                                && x.IdOffice == contextCompany.IdOffice).First();

                    var resulService = client.sincronizarListaProductosServiciosAsync(new SIN.SyncServices.sincronizarListaProductosServicios
                    {
                        SolicitudSincronizacion = new SIN.SyncServices.solicitudSincronizacion
                        {
                            codigoAmbiente = MOD_AMBIENTE,
                            codigoPuntoVenta = 0,
                            codigoPuntoVentaSpecified = false,
                            codigoSistema = COD_SISTEMA,
                            codigoSucursal = 0,
                            nit = EMISOR_NIT,
                            cuis = lastCUI.ValorCuis
                        }

                    }).Result;

                    resulService.RespuestaListaProductos?.listaCodigos?.ToList()
                        .ForEach(x =>
                        {

                            DBEntities.LogProductSin productoSin = new DBEntities.LogProductSin
                            {
                                CodeActivity = x.codigoActividad,
                                CodeProduct = Convert.ToString(x.codigoProducto),
                                Description = x.descripcionProducto,
                                DateRegister = DateTime.Now,
                                ResulJson = JsonConvert.SerializeObject(x)
                            };

                            repositoryFacturacion.SaveObject<DBEntities.LogProductSin>(new Entity<DBEntities.LogProductSin>
                            {
                                EntityDB = productoSin,
                                stateEntity = StateEntity.add
                            });
                            repositoryFacturacion.Commit();
                        });

                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public Response VerificarConexion()
        {
            Response responseObject = new Response
            {
                State = ResponseType.Success,
                Message = "Conexion correcta con Impuestos"
            };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);

                using ServicioFacturacionSincronizacionClient client = sinEndPointConfiguration.ServicioFacturacionSincronizacionClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    var resulVerficar = client.verificarComunicacionAsync(new SIN.SyncServices.verificarComunicacion { }).Result;
                    if (!resulVerficar.@return.transaccion)
                    {
                        string mensajeError = "";
                        resulVerficar.@return.mensajesList?.ToList().ForEach(yy =>
                        {
                            mensajeError += "\n" + yy.codigo + " " + yy.descripcion;
                        });
                        responseObject.State = ResponseType.Warning;
                        responseObject.Message = $"Error al comunicarse con Impuestos {mensajeError}";
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
        /// cambiar el modo de facturacion
        /// </summary>
        /// <param name="modo">0 = online, 1 = offline</param>
        /// <returns></returns>
        public Response CambiarModoFacturacion(int modo)
        {
            Response responseObject = new Response
            {
                State = ResponseType.Success,
                Message = "Cambio a modo " + (modo == 0 ? "Online" : "Offline")
            };
            try
            {
                ParametersSystem.ForEach(x =>
                {
                    if (x.KeyName == "SYSTEM_MODO")
                    {
                        x.Value = modo.ToString();
                    }
                });
                Entity<DBEntities.Parameter> entity = new Entity<DBEntities.Parameter>
                {
                    EntityDB = ParametersSystem.First(x => x.KeyName == "SYSTEM_MODO"),
                    stateEntity = StateEntity.modify
                };
                repositoryFacturacion.SaveObject<DBEntities.Parameter>(entity);
                repositoryFacturacion.Commit();
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }
    }
}
