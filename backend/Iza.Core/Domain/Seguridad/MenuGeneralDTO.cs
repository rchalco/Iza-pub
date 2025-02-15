using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Seguridad
{
    public class MenuGeneralDTO
    {
        public long idMenuOpcion { get; set; }
        public string title { get; set; }
        public string Url { get; set; }
        public string icon { get; set; }
    }
}
