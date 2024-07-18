using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Iventario
{
    public class AsignacionDTO
    {
        public int idClasificador { get; set; }
        public string clasificador { get; set; }
        public int idProducto { get; set; }
        public string nombreProducto { get; set; }
        public decimal stockAnterior { get; set; }
        public int ingresos { get; set; }
        public int disponibleAlmacenCentrals { get; set; }
    }
}
