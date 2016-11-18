namespace Cff.SaferTrader.Core.Reports
{
    public class JournalTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;

        public JournalTransaction(decimal amount)
        {
            this.amount = amount;
        }

        public void AddTo(Subledger subledger)
        {
            subledger.AddJournal(amount);
        }
    }
}