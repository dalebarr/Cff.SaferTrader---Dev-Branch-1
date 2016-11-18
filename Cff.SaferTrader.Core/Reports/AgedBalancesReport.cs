using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class AgedBalancesReport : ReportBase
    {
        private readonly IList<AgedBalancesReportRecord> records;
        private readonly string title;

        public AgedBalancesReport(ICalendar calendar, string title, IList<AgedBalancesReportRecord> records, AgedBalancesReportType type, string clientName, Date endDate)
            : base(calendar, "Aged Balances - " + type.Text, clientName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.records = records;
            this.title = title;
        }

        public IList<AgedBalancesReportRecord> Records
        {
            get { return records; }
        }

        public string Title
        {
            get 
            {
                return title;
            }
        }
    }
}