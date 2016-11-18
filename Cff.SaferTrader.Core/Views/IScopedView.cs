namespace Cff.SaferTrader.Core.Views
{
    public interface IScopedView
    {
        ICffClient Client { get; set; }
        ICffCustomer Customer { get; set; }
        //CffCustomer Customer { get; set; }
    }
}