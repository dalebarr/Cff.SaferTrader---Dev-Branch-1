using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface IRetentionReleaseEstimateView : IReportView
    {
        int ClientId();
        void Clear();
        void HideReportPanel();
        void DisplayReport(RetentionReleaseEstimateReport report);

    }
}