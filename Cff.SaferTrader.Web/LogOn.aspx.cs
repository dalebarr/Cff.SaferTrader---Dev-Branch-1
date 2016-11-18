using System;
using System.Web.Security;
using System.Web.UI;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Common;
using Cff.SaferTrader.Web.App_GlobalResources;
using Newtonsoft.Json;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Web.Http;

namespace Cff.SaferTrader.Web
{
    public partial class LogOn : Page, ILogOnView
    {
        private LogOnPresenter presenter;
        private String UrlAddress;
        //private static bool isLoaded;

        public string ParamRefQueryString { get { return Request.QueryString[QueryString.LogonParam.ToString()]; } }
        public string ParamActivate1String { get { return Request.QueryString[QueryString.ActivateParam1.ToString()]; } }
        public string ParamActivate2String { get { return Request.QueryString[QueryString.ActivateParam2.ToString()]; } }
        public string ParamApproval1String { get { return Request.QueryString[QueryString.ApprovalParam1.ToString()]; } }
        public string ParamApproval2String { get { return Request.QueryString[QueryString.ApprovalParam2.ToString()]; } }
        public string ParamLoginUserString { get { return Request.QueryString[QueryString.User.ToString()]; } }

        private void InitializeCurrentPathForJavaScript()
        {
            string relativePathToRoot = RelativePathComputer.ComputeRelativePathToRoot(Server.MapPath("~"),
                                                                                       Server.MapPath("."));
            string script = string.Format(@"var relativePathToRoot='{0}';", relativePathToRoot);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "initializeCurrentPath", script, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Write("Page_Load started");
            //if (!IsPostBack)
            //{
            // need to apply for other pages to improve performance
            Response.Cache.SetExpires(DateTime.Now.AddMonths(2)); // just adjust the expiration to suit the need
            Response.Cache.SetCacheability(System.Web.HttpCacheability.ServerAndNoCache);
            Response.Cache.SetValidUntilExpires(true);
            String sTitle = "";
            String sMsg = "";
            presenter = LogOnPresenter.Create(this);
            UrlAddress = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
            CffUserActivation record;
            String strStatus = this.Response.Status;
            String requestURL = this.Request.Url.ToString();
            bool isRequestAuthenticated = Request.IsAuthenticated;
            string strComID = Request["ComID"];


            Trace.Write(requestURL+" comID"+strComID);

            if (!string.IsNullOrEmpty(strComID) && strComID.ToLower().Trim() == "loginmyob")
            {
                LogOnControl.Visible = false;
                PasswordRecovery1.Visible = false;
                CreateUserWizard1.Visible = false;

                string strCID = Request["cid"];
                Core.SecurityManager.CAuth authUser = new Core.SecurityManager.CAuth(Convert.ToInt32(Request["cid"]),
                                                    Request["passkey"], Convert.ToInt16(Request["ctype"]), Request["ttype"],
                                                    Request["trxFrom"], Request["trxTo"], Request["mSIDs"]);

                string strTType = Request["ttype"];
                int result;
                if (int.TryParse(strTType, out result))
                {
                    if (authUser.isAuthenticated(true))
                    {
                        string xmlProduced = authUser.getXMLFileName();
                        string importMYOBURL = "~/ImportMYOB.aspx?clientID=" + strCID + "&inXMLMYOB=" + xmlProduced;
                        Context.RewritePath(importMYOBURL);
                        string redirecturl = FormsAuthentication.GetRedirectUrl(strCID, false);

                        Context.SkipAuthorization = true;
                        Context.User.IsInRole("Client");
                        Context.Response.RedirectLocation = importMYOBURL;
                        FormsAuthentication.SetAuthCookie(strCID, false);
                        Response.Redirect(importMYOBURL);
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("Authentication Failed!");
                    }
                }
                else
                { //non-numeric, check if myob keyupdate request, keysync request, ftp upload or client on-line alert
                    if (authUser.isAuthenticated(false))
                    {
                        string kType = strTType.Substring(0, 4);
                        switch (StringEnum.ParseAuthKeys(kType))
                        {
                            case CAUTHKEYS.KEY_UPDATE:
                                {//Update File Key Settings
                                    authUser.UpdateKey(StringEnum.GetStringValue(CAUTHKEYS.KEY_UPDATE));
                                    Response.Clear();
                                    Response.Write("OK!");
                                }
                                break;

                            case CAUTHKEYS.KEY_SYNCH:
                                { //Start Key Synchronization or update
                                    Int16 cx = Convert.ToInt16(strTType.Substring(4, 1));
                                    if (cx == (Int16)MYOB_KUPDATE_TYPE.TRIGGER_KEY_SYNC)
                                    {
                                        authUser.TriggerKeySync(strTType);
                                        Response.Clear();
                                        Response.Write("OK!");
                                    }
                                    else if (cx == (Int16)MYOB_KUPDATE_TYPE.RETRIEVE_FILEKEYS)
                                    {
                                        authUser.RetrieveKeys();
                                        string importMYOBURL = "ImportMYOB.aspx?clientID=" + strCID + "&inKSEMYOB="
                                                                    + authUser.getExportSettingsFName() + "&inKSIMYOB=" + authUser.getImportSettingsFName();
                                        Context.RewritePath(importMYOBURL);
                                        string redirecturl = FormsAuthentication.GetRedirectUrl(strCID, false);

                                        Context.SkipAuthorization = true;
                                        Context.User.IsInRole("Client");
                                        Context.Response.RedirectLocation = importMYOBURL;
                                        FormsAuthentication.SetAuthCookie(strCID, false);
                                        Response.Redirect(importMYOBURL); //ok wo viewid in querystring as this is a background authentication
                                    }
                                    else //cx==2
                                    {
                                        authUser.DeleteFiles();
                                        Response.Clear();
                                        Response.Write("OK!");
                                    }
                                }
                                break;

                            case CAUTHKEYS.FILEKEY_UPLOAD:
                                {  //update exkeyhist, set lastupdated to sent filename
                                    string strFName = strTType.Substring(4);
                                    authUser.NotifyFTPUpload(strFName);
                                    Response.Clear();
                                    Response.Write("OK!");
                                }
                                break;

                            case CAUTHKEYS.FILEKEY_DOWNLOAD:
                                { //download client file keys
                                    string lastImportedFileName = authUser.getLastImportedFile(strCID, System.Configuration.ConfigurationManager.AppSettings.Get("ftpFolder").ToString());
                                    string importMYOBURL = "~/ImportMYOB.aspx?clientID=" + strCID + "&inXMLMYOB=" + lastImportedFileName;
                                    Context.RewritePath(importMYOBURL);
                                    string redirecturl = FormsAuthentication.GetRedirectUrl(strCID, false);

                                    Context.SkipAuthorization = true;
                                    Context.User.IsInRole("Client");
                                    Context.Response.RedirectLocation = importMYOBURL;
                                    FormsAuthentication.SetAuthCookie(strCID, false);
                                    Response.Redirect(importMYOBURL);
                                }
                                break;

                            case CAUTHKEYS.CLIENT_IS_ONLINE:
                                { //tag client as online
                                    authUser.NotifyClientStatus(Convert.ToInt32(strCID), "ONLINE");
                                    Response.Clear();
                                    Response.Write("OK!");
                                }
                                break;

                            case CAUTHKEYS.CLIENT_IS_OFFLINE:
                                { //tag client as offline
                                    authUser.NotifyClientStatus(Convert.ToInt32(strCID), "OFFLINE");
                                    Response.Clear();
                                    Response.Write("OK!");
                                }
                                break;


                            default:
                                //else do sumthing else
                                break;
                        }
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("Authentication Failed!");
                    }
                }

            }
            else if (strComID == "cffApp")
            {
                Core.Letters.stpCaller vStpCaller = new Core.Letters.stpCaller();

                StreamReader reader = new StreamReader(Request.InputStream);
                string jsonRaw = reader.ReadToEnd();
                reader.Close();

                //string jsonRaw = new StreamReader(Request.InputStream).ReadToEnd();     // read json data in request stream
                string strVal = Request.Params.ToString();
                string rootPath = System.Configuration.ConfigurationManager.AppSettings["cffApplicationAttachedFiles"];
                string finalName = string.Empty;
                string appId = string.Empty;

                var finalJson = string.Empty;
                var properJson = string.Empty;
                int jsNdx2 = 0;
                //int reqObjLen = jsonRaw.Length;  // get request object size

                HttpFileCollection fAttachments = Request.Files;   // read attached file(s) in request stream

                Trace.Write("jsonRaw: " + jsonRaw);

                //start cleanup  
                string delString = "}--";
                int jsNdx = jsonRaw.IndexOf("{\"Id\":"); // start of json data

                if (jsNdx > 0)
                {
                    jsNdx2 = jsonRaw.LastIndexOf("}");
                    properJson = jsonRaw.Substring(jsNdx, ((jsNdx2 + 1) - jsNdx)); // sanitise json values

                    try
                    {
                        //JavaScriptSerializer js = new JavaScriptSerializer();
                        //finalJson = js.Serialize(js.DeserializeObject(properJson));   // check if json is valid
                        finalJson = properJson;
                    }
                    catch (Exception jEx)
                    {
                        if (jEx is ArgumentException)
                        {
                            Console.WriteLine("{0}", jEx.Message);
                            // fix JSON data
                            int jsNdxNew = (properJson.LastIndexOf(delString));
                            finalJson = properJson.Substring(jsNdx, (jsNdxNew + 1) - jsNdx);  // sanitise json values
                            //properJson.Remove(jsNdxNew, (reqObjLen - jsNdxNew));  // remove unnecessary chars
                        }
                    }
                }

                // send data via email to developer
                var mailTo = System.Configuration.ConfigurationManager.AppSettings["cffEmailRecipientWhenError"];
                var smtp = new SmtpClient();
                var credential = (NetworkCredential)smtp.Credentials;
                var smtpSect = (System.Net.Configuration.SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                MailMessage mailMsg = new MailMessage();
                mailMsg.SubjectEncoding = Encoding.UTF8;
                mailMsg.Subject = "re: CFF Borrower Online App";
                mailMsg.From = new MailAddress(smtpSect.From);
                mailMsg.To.Add(new MailAddress(mailTo));                
                mailMsg.Body = "Json data processed : [ Json start index : " + jsNdx + "] [ Request Object : " + jsonRaw + " ] [ Final JSON : " + finalJson + "]";

                smtp.Credentials = credential;
                smtp.Host = smtpSect.Network.Host;
                smtp.UseDefaultCredentials = false;
                smtp.Send(mailMsg);

                // Response.Write("Done reading Request Object !");
                // send data via email to developer
                
                int ndx = strVal.IndexOf("ASP.NET");
                if (ndx > 0)
                    strVal = strVal.Substring(0, ndx - 1);
                else
                {
                    ndx = strVal.IndexOf("ALL_HTTP");
                    if (ndx > 0)
                        strVal = strVal.Substring(0, ndx - 1);
                }
                //end cleanup

                if (!string.IsNullOrEmpty(strVal) && (jsNdx > 0))
                {
                    Response.Clear();
                    string[] sPar = strVal.Split('&');
                    string sAction = "";
                    if (sPar.Length > 1)
                    {
                        try
                        {
                            var app = JsonConvert.DeserializeObject<CffOnlineFormApplicationExt>(finalJson);
                            List<object> objParam = new List<object>();
                            sAction = sPar[0].Split('=')[0];
                            objParam.Clear();
                            objParam.Add("INSERT");
                            objParam.Add(app.Id);
                            objParam.Add(app.ApplicantType);
                            objParam.Add(app.Type);
                            objParam.Add(app.Name);
                            objParam.Add(app.CompanyNumber);
                            objParam.Add(app.NZBN);
                            objParam.Add(app.Telephone);
                            objParam.Add(app.Email);
                            objParam.Add(app.Profitable);
                            objParam.Add(app.Liabilities);
                            objParam.Add(app.PaymentPlan);
                            objParam.Add(app.NumActiveCustomers);
                            objParam.Add(app.LastMonthSales);
                            objParam.Add(app.LastYearSales);
                            objParam.Add(app.TotalDebtor);
                            objParam.Add(app.ValueStockOnHand);
                            objParam.Add(app.PreviousConvictionsOrObligations);
                            objParam.Add(app.Google);
                            objParam.Add(app.RadioStation);
                            objParam.Add(app.PaperAds);
                            objParam.Add(app.Referral);
                            objParam.Add(app.InterestCoNz);
                            objParam.Add(app.Other);
                            objParam.Add(app.NameSignature);
                            objParam.Add(app.DateSigned);

                            byte[] signF = Convert.FromBase64String(app.SignatureFile);

                            objParam.Add(signF);
                            objParam.Add(app.RandomId);

                            System.Data.DataSet theDS = vStpCaller.executeSPDataSet(objParam, Core.Letters.stpCaller.stpType.AddCffCustomerApplication);

                            appId = app.Id.ToString();               

                            List<object> stParam = new List<object>();
                            for (int i = 0; i < app.Directors.Count; i++)
                            {
                                stParam.Clear();
                                stParam.Add("INSERT");
                                stParam.Add(app.Directors[i].FullName);
                                stParam.Add(app.Directors[i].DateOfBirth);
                                stParam.Add(app.Directors[i].Trust);

                                var tList = "";
                                for (int i2 = 0; i2 < app.Directors[i].TrustList.Count; i2++)
                                {
                                    tList += "{ OwnershipType : " + app.Directors[i].TrustList[i2].OwnershipType + " / ";
                                    tList += "Address : " + app.Directors[i].TrustList[i2].Address + " / ";
                                    tList += "Value : " + app.Directors[i].TrustList[i2].Value + " / ";
                                    tList += "Mortgage : " + app.Directors[i].TrustList[i2].Mortgage + " }";
                                }

                                var fList = "";
                                for (int i3 = 0; i3 < app.Directors[i].FinancialList.Count; i3++)
                                {
                                    fList += "{ FinancialType : " + app.Directors[i].FinancialList[i3].FinancialType + " / ";
                                    fList += "Particular : " + app.Directors[i].FinancialList[i3].Particular + " / ";
                                    fList += "Asset : " + app.Directors[i].FinancialList[i3].Asset + " / ";                                    
                                    fList += "Liability : "+ app.Directors[i].FinancialList[i3].Liability + " }";                                    
                                }

                                stParam.Add(tList);
                                stParam.Add(fList);                                
                                stParam.Add((app.Directors[i].OtherAsset) !=null ? app.Directors[i].OtherAsset : 0 );
                                stParam.Add((app.Directors[i].OtherLiability) != null ? app.Directors[i].OtherLiability : 0);
                                stParam.Add(app.RandomId);

                                System.Data.DataSet ds = vStpCaller.executeSPDataSet(stParam, Core.Letters.stpCaller.stpType.AddCffCustomerApplicationDirectors);
                            }

                            //for (int i = 0; i < fAttachments.Count; i++)   // retrieve attached file(s) and save 
                            //{

                            //    HttpPostedFile fnPosted =  fAttachments.Get(i);
                            //    finalName = rootPath + app.Id + "_" + Path.GetFileName(fnPosted.FileName);
                            //    fAttachments[i].SaveAs(finalName);
                            //}

                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                var mail2 = System.Configuration.ConfigurationManager.AppSettings["cffEmailRecipientWhenError"];
                                var smtp2 = new SmtpClient();
                                var credential2 = (NetworkCredential)smtp2.Credentials;
                                var smtpSect2 = (System.Net.Configuration.SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                                MailMessage mailMsg2 = new MailMessage();
                                mailMsg2.SubjectEncoding = Encoding.UTF8;
                                mailMsg2.Subject = "re: CFF Borrower Online App error message";
                                mailMsg2.From = new MailAddress(smtpSect2.From);
                                mailMsg2.To.Add(new MailAddress(mail2));
                                mailMsg2.Body = "Exception Message : "+ex.StackTrace + " [ JSON Data : "+finalJson+" ] ";

                                smtp2.Credentials = credential2;
                                smtp2.Host = smtpSect.Network.Host;
                                smtp2.UseDefaultCredentials = false;
                                smtp2.Send(mailMsg2);

                                Response.Write("ERROR!");
                            }
                            catch (Exception exM)
                            {
                                Console.WriteLine("Email send exception and JSON data : {0}, {1}", exM.InnerException, finalJson);
                                Response.Write("ERROR!");
                            }
                        }
                    }

                    ////Response.AddHeader("Access-Control-Allow-Origin", "http://cashflowfunding.co.nz"); //for cross-domain posting
                    //Response.AddHeader("Content-Type", "application/jsonp");
                    //Response.AddHeader("Access-Control-Allow-Origin", "*"); // for cross-domain posting
                    //Response.AddHeader("Access-Control-Allow-Methods", "POST");
                    //Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                    //Response.End();
                }

                //for (int i = 0; i < fAttachments.Count; i++)   // retrieve attached file(s) and save 
                //{
                //    HttpPostedFile fnPosted = fAttachments.Get(i);
                //    finalName = rootPath + appId + "_" + Path.GetFileName(fnPosted.FileName);
                //    fAttachments[i].SaveAs(finalName);
                //}

                ////Response.AddHeader("Access-Control-Allow-Origin", "http://cashflowfunding.co.nz"); //for cross-domain posting
                Response.AddHeader("Content-Type", "application/jsonp");
                Response.AddHeader("Access-Control-Allow-Origin", "*"); // for cross-domain posting
                Response.AddHeader("Access-Control-Allow-Methods", "POST");
                Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                Response.End();

            }
            else if (strComID == "cffdg")
            {
                //request from cashflowfunding run stored proc
                Core.Letters.stpCaller xStpCaller = new Core.Letters.stpCaller();
                string strParams = Request.Params.ToString();

                //start cleanup
                int idx = strParams.IndexOf("ASP.NET");
                if (idx > 0)
                    strParams = strParams.Substring(0, idx - 1);
                else
                {
                    idx = strParams.IndexOf("ALL_HTTP");
                    if (idx > 0)
                        strParams = strParams.Substring(0, idx - 1);
                }
                //end cleanup

                if (!string.IsNullOrEmpty(strParams))
                {
                    string[] sPar = strParams.Split('&');
                    string sAction = "";
                    if (sPar.Length > 1)
                    {
                        List<object> objParam = new List<object>();
                        sAction = sPar[1].Split('=')[1];
                        objParam.Add(sAction);
                        strParams = sPar[2];
                        for (int ictr = 3; ictr < sPar.Length; ictr++)
                        {
                            strParams += "|" + sPar[ictr];
                        }
                        objParam.Add(strParams);
                        System.Data.DataSet theDS = xStpCaller.executeSPDataSet(objParam, Core.Letters.stpCaller.stpType.CashFlowFundingRequest);
                    }
                }

                Response.Clear();
                Response.AddHeader("Access-Control-Allow-Origin", "http://cashflowfunding.co.nz"); //for cross domain posting
                Response.Write("OK!");   // disabled before : dbb
            }
            else if (strComID == "cffdgInt")
            {   //request from cashflowfunding to retrieve interest values
                string strParams = Request.Params.ToString();

                //start cleanup
                int idx = strParams.IndexOf("ASP.NET");
                if (idx > 0)
                    strParams = strParams.Substring(0, idx - 1);
                else
                {
                    idx = strParams.IndexOf("ALL_HTTP");
                    if (idx > 0)
                        strParams = strParams.Substring(0, idx - 1);
                }
                //end cleanup

                bool bXml = true;
                string[] sPar = strParams.Split('&');
                string DataPar = "";
                if (sPar.Length > 1)
                {
                    bXml = false;
                    DataPar = sPar[1];
                    sPar = DataPar.Split('=');
                    if (sPar[0] == "DataType")
                        if (sPar.Length > 1)
                        {
                            DataPar = sPar[1];
                        }
                }

                if (bXml)
                {
                    Response.ClearHeaders();
                    Response.AddHeader("Content-type", "text/xml");
                    Response.AddHeader("Access-Control-Allow-Origin", "http://cashflowfunding.co.nz"); //for cross-domain posting

                    Response.Clear();
                    string xmlRes = RetrieveMorgageInterestRates("");
                    Response.Write(xmlRes);
                    Response.End();
                }
                else if (DataPar == "jsonp")
                {
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Type", "application/jsonp");
                    Response.AddHeader("Access-Control-Allow-Origin", "*"); //for cross-domain posting
                    Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
                    Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                    Response.Clear();
                    string jsonpRes = RetrieveMorgageInterestRates("jsonp");
                    Response.Write(jsonpRes);
                    Response.End();
                }
            }
            else
            {
                switch (ParamRefQueryString)
                {
                    case "forgotPwd":
                        ResponseMessage.Visible = false;
                        LogOnControl.Visible = false;
                        PasswordRecovery1.Visible = true;
                        CreateUserWizard1.Visible = false;
                        break;

                    case "newAccnt":
                        ResponseMessage.Visible = false;
                        LogOnControl.Visible = false;
                        PasswordRecovery1.Visible = false;
                        CreateUserWizard1.Visible = true;
                        break;

                    case "activate":
                        LogOnControl.Visible = false;
                        PasswordRecovery1.Visible = false;
                        CreateUserWizard1.Visible = false;
                        record = presenter.ActivateUser(ParamActivate1String, ParamActivate2String);
                        switch (record.Status)
                        {
                            case 1: // just recently verified
                                sTitle = Cff_WebResource.titleVerificationSuccessful;
                                sMsg = Cff_WebResource.msgEmailApprovalSent;
                                SendMailToManager(record, ParamActivate1String);
                                PostInfoPage(sTitle, sMsg);
                                break;

                            case 2: // already approved
                                ResponseMessage.Visible = false;
                                LogOnControl.Visible = true;
                                break;

                            case -1: // error verification
                                LogOnControl.Visible = false;
                                sTitle = Cff_WebResource.titleVerificationError;
                                sMsg = Cff_WebResource.msgVerificationInvalid;
                                PostInfoPage(sTitle, sMsg);
                                break;

                            case -2: // have been verified
                                LogOnControl.Visible = false;
                                sTitle = Cff_WebResource.titleVerificationError;
                                sMsg = Cff_WebResource.msgVerificationDup;
                                PostInfoPage(sTitle, sMsg);
                                break;

                            default: // no need for approval (user email is admin email)
                                LogOnControl.Visible = false;
                                sTitle = Cff_WebResource.titleVerificationSuccessful;
                                sMsg = Cff_WebResource.msgEmailApprovalNotNeeded;
                                PostInfoPage(sTitle, sMsg);
                                break;
                        }
                        break;

                    case "approval":
                        LogOnControl.Visible = false;
                        PasswordRecovery1.Visible = false;
                        CreateUserWizard1.Visible = false;
                        record = presenter.UserRequestApproval(ParamApproval1String, ParamActivate1String, ParamApproval2String);
                        if (record.Status != -1)
                        {
                            if (ParamApproval2String == "0")
                            {
                                SendDeclineMailTo(record);
                                String member = Membership.GetUser(new Guid(ParamActivate1String)).UserName;
                                Membership.DeleteUser(member);
                            }
                            else
                            {
                                SendApproveMailTo(record);
                            }
                            DoAfterApprovalPage();
                        }
                        else
                        {
                            PostInfoPage("Activation Error", "The account may no longer exist in our system.");
                        }

                        break;

                    default:
                        ResponseMessage.Visible = false;
                        PasswordRecovery1.Visible = false;
                        CreateUserWizard1.Visible = false;
                        LogOnControl.Visible = true;
                        if (ParamActivate1String != "" && ParamActivate1String != null)
                        {
                            LogOnControl.UserName = ParamLoginUserString;
                            if (ParamLoginUserString == null || ParamLoginUserString == "")
                                SetFocus2LogOnUserName(); //SetFocus("UserName");
                            else
                                SetFocus2LogOnUserPassword(); //SetFocus("Password"); //this focus to password
                        }
                        else
                        {
                            SetFocus2LogOnUserName(); //SetFocus("UserName");
                        }
                        break;
                }

                InitializeCurrentPathForJavaScript();
            }

            //}
        }

        private void SetFocus2LogOnUserName()
        {
            LogOnControl.Controls[0].Controls[1].Focus();
        }

        private void SetFocus2LogOnUserPassword()
        {
            LogOnControl.Controls[0].Controls[5].Focus();
        }


        private void PostInfoPage(String sTitle, String sMessage)
        {
            try
            {
                ResponseMessageTitle.InnerHtml = sTitle;
                ResponseMessageBody.InnerHtml = sMessage;
                ResponseMessage.Visible = true;
            }
            catch { }
        }

        private void DoAfterApprovalPage()
        {
            ResponseMessageTitle.InnerHtml = "Thank you!";
            ResponseMessageBody.InnerHtml = "Thank you for taking time on our security process. As you may aware that due to the sensitivity of your information to us, as valued customer we keep your record as secured as possible.<br/><br/>" +
            "If you have any comments or suggestion please let us know.<br/><br/><br/>";
            ResponseMessage.Visible = true;
        }

        protected void LogOnControl_LoggedOn(object sender, EventArgs e)
        {
            String strHdr = "";
            String strURL = Request.Url.AbsoluteUri; //String ip = "[" + Request.UserHostAddress + "] [" + Request.ServerVariables["HTTP_X_FORWARDED_FOR"] + "]"; //Request.ServerVariables["REMOTE_ADDR"];
            try
            {
                String strSecureCon = Request.IsSecureConnection.ToString();
                String[] strAHdr = Request.ServerVariables.AllKeys;
                for (int ix = 0; ix < strHdr.Length; ix++)
                {
                    strHdr += strAHdr[ix] + ";";
                }

                System.Diagnostics.EventLog objEventLog = new System.Diagnostics.EventLog();
                if (objEventLog != null) {
                    if (!(System.Diagnostics.EventLog.SourceExists("DebtorManagement")))
                    {
                        System.Diagnostics.EventLog.CreateEventSource("DebtorManagement", "DM");
                    }
                    objEventLog.Source = "DebtorManagement";
                    string text = " REMOTE_ADDR:" + Request.ServerVariables["REMOTE_ADDR"];
                    text += " HTTP_X_FORWARDED_FOR:" + Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    objEventLog.WriteEntry("strSecureCon: " + strSecureCon + " URIORIGSTRING: " + Request.Url.OriginalString + text, System.Diagnostics.EventLogEntryType.Information);
                }
            }
            catch
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            //if internal ip & length = 1 = pass else 
            //LogOnControl.RememberMeText = "IP: " + ip;
            if (LogOnControl.UserName.Trim().Length <= 3)
            {
                if (Request.IsSecureConnection == false)
                {
                    MembershipUser member = Membership.GetUser(LogOnControl.UserName);
                    if (member != null) {
                        Guid userId = (Guid)member.ProviderUserKey;
                        if (this.presenter != null)
                        {
                          this.presenter.LogOnUser(userId, Request.QueryString["ReturnUrl"], LogOnControl.RememberMeSet);
                        }
                    }
                }
                else {
                    FormsAuthentication.SignOut();
                    Roles.DeleteCookie();
                    Session.Clear();
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
            else {
                try {
                    MembershipUser member = Membership.GetUser(LogOnControl.UserName);
                    Guid userId = (Guid)member.ProviderUserKey;
                    presenter.LogOnUser(userId, Request.QueryString["ReturnUrl"], LogOnControl.RememberMeSet);
                } catch (Exception exc) {
                    string error = exc.Message;
                    if (exc.Message.ToLower().Contains("nullreference"))
                        FormsAuthentication.RedirectToLoginPage();
                }
            }
        }

        protected void LogOnControl_OnLogOnError(object sender, EventArgs e)
        {
            //check first if user and password is valid

            int IsSpecialAccount = presenter.VerifyIfSpecialAccount(LogOnControl.UserName, LogOnControl.Password);
            if (IsSpecialAccount == -1) // not a special account
            {
                MembershipUser user = Membership.GetUser(LogOnControl.UserName);
                // Applied security (suppress the real error by providing more generic msg.)
                if (user != null)
                {
                    String errorMsg = "";
                    if (user.IsLockedOut)
                        errorMsg = Cff_WebResource.LogOnFailLockedOut;
                    else if (user.IsApproved == false)
                        errorMsg = Cff_WebResource.LogOnFailApprovalNeeded;
                    else
                    {
                        //check if cff staff, check if password matches 
                        errorMsg = Cff_WebResource.LogOnFailIncorrectPassword;
                        SetFocus2LogOnUserPassword(); //SetFocus("Password");
                    }
                    LogOnControl.FailureText = errorMsg;
                }
                else
                {
                    LogOnControl.FailureText = Cff_WebResource.LogOnFail;
                    SetFocus2LogOnUserName(); //SetFocus("UserName");
                }
            }
            else if (IsSpecialAccount == 0) //lockedout
            {
                LogOnControl.FailureText = Cff_WebResource.LogOnFailLockedOut;
                SetFocus2LogOnUserName(); //SetFocus("UserName");
            }
            else
            {
                int lockoutCounter = 0;
                List<UserSpecialAccounts> accounts = presenter.GetAccountAccess(LogOnControl.UserName, LogOnControl.Password);
             
                foreach (var account in accounts)
                {
                    if (account.IsLockedOut) { lockoutCounter++; }
                }

                if (lockoutCounter == accounts.Count)
                {
                    IsSpecialAccount = 0;
                    accounts = null;
                    LogOnControl.FailureText = Cff_WebResource.LogOnFailLockedOut;
                    SetFocus2LogOnUserPassword();  //SetFocus("Password");
                }
                else 
                {
                    Button newButton = null;
                    wrapper.Visible = false;
                    mywrapper.Visible = true;
                    foreach (var account in accounts)
                    {
                        string viewID = Request.QueryString["ViewID"];
                        if (string.IsNullOrEmpty(viewID))
                            viewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);

                        newButton = new Button();
                        newButton.Attributes.Add("Class", "GButton");
                        newButton.ID = account.ID.ToString();
                        newButton.Text = account.Name;
                        newButton.UseSubmitBehavior = false;
                        newButton.PostBackUrl = "AccountToAccess.aspx?User=" + LogOnControl.UserName + "&Id=" + newButton.ID + "&ViewID=" + viewID + "&RememberMe=false";
                        if (account.IsClient == true)
                        {
                            if (account.IsLockedOut)
                            {
                                newButton.Enabled = false;
                            }
                            clientPlaceholder.Controls.Add(newButton);
                            clientTab.Visible = true;
                        }
                        else
                        {
                            if (account.IsLockedOut)
                            {
                                newButton.Enabled = false;
                            }
                            customerPlaceholder.Controls.Add(newButton);
                            custTab.Visible = true;
                        }
                    }
                }
            }
        }

        protected void LogOnUserLookupError(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "MessageScript", "alert(\"LogOnUserLookupError.\");", true);
        }
        
        protected void LogOnAnswerLookupError(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "MessageScript", "alert(\"LogOnAnswerLookupError.\");", true);
        }
        
