using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableCurrentNotes : IPrintable
    {

        private readonly IList<CustomerNote> notes;
        private readonly string customerName;
        private readonly string clientName;
        private readonly string reference;

        public PrintableCurrentNotes(string strRef, string customerName, string clientName, IList<CustomerNote> notes)
        {
            this.reference = strRef;
            this.customerName = customerName;
            this.clientName = clientName;
            this.notes = notes;
            
        }

        public string CustomerName
        {
            get { return customerName; }
        }


        public IList<CustomerNote> Notes
        {
            get { return notes; }
        }


        public string PopupPageName
        {
            get { return "CurrentNotesPopup.aspx"; }
        }

        public string ClientName
        {
            get { return this.clientName; }
        }

        public string Reference
        {
            get { return this.reference; }
        }
    }
  
}
