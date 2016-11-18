using System;
using System.Web.UI;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class MessageBox : ModalBox
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTitle)) MessageTitleLiteral.Text = MessageTitle;
            if (!string.IsNullOrEmpty(Message)) MessageLiteral.Text = Message;
            Hide();
        }

        protected void CancelButton_Click(object sender, ImageClickEventArgs e)
        {
            Hide();
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Display()
        {
            Visible = true;

            BlockScreen();
        }

        public string MessageTitle { set; get; }
        public string Message { set; get; }
    }
}