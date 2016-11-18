using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core.Views
{
    public interface ICreditsClaimedTabView
    {
        void DisplayCreditsClaimed(IList<ClaimedCredit> creditsClaimed);
    }
}
