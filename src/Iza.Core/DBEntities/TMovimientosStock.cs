using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TMovimientosStock
{
    public long IdMovStock { get; set; }

    public long IdSesion { get; set; }

    public long IdFechaProceso { get; set; }

    public int IdMovimiento { get; set; }

    public long? IdAlmacen { get; set; }

    public long? IdParamPrecio { get; set; }

    public long IdProducto { get; set; }

    public decimal CantidadMovimiento { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Diferencia { get; set; }

    public int? UnidadesPorCaja { get; set; }

    public decimal? PrecioCaja { get; set; }

    public DateTime? FechaDeVencimiento { get; set; }

    public long? IdPedido { get; set; }

    public long? IdAsignacion { get; set; }

    public int? IdEstado { get; set; }

    public string? Observacion { get; set; }

    public DateTime FechaRegistro { get; set; }
}
