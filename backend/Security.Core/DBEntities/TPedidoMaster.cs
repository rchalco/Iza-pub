using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TPedidoMaster
{
    public long IdPedMaster { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public long? IdMesero { get; set; }

    public long IdOperacionDiariaCaja { get; set; }

    public int? IdAlmacen { get; set; }

    public long? IdAmbiente { get; set; }

    public long? IdFactura { get; set; }

    public long? IdFacCliente { get; set; }

    public decimal? MontoPedidoTotal { get; set; }

    public decimal? MontoEntregado { get; set; }

    public decimal? MontoVuelto { get; set; }

    public decimal? MontoDescuento { get; set; }

    public int? IdFacTipoPago { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int? IdEstado { get; set; }

    public int? IdSesionBartender { get; set; }

    public DateTime? FechaBartender { get; set; }

    public string? Observaciones { get; set; }
}
