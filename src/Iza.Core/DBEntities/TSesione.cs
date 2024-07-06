using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TSesione
{
    public long IdSesion { get; set; }

    public long IdUsuario { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
