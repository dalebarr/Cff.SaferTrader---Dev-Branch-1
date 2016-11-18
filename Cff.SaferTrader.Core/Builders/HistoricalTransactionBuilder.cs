using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class HistoricalTransactionBuilder
    {
        private readonly DataRowReader reader;

        public HistoricalTransactionBuilder(DataRowReader reader)
        {
            this.reader = reader;
        }

        public IList<HistoricalTransaction> Build()
        {
            IList<HistoricalTransaction> historicalTransactions= new List<HistoricalTransaction>();
            while (reader.Read())
            {
                var transaction =
                    new HistoricalTransaction(reader.ToString("YearMonth"),
                                              reader.ToDecimal("Invoices"),
                                              reader.ToDecimal("NF Invoices"),
                                              reader.ToDecimal("Credits"),
                                              reader.ToDecimal("Receipts"),
                                              reader.ToDecimal("Journals_AR"), // What is Journals_NAR?
                                              reader.ToDecimal("Discounts"),
                                              reader.ToDecimal("Repurchases"),
                                              reader.ToDecimal("Overpayments"));
                historicalTransactions.Add(transaction);
            }
            return historicalTransactions;
        }
    }
}