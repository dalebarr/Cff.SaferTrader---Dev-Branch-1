namespace Cff.SaferTrader.Core.Views
{
    public interface IMainNavigationView
    {
        void ToggleCurrentTransactionsLink(bool visible);
        void ToggleTransactionsArchiveLink(bool visible);
        void ToggleTransactionsHistoryLink(bool visible);
        void ToggleContactsLink(bool visible);
        void ToggleReleaseTab(bool visible);

        CffPrincipal CurrentPrincipal { get; }
    }
}