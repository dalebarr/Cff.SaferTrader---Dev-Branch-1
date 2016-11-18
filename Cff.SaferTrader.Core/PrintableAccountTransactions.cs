using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableAccountTransactions: IPrintable
    {
        private readonly IList<AccountTransactionReportRecord> acctTrnsRptRecords;
        private readonly RetentionSchedule retnSchedule;
        private readonly decimal movement;
        private readonly decimal closingbalance;
        private readonly string viewID;

       public PrintableAccountTransactions(AccountTransactionReportBase acctTrnsRpt, RetentionSchedule retentionschedule, string viewIDValue)
       {
            this.retnSchedule = retentionschedule;
            this.movement = acctTrnsRpt.Movement;
            this.closingbalance = acctTrnsRpt.ClosingBalance;

            this.acctTrnsRptRecords = ((AccountTransactionReport)acctTrnsRpt).Records;
            this.viewID = viewIDValue;
       }

       public PrintableAccountTransactions(IList<AccountTransactionReportRecord> acctTrnsRpt, string viewIDValue)
      {
          this.acctTrnsRptRecords = acctTrnsRpt;
          this.viewID = viewIDValue;
      }

      public IList<AccountTransactionReportRecord> AccountTransReportRecords
      {
          get { return this.acctTrnsRptRecords; }
      }

      public RetentionSchedule RetentionSchedule
      {
          get { return this.retnSchedule; }
      }

      public Decimal Movement
      {
          get { return this.movement; }
      }

      public Decimal ClosingBalance
      {
          get { return this.closingbalance; }
      }

      public string PopupPageName
      { get { return "RetnAccountTransactionsPopup.aspx?ViewID=" + this.viewID; } }
    }
}
