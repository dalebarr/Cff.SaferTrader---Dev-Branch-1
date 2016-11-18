using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface ICurrentShortPaidView : IReportView
    {
        int ClientId();
        void Clear();
        bool IsSalvageIncluded();
        FacilityType FacilityType();

        void ShowAllClientsView();
        void ShowClientView();
        void DisplayReport(CurrentPaymentsReport report);
        int  ClientFacilityType();
    }
}