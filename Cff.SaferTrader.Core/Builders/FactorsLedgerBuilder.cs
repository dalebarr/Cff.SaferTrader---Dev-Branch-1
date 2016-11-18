using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Builders
{
    public class FactorsLedgerBuilder
    {
        public FactorsLedger Build(Subledger broughtForwardLedger, Subledger currentLedger)
        {
            return new FactorsLedger(broughtForwardLedger, currentLedger);
        }
    }
}