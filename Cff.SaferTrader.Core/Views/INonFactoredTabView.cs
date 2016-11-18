using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core.Views
{
    public interface INonFactoredTabView
    {
        void DisplayNonFactoredInvoices(IList<Invoice> nonFactoredInvoices);
    }
}
