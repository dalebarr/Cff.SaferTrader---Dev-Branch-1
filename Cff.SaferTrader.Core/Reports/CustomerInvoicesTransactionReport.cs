using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CustomerInvoicesTransactionReport : ReportBase
    {
        private readonly IList<CustomerInvoicesTransactionReportRecord> records;
        private readonly string title;

        public CustomerInvoicesTransactionReport(ICalendar calendar, string title, string customertName, 
            IList<CustomerInvoicesTransactionReportRecord> records) 
            : base(calendar, "Invoices", customertName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.records = records;
            this.title = title;
        }

        public IList<CustomerInvoicesTransactionReportRecord> Records
        {
            get { return records; }
        }

        public string Title
        {
            get { return title;}
        }
    }
}