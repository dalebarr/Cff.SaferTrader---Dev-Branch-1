using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class RetentionSchedulePresenter
    {
        private readonly IRetentionSchedulesView view;
        private readonly IRetentionRepository repository;

        public RetentionSchedulePresenter(IRetentionSchedulesView view, IRetentionRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }

        public static RetentionSchedulePresenter Create(IRetentionSchedulesView view)
        {
            return new RetentionSchedulePresenter(view, RepositoryFactory.CreateRetentionRepository());
        }

        public void LoadRetentionSchedules(int clientId, DateRange dateRange)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            view.DisplayRetentionSchedules(repository.LoadRetentionSchedules(clientId, dateRange));
        }

        public void LoadRetentionSchedulesForAllClients(Date date)
        {
            ArgumentChecker.ThrowIfNull(date, "date");
            view.DisplayRetentionSchedules(repository.LoadRetentionSchedulesForAllClients(date));
        }
    }
}