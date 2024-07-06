using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TIngrediente
{
    public long IdIngrediente { get; set; }

    public long IdSesion { get; set; }

    public long IdPrecio { get; set; }

    public long IdProducto { get; set; }

    public decimal Cantidad { get; set; }

    public decimal? MontoIndividual { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
