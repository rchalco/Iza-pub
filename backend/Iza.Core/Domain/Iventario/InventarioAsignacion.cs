using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Iventario
{
    public class InventarioAsignacion
    {
        public long idSesion { get; set; }
        public long idfechaproceso { get; set; }
        public long idAlmacenDesde { get; set; }
        public long idAlmacenHasta { get; set; }
        public string observaciones { get; set; } = string.Empty;
        public List<InventarioProducto> detalleProductos { get; set; }
    }
}
