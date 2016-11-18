
namespace Cff.SaferTrader.Core.Builders
{
    public class ManagementDetailsBuilder
    {
        private readonly IReader reader;

        public ManagementDetailsBuilder(IReader reader)
        {
            this.reader = reader;
        }

        public ManagementDetails Build()
        {
            reader.Read();

            Address address = new AddressBuilder(reader).BuildPostalAddress();
            BankDetails bankDetails = new BankDetailsBuilder(reader).Build();

            return new ManagementDetails(reader.ToString("name"),
                                         reader.ToString("LegalEntity1"),
                                         reader.ToString("LegalEntity2"), 
                                         reader.ToString("Phone"),
                                         reader.ToString("Fax"),
                                         reader.ToString("email"),
                                         reader.ToString("web"),
                                         address, bankDetails, reader.ToString("GSTCode"));
        }
    }
}