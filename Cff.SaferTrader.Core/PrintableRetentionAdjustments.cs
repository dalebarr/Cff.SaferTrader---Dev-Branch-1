using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
   [Serializable]
   public class PrintableRetentionAdjustments : IPrintable
   {

      private readonly IList<Charge> retnCharges;
      private readonly RetentionAdjustments retnAdjustments;
      private readonly RetentionSchedule retnSchedule;
      private readonly string viewID;

      public PrintableRetentionAdjustments(IList<Charge> charges, RetentionSchedule retentionschedule, string viewIDValue)
      {
          this.retnSchedule = retentionschedule;
          this.retnCharges = charges;
          this.viewID = viewIDValue;
      }

       public PrintableRetentionAdjustments(RetentionAdjustments retentionadjustments, string viewIDValue)
      {
          this.retnAdjustments = retentionadjustments;
          this.viewID = viewIDValue;
      } 

       public RetentionSchedule RetentionSchedule
      {
          get { return retnSchedule; }
      }

       public RetentionAdjustments RetentionAdjustments
       {
           get { return retnAdjustments; }
       }

       public IList<Charge> Charges
       {
           get { return retnCharges; }
       }
       
      public string PopupPageName
      { //CFF-13
             get { return "RetentionAdjustmentsPopup.aspx?ViewID=" + this.viewID; }
       }
   }
}
