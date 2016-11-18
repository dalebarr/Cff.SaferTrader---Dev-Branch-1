<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Journals.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.Journals" Title="Cashflow Funding Limited | Debtor Management | Journals" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/ReportPanels/TransactionReportPanelContainer.ascx" TagPrefix="uc" TagName="TransactionReportPanelContainer" %>

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
	<uc:PageDescription ID="journalsPageDescription" DescriptionTitle="" DescriptionContent="Journals - Journal transactions for a chosen month.  <a href='#'>more information</a>" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder"
    runat="server">
	<uc:TransactionReportPanelContainer id="reportPanelContainer" runat="server" />
</asp:Content>
