<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OverdueCharges.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Reports.OverdueCharges" MasterPageFile="~/Reports.Master"
    Title="Cashflow Funding Limited | Debtor Management | Interest & Charges" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/StatusPicker.ascx" TagPrefix="uc" TagName="StatusPicker" %>
<%@ Register Src="~/UserControls/ReportPanels/OverdueChargesReportPanel.ascx" TagPrefix="uc" TagName="OverdueChargesReportPanel" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc" TagName="AllClientsReportHelpMessage" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png"
                    AlternateText="Show page info" CssClass="informationImage" Height="16px" Width="16px"/>
            </a>Interest & Charges <% =targetName %>
        </h3>
    </div>
    <uc:PageDescription ID="overdueChargesPageDescription" DescriptionTitle="" DescriptionContent="Interest & Charges"
        runat="server" />
</asp:Content>
<asp:Content ID="ReportContent" ContentPlaceHolderID="ReportViewerContentPlaceholder"
    runat="server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <%--<div class="parameterSelector" style="width:65.6%">--%>
                <div class="parameterSelector" style="width:67.6%">
                        <table>
                             <tbody>
                                  <tr>
                                <td>
                                   <uc:DatePicker ID="DatePicker" runat="server" EnableAutoPostBack="false" />
                                </td>
                                <td>
                                   <uc:StatusPicker ID="StatusPicker" runat="server" EnableAutoPostBack="false" />
                                </td>
                                <td>
                                   <uc:AllClientsFilter ID="allClientsFilter" runat="server" UpdateButtonVisible="false" Visible="false" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif"
                                    CssClass="updateButton"  OnClick="UpdateButton_Click" Width="80px" Height="25px"
                                                OnClientClick="startAnimate();"   />
                                </td>
                                 </tr>
                             </tbody>
                     </table>
                 </div>
            </ContentTemplate>
    </asp:UpdatePanel>
    <div runat="server" id="reportData">
        <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DatePicker" />
                <asp:AsyncPostBackTrigger ControlID="StatusPicker" />
                <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
                <asp:PostBackTrigger ControlID="ExportButton" />
            </Triggers>
            <ContentTemplate>
                <uc:OverdueChargesReportPanel ID="ReportPanel" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="buttons" style="height:30px;margin-top:2px;width:66.6%">
            <div style="float:left;">
                <span>Date Viewed :
                    <asp:Literal ID="DateViewedLiteral" runat="server" />
                </span>
            </div>
            <div style="float:right;">
                <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png" />
            </div>
        </div>
    </div>
    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage" Visible="false"></uc:AllClientsReportHelpMessage>
</asp:Content>
