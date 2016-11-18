using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class DebtorsLedger
    {
        private readonly decimal current;
        private readonly decimal oneMonth;
        private readonly decimal twoMonths;
        private readonly decimal threeMonthsAndOver;
        private readonly decimal total;

        public DebtorsLedger(decimal current, decimal oneMonth, decimal twoMonths, decimal threeMonthsAndOver)
        {
            this.current = current;
            this.oneMonth = oneMonth;
            this.twoMonths = twoMonths;
            this.threeMonthsAndOver = threeMonthsAndOver;

            total = current + oneMonth + twoMonths + threeMonthsAndOver;
        }

        public decimal Total
        {
            get { return total; }
        }

        public decimal Current
        {
            get { return current; }
        }

        public decimal OneMonth
        {
            get { return oneMonth; }
        }

        public decimal TwoMonths
        {
            get { return twoMonths; }
        }

        public decimal ThreeMonthsAndOver
        {
            get { return threeMonthsAndOver; }
        }
    }
}
