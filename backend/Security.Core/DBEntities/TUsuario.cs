using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TUsuario
{
    public long IdUsuario { get; set; }

    public long IdSesion { get; set; }

    public long IdPersona { get; set; }

    public long IdRol { get; set; }

    public string Usuario { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public int Activo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
