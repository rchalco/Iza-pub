using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.General
{
    public class ClasificadorDTO
    {
        public int idClasificador { get; set; }
        public Nullable<int> idClasificadorTipo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Nullable<bool> Activo { get; set; }


    }
}
