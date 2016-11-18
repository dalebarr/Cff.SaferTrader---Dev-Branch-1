using System;

namespace Cff.SaferTrader.Core
{
    public class InvoiceNumber
    {
        public static void Validate(string invoiceNumber)
        {
            if (invoiceNumber.Trim().Length<3)
            {
                throw new ArgumentException("Please type in at leaset 3 characters for invoice number.");
            }
        }
    }
}