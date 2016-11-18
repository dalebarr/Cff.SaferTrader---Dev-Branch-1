using System;
using System.Linq;
using System.Windows;
using System.Collections;
using System.Collections.Generic;

using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;
using NPOI.HSSF.Record.Formula.Functions;

namespace Cff.SaferTrader.Web
{
    public partial class Notes : BasePage, INotesView
    {
        public string targetName = "";
        private NotesPresenter presenter;
        private string eventTarget = "";
        public bool custCheckBox;


        public CffGenGridView cffGGV_CustomerNotes;
        public CffGenGridView cffGGV_AllCustomerNotes;

        protected CffGenGridView cffGGV_ClientNotes;
        protected CffGenGridView cffGGV_ClientPermanentNotes;
        protected CffGenGridView cffGGV_CustomerPermanentNotes;

        
        private void AddJavaScriptIncludeInHeader(string path)
        {  /// Adds a JavaScript reference to the header of the document.
            try
            {
                var script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = ResolveUrl(path);
                Page.Header.Controls.Add(script);
            }
            catch { 
            }
        }

        private void AddHeaderLinkCss(string path)
        {
            try
            {
                var hdrlink = new HtmlGenericControl("link");
                hdrlink.Attributes["type"] = "text/css";
                hdrlink.Attributes["rel"] = "stylesheet";
                hdrlink.Attributes["href"] = ResolveUrl(path);
                Page.Header.Controls.Add(hdrlink);
            }
            catch { 
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery-ui.css");
            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery.ui.core.css");
            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery.ui.all.css");
            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery.ui.button.css");
            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery.ui.dialog.css");
            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery.ui.menu.css");
            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery.ui.theme.css");
            AddHeaderLinkCss("js/ui.1.10.4/themes/jquery.ui.datepicker.css");
            AddHeaderLinkCss("css/jquery.autocomplete.css");
            AddHeaderLinkCss("css/ui-datepicker-cff.css");
          
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/jquery-1.10.2.js");
            AddJavaScriptIncludeInHeader("js/jquery-migrate-1.0.0.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery-ui.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.core.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.button.js");

            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.widget.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.menu.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.slider.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.dialog.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.accordion.js");

            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.datepicker.js");
        }

        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);
            
            //customernotes gridview
            initializeCustomerNotes();

            //clientnotes gridview
            initializeClientNotes();

            //AllCustomerNotes
            initializeAllCustomerNotesPlaceHolder();

            //clientPermanentNotesGridView
            initializeClientPermanentNotesPlaceHolder();

            //CustomerPermanentNotes
            initializeCustomerPermanentNotesPlaceHolder();

            DateTime dtStart = DateTime.Now;
            dtStart = dtStart.AddDays(-61); // less than 30 days from the Date.Now (dbb)
            //string dtStamp = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + (DateTime.Now.Year).ToString(); //dbb
            string dtStamp = dtStart.Day.ToString().PadLeft(2, '0') + "/" + dtStart.Month.ToString().PadLeft(2, '0') + "/" + dtStart.Year.ToString();
            string dtStampNow = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();

            TbxCustomerNotesFilterFrom.Text = dtStamp;
            TbxCustomerNotesFilterTo.Text = dtStampNow;

            dtCliCustNotesFrom.Text = dtStamp;
            dtCliCustNotesTo.Text = dtStampNow;

            dtPermanentNotesFilterFrom.Text = dtStamp;
            dtPermanentNotesFilterTo.Text = dtStampNow;

            dtClientNotesFilterFrom.Text = dtStamp;
            dtClientNotesFilterTo.Text = dtStampNow;

        }

