<%@ Control Language="C#" AutoEventWireup="true" 
        CodeBehind="PageDescription.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.PageDescription" %>

<div class="pageHelp">
    <p class="description" style="display:none">
        <a class="toggleDescription closeDescription" onclick="toggleHelp(this);return false;">            
            <asp:Image ID="crossImage" runat="server" CssClass="informationImage" ImageUrl="~/images/cross.png" Height="16px" Width="16px" />
        </a>
        <asp:Literal ID="pageDescriptionLiteral" runat="server"></asp:Literal>
    </p>
</div>
