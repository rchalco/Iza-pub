using Invoice.Core.Business.Office;
using PlumbingProps.Config;
using SIN.CodesServices;
using SIN.OperationServices;
using SIN.SyncServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Invoice.Core.DBEntities;
using Invoice.Core.Base;

namespace Invoice.Core.UtileServices
{
    internal class SINEndPointConfigurationFactory(ContextCompany contextCompany) : BaseManager
    {
        private class SINEndPointConfiguration
        {
            public required Binding SINBinding { get; set; }
            public required EndpointAddress SINEndpointAddress { get; set; }
        }


        private static Dictionary<Type, SINEndPointConfiguration>? _ConfigurationEndpoints = null;        
        private Dictionary<Type, SINEndPointConfiguration> ConfigurationEndpoints()
        {
            if (_ConfigurationEndpoints == null)
            {
                var endpointsConfiguration = new Dictionary<string, string>();
                ConfigManager.GetConfiguration().GetSection("SINEnpointsAdress").GetChildren().ToList().ForEach(x =>
                {
                    endpointsConfiguration.Add(x.Key, x.Value ?? "");
                });

                _ConfigurationEndpoints = new Dictionary<Type, SINEndPointConfiguration>();

                _ConfigurationEndpoints.Add(typeof(ServicioFacturacionOperacionesClient), new SINEndPointConfiguration
                {
                    SINBinding = GetBindingForEndpoint(),
                    SINEndpointAddress = new EndpointAddress(endpointsConfiguration["FacturacionOperaciones"])
                });
                _ConfigurationEndpoints.Add(typeof(ServicioFacturacionCodigosClient), new SINEndPointConfiguration
                {
                    SINBinding = GetBindingForEndpoint(),
                    SINEndpointAddress = new EndpointAddress(endpointsConfiguration["FacturacionCodigos"])
                });
                _ConfigurationEndpoints.Add(typeof(ServicioFacturacionSincronizacionClient), new SINEndPointConfiguration
                {
                    SINBinding = GetBindingForEndpoint(),
                    SINEndpointAddress = new EndpointAddress(endpointsConfiguration["FacturacionSincronizacion"])
                });
                _ConfigurationEndpoints.Add(typeof(SIN.BasicExpensesServices.ServicioFacturacionClient), new SINEndPointConfiguration
                {
                    SINBinding = GetBindingForEndpoint(),
                    SINEndpointAddress = new EndpointAddress(endpointsConfiguration["ServicioFacturacionServicioBasico"])
                });
                _ConfigurationEndpoints.Add(typeof(SIN.OrdersServices.ServicioFacturacionClient), new SINEndPointConfiguration
                {
                    SINBinding = GetBindingForEndpoint(),
                    SINEndpointAddress = new EndpointAddress(endpointsConfiguration["ServicioFacturacionServicioCompraVentas"])
                });
                _ConfigurationEndpoints.Add(typeof(SIN.FinancialServices.ServicioFacturacionClient), new SINEndPointConfiguration
                {
                    SINBinding = GetBindingForEndpoint(),
                    SINEndpointAddress = new EndpointAddress(endpointsConfiguration["ServicioFacturacionServicioFinanciero"])
                });
                _ConfigurationEndpoints.Add(typeof(SIN.ExchanceMoneyServices.ServicioFacturacionClient), new SINEndPointConfiguration
                {
                    SINBinding = GetBindingForEndpoint(),
                    SINEndpointAddress = new EndpointAddress(endpointsConfiguration["ServicioFacturacionServicioCambioMoneda"])
                });
            }
            return _ConfigurationEndpoints;
        }

        private static string _apiKey = string.Empty;

        private const string NAME_HEADER_API_KEY = "Apikey";

        private string GetAPIKey()
        {
            if (_apiKey == string.Empty)
            {
                Company _company = repositoryFacturacion.SimpleSelect<Company>(x => x.IdCompany == contextCompany.IdCompany).First();
                if (string.IsNullOrEmpty(_company!.Sintoken))
                {
                    throw new Exception("No se tiene registrado el token del SIN en la BD-> tabla company");
                }
                _apiKey = "TokenApi " + _company!.Sintoken;
            }
            return _apiKey;
        }

