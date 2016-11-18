using System.IO;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Web.UserControls.Interfaces;

namespace Cff.SaferTrader.Web.UserControls
{
    public abstract class ExportablePanel : UserControl, IExportable, Cff.SaferTrader.Core.Views.IRedirectableView
    {
        public abstract void Export();

        /// <summary>
        /// Writes contents of stream into Response as an XML file and disposes stream
        /// </summary>
        protected void WriteToResponse(MemoryStream stream, string fileName)
        {
            try
            {
                Page.Response.Clear();
                Page.Response.Buffer = false;
                Page.Response.AppendHeader("Content-Type", string.Format("application/vnd.ms-excel"));
                Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Page.Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));

                Page.Response.BinaryWrite(stream.ToArray());
                Page.Response.End();
                stream.Dispose();
            }
            catch {
               Page.Response.End();
            }         
        }

        protected void WriteToResponse(byte[] dta, string fileName)
        {
            try
            {
                Page.Response.Clear();
                Page.Response.Buffer = false;
                Page.Response.AppendHeader("Content-Type", string.Format("application/vnd.openxmlformat-officedocument.spreadsheet.sheet"));
                Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Page.Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", fileName));

                Page.Response.BinaryWrite(dta);
                Page.Response.End();
            }
            catch {
                Page.Response.End();
            }
        }

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }

        public ICffClient Client
        {
            get { return SessionWrapper.Instance.Get.ClientFromQueryString; } 
            set {  }
            // set { SessionWrapper.Instance.Get.ClientFromQueryString = value; }
        }

        //public CffCustomer Customer
        public ICffCustomer Customer
        {
            get { return SessionWrapper.Instance.Get.CustomerFromQueryString; }
            set { SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)value; }
            //get { return SessionWrapper.Instance.Get.Customer; }
            //set { SessionWrapper.Instance.Get.Customer = value; }
        }
    }
}