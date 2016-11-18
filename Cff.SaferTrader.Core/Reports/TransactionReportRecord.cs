using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class TransactionReportRecord : BatchRecord
    {
        private readonly decimal amount;
        private readonly decimal? batchTotal;
        private readonly string customerName;
        private readonly int customerNumber;
        private readonly Date date;
        private readonly int id;
        private readonly int customerId;
        private readonly string invoice;
        private readonly Date processedDate;
        private readonly string reference;
        internal readonly TransactionStatus transactionStatus;
        private readonly string clientName;
        private readonly int clientNumber;
       

        public TransactionReportRecord(int id, int clientNumber, string clientName, int customerId, int customerNumber, 
            string customerName, Date date, string invoice, string reference, decimal amount, Date processedDate, int batch, decimal? batchTotal, TransactionStatus transactionStatus, TransactionType transacitonType) 
            :base(transacitonType.Type, batch)
        {
            ArgumentChecker.ThrowIfNull(transactionStatus, "transactionStatus");

            this.id = id;
            this.customerId = customerId;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.date = date;
            this.invoice = invoice;
            this.reference = reference;
            this.amount = amount;
            this.processedDate = processedDate;
            this.batchTotal = batchTotal;
            this.transactionStatus = transactionStatus;
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        /// <summary>
        /// Returns ClientNumber in string
        /// </summary>
        public string ClientNumberLabel
        {
            get { return clientNumber.ToString(); }
        }

        public int ClientNumber
        {
            get { return clientNumber; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public decimal? BatchTotal
        {
            get { return batchTotal; }
        }

        public TransactionStatus TransactionStatus
        {
            get { return transactionStatus; }
        }

        public int Id
        {
            get { return id; }
        }

        public int CustomerNumber
        {
            get { return customerNumber; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public Date Date
        {
            get { return date; }
        }

        public string Invoice
        {
            get { return invoice; }
        }

        public string Reference
        {
            get { return reference; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        /// <summary>
        /// Date the transaction has been factored
        /// </summary>
        public IDate ProcessedDate
        {
            get { return processedDate; }
        }

        public bool IsFunded
        {
            get { return    transactionStatus == TransactionStatus.Funded || 
                            transactionStatus == TransactionStatus.Marked ||
                            transactionStatus == TransactionStatus.Claimed ||
                            transactionStatus == TransactionStatus.Unclaimed; }
        }

        public string Status
        {
            get { return transactionStatus.Status; }
        }
    }
}