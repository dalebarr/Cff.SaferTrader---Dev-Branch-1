using System.Collections.Generic;
using System.Linq;

namespace Cff.SaferTrader.Core
{
    public static class RecordLimiter
    {
        public static IList<T> ReturnMaximumRecords<T>(IList<T> records)
        {
            ArgumentChecker.ThrowIfNull(records, "records");
            return records.Take(1000).ToList();
        }
    }
}