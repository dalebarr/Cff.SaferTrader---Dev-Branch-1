using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Views
{
    public interface IPromptReportView : IReportView
    {
        void DisplayDefaultPromptDays(int promptDays);
        void ShowReport(ReportBase report);
    }
}
