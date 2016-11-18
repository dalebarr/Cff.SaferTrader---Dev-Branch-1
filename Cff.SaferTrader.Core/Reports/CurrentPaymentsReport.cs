using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CurrentPaymentsReport : TransactionReportBase
    {
        private readonly string title;
        private readonly IList<CurrentPaymentsReportRecord> records;
        private readonly decimal fundedTotal;
        private readonly decimal nonFundedTotal;

        public CurrentPaymentsReport(ICalendar calendar, string title, string clientName, IList<CurrentPaymentsReportRecord> records, string name)
            : base(calendar, name, clientName)
        {
            this.title = title;
            this.records = records;

            foreach (CurrentPaymentsReportRecord record in records)
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
        }

        public IList<CurrentPaymentsReportRecord> Records
        {
            get { return records; }
        }

        public string Title
        {
            get { return title; }
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