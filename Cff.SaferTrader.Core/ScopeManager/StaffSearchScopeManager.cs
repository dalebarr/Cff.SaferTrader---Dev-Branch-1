using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.ScopeManager
{
    public class StaffSearchScopeManager : ISearchScopeManager
    {
        private readonly Scope scope;

        public StaffSearchScopeManager(Scope scope)
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
                searchScopes.Add(SearchScope.AllClients, "All Clients");
            }
            else if (scope == Scope.ClientScope)
            {
                searchScopes.Add(SearchScope.AllCustomers, "All Customers");
                searchScopes.Add(SearchScope.AllClients, "All Clients");
            }
            else
            {
                searchScopes.Add(SearchScope.AllClients, "All Clients");
            }
            return searchScopes;
        }
    }
}