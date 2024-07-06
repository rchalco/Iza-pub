using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceKind.BasicExpenses
{
    public class FacturaServicioBasico : IInvoice
    {
        public IHeaderFacturable header { get; set; } = new HeaderServicioBasico();
        public List<IDetailFacturable> detail { get; set; } = new List<IDetailFacturable>();
    }
}
