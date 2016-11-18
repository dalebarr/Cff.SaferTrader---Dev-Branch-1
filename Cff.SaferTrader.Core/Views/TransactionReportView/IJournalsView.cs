using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Views.TransactionReportView
{
    public interface IJournalsView : IReportView
    {
        bool IsSalvageIncluded();
        FacilityType FacilityType();
        DateRange DateRange();
        Date EndDate();
        void Clear();
        void ShowAllClientsView();
        void ShowClientView();
        void DisplayReport(TransactionReportBase report);
        void DisplayReportForCustomer(ReportBase report);
        void ShowCustomerView();
        int CustomerId();
        int ClientId();
        int ClientFacilityType();
    }
}