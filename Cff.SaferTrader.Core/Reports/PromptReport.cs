 using System;
 using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class PromptReport: ReportBase
    {
        private readonly string title;
        private readonly IList<PromptReportRecord> records;
        private decimal fundedBalanceTotal;
        private decimal nonFundedBalanceTotal;

        public PromptReport(ICalendar calendar, string title, string clientName, IList<PromptReportRecord> promptReportRecords)
            : this(calendar, title, clientName, promptReportRecords, "Prompt Report")
        {
        }

        public PromptReport(ICalendar calendar, string title, string clientName, IList<PromptReportRecord> promptReportRecords, string name) 
            : base(calendar, name, clientName)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(promptReportRecords, "promptReportRecords");
            ArgumentChecker.ThrowIfNullOrEmpty(name, "name");

            records = promptReportRecords;
            this.title = title;

            CalculateTotals();
        }

        public void CalculateTotals()
        {
            fundedBalanceTotal = 0;
            nonFundedBalanceTotal = 0;
            
            foreach(PromptReportRecord record in records)
            {
                if(record.TransactionStatus == TransactionStatus.Funded)
                {
                    fundedBalanceTotal += record.Balance;
                }
                else
                {
                    nonFundedBalanceTotal += record.Balance;
                }
            }
        }

        public string Title
        {
            get { return title; }
        }

        public decimal NonFundedBalancedTotal
        {
            get { return nonFundedBalanceTotal; }
        }

        public decimal FundedBalanceTotal
        {
            get{ return fundedBalanceTotal; }
        }

        public IList<PromptReportRecord> Records
        {
            get { return records; }
        }




    }
}