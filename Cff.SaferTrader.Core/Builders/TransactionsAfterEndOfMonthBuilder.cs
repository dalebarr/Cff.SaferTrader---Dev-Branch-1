namespace Cff.SaferTrader.Core.Builders
{
    public class TransactionsAfterEndOfMonthBuilder
    {
        private readonly CleverReader reader;

        public TransactionsAfterEndOfMonthBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public TransactionsAfterEndOfMonth Build()
        {
            return new TransactionsAfterEndOfMonth(reader.ToSmallInteger("hold"),
                                                   reader.ToDecimal("LikelyRepurchases"),
                                                   reader.ToDecimal("postrep"),
                                                   reader.ToDecimal("postcr"));
        }
    }
}