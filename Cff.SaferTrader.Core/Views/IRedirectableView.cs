namespace Cff.SaferTrader.Core.Views
{
    public interface IRedirectableView : IScopedView
    {
        void RedirectTo(string redirectionPath);
    }
}