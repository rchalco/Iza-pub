using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.General
{
    public class DataDocumento
    {
        public string titulo { get; set; } = string.Empty;
        public List<string> titulosTabla { get; set; } 
        public List<string> contenido { get; set; }
        public string pie { get; set; } = string.Empty;
        public string pathLogo { get; set; } = string.Empty;
        public bool? isTableDocument { get; set; }
        /// <summary>Tabla para impresora termica de 80mm (ver EngineImpresion.GenerarDocumentoTablaTermica).</summary>
        public bool? isThermalTable { get; set; }
        /// <summary>Anchos relativos de las columnas de la tabla; vacio reparte el ancho en partes iguales.</summary>
        public List<float> anchosColumnas { get; set; }
    }
}
