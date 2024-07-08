using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TMovimientosCajaChica
{
    public long IdMovimientoCajaChica { get; set; }

    public long? IdSesion { get; set; }

    public long IdFechaProceso { get; set; }

    public DateTime IdOperacionDiariaCaja { get; set; }

    public int? TipoOperacion { get; set; }

    public string? Motivo { get; set; }

    public decimal? Monto { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
