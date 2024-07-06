using Invoice.Core.Domain.Contracts;
using Invoice.Core.InvoiceKind.BasicExpenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceKind.BasicExpenses
{
    public class RequestServicioBasico : IFacturable
    {
        public int numeroFactura { get; set; }
        public string mes { get; set; }
        public int gestion { get; set; }
        public string ciudad { get; set; }
        public string municipio { get; set; }
        public string zona { get; set; }
        public string numeroMedidor { get; set; }
        public string nombreRazonSocial { get; set; }
        public string domicilioCliente { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public string numeroDocumento { get; set; }
        public string complemento { get; set; }
        public string codigoCliente { get; set; }
        public string mail { get; set; }
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
        public decimal montoTotalMoneda { get; set; }
        public decimal descuentoAdicional { get; set; }
        public int codigoExcepcion { get; set; }
        public List<RequestServicioBasicoDetalle> detalle { get; set; } = new List<RequestServicioBasicoDetalle>();
    }
    public class RequestServicioBasicoDetalle
    {
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal montoDescuento { get; set; }
        public decimal subTotal { get; set; }

    }
}
