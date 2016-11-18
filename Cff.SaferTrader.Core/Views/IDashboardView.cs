namespace Cff.SaferTrader.Core.Views
{
    public interface IDashboardView
    {
        string CustomerIdQueryString { get; }
        string ClientIdQueryString { get; }
        string UserQueryString { get; }
        string ViewIDQueryString { get; }
    }
}