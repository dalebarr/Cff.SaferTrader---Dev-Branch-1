<%@ Page Title="Cashflow Funding Limited | Debtor Management | Repurchase Transactions"
    Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Repurchases.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.Repurchases" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/TransactionStatusTypesFilter.ascx" TagPrefix="uc" TagName="TransactionStatusTypesFilter" %>
<%@ Register Src="~/UserControls/ReportPanels/TransactionReportPanel.ascx" TagPrefix="uc" TagName="TransactionReportPanel" %>
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
	<uc:PageDescription ID="repurchaseTransactionsPageDescription" DescriptionTitle=""
        DescriptionContent="Prepayments Transactions - Prepayments made in a chosen month.  <a href='#'>more information</a>" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder"
    runat="server">    
    <div class="parameterSelector" style="width: 69.6%">
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
                        <uc:AllClientsFilter ID="AllClientsFilter" runat="server" DatePickerVisible="false" UpdateButtonVisible="false" ShowAllKnownFacilityTypes="false" />
                    </td>
                    <td>
                        <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                            OnClientClick="startAnimate();" CssClass="updateButton" OnClick="UpdateButtonClick" />
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
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            <asp:PostBackTrigger ControlID="ExportButton" />
        </Triggers>
        <ContentTemplate>
            <uc:TransactionReportPanel ID="ReportPanel" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="buttons" style="height:30px; width:68.6%">
          <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
          </div>
          <div style="float:right;">
            <uc:ViewAllButton ID="ViewAllButton" runat="server" OnClick="ViewAllButton_Click" />
            <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export"
                ImageUrl="~/images/btn_export.png" />
          </div>
    </div>

     </div>
    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage"  Visible="false"></uc:AllClientsReportHelpMessage>
</asp:Content>
