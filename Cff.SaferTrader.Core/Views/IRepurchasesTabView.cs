using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface IRepurchasesTabView
    {
        void DisplayRepurchases(IList<RepurchasesLine> repurchasesLine);
    }
}