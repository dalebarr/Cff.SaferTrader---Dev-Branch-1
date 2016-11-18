using System;
using System.Linq;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class StatusReport : ReportBase
    {
        private readonly IList<StatusReportRecord> statusReportRecords;
        private decimal fundedTotal;
        private decimal nonFundedTotal;
        private readonly string title;

        public StatusReport(ICalendar calendar, string title, string clientName, IList<StatusReportRecord> statusReportRecords, TransactionStatus status)
            : base(calendar, "Status Report - " + status.Status, clientName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(statusReportRecords, "statusReportRecords");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.statusReportRecords = statusReportRecords;
            this.title = title;

            CalculateTotals();
        }

        public void CalculateTotals()
        {
            fundedTotal = 0;
            nonFundedTotal = 0;

            foreach (StatusReportRecord record in statusReportRecords)
            {
                if (record.TransactionStatus == TransactionStatus.Funded)
                {
                    fundedTotal += record.Amount;
                }
                else
                {
                    nonFundedTotal += record.Balance;
                }
            }
        }

        public IList<StatusReportRecord> Records
        {
            get { return statusReportRecords; }
        }

        public decimal TotalFundedInvoices
        {
            get { return fundedTotal; }
        }

        public decimal TotalNonFundedInvoices
        {
            get { return nonFundedTotal; }
        }

        public string Title
        {
            get { return title; }
        }
    }
}