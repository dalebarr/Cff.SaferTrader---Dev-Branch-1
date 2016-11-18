using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Views.TransactionReportView
{
    public interface IRepurchasesView :IReportView
    {
        int ClientId();
        bool IsSalvageIncluded();
        FacilityType FacilityType();
        Date EndDate();
        void Clear();
        void ShowAllClientsView();
        void ShowClientView();
        void DisplayReport(TransactionReportBase report);
        int ClientFacilityType();
    }
}