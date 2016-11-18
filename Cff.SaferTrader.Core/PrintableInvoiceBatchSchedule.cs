using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableInvoiceBatchSchedule : IPrintable
    {
        private readonly InvoiceBatch invoiceBatch;
        private readonly BatchSchedule batchSchedule;
        private readonly string viewID;

        public PrintableInvoiceBatchSchedule(InvoiceBatch invoiceBatch, BatchSchedule batchSchedule, string viewIDValue)
        {
            this.invoiceBatch = invoiceBatch;
            this.batchSchedule = batchSchedule;
            this.viewID = viewIDValue;
        }

        public InvoiceBatch InvoiceBatch
        {
            get { return invoiceBatch; }
        }

        public BatchSchedule BatchSchedule
        {
            get { return batchSchedule; }
        }

        public string PopupPageName
        {
            get { return "InvoiceBatchSchedulePopupPrint.aspx?ViewID=" + this.viewID; }
        }
    }
}