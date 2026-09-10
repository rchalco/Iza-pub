using System;

namespace Iza.Core.Domain.Pagos
{
    /// <summary>
    /// Un cobro acreditado sobre un QR de Iza. Espeja el contrato del proxy qr-banco-economico, que ya
    /// filtro los pagos de la cuenta a los QR emitidos por este suscriptor.
    /// </summary>
    public class QrPagadoDTO
    {
        public string qrId { get; set; } = string.Empty;
        public string? transactionId { get; set; }
        public DateTime fechaPago { get; set; }
        public string? horaPago { get; set; }
        public string moneda { get; set; } = string.Empty;
        public decimal monto { get; set; }
        public string? codigoBancoOrigen { get; set; }
        public string? nombrePagador { get; set; }
        public string? documentoPagador { get; set; }
        public string? cuentaPagador { get; set; }
        public string? descripcion { get; set; }
        public string? codigoSucursal { get; set; }
    }
}
