using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta
{
    public class ResulObtieneFormasdePago
    {
        public int idFormaPago { get; set; }
        public string FormaDePago { get; set; }
        public int ordenDespliegue { get; set; }

        public bool EsTarjetaPropia { get; set; }
    }
}
