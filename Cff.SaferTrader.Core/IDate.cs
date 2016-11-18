namespace Cff.SaferTrader.Core
{
    public interface IDate
    {
        bool HasValue { get; }
        Date Value { get;}
    }
}