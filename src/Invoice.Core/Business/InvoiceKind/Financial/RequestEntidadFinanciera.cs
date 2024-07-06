using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceKind.Financial
{
    public class RequestEntidadFinanciera : IFacturable
    {
        public string municipio { get; set; }
        public string nombreRazonSocial { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public string numeroDocumento { get; set; }
        public string complemento { get; set; }
        public string codigoCliente { get; set; }
        public string mail { get; set; }

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
        public decimal tipoCambioOficial { get; set; }
        public int numeroFactura { get; set; }

        public List<RequestEntidadFinancieraDetalle> detalle { get; set; } = new List<RequestEntidadFinancieraDetalle>();
    }

    public class RequestEntidadFinancieraDetalle
    {
        public string codigoProducto { get; set; }
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public string unidadMedida { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal montoDescuento { get; set; }
        public decimal subTotal { get; set; }

    }
}
