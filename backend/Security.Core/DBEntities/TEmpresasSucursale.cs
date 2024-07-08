using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class TEmpresasSucursale
{
    public long IdEmpresa { get; set; }

    public long? IdSesion { get; set; }

    public string RazonSocialVc { get; set; } = null!;

    public string? TipoContribuyente { get; set; }

    public string? ResponsableLegal { get; set; }

    public long? IdEmpresaPadre { get; set; }

    public string NitEmpresaVc { get; set; } = null!;

    public string? NombreSucursal { get; set; }

    public string? Direccion { get; set; }

    public string? Zona { get; set; }

    public string? Telefono { get; set; }

    public string? Ciudad { get; set; }

    public string? Firma { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaVigenciaHasta { get; set; }
}
