using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Cff.SaferTrader.Core
{
    public static class OrderByHelper
    {
        public static IList<T> Sort<T>(IList<T> records, string orderByString)
        {
            if (AllClientsOrderByType.IsValid(orderByString)||ClientOrderByType.IsValid(orderByString))
            {
                return records.AsQueryable().OrderBy(orderByString).ToList();
            }
            throw new InvalidOperationException("Unknown AllClientsOrderByType Or ClientOrderByType QueryString");
        }
    }
}