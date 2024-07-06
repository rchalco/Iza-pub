using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoice.Core.Business.InvoiceKind.Orders
{
    public class RequestCompraVenta : IFacturable
    {
        public string municipio { get; set; }
        public int numeroFactura { get; set; }
        public string numeroDocumento { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public string nombreRazonSocial { get; set; }
        [XmlElement(IsNullable = true)]
        public string complemento { get; set; }
        public string codigoCliente { get; set; }
        public string mail { get; set; }
        public int codigoMetodoPago { get; set; }
        public long numeroTarjeta { get; set; }
        public decimal montoTotal { get; set; }
        public decimal montoTotalSujetoIva { get; set; }
        public decimal montoTotalMoneda { get; set; }
        public decimal montoGiftCard { get; set; }
        public decimal descuentoAdicional { get; set; }
        public int codigoExcepcion { get; set; }
        public List<RequestCompraVentaDetalle> detalle { get; set; } = new List<RequestCompraVentaDetalle>();
    }

    public class RequestCompraVentaDetalle
    {
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal montoDescuento { get; set; }
        public decimal subTotal { get; set; }
        public string numeroSerie { get; set; }
        public string numeroImei { get; set; }

    }
}
