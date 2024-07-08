using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TProducto
{
    public long IdProducto { get; set; }

    public long? IdSesion { get; set; }

    public int IdClasificador { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string? Marca { get; set; }

    public string? Contenido { get; set; }

    public int? CajaXunidades { get; set; }

    public string? Descripcion { get; set; }

    public byte[]? PicProducto { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
