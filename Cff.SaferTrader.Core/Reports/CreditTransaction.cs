namespace Cff.SaferTrader.Core.Reports
{
    public class CreditTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;

        public CreditTransaction(decimal amount)
        {
            this.amount = amount;
        }

        #region ISubledgerTransaction Members

        public void AddTo(Subledger subledger)
        {
            subledger.AddCreditNote(amount);
        }

        #endregion
    }
}