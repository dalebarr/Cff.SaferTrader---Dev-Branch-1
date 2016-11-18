using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface IStatusView : IReportView
    {
        Date EndDate();
        int ClientId();
        CffCustomer Customer();
        void DisplayReport(StatusReport report);
        void Clear();
        bool IsSalvageIncluded();
        TransactionStatus transactionStatus();
        FacilityType FacilityType();
        void ShowAllClientsView();
        void ShowClientView();
        void ShowCustomerView();
        int ClientFacilityType();
    }
}