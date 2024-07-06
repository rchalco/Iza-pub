using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceKind.Orders
{
    public class DetailCompraVenta : IDetailFacturable
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
    }
}
