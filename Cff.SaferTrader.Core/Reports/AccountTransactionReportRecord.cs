using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class AccountTransactionReportRecord 
    {
        private readonly int clientid;
        private readonly IDate trandate;
       
        private readonly decimal debit;
        private readonly decimal credit;
        private readonly string bankjnldescription;
        private readonly string tranref;
        TransactionType trantype;

        public AccountTransactionReportRecord(int cid, decimal value, string bnkjnldesc, string trxref, IDate thedate, TransactionType trxtype)
        {
            this.clientid = cid;
            this.bankjnldescription = bnkjnldesc;
            this.tranref = trxref;
            this.trandate = (thedate==null)?((IDate)(new Date(Convert.ToDateTime("01/01/1900")))):thedate;
            this.trantype = (trxtype==null)?TransactionType.Other:trxtype;

            if (value < 0)
            {
                this.credit = Math.Abs(value);
            }
            else {
                this.debit = value;
            }
        }

        public decimal Debit {
            get { return this.debit; }
        }

        public decimal Credit {
            get { return this.credit; }
        }

        public int ClientId { get { return this.clientid; }  }

        public string Description { get { return this.bankjnldescription; } }

        public string TranType { get {
            try
            {
                return this.trantype.Type;
            }
            catch (Exception)
            {
                return null;
            }
           } 
        }

        public string TranRef { get { return this.tranref; } }

        public string TranDate
        {
            get
            {
                try
                {
                    if (trandate != null) { return this.trandate.ToString(); }
                    else { return null; }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

    }
}
