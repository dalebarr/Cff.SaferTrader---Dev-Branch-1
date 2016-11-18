namespace Cff.SaferTrader.Core.Builders
{
    public class ChargeBuilder
    {
        private readonly CleverReader reader;

        public ChargeBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public Charge Build()
        {
            return new Charge(reader.FromBigInteger("RetnChargeID"),
                              ChargeType.Parse(reader.ToSmallInteger("ChargeTypeID")),
                              reader.ToDecimal("Amount"),
                              reader.ToString("Descriptn"),
                              reader.ToDate("Modified"),
                              reader.ToString("ModifiedBy"));
        }
    }
}