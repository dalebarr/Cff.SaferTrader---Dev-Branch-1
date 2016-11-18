using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class SubledgerBuilder
    {
        public Subledger Build(DataRowCollection rows)
        {
            ArgumentChecker.ThrowIfNull(rows, "rows");
            Subledger subledger = new Subledger();
            DataRowReader reader = new DataRowReader(rows);
            while (reader.Read())
            {
                ISubledgerTransaction transaction = BuildSubledgerTransaction(  TransactionType.Parse(reader.ToInteger("TransTypeID")),
                                                                                TransactionStatus.Parse(reader.ToInteger("TypeStatus")), 
                                                                                reader.ToDecimal("Amount"));
                if (transaction != null)
                {
                    transaction.AddTo(subledger);
                }
            }
            return subledger;
        }

        private static ISubledgerTransaction BuildSubledgerTransaction(TransactionType type, TransactionStatus status, decimal amount)
        {
            ISubledgerTransaction subledgerTransaction = null;
            if (type == TransactionType.Invoice)
            {
                subledgerTransaction = new InvoiceTransaction(status, amount);
            }
            else if (type == TransactionType.Credit)
            {
                subledgerTransaction = new CreditTransaction(amount);
            }
            else if (type == TransactionType.JournalAr || type == TransactionType.JournalNar)
            {
                subledgerTransaction = new JournalTransaction(amount);
            }
            else if (type == TransactionType.Receipt)
            {
                subledgerTransaction = new ReceiptTransaction(amount);
            }
            else if (type == TransactionType.Overpayment)
            {
                subledgerTransaction = new OverpaymentTransaction(amount);
            }
            else if (type == TransactionType.CreditBalanceTransferCredit || type == TransactionType.CreditBalanceTransferDebit || type == TransactionType.Allocation)
            {
                subledgerTransaction = new CreditBalanceTransferTransaction(amount);
            }
            else if (type == TransactionType.Discount)
            {
                subledgerTransaction = new DiscountTransaction(amount);
            }
            else if (type == TransactionType.Repurchase)
            {
                subledgerTransaction = new RepurchaseTransaction(amount);
            }
            return subledgerTransaction;
        }
    }
}