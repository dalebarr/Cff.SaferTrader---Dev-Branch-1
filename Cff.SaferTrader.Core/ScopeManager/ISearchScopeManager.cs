using System.Collections.Generic;

namespace Cff.SaferTrader.Core.ScopeManager
{
    public interface ISearchScopeManager
    {
        Dictionary<SearchScope, string> LoadSearchScope();
    }
}