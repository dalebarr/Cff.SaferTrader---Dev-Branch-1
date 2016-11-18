using System;
using System.Collections;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ChargeCollection : ICollection<Charge>
    {
        private readonly IList<Charge> charges;

        public ChargeCollection()
        {
            charges = new List<Charge>();
        }

        public ChargeCollection(IList<Charge> charges)
        {
            this.charges = charges;
        }

        #region ICollection<Charge> Members

        public IEnumerator<Charge> GetEnumerator()
        {
            return charges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Charge item)
        {
            charges.Add(item);
        }

        public void Clear()
        {
            charges.Clear();
        }

        public bool Contains(Charge item)
        {
            return charges.Contains(item);
        }

        public void CopyTo(Charge[] array, int arrayIndex)
        {
            charges.CopyTo(array, arrayIndex);
        }

        public bool Remove(Charge item)
        {
            return charges.Remove(item);
        }

        public int Count
        {
            get { return charges.Count; }
        }

        public bool IsReadOnly
        {
            get { return charges.IsReadOnly; }
        }

        #endregion

        public decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (Charge charge in charges)
           {
                if ((charge.Type.Type == ChargeType.PlusOther.Type)  || (charge.Type.Type == ChargeType.PlusDouble.Type))    // dbb
                {
                    total -= charge.Amount;
                }
                else
                {
                    total += charge.Amount;
                }
            }
            return total;
        }

        public IList<Charge> GetList()
        {
            return charges;
        }
    }
}