namespace Cff.SaferTrader.Core.Builders
{
    public class CffClientBuilder
    {
        private readonly IReader reader;

        public CffClientBuilder(IReader reader)
        {
            this.reader = reader;
        }

        //private readonly CleverReader reader;

        //public CffClientBuilder(CleverReader reader)
        //{
        //    this.reader = reader;
        //}

        public CffClient Build()
        {
            return new CffClient(reader.ToString("ClientName"),
                                 reader.FromBigInteger("ClientID"),
                                 reader.FromBigInteger("ClientNum"),
                                 reader.ToSmallInteger("FacilityType"),
                                 reader.ToString("CollectionsBankAccount")
                                 ); 
        }
    }

    //public class CffClientBuilderCleverReader
    //{
    //    private readonly CleverReader reader;
    //    public CffClientBuilderCleverReader(CleverReader reader)
    //    {
    //        this.reader = reader;
    //    }

    //    public CffClient Build()
    //    {
    //        return new CffClient(reader.ToString("ClientName"),
    //                             reader.FromBigInteger("ClientID"),
    //                             reader.FromBigInteger("ClientNum"),
    //                             reader.ToSmallInteger("FacilityType"),
    //                             reader.ToString("CollectionsBankAccount"),
    //                             reader.ToSmallInteger("CFFDebtorAdmin"), //MSazra [20151006]
    //                             reader.ToBoolean("ClientHasLetterTemplates") //MSazra [20151006]
    //                             );
    //    }
    //}

}