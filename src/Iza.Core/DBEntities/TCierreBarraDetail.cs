using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TCierreBarraDetail
{
    public long IdAsigDetail { get; set; }

    public long IdSesion { get; set; }

    public long IdfechaProceso { get; set; }

    public long IdCierreBarraMaster { get; set; }

    public long? IdProducto { get; set; }

    public decimal? Entradaxasignaciones { get; set; }

    public decimal? Salidaxasignaciones { get; set; }

    public decimal? CantidadVendida { get; set; }

    public decimal? StockFinal { get; set; }

    public decimal? CantidadEntregada { get; set; }

    public decimal? Diferencia { get; set; }

    public int? IdEstado { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; }
}
