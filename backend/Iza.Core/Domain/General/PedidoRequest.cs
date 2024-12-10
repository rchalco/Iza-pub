using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.General
{
    public class PedidoRequest
    {
        public long idSesion { get; set; }
        public long idPedidoMaster { get; set; }
        public long idFormaPago{ get; set; }
        public long idFechaProceso { get; set; }

    }
}
