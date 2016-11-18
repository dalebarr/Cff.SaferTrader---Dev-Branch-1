using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class Percentage
    {
        private readonly double percentage;

        public Percentage(double percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException("percentage", "Percentage should be between 0 and 100");
            }
            this.percentage = percentage;
        }

        public decimal Of(double total)
        {
            return Convert.ToDecimal(total * percentage / 100);
        }

        public override string ToString()
        {
            return percentage.ToString("0.00") + "%";
        }

        public decimal Of(decimal total)
        {
            return Of((double) total);
        }

        public decimal Of(int total)
        {
            return Of((double)total);
        }
    }
}