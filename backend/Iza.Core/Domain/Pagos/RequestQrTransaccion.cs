namespace Iza.Core.Domain.Pagos
{
    public class RequestQrTransaccion
    {
        /// <summary>Importe exacto a cobrar. Obligatorio y mayor a cero: el QR es de monto fijo.</summary>
        public decimal monto { get; set; }

        /// <summary>
        /// Sucursal o punto de venta que recauda. Si se omite se usa <c>QrBancoEconomico:CodigoSucursal</c>.
        /// Viaja como <c>branchCode</c> y es lo que permite saber donde se cobro.
        /// </summary>
        public string? codigoSucursal { get; set; }

        /// <summary>Moneda del QR. Si se omite se usa <c>QrBancoEconomico:Moneda</c>.</summary>
        public string? moneda { get; set; }

        /// <summary>Glosa que ve el pagador. Si se omite se arma una con la sucursal y la fecha.</summary>
        public string? descripcion { get; set; }
    }
}
