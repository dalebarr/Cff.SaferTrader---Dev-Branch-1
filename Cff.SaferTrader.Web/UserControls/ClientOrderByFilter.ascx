<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientOrderByFilter.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ClientOrderByFilter" %>
<td>
    <label>
        Order by:</label>
    <asp:DropDownList ID="ClientOrderByFilterDropDownList" 
    CssClass="toDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ClientOrderByFilterDropDownList_SelectedIndexChanged" />
</td>