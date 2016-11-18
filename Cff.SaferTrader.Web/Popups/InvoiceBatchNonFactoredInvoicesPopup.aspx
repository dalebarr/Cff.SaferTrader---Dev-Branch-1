<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="InvoiceBatchNonFactoredInvoicesPopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.InvoiceBatchNonFactoredInvoicesPopup" Title="Non Factored" %>

<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>
<%@ Register Src="~/UserControls/ReleaseTabs/InvoiceBatchHeader.ascx" TagPrefix="uc" TagName="InvoiceBatchHeader" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
    Non Funding
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <uc:InvoiceBatchHeader ID="header" runat="server" />
    <div class="scroll">
        <uc:CffGenGridView ID="CffGGV_InvoiceGridView" runat="server"></uc:CffGenGridView>        
    </div>
    <div>
        <b>Date Printed: </b>
        <asp:Literal ID="DatePrintedLiteral" runat="server" />
        <br />
        <b>© 1998 -  <asp:Literal ID="YearLiteral" runat="server" /> Cashflow Funding Limited
        </b>
    </div>
</asp:Content>
