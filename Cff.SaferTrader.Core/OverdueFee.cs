using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class OverdueFee
    {
        private readonly decimal charges;
        private readonly decimal fee;
        private readonly decimal adminFee;
        private readonly decimal gstRate;

        public OverdueFee(decimal charges, decimal fee, decimal adminFee, decimal gstRate)
        {
            this.charges = charges;
            this.fee = fee;
            this.adminFee = adminFee;
            this.gstRate = gstRate / 100;
        }

        /// <summary>
        /// This is a straight port from gpCFL windows application
        /// </summary>
        public decimal CalculateGstOnAdminFee()
        {
            decimal gstOnOverdueFeeAdmin = 0;
            if (fee > 0)
            {
                // decimal overdueFeeGstExclusive = charges / (1 + (adminFee / fee * (decimal)0.125)); old implementation
                decimal overdueFeeGstExclusive;
                if (gstRate > 0)
                    overdueFeeGstExclusive = charges / (1 + (adminFee / fee * gstRate));
                else
                    overdueFeeGstExclusive = charges;
                decimal overdueFeeAdminGstExclusive = overdueFeeGstExclusive * adminFee / fee;

                gstOnOverdueFeeAdmin = GstHelper.CalculateChargeableGst(overdueFeeAdminGstExclusive, gstRate);
            }
            return gstOnOverdueFeeAdmin;
        }
    }
}