namespace Cff.SaferTrader.Core.Repositories
{
    public interface IRetentionNotesTabRepository
    {
        IRetentionNote LoadRetentionNotesFor(int retentionScheduleId);
    }
}