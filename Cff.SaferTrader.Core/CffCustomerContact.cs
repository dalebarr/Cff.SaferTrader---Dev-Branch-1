using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.SaferTrader.Core
{
    //MSarza [20150817]
    public class CffCustomerContact
    {
        public bool IsDefault {get; set;}
        public string LName { get; set; }
        public string FName { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Cell { get; set; }
        public bool Attn { get; set; }
        public bool EmailStatement { get; set; }
        public int EmailReceipt { get; set; }
    }
}
