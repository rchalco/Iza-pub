using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class LogClassifierSin
{
    public int IdClassifierSin { get; set; }

    public string Nemotico { get; set; } = null!;

    public string Request { get; set; } = null!;

    public string Response { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaBaja { get; set; }
}
