using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceConfiguration.Office.DTO
{
    public class DTOCompany
    {
        public int IdCompany { get; set; }

        public string CompanyKey { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Nit { get; set; } = null!;

        public string SocialReason { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string SystemName { get; set; } = null!;

        public string Sintoken { get; set; } = null!;

        public int SincodeEnviroment { get; set; }

        /// <summary>
        /// 1 = Electrónica en Línea   2 = Computarizada en Línea 3 = Portal Web en Línea
        /// </summary>
        public int SincodeModeInvoice { get; set; }

        public string SincodeSystem { get; set; } = null!;

        public virtual List<DTOOffice> Offices { get; set; } = new List<DTOOffice>();
        
    }
}
