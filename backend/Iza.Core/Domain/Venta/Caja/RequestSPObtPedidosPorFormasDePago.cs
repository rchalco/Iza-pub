using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta.Caja
{
    public class RequestSPObtPedidosPorFormasDePago
    {
        public int idSesion { get; set; }
        public int idFechaProceso { get; set; }
        public int idAlmacen { get; set; }
        public int idEstado { get; set; }
        public int idOperacionDiaria { get; set; }
    }
}
