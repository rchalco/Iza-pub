using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TFechasProceso
{
    public long IdFechaProceso { get; set; }

    public long? IdSesion { get; set; }

    public DateTime FechaDeProceso { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaCierre { get; set; }
}
