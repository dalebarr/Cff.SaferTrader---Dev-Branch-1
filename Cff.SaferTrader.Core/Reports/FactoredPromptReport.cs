using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class FactoredPromptReport : PromptReport
    {
        public FactoredPromptReport(ICalendar calendar, string title, string clientName, IList<PromptReportRecord> promptReportRecords) : 
            base(calendar, title, clientName, promptReportRecords, "Prompt - Factored Invoices Only")
        {
        }
    }
}