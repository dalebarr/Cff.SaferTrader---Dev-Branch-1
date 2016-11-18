using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class TransactionHistoryPresenter
    {
        private readonly ITransactionHistoryView view;
        private readonly ITransactionRepository repository;
        private readonly IRedirectionService redirectionService;
        private readonly ISecurityManager securityManager;

        public TransactionHistoryPresenter(ITransactionHistoryView view, ITransactionRepository repository, IRedirectionService redirectionService, ISecurityManager securityManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            ArgumentChecker.ThrowIfNull(redirectionService, "redirectionService");

            this.view = view;
            this.repository = repository;
            this.redirectionService = redirectionService;
            this.securityManager = securityManager;
        }
        
        public void LoadTransactionHistory(DateRange dateRange, int customerId, bool bInvoicesOnly)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            view.ShowTransactionHistory(repository.LoadTransactionHistory(dateRange, customerId, bInvoicesOnly));
        }

        public IList<Transaction> LoadTransactionHistoryDetails(Date date, int customerId)
        {
            ArgumentChecker.ThrowIfNull(date, "date");
            return repository.LoadTransactionHistoryDetails(date, customerId);
        }

        public void LoadBatchDetailsAndRedirect(BatchRecord transaction)
        {
            ArgumentChecker.ThrowIfNull(transaction, "transaction");
            redirectionService.RedirectToInvoiceBatches(transaction.Batch);
        }

        public void InitializeForScope(Scope scope)
        {
            ArgumentChecker.ThrowIfNull(scope, "scope");

            if (scope == Scope.CustomerScope)
            {
                view.ShowCustomerView();
            }
            else if (scope == Scope.AllClientsScope)
            {
                view.ShowAllClientsView();
            }
            else
            {
                view.ShowClientView();
            }
        }

        public void LoadTransactionHistoryForAllClients(DateRange dateRange, FacilityType facilityType, bool isSalvageIncluded)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            view.ShowTransactionHistory(repository.LoadTransactionHistoryForAllClients(dateRange, facilityType, isSalvageIncluded));
        }

        public void LoadTransactionHistoryForClient(DateRange dateRange, int clientId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            view.ShowTransactionHistory(repository.LoadTransactionHistoryForClient(dateRange, clientId));   
        }

        public void LockDown()
        {
            if (!securityManager.CanViewTransactionHistoryLink())
            {
                redirectionService.RedirectToTransactionSearch(view.QueryStringParameters);
            }
        }
    }
}