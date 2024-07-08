using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TCaja
{
    public long IdCaja { get; set; }

    public long? IdSesion { get; set; }

    public long IdUsuario { get; set; }

    public int? IdTipoCaja { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
