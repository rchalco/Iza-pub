using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class FacturasDetalle1
{
    public int IdFacturaDetalle { get; set; }

    public int? IdFactura { get; set; }

    public int? IdItem { get; set; }

    public string? Concepto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Monto { get; set; }

    public decimal? Ice { get; set; }

    public decimal? Excento { get; set; }

    public decimal? Descuento { get; set; }
}
