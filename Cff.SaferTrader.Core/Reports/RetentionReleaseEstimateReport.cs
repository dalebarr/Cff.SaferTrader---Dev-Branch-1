using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class RetentionReleaseEstimateReport : ReportBase
    {
        private readonly IList<RetentionReleaseEstimateReportRecord> records;
        private readonly ReleaseSummary releaseSummary;
        private readonly Deductables deductables;
        private readonly string title;

        public RetentionReleaseEstimateReport(ICalendar calendar, string title, string clientName, IList<RetentionReleaseEstimateReportRecord> records, ReleaseSummary releaseSummary, Deductables deductables) : base(calendar, "Retention Release Estimates", clientName)
        {
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNull(releaseSummary, "releaseSummary");
            ArgumentChecker.ThrowIfNull(deductables, "deductables");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.records = records;
            this.title = title;
            this.releaseSummary = releaseSummary;
            this.deductables = deductables;
        }

        #region IReport Members

        public Deductables Deductables
        {
            get { return deductables; }
        }

        public ReleaseSummary ReleaseSummary
        {
            get { return releaseSummary; }
        }

        public IList<RetentionReleaseEstimateReportRecord> Records
        {
            get { return records; }
        }

        public decimal EstimatedRelease
        {
            get { return releaseSummary.Total - deductables.Total; }
        }

        public string Title
        {
            get {
                return title;
            }
        }

        #endregion
    }
}