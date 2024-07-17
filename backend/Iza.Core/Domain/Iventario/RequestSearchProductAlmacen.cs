using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Iventario
{
    public class RequestSearchProductAlmacen
    {
        public int idSesion { get; set; }
        public int idFechaProceso { get; set; }
        public int idAlmacen { get; set; }
    }
}
