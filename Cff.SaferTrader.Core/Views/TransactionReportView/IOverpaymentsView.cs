using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Views.TransactionReportView
{
    public interface IOverpaymentsView : IReportView
    {
        int ClientId();
        int CustomerId();
        bool IsSalvageIncluded();
        FacilityType FacilityType();
        DateRange DateRange();
        Date EndDate();
        void Clear();
        void ShowAllClientsView();
        void ShowClientView();
        void ShowCustomerView();
        void DisplayReport(TransactionReportBase report);
        void DisplayReportForCustomer(ReportBase report);
        int ClientFacilityType();        
    }
}