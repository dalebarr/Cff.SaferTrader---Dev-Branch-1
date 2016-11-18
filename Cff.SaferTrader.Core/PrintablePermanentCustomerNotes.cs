using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintablePermanentCustomerNotes : IPrintable
    {
        private readonly string customerName;
        private readonly IList<PermanentCustomerNote> notes;
        private readonly string viewID;

        public PrintablePermanentCustomerNotes(string customerName, IList<PermanentCustomerNote> notes, string viewIDValue)
        {
            this.customerName = customerName;
            this.notes = notes;
            this.viewID = viewIDValue;
        }

        public string PopupPageName
        {
            get { return "PermanentCustomerNotesPopup.aspx?ViewID=" + this.viewID; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public IList<PermanentCustomerNote> Notes
        {
            get { return notes; }
        }
    }
}