using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface IRetentionRepurchasesClaimedTabView
    {
        void DisplayRetentionRepurchasesClaimed(IList<ClaimedRetentionRepurchase> claimedRetentionRepurchases);
    }
}