using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ChargeType
    {
        private static readonly ChargeType advancedRepayment = new ChargeType(8, "Advanced repayment");
        private static readonly ChargeType cashCheque = new ChargeType(10, "Cash Cheque");
        private static readonly ChargeType establishmentFee = new ChargeType(1, "Establishment Fee");
        private static readonly ChargeType factoredCredit = new ChargeType(7, "Factored Credit");
        private static readonly ChargeType importFacility = new ChargeType(9, "Import Facility");
        private static readonly ChargeType interest = new ChargeType(14, "Interest");
        private static readonly ChargeType negativeBatch = new ChargeType(2, "Negative Batch");
        private static readonly ChargeType negativeOther = new ChargeType(4, "Negative Other");
        private static readonly ChargeType negativeRetention = new ChargeType(3, "Negative Retention");
        private static readonly ChargeType payCreditor = new ChargeType(12, "Pay Creditor");
        private static readonly ChargeType paymentDirect = new ChargeType(13, "Payment Direct");
        private static readonly ChargeType plusDouble = new ChargeType(6, "Plus Double");
        private static readonly ChargeType plusOther = new ChargeType(5, "Plus Other");
        private static readonly ChargeType transactionFee = new ChargeType(11, "transactionFee");
        private static readonly Dictionary<int, ChargeType> knownTypes = InitializeKnownTypes();

        private readonly int id;
        private readonly string type;

        private ChargeType(int id, string type)
        {
            this.id = id;
            this.type = type;
        }

        private static Dictionary<int, ChargeType> InitializeKnownTypes()
        {
            var chargeTypes = new Dictionary<int, ChargeType>();
            chargeTypes.Add(advancedRepayment.Id, advancedRepayment);
            chargeTypes.Add(cashCheque.Id, cashCheque);
            chargeTypes.Add(establishmentFee.Id, establishmentFee);
            chargeTypes.Add(factoredCredit.Id, factoredCredit);
            chargeTypes.Add(importFacility.Id, importFacility);
            chargeTypes.Add(interest.Id, interest);
            chargeTypes.Add(negativeBatch.Id, negativeBatch);
            chargeTypes.Add(negativeOther.Id, negativeOther);
            chargeTypes.Add(negativeRetention.Id, negativeRetention);
            chargeTypes.Add(payCreditor.Id, payCreditor);
            chargeTypes.Add(paymentDirect.Id, paymentDirect);
            chargeTypes.Add(plusOther.Id, plusOther);
            chargeTypes.Add(plusDouble.Id, plusDouble);
            chargeTypes.Add(transactionFee.Id, transactionFee);
            return chargeTypes;
        }

        public static ChargeType Parse(int id)
        {
            /*ChargeType chargeType = null;
            if (knownTypes.ContainsKey(id))
            {
                chargeType = knownTypes[id];
            }
            return chargeType;
             * */
            return knownTypes.ContainsKey(id) ? knownTypes[id] : null;
        }

        public override string ToString()
        {
            return type;
        }

        public string Type
        {
            get { return type; }
        }

        public static ChargeType EstablishmentFee
        {
            get { return establishmentFee; }
        }

        public static ChargeType NegativeBatch
        {
            get { return negativeBatch; }
        }

        public static ChargeType NegativeRetention
        {
            get { return negativeRetention; }
        }

        public static ChargeType NegativeOther
        {
            get { return negativeOther; }
        }

        public static ChargeType PlusOther
        {
            get { return plusOther; }
        }

        public static ChargeType PlusDouble
        {
            get { return plusDouble; }
        }

        public static ChargeType FactoredCredit
        {
            get { return factoredCredit; }
        }

        public static ChargeType AdvancedRepayment
        {
            get { return advancedRepayment; }
        }

        public static ChargeType ImportFacility
        {
            get { return importFacility; }
        }

        public static ChargeType CashCheque
        {
            get { return cashCheque; }
        }

        public static ChargeType TransactionFee
        {
            get { return transactionFee; }
        }

        public static ChargeType PayCreditor
        {
            get { return payCreditor; }
        }

        public static ChargeType PaymentDirect
        {
            get { return paymentDirect; }
        }

        public static ChargeType Interest
        {
            get { return interest; }
        }

        public string Type1
        {
            get { return type; }
        }

        public int Id
        {
            get { return id; }
        }
    }
}