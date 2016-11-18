<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Control.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.Control" Title="Cashflow Funding Limited | Debtor Management | Control" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/ReportPanels/ControlReportPanel.ascx" TagPrefix="uc" TagName="ControlReportPanel" %>
<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc"  TagName="AllClientsReportHelpMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png"
                    AlternateText="Show page info" CssClass="informationImage" Height="16px" Width="16px"/>
            </a>Control <% =targetName %>
        </h3>
    </div>
    <uc:PageDescription ID="pageDescription" DescriptionTitle="" DescriptionContent="Control Report" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder" runat="server">
    <div class="parameterSelector" style="width:70%;position:relative;height:25px;vertical-align:top;">
        <table style="width:100%;vertical-align:top;">
            <tr style="width:100%;vertical-align:top;">
                <td style="width:12%;vertical-align:top;">
                    <span style="padding-top:0px;"><uc:DatePicker ID="DatePicker" runat="server" /></span>
                </td>
                <td style="width:90%;vertical-align:top;">
                    <span style="padding-top:0px;"><uc:AllClientsFilter ID="allClientsFilter" runat="server" Visible="false" /></span>
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="reportData"  style="width:70%;position:relative;">
        <div style="min-width:800px;vertical-align:top;">
              <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DatePicker" />
                    <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                    <asp:AsyncPostBackTrigger ControlID="allClientsFilter" />
                    <asp:PostBackTrigger ControlID="ExportButton" />
                </Triggers>
                <ContentTemplate>
                    <div style="width:100%;table-layout:fixed;">
                          <uc:ControlReportPanel ID="ReportPanel" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="buttons" style="width:100%;position:relative;padding-right:20px;height:30px;">
            <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server"  /></span>
            </div>
            <div style="float:right;">
                <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export"
                ImageUrl="~/images/btn_export.png" />
            </div>
        </div>
        <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage"  Visible="false"></uc:AllClientsReportHelpMessage>
    </div>
</asp:Content>
