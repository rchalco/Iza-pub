using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TAsignacionMaster
{
    public long IdAsignacionMaster { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public int? IdTipoMovimiento { get; set; }

    public int? IdAlmacenDesde { get; set; }

    public int? IdAlmacenHasta { get; set; }

    public int? IdEstado { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistroDesde { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
