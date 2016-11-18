<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true"
    CodeBehind="LettersPopup.aspx.cs" Inherits="Cff.SaferTrader.Web.Popups.LettersPopup" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls"
    TagPrefix="uc" %>
<%@ Register assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.DynamicData" tagprefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
    <iframe id="frameDetails" width="100%" height="980" scrolling= "no"  runat="server" frameborder="0"> </iframe> 
</asp:Content>

