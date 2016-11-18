<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" 
Inherits="Cff.SaferTrader.Web.Reports.Default" Title="Cashflow Funding Limited | Debtor Management | Reports" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            Reports <%=targetName %>
        </h3>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder" runat="server">
    <div id="noReportData" runat="server" class="instruction">
        <asp:Literal ID="noReportDataLiteral" runat="server" Text="Please select a report."></asp:Literal>
    </div>
</asp:Content>
