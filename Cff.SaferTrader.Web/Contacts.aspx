<%@ Page Title="Cashflow Funding Limited | Debtor Management | Contacts"
    Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Contacts" ValidateRequest="false" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/SafeTrader.Master" %>
<%@ Import Namespace="Cff.SaferTrader.Core" %>

<%@ Register Src="~/UserControls/CalendarDateRangePicker.ascx" TagPrefix="uc" TagName="DateRangePicker" %>
<%@ Register Src="UserControls/AlphabeticalPagination.ascx" TagPrefix="uc" TagName="AlphabeticalPagination" %>
<%@ Register Src="UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls"  TagPrefix="uc"%>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    
    <script type="text/javascript">
        function UpdPanelUpdate() {
            //button.UniqueID -- Need UniqueID when included in a masterpage
            if (button.Text != "<%=customerContactsGridView.FocusedRowIndex.ToString()%>") {
                __doPostBack("<%= button.UniqueID %>", "");
            }
        }
    </script>

   <script type="text/javascript">

        //AtD.rpc_css = 'http://www.polishmywriting.com/atd-jquery/server/proxycss.php?data=';             
        $("#sendNotify").hide();
        $("#noRecipient").hide();
        $("#exceedSmsLimit").hide();
        $("#exceed160Char").hide();
        $("#emptyMessage").hide();
        $("#smsException").hide();
        $("#invalidEmailMsg").hide();
        smsAdjustButtons();

        var prm = Sys.WebForms.PageRequestManager.getInstance();   // check for postback,
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null | e.get_error() != undefined) {
                    smsAdjustButtons();
                    loadSpellChecker();
                    attachSmsCountDown(); // reloads sms char countdown function
                }
            });
        }
     
       function dialogMsg(divId) {
           $(divId).dialog({
               modal: true,
               buttons: {
                   Ok: function () { $(this).dialog("close"); }
               },
               show: {
                   effect: "scale",
                   duration: 300
               },
               hide: {
                   effect: "puff",
                   duration: 300
               }
           });
           $(divId).show();
       }

       function getCountDown() {
           //Get the Textbox control
           var textField = document.getElementById("<%#SMSMsg.ClientID %>");
           // Check if user is entering more than the limit of characters
           if (textField.value.length >= textField.maxLength) {
               // Cut extra text
               document.getElementById("<%#txtCount.ClientID %>").innerHTML = textField.maxLength - textField.value.length;
           }
           //Do the math of chars left and pass the value to the label
       }

       function Confirm(txt, notif) {
           var notify = notif;
           var confirm_value = document.createElement("INPUT");
           confirm_value.type = "hidden";
           confirm_value.name = "confirm_value";
           if (confirm(txt)) {
               confirm_value.value = "Yes";
           } else {
               confirm_value.value = "No";
           }
           document.forms[0].appendChild(confirm_value);
           var fValue = document.getElementById("<%=SMSMsg.ClientID%>").value;

           if (fValue.length > 5) {
               if (fValue > 260) {
                   dialogMsg("#exceedSmsLimit");
               }
           } else {
               if (notify == 'Yes' && confirm_value.value == "Yes")
                   dialogMsg("#emptyMessage");
           }
       }

       function ValidationAlert(msg) {
           var x = $("input[id][id*='ctl00_MainContentPlaceholder_customerContactsGridView_'][id$='_TbxEmail']").val() == null ? 0 : $("input[id][id*='ctl00_MainContentPlaceholder_customerContactsGridView_'][id$='_TbxEmail']").val();    // customer contact email
           var y = $("input[id][id*='ctl00_MainContentPlaceholder_customerContactsGridView_'][id$='_CbxEmailStatement']").prop('checked');

           var x2 = $("input[id][id*='ctl00_MainContentPlaceholder_clientContactsGridView_'][id$='_TbxEmail']").val() == null ? 0 : $("input[id][id*='ctl00_MainContentPlaceholder_clientContactsGridView_'][id$='_TbxEmail']").val();   // client customer contact email

           if ((!isEmailValid(x)) && (x.length > 0)) {
               if (x != "[add new]") {
                   alert("Email '" + x + "' is not valid.");
               }
           } else if ((!isEmailValid(x2)) && (x2.length > 0)) {
               if (x2 != "[add new]") {
                   alert("Email '" + x2 + "' is not valid.");
               }
           }

           if (y == true && ((!x.trim()) || (x == "[add new]"))) {
               alert("Email Statement not allowed when Email is empty or not valid.");
           }
       }          

       function smsAdjustButtons() {
           $("#ctl00_MainContentPlaceholder_phoneMeButton").css("width", 111);
           $("#ctl00_MainContentPlaceholder_clientButton").css("width", 72);
           $("#ctl00_MainContentPlaceholder_currentButton").css("width", 82);
           $("#ctl00_MainContentPlaceholder_tnxButton").css("width", 81);
           $("#ctl00_MainContentPlaceholder_emailMeButton").css("width", 95);
           $("#ctl00_MainContentPlaceholder_CustButton").css("width", 81);
           $("#ctl00_MainContentPlaceholder_balButton").css("width", 72);
           $("#ctl00_MainContentPlaceholder_greaterTwoMoButton").css("width", 93);
           $("#ctl00_MainContentPlaceholder_greaterOneMoButton").css("width", 95);
       }

       var isFirefox = typeof InstallTrigger !== 'undefined';   // Firefox 1.0+       

       window.onload = function () {
           $("#sendNotify").hide();
           $("#noRecipient").hide();
           $("#exceedSmsLimit").hide();
           $("#exceed160Char").hide();
           $("#emptyMessage").hide();
           $("#smsException").hide();
           $("#invalidEmailMsg").hide();
           smsAdjustButtons();
           loadSpellChecker();
           attachSmsCountDown(); // reloads sms char countdown function

           if (isFirefox) {   // enable inputSearch func for FireFox only
               setForFireFox();
           }
       }

       function loadSpellChecker() {

           //("#<%=SMSMsg.ClientID%>").addProofreader();
           $("#<%=SMSMsg.ClientID%>").spellAsYouType();
            
       }

       function setForFireFox() {
           $('#ClientSearch').inputSearch({ searchIconVisible: false });
           $('#ClientSearch').css('width', '100%');

           $('#CustomerSearch').inputSearch({ searchIconVisible: false });
           $('#CustomerSearch').css('width', '100%');
       }

       function appendNote(val) {   // append user note during edit/add events in gridview
           
           var fColumn;
           var rowIdx = val.slice(0, 2);
           var cmdType = val.slice(2, 6); 
           var vPos = val.indexOf("_") + 1; 
           var vType = val.slice((vPos), vPos + 3);  // values: "clc" - client contacts, "cuc" - customer contacts
           var ctNum = "_ctl" + rowIdx + "_";

           if (vType == "clc") {
               if (cmdType == 'new_' || cmdType == 'add_')
                   fColumn = $("<span style='font-weight:bold;font-size:20px;color:#88422b; padding-left: 35px;'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Add Client contact</span>")
                   .insertAfter("input[id][id*='ctl00_MainContentPlaceholder_clientContactsGridView" + ctNum + "'][id$='_TbxFirstName']");
               else
                   fColumn = $("<span style='font-weight:bold;font-size:20px;color:#88422b;padding-left: 35px;'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Edit Client contact</span>")
                    .insertAfter("input[id][id*='ctl00_MainContentPlaceholder_clientContactsGridView" + ctNum + "'][id$='_TbxFirstName']");
           } else {
               if (cmdType == "new_"  || cmdType == "add_")
                   fColumn = $("<span style='font-weight:bold;font-size:20px;color:#88422b; padding-left: 485px;'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Add Customer contact</span>")
                   .insertAfter("input[id][id*='ctl00_MainContentPlaceholder_customerContactsGridView" + ctNum + "'][id$='_CbxIsDefault']");
               else
                   fColumn = $("<span style='font-weight:bold;font-size:20px;color:#88422b; padding-left: 485px;'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Edit Customer contact</span>")
                   .insertAfter("input[id][id*='ctl00_MainContentPlaceholder_customerContactsGridView" + ctNum + "'][id$='_CbxIsDefault']");
           }
       }

   </script>

    <asp:Button ID="button" Text="click" runat="server" OnClick="button_Click" Style="display: none;" />
    <div id="contentHeader">
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" Height="16px" Width="16px" />
            </a>
            Contacts <% =targetName %>
        </h3>
        <asp:HiddenField ID="PaginationIndex" runat="server" />
    </div>

    <uc:PageDescription ID="transactionsPageDescription" runat="server" DescriptionTitle="" DescriptionContent="Contacts" />
    <table id="contentViewerContainer" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td id="contentViewer" class="contacts">
                    <div style="display: <%=(this.CurrentScope()==Scope.AllClientsScope) ? "none" : "block"%>">
                        <h4 class="contacts">Client contacts
                            <a id="ClientContactsToggle" onclick="toggleGrid('ClientContactsToggle', 'clientSection');return false;">
                                <img src="images/expand.png" alt="expand" />
                            </a>
                        </h4>
                    </div>
                    <div id="clientSection" style="display: none">
                       <asp:UpdatePanel ID="ClientContactSectionUpdatePanel" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                            </Triggers>

                            <ContentTemplate>
                                <asp:Panel ID="ClientContactsPanel" runat="server" DefaultButton="ClientContactsSearchButton">
                                    <div id="ClientContact" style="display: <%=(this.CurrentScope()==Scope.AllClientsScope) ? "none" : "block"%>">
                                        <asp:UpdatePanel ID="ClientSearchSectionUpdatePanel" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <div class="parameterSelector" id="ClientSearchSection" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="ClientContactsSearchTextBoxUpdatePanel" runat="server" UpdateMode="Conditional">
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                                                        <asp:AsyncPostBackTrigger ControlID="ClientAlphabeticalPagination" />
                                                                    </Triggers>
                                                                    <ContentTemplate>
                                                                        <uc:SecureTextBox runat="server" ID="ClientContactsSearchTextBox"></uc:SecureTextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ClientContactsSearchButton" runat="server" AlternateText="Update"
                                                                    ImageUrl="~/images/btn_search_blue.png" OnClick="ClientContactsSearchButton_Click" />
                                                                <span class="error"></span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <uc:AlphabeticalPagination ID="ClientAlphabeticalPagination" IsClientContactsIndexing="true" runat="server" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>                                        
                                        <asp:UpdatePanel ID="ClientContactUpdatePanel" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ClientContactsSearchButton" />
                                                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                                <asp:AsyncPostBackTrigger ControlID="ClientAlphabeticalPagination" />
                                                <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="clientContactsGridView" EventName="RowCommand" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:PlaceHolder ID="clientContactsPlaceholder" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <h4 id="CustomerContactsHeader" style="display: <%=(this.CurrentScope()==Scope.AllClientsScope) ? "none" : "block"%>">Customer contacts
                        <a id="CustomerContactsToggle" onclick="toggleGrid('CustomerContactsToggle', 'CustomerContact');return false;">
                        <img src="images/collapse.png" alt="collapse" style="border: none; display: <%=(this.CurrentScope()==Scope.AllClientsScope) ? "none" : "inline"%>" /></a>
                    </h4>
                    <div id="CustomerContact" style="vertical-align: bottom; display: <%=(this.CurrentScope()==Scope.AllClientsScope) ? "none" : "block"%>">
                        <asp:UpdatePanel ID="CustomerContactSectionUpdatePanel" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="CustomerPanel" runat="server" DefaultButton="CustomerContactsSearchButton">
                                    <div class="parameterSelector" id="CustomerSearchSection" runat="server">
                                        <table>    <!-- <table width="100%"> -->
                                            <tr>
                                                <td style="height: 28px">
                                                    <asp:UpdatePanel ID="CustomerContactsSearchTextBoxUpdatePanel" runat="server" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                                            <asp:AsyncPostBackTrigger ControlID="CustomerAlphabeticalPagination" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <uc:SecureTextBox runat="server" ID="CustomerContactsSearchTextBox"></uc:SecureTextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td style="height: 28px">
                                                    <asp:ImageButton ID="CustomerContactsSearchButton" runat="server" AlternateText="Update"
                                                        ImageUrl="images/btn_search_blue.png" OnClick="CustomerContactsSearchButton_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <span class="error"></span>
                                        <uc:AlphabeticalPagination ID="CustomerAlphabeticalPagination" IsClientContactsIndexing="false" runat="server" />
                                    </div>

                                    <asp:UpdatePanel ID="CustomerContactUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="CustomerContactsSearchButton" />
                                            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                            <asp:AsyncPostBackTrigger ControlID="CustomerAlphabeticalPagination" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlEmailtoCustomer" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="customerContactsGridView" EventName="RowCommand" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:PlaceHolder ID="CustomerContactsPlaceHolder" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div id="Letters">
                            <table>




                            </table>
                            <asp:PlaceHolder ID="LettersPH" runat="server">
                                <hr style="color: grey; height: -12px" />
                                <asp:UpdatePanel ID="GroupBtnSendLetters" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <table style="border: none;">
                                            <tr>
                                                <td><span style="color: Teal; font-weight: bold; font-size: small;">Choose letter:&nbsp;</span></td>
                                                <td style="height: 0px" valign="middle">
                                                    <asp:DropDownList ID="ddlLetters" runat="server" Height="22px" Width="199px"></asp:DropDownList>&nbsp;</td>
                                                <td style="height: 0px" valign="middle">
                                                    <asp:Button ID="btnSendLetter" class="hvr-push" runat="server" Height="22px" Text="Send Letter" Width="143px" BackColor="Honeydew" OnClick="btnSendLetter_Click" />
                                                </td>
                                                <td>
                                                    <span style="font-size: small;">
                                                        <asp:Literal ID="sendStatusLiteral" runat="server"></asp:Literal>
                                                        <asp:HyperLink ID="hyperLinkFileGen" runat="server">Download this file</asp:HyperLink>
                                                        &nbsp; 
                                                        <asp:Label ID="lblDownloadHyperlink" runat="server" Text=" Click on link to download document."></asp:Label>
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <table width="100%" style="border: none;">
                                    <tbody>
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:UpdatePanel ID="GroupOutputTo" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="left">
                                                                    <span style="color: Teal; font-weight: bold; font-size: small;">Output Letter To:</span><br />
                                                                    <asp:RadioButton ID="rbtnEmailFormat" AutoPostBack="true" runat="server" Text="Email Format" GroupName="OutputTo" CssClass="radiobtn" OnCheckedChanged="rbtnEmailFormat_CheckedChanged" Visible="False" /><br />
                                                                    <asp:RadioButton ID="rbtnEmailPDF" AutoPostBack="true" runat="server" Text="Email PDF" GroupName="OutputTo" CssClass="radiobtn" Checked="True" OnCheckedChanged="rbtnEmailPDF_CheckedChanged" /><br />
                                                                    <asp:RadioButton ID="rbtnEmailWord" AutoPostBack="true" runat="server" Text="Email Word" GroupName="OutputTo" CssClass="radiobtn" OnCheckedChanged="rbtnEmailWord_CheckedChanged" /><br />
                                                                    <asp:RadioButton ID="rbtnWordPrinter" AutoPostBack="true" runat="server" Text="Word-Screen" GroupName="OutputTo" CssClass="radiobtn" OnCheckedChanged="rbtnWordPrinter_CheckedChanged" /><br />
                                                                    <asp:RadioButton ID="rbtnPDFFile" AutoPostBack="true" runat="server" Text="To PDF File" GroupName="OutputTo" CssClass="radiobtn" OnCheckedChanged="rbtnPDFFile_CheckedChanged" /><br />
                                                                    <asp:RadioButton ID="rbtnWordScreen" AutoPostBack="true" runat="server" Text="HTML-No Logo" GroupName="OutputTo" CssClass="radiobtn" OnCheckedChanged="rbtnWordScreen_CheckedChanged" Visible="False"/><br />
                                                                    <span style="vertical-align: middle">
                                                                        <asp:RadioButton Visible="false" ID="rbtnSendFax" AutoPostBack="true" runat="server" Text="Fax " GroupName="OutputTo" CssClass="radiobtn" OnCheckedChanged="rbtnSendFax_CheckedChanged" />
                                                                        <asp:DropDownList Visible="false" ID="ddlFaxToCustomer" AutoPostBack="false" runat="server" Height="20px" Width="75px" OnSelectedIndexChanged="ddlFaxToCustomer_SelectedIndexChanged"></asp:DropDownList>
                                                                    </span>
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 660px">
                                                <asp:UpdatePanel ID="GroupSendTo" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="customerContactsGridView" EventName="RowClicked" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlEmailToCustomer" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left" style="width: 310px">
                                                                        <span style="color: Teal; font-weight: bold; font-size: small;">Send: </span>
                                                                        <br />
                                                                        <span style="vertical-align: middle">
                                                                            <asp:RadioButton ID="rbtnEmailToCustomer" AutoPostBack="false" runat="server" Text="Email : " GroupName="SendTo" CssClass="radiobtn" />
                                                                            <asp:DropDownList ID="ddlEmailToCustomer" AutoPostBack="true" runat="server" Height="22px" Width="199px" OnSelectedIndexChanged="ddlEmailtoCustomer_SelectedIndexChanged"></asp:DropDownList>
                                                                        </span>
                                                                        <br />
                                                                        <asp:RadioButton ID="rbtnEmailClientCust" AutoPostBack="false" runat="server" Text="Email to client & customer" GroupName="SendTo" CssClass="radiobtn" /><br />
                                                                        <asp:RadioButton ID="rbtnEmailToClient" AutoPostBack="false" runat="server" Text="Email to client" GroupName="SendTo" CssClass="radiobtn" /><br />
                                                                        <asp:RadioButton Visible="false" ID="rbtnEmailToCollector" AutoPostBack="false" runat="server" Text="Email to collector" GroupName="SendTo" CssClass="radiobtn" /><br />
                                                                        <asp:CheckBox ID="chkboxEditEmail" AutoPostBack="false" runat="server" Text="Edit Email" CssClass="chkbox" /><br />
                                                                    </td>
                                                                    <td align="left" style="width: 180px;">
                                                                        <span style="color: Teal; font-weight: bold; font-size: small;">Salutation(s): </span>
                                                                        <br />
                                                                        <asp:CheckBox ID="chkboxAttn" Checked="true" AutoPostBack="true" runat="server" Text=" Attn:" CssClass="chkbox" OnCheckedChanged="chkboxAttn_CheckedChanged" />
                                                                        <span style="font-size: small;">&nbsp;<asp:Label ID="custContactNameLiteral" runat="server"></asp:Label></span><br />

                                                                        <asp:CheckBox ID="chkboxDear" Checked="true" AutoPostBack="true" runat="server" Text=" Dear:" CssClass="chkbox" OnCheckedChanged="chkboxDear_CheckedChanged" />
                                                                        <span style="font-size: small;">&nbsp;<asp:Literal ID="custFNameLiteral" runat="server"></asp:Literal></span><br />
                                                                    </td>
                                                                    <td align="left" style="width: 370px;">
                                                                        <p style="background-color: #EEFCE4; width: 130px; line-height: 8px">
                                                                            <span style="font-weight: bold; background-color: Green; color: White; font-size: small;">&nbsp;&nbsp;&nbsp;Special&nbsp;Cases&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                            <br />
                                                                            <br />
                                                                            <span style="vertical-align: middle; height: 40px">
                                                                                <asp:Button ID="btnEmailBankDetails" class="hvr-push" runat="server" Height="22px" Text="Email Bank Details" Width="125px" BackColor="Honeydew" OnClick="btnEmailBankDetails_Click" /></span>
                                                                            <br />
                                                                            <br />
                                                                            <span style="vertical-align: middle; height: 40px">
                                                                                <asp:Button ID="btnGenerateStatement" class="hvr-push" runat="server" Height="22px" Text="Generate Statement" Width="125px" BackColor="Honeydew" OnClick="btnGenerateStatement_Click" />
                                                                            </span>
                                                                        </p>
                                                                        <span style="font-size: small;">
                                                                            <asp:Literal ID="reportStatementLiteral" runat="server" Visible="false"></asp:Literal>
                                                                            <asp:HyperLink ID="hlReportStatement" runat="server"></asp:HyperLink>
                                                                            &nbsp; 
                                                                            <asp:Label ID="lblReportStatement" runat="server" Text=" Click on link to download document." Visible="false"></asp:Label>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <%--<hr style="height: -12px" />--%>
                                <asp:UpdatePanel ID="hiddenFieldsPanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Literal ID="hfDDEmailIdxLiteral" runat="server" Visible="false"></asp:Literal>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="smsPH" runat="server">
                        <h4 class="contacts" style="display: <%=(this.CurrentScope()==Scope.ClientScope) ? "none" : "block"%>">Send SMS
                            <a id="sendSMSToggle" onclick="toggleGrid('sendSMSToggle', 'sms');return false;">
                                <img src="images/collapse.png" alt="collapse" />
                            </a>
                        </h4>
                        <div id="sms" style="display: <%=(this.CurrentScope()==Scope.ClientScope) ? "none" : "block"%>">
                            <table>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="smsPanel" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <table style="width: 55%;">
                                                    <tr>
                                                        <td class="smsMsg">
                                                            <br />
                                                            <span style="font: caption; padding-top: 2px">Mobile # </span>
                                                            <asp:TextBox ID="mobileNum" BackColor="#FFF0F5" runat="server" ReadOnly="True"></asp:TextBox><span style="padding-left: 7px; padding-top: 2px">Name
                                                                <asp:TextBox BackColor="#FFF0F5" ID="custContactNameMobileLiteral" runat="server" ReadOnly="True"></asp:TextBox></span>
                                                            <br />
                                                            <br />
                                                            <%--<asp:TextBox ID="SMSMsg" Width="553px" Height="80px"  CssClass="customerNotes countdown limit_250" TextMode="MultiLine" MaxLength="260" BackColor="#E6E6FA" runat="server"></asp:TextBox>--%>
                                                            <asp:TextBox ID="SMSMsg" Width="406px" Height="88px" CssClass="smsMsg countdown" TextMode="MultiLine" MaxLength="260" BackColor="#E6E6FA" runat="server"
                                                                OnTextChanged="SMSMsg_OnTextChanged" AutoPostBack="True" Font-Size="Small">
                                                            </asp:TextBox>                                                                
                                                            <div style="width:100%;text-align:right">
                                                                <asp:Button ID="clearButton" class="hvr-push" runat="server" Text="Erase all"  Width="113px" BackColor="Honeydew" OnClick="ClearButton_Click" OnClientClick="Confirm('Are you sure to erase all?','No');" />                                                        
                                                                <asp:Button ID="smsButton" class="hvr-push" runat="server" Text="Send" Width="113px" BackColor="Honeydew" OnClick="btnSendSMS_Click" OnClientClick="Confirm('Are you sure?','Yes');" /><!-- dbb -->
                                                                <input type="button" style="width:113px;" class="hvr-push" id="spellCheckButton" value="Spell check" onclick="$('#ctl00_MainContentPlaceholder_SMSMsg').spellCheckInDialog({ theme: 'clean' });" />
                                                            </div>                                                                 
                                                        </td>                                                            
                                                    </tr>                                                    
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="smsOpt" runat="server" >
                                            <ContentTemplate>
                                                <table style="border-width: 1px;">
                                                    <tbody>
                                                        <tr>
                                                            <p style="width: 450px; padding-left: 8px; border: 1px solid lightgray; padding-bottom: 9px;"> <!-- <p style="width: 634px; padding-left: 8px; border: 1px solid lightgray; padding-bottom: 5px"> dbb [20160713] -->
                                                                <asp:Literal runat="server"><span style="font:caption;"><b>SMS Compose buttons:</b></span></asp:Literal><br />
                                                                <asp:Button ID="hiButton" Text="Hi 1st name" runat="server" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button ID="tnxButton" Text="thanks" runat="server" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button ID="cffButton" Text="Cashflow Funding Limited" runat="server" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button ID="phoneMeButton" Text="Please phone me" runat="server" OnClick="ComposeSmsButton_Click"  />
                                                                <asp:Button ID="oButton" Text="01 0242 0146299 000" runat="server" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button ID="emailMeButton" Text="Email me" runat="server" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="yourAcctWithButton" Text="re your account with" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="clientButton" Text="Client" OnClick="ComposeSmsButton_Click"  />
                                                                <asp:Button runat="server" ID="CustButton" Text="Customer" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="plsPayButton" Text="Please pay" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="balButton" Text="Balance" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="threePlusMoButton" Text="3+ Month" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="twoPlusMoButton" Text="2 Month" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="oneMoButton" Text="1 Month" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="currentButton" Text="Current" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="greaterTwoMoButton" Text=">=2 Month" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="greaterOneMoButton" Text=">=1 Month" OnClick="ComposeSmsButton_Click" />
                                                                <asp:TextBox runat="server" ID="dateButton" CssClass="smsToDateRange" OnTextChanged="ComposeSmsTextBox_Click" AutoPostBack="True"></asp:TextBox>
                                                                <asp:DropDownList ID="whenButton" runat="server" OnSelectedIndexChanged="ComposeSmsDdl_Click" AutoPostBack="True">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem Selected="True">--When--</asp:ListItem>
                                                                    <asp:ListItem> now</asp:ListItem>
                                                                    <asp:ListItem> today</asp:ListItem>
                                                                    <asp:ListItem> tomorrow</asp:ListItem>
                                                                    <asp:ListItem> within 7 days</asp:ListItem>
                                                                    <asp:ListItem> by the weekend</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Button runat="server" ID="replyQuoteButton" Text="Reply quoting:" OnClick="ComposeSmsButton_Click" />
                                                                <asp:Button runat="server" ID="refButton" Text="Reference" OnClick="ComposeSmsButton_Click" />
                                                            </p>
                                                        </tr>
                                                        <tr>
                                                            <asp:Label runat="server" ID="txtCount" class="remaining"></asp:Label>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>  
                                    </td>      
                                 </tr>                                            
                            </table>
                        </div>
                    </asp:PlaceHolder>
                </td>
                <td id="rhToggle">
                    <div id="rhToggleIcon">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="sendNotify" title="Success" class="ui-widget-content ui-corner-all">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Message sent.
        </p>
    </div>
    <div id="noRecipient" title="Send failed" class="ui-widget-content ui-corner-all">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
            No mobile number selected, please provide one.
        </p>
    </div>
    <div id="exceedSmsLimit" title="Send failed" class="ui-widget-content ui-corner-all">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
            Cannot send more than 260 characters.
        </p>
    </div>
    <div id="exceed160Char" title="Notification" class="ui-widget-content ui-corner-all">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
            Message body exceeds 160 characters limit, please check.
        </p>
    </div>
    <div id="emptyMessage" title="Send failed" class="ui-widget-content ui-corner-all">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
            SMS empty, try again.
        </p>
    </div>
    <div id="smsException" title="Error" class="ui-widget-content ui-corner-all">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
            Something wrong when sending sms to the server, You may try again later.
        </p>
    </div>
    <div id="invalidEmailMsg" title="Notification" class="ui-widget-content ui-corner-all">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
            The email address provided is invalid.
        </p>
    </div>
</asp:Content>

