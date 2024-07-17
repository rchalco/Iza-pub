using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Types
{
    public class typeDetailPedido
    {
        public int idParamPrecio { get; set; }
        public int idProducto { get; set; }
        public Nullable<int> cantidad { get; set; }
        public decimal? PrecioFinal { get; set; }
    }
}
