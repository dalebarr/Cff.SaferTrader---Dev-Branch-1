using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.SaferTrader.Core.Builders
{
    //MSarza [20150817]
    public class CffCustomerContactBuilder
    {
        private readonly CleverReader reader;

        public CffCustomerContactBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public IList<CffCustomerContact> BuildAll()
        {
            IList<CffCustomerContact> contacts = new List<CffCustomerContact>();
            while (!reader.IsNull && reader.Read())
            {
                CffCustomerContact contact = Build();
                contacts.Add(contact);
            }
            return contacts;
        }

        public CffCustomerContact Build()
        {               
            return new CffCustomerContact()
            {
                IsDefault = reader.ToBoolean("IsDefault"),
                LName = reader.ToString("LName"),
                FName =reader.ToString("FName"),
                Phone = reader.ToString("Phone"),
                Fax = reader.ToString("Fax"),
                Email = reader.ToString("Email"),
                Cell = reader.ToString("Cell"),
                Role = reader.ToString("Role"),
                Attn = reader.ToBoolean("Attn"),
                EmailStatement = reader.ToBoolean("EmailStatement"),
                EmailReceipt = reader.ToInteger("EmailReceipt")
            };
        }
    }
}
