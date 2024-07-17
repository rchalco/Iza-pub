using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Iventario
{
    public class AlmacenDTO
    {
        public long idAlmacen { get; set; }
        public long? IdSesion { get; set; }
        public long IdUsuario { get; set; }
        public int? IdTipoAlmacen { get; set; }
        public string descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaVigenciaHasta { get; set; }
    }
}
