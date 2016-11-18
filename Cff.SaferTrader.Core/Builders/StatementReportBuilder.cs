using System.Collections.Generic;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Builders
{
    public class StatementReportBuilder
    {
        public StatementReport Build(ManagementDetails managementDetails, PurchaserDetails purchaserDetails, 
            IList<StatementReportRecord> records, ICalendar calendar, Date endDate, string title)
        {
            return new StatementReport(managementDetails, purchaserDetails, records, calendar, endDate, title, QueryString.ViewIDValue);
        }
    }
}