using System;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Services
{
    public class TransactionReportsService : ITransactionReportsService
    {
        private readonly IReportRepository repository;
        private readonly ITransactionReportView view;

        public TransactionReportsService(ITransactionReportView view, IReportRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }
        public TransactionReportsService(IReportRepository repository)
        {
            ArgumentChecker.ThrowIfNull(repository, "repository");
            this.repository = repository;
        }

        public static TransactionReportsService Create()
        {
            return new TransactionReportsService(RepositoryFactory.CreateReportRepository());
        }
        public static TransactionReportsService Create(ITransactionReportView view)
        {
            return new TransactionReportsService(view, RepositoryFactory.CreateReportRepository());
        }

        public TransactionReportBase LoadCreditNotesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadCreditNotesReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadJournalsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadJournalsReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadReceiptsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadReceiptsReport(endDate, clientId, ClientFacilityType);
        }

        public ReportBase LoadReceiptReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            return repository.LoadReceiptReportForCustomer(dateRange, customerId);
        }

        public TransactionReportBase LoadInvoicesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadInvoicesReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadInvoicesReport(Date endDate, int clientId, string transactionFilter, int ClientFacilityType)

        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadInvoicesReport(endDate, clientId, transactionFilter, ClientFacilityType);
        }

        public TransactionReportBase LoadDiscountsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadDiscountsReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadRepurchasesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadRepurchasesReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadCreditBalanceTransfersReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadCreditBalanceTransfersReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadUnclaimedCreditNotesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            return repository.LoadUnclaimedCreditNotesReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadInvoicesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType, string TransactionFilter)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            return repository.LoadInvoicesReportForAllClients(endDate, facilityType, isSalvageIncluded, ClientFacilityType, SessionWrapper.Instance.Get.SelectedTransactionFilter);
        }

        public TransactionReportBase LoadUnallocatedReportForAllClients(FacilityType facilityType, bool isSalvageIncluded)
        {
            return repository.LoadUnallocatedReportForAllClients(facilityType,isSalvageIncluded);
        }

        public TransactionReportBase LoadUnallocatedReport(int clientId, int ClientFacilityType)
        {
            return repository.LoadUnallocatedReport(clientId,ClientFacilityType);
        }

        public TransactionReportBase LoadOverpaymentsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            return repository.LoadOverpaymentsReportForAllClients(endDate, facilityType, isSalvageIncluded);
        }

        public TransactionReportBase LoadOverpaymentsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            return repository.LoadOverpaymentsReport(endDate, clientId,ClientFacilityType);
        }

        public TransactionReportBase LoadCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            return repository.LoadCreditNotesReportForAllClients(endDate, facilityType, isSalvageIncluded);
        }

        public ReportBase LoadCreditNotesReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            return repository.LoadCreditNotesReportForCustomer(dateRange, customerId);
        }

        public TransactionReportBase LoadReceiptsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            return (repository.LoadReceiptsReportForAllClients(endDate, facilityType, isSalvageIncluded, ClientFacilityType));
        }

        public TransactionReportBase LoadCreditBalanceTransfersReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            return repository.LoadCreditBalanceTransfersReportForAllClients(endDate, facilityType, isSalvageIncluded);
        }

        public TransactionReportBase LoadJournalsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            return repository.LoadJournalsReportForAllClients(endDate, facilityType, isSalvageIncluded,ClientFacilityType);

        }

        public TransactionReportBase LoadDiscountsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
           return repository.LoadDiscountsReportForAllClients(endDate, facilityType, isSalvageIncluded, ClientFacilityType);
        }

        public TransactionReportBase LoadRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            if (!FacilityType.FactoringOrDiscountingTypes.ContainsKey(facilityType.Id))
            {
                throw new InvalidOperationException("Invalid facility type");
            }
            return repository.LoadRepurchasesReportForAllClients(endDate, facilityType, isSalvageIncluded, ClientFacilityType);
        }

        public TransactionReportBase LoadUnclaimedRepurchasesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadUnclaimedRepurchasesReport(endDate, clientId, ClientFacilityType);
        }

        public TransactionReportBase LoadUnclaimedRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            if (!FacilityType.FactoringOrDiscountingTypes.ContainsKey(facilityType.Id))
            {
                throw new InvalidOperationException("Invalid facility type");
            }
            return repository.LoadUnclaimedRepurchasesReportForAllClients(endDate, facilityType, isSalvageIncluded,ClientFacilityType);
        }

        public TransactionReportBase LoadUnclaimedCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            if (!FacilityType.FactoringOrDiscountingTypes.ContainsKey(facilityType.Id))
            {
                throw new InvalidOperationException("Invalid facility type");
            }
            return repository.LoadUnclaimedCreditNotesReportForAllClients(endDate, facilityType, isSalvageIncluded,ClientFacilityType);
        }

        public AccountTransactionReportBase LoadAccountTransactionReportForAllClients(int coderef, Date endDate, FacilityType facilityType, bool isSalvageIncluded)
        {
            if (!FacilityType.FactoringOrDiscountingTypes.ContainsKey(facilityType.Id))
            {
                throw new InvalidOperationException("Invalid facility type");
            }
            return repository.LoadAccountTransactionReportForAllClients(coderef, endDate, facilityType, isSalvageIncluded);
        }

        public AccountTransactionReportBase LoadAccountTransactionReport(int coderef, Date endDate, int clientId)
        {
            ArgumentChecker.ThrowIfNull(endDate, "dateRange");
            ArgumentChecker.ThrowIfNull(clientId, "clientId");
            return repository.LoadAccountTransactionReport(coderef, endDate, clientId);
        }

        public ReportBase LoadInvoicesReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            return repository.LoadInvoicesReportForCustomer(dateRange, customerId);
        }

        public void InitializeForScope(Scope scope)
        {
            if (scope == Scope.AllClientsScope)
            {
                view.ShowAllClientsView();
            }
            else if (scope == Scope.ClientScope)
            {
                view.ShowClientView();
            }
            else
            {
                view.ShowCustomerView();
            }
        }

        public ReportBase LoadJournalsReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            return repository.LoadJournalsReportForCustomer(dateRange, customerId);
        }

        public ReportBase LoadCreditBalanceTransfersReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
           return repository.LoadCreditBalanceTransfersReportForCustomer(dateRange, customerId);
        }

        public ReportBase LoadDiscountsReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            return repository.LoadDiscountsReportForCustomer(dateRange, customerId);
        }

        public ReportBase LoadOverpaymentsReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            return repository.LoadOverpaymentsReportForCustomer(dateRange, customerId);
        }
    }
}