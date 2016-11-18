using DevExpress.Web.ASPxGridView;

namespace Cff.SaferTrader.Web.UserControls
{
    public class ReadOnlyCffGridView : ASPxGridView
    {
        public ReadOnlyCffGridView()
        {
            SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            SettingsBehavior.AllowSort = false;
            SettingsBehavior.AllowDragDrop = false;
        }
    }
}