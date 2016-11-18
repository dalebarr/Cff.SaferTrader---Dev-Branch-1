using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableOverdueCharges : IPrintable
    {
        private readonly RetentionSchedule retnSched;
        private readonly IList<Reports.OverdueChargesReportRecord> ODCReports;
        private readonly string viewID;

        public PrintableOverdueCharges(Reports.OverdueChargesReport overduechargesreport, RetentionSchedule retnSchedule, string viewIDValue)
        {
            this.ODCReports = overduechargesreport.Records;
            this.retnSched = retnSchedule;
            this.viewID = viewIDValue;
        }

        public IList<Reports.OverdueChargesReportRecord> ODCReportRecords
        {
            get { return ODCReports; }
        }

        public RetentionSchedule rtnSchedule
        {
            get { return retnSched; }
        }

        public string PopupPageName
         { //CFF-13
             get { return "OverdueChargesPopup.aspx?ViewID=" + this.viewID; }
         }
    }
}
