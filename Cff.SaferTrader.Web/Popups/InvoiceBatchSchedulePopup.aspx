<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="InvoiceBatchSchedulePopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.InvoiceBatchSchedulePopup" Title="Schedule" %>

<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/BatchScheduleDetails.ascx" TagName="BatchScheduleDetails" %>
<%@ Register Src="~/UserControls/ReleaseTabs/InvoiceBatchHeader.ascx" TagPrefix="uc" TagName="InvoiceBatchHeader" %>
<%@ Register TagPrefix="uc" TagName="BatchSchedulePanel" Src="~/UserControls/ReleaseTabs/BatchSchedulePanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" ShowName="false" ShowAddress="false" ShowGst="true" />
    <uc:InvoiceBatchHeader id="header" runat="server" />
    <uc:BatchSchedulePanel id="batchSchedulePanel" runat="server" />
</asp:Content>
