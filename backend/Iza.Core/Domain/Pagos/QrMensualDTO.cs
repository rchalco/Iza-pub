using System;

namespace Iza.Core.Domain.Pagos
{
    /// <summary>
    /// QR de cobro vigente durante un mes para una sucursal. Es reutilizable (<c>singleUse = false</c>)
    /// y de monto editable, de modo que el mismo codigo sirve para todos los pedidos del periodo y el
    /// cliente digita el importe al pagar.
    /// </summary>
    public class QrMensualDTO
    {
        /// <summary>Identificador que asigna Baneco. Es el que devuelve el reporte de QR pagados.</summary>
        public string qrId { get; set; } = string.Empty;

        /// <summary>Imagen PNG del QR en Base64, tal como la entrega el banco.</summary>
        public string qrImageBase64 { get; set; } = string.Empty;

        /// <summary>Identificador que Iza envio al emitirlo. Unico por suscriptor.</summary>
        public string transactionId { get; set; } = string.Empty;

        public string moneda { get; set; } = string.Empty;

        public string codigoSucursal { get; set; } = string.Empty;

        public DateTime fechaEmision { get; set; }

        /// <summary>Ultimo dia en que el QR admite pagos.</summary>
        public DateTime fechaVencimiento { get; set; }

        /// <summary>Momento en que la cache deja de entregarlo y se emite uno nuevo.</summary>
        public DateTime fechaRenovacion { get; set; }

        /// <summary>El monto lo digita el pagador; el QR no lleva importe fijo.</summary>
        public bool montoEditable { get; set; } = true;

        /// <summary><c>true</c> si se sirvio desde la cache y no se llamo al banco.</summary>
        public bool desdeCache { get; set; }
    }
}
