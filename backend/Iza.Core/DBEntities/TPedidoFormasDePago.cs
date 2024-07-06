using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TPedidoFormasDePago
{
    public long IdPedFormaPago { get; set; }

    public long IdPedMaster { get; set; }

    public int IdFormaDePago { get; set; }

    public decimal MontoCubierto { get; set; }

    public decimal? Diferencia { get; set; }

    public int? Estado { get; set; }

    public DateTime FechaRegistro { get; set; }
}
