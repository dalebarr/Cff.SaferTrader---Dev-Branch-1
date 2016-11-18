using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.SaferTrader.Core.Builders
{
    //MSarzea [20150731] 
    public class CffMgtDetailsBuilder
    {
        private readonly IReader reader;

        public CffMgtDetailsBuilder(IReader reader)
        {
            this.reader = reader;
        }

        public CffMgtDetails Build()
        {
            return new CffMgtDetails(reader.ToString("Name"),
                                reader.ToString("LegalEntity1"),
                                reader.ToString("LegalEntity2"),
                                reader.ToString("LegalEntity3"),
                                //reader.ToString("PostalAddress"),
                                reader.ToString("PostalAddress1"),
                                reader.ToString("PostalAddress2"),
                                reader.ToString("PostalAddress3"),
                                reader.ToString("PostalAddress4"),
                                //reader.ToString("PhysicalAddress"),
                                reader.ToString("PhysicalAddress1"),
                                reader.ToString("PhysicalAddress2"),
                                reader.ToString("PhysicalAddress3"),
                                reader.ToString("PhysicalAddress4"),
                                reader.ToString("PhysicalAddress5"),
                                reader.ToString("Phone"),
                                reader.ToString("Fax"),
                                reader.ToString("web"),
                                reader.ToString("email"),
                                reader.ToString("Bank"),
                                reader.ToString("Branch"),
                                reader.ToString("BankAccount"),
                                reader.ToString("EnquiryEmailContact")); 
        }
    }
}
