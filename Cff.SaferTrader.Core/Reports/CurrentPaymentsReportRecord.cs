using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CurrentPaymentsReportRecord : TransactionReportRecord
    {
        private readonly decimal balance;

        public CurrentPaymentsReportRecord(int id, int clientNumber, string clientName, int customerId, int customerNumber, string customerName, Date date, string invoice, string reference, decimal amount, decimal balance, TransactionStatus transactionStatus, Date processedDate, int batch, TransactionType transactionType)
            : base(id, clientNumber, clientName, customerId, customerNumber, customerName, date, invoice, reference, amount, processedDate, batch, null, transactionStatus, transactionType)
        {
            this.balance = balance;
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public new string Status
        {
            get { return transactionStatus.Status; }
        }
    }
}