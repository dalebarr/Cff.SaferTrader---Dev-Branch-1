using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class CreditsClaimedBuilder
    {
        public IList<ClaimedCredit> Build(DataTable dataTable)
        {
            IList<ClaimedCredit> creditsClaimed = new List<ClaimedCredit>();
            var reader = new DataRowReader(dataTable.Rows);
            while (reader.Read())
            {
                creditsClaimed.Add(Build(reader));
            }
            return creditsClaimed;
        }

        private static ClaimedCredit Build(DataRowReader reader)
        {
            return new ClaimedCredit(reader.ToInteger("CustId"), reader.ToInteger("CustNum"),
                                    reader.ToString("Customer"),
                                    reader.ToString("TrnNum"),
                                    reader.ToDate("DateReceived"),
                                    reader.ToDecimal("Amount"),
                                    reader.ToDecimal("theSum"),
                                    reader.ToInteger("BatchID"),
                                    reader.ToDate("Created"),
                                    reader.ToDate("Modified"),
                                    reader.ToString("ModifiedBy"));
        }
    }
}
