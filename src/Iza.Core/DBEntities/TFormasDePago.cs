using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TFormasDePago
{
    public int IdFormaDePago { get; set; }

    public string FormaDePago { get; set; } = null!;

    public int OrdenDespliegue { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaRegistroHasta { get; set; }
}
