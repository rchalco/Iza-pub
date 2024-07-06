using DevExpress.Xpo;
using Invoice.Core.Domain.Contracts;

namespace Invoice.Core.InvoiceKind.Orders
{
    public class ReportCompraVentaDTO : XPObject
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
        public string numeroDocumento { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public string nombreRazonSocial { get; set; }
        public string complemento { get; set; }
        public string codigoCliente { get; set; }
        public int codigoMetodoPago { get; set; }
        public long numeroTarjeta { get; set; }
        public decimal montoTotal { get; set; }
        public decimal montoTotalSujetoIva { get; set; }
        public int codigoMoneda { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal montoTotalMoneda { get; set; }
        public decimal montoGiftCard { get; set; }
        public decimal montoTasa { get; set; }
        public decimal descuentoAdicional { get; set; }
        public int codigoExcepcion { get; set; }
        public string cafc { get; set; }
        public string leyenda { get; set; }
        public string usuario { get; set; }
        public int codigoDocumentoSector { get; set; }

        /// DATOS SOLO PARA IMPRESION 
        public string sucursalCasaMatriz { get; set; }
        public string puntoVenta { get; set; }
        public string montoliteral { get; set; }
        public string QR { get; set; }

        /// 
        public List<CompraVentaDetalleDTO> detalle { get; set; } = new List<CompraVentaDetalleDTO>();
    }

    public class CompraVentaDetalleDTO
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
        public string numeroSerie { get; set; }
        public string numeroImei { get; set; }
        public string unidadMedidaDescripcion { get; set; }


    }



}
