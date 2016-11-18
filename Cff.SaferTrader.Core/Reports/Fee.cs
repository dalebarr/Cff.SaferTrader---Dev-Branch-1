using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class Fee
    {
        private readonly int count;
        private readonly decimal rate;
        private readonly decimal total;

        public Fee(decimal rate, int count)
        {
            this.rate = rate;
            this.count = count;

            total = rate*count;
        }

        public decimal Total
        {
            get { return total; }
        }

        public decimal Rate
        {
            get { return rate; }
        }

        public int Count
        {
            get { return count; }
        }

        public static Fee None
        {
            get { return new Fee(0, 0); }
        }
    }
}