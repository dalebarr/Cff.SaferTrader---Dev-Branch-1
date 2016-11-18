using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableInvoiceBatchCharges : IPrintable
    {
        private readonly IList<Charge> charges;
        private readonly InvoiceBatch invoiceBatch;
        private readonly string viewID;

        public PrintableInvoiceBatchCharges(InvoiceBatch invoiceBatch, IList<Charge> charges, string viewIDValue)
        {
            this.invoiceBatch = invoiceBatch;
            this.charges = charges;
            this.viewID = viewIDValue;
        }

        #region IPrintable Members

        public string PopupPageName
        {
            get { return "InvoiceBatchAdjustmentsPopup.aspx?ViewID=" + this.viewID; }
        }

        #endregion

        public InvoiceBatch InvoiceBatch
        {
            get { return invoiceBatch; }
        }

        public IList<Charge> BatchCharges
        {
            get { return charges; }
        }
    }
}