using System;

namespace Iza.Core.Domain.Reportes
{
    public class FechaProcesoDTO
    {
        public long idFechaProceso { get; set; }
        public long? idSesion { get; set; }
        public DateTime FechaDeProceso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaCierre { get; set; }
    }
}
