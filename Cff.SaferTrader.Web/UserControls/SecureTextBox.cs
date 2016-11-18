using System;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace Cff.SaferTrader.Web.UserControls
{
    /// <summary>
    /// TextBox secured with HTML encoding
    /// </summary>
    [SupportsEventValidation]
    public class SecureTextBox : System.Web.UI.WebControls.TextBox
    {
        #pragma warning disable
        [Obsolete("Use EncodedText property to allow HTML encoding")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        #pragma warning restore

        /// <summary>
        /// Returns Text HTML encoded. Set HTML decodes value.
        /// </summary>
        public string EncodedText
        {
            get
            {
                // Ensures that Text returned to server is HTML encoded so that any control can render it safely
                return System.Web.HttpUtility.HtmlEncode(base.Text);
            }

            set
            {
                // Ensures that Text displayed inside the control is HTML decoded for correct rendering
                base.Text = System.Web.HttpUtility.HtmlDecode(value);
            }
        }
    }
}