using Invoice.Core.DBEntities;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Domain.Contracts
{
    public interface IAdapterInvoice
    {
        IInvoice GetInvoice(IFacturable factura);
        (string fileName, string dataB64) GetReportInvoice(IInvoice invoice, string pathReports);
        Response SendFactura(DBEntities.Invoice invoice);
        Response AnularFactura(int idInvoice, int idMotivo = 1);
        Response SendBatchInvoices(List<DBEntities.Invoice> setInvoices, EventoSignificativo eventoSignificativo, string fileTGZ, string folderTGZ, string PATH_BATCHFILES);
        Response RevertirFactura(int idInvoice);

    }
}
