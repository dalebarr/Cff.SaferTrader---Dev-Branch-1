using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.ScopeManager
{
    public class CustomerSearchScopeManager : ISearchScopeManager
    {
        public Dictionary<SearchScope, string> LoadSearchScope()
        {
            Dictionary<SearchScope, string> searchScopes = new Dictionary<SearchScope, string>();
            searchScopes.Add(SearchScope.CurrentCustomer, "Current Customer");
            return searchScopes;
        }
    }
}