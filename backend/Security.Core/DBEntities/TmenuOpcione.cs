using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TmenuOpcione
{
    public int IdMenuOpcion { get; set; }

    public string Title { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Decripcion { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
