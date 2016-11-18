using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Views
{
    public interface IStatementView : IReportView
    {
        void HideReportViewer();
        void ShowCustomerView();
        void ShowReport(StatementReport report);
        int ClientId { get; }
        int CustomerId { get; }
        Date EndDate { get; }
        Scope Scope { get; }
    }
}