using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TAlmacene
{
    public long IdAlmacen { get; set; }

    public long? IdSesion { get; set; }

    public long IdUsuario { get; set; }

    public int? IdTipoAlmacen { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
