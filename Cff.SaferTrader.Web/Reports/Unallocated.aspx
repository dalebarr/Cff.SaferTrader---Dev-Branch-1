<%@ Page Title="Cashflow Funding Limited | Debtor Management | Unallocated"
    Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Unallocated.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.Unallocated" %>
    <%@ MasterType VirtualPath="~/Reports.master" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/ReportPanels/TransactionReportPanel.ascx" TagPrefix="uc"
    TagName="TransactionReportPanel" %>
    <%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc"
    TagName="AllClientsReportHelpMessage" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
	<div id="contentHeader">
		<h3>
			<a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"
                CssClass="informationImage" Height="16px" Width="16px"/>
            </a>
			Unallocated <% =targetName %>
        </h3>
	</div>
	<uc:PageDescription ID="invoicesPageDescription" DescriptionTitle="Unallocated"
        DescriptionContent="Unallocated - Unallocated receipts for a chosen month.  <a href='#'>more information</a>" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder"
    runat="server"> 
    <div class="parameterSelector" runat="server" id="parameterSelectorDiv">
        <table>
            <tr>
                <uc:AllClientsFilter ID="AllClientsFilter" runat="server" DatePickerVisible="false" UpdateButtonVisible="false" />
                <td>
                    <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                        OnClientClick="startAnimate();" CssClass="updateButton" OnClick="UpdateButtonClick" />
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="reportData">
    <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ViewAllButton" />
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            <asp:PostBackTrigger ControlID="ExportButton" />
        </Triggers>
        <ContentTemplate>
            <uc:TransactionReportPanel ID="ReportPanel" runat="server" />
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
</asp:Content>
