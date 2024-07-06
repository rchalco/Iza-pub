using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class Parameter
{
    public int IdParameter { get; set; }

    public string KeyName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Value { get; set; } = null!;

    public DateTime DateRegister { get; set; }

    public DateTime DateUpdate { get; set; }

    public string Group { get; set; } = null!;

    public bool Enabled { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;
}
