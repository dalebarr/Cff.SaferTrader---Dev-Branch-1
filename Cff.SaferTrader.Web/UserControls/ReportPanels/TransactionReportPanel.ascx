<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionReportPanel.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.TransactionReportPanel" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<%--<div class="scroll">--%>
<div>
    <asp:PlaceHolder ID="reportPlaceholder" runat="server" />
</div>
<div class="summary clearfix">
    <dl style="width:200px">
        <dt class="heading">Funded Total</dt>
        <dd>
            <asp:Literal ID="FundedTotalLiteral" runat="server" />
        </dd>
    </dl>
    <dl style="width:200px">
        <dt class="heading">Non Funded Total</dt>
        <dd>
            <asp:Literal ID="NonFundedTotalLiteral" runat="server" />
        </dd>
    </dl>
</div>
