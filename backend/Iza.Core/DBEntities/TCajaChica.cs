using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TCajaChica
{
    public long IdCajaChica { get; set; }

    public long? IdSesion { get; set; }

    public long? IdFechaProceso { get; set; }

    public string? Concepto { get; set; }

    public decimal? Monto { get; set; }

    public int? EntradaSalida { get; set; }

    public int? Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
