using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class Office
{
    public long IdOffice { get; set; }

    public string Name { get; set; } = null!;

    public int SinidPuntoVenta { get; set; }

    public string? SincodigoSucursal { get; set; }

    public string? SincodigoTipoPuntoVenta { get; set; }

    public string? CuisCreacion { get; set; }

    public string? Descripcion { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? Estado { get; set; }

    public int? CompanyId { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<Cufd> Cufds { get; set; } = new List<Cufd>();

    public virtual ICollection<Cui> Cuis { get; set; } = new List<Cui>();

    public virtual ICollection<EventoSignificativo> EventoSignificativos { get; set; } = new List<EventoSignificativo>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
