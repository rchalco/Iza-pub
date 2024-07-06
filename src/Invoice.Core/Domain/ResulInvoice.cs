using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Domain
{
    public class ResulInvoice
    {
        public int nroFactura { get; set; }
        public string mail { get; set; }
        public string cufd { get; set; }
        public string reportePDF { get; set; }

    }
}
