using Iza.Core.Domain.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta
{
    public class RequestCajerosActivos
    {
        public long idSesion { get; set; }
        public List<LoginDTO> detalleCajeros { get; set; }
    }
}
