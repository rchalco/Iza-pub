using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TAsignacionDetail
{
    public long IdAsignacionDetail { get; set; }

    public long IdAsignacionMaster { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? MontoDeCompra { get; set; }

    public int? IdEstado { get; set; }

    public DateTime FechaRegistro { get; set; }
}
