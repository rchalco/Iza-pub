using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Domain
{
    public class LoginDTO
    {
        public long idUsuario { get; set; }
        public string usuario_vc { get; set; }
        public string Password { get; set; }
        public string PasswordNuevo { get; set; }
        public string Log_respuesta { get; set; }
        public long idSesion { get; set; }
        public long idRol { get; set; }
        public long idOperacionDiariaCaja { get; set; }
        public long idAlmacen { get; set; }
        public bool respuesta { get; set; }
        public string rol_name { get; set; }
        public long idCaja { get; set; }
        public long idFechaProceso { get; set; }
        public DateTime FechaProceso { get; set; }
        public long idPersona { get; set; }
        /// <summary>
        /// Para habilitar o deshabilitar
        /// </summary>
        public bool activo { get; set; }

    }
}
