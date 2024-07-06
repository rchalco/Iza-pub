using Newtonsoft.Json;
using SIN.OperationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Invoice.Core.Base;
using Invoice.Core.Business.Office;
using PlumbingProps.Wrapper;
using SIN.CodesServices;
using Invoice.Core.UtileServices;
using Invoice.Core.Business.InvoiceConfiguration.Office.DTO;
using PlumbingProps.CrossUtil;
using Invoice.Core.Business.InvoiceConfiguration.Parameters;
using Invoice.Core.DBEntities;

namespace Invoice.Core.Business.InvoiceConfiguration.Office
{
    public class OfficeManager(ContextCompany contextCompany) : BaseManager
    {
        public Response CrearOficina(DTOOffice _office)
        {
            Response responseObject = new Response
            {
                State = ResponseType.Success,
                Message = "Oficina creada correctamente"
            };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                using ServicioFacturacionOperacionesClient client = sinEndPointConfiguration.ServicioFacturacionOperacionesClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
                    DBEntities.Office officeFirst = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == officeFirst.IdOffice).First();

                    long _codigoSucursal = repositoryFacturacion.Getall<DBEntities.Office>().Last().IdOffice + 1;

                    solicitudRegistroPuntoVenta _solicitudRegistroPuntoVenta = new solicitudRegistroPuntoVenta
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoModalidad = parameterManager.MODALIDAD_ELECTRONICA,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = _office.IsCasaMatriz ? 0 : Convert.ToInt32(_codigoSucursal),
                        codigoTipoPuntoVenta = _office.TipoPuntoVenta,
                        cuis = lastCUI.ValorCuis,
                        descripcion = _office.Descripcion,
                        nit = Convert.ToInt64(company.Nit),
                        nombrePuntoVenta = _office.NombrePuntoVenta
                    };

                    var resulService = client.registroPuntoVentaAsync(new SIN.OperationServices.registroPuntoVenta
                    {
                        SolicitudRegistroPuntoVenta = _solicitudRegistroPuntoVenta
                    }).Result;

                    DBEntities.Office office = new DBEntities.Office
                    {
                        SincodigoSucursal = Convert.ToString(_solicitudRegistroPuntoVenta.codigoSucursal),
                        SincodigoTipoPuntoVenta = Convert.ToString(_office.TipoPuntoVenta),
                        CuisCreacion = lastCUI.ValorCuis,
                        Descripcion = _office.Descripcion,
                        IdOffice = _codigoSucursal,
                        SinidPuntoVenta = resulService.RespuestaRegistroPuntoVenta.codigoPuntoVenta,
                        Name = _office.NombrePuntoVenta,
                        Request = JsonConvert.SerializeObject(_solicitudRegistroPuntoVenta),
                        Response = JsonConvert.SerializeObject(resulService),
                        Estado = 1,
                        FechaRegistro = DateTime.Now,
                        CompanyId = contextCompany.IdCompany,
                        Direccion = _office.Direccion,
                        Telefono = _office.Telefono
                    };

