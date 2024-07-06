using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class Cufd
{
    public long IdCufd { get; set; }

    public string ValorCufd { get; set; } = null!;

    public string CodigoControl { get; set; } = null!;

    public string CuisRequest { get; set; } = null!;

    public bool EsRegistroActual { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string Request { get; set; } = null!;

    public string Response { get; set; } = null!;

    public long IdOffice { get; set; }

    public long? IdCuis { get; set; }

    public DateTime? FechaBaja { get; set; }

    public virtual Cui? IdCuisNavigation { get; set; }

    public virtual Office IdOfficeNavigation { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
