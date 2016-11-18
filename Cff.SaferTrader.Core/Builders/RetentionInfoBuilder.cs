namespace Cff.SaferTrader.Core.Builders
{
    public class RetentionInfoBuilder
    {
        private readonly CleverReader reader;

        public RetentionInfoBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public RetentionInfo Build()
        {
            return new RetentionInfo(reader.ToDecimal("retheld"),
                                 new Percentage((double)reader.ToDecimal("retpc")),
                                 reader.ToDecimal("facinvs"));
        }
    }
}