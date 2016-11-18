using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CreditLineBuilder
    {
        public IList<CreditLine> Build(DataTable dataTable)
        {
            ArgumentChecker.ThrowIfNull(dataTable, "dataTable");
            
            IList<CreditLine> creditLines = new List<CreditLine>();

            var reader = new DataRowReader(dataTable.Rows);
            while (reader.Read())
            {
                creditLines.Add(Build(reader));
            }

            return creditLines;
        }
        
        private static CreditLine Build(DataRowReader reader)
        {           
            return new CreditLine(reader.ToInteger("TrueTrnID"), reader.ToInteger("CustID"),
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
