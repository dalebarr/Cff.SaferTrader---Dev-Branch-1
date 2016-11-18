using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
//1	    Invoice             
//2	    Credit
//3	    Receipt             
//4	    CBT_Cr          
//5	    Repurchase          
//6	    Discount            
//7	    Jnl AR
//8	    Jnl NAR
//9	    CBT_Dr      
//10	O/payt         
//11	Unconfirmed
//12	Processing
//13	Allocation

//TODO: synchronize recent changes with variable names
    [Serializable]
    public class TransactionType
    {
        private readonly int id;
        private readonly string type;
        private static readonly TransactionType invoice = new TransactionType(1, "Invoice");
        private static readonly TransactionType credit = new TransactionType(2, "Credit");   
        private static readonly TransactionType receipt = new TransactionType(3, "Receipt"); 
        private static readonly TransactionType creditBalanceTransferCredit = new TransactionType(4, "CBT Credit");
        private static readonly TransactionType repurchase = new TransactionType(5, "Repurchase");
        private static readonly TransactionType discount = new TransactionType(6, "Disount");
        private static readonly TransactionType journalAR = new TransactionType(7, "Journal AR");
        private static readonly TransactionType journalNar = new TransactionType(8, "Journal NAR");
        private static readonly TransactionType creditBalanceTransferDebit = new TransactionType(9, "CBT Debit");
        private static readonly TransactionType overpayment = new TransactionType(10, "Overpayment");
        private static readonly TransactionType payment = new TransactionType(11, "Payment");
        private static readonly TransactionType journal = new TransactionType(12, "Journal");
        private static readonly TransactionType allocation = new TransactionType(13, "Allocation");

        private static readonly TransactionType other = new TransactionType(-1, "Other");

        private static readonly TransactionType retnInvoice = new TransactionType(1, "Payment");  //new TransactionType(1, "Invoice"); -- changed to payment as per marty 02/20/2012
        private static readonly TransactionType retnCredit = new TransactionType(2, "Receipt");   //new TransactionType(2, "Credit"); -- changed to receipt as per marty 09/02/2012
        private static readonly TransactionType retnReceipt = new TransactionType(3, "Journal");  //new TransactionType(3, "Receipt"); -- changed to Journal as per marty 01/02/2012

        private static readonly Dictionary<int, TransactionType> knownTypes = InitializeKnownTypes();
        private static readonly Dictionary<int, TransactionType> knownTypesRetnStatus = InitializeKnownTypesRetnStatus();

        public static TransactionType Parse(int id)
        {
            TransactionType transactionType = other;
            if (knownTypes.ContainsKey(id))
            {
                transactionType = knownTypes[id];
            }
            return transactionType;
        }

        private static Dictionary<int, TransactionType> InitializeKnownTypes()
        {
            Dictionary<int, TransactionType> types = new Dictionary<int, TransactionType>();
            types.Add(invoice.Id, invoice);
            types.Add(credit.Id, credit);
            types.Add(receipt.Id, receipt);
            types.Add(creditBalanceTransferCredit.Id, creditBalanceTransferCredit);
            types.Add(repurchase.Id, repurchase);
            types.Add(discount.Id, discount);
            types.Add(journalAR.Id, journalAR);
            types.Add(journalNar.Id, journalNar);
            types.Add(creditBalanceTransferDebit.Id, creditBalanceTransferDebit);
            types.Add(overpayment.Id, overpayment);
            types.Add(allocation.Id, allocation);
            return types;
        }


        public static TransactionType ParseRetnStatus(int id)
        {
            TransactionType transactionType = other;
            if (knownTypesRetnStatus.ContainsKey(id))
            {
                transactionType = knownTypesRetnStatus[id];
            }
            return transactionType;
        }


        private static Dictionary<int, TransactionType> InitializeKnownTypesRetnStatus()
        {
            Dictionary<int, TransactionType> types = new Dictionary<int, TransactionType>();
            types.Add(retnInvoice.Id, retnInvoice);
            types.Add(retnCredit.Id, retnCredit);
            types.Add(retnReceipt.Id, retnReceipt);
            types.Add(creditBalanceTransferCredit.Id, creditBalanceTransferCredit);
            types.Add(repurchase.Id, repurchase);
            types.Add(discount.Id, discount);
            types.Add(journalAR.Id, journalAR);
            types.Add(journalNar.Id, journalNar);
            types.Add(creditBalanceTransferDebit.Id, creditBalanceTransferDebit);
            types.Add(overpayment.Id, overpayment);
            types.Add(allocation.Id, allocation);
            return types;
        }

        private TransactionType(int id, string type)
        {
            this.id = id;
            this.type = type;
        }

        public int Id
        {
            get { return id; }
        }

        public string Type
        {
            get { return type; }
        }

        public static Dictionary<int, TransactionType> KnownTypes
        {
            get { return knownTypes; }
        }

        public static Dictionary<int, TransactionType> KnownTypesRetnStatus
        {
            get { return knownTypesRetnStatus; }
        }

        public static TransactionType Invoice
        {
            get { return invoice; }
        }

        public static TransactionType Credit
        {
            get { return credit; }
        }

        public static TransactionType Receipt
        {
            get { return receipt; }
        }

        public static TransactionType CreditBalanceTransferCredit
        {
            get { return creditBalanceTransferCredit; }
        }

        public static TransactionType Repurchase
        {
            get { return repurchase; }
        }

        public static TransactionType JournalAr
        {
            get { return journalAR; }
        }

        public static TransactionType JournalNar
        {
            get { return journalNar; }
        }

        public static TransactionType CreditBalanceTransferDebit
        {
            get { return creditBalanceTransferDebit; }
        }

        public static TransactionType Overpayment
        {
            get { return overpayment; }
        }

        public static TransactionType Discount
        {
            get { return discount; }
        }

        public static TransactionType Journal
        {
            get { return journal; }
        }

        public static TransactionType Payment
        {
            get { return payment; }
        }

        public static TransactionType Allocation
        {
            get { return allocation; }
        }

        public static TransactionType Other
        {
            get { return other; }
        }

       
    }
}
