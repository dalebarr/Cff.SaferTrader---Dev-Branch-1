<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBox.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.MessageBox" %>

<div id="modalBox">
    <div id="addNotePanel" class="clearfix">
        <h4>
            <asp:Label ID="MessageTitleLiteral" runat="server" />
        </h4>
        
        <asp:Label ID="MessageLiteral" runat="server"  CssClass="modalBoxText" />
        
        <div class="clear">
            <asp:ImageButton ID="CancelButton" runat="server" 
                AlternateText="Cancel"
                ImageUrl="~/images/btn_cancel.png"
                OnClick="CancelButton_Click" />
        </div>
    </div>
</div>