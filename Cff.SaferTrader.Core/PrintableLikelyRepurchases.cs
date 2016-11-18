using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableLikelyRepurchases:IPrintable
    {
        private readonly IList<LikelyRepurchasesLine> repurchasesLineList;
        private readonly LikelyRepurchasesLine likelyrepurchases;
        private readonly RetentionSchedule retnSchedule;
        private readonly string viewID;

        public PrintableLikelyRepurchases(IList<LikelyRepurchasesLine> repline, RetentionSchedule retentionschedule, string viewIDValue)
        {
            this.retnSchedule = retentionschedule;
            this.repurchasesLineList= repline;
            this.viewID = viewIDValue;
        }

       public PrintableLikelyRepurchases(LikelyRepurchasesLine repline, string viewIDValue)
      {
          this.likelyrepurchases = repline;
          this.viewID = viewIDValue;
       } 

       public RetentionSchedule RetentionSchedule
      {
          get { return this.retnSchedule; }
      }

       public LikelyRepurchasesLine LikelyRepurchases
       {
           get { return this.likelyrepurchases; }
       }

       public IList<LikelyRepurchasesLine> RepurchasesLineList
       {
           get { return this.repurchasesLineList; }
       }
       
      public string PopupPageName
      { get { return "RetentionLikelyRepurchasesPopup.aspx?ViewID=" + this.viewID; } }
    }
}
