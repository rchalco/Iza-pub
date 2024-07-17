using Iza.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta
{
    public class RequestRegistroVenta
    {
        public List<typeDetailPedido> detalleVentas { get; set; }
        public List<typeFormaDePagoPedido> formasDePago { get; set; }
        public long idSesion { get; set; }
        public long idMesero { get; set; }
        public long idFechaProceso { get; set; }
        public long idAlmancen { get; set; }
        public long idAmabiente { get; set; }
        public decimal MontoPedido { get; set; }
        public int estadoPedido { get; set; }
        public string Observaciones { get; set; }
        public long idPedidoMaestro { get; set; }
        public long idOperacionDiariaCaja { get; set; }
    }
}
