using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Seguridad
{
    public class RequestMenuUsuario
    {
        public long IdSesion { get; set; }
        public long IdRol { get; set; }
    }
}
