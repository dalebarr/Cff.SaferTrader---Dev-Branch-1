using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class ChargesTabPresenter
    {
        private readonly IChargesTabView view;
        private readonly IRetentionRepository repository;

        private ChargesTabPresenter(IChargesTabView view, IRetentionRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }

        public static ChargesTabPresenter Create(IChargesTabView view)
        {
            return new ChargesTabPresenter(view, RepositoryFactory.CreateRetentionRepository());
        }

        public void LoadCharges(RetentionSchedule retentionSchedule)
        {
            IList<Charge> chargeList = new List<Charge>();
            if (retentionSchedule != null) {
                ChargeCollection charges = repository.LoadCharges(retentionSchedule.Id);
                if (charges != null)
                {
                    chargeList = charges.GetList();
                }
            }
            
            view.DisplayCharges(chargeList);
        }
    }
}