using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class TestInvoice
{
    public int IdInvoice { get; set; }

    public string? InvoiceSign { get; set; }

    public byte[]? FacturaBytes { get; set; }

    public string? ProcessObject { get; set; }

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
}
