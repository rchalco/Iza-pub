using Invoice.Core.Domain.Contracts;

namespace Invoice.Core.InvoiceKind.BasicExpenses
{
    public class ReportServicioBasicoDTO
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
        public string mes { get; set; }
        public int gestion { get; set; }
        public string ciudad { get; set; }
        public string zona { get; set; }
        public string numeroMedidor { get; set; }
        public DateTime fechaEmision { get; set; }
        public string nombreRazonSocial { get; set; }
        public string domicilioCliente { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public string numeroDocumento { get; set; }
        public string complemento { get; set; }
        public string codigoCliente { get; set; }
        public int codigoMetodoPago { get; set; }
        public long numeroTarjeta { get; set; }
        public decimal montoTotal { get; set; }
        public decimal montoTotalSujetoIva { get; set; }
        public decimal consumoPeriodo { get; set; }
        public int beneficiarioLey1886 { get; set; }
        public decimal montoDescuentoLey1886 { get; set; }
        public decimal montoDescuentoTarifaDignidad { get; set; }
        public decimal tasaAseo { get; set; }
        public decimal tasaAlumbrado { get; set; }
        public decimal ajusteNoSujetoIva { get; set; }
        public string detalleAjusteNoSujetoIva { get; set; }
        public decimal ajusteSujetoIva { get; set; }
        public string detalleAjusteSujetoIva { get; set; }
        public decimal otrosPagosNoSujetoIva { get; set; }
        public string detalleOtrosPagosNoSujetoIva { get; set; }
        public decimal otrasTasas { get; set; }
        public int codigoMoneda { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal montoTotalMoneda { get; set; }
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

        /// 
        public List<ServicioBasicoDetalleDTO> detalle { get; set; } = new List<ServicioBasicoDetalleDTO>();
    }

    public class ServicioBasicoDetalleDTO
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
