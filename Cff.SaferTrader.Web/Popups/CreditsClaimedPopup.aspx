<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="CreditsClaimedPopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.CreditsClaimedPopup" %>
    
<%@ Register TagPrefix="uc" TagName="RetentionDetailsPanel" Src="~/UserControls/ReleaseTabs/RetentionDetailsPanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>

<%@ Register assembly="Cff.SaferTrader.Web" namespace="Cff.SaferTrader.Web.UserControls" tagprefix="cc1" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" ShowAddress="false" ShowGst="true" />
    <h4>
        <asp:Literal ID="RetnHeaderLiteral" runat="server" />
        &nbsp;- Credits Claimed of <asp:Literal ID="clientNameLiteral" runat="server" />
        &nbsp; for
        Month Ending <asp:Literal ID="EOMLiteral" runat="server" /> &nbsp;</h4>
    <div>
        <div style="border:none;border-width:0px;">
            <uc:CffGenGridView ID="CffGGV_CreditsClaimed" runat="server" BorderStyle="Solid" ></uc:CffGenGridView>
        </div>
    
        <div style="border:none;border-width:0px;">
            <b>Date Printed:&nbsp;&nbsp;&nbsp; 
                        <asp:Literal ID="DatePrintedLiteral" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp; 
            <br />
            </b>
        </div>
    
    </div>
    <p>
        <b id="cffGridCred">© 1998 -  <asp:Literal ID="YearLiteral" runat="server" /> Cashflow Funding Limited</b>
    </p>
</asp:Content>

