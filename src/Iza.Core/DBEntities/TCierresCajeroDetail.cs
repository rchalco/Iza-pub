using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TCierresCajeroDetail
{
    public long IdCierreCajeroDetail { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public long IdCierreCajeroMaster { get; set; }

    public int? IdFormaDePago { get; set; }

    public decimal? TotalVendido { get; set; }

    public decimal? TotalDeclarado { get; set; }

    public decimal? Diferencia { get; set; }

    public string? Observaciones { get; set; }

    public int? IdEstado { get; set; }

    public DateTime FechaRegistro { get; set; }
}
