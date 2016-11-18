<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/UserControls/ReleaseTabs/OverdueChargesTab.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.OverdueChargesTab" %>

<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>
<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/StatusPicker.ascx" TagPrefix="uc" TagName="StatusPicker" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<div id="overdueCharges" style="overflow:hidden">
    <script type="text/javascript"  src="./js/ui/jquery-1.2.6.min.js" ></script>
        
    <div class="parameterSelector">
        <table>
            <tr>
                <uc:StatusPicker ID="StatusPicker" runat="server" />
            </tr>
        </table>        
    </div>
    <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="StatusPicker" />
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
        </Triggers>
        <ContentTemplate>
            <div>
                <asp:PlaceHolder ID="GVPlaceHolder" runat="server"></asp:PlaceHolder>
            </div>
            <div class="dateViewed clearfix">
                <p>
                    <span>Date Viewed</span>
                    <asp:Literal ID="DateViewedLiteral" runat="server" /></p>
            </div>
       </ContentTemplate>
    </asp:UpdatePanel>
</div>