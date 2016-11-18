using System;

namespace Cff.SaferTrader.Web.UserControls
{
    public class ReportGridViewCustomCallbackEventArgs : EventArgs
    {

        public ReportGridViewCustomCallbackEventArgs(string parameters) { }

        public string Parameters { get; set; }

    }
}