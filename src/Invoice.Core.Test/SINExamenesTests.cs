using Invoice.Core.Business.InvoiceConfiguration.Sync;
using Invoice.Core.Business.Office;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Test
{
    public class SINExamenesTests
    {

        [Test]
        public void SinSincronizarActividades_Success_Success()
        {
            ContextCompany contextCompany = new ContextCompany
            {
                IdCompany = 1,
                IdOffice = 2,
                IdOfficeExternal = 0,
                CompanyKey = "GAMATEK"
            };
            ClassifierManager classifierManager = new ClassifierManager(contextCompany);
            for (int i = 0; i < 1; i++)
            {
                Response response = classifierManager.SincronizarActividades();
                Assert.That(response.State, Is.EqualTo(ResponseType.Success));
            }
        }

        /***************************************************/
        [Test]
        public void sincronizarParametricaUnidadMedida_Success()
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
                Response response = classifierManager.sincronizarParametricaUnidadMedida();
                Assert.That(response.State, Is.EqualTo(ResponseType.Success));
            }            
        }

    }
}
