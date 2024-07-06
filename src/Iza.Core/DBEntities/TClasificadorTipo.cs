using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TClasificadorTipo
{
    public long IdClasificadorTipo { get; set; }

    public long? IdSesion { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Observaciones { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciHasta { get; set; }
}
