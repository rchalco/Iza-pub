using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TCantidadDeProductosPorJornadum
{
    public long IdJornada { get; set; }

    public long? IdSesion { get; set; }

    public long IdFechaProceso { get; set; }

    public int IdAlmacen { get; set; }

    public int? IdOperacion { get; set; }

    public int? IdProducto { get; set; }

    public decimal Cantidad { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaRegistroHasta { get; set; }
}
