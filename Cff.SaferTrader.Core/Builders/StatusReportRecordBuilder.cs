using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class StatusReportRecordBuilder
    {
        public IList<StatusReportRecord> Build(DataTableCollection dataTables, int clientNumber, string clientName)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            //recordView.RowFilter = "TransTypeID = 1 AND Amount > 0";
            recordView.RowFilter = "(TransTypeID = 1 AND Amount > 0) Or (TransTypeID = 13 AND Balance <> 0)";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactions(new DataRowReader(dataRowCollection), clientNumber, clientName);
        }
        
        public IList<StatusReportRecord> BuildFactoredInvoices(DataTableCollection dataTables, int clientNumber, string clientName)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            //recordView.RowFilter = "TransTypeID = 1 AND (TypeStatus = 0 Or TypeStatus = 4)";
            recordView.RowFilter = "(TransTypeID = 1 AND (TypeStatus = 0 Or TypeStatus = 4)) Or (TransTypeID = 13 AND Balance <> 0)";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactions(new DataRowReader(dataRowCollection), clientNumber, clientName);
        }

        public IList<StatusReportRecord> BuildNonFactoredInvoices(DataTableCollection dataTables, int clientNumber, string clientName)
        {
            DataView recordView = new DataView(dataTables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1 AND (TypeStatus = 1 Or TypeStatus = 2) AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactions(new DataRowReader(dataRowCollection), clientNumber, clientName);
        }

        private static IList<StatusReportRecord> BuildTransactions(DataRowReader reader, int clientNumber, string clientName)
        {
            IList<StatusReportRecord> records = new List<StatusReportRecord>();
            
            while (reader.Read())
            {
                records.Add(BuildTransaction(reader, clientNumber, clientName)); 
            }
            return records;
        }

        private static StatusReportRecord BuildTransaction(DataRowReader reader, int clientNumber, string clientName)
        {
            return new StatusReportRecord(clientNumber, clientName, 
                                            reader.ToInteger("CustomerID"), 
                                            reader.ToInteger("CustNum"),
                                            reader.ToString("Customer"),
                                            reader.ToString("Title"),
                                            reader.ToNullableDate("Dated"),
                                            reader.ToNullableDate("Factored"),
                                            reader.ToInteger("Age"),
                                            reader.ToInteger("Batch"),
                                            reader.ToString("Number"),
                                            reader.ToString("Reference"),
                                            reader.ToDecimal("O_Balance"),
                                            reader.ToDecimal("Amount"),
                                            reader.ToDecimal("Receipts"),
                                            reader.ToDecimal("Credits"),
                                            reader.ToDecimal("Discounts"),
                                            reader.ToDecimal("Journals"),
                                            reader.ToDecimal("Repurchase"),
                                            reader.ToDecimal("Other"),
                                            reader.ToNullableDate("TrnsTrnDate"),
                                            reader.ToDecimal("Balance"),
                                            reader.ToDecimal("Retention"),
                                            reader.ToDecimal("Charges"),
                                            TransactionStatus.Parse(reader.ToInteger("TypeStatus")),
                                            reader.ToNullableDate("Repurchased"),
                                            reader.ToString("Type"));
        }

        public IList<StatusReportRecord> BuildForAllClients(DataTableCollection tables)
        {
            DataView recordView = new DataView(tables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1 AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactionsForAllClients(new DataRowReader(dataRowCollection));
        }

        public IList<StatusReportRecord> BuildFactoredInvoicesForAllClients(DataTableCollection tables)
        {
            DataView recordView = new DataView(tables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1 AND (TypeStatus = 0 Or TypeStatus = 4) AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactionsForAllClients(new DataRowReader(dataRowCollection));
        }

        public IList<StatusReportRecord> BuildNonFactoredInvoicesForAllClients(DataTableCollection tables)
        {
            DataView recordView = new DataView(tables[1]);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1 AND (TypeStatus = 1 Or TypeStatus = 2) AND Amount > 0";

            DataRowCollection dataRowCollection = recordView.ToTable().Rows;
            return BuildTransactionsForAllClients(new DataRowReader(dataRowCollection));
        }

        private static IList<StatusReportRecord> BuildTransactionsForAllClients(DataRowReader reader)
        {
            IList<StatusReportRecord> records = new List<StatusReportRecord>();

            while (reader.Read())
            {
                records.Add(BuildTransaction(reader, reader.ToInteger("ClientNum"), reader.ToString("ClientName")));
            }
            return records;
        }
    }
}
