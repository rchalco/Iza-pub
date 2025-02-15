using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta.Caja
{
    public class ResponseSPObtPedidosPorFormasDePago
    {
        public int idPedMaster { get; set; }
        public decimal montoApertura { get; set; }
        public decimal montoPedidoTotal { get; set; }
        public int idFormaDePago { get; set; }
        public string FormaDePago { get; set; } = string.Empty;
        public decimal MontoCubierto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string fechaTransaccion { get; set; } = string.Empty;
    }
}
