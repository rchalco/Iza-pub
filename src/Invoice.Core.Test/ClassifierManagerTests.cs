using Invoice.Core.Business.InvoiceConfiguration.Office.DTO;
using Invoice.Core.Business.InvoiceConfiguration.Office;
using Invoice.Core.Business.Office;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoice.Core.Business.InvoiceConfiguration.Sync;

namespace Invoice.Core.Test
{
    public class ClassifierManagerTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SincronizarActividades_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "PERFECTO"
            };
                ClassifierManager classifierManager = new ClassifierManager(contextCompany);
                Response response = classifierManager.SincronizarActividades();
                Assert.That(response.State, Is.EqualTo(ResponseType.Success));
            
        }

        [Test]
        public void SincronizarFechaHoraActual_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.SincronizarFechaHoraActual();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        
        }

        [Test]
        public void SincronizarListaActividadesDocumentoSector_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.SincronizarListaActividadesDocumentoSector();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarListaLeyendasFactura_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarListaLeyendasFactura();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));

        }

        [Test]
        public void sincronizarListaMensajesServicios_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarListaMensajesServicios();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarListaProductosServicios_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarListaProductosServicios();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));

        }

        [Test]
        public void sincronizarParametricaEventosSignificativos_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaEventosSignificativos();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarParametricaMotivoAnulacion_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaMotivoAnulacion();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
            
        }

        [Test]
        public void sincronizarParametricaPaisOrigen_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaPaisOrigen();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarParametricaTipoDocumentoIdentidad_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaTipoDocumentoIdentidad();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }


        [Test]
        public void SincronizarTipoDocumentoSector_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.SincronizarTipoDocumentoSector();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarParametricaTipoEmision_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaTipoEmision();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarParametricaTipoHabitacion_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaTipoHabitacion();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success)); 
        }

        [Test]
        public void sincronizarParametricaTipoMetodoPago_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaTipoMetodoPago();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarParametricaTipoMoneda_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaTipoMoneda();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarParametricaTipoPuntoVenta_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaTipoPuntoVenta();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }

        [Test]
        public void sincronizarParametricaTipoFactura_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            for (int i = 0; i < 50; i++)
            {
                Response response = classifierManager.sincronizarParametricaTipoFactura();
                Assert.That(response.State, Is.EqualTo(ResponseType.Success));
            }
        }

        [Test]
        public void sincronizarParametricaUnidadMedida_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 1,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            Response response = classifierManager.sincronizarParametricaUnidadMedida();
            Assert.That(response.State, Is.EqualTo(ResponseType.Success));
        }
        //sincronizarParametricaUnidadMedida

    }
}
