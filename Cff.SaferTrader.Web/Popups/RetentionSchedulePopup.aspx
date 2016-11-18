<%@ Page Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="RetentionSchedulePopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.RetentionSchedulePopup" %>
    
<%@ Register TagPrefix="uc" TagName="RetentionDetailsPanel" Src="~/UserControls/ReleaseTabs/RetentionDetailsPanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceHolder" runat="server">
    <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" ShowAddress="false" ShowGst="true" />
    <h4>
        <asp:Literal ID="RetnStatementTitleLiteral" runat="server" /> &nbsp;of <asp:Literal ID="clientNameLiteral" runat="server" />
    &nbsp;for
        Month Ending <asp:Literal ID="EndOfMonthLiteral" runat="server" /> &nbsp;</h4>
    <div id="retentionDetails" class="scroll">
        <uc:RetentionDetailsPanel ID="retentionDetailsPanel" runat="server" ShowSummary="false" />
    </div>
</asp:Content>

