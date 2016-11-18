namespace Cff.SaferTrader.Core
{
    public interface ICalendar
    {
        Date Now { get; }
        Date Today { get; }
    }
}