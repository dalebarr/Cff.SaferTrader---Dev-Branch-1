using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Builders;
using DataEntry;
namespace Cff.SaferTrader.Core.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ICalendar calendar;
        private readonly Customer customer;
        private Clients client;

        public TransactionRepository(ICalendar calendar)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            this.calendar = calendar;
            customer = new Customer();
            client = new Clients();
        }
        #region ITransactionRepository Members

        public IList<Transaction> LoadCurrentTransactions(int customerId)
        {
            // WhichTrns = 0 : Current transactions
            // ArchiveLen = 84 : defaulting to 84 months
             DataTableCollection transactionDataTables = null;
            DataSet theDS = customer.getAllTransactions(customerId, "All", 0, 0, calendar.Today.ToYearMonthValue(), 84, calendar.Today.Value.DateTime);
            if (theDS!=null)
                if (theDS.Tables!=null)
                     transactionDataTables = theDS.Tables;
            
            IList<Transaction> transactions = new List<Transaction>();
            if (transactionDataTables != null) {
                DataRowReader reader = new DataRowReader(transactionDataTables[0].Rows);
                while (reader.Read())
                {
                    Transaction transaction = new TransactionBuilder(reader).Build();
                    transactions.Add(transaction);
                }
            }
           
            return transactions;
        }

        public IList<Transaction> LoadCurrentTransactionsInvoices(int customerId)
        {
            // WhichTrns = 1 : Current transactions Invoices Only
            // ArchiveLen = 84 : defaulting to 84 months
            DataTableCollection transactionDataTables = null;
            DataSet theDS = customer.getAllTransactions(customerId, "All", 1, 0, calendar.Today.ToYearMonthValue(), 0,
                                            calendar.Today.Value.DateTime);

            if (theDS != null)
               transactionDataTables = theDS.Tables;

            IList<Transaction> transactions = new List<Transaction>();
            if (transactionDataTables != null) {
                DataRowReader reader = new DataRowReader(transactionDataTables[0].Rows);
                while (reader.Read())
                {
                    string transType = reader.ToString("Type").Trim();
                    if (transType.ToLower() == "invoice")
                    {
                        Transaction transaction = new TransactionBuilder(reader).Build();
                        transactions.Add(transaction);
                    }
                }
            }
            return transactions;
        }

        public IList<ArchivedTransaction> LoadTransactionArchive(DateRange dateRange, int customerId, bool bInvoicesOnly)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            int whichTrns = 10;

            if (bInvoicesOnly == true) { whichTrns = 11; }

            DataTableCollection transactionDataTables = null;
            DataSet theDS = customer.getAllTransactions(customerId, "All", whichTrns, 0,
                                                dateRange.EndDate.ToYearMonthValue(),
                                                dateRange.NumberOfMonths,
                                                dateRange.EndDate.Value.DateTime);
            if (theDS!=null)
                if (theDS.Tables!=null)
                    transactionDataTables = theDS.Tables;

            IList<ArchivedTransaction> archivedTransactions = new List<ArchivedTransaction>();

            if (transactionDataTables != null) {
                DataRowReader reader = new DataRowReader(transactionDataTables[0].Rows);
                while (reader.Read())
                {
                    var transaction =
                        new ArchivedTransaction(reader.ToInteger("TrueTrnID"),
                                                reader.ToDate("Dated"),
                                                reader.ToNullableDate("LastRec"),
                                                reader.ToDate("Factored"),
                                                reader.ToString("Type").Trim(),
                                                reader.ToString("Number"),
                                                reader.ToString("Reference"),
                                                reader.ToDecimal("Amount"),
                                                reader.ToString("Status"),
                                                reader.ToInteger("Batch"),
                                                reader.ToString("CurrentTrnNotes"), //add 14072012 as per marty's request
                                                customerId);
                    archivedTransactions.Add(transaction);
                }
            }
            return archivedTransactions;
        }

        public IList<HistoricalTransaction> LoadTransactionHistory(DateRange dateRange, int customerId, bool bInvoicesOnly)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            short WhichType = 0;
            int asAt = 0;
            if (bInvoicesOnly == true) {
                WhichType = 5;
                asAt = 1;
            }

            DataSet theDS = customer.getCustomerHistory(customerId, WhichType, asAt,
                                                                                    dateRange.EndDate.ToYearMonthValue(),
                                                                                    dateRange.NumberOfMonths);
            if (theDS == null)
                return null;

            DataTableCollection transactionDataTables = theDS.Tables;
            DataRowReader reader = new DataRowReader(transactionDataTables[0].Rows);
            return new HistoricalTransactionBuilder(reader).Build();
        }

        public IList<Transaction> LoadTransactionHistoryDetails(Date date, int customerId)
        {
            ArgumentChecker.ThrowIfNull(date, "date");
            DataTableCollection transactionDataTables =
                customer.getCustomerHistory(customerId, 5, 0, date.ToYearMonthValue(), 0).Tables;
            IList<Transaction> transactions = new List<Transaction>();

            DataRowReader reader = new DataRowReader(transactionDataTables[0].Rows);
            while (reader.Read())
            {
                Transaction transaction = new TransactionBuilder(reader).BuildDetailTransaction();
                transactions.Add(transaction);
            }
            return transactions;
        }

        public IList<HistoricalTransaction> LoadTransactionHistoryForAllClients(DateRange dateRange, FacilityType facilityType, bool isSalvageIncluded)
        {
            int clientId = 0;
            if (isSalvageIncluded)
            {
                clientId = -50;
            }
            Date endDate = dateRange.EndDate;
            DataTableCollection transactionDataTables = client.getAllClientHistory(clientId, 10, endDate.IsThisMonth(calendar), 
                                                                                   endDate.ToYearMonthValue(), dateRange.NumberOfMonths, 
                                                                                   facilityType.Id).Tables;

            DataRowReader reader = new DataRowReader(transactionDataTables[0].Rows);
            return new HistoricalTransactionBuilder(reader).Build();
        }

        public IList<HistoricalTransaction> LoadTransactionHistoryForClient(DateRange dateRange, int clientId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");

            DataTableCollection transactionDataTables = null;
            
            DataSet theDS = client.getClientHistory(clientId, -1, 0,dateRange.EndDate.ToYearMonthValue(),dateRange.NumberOfMonths);
            if (theDS != null)
            {
                transactionDataTables = theDS.Tables;
                DataRowReader reader = new DataRowReader(transactionDataTables[0].Rows);
                return new HistoricalTransactionBuilder(reader).Build();
            }

            return null;
       }

        #endregion
    }
}