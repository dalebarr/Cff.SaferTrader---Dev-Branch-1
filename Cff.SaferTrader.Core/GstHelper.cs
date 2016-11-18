using System;

namespace Cff.SaferTrader.Core
{
    public static class GstHelper
    {
        //private const decimal gstRate = (decimal) 0.125; //old implementation

        public static decimal CalculateGstCharged(decimal amount, decimal gstRate)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return amount - amount / (1 + gstRate);
        }

        public static decimal CalculateChargeableGst(decimal amount, decimal gstRate)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return amount * gstRate;
        }
    }
}