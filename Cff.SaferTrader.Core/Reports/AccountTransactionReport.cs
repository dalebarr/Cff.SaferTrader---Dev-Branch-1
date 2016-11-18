using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class AccountTransactionReport : AccountTransactionReportBase
    {
        private decimal totalcredit;
        private decimal totaldebit;
        private decimal movement;
        private decimal closingbalance;
        private decimal openingbalance;

        private string title;
        IList<AccountTransactionReportRecord> records;

        //iff AllClients
        IList<AccountTransactionReport> subrecords;

        public AccountTransactionReport(ICalendar calendar, string title, string clientName, IList<AccountTransactionReportRecord> records, 
                        string name, decimal totCredit, decimal totDebit, decimal movementAmt, decimal closingBal, decimal openingBal)
            : base(calendar, title, clientName)
        { 

            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.records = records;
            this.totalcredit = totCredit;
            this.totaldebit = totDebit;
            this.movement = movementAmt;
            this.closingbalance = closingBal;
            this.openingbalance = openingBal;
            this.title = title;
          
        }

        public AccountTransactionReport(ICalendar calendar, string title, string clientName, IList<AccountTransactionReport> subrecords, string name)
            : base(calendar, title, clientName)
        {

            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.subrecords = subrecords;
            this.title = title;
        }


        public IList<AccountTransactionReportRecord> Records { get { return this.records; } }
        public override decimal TotalCredit { get { return this.totalcredit;  } }
        public override decimal TotalDebit { get { return this.totaldebit; } }
        public override decimal Movement { get { return this.movement; } }
        public override decimal ClosingBalance { get { return this.closingbalance; } }
        public override decimal OpeningBalance { get { return this.openingbalance; } }
        public string Title { get { return this.title; } }

    }
}
