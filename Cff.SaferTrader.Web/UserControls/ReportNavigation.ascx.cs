using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.ReportManager;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class ReportNavigation : UserControl
    {
        private void AddJavaScriptIncludeInHeader(string path)
        {  /// Adds a JavaScript reference to the header of the document.
            try
            {
                var script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = ResolveUrl(path);
                Page.Header.Controls.Add(script);
            }
            catch { }
        }

        private void InitializeCurrentPathForJavaScript()
        {
            string relativePathToRoot = RelativePathComputer.ComputeRelativePathToRoot(Server.MapPath("~"),
                                                                                       Server.MapPath("."));
            string script = string.Format(@"var relativePathToRoot='{0}';", relativePathToRoot);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "initializeCurrentPath", script, true);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
        }
    
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeCurrentPathForJavaScript();

            if (!IsPostBack) {
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/jquery-1.10.2.js");
                AddJavaScriptIncludeInHeader("js/jquery-migrate-1.0.0.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery-ui.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.core.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.button.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.widget.js");

                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.slider.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.dialog.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.menu.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.accordion.js");
                AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.datepicker.js");
            }

            int facilityType = 0; 
            IReportManager reportManager = null;
            ReportScopeManager reportScopeManager =null;
            if (SessionWrapper.Instance.Get != null) {
                reportManager = ReportManagerFactory.Create(SessionWrapper.Instance.Get.Scope, Context.User as CffPrincipal);
                reportScopeManager  = new ReportScopeManager(SessionWrapper.Instance.Get.Scope);
                facilityType = SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType;
            }
            else if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue)!=null){
                reportManager = ReportManagerFactory.Create(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope, Context.User as CffPrincipal);
                reportScopeManager = new ReportScopeManager(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope);
                facilityType = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType;
            }

            AppendQueryStringParameters();

            controlLi.Visible = reportManager.CanViewControlReport() && reportScopeManager.IsControlReportAvailable();
            overdueChargesLi.Visible = reportManager.CanViewOverdueChargesReport();
            agedBalancesLi.Visible = reportManager.CanViewAgedBalances() && reportScopeManager.IsAgedBalancesReportAvailable();

            if (facilityType != 5 && facilityType != 4) // CA & Loan    // dbb
            {
                promptLi.Visible = reportManager.CanViewPromptReport() && reportScopeManager.IsPromptReportAvailable();
                statusLi.Visible = reportManager.CanViewStatusReport();
                unclaimedCreditNotesLi.Visible = reportManager.CanViewUnclaimedCreditNotesReport() &&
                                                 reportScopeManager.IsUnclaimedCreditNotesReportAvailable();
                unclaimedRepurchasesLi.Visible = reportManager.CanViewUnclaimedRepurchasesReport() &&
                                                 reportScopeManager.IsUnclaimedRepurchasesReportAvailable();
                CreditLimitExceededLinkLi.Visible = reportManager.CanViewCreditLimitExceededReport() &&
                                                    reportScopeManager.IsCreditLimitExceededReportAvailable();
                CreditStoppedLinkLi.Visible = reportManager.CanViewCreditLimitExceededReport() &&
                                              reportScopeManager.IsCreditStopSuggestionsReportAvailable();
                CurrentShortPaidLi.Visible = reportManager.CanViewCurrentShortPaidReport() &&
                                             reportScopeManager.IsCurrentShortPaidReportAvailable();
                CurrentOverpaidLi.Visible = reportManager.CanViewCurrentOverpaidReport() &&
                                            reportScopeManager.IsCurrentOverpaidReportAvailable();
                CallsDueLi.Visible = reportManager.CanViewCallsDueReport() &&
                                     reportScopeManager.IsCallsDueReportAvailable();
                ClientActionLi.Visible = reportManager.CanViewClientActionReport() &&
                                         reportScopeManager.IsClientActionReportAvailable();
            }
            else
            {
                if (facilityType == 4)
                {
                    promptLi.Visible = false;
                    statusLi.Visible = false;
                }
                unclaimedCreditNotesLi.Visible = false;
                unclaimedRepurchasesLi.Visible = false;
                CreditLimitExceededLinkLi.Visible = false;
                CreditStoppedLinkLi.Visible = false;
                CurrentShortPaidLi.Visible = false;
                CurrentOverpaidLi.Visible = false;
                CallsDueLi.Visible = false;
                ClientActionLi.Visible = false;
            }


            CustomerWatchLi.Visible = reportManager.CanViewCustomerWatchReport() &&
                                        reportScopeManager.IsCustomerWatchReportAvailable();
            CreditNotesLink.Visible = reportManager.CanViewCreditNotesReport();
            JournalsLink.Visible = reportManager.CanViewJournalsReport();
            CreditBalanceTransfersLink.Visible = reportManager.CanViewCreditBalanceTransfersReport();
            InvoicesLink.Visible = reportManager.CanViewInvoicesReport();
            ReceiptsLink.Visible = reportManager.CanViewReceiptsReport();
            DiscountsLink.Visible = reportManager.CanViewDiscountsReport();
            RepurchasesLi.Visible = reportManager.CanViewRepurchaserasReport() && reportScopeManager.IsRepurchasesReportAvailable();
            
            
            UnallocatedLi.Visible = reportManager.CanViewUnallocatedReport() && reportScopeManager.IsUnallocatedReportAvailable();
            OverpaymentsLi.Visible = reportManager.CanViewOverpaymentsReport();
            StatementLi.Visible = reportManager.CanViewStatementReport() && reportScopeManager.IsStatementsReportAvailable();
            AccountTransLink.Visible = false;
            //reportManager.CanViewAccountTransReport(); //REF: BT#101
            //RetentionReleaseEstimateLi.Visible = reportManager.CanViewRetentionReleaseEstimateReport() &&
            //                                       reportScopeManager.IsRetentionReleaseEstimateReportAvailable();

            switch (facilityType) { 
                case 0:
                    {
                        UnclaimedRepurchasesLink.Text = "Unclaimed Repurch/Prepay";
                        InvoicesLink.Text = "Invoices/Dr Notes";
                        RepurchasesLink.Text = "Repurchases/Prepayments";
                        OverdueChargesLink.Text = "Interest & Charges";
                    }
                    break;

                case 2:
                    {
                        UnclaimedRepurchasesLink.Text = "Unclaimed Prepayments";
                        InvoicesLink.Text = "Invoices";
                        RepurchasesLink.Text = "Prepayments";
                        OverdueChargesLink.Text = "Interest & Charges";
                    }
                    break;

                case 3:
                     {
                        UnclaimedRepurchasesLink.Text = "Unclaimed Reclassifications";
                        InvoicesLink.Text = "Dr Notes/Fees";
                        RepurchasesLink.Text = "Reclassifications";
                        OverdueChargesLink.Text = "Interest & Charges";
                    }
                    break;

                case 4:
                     {
                        UnclaimedRepurchasesLink.Text = "Unclaimed Reclassifications";
                        InvoicesLink.Text = "Advances/Fees";
                        RepurchasesLink.Text = "Reclassifications";
                        OverdueChargesLink.Text = "Interest and Charges";
                    }
                    break;

                case 5:
                    {
                        UnclaimedRepurchasesLink.Text = "Unclaimed Prepayments";
                        InvoicesLink.Text = "Drawings/Fees";
                        RepurchasesLink.Text = "Prepayments";
                        OverdueChargesLink.Text = "Interest and Charges";// "Delayed Payment Fees";
                    }
                    break;

                default:
                     {
                        UnclaimedRepurchasesLink.Text = "Unclaimed Prepayments";
                        InvoicesLink.Text = "Invoices";
                        RepurchasesLink.Text = "Prepayments";
                        OverdueChargesLink.Text = "Interest & Charges";
                    }
                    break;
            }
            //UnclaimedRepurchasesLink.Text = "Unclaimed Repurchases";
        }

        private void AppendQueryStringParameters()
        {
            ControlLink.NavigateUrl = "~/Reports/Control.aspx" + QueryStringParameters;
            AgedBalancesLink.NavigateUrl = "~/Reports/AgedBalances.aspx" + QueryStringParameters;
            OverdueChargesLink.NavigateUrl = "~/Reports/OverdueCharges.aspx" + QueryStringParameters;
            PromptLink.NavigateUrl = "~/Reports/Prompt.aspx" + QueryStringParameters;
            StatusLink.NavigateUrl = "~/Reports/Status.aspx" + QueryStringParameters;

            UnclaimedCreditNotesLink.NavigateUrl = "~/Reports/UnclaimedCreditNotes.aspx" + QueryStringParameters;
            UnclaimedRepurchasesLink.NavigateUrl = "~/Reports/UnclaimedRepurchases.aspx" + QueryStringParameters;
            CreditLimitExceededLink.NavigateUrl = "~/Reports/CreditLimitExceeded.aspx" + QueryStringParameters;
            CreditStoppedLink.NavigateUrl = "~/Reports/CreditStopSuggestions.aspx" + QueryStringParameters;
            CurrentOverpaidLink.NavigateUrl = "~/Reports/CurrentOverpaid.aspx" + QueryStringParameters;
            CallsDueLink.NavigateUrl = "~/Reports/CallsDue.aspx" + QueryStringParameters;
            ClientActionLink.NavigateUrl = "~/Reports/ClientAction.aspx" + QueryStringParameters;
         
            CustomerWatchLink.NavigateUrl = "~/Reports/CustomerWatch.aspx" + QueryStringParameters;
            StatementLink.NavigateUrl = "~/Reports/Statement.aspx" + QueryStringParameters;
            CreditNotesLink.NavigateUrl = "~/Reports/CreditNotes.aspx" + QueryStringParameters;
            JournalsLink.NavigateUrl = "~/Reports/Journals.aspx" + QueryStringParameters;
            CreditBalanceTransfersLink.NavigateUrl = "~/Reports/CreditBalanceTransfers.aspx" + QueryStringParameters;
            InvoicesLink.NavigateUrl = "~/Reports/Invoices.aspx" + QueryStringParameters;
            ReceiptsLink.NavigateUrl = "~/Reports/Receipts.aspx" + QueryStringParameters;
            DiscountsLink.NavigateUrl = "~/Reports/Discounts.aspx" + QueryStringParameters;
            RepurchasesLink.NavigateUrl = "~/Reports/Repurchases.aspx" + QueryStringParameters;
            CurrentShortPaidLink.NavigateUrl = "~/Reports/CurrentShortPaid.aspx" + QueryStringParameters;
            
            UnallocatedLink.NavigateUrl = "~/Reports/Unallocated.aspx" + QueryStringParameters;
            OverpaymentsLink.NavigateUrl = "~/Reports/Overpayments.aspx" + QueryStringParameters;
            AccountTransLink.NavigateUrl = "~/Reports/AccountTransRpt.aspx" + QueryStringParameters;

            //RetentionReleaseEstimateLink.NavigateUrl = "~/Reports/RetentionReleaseEstimate.aspx" + QueryStringParameters;
        }

        public static string CustomerIdQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Customer.ToString()]; }
        }

        public static string ClientIdQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Client.ToString()]; }
        }

        public static string UserQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.User.ToString()]; }
        }

        public static string CriteriaQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Criteria.ToString()]; }
        }

        public static string StartsWithQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.StartsWith.ToString()]; }
        }

        public static string ViewIDQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.ViewID.ToString()]; }
        }

        public static string QueryStringParameters
        {
            get
            {
                if (!string.IsNullOrEmpty(ClientIdQueryString) && !string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString;
                    queryString = "?Client=" + ClientIdQueryString + "&Customer=" + CustomerIdQueryString + "&User=" + UserQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString;
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }
                    return queryString;
                }
                else if (string.IsNullOrEmpty(ClientIdQueryString) && !string.IsNullOrEmpty(CustomerIdQueryString) && SessionWrapper.Instance.Get!=null)
                {
                    string queryString;
                    queryString = "?Client=" + (SessionWrapper.Instance.Get.ClientFromQueryString.Id.ToString()) + "&Customer=" + CustomerIdQueryString + "&User=" + UserQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString;
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }
                    return queryString;
                }


                if (!string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString;
                    queryString = "?Customer=" + CustomerIdQueryString + "&User=" + UserQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString;
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }

                    return queryString;
                }

                if (!string.IsNullOrEmpty(ClientIdQueryString) && string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString;
                    queryString = "?Client=" + ClientIdQueryString + "&User=" + UserQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString;
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }

                    return queryString;
                }

                return "?Client=-1";
            }
        }
    }
}