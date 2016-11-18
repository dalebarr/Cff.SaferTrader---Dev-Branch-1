using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CreditStopSuggestionsReportRecordBuilder
    {
        public IList<CreditStopSuggestionsReportRecord> BuildForAllClients(DataRowCollection rows)
        {
            IList<CreditStopSuggestionsReportRecord> records = new List<CreditStopSuggestionsReportRecord>();
            DataRowReader reader = new DataRowReader(rows);
            while (reader.Read())
            {
                CreditStopSuggestionsReportRecord record = Build(reader, reader.ToInteger("ClientNum"), reader.ToString("ClientName"));
                records.Add(record);
            }
            return records;
        }

        public IList<CreditStopSuggestionsReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName)
        {
            IList<CreditStopSuggestionsReportRecord> records = new List<CreditStopSuggestionsReportRecord>();
            DataRowReader reader = new DataRowReader(rows);
            while (reader.Read())
            {
                records.Add(Build(reader, clientNumber, clientName));
            }
            return records;
        }

        private static CreditStopSuggestionsReportRecord Build(DataRowReader reader, int clientNumber, string clientName)
        {
            // move this into the record?
            string firstName = reader.ToString("FName");
            string lastName = reader.ToString("LName");
            string contact;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                contact = firstName + " " + lastName;
            }
            else
            {
                contact = firstName + lastName;
            }

            return new CreditStopSuggestionsReportRecord(reader.ToInteger("CustomerID"),
                                                clientNumber,
                                                clientName,
                                                reader.ToInteger("CustNum"),
                                                reader.ToString("Customer"),
                                                reader.ToNullableDecimal("Current_"),
                                                reader.ToNullableDecimal("One_Month"),
                                                reader.ToNullableDecimal("Two_Month"),
                                                reader.ToNullableDecimal("Three_Month"),
                                                reader.ToDecimal("Balance"),
                                                reader.ToDecimal("CreditLimit"),
                                                reader.ToDate("NextCall"),
                                                contact,
                                                reader.ToString("Phone"),
                                                reader.ToString("Cell"),
                                                reader.ToString("Email"));
        }
    }
}
