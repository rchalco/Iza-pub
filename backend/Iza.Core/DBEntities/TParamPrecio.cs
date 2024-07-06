using System;
using System.Collections.Generic;

namespace Iza.Core.DBEntities;

public partial class TParamPrecio
{
    public long IdPrecio { get; set; }

    public long IdSesion { get; set; }

    public long IdClasificador { get; set; }

    public string? DescripcionMenu { get; set; }

    public int Cantidad { get; set; }

    public string? Embase { get; set; }

    public decimal Precio { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public byte[]? PicProducto { get; set; }

    public int OrdenDespliegue { get; set; }

    public int DepliegueDerecha { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }

    public int? Orden { get; set; }
}
