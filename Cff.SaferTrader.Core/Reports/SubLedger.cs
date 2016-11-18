using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class Subledger
    {
        private decimal cashReceived;
        private decimal creditNotes;
        private decimal discounts;
        private decimal fundedInvoices;
        private decimal netAdjustment;
        private decimal netJournals;
        private decimal nonFundedInvoices;
        private decimal overpayments;
        private decimal repurchase;

        public void AddRepurchase(decimal amount)
        {
            repurchase += amount;
        }

        public void AddFundedInvoice(decimal amount)
        {
            fundedInvoices += amount;
        }

        public void AddNonFundedInvoice(decimal amount)
        {
            nonFundedInvoices += amount;
        }

        public void AddCreditNote(decimal amount)
        {
            creditNotes += amount;
        }

        public void AddJournal(decimal amount)
        {
            netJournals += amount;
        }

        public void AddReceipt(decimal amount)
        {
            cashReceived += amount;
        }

        public void AddOverpayment(decimal amount)
        {
            overpayments += amount;
        }

        public void AddCreditBalanceTransfer(decimal amount)
        {
            netAdjustment += amount;
        }

        public void AddDiscount(decimal amount)
        {
            discounts += amount;
        }

        public decimal FundedInvoices
        {
            get { return fundedInvoices; }
        }

        public decimal NonFundedInvoices
        {
            get { return nonFundedInvoices; }
        }

        public decimal CreditNotes
        {
            get { return creditNotes; }
        }

        public decimal NetJournals
        {
            get { return netJournals; }
        }

        public decimal CashReceived
        {
            get { return cashReceived; }
        }

        public decimal Overpayments
        {
            get { return overpayments; }
        }

        public decimal NetAdjustment
        {
            get { return netAdjustment; }
        }

        public decimal Discounts
        {
            get { return discounts; }
        }

        public decimal Total
        {
            get
            {
                return fundedInvoices +
                       +nonFundedInvoices
                       + creditNotes
                       + netJournals
                       + cashReceived
                       + overpayments
                       + netAdjustment
                       + discounts;
            }
        }

        public decimal Repurchase
        {
            get { return repurchase; }
        }
    }
}