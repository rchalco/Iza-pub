using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TLogVar
{
    public long IdLogVar { get; set; }

    public string? Vars { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
