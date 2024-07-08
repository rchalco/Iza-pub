using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TBarrasDeAtencion
{
    public long IdBarraDeAtencion { get; set; }

    public long? IdSesion { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
