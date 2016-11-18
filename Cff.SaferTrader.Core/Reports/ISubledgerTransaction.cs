namespace Cff.SaferTrader.Core.Reports
{
    public interface ISubledgerTransaction
    {
        void AddTo(Subledger subledger);
    }
}