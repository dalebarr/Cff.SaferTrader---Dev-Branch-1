<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreditsClaimed.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.CreditsClaimed" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<div ID="divCreditsClaimedPC" style="overflow:hidden;">
    <asp:PlaceHolder ID="CCGridViewPlaceHolder" runat="server"></asp:PlaceHolder>
</div>
