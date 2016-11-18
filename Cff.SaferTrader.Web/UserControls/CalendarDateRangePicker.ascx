<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarDateRangePicker.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.CalendarDateRangePicker" %>

<div style="display:table">
    <div class="fromDate" style="display:table-cell">
        <label for="<%=FromDateTextBox.ClientID %>">
            From:</label>
        <asp:TextBox ID="FromDateTextBox" CssClass="fromDateRange" runat="server"  ClientIDMode="AutoID" />
    </div>
    <div class="toDate" style="display:table-cell">
        <label for="<%=ToDateTextBox.ClientID %>">
            To:</label>
        <asp:TextBox ID="ToDateTextBox" CssClass="toDateRange" runat="server" ClientIDMode="AutoID"/>
    </div>
</div>



