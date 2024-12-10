using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Reportes
{
    public class DetallePedidosDTO
    {
        public long idPedMaster { get; set; }
        public long idPedFormaPago { get; set; }
        public string fechaRegistro { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        public string formaDePago { get; set; } = string.Empty;
        public decimal montoCubierto { get; set; }
        public string estado { get; set; } = string.Empty;

    }
}
