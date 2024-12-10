using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta.Caja
{
    public class ResponseObtieneCajerosCierre
    {
        public int idOperacionDiariaCaja { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string nombreCompleto { get; set; } = string.Empty;
        public string ci { get; set; } = string.Empty;
        public string barra { get; set; } = string.Empty;
    }
}
