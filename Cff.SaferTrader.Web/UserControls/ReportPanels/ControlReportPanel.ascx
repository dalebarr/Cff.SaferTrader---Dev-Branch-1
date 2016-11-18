<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlReportPanel.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.ControlReportPanel" %>
<table  style="width:100%;table-layout:fixed;">
    <tbody>
        <tr>
            <td style="width:100%;white-space:nowrap;overflow:hidden;min-width:500px;vertical-align:top;table-layout:fixed;">
                <div style="float:left;width:60%;">
                   <div style="width:100%;margin-top:2px;min-width:400px;vertical-align:top;">
                        <table class="controlReport">
                            <caption>
                                Control report
                            </caption>
                            <thead>
                                <tr>
                                    <th colspan="3">
                                        Debtors Ledger
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                       Current:
                                    </td>
                                    <td class="right" >
                                       <asp:Literal ID="CurrentLiteral" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                       1 month:
                                    </td>
                                    <td class="right">
                                       <asp:Literal ID="OneMonthLiteral" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                       2 months:
                                    </td>
                                    <td class="right">
                                       <asp:Literal ID="TwoMonthsLiteral" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                       3 months & over:
                                    </td>
                                    <td class="right">
                                       <asp:Literal ID="ThreeMonthsLiteral" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="2" style="overflow:hidden;">
                                       Total Ledger:
                                    </td>
                                    <td class="right" style="overflow:hidden;">
                                       <asp:Literal ID="TotalDebtorsLedgerLiteral" runat="server" />
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
  
                   <div style="width:100%;margin-top:2px;min-width:400px;vertical-align:top;">
                          <table class="controlReport">
                                <thead>
                                    <tr>
                                        <th colspan="3">
                                            Factors Ledger
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="3" class="subThead">
                                            Brought Forward
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Funded Invoices:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="FundedInvoicesBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Non Funded Invoices:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="NonFundedInvoicesBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Credit Notes:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="CreditNotesBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Net Journals:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="NetJournalsBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Cash Received:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="CashReceiveBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Overpayments:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="OverpaymentsBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Net Adjustment:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="NetAdjustmentBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Discounts:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="DiscountsBroughtForwardLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="subTfoot">
                                        <td colspan="2">
                                            Total Brought Forward:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="TotalBroughtForwardLiteral" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="subThead">
                                            Current
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Funded Invoices:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="FundedInvoicesCurrentLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Non Funded Invoices:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="NonFundedInvoicesCurrentLiteral" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Credit Notes:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="CreditNotesCurrentLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Net Journals:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="NetJournalsCurrentLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Cash Received:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="CashReceivedCurrentLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Overpayments:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="OverpaymentsCurrentLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Net Adjustment:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="NetAdjustmentCurrentLiteral" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Discounts:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="DiscountsCurrentLiteral" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="subTfoot">
                                        <td colspan="2">
                                            Total Current:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="TotalCurrentLiteral" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="2">
                                            Total Ledger:
                                        </td>
                                        <td class="right">
                                            <asp:Literal ID="TotalFactorsLedgerLiteral" runat="server" />
                                        </td>
                                    </tr>
                                </tfoot>
                         </table>
                    </div>
                </div>
                 <div style="width:35%;float:right;text-align:right;border:2px solid gray;padding-left:5px;padding-right:10px;">
                    <dl>
                            <dt  style="border-bottom:1px solid gray;"><span>Balance of Funded Invoices:</span> </dt>
                            <dd>
                                <asp:Literal ID="FundedInvoicesBalanceLiteral" runat="server" />
                            </dd>
                    </dl>
                    <dl>
                        <dt  style="border-bottom:1px solid gray;"><span>Balance of Non Funded Invoices:</span> </dt>
                            <dd>
                                <asp:Literal ID="NonFundedInvoicesBalanceLiteral" runat="server" />
                            </dd>
                    </dl>
                    <dl>
                            <dt  style="border-bottom:1px solid gray;"><span>Unallocated Transactions:</span> </dt>
                            <dd>
                                <asp:Literal ID="UnallocatedTransactionsLiteral" runat="server" />
                            </dd>
                        </dl>
                    <dl>
                            <dt  style="border-bottom:1px solid gray;"><span>Prepayments this Month:</span> </dt>
                            <dd>
                                <asp:Literal ID="RepurchasesThisMonthLiteral" runat="server" />
                            </dd>
                        </dl>
                    <dl>
                            <dt  style="border-bottom:1px solid gray;"><span>Prepayments to be Claimed:</span> </dt>
                            <dd>
                                <asp:Literal ID="FundedToBeRepurchasedLiteral" runat="server" />
                            </dd>
                        </dl>
                    <dl>
                            <dt  style="border-bottom:1px solid gray;"><span>Credits to be Claimed:</span> </dt>
                            <dd>
                                <asp:Literal ID="CreditsToBeClaimedLiteral" runat="server" />
                            </dd>
                        </dl>
                    <dl>
                            <dt  style="border-bottom:1px solid gray;"><span>Allocated in Period:</span> </dt>
                            <dd>
                                <asp:Literal ID="AllocatedInPeriodLiteral" runat="server" />
                            </dd>
                        </dl>
                    <dl>
                            <dt  style="border-bottom:1px solid gray;"><span>CBT's in Period:</span> </dt>
                            <dd>
                                <asp:Literal ID="CBTSInPeriodLiteral" runat="server" />
                            </dd>
                        </dl>
                </div>
            </td>
        </tr>
    </tbody>
</table>



