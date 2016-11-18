namespace Cff.SaferTrader.Core
{
    public interface IReader
    {
        bool Read();
        string ToString(string fieldName);
        int FromBigInteger(string fieldName);
        Date ToDate(string fieldName);
        short ToSmallInteger(string fieldName);
    }
}