using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class InvoiceBatchSchedulePopupPrint : InvoiceBatchPopup, IScheduleTabView
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
            batchSchedulePanelPrint.DisplaySchedule(batchSchedule);
        }

        public void ShowScheduleIsInProcessing(BatchSchedule batchSchedule)
        {
            batchSchedulePanelPrint.ShowScheduleIsInProcessing(batchSchedule);
        }

        public void DisplayScheduleSummary(BatchScheduleFinanceInfo scheduleFinanceInfo)
        {
            batchSchedulePanelPrint.DisplayScheduleSummary(scheduleFinanceInfo);
        }

        public void HideNote()
        {
            batchSchedulePanelPrint.HideNote();
        }

        public void ShowCheckOrConfirmRow()
        {
            batchSchedulePanelPrint.ShowCheckOrConfirmRow();
        }

        public void HideCheckOrConfirmRow()
        {
            batchSchedulePanelPrint.HideCheckOrConfirmRow();
        }

        public void ShowNote()
        {
            batchSchedulePanelPrint.ShowNote();
        }
    }
}