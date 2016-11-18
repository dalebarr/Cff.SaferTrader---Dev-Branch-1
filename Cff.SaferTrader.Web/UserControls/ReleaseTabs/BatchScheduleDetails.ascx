
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatchScheduleDetails.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.BatchScheduleDetails" %>
<style type="text/css">
    .auto-style3 {
        width: 330px;
    }
</style>

<table style="margin-right: 0px; margin-top: 0px; border: none; width: 713px;">
    <tbody>
        <asp:Panel ID="_DivDeductions" runat="server" Visible="true">
            <asp:Panel ID="_PanelAssignmentCr" runat="server" Visible="false" style="border:none;" BorderColor="#000FFF" BorderStyle="None" BorderWidth="0px">
                <tr style="width:100%;">
                    <td style="width:369px;">
                        <asp:Label ID="_LabelResidual" runat="server" Text="Less Residual Amount:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right lastValue" style="width:228px;text-decoration: underline;">
                        <asp:Literal ID="_LiteralResidual" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
                </tr>
                <tr style="width:100%;">
                    <td style="width:369px;">
                        <asp:Label ID="_LabelAssignCr" runat="server" Text="Assignment Credit:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right" style="width:228px;">
                        <asp:Literal ID="_LiteralAssignCr" runat="server"></asp:Literal>
                    </td>
                </tr>
            </asp:Panel>        
            
            <asp:Panel  ID="_PanelAssignmentCr2" runat="server" Visible="true" style="border:none;" BorderColor="#000FFF" BorderStyle="None" BorderWidth="0px">
                <tr style="border:none;width:100%;">
                    <td class="left" style="width:369px;">
                        <asp:Label ID="_LabelLess" runat="server" Text="Less:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;">
                    <td class="right" style="width:228px;">
                    <td class="right" style="width:228px;">
                </tr>
                <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_adminFeeLabel" runat="server" Text="Admin Fee:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;text-align:right;">
                        <asp:Literal ID="_adminFeeLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right" style="width:228px;"></td>
                </tr>
                <tr>
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_adminFeeGstLabel" runat="server" Text="GST"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;text-align:right;">
                        <asp:Panel ID="_panelAdminFeeGst" runat="server" Visible="true">
                            <asp:Literal ID="_adminFeeGstLiteral" runat="server"></asp:Literal>
                            <asp:Label ID="_adminFeeTotalLabel" runat="server"></asp:Label>
                        </asp:Panel>    
                   </td>
                    <td class="right" style="width:228px;vertical-align: bottom;">
                        <asp:Literal ID="_adminFeeTotalLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
                </tr>
                <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_factorFeeLabel" runat="server" Text="Factor Fee:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;">
                    <td class="right" style="width:228px;text-align:right;" >
                        <asp:Literal ID="_factorFeeLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
       	        </tr>
            </asp:Panel>    
            
            <asp:Panel ID="_panelNonCompliantFee" runat="server" Visible="false">
                <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_nonCompliantFeeLabel" runat="server" Text="Non Compliant Fee:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;">
                    <td class="right"  style="width:228px;text-align:right;" >
                        <asp:Literal ID="_nonCompliantFeeLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
       	        </tr>
            </asp:Panel>

            <asp:Panel ID="_PanelRetention" runat="server" Visible="true">
                <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_retentionLabel" runat="server" Text="Retention Held:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;">
                    <td class="right" style="width:228px;text-align:right;" >
                        <asp:Literal ID="_retentionLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
                </tr>
            </asp:Panel>
  
            <asp:Panel ID="_panelPrepayment" runat="server" Visible="false">
                  <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_repurchasesLabel2" runat="server" Text="Prepayments:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;text-align:right;" >
                        <asp:Literal ID="_repurchLiteral100" runat="server" Visible="false"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
                    <td class="right" style="width:228px;">
                </tr>
            </asp:Panel>
            
            <asp:Panel ID="_panelViewRepurchases" runat="server" Visible="false">
                 <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_repurchasesLabel" runat="server" Text="Prepayments:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;">
                        <asp:Label ID="_repurchResidual" runat="server" Visible="true" Font-Underline="true"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;text-align:right;" >
                        <asp:Literal ID="_repurchasesLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;"></td>
                </tr>
            </asp:Panel>
            
            <asp:Panel ID="_panelCredit" runat="server" Visible="false"  style="border:none;">
                  <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_creditLabel2" runat="server" Text="Funding Credits:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;text-align:right;" >
                        <asp:Literal ID="_creditLiteral100" runat="server" Visible="false"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
                    <td class="right" style="width:228px;">
                </tr>
            </asp:Panel>
            
             <asp:Panel ID="_panelCredit2" runat="server" Visible="true"  style="border:none;">
                <tr style="width:100%;">
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_creditLabel" runat="server" Text="Credits:"></asp:Label>
                    </td>
                    <td class="right" style="width: 228px;">
                        <asp:Label ID="_creditResidual" runat="server" Visible="true" Font-Underline="true"></asp:Label>
                    </td>
                    <td class="right" style="width: 228px;">
                        <asp:Literal ID="_creditLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;"></td>
                </tr>
                <tr style="width:100%;" >
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_postageLabel" runat="server" Text="Postage:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;text-align:right;" >
                        <asp:Literal ID="_postageLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right" style="width:228px;"></td>
                </tr>
                <tr>
                    <td class="left" style="width: 369px;">
                        <asp:Label ID="_postGstLabel" runat="server" Text="GST"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;text-align:right;" >
                        <asp:Panel ID="_panelPostGst" runat="server" Visible="true">
                            <asp:Literal ID="_postGstLiteral" runat="server"></asp:Literal>
                            <asp:Label ID="_postageTotalLabel" runat="server"></asp:Label>
                        </asp:Panel>
                    </td>
                    <td class="right" style="vertical-align: bottom;text-align:right;border-bottom:1px solid grey;width: 228px">
                       <asp:Literal ID="_postageTotalLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;">
                </tr>
                <tr style="width:100%;line-height:2px;" >
                    <td class="right" style="width:228px;"></td>
                    <td style="width:150px;text-align:right;" >
                        <div style="display:table;width:100%;">
                            <div style="display:table-cell;width:30%;"></div>
                            <div style="display:table-cell;width:30%;"></div>
                            <div style="display:table-cell;width:auto;"></div>
                        </div>
                    </td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right" style="width:228px;"></td>
                </tr>
                <tr>
                    <td style="width:369px;">
                        <asp:Label ID="_deductionsLabel" runat="server" Text="Deductions:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;">
                    <td class="right" style="width:228px;">&nbsp;</td>
                    <td class="right" style="width:228px;text-decoration: underline;">
                        <asp:Literal ID="_deductionsLiteral" runat="server"></asp:Literal>
                    </td>
                </tr>
            </asp:Panel>
            
            <asp:Panel ID="_PanelCharges" runat="server" Visible="true"  style="border:none;">
                <div id="_PanelChargesTable"  style="border:none;">
                     <tr>
                         <td style="width:369px;">
                                <asp:Label ID="_chargesTotalLabel" runat="server" Text="Adjustment/Charges:"></asp:Label>
                         </td>
                         <td class="right" style="width:228px;"></td>
                         <td class="right" style="width:228px;">&nbsp;</td>
                         <td class="right" style="width:228px;">
                              <asp:Literal ID="_chargesTotalLiteral" runat="server"></asp:Literal>
                         </td>
                     </tr>
                </div>
            </asp:Panel>
       
            <asp:Panel ID="_PanelCharges2" runat="server" Visible="true">  
                <tr>
                    <td style="width:369px;">
                        <asp:Label ID="_availableForReleaseLabel" runat="server" Text="Available for Release:"></asp:Label>
                    </td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right" style="width:228px;">&nbsp;</td>
                    <td class="right" style="width:228px;border-bottom:1px double black;">
                        <asp:Literal ID="_availableForReleaseLiteral" runat="server"></asp:Literal>
                    </td>
                </tr>
            </asp:Panel>

            <asp:Panel ID="_panelSumFeesForCA" runat="server" Visible="false">
                <tr>
                    <td style="width:369px;font-size: 10.5px;white-space: nowrap">
                        <asp:Label ID="_labelSumFeesCA" runat="server" Text="Sum of Fees to be Charged to Current Account:"></asp:Label>
                    </td>
                    <td class="right lastValue" style="width:228px;">
                        <asp:Literal ID="_literalSumFeesCA" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:228px;"></td>
                    <td class="right" style="width:228px;"></td>
                </tr>
            </asp:Panel>
        </asp:Panel>       
    </tbody>
</table>

<br />

<table style="width: 656px; margin-right: 0px; margin-top: 0px; border: none;display: none;" >
    <tbody>
        <tr style="border:none;">
            <td style="width:200px;">
                <asp:Label ID="_dateFinishedLabel" runat="server" Text="Finish Date:"></asp:Label>
                <asp:Literal ID="_dateFinishedLiteral" runat="server"></asp:Literal>
            </td>
            <td></td>
            <td class="right"  style="width:200px;">
                <asp:Literal ID="_releasedLiteral" runat="server"></asp:Literal>
            </td>  
            <td class="right"  style="width:200px;">
                <asp:Label ID="_statusDescriptionLabel" runat="server" Text="Status:"></asp:Label>
                <asp:Literal ID="_statusDescriptionLiteral" runat="server"></asp:Literal>
            </td>
        </tr>
    </tbody>
</table>


<div id="noteRow" runat="server" style="width:600px;">
      <p style="width:100%;text-align:justify;">
        <asp:Label ID="_scheduleBatchNoteLabel" runat="server" Text=""></asp:Label>
        <asp:Literal ID="_scheduleBatchNoteLiteral" runat="server"></asp:Literal>
      </p>
</div>