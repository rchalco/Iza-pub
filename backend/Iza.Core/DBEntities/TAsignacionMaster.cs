using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TAsignacionMaster
{
    public long IdAsigMaster { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public int? IdAlmacenDesde { get; set; }

    public int? IdAlmacenHasta { get; set; }

    public int? IdEstado { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; }
}
