using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class ClientActionReport : ReportBase
    {
        private readonly string title;
        private readonly string orderByString;
        private readonly IList<ClientActionReportRecord> records;
        public ClientActionReport(ICalendar calendar, string name, string clientName, IList<ClientActionReportRecord> records, string title, string orderByString) : base(calendar, name, clientName)
        {
            this.title = title;
            this.orderByString = orderByString;
            this.records = records;
        }

        public string OrderByString
        {
            get { return orderByString; }
        }

        public IList<ClientActionReportRecord> Records
        {
            get { return OrderByHelper.Sort(records, OrderByString); }
        }

        public string Title
        {
            get { return title; }
        }
    }
}