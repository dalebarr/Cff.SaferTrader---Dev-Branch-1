using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface IBatchChargesTabView
    {
        void DisplayBatchCharges(IList<Charge> charges);
    }
}