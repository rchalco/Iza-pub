using Invoice.Core.Domain.Contracts;

namespace Invoice.Core.Business.InvoiceKind.Financial
{
    /// <summary>
    /// SIRVE para ENTIDAD FINANCIERA Y CAMBIO DE MONEDA TIENE MISMA ESTRUCTURA
    /// </summary>
    public class ReportEntidadFinancieraDTO
    {
        public int nitEmisor { get; set; }
        public string razonSocialEmisor { get; set; }
        public string municipio { get; set; }
        public string telefono { get; set; }
        public int numeroFactura { get; set; }
        public string cuf { get; set; }
        public string cufd { get; set; }
        public int codigoSucursal { get; set; }
        public string direccion { get; set; }
        public int codigoPuntoVenta { get; set; }
        public DateTime fechaEmision { get; set; }
        public string nombreRazonSocial { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public string numeroDocumento { get; set; }
        public string complemento { get; set; }
        public string codigoCliente { get; set; }
        public int codigoMetodoPago { get; set; }
        public long numeroTarjeta { get; set; }
        public decimal montoTotalArrendamientoFinanciero { get; set; }
        public decimal montoTotal { get; set; }
        public decimal montoTotalSujetoIva { get; set; }
        public int codigoMoneda { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal montoTotalMoneda { get; set; }
        public decimal descuentoAdicional { get; set; }
        public int codigoExcepcion { get; set; }
        public string cafc { get; set; }
        public string leyenda { get; set; }
        public string usuario { get; set; }
        public decimal tipoCambioOficial { get; set; }

        public int codigoDocumentoSector { get; set; }
        /// DATOS SOLO PARA IMPRESION 
        public string sucursalCasaMatriz { get; set; }
        public string puntoVenta { get; set; }
        public string montoliteral { get; set; }
        public string moneda { get; set; }

        /// 
        public List<EntidadFinancieraDetalleDTO> detalle { get; set; } = new List<EntidadFinancieraDetalleDTO>();
    }

    public class EntidadFinancieraDetalleDTO
    {
        public string actividadEconomica { get; set; }
        public int codigoProductoSin { get; set; }
        public string codigoProducto { get; set; }
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public string unidadMedida { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal montoDescuento { get; set; }
        public decimal subTotal { get; set; }

    }
}
