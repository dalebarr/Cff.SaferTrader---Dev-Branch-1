using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public interface IBatchTab
    {
        void LoadTab(InvoiceBatch invoiceBatch);
        void ClearTabData();
    }
}