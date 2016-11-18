using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class ClaimedRetentionRepurchaseBuilder
    {
        public IList<ClaimedRetentionRepurchase> Build(DataTable dataTable)
        {
            IList<ClaimedRetentionRepurchase> claimedRetentionRepurchases = new List<ClaimedRetentionRepurchase>();
            var reader = new DataRowReader(dataTable.Rows);
            while (reader.Read())
            {
                claimedRetentionRepurchases.Add(Build(reader));
            }
            return claimedRetentionRepurchases;
        }

        private static ClaimedRetentionRepurchase Build(DataRowReader reader)
        {
            return new ClaimedRetentionRepurchase(reader.ToInteger("CustID"), reader.ToInteger("CustNum"),
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