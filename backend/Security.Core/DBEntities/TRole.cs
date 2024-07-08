using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TRole
{
    public long IdRol { get; set; }

    public long? IdSesion { get; set; }

    public string Rol { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
