using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cff.SaferTrader.Core.Repositories
{
    public class TransactionSearchRepository : SqlManager, ITransactionSearchRepository
    {
        public TransactionSearchRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IList<TransactionSearchResult> SearchTransactions(DateRange dateRange, string invoiceNumber, TransactionSearchType transactionType, SearchScope searchScope, CffCustomer customer, ICffClient client, string batchFrom, string batchTo)
        {
            if (invoiceNumber.Length<3)
            {
                throw new ArgumentException("You need more than 3 invoice number to search ");
                
            }
            SqlParameter[] queryBuilder = CreateSqlBuilder(dateRange, invoiceNumber,
                                                           transactionType, searchScope, customer, client, batchFrom, batchTo);
            IList<TransactionSearchResult> transactionSearchResults = new List<TransactionSearchResult>();
            using (SqlConnection connection = CreateConnection())
            {
                try
                {
                    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                              CommandType.StoredProcedure,
                                                                              "stGetCustomersSearchAll",
                                                                              queryBuilder))
                    {
                        CleverReader cleverReader = new CleverReader(dataReader);
                        while (!cleverReader.IsNull && cleverReader.Read())
                        {
                            var transactionSearchResult =
                                new TransactionSearchResult(cleverReader.ToDate("Transdate"),
                                                            cleverReader.ToDate("factorDate"),
                                                            cleverReader.ToString("TransRef"),
                                                            cleverReader.ToDecimal("TransAmount"),
                                                            cleverReader.ToDecimal("TransBalance"),
                                                            cleverReader.FromBigInteger("BatchID"),
                                                            cleverReader.FromBigInteger("CustNum"),
                                                            cleverReader.FromBigInteger("CustomerID"),
                                                            cleverReader.ToString("Customer"),
                                                            cleverReader.FromBigInteger("ClientID"),
                                                            cleverReader.ToString("ClientName"),
                                                            cleverReader.ToString("Title"),
                                                            cleverReader.ToDecimal("Balance"),
                                                            cleverReader.ToString("BatchFrom"),
                                                            cleverReader.ToString("BatchTo"));
                            transactionSearchResults.Add(transactionSearchResult);
                        }
                    }
                }
                catch (SqlException exception)
                {
                    if (exception.Message.Contains("Timeout expired"))
                    {
                        throw new CffTimeoutException(exception.Message, exception);
                    }
                    throw;
                }
            }
            Console.WriteLine(transactionSearchResults.Count);
            return RecordLimiter.ReturnMaximumRecords(transactionSearchResults);
        }

        public IList<CreditNoteSearchResult> SearchCreditNotesTransactions(DateRange dateRange, string transactionNumber, TransactionSearchType transactionSearchType, SearchScope searchScope, CffCustomer customer, ICffClient client, string batchFrom, string batchTo)
        {
            if (transactionNumber.Length < 3)
            {
                throw new ArgumentException("You need more than 3 transaction Number to search ");

            }
            SqlParameter[] queryBuilder = CreateSqlBuilder(dateRange, transactionNumber,
                                                           transactionSearchType, searchScope, customer, client, batchFrom, batchTo);
            IList<CreditNoteSearchResult> creditNoteSearchResults = new List<CreditNoteSearchResult>();
            using (SqlConnection connection = CreateConnection())
            {
                try
                {
                    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                              CommandType.StoredProcedure,
                                                                              "stGetCustomersSearchAll",
                                                                              queryBuilder))
                    {
                        CleverReader cleverReader = new CleverReader(dataReader);
                        while (!cleverReader.IsNull && cleverReader.Read())
                        {
                            var transactionSearchResult =
                                new CreditNoteSearchResult(
                                    cleverReader.ToDecimal("Amount"),
                                    cleverReader.ToInteger("BatchID"),
                                    cleverReader.ToDate("datereceived"),
                                    cleverReader.ToString("Reference"),
                                    cleverReader.ToDate("Created"),
                                    cleverReader.FromBigInteger("CustomerID"),
                                    cleverReader.ToString("Customer"),
                                    cleverReader.FromBigInteger("ClientID"),
                                    cleverReader.ToString("ClientName"),
                                    cleverReader.FromBigInteger("CustNum"),
                                    cleverReader.ToString("Title"),
                                    cleverReader.ToDecimal("Balance"),
                                    cleverReader.ToString("BatchFrom"),
                                    cleverReader.ToString("BatchTo"));
                            creditNoteSearchResults.Add(transactionSearchResult);
                        }
                    }
                }
                catch (SqlException exception)
                {
                    if (exception.Message.Contains("Timeout expired"))
                    {
                        throw new CffTimeoutException(exception.Message, exception);   
                    }
                    throw;
                }
            }
            return RecordLimiter.ReturnMaximumRecords(creditNoteSearchResults);
        }

        private static SqlParameter[] CreateSqlBuilder(DateRange dateRange, string invoiceNumber, TransactionSearchType transactionSearchType, SearchScope searchScope, CffCustomer customer, ICffClient client, string batchFrom, string batchTo)
        {
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(invoiceNumber))
            {
                SqlParameter invoiceNumberParameter = new SqlParameter("@Seek", SqlDbType.NVarChar, 40);
                invoiceNumberParameter.Value = invoiceNumber + "%";
                sqlParameterList.Add(invoiceNumberParameter);
            }
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.Int);
            SqlParameter customerIdParameter = new SqlParameter("@CustID ", SqlDbType.Int);
            switch (searchScope)
            {
                case SearchScope.AllClients:
                    clientIdParameter.Value = -1;
                    customerIdParameter.Value = -1;
                    break;
                case SearchScope.AllCustomers:
                    clientIdParameter.Value = client.Id;
                    customerIdParameter.Value = -1;
                    break;
                case SearchScope.CurrentCustomer:
                    clientIdParameter.Value = client.Id;
                    customerIdParameter.Value =customer.Id;
                    break;
            }
            sqlParameterList.Add(clientIdParameter);
            sqlParameterList.Add(customerIdParameter);
            SqlParameter transactionSearchTypeParameter = new SqlParameter("@SearchBy", SqlDbType.VarChar, 20);
            switch (transactionSearchType)
            {
                case TransactionSearchType.Invoices:
                    transactionSearchTypeParameter.Value = "Invoice";
                    sqlParameterList.Add(transactionSearchTypeParameter);
                    break;
                case TransactionSearchType.CreditNotes:
                    transactionSearchTypeParameter.Value = "CreditEtc";
                    sqlParameterList.Add(transactionSearchTypeParameter);
                    break;
            }
            SqlParameter startDateParameter = new SqlParameter("@dtFrom", SqlDbType.DateTime);
            startDateParameter.Value = dateRange.StartDate.Value.DateTime;
            sqlParameterList.Add(startDateParameter);
            SqlParameter endDateParameter = new SqlParameter("@dtTo", SqlDbType.DateTime);
            endDateParameter.Value = dateRange.EndDate.Value.DateTime;
            sqlParameterList.Add(endDateParameter);
            SqlParameter btFrom = new SqlParameter("@BatchFrom", SqlDbType.VarChar);
            btFrom.Value = batchFrom;
            sqlParameterList.Add(btFrom);
            SqlParameter btTo = new SqlParameter("@BatchTo", SqlDbType.VarChar);
            btTo.Value = batchTo;
            sqlParameterList.Add(btTo);
            return sqlParameterList.ToArray();
        }
    }
}