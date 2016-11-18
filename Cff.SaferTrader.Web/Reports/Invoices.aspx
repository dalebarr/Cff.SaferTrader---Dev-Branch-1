<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Invoices.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.Invoices" Title="Cashflow Funding Limited | Debtor Management | Invoices" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>

<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/TransactionStatusTypesFilter.ascx" TagPrefix="uc" TagName="TransactionStatusTypesFilter" %>

<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc" TagName="AllClientsReportHelpMessage" %>
<%@ Register Src="~/UserControls/ReportPanels/InvoiceTransactionReportPanel.ascx" TagPrefix="uc" TagName="InvoiceTransactionReportPanel" %>
<%@ Register Src="~/UserControls/ReportPanels/CustomerInvoiceTransactionReportPanel.ascx" TagPrefix="uc" TagName="CustomerInvoiceTransactionReportPanel" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader" runat="server">
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
    <uc:PageDescription ID="invoicesPageDescription" DescriptionTitle="" DescriptionContent="Invoices - Invoices presented for factoring within a chosen month.  <a href='#'>more information</a>" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder" runat="server">        
    <div class="parameterSelector" style="width:79.6%">
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
        <table>
            <tbody>
             <tr>
                <td>
                    <uc:DatePicker ID="DatePicker" runat="server" />
                </td>
                <td>
                    <uc:TransactionStatusTypesFilter ID="TransactionStatusTypesFilterControl"  runat="server" />
                </td>
                <td>
                    <uc:AllClientsFilter ID="AllClientsFilter" runat="server" UpdateButtonVisible="false" Visible="false" />
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
                            OnClientClick="startAnimate();" CssClass="updateButton calendarRelatedButton"/>
                </td>
             </tr>
            </tbody>
        </table>
        <span class="error"></span>
         <script type="text/javascript">
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
         </script>
    </div>  

    <div runat="server" id="reportData">  
        <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DatePicker" />
                <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                <asp:PostBackTrigger ControlID="ExportButton" />
            </Triggers>
            <ContentTemplate>
                <uc:InvoiceTransactionReportPanel ID="reportPanel" runat="server" />
                <uc:CustomerInvoiceTransactionReportPanel ID="customerReportPanel" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="buttons" style="height:30px;width:78.6%">
            <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
            </div>
            <div style="float:right;">
                <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png" />
            </div>
        </div>
    </div>
    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage"  Visible="false"></uc:AllClientsReportHelpMessage>

</asp:Content>
