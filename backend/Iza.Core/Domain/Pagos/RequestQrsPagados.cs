using System;

namespace Iza.Core.Domain.Pagos
{
    public class RequestQrsPagados
    {
        /// <summary>Fecha de los cobros a listar. El banco reporta por dia, no por rango.</summary>
        public DateTime fecha { get; set; }
    }
}
