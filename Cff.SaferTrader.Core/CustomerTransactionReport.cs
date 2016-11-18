using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CustomerTransactionReport : ReportBase
    {
        private readonly string title;
        private readonly IList<CustomerTransactionReportRecord> records;

        public CustomerTransactionReport(ICalendar calendar, string title, string customerName, IList<CustomerTransactionReportRecord> records, string name)
            :base(calendar, name, customerName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.title = title;
            this.records = records;
        }

        public IList<CustomerTransactionReportRecord> Records
        {
            get { return records; }
        }

        public string Title
        {
            get { return title; }
        }
    }
}