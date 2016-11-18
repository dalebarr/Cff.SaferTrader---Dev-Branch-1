using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CurrentsPaymentReportRecordBuilder
    {
        public IList<CurrentPaymentsReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName)
        {
            IList<CurrentPaymentsReportRecord> records = new List<CurrentPaymentsReportRecord>();
            var reader = new DataRowReader(rows);
            while (reader.Read())
            {
                records.Add(Build(reader, clientNumber, clientName));
            }
            return records;
        }

        public IList<CurrentPaymentsReportRecord> BuildForAllClients(DataRowCollection rows)
        {
            IList<CurrentPaymentsReportRecord> records = new List<CurrentPaymentsReportRecord>();
            var reader = new DataRowReader(rows);
            while (reader.Read())
            {
                records.Add(Build(reader, reader.ToInteger("ClientNum"), reader.ToString("ClientName")));
            }
            return records;
        }

        private static CurrentPaymentsReportRecord Build(DataRowReader reader, int clientNumber, string clientName)
        {
            return new CurrentPaymentsReportRecord(reader.ToInteger("TrnID"),
                                                   clientNumber, clientName, reader.ToInteger("CustomerID"),
                                                   reader.ToInteger("CustNum"), reader.ToString("Customer"),
                                                   reader.ToDate("Dated"), reader.ToString("TrnNumber"),
                                                   reader.ToString("Reference"), reader.ToDecimal("Amount"),
                                                   reader.ToDecimal("Balance"),
                                                   TransactionStatus.Parse(reader.ToInteger("TypeStatusID")),
                                                   reader.ToDate("Factored"), reader.ToInteger("BatchID"),
                                                   TransactionType.Parse(reader.ToInteger("TransTypeID")));
        }
    }
}