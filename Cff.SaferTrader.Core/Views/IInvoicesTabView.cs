using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface IInvoicesTabView
    {
        void DisplayInvoices(IList<Invoice> invoices);
    }
}