using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TJournalOperation
{
    public long IdJournalClose { get; set; }

    public long? IdSesion { get; set; }

    public long IdFechaProceso { get; set; }

    public int? IdOperacion { get; set; }

    public int? IdProducto { get; set; }

    public decimal Cantidad { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
