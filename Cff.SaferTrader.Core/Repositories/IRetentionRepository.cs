using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface IRetentionRepository
    {
        IList<RetentionSchedule> LoadRetentionSchedules(int clientId, DateRange dateRange);
        ChargeCollection LoadCharges(int retentionId);
        RetentionDetails LoadRetentionDetails(int retentionId);
        IList<ClaimedCredit> LoadCreditsClaimed(int retentionItemDate, int clientId);
        IList<ClaimedRetentionRepurchase> LoadClaimedRetentionRepurchase(int retentionItemDate, int clientId);
        IList<RetentionSchedule> LoadRetentionSchedulesForAllClients(Date date);
    }
}