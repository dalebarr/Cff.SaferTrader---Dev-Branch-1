namespace Cff.SaferTrader.Core.Builders
{
    public class OverdueFeeBuidler
    {
        private readonly CleverReader reader;

        public OverdueFeeBuidler(CleverReader reader)
        {
            this.reader = reader;
        }

        public OverdueFee Build()
        {
            return new OverdueFee(reader.ToDecimal("intCharges"),
                                  reader.ToDecimal("facint"),
                                  reader.ToDecimal("odadmin"),
                                  System.Convert.ToDecimal(reader.ToDouble("GSTRate")));
        }
    }
}