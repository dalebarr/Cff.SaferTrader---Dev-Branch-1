using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class RetentionInfo
    {
        private readonly decimal factoredInvoices;
        private readonly Percentage factoredInvoicesPercentage;
        private readonly decimal factoredRetention;
        private readonly decimal surplus;
        private readonly decimal retentionHeld;

        public RetentionInfo(decimal retentionHeld, Percentage factoredInvoicesPercentage, decimal factoredInvoices)
        {
            ArgumentChecker.ThrowIfNull(factoredInvoicesPercentage, "factoredInvoicesPercentage");

            this.retentionHeld = retentionHeld;
            this.factoredInvoicesPercentage = factoredInvoicesPercentage;
            this.factoredInvoices = factoredInvoices;

            factoredRetention = factoredInvoicesPercentage.Of(factoredInvoices);
            surplus = retentionHeld - factoredRetention;
        }

        public decimal FactoredInvoices
        {
            get { return factoredInvoices; }
        }

        public decimal RetentionHeld
        {
            get { return retentionHeld; }
        }

        public decimal FactoredRetention
        {
            get { return factoredRetention; }
        }

        /// <summary>
        /// Surplus / (Shortfall)
        /// </summary>
        public decimal Surplus
        {
            get { return surplus; }
        }

        public Percentage FactoredInvoicesPercentage
        {
            get { return factoredInvoicesPercentage; }
        }
    }
}