using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Web;
using Cff.SaferTrader.Web.UserControls;

namespace Cff.SaferTrader.Web.UserControls.Interfaces
{
    public interface IBatchTab 
    {
        void LoadTab(InvoiceBatch invoiceBatch);
        void ClearTabData();
    }
}
