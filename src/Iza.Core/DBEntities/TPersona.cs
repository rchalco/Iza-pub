using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TPersona
{
    public long IdPersona { get; set; }

    public long IdSesion { get; set; }

    public string DocumentoDeIdentidad { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
