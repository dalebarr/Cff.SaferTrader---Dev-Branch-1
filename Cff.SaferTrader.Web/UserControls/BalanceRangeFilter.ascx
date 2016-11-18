<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BalanceRangeFilter.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.BalanceRangeFilter" %>

<td>
    <label>
        Between:</label>
    <asp:DropDownList ID="MinBalanceDropDownList" CssClass="toDropDownList" runat="server" />
</td>
<td>   
     <label>
        And:</label>
    <asp:DropDownList ID="MaxBalanceDropDownList" CssClass="toDropDownList" runat="server" />
</td>