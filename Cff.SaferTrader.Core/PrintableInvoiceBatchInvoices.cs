using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PrintableInvoiceBatchInvoices : IPrintable
    {
        private readonly InvoiceBatch invoiceBatch;
        private readonly IList<Invoice> invoices;
        private readonly string viewID;

        public PrintableInvoiceBatchInvoices(InvoiceBatch invoiceBatch, IList<Invoice> invoices, string viewIDValue)
        {
            this.invoiceBatch = invoiceBatch;
            this.invoices = invoices;
            this.viewID = viewIDValue;
        }

        #region IPrintable Members

        public string PopupPageName
        {
            get { return "InvoiceBatchInvoicesPopup.aspx?ViewID=" + this.viewID; }
        }

        #endregion

        public InvoiceBatch InvoiceBatch
        {
            get { return invoiceBatch; }
        }

        public IList<Invoice> BatchInvoices
        {
            get { return invoices; }
        }
    }

    [Serializable]
    public class PrintableInvoiceBatchNonFactoredInvoices : IPrintable
    {
        private readonly InvoiceBatch invoiceBatch;
        private readonly IList<Invoice> nonFactoredInvoices;
        private readonly string viewID;

        public PrintableInvoiceBatchNonFactoredInvoices(InvoiceBatch invoiceBatch, IList<Invoice> nonFactoredInvoices, string viewIDValue)
        {
            this.invoiceBatch = invoiceBatch;
            this.nonFactoredInvoices = nonFactoredInvoices;
            this.viewID = viewIDValue;
        }

        #region IPrintable Members

        public string PopupPageName
        {
            get { return "InvoiceBatchNonFactoredInvoicesPopup.aspx?ViewID=" + this.viewID; }
        }

        #endregion

        public InvoiceBatch InvoiceBatch
        {
            get { return invoiceBatch; }
        }

        public IList<Invoice> BatchInvoices
        {
            get { return nonFactoredInvoices; }
        }
    }

    [Serializable]
    public class PrintableInvoiceBatches : IPrintable
    {
        private readonly IList<InvoiceBatch> _InvoiceBatches;
        private readonly string _viewID;

        public PrintableInvoiceBatches(IList<InvoiceBatch> invoiceBatches, string viewIDValue)
        {
            this._InvoiceBatches = invoiceBatches;
            this._viewID = viewIDValue;
        }

        #region IPrintable Members

        public string PopupPageName
        {
            get { return "InvoiceBatchesPopup.aspx?ViewID=" + this._viewID; }
        }

        #endregion


        public IList<InvoiceBatch> InvoiceBatches
        {
            get { return this._InvoiceBatches; }
        }
    }

}