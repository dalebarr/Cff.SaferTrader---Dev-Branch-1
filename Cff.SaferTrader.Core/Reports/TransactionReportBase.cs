using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public abstract class TransactionReportBase :ReportBase
    {
        /// <summary>
        /// Creates a TransactionReportBase
        /// </summary>
        /// <param name="calendar">ICalendar the report is keeps</param>
        /// <param name="name">Name of the report e.g. Invoices</param>
        /// <param name="companyName">Name of company the report is about - can be a client or a customer</param>
        protected TransactionReportBase(ICalendar calendar, string name, string companyName)
            : base(calendar, name, companyName)
        {
        }

        public abstract decimal FundedTotal { get; }
        public abstract decimal NonFundedTotal { get; }
    }
}