using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class RepurchasesLineBuilder
    {
        public IList<RepurchasesLine> Build(DataTable dataTable)
        {
            ArgumentChecker.ThrowIfNull(dataTable, "dataTable");

            IList<RepurchasesLine> repurchasesLines = new List<RepurchasesLine>();

            var reader = new DataRowReader(dataTable.Rows);
            while (reader.Read())
            {
                repurchasesLines.Add(Build(reader));
            }

            return repurchasesLines;
        }

        private static RepurchasesLine Build(DataRowReader reader)
        {
            return new RepurchasesLine(reader.ToInteger("TrueTrnID"), reader.ToInteger("CustID"),
                                       reader.ToInteger("CustNum"),
                                       reader.ToString("Customer"),
                                       reader.ToString("TrnNum"),
                                       reader.ToDate("DateReceived"),
                                       reader.ToDecimal("Amount"),
                                       reader.ToDecimal("theSum"),
                                       reader.ToInteger("BatchId"),
                                       reader.ToDate("Created"),
                                       reader.ToDate("Modified"),
                                       reader.ToString("ModifiedBy"));
        }
    }
}