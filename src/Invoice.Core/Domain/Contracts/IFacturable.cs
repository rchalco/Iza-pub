using Invoice.Core.InvoiceKind.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoice.Core.Domain.Contracts
{
    public interface IFacturable
    {
        public int numeroFactura { get; set; }
        public string mail { get; set; }
        public string numeroDocumento { get; set; }
        public string nombreRazonSocial { get; set; }
        public int codigoTipoDocumentoIdentidad { get; set; }
        public int codigoExcepcion { get; set; }

    }

    public interface IInvoice
    {
        public IHeaderFacturable header { get; set; }
        public List<IDetailFacturable> detail { get; set; }
    }

    public interface IHeaderFacturable
    {
        public string cuf { get; set; }
        public int numeroFactura { get; set; }
        public int codigoDocumentoSector { get; set; }
        public long nitEmisor { get; set; }
    }

    public interface IDetailFacturable
    {
    }
}
