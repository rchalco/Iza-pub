using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TCierresCajeroMaster
{
    public long IdCierreCajeroMaster { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public int? IdOperacionDiariaCaja { get; set; }

    public decimal? MontoApertura { get; set; }

    public decimal? MontoTotalCierre { get; set; }

    public int? IdEstado { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; }
}
