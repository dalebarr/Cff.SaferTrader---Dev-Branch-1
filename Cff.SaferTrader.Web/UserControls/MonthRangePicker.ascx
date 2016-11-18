<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonthRangePicker.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.MonthRangePicker" %>

<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>

<script type="text/javascript">
    function validateDateRange() {
         var data = "{'startDate':'" + $(".fromDropDownList").val() + "', 'endDate':'" + $(".toDropDownList").val() + "'}"
        callValidationService("ValidateDateRange", data, success);
    }

    function success(message) {
        $(".error").text(message.d);
        if (message.d != '') {
            $(".updateButton").attr("disabled", "disabled");
        }
        else {
            $(".updateButton").removeAttr("disabled");
        }
        setTimeout("stopAnimate()", 500);
    }

    function changeDocumentCursor() {
        document.body.style.cursor = "wait";
        $("#spanUpdateButton").attr("title", "Updating...");

        setTimeout(function () {
            document.body.style.cursor = "auto";
            $("#spanUpdateButton").attr("title", "Update");
        }, 500);
    }

    $(document).ready(function() {
        validateDateRange();
    });


</script>

<div class="parameterSelector">    
    <table style="vertical-align:top">
        <tbody>
            <tr>
                <td>
                    <label>
                        From:</label>
                    <asp:DropDownList ID="FromDropDownList" CssClass="fromDropDownList" runat="server" />
                </td>
                <td>
                    <label>
                        To:</label>
                    <asp:DropDownList ID="ToDropDownList" CssClass="toDropDownList" runat="server" />
                </td>
                <td>
                    <uc:AllClientsFilter ID="allClientsFilter" runat="server" UpdateButtonVisible="false" Visible="false" />
                </td>
                <td id="UpdateButtonColumn" runat="server">
                    <span id="spanUpdateButton" title="Update" onmouseover="document.body.style.cursor='pointer';" onmouseout="document.body.style.cursor='default';">    
                        <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="../images/btn_update.gif" Width="80px" Height="25px"
                                CssClass="updateButton" OnClick="UpdateButton_Click" OnClientClick="changeDocumentCursor();"/>
                    </span>
                </td>
            </tr>
        </tbody>
    </table>
</div>
