namespace Cff.SaferTrader.Core.Builders
{
    public class BankDetailsBuilder
    {
        private readonly IReader reader;

        public BankDetailsBuilder(IReader reader)
        {
            this.reader = reader;
        }

        public BankDetails Build()
        {
            return new BankDetails(reader.ToString("Bank"),
                                   reader.ToString("Branch"),
                                   reader.ToString("BankAccount"));
        }
    }
}