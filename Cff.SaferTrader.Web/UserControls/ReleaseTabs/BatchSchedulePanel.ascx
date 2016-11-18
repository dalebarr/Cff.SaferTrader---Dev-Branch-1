<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatchSchedulePanel.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.BatchSchedulePanel" %>
<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/BatchScheduleDetails.ascx" TagName="BatchScheduleDetails" %>
<asp:Label ID="lblClientFacilityType" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Panel ID="DivFacilityType1" runat="server" Visible="false">  
        <table id="BatchScheduleTable" style="border:none; margin-right: 0px; margin-top: 0px;width:713px;">
            <tbody>
                <tr style="width:100%;">
                    <td style="width:576px;">
                        <asp:Literal ID="TotalInvoiceLabelLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:255px;">
                        <asp:Literal ID="TotalInvoiceLiteral" runat="server"></asp:Literal>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr style="width:100%;">
                    <td class="left" colspan="3">
                        Less:
                    </td>
                    <td></td>
                </tr>
                <tr id="checkOrConfirmRow" runat="server" style="width:100%;">
                    <td style="width:576px;">
                        <asp:Label ID="CheckConfirmLabel" runat="server" Text="To be Checked or Confirmed:"></asp:Label>
                    </td>
                    <td></td>
                    <td class="right" style="width:255px;">
                        <asp:Literal ID="CheckConfirmLiteral" runat="server"></asp:Literal>
                    </td>
                    <td></td>
<%--                    <td></td>--%>
                </tr>
                <tr style="width:100%;">
                    <td style="width:576px;">
                        <asp:Literal ID="NonFactoredLabelLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="right" style="width:255px;border-bottom:1px solid grey;">
                        <asp:Literal ID="NonFactoredLiteral" runat="server"></asp:Literal>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr style="width:100%;">
                    <td style="width:460px;">
                        <asp:Literal ID="FactoredLabelLiteral" runat="server"></asp:Literal>
                    </td>
                    <td style="width: 177px;"></td>
                    <td class="right" style="width: 180px;">
                        <asp:Label ID="FactoredLiteral01" runat="server" Visible="True"></asp:Label>
                    </td>
                    <td class="right" style="Width: 220px;">
                        <asp:Label ID="FactoredLiteral" runat="server" Visible="True" ></asp:Label>
                    </td>
                </tr>
                <tr style="width:100%;">
                    <td style="width: 177px;"></td>
                    <td style="width: 177px;"></td>
                    <td style="width: 177px;"></td>
                    <td style="width:177px;display: none">
                        <h5><asp:Literal ID="BatchInProcessingLiteral" runat="server"></asp:Literal></h5>
                    </td>
                    <td class="right" style="width: 177px;"></td>
                </tr>
            </tbody>
        </table>
        <%--<uc:BatchScheduleDetails ID="ScheduleDetails" runat="server" Visible="false"></uc:BatchScheduleDetails>--%>
    </asp:Panel>
    <uc:BatchScheduleDetails ID="ScheduleDetails" runat="server" Visible="false"></uc:BatchScheduleDetails>
