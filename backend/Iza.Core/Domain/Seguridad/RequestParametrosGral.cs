using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Seguridad
{
    public class RequestParametrosGral
    {
        public string ParametroTexto1 { get; set; }
        public string ParametroTexto2 { get; set; }
        public DateTime ParametroFecha1 { get; set; }
        public DateTime ParametroFecha2 { get; set; }
        public long ParametroLong1 { get; set; }
        public long ParametroLong2 { get; set; }
        public long ParametroLong3 { get; set; }
        public decimal ParametroDecimal1 { get; set; }
        public int ParametroInt1 { get; set; }
    }
}
