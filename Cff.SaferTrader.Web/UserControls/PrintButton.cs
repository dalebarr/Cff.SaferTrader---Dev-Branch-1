using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls
{
    public class PrintButton: ImageButton
    {
        public PrintButton()
        {
            base.AlternateText = "Print";
            base.ImageUrl = "~/images/btn_print_new.png";
        }
    }
}