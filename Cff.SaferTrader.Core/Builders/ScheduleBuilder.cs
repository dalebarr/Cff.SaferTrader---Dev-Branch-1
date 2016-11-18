using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class ScheduleBuilder
    {
        public BatchSchedule Build(DataTable batchTable, BatchScheduleFinanceInfo batchScheduleFinanceInfo, ClientAttribute clientAttribute)
        {
BatchSchedule batchSchedule = null;
    var reader = new DataRowReader(batchTable.Rows);
            if (reader.Read())
            {
                batchSchedule=new BatchSchedule(reader.ToInteger("BatchNumber"),
                                                    reader.ToString("txtStatus"),
                                                    reader.ToShort("Status"),
                                                    reader.ToNullableDate("BatchDate"),
                                                    reader.ToNullableDate("Released"),
                                                    reader.ToNullableDate("Modified"),
                                                    reader.ToNullableDate("DtFinished"),
                                                    reader.ToString("Header"),
                                                    reader.ToString("BNotes"),
                                                    reader.ToString("CreatedBy"),
                                                    reader.ToString("ModifiedBy"), batchScheduleFinanceInfo, clientAttribute);
            }
            return batchSchedule;
        }
    }
}