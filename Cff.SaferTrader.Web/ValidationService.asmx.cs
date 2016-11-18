using System;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web
{
    [WebService(Namespace = "http://www.cff.co.nz/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ValidationService : WebService
    {
        [WebMethod]
        public string ValidateDateRange(string startDate, string endDate)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(endDate) && (endDate!="undefined"))
                    DateRange.Validate(new Date(DateTime.Parse(startDate)), new Date(DateTime.Parse(endDate)));
            }
            catch (ArgumentException e)
            {
                message = e.Message;
            }
            return message;
        }

        [WebMethod]
        public string ValidateInvoiceNumber(string invoiceNumber)
        {
            string message = string.Empty;
            try
            {
                InvoiceNumber.Validate(invoiceNumber);
            }
            catch (ArgumentException e)
            {
                message = e.Message;
            }
            return message;
        }

        [WebMethod]
        public string GetPostData(CffOnlineFormApplicationExt model)
        {
            return "test";
        }
    }
}