        private void InitializeCurrentPathForJavaScript()
        {
            string relativePathToRoot = RelativePathComputer.ComputeRelativePathToRoot(Server.MapPath("~"),
                                                                                       Server.MapPath("."));
            string script = string.Format(@"var relativePathToRoot='{0}';", relativePathToRoot);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "initializeCurrentPath", script, true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                initializeActivityTypes();

            InitializeCurrentPathForJavaScript();

            assignCliCustNotesCallBacks();
            assignPermanentNotesCallBacks();

            if (SessionWrapper.Instance.Get == null)
                ValidateSessionInstance();

            if (SessionWrapper.Instance.Get != null)
                SessionWrapper.Instance.Get.IsHidePermanentNotesGridView = true;

            CustomerNotesAdder.SaveSuccessful += CustomerNotesAdderSaved;
            CustomerNotesAdder.Cancel += CustomerNotesAdderCancelled;
            CustomerNotesAdder.NextCallDueUpdated += CustomerNotesAdderNextCallDueUpdated;
            EditCustomerNotesModalBox.NoteSaved += EditCustomerNotesModalBoxNoteSaved;

            if (SessionWrapper.Instance.Get != null)
            {
                ISecurityManager securityManager = SecurityManagerFactory.Create(CurrentPrincipal, SessionWrapper.Instance.Get.Scope);
                presenter = new NotesPresenter(this, RepositoryFactory.CreateCustomerNotesRepository(), securityManager);
                presenter.LockDown();

                //start related ref:CFF-18
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                {
                    targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
                    //AspLabelWhichNotes.Text = "Permanent and Client Notes";  //dbb
                }

                if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
                {
                    if (targetName != null || !targetName.Equals(""))
                    {
                        targetName += " / ";
                        targetName = string.Concat(targetName, SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
                    }
                    else
                    {
                        targetName = ": " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name;
                    }
                    //AspLabelWhichNotes.Text = "Permanent and Customer Notes";   //dbb
                    CliCustNotesUpdatePanel.Visible = false;
                }
            }
            //end

            custCheckBox = CustomerNotesAdder.PermanentCheckBoxEnable();

            if (IsPostBack)
            {
                string cArgs = "";
                Control c = GetPostBackControl(this.Page, out cArgs);
                if (c != null)
                {
                    eventTarget = c.ID;
                }

                ToggleCustomerNotes(false);
                ToggleAllCustomerNotes(false);
                ToggleCustomerPermanentNotes(false);
                ToggleClientPermanentNotes(false);

                Scope cScope = this.CurrentScope();
                if (cScope == Scope.CustomerScope)
                {
                    ViewState.Add("ActivityType", CbxCustomerActivityTypeDropDownList.SelectedValue);
                    ViewState.Add("NoteType", CbxCustomerNoteTypeDropDownList.SelectedValue);

                    ShowPermanentNotes(ViewState["PermanentNotes"] as IList<PermanentCustomerNote>);
                    ShowCustomerNotes(ViewState["CustomerNotes"] as IList<CustomerNote>);
                    ShowClientNotes(ViewState["ClientNotes"] as IList<ClientNote>);
                }
                else if (cScope == Scope.AllClientsScope)
                {
                    ShowAllClientsPermanentNotes(ViewState["AllClientPermanentNotes"] as IList<AllClientsPermanentNote>);
                    ShowAllCustomerNotesForClientOnRange(ViewState["CliCustNotes"] as IList<CustomerNote>);
                }
                else
                {
                    ShowAClientsPermanentNotes(ViewState["ClientPermanentNotes"] as IList<PermanentClientNote>);
                    ShowAllCustomerNotesForClientOnRange(ViewState["CliCustNotes"] as IList<CustomerNote>);
                    ShowClientNotes(ViewState["ClientNotes"] as IList<ClientNote>);
                }

            }
            else
            {
                Scope cScope = this.CurrentScope();
                LoadNotes();
            }
        }


        public static Control GetPostBackControl(Page page, out string controlArgument)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            controlArgument = page.Request.Params.Get("__EVENTARGUMENT");

            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }

            }
            return control;
        }


        private void ValidateSessionInstance()
        {
            int clientID = 0;
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;
            if (QueryString.ClientId != null)
                clientID = Convert.ToInt32(QueryString.ClientId.ToString());
            else
                clientID = 0;

            if (clientID <= 0)
            {
                //QueryString is missing/deleted 
                clientID = Convert.ToInt32((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.ClientId.ToString());
            }

            if (SessionWrapper.Instance.IsSessionValid)
            {
                if ((cffPrincipal.CffUser.UserType == UserType.EmployeeStaffUser)
                         || (cffPrincipal.CffUser.UserType == UserType.EmployeeManagementUser)
                               || (cffPrincipal.CffUser.UserType == UserType.EmployeeAdministratorUser))
                {
                    //generate a new viewID key and save this as new session 
                    string viewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(clientID);
                    SessionWrapper.Instance.GetSession(viewID).UserIdentity = 1;
                    SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = true;
                    SessionWrapper.Instance.GetSession(viewID).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();

                    string rUrl = ResolveClientUrl(Context.Request.Url.AbsolutePath + "?Client=" + clientID.ToString() + "&User=" + cffPrincipal.CffUser.EmployeeId + "&ViewID=" + viewID);
                    if (QueryString.CustomerId != null)
                    {
                        CffCustomer cCustomer = RepositoryFactory.CreateCustomerRepository().GetMatchedCustomerInfo((Convert.ToInt32(QueryString.CustomerId)), clientID).CffCustomerInformation.Customer;
                        SessionWrapper.Instance.GetSession(viewID).CustomerFromQueryString = (ICffCustomer)(new CffCustomerExt(cCustomer.Name, cCustomer.Id, cCustomer.Number));
                        rUrl += "&Customer=" + QueryString.CustomerId.ToString();
                    }

                    if (QueryString.Criteria != null)
                    {
                        rUrl += "&Criteria=" + QueryString.CriteriaValue.ToString().Replace("#", "");
                    }

                    this.Response.Redirect(rUrl);
                }
                else
                {
                    Boolean bRetainClientID = false;
                    Boolean bIsMultipleAccounts = false;

                    if (cffPrincipal.CffUser.UserType == UserType.ClientManagementUser ||
                              cffPrincipal.CffUser.UserType == UserType.ClientStaffUser ||
                                  cffPrincipal.CffUser.UserType == UserType.CustomerUser)
                    { //validate clientid (client/user may manually input this id)
                        //if user is of multiple accounts, validate if this client id is valid for this user
                        List<UserSpecialAccounts> accounts = new List<UserSpecialAccounts>();
                        accounts = RepositoryFactory.CreateUserClientsRepository().GetSpecialAccountAccessByID((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.EmployeeId);
                        if (accounts != null)
                        {
                            bIsMultipleAccounts = true;
                            foreach (UserSpecialAccounts xAcct in accounts)
                                if (clientID == Convert.ToInt32(xAcct.cId.ToString()))
                                {
                                    bRetainClientID = true;
                                    break;
                                }
                        }

                        if ((!bRetainClientID) && (clientID != Convert.ToInt32((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.ClientId.ToString())))
                            clientID = Convert.ToInt32((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.ClientId.ToString());
                    }

                    //check if there exists previous session instance
                    string viewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(clientID);
                    SessionWrapper.Instance.GetSession(viewID).UserIdentity = 1;
                    SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = true;
                    SessionWrapper.Instance.GetSession(viewID).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
                    SessionWrapper.Instance.GetSession(viewID).IsMultipleAccounts = bIsMultipleAccounts;

                    string rUrl = ResolveClientUrl(Context.Request.Url.AbsolutePath + "?Client=" + clientID.ToString() + "&User=" + cffPrincipal.CffUser.EmployeeId + "&ViewID=" + viewID);
                    if (QueryString.CustomerId != null)
                    {
                        SessionWrapper.Instance.GetSession(viewID).CustomerFromQueryString =
                            (ICffCustomer)RepositoryFactory.CreateCustomerRepository().GetMatchedCustomerInfo((int)QueryString.CustomerId, clientID).CffCustomerInformation.Customer;
                        rUrl += "&Customer=" + QueryString.CustomerId.ToString();
                    }

                    if (QueryString.Criteria != null)
                    {
                        rUrl += "&Criteria=" + QueryString.CriteriaValue.ToString().Replace("#", "");
                    }

                  this.Response.Redirect(rUrl);
                }
            }
            else
            { //redirect to logon page - allow only one window tab instance to open, unless it came from the reports tab etc
                string rUrl = ResolveClientUrl("~/Logon.aspx");
                this.Response.Redirect(rUrl);
            }
        }

      
        protected void initializeClientPermanentNotesPlaceHolder()
        {

            cffGGV_ClientPermanentNotes= new CffGenGridView();
            cffGGV_ClientPermanentNotes.ID = "ClientPermanentNotesGridView";
            cffGGV_ClientPermanentNotes.PageSize = 250;
            
            //start Form Edit Settings
            //cffGGV_ClientPermanentNotes.CssClass = "CffGGV";
            cffGGV_ClientPermanentNotes.HeaderStyle.CssClass = "cffGGVHeader";

            //cffGGV_ClientPermanentNotes.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_ClientPermanentNotes.SelectedRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_ClientPermanentNotes.BorderWidth = Unit.Pixel(1);
            cffGGV_ClientPermanentNotes.Width = Unit.Percentage(100);
            cffGGV_ClientPermanentNotes.HorizontalAlign = HorizontalAlign.Center;

            cffGGV_ClientPermanentNotes.AllowSorting = true;
            cffGGV_ClientPermanentNotes.AutoGenerateColumns = false;
            cffGGV_ClientPermanentNotes.EnableViewState = true;

            //end Form Edit Settings
            cffGGV_ClientPermanentNotes.AllowGroupBy = true;

            Scope scope = Scope.AllClientsScope;
            if (SessionWrapper.Instance.Get != null)
                scope = SessionWrapper.Instance.Get.Scope;

            if (scope == Scope.AllClientsScope)
            {
                cffGGV_ClientPermanentNotes.SetSortExpression = "ClientName";
                cffGGV_ClientPermanentNotes.GroupBySettings.GroupByExpression = "ClientName";
                //cffGGV_ClientPermanentNotes.EditingMode = CffGridViewEditingMode.GroupByEditingForm;
                cffGGV_ClientPermanentNotes.EditingMode = CffGridViewEditingMode.InLine;
                //cffGGV_ClientPermanentNotes.EditColumnSettings.ColumnsPerRow = 2;

                cffGGV_ClientPermanentNotes.Columns.Clear();
                cffGGV_ClientPermanentNotes.Caption = "All Clients Permanent Notes";
                cffGGV_ClientPermanentNotes.InsertBoundHyperLinkColumn("ClientName", "ClientName", "ClientId", "30%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Center);
                cffGGV_ClientPermanentNotes.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientPermanentNotes.InsertDataColumn("Cr. By", "createdByEmployeeName", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientPermanentNotes.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientPermanentNotes.InsertDataColumn("Mod. By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientPermanentNotes.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "50%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Center);
                cffGGV_ClientPermanentNotes.InsertCommandButtonColumn(" ", CffGridViewButtonType.Edit, "cffGGV_CommandButtons", System.Web.UI.WebControls.HorizontalAlign.Center);
            }
            else {
                cffGGV_ClientPermanentNotes.SetSortDirection = SortDirection.Descending;
                cffGGV_ClientPermanentNotes.SetSortExpression = "Created";
                cffGGV_ClientPermanentNotes.GroupBySettings.GroupByExpression = "Created";
                //cffGGV_ClientPermanentNotes.EditingMode = CffGridViewEditingMode.GroupByEditingForm;
                cffGGV_ClientPermanentNotes.EditingMode = CffGridViewEditingMode.InLine;

                cffGGV_ClientPermanentNotes.Columns.Clear();
                cffGGV_ClientPermanentNotes.Caption = "Client Permanent Notes";
                cffGGV_ClientPermanentNotes.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientPermanentNotes.InsertDataColumn("Cr. By", "createdByEmployeeName", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientPermanentNotes.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientPermanentNotes.InsertDataColumn("Mod. By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientPermanentNotes.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "50%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Center);
                cffGGV_ClientPermanentNotes.InsertCommandButtonColumn(" ", CffGridViewButtonType.Edit, "cffGGV_CommandButtons", System.Web.UI.WebControls.HorizontalAlign.Center);

                cffGGV_ClientPermanentNotes.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
                cffGGV_ClientPermanentNotes.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
                cffGGV_ClientPermanentNotes.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            }

            clientPermanentNotesPlaceholder.Controls.Clear();
            clientPermanentNotesPlaceholder.Controls.Add(cffGGV_ClientPermanentNotes);
            clientPermanentNotesPlaceholder.Visible = true;
         
        }

        protected void initializeCustomerPermanentNotesPlaceHolder()
        {
            cffGGV_CustomerPermanentNotes = new CffGenGridView();
            cffGGV_CustomerPermanentNotes.ID = "CustomerPermanentNotesGridView";
            cffGGV_CustomerPermanentNotes.Caption = "Customer Permanent Notes";
            cffGGV_CustomerPermanentNotes.PageSize = 250;
            cffGGV_CustomerPermanentNotes.DefaultPageSize = 250;
            //cffGGV_CustomerPermanentNotes.KeyFieldName = "NoteId";
            
            cffGGV_CustomerPermanentNotes.AutoGenerateColumns = false;
            cffGGV_CustomerPermanentNotes.AllowSorting = true;
            cffGGV_CustomerPermanentNotes.EnableViewState = true;
            cffGGV_CustomerPermanentNotes.AllowGroupBy = true;
            cffGGV_CustomerPermanentNotes.SetSortDirection = SortDirection.Descending;
            cffGGV_CustomerPermanentNotes.GroupBySettings.GroupByExpression = "CreatedSortingDate";  //"Created";
            cffGGV_CustomerPermanentNotes.SetSortExpression = "CreatedSortingDate";    // "Created";
            cffGGV_CustomerPermanentNotes.CaptionHeaderSettings.BoldCaption = true;
            cffGGV_CustomerPermanentNotes.CaptionHeaderSettings.HorizontalAlignment = HorizontalAlign.Center;

            //start Form Edit Settings
            //cffGGV_CustomerPermanentNotes.CssClass = "CffGGV";
            cffGGV_CustomerPermanentNotes.HeaderStyle.CssClass = "cffGGVHeader";

            cffGGV_CustomerPermanentNotes.BorderWidth = Unit.Pixel(1);
            cffGGV_CustomerPermanentNotes.Width  = Unit.Percentage(100);
            cffGGV_CustomerPermanentNotes.HorizontalAlign = HorizontalAlign.Center;
            cffGGV_CustomerPermanentNotes.SelectedRowStyle.BackColor = System.Drawing.Color.Honeydew;
            //cffGGV_CustomerPermanentNotes.EditColumnSettings.ColumnsPerRow = 3;
            //cffGGV_CustomerPermanentNotes.EditingMode = CffGridViewEditingMode.GroupByEditingForm;
            cffGGV_CustomerPermanentNotes.EditingMode = CffGridViewEditingMode.InLine;
            //end Form Edit Settings

            cffGGV_CustomerPermanentNotes.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            cffGGV_CustomerPermanentNotes.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_CustomerPermanentNotes.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_CustomerPermanentNotes.InsertDataColumn("Mod. By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            cffGGV_CustomerPermanentNotes.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "80%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
            cffGGV_CustomerPermanentNotes.InsertCommandButtonColumn(" ", CffGridViewButtonType.Edit, "cffGGV_CommandButtons", System.Web.UI.WebControls.HorizontalAlign.Center);
      

            cffGGV_CustomerPermanentNotes.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
            cffGGV_CustomerPermanentNotes.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            cffGGV_CustomerPermanentNotes.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            customerPermanentNotesPlaceholder.Controls.Clear();
            customerPermanentNotesPlaceholder.Controls.Add(cffGGV_CustomerPermanentNotes);

        }

        protected void initializeClientNotes()
        {
            cffGGV_ClientNotes = new CffGenGridView();
            cffGGV_ClientNotes.PageSize = 250;  //rows per page

            cffGGV_ClientNotes.AllowSorting = true;
            cffGGV_ClientNotes.AutoGenerateColumns = false;
            cffGGV_ClientNotes.EnableViewState = true;

            //start Form Edit Settings
            //cffGGV_ClientNotes.CssClass = "CffGGV";
            cffGGV_ClientNotes.HeaderStyle.CssClass = "cffGGVHeader";

            //cffGGV_ClientNotes.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_ClientNotes.SelectedRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_ClientNotes.BorderWidth = Unit.Pixel(1);
            cffGGV_ClientNotes.Width = Unit.Percentage(100);
            cffGGV_ClientNotes.HorizontalAlign = HorizontalAlign.Center;

            //cffGGV_ClientNotes.EditColumnSettings.ColumnsPerRow = 3;
            //cffGGV_ClientNotes.EditingMode = CffGridViewEditingMode.GroupByEditingForm;
            cffGGV_ClientNotes.CaptionHeaderSettings.BoldCaption = true;
            cffGGV_ClientNotes.EditingMode = CffGridViewEditingMode.InLine;


            Scope scope = Scope.AllClientsScope;
            if (SessionWrapper.Instance.Get != null)
                scope = SessionWrapper.Instance.Get.Scope;

            if (scope == Scope.AllClientsScope)
            {
                cffGGV_ClientNotes.AllowGroupBy = true;
                cffGGV_ClientNotes.SetSortExpression = "ClientName";
                cffGGV_ClientNotes.GroupBySettings.GroupByExpression = "ClientName";
                cffGGV_ClientNotes.SetSortDirection = SortDirection.Ascending;

                //end Form Edit Settings
                cffGGV_ClientNotes.ID = "ClientNotesGridView";
                cffGGV_ClientNotes.Caption = "All Client Notes";
                cffGGV_ClientNotes.InsertBoundHyperLinkColumn("ClientName", "ClientName", "ClientId", "20%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Center);
                cffGGV_ClientNotes.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientNotes.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientNotes.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientNotes.InsertDataColumn("Mod. By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientNotes.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "40%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
                cffGGV_ClientNotes.InsertCommandButtonColumn("", CffGridViewButtonType.Edit, "cffGGV_CommandButtons", System.Web.UI.WebControls.HorizontalAlign.Center);
            }
            else {
                //end Form Edit Settings
                cffGGV_ClientNotes.AllowGroupBy = false;
                cffGGV_ClientNotes.SetSortExpression = "CreatedSortingDate";  // Created
                cffGGV_ClientNotes.SetSortDirection = SortDirection.Ascending;
      
                cffGGV_ClientNotes.ID = "AllClientNotesGridView";
                cffGGV_ClientNotes.Caption = "Client Notes";
                cffGGV_ClientNotes.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientNotes.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientNotes.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientNotes.InsertDataColumn("Mod. By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                //cffGGV_ClientNotes.InsertDataColumn("NoteType", "NoteTypeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
                cffGGV_ClientNotes.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "50%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
                cffGGV_ClientNotes.InsertCommandButtonColumn("", CffGridViewButtonType.Edit, "cffGGV_CommandButtons", System.Web.UI.WebControls.HorizontalAlign.Center);
            }

            cffGGV_ClientNotes.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
            cffGGV_ClientNotes.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;

            CffGGV_ClientNotesPlaceHolder.Controls.Clear();
            CffGGV_ClientNotesPlaceHolder.Controls.Add(cffGGV_ClientNotes);
        }

        protected void initializeAllCustomerNotesPlaceHolder()
        {
            cffGGV_AllCustomerNotes = new CffGenGridView();
            cffGGV_AllCustomerNotes.ID = "AllCustomerNotes";
            cffGGV_AllCustomerNotes.AllowSorting = true;
            cffGGV_AllCustomerNotes.AllowGroupBy = true;
            cffGGV_AllCustomerNotes.EnableViewState = true;
            cffGGV_AllCustomerNotes.AutoGenerateColumns = false;
            cffGGV_AllCustomerNotes.SetSortExpression = "CustomerName";
            cffGGV_AllCustomerNotes.SetSortDirection = SortDirection.Ascending;
            cffGGV_AllCustomerNotes.GroupBySettings.GroupByExpression = "CustomerName";

            cffGGV_AllCustomerNotes.HeaderStyle.CssClass = "cffGGVHeader";
            cffGGV_AllCustomerNotes.FooterStyle.CssClass = "dxgvFooter";

            //cffGGV_AllCustomerNotes.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_AllCustomerNotes.BorderWidth = Unit.Pixel(1);
            cffGGV_AllCustomerNotes.SelectedRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_AllCustomerNotes.Width = Unit.Percentage(100);
            cffGGV_AllCustomerNotes.HorizontalAlign = HorizontalAlign.Center;
            cffGGV_AllCustomerNotes.CaptionHeaderSettings.BoldCaption = true;
            cffGGV_AllCustomerNotes.CaptionHeaderSettings.HorizontalAlignment = HorizontalAlign.Center;

            //let's do an inline editing for allcustomers due to the grouped by column
            cffGGV_AllCustomerNotes.EditingMode = CffGridViewEditingMode.InLine;
            
            //todo: we should be able to know if grouped by column @editingmode = displayrow_with_edit_form
            cffGGV_AllCustomerNotes.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "15%", "cffGGV_leftAlignedCell");
            cffGGV_AllCustomerNotes.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            cffGGV_AllCustomerNotes.InsertDataColumn("Cr. By", "createdByEmployeeName", CffGridViewColumnType.Text, "15%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_AllCustomerNotes.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_AllCustomerNotes.InsertDataColumn("Mod. By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_AllCustomerNotes.InsertDataColumn("Activity Type", "ActivityTypeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_AllCustomerNotes.InsertDataColumn("NoteType", "NoteTypeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            cffGGV_AllCustomerNotes.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "45%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
            cffGGV_AllCustomerNotes.InsertCommandButtonColumn(" ", CffGridViewButtonType.Edit, "cffGGV_CommandButtons", System.Web.UI.WebControls.HorizontalAlign.Center);

            cffGGV_AllCustomerNotes.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
            cffGGV_AllCustomerNotes.PageSize = 250;
            cffGGV_AllCustomerNotes.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            cffGGV_AllCustomerNotes.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            CliCustNotesPlaceHolder.Controls.Clear();
            CliCustNotesPlaceHolder.Controls.Add(cffGGV_AllCustomerNotes);

        }

        protected void initializeCustomerNotes()
        {
            cffGGV_CustomerNotes = new CffGenGridView();
            cffGGV_CustomerNotes.ID = "CustomerNotes";
            cffGGV_CustomerNotes.AllowSorting = true;
            cffGGV_CustomerNotes.AllowGroupBy = true;
            cffGGV_CustomerNotes.EnableViewState = true;
            cffGGV_CustomerNotes.AutoGenerateColumns = false;
            cffGGV_CustomerNotes.SetSortExpression = "CreatedSortingDate";
            cffGGV_CustomerNotes.SetSortDirection = SortDirection.Descending;
            cffGGV_CustomerNotes.GroupBySettings.GroupByExpression = "CreatedSortingDate";
      
            //start Form Edit Settings
            //cffGGV_CustomerNotes.CssClass = "CffGGV";
            cffGGV_CustomerNotes.HeaderStyle.CssClass = "cffGGVHeader";
            cffGGV_CustomerNotes.FooterStyle.CssClass = "dxgvFooter";

            //cffGGV_CustomerNotes.AlternatingRowStyle.BackColor = System.Drawing.Color.PapayaWhip;
            //cffGGV_CustomerNotes.BorderWidth = Unit.Pixel(1);
            cffGGV_CustomerNotes.SelectedRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_CustomerNotes.RowCssClass = "dxgvDataRow";
            cffGGV_CustomerNotes.Width = Unit.Percentage(100);
            cffGGV_CustomerNotes.HorizontalAlign = HorizontalAlign.Center;
            cffGGV_CustomerNotes.CaptionHeaderSettings.BoldCaption = true;
            
            //let's do a "GroupBy" editing form for customernotes due to the groupby column
            cffGGV_CustomerNotes.EditingMode = CffGridViewEditingMode.InLine;
            //cffGGV_CustomerNotes.EditingMode = CffGridViewEditingMode.GroupByEditingForm;
            //cffGGV_CustomerNotes.EditColumnSettings.ColumnsPerRow = 3;
            //end Form Edit Settings

            cffGGV_CustomerNotes.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "15%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            cffGGV_CustomerNotes.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_CustomerNotes.InsertDataColumn("Modified", "Modified", CffGridViewColumnType.DateTime, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_CustomerNotes.InsertDataColumn("Mod. By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_CustomerNotes.InsertDataColumn("Activity", "ActivityTypeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            //cffGGV_CustomerNotes.InsertDataColumn("NoteType", "NoteTypeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            cffGGV_CustomerNotes.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "60%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
            cffGGV_CustomerNotes.InsertCommandButtonColumn("", CffGridViewButtonType.Edit, "cffGGV_CommandButtons", System.Web.UI.WebControls.HorizontalAlign.Center);

            cffGGV_CustomerNotes.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
            cffGGV_CustomerNotes.PageSize = 250;
            cffGGV_CustomerNotes.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            cffGGV_CustomerNotes.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            CffGGV_CustomerNotesPlaceHolder.Controls.Clear();
            CffGGV_CustomerNotesPlaceHolder.Controls.Add(cffGGV_CustomerNotes);
        }

        private string GetMonthString(int mo)
        {
            return ((mo == 1) ? "January" : (mo == 2) ? "February"
                    : (mo == 3) ? "March" : (mo == 4) ? "April" 
                    : (mo == 5) ? "May" : (mo == 6) ? "June" 
                    : (mo == 7) ? "July" : (mo == 8) ? "August" 
                    : (mo == 9) ? "September" : (mo == 10) ? "October" 
                    : (mo == 11) ? "November" : "December");
        }

        protected void initializeActivityTypes()
        {
            if (SessionWrapper.Instance.Get != null)
            {
                if (SessionWrapper.Instance.Get.Scope == Scope.CustomerScope)
                {
                    PopulateActivityTypes(ActivityType.KnownTypesForFilteringNotes);
                    PopulateNoteTypes(NoteType.KnownTypesForFilteringNotes);
                }
            }
        }


        protected void PopulateActivityTypes(IList<System.Web.UI.WebControls.ListItem> activityTypes)
        {
            CbxCustomerActivityTypeDropDownList.DataSource = activityTypes;
            CbxCustomerActivityTypeDropDownList.DataTextField = "Text";
            CbxCustomerActivityTypeDropDownList.DataValueField = "Value";
            CbxCustomerActivityTypeDropDownList.DataBind();
        }

        protected void PopulateNoteTypes(IList<System.Web.UI.WebControls.ListItem> noteTypes)
        {
            CbxCustomerNoteTypeDropDownList.DataSource = noteTypes;
            CbxCustomerNoteTypeDropDownList.DataTextField = "Text";
            CbxCustomerNoteTypeDropDownList.DataValueField = "Value";
            CbxCustomerNoteTypeDropDownList.DataBind();
        }


        protected void assignCliCustNotesCallBacks()
        {
            cffGGV_CustomerNotes.PagerCommand += cffGGV_CustomerNotes_PagerCommand;
            cffGGV_CustomerNotes.RowCommand += cffGGV_CustomerNotesRowCommand;
            cffGGV_CustomerNotes.RowUpdating += cffGGV_CustomerNotes_RowUpdating;
            cffGGV_CustomerNotes.RowCancelingEdit+= cffGGV_CustomerNotes_RowCancelingEdit;

            cffGGV_AllCustomerNotes.PagerCommand += cffGGV_AllCustomerNotes_PagerCommand;
            cffGGV_AllCustomerNotes.RowCommand += cffGGV_AllCustomerNotesRowCommand;
            cffGGV_AllCustomerNotes.RowCancelingEdit += cffGGV_AllCustomerNotes_RowCancelingEdit;
            cffGGV_AllCustomerNotes.RowEditing += cffGGV_AllCustomerNotes_RowEditing;
            
            cffGGV_ClientNotes.RowCommand += cffGGV_ClientNotes_RowCommand;
        }

        void cffGGV_AllCustomerNotes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (cffGGV_AllCustomerNotes.Caption.Equals(""))
            {
                cffGGV_AllCustomerNotes.IsInEditMode = false;
            }
        }



        void cffGGV_CustomerNotes_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            cffGGV_CustomerNotes.Caption = "**Editing**";
        }

        protected void assignPermanentNotesCallBacks()
        {
            cffGGV_ClientPermanentNotes.RowCommand += cffGGV_ClientPermanentNotes_RowCommand;
            cffGGV_ClientPermanentNotes.PagerCommand += cffGGV_ClientPermanentNotes_PagerCommand;

            cffGGV_CustomerPermanentNotes.RowCommand += cffGGV_CustomerPermanentNotes_RowCommand;
            cffGGV_CustomerPermanentNotes.PagerCommand += cffGGV_CustomerPermanentNotes_PagerCommand;
        }


        void cffGGV_CustomerPermanentNotes_PagerCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "PagePrev":
                case "PageNext":
                case "PageFirst":
                case "PageLast":
                    cffGGV_CustomerPermanentNotes.PageIndex = ((CffGenGridView)sender).PageIndex;
                    cffGGV_CustomerPermanentNotes.DataSource = ((CffGenGridView)sender).DataSource;
                    break;

                default:
                    break;
            }
        }

        void cffGGV_ClientPermanentNotes_PagerCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "PagePrev":
                case "PageNext":
                case "PageFirst":
                case "PageLast":
                    cffGGV_ClientPermanentNotes.PageIndex = ((CffGenGridView)sender).PageIndex;
                    cffGGV_ClientPermanentNotes.DataSource = ((CffGenGridView)sender).DataSource;
                    break;

                default:
                    break;
            }
        }

        void cffGGV_AllCustomerNotes_PagerCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "PagePrev":
                case "PageNext":
                case "PageFirst":
                case "PageLast":
                    cffGGV_AllCustomerNotes.PageIndex = ((CffGenGridView)sender).PageIndex;
                    cffGGV_AllCustomerNotes.DataSource = ((CffGenGridView)sender).DataSource;
                    break;

                default:
                    break;
            }
        }

        void cffGGV_AllCustomerNotes_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            cffGGV_AllCustomerNotes.EditIndex = -1;
            cffGGV_AllCustomerNotes.IsInEditMode = false;
            cffGGV_AllCustomerNotes.IsInUpdateMode = false;
            cffGGV_AllCustomerNotes.IsInAddMode = false;
        }


        void cffGGV_CustomerNotes_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            cffGGV_CustomerNotes.EditIndex = -1;
            cffGGV_CustomerNotes.IsInEditMode = false;
            cffGGV_CustomerNotes.IsInUpdateMode = false;
            cffGGV_CustomerNotes.IsInAddMode = false;
        }

        void cffGGV_CustomerNotes_PagerCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        { //you may modify this functionalit to manually handle the grid navigation, fetch your new next/prev data or refresh the datasource
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "PagePrev":
                case "PageNext":
                case "PageFirst":
                case "PageLast":
                    cffGGV_CustomerNotes.PageIndex = ((CffGenGridView)sender).PageIndex;
                    cffGGV_CustomerNotes.DataSource = ((CffGenGridView)sender).DataSource;
                    break;

                default:
                    break;
            }
        }

        void cffGGV_ClientNotes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {   
            int rowIndex = 0;
            CffGenGridView dGV = (CffGenGridView)sender;

            Scope xS = Scope.ClientScope;
             if (SessionWrapper.Instance.Get != null)
                    xS = SessionWrapper.Instance.Get.Scope;

             if (e.CommandArgument.ToString().IndexOf("$") >= 0)
             {
                 string[] cargs = e.CommandArgument.ToString().Split('$');
                 rowIndex = Convert.ToInt32(cargs[1]);
             }
             else
                 rowIndex = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit": 
                    {
                      if (!dGV.Caption.Equals("")) break;

                      ClientNote cNote = null;
                      if (cffGGV_ClientNotes.DataSource != null)
                          cNote = (ClientNote)((IList<object>)cffGGV_ClientNotes.DataSource)[rowIndex];

                      else if (dGV.DataSource != null)
                          cNote = (ClientNote)((IList<ClientNote>)dGV.DataSource);
                      
                      else if (cffGGV_ClientNotes.GridBag != null)
                          cNote = (ClientNote)cffGGV_ClientNotes.GridBag;

                      else if ((ViewState["ClientNotes"] as IList<ClientNote>) != null)
                      {
                          cffGGV_ClientNotes.DataSource = (ViewState["ClientNotes"] as IList<ClientNote>);
                          cNote = (ClientNote)((IList<ClientNote>)cffGGV_ClientNotes.DataSource)[rowIndex];
                      }

                      cffGGV_ClientNotes.GridBag = cNote.Clone();

                      if (cNote == null)
                      {
                          cffGGV_ClientNotes.EditIndex = -1;
                          cffGGV_ClientNotes.IsInAddMode = false;
                          cffGGV_ClientNotes.IsInEditMode = false;
                          cffGGV_ClientNotes.IsInUpdateMode = false;
                          cffGGV_ClientNotes.IsCancelingEdit = true;
                          cffGGV_ClientNotes.Caption = "";
                          break;
                      }

                      if (!cNote.AuthorId.ToString().Equals((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.EmployeeId.ToString()))
                      {
                          cffGGV_ClientNotes.Caption = "Unable to Edit. You need to be the Administrator or the author of this note.";
                          cffGGV_ClientNotes.EditIndex = -1;
                          cffGGV_ClientNotes.IsInAddMode = false;
                          cffGGV_ClientNotes.IsInEditMode = false;
                          cffGGV_ClientNotes.IsInUpdateMode = false;
                          cffGGV_ClientNotes.IsCancelingEdit = true;
                      }
                      else 
                      {
                          cffGGV_ClientNotes.Caption = "***Edit Client Notes ";
                          if (xS == Scope.ClientScope)
                              cffGGV_ClientNotes.Caption += "(Client Scope)***";
                          else
                              cffGGV_ClientNotes.Caption += "(All Clients Scope)***";
                      }

                        dGV.Caption = cffGGV_ClientNotes.Caption;

                    } break;

                case "Update":
                    {
                        IList<ClientNote> clientNoteList = ViewState["ClientNote"] as IList<ClientNote>;
                        if (e.CommandSource != null)
                        {
                            if (e.CommandSource.GetType().Name == "GridView")
                            {   //do your data posting here
                                dGV = (CffGenGridView)e.CommandSource;
                            }
                            else if (e.CommandSource.GetType().Name == "ImageButton")
                            {
                                dGV = (CffGenGridView)((((System.Web.UI.Control)(e.CommandSource)).BindingContainer).BindingContainer);
                                //System.Web.UI.WebControls.GridViewRow dRow = (System.Web.UI.WebControls.GridViewRow)(((System.Web.UI.Control)(e.CommandSource)).BindingContainer);
                            }

                            ClientNote xnote = (ClientNote)(((ClientNote)dGV.GridBag).Clone());
                            string strNote = "";
                            string strModBy = "";
                            int iModBy = -1;
                        
                            IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", dGV);
                            foreach (CffGVUpdStruct xV in gVUV)
                            {
                                switch (xV.name.Trim().ToUpper())
                                {
                                    case "COMMENT":
                                        strNote = xV.value;
                                        break;

                                    case "MODIFIEDBY":
                                        iModBy = Convert.ToInt32(xV.value);
                                        break;

                                    case "MODIFIEDBYEMPLOYEENAME":
                                        strModBy = xV.value;
                                        break;

                                    default:
                                        break;
                                }
                            }

                            xnote.Comment = strNote.Replace("\n", " "); //todo: for some reason the next line break in this field causes the grid to throw an exception; we replace next line with ' ' for now
                            xnote.Modified = new Date(DateTime.Now);
                            xnote.ModifiedBy = iModBy;
                            xnote.ModifiedByEmployeeName = strModBy;

                            //update db notes table 
                            strNote = "";
                            presenter.UpdateClientNotes(xnote, ref strNote);
                            if (string.IsNullOrEmpty(strNote))
                            {
                                cffGGV_ClientNotes.Caption = "**Updated**";
                                cffGGV_ClientNotes.GridBag = xnote;
                            }
                            else
                                cffGGV_ClientNotes.Caption = strNote;

                            IList<ClientNote> xNote = this.presenter.GetClientNotesList((int)xnote.ClientId, GetSelectedDateRange(dtClientNotesFilterFrom, dtClientNotesFilterTo));
                            cffGGV_ClientNotes.DataSource = xNote;
                            dGV.Caption = cffGGV_ClientNotes.Caption;

                            if (xS == Scope.ClientScope)
                            {
                                //cffGGV_ClientNotes.SetSortExpression = "Created";
                                //cffGGV_ClientNotes.GroupBySettings.GroupByExpression = "Created";
                            }
                            else
                            {
                                //cffGGV_ClientNotes.SetSortExpression = "ClientName";
                                //cffGGV_ClientNotes.GroupBySettings.GroupByExpression = "ClientName";
                            }
                        }
                    }
                    break;

                case "Select":
                        if (xS == Scope.AllClientsScope)
                        {

                        }
                        else if (xS == Scope.ClientScope)
                        {

                        }
                        break;

                default://Cancel
                        cffGGV_ClientNotes.EditIndex = -1;
                        cffGGV_ClientNotes.IsInAddMode = false;
                        cffGGV_ClientNotes.IsInEditMode = false;
                        cffGGV_ClientNotes.IsInUpdateMode = false;
                        cffGGV_ClientNotes.IsCancelingEdit = true;
                        cffGGV_ClientNotes.Caption = "";
                        LoadNotes();
                        break;
            }

        }

        void cffGGV_CustomerPermanentNotes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = 0;
            CffGenGridView dGV = (CffGenGridView)sender;

            Scope xS = Scope.ClientScope;
            if (SessionWrapper.Instance.Get != null)
                xS = SessionWrapper.Instance.Get.Scope;

            if (e.CommandArgument.ToString().IndexOf("$") >= 0)
            {
                string[] cargs = e.CommandArgument.ToString().Split('$');
                rowIndex = Convert.ToInt32(cargs[1]);
            }
            else
                rowIndex = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit":
                    {
                        //set modified by as current user; set last modified as current date
                        //maybe add this to cff grid so we don't need to fetch from db again
                        PermanentCustomerNote pNote = null;

                        pNote = (PermanentCustomerNote)((IList<object>)cffGGV_CustomerPermanentNotes.DataSource)[rowIndex];
                        object xpNote = pNote.Clone();
                        cffGGV_CustomerPermanentNotes.GridBag = xpNote;

                        //allow edit only if user is owner of the note or is in administrative login
                        bool userIsAuthor = CurrentPrincipal.CffUser.EmployeeId.Equals(pNote.AuthorId);
                        bool noteIsEditable = !(pNote.NoteType.Id.Equals(NoteType.OldNote.Id));

                        if ((userIsAuthor && noteIsEditable) || (CurrentPrincipal.IsInAdministratorRole && noteIsEditable))
                        {
                            //EditCustomerNotesModalBox.Show(cNote);
                            pNote.Modified = new Date(DateTime.Now);
                            pNote.ModifiedBy = ((SaferTrader.Core.CffPrincipal)Context.User).CffUser.EmployeeId;
                            pNote.ModifiedByEmployeeName = ((SaferTrader.Core.CffPrincipal)Context.User).CffUser.DisplayName;
                            cffGGV_CustomerPermanentNotes.Caption = "***Editing Permanent Customer Notes***";
                            cffGGV_CustomerPermanentNotes.DataBind();  // dbb
                        }
                        else
                        {
                            string caption = "";
                            if (pNote.NoteType.Id.Equals(NoteType.OldNote.Id))
                                caption = "EDITING OF OLD NOTES ARE NOT ALLOWED.";
                            else
                                caption = "UNABLE TO EDIT - YOU NEED TO BE ADMIN OR THE AUTHOR OF THIS NOTE.";
                            
                            cffGGV_CustomerPermanentNotes.Caption = caption;    //do a databind to reflect the editable values
                            cffGGV_CustomerPermanentNotes.EditIndex = -1;
                            cffGGV_CustomerPermanentNotes.IsInAddMode = false;
                            cffGGV_CustomerPermanentNotes.IsInEditMode = false;
                            cffGGV_CustomerPermanentNotes.IsInUpdateMode = false;
                        }
                    }
                    break;

                case "Update":
                    {
                        IList<PermanentCustomerNote> permanentNotes = ViewState["PermanentNotes"] as IList<PermanentCustomerNote>;   //ViewState["CustomerPermanentNote"] as IList<PermanentCustomerNote>;  dbb
                        PermanentCustomerNote pCustNote = null;

                        if (e.CommandSource != null)
                        {
                            if (e.CommandSource.GetType().Name == "GridView")
                            {   //do your data posting here
                                dGV = (CffGenGridView)e.CommandSource;
                            }
                            else if (e.CommandSource.GetType().Name == "ImageButton")
                            {
                                dGV = (CffGenGridView)((((System.Web.UI.Control)(e.CommandSource)).BindingContainer).BindingContainer);
                                //System.Web.UI.WebControls.GridViewRow dRow = (System.Web.UI.WebControls.GridViewRow)(((System.Web.UI.Control)(e.CommandSource)).BindingContainer);
                            }

                            if (cffGGV_CustomerPermanentNotes.DataSource == null)
                                cffGGV_CustomerPermanentNotes.DataSource = dGV.GridBag;   //dbb

                            try
                            {
                                if (permanentNotes == null)
                                    permanentNotes = (IList<PermanentCustomerNote>)cffGGV_CustomerPermanentNotes.DataSource;
                            }
                            catch {
                                pCustNote = (PermanentCustomerNote)((IList<object>)cffGGV_CustomerPermanentNotes.DataSource)[rowIndex];
                            
                            }
                            
                            PermanentCustomerNote xnote = (PermanentCustomerNote)(((PermanentCustomerNote)dGV.GridBag).Clone());
                            string strNote = "";
                            string strModBy = "";
                            int iModBy = -1;


                            IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", dGV);
                            foreach (CffGVUpdStruct xV in gVUV)
                            {
                                switch (xV.name.Trim().ToUpper())
                                {
                                    case "COMMENT":
                                        strNote = xV.value;
                                        break;

                                    case "MODIFIEDBY":
                                        iModBy = Convert.ToInt32(xV.value);
                                        break;

                                    case "MODIFIEDBYEMPLOYEENAME":
                                        strModBy = xV.value;
                                        break;

                                    default:
                                        break;
                                }
                            }

                            xnote.Comment = strNote.Replace("\n", " "); //todo: for some reason the next line break in this field causes the grid to throw an exception; we replace next line with ' ' for now
                            xnote.Modified = new Date(DateTime.Now);
                            xnote.ModifiedBy = iModBy;
                            xnote.ModifiedByEmployeeName = strModBy;

                            //update db notes table 
                            strNote = "";
                            presenter.UpdatePermanentCustNote(xnote, ref strNote);
                            if (string.IsNullOrEmpty(strNote))
                            {
                                cffGGV_CustomerPermanentNotes.Caption = "**Updated**";
                                cffGGV_CustomerPermanentNotes.GridBag = xnote;

                                IList<PermanentCustomerNote> xPN = this.presenter.GetPermanentCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Id,
                                        GetSelectedDateRange(dtPermanentNotesFilterFrom, dtPermanentNotesFilterTo));

                                ViewState.Add("PermanentNotes", xPN); //ViewState.Add("PermanentCustomerNote", xPN);
                                cffGGV_CustomerPermanentNotes.DataSource = xPN;
                                dGV.Caption = cffGGV_CustomerPermanentNotes.Caption;

                                //cffGGV_CustomerPermanentNotes.SetSortExpression = "Created";                              //dbb
                                //cffGGV_CustomerPermanentNotes.GroupBySettings.GroupByExpression = "Created";              //dbb
                            }
                            else
                            {
                                cffGGV_CustomerPermanentNotes.Caption = strNote;
                                dGV.Caption = strNote;
                            }
                            
                        }
                    }
                    break;

                case "Select":
                    {

                    }
                    break;

                default://Cancel
                    {
                        cffGGV_CustomerPermanentNotes.EditIndex = -1;
                        cffGGV_CustomerPermanentNotes.IsInAddMode = false;
                        cffGGV_CustomerPermanentNotes.IsInEditMode = false;
                        cffGGV_CustomerPermanentNotes.IsInUpdateMode = false;
                        cffGGV_CustomerPermanentNotes.IsCancelingEdit = true;
                        cffGGV_CustomerPermanentNotes.Caption = "";

                        dGV.DataBind();
                        LoadNotes();
                    }
                    break;
            }

        }

        void cffGGV_ClientPermanentNotes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {   
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            CffGenGridView dGV = (CffGenGridView)sender;

            Scope xS = Scope.ClientScope;
            if (SessionWrapper.Instance.Get != null)
                xS = SessionWrapper.Instance.Get.Scope;

            object xNote = null;
            int authorID = -1;
            string hdrMsg = "";

            switch (e.CommandName)
            {
                case "Edit":
                    {
                        if (xS == Scope.AllClientsScope)
                        {
                            if (cffGGV_ClientPermanentNotes.DataSource == null)
                                break;

                            AllClientsPermanentNote cNote = (AllClientsPermanentNote)(((IList<object>)cffGGV_ClientPermanentNotes.DataSource)[rowIndex]);
                            xNote = cNote.Clone();
                            authorID = cNote.AuthorId;
                            hdrMsg = "All Clients";
                            cffGGV_ClientPermanentNotes.GridBag = xNote;
                        }
                        else 
                        {
                            if (cffGGV_ClientPermanentNotes.DataSource == null)
                                break;

                            PermanentClientNote cNote = (PermanentClientNote)(((IList<object>)cffGGV_ClientPermanentNotes.DataSource)[rowIndex]);
                            xNote = cNote.Clone();
                            authorID = cNote.AuthorId;
                            hdrMsg = "Clients";
                            cffGGV_ClientPermanentNotes.GridBag = xNote;
                        }
                      
                        if (!authorID.ToString().Equals(((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.EmployeeId).ToString()))
                        {
                           cffGGV_ClientPermanentNotes.Caption = "Unable to Edit. You need to be an Administrator or author of this note.";
                           cffGGV_ClientPermanentNotes.EditIndex = -1;
                           cffGGV_ClientPermanentNotes.IsInAddMode = false;
                           cffGGV_ClientPermanentNotes.IsInEditMode = false;
                           cffGGV_ClientPermanentNotes.IsInUpdateMode = false;
                           cffGGV_ClientPermanentNotes.IsCancelingEdit = true;
                         
                            dGV.Caption = "Unable to Edit. You need to be an Administrator or author of this note.";
                            dGV.EditIndex = -1;
                            dGV.IsInAddMode = false;
                            dGV.IsInEditMode = false;
                            dGV.IsInUpdateMode = false;
                            dGV.IsCancelingEdit = true;
                        }
                        else {
                            dGV.Caption = "***Editing " + hdrMsg + " Permanent Notes***";
                            cffGGV_ClientPermanentNotes.DataBind();
                        }
                    }
                    break;

                case "Update":
                    {
                       
                        if (xS == Scope.AllClientsScope)
                        {
                            AllClientsPermanentNote acNote = (AllClientsPermanentNote)(((IList<object>)cffGGV_ClientPermanentNotes.DataSource)[rowIndex]);
                            xNote = acNote.Clone();
                            authorID = acNote.AuthorId;
                            hdrMsg = "All Clients";
                        
                        }
                        else if (xS == Scope.ClientScope)
                        {
                            PermanentClientNote pcNote = (PermanentClientNote)(((IList<object>)cffGGV_ClientPermanentNotes.DataSource)[rowIndex]);
                            xNote = pcNote.Clone();
                            authorID = pcNote.AuthorId;
                            hdrMsg = "All Clients";
                        }

                        string strNote = "";
                        string strModBy = "";
                        int iModBy = -1;


                        IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", dGV);
                        foreach (CffGVUpdStruct xV in gVUV)
                        {
                            switch (xV.name.Trim().ToUpper())
                            {
                                case "COMMENT":
                                    strNote = xV.value;
                                    break;

                                case "MODIFIEDBY":
                                    iModBy = Convert.ToInt32(xV.value);
                                    break;

                                case "MODIFIEDBYEMPLOYEENAME":
                                    strModBy = xV.value;
                                    break;

                                default:
                                    break;
                            }
                        }

                        if (xS == Scope.AllClientsScope)
                        {
                            ((AllClientsPermanentNote)xNote).Comment = strNote.Replace("\n", " "); //todo: for some reason the next line break in this field causes the grid to throw an exception; we replace next line with ';' for now
                            ((AllClientsPermanentNote)xNote).Modified = new Date(DateTime.Now);
                            ((AllClientsPermanentNote)xNote).ModifiedBy = iModBy;
                            ((AllClientsPermanentNote)xNote).ModifiedByEmployeeName = strModBy;

                            //update db notes table 
                            strNote = "";
                            presenter.UpdatePermanentClientNotes(((PermanentClientNote)xNote), ref strNote);
                            if (string.IsNullOrEmpty(strNote))
                            {
                                cffGGV_ClientPermanentNotes.Caption = "**Updated**";
                                cffGGV_ClientPermanentNotes.GridBag = ((PermanentClientNote)xNote);
                            }
                            else
                                cffGGV_ClientPermanentNotes.Caption = strNote;

                            IList<AllClientsPermanentNote> xPN = this.presenter.GetAllClientsPermanentNotes(GetSelectedDateRange(dtPermanentNotesFilterFrom, dtPermanentNotesFilterTo));
                            cffGGV_ClientPermanentNotes.DataSource = xPN;
                            cffGGV_ClientPermanentNotes.SetSortExpression = "ClientName";
                            cffGGV_ClientPermanentNotes.GroupBySettings.GroupByExpression = "ClientName";
                        }

                        else if (xS == Scope.ClientScope)
                        {
                            ((PermanentClientNote)xNote).Comment = strNote.Replace("\n", " "); //todo: for some reason the next line break in this field causes the grid to throw an exception; we replace next line with ';' for now
                            ((PermanentClientNote)xNote).Modified = new Date(DateTime.Now);
                            ((PermanentClientNote)xNote).ModifiedBy = iModBy;
                            ((PermanentClientNote)xNote).ModifiedByEmployeeName = strModBy;

                            //update db notes table 
                            strNote = "";
                            presenter.UpdatePermanentClientNotes(((PermanentClientNote)xNote), ref strNote);
                            if (string.IsNullOrEmpty(strNote))
                            {
                                cffGGV_ClientPermanentNotes.Caption = "**Updated**";
                                cffGGV_ClientPermanentNotes.GridBag = ((PermanentClientNote)xNote);
                            }
                            else
                                cffGGV_ClientPermanentNotes.Caption = strNote;

                            IList<PermanentClientNote> xPN = this.presenter.GetPermanentClientNotesOnRange(SessionWrapper.Instance.Get.ClientFromQueryString.Id, GetSelectedDateRange(dtPermanentNotesFilterFrom, dtPermanentNotesFilterTo));
                            cffGGV_ClientPermanentNotes.DataSource = xPN;
                            cffGGV_ClientPermanentNotes.SetSortExpression = "Created";
                            cffGGV_ClientPermanentNotes.GroupBySettings.GroupByExpression = "Created";
                        }
                    }
                    break;

                case "Select":
                    {
                    
                    }
                    break;

                default://Cancel
                    {
                        cffGGV_ClientPermanentNotes.EditIndex = -1;
                        cffGGV_ClientPermanentNotes.IsInAddMode = false;
                        cffGGV_ClientPermanentNotes.IsInEditMode = false;
                        cffGGV_ClientPermanentNotes.IsInUpdateMode = false;
                        cffGGV_ClientPermanentNotes.IsCancelingEdit = true;
                        cffGGV_ClientPermanentNotes.Caption = "";

                        dGV.DataBind();
                        LoadNotes();
                    }
                    break;
            }

        }

        void cffGGV_AllCustomerNotesRowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument); //dbb
            CffGenGridView dGV = (CffGenGridView)sender;

            switch (e.CommandName)
            {
                case "Edit": //add other edit event handlers as needed
                    //set modified by as current user; set last modified as current date
                    //maybe add this to cff grid so we don't need to fetch from db again
                    if (cffGGV_AllCustomerNotes.Caption.Equals(""))
                    {
                        CustomerNote cNote = null;
                        object xNote = null;

                        cNote = (CustomerNote) ((IList<object>) cffGGV_AllCustomerNotes.DataSource)[rowIndex];
                        xNote = cNote.Clone();
                        cffGGV_AllCustomerNotes.GridBag = xNote;

                        //allow edit only if user is owner of the note or is in administrative login
                        bool userIsAuthor = CurrentPrincipal.CffUser.EmployeeId.Equals(cNote.AuthorId);
                        bool noteIsEditable = !(cNote.NoteType.Id.Equals(NoteType.OldNote.Id) ||
                                                cNote.ActivityType.Id.Equals(ActivityType.OldNote.Id));

                        if ((userIsAuthor && noteIsEditable) ||
                            (CurrentPrincipal.IsInAdministratorRole && noteIsEditable))
                        {
                            //EditCustomerNotesModalBox.Show(cNote);
                            cNote.Modified = new Date(DateTime.Now);
                            cNote.ModifiedBy = ((SaferTrader.Core.CffPrincipal) Context.User).CffUser.EmployeeId;
                            cNote.ModifiedByEmployeeName = ((SaferTrader.Core.CffPrincipal) Context.User).CffUser.DisplayName;
                            cffGGV_AllCustomerNotes.Caption = "***Editing All Customer Notes***";
                            //cffGGV_AllCustomerNotes.DataBind(); //do a databind to reflect the editable values  //dbb
                        }
                        else
                        {
                            string caption = "";
                            if (cNote.NoteType.Id.Equals(NoteType.OldNote.Id) ||
                                cNote.ActivityType.Id.Equals(ActivityType.OldNote.Id))
                                caption = "EDITING OF OLD NOTES ARE NOT ALLOWED.";
                            else
                                caption = "UNABLE TO EDIT - YOU NEED TO BE ADMIN OR THE AUTHOR OF THIS NOTE.";

                            cffGGV_AllCustomerNotes.Caption = caption; //do a databind to reflect the editable values
                            cffGGV_AllCustomerNotes.EditIndex = -1;
                            cffGGV_AllCustomerNotes.IsInAddMode = false;
                            cffGGV_AllCustomerNotes.IsInEditMode = false;
                            cffGGV_AllCustomerNotes.IsInUpdateMode = false;
                            cffGGV_AllCustomerNotes.IsCancelingEdit = true;
                        }
                    }
                    break;

                case "Update":

                    IList<CustomerNote> custNoteList = ViewState["CustomerNote"] as IList<CustomerNote>;

                    if (e.CommandSource != null)
                    {
                         if (e.CommandSource.GetType().Name == "GridView")
                         {   //do your data posting here
                            dGV = (CffGenGridView)e.CommandSource;
                         }
                         else if (e.CommandSource.GetType().Name == "ImageButton")
                         {
                            dGV = (CffGenGridView)((((System.Web.UI.Control)(e.CommandSource)).BindingContainer).BindingContainer);
                            //System.Web.UI.WebControls.GridViewRow dRow = (System.Web.UI.WebControls.GridViewRow)(((System.Web.UI.Control)(e.CommandSource)).BindingContainer);
                         }

                         if (cffGGV_AllCustomerNotes.DataSource == null)
                            cffGGV_AllCustomerNotes.DataSource = dGV.GridBag;
                     
                        if (custNoteList == null)
                            custNoteList = (IList<CustomerNote>)cffGGV_AllCustomerNotes.DataSource;

                        CustomerNote xnote = (CustomerNote)((CustomerNote)dGV.GridBag).Clone();
                        string strNote = "";
                        string strModBy = "";
                        int iModBy = -1;

                        IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", dGV);
                        foreach (CffGVUpdStruct xV in gVUV)
                        {
                            switch (xV.name.Trim().ToUpper())
                            {
                                case "COMMENT":
                                    strNote = xV.value;
                                    break;

                                case "MODIFIEDBY":
                                    iModBy = Convert.ToInt32(xV.value);
                                    break;

                                case "MODIFIEDBYEMPLOYEENAME":
                                    strModBy = xV.value;
                                    break;

                                default:
                                    break;
                            }
                        }

                        xnote.Comment = strNote;
                        xnote.Modified = new Date(DateTime.Now);
                        xnote.ModifiedBy = iModBy;
                        xnote.ModifiedByEmployeeName = strModBy;

                        //update db notes table 
                        strNote = "";  
                        presenter.UpdateCustomerNotes(xnote, ref strNote);
                        if (string.IsNullOrEmpty(strNote))
                        {
                            cffGGV_AllCustomerNotes.Caption = "**Updated**";
                            cffGGV_AllCustomerNotes.GridBag = xnote;
                            //IList<CustomerNote> xcN = presenter.GetCustomerNotes((int)xnote.CustomerId, GetSelectedDateRange(dtCliCustNotesFrom, dtCliCustNotesTo)); // dbb
                            IList<CustomerNote> xcN = presenter.GetAllCustomerNotes((int) xnote.ClientId, 
                                GetSelectedDateRange(dtCliCustNotesFrom, dtCliCustNotesTo));

                            //ViewState.Add("AllCustomerNote", xcN); //dbb
                            ViewState.Add("CliCustNotes", xcN);
                            cffGGV_AllCustomerNotes.DataSource = xcN;
                            dGV.Caption = cffGGV_AllCustomerNotes.Caption;

                        }
                        else
                        {
                            cffGGV_AllCustomerNotes.Caption = strNote;
                            dGV.Caption = strNote;
                        }
                    }
                    break;

                case "Select":
                    //minimize/maximize
                    //call databind()
                    break;

                default: //cancel
                    cffGGV_AllCustomerNotes.EditIndex = -1;
                    cffGGV_AllCustomerNotes.IsInAddMode = false;
                    cffGGV_AllCustomerNotes.IsInEditMode = false;
                    cffGGV_AllCustomerNotes.IsInUpdateMode = false;
                    cffGGV_AllCustomerNotes.IsCancelingEdit = true;
                    cffGGV_AllCustomerNotes.Caption = "";
                    LoadNotes();
                    break;
            }
        }

        void cffGGV_CustomerNotesRowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {  //handle additional row command events here
            int rowIndex = 0;
            CffGenGridView dGVc = (CffGenGridView)sender;

            if (e.CommandArgument.ToString().IndexOf("$") >= 0)
            {
                string[] cargs = e.CommandArgument.ToString().Split('$');
                rowIndex = Convert.ToInt32(cargs[1]);
            }
            else
            {
                rowIndex = Convert.ToInt32(e.CommandArgument);
            }

            switch (e.CommandName)
            {
                case "Edit": //add other edit event handlers as needed
                    //set modified by as current user; set last modified as current date
                    //maybe add this to cff grid so we don't need to fetch from db again
                     CustomerNote cNote = null;
                     object xNote  = null;

                     cNote = (CustomerNote)((IList<object>)cffGGV_CustomerNotes.DataSource)[rowIndex];
                     xNote = cNote.Clone();
                     cffGGV_CustomerNotes.GridBag = xNote;
                     //cffGGV_CustomerNotes.Caption = "";

                     //allow edit only if user is owner of the note or is in administrative login
                     bool userIsAuthor = CurrentPrincipal.CffUser.EmployeeId.Equals(cNote.AuthorId);
                     bool noteIsEditable = !(cNote.NoteType.Id.Equals(NoteType.OldNote.Id) ||
                                                cNote.ActivityType.Id.Equals(ActivityType.OldNote.Id));

                     if ((userIsAuthor && noteIsEditable) || (CurrentPrincipal.IsInAdministratorRole && noteIsEditable))
                     {
                          //EditCustomerNotesModalBox.Show(cNote);
                         cNote.Modified = new Date(DateTime.Now);
                         cNote.ModifiedBy = ((SaferTrader.Core.CffPrincipal)Context.User).CffUser.EmployeeId;
                         cNote.ModifiedByEmployeeName = ((SaferTrader.Core.CffPrincipal)Context.User).CffUser.DisplayName;
                         cffGGV_CustomerNotes.Caption = "***Editing Customer Notes***";
                         cffGGV_CustomerNotes.DataBind(); // to reflect the editable values  // dbb
                     }
                     else
                     {
                         string caption = "";
                         if (cNote.NoteType.Id.Equals(NoteType.OldNote.Id) || cNote.ActivityType.Id.Equals(ActivityType.OldNote.Id))
                             caption = "EDITING OF OLD NOTES ARE NOT ALLOWED.";
                         else 
                             caption = "UNABLE TO EDIT - YOU NEED TO BE ADMIN OR THE AUTHOR OF THIS NOTE.";

                        cffGGV_CustomerNotes.Caption = caption;    //do a databind to reflect the editable values
                        cffGGV_CustomerNotes.IsInAddMode = false;
                        cffGGV_CustomerNotes.IsInEditMode = false;
                        cffGGV_CustomerNotes.IsInUpdateMode = false;
                        cffGGV_CustomerNotes.IsCancelingEdit = true;
                        cffGGV_CustomerNotes.EditIndex = -1;

                     }
                     break;

                case "Update":

                     IList <CustomerNote> custNoteList = ViewState["CustomerNote"] as IList<CustomerNote>;
                     
                     if (e.CommandSource != null) {
                         //do your data posting here
                        if (e.CommandSource.GetType().Name == "GridView")
                        {   //do your data posting here
                            dGVc = (CffGenGridView)e.CommandSource;
                        }
                        else if (e.CommandSource.GetType().Name == "ImageButton")
                        {
                            dGVc = (CffGenGridView)((((System.Web.UI.Control)(e.CommandSource)).BindingContainer).BindingContainer);
                            //System.Web.UI.WebControls.GridViewRow dRow = (System.Web.UI.WebControls.GridViewRow)(((System.Web.UI.Control)(e.CommandSource)).BindingContainer);
                         }

                         if (cffGGV_CustomerNotes.DataSource == null)
                             cffGGV_CustomerNotes.DataSource = dGVc.GridBag;  //dbb
                       
                         if (custNoteList == null)
                             custNoteList = (IList<CustomerNote>)cffGGV_CustomerNotes.DataSource; //dbb

                         CustomerNote xnote = (CustomerNote)((CustomerNote)dGVc.GridBag).Clone();
                         string strNote = "";
                         string strModBy = "";
                         int iModBy = -1;

                         IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", dGVc);
                         foreach (CffGVUpdStruct xV in gVUV)
                         {
                             switch (xV.name.Trim().ToUpper())
                             {
                                 case "COMMENT":
                                     strNote = xV.value;
                                     break;

                                 case "MODIFIEDBY":
                                     iModBy = Convert.ToInt32(xV.value);
                                     break;
                                 
                                 case "MODIFIEDBYEMPLOYEENAME":
                                     strModBy = xV.value;
                                     break;

                                default:
                                     break;
                             }
                         }

                         xnote.Comment = strNote;
                         xnote.Modified = new Date(DateTime.Now);
                         xnote.ModifiedBy = iModBy;
                         xnote.ModifiedByEmployeeName = strModBy;

                         /*CustomerNote nNote = new CustomerNote(xnote.NoteId, new Date(DateTime.Now), xnote.ActivityType, 
                                                    xnote.NoteType, strNote, xnote.AuthorId, xnote.CreatedByEmployeeName, 
                                                        iModBy, strModBy, new Date(DateTime.Now));*/

                         //update db notes table 
                         strNote = "";
                         presenter.UpdateCustomerNotes(xnote, ref strNote);
                         if (string.IsNullOrEmpty(strNote))
                         {
                             cffGGV_CustomerNotes.Caption = "**Updated**";
                             cffGGV_CustomerNotes.GridBag = xnote;
                             //IList<CustomerNote> xcN = presenter.GetCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Id, 
                             //                               GetSelectedDateRange(TbxCustomerNotesFilterFrom, TbxCustomerNotesFilterTo));

                             IList<CustomerNote> xcN = presenter.GetCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Id, GetSelectedDateRange(TbxCustomerNotesFilterFrom, TbxCustomerNotesFilterTo),
                                                    ActivityType.Parse(int.Parse(CbxCustomerActivityTypeDropDownList.SelectedValue)),
                                                  NoteType.Parse(int.Parse(CbxCustomerNoteTypeDropDownList.SelectedValue)));

                             ViewState.Add("CustomerNotes", xcN);
                             cffGGV_CustomerNotes.DataSource = xcN;
                         }
                         else
                         {
                             cffGGV_CustomerNotes.Caption = strNote;
                             dGVc.Caption = strNote;
                         }
                     }   
                     
                     break;

                case "Select":
                    //minimize/maximize
                    //call databind()
                    break;

                default: //cancel
                    cffGGV_CustomerNotes.EditIndex = -1;     //dbb
                    cffGGV_CustomerNotes.IsInAddMode = false;
                    cffGGV_CustomerNotes.IsInEditMode = false;
                    cffGGV_CustomerNotes.IsInUpdateMode = false;
                    cffGGV_CustomerNotes.IsCancelingEdit = true;
                    cffGGV_CustomerNotes.Caption = "";
                    LoadNotes();

                    //object xObj = cffGGV_CustomerNotes.GridBag;
                    //((List<CustomerNote>)cffGGV_CustomerNotes.DataSource)[rowIndex] = (CustomerNote)cffGGV_CustomerNotes.GridBag; 
                    //cffGGV_CustomerNotes.DataBind();
                    break;
            }
        }


        private IList<CffGVUpdStruct> RetrieveViewStateValues(string viewstateID, CffGenGridView dGrid)
        { //TODO:: refactor this, as param request may not always have the same starting index, and $ @split may not always be @2
            string gID = dGrid.ID;
            IList<CffGVUpdStruct> xNRPK = new List<CffGVUpdStruct>();

            if (viewstateID == "UpdateValues")
            {
                string ctrlName = "";
                for (int iCtr = 0; iCtr < this.Page.Request.Params.Count; iCtr++)
                {
                    ctrlName = this.Page.Request.Params.GetKey(iCtr);
                    if (ctrlName != null)
                    {
                        if (ctrlName.Contains(gID))
                        {
                            CffGVUpdStruct xPar;
                            string cctrlName = "";
                            try
                            {
                                cctrlName = ctrlName.Substring(ctrlName.IndexOf(gID) + gID.Length).Split('$')[2].Substring(3);
                                for (int colCtr = 0; colCtr < dGrid.Columns.Count; colCtr++)
                                {
                                    if (dGrid.Columns[colCtr].GetType().Name == "CffTemplateField")
                                    {
                                        if (cctrlName == ((dGrid.Columns[colCtr] as CffTemplateField).DataBoundColumnName))
                                        {
                                            xPar.type = this.Page.Request.Params[iCtr].GetType();
                                            xPar.value = this.Page.Request.Params[iCtr];
                                            xPar.name = (dGrid.Columns[colCtr] as CffTemplateField).DataBoundColumnName;
                                            xNRPK.Add(xPar);
                                            break;
                                        }
                                    }
                                }
                            }
                            catch {
                            }
                            
                           
                        }
                    }
                }
            }
            return xNRPK;
        }

        
 #region ICustomerNotesView Members
        public void ShowCustomerNotes(IList<CustomerNote> customerNotes)
        {
            ViewState.Add("CustomerNotes", customerNotes);
            cffGGV_CustomerNotes.DataSource = customerNotes;

            if (!cffGGV_CustomerNotes.IsInEditMode)
            {
                cffGGV_CustomerNotes.Caption = "";
                cffGGV_CustomerNotes.DataBind();
                cffGGV_CustomerNotes.Sort("CreatedSortingDate", SortDirection.Descending); //do not forget to call this - ref. mariper
                //cffGGV_CustomerNotes.EditIndex = -1;
            }

            ToggleCustomerNotes(true);
            cffGGV_CustomerNotes.Visible = true;

        }

        public void ShowPermanentNotes(IList<PermanentCustomerNote> permanentNotes)
        {
            ViewState.Add("PermanentNotes", permanentNotes);
            cffGGV_CustomerPermanentNotes.DataSource = permanentNotes;

            cffGGV_CustomerPermanentNotes.SetSortDirection = SortDirection.Descending;
            cffGGV_CustomerPermanentNotes.SetSortExpression = "CreatedSortingDate";   //"Created";
            ToggleCustomerPermanentNotes(true);

            if (!cffGGV_CustomerPermanentNotes.IsInEditMode)
            {
                cffGGV_CustomerPermanentNotes.Caption = "";
                cffGGV_CustomerPermanentNotes.DataBind();
                //cffGGV_CustomerPermanentNotes.Sort("Created", System.Web.UI.WebControls.SortDirection.Descending);
                cffGGV_CustomerPermanentNotes.Sort("CreatedSortingDate", System.Web.UI.WebControls.SortDirection.Descending);
                cffGGV_CustomerPermanentNotes.Visible = true;
            }
        }

        public void ShowAllClientsPermanentNotes(IList<AllClientsPermanentNote> allclientsPermanentNotes)
        {
            ViewState.Add("AllClientPermanentNotes", allclientsPermanentNotes);
            cffGGV_ClientPermanentNotes.DataSource = allclientsPermanentNotes;

            if (!cffGGV_ClientPermanentNotes.IsInEditMode)
            {
                cffGGV_ClientPermanentNotes.Caption = "";
                cffGGV_ClientPermanentNotes.DataBind();
                cffGGV_ClientPermanentNotes.Sort("Created", System.Web.UI.WebControls.SortDirection.Ascending);
                ToggleClientPermanentNotes(true);
                cffGGV_ClientPermanentNotes.Visible = true;
            }

        }

        public void ShowAClientsPermanentNotes(IList<PermanentClientNote> clientPermanentNotes)
        {
            ViewState.Add("ClientPermanentNotes", clientPermanentNotes);
            cffGGV_ClientPermanentNotes.DataSource = clientPermanentNotes;

            cffGGV_ClientPermanentNotes.SetSortDirection = SortDirection.Descending;
            cffGGV_ClientPermanentNotes.SetSortExpression = "Created";

            if (!cffGGV_ClientPermanentNotes.IsInEditMode)
            {
                cffGGV_ClientPermanentNotes.Caption = "";
                cffGGV_ClientPermanentNotes.DataBind();
                cffGGV_ClientPermanentNotes.Sort("Created", System.Web.UI.WebControls.SortDirection.Descending);
                ToggleClientPermanentNotes(true);
                cffGGV_ClientPermanentNotes.Visible = true;

            }

        }

        public void ShowClientNotes(IList<ClientNote> clientNotes)
        {
            ViewState.Add("ClientNotes", clientNotes);
            cffGGV_ClientNotes.DataSource = clientNotes;

            if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope) //dbb 
            {
                cffGGV_ClientNotes.SetSortExpression = "ClientName";
                cffGGV_ClientNotes.GroupBySettings.GroupByExpression = "ClientName";
                cffGGV_ClientNotes.SetSortDirection = SortDirection.Ascending;
            }
            else
            {
                cffGGV_ClientNotes.SetSortExpression = "CreatedSortingDate";
                cffGGV_ClientNotes.SetSortDirection = SortDirection.Descending;
            }

            if (!cffGGV_ClientNotes.IsInEditMode)
            {
                cffGGV_ClientNotes.Caption = "";
                cffGGV_ClientNotes.DataBind();

                //if (SessionWrapper.Instance.Get.Scope == Scope.ClientScope)   //dbb
                if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope)   //dbb
                    cffGGV_ClientNotes.Sort("ClientName", System.Web.UI.WebControls.SortDirection.Ascending);
                else
                    cffGGV_ClientNotes.Sort("CreatedSortingDate", System.Web.UI.WebControls.SortDirection.Descending);
            }
            cffGGV_ClientNotes.Visible = true;
            CffGGV_ClientNotesPlaceHolder.Visible = true;
            ClientNotesDivPanel.Visible = true;
        }

        public void ShowAllCustomerNotesForClientOnRange(IList<CustomerNote> customerNotes)
        {
            try
            {
                //ViewState.Add("CliCustNotes", customerNotes);   // dbb
                //cffGGV_AllCustomerNotes.DataSource = customerNotes;
                IEnumerable<CustomerNote> cList = customerNotes.OrderBy(cn => cn.CustomerName);
                IList<CustomerNote> sortedCList = cList.ToList();

                ViewState.Add("CliCustNotes", sortedCList);   
                cffGGV_AllCustomerNotes.DataSource = sortedCList;

               //TODO:: when in edit mode lock this row so that other data row are not clickable
                if (!cffGGV_AllCustomerNotes.IsInEditMode)
                {
                    cffGGV_AllCustomerNotes.Caption = "";
                    cffGGV_AllCustomerNotes.DataBind();
                    cffGGV_AllCustomerNotes.Sort("CustomerName", SortDirection.Ascending);
                    cffGGV_AllCustomerNotes.Visible = true;
                }
                ToggleAllCustomerNotes(true);

            }
            catch (Exception exc)
            {
                string error = exc.Message;
            }
        }



        public void ToggleAllCustomerNotes(bool visible)
        {
            CliCustNotesUpdatePanel.Visible = visible;
            CliCustNotesPlaceHolder.Visible = visible;
        }


        public void ToggleCustomerPermanentNotes(bool visible)
        {
            customerPermanentNotesPlaceholder.Visible = visible;
        }

        public void ToggleClientPermanentNotes(bool visible)
        {
            ClientPermanentNotesPanel.Visible = visible;
            SessionWrapper.Instance.Get.IsHidePermanentNotesGridView = visible;
        }

        public void ToggleCustomerNotesAdder(bool visible)
        {
            CustomerNotesAdder.Visible = visible;
        }

        public void ToggleCustomerNotes(bool visible)
        {
            CustomerNotesUpdatePanel.Visible = visible;
            CffGGV_CustomerNotesPlaceHolder.Visible = visible;
        }

