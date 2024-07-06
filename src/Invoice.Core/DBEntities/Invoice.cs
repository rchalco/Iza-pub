using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class Invoice
{
    public int IdInvoice { get; set; }

    public string? InvoiceSign { get; set; }

    public string? InvoiceJson { get; set; }

    public string ProcessObject { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public string State { get; set; } = null!;

    public string DetailProcess { get; set; } = null!;

    public long? IdEventoSignificativo { get; set; }

    public string? CodeBatch { get; set; }

    public string? FileNameCompress { get; set; }

    public string? FolderContainer { get; set; }

    public DateTime? ProcessDate { get; set; }

    public string? InvoiceNumber { get; set; }

    public int? IndexFile { get; set; }

    public long? IdOffice { get; set; }

    public long? IdCufd { get; set; }

    public int TypeProcess { get; set; }

    public DateTime? ModifyDate { get; set; }

    public string? InvoiceSha256 { get; set; }

    public string? CodigoSector { get; set; }

    public string? Cuf { get; set; }

    public string? RequestSendOnline { get; set; }

    public string? ResponseSendOnline { get; set; }

    public string? RequestAnulacion { get; set; }

    public string? ResponseAnulacion { get; set; }

    public string? PathInvoicePdf { get; set; }

    public string? TypeObjectProcessed { get; set; }

    public virtual Cufd? IdCufdNavigation { get; set; }

    public virtual EventoSignificativo? IdEventoSignificativoNavigation { get; set; }

    public virtual Office? IdOfficeNavigation { get; set; }
}
