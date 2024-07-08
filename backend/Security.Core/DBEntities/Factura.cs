using System;
using System.Collections.Generic;

namespace Security.Core.DBEntities;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int IdDosificacion { get; set; }

    public decimal NumFactura { get; set; }

    public string? NitCliente { get; set; }

    public string? NombreFactura { get; set; }

    public string CompraVenta { get; set; } = null!;

    public bool Anulada { get; set; }

    public int Impresiones { get; set; }

    public DateTime FechaEmision { get; set; }

    public DateTime? FechaUltModificacion { get; set; }

    public DateTime? FechaAnulacion { get; set; }

    public string? CodControl { get; set; }

    public virtual ICollection<FacturasDetalle> FacturasDetalles { get; set; } = new List<FacturasDetalle>();

    public virtual Dosificacion IdDosificacionNavigation { get; set; } = null!;
}
