namespace InvoiceService.Models
{
    public class RequestReenvioFactura
    {
        public int idInvoice { get; set; }
        public string mail { get; set; }
        public string razonSocial { get; set; }
        public string operacion { get; set; }
    }
}
