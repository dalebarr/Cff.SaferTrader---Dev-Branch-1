using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class Charge
    {
        private readonly decimal amount;
        private readonly string description;
        private readonly int id;
        private readonly string modifiedBy;
        private readonly Date modifiedDate;
        private readonly ChargeType type;

        public Charge(int id, ChargeType type, decimal amount, string description, Date modifiedDate, string modifiedBy)
        {
            ArgumentChecker.ThrowIfNull(type, "type");

            this.id = id;
            this.type = type;
            this.amount = amount;
            this.description = description;
            this.modifiedDate = modifiedDate;
            this.modifiedBy = modifiedBy;
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public string Description
        {
            get { return description; }
        }

        public int Id
        {
            get { return id; }
        }

        public string ModifiedBy
        {
            get { return modifiedBy; }
        }

        public Date ModifiedDate
        {
            get { return modifiedDate; }
        }

        public ChargeType Type
        {
            get { return type; }
        }
    }
}