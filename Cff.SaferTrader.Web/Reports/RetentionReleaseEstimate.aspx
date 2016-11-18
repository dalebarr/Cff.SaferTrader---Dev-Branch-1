<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="RetentionReleaseEstimate.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.RetentionReleaseEstimate" Title="Cashflow Funding Limited | Debtor Management | Retention Release Estimate" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/ReportPanels/RetentionReleaseEstimateReportPanel.ascx" TagPrefix="uc" TagName="RetentionReleaseEstimateReportPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
		<h3>
			<a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"
                CssClass="informationImage" Height="16px" Width="16px"/>
            </a>
			Retention Release Estimate <% =targetName %>
        </h3>
	</div>
	<uc:PageDescription ID="PageDescription" DescriptionTitle="" DescriptionContent="Retention Release Estimate"
        runat="server" />    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder"
    runat="server">    
    <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ViewAllButton" />
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            <asp:PostBackTrigger ControlID="ExportButton" />
        </Triggers>
        <ContentTemplate>            
            <uc:RetentionReleaseEstimateReportPanel id="ReportPanel" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="buttons"  style="height:30px;">
         <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
         </div>
         <div style="float:right;">
            <uc:ViewAllButton ID="ViewAllButton" runat="server" OnClick="ViewAllButton_Click" />
            <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png" />
         </div>
    </div>
</asp:Content>
