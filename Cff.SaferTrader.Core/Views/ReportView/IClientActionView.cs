using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views.ReportView
{
    public interface IClientActionView : IReportView
    {
        void ShowAllClientsView();
        void ShowClientView();
        void DisplayReport(ClientActionReport report);
        void Clear();
        int ClientId();
        BalanceRange BalanceRange();
        FacilityType FacilityType();
        bool IsSalvageIncluded();
        string AllClientsOrderByString();
    }
}