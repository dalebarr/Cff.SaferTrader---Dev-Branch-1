using System.Web.UI;

namespace Cff.SaferTrader.Web.Popups
{
    public abstract class NotesPopup : Page
    {
        internal void SetTitle(string pageName, string companyName)
        {
            Title = string.Format("{0} for {1}", pageName, companyName);
        }
    }
}