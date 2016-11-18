using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class BalanceRange
    {
        private readonly decimal minBalance;
        private readonly decimal maxBalance;

        public BalanceRange(decimal minBalance, decimal maxBalance)
        {
            this.minBalance = minBalance;
            this.maxBalance = maxBalance;
        }

        public decimal MaxBalance
        {
            get { return maxBalance; }
        }

        public decimal MinBalance
        {
            get { return minBalance; }
        }
    }
}