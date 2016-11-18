namespace Cff.SaferTrader.Core.Builders
{
    public class ClientCustomerTermBuilder
    {
        private readonly CleverReader reader;

        public ClientCustomerTermBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public ClientCustomerTerm Build()
        {
            return new ClientCustomerTerm(
                reader.ToSmallInteger("customerInvTerms"),
                reader.ToSmallInteger("SetCustTerms"),
                reader.ToSmallInteger("CustTerms"),
                reader.ToSmallInteger("TermsFrom"),
                reader.ToSmallInteger("clientInvTerms"),
                reader.ToSmallInteger("InvDays"),
                reader.ToSmallInteger("InvDaysFrom")
                );
        }
    }
}