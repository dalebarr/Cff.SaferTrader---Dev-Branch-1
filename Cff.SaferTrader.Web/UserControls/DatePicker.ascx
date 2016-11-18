<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatePicker.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.DatePicker" %>

<div style="float:left;display:inline;">
    <label>Show for month ending:</label>
    <asp:DropDownList ID="ToDropDownList" runat="server" OnSelectedIndexChanged="ToDropDownList_SelectedIndexChanged" />
</div>
