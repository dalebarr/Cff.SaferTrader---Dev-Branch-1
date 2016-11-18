using System;
using System.Data;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class AgedBalancesReportRecordBuilder
    {
        public IList<AgedBalancesReportRecord> BuildForAllClients(DataRowCollection rows)
        {
            IList<AgedBalancesReportRecord> records = new List<AgedBalancesReportRecord>();
            DataRowReader reader = new DataRowReader(rows);
            while (reader.Read())
            {
                AgedBalancesReportRecord record = Build(reader, reader.ToInteger("ClientNum"), reader.ToString("ClientName"));
                records.Add(record);
            }
            return records;
        }

        public IList<AgedBalancesReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName)
        {
            string strNote = "";
            int custID1;
            bool isWithNotes = false;
            AgedBalancesReportRecord ABRecord = null;
            IList<AgedBalancesReportRecord> records = new List<AgedBalancesReportRecord>();
            DataRowReader reader = new DataRowReader(rows);

            reader.Read();

            try
            {
                custID1 = reader.ToInteger("customerID");
            }
            catch 
            {
                custID1 = -1;
            }
            if (custID1 > 0)
            { 
                try
                {
                    strNote = reader.ToString("Note");
                    isWithNotes = true;
                }
                catch (Exception e) 
                {
                    string sexc = e.Message;
                }

                if (isWithNotes)
                {
                    ABRecord = new AgedBalancesReportRecord(reader.ToInteger("CustomerID"),
                                                     clientNumber,
                                                     clientName,
                                                     reader.ToInteger("CustNum"),
                                                     reader.ToString("Customer"),
                                                     reader.ToNullableDecimal("Current_"),
                                                     reader.ToNullableDecimal("One_Month"),
                                                     reader.ToNullableDecimal("Two_Month"),
                                                     reader.ToNullableDecimal("Three_Month"),
                                                     reader.ToDecimal("Balance"),
                                                     reader.ToDate("NextCall"),
                                                     (reader.ToString("FName") + " " + reader.ToString("LName")).Trim(),
                                                     reader.ToString("Phone"),
                                                     reader.ToString("Cell"),
                                                     reader.ToString("Email"));
                    ABRecord.CustNoteList.Add(new CustomerNote(reader.ToString("Note"), reader.ToDate("created"), reader.ToString("createdby")));
                }
                else
                {
                    records.Add(Build(reader, clientNumber, clientName));
                }

                while (reader.Read())
                {
                    if (isWithNotes)
                    {
                        if (custID1 == reader.ToInteger("CustomerID"))
                        {
                            ABRecord.CustNoteList.Add(new CustomerNote(reader.ToString("Note"), reader.ToDate("created"), reader.ToString("createdby")));
                        }
                        else {
                            records.Add(ABRecord);
                            custID1 = reader.ToInteger("CustomerID");
                            ABRecord = new AgedBalancesReportRecord(custID1,
                                                     clientNumber,
                                                     clientName,
                                                     reader.ToInteger("CustNum"),
                                                     reader.ToString("Customer"),
                                                     reader.ToNullableDecimal("Current_"),
                                                     reader.ToNullableDecimal("One_Month"),
                                                     reader.ToNullableDecimal("Two_Month"),
                                                     reader.ToNullableDecimal("Three_Month"),
                                                     reader.ToDecimal("Balance"),
                                                     reader.ToDate("NextCall"),
                                                     (reader.ToString("FName") + " " + reader.ToString("LName")).Trim(),
                                                     reader.ToString("Phone"),
                                                     reader.ToString("Cell"),
                                                     reader.ToString("Email"));
                            ABRecord.CustNoteList.Add(new CustomerNote(reader.ToString("Note"), reader.ToDate("created"), reader.ToString("createdby")));
                        }
                    }
                    else
                    {
                        records.Add(Build(reader, clientNumber, clientName));
                    }
                }

                if (isWithNotes)
                {
                    records.Add(ABRecord);
                }
            }
            return records;
        }

        private static AgedBalancesReportRecord Build(DataRowReader reader, int clientNumber, string clientName)
        {
            //stored procedure returns FName, LName fields separately
            return new AgedBalancesReportRecord(reader.ToInteger("CustomerID"),
                                                clientNumber,
                                                clientName,
                                                reader.ToInteger("CustNum"),
                                                reader.ToString("Customer"),
                                                reader.ToNullableDecimal("Current_"),
                                                reader.ToNullableDecimal("One_Month"),
                                                reader.ToNullableDecimal("Two_Month"),
                                                reader.ToNullableDecimal("Three_Month"),
                                                reader.ToDecimal("Balance"),
                                                reader.ToDate("NextCall"),
                                                (reader.ToString("FName") + " " + reader.ToString("LName")).Trim(), //contact,
                                                reader.ToString("Phone"),
                                                reader.ToString("Cell"),
                                                reader.ToString("Email"));
        }
    }
}
