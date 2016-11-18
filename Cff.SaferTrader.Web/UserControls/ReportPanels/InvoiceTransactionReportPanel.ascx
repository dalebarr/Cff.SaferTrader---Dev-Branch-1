<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceTransactionReportPanel.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.InvoiceTransactionReportPanel" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>
<%--<div class="scroll">--%>
<div>    
    <asp:PlaceHolder ID="reportPlaceholder" runat="server" />
</div>
<div class="summary clearfix">
    <dl>
        <dt class="heading">Funded Total</dt>
        <dd>
            <asp:Literal ID="FundedTotalLiteral" runat="server" />
        </dd>
    </dl>
    <dl>
        <dt class="heading">Non Funded Total</dt>
        <dd>
            <asp:Literal ID="NonFundedTotalLiteral" runat="server" />
        </dd>
    </dl>        
    <dl>
        <dt class="heading">Mean Debtor Days</dt>
        <dt>
            <span>Based on Invoice Date: </span>
        </dt>
        <dd>            
            <asp:Literal ID="MeanDebtorDaysByInvoiceLiteral" runat="server" />
        </dd>        
        <dt><span>Based on BOM Following: </span></dt>
        <dd>            
            <asp:Literal ID="MeanDebtorDaysByBOMLiteral" runat="server" />
        </dd>    
        <dt><span>Count: </span></dt>
        <dd>
            <asp:Literal ID="PaidForRecordCountLiteral" runat="server" />
        </dd>
    </dl>
    <dl>
        <dt class="heading">Mean Age of Unpaid</dt>
        <dt>
            <span>Based on Invoice Date: </span>
        </dt>
        <dd>            
            <asp:Literal ID="MeanAgeOfUnpaidByInvoiceLiteral" runat="server" />
        </dd>
        <dt>
            <span>Based on BOM Following: </span>
        </dt>
        <dd>
            <asp:Literal ID="MeanAgeOfUnpaidByBOMLiteralLiteral" runat="server" />
        </dd>
        <dt>
            <span>Count: </span>
        </dt>
        <dd>
            <asp:Literal ID="UnpaidForRecordCountLiteral" runat="server" />
        </dd>
    </dl>
</div>
