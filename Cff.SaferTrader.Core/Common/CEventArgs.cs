using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.SaferTrader.Core.Common
{
    public class CEventArgs : EventArgs
    {
        private int status;
        private string statusdesc;

        public CEventArgs(int _status, string _statusdesc) : base()
        {
            this.status = _status;
            this.statusdesc = _statusdesc;
        }

        public int Status { get { return this.status; } }
        public string StatusDesc { get { return this.statusdesc; } }
    }
}
