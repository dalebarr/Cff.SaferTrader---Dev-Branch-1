using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Views.TransactionReportView
{
    public interface IAccountTransactionsView : IReportView
    {
        int ClientId();
        FacilityType FacilityType();
        bool IsSalvageIncluded();

        Date EndDate();
        void Clear();
        void ShowAllClientsView();
        void ShowClientView();
        void DisplayReport(AccountTransactionReportBase report);
    }
}
