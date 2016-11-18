using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class BatchScheduleFinanceInfoBuilder
    {
        public BatchScheduleFinanceInfo Build(DataTable batchTable, ChargeCollection charges)
        {
            BatchScheduleFinanceInfo batchScheduleFinanceInfo=null;
            var reader = new DataRowReader(batchTable.Rows);
            if (reader.Read())
            {
                batchScheduleFinanceInfo = new BatchScheduleFinanceInfo(
                    reader.ToDecimal("FacInv"),
                    reader.ToDecimal("NFInv"),
                    reader.ToDecimal("Admin"),
                    reader.ToDecimal("AdminGST"),
                    reader.ToDecimal("FactorFee"),
                    reader.ToDecimal("Retention"),
                    reader.ToDecimal("Repurchase"),
                    reader.ToDecimal("Credit"),
                    reader.ToDecimal("Post"),
                    reader.ToDecimal("PostGST"),
                    reader.ToDecimal("CheckConfirm"),
                    reader.ToDecimal("TotalInv"), charges,
                    reader.ToInteger("FacilityType"),
                    reader.ToDecimal("NonCompliantFee"),
                    reader.ToDecimal("RetnPercent"));
            }
            return batchScheduleFinanceInfo;
        }
    }
}