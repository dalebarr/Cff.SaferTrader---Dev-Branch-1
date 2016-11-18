using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views 
{
    public interface IChargesTabView
    {
        void DisplayCharges(IList<Charge> charges);
    }
}