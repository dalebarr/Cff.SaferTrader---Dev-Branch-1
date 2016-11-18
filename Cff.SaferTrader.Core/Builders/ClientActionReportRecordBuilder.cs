using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class ClientActionReportRecordBuilder
    {
        public IList<ClientActionReportRecord> Build(DataRowCollection dataRowCollection, int clientNumber,
                                                     string clientName)
        {
            IList<ClientActionReportRecord> callsDueReportRecords = new List<ClientActionReportRecord>();

            var reader = new DataRowReader(dataRowCollection);
            while (reader.Read())
            {
                callsDueReportRecords.Add(Build(reader, clientNumber, clientName));
            }

            return callsDueReportRecords;
        }

        private static ClientActionReportRecord Build(DataRowReader reader, int clientNumber, string clientName)
        {
            return new ClientActionReportRecord(clientNumber, clientName, reader.ToInteger("CustomerID"),
                                                reader.ToInteger("CustNum"),
                                                reader.ToString("Customer"),
                                                reader.ToString("Title"),
                                                reader.ToDecimal("Balance"),
                                                reader.ToString("msg"),
                                                reader.ToString("Age"),
                                                reader.ToDate("Due")
                );
        }

        public IList<ClientActionReportRecord> Build(DataRowCollection dataRowCollection)
        {
            IList<ClientActionReportRecord> callsDueReportRecords = new List<ClientActionReportRecord>();

            DataRowReader reader = new DataRowReader(dataRowCollection);
            while (reader.Read())
            {
                callsDueReportRecords.Add(Build(reader));
            }

            return callsDueReportRecords;
        }

        private static ClientActionReportRecord Build(DataRowReader reader)
        {
            return new ClientActionReportRecord(reader.ToInteger("ClientNum"),
                                                reader.ToString("ClientName"), reader.ToInteger("CustomerID"),
                                                reader.ToInteger("CustNum"),
                                                reader.ToString("Customer"),
                                                reader.ToString("Title"),
                                                reader.ToDecimal("Balance"),
                                                reader.ToString("msg"),
                                                reader.ToString("Age"),
                                                reader.ToDate("Due")
                );
        }
    }
}