using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CreditStopSuggestionsReport : ReportBase
    {
        private readonly string title;
        private readonly IList<CreditStopSuggestionsReportRecord> records;

        public CreditStopSuggestionsReport(ICalendar calendar, string title, string clientName, IList<CreditStopSuggestionsReportRecord> records) 
            : base(calendar, "Credit Stop Suggestions", clientName)
        {
            this.title = title;
            this.records = records;
        }

        public IList<CreditStopSuggestionsReportRecord> Records
        {
            get { return records; }
        }

        public string Title
        {
            get { return title; }
        }
    }
}
