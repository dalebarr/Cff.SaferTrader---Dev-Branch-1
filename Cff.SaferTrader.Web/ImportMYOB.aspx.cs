using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.App_GlobalResources;
using Cff.SaferTrader.Core;
//using System.Web.UI.WebControls;
using System.Collections.Specialized;


namespace Cff.SaferTrader.Web
{
    public partial class ImportMYOB : System.Web.UI.Page
    {
        public string ParamRefQueryString
        {
            get { return Request.QueryString[QueryString.LogonParam.ToString()]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["clientID"] != null)
            {
                //ViewState.Add("downloadFileName", "test.txt");
                //String xmlStr = "&lt;MYOB&gt;";
                //xmlStr += "MYOB TEST";
                //xmlStr += "&lt;/MYOB&gt;";
                this.Page.Response.Write(Request.QueryString.ToString());
            }
            
        }
    }
}
