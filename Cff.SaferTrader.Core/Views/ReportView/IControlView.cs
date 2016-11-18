using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface IControlView : IReportView
    {
        Date EndDate();
        int ClientId();
        void DisplayReport(ControlReport report);
        void Clear();
        bool IsSalvageIncluded();
        FacilityType FacilityType();
        void ShowAllClientsView();
        void ShowClientView();
    }
}