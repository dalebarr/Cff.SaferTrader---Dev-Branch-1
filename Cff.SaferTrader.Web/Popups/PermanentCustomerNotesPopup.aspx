<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true"
    CodeBehind="PermanentCustomerNotesPopup.aspx.cs" Inherits="Cff.SaferTrader.Web.Popups.PermanentCustomerNotesPopup" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls"  TagPrefix="uc"%>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
    Permanent Notes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <div class="scroll">
        <uc:CffGenGridView ID="notesGridView" runat="server" >
        </uc:CffGenGridView>
    </div>
    
     <div style="border:none;border-width:0px;">
        <br />
        <p>
           <asp:Label ID="lblDatePrinted" runat="server"/>
           <br />
           <asp:Label ID="lblCopyRight" runat="server" />
        </p>
    </div>
</asp:Content>


