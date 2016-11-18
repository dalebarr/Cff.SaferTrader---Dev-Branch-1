using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.ScopeManager
{
    public class ClientStaffSearchScopeManager : ISearchScopeManager
    {
         private readonly Scope scope;

         public ClientStaffSearchScopeManager(Scope scope)
        {
            ArgumentChecker.ThrowIfNull(scope, "scope");

            this.scope = scope;
        }

        public Dictionary<SearchScope, string> LoadSearchScope()
        {
            Dictionary<SearchScope, string> searchScopes = new Dictionary<SearchScope, string>();
            if (scope == Scope.CustomerScope)
            {
                searchScopes.Add(SearchScope.CurrentCustomer, "Current Customer");
                searchScopes.Add(SearchScope.AllCustomers, "All Customers");
            }
            else if (scope == Scope.ClientScope)
            {
                searchScopes.Add(SearchScope.AllCustomers, "All Customers");
            }
            
            return searchScopes;
        }
    }
}