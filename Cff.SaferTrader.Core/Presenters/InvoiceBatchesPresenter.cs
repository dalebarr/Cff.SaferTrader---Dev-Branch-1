using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class InvoiceBatchesPresenter
    {
        private readonly IBatchRepository repository;
        private readonly IInvoiceBatchesView view;

        public InvoiceBatchesPresenter(IInvoiceBatchesView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }

        public static InvoiceBatchesPresenter Create(IInvoiceBatchesView view)
        {
            return new InvoiceBatchesPresenter(view, RepositoryFactory.CreateBatchRepository());
        }

        public void startDataFetchEventHandler(object sender, EventArgs e)
        {
            view.StartDBFetchEventHandler(sender, e);
        }


        public void endDataFetchEventHandler(object sender, EventArgs e)
        {
            view.EndDBFetchEventHandler(sender, e);
        }
        
        public void LoadInvoiceBatchesForDateRange(int clientId, BatchType batchType, DateRange dateRange, bool isResetBatchNum = false)
        {
            ArgumentChecker.ThrowIfNull(batchType, "batchType");
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            if (!batchType.IsDateRangeDependant)
            {
                throw new InvalidOperationException("Batch type is not date range dependent");
            }
            
            (repository as BatchRepository).StartDataFetchEvent +=  startDataFetchEventHandler;
            (repository as BatchRepository).EndDataFetchEvent += endDataFetchEventHandler;
            IList<InvoiceBatch> invBatchesList = repository.LoadInvoiceBatchesForDateRange(clientId, batchType, dateRange);
            view.DisplayInvoiceBatches(invBatchesList, isResetBatchNum);
        }

        public void LoadInvoiceBatchesFor(int clientId, BatchType batchType)
        {
            ArgumentChecker.ThrowIfNull(batchType, "batchType");
            if (batchType.IsDateRangeDependant)
            {
                throw new InvalidOperationException("Batch type is date range dependent");
            }

            view.DisplayInvoiceBatches(repository.LoadInvoiceBatchesFor(clientId, batchType), false);
        }

        public void InitializeForScope(Scope scope)
        {
            view.PopulateBatchTypeDropDown();

            if (scope == Scope.AllClientsScope)
            {
                view.ShowAllClientsView();
            }
            else
            {
                view.ShowClientView();
            }
        }

        public void SearchInvoiceBatchesForBatchNumber(int clientId, string batchNumberToSearch)
        {
            ArgumentChecker.ThrowIfNull(batchNumberToSearch, "batchNumberToSearch");

            view.DisplayInvoiceBatches(repository.LoadInvoiceBatchesForBatchNumber(clientId, batchNumberToSearch), false);
        }

        public void SelectInvoiceBatchesForBatchNumber(int clientId, string batchNumber)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(batchNumber, "batchNumber");
            
            IList<InvoiceBatch> batches  = new  List<InvoiceBatch>();
            if (clientId < 0) {
                batches = repository.LoadInvoiceBatchesFor(clientId, BatchType.KnownTypes[10]);
            }
            else 
                batches = repository.LoadInvoiceBatchesForBatchNumber(clientId, batchNumber);
            view.DisplayInvoiceBatches(batches, false);

            if (batches.Count > 0)
            {
                 view.SelectFirstBatchRecord();
            }
        }
    }
}