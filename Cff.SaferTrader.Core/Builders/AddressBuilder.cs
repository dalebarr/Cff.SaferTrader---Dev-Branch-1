namespace Cff.SaferTrader.Core.Builders
{
    public class AddressBuilder
    {
        private readonly IReader reader;

        public AddressBuilder(IReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Builds Address from PostalAddress fields e.g. PostalAddress1
        /// </summary>
        public Address BuildPostalAddress()
        {
            return new Address(reader.ToString("PostalAddress1"),
                               reader.ToString("PostalAddress2"),
                               reader.ToString("PostalAddress3"),
                               reader.ToString("PostalAddress4"));
        }

        /// <summary>
        /// Builds Address from Address fields e.g. Address1
        /// </summary>
        public Address Build()
        {
            return new Address(reader.ToString("Address1"),
                               reader.ToString("Address2"),
                               reader.ToString("Address3"),
                               reader.ToString("Address4"));
        }
    }
}