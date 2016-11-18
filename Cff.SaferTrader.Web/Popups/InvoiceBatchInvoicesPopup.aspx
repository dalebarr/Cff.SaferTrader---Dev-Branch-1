<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="InvoiceBatchInvoicesPopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.InvoiceBatchInvoicesPopup" Title="Invoices" %>

<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>
<%@ Register Src="~/UserControls/ReleaseTabs/InvoiceBatchHeader.ascx" TagPrefix="uc" TagName="InvoiceBatchHeader" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceHolder" runat="server">
    Invoices
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <uc:InvoiceBatchHeader ID="header" runat="server" />
    <div class="scroll" style="border:none;border-width:0px;">
        <uc:CffGenGridView ID="CffGGV_InvoiceGridView" runat="server"></uc:CffGenGridView>
    </div>
    <div style="border:none;border-width:0px;">
        <b>Date Printed: </b>
        <asp:Literal ID="DatePrintedLiteral" runat="server" />
        <br/>
        <b>© 1998 - 2014 Cashflow Funding Limited</b>
    </div>
</asp:Content>
