using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class TransactionReport : TransactionReportBase
    {
        private readonly decimal fundedTotal;
        private readonly decimal nonFundedTotal;
        private readonly IList<TransactionReportRecord> records;
        private readonly string title;

        public TransactionReport(ICalendar calendar, string title, string clientName, IList<TransactionReportRecord> records, string name)
            : base(calendar, name, clientName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            foreach (TransactionReportRecord record in records)
            {
                if (record.IsFunded)
                {
                    fundedTotal += record.Amount;
                }
                else
                {
                    nonFundedTotal += record.Amount;
                }
            }
            this.title = title;
            this.records = records;
        }

        public string Title
        {
            get { return title; }
        }

        public IList<TransactionReportRecord> Records
        {
            get { return records; }
        }

        public override decimal FundedTotal
        {
            get { return fundedTotal; }
        }

        public override decimal NonFundedTotal
        {
            get { return nonFundedTotal; }
        }
    }
}