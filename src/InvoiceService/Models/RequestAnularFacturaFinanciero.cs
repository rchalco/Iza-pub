using Invoice.Core.Business.InvoiceKind.Financial;

namespace InvoiceService.Models
{
    public class RequestAnularFacturaFinanciero
    {
        public int IdInvoice { get; set; }
        public int Motivo { get; set; }
    }
}
