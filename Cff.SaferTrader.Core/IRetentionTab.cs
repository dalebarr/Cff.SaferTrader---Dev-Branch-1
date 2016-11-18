using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public interface IRetentionTab
    {
        void LoadTab(RetentionSchedule retentionSchedule);
        void ClearTabData();
    }
}