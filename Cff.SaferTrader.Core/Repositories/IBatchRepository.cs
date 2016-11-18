using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface IBatchRepository
    {
        IList<InvoiceBatch> LoadInvoiceBatchesForDateRange(int clientId, BatchType batchType, DateRange dateRange);
        IList<Invoice> LoadInvoicesFor(int clientId, int batchId);
        IList<Invoice> LoadNonFactoredInvoicesFor(int clientId, int batchId);
        IList<CreditLine> LoadCreditLinesFor(int clientId, int batchId);
        
        IList<RepurchasesLine> LoadRepurchasesLinesFor(int clientId, int batchId);
        IList<LikelyRepurchasesLine> LoadLikelyRepurchasesLinesFor(int clientId, int batchId, int custId, int userID, string strAsAt);

        BatchSchedule LoadBatchScheduleFor(int clientId, int batchId, ChargeCollection charges);
        ChargeCollection LoadBatchCharges(int batchId);
        IList<InvoiceBatch> LoadInvoiceBatchesFor(int clientId, BatchType batchType);
        IList<InvoiceBatch> LoadInvoiceBatchesForBatchNumber(int clientId, string batchNumberToSearch);
    }
}