using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class EventoSignificativo
{
    public long IdEventoSignificativo { get; set; }

    public int CodSucursal { get; set; }

    public int CodPuntoVenta { get; set; }

    public long CodEvento { get; set; }

    public string DescripcionEvento { get; set; } = null!;

    public int Idcufd { get; set; }

    public string CufdEvento { get; set; } = null!;

    public string? Cufd { get; set; }

    public string Request { get; set; } = null!;

    public string Response { get; set; } = null!;

    public string CodEventoSignificativo { get; set; } = null!;

    public bool Procesado { get; set; }

    public int? Idcufdevento { get; set; }

    public DateTime? FechaHoraIni { get; set; }

    public DateTime? FechaHoraFin { get; set; }

    public string? CodigoControlCufd { get; set; }

    public string? CodigoControlCufdevento { get; set; }

    public long? IdOffice { get; set; }

    public virtual Office? IdOfficeNavigation { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<LogBatchProcess> LogBatchProcesses { get; set; } = new List<LogBatchProcess>();
}