        public ServicioFacturacionOperacionesClient ServicioFacturacionOperacionesClient()
        {
            ServicioFacturacionOperacionesClient client = new ServicioFacturacionOperacionesClient(
                 ConfigurationEndpoints()[typeof(ServicioFacturacionOperacionesClient)].SINBinding,
                 ConfigurationEndpoints()[typeof(ServicioFacturacionOperacionesClient)].SINEndpointAddress
                );
           // ConfigScope<ServicioFacturacionOperaciones>(client);
            return client;
        }

        public ServicioFacturacionCodigosClient ServicioFacturacionCodigosClient()
        {
            ServicioFacturacionCodigosClient client = new ServicioFacturacionCodigosClient(
                 ConfigurationEndpoints()[typeof(ServicioFacturacionCodigosClient)].SINBinding,
                 ConfigurationEndpoints()[typeof(ServicioFacturacionCodigosClient)].SINEndpointAddress
                );
            //ConfigScope<ServicioFacturacionCodigos>(client);
            return client;
        }

        public ServicioFacturacionSincronizacionClient ServicioFacturacionSincronizacionClient()
        {
            ServicioFacturacionSincronizacionClient client = new ServicioFacturacionSincronizacionClient(
                 ConfigurationEndpoints()[typeof(ServicioFacturacionSincronizacionClient)].SINBinding,
                 ConfigurationEndpoints()[typeof(ServicioFacturacionSincronizacionClient)].SINEndpointAddress
                );
            //ConfigScope<ServicioFacturacionSincronizacion>(client);
            return client;
        }

        public SIN.BasicExpensesServices.ServicioFacturacionClient ServicioFacturacionServiciosBasicoClient()
        {
            SIN.BasicExpensesServices.ServicioFacturacionClient client = new SIN.BasicExpensesServices.ServicioFacturacionClient(
                 ConfigurationEndpoints()[typeof(SIN.BasicExpensesServices.ServicioFacturacionClient)].SINBinding,
                 ConfigurationEndpoints()[typeof(SIN.BasicExpensesServices.ServicioFacturacionClient)].SINEndpointAddress
                );
            //ConfigScope<SIN.BasicExpensesServices.ServicioFacturacion>(client);
            return client;
        }

        public SIN.OrdersServices.ServicioFacturacionClient ServicioFacturacionCompraVentaClient()
        {
            SIN.OrdersServices.ServicioFacturacionClient client = new SIN.OrdersServices.ServicioFacturacionClient(
                 ConfigurationEndpoints()[typeof(SIN.OrdersServices.ServicioFacturacionClient)].SINBinding,
                 ConfigurationEndpoints()[typeof(SIN.OrdersServices.ServicioFacturacionClient)].SINEndpointAddress
                );
            //ConfigScope<SIN.OrdersServices.ServicioFacturacion>(client);
            return client;
        }

        public SIN.FinancialServices.ServicioFacturacionClient ServicioFacturacionFinancieroClient()
        {
            SIN.FinancialServices.ServicioFacturacionClient client = new SIN.FinancialServices.ServicioFacturacionClient(
                 ConfigurationEndpoints()[typeof(SIN.FinancialServices.ServicioFacturacionClient)].SINBinding,
                 ConfigurationEndpoints()[typeof(SIN.FinancialServices.ServicioFacturacionClient)].SINEndpointAddress
                );
            //ConfigScope<SIN.FinancialServices.ServicioFacturacion>(client);
            return client;
        }

        public SIN.ExchanceMoneyServices.ServicioFacturacionClient ServicioFacturacionCambioMonedaClient()
        {
            SIN.ExchanceMoneyServices.ServicioFacturacionClient client = new SIN.ExchanceMoneyServices.ServicioFacturacionClient(
                 ConfigurationEndpoints()[typeof(SIN.ExchanceMoneyServices.ServicioFacturacionClient)].SINBinding,
                 ConfigurationEndpoints()[typeof(SIN.ExchanceMoneyServices.ServicioFacturacionClient)].SINEndpointAddress
                );
            //ConfigScope<SIN.ExchanceMoneyServices.ServicioFacturacion>(client);
            return client;
        }

        public OperationContextScope GetConfigScope(IClientChannel channel)
        {
            OperationContextScope OperationContextScope = new OperationContextScope(channel);
            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            requestMessage.Headers[NAME_HEADER_API_KEY] = GetAPIKey();
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
            return OperationContextScope;
        }

        private Binding GetBindingForEndpoint()
        {
            System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
            result.MaxBufferSize = int.MaxValue;
            result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            result.MaxReceivedMessageSize = int.MaxValue;
            result.AllowCookies = true;
            result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            return result;
        }
    }
}
