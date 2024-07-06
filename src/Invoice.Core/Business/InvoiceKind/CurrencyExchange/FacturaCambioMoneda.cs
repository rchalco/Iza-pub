using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceKind.CurrencyExchange
{
    public class FacturaCambioMoneda : IInvoice
    {
        public IHeaderFacturable header { get; set; }
        public List<IDetailFacturable> detail { get; set; }

    }
}
