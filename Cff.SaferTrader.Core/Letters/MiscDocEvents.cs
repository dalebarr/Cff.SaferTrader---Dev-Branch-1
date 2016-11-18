using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Core.Letters
{
    [Serializable]
    public class MiscDocEvents
    {
        private readonly string origDocName;
        private readonly string newDocName;
        private string errorMsg;

        public MiscDocEvents(string strDocName)
        { 
            this.origDocName = strDocName;
        }
        
        public MiscDocEvents(string origin, string destination)
        {
            this.origDocName = origin;
            this.newDocName = destination;
        }    

        public void copyToBackup(){
            try {
                File.Copy(origDocName, newDocName);
            } catch (Exception exc)
            {
                this.errorMsg = exc.Message;
            }
        }

        public string ErrorMsg {
            get { return this.errorMsg; }
        }
    }
}
