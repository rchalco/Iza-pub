using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TCierreBarraMaster
{
    public long IdCierreBarraMaster { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public int? IdAlmacen { get; set; }

    public int? IdEstado { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; }
}
