using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableCustomerNotes : IPrintable
    {
        private readonly IList<CustomerNote> notes;
        private readonly string customerName;
        private readonly bool isGroupByCustomerName;
        private readonly string clientName;
        private readonly string reference;
        private readonly string viewID;

        public PrintableCustomerNotes(string customerName, IList<CustomerNote> notes, string viewIDValue)
        {
            this.customerName = customerName;
            this.notes = notes;
            this.viewID = viewIDValue;
        }

        public PrintableCustomerNotes(string customerName, string strRef, IList<CustomerNote> notes, string viewIDValue)
        {
            this.reference = strRef;
            this.customerName = customerName;
            this.notes = notes;
            this.viewID = viewIDValue;
        }

        public PrintableCustomerNotes(string clientName, IList<CustomerNote> notes, bool pGroupByCustName, string viewIDValue)
        {
            this.clientName = clientName;
            this.notes = notes;
            this.isGroupByCustomerName = pGroupByCustName;
            this.viewID = viewIDValue;
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
            get { return "CustomerNotesPopup.aspx?ViewID=" + this.viewID; }
        }

        public bool IsGroupByCustomerName
        {
            get { return this.isGroupByCustomerName; }
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