using Invoice.Core.Business.InvoiceKind.BasicExpenses;
using Invoice.Core.Business.InvoiceKind.Financial;
using Invoice.Core.Business.InvoiceKind.Orders;
using Invoice.Core.Business.Office;
using Invoice.Core.Domain.Contracts;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoice.Core.InvoiceKind.BasicExpenses;
using BasicExpenses = Invoice.Core.Business.InvoiceKind.BasicExpenses;
using Financial = Invoice.Core.Business.InvoiceKind.Financial;
using Orders = Invoice.Core.Business.InvoiceKind.Orders;
using Exchange = Invoice.Core.Business.InvoiceKind.CurrencyExchange;

using Invoice.Core.InvoiceKind.Orders;
using Invoice.Core.Business.InvoiceKind.CurrencyExchange;

namespace Invoice.Core.Engines
{
    public class SetupAdaptersInvoice(ContextCompany contextCompany)
    {
        Dictionary<Type, IAdapterInvoice> adaptersConfigurations = new Dictionary<Type, IAdapterInvoice>();
        public  Dictionary<Type, IAdapterInvoice> AdaptersConfigurations
        {
            get
            {
                if (adaptersConfigurations.Count == 0)
                {
                    BasicExpenses.AdapterInvoice adapterInvoiceBasicService = new BasicExpenses.AdapterInvoice(contextCompany);
                    adaptersConfigurations.Add(typeof(RequestServicioBasico), adapterInvoiceBasicService);

                    Financial.AdapterInvoice adapterInvoiceFinancial = new Financial.AdapterInvoice(contextCompany);
                    adaptersConfigurations.Add(typeof(RequestEntidadFinanciera), adapterInvoiceFinancial);

                    Orders.AdapterInvoice adapterInvoiceOders = new Orders.AdapterInvoice(contextCompany);
                    adaptersConfigurations.Add(typeof(RequestCompraVenta), adapterInvoiceOders);

                    Exchange.AdapterInvoice adapterInvoiceExchange = new Exchange.AdapterInvoice(contextCompany);
                    adaptersConfigurations.Add(typeof(RequestCambioMoneda), adapterInvoiceExchange);
                }
                return adaptersConfigurations;
            }
        }
    }
}
