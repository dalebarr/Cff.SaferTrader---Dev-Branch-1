using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class ReleaseSummaryBuilder
    {
        public ReleaseSummary Build(DataRowCollection rows)
        {
            decimal fundedTransactionRelease = 0;
            decimal nonFundedTransactionRelease = 0;

            DataRowReader reader = new DataRowReader(rows);
            if (reader.Read())
            {
                fundedTransactionRelease = reader.ToDecimal("SumRetentionAmtFunded") * -1;
                nonFundedTransactionRelease = reader.ToDecimal("SumRetentionAmtNF") * -1;
            }
            return new ReleaseSummary(fundedTransactionRelease, nonFundedTransactionRelease);
        }
    }
}