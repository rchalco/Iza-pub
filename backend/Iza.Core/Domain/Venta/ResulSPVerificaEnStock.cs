using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta
{
    public class ResulSPVerificaEnStock
    {
        public int idProducto { get; set; }
        public string nombreProducto { get; set; }
        public decimal PEDIDO { get; set; }
        public decimal EnSTOCK { get; set; }
        public decimal DIFERENCIA { get; set; }
    }
}
