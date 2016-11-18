using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Cff.SaferTrader.Web.UserControls
{
    public abstract class ModalBox : UserControl
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (Visible)
            {
                BlockScreen();
            }
        }

        protected void BlockScreen()
        {
            HtmlGenericControl blockUI = new HtmlGenericControl("div");
            blockUI.Attributes.Add("class", "blockUI");
            Controls.Add(blockUI);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "expandBlockUI", "expandBlockUI();", true);
        }
    }
}