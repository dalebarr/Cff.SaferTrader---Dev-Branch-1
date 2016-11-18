using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using System.IO;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class LettersPopup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var printable = SessionWrapper.Instance.Get.PrintBag as Cff.SaferTrader.Core.Letters.PrintableLetters;
                string FileError = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + "error.txt";
                if (printable != null)
                {
                    if (printable.PrintThis == true)
                    {
                        //frameDetails.Visible = false;
                        //DisplayDocument(printable.LetterName);
                        
                        //populateLabel(printable.LetterName);
                        //Response.AddHeader("content-disposition", "inline; filename=" + printable.LetterName);

                        try //Copied from below!!! would be nice if it was sent direclty to the printer 
                        {   

                            string FileSource = ""; //ResolveUrl("http://www.nbr.co.nz");
                            //Msarza [20150807] - moved report files to web.config rather than being hardcoded
                            //FileSource = ResolveUrl("http://" + Request.Url.Authority.ToString() + "/Reportfiles/" + printable.LetterName);
                            FileSource = ResolveUrl("http://" + Request.Url.Authority.ToString() + "/" + System.Configuration.ConfigurationManager.AppSettings["IISReportsFolder"] + "/" + printable.LetterName);
                            frameDetails.Attributes.Remove("src");
                            frameDetails.Attributes.Add("src", FileSource);
                        }
                        catch (Exception exc1)
                        {
                            File.WriteAllText(FileError, @"Error: " + exc1.Message);
                            frameDetails.Attributes.Remove("src");
                            frameDetails.Attributes.Add("src", System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + printable.LetterName);
                        }




                    }
                    else
                    {
                        string debugMode = System.Configuration.ConfigurationManager.AppSettings["TestMode"];
                        //this.reportTitleLiteral.Text = ""; //printable.ReportHeader;
                      
                        if (debugMode.Contains("true"))
                        {
                            frameDetails.Attributes["src"] = printable.LetterName;
                        }
                        else
                        {
                            //string host = System.Net.Dns.GetHostName(); //System.Environment.MachineName
                            //System.Net.IPHostEntry ipentry = System.Net.Dns.GetHostEntry(host); //GetHostByName(host);
                            //System.Net.IPAddress[] ipaddresses = System.Net.Dns.GetHostAddresses(host); //ipentry.AddressList;
                            //string thisHostsName = System.Configuration.ConfigurationManager.AppSettings["thisHostName"];//ipentry.HostName;
                            //string ipaddress = System.Configuration.ConfigurationManager.AppSettings["thisHostIP"];
                            //string FileError = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + "error.txt";

                            try
                            {   //todo: in windows 7 the first address is IPV6. we want IPV4 format here.
                                //string ipaddress = ipentry.AddressList[2].ToString(); //ipv4
                                //note: result of gethostaddresses are not the same when implemented in different servers
                                //string FileSource = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + printable.LetterName;
                                //string ipaddress = ipentry.AddressList[0].ToString();

                                //string FileSource = ResolveUrl("http://" + ipaddress + "/ReportFiles/" + printable.LetterName);
                                //if  (System.Configuration.ConfigurationManager.AppSettings["useHostName"] == "true")
                                //{
                                //    FileSource = ResolveUrl("http://" + host + "/ReportFiles/" + printable.LetterName);
                                //}
                                
                                ////string FileSource = ResolveUrl("~/ReportFiles/" + printable.LetterName);
                                //string FileSource = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + printable.LetterName;

                                string FileSource = ""; //ResolveUrl("http://www.nbr.co.nz");
                                //FileSource = ResolveUrl("http://" + Server.MachineName + "/ReportFiles/" + printable.LetterName);

                                //Msarza [20150807] - moved report files to web.config rather than being hardcoded
                                //FileSource = ResolveUrl("http://" + Request.Url.Authority.ToString() + "/Reportfiles/" + printable.LetterName);
                                FileSource = ResolveUrl("http://" + Request.Url.Authority.ToString() + "/" + System.Configuration.ConfigurationManager.AppSettings["IISReportsFolder"] + "/" + printable.LetterName);

                                frameDetails.Attributes.Remove("src");
                                frameDetails.Attributes.Add("src", FileSource);

                            }
                            catch (Exception exc1)
                            {
                                File.WriteAllText(FileError, "Error: " + exc1.Message );
                                frameDetails.Attributes.Remove("src");
                                ////frameDetails.Attributes.Add("src", FileError);
                                //frameDetails.Attributes.Add("src", "http://" + thisHostsName + "/reportfiles/" + printable.LetterName);
                                frameDetails.Attributes.Add("src", System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + printable.LetterName);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Response.Write(exc.Message);
            }
        }

        protected void DisplayDocument(string htmlFilePath)
        {  //have garbage data but only on end of document; display print button but image is broken
            //Read the Html File as Byte Array and Display it on browser
            //Response.ContentType = "application/msword"; //this opens saveas window
            byte[] bytes;
            using (FileStream fs = new FileStream(htmlFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(fs);
                bytes = reader.ReadBytes((int)fs.Length);
                fs.Close();
            }


            System.Text.StringBuilder strBody2 = new System.Text.StringBuilder("");
            strBody2.Append("<div id='printControlBar'>" +
                            "<table><tr>" +
                            "<th align='left' style='width:750'><img src='../images/printLogo_hiRes.jpg' width='215' height='25' border='0' alt='print Logo' /></th>" +
                            "<th align='right'>"+ 
                            "<input name='print' type='image' value='Print' src='../images/btn_print_new.png' alt='Print' onclick='window.print()'/>"+
                            "</th></tr></table>" +
                            "</div>");
            byte[] byteBody2 = System.Text.Encoding.ASCII.GetBytes(strBody2.ToString());
            Response.Clear();
            Response.BinaryWrite(byteBody2);
            Response.BinaryWrite(bytes);
            Response.Flush();

            //Delete the Html File
            //File.Delete(htmlFilePath);
            Response.End();
        }

        private void populateLabel(string filename)
        { //this puts a lot of garbage data to page
            System.Text.StringBuilder strHtml;
            //filename = "C:\\inetpub\\CFFIIS\\reportfiles\\2_Month_Letter_Bublitz_Transport_Limited_20101110090734.htm";
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fs);
                strHtml = new System.Text.StringBuilder(reader.ReadToEnd());
                fs.Close();
            }
            string strHClean = CleanWordHtml(strHtml.ToString());
            strHClean = FixEntities(strHClean);
            //lblValue.Text = strHClean;
            File.Delete(filename);
        }


        static string CleanWordHtml(string html)
        { //todo: test this
            System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
            // get rid of unnecessary tag spans (comments and title)
            sc.Add(@"<!--(\w|\W)+?-->");
            sc.Add(@"<title>(\w|\W)+?</title>");
            // Get rid of classes and styles
            sc.Add(@"\s?class=\w+");
            sc.Add(@"\s+style='[^']+'");
            // Get rid of unnecessary tags
            sc.Add(
            @"<(meta|link|/?o:|/?style|/?div|/?st\d|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>");
            // Get rid of empty paragraph tags
            sc.Add(@"(<[^>]+>)+&nbsp;(</\w+>)+");

            // remove bizarre v: element attached to <img> tag
            
            sc.Add(@"\s+v:\w+=""[^""]+""");
            
            // remove extra lines
            sc.Add(@"(\n\r){2,}");
            foreach (string s in sc)
            {
                html = System.Text.RegularExpressions.Regex.Replace(html, s, "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            return html;
        }

        static string FixEntities(string html)
        { //todo: test this
            System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("\"", "&ldquo;");
            nvc.Add("\"", "&rdquo;");
            nvc.Add("–", "&mdash;");
            foreach (string key in nvc.Keys)
            {
                html = html.Replace(key, nvc[key]);
            }
            return html;
        }

     }
}