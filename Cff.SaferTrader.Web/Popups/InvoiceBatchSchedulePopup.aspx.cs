using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class InvoiceBatchSchedulePopup : InvoiceBatchPopup, IScheduleTabView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableInvoiceBatchSchedule;

            if (printable != null)
            {
                SetTitle("Schedule", printable.InvoiceBatch);
                header.DisplayHeader(printable.InvoiceBatch);
                printable.BatchSchedule.Display(this);
            }
        }

        public void DisplaySchedule(BatchSchedule batchSchedule)
        {
            batchSchedulePanel.DisplaySchedule(batchSchedule);
        }

        public void ShowScheduleIsInProcessing(BatchSchedule batchSchedule)
        {
            batchSchedulePanel.ShowScheduleIsInProcessing(batchSchedule);
        }

        public void DisplayScheduleSummary(BatchScheduleFinanceInfo scheduleFinanceInfo)
        {
            batchSchedulePanel.DisplayScheduleSummary(scheduleFinanceInfo);
        }

        public void HideNote()
        {
            batchSchedulePanel.HideNote();
        }

        public void ShowCheckOrConfirmRow()
        {
            batchSchedulePanel.ShowCheckOrConfirmRow();
        }

        public void HideCheckOrConfirmRow()
        {
            batchSchedulePanel.HideCheckOrConfirmRow();
        }

        public void ShowNote()
        {
            batchSchedulePanel.ShowNote();
        }
    }
}