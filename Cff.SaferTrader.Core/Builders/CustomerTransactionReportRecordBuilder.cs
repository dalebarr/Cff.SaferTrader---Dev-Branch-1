using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CustomerTransactionReportRecordBuilder
    {
        private readonly ICalendar calendar;

        public CustomerTransactionReportRecordBuilder(ICalendar calendar)
        {
            this.calendar = calendar;
        }

        public IList<CustomerTransactionReportRecord> Build(DataRowCollection rows)
        {
            IList<CustomerTransactionReportRecord> records = new List<CustomerTransactionReportRecord>();
            DataRowReader reader = new DataRowReader(rows);

            while (reader.Read())
            {
                records.Add(Build(reader));
            }
            return records;
        }

        private CustomerTransactionReportRecord Build(DataRowReader reader)
        {
            CustomerTransactionReportRecord custTransReportRecord = null;
            short transId = System.Convert.ToInt16(reader.ToString("TransTypeId").Trim());
            if (transId < 2 || transId == 4 || transId > 7)
            {
                custTransReportRecord = new CustomerTransactionReportRecord(reader.ToInteger("TrueTrnID"),
                                                           reader.ToDate("Dated"),
                                                           reader.ToString("Number"),
                                                           reader.ToString("Reference"),
                                                           reader.ToDecimal("Amount"),
                                                           reader.ToDate("Factored"),
                                                           calendar,
                                                           reader.ToString("Type").Trim());
            }
            else
            {
                custTransReportRecord = new CustomerTransactionReportRecord(reader.ToInteger("TrueTrnID"),
                                                           reader.ToDate("Dated"),
                                                           reader.ToString("Number"),
                                                           reader.ToString("Reference"),
                                                           reader.ToDecimal("Balance"),
                                                           reader.ToDate("Factored"),
                                                           calendar,
                                                           reader.ToString("Type").Trim());
            }
            return custTransReportRecord;
        }   
    }
}