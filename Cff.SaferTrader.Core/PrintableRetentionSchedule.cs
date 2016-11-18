using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableRetentionSchedule : IPrintable
    {
        private readonly RetentionSchedule retentionSchedule;
        private readonly RetentionDetails retentionDetails;
        private readonly string viewID;

        public PrintableRetentionSchedule(RetentionSchedule retentionSchedule, RetentionDetails retentionDetails, string viewIDValue)
        {
            this.retentionSchedule = retentionSchedule;
            this.retentionDetails = retentionDetails;
            this.viewID = viewIDValue;
        }

        public RetentionDetails RetentionDetails
        {
            get { return retentionDetails; }
        }

        public RetentionSchedule RetentionSchedule
        {
            get { return retentionSchedule; }
        }

        public string PopupPageName
        {
            get { return "RetentionSchedulePopup.aspx?ViewID=" + this.viewID; }
        }
    }
}