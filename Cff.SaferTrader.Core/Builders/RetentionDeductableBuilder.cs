namespace Cff.SaferTrader.Core.Builders
{
    public class RetentionDeductableBuilder
    {
        private readonly CleverReader reader;

        public RetentionDeductableBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public RetentionDeductable Build()
        {
            return new RetentionDeductable(reader.ToDecimal("intCharges"),
                                  reader.ToDecimal("bkfees"),
                                  reader.ToDecimal("postRate"),
                                  reader.ToDecimal("postamt"),
                                  reader.ToDecimal("disc"),
                                  reader.ToDecimal("prerep"),
                                  reader.ToDecimal("precr"),
                                  reader.ToDecimal("tolls"),
                                  reader.ToDecimal("letters"),
                                  reader.ToDecimal("repayt"),
                                  reader.ToDecimal("LikelyRepurchases"),
                                  System.Convert.ToDecimal(reader.ToDouble("GSTRate")),
                                  reader.ToString("RetnNotes"));
        }
    }
}