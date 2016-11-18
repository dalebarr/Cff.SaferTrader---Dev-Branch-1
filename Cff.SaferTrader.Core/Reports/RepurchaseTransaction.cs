namespace Cff.SaferTrader.Core.Reports
{
    public class RepurchaseTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;

        public RepurchaseTransaction(decimal amount)
        {
            this.amount = amount;
        }

        #region ISubledgerTransaction Members

        public void AddTo(Subledger subledger)
        {
            subledger.AddRepurchase(amount);
        }

        #endregion
    }
}