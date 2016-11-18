using System.Web.UI;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class InvoiceBatchHeader : UserControl
    {
        public void DisplayHeader(InvoiceBatch invoiceBatch)
        {

            if (invoiceBatch.FacilityType == 1 || invoiceBatch.FacilityType == 3)
            {
                BatchLiteral.Text = "Invoice Payment Schedule/Batch:";
            }
            else if (invoiceBatch.FacilityType == 4)
            {
                BatchLiteral.Text = "Advance/Fee Batch:";
            }
            else if (invoiceBatch.FacilityType == 5)
            {
                BatchLiteral.Text = "Drawing/Fee Batch:";
            }
            else //2
            {
                BatchLiteral.Text = "Invoice Processing/Fee Batch:";
            }


            BatchNumberLiteral.Text = invoiceBatch.Number.ToString();
            clientNameLiteral.Text = invoiceBatch.ClientName;
            HeaderLiteral.Text = invoiceBatch.Header;
            DateLiteral.Text = invoiceBatch.Date.ToString();
            ModifiedDateLiteral.Text = invoiceBatch.ModifiedDate.ToString();
            ReleasedDateLiteral.Text = string.IsNullOrEmpty(invoiceBatch.Released.ToString())?"": ("Released: " + invoiceBatch.Released.ToString());
            StatusLiteral.Text = invoiceBatch.Status;
        }
    }
}