<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogOn.aspx.cs" Inherits="Cff.SaferTrader.Web.LogOn" %>


<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register TagPrefix="uc" TagName="ManagementContactDetails" Src="~/UserControls/ManagementContactDetails.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7,8,9,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cashflow Funding Limited | Debtor Management</title>
    <link rel="stylesheet" href="css/style.css" type="text/css" media="all" />
    <link rel="stylesheet" href="js/ui.1.10.4/themes/smoothness/jquery-ui.css" type="text/css" media="all" />
     
    <script src="js/jquery.min.js"  type="text/javascript"></script>
    <%-- <script src="js/ui.1.10.4/ui/jquery-ui.js" type="text/javascript"></script>     
    <script src="js/SuperFish/superfish.js" type="text/javascript"></script>
    <script src="js/jquery.countdown.min.js" type="text/javascript"></script>--%>

    <style type="text/css">
       #Client input { margin-bottom: 5px; text-align:center; margin-left:10px; margin-right:5px; }

       .announcement {
            padding: 0px 10px 0px 37px; 
            width: 450px; 
            color:brown; 
            text-align:center;
            font-size:12px;
            -webkit-flex-wrap: wrap; /* Safari 6.1+ */
            display: flex;
            flex-wrap: wrap;
            word-wrap: break-word;
       }      

    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#cTimer').countdown("2016/08/12 18:00:00", function (event) {
                var totalHrs = event.offset.totalDays * 24 + event.offset.hours;
                var totalMins = event.offset.totalMinutes / 60 + event.offset.minutes;
                var totalSecs = event.offset.seconds;
                //$(this).html(event.strftime(totalHrs + ' hr %M min %S sec'));

                if (totalHrs > 0 || totalMins > 0 || totalSecs > 0) {
                    $(this).css("display", "block");
                }

                if (totalHrs == 0 && totalMins == 0 && totalSecs == 0) {
                    $(this).css("display","none");
                    $(this).countdown('stop');
                    $(this).stop();
                }

            });
        });
    </script>
</head>

