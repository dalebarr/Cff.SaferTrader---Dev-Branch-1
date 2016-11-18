using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableCreditsClaimed : IPrintable
    {
        private readonly RetentionSchedule retnSched;
        private readonly  IList<ClaimedCredit> cCredits;
        private readonly string viewID;

        public PrintableCreditsClaimed( IList<ClaimedCredit> claimedcredits, RetentionSchedule retnSchedule, string viewIDValue)
        {
            this.cCredits = claimedcredits;
            this.retnSched = retnSchedule;
            this.viewID = viewIDValue;
        }

        public RetentionSchedule RetnSchedule
        {
            get { return retnSched; }
        }

        public IList<ClaimedCredit> ClaimedCredits
        {
            get { return cCredits; }
        }

        public string PopupPageName
        {
            get { return "CreditsClaimedPopup.aspx?ViewID=" + this.viewID; }
        }

    }
}
