using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class BatchChargesTabPresenter
    {
        private readonly IBatchChargesTabView view;
        private readonly IBatchRepository repository;

        public BatchChargesTabPresenter(IBatchChargesTabView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }

        public static BatchChargesTabPresenter Create(IBatchChargesTabView view)
        {
            return new BatchChargesTabPresenter(view, RepositoryFactory.CreateBatchRepository());
        }

        public void LoadBatchChargesFor(int batchId)
        {
            IList<Charge> chargeList = new List<Charge>();
            ChargeCollection charges = repository.LoadBatchCharges(batchId);
            if (charges != null)
            {
                chargeList = charges.GetList();
            }

            view.DisplayBatchCharges(chargeList);
        }
    }
}