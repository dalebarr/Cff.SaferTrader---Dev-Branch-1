using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Letters;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web.Security;


namespace Cff.SaferTrader.Web
{
    public partial class Contacts : BasePage, IContactsView, IPrintableView
    {
        public string targetName = "";
        public CffGenGridView customerContactsGridView;

        private ContactsPresenter presenter;
        private string eventTarget = "";

        protected CffGenGridView clientContactsGridView;
        protected IList<CustomerContact> editCustomerContacts;
        protected IList<ClientContact> editClientContacts;
        protected IList<int> objRowIndex;

        // for sms var  dbb 12/08/2015
        private int remChar = 0;    // remaining sms characters
        private int defChar = 160;  // default sms length
        private int maxChar = 260;  // ozekei sms server message limit
        private int notifCtr = 0;
        private string loggedUser = "";

        private Letters.CustomerLetter customerLetter = new Letters.CustomerLetter();
        
        #region IContactsView

        public void DisplayClientContacts(IList<ClientContact> clientContacts)
        {
            this.ViewState.Add("clientContacts", clientContacts);

            if (clientContacts != null)
                clientContactsGridView.PagerSettings.Visible = (clientContacts.Count > 50) ? true : false;

            clientContactsGridView.DataSource = clientContacts;
            clientContactsGridView.DataBind();

            if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope)
            {
                CustomerContactSectionUpdatePanel.Visible = false;
            }

            ClientContactSectionUpdatePanel.Visible = true;
        }

        public void DisplayCustomerContacts(IList<CustomerContact> customerContacts)
        {
            try
            {
                this.ViewState.Add("customerContacts", customerContacts);

                if (!string.IsNullOrEmpty(this.PaginationIndex.Value))
                    this.ViewState.Add("letterPaginationIndex", this.PaginationIndex.Value);
                else if (this.ViewState["letterPaginationIndex"] != null)
                    this.PaginationIndex.Value = (string.IsNullOrEmpty(this.ViewState["letterPaginationIndex"].ToString())) ? "ALL" : this.ViewState["letterPaginationIndex"].ToString();

                if (customerContacts != null)
                    customerContactsGridView.PagerSettings.Visible = (customerContacts.Count > 50) ? true : false;

                if (this.ViewState["customerContactsGVCaption"] == null)
                { //check if resulting caption was stroed @sessionwrapper cache 
                    if (SessionWrapper.Instance.Get.SessionCache != null)
                    {
                        if (SessionWrapper.Instance.Get.SessionCache.GetType().Name == "CffSessionCache")
                        {
                            if (SessionWrapper.Instance.Get.SessionCache.CacheObject != null)
                            {
                                if (SessionWrapper.Instance.Get.SessionCache.CacheObject.GetType().Name == "StateBag")
                                {
                                    StateBag xObj = (StateBag)(SessionWrapper.Instance.Get.SessionCache.CacheObject);
                                    if (xObj["customerContactsGVCaption"] != null)
                                    {
                                        this.customerContactsGridView.Caption = xObj["customerContactsGVCaption"].ToString();
                                        SessionWrapper.Instance.Get.SessionCache = null;  //clear after retrieval                                  
                                    }
                                }
                            }
                        }
                    }
                }

                // dbb [20160803]
                //else
                //    this.customerContactsGridView.Caption = this.ViewState["customerContactsGVCaption"].ToString();

                customerContactsGridView.CurrentPageIndex = customerContactsGridView.PageIndex;

                // dbb [20160803]
                customerContactsGridView.RowCommand += cffGGV_CustomerContacts_RowCommand;

                customerContactsGridView.PageIndexChanged += customerContactsGridView_PageIndexChanged;
                //customerContactsGridView.OnTextChanged += cffGGV_CustomerContacts_TextChanged;
                //note: use this if you want to enable editing in-line
                //customerContactsGridView.RowUpdating += customerContactsGridView_RowUpdating;

                customerContactsGridView.AllowPaging = true;
                customerContactsGridView.DataSource = customerContacts;
                customerContactsGridView.GridBag = customerContacts;
                customerContactsGridView.DataBind();
                if (SessionWrapper.Instance.Get.Scope != Scope.AllClientsScope)
                {
                    CustomerContactSectionUpdatePanel.Visible = true;
                }

                if (ViewState["SelectedRowIndex"] != null)
                {
                    customerContactsGridView.SelectedIndex = Convert.ToInt32(ViewState["SelectedRowIndex"]);
                    customerContactsGridView.FocusedRowIndex = Convert.ToInt32(ViewState["SelectedRowIndex"]);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : {0}", e.Message);
            }
        }


        void customerContactsGridView_PageIndexChanged(object sender, EventArgs e)
        { //note: handle this event if you want to enable custom paging
            this.ViewState.Add("GridPageIndex", ((CffGenGridView)sender).PageIndex + 1);
            this.ViewState.Add("letterPaginationIndex", this.PaginationIndex.Value);
        }

        public void ClearClientSearchTextBox()
        {
            ClientContactsSearchTextBox.EncodedText = string.Empty;
        }

        public void ShowAllClientsView()
        {
            ClientSearchSection.Visible = true;
            if (CurrentPrincipal.CffUser.ClientId < 0)
            {
                ClientContactsPanel.Visible = (CurrentPrincipal.CffUser.UserType.Name == "Administrator") ? true : false;
            }
            else
                ClientContactsPanel.Visible = CurrentPrincipal.IsInAdministratorRole;

            ClientAlphabeticalPagination.Visible = true;
            CustomerAlphabeticalPagination.Visible = false;
        }

        public void ShowClientView()
        {
            ClientSearchSection.Visible = false;
            ClientContactsPanel.Visible = true;     // CurrentPrincipal.IsInAdministratorRole;  // dbb
            CustomerAlphabeticalPagination.Visible = true;
        }

        public void ClearCustomerSearchTextBox()
        {
            CustomerContactsSearchTextBox.EncodedText = string.Empty;
        }

        public void ShowCustomerView()
        {
            try
            {
                ClientSearchSection.Visible = false;
                ClientContactsPanel.Visible = true;         //CurrentPrincipal.IsInAdministratorRole;   // false;  // dbb
                CustomerSearchSection.Visible = false;
                CustomerAlphabeticalPagination.Visible = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : {0}", e.Message);
            }
        }

        #endregion



        protected void InitClientContactsGridView()
        {

            clientContactsGridView = new CffGenGridView();
            clientContactsGridView.ID = "clientContactsGridView";
            clientContactsGridView.KeyFieldName = "ContactId";
            clientContactsGridView.ShowHeaderWhenEmpty = true;
            clientContactsGridView.EmptyDataText = "No data to display";
            //clientContactsGridView.BorderWidth = Unit.Pixel(0);  ////Comparison update
            clientContactsGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            clientContactsGridView.AutoGenerateColumns = false;

            ClientContactUpdatePanel.Visible = true;
            CustomerContactSectionUpdatePanel.Visible = true;
            ClientContactSectionUpdatePanel.Visible = true;

            clientContactsGridView.Width = Unit.Percentage(100);
            clientContactsGridView.CssClass = "cffGGV";
            clientContactsGridView.HeaderStyle.CssClass = "cffGGVHeader";
            clientContactsGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            clientContactsGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            clientContactsGridView.CaptionHeaderSettings.BoldCaption = true;
            clientContactsGridView.CaptionHeaderSettings.HorizontalAlignment = HorizontalAlign.Center;

            clientContactsGridView.EnableRowSelect = true;
            clientContactsGridView.RowCommand += cffGGV_ClientContacts_RowCommand; // MSarza : updated prior line
           
            clientContactsGridView.PageSize = 100;
            clientContactsGridView.AllowPaging = true;
            clientContactsGridView.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            clientContactsGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | 
                                                                CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom ;


            //if (this.CurrentScope()==Scope.CustomerScope)   // dbb: disabled
            //{
            //    clientContactsPlaceholder.Visible = false;
            //    clientContactsGridView.Visible = false;
            //}
            //else
            //{
            clientContactsPlaceholder.Visible = true;
            clientContactsGridView.Visible = true;
            //}

            clientContactsGridView.Columns.Clear();
            if (this.CurrentScope() == Scope.AllClientsScope)
            {
                clientContactsGridView.InsertDataColumn("#", "ClientId", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
                clientContactsGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientId", "25%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                clientContactsGridView.InsertDataColumn("First name", "FirstName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                clientContactsGridView.InsertDataColumn("Last name", "LastName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                clientContactsGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                clientContactsGridView.InsertDataColumn("Mobile phone", "MobilePhone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                clientContactsGridView.InsertDataColumn("Fax", "Fax", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                clientContactsGridView.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                clientContactsGridView.InsertDataColumn("Role", "Role", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);

                ClientContactUpdatePanel.Visible = true;
                CustomerContactUpdatePanel.Visible = false;
            }
            else {
                if (this.CurrentScope() == Scope.CustomerScope)
                {
                    clientContactsGridView.EditingMode = CffGridViewEditingMode.EditForm;
                    clientContactsGridView.EditingSettings.EditFormColumnCount = 1;
                    clientContactsGridView.EditingSettings.ShowUpdateButtonOnEdit = false;
                    clientContactsGridView.EditingSettings.ShowCancelButtonOnEdit = false;
                    clientContactsGridView.EditColumnSettings.ColumnsPerRow = 1;
                    clientContactsGridView.EnableSortingAndPagingCallbacks = false;
                    clientContactsGridView.RowClicked += clientContactsGridView_RowClicked;   // added by dbb

                    clientContactsGridView.InsertDataColumn("First name", "FirstName", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Last name", "LastName", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Mobile phone", "MobilePhone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Fax", "Fax", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Role", "Role", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertCommandButtonColumn("", CffGridViewButtonType.Add | CffGridViewButtonType.Edit, "cffGGV_CommandButtons", HorizontalAlign.Center, "10%");
                    //clientContactsGridView.DataKeyNames = new string[] { "FirstName", "LastName", "Phone", "MobilePhone", "Fax", "Email", "Role" };
                } else
                {
                    clientContactsGridView.InsertDataColumn("First name", "FirstName", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Last name", "LastName", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Mobile phone", "MobilePhone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Fax", "Fax", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                    clientContactsGridView.InsertDataColumn("Role", "Role", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                }
                
            }

            //clientContactsGridView.CustomCallback += ClientContactsGridViewCustomCallback;
            clientContactsPlaceholder.Controls.Add(clientContactsGridView);
        }

        protected void InitCustomerContactsGridView()
        {
            CustomerContactsPlaceHolder.Visible = true;
            CustomerContactUpdatePanel.Visible = true;
            CustomerContactSectionUpdatePanel.Visible = false;

            customerContactsGridView = new CffGenGridView();
            customerContactsGridView.ID = "customerContactsGridView";
            customerContactsGridView.KeyFieldName = "ContactId";
            customerContactsGridView.ShowHeaderWhenEmpty = true;
            customerContactsGridView.EmptyDataText = "No data to display";
            customerContactsGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
      
            customerContactsGridView.AutoGenerateColumns = false;
            customerContactsGridView.ShowHeaderWhenEmpty = true;

            customerContactsGridView.Width = Unit.Percentage(100);
            customerContactsGridView.CssClass = "cffGGV";
            customerContactsGridView.HeaderStyle.CssClass = "cffGGVHeader";
            customerContactsGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            customerContactsGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            customerContactsGridView.Visible = true;
            customerContactsGridView.CaptionHeaderSettings.BoldCaption = true;
            customerContactsGridView.CaptionHeaderSettings.HorizontalAlignment = HorizontalAlign.Center;
            
            customerContactsGridView.PageSize = 100;
            customerContactsGridView.AllowPaging = true;
            customerContactsGridView.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            customerContactsGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page |
                                                                    CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            customerContactsGridView.EnableRowSelect = true;

            if (this.CurrentScope() == Scope.CustomerScope)
            {
                //editing Settings
                customerContactsGridView.EditingMode = CffGridViewEditingMode.EditForm;    //.EditFormAndDisplayRow;  modified by dbb [2016/07/19]
                customerContactsGridView.EditingSettings.EditFormColumnCount = 3;
                customerContactsGridView.EditingSettings.EditFormColumnSize = Convert.ToInt32(100 / customerContactsGridView.EditingSettings.EditFormColumnCount);
                customerContactsGridView.EditingSettings.ShowUpdateButtonOnEdit = true;
                customerContactsGridView.EditingSettings.ShowCancelButtonOnEdit = true;

                customerContactsGridView.EditColumnSettings.ColumnsPerRow = 1;
                customerContactsGridView.EnableSortingAndPagingCallbacks = false;

                //Set up the Event Handlers
                customerContactsGridView.RowCommand += cffGGV_CustomerContacts_RowCommand;
                customerContactsGridView.RowUpdateSuccess += customerContactsGridView_RowUpdateSuccess;
                customerContactsGridView.PageIndexChanged += customerContactsGridView_PageIndexChanged;
                clientContactsGridView.RowClicked += clientContactsGridView_RowClicked;
                customerContactsGridView.RowClicked += customerContactsGridView_RowClicked;

                //selection changed
                //customerContactsGridView.CustomButtonCallback += customerContactsGridViewCustomButtonCallBack;
                //customerContactsGridView.CustomCallback += CustomerContactsGridViewCustomCallback;

                //customerContactsGridView.EnableCallBacks = false;
                customerContactsGridView.InsertDataColumn("Default", "IsDefault", CffGridViewColumnType.Boolean, "2%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);

                //MSarza -- added line
                customerContactsGridView.InsertDataColumn("Email Receipts", "emailReceipt", CffGridViewColumnType.Dropdown, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);                
                customerContactsGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
                customerContactsGridView.InsertDataColumn("Mobile phone", "MobilePhone", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
                customerContactsGridView.InsertDataColumn("Fax", "Fax", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
                customerContactsGridView.InsertDataColumn("Role", "Role", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
                customerContactsGridView.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                customerContactsGridView.InsertDataColumn("First name", "FirstName", CffGridViewColumnType.Text, "14%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                customerContactsGridView.InsertDataColumn("Last name", "LastName", CffGridViewColumnType.Text, "14%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left); 

                int custID = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
                //MSarza [20150901]
                //if (SessionWrapper.Instance.Get.IsClientAdminByCFF == false)
                if (SessionWrapper.Instance.Get.IsClientDebtorAdmin == true)
                    {
                        //redundant but we need to recompute the cell width so we do it this way
                        customerContactsGridView.InsertDataColumn("Email Statement", "EmailStatement",
                            CffGridViewColumnType.Boolean, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center,
                            HorizontalAlign.Center);
                        //customerContactsGridView.InsertDataColumn("Default", "IsDefault", CffGridViewColumnType.Boolean, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
                        customerContactsGridView.InsertCommandButtonColumn("",
                            CffGridViewButtonType.Add | CffGridViewButtonType.Edit, "cffGGV_CommandButtons",
                            HorizontalAlign.Center, "10%");
                    }
                    else
                    {
                        customerContactsGridView.InsertCommandButtonColumn(" ",
                            CffGridViewButtonType.Add | CffGridViewButtonType.Edit, "cffGGV_CommandButtons",
                            HorizontalAlign.Center, "10%");
                    }


                //MSarza - modified commented line
                customerContactsGridView.DataKeyNames = new string[] { "IsDefault", "EmailReceipt", "Phone", "MobilePhone", "Fax", "Role", "Email", "FirstName", "LastName", "EmailStatement" };
                //customerContactsGridView.DataKeyNames = new string[]{"IsDefault", "Phone", "MobilePhone", "Fax", "Role", "Email", "FirstName", "LastName"};

            }
            else if (this.CurrentScope() == Scope.ClientScope)
            {
                customerContactsGridView.RowCommand += cffGGV_CustomerContacts_RowCommand;
                customerContactsGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
                customerContactsGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId",  "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                customerContactsGridView.InsertDataColumn("First name", "FirstName", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                customerContactsGridView.InsertDataColumn("Last name", "LastName", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                customerContactsGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                customerContactsGridView.InsertDataColumn("Mobile Phone", "MobilePhone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                customerContactsGridView.InsertDataColumn("Fax", "Fax", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                customerContactsGridView.InsertDataColumn("Role", "Role", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                customerContactsGridView.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

                customerContactsGridView.DataKeyNames = new string[] { "IsDefault", "Phone", "MobilePhone", "Fax", "Role", "Email", "FirstName", "LastName" };
            }

            CustomerContactsPlaceHolder.Controls.Add(customerContactsGridView);
        }


        void customerContactsGridView_RowUpdateSuccess(object sender, EventArgs e)
        {
            //Note: we need to do a force to page refresh due to customized "ADD" row command
            CffGenGridView gV = (CffGenGridView)sender;

            CffSessionCache xCache = new CffSessionCache();
            xCache.CacheObject = (object)ViewState;

            //store needed viewstate values in sessionwrapper - for resulting gridview caption
            SessionWrapper.Instance.Get.SessionCache = (ISessionCache)xCache;
            Response.Redirect(Request.RawUrl);
        }

        protected void cffGGV_ClientContacts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            //int rowIndex = Convert.ToInt32(e.CommandArgument);
            int rowIndex = 0;
            bool displaySuccess = false;

            CffGenGridView gridView = (CffGenGridView)sender;
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;

            var ca =  e.CommandArgument.ToString().IndexOf("$");
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
                        if (clientContactsGridView.DataSource == null)
                            clientContactsGridView.DataSource = gridView.GridBag;

                        editClientContacts = ViewState["clientContacts"] as IList<ClientContact>;

                        if (clientContactsGridView.Rows.Count > 0)
                        {
                            ViewState.Add("clientContacts", (clientContactsGridView.DataSource as IList<ClientContact>));

                            int rIdx = gridView.EditIndex;
                            clientContactsGridView.FocusedRowIndex = rIdx;

                            ClientContact dRow = (ClientContact)clientContactsGridView.GetRow(rIdx);
                            dRow.Modified = DateTime.Now;

                            if (cffPrincipal.IsInAdministratorRole  || cffPrincipal.IsInManagementRole)
                            {
                                if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsEditContactDetails == true)
                                { //if this user is client-management, allow auto update only when client is not administered by CFF.
                                    //clientContactsGridView.Caption = " Note: Your edited values will be subject for validation";
                                }
                                else
                                {
                                    //clientContactsGridView.Caption = " Note: Edit client contact";
                                }
                            }
                            else
                            {
                                //clientContactsGridView.Caption = " Note: Your edited values will be subject for validation";
                            }

                            ViewState.Add("clientContactsGVCaption", clientContactsGridView.Caption);
                            clientContactsGridView.IsInEditMode = true;
                            clientContactsGridView.IsInAddMode = false;
                            clientContactsGridView.DataBind();

                            // [db 20160802]
                            string ctlE = (rIdx <= 9) ? "0" + (rIdx + 2).ToString() : (rIdx + 2).ToString();
                            string valType = "_clc";  // client contacts   
                            string jsEfn = "appendNote('" + ctlE + e.CommandName.ToLower() + valType + "')";
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsEfn, true);
                        }
                    }
                    break;

                case "New":
                    {
                        if (clientContactsGridView.DataSource == null)
                            clientContactsGridView.DataSource = gridView.GridBag; //figure out why is the datasource from sender missing

                        editClientContacts = ViewState["clientContacts"] as IList<ClientContact>;
                        if (clientContactsGridView.Rows.Count > 0)
                        {
                            ViewState.Add("clientContacts", (clientContactsGridView.DataSource as IList<ClientContact>));
                            int rIdx = gridView.FocusedRowIndex;
                            clientContactsGridView.FocusedRowIndex = rIdx;

                            //ClientContact dRow = (ClientContact)clientContactsGridView.GetRow(rIdx);
                            //dRow.Modified = DateTime.Now;

                            if (cffPrincipal.IsInAdministratorRole || (cffPrincipal.IsInManagementRole))
                            {
                                if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsEditContactDetails == true)
                                { //if this user is client-management, allow auto add only when client is not administered by CFF.
                                    //clientContactsGridView.Caption = " Note: Your added values will be subject for validation";
                                }
                                else { 
                                    clientContactsGridView.Caption = "";
                                    }
                            }
                            else
                            { //ref: 4-iv
                                //clientContactsGridView.Caption = " Note: Your added values will be subject for validation";
                            }

                            ViewState.Add("clientContactsGVCaption", clientContactsGridView.Caption);
                            clientContactsGridView.IsInEditMode = false;
                            clientContactsGridView.IsInAddMode = true;
                            clientContactsGridView.DataBind();

                            // [db 20160802]                            
                            string ctlN = (rowIndex <= 9) ? "0" + (rowIndex + 2).ToString() : (rowIndex + 2).ToString();
                            string valType = "_clc";  // client contacts   
                            string jsNfn = "appendNote('" + ctlN + e.CommandName.ToLower() + valType + "')";
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsNfn, true);

                        }
                    }
                    break;

                case "Update":
                    {
                        if (clientContactsGridView.DataSource == null) clientContactsGridView.DataSource = gridView.GridBag;

                        editClientContacts = ViewState["clientContacts"] as IList<ClientContact>;
                        
                        if (e.CommandSource.GetType().Name == "GridView")
                        {
                            gridView = (CffGenGridView)e.CommandSource;
                        }

                        if (gridView != null)
                        {
                            IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", gridView);

                            foreach (CffGVUpdStruct xV in gVUV)
                            {
                                switch (xV.name.Trim().ToUpper())
                                {
                                    case "FIRSTNAME":
                                        editClientContacts[rowIndex].FirstName = xV.value;
                                        break;
                                    case "LASTNAME":
                                        editClientContacts[rowIndex].LastName = xV.value;
                                        break;
                                    case "PHONE":
                                        editClientContacts[rowIndex].Phone = xV.value;
                                        break;
                                    case "MOBILEPHONE":
                                        editClientContacts[rowIndex].MobilePhone = xV.value;
                                        break;
                                    case "FAX":
                                        editClientContacts[rowIndex].Fax = xV.value;
                                        break;
                                    case "EMAIL":
                                        editClientContacts[rowIndex].Email = xV.value;
                                        break;
                                    case "ROLE":
                                        editClientContacts[rowIndex].Role = xV.value;
                                        break;
                                }

                            }

                            if (!IsEmailValid(editClientContacts[rowIndex].Email) && editClientContacts[rowIndex].Email != "" )
                            {
                                clientContactsGridView.EditIndex = -1;
                                clientContactsGridView.IsInEditMode = true;
                                clientContactsGridView.IsInUpdateMode = false;
                                clientContactsGridView.IsInAddMode = false;
                                clientContactsGridView.IsUpdated = false; //no need to page refresh
                                //clientContactsGridView.Caption = "";

                                // [db 20160802]                                
                                string ctlN = (rowIndex <= 9) ? "0" + (rowIndex + 2).ToString() : (rowIndex + 2).ToString();
                                string valType = "_clc";  // client contacts   
                                string jsNfn = "appendNote('" + ctlN + e.CommandName.ToLower() + valType + "')";
                                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsNfn, true);

                                break;
                            }

                            List<object> objParam = new List<object>();
                            if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                            {                               
                                objParam.Clear();
                                objParam.Add("Update");
                                objParam.Add(editClientContacts[rowIndex].ClientId);
                                objParam.Add(editClientContacts[rowIndex].ContactId);
                                objParam.Add(editClientContacts[rowIndex].FirstName);
                                objParam.Add(editClientContacts[rowIndex].LastName);
                                objParam.Add(editClientContacts[rowIndex].Phone);
                                objParam.Add(editClientContacts[rowIndex].Fax);
                                objParam.Add(editClientContacts[rowIndex].MobilePhone);
                                objParam.Add(editClientContacts[rowIndex].Role);
                                objParam.Add(editClientContacts[rowIndex].Email);
                                objParam.Add(System.DateTime.Now);
                                objParam.Add(Convert.ToInt16(cffPrincipal.CffUser.ClientId));   /// .EmployeeId));

                                stpCaller stproc = new stpCaller();
                                int rtnVal = stproc.executeSP(objParam, stpCaller.stpType.InsUpdateCliContact); // stproc.executeSP(objParam, stpCaller.stpType.UpdClientContacts);
                                displaySuccess = true;
                            }

                            if (displaySuccess)
                            {
                                if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                {
                                    //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                    if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                    {
                                        clientContactsGridView.Caption = " Below changes have been submitted and are now subject for review";
                                    }
                                    else
                                    {
                                        gridView.Caption = " Client contact updated";
                                        clientContactsGridView.Caption = " Client contact updated";
                                    }
                                }
                                else
                                { //ref: 4-iv
                                    clientContactsGridView.Caption = " Below changes have been submitted and are now subject for review";
                                }

                                 ((List<ClientContact>)clientContactsGridView.DataSource)[rowIndex] = editClientContacts[rowIndex];
                                clientContactsGridView.GridBag = editClientContacts[rowIndex];
                            }
                            else                            
                                ((List<ClientContact>)clientContactsGridView.DataSource)[rowIndex] = (ClientContact)clientContactsGridView.GridBag;

                            ViewState.Add("clientContacts", editClientContacts);
                            ViewState.Add("clientContactsGVCaption", clientContactsGridView.Caption);
                        }

                        clientContactsGridView.EditIndex = -1;
                        clientContactsGridView.IsUpdated = true;
                        clientContactsGridView.DataSource = editClientContacts;
                        clientContactsGridView.DataBind();
                    }
                    break;

                case "ADD":
                    {
                        if (clientContactsGridView.DataSource == null)
                            clientContactsGridView.DataSource = gridView.GridBag;

                        editClientContacts = ViewState["clientContacts"] as IList<ClientContact>;

                        if (clientContactsGridView.IsInAddMode)
                        { //so we don't double post
                            //Start save newly added contacts to database
                            if (clientContactsGridView.UpdatingValues != null)
                            {
                                IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", gridView);
                                ClientContact newContact = new ClientContact();

                                foreach (CffGVUpdStruct xV in gVUV)
                                {  //NOTE:: REFACTOR THIS SO WE CAN COMPARE VALUES DYNAMICALLY
                                    switch (xV.name.Trim().ToUpper())
                                    {
                                        case "FIRSTNAME":
                                            newContact.FirstName = xV.value;
                                            break;

                                        case "LASTNAME":
                                            newContact.LastName = xV.value;
                                            break;

                                        case "PHONE":
                                            newContact.Phone = xV.value;
                                            break;

                                        case "FAX":
                                            newContact.Fax = xV.value;
                                            break;

                                        case "MOBILEPHONE":
                                            newContact.MobilePhone = xV.value;
                                            break;

                                        case "ROLE":
                                            newContact.Role = xV.value;
                                            break;

                                        case "EMAIL":
                                            newContact.Email = xV.value;
                                            break;
                                    }
                                }                              

                                //MSarza added email validation; this is supported by js validation as this will not invoke a postback
                                //if (!(String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) && !IsEmailValid(newContact.Email)) break;

                                if ((!IsEmailValid(newContact.Email)) || ((newContact.FirstName == "[add new]") && (newContact.LastName == "[add new]"))) {

                                    clientContactsGridView.EditIndex = -1;
                                    clientContactsGridView.IsInUpdateMode = false;
                                    clientContactsGridView.IsUpdated = false; //need this part so we can do a page refresh                                                                   

                                    // [db 20160802]                                    
                                    string ctlN = (rowIndex <= 9) ? "0" + (rowIndex + 2).ToString() : (rowIndex + 2).ToString();
                                    string valType = "_clc"; // client contacts
                                    string jsNfn = "appendNote('" + ctlN + e.CommandName.ToLower() + valType + "')";
                                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsNfn, true);

                                    break;
                                }


                                List<object> objParam = new List<object>();
                                if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                {
                                    //MSarza [20150901]  
                                    //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                    if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                    { //role is client management, allow edit iff client is not administered by CFF
                                        objParam.Add("Insert");
                                    }
                                    else
                                    {  //Role is Admin, Management or Staff: Add and post to dbase    // dbb [20160725]
                                       //objParam.Add("AddNewContact");
                                        editClientContacts.Add(newContact);
                                        objParam.Add("Insert");
                                    }
                                }
                                else
                                { //ref: 4-iv - post changes to this customer contact for validation: below already contains the updated values
                                    objParam.Add("Insert");
                                }


                                stpCaller myStp = new stpCaller();
                                //int retVal = myStp.executeSP(objParam, stpCaller.stpType.InsUpdCustContForValid);
                                displaySuccess = true;

                                if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                {  //Role is Admin, Management or Staff: Edit current row and post to dbase. 
                                   //when client is in management role, allow update iff client is not administered by cff
                                   //MSarza [20150901]  
                                   //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                    if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                    {
                                        //do nothing 
                                        displaySuccess = false;
                                    }
                                    else
                                    {

                                        objParam.Add((newContact.ClientId == 0) ? editClientContacts[0].ClientId : newContact.ClientId);
                                        objParam.Add(newContact.ContactId);
                                        objParam.Add(newContact.FirstName);
                                        objParam.Add(newContact.LastName);
                                        objParam.Add(newContact.Phone);
                                        objParam.Add(newContact.Fax);
                                        objParam.Add(newContact.MobilePhone);
                                        objParam.Add(newContact.Role);
                                        objParam.Add(newContact.Email);
                                        objParam.Add(DateTime.Now);
                                        objParam.Add(Convert.ToInt16(cffPrincipal.CffUser.ClientId));   ///.EmployeeId));

                                        //if (!(String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) && !IsEmailValid(newContact.Email)) break;
                                        int retVal = myStp.executeSP(objParam, Cff.SaferTrader.Core.Letters.stpCaller.stpType.InsUpdateCliContact);                          

                                    }
                                }

                                if (displaySuccess)
                                {
                                    if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                    {
                                        //MSarza [20150901]  
                                        //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                        if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                        {
                                            clientContactsGridView.Caption = " Below changes have been submitted and are now subject for review";
                                        }
                                        else
                                        {
                                            gridView.Caption = " Client contact added";
                                            clientContactsGridView.Caption = " Client contact added";
                                        }
                                    }
                                    else
                                    { //ref: 4-iv
                                        clientContactsGridView.Caption = " Below changes have been submitted and are now subject for review";
                                    }
                                } else
                                {
                                    clientContactsGridView.Caption = " Access error ";
                                }

                                ViewState.Add("clientContacts", editClientContacts);
                                ViewState.Add("clientContactsGVCaption", clientContactsGridView.Caption);

                            }
                            //end save newly added contacts to database
                        }

                        clientContactsGridView.EditIndex = -1;
                        clientContactsGridView.IsInEditMode = false;
                        clientContactsGridView.IsInUpdateMode = false;
                        clientContactsGridView.IsInAddMode = false;
                        clientContactsGridView.IsUpdated = true; //need this part so we can do a page refresh

                        clientContactsGridView.DataSource = editClientContacts;
                        clientContactsGridView.DataBind();
                    }
                    break;

                default:
                    clientContactsGridView.EditIndex = -1;
                    clientContactsGridView.IsInEditMode = false;
                    clientContactsGridView.IsInUpdateMode = false;
                    clientContactsGridView.IsInAddMode = false;
                    clientContactsGridView.IsUpdated = true; //need this part so we can do a page refresh
                    clientContactsGridView.Caption = "";
                    //ViewState.Add("clientContactsGVCaption", "");
                    presenter.LoadAClientContacts(SessionWrapper.Instance.Get.ClientFromQueryString.Id, "Select");  // dbb
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
                            for (int colCtr = 0; colCtr < dGrid.Columns.Count; colCtr++)
                            {
                                if (dGrid.Columns[colCtr].GetType().Name == "CffTemplateField")
                                {
                                    string colName = (dGrid.Columns[colCtr] as CffTemplateField).DataBoundColumnName;
                                    string cctrlName = ctrlName.Substring(ctrlName.IndexOf(gID) + gID.Length).Split('$')[2].Substring(3);
                                    if (cctrlName == colName)
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
                    }
                }
            }
            return xNRPK;
        }
        //protected void cffGGV_CustomerContacts_TextChanged(object sender, EventArgs e)
        //{
        //}

        protected void cffGGV_CustomerContacts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = 0;
            bool displaySuccess = false;

            CffGenGridView gridView = (CffGenGridView)sender;
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;

            if (e.CommandArgument.ToString().IndexOf("$") >= 0)
            {
                string[] cargs = e.CommandArgument.ToString().Split('$');
                rowIndex = Convert.ToInt32(cargs[1]);
            }
            else
                rowIndex = Convert.ToInt32(e.CommandArgument);

             switch (e.CommandName)
             {
                 case "Edit": {
                    
                        if (customerContactsGridView.DataSource == null)
                             customerContactsGridView.DataSource = gridView.GridBag; //figure out why is the datasource from sender missing
                        
                        editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;

                        if (customerContactsGridView.Rows.Count > 0)
                        {
                            ViewState.Add("customerContacts", (customerContactsGridView.DataSource as IList<CustomerContact>));

                            //from CellEditOrInitialize
                            int rIdx = gridView.EditIndex;   //.FocusedRowIndex;   //dbb
                                                             //int rIdx = rowIndex;                 //dbb
                            customerContactsGridView.FocusedRowIndex = rIdx;

                            Cff.SaferTrader.Core.CustomerContact dRow = (Cff.SaferTrader.Core.CustomerContact)customerContactsGridView.GetRow(rIdx);
                            dRow.Modified = System.DateTime.Now;

                             if (cffPrincipal.IsInAdministratorRole || (cffPrincipal.IsInManagementRole))
                             {
                                 if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsEditContactDetails == true)
                                 { //if this user is client-management, allow auto update only when client is not administered by CFF.
                                     //customerContactsGridView.Caption = " Note: Your edited values will be subject for validation";
                                 }
                                 else
                                 {
                                     customerContactsGridView.Caption = "";
                                 }
                             }
                             else
                             { //ref: 4-iv
                                 //customerContactsGridView.Caption = " Note: Your edited values will be subject for validation";
                             }

                            //ViewState.Add("customerContactsGVCaption", customerContactsGridView.Caption);
                            customerContactsGridView.IsInEditMode = true;
                            customerContactsGridView.IsInAddMode = false;
                            customerContactsGridView.DataBind();

                            // [db 20160802]
                            string ctlE = (rIdx <= 9) ? "0" + (rIdx + 2).ToString() : (rIdx + 2).ToString();
                            string valType = "_cuc";  // customer contacts   
                            string jsEfn = "appendNote('" + ctlE + e.CommandName.ToLower() + valType + "')";
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsEfn, true);

                        }
                    }
                    break;

