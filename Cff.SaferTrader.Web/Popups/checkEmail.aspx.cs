using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using System.Net.Mail;


namespace Cff.SaferTrader.Web
{
    public partial class checkEmail : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //var docDetails = SessionWrapper.Instance.DocBag as Cff.SaferTrader.Core.Letters.DocumentMailer;
                var docDetails = SessionWrapper.Instance.Get.DocBag as Cff.SaferTrader.Core.Letters.DocumentMailer;
                
                txtEmailTo.Text = docDetails.SendTo;
                txtEmailCC.Text = docDetails.SendCC;
                EmailBCCLiteral.Text = docDetails.SendBCC;
                txtEmailSubject.Text = docDetails.Subject;
                EmailAttachment.Text = docDetails.FileAttachment;
                txtBody.Text = docDetails.SendBody;
                sendStatusLiteral.Visible = false;

                //MSarza[20161003]: Added hCode referencing so as to track which letter is sent and corresponding info
                //                  passed for appropriate notes insertion. Also added HiddenField hCode to the html form.
                hCode.Value = docDetails.HashCode.ToString();
                HttpContext.Current.Session[docDetails.HashCode.ToString()] = docDetails;
            }

            AttachmentDetail.Visible = false;
        }

        protected void SendEmail_Click(object sender, EventArgs e)
        {
            if (txtEmailTo.Text.Contains("@"))
            {
                String[] strDummy = txtEmailTo.Text.Split('@');
                if (strDummy.Count() < 2) {
                    sendStatusLiteral.Visible = true;
                    sendStatusLiteral.Text = "Invalid email address.";
                }
            }
            else
            {
                sendStatusLiteral.Visible = true;
                sendStatusLiteral.Text = "Invalid email address.";
                return;
            }

            try
            {
                MailMessage mailMsg = new MailMessage();
                if ((AttachmentFile.FileName != null) && (AttachmentFile.FileName.Length > 0))
                { //add attachments to email
                    mailMsg.Attachments.Add(new Attachment(AttachmentFile.PostedFile.InputStream, AttachmentFile.FileName));
                }

                try
                {
                    string fAttachment = ""; // Server.MapPath("/reportfiles/" + EmailAttachment.Text.Trim());
                    //if (EmailAttachment.Text.Trim() == "Click on browse for file attachment.")
                    //{
                    //fAttachment = Server.MapPath("/reportfiles/" + EmailAttachment.Text.Trim());
                    //fAttachment = "C://inetpub//CFFIIS//reportfiles//" + EmailAttachment.Text.Trim();
                    fAttachment = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + EmailAttachment.Text.Trim();

                    mailMsg.Attachments.Add(new Attachment(fAttachment));
                    //}
                }
                catch (Exception)
                {
                    mailMsg.Attachments.Clear();
                }

                //mailMsg.IsBodyHtml = true;
                mailMsg.Subject = txtEmailSubject.Text;

                mailMsg.From = new MailAddress("webadmin@factor.co.nz");
                //mailMsg.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["SendClientLettersEmailFrom"].ToString());

                //txtEmailTo.Text may contain more than one email address! 
                //Split and mailMsg.To.Add separately to avoid an error
                String[] strEmailAddr = txtEmailTo.Text.Split(';');
                if (strEmailAddr.Count() > 0)
                {
                    for (int ix = 0; ix < strEmailAddr.Count(); ix++)
                    {
                        mailMsg.To.Add(new MailAddress(strEmailAddr[ix]));
                    }
                }
                else
                {
                    mailMsg.To.Add(new MailAddress(txtEmailTo.Text));
                }
              
                if (txtEmailCC.Text.Length > 0)
                {
                    //    mailMsg.CC.Add(new MailAddress(txtEmailCC.Text));
                    //}
                    String[] strEmailCC = txtEmailCC.Text.Split(';');
                    if (strEmailCC.Count() > 0)
                    {
                        for (int ix = 0; ix < strEmailCC.Count(); ix++)
                        {
                            mailMsg.CC.Add(new MailAddress(strEmailCC[ix]));
                        }
                    }
                    else
                    {
                        mailMsg.CC.Add(new MailAddress(txtEmailCC.Text));
                    }
                }

                if (EmailBCCLiteral.Text.Length > 0)
                {
                    mailMsg.Bcc.Add(new MailAddress(EmailBCCLiteral.Text));
                }
                mailMsg.Body = txtBody.Text;

                //todo: see if we can put this in thread as user may click on send button multiple times 
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential("webadmin", "~Myf4ct-0r5!");
                smtp.Host = "cfexch2.cff.local";
                //smtp.UseDefaultCredentials = false;

                smtp.UseDefaultCredentials = false;
                //smtp.Host = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"].ToString();

                smtp.Send(mailMsg);
                sendStatusLiteral.Visible = true;
                sendStatusLiteral.Text = "Email sent to " + txtEmailTo.Text;

                //trigger update of customer notes if email sent is letter statement
                //MSarza[20161003]: Use of hCode.Value instead so as to make notes inserted consisten with what is being sent
                //var docDetails = SessionWrapper.Instance.Get.DocBag as Cff.SaferTrader.Core.Letters.DocumentMailer;
                var docDetails = HttpContext.Current.Session[hCode.Value] as Cff.SaferTrader.Core.Letters.DocumentMailer;

                if(docDetails.SendTo != txtEmailTo.Text)
                    docDetails.SendTo = txtEmailTo.Text; //MSarza[20160929]
                if (docDetails.SendCC != txtEmailCC.Text)
                    docDetails.SendCC = txtEmailCC.Text; //MSarza[20160929]

                if (docDetails.LetterType == 0)  // LetterType: 0- letter, 1-statement, 2-banking details
                {
                    Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(docDetails);
                    System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterSent);
                    thrMEvt.Start();
                }
            }
            catch (Exception exc)
            {
                sendStatusLiteral.Text = "Sending Failed: " + exc.Message;
                sendStatusLiteral.Visible = true;
            }

            finally
            {
                SendEmail.Enabled = false;
                SendEmail.BackColor = System.Drawing.ColorTranslator.FromHtml("#CFCFCF");
                AttachmentFile.Visible = false;
                AttachmentDetail.Text = AttachmentFile.FileName.ToString();
                AttachmentDetail.Visible = true;
              
                txtEmailTo.Enabled = false;
                txtEmailCC.Enabled = false;
                txtEmailSubject.Enabled = false;
            }
        }

        private void sendViaLink() {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder("");
            strBuilder.Append("window.open(\"mailto:" + txtEmailTo.Text + 
                            "?subject='" + txtEmailSubject.Text + "'");
            if (txtEmailCC.Text.Length == 0)
            {
                strBuilder.Append("?cc=" + EmailBCCLiteral.Text);
            }
            else {
                strBuilder.Append("?cc=" + txtEmailCC.Text);
                strBuilder.Append("?bcc=" + EmailBCCLiteral.Text);
            }
            //todo: attachment?
            strBuilder.Append("&body= '"+ txtBody.Text + "'\")");
            Response.Write(strBuilder);
        }
    }
}
