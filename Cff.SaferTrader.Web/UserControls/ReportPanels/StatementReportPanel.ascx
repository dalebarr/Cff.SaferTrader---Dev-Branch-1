<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatementReportPanel.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.StatementReportPanel" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/ManagementDetailsBox.ascx" TagPrefix="uc" TagName="ManagementDetailsBox" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<div style="min-width:885px; max-width:100%;">
    <div class="statementHeader clearfix">
        <div class="left-col" style="width:450px; margin-top:35px; height:auto;">
            <div>
                <h2>Purchaser</h2>
            </div>
            <div>
                <p style="text-align:left;">
                     <asp:Literal ID="customerNameLiteral" runat="server" />
                    <br />
                    <asp:Literal ID="customerAddressOneLiteral" runat="server" />
                    <br />
                    <asp:Literal ID="customerAddressTwoLiteral" runat="server" />
                    <br />
                    <asp:Literal ID="customerAddressThreeLiteral" runat="server" />
                    <br />
                    <asp:Literal ID="customerAddressFourLiteral" runat="server" />
                    <br />
                    <strong style="margin:0;padding:0;">Payment of this account must be made to
                        <asp:Literal ID="accountNameLiteral" runat="server" />
                    </strong>
                    <br />
                    <b>
                        <asp:Literal ID="bankAndBranchLiteral" runat="server" />
                    </b>
                    <asp:Literal ID="accountNumberLiteral" runat="server" />
                    <b>Please Quote Reference:</b>
                    <asp:Literal ID="referenceLiteralTwo" runat="server" />
                </p>
            </div>
        </div>
        <div class="right-col" style="width:425px;">
                <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" />
                <strong>Reference:</strong>
                <asp:Literal ID="referenceLiteral" runat="server" />
                <br />
                <strong>ID:</strong>
                <asp:Literal ID="customerIdLiteral" runat="server" />
        </div>
    </div>
</div>

 <div class="scroll">
        <asp:PlaceHolder ID="CffGenGridViewPlaceHolder" runat="server" ></asp:PlaceHolder>
 </div>

 <div class="statementSubFooter clearfix">
        <table style="border:0;padding:0;">
            <tbody>
                 <tr>
                    <td>
                        <asp:Literal ID="threeMonthsOrOverLiteral" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="twoMonthsLiteral" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="oneMonthLiteral" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="currentLiteral" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="balanceLiteral" runat="server" />
                    </td>
                </tr>
                 <tr class="highLight">
                    <th>
                        3+ months
                    </th>
                    <th>
                        2 months
                    </th>
                    <th>
                        1 month
                    </th>
                    <th>
                        Current
                    </th>
                    <th>
                        Balance
                    </th>
                </tr>
            </tbody>
        </table>
</div>

<div class="statementFooter clearfix">
        <div class="left-col">
            <p>
                <asp:Literal ID="managementNameLiteral" runat="server" />
                <br />
                <asp:Literal ID="managementAddressOneLiteral" runat="server" />
                <br />
                <asp:Literal ID="managementAddressTwoLiteral" runat="server" />
                <br />
                <asp:Literal ID="managementAddressThreeLiteral" runat="server" />
                <br />
                <asp:Literal ID="managementAddressFourLiteral" runat="server" />
            </p>
        </div>
        <div class="stmnt_right-col">
                  <table style="border:0;padding:0;">
                      <tbody>
                            <tr>
                                <td><strong>Customer:</strong></td>
                                <td style="text-align:right; border: none;"><asp:Literal ID="customerNameLiteral2" runat="server" /></td>
                            </tr>
                    
                            <tr>
                                <td><strong>Customer Number:</strong></td>
                                <td style="text-align:right; border: none;"><asp:Literal ID="customerNumberLiteral" runat="server" /></td>
                           </tr>
                    
                            <tr>
                                <td><strong>Month Ending:</strong></td>
                                <td style="text-align:right; border: none;"><asp:Literal ID="monthEndingLiteral" runat="server" /></td>
                           </tr>
                    
                            <tr>
                                <td><strong>Reference:</strong></td>
                                <td style="text-align:right; border: none;"><asp:Literal ID="referenceLiteralThree" runat="server" /></td>
                            </tr>
                    
                            <tr>
                                <td><strong>Please Direct Credit Into:</strong></td>
                                <td style="text-align:right; border: none;"><asp:Literal ID="accountNumberLiteralTwo" runat="server" /></td>
                            </tr>
                    
                            <tr>
                                <td><strong>Client:</strong></td>
                                <td style="text-align:right; border: none;"><asp:Literal ID="clientNameLiteral" runat="server" /></td>
                            </tr>
                      </tbody>
                </table>
        </div>
        <h4>
            REMITTANCE ADVICE <span>PLEASE RETURN WITH YOUR PAYMENT</span>
        </h4>
</div>