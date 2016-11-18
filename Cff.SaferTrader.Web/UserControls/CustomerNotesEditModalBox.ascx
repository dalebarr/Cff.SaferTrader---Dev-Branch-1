<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerNotesEditModalBox.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.CustomerNotesEditModalBox" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<div id="modalBox">
    <div id="addNotePanel" class="clearfix">
        <h4>Edit note</h4>
        <div id="CustomerNoteDescriptors" runat="server" class="customerNoteDescriptors">
            <div>
                <label>
                    Activity type:</label>
                <asp:DropDownList ID="ActivityTypeDropDownList" runat="server" />
            </div>
            <div>
                <label>
                    Note type:</label>
                <asp:DropDownList ID="NoteTypeDropDownList" runat="server"  />
            </div>
        </div>
        <div class="note">
            <asp:TextBox ID="CommentEditTextBox" CssClass="customerNotes countdown limit_250"  runat="server" TextMode="MultiLine" Width="290px" ></asp:TextBox>
        </div>
        <div>
            <asp:ImageButton ID="SaveButton" runat="server" AlternateText="Save" OnClick="SaveButton_Click"
                ValidationGroup="customerNotesEdit" ImageUrl="~/images/btn_save.png" OnClientClick="EmptyAddNotesFeedbackLabel();" />
            <asp:ImageButton ID="CancelButton" runat="server" AlternateText="Cancel" OnClick="CancelButton_Click"  ImageUrl="~/images/btn_cancel.png" CausesValidation="false" />
            <span class="remaining">250 characters remaining.</span>
        </div>
        <div>
            <asp:Label ID="FeedbackLabel" runat="server" CssClass="feedback" />
            <asp:RequiredFieldValidator ID="CommentEditTextBoxRequiredFieldValidator" runat="server"
                ControlToValidate="CommentEditTextBox2" ErrorMessage="Please enter a message."
                Display="Dynamic" ValidationGroup="customerNotesEdit" />
        </div>
    </div>
</div>