<body>
    <form id="form2" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server">
            <Scripts>
                <asp:ScriptReference Path="js/ui.1.10.4/ui/jquery-ui.js" />
                <asp:ScriptReference Path="js/SuperFish/superfish.js" />
                <asp:ScriptReference Path="js/jquery.countdown.min.js" />
            </Scripts>
        </asp:ScriptManager>

        <div id="wrapper" class="login" runat="server">
            <!--JUST PLAIN MESSAGE,THANKYOU,CONGRATULATION,ETC. TEMPLATE -->
            <div ID="ResponseMessage" runat="server"><br />
                <h1>Cashflow Funding Limited</h1>
                <div class="pwdRequestBlackText14">
                    <div id="ResponseMessageTitle" runat="server"/>
                </div>            
                <table border="0" width="92%">
                    <tr>
                      <td>
                        <div class="pwdRequestBlackText14">
                            <div id="ResponseMessageBody" runat="server"/>
                        </div><br />
                        <div class="pwdRequestBlackText14">If you want to sign-in, <a href="Logon.aspx">Login</a></div><br />
                        <div class="pwdRequestBlackText14">If you want to visit our website, <a href="http://www.factors.co.nz">Cashflow Funding Limited</a></div><br />
                      </td>
                    </tr>
                </table>
                <div class="loginFooter">
                    <p>
                        Phone  09 579 4204 - Fax 09 525 3598 - Email info@factor.co.nz - Web www.factors.co.nz
                    </p>
                </div>
            </div>
        
            <asp:Login ID="LogOnControl" runat="server" DestinationPageUrl="~/LogOnRedirection.aspx"
                OnLoginError="LogOnControl_OnLogOnError" OnLoggedIn="LogOnControl_LoggedOn" Visible="true">
                <LayoutTemplate>
                    <h1>Cashflow Funding Limited</h1>
                    <div class="announcement" id="cTimer" style="display:none;">   
                         <p>“Please note that we will be switching our internet connection on the <b>Friday 12th August after 4:30pm</b>.  Please also note that it could take a while before our new set up is ‘visible’”. </p>
                    </div>
                    <div class="loginUsername">
                        <asp:TextBox ID="UserName" runat="server" />
                         <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                            ErrorMessage="Username is required" ValidationGroup="Login1" Text="Username is required" />
                    </div>
                    <div class="loginPassword">
                        <asp:TextBox ID="Password" TextMode="Password" runat="server" />
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                            ErrorMessage="Password is required" ValidationGroup="Login1" Text="Password is required" />
                    </div>
                    <div class="loginButton">
                        <asp:HiddenField ID="hField" runat="server" Visible="false" />
                                <asp:ImageButton ID="LoginButton" runat="server" CssClass="blueButton" CommandName="Login" ImageUrl="~/images/btn_login.png" ValidationGroup="Login1"  />
                    
                        <div class="remembermeTxt">
                            <table width="85%" border="0">
                                <tr>
                                    <td style="text-align:center; vertical-align:middle;"><asp:CheckBox ID="RememberMe" runat="server" Font-Size="0.7em" /></td>
                                    <td style="text-align:left; vertical-align:middle;">Remember Me</td>
                                    <td style="text-align:right; vertical-align:middle;"><a href="LogOn.aspx?ComID=forgotPwd">Forgot your password?</a></td>
                                    <td style="text-align:right; vertical-align:middle;"><a href="LogOn.aspx?ComID=newAccnt">Register</a></td>
                                </tr>
                            </table>
                        </div>
                    
                        <div class="loginFailureText">
                            <asp:Label ID="FailureText" runat="server" />
                        </div>
                        <div><div style="float:left">Any interaction with this web site is governed by the terms & conditions of the &nbsp;</div><div style="float:left"><asp:Button ID="Agreement" OnClick="OnAgreementClick" Text="Agreement" runat="server"/></div></div>
                    </div>   
                    <div class="loginFooter">
                        <p>
                            Phone: 09 579 4204 - Fax: 09 525 3598 - Email: info@factor.co.nz - Web: www.factors.co.nz
                        </p>
                    </div>
                </LayoutTemplate>
            </asp:Login>
        
            <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" maildefinition-from="webadmin@factor.co.nz" 
                    OnUserLookupError="LogOnUserLookupError" OnAnswerLookupError = "LogOnAnswerLookupError" 
                        OnSendMailError="LogOnSendingMailError" OnSendingMail="LogOnSendingMail">
                <UserNameTemplate>
                    <h1>Cashflow Funding Limited</h1>
                    <div class="pwdRequestBlackText14"><b>Forgot Password</b></div>
                    <div class="pwdRequestBlackText14">If you want to sign-in, <a href="Logon.aspx">Login</a></div>
                    <div class="pwdRequestBlackText14">If you want to create an account, <a href="Logon.aspx?ComID=newAccnt">Sign-up to join</a></div>
                    <div class="loginUsername">
                        <asp:TextBox ID="UserName" runat="server" />
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                            ErrorMessage="Username is required" ValidationGroup="PasswordRecovery1" Text="Username is required." />
                    </div>
                    <div class="submitButton">
                        <asp:ImageButton ID="SubmitButton" runat="server" CssClass="blueButton" CommandName="Submit" ImageUrl="~/images/btn_ok.png" ValidationGroup="PasswordRecovery1"  />
                        <div class="loginFailureText">
                            <asp:Label ID="FailureText" runat="server" />
                        </div>
                    </div>
                    <div class="loginFooter">
                        <p>
                            Phone  09 579 4204 - Fax 09 525 3598 - Email info@factor.co.nz - Web www.factors.co.nz
                        </p>
                    </div>
                </UserNameTemplate>
                <MailDefinition From="webadmin@factor.co.nz"></MailDefinition>
                <SuccessTemplate>
                    <h1>Cashflow Funding Limited</h1>
                    <div class="pwdRequestBlackText14">A new password has been sent to your e-mail. Click <a href="LogOn.aspx">here</a> to login</div>
                    <div class="loginFooter">
                        <p>
                            Phone  09 579 4204 - Fax 09 525 3598 - Email info@factor.co.nz - Web www.factors.co.nz
                        </p>
                    </div>
                </SuccessTemplate>
            </asp:PasswordRecovery>
        
            <asp:CreateUserWizard ID="CreateUserWizard1" 
                runat="server" 
                ContinueDestinationPageUrl="~/LogOn.aspx" 
                OnCreatingUser="CreateUserWizard1_CreatingUser" 
                OnCreatedUser="CreateuserWizard1_CreatedUser" 
                PasswordRegularExpression='@\"(?:.{7,})(?=(.*\d){1,})(?=(.*\W){1,})' 
                PasswordRegularExpressionErrorMessage="Your password must be 7 characters long, and contain at least one number and one special character.">
                <WizardSteps>
                    <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                        <ContentTemplate>
                            <h1>Cashflow Funding Limited</h1>
                            <div class="pwdRequestBlackText14"><b>Free Registration</b></div>
                            <div class="pwdRequestBlackText14">If you already have existing account, <a href="Logon.aspx">Login</a></div>
                            <div class="pwdRequestBlackText14">If you forgot your password, <a href="Logon.aspx?ComID=forgotPwd">Remind me with my password</a></div><br />
                            <table border="0">
                                <tr>
                                    <td>
                                        <div class="nameField">
                                            <div class="pwdRequestBlackText14">Full Name:</div>
                                            <asp:TextBox ID="Name" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator 
                                                ControlToValidate="Name" 
                                                ErrorMessage="Fullname is required."
                                                ID="FullnameFieldValidator" 
                                                runat="server" 
                                                ToolTip="Fullname is required." 
                                                ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="pwdRequestBlackText14">E-mail:</div>
                                        <div class="userField">
                                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ControlToValidate="UserName" 
                                                ErrorMessage="Email is required." 
                                                ID="UserNameFieldValidator" 
                                                runat="server" 
                                                ToolTip="Email is required." 
                                                ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                             <asp:RegularExpressionValidator id="UserNameValidity" 
                                                 ControlToValidate="UserName"
                                                 ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)" 
                                                 Display="Static"
                                                 EnableClientScript="false"
                                                 ErrorMessage="Email address is invalid"
                                                 runat="server"/>
                                        </div>
                                        <div class="emailField">
                                            <asp:TextBox ID="Email" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                        <div class="passwordField">
                                            <div class="pwdRequestBlackText14">Password:</div>
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator 
                                                ControlToValidate="Password" 
                                                ErrorMessage="Password is required."
                                                ID="PasswordFieldValidator" 
                                                runat="server" 
                                                ToolTip="Password is required." 
                                                ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                        
                                        </div>
                                        <div class="confirmPasswordField">
                                            <div class="pwdRequestBlackText14">Confirm Password:</div>
                                            <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator 
                                                ControlToValidate="ConfirmPassword" 
                                                ErrorMessage="Confirm Password is required."
                                                ID="ConfirmPasswordFieldValidator" 
                                                runat="server" 
                                                ToolTip="Confirm Password is required." 
                                                ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                            <asp:CompareValidator runat="server" 
                                                ID="PasswordCompareValidator" 
                                                ControlToValidate="ConfirmPassword" 
                                                ControlToCompare="Password" 
                                                ErrorMessage="Your password does not match"></asp:CompareValidator>
                                        </div>
                                         <div class="passKeyField">
                                            <div class="pwdRequestBlackText14">Pass Key:</div>
                                            <asp:TextBox ID="PassKey" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator 
                                                ControlToValidate="PassKey" 
                                                ErrorMessage="PassKey is required."
                                                ID="PassKeyFieldValidator" 
                                                runat="server" 
                                                ToolTip="PassKey is required." 
                                                ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="errorField2">
                                            <asp:Literal EnableViewState="False" ID="ErrorMessage" runat="server"></asp:Literal>
                                            <asp:Label EnableViewState="False" ID="ErrorLabel" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <CustomNavigationTemplate>
                            <div class="continueButton">
                                <asp:ImageButton ID="StepNextButton" runat="server" CssClass="blueButton" CommandName="MoveNext"
                                    ImageUrl="~/images/btn_gcontinue.png" ValidationGroup="CreateUserWizard1" CausesValidation="true"  />
                                <div class="loginFailureText">
                                    <asp:Label ID="FailureText" runat="server"/>
                                </div>
                            </div>
                        </CustomNavigationTemplate>
                    </asp:CreateUserWizardStep>
                    <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                        <ContentTemplate>
                            <h1>Cashflow Funding Limited</h1>
                            <div class="pwdRequestBlackText14"><h4></b>Thank you!<br /></h4></div>
                            <table border="0" width="92%">
                                <tr>
                                  <td>
                                    <div class="pwdRequestBlackText14">
                                        Your account has been created. However, for security reason this website requires email verification and account activation from you company registrant.<br /><br />
                                        Your confirmation link has been sent to your email address.
                                    </div><br />
                                    <div class="pwdRequestBlackText14">If you want to sign-in, <a href="Logon.aspx">Login</a></div><br />
                                    <div class="pwdRequestBlackText14">If you want to visit our website, <a href="http://www.factors.co.nz">Cashflow Funding Limited</a></div><br />
                                  </td>
                                </tr>
                            </table>
                            <div class="loginFooter">
                                <p>
                                    Phone  09 579 4204 - Fax 09 525 3598 - Email info@factor.co.nz - Web www.factors.co.nz
                                </p>
                            </div>
                        </ContentTemplate>
                    </asp:CompleteWizardStep>
                </WizardSteps>
            
                <FinishNavigationTemplate>
                    <div class="continueButton">
                        <asp:ImageButton ID="StepNextButton" runat="server" CssClass="blueButton" CommandName="MoveComplete" ImageUrl="images/btn_joinmonex.png" ValidationGroup="CreateUserWizardStep2"  />
                        <div class="loginFailureText">
                            <asp:Label ID="FailureText" runat="server" />
                        </div>
                    </div>
                </FinishNavigationTemplate>
            
            </asp:CreateUserWizard>
        </div>
    
        <div id="mywrapper" class="access" visible="false" runat="server">
            <div id="access_content">
                    <h1>Cashflow Funding Limited</h1>
                    <div id="tab" style="height:auto;">
                        <ul style="list-style-type: none;">
                            <li id="clientTab" runat="server" visible="false">
                                <a href="#Client">Client</a>
                            </li>
                            <li id="custTab" runat="server" visible="false">
                                <a href="#Customer">Customer</a>
                            </li>
                        </ul>
                        <div id="Client">
                            <asp:PlaceHolder ID="clientPlaceholder" runat="server" />
                        </div>
                        <div id="Customer">
                            <asp:PlaceHolder ID="customerPlaceholder" runat="server" />
                        </div>
                   </div>
                   <div class="loginFooter">
                        <p>
                            Phone  09 579 4204 - Fax 09 525 3598 - Email info@factor.co.nz - Web www.factors.co.nz
                        </p>
                    </div>
                </div>
        </div>
    </form>
</body>
 
</html>


