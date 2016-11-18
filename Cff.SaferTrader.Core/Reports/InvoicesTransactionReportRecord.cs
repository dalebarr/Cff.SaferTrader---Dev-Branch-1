using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class InvoicesTransactionReportRecord : TransactionReportRecord
    {
        private readonly decimal balance;
        private readonly IDate receiptDate;
        private readonly ICalendar calendar;

        public InvoicesTransactionReportRecord
            (int id, int clientNumber, string clientName, int customerId, int customerNumber, string customerName, 
            Date date, string invoice, string reference, decimal amount, Date processedDate, int batch, decimal? batchTotal, TransactionStatus transactionStatus, decimal balance, IDate receiptDate, ICalendar calendar)
            : base(id, clientNumber, clientName, customerId, customerNumber, customerName, date, invoice, reference, amount, processedDate, batch, batchTotal, transactionStatus, TransactionType.Invoice)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            
            this.balance = balance;
            this.receiptDate = receiptDate;
            this.calendar = calendar;
        }

        public int CalculateAge()
        {
            int age;
            if (IsPaidFor && receiptDate.HasValue)
            {
                // Could possibly use DateRange class here?
                age = (receiptDate.Value - Date).Days + 1;
            }
            else
            {
                age = (calendar.Today - Date).Days + 1;
            }
            return age;
        }

        /// <summary>
        /// BOM refers to Beginning of the following Month since the invoice date
        /// </summary>
        public int CalculateAgeFromBom()
        {
            int ageFromBom;
            Date beginningOfFollowingMonth = Date.FirstDayOfNextMonth;

            if (IsPaidFor && receiptDate.HasValue)
            {
                ageFromBom = (receiptDate.Value - beginningOfFollowingMonth).Days + 1;
            }
            else
            {
                ageFromBom = (calendar.Today - beginningOfFollowingMonth).Days + 1;
                // age is zero if it is still the month this record was invoiced
                ageFromBom = ageFromBom >= 0 ? ageFromBom : 0;
            }
            return ageFromBom;
        }

        public new string Status
        {
            get
            {
                string status = TransactionStatus.Status;
                if (IsPaidFor)
                {
                    status = status + " - Paid";
                }
                return status;
            }
        }

        public string StatusAge
        {
            get { return string.Format("{0} - {1}", Status, CalculateAge()); }
        }

        public bool IsPaidFor
        {
            get { return balance <= 0 && receiptDate is Date; }
        }
    }
}