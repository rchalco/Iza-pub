using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class CaptionsInvoice
{
    public int IdLeyenda { get; set; }

    public string? CodigoActividad { get; set; }

    public string? DescripcionLeyenda { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
