namespace Cff.SaferTrader.Core.Builders
{
    public class BatchChargeBuilder
    {
        private readonly CleverReader reader;

        public BatchChargeBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public Charge Build()
        {
            return new Charge(reader.FromBigInteger("BChargeID"),
                              ChargeType.Parse(reader.ToSmallInteger("ChargeTypeID")),
                              reader.ToDecimal("Amount"),
                              reader.ToString("Descriptn"),
                              reader.ToDate("Modified"),
                              reader.ToString("ModifiedBy"));
        }
    }
}