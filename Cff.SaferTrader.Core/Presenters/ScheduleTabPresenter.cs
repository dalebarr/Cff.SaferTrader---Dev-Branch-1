using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class ScheduleTabPresenter
    {
        private readonly IScheduleTabView view;
        private readonly IBatchRepository repository;

        public ScheduleTabPresenter(IScheduleTabView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            this.view = view;
            this.repository = repository;
        }

        public static ScheduleTabPresenter Create(IScheduleTabView view)
        {
            return new ScheduleTabPresenter(view, RepositoryFactory.CreateBatchRepository());
        }

        public void LoadBatchScheduleFor(int clientId, int batchId)
        {
            ChargeCollection charges = repository.LoadBatchCharges(batchId);
            BatchSchedule batchSchedule = repository.LoadBatchScheduleFor(clientId, batchId, charges);

            if (batchSchedule != null)
            {
                batchSchedule.Display(view);
            }
        }
    }
}