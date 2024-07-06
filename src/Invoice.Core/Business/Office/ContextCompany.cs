using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.Office
{
    public class ContextCompany
    {
        public int IdCompany { get; set; }
        public int IdOffice { get; set; }
        public int IdOfficeExternal { get; set; } = 0;
        public string CompanyKey { get; set; }
    }
}
