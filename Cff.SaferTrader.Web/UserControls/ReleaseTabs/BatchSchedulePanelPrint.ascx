<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatchSchedulePanelPrint.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.BatchSchedulePanelPrint" %>
<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/BatchScheduleDetails.ascx" TagName="BatchScheduleDetails" %>
<asp:Label ID="lblClientFacilityType" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Panel ID="DivFacilityType1" runat="server" Visible="false">  
        <table id="BatchScheduleTablePrint" style="border:none; margin-right: 0px; margin-top: 0px;width:713px;">
            <tbody>
                <tr style="width: 100%">
                    <td style="width:239px;">
                        <asp:Literal ID="TotalInvoiceLabelLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width: 100px;">
                        <asp:Literal ID="TotalInvoiceLiteral" runat="server"></asp:Literal>
                    </td>
                    <td style="width:100px"></td>
                    <td style="width:100px"></td>
                </tr>
                <tr style="width:100%;">
                    <td class="left" colspan="3">
                        Less:
                    </td>
                    <td></td>
                </tr>
                <tr id="checkOrConfirmRow" runat="server">
                    <td style="width:239px;">
                        <asp:Label ID="CheckConfirmLabel" runat="server" Text="To be Checked or Confirmed:"></asp:Label>
                    </td>
                    <td style="width:100px;"></td>
                    <td class="right" style="width: 100px;">
                        <asp:Literal ID="CheckConfirmLiteral" runat="server"></asp:Literal>
                    </td>
                    <td style="width:100px;"></td>
                    <td style="width:100px;"></td>
                </tr>
                <tr style="width: 100%">
                    <td style="width:239px;">
                        <asp:Literal ID="NonFactoredLabelLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:100px;border-bottom:1px solid grey;">
                        <asp:Literal ID="NonFactoredLiteral" runat="server"></asp:Literal>
                    </td>
                    <td style="width:100px;"></td>
                    <td style="width:100px;"></td>
                </tr>
                <tr style="width:100%">
                    <td style="width:239px;">
                        <asp:Literal ID="FactoredLabelLiteral" runat="server"></asp:Literal>
                    </td>
                    <td style="width:100px;"></td>
                    <td class="right" style="width:100px;">
                        <asp:Label ID="FactoredLiteral01" runat="server" Visible="True"></asp:Label>
                    </td>
                    <td class="right" style="width:104px;">
                        <asp:Label ID="FactoredLiteral" runat="server" Visible="True" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td style="display: none; width:100px">
                        <h5><asp:Literal ID="BatchInProcessingLiteral" runat="server"></asp:Literal></h5>
                    </td>
                    <td style="width:100px;"></td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    <uc:BatchScheduleDetails ID="ScheduleDetails" runat="server" Visible="false"></uc:BatchScheduleDetails>
