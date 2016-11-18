using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class TransactionSearchResult : BatchRecord
    {
        private readonly decimal invoiceAmount;
        private readonly decimal invoiceBalance;
        private readonly Date invoiceDate;
        private readonly string invoiceNumber;
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

        public TransactionSearchResult(Date invoiceDate, Date processed, string invoiceNumber, decimal invoiceAmount, decimal invoiceBalance,
                                       int batch, int customerNumber, int customerId, string customerName, int clientId,
                                       string clientName, string title, decimal customerBalance, string batchFrom, string batchTo) : base(TransactionType.Invoice.Type, batch)
        {
            this.customerNumber = customerNumber;
            this.customerBalance = customerBalance;
            this.title = title;
            this.clientName = clientName;
            this.clientId = clientId;
            this.customerName = customerName;
            this.customerId = customerId;
            this.processed = processed;
            this.invoiceDate = invoiceDate;
            this.invoiceNumber = invoiceNumber;
            this.invoiceAmount = invoiceAmount;
            this.invoiceBalance = invoiceBalance;
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

        public Date InvoiceDate
        {
            get { return invoiceDate; }
        }

        public string InvoiceNumber
        {
            get { return invoiceNumber; }
        }

        public decimal InvoiceAmount
        {
            get { return invoiceAmount; }
        }

        public decimal InvoiceBalance
        {
            get { return invoiceBalance; }
        }

        public Date Processed
        {
            get { return processed; }
        }

     
    }
}