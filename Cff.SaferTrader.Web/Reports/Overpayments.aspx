<%@ Page Title="Cashflow Funding Limited | Debtor Management | Overpayments" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Overpayments.aspx.cs" Inherits="Cff.SaferTrader.Web.Reports.Overpayments" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>

<%@ Register Src="~/UserControls/ReportPanels/TransactionReportPanel.ascx" TagPrefix="uc" TagName="TransactionReportPanel" %>
<%@ Register Src="~/UserControls/TransactionStatusTypesFilter.ascx" TagPrefix="uc" TagName="TransactionStatusTypesFilter" %>
<%@ Register Src="~/UserControls/ReportPanels/CustomerOverpaymentsTransactionReportPanel.ascx" TagPrefix="uc" TagName="CustomerOverpaymentsTransactionReportPanel" %>
<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc" TagName="AllClientsReportHelpMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
		<h3>
		    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                        <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" 
                        CssClass="informationImage" Height="16px" Width="16px"/>
                    </a>
                    <div id="TitleDiv" runat="server"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
		</h3>
	</div>
	<uc:PageDescription ID="invoicesPageDescription" DescriptionTitle="Overpayments" DescriptionContent="Overpayments - Overpayments for a chosen month.  <a href='#'>more information</a>" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder" runat="server">    
    <script type="text/javascript" src="~/js/ui.1.10.4/jquery-1.10.2.js" ></script>
    <script type="text/javascript" src="~/js/jquery-migrate-1.0.0.js" ></script>
    <script type="text/javascript" src="~/js/jquery.query-object.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery-ui.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.core.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.button.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.widget.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.menu.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.slider.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.dialog.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.accordion.js" ></script>
    <script type="text/javascript" src="~/js/ui.1.10.4/ui/jquery.ui.datepicker.js" ></script>
     <script type="text/javascript" src="~/js/cff.js" ></script>


    <div class="parameterSelector" runat="server" id="parameterSelectorDiv" style="width:69.6%">
        <table>
            <tbody>
               <tr>
                <td>
                    <uc:DatePicker ID="DatePicker" runat="server"/>
                </td>
                <td>
                    <uc:TransactionStatusTypesFilter ID="TransactionStatusTypesFilterControl"  runat="server" />
                </td>
                <td>
                    <uc:AllClientsFilter ID="AllClientsFilter" runat="server" DatePickerVisible="true" UpdateButtonVisible="false" />
                </td>
                <td>
                  <div id="DateRangeDiv" runat="server" style="width:100%;">
                        <div style="width:50%;float:left;padding-right:10px;">
                              From: <asp:TextBox ID="FromDateTextBox" CssClass="fromDateRange" runat="server"  ClientIDMode="AutoID" />
                        </div>
                        <div style="width:45%;float:right;">
                              To: <asp:TextBox ID="ToDateTextBox" CssClass="toDateRange" runat="server" ClientIDMode="AutoID"/>
                        </div>
                  </div>
                </td>
                <td style="padding-left: 7px;">
                    <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                        OnClientClick="startAnimate();" CssClass="updateButton" />
                </td>
                </tr>
            </tbody>
        </table>
    </div>


    <div runat="server" id="reportData">
    <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ViewAllButton" />
            <asp:AsyncPostBackTrigger ControlID="DatePicker" />
            <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            <asp:PostBackTrigger ControlID="ExportButton" />
            
        </Triggers>
        <ContentTemplate>
            <uc:TransactionReportPanel ID="ReportPanel" runat="server" />
            <uc:CustomerOverpaymentsTransactionReportPanel ID="customerReportPanel" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="buttons"  style="height:30px;width: 68.6%">
          <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
         </div>
         <div style="float:right;">
            <uc:ViewAllButton ID="ViewAllButton" runat="server" OnClick="ViewAllButton_Click" />
            <asp:ImageButton ID="ExportButton" OnClick="ExportButton_Click" runat="server" AlternateText="Export"
                ImageUrl="~/images/btn_export.png" />
         </div>
    </div>

     </div>
    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage"  Visible="false"></uc:AllClientsReportHelpMessage>

    <script type="text/javascript">
         //do not remove this part - needed as datepicker do not get reattached during postbacks
         Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
             var fromdateRangeOptions = {
                 dateFormat: 'dd/mm/yy',
                 showOn: 'both',
                 buttonImage: relativePathToRoot + 'images/calendar.png',
                 buttonImageOnly: true,
                 buttonText: "Click to select a date",
                 goToCurrent: true,
                 clickInput: true,
                 yearRange: "-30:+0",
                 maxDate: "+0M +0D",
                 prevText: "",
                 nextText: "",
                 changeMonth: true,
                 changeYear: true,
                 showMonthAfterYear: true
             };

             var todateRangeOptions = {
                 dateFormat: 'dd/mm/yy',
                 showOn: 'both',
                 buttonImage: relativePathToRoot + 'images/calendar.png',
                 buttonImageOnly: true,
                 buttonText: "Click to select a date",
                 goToCurrent: true,
                 clickInput: true,
                 yearRange: "-30:+0",
                 maxDate: "+0M +0D",
                 prevText: "",
                 nextText: "",
                 changeMonth: true,
                 changeYear: true,
                 showMonthAfterYear: true,
                 hideIfNoPrevNext: true
             };

             $("#<%= FromDateTextBox.ClientID %>").datepicker(fromdateRangeOptions);
             $("#<%= ToDateTextBox.ClientID %>").datepicker(todateRangeOptions);
         });
         //do not remove this part - ref. Mariper
     </script>
</asp:Content>
