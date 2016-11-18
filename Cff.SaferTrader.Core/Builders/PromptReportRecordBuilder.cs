using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class PromptReportRecordBuilder
    {
        public IList<PromptReportRecord> Build(DataRowCollection rowCollection, int clientNumber, string clientName)
        {
            IList<PromptReportRecord> records = new List<PromptReportRecord>();
            DataRowReader reader = new DataRowReader(rowCollection);
            while (reader.Read())
            {
                records.Add(Build(reader, clientNumber, clientName));
            }
            return records;
        }

        public IList<PromptReportRecord> BuildForAllClients(DataRowCollection rows)
        {
            IList<PromptReportRecord> records = new List<PromptReportRecord>();
            DataRowReader reader = new DataRowReader(rows);
            while (reader.Read())
            {
                records.Add(Build(reader, reader.ToInteger("ClientNum"), reader.ToString("ClientName")));
            }
            return records;
        }

        private static PromptReportRecord Build(DataRowReader reader, int clientNumber, string clientName)
        {
            return new PromptReportRecord(reader.ToInteger("TrnID"),
                                          clientNumber,
                                          clientName, 
                                          reader.ToInteger("CustomerID"),
                                          reader.ToInteger("CustNum"),
                                          reader.ToString("Customer"),
                                          reader.ToDate("Dated"),
                                          reader.ToString("Reference"),
                                          reader.ToDecimal("Amount"),
                                          reader.ToDate("Factored"),
                                          reader.ToInteger("BatchID"),
                                          TransactionStatus.Parse(reader.ToInteger("TypeStatusID")),
                                          reader.ToDecimal("Balance"),
                                          reader.ToInteger("Age"),
                                          reader.ToString("TrnNumber"),
                                          TransactionType.Parse(reader.ToInteger("TransTypeID")));
        }
    }
}
