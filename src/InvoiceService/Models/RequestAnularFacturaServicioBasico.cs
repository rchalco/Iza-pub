using Invoice.Core.Business.InvoiceKind.BasicExpenses;
using Invoice.Core.Business.InvoiceKind.Orders;

namespace InvoiceService.Models
{
    public class RequestAnularFacturaServicioBasico
    {
        public int IdInvoice { get; set; }
        public int Motivo { get; set; }
    }
}
