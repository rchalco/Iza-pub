using System;
using System.Collections.Generic;

namespace Invoice.Core.DBEntities;

public partial class LogBatchProcess
{
    public int IdLogBatchProcess { get; set; }

    public string FileName { get; set; } = null!;

    public string RequestEnviado { get; set; } = null!;

    public string ResponseEnviado { get; set; } = null!;

    public string? EstadoEnviado { get; set; }

    public string? CodigoDescripcionEnviado { get; set; }

    public string? CodigoRecepcionEnviado { get; set; }

    public string? EstadoValidado { get; set; }

    public string? CodigoDescripcionValidado { get; set; }

    public string? CodigoRecepcionEnviadoValidado { get; set; }

    public string? RequestValidado { get; set; }

    public string? ResponseValidado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? CodeBatch { get; set; }

    public long? IdEventoSignificativo { get; set; }

    public virtual EventoSignificativo? IdEventoSignificativoNavigation { get; set; }
}
