using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface IRetentionSchedulesView
    {
        void DisplayRetentionSchedules(IList<RetentionSchedule> retentionSchedules);
    }
}