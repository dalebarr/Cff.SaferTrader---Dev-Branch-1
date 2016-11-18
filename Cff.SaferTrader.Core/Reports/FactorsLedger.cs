using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class FactorsLedger
    {
        private readonly Subledger broughtForwardLedger;
        private readonly Subledger currentLedger;

        public FactorsLedger(Subledger broughtForwardLedger, Subledger currentLedger)
        {
            ArgumentChecker.ThrowIfNull(broughtForwardLedger, "broughtForwardLedger");
            ArgumentChecker.ThrowIfNull(currentLedger, "currentLedger");

            this.broughtForwardLedger = broughtForwardLedger;
            this.currentLedger = currentLedger;
        }

        public decimal Total
        {
            get { return broughtForwardLedger.Total + currentLedger.Total; }
        }

        public Subledger BroughtForwardLedger
        {
            get { return broughtForwardLedger; }
        }

        public Subledger CurrentLedger
        {
            get { return currentLedger; }
        }
    }
}