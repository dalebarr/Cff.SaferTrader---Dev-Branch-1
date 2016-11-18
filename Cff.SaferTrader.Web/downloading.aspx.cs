using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Threading;


namespace Cff.SaferTrader.Web
{
    public partial class downloading : System.Web.UI.Page
    {
       //private static string errorMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["file"] != null)
            {
                string filename = Request["file"].ToString();
                //note: have moved this part to the calling program
                //if (filename.Contains('\')==false) {
                //    filename = "\\reportfiles\\" + filename;
                //    string fileUrl = Server.MapPath(filename);
                //    filename = Server.MapPath(filename);
                //    fileDownload(filename, fileUrl);
                //}
                fileDownload(filename, "");
            }
        }

        private void fileDownload(string fileName, string fileUrl)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(fileName);
            if (file.Exists) {
                   Response.Clear();
                   Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                   Response.AddHeader("Content-Length", file.Length.ToString());
                   Response.ContentType = "application/octet-stream";
                   Response.WriteFile(file.FullName);
                   Response.End();
            } else { Response.Write("This file does not exist."); }
       }

    }
}
