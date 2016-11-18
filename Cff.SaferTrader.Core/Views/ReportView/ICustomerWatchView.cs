using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface ICustomerWatchView : IReportView
    {
        PeriodReportType ReportType();
        BalanceRange BalanceRange();
        FacilityType FacilityType();
        bool IsSalvageIncluded();
        string AllClientsOrderByString();
        void DisplayReport(CallsDueReport report);
        void Clear();
        int ClientId();
        void ShowAllClientsView();
        void ShowClientView();
        void HideReportPanel();
    }
}