using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class CffGridViewRowClickedEventArgs : EventArgs
    {
        private GridViewRow _row;
     
        public CffGridViewRowClickedEventArgs(GridViewRow row)
            : base() 
        {
            this._row = row;
        }

        public GridViewRow Row
        {
            get
            {
                return _row;
            }
        }
    }

}