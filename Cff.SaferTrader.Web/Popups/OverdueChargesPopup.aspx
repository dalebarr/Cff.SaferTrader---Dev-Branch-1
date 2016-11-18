<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="OverdueChargesPopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.OverdueChargesPopup" %>
    
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" ShowAddress="false" ShowGst="true" />
    <h4>
        <asp:Literal ID="RetnHeaderLiteral" runat="server" />
        &nbsp;- Interest & Charges of <asp:Literal ID="clientNameLiteral" runat="server" />
        &nbsp; for
        Month Ending <asp:Literal ID="EOMLiteral" runat="server" /> </h4>
    <div>
       <uc:CffGenGridView ID="CffGridOverdueCharges" runat="server"></uc:CffGenGridView>
    </div>
    <p>
        &nbsp;
    </p>
    <p>
        &nbsp;
    </p>
    <p>
        <b>Date Printed:&nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="DatePrintedLiteral" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp; </b>
    </p>
    <p>
        <b>© 1998 - <asp:Literal ID="YearLiteral" runat="server" /> Cashflow Funding Limited</b>
    </p>
 </asp:Content>

