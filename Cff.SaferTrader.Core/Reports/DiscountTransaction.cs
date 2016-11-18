namespace Cff.SaferTrader.Core.Reports
{
    public class DiscountTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;

        public DiscountTransaction(decimal amount)
        {
            this.amount = amount;
        }

        #region ISubledgerTransaction Members

        public void AddTo(Subledger subledger)
        {
            subledger.AddDiscount(amount);
        }

        #endregion
    }
}