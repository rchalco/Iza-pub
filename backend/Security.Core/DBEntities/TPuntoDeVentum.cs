using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TPuntoDeVentum
{
    public long IdPuntoDeVenta { get; set; }

    public long? IdSesion { get; set; }

    public int? IdBarraDeAtencion { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
