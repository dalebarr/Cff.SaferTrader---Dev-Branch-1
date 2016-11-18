using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CreditLimitExceededReport : ReportBase
    {
        private readonly string title;
        private readonly IList<CreditLimitExceededReportRecord> records;

        public CreditLimitExceededReport(ICalendar calendar, string title, string clientName, IList<CreditLimitExceededReportRecord> records) 
            : base(calendar, "Credit Limit Exceeded", clientName)
        {
            this.title = title;
            this.records = records;
        }

        public IList<CreditLimitExceededReportRecord> Records
        {
            get { return records; }
        }

        public string Title
        {
            get { return title; }
        }
    }
}
