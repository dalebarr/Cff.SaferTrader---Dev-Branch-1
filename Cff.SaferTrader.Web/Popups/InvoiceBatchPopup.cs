using System.Web.UI;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.Popups
{
    public abstract class InvoiceBatchPopup : Page
    {
        internal void SetTitle(string tabName, InvoiceBatch invoiceBatch)
        {
            Title = string.Format("{0} - Batch: {1} {2}", tabName, invoiceBatch.Number, invoiceBatch.ClientName);
        }
    }

   
}