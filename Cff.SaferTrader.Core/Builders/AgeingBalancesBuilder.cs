namespace Cff.SaferTrader.Core.Builders
{
    public class AgeingBalancesBuilder
    {
        private readonly CleverReader reader;

        public AgeingBalancesBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public AgeingBalances Build()
        {
            decimal current = 0, oneMonth = 0, twoMonth =0, balance = 0;
            if (reader.Read())
            {
                current = reader.ToDecimal("Current_");
                oneMonth = reader.ToDecimal("One_Month");
                twoMonth = reader.ToDecimal("Two_Month");
                balance = reader.ToDecimal("Balance");
            }
            return new AgeingBalances(current, oneMonth, twoMonth, balance);
        }
    }
}