<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllClientsFilter.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.AllClientsFilter" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<div style="display:inline;min-width:200px;text-decoration:none;vertical-align:top;padding:0;margin:0;">
     <span>
        <span style="vertical-align:top;">
            Facility type:
            <asp:DropDownList ID="FacilityTypeDropDownList" CssClass="toDropDownList" runat="server" style="vertical-align:top;width:180px;" />
            <asp:CheckBox id="SalvageIncludedCheckBox" runat="server" Checked="true" TextAlign="right"/>Include Salvage
        </span>
        <span id="UpdateButtonColumn" runat="server" >
            <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                    OnClientClick="startAnimate();" CssClass="updateButton" style="vertical-align:top;margin:0;padding:0;" OnClick="UpdateButton_Click" />
        </span>
     </span>
</div>