#endregion

#region Customer Notes
        private void EditCustomerNotesModalBoxNoteSaved(object sender, EventArgs e)
        {
            LoadNotes();
        }

     
        private void CustomerNotesAdderNextCallDueUpdated(object sender, EventArgs e)
        {
            Master.RefreshCustomerInformation(SessionWrapper.Instance.Get.IsClientSelected);
        }

        public void CustomerNotesFilterClicked(object sender, ImageClickEventArgs e)
        {
            LoadNotes();
            cffGGV_AllCustomerNotes.ResetPaginationAndFocus();
            cffGGV_AllCustomerNotes.Caption = "";

            cffGGV_CustomerNotes.ResetPaginationAndFocus();
            cffGGV_CustomerNotes.Caption = "";
            cffGGV_CustomerNotes.EditIndex = -1;
            cffGGV_CustomerNotes.IsInAddMode = false;
            cffGGV_CustomerNotes.IsInEditMode = false;
            cffGGV_CustomerNotes.IsInUpdateMode = false;
            cffGGV_CustomerNotes.IsCancelingEdit = true;
        }

        protected void CustomerNotesAdderSaved(object sender, EventArgs e)
        {
           LoadNotes();
           cffGGV_CustomerNotes.SetEditRow(0);
           cffGGV_CustomerNotes.Caption = "Notes Added";
           cffGGV_CustomerNotes.CaptionHeaderSettings.BoldCaption = true;
           cffGGV_CustomerNotes.EditIndex = -1;
           cffGGV_CustomerNotes.IsInAddMode = false;
           cffGGV_CustomerNotes.IsInEditMode = false;
           cffGGV_CustomerNotes.IsInUpdateMode = false;
           cffGGV_CustomerNotes.IsCancelingEdit = true;

        }

        private void CustomerNotesAdderCancelled(object sender, EventArgs e)
        {
            LoadNotes();

            ShowAllCustomerNotesForClientOnRange(ViewState["CliCustNotes"] as IList<CustomerNote>);
            ShowCustomerNotes(ViewState["CustomerNotes"] as IList<CustomerNote>);

            cffGGV_AllCustomerNotes.ResetPaginationAndFocus();
            cffGGV_AllCustomerNotes.Caption = "";

            cffGGV_CustomerNotes.ResetPaginationAndFocus();
            cffGGV_CustomerNotes.Caption = "";

        }

        protected void CustomerNotesExportButtonClick(object sender, ImageClickEventArgs e)
        {
            if (SessionWrapper.Instance.Get.Scope == Scope.CustomerScope)
                cffGGV_CustomerNotes.ExportAsNote("Customer Notes for " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
            else
                cffGGV_AllCustomerNotes.ExportAsNote("All Customer Notes for " + SessionWrapper.Instance.Get.ClientFromQueryString.Name);
        }
        #endregion


#region  Permanent Customer Notes
        protected void PermanentNotesExportButtonClick(object sender, ImageClickEventArgs e)
        {
            //permanentNotesGridView.ExportAsNote("Permanent Notes for " + SessionWrapper.Instance.Get..Customer.Name);
            //permanentNotesGridView.ExportAsNote("Permanent Notes for " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
        }

        protected void AddCustomerNoteButtonClick(object sender, EventArgs e)
        {
            //int ix = 0;
        }


 #endregion

 
        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);

            cffGGV_CustomerNotes.ResetPaginationAndFocus();
            cffGGV_AllCustomerNotes.ResetPaginationAndFocus();
            cffGGV_ClientNotes.ResetPaginationAndFocus();
            cffGGV_ClientPermanentNotes.ResetPaginationAndFocus();
            cffGGV_CustomerPermanentNotes.ResetPaginationAndFocus();

            presenter.LockDown();
            LoadNotes();
        }

        private void LoadNotes()
        {
            if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope)
            {
                presenter.LoadAllClientsPermanentNotes();
                presenter.LoadAllCustomerNotesForClientOnRange(-1, GetSelectedDateRange(dtCliCustNotesFrom, dtCliCustNotesTo));

            }
            else if (SessionWrapper.Instance.Get.Scope == Scope.CustomerScope)
            {
               presenter.LoadCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Id,
                                              GetSelectedDateRange(TbxCustomerNotesFilterFrom, TbxCustomerNotesFilterTo),
                                                  ActivityType.Parse(int.Parse(CbxCustomerActivityTypeDropDownList.SelectedValue)),
                                                  NoteType.Parse(int.Parse(CbxCustomerNoteTypeDropDownList.SelectedValue)));
               if (custCheckBox)
                   presenter.LoadPermanentCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Id, GetSelectedDateRange(dtCliCustNotesFrom, dtCliCustNotesTo));



            }
            else
            {
                presenter.LoadClientNotes(SessionWrapper.Instance.Get.ClientFromQueryString.Id, GetSelectedDateRange(dtClientNotesFilterFrom, dtClientNotesFilterTo));
                presenter.LoadAClientsPermanentNotes(SessionWrapper.Instance.Get.ClientFromQueryString.Id, GetSelectedDateRange(dtPermanentNotesFilterFrom, dtPermanentNotesFilterTo));
                //presenter.LoadAllCustomerNotesForClientOnRange(SessionWrapper.Instance.Get.ClientFromQueryString.Id, 
                //                        GetSelectedDateRange(dtClientNotesFilterFrom, dtClientNotesFilterTo));    

                presenter.LoadAllCustomerNotesForClientOnRange(SessionWrapper.Instance.Get.ClientFromQueryString.Id, GetSelectedDateRange(dtCliCustNotesFrom, dtCliCustNotesTo));  //dbb
            }
        }

       

        private CffPrincipal CurrentPrincipal
        {
            get { return Context.User as CffPrincipal; }
        }

