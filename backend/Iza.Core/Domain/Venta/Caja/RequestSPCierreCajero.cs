using Iza.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta.Caja
{
    public class RequestSPCierreCajero
    {
        public int idSesion { get; set; }
        public int idfechaproceso { get; set; }
        public int idOperacionDiariaCaja { get; set; }
        public List<typeDetailCierreCajero> detalle { get; set; }
        public decimal MontoApertura { get; set; }
        public decimal MontoTotalCierre { get; set; }
        public int idEstado { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }
}
