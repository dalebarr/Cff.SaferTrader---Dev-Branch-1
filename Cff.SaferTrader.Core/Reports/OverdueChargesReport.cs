using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class OverdueChargesReport : ReportBase
    {
        private readonly string title;
        private readonly IList<OverdueChargesReportRecord> overdueChargesRecords;
        
        public OverdueChargesReport(ICalendar calender, string title, string clientName, IList<OverdueChargesReportRecord> overdueChargesRecords, TransactionStatus status, int ClientFacilityType) : 
            base(calender, "Interest & Charges - " + status.Status, clientName)
        {
            ArgumentChecker.ThrowIfNull(overdueChargesRecords, "overdueChargesRecords");
            ArgumentChecker.ThrowIfNull(calender, "calender");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.title = title;
            this.overdueChargesRecords = overdueChargesRecords;
        }

        public IList<OverdueChargesReportRecord> Records
        {
            get { return overdueChargesRecords; }
        }

        public string Title
        {
            get { return title; }
        }
    }
}