using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls.Interfaces;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class ScheduleTab : UserControl, IScheduleTabView, IBatchTab, IPrintableView
    {
        private ScheduleTabPresenter presenter;

        #region IBatchTab Members

        public void LoadTab(InvoiceBatch invoiceBatch)
        {
            ViewState.Add("InvoiceBatch", invoiceBatch);

            presenter = ScheduleTabPresenter.Create(this);
            presenter.LoadBatchScheduleFor(invoiceBatch.ClientId, invoiceBatch.Number);
        }

        public void ClearTabData()
        {
            batchSchedulePanel.Clear();
        }

        #endregion

        #region IScheduleTabView Members

        public void DisplaySchedule(BatchSchedule batchSchedule)
        {
            ViewState.Add("BatchSchedule", batchSchedule);

            batchSchedulePanel.DisplaySchedule(batchSchedule);
        }

        public void ShowScheduleIsInProcessing(BatchSchedule batchSchedule)
        {
            ViewState.Add("BatchSchedule", batchSchedule);
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

        #endregion


        #region IPrintableView Members
            public void Print()
            {
                PrintableInvoiceBatchSchedule printable = new PrintableInvoiceBatchSchedule(ViewState["InvoiceBatch"] as InvoiceBatch,
                                                                                            ViewState["BatchSchedule"] as BatchSchedule, QueryString.ViewIDValue);
                //string script = PopupHelper.ShowPopup(printable, Server);   // dbb
                string script = PopupHelper.ShowPopupReportType(printable, Server, false);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
        #endregion

    }
}