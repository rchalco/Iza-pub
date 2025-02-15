using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.General
{
    public class GeneralRequestFecha
    {
        public long idSesion { get; set; }
        public long idFechaProceso { get; set; }
        public DateTime fechaProceso { get; set; }
    }
}
