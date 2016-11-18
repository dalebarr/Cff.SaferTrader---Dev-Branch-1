using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ClientAttribute
    {
        private readonly decimal nfAdminFee;
        private readonly decimal adminFee;
        private readonly decimal factorFee;
        private readonly decimal retentionPercent;

        public ClientAttribute(decimal nfAdminFee, decimal adminFee, decimal factorFee, decimal retentionPercent)
        {
            this.nfAdminFee = nfAdminFee;
            this.adminFee = adminFee;
            this.factorFee = factorFee;
            this.retentionPercent = retentionPercent;
        }

        public decimal RetentionPercent
        {
            get { return retentionPercent; }
        }

        public decimal FactorFee
        {
            get { return factorFee; }
        }

        public decimal AdminFee
        {
            get { return adminFee; }
        }

        public decimal NfAdminFee
        {
            get { return nfAdminFee; }
        }
    }
}