using System;
namespace Cff.SaferTrader.Web
{
    public class SearchCustomerEventArgs : EventArgs
    {
        public string CustomerNameId { get; set; }

        public SearchCustomerEventArgs(string customerNameId)
        {
            CustomerNameId = customerNameId;
        }
    }
}