using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CallsDueReportRecordBuilder
    {
        public IList<CallsDueReportRecord> Build(DataRowCollection dataRowCollection, int clientNumber,
                                                 string clientName)
        {
            IList<CallsDueReportRecord> callsDueReportRecords = new List<CallsDueReportRecord>();

            var reader = new DataRowReader(dataRowCollection);
            while (reader.Read())
            {
                callsDueReportRecords.Add(Build(reader, clientNumber, clientName));
            }

            return callsDueReportRecords;
        }

        private static CallsDueReportRecord Build(DataRowReader reader, int clientNumber, string clientName)
        {
            return new CallsDueReportRecord(clientNumber, clientName,
                                            reader.ToInteger("CustomerID"),
                                            reader.ToInteger("CustNum"),
                                            reader.ToString("Customer"),
                                            reader.ToString("Title"),
                                            reader.ToDate("Due"),
                                            reader.ToDecimal("Balance"),
                                            reader.ToString("Age"),
                                            reader.ToString("FName"),
                                            reader.ToString("LName"),
                                            reader.ToString("Phone"),
                                            reader.ToString("Cell"),
                                            reader.ToString("Fax")
                );
        }

        public IList<CallsDueReportRecord> Build(DataRowCollection dataRowCollection)
        {
            IList<CallsDueReportRecord> callsDueReportRecords = new List<CallsDueReportRecord>();

            var reader = new DataRowReader(dataRowCollection);
            while (reader.Read())
            {
                callsDueReportRecords.Add(Build(reader));
            }
            return  callsDueReportRecords;
        }

        private static CallsDueReportRecord Build(DataRowReader reader)
        {
            return new CallsDueReportRecord(reader.ToInteger("ClientNum"), reader.ToString("ClientName"),
                                            reader.ToInteger("CustomerID"),
                                            reader.ToInteger("CustNum"),
                                            reader.ToString("Customer"),
                                            reader.ToString("Title"),
                                            reader.ToDate("Due"),
                                            reader.ToDecimal("Balance"),
                                            reader.ToString("Age"),
                                            reader.ToString("FName"),
                                            reader.ToString("LName"),
                                            reader.ToString("Phone"),
                                            reader.ToString("Cell"),
                                            reader.ToString("Fax")
                );
        }
    }
}