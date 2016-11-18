using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CustomerInvoicesTransactionReportRecordBuilder
    {
        private readonly ICalendar calendar;

        public CustomerInvoicesTransactionReportRecordBuilder(ICalendar calendar)
        {
            this.calendar = calendar;
        }

        public IList<CustomerInvoicesTransactionReportRecord> Build(DataRowCollection rows)
        {
            IList<CustomerInvoicesTransactionReportRecord> records = new List<CustomerInvoicesTransactionReportRecord>();
            DataRowReader reader = new DataRowReader(rows);

            while (reader.Read())
            {
                records.Add(Build(reader));
            }
            return records;
        }

        private CustomerInvoicesTransactionReportRecord Build(DataRowReader reader)
        {
            return new CustomerInvoicesTransactionReportRecord(reader.ToInteger("TrueTrnID"),
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