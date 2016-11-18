using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface ITransactionRepository
    {
        IList<Transaction> LoadCurrentTransactions(int customerId);
        IList<Transaction> LoadCurrentTransactionsInvoices(int customerId);
        IList<ArchivedTransaction> LoadTransactionArchive(DateRange dateRange, int customerId, bool bInvoicesOnly);
        IList<HistoricalTransaction> LoadTransactionHistory(DateRange dateRange, int customerId,bool bInvoicesOnly);
        IList<Transaction> LoadTransactionHistoryDetails(Date date, int customerId);
        IList<HistoricalTransaction> LoadTransactionHistoryForAllClients(DateRange dateRange, FacilityType facilityType, bool isSalvageIncluded);
        IList<HistoricalTransaction> LoadTransactionHistoryForClient(DateRange dateRange, int clientId);
    }
}