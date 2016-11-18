namespace Cff.SaferTrader.Core.Views
{
    public interface IScheduleTabView
    {
        void DisplaySchedule(BatchSchedule batchSchedule);
        void ShowScheduleIsInProcessing(BatchSchedule batchSchedule);
        void DisplayScheduleSummary(BatchScheduleFinanceInfo scheduleFinanceInfo);
        void HideNote();
        void ShowCheckOrConfirmRow();
        void HideCheckOrConfirmRow();
        void ShowNote();
    }
}