using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class TransactionsAfterEndOfMonth
    {
        private readonly decimal balance;
        private readonly decimal creditNotes;
        private readonly decimal repurchases;
        private readonly decimal likelyrepurchased;

        public TransactionsAfterEndOfMonth(short hold, decimal likelyRep, decimal repurchases, decimal creditNotes)
        {
            this.repurchases = repurchases;
            this.creditNotes = creditNotes;
            this.likelyrepurchased = likelyRep;
            if (hold == 2) // is Est OK
                balance = repurchases + creditNotes + likelyrepurchased;
            else
            {
                balance = repurchases + creditNotes;
            }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public decimal Repurchases
        {
            get { return repurchases; }
        }

        public decimal CreditNotes
        {
            get { return creditNotes; }
        }

        public decimal LikelyRepurchases
        {
            get { return likelyrepurchased; }
        }
    }
}