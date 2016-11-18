using System;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Core.Presenters
{
    public class AccountTransactionTabPresenter
    {
        private readonly IReportRepository repository;
        private readonly IAccountTransactionsView view;

        public AccountTransactionTabPresenter(IAccountTransactionsView view, IReportRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            this.repository = repository;
            this.view = view;
        }

        public static AccountTransactionTabPresenter Create(IAccountTransactionsView view)
        {
            return new AccountTransactionTabPresenter(view, RepositoryFactory.CreateReportRepository());
        }

        public void LoadAccountTransactions(int coderef, Date endDate, int clientId)
        {
            view.DisplayReport(repository.LoadAccountTransactionReport(coderef, endDate, clientId));
        }

    }
}
