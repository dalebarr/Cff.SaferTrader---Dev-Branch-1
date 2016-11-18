using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PurchaserDetails
    {
        private readonly CffCustomer customer;
        private readonly Address customerAddress;
        private readonly CffClient client;

        public PurchaserDetails(CffClient client, CffCustomer customer, Address customerAddress)
        {
            ArgumentChecker.ThrowIfNull(customer, "customer");
            ArgumentChecker.ThrowIfNull(client, "client");
            ArgumentChecker.ThrowIfNull(customerAddress, "customerAddress");

            this.customer = customer;
            this.customerAddress = customerAddress;
            this.client = client;
        }

        public string Name
        {
            get { return customer.Name; }
        }

        public Address Address
        {
            get { return customerAddress; }
        }

        public string Reference
        {
            get { return string.Format("{0}/{1}", client.Id, customer.Number); }
        }

        public int Id
        {
            get { return customer.Id; }
        }

        public string ClientName
        {
            get { return client.Name; }
        }

        public int Number
        {
            get { return customer.Number; }
        }

        public int Clientid
        {
            get { return client.Id; }
        }

        public string CollectionsBankAccount
        {
            get { return client.CollectionsBankAccount; }
        }


    }
}