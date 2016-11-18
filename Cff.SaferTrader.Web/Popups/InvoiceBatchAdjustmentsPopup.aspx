<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="InvoiceBatchAdjustmentsPopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.InvoiceBatchAdjustmentsPopup" Title="Adjustments" %>

<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>
<%@ Register Src="~/UserControls/ReleaseTabs/InvoiceBatchHeader.ascx" TagPrefix="uc" TagName="InvoiceBatchHeader" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
    Adjustments
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceholder" runat="server">

    <uc:InvoiceBatchHeader id="header" runat="server" />
    <div class="scroll"  style="border-width:0px;border:none;">
        <uc:CffGenGridView ID="CffGGV_BatchChargesGridView" runat="server" EnableViewstate="true"></uc:CffGenGridView>
    </div>
    <div style="border-width:0px;border:none;">
        <b>Date Printed: </b>
        <asp:Literal ID="DatePrintedLiteral" runat="server" />
        <br />
        <b>© 1998 - 2014 Cashflow Funding Limited</b>
    </div>

</asp:Content>
