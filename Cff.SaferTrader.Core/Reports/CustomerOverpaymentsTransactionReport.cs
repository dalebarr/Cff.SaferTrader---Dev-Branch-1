using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CustomerOverpaymentsTransactionReport : ReportBase
    {
        private readonly IList<CustomerOverpaymentsTransactionReportRecord> records;
        private readonly string title;

        public CustomerOverpaymentsTransactionReport(ICalendar calendar, string title, string customertName,
            IList<CustomerOverpaymentsTransactionReportRecord> records) 
            : base(calendar, "Overpayments", customertName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.records = records;
            this.title = title;
        }

        public IList<CustomerOverpaymentsTransactionReportRecord> Records
        {
            get { return records; }
        }

        public string Title
        {
            get { return title;}
        }
    }
}
