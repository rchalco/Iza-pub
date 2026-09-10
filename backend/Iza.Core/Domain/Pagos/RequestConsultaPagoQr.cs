using System;

namespace Iza.Core.Domain.Pagos
{
    public class RequestConsultaPagoQr
    {
        /// <summary>QR a verificar. Es el <c>qrId</c> que devolvio la emision.</summary>
        public string qrId { get; set; } = string.Empty;

        /// <summary>
        /// Dia sobre el que se busca el cobro. El banco reporta por dia; si se omite se usa hoy.
        /// Un QR emitido cerca de medianoche puede acreditarse al dia siguiente.
        /// </summary>
        public DateTime fecha { get; set; }
    }
}
