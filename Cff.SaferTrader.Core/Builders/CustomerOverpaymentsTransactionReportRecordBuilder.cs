using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CustomerOverpaymentsTransactionReportRecordBuilder
    {
        private readonly ICalendar calendar;

        public CustomerOverpaymentsTransactionReportRecordBuilder(ICalendar calendar)
        {
            this.calendar = calendar;
        }

        public IList<CustomerOverpaymentsTransactionReportRecord> Build(DataRowCollection rows)
        {
            IList<CustomerOverpaymentsTransactionReportRecord> records = new List<CustomerOverpaymentsTransactionReportRecord>();
            DataRowReader reader = new DataRowReader(rows);

            while (reader.Read())
            {
                records.Add(Build(reader));
            }
            return records;
        }

        private CustomerOverpaymentsTransactionReportRecord Build(DataRowReader reader)
        {
            return new CustomerOverpaymentsTransactionReportRecord(reader.ToInteger("TrueTrnID"),
                                                               reader.ToDate("Dated"),
                                                               reader.ToString("Number"),
                                                               reader.ToString("Reference"),
                                                               reader.ToDecimal("Amount"),
                                                               reader.ToDate("Factored"),
                                                               reader.ToDecimal("Balance"),
                                                               calendar,
                                                               reader.ToString("Type"));
        }
    }
}
