<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RetentionDetailsPanel.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.RetentionDetailsPanel" %>

    <style type="text/css">
        .style1
        {
            border:none;
            height: 25px;
        }
        .style2
        {
            border:none;
            text-align: right;
            width: 200px;
        }
        .style3
        {
            border:none;
            text-align: right;
            width: 150px;
        }
        .style4
        {
            border:none;
            text-align: left;
            width: 300px;
        }
        </style>

    <table style="width: 630px; margin-right: 0px; margin-top: 0px; border: none;">
        <tbody style="border:none;">
            <tr style="border:none;">
                <th colspan="3" style="text-align:left">
                    <asp:Label ID="DebtorsLedgerLabel" runat="server" Text="Debtors Ledger:"></asp:Label>
                </th>
            </tr>
            <tr style="border:none;">
                <td class="style4" style="text-align: left">
                    <asp:Label ID="NonFundedLabel" runat="server" Text="Non Funded:"></asp:Label>
                </td>
                <td class="style3" style="text-align: right">
                    <asp:Literal ID="NonFactoredLiteral" runat="server" />
                </td>
                <td class="style3"></td>
            </tr>
            <tr style="border:none;">
                <td class="style4">
                    <asp:Label ID="FundedInvoicesLabel" runat="server" Text="Funded Invoices:"></asp:Label>
                </td>
                <td class="style3" style="text-decoration: underline">
                    <asp:Literal ID="FactoredLiteral" runat="server" />
                </td>
                <td class="style3"></td>
            </tr>
            <tr style="border:none;">
                <td class="style4">
                    Total Ledger:
                </td>
                <td class="style3">
                    <u><span style="border-bottom: 1px double #000;">
                        <asp:Literal ID="totalLedgerLiteral" runat="server" />
                    </span></u>
                </td>
                <td class="style3"></td>
            </tr>
            <asp:Panel ID="RetentionPanel" runat="server" Visible="false">
               <tr style="border:none;">
                    <td class="style4">
                        Retention Held:
                    </td>
                    <td class="style3">
                    </td>
                    <td class="style3">
                        <asp:Literal ID="RetentionHeldLiteral" runat="server" />
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        Funded Retention (<asp:Literal ID="PercentageHeldLiteral" runat="server" />
                        of funded invoices):
                    </td>
                    <td class="style3">
                    </td>
                    <td class="style3" style="text-decoration: underline">
                        <asp:Literal ID="FactoredInvoicesPercentageLiteral" runat="server" />
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        Surplus (shortfall):
                    </td>
                    <td class="style3">
                    </td>
                    <td class="style3">
                        <asp:Literal ID="NetRetentionLiteral" runat="server" />
                    </td>
                </tr>
                <tr style="border:none;">
                    <th colspan="3" align="left" class="style4">
                        <b><asp:Label ID="LessLabel" runat="server" Text="Less:"></asp:Label></b>
                    </th>
            </tr>
            </asp:Panel>
            <asp:Panel ID="SpaceerPanel" runat="server" Visible="false">
                <tr style="border:none;">
                </tr>
                <tr style="border:none;">
                </tr>
                <tr style="border:none;">
                </tr>
                <tr style="border:none;">
                    <th colspan="3" align="left">
                        <asp:Label ID="SumChargedHeaderLabel" runat="server" Text="Sum Charged Via Current Account"></asp:Label>
                    </th>
                </tr>
            </asp:Panel>

            <asp:Panel ID="Disc_Cr_PrepayPanel" runat="server" Visible="false">            
                <tr style="border:none;">
                    <td class="style4">
                        Discounts:
                    </td>
                    <td class="style3" align="right">
                        <asp:Literal ID="DiscountsLiteral" runat="server" />
                    </td>
                    <td class="style3">
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        Repurchases:
                    </td>
                    <td class="style3" align="right">
                        <asp:Literal ID="RepurchasesLiteral" runat="server" />
                    </td>
                    <td class="style3">
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        Credits:
                    </td>
                    <td class="style3">
                        <asp:Literal ID="CreditNotesLiteral" runat="server" />
                    </td>
                    <td class="style3">
                    </td>
                </tr>
            </asp:Panel>
            <tr style="border:none;">
                <td class="style4">
                    <asp:Label ID="OverdueChargesLabel" runat="server" Text="Interest & Charges**:"></asp:Label>
                </td>
                <td class="style3">
                    <asp:Literal ID="OverdueChargesLiteral" runat="server" />
                </td>
                <td class="style3">
                </td>
            </tr>
            <tr style="border:none;">
                <td align="left" class="style4">
                    Postage Statements(@
                    <asp:Literal ID="PostageRateLiteral" runat="server" />)**:
                </td>
                <td class="style3">
                    <asp:Literal ID="PostageAmountLiteral" runat="server" />
                </td>
                <td class="style3">
                </td>
            </tr>
            <tr style="border:none;">
                <td class="style4">
                    Bank Fees:
                </td>
                <td class="style3">
                    <asp:Literal ID="BankFeesLiteral" runat="server" />
                </td>
                <td class="style3">
                </td>
            </tr>
            <tr style="border:none;">
                <td class="style4">
                    Communications**:
                </td>
                <td class="style3">
                    <asp:Literal ID="TollsLiteral" runat="server" />
                </td>
                <td class="style3">
                </td>
            </tr>
            <tr style="border:none;">
                <td class="style4">
                    Letters Sent**:
                </td>
                <td class="style3">
                    <asp:Literal ID="LettersSentLiteral" runat="server" />
                </td>
                <td class="style3">
                </td>
            </tr>
            <tr style="border:none;">
                <td class="style4">
                    <asp:Label ID="RepaymentLabel" runat="server" Text="Repayment:"> </asp:Label>
                </td>
                <td class="style3">
                    <asp:Label ID="RepaymentLiteral" runat="server" Font-Underline="false"> </asp:Label>
                </td>
                <td class="style3">
                </td>
            </tr>
            <asp:Panel ID="NFRec_PostEOM_etcPanel" runat="server" Visible="false">                        
                <tr style="border:none;">
                    <td class="style4">
                    </td>
                    <td class="style3">
                    </td>
                    <td class="style3" style="text-decoration: underline">
                        <asp:Literal ID="TotalDeductablesLiteral" runat="server" />
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                    </td>
                    <td class="style3">
                    </td>
                    <td class="style3">
                        <asp:Literal ID="SurplusLessDeductablesLiteral" runat="server" />
                    </td>
                </tr>
                <tr style="border:none;">
                    <th colspan="3" style="text-align: left">
                        <b>Plus:</b>
                    </th>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        Non Funded Receipts:
                    </td>
                    <td class="style2">
                    </td>
                    <td class="style3" style="text-decoration: underline">
                        <asp:Literal ID="NonFactoredReceiptsLiteral" runat="server" />
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                    </td>
                    <td class="style2">
                    </td>
                    <td class="style3">
                        <asp:Literal ID="RetentionPriorToEndOfMonthLiteral" runat="server" />
                    </td>
                </tr>
                <tr style="border:none;">
                    <th colspan="3" align="left">
                        Less transactions subsequent to <asp:Literal ID="endOfMonthLiteralTwo" runat="server" />:
                    </th>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        <asp:Literal ID="LikelyRepurchasesLiteral" runat="server">Likely Repurchases :</asp:Literal>
                    </td>
                   <td class="style3">
                       <asp:Literal ID="LikelyRepurchasesAmountLiteral" runat="server" />
                    </td>
                    <td class="style3"></td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        Repurchases:
                    </td>
                    <td class="style3">
                        <asp:Literal ID="RepurchasesAfterEOMLiteral" runat="server" />
                    </td>
                    <td class="style3">
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                        Credit Notes:
                    </td>
                    <td class="style3" style="text-decoration: underline">
                        <asp:Literal ID="CreditNotesAfterEOMLiteral" runat="server" />
                    </td>
                    <td class="style3">
                    </td>
                </tr>
                <tr style="border:none;">
                    <td class="style4">
                    </td>
                    <td class="style3">
                    </td>
                    <td class="style3">
                        <asp:Literal ID="NetRetentionAfterEOMLiteral" runat="server" />
                    </td>
                </tr>
            </asp:Panel>
            <tr style="border:none;">
                <td class="style4">
                    Adjustments:
                </td>
                <td class="style3" style="text-decoration: underline">
                    <asp:Literal ID="CA_DrMgt_AdjustmentsLiteralLiteral" runat="server" Visible="false" />
                </td>
                <td class="style3">
                    <asp:Literal ID="AdjustmentsLiteral" runat="server" Visible="true" />
                </td>
            </tr>

            <tr style="border:none;">
                <td class="style4">
                    <asp:Literal ID="ReleaseLiteral" runat="server" Text="Retention"/>
                </td>
                <asp:Panel ID="ReleasePanel" runat="server" Visible="true">                        
                    <td class="style3">
                        <asp:Literal ID="heldLiteral" runat="server" Text="Held"/>
                    </td>
                </asp:Panel>           
                <td class="style3" > 
                  <u><span style="border-bottom: 1px double #000;">
                        <asp:Literal ID="EstimatedRetentionReleaseLiteral" runat="server" />
                     </span></u>
                </td>
                <%-- <asp:Panel ID="ReleasePanel2" runat="server" Visible="false"> 
                    <td>2</td>                       
                </asp:Panel>
                --%>
            </tr>

            <tr style="border:none;">
                <td class="style4" colspan="3">
                    <b>Notes:</b>&nbsp;<asp:Literal ID="RetnNotes" runat="server" />
                </td>
            </tr>
            <tr style="border:none;">
                <td class="style4">
                    ** This invoice includes GST of
                    <asp:Literal ID="GstLiteral" runat="server" />
                </td>
                <td class="style3">
                </td>
                <td class="style3">
                </td>
            </tr>
        </tbody>
    </table>
    <div class="summary clearfix" id="summarySection" runat="server">
        <dl>
            <dt>Funding Period</dt>
            <dd>
                <asp:Literal ID="FactorDaysLiteral" runat="server" /></dd>
        </dl>
        <dl>
            <dt>Opening Balance</dt>
            <dd>
                <asp:Literal ID="OpeningBalanceLiteral" runat="server" /></dd>
        </dl>
        <dl>
            <dt>Invoices Purchased</dt>
            <dd>
                <asp:Literal ID="InvoicesPurchasedLiteral" runat="server" /></dd>
        </dl>
        <dl>
            <dt>Credit Transactions</dt>
            <dd>
                <asp:Literal ID="CreditTransactionsLiteral" runat="server" /></dd>
        </dl>
        <dl>
            <dt>Closing Balance</dt>
            <dd>
                <asp:Literal ID="ClosingBalanceLiteral" runat="server" /></dd>
        </dl>
    </div>
