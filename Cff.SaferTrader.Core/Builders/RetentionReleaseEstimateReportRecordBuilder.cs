using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class RetentionReleaseEstimateReportRecordBuilder
    {
        public IList<RetentionReleaseEstimateReportRecord> Build(DataTable table)
        {
            ArgumentChecker.ThrowIfNull(table, "table");

            DataView recordView = new DataView(table);
            recordView.Sort = "TypeStatus, Customer";
            recordView.RowFilter = "TransTypeID = 1 AND Retention <> 0";

            IList<RetentionReleaseEstimateReportRecord> records = new List<RetentionReleaseEstimateReportRecord>();
            var reader = new DataRowReader(recordView.ToTable().Rows);
            while (reader.Read())
            {
                records.Add(Build(reader));
            }
            return records;
        }

        private static RetentionReleaseEstimateReportRecord Build(DataRowReader reader)
        {
            return new RetentionReleaseEstimateReportRecord(reader.ToInteger("TrueTrnID"),
                                                            reader.ToInteger("CustomerId"),
                                                            reader.ToInteger("CustNum"),
                                                            reader.ToString("Customer"),
                                                            reader.ToDate("Dated"),
                                                            reader.ToString("Number"),
                                                            reader.ToString("Reference"), 
                                                            reader.ToDecimal("Amount"),
                                                            reader.ToDecimal("O_Balance"), 
                                                            reader.ToDecimal("Balance"),
                                                            reader.ToDecimal("Retention"), 
                                                            TransactionStatus.Parse(reader.ToInteger("TypeStatus")));
        }
    }
}