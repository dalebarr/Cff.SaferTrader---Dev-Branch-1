namespace Cff.SaferTrader.Web.UserControls
{
    public abstract class ExportableGridPanel : ExportablePanel
    {
        public abstract void ShowAllRecords();
        public abstract void ShowPager();
        public abstract void ResetPaginationAndFocus();
        public abstract bool IsViewAllButtonRequired();
    }
}