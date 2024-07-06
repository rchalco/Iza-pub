using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TOperacionDiariaCaja
{
    public long IdOperacionDiariaCaja { get; set; }

    public long IdSesion { get; set; }

    public long? IdCaja { get; set; }

    public int IdAlmacen { get; set; }

    public decimal MontoApertura { get; set; }

    public long? IdFechaProceso { get; set; }

    public decimal? MontoCierre { get; set; }

    public decimal? MontoCierreSis { get; set; }

    public decimal? Diferencia { get; set; }

    public string? ObservacioApertura { get; set; }

    public string? ObservacionCierre { get; set; }

    public DateTime FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }
}
