using System;
using System.Collections.Generic;


namespace Cff.SaferTrader.Core.Views
{
    public interface IInvoiceBatchesView
    {
        void DisplayInvoiceBatches(IList<InvoiceBatch> invoiceBatches, bool boolPar);
        void ShowAllClientsView();
        void ShowClientView();
        void PopulateBatchTypeDropDown();
        void SelectFirstBatchRecord();

        void StartDBFetchEventHandler(object sender, EventArgs e);
        void EndDBFetchEventHandler(object sender, EventArgs e);
    }
}