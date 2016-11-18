<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerNotesAdderModalBox.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.CustomerNotesAdderModalBox" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>  

<div id="modalBox" style="background-color:aliceblue;">
     <h4>Add note</h4>
    <asp:UpdatePanel ID="AddNotesCustModal" runat="server" UpdateMode="Conditional"  ChildrenAsTriggers="true">
        <Triggers>
             <asp:AsyncPostBackTrigger ControlID="PermanentNoteCheckBox" />
             <asp:AsyncPostBackTrigger ControlID="ActivityTypeDropDownList" />
             <asp:AsyncPostBackTrigger ControlID="NoteTypeDropDownList" />
             <asp:AsyncPostBackTrigger ControlID="NextCallDueTextBox" />
            <asp:AsyncPostBackTrigger ControlID="SaveButton" />
            <asp:AsyncPostBackTrigger ControlID="CancelButton" />
        </Triggers>
        <ContentTemplate>
              <asp:Panel ID="AddPermanentNote" runat="server" CssClass="permanentNote">
                <label>Permanent note:</label>
                <asp:CheckBox ID="PermanentNoteCheckBox" runat="server" OnCheckedChanged="PermanentNoteCheckBox_CheckChanged" AutoPostBack="true"  />
            </asp:Panel>
              <div id="CustomerNoteDescriptors" runat="server" class="customerNoteDescriptors">
                <div>
                    <label>
                        Activity type:</label>
                    <asp:DropDownList ID="ActivityTypeDropDownList" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged"/>
                </div>
                <div>
                    <label>
                        Note type:</label>
                    <asp:DropDownList ID="NoteTypeDropDownList" runat="server"  OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" />
                </div>
                <div id="NextCallDueDiv" runat="server">
                    <label>Next call due:</label>
                    <uc:SecureTextBox ID="NextCallDueTextBox" CssClass="dateRange" runat="server"  OnTextChanged="TextBox_TextChanged"/>
                </div>
            </div>
             <div class="note">
                <uc:SecureTextBox ID="CommentTextBox" CssClass="customerNotes countdown limit_250" runat="server" TextMode="MultiLine" />
            </div>
            <div>
                <asp:ImageButton ID="SaveButton" runat="server" AlternateText="Save" OnClick="SaveButton_Click"  ValidationGroup="customerNotes" ImageUrl="~/images/btn_save.png" />
                <asp:ImageButton ID="CancelButton" runat="server" AlternateText="Cancel" OnClick="CancelButton_Click" ImageUrl="~/images/btn_cancel.png" />
                    <span class="remaining">250 characters remaining.</span>
           </div>
            <div>
                <asp:Label ID="FeedbackLabel" runat="server" CssClass="feedback" />
                <asp:RequiredFieldValidator ID="CommentTextBoxRequiredFieldValidator" runat="server"
                    ControlToValidate="CommentTextBox" ErrorMessage="Please enter a message." Display="Dynamic"
                    ValidationGroup="customerNotes" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
   
   
   
   
    
    
</div>

