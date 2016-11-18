using Cff.SaferTrader.Core.Services;

namespace Cff.SaferTrader.Web.UserControls
{
    /// <summary>
    /// Consumes RedirectionService to redirect based on RedirectionParameter
    /// </summary>
    public class Redirector
    {
        private readonly IRedirectionService service;

        public Redirector(IRedirectionService service)
        {
            this.service = service;
        }

        public void Redirect(RedirectionParameter parameter)
        {
            string fieldName = parameter.FieldName;
            int clientId = parameter.ClientId;
            int customerId = parameter.CustomerId;
            int batch = parameter.Batch;

            if (fieldName.Equals("ClientName"))
            {
                service.SelectClientAndRedirectToDashboard(clientId);
            }
            else if (fieldName.Equals("CustomerName") && customerId > 0)
            {
                service.SelectClientCustomerAndRedirectToDashboard(clientId, customerId);
            }
            else if (fieldName.Equals("Batch") && batch > 0)
            {
                service.SelectClientCustomerAndRedirectToInvoiceBatches(clientId, customerId, batch);
            }
        }
    }
}