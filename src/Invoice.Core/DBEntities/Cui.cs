using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class Cui
{
    public long IdCuis { get; set; }

    public string ValorCuis { get; set; } = null!;

    public bool EsRegistroActual { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string Request { get; set; } = null!;

    public string Response { get; set; } = null!;

    public long IdOfficeExternal { get; set; }

    public long IdOffice { get; set; }

    public DateTime? FechaBaja { get; set; }

    public virtual ICollection<Cufd> Cufds { get; set; } = new List<Cufd>();

    public virtual Office IdOfficeNavigation { get; set; } = null!;
}
