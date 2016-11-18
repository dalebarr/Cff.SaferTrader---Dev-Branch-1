using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class MainNavigationPresenter
    {
        private readonly IMainNavigationView view;
        private readonly ISecurityManager securityManager;

        public MainNavigationPresenter(IMainNavigationView view, ISecurityManager securityManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            this.view = view;
            this.securityManager = securityManager;
        }

        public void LockDown()
        {
            view.ToggleCurrentTransactionsLink(securityManager.CanViewCurrentTransactionsLink());
            view.ToggleTransactionsArchiveLink(securityManager.CanViewTransactionArchiveLink());

            view.ToggleTransactionsHistoryLink(securityManager.CanViewTransactionHistoryLink());
            view.ToggleContactsLink(securityManager.CanViewContactsLink());

            view.ToggleReleaseTab(securityManager.CanViewReleaseTab());
        }
    }
}