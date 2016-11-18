
namespace Cff.SaferTrader.Web.UserControls
{
    /// <summary>
    /// Contains all information required to redirect
    /// </summary>
    public class RedirectionParameter
    {
        private readonly int clientId;
        private readonly int customerId;
        private readonly int batch;
        private readonly string fieldName;

        public RedirectionParameter(string fieldName, int clientId, int customerId, int batch)
        {
            this.fieldName = fieldName;
            this.clientId = clientId;
            this.customerId = customerId;
            this.batch = batch;
        }

        public RedirectionParameter(string fieldName, int clientId, int customerId) : this(fieldName, clientId, customerId, 0) 
        {
        }

        public RedirectionParameter(string fieldName, int clientId) : this(fieldName, clientId, 0)
        {
        }

        public int ClientId
        {
            get { return clientId; }
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        public int Batch
        {
            get { return batch; }
        }

        public string FieldName
        {
            get { return fieldName; }
        }
    }
}