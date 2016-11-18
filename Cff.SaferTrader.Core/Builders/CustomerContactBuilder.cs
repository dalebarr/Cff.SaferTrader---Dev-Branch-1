using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Builders
{
    public class CustomerContactBuilder
    {
        private readonly CleverReader reader;

        public CustomerContactBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public IList<CustomerContact> BuildAll()
        {
            IList<CustomerContact> contacts = new List<CustomerContact>();
            while (!reader.IsNull && reader.Read())
            {
                CustomerContact contact = Build();
                contacts.Add(contact);
            }
            return contacts;
        }

        public CustomerContact Build()
        {
            return new CustomerContact(reader.FromBigInteger("CustContactsID"),
                                       reader.FromBigInteger("ClientId"),
                                       reader.FromBigInteger("ClientNumber"),
                                       reader.ToString("ClientName"),
                                       reader.FromBigInteger("CustomerID"),
                                       reader.FromBigInteger("CustomerNumber"),
                                       reader.ToString("CustomerName"),
                                       reader.ToString("LName"),
                                       reader.ToString("FName"),
                                       reader.ToString("Phone"),
                                       reader.ToString("Fax"),
                                       reader.ToString("Email"),
                                       reader.ToString("Cell"),
                                       reader.ToString("Role"),
                                       reader.ToBoolean("isDefault"),
                                       reader.ToBoolean("Attn"),
                                       reader.ToDateTimeWithMinimum("Modified"),
                                       reader.ToSmallInteger("ModifiedBy"),
                                       reader.ToString("Address1"),
                                       reader.ToString("Address2"), 
                                       reader.ToString("Address3"), 
                                       reader.ToString("Address4"),
                                       reader.ToBoolean("emailstatement"),
                                       reader.ToSmallInteger("emailReceipt")             //MSarza - inserted line

                                       );
        }
    }
}