using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class StatementPresenter
    {
        private readonly IStatementView view;
        private readonly IReportRepository repository;
        private readonly IReportManager reportManager;

        public StatementPresenter(IStatementView view, IReportRepository repository, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            ArgumentChecker.ThrowIfNull(reportManager, "reportManager");

            this.view = view;
            this.repository = repository;
            this.reportManager = reportManager;
        }

        public void InitializeForScope()
        {
            if (reportManager.CanViewStatementReport())
            {
                if (view.Scope == Scope.CustomerScope)
                {
                    view.ShowCustomerView();
                }
                else
                {
                    view.DisplayReportNotAvailableError();
                }
            }
            else
            {
                view.DisplayAccessDeniedError();
            }
        }

        public void LoadStatementReport()
        {
            if (reportManager.CanViewStatementReport())
            {
                if (view.Scope == Scope.CustomerScope)
                {
                    view.ShowReport(repository.LoadStatementReport(view.ClientId, view.CustomerId, view.EndDate));
                }
                else
                {
                    view.DisplayReportNotAvailableError();
                }

            }
            else
            {
                view.DisplayAccessDeniedError();
            }
        }

        public IReportManager getReportMgr()
        {
            return reportManager;
        }

    }
}