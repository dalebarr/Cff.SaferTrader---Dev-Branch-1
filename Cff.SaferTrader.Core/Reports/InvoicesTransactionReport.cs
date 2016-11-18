using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class InvoicesTransactionReport : TransactionReportBase
    {
        private decimal fundedTotal;
        private decimal nonFundedTotal;
        private readonly IList<InvoicesTransactionReportRecord> records;
        private int numberOfRecordsPaidFor;
        private int numberOfRecordsNotPaidFor;
        private int totalAgeOfRecordsPaidFor;
        private int totalAgeOfUnpaidForRecords;
        private int totalAgeFromBeginningOfFollowingMonthForRecordsNotPaidFor;
        private int totalAgeFromBeginningOfFollowingMonthForRecordsPaidFor;
        private readonly string title;

        public InvoicesTransactionReport(ICalendar calendar, string title, string clientName, IList<InvoicesTransactionReportRecord> records) 
            : base(calendar, "Invoices", clientName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(records, "records");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");
            
            this.records = records;
            this.title = title;

            CalculateTotals();
        }

        private void CalculateTotals()
        {
            foreach (InvoicesTransactionReportRecord record in records)
            {
                if (record.IsFunded)
                {
                    fundedTotal += record.Amount;
                }
                else
                {
                    nonFundedTotal += record.Amount;
                }

                int age = record.CalculateAge();
                int ageFromBeginningOfFollowingMonth = record.CalculateAgeFromBom();

                if (record.IsPaidFor)
                {
                    numberOfRecordsPaidFor++;
                    totalAgeOfRecordsPaidFor += age;
                    totalAgeFromBeginningOfFollowingMonthForRecordsPaidFor += ageFromBeginningOfFollowingMonth;
                }
                else
                {
                    numberOfRecordsNotPaidFor++;
                    totalAgeOfUnpaidForRecords += age;
                    totalAgeFromBeginningOfFollowingMonthForRecordsNotPaidFor += ageFromBeginningOfFollowingMonth;
                }
            }
        }

        public IList<InvoicesTransactionReportRecord> Records
        {
            get { return records; }
        }

        public override decimal FundedTotal
        {
            get { return fundedTotal; }
        }

        public override decimal NonFundedTotal
        {
            get { return nonFundedTotal; }
        }
        
        public int NumberOfRecordsPaidFor
        {
            get { return numberOfRecordsPaidFor; }
        }

        public int NumberOfRecordsNotPaidFor
        {
            get { return numberOfRecordsNotPaidFor; }
        }

        /// <summary>
        /// Mean age of paid records
        /// </summary>
        public double MeanDebtorDays
        {
            get
            {
                double average = 0;
                if (numberOfRecordsPaidFor > 0)
                {
                    average = (double)totalAgeOfRecordsPaidFor / numberOfRecordsPaidFor;
                }
                return average;
            }
        }

        public double MeanAgeOfUnpaidRecords
        {
            get
            {
                double average = 0;
                if (NumberOfRecordsNotPaidFor > 0)
                {
                    average = (double)totalAgeOfUnpaidForRecords / numberOfRecordsNotPaidFor;
                }
                return average;
            }
        }

        /// <summary>
        /// Mean debtor days from beginning of following month from the invoice issue date
        /// </summary>
        public double MeanDebtorDaysFromBeginningOfFollowingMonth
        {
            get
            {
                double average = 0;
                if (numberOfRecordsPaidFor > 0)
                {
                    average = (double)totalAgeFromBeginningOfFollowingMonthForRecordsPaidFor / numberOfRecordsPaidFor;
                }
                return average;
            }
        }

        // <summary>
        /// Mean age of unpaid records from beginning of following month from the invoice issue date
        /// </summary>
        public double MeanAgeOfUnpaidRecordsFromBeginningOfFollowingMonth
        {
            get
            {
                double average = 0;
                if (NumberOfRecordsNotPaidFor > 0)
                {
                    average = (double)totalAgeFromBeginningOfFollowingMonthForRecordsNotPaidFor / numberOfRecordsNotPaidFor;
                }
                return average;
            }
        }

        public string Title
        {
            get { return title; }
        }
    }
}