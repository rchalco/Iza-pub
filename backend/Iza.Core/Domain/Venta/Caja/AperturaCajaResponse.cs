using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta.Caja
{
    public class AperturaCajaResponse
    {
        public long idFechaProceso { get; set; }
        public DateTime FechaProceso { get; set; }
        public long idOperacionDiaria { get; set; }
    }
}
