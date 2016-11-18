using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface ILikelyRepurchasesTabView 
    {
        void DisplayLikelyRepurchases(IList<LikelyRepurchasesLine> likelyRepurchasesLine);
    }
}
