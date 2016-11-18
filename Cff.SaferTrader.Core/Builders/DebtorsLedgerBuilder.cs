using System;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class DebtorsLedgerBuilder
    {
        public DebtorsLedger Build(DataRowCollection rows)
        {
            ArgumentChecker.ThrowIfNull(rows, "rows");

            decimal current = 0;
            decimal oneMonth = 0;
            decimal twoMonths = 0;
            decimal threeMonthsAndOver = 0;
            DataRowReader reader = new DataRowReader(rows);
            if (reader.Read())
            {
                current = reader.ToDecimal("Current_");
                oneMonth = reader.ToDecimal("One_Month");
                twoMonths = reader.ToDecimal("Two_Month");
                threeMonthsAndOver = reader.ToDecimal("Three_Month");
            }
            return new DebtorsLedger(current,
                                     oneMonth,
                                     twoMonths,
                                     threeMonthsAndOver);
        }
    }
}