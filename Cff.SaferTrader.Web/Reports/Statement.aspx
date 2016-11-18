<%@ Page Title="Cashflow Funding Limited | Debtor Management | Statement" Language="C#" MasterPageFile="~/Reports.Master"
    AutoEventWireup="true" CodeBehind="Statement.aspx.cs" Inherits="Cff.SaferTrader.Web.Reports.Statement" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/ManagementDetailsBox.ascx" TagPrefix="uc" TagName="ManagementDetailsBox" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/ReportPanels/StatementReportPanel.ascx" TagPrefix="uc" TagName="StatementReportPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"
                    CssClass="informationImage" Height="16px" Width="16px"/>
            </a>Statement <% =targetName %>
        </h3>
    </div>
    <uc:PageDescription ID="repurchaseTransactionsPageDescription" DescriptionTitle="" DescriptionContent="Statement"
        runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder" runat="server">
    <div class="statementReport">
        <div class="parameterSelector">
            <table>
                <tbody>
                   <tr>
                       <td>
                             <uc:DatePicker ID="DatePicker" runat="server" />
                       </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div>
             <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DatePicker" />
                </Triggers>
                <ContentTemplate>
                     <uc:StatementReportPanel id="stReportPanel" runat="server" EnableViewState="true" ClientIDMode="Inherit" />                
                 </ContentTemplate>
            </asp:UpdatePanel>
        </div>
     

         <div class="buttons"  style="height:30px;">
            <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
            </div>
            <div style="float:right;">
                <asp:ImageButton ID="ExportButton" runat="server" OnClick="PrintButton_Click" AlternateText="Print" ImageUrl="~/images/btn_print_new.png" />
            </div>
        </div>
    </div>
</asp:Content>