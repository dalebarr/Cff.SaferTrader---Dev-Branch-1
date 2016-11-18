namespace Cff.SaferTrader.Core
{
    public class EndOfMonthHelper
    {
        public static int ParseRententionEndOfMonth(Date endOfMonth)
        {
            ArgumentChecker.ThrowIfNull(endOfMonth, "endOfMonth");
            string month = endOfMonth.Month.ToString();

            if (endOfMonth.Month < 10)
            {
                month = "0" + month;
            }

            return int.Parse("-" + endOfMonth.Year + month);
        }
    }
}