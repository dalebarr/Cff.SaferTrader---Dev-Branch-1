using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public abstract class AccountTransactionReportBase: ReportBase
    {
         /// <summary>
        /// Creates a AccountTransactionReportBase
        /// </summary>
        /// <param name="calendar">ICalendar the report is keeps</param>
        /// <param name="name">Name of the report e.g. Account Transaction Report</param>
        /// <param name="companyName">Name of company the report is about - can be a client or a customer</param>
        protected AccountTransactionReportBase(ICalendar calendar, string name, string companyName)
            : base(calendar, name, companyName)
        {

        }

        public abstract decimal TotalCredit { get; }
        public abstract decimal TotalDebit  { get; }
        public abstract decimal Movement { get; }
        public abstract decimal ClosingBalance { get; }
        public abstract decimal OpeningBalance { get; }
    }
}
