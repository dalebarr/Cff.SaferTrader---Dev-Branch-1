<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerNotesFilter.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.CustomerNotesFilter" %>
<%@ Register Src="~/UserControls/CalendarDateRangePicker.ascx" TagPrefix="uc" TagName="CalendarDateRangePicker" %>
<div id="CustomerNotesFiler" class="parameterSelector">
    <table>
        <tr>
            <td>
                From: <asp:TextBox ID="dtCustomerNotesFilterFrom" runat="server" ClientIDMode="AutoID" ></asp:TextBox> 
                    &nbsp; To: <asp:TextBox ID="dtCustomerNotesFilterTo" runat="server" ClientIDMode="AutoID"></asp:TextBox>
            </td>
            <td>
                <label>
                    Activity type:</label>
                <asp:DropDownList ID="ActivityTypeDropDownList" runat="server" />
            </td>
            <td>
                <label>
                    Note type:</label>
                <asp:DropDownList ID="NoteTypeDropDownList" runat="server" />
            </td>
            <td>
                <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                    OnClientClick="startAnimate();" CssClass="updateButton calendarRelatedButton"
                    OnClick="UpdateButton_Click" />
            </td>
        </tr>
    </table>
    <span class="error"></span>
</div>
