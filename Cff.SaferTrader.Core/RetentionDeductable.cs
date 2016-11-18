using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class RetentionDeductable
    {
        private readonly decimal bankFees;
        private readonly decimal overdueCharges;
        private readonly decimal creditNotes;
        private readonly decimal discounts;
        private readonly decimal lettersSent;
        private readonly decimal postAmount;
        private readonly decimal postRate;
        private readonly decimal repayment;
        private readonly decimal repurchases;
        private readonly decimal tolls;
        private readonly decimal likelyrepurchases;
        private readonly decimal gstRate;
        private readonly string retnNotes;

        public RetentionDeductable(decimal overdueCharges, decimal bankFees, decimal postRate, decimal postAmount, decimal discounts,
                          decimal repurchases, decimal creditNotes, decimal tolls, decimal lettersSent,
                          decimal repayment, decimal likelyrepurchases, decimal gstRate, string retnNotes)
        {
            this.overdueCharges = overdueCharges;
            this.bankFees = bankFees;
            this.postRate = postRate;
            this.postAmount = postAmount;
            this.discounts = discounts;
            this.repurchases = repurchases;
            this.creditNotes = creditNotes;
            this.tolls = tolls;
            this.lettersSent = lettersSent;
            this.repayment = repayment;
            this.likelyrepurchases = likelyrepurchases;
            this.gstRate = gstRate / 100;
            this.retnNotes = retnNotes;
        }

        public decimal CalculateTotal()
        {
            return overdueCharges + bankFees + postAmount + discounts + repurchases + creditNotes + tolls + lettersSent + repayment;
        }

        public decimal CAChargesTotal()
        {
            return overdueCharges + bankFees + postAmount + tolls + lettersSent + repayment;
        }


        public decimal PostAmount
        {
            get { return postAmount; }
        }

        public decimal PostRate
        {
            get { return postRate; }
        }

        /// <summary>
        /// Overdue charges (GST inclusive)
        /// </summary>
        public decimal OverdueCharges
        {
            get { return overdueCharges; }
        }

        public decimal BankFees
        {
            get { return bankFees; }
        }

        public decimal Discounts
        {
            get { return discounts; }
        }

        public decimal Repurchases
        {
            get { return repurchases; }
        }

        public decimal CreditNotes
        {
            get { return creditNotes; }
        }

        public decimal Tolls
        {
            get { return tolls; }
        }

        public decimal LettersSent
        {
            get { return lettersSent; }
        }

        public decimal Repayment
        {
            get { return repayment; }
        }

        public decimal CalculateGst()
        {
            return GstHelper.CalculateGstCharged(PostAmount, gstRate) + GstHelper.CalculateGstCharged(Tolls, gstRate) + GstHelper.CalculateGstCharged(LettersSent, gstRate);
        }

        public decimal LikelyRepurchases
        {
            get { return likelyrepurchases; }
        }

        public string RetentionNotes
        {
            get { return retnNotes; }
        }
    }
}