#region Control Events
        protected void CustomerNotesPrintButton_Click(object sender, ImageClickEventArgs e)
        {
            //PrintableCustomerNotes printable = new PrintableCustomerNotes(SessionWrapper.Instance.Get..Customer.Name, ViewState["CustomerNotes"] as IList<CustomerNote>);
            PrintableCustomerNotes printable = new PrintableCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Name, ViewState["CustomerNotes"] as IList<CustomerNote>, QueryString.ViewIDValue);
            //string script = PopupHelper.ShowPopup(printable, Server, false);
            string script = PopupHelper.ShowPopupReportType(printable, Server, false);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }

        protected void PermanentNotesPrintButton_Click(object sender, ImageClickEventArgs e)
        {
            //PrintablePermanentCustomerNotes printable = new PrintablePermanentCustomerNotes(SessionWrapper.Instance.Get..Customer.Name, ViewState["PermanentNotes"] as IList<PermanentCustomerNote>);
            PrintablePermanentCustomerNotes printable = new PrintablePermanentCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Name, ViewState["PermanentNotes"] as IList<PermanentCustomerNote>, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }

        protected void ClientNotesPrintButton_Click(object sender, ImageClickEventArgs e)
        {
            PrintableClientNotes printable = new PrintableClientNotes(SessionWrapper.Instance.Get.ClientFromQueryString.Name, ViewState["ClientNotes"] as IList<ClientNote>, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }

        protected void ClientPermanentNotesPrintButton_Click(object sender, ImageClickEventArgs e)
        {
            PrintablePermanentClientNotes printable = new PrintablePermanentClientNotes(SessionWrapper.Instance.Get.ClientFromQueryString.Name, 
                                                                ViewState["ClientPermanentNotes"] as IList<PermanentClientNote>, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }


        protected void CliCustNotesPrintButton_Click(object sender, ImageClickEventArgs e)
        {
            //export all client's customer notes
            if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
            {
                PrintableCustomerNotes printable = new PrintableCustomerNotes(SessionWrapper.Instance.Get.ClientFromQueryString.Name, ViewState["CliCustNotes"] as IList<CustomerNote>, true, QueryString.ViewIDValue);
                //string script = PopupHelper.ShowPopup(printable, Server);
                string script = PopupHelper.ShowPopupReportType(printable, Server, true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
        }


        protected void CliCustNotesUpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(dtCliCustNotesFrom.Text))
            {
                dtCliCustNotesFrom.Text = DateTime.Now.ToShortDateString();
            }

            if (string.IsNullOrEmpty(dtCliCustNotesTo.Text))
            {
                dtCliCustNotesTo.Text = DateTime.Now.ToShortDateString();
            }

            if (Convert.ToDateTime(dtCliCustNotesTo.Text) < Convert.ToDateTime(dtCliCustNotesFrom.Text))
            {
                lblDateRangeValidMsg_CliCust.Text = "**Invalid Date Range. End Date must be greater than Start Date.***";
                lblDateRangeValidMsg_CliCust.Visible = true;
                return;
            }

            if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
            {
                lblDateRangeValidMsg_CliCust.Visible = false;
                presenter.LoadAllCustomerNotesForClientOnRange(SessionWrapper.Instance.Get.ClientFromQueryString.Id, GetSelectedDateRange(dtCliCustNotesFrom, dtCliCustNotesTo));
                cffGGV_AllCustomerNotes.ResetPaginationAndFocus();
            }
        }


        protected void PermanentNotesUpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
            {
                DateRange selectedDateRange = GetSelectedDateRange(dtPermanentNotesFilterFrom, dtPermanentNotesFilterTo);

                if (SessionWrapper.Instance.Get.Scope == Scope.CustomerScope)
                {
                    presenter.LoadPermanentCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Id, selectedDateRange);
                    cffGGV_CustomerPermanentNotes.ResetPaginationAndFocus();
                }
                else if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope) 
                {
                    presenter.LoadAllClientsPermanentNotesOnRange(selectedDateRange);
                    cffGGV_ClientPermanentNotes.ResetPaginationAndFocus();
                }
                else
                {
                    presenter.LoadAClientsPermanentNotes(SessionWrapper.Instance.Get.ClientFromQueryString.Id, selectedDateRange);
                    cffGGV_ClientPermanentNotes.ResetPaginationAndFocus();

                    presenter.LoadAllCustomerNotesForClientOnRange(SessionWrapper.Instance.Get.ClientFromQueryString.Id, selectedDateRange);
                    cffGGV_AllCustomerNotes.ResetPaginationAndFocus();
                }
            }
        }

        protected void btnClientNotesFilter_Click(object sender, ImageClickEventArgs e)
        {
            if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
            {
                presenter.LoadClientNotes(SessionWrapper.Instance.Get.ClientFromQueryString.Id, GetSelectedDateRange(dtClientNotesFilterFrom, dtClientNotesFilterTo));
                cffGGV_ClientNotes.ResetPaginationAndFocus();
            }
        }

        protected void btnCustomerNotesFilter_Click(object sender, ImageClickEventArgs e)
        {
            if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
            {
                presenter.LoadCustomerNotes(SessionWrapper.Instance.Get.CustomerFromQueryString.Id, GetSelectedDateRange(TbxCustomerNotesFilterFrom, TbxCustomerNotesFilterTo), 
                                                    ActivityType.Parse(int.Parse(CbxCustomerActivityTypeDropDownList.SelectedValue)),
                                                  NoteType.Parse(int.Parse(CbxCustomerNoteTypeDropDownList.SelectedValue)));
                cffGGV_CustomerNotes.ResetPaginationAndFocus();
            }
        }

 #endregion

        private DateRange GetSelectedDateRange(System.Web.UI.WebControls.TextBox tBoxFrom, System.Web.UI.WebControls.TextBox tBoxTo)
        {
            try
            {
                if (string.IsNullOrEmpty(tBoxFrom.Text))
                {
                    tBoxFrom.Text = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + (DateTime.Now.Year-1).ToString();
                }

                if (string.IsNullOrEmpty(tBoxTo.Text)) {
                    tBoxTo.Text = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();
                }

                Date startDate = new Date(Convert.ToDateTime(tBoxFrom.Text)).StartOfDate;
                Date endDate = new Date(Convert.ToDateTime(tBoxTo.Text)).EndOfDay;
                SessionWrapper.Instance.Get.SelectedDateFromInDatePicker = tBoxFrom.Text;
                SessionWrapper.Instance.Get.SelectedDateToInDatePicker = tBoxTo.Text;

                return new DateRange(startDate, endDate);
            }
            catch
            {
                return new DateRange(new Date(DateTime.Parse(DateTime.Now.ToShortDateString())), new Date(DateTime.Parse(DateTime.Now.ToShortDateString())));
            }
        }

 
    } //end class
} //end namespace