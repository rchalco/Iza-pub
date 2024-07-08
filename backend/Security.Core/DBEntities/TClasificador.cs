using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TClasificador
{
    public long IdClasificador { get; set; }

    public long IdSesion { get; set; }

    public long IdClasificadorTipo { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Observaciones { get; set; }

    public string? EtiquetaIzquierda { get; set; }

    public string? EtiquetaDerecha { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciHasta { get; set; }
}
