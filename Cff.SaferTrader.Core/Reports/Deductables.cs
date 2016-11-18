using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class Deductables
    {
        private readonly Fee chequeFee;
        private readonly Fee letterFee;
        private readonly decimal likelyRepurchases;
        private readonly decimal overdueCharges;
        private readonly Fee postage;
        private readonly decimal unclaimedCredits;
        private readonly decimal unclaimedDiscounts;
        private readonly decimal unclaimedRepurchases;

        public Deductables(decimal unclaimedCredits, decimal unclaimedRepurchases, decimal unclaimedDiscounts, decimal likelyRepurchases, 
            decimal overdueCharges, Fee chequeFee, Fee postage, Fee letterFee)
        {
            this.unclaimedCredits = unclaimedCredits;
            this.unclaimedRepurchases = unclaimedRepurchases;
            this.unclaimedDiscounts = unclaimedDiscounts;
            this.likelyRepurchases = likelyRepurchases;
            this.overdueCharges = overdueCharges;
            this.chequeFee = chequeFee;
            this.postage = postage;
            this.letterFee = letterFee;
        }

        public decimal Total
        {
            get
            {
                return unclaimedCredits + unclaimedRepurchases + unclaimedDiscounts + likelyRepurchases + overdueCharges +
                       chequeFee.Total + postage.Total + letterFee.Total;
            }
        }

        public decimal UnclaimedCredits
        {
            get { return unclaimedCredits; }
        }

        public decimal UnclaimedRepurchases
        {
            get { return unclaimedRepurchases; }
        }

        public decimal UnclaimedDiscounts
        {
            get { return unclaimedDiscounts; }
        }

        public decimal LikelyRepurchases
        {
            get { return likelyRepurchases; }
        }

        public decimal OverdueCharges
        {
            get { return overdueCharges; }
        }

        /// <summary>
        /// Cheque Fee for client * Number of cheques
        /// </summary>
        public Fee ChequeFee
        {
            get { return chequeFee; }
        }

        /// <summary>
        /// Postage Fee for client * Number of posts
        /// </summary>
        public Fee Postage
        {
            get { return postage; }
        }

        /// <summary>
        /// Letter Fee for client * Number of letters
        /// </summary>
        public Fee LetterFees
        {
            get { return letterFee; }
        }
    }
}