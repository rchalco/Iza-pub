using Invoice.Core.Business.InvoiceConfiguration.Office;
using Invoice.Core.Business.InvoiceConfiguration.Office.DTO;
using Invoice.Core.Business.Office;
using PlumbingProps.Wrapper;

namespace Invoice.Core.Test
{
    public class OfficeManagerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CrearOficina_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 0,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            DTOOffice _office = new DTOOffice
            {
                Descripcion = "Sucursal 1",
                Direccion = "Calle Ayacucho",
                IdOficinaExterna = 0,
                IsCasaMatriz = false,
                NombrePuntoVenta = "Sucursal 1",
                Telefono = "2-225458",
                TipoPuntoVenta = 2
            };
            OfficeManager _officeManager = new OfficeManager(contextCompany);
            Response response = _officeManager.CrearOficina(_office);
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }


        [Test]
        public void ObtenerOficinas_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            OfficeManager _officeManager = new OfficeManager(contextCompany);
            Response response = _officeManager.ObtenerPuntosVentaSIN();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void ObtenerCompany_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 0,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            OfficeManager _officeManager = new OfficeManager(contextCompany);
            Response response = _officeManager.ObtenerCompany();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void CrearPuntosVenta()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 4,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            OfficeManager _officeManager = new OfficeManager(contextCompany);
            Response response = _officeManager.CrearPuntosVenta();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void ConsutarPuntosVenta()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            OfficeManager _officeManager = new OfficeManager(contextCompany);
            Response response = _officeManager.ConsutarPuntosVenta();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }
    }
}
