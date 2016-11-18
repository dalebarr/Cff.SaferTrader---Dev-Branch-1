using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableClientNotes : IPrintable
    {
        private readonly string clientName;
        private readonly IList<ClientNote> notes;
        private readonly string viewID;

        public PrintableClientNotes(string clientName, IList<ClientNote> notes, string viewIDValue)
        {
            this.clientName = clientName;
            this.notes = notes;
            this.viewID = viewIDValue;
        }

        public string PopupPageName
        {
            get { return "ClientNotesPopup.aspx?ViewID=" + this.viewID; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public IList<ClientNote> Notes
        {
            get { return notes; }
        }
    }
}