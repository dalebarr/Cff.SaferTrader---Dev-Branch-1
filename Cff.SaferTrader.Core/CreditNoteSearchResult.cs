using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CreditNoteSearchResult : BatchRecord
    {
        private readonly decimal creditNoteAmount;
        private readonly int batch;
        private readonly Date dated;
        private readonly string transactionNumber;
        private readonly Date processed;
        private readonly int customerId;
        private readonly string customerName;
        private readonly int clientId;
        private readonly string clientName;
        private readonly int customerNumber;
        private readonly string title;
        private readonly decimal customerBalance;
        private readonly string batchFrom;
        private readonly string batchTo;

        public CreditNoteSearchResult(decimal creditNoteAmount, int batch, Date dated, string transactionNumber, Date processed, 
            int customerId, string customerName, int clientId, string clientName, int customerNumber, string title, decimal customerBalance, string batchFrom, string batchTo)
            : base(TransactionType.Credit.Type, batch)
        {
            this.creditNoteAmount = creditNoteAmount;
            this.batch = batch;
            this.dated = dated;
            this.transactionNumber = transactionNumber;
            this.processed = processed;
            this.customerId = customerId;
            this.customerName = customerName;
            this.clientId = clientId;
            this.clientName = clientName;
            this.customerNumber = customerNumber;
            this.title = title;
            this.customerBalance = customerBalance;
            this.batchFrom = batchFrom;
            this.batchTo = batchTo;
        }

        public string BatchTo
        {
            get { return batchTo; }
        }

        public string BatchFrom
        {
            get { return batchFrom; }
        }

        public decimal CustomerBalance
        {
            get { return customerBalance; }
        }

        public string Title
        {
            get { return title; }
        }

        public int CustomerNumber
        {
            get { return customerNumber; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public int ClientId
        {
            get { return clientId; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        public Date Processed
        {
            get { return processed; }
        }

        public string TransactionNumber
        {
            get { return transactionNumber; }
        }

        public Date Dated
        {
            get { return dated; }
        }

        public decimal CreditNoteAmount
        {
            get { return creditNoteAmount; }
        }
    }
}