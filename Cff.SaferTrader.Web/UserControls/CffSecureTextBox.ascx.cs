using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class CffSecureTextBox : System.Web.UI.UserControl
    {
        private string _text;
        private string _textMode;
        private string _runat;

        private string _id;
        private string _cssClass;

        public event EventHandler OnTextChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
          

            this.TextBox1.Text = this._text;

            if (!string.IsNullOrEmpty(this._textMode))
                this.TextBox1.Attributes.Add("TextMode", _textMode);

            if (!string.IsNullOrEmpty(this._runat))
                this.TextBox1.Attributes.Add("runat", this._runat);

            this.TextBox1.CssClass = this._cssClass;

            this._id = this.ID;
        }

        public string Text
        {
            get { return this.TextBox1.Text;  }
            set { this._text = value;  }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.OnTextChanged != null)
                this.OnTextChanged(this, e);
        }


        public string TextMode
        {
            get { return this.TextBox1.TextMode.ToString(); }
            set {   this._textMode = value;  }
        }

        public string runat
        {
            set { this._runat = value;  }
        }

      
        public string CssClass
        {
            get { return this.TextBox1.CssClass; }
            set { this._cssClass = value; }
        }

        public string EncodedText
        {
            get
            {
                // Ensures that Text returned to server is HTML encoded so that any control can render it safely
                return System.Web.HttpUtility.HtmlEncode(this.TextBox1.Text);
            }

            set
            {
                // Ensures that Text displayed inside the control is HTML decoded for correct rendering
                this.TextBox1.Text = System.Web.HttpUtility.HtmlDecode(value);
            }
        }

    }
}