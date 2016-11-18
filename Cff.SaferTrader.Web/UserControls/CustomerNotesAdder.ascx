<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerNotesAdder.ascx.cs"  Inherits="Cff.SaferTrader.Web.UserControls.CustomerNotesAdder" %>


<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="xc" %> 

<style type="text/css">
    .limit_250
    {}
    .auto-style1
    {
        float:left;
        vertical-align:top;
        width: 100px;
        padding:0;
        margin:0;
    }
    .dateRange
    {}
    .permanentNote
    {
        margin-right: 9px;
    }
    .auto-style2
    {
        float: left;
        vertical-align:top;
        width:600px;
        height: 20px;
    }
</style>

<div id="addNotePanel" style="background-color:aliceblue;width:100%; height: 173px;">
    <table style="width:100%; height: 33px; margin-bottom: 0px;">
        <tbody>
            <tr style="width:100%;">
                <td class="auto-style1" style="padding-left: 7px;">
                     <asp:Panel ID="AddPermanentNote" runat="server" CssClass="permanentNote" Width="120px">
                        <asp:CheckBox ID="PermanentNoteCheckBox" runat="server" OnCheckedChanged="PermanentNoteCheckBox_CheckChanged"  AutoPostBack="true" Text="Permanent Note" TextAlign="left" />
                     </asp:Panel>
                </td>
                <td class="auto-style2">
                     <div  id="CustomerNoteDescriptors" runat="server" class="customerNoteDescriptors">
                         <div style="width:47%; float:left;vertical-align:top;margin-left:5px;">
                            <span style="vertical-align:top;padding-left: 4px;">Activity type:</span><asp:DropDownList ID="ActivityTypeDropDownList" runat="server" Height="16px" Width="15px"/>
                         </div>
                         <div style="width:43%;float:left;vertical-align:top;margin-left:18px;">
                             <span  style="vertical-align:top;">Note type:</span><asp:DropDownList ID="NoteTypeDropDownList" runat="server" Height="16px" Width="50px" />
                         </div>
                         <div id="NextCallDueDiv" runat="server" style="width:30%;float:left;vertical-align:top;display: none;">
                            <span  style="vertical-align:top;">Next call due:</span><asp:TextBox ID="NextCallDueTextBox" CssClass="dateRange" runat="server" />
                        </div>
                     </div>
                </td>
            </tr>
        </tbody>
    </table>
         
      <div class="note" style="width:99%; height: 85px; margin: 0px 0px 0px 2px; padding:0;">
          <p style="width: 680px; padding-left: 8px;">
            <asp:TextBox ID="CommentTextBox" CssClass="customerNotes countdown limit_250" runat="server" TextMode="MultiLine" Width="634px" /> 
          </p>
          <p style="width: 680px;padding-left: 8px;">
              <asp:ImageButton ID="SaveButton" runat="server" AlternateText="Save" OnClick="SaveButton_Click"
                    ValidationGroup="customerNotes" ImageUrl="~/images/btn_save.png" OnClientClick="EmptyAddNotesFeedbackLabel();" />
              <asp:ImageButton ID="CancelButton" runat="server" AlternateText="Cancel" OnClick="CancelButton_Click"
                    ImageUrl="~/images/btn_cancel.png" />
              <span class="remaining">250 characters remaining.</span>   

              <asp:Label ID="FeedbackLabel" runat="server" CssClass="feedback" />
              <asp:RequiredFieldValidator ID="CommentTextBoxRequiredFieldValidator" runat="server"
                    ControlToValidate="CommentTextBox" ErrorMessage="Please enter a message." Display="Dynamic"
                    ValidationGroup="customerNotes" />
          </p>    
     </div>  
 </div>