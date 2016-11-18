using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.SaferTrader.Core
{ 
    [Serializable]
    public class CffCustomerExt : CffCustomer, ICffCustomer
    {
        public CffCustomerExt(string name, int id, int number):base(name, id, number) 
        {}
    }
}
