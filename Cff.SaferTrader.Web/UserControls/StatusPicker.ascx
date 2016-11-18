<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatusPicker.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.StatusPicker" %>
<div style="float:left;width:auto;height:auto;">
    <label> Select status:</label>
    <asp:DropDownList ID="StatusSelectorDropDownList" runat="server" OnSelectedIndexChanged="StatusSelectorDropDownList_SelectedIndexChanged" />
</div>
