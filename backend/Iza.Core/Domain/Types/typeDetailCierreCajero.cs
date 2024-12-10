using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Types
{
    public class typeDetailCierreCajero
    {
        public int idFormaDePago { get; set; }
        public decimal TotalVendido { get; set; }
        public decimal totalDeclarado { get; set; }
        public decimal Diferencia { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }
}