                    repositoryFacturacion.SaveObject<DBEntities.Office>(new CoreAccesLayer.Wraper.Entity<DBEntities.Office>
                    {
                        EntityDB = office,
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

        public ResponseObject<consultaPuntoVentaResponse> ObtenerPuntosVentaSIN()
        {
            ResponseObject<consultaPuntoVentaResponse> responseObject = new ResponseObject<consultaPuntoVentaResponse>
            {
                State = ResponseType.Success,
                Message = "Oficinas obtenidas correctamente"
            };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                using ServicioFacturacionOperacionesClient client = sinEndPointConfiguration.ServicioFacturacionOperacionesClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
                    DBEntities.Office officeFirst = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                    DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == officeFirst.IdOffice).First();

                    solicitudConsultaPuntoVenta consultaPuntoVenta = new solicitudConsultaPuntoVenta
                    {
                        codigoAmbiente = parameterManager.MOD_AMBIENTE,
                        codigoSistema = parameterManager.COD_SISTEMA,
                        codigoSucursal = Convert.ToInt32(officeFirst.SincodigoSucursal),
                        cuis = lastCUI.ValorCuis,
                        nit = Convert.ToInt64(company.Nit)
                    };

                    var resulOficinas = client.consultaPuntoVentaAsync(new consultaPuntoVenta
                    {
                        SolicitudConsultaPuntoVenta = consultaPuntoVenta
                    }).Result;
                    responseObject.Data = resulOficinas;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public ResponseObject<DTOCompany> ObtenerCompany()
        {
            ResponseObject<DTOCompany> responseObject = new ResponseObject<DTOCompany>
            {
                State = ResponseType.Success,
                Message = "Company obtenido correctamente"
            };
            try
            {
                DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
                List<DBEntities.Office> offices = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.CompanyId == contextCompany.IdCompany);
                MapHelper mapHelper = new MapHelper();
                responseObject.Data = mapHelper.MapObject<DBEntities.Company, DTOCompany>(company);
                responseObject.Data?.Offices.ToList().ForEach(office =>
                {
                    office.NombrePuntoVenta = offices.First<DBEntities.Office>(x => x.IdOffice == office.IdOffice).Name;
                });
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public ResponseObject<SIN.OperationServices.consultaPuntoVentaResponse> ConsutarPuntosVenta()
        {
            ResponseObject<SIN.OperationServices.consultaPuntoVentaResponse> responseObject = new ResponseObject<SIN.OperationServices.consultaPuntoVentaResponse>
            {
                Code = "00",
                State = ResponseType.Success,
                Message = "Consulta realizada con exito"
            };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                DBEntities.Office office = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.IdOffice == contextCompany.IdOffice).First();
                DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();
                using ServicioFacturacionOperacionesClient client = sinEndPointConfiguration.ServicioFacturacionOperacionesClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    var resulService = client.consultaPuntoVentaAsync(new consultaPuntoVenta
                    {
                        SolicitudConsultaPuntoVenta = new solicitudConsultaPuntoVenta
                        {
                            codigoAmbiente = 2,
                            codigoSistema = parameterManager.COD_SISTEMA,
                            codigoSucursal = Convert.ToInt32(office.SincodigoSucursal),
                            cuis = lastCUI.ValorCuis,
                            nit = Convert.ToInt64(company.Nit)
                        }

                    }).Result;
                    responseObject.Data = resulService;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        public Response CrearPuntosVenta()
        {
            Response responseObject = new Response
            {
                Code = "00",
                Message = "Punto de venta creado con exito",
                State = ResponseType.Success
            };
            try
            {
                SINEndPointConfigurationFactory sinEndPointConfiguration = new SINEndPointConfigurationFactory(contextCompany);
                ParameterManager parameterManager = new ParameterManager(contextCompany);
                DBEntities.Cui lastCUI = repositoryFacturacion.SimpleSelect<DBEntities.Cui>(x => x.EsRegistroActual && x.IdOffice == contextCompany.IdOffice).First();
                DBEntities.Office officeFirst = repositoryFacturacion.SimpleSelect<DBEntities.Office>(x => x.CompanyId == contextCompany.IdCompany).First();
                DBEntities.Company company = repositoryFacturacion.SimpleSelect<DBEntities.Company>(x => x.IdCompany == contextCompany.IdCompany).First();

                using ServicioFacturacionOperacionesClient client = sinEndPointConfiguration.ServicioFacturacionOperacionesClient();
                using (var scope = sinEndPointConfiguration.GetConfigScope(client.InnerChannel))
                {
                    var resulService = client.registroPuntoVentaAsync(new registroPuntoVenta
                    {
                        SolicitudRegistroPuntoVenta = new solicitudRegistroPuntoVenta
                        {
                            codigoAmbiente = 2,
                            codigoModalidad = 2,
                            codigoSistema = parameterManager.COD_SISTEMA,
                            codigoSucursal = 0,
                            codigoTipoPuntoVenta = Convert.ToInt32(officeFirst.SincodigoTipoPuntoVenta),
                            cuis = lastCUI.ValorCuis,
                            descripcion = officeFirst.Descripcion,
                            nit = Convert.ToInt64(company.Nit),
                            nombrePuntoVenta = officeFirst.Name
                        }
                    }).Result;
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
