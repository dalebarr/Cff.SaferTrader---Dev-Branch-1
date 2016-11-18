namespace Cff.SaferTrader.Core.Builders
{
    public class RetentionDetailsBuilder
    {
        private readonly CleverReader reader;

        public RetentionDetailsBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public RetentionDetails Build(RetentionInfo retentionInfo, RetentionDeductable retentionDeductable,
                                      TransactionsAfterEndOfMonth transactionsAfterEndOfMonth,
                                      RetentionSummary retentionSummary, ChargeCollection charges, OverdueFee overdueFee)
        {
            return new RetentionDetails(reader.FromBigInteger("RetnID"),
                                        reader.ToNullableDate("Released"),
                                        reader.ToDate("eom"),
                                        reader.ToDecimal("nfinvs"),
                                        reader.ToDecimal("facinvs"),
                                        retentionInfo,
                                        retentionDeductable,
                                        reader.ToDecimal("nfrec"),
                                        transactionsAfterEndOfMonth,
                                        retentionSummary, 
                                        charges,
                                        overdueFee, 
                                        reader.ToString("status"),
                                        reader.ToSmallInteger("hold"),
                                        reader.ToSmallInteger("clientFacilityType"));
        }
    }
}