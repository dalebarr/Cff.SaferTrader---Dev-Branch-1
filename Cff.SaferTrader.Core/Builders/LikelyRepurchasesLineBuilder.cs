using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class LikelyRepurchasesLineBuilder
    {
       
        public IList<LikelyRepurchasesLine> Build(DataTable dataTable)
        {
            ArgumentChecker.ThrowIfNull(dataTable, "dataTable");

            IList<LikelyRepurchasesLine> likelyrepurchaseslines = new List<LikelyRepurchasesLine>();

            decimal theSum = 0;

            var reader = new DataRowReader(dataTable.Rows);
            while (reader.Read())
            {
                theSum += reader.ToDecimal("Balance");
                likelyrepurchaseslines.Add(Build(reader, theSum));
            }

            return likelyrepurchaseslines;
        }

        private static LikelyRepurchasesLine Build(DataRowReader reader, decimal theSum)
        {
            return new LikelyRepurchasesLine( reader.ToInteger("customerid"),
                                       reader.ToInteger("CustNum"),
                                       reader.ToString("Customer"),
                                       reader.ToString("title"),
                                       reader.ToInteger("Age"),
                                       reader.ToDecimal("amount"),
                                       reader.ToDecimal("Balance"),
                                       theSum,
                                       reader.ToDate("Dated"),
                                       reader.ToDate("Factored"), //processed
                                       reader.ToString("TrnID"),
                                       reader.ToString("TrnNumber"));
        }

    }
}
