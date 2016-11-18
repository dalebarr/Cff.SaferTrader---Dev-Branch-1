<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionStatusTypesFilter.ascx.cs"
         Inherits="Cff.SaferTrader.Web.UserControls.TransactionStatusTypesFilter" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<td>
    <label>Transaction Status:</label>
    <asp:DropDownList ID="TransactionStatusTypeDropDownList" runat="server"  OnSelectedIndexChanged="TransactionStatusTypeDropDownList_SelectedIndexChanged">
        <asp:ListItem>All</asp:ListItem>
        <asp:ListItem>Funding</asp:ListItem>
        <asp:ListItem>Non-Funding</asp:ListItem>
    </asp:DropDownList>
    
</td>
