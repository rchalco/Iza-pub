using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoice.Core.Business.InvoiceKind.Orders
{
    [Serializable, XmlRoot("cabecera")]
    public class HeaderVentaCompra: IHeaderFacturable
    {
        public long nitEmisor { get; set; }
        public string razonSocialEmisor { get; set; }
        public string municipio { get; set; }
        public string telefono { get; set; }
        public int numeroFactura { get; set; }
        public string cuf { get; set; }
        public string cufd { get; set; }
        public int codigoSucursal { get; set; }
        public string direccion { get; set; }
        [XmlElement(IsNullable = true)]
        public int? codigoPuntoVenta { get; set; }
        public string fechaEmision { get; set; }
        public string nombreRazonSocial { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public string numeroDocumento { get; set; }        
        [XmlElement(IsNullable = true)]
        public string complemento { get; set; }
        public string codigoCliente { get; set; }
        public int codigoMetodoPago { get; set; }
        [XmlElement(IsNullable = true)]
        public long? numeroTarjeta { get; set; }
        public decimal montoTotal { get; set; }
        public decimal montoTotalSujetoIva { get; set; }
        public int codigoMoneda { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal montoTotalMoneda { get; set; }
        [XmlElement(IsNullable = true)]
        public decimal? montoGiftCard { get; set; }        
        public decimal descuentoAdicional { get; set; }
        [XmlElement(IsNullable = true)]
        public int? codigoExcepcion { get; set; }
        [XmlElement(IsNullable = true)]
        public string? cafc { get; set; }
        public string leyenda { get; set; }
        public string usuario { get; set; }
        public int codigoDocumentoSector { get; set; }
    }
}
