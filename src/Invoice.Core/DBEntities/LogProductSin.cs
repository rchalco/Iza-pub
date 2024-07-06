using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class LogProductSin
{
    public int IdProductSin { get; set; }

    public string CodeActivity { get; set; } = null!;

    public string CodeProduct { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ResulJson { get; set; } = null!;

    public DateTime DateRegister { get; set; }
}
