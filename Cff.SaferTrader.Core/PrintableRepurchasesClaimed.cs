using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableRepurchasesClaimed : IPrintable
    {
        private readonly RetentionSchedule retnSched;
        private readonly IList<ClaimedRetentionRepurchase> repurchClaimed;
        private readonly string viewID;

        public PrintableRepurchasesClaimed(IList<ClaimedRetentionRepurchase> repurchasesclaimed, 
                    RetentionSchedule retnSchedule, string viewIDValue)
        {

            this.repurchClaimed = new List<ClaimedRetentionRepurchase>();

            foreach (ClaimedRetentionRepurchase cRep in repurchasesclaimed)
            {
                
                cRep.Amount = Math.Abs(cRep.Amount);
                this.repurchClaimed.Add(cRep);
            }
            this.retnSched = retnSchedule;
            this.viewID = viewIDValue;

        }

        public RetentionSchedule RetnSchedule
        {
            get { return retnSched; }
        }


        public IList<ClaimedRetentionRepurchase> RepurchasesClaimed
        {
            get { return repurchClaimed; }
        }


        public string PopupPageName
        {
            get { return "RepurchasesClaimedPopup.aspx?ViewID=" + this.viewID; }
        }
    }


}
