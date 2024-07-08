using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TPedidoDetail
{
    public long IdPedDetail { get; set; }

    public long? IdPedMaster { get; set; }

    public long? IdParamPrecio { get; set; }

    public long? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioTotalDetalle { get; set; }

    public int? IdEstado { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