                case "New":
                    {
                        if (customerContactsGridView.DataSource == null)
                            customerContactsGridView.DataSource = gridView.GridBag; //figure out why is the datasource from sender missing

                        editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;

                        if (customerContactsGridView.Rows.Count > 0)
                        {
                            ViewState.Add("customerContacts", (customerContactsGridView.DataSource as IList<CustomerContact>));

                            //from CellEditOrInitialize
                            //int rIdx = gridView.FocusedRowIndex;
                            int rIdx = gridView.EditIndex;
                            customerContactsGridView.FocusedRowIndex = rIdx;

                            Cff.SaferTrader.Core.CustomerContact dRow = (Cff.SaferTrader.Core.CustomerContact)customerContactsGridView.GetRow(rIdx);
                            dRow.Modified = System.DateTime.Now;

                            if (cffPrincipal.IsInAdministratorRole || (cffPrincipal.IsInManagementRole))
                            {
                                if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsEditContactDetails == true)
                                { //if this user is client-management, allow auto add only when client is not administered by CFF.
                                    //customerContactsGridView.Caption = " Note: Your added values will be subject for validation";
                                }
                                else
                                {
                                    customerContactsGridView.Caption = "";
                                }
                            }
                            else
                            { //ref: 4-iv
                                //customerContactsGridView.Caption = "**Note: Your added values will be subject for validation";
                            }

                            ViewState.Add("customerContactsGVCaption", customerContactsGridView.Caption);
                            customerContactsGridView.IsInEditMode = false;
                            customerContactsGridView.IsInAddMode = true;
                            customerContactsGridView.DataBind();

                            // [db 20160802]
                            string ctlN = (rowIndex <= 9) ? "0" + (rowIndex + 2).ToString() : (rowIndex + 2).ToString();
                            string valType = "_cuc";  // customer contacts   
                            string jsNfn = "appendNote('" + ctlN + e.CommandName.ToLower() + valType + "')";
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsNfn, true);
                        }
                    }
                    break;

                case "Update": 
                     {
                         //CustomerContactsGridViewRowValidating(sender, e);

                         if (customerContactsGridView.DataSource == null)
                             customerContactsGridView.DataSource = gridView.GridBag; //todo:: figure out why is the datasource from sender missing

                        editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;

                        //Msarza:  The Checked value of a CheckBox is sent by the browser as the value "on". 
                        //         And if it is not checked in the browser, nothing is sent with the request. 
                        //         Hence, if IsDefault and EmailStatement is not detected on the foreach-switch below if
                        //         said chackboxes are not ticked; which is the reason for the below initialisation to false.
                        editCustomerContacts[rowIndex].IsDefault = false;
                        editCustomerContacts[rowIndex].EmailStatement = false;

                        if (e.CommandSource.GetType().Name == "GridView")
                        {   //do your data posting here
                            gridView = (CffGenGridView)e.CommandSource;
                        }

                        if (gridView != null)
                        {
                            IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", gridView);

                            //NOTE:: REFACTOR THIS SO WE CAN COMPARE VALUES DYNAMICALLY
                            foreach (CffGVUpdStruct xV in gVUV)
                            {
                                switch (xV.name.Trim().ToUpper())
                                {
                                    case "ISDEFAULT":
                                        editCustomerContacts[rowIndex].IsDefault = (xV.value == "on") ? true : false;
                                        break;

                                    case "EMAILSTATEMENT":
                                        editCustomerContacts[rowIndex].EmailStatement = (xV.value == "on") ? true : false;
                                        break;

                                    //MSarza
                                    case "EMAILRECEIPT":
                                        editCustomerContacts[rowIndex].EmailReceipt = Convert.ToInt16(xV.value);
                                        break;

                                    case "MOBILEPHONE":
                                        editCustomerContacts[rowIndex].MobilePhone = xV.value;
                                        break;

                                    case "EMAIL":
                                        editCustomerContacts[rowIndex].Email = xV.value;
                                        break;

                                    case "FAX":
                                        editCustomerContacts[rowIndex].Fax = xV.value;
                                        break;

                                    case "PHONE":
                                        editCustomerContacts[rowIndex].Phone = xV.value;
                                        break;

                                    case "ROLE":
                                        editCustomerContacts[rowIndex].Role = xV.value;
                                        break;

                                    case "FIRSTNAME":
                                        editCustomerContacts[rowIndex].FirstName = xV.value;
                                        break;

                                    case "LASTNAME":
                                        editCustomerContacts[rowIndex].LastName = xV.value;
                                        break;

                                    default:
                                        break;

                                }
                            }

                            if (!IsEmailValid(editCustomerContacts[rowIndex].Email) && editCustomerContacts[rowIndex].Email != "") {
                                customerContactsGridView.EditIndex = -1;
                                customerContactsGridView.IsInEditMode = true;
                                customerContactsGridView.IsInUpdateMode = false;
                                customerContactsGridView.IsInAddMode = false;
                                customerContactsGridView.IsUpdated = false; //need this part so we can do a page refresh

                                // [db 20160802]
                                string ctlE = (rowIndex <= 9) ? "0" + (rowIndex + 2).ToString() : (rowIndex + 2).ToString();
                                string valType = "_cuc";  // customer contacts   
                                string jsEfn = "appendNote('" + ctlE + e.CommandName.ToLower() + valType + "')";
                                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsEfn, true);

                                break;
                            }

                            List<object> objParam = new List<object>();
                             if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                             {
                                 //MSarza [20150901]
                                 //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                 if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                 { //role is client management, allow edit iff client is not administered by CFF
                                     objParam.Add("Insert");
                                 }
                                 else
                                 {  //Role is Admin, Management or Staff: Edit current row and post to dbase
                                     objParam.Add("UpdCustContact");
                                 }
                             }
                             else
                             { //ref: 4-iv - post changes to this customer contact for validation: below already contains the updated values
                                 objParam.Add("Insert");
                             }

                             objParam.Add(editCustomerContacts[rowIndex].ContactId);
                             objParam.Add(editCustomerContacts[rowIndex].CustomerId);
                             objParam.Add(editCustomerContacts[rowIndex].LastName);
                             objParam.Add(editCustomerContacts[rowIndex].FirstName);
                             objParam.Add(editCustomerContacts[rowIndex].Role);
                             objParam.Add(editCustomerContacts[rowIndex].Phone);
                             objParam.Add(editCustomerContacts[rowIndex].Fax);
                             objParam.Add(editCustomerContacts[rowIndex].Email);
                             objParam.Add(editCustomerContacts[rowIndex].MobilePhone);
                             objParam.Add((editCustomerContacts[rowIndex].IsDefault == false) ? 0 : 1);
                             objParam.Add(System.DateTime.Now);
                             objParam.Add((editCustomerContacts[rowIndex].Attn==false)?0:1);
                             objParam.Add(Convert.ToInt16(cffPrincipal.CffUser.ClientId)); ///.EmployeeId));
                             objParam.Add((editCustomerContacts[rowIndex].EmailStatement==false)?0:1);
                             objParam.Add(editCustomerContacts[rowIndex].EmailReceipt);
                              
                             //MSarza added email validation; this is supported by js validation as this will not invoke a postback
                            if (!(String.IsNullOrEmpty(editCustomerContacts[rowIndex].Email)) && !IsEmailValid(editCustomerContacts[rowIndex].Email)) break;
                            //MSarza: if there is no email address, make sure email statement is 0 or false
                            //if (String.IsNullOrEmpty(editCustomerContacts[rowIndex].Email)) objParam[14] = 0;
                            //MSarza: if there is no email address, validate with user
                            //if (String.IsNullOrEmpty(editCustomerContacts[rowIndex].Email)) break;
                            //MSarza: if there is no email address, validate with user when EmailStatement is ticked
                            if ((String.IsNullOrEmpty(editCustomerContacts[rowIndex].Email)) && editCustomerContacts[rowIndex].EmailStatement == true) break;

                            Cff.SaferTrader.Core.Letters.stpCaller myStp = new Cff.SaferTrader.Core.Letters.stpCaller();
                            int retVal = myStp.executeSP(objParam, Cff.SaferTrader.Core.Letters.stpCaller.stpType.InsUpdCustContForValid);
                            displaySuccess = true;

                            if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                            {  //Role is Admin, Management or Staff: Edit current row and post to dbase. 
                               //when client is in management role, allow update iff client is not administered by cff
                               //MSarza [20150901]   
                               //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                {
                                    //do nothing 
                                }
                                else
                                {
                                    if (editCustomerContacts[rowIndex].IsDefault == true)
                                    {  // do update iff default contact was reassigned
                                        for (int ix = 0; ix < editCustomerContacts.Count; ix++)
                                        {
                                            if ((ix != rowIndex) && (editCustomerContacts[ix].IsDefault == true))
                                            { //toggle undefaulted contacts
                                                editCustomerContacts[ix].IsDefault = false;

                                                 objParam.Clear();
                                                 objParam.Add("UpdCustContact");
                                                 objParam.Add(editCustomerContacts[ix].ContactId);
                                                 objParam.Add(editCustomerContacts[ix].CustomerId);
                                                 objParam.Add(editCustomerContacts[ix].LastName);
                                                 objParam.Add(editCustomerContacts[ix].FirstName);
                                                 objParam.Add(editCustomerContacts[ix].Role);
                                                 objParam.Add(editCustomerContacts[ix].Phone);
                                                 objParam.Add(editCustomerContacts[ix].Fax);
                                                 objParam.Add(editCustomerContacts[ix].Email);
                                                 objParam.Add(editCustomerContacts[ix].MobilePhone);
                                                 objParam.Add(0); //false
                                                 objParam.Add(System.DateTime.Now);
                                                 objParam.Add((editCustomerContacts[ix].Attn==false)?0:1);
                                                 objParam.Add(Convert.ToInt16(cffPrincipal.CffUser.ClientId)); ///.EmployeeId));  //objParam.Add(editCustomerContacts[rIdx].ModifiedBy);
                                                 objParam.Add((editCustomerContacts[ix].EmailStatement==false)?0:1);
                                                 objParam.Add(editCustomerContacts[rowIndex].EmailReceipt);

                                                //MSarza added email validation; this is supported by js validation as this will not invoke a postback
                                                if (!(String.IsNullOrEmpty(editCustomerContacts[rowIndex].Email)) && !IsEmailValid(editCustomerContacts[rowIndex].Email)) break;
                                                //MSarza: if there is no email address, make sure email statement is 0 or false
                                                if (String.IsNullOrEmpty(editCustomerContacts[rowIndex].Email)) objParam[14] = 0;

                                                retVal = myStp.executeSP(objParam, Cff.SaferTrader.Core.Letters.stpCaller.stpType.InsUpdCustContForValid);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                             if (displaySuccess)
                             {
                                 if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                 {
                                     //MSarza [20150901]  
                                     //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                     if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                     {
                                         customerContactsGridView.Caption = " Below changes have been submitted and are now subject for review";
                                     }
                                     else
                                     {
                                         gridView.Caption = " Customer contacts updated";
                                         customerContactsGridView.Caption = " Customer contacts updated";
                                     }
                                 }
                                 else
                                 { //ref: 4-iv
                                     customerContactsGridView.Caption = " Below changes have been submitted and are now subject for review";
                                 }

                                ((List<CustomerContact>)gridView.DataSource)[rowIndex] = editCustomerContacts[rowIndex];
                                customerContactsGridView.GridBag = editCustomerContacts[rowIndex];
                            }
                            else
                                ((List<CustomerContact>)customerContactsGridView.DataSource)[rowIndex] = (CustomerContact)customerContactsGridView.GridBag;

                            ViewState.Add("customerContacts", editCustomerContacts);
                            ViewState.Add("customerContactsGVCaption", customerContactsGridView.Caption);
                        }

                        customerContactsGridView.EditIndex = -1;
                        customerContactsGridView.IsInAddMode = false;
                        customerContactsGridView.IsInEditMode = false;
                        customerContactsGridView.IsInUpdateMode = false;
                        customerContactsGridView.IsUpdated = true; //need this part so we can do a page refresh
                        customerContactsGridView.DataSource = editCustomerContacts;
                        customerContactsGridView.DataBind();
                    }
                    break;                 

                 case "ADD":
                    {
                        if (customerContactsGridView.DataSource == null)
                            customerContactsGridView.DataSource = gridView.GridBag;

                        editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;

                        if (customerContactsGridView.IsInAddMode)
                        { //so we don't double post
                            //Start save newly added contacts to database
                            if (customerContactsGridView.UpdatingValues != null)
                            {
                                IList<CffGVUpdStruct> gVUV = RetrieveViewStateValues("UpdateValues", gridView);
                                CustomerContact newContact = new CustomerContact();

                                foreach (CffGVUpdStruct xV in gVUV)
                                {  //NOTE:: REFACTOR THIS SO WE CAN COMPARE VALUES DYNAMICALLY
                                    switch (xV.name.Trim().ToUpper())
                                    {
                                        case "ISDEFAULT":
                                            newContact.IsDefault = (xV.value == "on") ? true : false;
                                            break;

                                        case "EMAILSTATEMENT":
                                            newContact.EmailStatement = (xV.value == "on") ? true : false;
                                            break;

                                        //MSarza
                                        case "EMAILRECEIPT":
                                            newContact.EmailReceipt = Convert.ToInt16(xV.value);
                                            break;

                                        case "MOBILEPHONE":
                                            newContact.MobilePhone = xV.value;
                                            break;

                                        case "EMAIL":
                                            newContact.Email = xV.value;
                                            break;

                                        case "FAX":
                                            newContact.Fax = xV.value;
                                            break;

                                        case "PHONE":
                                            newContact.Phone = xV.value;
                                            break;

                                        case "ROLE":
                                            newContact.Role = xV.value;
                                            break;

                                        case "FIRSTNAME":
                                            newContact.FirstName = xV.value;
                                            break;

                                        case "LASTNAME":
                                            newContact.LastName = xV.value;
                                            break;

                                        default:
                                            break;

                                    }
                                }


                                //MSarza added email validation; this is supported by js validation as this will not invoke a postback
                                //if (!(String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) && !IsEmailValid(newContact.Email)) break;  // dbb [20160802]
                                //MSarza: if there is no email address or "[add new]", make sure email statement is 0 or false
                                //if (String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) objParam[14] = 0;
                                //MSarza: if there is no email address, validate with user when EmailStatement is ticked
                                //if ((String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) && newContact.EmailStatement == true) break; // dbb [20160802]

                                // dbb [20160729]
                                if (!IsEmailValid(newContact.Email) || newContact.FirstName == "[add new]" && newContact.LastName == "[add new]" && newContact.EmailStatement == true) 
                                {
                                    customerContactsGridView.EditIndex = -1;
                                    customerContactsGridView.IsInUpdateMode = false;
                                    customerContactsGridView.IsUpdated = true;
                                    customerContactsGridView.IsInAddMode = true;
                                    customerContactsGridView.IsInEditMode = false;

                                    // [db 20160802]                                    
                                    string ctlN = (rowIndex <= 9) ? "0" + (rowIndex + 2).ToString() : (rowIndex + 2).ToString();
                                    string valType = "_cuc"; // customer contacts
                                    string jsNfn = "appendNote('" + ctlN + e.CommandName.ToLower() + valType + "')";
                                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "NoteScript", jsNfn, true);

                                    break;
                                }

                                List<object> objParam = new List<object>();
                                if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                {
                                    //MSarza [20150901]  
                                    //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                    if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                    { //role is client management, allow edit iff client is not administered by CFF
                                        objParam.Add("Insert");
                                    }
                                    else
                                    {  //Role is Admin, Management or Staff: Add and post to dbase
                                        editCustomerContacts.Add(newContact);
                                        objParam.Add("AddNewContact");
                                    }
                                }
                                else
                                { //ref: 4-iv - post changes to this customer contact for validation: below already contains the updated values
                                    objParam.Add("Insert");
                                }

                                objParam.Add(newContact.ContactId);
                                objParam.Add((newContact.CustomerId == 0) ? editCustomerContacts[0].CustomerId : newContact.CustomerId);
                                objParam.Add(newContact.LastName);
                                objParam.Add(newContact.FirstName);
                                objParam.Add(newContact.Role);
                                objParam.Add(newContact.Phone);
                                objParam.Add(newContact.Fax);
                                objParam.Add(newContact.Email);
                                objParam.Add(newContact.MobilePhone);
                                objParam.Add((newContact.IsDefault == false) ? 0 : 1);
                                objParam.Add(System.DateTime.Now);
                                objParam.Add((newContact.Attn == false) ? 0 : 1);
                                objParam.Add(Convert.ToInt16(cffPrincipal.CffUser.ClientId)); ///.EmployeeId));
                                objParam.Add((newContact.EmailStatement == false) ? 0 : 1);
                                objParam.Add(newContact.EmailReceipt);

                                Cff.SaferTrader.Core.Letters.stpCaller myStp = new Cff.SaferTrader.Core.Letters.stpCaller();
                                int retVal = myStp.executeSP(objParam, Cff.SaferTrader.Core.Letters.stpCaller.stpType.InsUpdCustContForValid);
                                displaySuccess = true;

                                if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                {  //Role is Admin, Management or Staff: Edit current row and post to dbase. 
                                   //when client is in management role, allow update iff client is not administered by cff
                                   //MSarza [20150901]  
                                   //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                    if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                    {
                                        //do nothing 
                                    }
                                    else
                                    {
                                        if (newContact.IsDefault == true)
                                        {  // do update iff default contact was reassigned
                                            for (int ix = 0; ix < editCustomerContacts.Count; ix++)
                                            {
                                                if ((ix != rowIndex) && (editCustomerContacts[ix].IsDefault == true))
                                                { //toggle undefaulted contacts
                                                    editCustomerContacts[ix].IsDefault = false;

                                                    objParam.Clear();
                                                    objParam.Add("UpdCustContact");
                                                    objParam.Add(editCustomerContacts[ix].ContactId);
                                                    objParam.Add(editCustomerContacts[ix].CustomerId);
                                                    objParam.Add(editCustomerContacts[ix].LastName);
                                                    objParam.Add(editCustomerContacts[ix].FirstName);
                                                    objParam.Add(editCustomerContacts[ix].Role);
                                                    objParam.Add(editCustomerContacts[ix].Phone);
                                                    objParam.Add(editCustomerContacts[ix].Fax);
                                                    objParam.Add(editCustomerContacts[ix].Email);
                                                    objParam.Add(editCustomerContacts[ix].MobilePhone);
                                                    objParam.Add(false);
                                                    objParam.Add(System.DateTime.Now);
                                                    objParam.Add((editCustomerContacts[ix].Attn == false) ? 0 : 1);
                                                    objParam.Add(Convert.ToInt16(cffPrincipal.CffUser.ClientId));    ///.EmployeeId));  //objParam.Add(editCustomerContacts[rIdx].ModifiedBy);
                                                    objParam.Add((editCustomerContacts[ix].EmailStatement == false) ? 0 : 1);
                                                    objParam.Add(newContact.EmailReceipt);

                                                    //MSarza added email validation; this is supported by js validation as this will not invoke a postback
                                                    if (!(String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) && !IsEmailValid(newContact.Email)) break;
                                                    //MSarza: if there is no email address or "[add new]", make sure email statement is 0 or false
                                                    //if (String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) objParam[14] = 0;
                                                    //MSarza: if there is no email address, validate with user when EmailStatement is ticked
                                                    if ((String.IsNullOrEmpty(Regex.Replace(newContact.Email, @"(?<=^|\s)\[add new\](?=\s|$)", ""))) && editCustomerContacts[ix].EmailStatement) break;

                                                    retVal = myStp.executeSP(objParam, Cff.SaferTrader.Core.Letters.stpCaller.stpType.InsUpdCustContForValid);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (displaySuccess)
                                {
                                    if (cffPrincipal.IsInAdministratorRole || cffPrincipal.IsInManagementRole)
                                    {
                                        //MSarza [20150901]  
                                        //if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsClientAdminByCFF == true)
                                        if (cffPrincipal.IsInManagementRole && (cffPrincipal.CffUser.UserType.Name.IndexOf("Client") >= 0) && SessionWrapper.Instance.Get.IsCffDebtorAdminForClient == true)
                                        {
                                            customerContactsGridView.Caption = " Note: Below changes have been submitted and are now subject for review";
                                        }
                                        else
                                        {
                                            gridView.Caption = " Customer contact added";
                                            customerContactsGridView.Caption = " Customer contact added";
                                        }
                                    }
                                    else
                                    { //ref: 4-iv
                                        customerContactsGridView.Caption = "Note: Below changes have been submitted and are now subject for review";
                                    }
                                }

                                ViewState.Add("customerContacts", editCustomerContacts);
                                ViewState.Add("customerContactsGVCaption", customerContactsGridView.Caption);

                            }
                            //end save newly added contacts to database
                        }

                        customerContactsGridView.EditIndex = -1;
                        customerContactsGridView.IsInEditMode = false;
                        customerContactsGridView.IsInUpdateMode = false;
                        customerContactsGridView.IsInAddMode = false;
                        customerContactsGridView.IsUpdated = true; //need this part so we can do a page refresh

                        customerContactsGridView.DataSource = editCustomerContacts;
                        customerContactsGridView.DataBind();
                    }
                    break;

                default:
                    {
                        customerContactsGridView.EditIndex = -1;
                        customerContactsGridView.IsInEditMode = false;
                        customerContactsGridView.IsInUpdateMode = false;
                        customerContactsGridView.IsInAddMode = false;
                        customerContactsGridView.Caption = "";
                        ViewState.Add("customerContactsGVCaption", "");
                        presenter.LoadACustomersContacts(SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
                        populateEmailList(customerContactsGridView.FocusedRowIndex);
                    }
                    break;
            }
        }


        public static bool myIsNumeric(string Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);
            if (User.Identity.IsAuthenticated)
            {
                CustomerAlphabeticalPagination.UpdatePageIndex += CustomerContactPageIndexUpdate;
                ClientAlphabeticalPagination.UpdatePageIndex += ClientContactPageIndexUpdate;

                presenter = ContactsPresenter.Create(this);
                if (SessionWrapper.Instance.Get != null)
                {
                    presenter.InitializeForScope(SessionWrapper.Instance.Get.Scope);

                    // start related ref:CFF-18
                    if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                    {
                        targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
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
                    }
                }

                //Note: Configure Client and Contact Columns
                InitClientContactsGridView();
                InitCustomerContactsGridView();

                ClearSearchTextBox();

                // for SMS use only
                remChar = (defChar - SMSMsg.Text.Length);
                notifCtr = 0;
                if (remChar >= 0 && remChar <= defChar)
                {
                    txtCount.Text = remChar.ToString() + " characters remaining";
                    smsPanel.Update();
                }
            }
            else
            {
                throw new System.ArgumentException("User not authenticated", "Invalid user.");
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!User.Identity.IsAuthenticated)
            {
                throw new System.ArgumentException("User not authenticated", "Invalid user.");
            }

            loggedUser = User.Identity.Name;

            if (Page.IsPostBack && customerContactsGridView.IsRowCommandPostBack && customerContactsGridView.IsInEditMode && !customerContactsGridView.IsInAddMode)
            {
                customerContactsGridView.Visible = true;
                clientContactsGridView.Visible = true;   // false;
                DisplayCustomerContacts(ViewState["customerContacts"] as IList<CustomerContact>);
                DisplayClientContacts(ViewState["clientContacts"] as IList<ClientContact>);
            }
            else if (Page.IsPostBack && (!customerContactsGridView.IsUpdated || !clientContactsGridView.IsUpdated))
            {
                string cArgs = "";
                Control c = GetPostBackControl(this.Page, out cArgs);
                if (c != null)
                {
                    eventTarget = c.ID;
                }
                
                string curLetterIdx = this.PaginationIndex.Value.ToString();             

                if (eventTarget == "customerContactsGridView")
                //|| customerContactsGridView.IsCallback == true // (customerContactsGridView.IsCallback)customerContactsGridView.Visible = true;
                {

                    CffGenGridView gridView = customerContactsGridView;
                    if (customerContactsGridView.DataSource == null)
                    {
                        customerContactsGridView.DataSource = gridView.GridBag;   //figure out why is the datasource from sender missing
                    }

                    //true when edit is pressed
                    if (customerContactsGridView.EditIndex >= 0)
                        customerContactsGridView.GridBag =
                            ((CffGenGridView)c).GetRow(customerContactsGridView.EditIndex);   // dbb

                    //bool isgridenabled = customerContactsGridView.Enabled;
                    //bool isclientsideapi = customerContactsGridView.IsClientSideAPIEnabled(); //true when edit
                    if (customerContactsGridView.IsNewRowEditing)
                    {
                        customerContactsGridView.EditingSettings.NewItemRowPosition = CffGridViewNewRowPosition.Bottom;
                    }
                    else if (customerContactsGridView.IsInEditMode)
                    {
                        //true when edit, update or cancel
                        //if rowediting get the current value of edited control and save to gridbag

                        //if rowupdating get the current value of edited control here and save to gridbag
                    }
                    else
                    {
                        customerContactsGridView.EditingSettings.NewItemRowPosition = CffGridViewNewRowPosition.Bottom;
                        customerContactsGridView.EditingSettings.Mode = CffGridViewEditingMode.EditForm;    //.EditFormAndDisplayRow;  // dbb [20160808]
                    }

                    string selRowIndex = ddlEmailToCustomer.SelectedIndex.ToString();

                    //check if event argument is not empty 
                    if (!string.IsNullOrEmpty(cArgs))
                    {
                        if (cArgs.IndexOf("rc") >= 0)
                            selRowIndex = cArgs.Substring(2); //postback happens on gridview row click
                    }
                    else cArgs = "";

                    //if (cArgs.IndexOf("Edit") >= 0 || cArgs.IndexOf("New") >= 0) // dbb
                    if (cArgs.IndexOf("Edit") >= 0)
                        LoadContacts();
                    else
                    {
                        ViewState.Add("SelectedRowIndex", Convert.ToInt32(selRowIndex));
                        DisplayCustomerContacts(ViewState["customerContacts"] as IList<CustomerContact>);

                        if (customerContactsGridView.DataSource == null && ViewState["customerContacts"] != null &&
                            !string.IsNullOrEmpty(selRowIndex))

                            DisplayCustomerContacts(ViewState["customerContacts"] as IList<CustomerContact>);

                        if (cArgs.IndexOf("New") >= 0)
                        {
                            customerContactsGridView.IsInAddMode = true;
                            customerContactsGridView.IsInEditMode = false;
                            customerContactsGridView.DataBind();
                        }
                        
                    }
                }
                else
                {
                    if (eventTarget == "ddlEmailToCustomer")
                        ViewState.Add("SelectedRowIndex", ddlEmailToCustomer.SelectedIndex);

                    string selRowIndex = ddlEmailToCustomer.SelectedIndex.ToString();

                    //check if event argument is not empty 
                    if (!string.IsNullOrEmpty(cArgs))
                    {
                        if (cArgs.IndexOf("rc") >= 0)
                            selRowIndex = cArgs.Substring(2); //postback happens on gridview row click
                    }
                    else cArgs = "";

                    //if (cArgs.IndexOf("Edit") >= 0 || cArgs.IndexOf("New") >= 0) // dbb

                    if (cArgs.IndexOf("Edit") >= 0)
                        LoadContacts();
                    else
                    {
                        ViewState.Add("SelectedRowIndex", Convert.ToInt32(selRowIndex));
                        //DisplayClientContacts(ViewState["clientContacts"] as IList<ClientContact>);

                        if (clientContactsGridView.DataSource == null && ViewState["clientContacts"] != null &&
                            !string.IsNullOrEmpty(selRowIndex))

                            DisplayClientContacts(ViewState["clientContacts"] as IList<ClientContact>);

                        if (cArgs.IndexOf("New") >= 0)
                        {
                            clientContactsGridView.IsInAddMode = true;
                            clientContactsGridView.IsInEditMode = false;
                            clientContactsGridView.DataBind();
                        }
                        else
                        {
                            clientContactsGridView.IsInEditMode = true;
                            clientContactsGridView.EditIndex = -1;
                        }
                    }
                    //LoadContacts();
                }
            }
            else
            {
                LoadContacts();
            }
        }

        private void clientContactsGridView_RowClicked(object sender, CffGridViewRowClickedEventArgs args)
        {

            populateMobileNumClientName(Convert.ToInt32(args.Row.RowIndex));           
            
            clientContactsGridView.SelectedIndex = args.Row.RowIndex;
            clientContactsGridView.IsInEditMode = false;
            DisplayClientContacts(ViewState["clientContacts"] as IList<ClientContact>);
            clientContactsGridView.Rows[args.Row.RowIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "WhiteSmoke");            
        }

        private void customerContactsGridView_RowClicked(object sender, CffGridViewRowClickedEventArgs args)
        {
            if (ddlEmailToCustomer.DataSource == null)
                populateEmailList(Convert.ToInt32(args.Row.RowIndex));

            ddlEmailToCustomer.SelectedIndex = args.Row.RowIndex;
            customerContactsGridView.Rows[args.Row.RowIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "WhiteSmoke");
        }

        private void ClearSearchTextBox()
        {
            CustomerContactsSearchTextBox.EncodedText = string.Empty;
            ClientContactsSearchTextBox.EncodedText = string.Empty;
        }


        private void LoadContacts()
        {
            LettersPH.Visible = false;
            string letterPageIndex = (this.ViewState["letterPaginationIndex"] == null) ? "ALL" :
                                        string.IsNullOrEmpty(this.ViewState["letterPaginationIndex"].ToString()) ? "ALL" : this.ViewState["letterPaginationIndex"].ToString();

            if (this.ClientAlphabeticalPagination.IsPostBack && (letterPageIndex != "ALL"))
            {
                if (this.CurrentScope() == Scope.CustomerScope)
                {
                    presenter.LoadCustomerContactsForACustomerWithCustomerNameStartWith(this.ClientAlphabeticalPagination.currentTextPage,
                                (this.Customer != null) ? this.Customer.Id : (QueryString.CustomerId != null) ? (int)QueryString.CustomerId : 0);
                }
                else if (this.CurrentScope() == Scope.ClientScope)
                {
                    presenter.LoadCustomerContactsForAClientWithCustomerNameStartWith(this.ClientAlphabeticalPagination.currentTextPage,
                                   (this.Client != null) ? this.Client.Id : (QueryString.ClientId != null) ? (int)QueryString.ClientId : 0);

                }
                else if (this.CurrentScope() == Scope.AllClientsScope)
                {
                    presenter.LoadAllClientsCustomerContactstWithCustomerNameStartWith(this.ClientAlphabeticalPagination.currentTextPage);
                }
            }
            else
            {
                if ((this.ViewState.Count > 0 && (letterPageIndex != "ALL")) || (this.ViewState.Count > 0 &&
                                                    (this.customerContactsGridView.IsInEditMode || this.customerContactsGridView.IsInUpdateMode)))
                {
                    DisplayClientContacts(ViewState["clientContacts"] as IList<ClientContact>);
                    DisplayCustomerContacts(ViewState["customerContacts"] as IList<CustomerContact>);
                }
                else
                {
                    // DM-217 Contacts are not loaded by default in All Clients scope
                    DisplayClientContacts(ViewState["clientContacts"] as IList<ClientContact>); // disable by dbb

                    if (this.IsCustomerSessionScopeSelected)
                    {
                        presenter.LoadACustomersContacts(SessionWrapper.Instance.Get.CustomerFromQueryString.Id);    //.Customer.Id);  // dbb
                        //presenter.LoadAClientContacts(SessionWrapper.Instance.Get.ClientFromQueryString.Id);    // dbb
                        presenter.LoadAClientContacts(SessionWrapper.Instance.Get.ClientFromQueryString.Id, "Select");    // dbb

                        customerContactsGridView.Visible = true;
                        clientContactsGridView.Visible = true;

                        CffPrincipal cffPrincipal = Context.User as CffPrincipal;
                        Cff.SaferTrader.Core.Repositories.ICustomerRepository repo = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateCustomerRepository();
                        ClientInformationAndAgeingBalances cinfo = repo.GetMatchedClientInfo((this.Client == null) ? 0 : this.Client.Id);

                        //MSarza [20150901]
                        //if ((cinfo.ClientInformation.AdministeredBy == "Yes") & (cffPrincipal.CffUser.ClientId > 0)) //Administered by CFF
                        if ((cinfo.ClientInformation.IsClientDebtorAdmin == false) & (cffPrincipal.CffUser.ClientId > 0))
                        {
                            LettersPH.Visible = false;
                            smsPH.Visible = false;
                        }
                        else
                        {   //administered either by both CFF and client or by Client only, display letters for management or admin role only
                            if (cffPrincipal.IsInManagementRole || cffPrincipal.IsInAdministratorRole)
                            {
                                LettersPH.Visible = true;
                                smsPH.Visible = true;

                                if (!this.Page.IsPostBack)
                                    PopulateLettersList(((CffPrincipal)Context.User).CffUser.EmployeeId, cinfo.ClientInformation.ClientNumber);
                            }
                            else
                            {
                                if (cinfo.ClientInformation.FacilityType.Contains("Factoring"))
                                { //if factoring client, no letters
                                    LettersPH.Visible = false;
                                    smsPH.Visible = false;

                                }
                                else
                                {
                                    LettersPH.Visible = true;
                                    smsPH.Visible = true;

                                    if (!this.Page.IsPostBack)
                                        PopulateLettersList(((CffPrincipal)Context.User).CffUser.EmployeeId, cinfo.ClientInformation.ClientNumber);
                                }
                            }
                        }

                        editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
                        editClientContacts = ViewState["clientContacts"] as IList<ClientContact>;       // added by dbb

                        if (editCustomerContacts == null)
                        {
                            if (this.Customer != null)
                                presenter.LoadACustomersContacts(this.Customer.Id);
                            else if (QueryString.CustomerId != null)
                                presenter.LoadACustomersContacts((int)QueryString.CustomerId);

                            editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
                        }

                        if (editClientContacts == null)   // check if clientcontacts grid is empty
                        {
                            if (this.Client != null)
                                presenter.LoadAClientContacts(this.Client.Id, "Select");   // dbb
                            else if (QueryString.ClientId != null)
                                presenter.LoadAClientContacts((int) QueryString.ClientId, "Select");  //dbb

                            editClientContacts = ViewState["clientContacts"] as IList<ClientContact>;
                        }


                        if (LettersPH.Visible == true && editCustomerContacts != null)
                        { //letters only visible to Admin, Management and Staff
                            int dfltContactIx = 0;
                            for (int ix = 0; ix < editCustomerContacts.Count; ix++)
                            {
                                if (editCustomerContacts[ix].IsDefault)
                                {
                                    customerContactsGridView.FocusedRowIndex = ix;
                                    dfltContactIx = ix;
                                }
                            }

                            if (eventTarget == "" || eventTarget == "customerContactGridView")
                                populateEmailList(dfltContactIx);

                            enableSendToOptions(); // disableSendToOptions();
                            hyperLinkFileGen.Visible = false;
                            lblDownloadHyperlink.Visible = false;
                        }
                    }
                    else if (this.IsAllClientsSessionScopeSelected)
                    {
                        CustomerContactUpdatePanel.Visible = false;
                        clientContactsGridView.Visible = true;
                        presenter.LoadAllClientsContacts();
                        //presenter.LoadAllCustomersContacts();   // dbb
                    }
                    else
                    {
                        customerContactsGridView.Visible = true;
                        clientContactsGridView.Visible = true; // false; modified by dbb

                        if (this.Client != null)
                        {
                            presenter.LoadAClientContacts(this.Client.Id, "Select");   //dbb
                            presenter.LoadAllCustomersContactsForAClient(this.Client.Id);
                        }
                        else if (QueryString.ClientId != null)
                        {
                            presenter.LoadAClientContacts((int)QueryString.ClientId, "Select");    //dbb
                            presenter.LoadAllCustomersContactsForAClient((int)QueryString.ClientId);
                        }
                    }
                }
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
                    if (!string.IsNullOrEmpty(ctl))
                    {
                        Control c = page.FindControl(ctl);
                        if (c is System.Web.UI.WebControls.Button)
                        {
                            control = c;
                            break;
                        }
                    }
                }
            }
            return control;
        }

        // MSarza [20152015]: Modified to reflect new business rule on Client Admin Type
        private void PopulateLettersList(int userID, int clientID)  
        {
            string rootPath = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePath"];
            string dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            customerLetter.InitialiseCustomerLetter(userID, clientID, rootPath, dbConnStr);
            List<String> fileList = new List<String>();
            fileList = customerLetter.CustomerLettersMenuList;

            ddlLetters.SelectedIndexChanged += new EventHandler(ddlLetters_SelectedIndexChanged);
            ddlLetters.DataSource = fileList;
            ddlLetters.DataBind();
        }

        private void populateMobileNumClientName(int rowIdx)
        {
            try
            {
                IList<ClientContact> clientContacts = ViewState["clientContacts"] as IList<ClientContact>;

                int rIdx = rowIdx;
                mobileNum.Text = clientContacts[rIdx].MobilePhone;
                mobileNum.Text = Regex.Replace(mobileNum.Text, @"\s+", "");
                custContactNameMobileLiteral.Text = clientContacts[rIdx].FullName;
                custFNameLiteral.Text = clientContacts[rIdx].FirstName;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.StackTrace.ToString());
            }
        }

        private void populateEmailList(int rowIndx)
        {
            try
            {
                IList<CustomerContact> nContacts = ViewState["customerContacts"] as IList<CustomerContact>;
                List<String> emailList = new List<String>();
                List<String> faxList = new List<String>();

                for (int ix = 0; ix < nContacts.Count; ix++)
                {
                    emailList.Add(nContacts[ix].Email);
                    faxList.Add(nContacts[ix].Fax);
                }

                int rIdx = rowIndx; // customerContactsGridView.FocusedRowIndex;

                //if ((eventTarget == "customerContactsGridView") || (!this.Page.IsPostBack)) //((customerContactsGridView.IsCallback) || (!this.Page.IsPostBack))
                //{
                //    //rIdx = customerContactsGridView.FocusedRowIndex;   //btnSendLetter_Click
                //    //ddlEmailToCustomer.SelectedIndex = rIdx;
                //    //ddlFaxToCustomer.SelectedIndex = rIdx;
                //    rIdx = ddlEmailToCustomer.SelectedIndex;
                //}
                //else if ((ddlEmailToCustomer.SelectedIndex >= 0) && (ddlEmailToCustomer.Enabled))
                //{
                //    rIdx = ddlEmailToCustomer.SelectedIndex;
                //}

                ddlEmailToCustomer.DataSource = emailList;
                ddlEmailToCustomer.DataBind();

                ddlFaxToCustomer.DataSource = faxList;
                ddlFaxToCustomer.DataBind();

                ddlEmailToCustomer.SelectedIndex = rIdx;
                ddlFaxToCustomer.SelectedIndex = rIdx;
                custContactNameLiteral.Text = nContacts[rIdx].FullName;
                custFNameLiteral.Text = nContacts[rIdx].FirstName;
                mobileNum.Text = nContacts[rIdx].MobilePhone;
                custContactNameMobileLiteral.Text = nContacts[rIdx].FullName;
                if (!Page.IsPostBack) dateButton.Text = DateTime.Now.ToString("dd/MMM/yy");   // dbb
            }
            catch (Exception ex)
            {
                Debug.Write(ex.StackTrace.ToString());
            }

        }

        private string cleanupFileName(string xName)
        {
            xName = xName.Replace("MTH", "Month");
            xName = xName.Replace("_", " ");
            return xName;
        }

        private void clearStatusTexts()
        {
            sendStatusLiteral.Visible = false;
            hyperLinkFileGen.Visible = false;
            lblDownloadHyperlink.Visible = false;
        }

        private void disableSendToOptions()
        {
            clearStatusTexts();

            rbtnEmailToClient.Enabled = false;
            rbtnEmailToCustomer.Enabled = false;
            rbtnEmailToCollector.Enabled = false;
            rbtnEmailClientCust.Enabled = false;
            chkboxEditEmail.Enabled = false;
            ddlEmailToCustomer.Enabled = false;
            //btnEmailBankDetails.Enabled = false;
        }

        private void enableSendToOptions()
        {
            clearStatusTexts();

            rbtnEmailToClient.Enabled = true;
            rbtnEmailToCustomer.Enabled = true;
            rbtnEmailToCollector.Enabled = true;
            rbtnEmailClientCust.Enabled = true;
            chkboxEditEmail.Enabled = true;
            rbtnEmailToCustomer.Checked = true;
            chkboxEditEmail.Checked = true;
            ddlEmailToCustomer.Enabled = true;
            //btnEmailBankDetails.Enabled = true;
        }

        void ddlLetters_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();      //mkjREF
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);

            presenter.InitializeForScope(SessionWrapper.Instance.Get.Scope);

            clientContactsGridView.ResetPaginationAndFocus();
            customerContactsGridView.ResetPaginationAndFocus();
            ClearSearchTextBox();
            LoadContacts();
        }

        protected void ClientContactsSearchButton_Click(object sender, ImageClickEventArgs e)
        {
            Scope xScope = this.CurrentScope();
            if (xScope == Scope.AllClientsScope)
                presenter.SearchAllClientsContact(ClientContactsSearchTextBox.EncodedText.Trim());
            else
                presenter.SearchAClientsContact(ClientContactsSearchTextBox.EncodedText.Trim(), SessionWrapper.Instance.Get.ClientFromQueryString.Id);
        }


        protected void CustomerContactsSearchButton_Click(object sender, ImageClickEventArgs e)
        {
            Scope xScope = this.CurrentScope();
            switch (xScope)
            {
                case Scope.CustomerScope:
                    presenter.SearchCustomerContactForACustomer(CustomerContactsSearchTextBox.EncodedText.Trim(), SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
                    break;

                case Scope.AllClientsScope:
                    presenter.SearchAllClientsCustomerContact(CustomerContactsSearchTextBox.EncodedText.Trim());
                    break;

                default:
                    presenter.SearchAClientsCustomerContact(CustomerContactsSearchTextBox.EncodedText.Trim(), SessionWrapper.Instance.Get.ClientFromQueryString.Id);
                    break;
            }
        }

        protected void ClientContactsGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            ClientContact record = (ClientContact)clientContactsGridView.GetRow(parameter.RowIndex);
            RedirectionParameter redirectionParameter = new RedirectionParameter(parameter.FieldName,
                                                                                 record.ClientNumber);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.Get.Scope);
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

        protected void CustomerContactsGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            CustomerContact record = (CustomerContact)customerContactsGridView.GetRow(parameter.RowIndex);
            RedirectionParameter redirectionParameter = new RedirectionParameter(parameter.FieldName,
                                                                                 record.ClientNumber, record.CustomerId);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.Get.Scope);
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

        private void CustomerContactPageIndexUpdate(object sender, EventArgs eventArgs)
        {
            AlphabeticalPaginationEventArgs args = eventArgs as AlphabeticalPaginationEventArgs;
            ArgumentChecker.ThrowIfNull(args, "AlphabeticalPaginationEventArgs is null");

            this.ClientAlphabeticalPagination.currentTextPage = args.Letter;
            this.ClientAlphabeticalPagination.Page.EnableViewState = true;
            this.PaginationIndex.Value = args.Letter;
            this.ViewState.Add("letterPaginationIndex", this.PaginationIndex.Value);

            if (args.Letter == "ALL")
            {
                LoadContacts();
            }
            else
            {
                if (SessionWrapper.Instance.Get.Scope == Scope.CustomerScope)
                {
                    presenter.LoadCustomerContactsForACustomerWithCustomerNameStartWith(args.Letter, SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
                }
                else if (SessionWrapper.Instance.Get.Scope == Scope.ClientScope)
                {
                    presenter.LoadCustomerContactsForAClientWithCustomerNameStartWith(args.Letter, SessionWrapper.Instance.Get.ClientFromQueryString.Id);

                }
                else if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope)
                {
                    presenter.LoadAllClientsCustomerContactstWithCustomerNameStartWith(args.Letter);
                }
            }

            this.SaveViewState();
        }

        private void ClientContactPageIndexUpdate(object sender, EventArgs e)
        {
            this.ClientAlphabeticalPagination.currentTextPage = ((AlphabeticalPaginationEventArgs)e).Letter;
            if (((AlphabeticalPaginationEventArgs)e).Letter == "ALL")
            {
                LoadContacts();
            }
            else
                presenter.LoadClientsContactsWithClientNameStartWith(((AlphabeticalPaginationEventArgs)e).Letter);
        }

        public CffPrincipal CurrentPrincipal
        {
            get { return Context.User as CffPrincipal; }
        }

        protected void rbtnWordScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnWordScreen.Checked)
            {
                btnSendLetter.Text = "Generate Letter";
                disableSendToOptions();
            }
        }

        protected void rbtnPDFFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPDFFile.Checked)
            {
                btnSendLetter.Text = "Generate Letter";
                disableSendToOptions();
            }
        }

        protected void rbtnWordPrinter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnWordPrinter.Checked)
            {
                btnSendLetter.Text = "Generate Document";
                disableSendToOptions();
                //System.Diagnostics.Process.Start("Notepad.exe", Server.MapPath("/reportsfile/test.txt")); //note:invoking this on client side runs as network service
            }
        }

        protected void rbtnSendFax_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnSendFax.Checked)
            {
                btnSendLetter.Text = "Send Communication";
                ddlFaxToCustomer.SelectedIndex = customerContactsGridView.FocusedRowIndex;
                disableSendToOptions();
            }
        }

        protected void rbtnEmailWord_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEmailWord.Checked)
            {
                btnSendLetter.Text = "Send Letter";
                reportStatementLiteral.Visible = false;
                hlReportStatement.Visible = false;
                lblReportStatement.Visible = false;
                enableSendToOptions();
            }
            else
            {
                disableSendToOptions();
            }
        }

        protected void rbtnEmailFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEmailFormat.Checked)
            {
                btnSendLetter.Text = "Send Letter";
                reportStatementLiteral.Visible = false;
                hlReportStatement.Visible = false;
                lblReportStatement.Visible = false;
                enableSendToOptions();
            }
            else
            {
                disableSendToOptions();
            }
        }

        protected void rbtnEmailPDF_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEmailFormat.Checked)
            {
                btnSendLetter.Text = "Send Letter";
                reportStatementLiteral.Visible = false;
                hlReportStatement.Visible = false;
                lblReportStatement.Visible = false;
                enableSendToOptions();
            }
            else
            {
                disableSendToOptions();
            }
        }

        protected void btnSendLetter_Click(object sender, EventArgs e)
        {
            int userID = ((CffPrincipal)Context.User).CffUser.EmployeeId;
            int clientID = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            int custID = SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
            string letterName = ddlLetters.Text;
            string letterOutputType = "";
            string htmlOutServerDirectory = "";
            bool letterDetailsNotToBeAttachedButInBody = false;      // MSarza
            bool isHtmlEmail = false;             // MSarza
            string strMailBodyContents = "";            // MSarza

            //// As per old definition
            //if ((rbtnPDFFile.Checked) || (rbtnEmailPDF.Checked)) { letterOutputType = "pdf"; }  // On Contacts.aspx.cs: Email PDF and To PdDF File 
            //else letterOutputType = "word";
            //As per VB application
            if (rbtnPDFFile.Checked) letterOutputType = "pdf";
            else if (rbtnEmailPDF.Checked) letterOutputType = "emailpdf";
            else if (rbtnWordPrinter.Checked) letterOutputType = "word";
            else if (rbtnEmailWord.Checked) letterOutputType = "emailword";
            else letterOutputType = "word";

            string libToUseToCreateLetters = System.Configuration.ConfigurationManager.AppSettings["LibToUseToCreateCustomerLetters"];
            string libToUseToGeneratePdfLetters = System.Configuration.ConfigurationManager.AppSettings["LibToUseToGeneratePdfCustomerLetters"];
            string lettersLibDataUpdateReferenceTagType = System.Configuration.ConfigurationManager.AppSettings["LettersLibDataUpdateReferenceTagType"];
            string reportsDirectory = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
            string rootPath = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePath"];
            string dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            //string custTemplatesDirectory = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathCustomer"];
            //string templatesDirectory = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePath"];

            //string reportsBackupDirectory = System.Configuration.ConfigurationManager.AppSettings["ReportsBackupFolder"];
            //string htmlOutServerDirectory = Server.MapPath("/" + System.Configuration.ConfigurationManager.AppSettings["IISReportsFolder"] + "/");

            //bool isClientUser = ((CffPrincipal)Context.User).IsInClientRole;

            //string clientName = SessionWrapper.Instance.Get.ClientFromQueryString.Name;
            //int employeeID = ((CffPrincipal)Context.User).CffUser.EmployeeId; //this employee user id

            //string custName = SessionWrapper.Instance.Get.CustomerFromQueryString.Name;
            //int custNumber = SessionWrapper.Instance.Get.CustomerFromQueryString.Number;
            //DateTime dtNow = DateTime.Now;
            //int yrMth = Convert.ToInt32(dtNow.Year.ToString() + dtNow.Month.ToString().PadLeft(2, '0'));
            //short facilityType = Convert.ToInt16(SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType);

            string attnH;
            string dearH;
            if (chkboxAttn.Checked)
            {

                if (ddlEmailToCustomer.Items.Count == 1)
                    attnH = "Attn: " + custContactNameLiteral.Text;
                else
                {
                    editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
                    if (rbtnEmailPDF.Checked || rbtnEmailWord.Checked)
                    { //get value from whatever is in the drop down selected index
                        attnH = "Attn: " + editCustomerContacts[ddlEmailToCustomer.SelectedIndex].FullName;
                    }
                    else
                    { //get value from datagrid
                        attnH = "Attn: " + editCustomerContacts[customerContactsGridView.FocusedRowIndex].FullName;
                    }
                }
            }
            else { attnH = ""; }

            if (chkboxDear.Checked)
            {
                if (ddlEmailToCustomer.Items.Count == 1)
                    dearH = "Dear " + custFNameLiteral.Text;
                else
                {
                    editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
                    if (rbtnEmailPDF.Checked || rbtnEmailWord.Checked)
                    { //get value from whatever is in the drop down selected index
                        dearH = "Dear " + editCustomerContacts[ddlEmailToCustomer.SelectedIndex].FirstName;
                    }
                    else
                    { //get value from datagrid
                        dearH = "Dear " + editCustomerContacts[customerContactsGridView.FocusedRowIndex].FirstName;
                    }
                }
            }
            else { dearH = "Dear Sir/Madam"; }

            string generateLetterResult;

            string sendByEmailTo;
            if (rbtnEmailToCustomer.Checked)
                sendByEmailTo = ddlEmailToCustomer.Text;
            else if (rbtnEmailClientCust.Checked)
                sendByEmailTo = ddlEmailToCustomer.Text + "|includeCltEmail|";
            else if (rbtnEmailToClient.Checked)
                sendByEmailTo = "|includeCltEmail|";
            else
                sendByEmailTo = "";

            #region page output variables
            string outputFileName = "";
            string outputFileNameNoExt = "";
            string outputFullFilePath = "";
            DateTime dtNow = DateTime.Now;
            string emailSignature = "[Not supplied]";
            string emailEntity = "[Not set]";
            #endregion

            if (libToUseToCreateLetters.ToLower().Contains("openxml"))
            {
                customerLetter.InitialiseCustomerLetter(userID, clientID, rootPath, dbConnStr);

                generateLetterResult = customerLetter.GenerateLetter(userID, custID, letterName, letterOutputType, reportsDirectory,
                                                        attnHeader: attnH,
                                                        dearHeader: dearH,
                                                        libraryToUse: libToUseToCreateLetters,
                                                        insertionTagType: lettersLibDataUpdateReferenceTagType,
                                                        pdfLibToUse: libToUseToGeneratePdfLetters,
                                                        htmlOutputDirectory: htmlOutServerDirectory,
                                                        sendByEmailTo: sendByEmailTo
                                                        );

                outputFileName = customerLetter.OutputFileName;
                outputFileNameNoExt = customerLetter.OutputFileNameNoExt;
                outputFullFilePath = customerLetter.LetterOutputFullPath;
                dtNow = customerLetter.DateNowLetterReference;
                emailSignature = customerLetter.UserSignature;
                emailEntity = customerLetter.UserOnBehalf + customerLetter.UserLegalEntity;

                sendByEmailTo = customerLetter.EmailCopyTo;

                //if (customerLetter.LetterBodyInHtmlFormat.ToLower().Contains("fault"))
                //{
                //    strMailHtmlBody = "";
                //    htmlEmail = false;
                //}
                //else
                //{
                //    strMailHtmlBody = customerLetter.LetterBodyInHtmlFormat;
                //    htmlEmail = true;
                //}
                strMailBodyContents = customerLetter.LetterBodyInFormattedText;
                isHtmlEmail = false;
            }
            else
            {
                // write implementation here
                generateLetterResult = "Error: Not implemented.";
            }

            try
            {

                if (generateLetterResult.ToLower().Contains("error"))
                {
                    hyperLinkFileGen.Visible = true;
                    sendStatusLiteral.Visible = true;
                    hyperLinkFileGen.Text = generateLetterResult.Replace("Fault", "Error");
                    sendStatusLiteral.Text = generateLetterResult.Replace("Fault", "Error");
                    return;
                }
                else
                {

                    /* VB app behavior as at 20160916
                     * Letters whose contents are to be in the email body if option rbtnEmailWord.Checked = true
                     *      client action
                     *      credit to come
                     *      ael
                     *      credit consultants
                     * Letters requiring permanent notes
                     *      7 Day
                     *      Default
                     *      AEL
                     *      Credit Consultants" 
                     */



                    //20160916: aligning with VB app behaviour
                    if ((letterOutputType.ToLower() == "emailword") && (letterName.ToLower().Contains("client action") || letterName.ToLower().Contains("credit to come") ||
                    letterName.ToLower().Contains("ael") || letterName.ToLower().Contains("credit consultants")))
                    {
                        sendStatusLiteral.Visible = false;
                        hyperLinkFileGen.Visible = false;
                        lblDownloadHyperlink.Visible = false;

                        sendStatusLiteral.Text = "";
                        hyperLinkFileGen.Text = "";
                        hyperLinkFileGen.NavigateUrl = "";
                        generateLetterResult = "";

                        letterDetailsNotToBeAttachedButInBody = true;
                        //outputFileName = "";    // prevent attachment as in vb app

                    }
                    else
                    {
                        sendStatusLiteral.Visible = true;
                        hyperLinkFileGen.Visible = true;
                        lblDownloadHyperlink.Visible = true;

                        sendStatusLiteral.Text = "Generated: ";
                        hyperLinkFileGen.Text = outputFileName;
                        hyperLinkFileGen.NavigateUrl = "downloading.aspx?ViewID=" + QueryString.ViewIDValue + "&file=" + outputFullFilePath;
                        generateLetterResult = outputFileName;
                    }


                    #region raise appropriate popup
                    if (rbtnWordScreen.Checked)   // not in front end for generating letters
                    {
                        sendDocumentToScreen(outputFileNameNoExt);
                        DocumentMailer myDoc = null;
                        myDoc = new DocumentMailer(0, (short)ddlLetters.SelectedIndex, dtNow, clientID, custID, userID,
                                                        generateLetterResult, outputFileNameNoExt,
                                                        "", "", "", "", QueryString.ViewIDValue, false);
                        //update current notes when letter is generated 
                        Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(myDoc);
                        System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterGenerated);
                        thrMEvt.Start();

                        //Response.Redirect("downloading.aspx?file=" + rptFolder + strDummy[1].Trim() + ".doc");
                        //Server.Transfer("downloading.aspx?file=" + rptFolder + strDummy[1].Trim() + ".doc");
                    }

                    else if (rbtnSendFax.Checked)  // not in front end for generating letters
                    { //send as fax document
                        sendDocumentToFax(dtNow, clientID, custID, userID, customerLetter.OutputFileNameNoExt.Trim());
                    }

                    else if (rbtnEmailPDF.Checked)      // Letters front end label: Email PDF
                    {  //email PDF as attachment
                        string fileToAttch = outputFileName;
                        if (letterDetailsNotToBeAttachedButInBody)
                            fileToAttch = letterName;

                        //MSarza[20160929]: Added in order to record in current notes that file had been generated even if
                        //                      user do not proceed emailing the letter                  
                        DocumentMailer myDoc = null;
                        myDoc = new DocumentMailer(0, (short)ddlLetters.SelectedIndex, dtNow, clientID, custID, userID,
                                                        fileToAttch, "", "", "", "", "", QueryString.ViewIDValue, letterDetailsNotToBeAttachedButInBody);
                        Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(myDoc);
                        System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterGenerated);
                        thrMEvt.Start();
                        //--

                        if (chkboxEditEmail.Checked) { sendDocumentToEmail(0, dtNow, clientID, custID, userID, fileToAttch, "", "", chkboxEditEmail.Checked, emailSignature, emailEntity, isBodyHtml: isHtmlEmail, noAttachmentDetailsInBody: letterDetailsNotToBeAttachedButInBody); }
                        else { sendDocumentToEmail(0, dtNow, clientID, custID, userID, fileToAttch, "", "", chkboxEditEmail.Checked, "", "", isBodyHtml: isHtmlEmail, noAttachmentDetailsInBody: letterDetailsNotToBeAttachedButInBody); }
                    }

                    else if (rbtnWordPrinter.Checked)   // Letters front end label: Word-Screen
                    {
                        sendDocumentToPrinter(outputFullFilePath);

                        DocumentMailer myDoc = null;
                        myDoc = new DocumentMailer(0, (short)ddlLetters.SelectedIndex, dtNow, clientID, custID, userID,
                                                        generateLetterResult, outputFileNameNoExt,
                                                        "", "", "", "", QueryString.ViewIDValue, false);
                        ////update current notes when letter is generated 
                        Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(myDoc);
                        //MSarza: Letter is actually only generated and not printed and radio button front end label is Word - Screen, hence,
                        //              updated to mEvt.updateLetterGenerated
                        //System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterPrinted);  
                        System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterGenerated);
                        thrMEvt.Start();
                    }

                    else if (rbtnEmailWord.Checked)      // Letters front end label: Email Word
                    { //email word as attachment 
                        string fileToAttch = outputFileName;
                        if (letterDetailsNotToBeAttachedButInBody)
                            fileToAttch = letterName;

                        //MSarza[20160929]: Added in order to record in current notes that file had been generated even if
                        //                      user do not proceed emailing the letter                  
                        DocumentMailer myDoc = null;
                        myDoc = new DocumentMailer(0, (short)ddlLetters.SelectedIndex, dtNow, clientID, custID, userID,
                                                        fileToAttch, "", "", "", "", "", QueryString.ViewIDValue, letterDetailsNotToBeAttachedButInBody);
                        Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(myDoc);
                        System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterGenerated);
                        thrMEvt.Start();
                        //--

                        if (chkboxEditEmail.Checked) { sendDocumentToEmail(0, dtNow, clientID, custID, userID, fileToAttch, "", strMailBodyContents, chkboxEditEmail.Checked, emailSignature, emailEntity, isBodyHtml: isHtmlEmail, noAttachmentDetailsInBody: letterDetailsNotToBeAttachedButInBody); }
                        else { sendDocumentToEmail(0, dtNow, clientID, custID, userID, fileToAttch, "", strMailBodyContents, chkboxEditEmail.Checked, "", "", isBodyHtml: isHtmlEmail, noAttachmentDetailsInBody: letterDetailsNotToBeAttachedButInBody); }
                    }

                    else if (rbtnPDFFile.Checked)  // MSarza[20160928] : Added this missing condition for checkbox front end labelled as To PDF File
                    {


                    }
                    #endregion
                }
            }
            catch (Exception exc)
            {
                //Could be here because of a response.redirect!!! 
                hyperLinkFileGen.Visible = true;
                sendStatusLiteral.Visible = true;
                hyperLinkFileGen.Text = "Error: " + exc.Message;
                sendStatusLiteral.Text = "Error: " + exc.Message;
            }
        }


        protected void btnEmailBankDetails_Click(object sender, EventArgs e)
        {

            int clientID = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            //int clientNum = SessionWrapper.Instance.Get.ClientFromQueryString.Number;
            //int customerID = SessionWrapper.Instance.Get.CustomerFromQueryString.Number;

            //Retriveing user signature (based on IOutput.cs code)
            int employeeID = ((CffPrincipal)Context.User).CffUser.EmployeeId;
            stpCaller stpC = new stpCaller();
            List<object> arrParam = new List<object>();
            arrParam.Clear();
            arrParam.Add(employeeID);
            arrParam.Add("ALL");
            string userSignature = "[Signature not supplied]";

            using (System.Data.DataSet userDetailsDS = stpC.executeSPDataSet(arrParam, stpCaller.stpType.GetUserDetails))
            {
                if ((userDetailsDS != null) && (userDetailsDS.Tables[0].Rows.Count > 0))
                {
                    userSignature = Convert.ToString(userDetailsDS.Tables[0].Rows[0]["Signature"]);
                }
            }


            string fullAcct = "01 0242 0146299 000";
            string acctName = "Account Name:  " + "\t" + "Cashflow Funding Limited";
            string acctNum = "Account Number:" + "\t" + fullAcct;
            string spacer = "\t" + "----------------------------------------";
            string bank = "\t" + "- Bank (ANZ):" + "\t\t" + "01";
            string branch = "\t" + "- Branch (Penrose):" + "\t" + "0242";
            string acct = "\t" + "- Account:" + "\t\t" + "0146299";
            string suffix = "\t" + "- Suffix:  " + "\t\t" + "000";

            string swift = "Swift(Code):" + "\t\t" + "ANZBNZ22" + "\r\n";
            string rfrnce = "Reference:" + "\t\t" + SessionWrapper.Instance.Get.ClientFromQueryString.Number + "/" + SessionWrapper.Instance.Get.CustomerFromQueryString.Number;

            string disclaimer = "Disclaimer: This email is for the use of the intended recipient(s) only. " +
                                "If you have received this in error, please reply to this message immediately and then delete it. " +
                                "If you are not the intended recipient, you may not use, keep, disclose, copy or distribute any information contained in this email.";

            arrParam.Clear();
            arrParam.Add("ClientCffDetails");
            arrParam.Add(clientID);
            arrParam.Add(0);

            string collectionsBankAcct = "";
            string legalEntity = "";

            if (((CffPrincipal)Context.User).IsInClientRole)
                legalEntity = "On behalf of: ";

            using (System.Data.DataSet theDS = stpC.executeSPDataSet(arrParam, stpCaller.stpType.GetClientSelectedData))
            {
                if ((theDS != null) && (theDS.Tables[0] != null) && (theDS.Tables[0].Rows.Count > 0))
                {
                    collectionsBankAcct = theDS.Tables[0].Rows[0]["CollectionsBankAccount"].ToString();
                    legalEntity += theDS.Tables[0].Rows[0]["CffLegalEntity"].ToString();
                }
            }

            if (fullAcct != collectionsBankAcct)
            {
                string[] splitAccount = collectionsBankAcct.Split(' ');
                acctName = "Account Name:  " + "\t" + "Cashflow Funding Limited";
                acctNum = "Account Number:" + "\t" + collectionsBankAcct;
                bank = "\t" + "- Bank:" + "\t\t" + splitAccount[0];
                branch = "\t" + "- Branch:  " + "\t" + splitAccount[1];
                acct = "\t" + "- Account:" + "\t" + splitAccount[2];
                suffix = "\t" + "- Suffix:  " + "\t" + splitAccount[3];
                swift = "";
            }


            string strSubject = "Banking Details - Cashflow Funding Limited - " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
            string mailBody = "Thank your for requesting our banking details, please find them below.";
            mailBody += "\r\n\r\n";
            mailBody += "Please include " + SessionWrapper.Instance.Get.ClientFromQueryString.Number + "/" +
                        SessionWrapper.Instance.Get.CustomerFromQueryString.Number +
                        " as reference on each payment for your account with "
                        + SessionWrapper.Instance.Get.ClientFromQueryString.Name +
                        " as this will allow us to uniquely identify your payment and credit the correct account.";
            mailBody += "\r\n\r\n";
            mailBody += acctName;
            mailBody += "\r\n";
            mailBody += acctNum;
            mailBody += "\r\n";
            mailBody += spacer;
            mailBody += "\r\n";
            mailBody += bank;
            mailBody += "\r\n";
            mailBody += branch;
            mailBody += "\r\n";
            mailBody += acct;
            mailBody += "\r\n";
            mailBody += suffix;
            mailBody += "\r\n";
            mailBody += spacer;
            mailBody += "\r\n\r\n";
            mailBody += swift;
            mailBody += rfrnce;
            mailBody += "\r\n\r\n";
            mailBody += "Regards,";
            mailBody += "\r\n\r\n\r\n";
            mailBody += userSignature;
            mailBody += "\r\n";
            mailBody += legalEntity;
            mailBody += "\r\n";
            mailBody += "Ph:  +64 9 579 4204";
            mailBody += "\r\n";
            mailBody += "Fax: +64 9 525 3598";
            mailBody += "\r\n\r\n\r\n\r\n";
            mailBody += "------------------------------";
            mailBody += "\r\n";
            mailBody += disclaimer;
                
            sendDocumentToEmail(2, DateTime.Now, 0, 0, 0, "", strSubject, mailBody, true, "", "");
        }

        protected void btnGenerateStatement_Click(object sender, EventArgs e) 
        {
            int clientID = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            int custID = SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
            int userID = ((CffPrincipal)Context.User).CffUser.EmployeeId;
            string letterName = "StatementReport";

            string libToUseToCreateLetters = System.Configuration.ConfigurationManager.AppSettings["LibToUseToCreateCustomerLetters"];
            string libToUseToGeneratePdfLetters = System.Configuration.ConfigurationManager.AppSettings["LibToUseToGeneratePdfCustomerLetters"];
            string lettersLibDataUpdateReferenceTagType = System.Configuration.ConfigurationManager.AppSettings["LettersLibDataUpdateReferenceTagType"];
            string rootPath = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePath"];
            string dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;


            if (libToUseToCreateLetters.ToLower() == "interopold")  
            {
                Cff.SaferTrader.Core.Repositories.IReportRepository sttmntRRepo = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateReportRepository();
                Cff.SaferTrader.Core.Reports.StatementReport stRpt = sttmntRRepo.LoadStatementReport(clientID, custID, new Date(System.DateTime.Now));
                if (stRpt == null)
                {
                    reportStatementLiteral.Text = "Nothing fetched from the database; no statement generated.";
                    reportStatementLiteral.Visible = true;
                    return;
                }
                IList<SaferTrader.Core.Reports.StatementReportRecord> stRptRecs = stRpt.Records;


                LetterDetails lDetails = new LetterDetails(stRptRecs);
                lDetails.ClientId = stRpt.PurchaserDetails.Clientid;
                lDetails.ClientName = stRpt.PurchaserDetails.ClientName;
                lDetails.CollectionsBankAccount = stRpt.PurchaserDetails.CollectionsBankAccount;

                lDetails.CustName = SessionWrapper.Instance.Get.CustomerFromQueryString.Name;
                lDetails.CustNumber = SessionWrapper.Instance.Get.CustomerFromQueryString.Number;
                //todo: lDetails.CustContactID = ;

                lDetails.CustomerAddress1 = stRpt.PurchaserDetails.Address.AddressOne;
                lDetails.CustomerAddress2 = stRpt.PurchaserDetails.Address.AddressTwo;
                lDetails.CustomerAddress3 = stRpt.PurchaserDetails.Address.AddressThree;
                lDetails.CustomerAddress4 = stRpt.PurchaserDetails.Address.AddressFour;

                lDetails.DateAsAt = System.DateTime.Now;
                lDetails.Balance = stRpt.AgeingBalances.Balance;
                lDetails.Month1 = stRpt.AgeingBalances.OneMonthAgeing;
                lDetails.Month2 = stRpt.AgeingBalances.TwoMonthAgeing;
                lDetails.Month3 = stRpt.AgeingBalances.ThreeMonthPlusAgeing;
                lDetails.Current = stRpt.AgeingBalances.Current;

                IOutput myOutput = new IOutput(lDetails);
                string strDoc = myOutput.generateReportStatement("Word");
                reportStatementLiteral.Text = "Generated: " + strDoc;
                reportStatementLiteral.Visible = true;

                string[] strDummy = strDoc.Split(':');
                if (strDummy[0].Contains("OK"))
                {
                    reportStatementLiteral.Visible = false;

                    string theFileName = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"] + strDummy[1].Trim() + ".doc";
                    if (System.IO.File.Exists(theFileName) == false)
                    {
                        theFileName = "\\reportfiles\\" + theFileName;
                        theFileName = Server.MapPath(theFileName);
                    }
                    else
                    {
                        try
                        { //copy to backup
                            string rptBackupFolder = System.Configuration.ConfigurationManager.AppSettings["ReportsBackupFolder"];
                            Cff.SaferTrader.Core.Letters.MiscDocEvents miscEvt = new Cff.SaferTrader.Core.Letters.MiscDocEvents(theFileName, rptBackupFolder + strDummy[1].Trim() + ".doc");
                            System.Threading.Thread thrMisc = new System.Threading.Thread(miscEvt.copyToBackup);
                            thrMisc.Start();
                        }
                        catch (Exception) { }
                    }

                    reportStatementLiteral.Text = "Generated: " + theFileName;
                    reportStatementLiteral.Visible = true;
                    hlReportStatement.Text = strDummy[1].Trim() + ".doc";
                    hlReportStatement.NavigateUrl = "downloading.aspx?file=" + theFileName;
                    hlReportStatement.Visible = true;
                    lblReportStatement.Visible = true;
                }
                else
                {
                    reportStatementLiteral.Text = strDoc;
                    reportStatementLiteral.Visible = true;
                }
            }
            else
            {
                string reportsDirectory = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
                string letterOutputType;
                if ((rbtnPDFFile.Checked) || (rbtnEmailPDF.Checked)) { letterOutputType = "pdf"; }  // On Contacts.aspx.cs: Email PDF and To PdDF File 
                else letterOutputType = "word";

                string generateLetterResult;

                #region page output variables
                string outputFileName = "";
                string outputFileNameNoExt = "";
                string outputFullFilePath = "";
                DateTime dtNow = DateTime.Now;
                string emailSignature = "[Not supplied]";
                string emailEntity = "[Not set]";
                #endregion

                if (libToUseToCreateLetters.ToLower() == "openxml")           
                {
                    customerLetter.InitialiseCustomerLetter(userID, clientID, rootPath, dbConnStr);

                    generateLetterResult = customerLetter.GenerateLetter(userID, custID, letterName, letterOutputType, reportsDirectory,
                                                                            libraryToUse: libToUseToCreateLetters,
                                                                            insertionTagType: lettersLibDataUpdateReferenceTagType,
                                                                            pdfLibToUse: libToUseToGeneratePdfLetters
                                                            );

                    outputFileName = customerLetter.OutputFileName;
                    outputFileNameNoExt = customerLetter.OutputFileNameNoExt;
                    outputFullFilePath = customerLetter.LetterOutputFullPath;
                    dtNow = customerLetter.DateNowLetterReference;
                    emailSignature = customerLetter.UserSignature;
                    emailEntity = customerLetter.UserOnBehalf + customerLetter.UserLegalEntity;

                }
                else
                {
                    // write implementation here
                    generateLetterResult = "Error: Not implemented.";
                }

                reportStatementLiteral.Visible = false;
                if (!generateLetterResult.ToLower().Contains("error") && File.Exists(outputFullFilePath))
                {

                    try
                    {
                        // Backup to be done externally
                        //string rptBackupFolder = System.Configuration.ConfigurationManager.AppSettings["ReportsBackupFolder"];
                        //Cff.SaferTrader.Core.Letters.MiscDocEvents miscEvt = new Cff.SaferTrader.Core.Letters.MiscDocEvents(reportsDirectory + fileGenerated, rptBackupFolder + fileGenerated);
                        //System.Threading.Thread thrMisc = new System.Threading.Thread(miscEvt.copyToBackup);
                        //thrMisc.Start();

                        //hlReportStatement.Text = fileGenerated;
                        hlReportStatement.Text = outputFileName;
                        hlReportStatement.NavigateUrl = "downloading.aspx?file=" + outputFullFilePath;
                        hlReportStatement.Visible = true;
                        lblReportStatement.Visible = true;

                        reportStatementLiteral.Text = "Generated: " + outputFileName;
                        reportStatementLiteral.Visible = true;

                        #region raise appropriate popup
                        if (rbtnWordScreen.Checked)
                        {
                            sendDocumentToScreen(outputFileNameNoExt);
                            DocumentMailer myDoc = null;
                            myDoc = new DocumentMailer(0, (short)ddlLetters.SelectedIndex, dtNow, clientID, custID, userID,
                                                            generateLetterResult, outputFileNameNoExt,
                                                            "", "", "", "", QueryString.ViewIDValue, false);
                            //update current notes when letter is generated 
                            Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(myDoc);
                            System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterGenerated);
                            thrMEvt.Start();

                            //Response.Redirect("downloading.aspx?file=" + rptFolder + strDummy[1].Trim() + ".doc");
                            //Server.Transfer("downloading.aspx?file=" + rptFolder + strDummy[1].Trim() + ".doc");
                        }

                        else if (rbtnWordPrinter.Checked)
                        {
                            sendDocumentToPrinter(outputFullFilePath);

                            DocumentMailer myDoc = null;
                            myDoc = new DocumentMailer(0, (short)ddlLetters.SelectedIndex, dtNow, clientID, custID, userID,
                                                            generateLetterResult, outputFileNameNoExt,
                                                            "", "", "", "", QueryString.ViewIDValue, false);
                            //update current notes when letter is generated 
                            Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(myDoc);
                            System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterPrinted);
                            thrMEvt.Start();
                        }

                        else if (rbtnEmailWord.Checked)
                        { //email word as attachment 
                            if (chkboxEditEmail.Checked) { sendDocumentToEmail(0, dtNow, clientID, custID, userID, outputFileName, "", "", true, emailSignature, emailEntity); }
                            else { sendDocumentToEmail(0, dtNow, clientID, custID, userID, outputFileName, "", "", false, "", ""); }
                        }

                        else if (rbtnEmailPDF.Checked)
                        {  //email PDF as attachment
                            if (chkboxEditEmail.Checked) { sendDocumentToEmail(0, dtNow, clientID, custID, userID, outputFileName, "", "", true, emailSignature, emailEntity); }
                            else { sendDocumentToEmail(0, dtNow, clientID, custID, userID, outputFileName, "", "", false, "", ""); }
                        }

                        else if (rbtnSendFax.Checked)
                        { //send as fax document
                            sendDocumentToFax(dtNow, clientID, custID, userID, customerLetter.OutputFileNameNoExt.Trim());
                        }
                        #endregion

                    }
                    catch (Exception)
                    {
                        reportStatementLiteral.Text = "Failed to complete process to generate: " + outputFileName;
                        reportStatementLiteral.Visible = true;
                    }
                }
                else
                {
                    reportStatementLiteral.Text = "Failed to generate: " + outputFileName + ": " + generateLetterResult;
                    reportStatementLiteral.Visible = true;
                }
            }


        }

        private void sendDocumentToFax(DateTime dtStamp, int cliID, int custID, int uID, string strF)
        { //TODO: send via smtp mail with to: [FAX:#]
            try
            {
                /*
                string debugMode = System.Configuration.ConfigurationManager.AppSettings["TestMode"];
                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
                if ((strF != null) && (strF.Length > 0))
                { //add attachment to email
                    if (debugMode.ToLower().Contains("true"))
                    { mailMsg.Attachments.Add(new System.Net.Mail.Attachment("C://inetpub//CFFIIS//reportfiles//" + strF.Trim())); }
                    else { mailMsg.Attachments.Add(new System.Net.Mail.Attachment(Server.MapPath("/reportfiles/" + strF.Trim()))); }
                }

                //mailMsg.IsBodyHtml = true;
                mailMsg.Subject = "Your letter statement from CFF";
                mailMsg.From = new System.Net.Mail.MailAddress("admin@factor.co.nz");

                if (debugMode.ToLower().Contains("true"))
                {
                    mailMsg.
                    mailMsg.To.Add(new System.Net.Mail.MailAddress(" [fax:095253598]")); 
                }
                else
                {
                    mailMsg.To.Add(new System.Net.Mail.MailAddress("[fax:"+ddlFaxToCustomer.SelectedValue.ToString()+"]"));
                }

                mailMsg.Body = "Please see fax document in relation to " + SessionWrapper.Instance.Get.ClientFromQueryString.Name + ".";
          
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["CredEUser"],
                        System.Configuration.ConfigurationManager.AppSettings["credEPwd"]); 
                smtp.Send(mailMsg);
                sendStatusLiteral.Text = "Fax Sent to " + ddlFaxToCustomer.SelectedValue.ToString() + ".";
                */

                string rptFolder = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
                string rptHdr = strF.Replace("_", " ");
                rptHdr = rptHdr.Replace("MTH", "Month");

                strF = rptFolder + strF.Trim() + ".htm";

                PrintableLetters printable = new PrintableLetters(strF, rptHdr, true, QueryString.ViewIDValue);
                printable.FaxEnable = true;
                string script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
            catch (Exception exc)
            {
                sendStatusLiteral.Text = "Error: " + exc.Message;
            }
            sendStatusLiteral.Text = "Fax function not yet supported. Set a local fax printer for print to fax.";
        }

        private void sendDocumentToEmail(Int16 iLtrType, DateTime dtStamp, int cliID, int custID, int uID, string strF, 
                                                string strSubject, string strMailBody, bool editMail,
                                                string userSignature, string userEntity,  
                                                //string emailTo,                                              
                                                bool isBodyHtml = false,
                                                bool noAttachmentDetailsInBody = false
                                                )
        {
            try
            {
                string debugMode = System.Configuration.ConfigurationManager.AppSettings["TestMode"];
                string rptHdr = "";
                if (strSubject.Length > 0) { rptHdr = strSubject; }
                else { rptHdr = "Cashflow Funding Limited - " + SessionWrapper.Instance.Get.ClientFromQueryString.Name; }

                string sendTo = "";
                string sendCC = "";
                string sendBCC = System.Configuration.ConfigurationManager.AppSettings["mailBackup"];

                if (rbtnEmailToCustomer.Checked)
                {
                    sendTo = ddlEmailToCustomer.Text;
                }
                else if ((rbtnEmailToClient.Checked) || (rbtnEmailClientCust.Checked))
                {
                    List<object> objParams = new List<object>();
                    objParams.Add("ClientReportsEmail");
                    objParams.Add((int)SessionWrapper.Instance.Get.ClientFromQueryString.Id);
                    objParams.Add("0");
                    stpCaller stpc = new stpCaller();
                    System.Data.DataSet DS = stpc.executeSPDataSet(objParams, stpCaller.stpType.GetClientSelectedData);

                    if (DS != null)
                    {
                        if (DS.Tables[0] != null)
                        {
                            if (DS.Tables[0].Rows.Count > 0)
                            {
                                sendTo = DS.Tables[0].Rows[0]["RptsEmail"].ToString();
                            }
                        }
                    }

                    if (rbtnEmailClientCust.Checked)
                    { sendTo += ";" + ddlEmailToCustomer.Text; }
                }
                else if (rbtnEmailToCollector.Checked)
                {
                    List<object> objParams = new List<object>();
                    objParams.Add("CollectorsDetails");
                    if (strF.Contains("AEL")) { objParams.Add(1); }
                    else { objParams.Add(2); }
                    objParams.Add("0");
                    stpCaller stpc = new stpCaller();
                    System.Data.DataSet DS = stpc.executeSPDataSet(objParams, stpCaller.stpType.GetClientSelectedData);
                    if (DS != null)
                    {
                        if (DS.Tables[0] != null)
                        {
                            if (DS.Tables[0].Rows.Count > 0)
                            {
                                sendTo = DS.Tables[0].Rows[0]["email"].ToString();
                            }
                        }
                    }
                }
                else
                {
                    //e.g btnEmailBankDetails_Click & email option buttons not selected
                    sendTo = ddlEmailToCustomer.Text;
                }


                if (sendTo.Length > 0) //could check to see if valid email address!!
                {
                    if (editMail)
                    {
                        DocumentMailer myDoc = null;
                        if (strMailBody.Length > 0)
                        {
                            myDoc = new DocumentMailer(iLtrType, (short)ddlLetters.SelectedIndex, dtStamp, cliID, custID, uID,
                                                            strF, rptHdr, sendTo, "", "", strMailBody, QueryString.ViewIDValue, noAttachmentDetailsInBody);
                        }
                        else
                        {
                            //MSarza [20151022]

                            string mailbody;
                            mailbody = "\r\nPlease see attached in relation to "
                                                + SessionWrapper.Instance.Get.ClientFromQueryString.Name + "."
                                                + "\r\n\r\n\r\n\r\n"
                                                + userSignature
                                                + "\r\n"
                                                + userEntity
                                                + "\r\n\r\n\r\n\r\n"
                                                + "-------------------------------------\r\n"
                                                + "Disclaimer: This email is confidential,may be subject to legal privelege, and for the use of the intended recipient(s) only. "
                                                + "Unless you are the intended addressee (or authorized to receive on behalf of the intended addressee), you may not keep, use, copy or disclose to "
                                                + "anyone the message or any information contained in this email and the attached files (if any). "
                                                + "If you have received the message in error, please reply to this message immediately and then delete it. "
                                                + "If verification is required please request a hard-copy version. ";
                            myDoc = new DocumentMailer(iLtrType, (short)ddlLetters.SelectedIndex, dtStamp, cliID, custID, uID,
                                                                strF, rptHdr, sendTo, sendCC, sendBCC, mailbody, QueryString.ViewIDValue, false);
                        }

                        string script = PopupHelper.ShowPopup(myDoc, Server);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
                    }
                    else
                    { //send via smtp
                        try
                        {
                            System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
                            if ((strF != null) && (strF.Length > 0))       
                            { //add attachments to email

                                string rptFolder = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
                                if (debugMode.ToLower().Contains("true"))
                                {
                                    //mailMsg.Attachments.Add(new System.Net.Mail.Attachment("C://inetpub//CFFIIS//reportfiles//" + strF.Trim()));
                                    mailMsg.Attachments.Add(new System.Net.Mail.Attachment(rptFolder + strF.Trim()));
                                }
                                else
                                {
                                    //mailMsg.Attachments.Add(new System.Net.Mail.Attachment(Server.MapPath("/reportfiles/" + strF.Trim()))); 
                                    mailMsg.Attachments.Add(new System.Net.Mail.Attachment(rptFolder + strF.Trim()));
                                }
                            }

                            //mailMsg.IsBodyHtml = true;
                            mailMsg.Subject = rptHdr;
                            mailMsg.From = new System.Net.Mail.MailAddress("webadmin@factor.co.nz");

                            if (debugMode.ToLower().Contains("true"))
                            { mailMsg.To.Add(new System.Net.Mail.MailAddress("marty@factor.co.nz")); }

                            else
                            {
                                mailMsg.To.Add(new System.Net.Mail.MailAddress(sendTo));
                            }

                            if (sendCC.Length > 0) { mailMsg.CC.Add(new System.Net.Mail.MailAddress(sendCC)); }
                            if ((sendBCC != null) && sendBCC.Length > 0) { mailMsg.Bcc.Add(new System.Net.Mail.MailAddress(sendBCC)); }

                            if (strMailBody.Length > 0)
                            {
                                mailMsg.Body = strMailBody;
                                mailMsg.IsBodyHtml = isBodyHtml; ////////
                                
                            }
                            else
                            {
                                mailMsg.Body = "Please see attached in relation to " + SessionWrapper.Instance.Get.ClientFromQueryString.Name + ".";
                            }

                            //smtp.Credentials = new System.Net.NetworkCredential("webadmin", "~Myf4ct-0r5!");
                            //smtp.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["CredEUser"], 
                            //        System.Configuration.ConfigurationManager.AppSettings["credEPwd"]); //TODO

                            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                            smtp.Send(mailMsg);
                            sendStatusLiteral.Text = "Email sent to " + sendTo;

                            //MSarza[20160929]: Added to be able to update notes
                            DocumentMailer myDoc = null;
                            myDoc = new DocumentMailer(iLtrType, (short)ddlLetters.SelectedIndex, dtStamp, cliID, custID, uID,
                                                                strF, rptHdr, sendTo, sendCC, sendBCC, mailMsg.Body, QueryString.ViewIDValue, false);
                            Cff.SaferTrader.Core.Letters.MailEvents mEvt = new Cff.SaferTrader.Core.Letters.MailEvents(myDoc);
                            System.Threading.Thread thrMEvt = new System.Threading.Thread(mEvt.updateLetterSent);
                            thrMEvt.Start();

                        }
                        catch (Exception exc)
                        {
                            sendStatusLiteral.Text = exc.Message;
                            sendStatusLiteral.Visible = true;
                        }
                    }
                }
                else
                {
                    //string script = "alert(\"The email address provided is invalid.\");";
                    string script = "dialogMsg('#invalidEmailMsg')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageScript", script, true);
                }
            }
            catch (Exception)
            {

            }
        }

        private void sendDocumentToScreen(string strF)
        { //display word document in popup browser
            try
            {
                string debugMode = System.Configuration.ConfigurationManager.AppSettings["TestMode"];
                //string rptFolder = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
                string rptHdr = strF.Replace("_", " ");
                rptHdr = rptHdr.Replace("MTH", "Month");
                if (debugMode.Contains("true"))
                    strF = Server.MapPath("/reportfiles/" + strF.Trim() + ".htm");
                //{ strF = "C://inetpub//CFFIIS//reportfiles//" + strF.Trim() + ".htm"; }
                else
                {
                    strF = strF.Trim() + ".htm";
                    //strF = rptFolder + strF.Trim() + ".htm";
                    //strF = Server.MapPath("/Reportfiles/" + strF.Trim() + ".htm");
                    //strF = Server.MapPath(strF.Trim() + ".htm");
                }
                PrintableLetters printable = new PrintableLetters(strF, rptHdr, false, QueryString.ViewIDValue);
                string script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
            catch (Exception exc)
            {
                sendStatusLiteral.Text = exc.Message;
            }
        }

        private void sendDocumentToPrinter(string docToPrint)
        {
            try
            {
                string redirectURL = "downloading.aspx?ViewID=" + QueryString.ViewIDValue + "&file=" + docToPrint;
                Response.Redirect(redirectURL);
            }
            catch (Exception exc)
            {
                sendStatusLiteral.Text = exc.Message;
            }

        }

        protected void customerContactsGridViewTextColumnValueChanged(object sender, EventArgs e)
        {
            TextBox tBox = (TextBox)sender;
            editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;

            int rIdx = customerContactsGridView.FocusedRowIndex;   //.EditingRowVisibleIndex;
            switch (tBox.ID)
            {
                case "LastName":
                    editCustomerContacts[rIdx].LastName = tBox.Text;
                    editCustomerContacts[rIdx].FullName = editCustomerContacts[0].FirstName + " " + editCustomerContacts[0].LastName;
                    break;

                case "FirstName":
                    editCustomerContacts[rIdx].FirstName = tBox.Text;
                    editCustomerContacts[rIdx].FullName = editCustomerContacts[0].FirstName + " " + editCustomerContacts[0].LastName;
                    break;

                case "MobilePhone":
                    editCustomerContacts[rIdx].MobilePhone = tBox.Text;
                    break;

                case "Email":
                    editCustomerContacts[rIdx].Email = tBox.Text;
                    break;

                case "Phone":
                    editCustomerContacts[rIdx].Phone = tBox.Text;
                    break;

                case "Fax":
                    editCustomerContacts[rIdx].Fax = tBox.Text;
                    break;

                case "Role":
                    editCustomerContacts[rIdx].Role = tBox.Text;
                    break;

                default:
                    break;
            }
        }


        protected void chkboxAttn_CheckedChanged(object sender, EventArgs e)
        {
            customerContactsGridView.FocusedRowIndex = ddlEmailToCustomer.SelectedIndex;
            int rowIdx = ddlEmailToCustomer.SelectedIndex;
            editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
            custContactNameLiteral.Text = editCustomerContacts[rowIdx].FullName;
            custFNameLiteral.Text = editCustomerContacts[rowIdx].FirstName;
        }

        protected void chkboxDear_CheckedChanged(object sender, EventArgs e)
        {
            customerContactsGridView.FocusedRowIndex = ddlEmailToCustomer.SelectedIndex;
            int rowIdx = ddlEmailToCustomer.SelectedIndex;
            editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
            custContactNameLiteral.Text = editCustomerContacts[rowIdx].FullName;
            custFNameLiteral.Text = editCustomerContacts[rowIdx].FirstName;
        }

        protected void ddlEmailtoCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customerContactsGridView.DataSource == null && ViewState["customerContacts"] != null)
                DisplayCustomerContacts(ViewState["customerContacts"] as IList<CustomerContact>);

            int rowIdx = ddlEmailToCustomer.SelectedIndex;
            if (ViewState["SelectedRowIndex"] != null)
                rowIdx = Convert.ToInt32(ViewState["SelectedRowIndex"]);

            customerContactsGridView.SelectedIndex = rowIdx;
            customerContactsGridView.FocusedRowIndex = rowIdx;
            customerContactsGridView.Rows[rowIdx].Style.Add(HtmlTextWriterStyle.BackgroundColor, "HoneyDew");

            editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
            custFNameLiteral.Text = editCustomerContacts[rowIdx].FirstName;
            custContactNameLiteral.Text = editCustomerContacts[rowIdx].FullName;
        }

        protected void ddlFaxToCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEmailToCustomer.SelectedIndex = ddlFaxToCustomer.SelectedIndex;
        }

        protected void ddlEmailToCustomer_DataBound(object sender, EventArgs e)
        {
            if (eventTarget == "customerContactsGridView")    // || customerContactsGridView.IsCallback == true) //(customerContactsGridView.IsCallback) 
            {
                int rowIdx = customerContactsGridView.FocusedRowIndex;
                ddlEmailToCustomer.SelectedIndex = rowIdx;

                editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
                custContactNameLiteral.Text = editCustomerContacts[rowIdx].FullName;
                custFNameLiteral.Text = editCustomerContacts[rowIdx].FirstName;
                ddlFaxToCustomer.SelectedIndex = rowIdx;
            }
        }


        public void Print()
        {
        }


        protected void button_Click(object sender, EventArgs e)
        {
            ddlEmailToCustomer.SelectedIndex = customerContactsGridView.FocusedRowIndex;
            editCustomerContacts = ViewState["customerContacts"] as IList<CustomerContact>;
            custContactNameLiteral.Text = editCustomerContacts[customerContactsGridView.FocusedRowIndex].FullName;
            custFNameLiteral.Text = editCustomerContacts[customerContactsGridView.FocusedRowIndex].FirstName;
            button.Text = customerContactsGridView.FocusedRowIndex.ToString();
        }


        #region  "Helper Functions"

        private bool IsEmailValid(string email)
        {

            Regex reg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match mtch = reg.Match(email);

            //bool isValid = new EmailAddressAttribute().IsValid(email);    // dbb [20160729]

            //bool isValid = false || Regex.IsMatch(email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            //                                             @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            //    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            //return isValid;
            return mtch.Success;
        }

        #endregion


        // SMS methods   dbb 12/08/2015

        protected void btnSendSMS_Click(object sender, EventArgs e)
        {
            string smsVal = SMSMsg.Text;
            string mobileNo = mobileNum.Text;
            string conf = "";
            string confirmVal = Request.Form["confirm_value"];
            string[] words = confirmVal.Split(',');
            int wordKey = 0;
            for (int wordLen = 0; wordLen < words.Length; wordLen++)
            {
                wordKey = wordLen;
            }

            conf = words[wordKey];

            if (conf == "No")
            {
                smsPanel.Focus();
            }
            else
            {
                try
                {
                    if (mobileNo.Length >= 9)
                    {
                        if (smsVal.Length > 5) // && smsVal.Length < defChar)
                        {
                            int custId = SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
                            int clientId = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
                            string modifiedBy = SessionWrapper.Instance.Get.UserIdentity.ToString();
                            string yrMth = DateTime.Now.ToString("yyyyMM");
                            string refId = SessionWrapper.Instance.Get.ClientFromQueryString.Id + "-" +
                                           SessionWrapper.Instance.Get.CustomerFromQueryString.Number;
                            string sentTime = DateTime.Now.ToString();
                            string recvTime = DateTime.Now.ToString();
                            DateTime mod = DateTime.Now;

                            List<object> objParams = new List<object>();

                            objParams.Add("+6495794204"); // sender
                            objParams.Add(mobileNum.Text); // receiver
                            objParams.Add(smsVal); // msg
                            objParams.Add("CFFWebApp"); // operator
                            objParams.Add("SMS:TEXT"); // msgType
                            objParams.Add(refId); // reference : clientId+'-'+customerId
                            objParams.Add("send"); // default value
                            objParams.Add(clientId.ToString()); // clientId
                            objParams.Add(custId.ToString()); // custId
                            objParams.Add(Int32.Parse(modifiedBy)); // EmployeeId
                            objParams.Add(Int32.Parse(yrMth)); // yrmth
                            objParams.Add("Inserting SMS"); // return value

                            stpCaller stpC = new stpCaller();
                            int retVal = stpC.executeSP(objParams, stpCaller.stpType.smsMessageIn);

                            SMSMsg.Text = string.Empty; // reset sms field
                            remChar = (defChar - SMSMsg.Text.Length);
                            txtCount.Text = remChar.ToString() + " characters remaining";
                            smsPanel.Update();
                            string sndScript = "dialogMsg('#sendNotify')";
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MessageScript", sndScript,
                                true);
                        }
                        else
                        {
                            SMSMsg.BorderColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                    else
                    {
                        mobileNum.BorderColor = System.Drawing.Color.OrangeRed;
                        string recEx = "dialogMsg('#noRecipient')";
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MessageScript", recEx, true);

                    }
                }
                catch (Exception ex)
                {
                    string msgErr = ex.StackTrace;
                    string scErr = "dialogMsg('#smsException')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageScript", scErr, true);
                }
            }
        }


        protected void ClearButton_Click(object sender, EventArgs e)
        {
            string conf = "";
            string confirmValue = Request.Form["confirm_value"];
            string[] words = confirmValue.Split(',');
            int wordKey = 0;
            for (int wordLen = 0; wordLen < words.Length; wordLen++)
            {
                wordKey = wordLen;
            }
            conf = words[wordKey];

            if (conf == "Yes")
            {
                SMSMsg.Text = string.Empty;
                remChar = (defChar - SMSMsg.Text.Length);
                txtCount.Text = remChar.ToString() + " characters remaining";
                smsPanel.Update();
            }
        }

        protected void ComposeSmsButton_Click(object sender, EventArgs e)
        {
            string custName = SessionWrapper.Instance.Get.CustomerFromQueryString.Name;
            string clientName = SessionWrapper.Instance.Get.ClientFromQueryString.Name;

            MembershipUser member = Membership.GetUser(loggedUser);
            string userEmail = member.Email;

            Cff.SaferTrader.Core.Repositories.IReportRepository repo = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateReportRepository();
            Cff.SaferTrader.Core.Reports.StatementReport stRpt = repo.LoadStatementReport(SessionWrapper.Instance.Get.ClientFromQueryString.Id,
                                SessionWrapper.Instance.Get.CustomerFromQueryString.Id, new Date(System.DateTime.Now));
            if (stRpt == null)
            {
                reportStatementLiteral.Text = "Nothing fetched from the database.";
                reportStatementLiteral.Visible = true;
                return;
            }

            Cff.SaferTrader.Core.Repositories.ICustomerRepository custRepo = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateCustomerRepository();
            ClientAndCustomerInformation cinfo = custRepo.GetMatchedCustomerInfo(SessionWrapper.Instance.Get.CustomerFromQueryString.Id, SessionWrapper.Instance.Get.ClientFromQueryString.Id);

            if (SMSMsg.Text.Length <= maxChar)
            {

                Button btn = (Button)sender;

                if (btn.ID.Equals("hiButton"))
                    SMSMsg.Text = SMSMsg.Text + "Hi " + custFNameLiteral.Text;
                if (btn.ID.Equals("tnxButton"))
                    SMSMsg.Text = SMSMsg.Text + " thanks";
                if (btn.ID.Equals("cffButton"))
                    SMSMsg.Text = SMSMsg.Text + " Cashflow Funding Limited";
                if (btn.ID.Equals("phoneMeButton"))
                    SMSMsg.Text = SMSMsg.Text + " Please phone " + cinfo.CffClientInformation.ClientSignature;
                if (btn.ID.Equals("oButton"))
                    SMSMsg.Text = SMSMsg.Text + " 01 0242 0146299 000";
                if (btn.ID.Equals("emailMeButton"))
                    SMSMsg.Text = SMSMsg.Text + ' ' + userEmail;
                if (btn.ID.Equals("yourAcctWithButton"))
                    SMSMsg.Text = SMSMsg.Text + " re your account with";
                if (btn.ID.Equals("clientButton"))
                    SMSMsg.Text = SMSMsg.Text + ' ' + clientName;
                if (btn.ID.Equals("CustButton"))
                    SMSMsg.Text = SMSMsg.Text + ' ' + custName;
                if (btn.ID.Equals("plsPayButton"))
                    SMSMsg.Text = SMSMsg.Text + " Please pay";
                if (btn.ID.Equals("balButton"))
                    SMSMsg.Text = SMSMsg.Text + " $" + Decimal.Round(stRpt.AgeingBalances.Balance, 2);
                if (btn.ID.Equals("threePlusMoButton"))
                    SMSMsg.Text = SMSMsg.Text + " $" + Decimal.Round(stRpt.AgeingBalances.ThreeMonthPlusAgeing, 2);
                if (btn.ID.Equals("twoPlusMoButton"))
                    SMSMsg.Text = SMSMsg.Text + " $" + Decimal.Round(stRpt.AgeingBalances.TwoMonthAgeing, 2);
                if (btn.ID.Equals("oneMoButton"))
                    SMSMsg.Text = SMSMsg.Text + " $" + Decimal.Round(stRpt.AgeingBalances.OneMonthAgeing, 2);
                if (btn.ID.Equals("currentButton"))
                    SMSMsg.Text = SMSMsg.Text + " $" + Decimal.Round(stRpt.AgeingBalances.Current, 2);
                if (btn.ID.Equals("greaterTwoMoButton"))
                    SMSMsg.Text = SMSMsg.Text + " $" +
                                  Decimal.Round(
                                      (stRpt.AgeingBalances.TwoMonthAgeing + stRpt.AgeingBalances.ThreeMonthPlusAgeing),
                                      2);
                if (btn.ID.Equals("greaterOneMoButton"))
                    SMSMsg.Text = SMSMsg.Text + " $" +
                                  Decimal.Round(
                                      (stRpt.AgeingBalances.OneMonthAgeing + stRpt.AgeingBalances.TwoMonthAgeing +
                                       stRpt.AgeingBalances.ThreeMonthPlusAgeing), 2);
                if (btn.ID.Equals("dateButton"))
                    SMSMsg.Text = SMSMsg.Text + dateButton;
                if (btn.ID.Equals("whenButton"))
                    SMSMsg.Text = SMSMsg.Text + whenButton;
                if (btn.ID.Equals("replyQuoteButton"))
                    SMSMsg.Text = SMSMsg.Text + " Reply quoting:";
                if (btn.ID.Equals("refButton"))
                    SMSMsg.Text = SMSMsg.Text + ' ' + SessionWrapper.Instance.Get.ClientFromQueryString.Id + "-" +
                                  SessionWrapper.Instance.Get.CustomerFromQueryString.Number;

                SMSMsg.Text = Regex.Replace(SMSMsg.Text, @"\s+", " ");
                SMSMsg.Text = SMSMsg.Text.Trim();
            }
            remChar = (defChar - SMSMsg.Text.Length);
            smsPanel.Update();

            if (remChar > 0 && remChar <= defChar)
            {
                if (remChar >= 0) txtCount.Text = remChar.ToString() + " characters remaining";
            }
            if (SMSMsg.Text.Length > defChar)
            {
                if (notifCtr == 0)
                {
                    txtCount.Text = "0 characters remaining.";
                    notifCtr = notifCtr + 1;
                    string errEx = "dialogMsg('#exceed160Char')";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MessageScript", errEx, true);

                }
            }
        }

        protected void ComposeSmsTextBox_Click(object sender, EventArgs e)
        {
            TextBox txb = (TextBox)sender;
            SMSMsg.Text = SMSMsg.Text + ' ' + txb.Text;
            SMSMsg.Text = Regex.Replace(SMSMsg.Text, @"\s+", " ");

            remChar = (defChar - SMSMsg.Text.Length);
            smsPanel.Update();

            if (remChar >= 0 && remChar <= defChar)
            {
                if (remChar >= 0)
                    txtCount.Text = remChar.ToString() + " characters remaining";

            }

            if (SMSMsg.Text.Length >= defChar)
            {
                txtCount.Text = "0 characters remaining.";
                string errEx = "dialogMsg('#exceed160Char')";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MessageScript", errEx, true);
            }
        }

        protected void ComposeSmsDdl_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            SMSMsg.Text = SMSMsg.Text + ' ' + ddl.SelectedValue;
            SMSMsg.Text = Regex.Replace(SMSMsg.Text, @"\s+", " ");

            remChar = (defChar - SMSMsg.Text.Length);
            smsPanel.Update();

            if (remChar >= 0 && remChar <= defChar)
            {
                if (remChar >= 0) txtCount.Text = remChar.ToString() + " characters remaining";
            }

            if (SMSMsg.Text.Length >= defChar)
            {
                txtCount.Text = "0 characters remaining";
                string errEx = "dialogMsg('#exceed160Char')";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MessageScript", errEx, true);
            }
        }

        void smsPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();      //mkjREF
        }

        //*private string GetCustomerLetterTemplate(string letterName, string custTemplatePath, int clientID, bool clientHasLetterTemplate, bool isClientUser, string clientFacilityType)
        //{
        //    string lName;
        //    lName = CleanString(letterName);
        //    //lName = letterName;
        //    string strResult;

        //    #region Get Notification templates
        //    if (lName.Contains("Notification"))
        //    {

        //        if (lName.Contains("Client_Notification"))
        //        {
        //            if (clientHasLetterTemplate)
        //            {
        //                custTemplatePath += clientID.ToString() + "\\Notification\\ClientNotification\\";

        //                if (File.Exists(custTemplatePath + clientID.ToString() + "_" + lName + ".docx"))
        //                    strResult = custTemplatePath + clientID.ToString() + "_" + lName + ".docx";
        //                else if (File.Exists(custTemplatePath + clientID.ToString() + "_" + lName + ".doc"))
        //                    strResult = custTemplatePath + clientID.ToString() + "_" + lName + ".doc";
        //                else { strResult = "Error: Customer notification template not found"; }
        //            }
        //            else
        //            {
        //                custTemplatePath += "Notification\\ClientNotification\\";

        //                if (File.Exists(custTemplatePath + clientID.ToString() + "_" + lName + ".docx"))
        //                    strResult = custTemplatePath + clientID.ToString() + "_" + lName + ".docx";
        //                else if (File.Exists(custTemplatePath + clientID.ToString() + "_" + lName + ".doc"))
        //                    strResult = custTemplatePath + clientID.ToString() + "_" + lName + ".doc";
        //                else if (File.Exists(custTemplatePath + "000_" + lName + ".docx"))
        //                    strResult = custTemplatePath + "000_" + lName + ".docx";
        //                else if (File.Exists(custTemplatePath + "000_" + lName + ".doc"))
        //                    strResult = "000_" + lName + ".doc";
        //                else { strResult = "Error: Customer notification template not found"; }

        //            }                        
        //        }
        //        else
        //        {
        //            custTemplatePath += "Notification\\";

        //            if (File.Exists(custTemplatePath + clientID.ToString() + "_" + lName + ".docx"))
        //                strResult = custTemplatePath + clientID.ToString() + "_" + lName + ".docx";
        //            else if (File.Exists(custTemplatePath + clientID.ToString() + "_" + lName + ".doc"))
        //                strResult = custTemplatePath + clientID.ToString() + "_" + lName + ".doc";
        //            else
        //            {
        //                string np;
        //                // the following facilitype definition was based on stored procedure definition
        //                switch (clientFacilityType.ToLower())
        //                    {
						  //  case "factoring":
        //                        np = "001_"; break;                                
						  //  case "debtor mgt":
        //                        np = "002_"; break;
						  //  case "cfsl":
        //                        np = "003_"; break;
						  //  case "loan":
        //                        np = "004_"; break;
						  //  case "current a/c":
        //                        np = "005_"; break;
						  //  default: 
        //                        np = "xxx_"; break;
        //                   }
                        
        //                if (File.Exists(custTemplatePath + np + lName + ".docx"))
        //                    strResult = custTemplatePath + np + lName + ".docx";
        //                else if (File.Exists(custTemplatePath + np + lName + ".doc"))
        //                    strResult = custTemplatePath + np + lName + ".doc";
        //                else
        //                {
        //                    // MSarza [20160209]: As per Marty, all clientfacility types' notification templates should
        //                    //      default to the 000_ notification template
        //                    //if (clientFacilityType.ToLower() == "factoring")
        //                    //{
        //                        if (File.Exists(custTemplatePath + "000_" + lName + ".docx"))
        //                            strResult = custTemplatePath + "000_" + lName + ".docx";
        //                        else if (File.Exists(custTemplatePath + "000_" + lName + ".doc"))
        //                            strResult = custTemplatePath + "000_" + lName + ".doc";
        //                        else
        //                            strResult = "Error: Notification template not found"; 
        //                    //}
        //                    //else
        //                    //    strResult = "Error: Notification template not found"; 
        //                }
        //            }
        //        }
        //    }
        //    #endregion
        //    else
        //    {
        //        List<String> fileList = new List<String>();

        //        // if user is cff, can use default templates not identifed as one which the client has its own equivalent template; see PopulateLettersList method
        //        //          --> if client has equivalent template, must strictly use that. Cannot use any other even if it has errors or faulters out
        //        //          --> can only use default templates if client (when has own templates) has no equivalent template

        //        if (clientHasLetterTemplate)
        //        {
        //            string hasTemplatePath = custTemplatePath + clientID + "\\";
        //            if (Directory.Exists(hasTemplatePath))
        //            {
        //                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(hasTemplatePath);
        //                System.IO.FileInfo[] files = di.GetFiles("*.doc*");
        //                if (files.Length > 0)
        //                {
        //                    int x;
        //                    string[] tmpStr;
        //                    for (x = 0; x < files.Length; x++)
        //                    {
        //                        tmpStr = files[x].Name.Split('.');
        //                        if (tmpStr[0][0] != '~')
        //                            fileList.Add(cleanupFileName(tmpStr[0]));
        //                    }
        //                }

        //                foreach(string fn in fileList)
        //                {
        //                    if (fn == letterName)       // letterName used because it is from ddlLetters
        //                    {
        //                        custTemplatePath += clientID.ToString() + "\\";
        //                    }
        //                }

        //                // make sure PopulateLettersList had appropriately filtered out duplicate the letters in customer nad default directories, hence this simple assignment
        //                custTemplatePath += lName;  
        //            }
        //            else
        //                if (isClientUser)
        //                    return "Error: Customer has templates set but does not exist.";
        //                else
        //                {
        //                    // make sure PopulateLettersList had appropriately filtered out duplicate the letters in customer nad default directories, hence this simple assignment
        //                    custTemplatePath += lName;
        //                }

        //        }
        //        else
        //            custTemplatePath += lName;



        //        if (custTemplatePath.Contains("Month"))
        //        {
        //            if (System.IO.File.Exists(custTemplatePath + ".docx"))
        //                strResult = custTemplatePath + ".docx";
        //            else if (System.IO.File.Exists(custTemplatePath + ".doc"))
        //                strResult = custTemplatePath + ".doc";
        //            else if (System.IO.File.Exists(custTemplatePath.Replace("Month", "MTH") + ".docx"))
        //                strResult = custTemplatePath.Replace("Month", "MTH") + ".docx";
        //            else if (System.IO.File.Exists(custTemplatePath.Replace("Month", "MTH") + ".doc"))
        //                strResult = custTemplatePath.Replace("Month", "MTH") + ".doc";
        //            else { strResult = "Error: Customer " + lName + " letter template not found"; }
        //        }
        //        else
        //        {
        //            if (System.IO.File.Exists(custTemplatePath + ".docx"))
        //                strResult = custTemplatePath + ".docx";
        //            else if (System.IO.File.Exists(custTemplatePath + ".doc"))
        //                strResult = custTemplatePath + ".doc";
        //            else { strResult = "Error: Customer " + lName + " letter template not found"; }
        //        }
        //    }

        //    return strResult;
        //}
    
        private string CleanString(string stringToClean)
        {
            stringToClean = stringToClean.Replace("&", "and");
            stringToClean = stringToClean.Replace("/", "_");
            stringToClean = stringToClean.Replace("\\", "");
            stringToClean = stringToClean.Replace("(", "");
            stringToClean = stringToClean.Replace(")", "");
            stringToClean = stringToClean.Replace(":", "");
            stringToClean = stringToClean.Replace(",", "");
            stringToClean = stringToClean.Replace("?", "");
            stringToClean = stringToClean.Replace("@", "at");
            stringToClean = stringToClean.Replace(" - ", "_");
            stringToClean = stringToClean.Replace(" ", "_");
            stringToClean = stringToClean.Replace("__", "_");
            stringToClean = stringToClean.Replace(">", "");
            stringToClean = stringToClean.Replace("=", "");
            stringToClean = stringToClean.Replace("'", "");
            stringToClean = stringToClean.Replace("#", "");
            return stringToClean;
        }

        //*private string CheckClientLetterhead(int clientID, bool isUserClient, bool cffAdminAsCff
		//						, bool clientHasOwnLetterTemplates, string clientLetterHeadDirectory
		//						, string letterName, string templatePath
        //                        , bool overrideSendStatementAs = false)
        //{
	    //    // Rule as at 20151001:
	    //    //	     Cff as Cff      --> Cff letterhead
	    //    //       Cff as Client   --> Client letterhead
	    //    //       Client          --> Client letterhead
	    //    // NOTE1: If Client has own letter templates, it is assumed that the letterhead therein is the client's.
        //    // NOTE2: Above rule is superseded by rules on Notification and Client Notification letters where the former is issued only by CFF as CFF and
        //    //          the latter issued only by Client either by itself or by CFF as the client.
		//	
		//	string lName = CleanString(letterName);
        //    string clientLetterHeadFilePath = clientLetterHeadDirectory;
		//	
		//	if (lName.Contains("Notification"))
        //    {
		//		if (lName.Contains("Client_Notification"))
		//		{
        //            if (clientHasOwnLetterTemplates)
        //                clientLetterHeadFilePath = "Do not update";
        //            else
        //            {
        //                templatePath +=  "\\Notification\\ClientNotification\\";
        //                if (File.Exists(templatePath + clientID.ToString() + "_" + lName + ".docx"))
        //                    clientLetterHeadFilePath = "Do not update";
        //                else if (File.Exists(templatePath + clientID.ToString() + "_" + lName + ".doc"))
        //                    clientLetterHeadFilePath = "Do not update";
        //                else
        //                    clientLetterHeadFilePath += GetClientLetterheadImageFileName(clientID);
        //            }
		//		}
		//		else clientLetterHeadFilePath = "Do not update";  // Notification templates are sent only by Cff as Cff hence letterehead must be on template
		//	}
        //    else if (lName.Contains("Statement"))  
        //    {
        //        // Statement templates are classified as Special Cases letters whose templates will only be on a single directory (of same name) 
        //        // and will have Cff logo on it. As per business rule changes, when the client generates it, the letterhead should be updated 
        //        // to its own, hence, the OverrideSendStatementAs field to implement prior rules onto the new rule.
        //        if (overrideSendStatementAs) //if true, logo should always be that of CFF; implementing original statement report rule on to the new rule
        //        {
        //            clientLetterHeadFilePath = "Do not update";
        //        }
        //        else
        //        {
        //            if ((isUserClient) || (!cffAdminAsCff))
        //            clientLetterHeadFilePath += GetClientLetterheadImageFileName(clientID);
        //        else
        //            clientLetterHeadFilePath = "Do not update";
        //    }
        //    }
		//	else 
		//	{
		//		if (isUserClient) // user is Client
		//		{
		//			if (clientHasOwnLetterTemplates)
		//				clientLetterHeadFilePath = "Do not update";     
		//			else 
		//				clientLetterHeadFilePath += GetClientLetterheadImageFileName(clientID);
		//		}
		//		else //user is cff
		//		{
		//			if (cffAdminAsCff)	// cff as cff
		//			{
		//				if (clientHasOwnLetterTemplates)
		//					clientLetterHeadFilePath += "default.jpg";
		//				else clientLetterHeadFilePath = "Do not update";
		//			}
		//			else //cff as client
		//			{
		//				if (clientHasOwnLetterTemplates)
		//					clientLetterHeadFilePath = "Do not update";	
		//				else 
		//					clientLetterHeadFilePath += GetClientLetterheadImageFileName(clientID);
		//			}
		//		}
		//	}
		//	return clientLetterHeadFilePath;
        //}

        private string GetClientLetterheadImageFileName(int clientID)
        {
            List<object> arrParam = new List<object>();
            arrParam.Clear();
            arrParam.Add("LetterHead");
            arrParam.Add(clientID);
            arrParam.Add(0);

            stpCaller stpC = new stpCaller();
            System.Data.DataSet theDS;
            //theDS.Clear();
            theDS = stpC.executeSPDataSet(arrParam, stpCaller.stpType.GetClientSelectedData);

            if (theDS.Tables[0].Rows.Count > 0)
                return theDS.Tables[0].Rows[0]["letterheadFileName"].ToString();
            else
                return "";
        }

        protected void SMSMsg_OnTextChanged(object sender, EventArgs e)
        {
            string sms = ((TextBox)sender).Text;
            sms = Regex.Replace(sms, @"\s+", " ");

            SMSMsg.Text = sms;
            remChar = (defChar - sms.Length);
            smsPanel.Update();

            if (remChar >= 0 && remChar <= defChar)
            {
                if (remChar >= 0) txtCount.Text = remChar.ToString() + " characters remaining";
            }
        }



        private string AdjustLineAddresses(string line1, string line2, string line3, string line4, string line5, int lineIndex)
        {
            string fullLine = "";

            if (line1.Length > 1) fullLine += "!" + line1;
            if (line2.Length > 1) fullLine += "!" + line2;
            if (line3.Length > 1) fullLine += "!" + line3;
            if (line4.Length > 1) fullLine += "!" + line4;

            string[] lineArray = fullLine.Split('!');

            switch (lineIndex)
            {
                case 1:
                    if (lineArray.Length > 1)
                        return lineArray[1].ToString();
                    else return "";
                case 2:
                    if (lineArray.Length > 2)
                        return lineArray[2].ToString();
                    else return "";
                case 3:
                    if (lineArray.Length > 3)
                        return lineArray[3].ToString();
                    else return "";
                case 4:
                    if (lineArray.Length > 4)
                        return lineArray[4].ToString();
                    else return "";
                case 5:
                    if (lineArray.Length > 5)
                        return lineArray[5].ToString();
                    else return "";
                default:
                    return "";
            }

        }

        private string LineUpAddresses(string line1, string line2, string line3, string line4, string line5)
        {
            string fullLine = "";
            if (line1.Length > 1) fullLine += ", " + line1;
            if (line2.Length > 1) fullLine += ", " + line2;
            if (line3.Length > 1) fullLine += ", " + line3;
            if (line4.Length > 1) fullLine += ", " + line4;
            if (line5.Length > 1) fullLine += ", " + line5;

            if (fullLine.Substring(0, 2) == ", ")
                return fullLine.Substring(2, (fullLine.Length - 2));
            return fullLine;
        }

    }
}