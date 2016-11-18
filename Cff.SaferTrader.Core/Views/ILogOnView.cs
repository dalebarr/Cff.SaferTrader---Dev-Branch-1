namespace Cff.SaferTrader.Core.Views
{
    public interface ILogOnView
    {
        void Redirect(string targetUrl);
        void RedirectToAgreement();
    }

    public interface IAccountToAccess
    {
    }

    public interface IAgreementPageView
    {
    }
}
