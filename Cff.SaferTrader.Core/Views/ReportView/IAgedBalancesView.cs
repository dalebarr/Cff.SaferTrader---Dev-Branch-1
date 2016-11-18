using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface IAgedBalancesView : IReportView
    {
        Scope CurrentScope();
        Date DateAsAt();
        int ClientId();
        AgedBalancesReportType ReportType();
        FacilityType FacilityType();
        bool IsSalvageIncluded();
        void DisplayReport(AgedBalancesReport report);
        void Clear();
        void ShowAllClientsView();
        void ShowClientView();
        int ClientFacilityType();
    }
}