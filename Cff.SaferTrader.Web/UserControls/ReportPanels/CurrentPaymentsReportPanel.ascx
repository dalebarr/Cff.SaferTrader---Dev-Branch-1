<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrentPaymentsReportPanel.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.CurrentPaymentsReportPanel" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<div>
    <asp:PlaceHolder ID="reportPlaceholder" runat="server"/>    
</div>
<div class="summary clearfix">
    <dl style="width: 200px;">
        <dt class="heading">Funded Total</dt>
        <dd>
            <asp:Literal ID="FundedTotalLiteral" runat="server" />
        </dd>
    </dl>
    <dl style="width: 200px;">
        <dt class="heading">Non Funded Total</dt>
        <dd>
            <asp:Literal ID="NonFundedTotalLiteral" runat="server" />
        </dd>
    </dl>
</div>
