using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TAsignacionDetail1
{
    public long IdAsigDetail { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public long IdAsigMaster { get; set; }

    public int? IdProducto { get; set; }

    public int? IdTipoUnidad { get; set; }

    public int? UnidadesPorCaja { get; set; }

    public int? Cantidad { get; set; }

    public decimal? MontoDeCompra { get; set; }

    public decimal? CostoUnitario { get; set; }

    public DateTime? FechaDeVencimiento { get; set; }

    public int? IdEstado { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; }
}
