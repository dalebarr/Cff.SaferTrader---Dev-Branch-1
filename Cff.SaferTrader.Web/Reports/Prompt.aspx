<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Prompt.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.Prompt" Title="Cashflow Funding Limited | Debtor Management | Prompt" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/PromptDaysPicker.ascx" TagPrefix="uc" TagName="PromptDaysPicker" %>
<%@ Register Src="~/UserControls/ReportPanels/PromptReportPanel.ascx" TagPrefix="uc" TagName="PromptReportPanel" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc" TagName="AllClientsReportHelpMessage" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png"
                    AlternateText="Show page info" CssClass="informationImage" Height="16px" Width="16px"/>
            </a>Prompt <% =targetName %>
        </h3>
    </div>
    <uc:PageDescription ID="promptPageDescription" DescriptionTitle="" DescriptionContent="Prompt"
        runat="server" />
</asp:Content>
<asp:Content ID="ReportContent" ContentPlaceHolderID="ReportViewerContentPlaceholder"
    runat="server">
    <div class="parameterSelector" style="width:79.5%;">
        <table>
            <tr style="float:left;">
                <td>
                    <uc:PromptDaysPicker ID="PromptDaysPicker" runat="server" />
                </td>
                <td>
                    <uc:AllClientsFilter ID="allClientsFilter" runat="server" UpdateButtonVisible="false" />
                </td>
                <td>
                   <span style="display:inline-block"><asp:CheckBox ID="IsFactoredCheckBox" runat="server" Checked="true" Text="" textalign="Left"/> Show only factored invoices</span>
                </td>
                <td>
                    <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="70px" Height="25px"
                        OnClientClick="startAnimate();" CssClass="updateButton" OnClick="UpdateButton_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="reportData">
        <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="PromptDaysPicker" />
                <asp:AsyncPostBackTrigger ControlID="IsFactoredCheckBox" />
                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                <asp:PostBackTrigger ControlID="ExportButton" />
            </Triggers>
            <ContentTemplate>
                <uc:PromptReportPanel ID="ReportPanel" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="buttons" style="height:30px;width:78.6%">
            <div style="float:left;">
                <span>Date Viewed : 
                    <asp:Literal ID="DateViewedLiteral" runat="server" />
                </span>
            </div>
            <div style="float:right;">
            <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export"
                ImageUrl="~/images/btn_export.png" />
            </div>
       </div>
    </div>
    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage" Visible="false">
    </uc:AllClientsReportHelpMessage>
</asp:Content>
