using Invoice.Core.Business.InvoiceConfiguration.Parameters;
using Invoice.Core.Business.Office;
using PlumbingProps.Wrapper;

namespace Invoice.Core.Test
{
    public class ParameterManagerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SincronizarCodigoCUISGlobal_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
            };
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            Response response = parameterManager.SincronizarCodigoCUISGlobal();
            Assert.AreEqual(response.State, ResponseType.Success);
        }

        [Test]
        public void CrearCUFD_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
            };
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            //for (int i = 0; i < 100; i++)
            //{
                Response response = parameterManager.CrearCUFD();
                Assert.AreEqual(response.State, ResponseType.Success);
            //}
        }

        [Test]
        public void VerifyCUFDCurrentDate_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
            };
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            Response response = parameterManager.VerifyCUFDCurrentDate();
            Assert.AreEqual(response.State, ResponseType.Success);
        }

        [Test]
        public void ObtenerLeyendas_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
            };
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            Response response = parameterManager.ObtenerLeyendas();
            Assert.AreEqual(response.State, ResponseType.Success);
        }

        [Test]
        public void ObtenerLeyenda3Leyendas_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
            };
            int codigoActividad = 351000;
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            Response response = parameterManager.ObtenerLeyenda3Leyendas(codigoActividad);
            Assert.AreEqual(response.State, ResponseType.Success);
        }

        [Test]
        public void ParametrizarProductos_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
            };
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            Response response = parameterManager.ParametrizarProductos();
            Assert.AreEqual(response.State, ResponseType.Success);
        }

        [Test]
        public void VerificarConexion_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
            };
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            Response response = parameterManager.VerificarConexion();
            Assert.AreEqual(response.State, ResponseType.Success);
        }

        [Test]
        public void CambiarModoFacturacion_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
            };
            int modo = 0;//0 = online - 1 = offline
            ParameterManager parameterManager = new ParameterManager(contextCompany);
            Response response = parameterManager.CambiarModoFacturacion(modo);
            Assert.AreEqual(response.State, ResponseType.Success);
        }
    }
}