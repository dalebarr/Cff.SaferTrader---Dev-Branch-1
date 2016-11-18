<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromptReportPanel.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.PromptReportPanel" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<%--<div class="scroll">--%>
<div>    
    <asp:PlaceHolder ID="reportPlaceholder" runat="server"/>
</div>
<div class="summary clearfix">
    <dl style="width: 200px">
        <dt class="heading">Funded Balance</dt>
        <dd>
            <asp:Literal ID="FundedBalanceLiteral" runat="server" />
        </dd>
    </dl>
    <dl style="width: 200px">
        <dt class="heading">Non Funded Balance</dt>
        <dd>
            <asp:Literal ID="NonFundedBalance" runat="server" />
        </dd>
    </dl>
</div>
