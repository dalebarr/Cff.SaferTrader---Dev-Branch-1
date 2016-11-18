<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="CustomerNotesPopup.aspx.cs" Inherits="Cff.SaferTrader.Web.Popups.CustomerNotesPopup" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls"   TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
    <asp:Literal ID="HeaderLiteral" runat="server"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceholder" runat="server">
    <div class="scroll" style="border:none;border-width:0px;">
        <asp:PlaceHolder ID="notesGridViewPlaceHolder" EnableViewState="true" runat="server" />
    </div>
    <div style="border:none;border-width:0px;">
<%--        <br />--%>
        <p>
             <asp:Label ID="lblDatePrinted" runat="server"/>
            <br />
            <asp:Label ID="lblCopyRight" runat="server" />
        </p>
    </div>
</asp:Content>
