namespace Cff.SaferTrader.Core.Builders
{
    public class RetentionScheduleBuilder
    {
        private readonly CleverReader reader;

        public RetentionScheduleBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public RetentionSchedule Build()
        {
            return new RetentionSchedule(reader.FromBigInteger("RetnID"),
                                         reader.ToString("ClientName"), 
                                         reader.ToDate("eom"),
                                         reader.ToString("status"),
                                         reader.ToNullableDate("Released"),
                                         reader.FromBigInteger("ClientID"),
                                         reader.ToString("RetnNotes"),
                                         reader.ToSmallInteger("ClientFacilityType"));

        }
    }
}