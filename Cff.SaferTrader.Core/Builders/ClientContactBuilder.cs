using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Builders
{
    public class ClientContactBuilder
    {
        private readonly CleverReader reader;

        public ClientContactBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public IList<ClientContact> BuildAll()
        {
            IList<ClientContact> contacts = new List<ClientContact>();
            while (!reader.IsNull && reader.Read())
            {
                ClientContact contact = Build();
                contacts.Add(contact);
            }
            return contacts;
        }

        public ClientContact Build()
        {
            return new ClientContact(reader.FromBigInteger("ClientContactsID"),
                                   reader.FromBigInteger("ClientId"),
                                   reader.FromBigInteger("ClientNumber"),
                                   reader.ToString("ClientName"),
                                   reader.ToString("LName"),
                                   reader.ToString("FName"),
                                   reader.ToString("Phone"),
                                   reader.ToString("Fax"),
                                   reader.ToString("Email"),
                                   reader.ToString("Cell"),
                                   reader.ToString("Role"),
                                   reader.ToString("Address1"),
                                   reader.ToString("Address2"),
                                   reader.ToString("Address3"),
                                   reader.ToString("Address4")
                                   );
        }
    }
}
