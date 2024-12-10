using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Types
{
    public class typeIngredientesDeMenu
    {
        public long idProducto { get; set; }
        public decimal medidaUnitaria { get; set;}
        public decimal medidaPorcentaje { get; set; }
        public string unidadDeMedida { get; set; } = string.Empty;
    }
}
