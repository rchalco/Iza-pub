using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Reportes
{
    public class ProductosVendidosPorBarraDTO
    {
        public long idProducto { get; set; }
        public string nombreProducto { get; set; } = string.Empty;
        public int enteras { get; set; }
        public int unidades { get; set; }
    }
}
