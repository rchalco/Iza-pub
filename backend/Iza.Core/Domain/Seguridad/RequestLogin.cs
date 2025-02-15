using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Seguridad
{
    public class RequestLogin
    {
        public string usuario { get; set; }
        public string password { get; set; }
        public string passwordNuevo { get; set; }
        public long idEmpresa { get; set; }
        public string version { get; set; }
    }
}
