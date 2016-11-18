using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class OverdueChargesReportRecordBuilder
    {
        public IList<OverdueChargesReportRecord> BuildFactoredInvoicesForAllClients(DataTableCollection dataTables)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1  AND Charges <> 0 AND (TypeStatus = 0 Or TypeStatus = 4)";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactionsForAllClients(new DataRowReader(dataRowCollection));
        }

        public IList<OverdueChargesReportRecord> BuildNonFactoredInvoicesForAllClients(DataTableCollection dataTables)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1  AND Charges <> 0 AND (TypeStatus = 1 Or TypeStatus = 2) AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactionsForAllClients(new DataRowReader(dataRowCollection));
        }

        public IList<OverdueChargesReportRecord> BuildForAllClients(DataTableCollection dataTables)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1  AND Charges <> 0 AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactionsForAllClients(new DataRowReader(dataRowCollection));
        }

        public IList<OverdueChargesReportRecord> Build(DataTableCollection dataTables, int clientNumber, string clientName)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1  AND Charges <> 0 AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactions(new DataRowReader(dataRowCollection), clientNumber, clientName);
        }

        public IList<OverdueChargesReportRecord> BuildFactoredInvoices(DataTableCollection dataTables, int clientNumber, string clientName)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1  AND Charges <> 0 AND (TypeStatus = 0 Or TypeStatus = 4)";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactions(new DataRowReader(dataRowCollection), clientNumber, clientName);
        }

        public IList<OverdueChargesReportRecord> BuildNonFactoredInvoices(DataTableCollection dataTables, int clientNumber, string clientName)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1  AND Charges <> 0 AND (TypeStatus = 1 Or TypeStatus = 2) AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactions(new DataRowReader(dataRowCollection), clientNumber, clientName);
        }
        
        private static IList<OverdueChargesReportRecord> BuildTransactions(DataRowReader reader, int clientNumber, string clientName)
        {
            IList<OverdueChargesReportRecord> records = new List<OverdueChargesReportRecord>();

            while (reader.Read())
            {
                records.Add(BuildTransaction(reader, clientNumber, clientName));
            }
            return records;
        }

        private static IList<OverdueChargesReportRecord> BuildTransactionsForAllClients(DataRowReader reader)
        {
            IList<OverdueChargesReportRecord> records = new List<OverdueChargesReportRecord>();

            while (reader.Read())
            {
                records.Add(BuildTransaction(reader, reader.ToInteger("ClientNum"), reader.ToString("ClientName")));
            }
            return records;
        }

        private static OverdueChargesReportRecord BuildTransaction(DataRowReader reader, int clientNumber, string clientName)
        {           
            return new OverdueChargesReportRecord(clientNumber, clientName, 
                                          reader.ToInteger("CustomerID"),
                                          reader.ToInteger("CustNum"),
                                          reader.ToString("Customer"),
                                          reader.ToString("Title"),
                                          reader.ToDate("Factored"),
                                          reader.ToInteger("Age"),
                                          reader.ToString("Number"),
                                          reader.ToString("Reference"),
                                          reader.ToDecimal("Charges"), 
                                          reader.ToDecimal("ChargesInclGST"),
                                          reader.ToDecimal("Amount"),
                                          reader.ToDecimal("Balance"), 
                                          //TransactionStatus.Parse(reader.ToInteger("TypeStatus")));  
                                          reader.ToString("Status"));  
        }
    }
}
