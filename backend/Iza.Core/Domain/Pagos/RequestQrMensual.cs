namespace Iza.Core.Domain.Pagos
{
    public class RequestQrMensual
    {
        /// <summary>
        /// Sucursal o punto de venta que recauda. Si se omite se usa <c>QrBancoEconomico:CodigoSucursal</c>.
        /// Determina la clave de cache: cada sucursal tiene su propio QR del mes.
        /// </summary>
        public string? codigoSucursal { get; set; }

        /// <summary>Moneda del QR. Si se omite se usa <c>QrBancoEconomico:Moneda</c>.</summary>
        public string? moneda { get; set; }

        /// <summary>
        /// Emite un QR nuevo aunque el vigente siga en cache. Solo para reemplazar un codigo comprometido
        /// o mal impreso: el QR anterior sigue aceptando pagos hasta su vencimiento.
        /// </summary>
        public bool forzarRenovacion { get; set; }
    }
}
