using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintablePermanentClientNotes : IPrintable
    {
        private readonly string clientName;
        private readonly IList<PermanentClientNote> notes;
        private readonly string viewID;

        public PrintablePermanentClientNotes(string clientName, IList<PermanentClientNote> notes, string viewIDValue)
        {
            this.clientName = clientName;
            this.notes = notes;
            this.viewID = viewIDValue;
        }

        public string PopupPageName
        {
            get { return "PermanentClientNotesPopup.aspx?ViewID=" + this.viewID; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public IList<PermanentClientNote> Notes
        {
            get { return notes; }
        }
    }
}