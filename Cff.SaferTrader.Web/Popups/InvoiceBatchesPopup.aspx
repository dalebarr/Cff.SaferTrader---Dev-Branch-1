<%@ Page Title="" Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="InvoiceBatchesPopup.aspx.cs" Inherits="Cff.SaferTrader.Web.Popups.InvoiceBatchesPopup" %>

<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>
<%@ Register Src="~/UserControls/ReleaseTabs/InvoiceBatchHeader.ascx" TagPrefix="uc" TagName="InvoiceBatchHeader" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceholder" runat="server">
    <div>
         <b>Invoice Batches : </b>
        <asp:Literal ID="ClientNameLiteral" runat="server" />
        <br />
    </div>
     <div class="scroll"  style="border-width:0px;border:none;">
        <uc:cffgengridview ID="CffGGV_InvoiceBatchesGridView" runat="server" EnableViewstate="true"></uc:cffgengridview>
    </div>
    <div style="border-width:0px;border:none;">
        <b>Date Printed: </b>
        <asp:Literal ID="DatePrintedLiteral" runat="server" />
        <br />
        <b>© 1998 - 2014 Cashflow Funding Limited</b>
    </div>
</asp:Content>