        protected void LogOnSendingMailError(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "MessageScript", "alert(\"LogOnSendingMailError.\");", true);
        }

        protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            CreateUserWizard cuw = (CreateUserWizard)sender;
            cuw.Email = cuw.UserName;
            if (((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("Password")).Text !=
            ((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ConfirmPassword")).Text)
            {
                ((Label)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ErrorLabel")).Text = Cff_WebResource.PasswordMismatched;
                ((Label)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ErrorLabel")).Visible = true;
                e.Cancel = true;
            }
            String PassKey = ((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("PassKey")).Text;
            if (presenter.VerifyPasskey(PassKey) == false)
            {
                // This passkey exist and valid
                ((Label)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ErrorLabel")).Text = Cff_WebResource.CreateUserFailPasskeyError;
                ((Label)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ErrorLabel")).Visible = true;
                e.Cancel = true;
            }
        }

        protected void CreateuserWizard1_CreatedUser(object sender, EventArgs e)
        {
            MembershipUser membershipUser = Membership.GetUser(CreateUserWizard1.UserName);
            membershipUser.IsApproved = false; // the new generated user needs an approval
            Membership.UpdateUser(membershipUser);
            string membershipUserProviderUserKeyToString = membershipUser.ProviderUserKey.ToString();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("FullName", ((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("Name")).Text);
            collection.Add("Password", CreateUserWizard1.Password);
            collection.Add("Email", CreateUserWizard1.Email);
            collection.Add("UserID", membershipUserProviderUserKeyToString);
            collection.Add("Signature", ((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("Name")).Text); // just use the name
            // personal information
            String sRole = presenter.GetRoleByPassKey(((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("PassKey")).Text);
            int UserType = 1;
            if (sRole == "Client - Staff") UserType = 6;
            else if (sRole == "Customer") UserType = 3;
            else if (sRole == "Management") UserType = 5;
            else if (sRole == "Administrator") UserType = 4;
            collection.Add("UserTypeId", UserType.ToString());

            presenter.CreateNewUser(collection, membershipUserProviderUserKeyToString);
            
            Roles.AddUserToRole(CreateUserWizard1.UserName, sRole);
            SendMailVerificationTo(membershipUser, ((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("PassKey")).Text);
        }

        /* Activation email to sent */
        private void SendMailVerificationTo(MembershipUser member, String pKey)
        {
            Guid userId = (Guid)member.ProviderUserKey;
            SmtpClient mailClient = new SmtpClient();
            MailMessage EmailMsg = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["adminEMail"].ToString(), member.Email);
            EmailMsg.Subject = "Email Verification from Cashflow Funding Limited";
            EmailMsg.IsBodyHtml = true;
            EmailMsg.Body = "Hi <i>" + member.UserName + " ,</i><br/><br/>Thank you for registering in Commercia Factors & Finance - Client Portal website.<br/>As part of our commitment on securing our client information, we would like you first to verify your email address by clicking \"Verify Now\" link below. <br/><br/>Your verification link : <a href=" + UrlAddress + "/LogOn.aspx?ComID=activate&UKey="
                + userId.ToString() + "&PKey=" + pKey + ">Verify Now</a>.<br/><br/>If you have any problem on the link we provided, please copy the URL address below and open it from your browser.<br/><br/>Url: "+ UrlAddress +"/LogOn.aspx?ComID=activate&UKey="
                + userId.ToString() + "&PKey=" + pKey + "<br/><br/>After you verified your email account we will then send an email to your company registrant for approval.<br/><br/>Thank you for your patience.<br/><b>Cashflow Funding Limited</b>";
            try
            {
                mailClient.Send(EmailMsg);  // error here
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());
            }
        }

        private void SendMailToManager(CffUserActivation record, String uKey)
        {
            SmtpClient mailClient = new SmtpClient();
            MailMessage EmailMsg = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["adminEMail"].ToString(), record.MngtEmail);
            EmailMsg.Subject = "User request for CFF Web Access approval";
            EmailMsg.IsBodyHtml = true;
            EmailMsg.Body = "Hi Management,<br/><br/>A new user \"" + record.Name + "\" is requesting your approval to access your records in CFF Client Portal.<br/><br/>To approve the access please click the link <a href= " + UrlAddress + "/LogOn.aspx?ComID=approval&MKey=" + record.MngtUKey + "&UKey=" + uKey + "&action=1>Approve</a>. otherwise click <a href=" + UrlAddress + "/LogOn.aspx?ComID=approval&MKey=" + record.MngtUKey + "&UKey=" + uKey + "&action=0>Decline</a> to decline the users request.<br/><br/>Please be aware that CFF will not hold liable on any damages in result of your approval to this client.<br/><br/>Best Regards,<b>Cashflow Funding Limited</b>";
            try
            {
                mailClient.Send(EmailMsg);  // error here
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());
            }
        }

        private void SendDeclineMailTo(CffUserActivation record)
        {
            SmtpClient mailClient = new SmtpClient();
            MailMessage EmailMsg = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["adminEMail"].ToString(), record.UserEmail);
            EmailMsg.Subject = "Access Request Decline";
            EmailMsg.IsBodyHtml = true;
            EmailMsg.Body = "Hi " + record.Name + ",<br/><br/>We are sorry to inform you that the Client/Customer Administrator decline your request to access their CFF account.<br/><br/>Thank you for your time.<br/><br/><b>Cashflow Funding Limited</b>";
            try
            {
                mailClient.Send(EmailMsg);  // error here
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());
            }
        }

        private void SendApproveMailTo(CffUserActivation record)
        {
            SmtpClient mailClient = new SmtpClient();
            MailMessage EmailMsg = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["adminEMail"].ToString(), record.UserEmail);
            EmailMsg.Subject = "Access Request Approve";
            EmailMsg.IsBodyHtml = true;
            EmailMsg.Body = "Hi " + record.Name + ",<br/><br/>Congratulation!<br/><br/>The administrator approved your request to access their CFF account. You may now start logging-in to our client portal.<br/><br/>To login please click the link <a href='" + UrlAddress + "/LogOn.aspx?User=" + record.UserEmail + "' target='_blank'>Cashflow Funding Limited - Client Portal</a><br/><br/>If you have problem in the link provided please open this address " + UrlAddress + "/LogOn.aspx manually to your browser.<br/><br/><br/>Best Regards,<br/><b>Cashflow Funding Limited</b>";
            try
            {
                mailClient.Send(EmailMsg);  // error here
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());
            }
        }

        protected void LogOnSendingMail(object sender, MailMessageEventArgs e)
        {
            e.Message.IsBodyHtml = false;
            e.Message.Subject = "Cashflow Funding Limited - New Generated Password.";
        }

        protected void OnAgreementClick(object sender, EventArgs e)
        {
            RedirectToAgreement();
        }

        public void Redirect(string targetUrl)
        {
            Response.Redirect(targetUrl);
        }

        public void RedirectToAgreement()
        {
            Response.Redirect("AgreementPage.aspx");
        }

        private string RetrieveMorgageInterestRates(string datatype)
        {
            try
            {
                //String xmlResponse = "<MortgageRates><lendingInstitutions DateCreated=\"2014-03-18T14:42:34.310\" Author=\"JDJL Limited\" Bank=\"ANZ\" MortgageRate=\"5.74\" BaseRate=\"9.65\" OverdraftFees=\"1.44\" /></MortgageRates>";  // disabled by dbb 31/12/2014
                String xmlResponse = "<MortgageRates><lendingInstitutions ErrorLog=\"Try block\" /></MortgageRates>";
                Core.Letters.stpCaller xStpCaller = new Core.Letters.stpCaller();

                List<object> objParam = new List<object>();
                objParam.Add("GETALLXML");

                //objParam.Add(DateTime.Now);     // disabled by dbb 31/12/2014
                //objParam.Add("");               // disabled by dbb 31/12/2014
                System.Data.DataSet theDS = xStpCaller.executeSPDataSet(objParam, Cff.SaferTrader.Core.Letters.stpCaller.stpType.CashFlowFundingInterestReq);
                if (theDS != null)
                    if (theDS.Tables != null)
                        if (theDS.Tables.Count > 0)
                            if (theDS.Tables[0].Rows.Count > 0)
                                xmlResponse = theDS.Tables[0].Rows[0]["XMLRESULT"].ToString();

                if (string.IsNullOrEmpty(datatype))
                    return xmlResponse;
                else //return json string for now
                    return "{XmlResult:\"" + xmlResponse.Replace("\"", "\\\"") + "\"}";
            }
            catch (Exception ex)
            {
                String exMsg = ex.ToString();

                string logPath = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
                if (!logPath.EndsWith(@"\"))
                    logPath += "\\";
                if (Directory.Exists(logPath))
                {
                    logPath += "RetrieveMorgageInterestRates.txt";
                    File.AppendAllText(logPath, "Logged: " + DateTime.Now.ToString("yyyy-MMM-dd hh:mm tt"));
                    File.AppendAllText(logPath, "Logged: " + DateTime.Now.ToString(exMsg));
                }

                return "<MortgageRates><lendingInstitutions ErrorLog=\"Catch block\" /></MortgageRates>";
                //return "<MortgageRates><lendingInstitutions DateCreated=\"2014-03-18T14:42:34.310\" Author=\"JDJL Limited\" Bank=\"ANZ\" MortgageRate=\"5.74\" BaseRate=\"9.65\" OverdraftFees=\"1.44\" /></MortgageRates>";   // disabled by dbb 31/12/2014

            }
        }    

        
      
    }
}