namespace Cff.SaferTrader.Core.Builders
{
    public class RetentionSummaryBuilder
    {
        private readonly CleverReader reader;

        public RetentionSummaryBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public RetentionSummary Build()
        {
            return new RetentionSummary(reader.ToSmallInteger("facdays"),
                                        reader.ToDecimal("obal"),
                                        reader.ToDecimal("invpurch"),
                                        reader.ToDecimal("nfinvs"),
                                        reader.ToDecimal("facinvs"));
        }
    }
}