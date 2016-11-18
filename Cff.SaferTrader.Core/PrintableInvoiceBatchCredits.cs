using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableInvoiceBatchCredits : IPrintable
    {
        private readonly IList<CreditLine> credits;
        private readonly InvoiceBatch invoiceBatch;
        private readonly string viewID;

        public PrintableInvoiceBatchCredits(InvoiceBatch invoiceBatch, IList<CreditLine> credits, string viewIDValue)
        {
            this.invoiceBatch = invoiceBatch;
            this.credits = credits;
            this.viewID = viewIDValue;
        }

        #region IPrintable Members

        public string PopupPageName
        {
            get { return "InvoiceBatchCreditsPopup.aspx?ViewID=" + this.viewID; }
        }

        #endregion

        public InvoiceBatch InvoiceBatch
        {
            get { return invoiceBatch; }
        }

        public IList<CreditLine> Credits
        {
            get { return credits; }
        }
    }
}