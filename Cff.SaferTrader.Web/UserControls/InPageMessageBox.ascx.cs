using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class InPageMessageBox : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Message)) MessageLabel.Text = Message;
        }

        public string Message { set; get; }
    }
}