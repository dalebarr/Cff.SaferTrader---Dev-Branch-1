<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgedBalancesReportPanel.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.AgedBalancesReportPanel" %>
    
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<div class="scroll">                
    <asp:PlaceHolder ID="reportPlaceholder" runat="server" />
</div>
<div class="dateViewed clearfix">
    <p>
        <span>Date Viewed</span>
        <asp:Literal ID="DateViewedLiteral" runat="server" /></p>
        <asp:Literal ID="IsReportWithNotesLiteral" runat="server" Visible="false" />
</div>