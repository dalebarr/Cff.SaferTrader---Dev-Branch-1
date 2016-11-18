using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableInvoiceBatchRepurchases:IPrintable
    {
        private readonly IList<RepurchasesLine> repurchases;
        private readonly InvoiceBatch invoiceBatch;
        private readonly string viewID;

        public PrintableInvoiceBatchRepurchases(InvoiceBatch invoiceBatch, IList<RepurchasesLine> repurchases, string viewIDValue)
        {
            this.invoiceBatch = invoiceBatch;
            this.repurchases = repurchases;
            this.viewID = viewIDValue;
        }

        #region IPrintable Members

        public string PopupPageName
        {
            get { return "InvoiceBatchRepurchasesPopup.aspx?ViewID=" + this.viewID; }
        }

        #endregion

        public InvoiceBatch InvoiceBatch
        {
            get { return invoiceBatch; }
        }

        public IList<RepurchasesLine> Repurchases
        {
            get { return repurchases; }
        }
    }
}