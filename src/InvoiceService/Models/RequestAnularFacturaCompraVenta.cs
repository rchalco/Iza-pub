using Invoice.Core.Business.InvoiceKind.Orders;

namespace InvoiceService.Models
{
    public class RequestAnularFacturaCompraVenta
    {
        public int IdInvoice { get; set; }
        public int Motivo { get; set; }
    }
}
