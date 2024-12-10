using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.General
{
    public class GeneralRequestBusqueda
    {
        public long idSesion { get; set; }
        public string textoABuscar { get; set; }
    }
}
