using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface IOverdueChargesView : IReportView, IRedirectableView
    {
        Date EndDate();
        int ClientId();
        void DisplayReport(OverdueChargesReport report);
        void Clear();
        bool IsSalvageIncluded();
        TransactionStatus TransactionStatus();
        FacilityType FacilityType();
        void ShowAllClientsView();
        void ShowClientView();
        void ShowCustomerView();
        int ClientFacilityType();
    }
}