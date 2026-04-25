using System;
using System.Collections.Generic;
using System.Text;

namespace Iza.Core.Domain.Reportes
{
    public class ResponseVentasXDiaXMenu
    {
        public string fechaProceso { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public long idPrecio { get; set; }
        public string menu { get; set; } = string.Empty;

        public int cantidad { get; set; }
        public decimal monto { get; set; }
    }
}
