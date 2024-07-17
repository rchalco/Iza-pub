using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Types
{
    public class typeFormaDePagoPedido
    {
        public int idPedidoMaestro { get; set; }
        public int idFormaPago { get; set; }
        public decimal MontoCubierto { get; set; }
        public decimal Diferencia { get; set; }
    }
}
