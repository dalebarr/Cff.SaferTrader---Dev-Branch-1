using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface IClientSearchView
    {
        void DisplayMatchedClientNameAndNum(string matchedClientsJSON);
    }
}