using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cff.SaferTrader.Web.UserControls
{
    public class ReportGridViewCustomButtonCallbackEventArgs : EventArgs
    {
        public ReportGridViewCustomButtonCallbackEventArgs(string buttonID, int visibleIndex) { }

        public string ButtonID { get; set; }

        public int VisibleIndex { get; set; }
    }
}