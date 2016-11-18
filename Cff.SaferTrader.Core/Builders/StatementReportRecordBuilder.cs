using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class StatementReportRecordBuilder
    {
        private readonly DataTable table;

        public StatementReportRecordBuilder(DataTable table)
        {
            this.table = table;
        }

        public IList<StatementReportRecord> Build()
        {
            // Exclude Repurchase Transactions
            DataView dataView = new DataView(table);
            dataView.RowFilter = "TransTypeID <> 5";
            DataRowCollection rows = dataView.ToTable().Rows;
            DataRowReader reader = new DataRowReader(rows);

            IList<StatementReportRecord> records = new List<StatementReportRecord>();
            while (reader.Read())
            {
                StatementReportRecord record = new StatementReportRecord(reader.ToDate("Dated"),
                                                                         reader.ToString("Type"),
                                                                         reader.ToString("Number"),
                                                                         reader.ToString("Reference"),
                                                                         reader.ToDecimal("Amount"), 
                                                                         reader.ToInteger("Age"),
                                                                         reader.ToInteger("TransTypeID"));      //MSarza [20151007]
                records.Add(record);
            }
            return records;
        }
    }
}