using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Reportes
{
    public class PedidoDTO
    {
        public int idProducto { get; set; }
        public int idPedido { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }

    }
}
