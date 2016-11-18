using System;
using System.Collections.Generic;
using System.Linq;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CallsDueReport : ReportBase
    {
        private readonly string title;
        private readonly IList<CallsDueReportRecord> records;
        private readonly string orderByString;

        public CallsDueReport(ICalendar calendar, string title, string clientName, IList<CallsDueReportRecord> records, string name, string orderByString) : base(calendar, name, clientName)
        {
            this.title = title;
            this.orderByString = orderByString;
            this.records = records;
        }

        public string OrderByString
        {
            get { return orderByString; }
        }

        public IList<CallsDueReportRecord> Records
        {
            get { return OrderByHelper.Sort(records, OrderByString); }
        }

        public string Title
        {
            get { return title; }
        }
    }
}