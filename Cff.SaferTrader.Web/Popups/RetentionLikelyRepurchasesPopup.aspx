<%@ Page Title="" Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="RetentionLikelyRepurchasesPopup.aspx.cs" Inherits="Cff.SaferTrader.Web.Popups.RetentionLikelyRepurchasesPopup" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<%@ Register TagPrefix="uc" TagName="RetentionDetailsPanel" Src="~/UserControls/ReleaseTabs/RetentionDetailsPanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceholder" runat="server">
    <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" ShowAddress="false" ShowGst="true" />
    <h4>
        &nbsp;Retention Likely Repurchases for <asp:Literal ID="clientNameLiteral" runat="server" />
         &nbsp;for
        Month Ending <asp:Literal ID="EOMLiteral" runat="server" /> 
    </h4>
    
    <p>&nbsp;</p>
    
    <div>
        <uc:CffGenGridView ID="CffGGV_LikelyRepurchasesGridView" runat="server"></uc:CffGenGridView>
    </div>
 
    
    <p><b>Date Printed:&nbsp;&nbsp;&nbsp;<asp:Literal ID="DatePrintedLiteral" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp; </b></p>
    <p><b>© 1998 - 2014 Cashflow Funding Limited</b></p>
         
</asp:Content>
