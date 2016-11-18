using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class StatementReport : ReportBase, IPrintable
    {
        private readonly ManagementDetails managementDetails;
        private readonly PurchaserDetails purchaserDetails;
        private readonly IList<StatementReportRecord> records;
        private readonly Date dateAsAt;
        private readonly string title;
        private readonly AgeingBalances ageingBalances;
        private readonly string viewID;

        public StatementReport(ManagementDetails managementDetails, PurchaserDetails purchaserDetails, IList<StatementReportRecord> records, ICalendar calendar, Date dateAsAt, string title, string viewIDValue)
            : base(calendar, "Statement", purchaserDetails.ClientName)
        {
            ArgumentChecker.ThrowIfNull(managementDetails, "managementDetails");
            ArgumentChecker.ThrowIfNull(records, "records");

            this.managementDetails = managementDetails;
            this.purchaserDetails = purchaserDetails;
            this.records = records;
            this.dateAsAt = dateAsAt;
            this.title = title;
            this.viewID = viewIDValue;

            ageingBalances = new AgeingBalances(records);
        }

        public string Title
        {
            get { return title; }
        }

        public IList<StatementReportRecord> Records
        {
            get { return records; }
        }

        public PurchaserDetails PurchaserDetails
        {
            get { return purchaserDetails; }
        }

        public ManagementDetails ManagementDetails
        {
            get { return managementDetails; }
        }

        public AgeingBalances AgeingBalances
        {
            get { return ageingBalances; }
        }

        public Date MonthEnding
        {
            get { return dateAsAt; }
        }

        public string PopupPageName
        {
            get { return "StatementReportPopup.aspx?ViewID=" + this.viewID; }
        }
    }
}