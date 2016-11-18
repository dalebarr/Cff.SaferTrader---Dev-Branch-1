<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="InvoiceBatchCreditsPopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.InvoiceBatchCreditsPopup" Title="Credits" %>

<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>
<%@ Register Src="~/UserControls/ReleaseTabs/InvoiceBatchHeader.ascx" TagPrefix="uc" TagName="InvoiceBatchHeader" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
    Credits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <uc:InvoiceBatchHeader ID="header" runat="server" />
    <div class="scroll">
        <uc:CffGenGridView ID="CffGGV_ChargesGridView" runat="server" EnableViewstate="true"></uc:CffGenGridView>
    </div>
    <div>
        <b>Date Printed: 
        <asp:Literal ID="DatePrintedLiteral" runat="server" />
        <br />
        <b>© 1998 - 2014 Cashflow Funding Limited</b>
    </div>
</asp:Content>
