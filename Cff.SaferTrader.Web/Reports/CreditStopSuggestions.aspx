<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="CreditStopSuggestions.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.CreditStopSuggestions" Title="Cashflow Funding Limited | Debtor Management | Credit Stop Suggestions" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>

<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc" TagName="AllClientsReportHelpMessage" %>
<%@ Register Src="~/UserControls/ReportPanels/CreditStopSuggestionsReportPanel.ascx" TagPrefix="uc" TagName="CreditStopSuggestionsReportPanel" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png"
                    AlternateText="Show page info" Height="16px" Width="16px"/>
            </a>Credit Stop Suggestions <% =targetName %>
        </h3>
    </div>
    <uc:PageDescription ID="pageDescription" DescriptionTitle="Credit Stop Suggestions"
        DescriptionContent="Credit Stop Suggestions" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder"
    runat="server">
    <div class="parameterSelector" runat="server" id="parameterSelectorDiv" visible="false">
        <table>
            <tr>
                <uc:AllClientsFilter ID="AllClientsFilter" runat="server" />
            </tr>
        </table>
    </div>
    <div runat="server" id="reportData">
        <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                <asp:PostBackTrigger ControlID="ExportButton" />
                <asp:AsyncPostBackTrigger ControlID="AllClientsFilter" />
            </Triggers>
            <ContentTemplate>
                <uc:CreditStopSuggestionsReportPanel ID="ReportPanel" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="buttons" style="height:30px;">
            <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
           </div>
            <div style="float:right;">
                <asp:ImageButton ID="ExportButton" OnClick="ExportButton_Click" runat="server" AlternateText="Export"
                ImageUrl="~/images/btn_export.png" />
            </div>
        </div>
    </div>

    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage" Visible="false">
    </uc:AllClientsReportHelpMessage>
</asp:Content>
