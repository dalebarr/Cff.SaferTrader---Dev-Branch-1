namespace Cff.SaferTrader.Core.Builders
{
    public class CffClientInformationBuilder
    {
        private readonly CleverReader reader;

        public CffClientInformationBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public CffClientInformation Build(CffClient client)
        {
            return new CffClientInformation(client, 
                            reader.ToSmallInteger("ClientNoCalls"),
                            reader.ToString("CffLegalEntity"),           //MSarza [20150730]
                            reader.ToString("ClientSignature")           //MSarza [20150810]
                            );
        }
    }
}