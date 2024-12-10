using Iza.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Iventario
{
    public class IngredientesDeMenuGeneralDTO
    {
        public long idSesion { get; set; } = 0;
        public long idFechaProceso { get; set; } = 0;
        public long idPrecio { get; set; }
        public string descripcionMenu { get; set; } = string.Empty;
        public string embase { get; set; } = string.Empty;
        public decimal precio { get; set; }
        public long idIngrediente { get; set; }
        public long idProducto { get; set; }
        public string nombreProducto { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        //public decimal unidades { get; set; }

        public long idCategoria { get; set; }
        public string marca { get; set; } = string.Empty;
        public string contenido { get; set; } = string.Empty;
        public int embaseXUnidades { get; set; }
        public bool activo { get; set; }
        public decimal medidaUnitaria { get; set; }
        //public decimal medidaPorcentaje { get; set; }
        public string unidaDeMedida { get; set; } = string.Empty;
        public bool depliegueDerecha { get; set; }
        public decimal precioUnitario { get; set; }
        public int cantidad { get; set; }
        public bool esParaMenu { get; set; }
        public bool esProducto { get; set; }
        

        public List<typeIngredientesDeMenu> detalle { get; set; } 
    }
}
