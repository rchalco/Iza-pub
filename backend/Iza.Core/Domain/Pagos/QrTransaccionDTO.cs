using System;

namespace Iza.Core.Domain.Pagos
{
    /// <summary>
    /// QR de cobro emitido para una sola venta. Es de un solo uso (<c>singleUse = true</c>) y de monto
    /// fijo (<c>modifyAmount = false</c>): el pagador no puede alterar el importe ni reutilizar el
    /// codigo, de modo que un cobro acreditado sobre este <c>qrId</c> corresponde a esta venta y a
    /// ninguna otra.
    /// </summary>
    public class QrTransaccionDTO
    {
        /// <summary>Identificador que asigna Baneco. Es la clave con la que se verifica el pago.</summary>
        public string qrId { get; set; } = string.Empty;

        /// <summary>Imagen PNG del QR en Base64, tal como la entrega el banco.</summary>
        public string qrImageBase64 { get; set; } = string.Empty;

        /// <summary>Identificador que Iza envio al emitirlo. Unico por suscriptor.</summary>
        public string transactionId { get; set; } = string.Empty;

        /// <summary>Importe exacto que el QR va a cobrar. No es editable por el pagador.</summary>
        public decimal monto { get; set; }

        public string moneda { get; set; } = string.Empty;

        public string codigoSucursal { get; set; } = string.Empty;

        public string descripcion { get; set; } = string.Empty;

        public DateTime fechaEmision { get; set; }

        /// <summary>Ultimo dia en que el QR admite el pago.</summary>
        public DateTime fechaVencimiento { get; set; }

        /// <summary>Siempre <c>true</c>: el QR se invalida con el primer cobro.</summary>
        public bool usoUnico { get; set; } = true;

        /// <summary>Siempre <c>false</c>: el importe viaja fijo en el codigo.</summary>
        public bool montoEditable { get; set; }
    }
}
