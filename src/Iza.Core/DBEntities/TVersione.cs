using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TVersione
{
    public int IdVersion { get; set; }

    public string Version { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
