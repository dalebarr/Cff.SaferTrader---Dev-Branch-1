<%@ Page Title="" Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="RetnAccountTransactionsPopup.aspx.cs" Inherits="Cff.SaferTrader.Web.Popups.RetnAccountTransactionsPopup" %>
   
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>      
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceholder" runat="server">
    <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" ShowAddress="false" ShowGst="true" />
    <h4>
        &nbsp;Retention Account Transactions for <asp:Literal ID="clientNameLiteral" runat="server" />
         &nbsp;for
        Month Ending <asp:Literal ID="EOMLiteral" runat="server" /> 
    </h4>
    
    <p>&nbsp;</p>
    
    <div>
        <uc:CffGenGridView ID="CffGGV_GridAccountTransactions" runat="server"></uc:CffGenGridView>
        <p><b>Closing Balance:  <span style="text-decoration:underline;"><asp:Literal ID="ClosingBalanceLiteral" runat="server" /></span></b> <br />
            <b>Movement:  <span style="text-decoration:underline;border-bottom:1px solid;"><asp:Literal ID="MovementLiteral" runat="server" /></span></b>
        </p>
    </div>

    
    
    <p><b>Date Printed:&nbsp;&nbsp;&nbsp;<asp:Literal ID="DatePrintedLiteral" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp; </b></p>
    <p><b>© 1998 - <asp:Literal ID="YearLiteral" runat="server" /> Cashflow Funding Limited</b></p>
         
</asp:Content>
