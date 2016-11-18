<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ChangePassword" %>
<%@ Register TagPrefix="uc" TagName="ManagementContactDetails" Src="ManagementContactDetails.ascx" %>

<div id="modalBox" style="margin-top: -150px;">
    <div class="changePassword">
        <asp:ChangePassword ID="ChangePassword1" runat="server" OnChangedPassword="OnChangedPassword">
            <ChangePasswordTemplate>
                <h1>Change Password</h1>
                <div class="userField">
                    <div class="pwdRequestBlackText14">Email Address:</div>
                    <b><asp:Label ID="EmailAddressTxt" runat="server" Width="400"/></asp:Label></b>
                </div>
                <div class="CurrentPasswordField">
                    <div class="pwdRequestBlackText14">Password:</div>
                    <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="CurrentPassword" ErrorMessage="Password is required." ID="CurrentPasswordRequired" 
                        runat="server" ToolTip="Password is required." ValidationGroup="ChangePassword1">*
                    </asp:RequiredFieldValidator>
                </div>
                <div class="NewPasswordField">
                    <div class="pwdRequestBlackText14">New Password:</div>
                    <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="NewPassword" ErrorMessage="New Password is required." ID="NewPasswordRequired" 
                        runat="server" ToolTip="New Password is required." ValidationGroup="ChangePassword1">*
                    </asp:RequiredFieldValidator>
                </div>
                <div class="ConfirmNewPasswordField">
                    <div class="pwdRequestBlackText14">Confirm New Password:</div>
                    <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="ConfirmNewPassword" ErrorMessage="Confirm New Password is required." ID="ConfirmNewPasswordRequired" 
                        runat="server" ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*
                    </asp:RequiredFieldValidator>
                </div>
                <div class="loginFailureText">
                    <asp:CompareValidator ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="Dynamic" 
                        ErrorMessage="The confirm New Password must match the New Password entry." ID="NewPasswordCompare" runat="server" ValidationGroup="ChangePassword1">
                    </asp:CompareValidator>
                    <asp:Literal EnableViewState="False" ID="FailureText" runat="server"></asp:Literal>
                </div>
                <div class="submitButton">
                    <asp:ImageButton ID="CancelPushButton" runat="server" CssClass="blueButton" CommandName="Cancel" CausesValidation="False"
                        ImageUrl="~/images/gcancel.png" OnClick="CancelButton_OnClick"  />
                    <asp:ImageButton ID="ChangePasswordPushButton" runat="server" CssClass="blueButton" CommandName="ChangePassword"
                        ImageUrl="~/images/gcommit.png" ValidationGroup="PasswordRecovery1"/>
                </div>
                <div class="loginFooter">
                    <p>
                        <%--<uc:ManagementContactDetails id="managementContactDetailsBox" runat="server" />--%>
                    </p>
                </div>
            </ChangePasswordTemplate>
            <SuccessTemplate>
                <h1>Monex</h1>
                <div class="pwdRequestBlackText14"><b>Change Password Complete!</b></div>
                <div class="pwdRequestBlackText14">The new password will take effect the next time you login to your account.</div>
                <div class="submitButton">
                    <asp:ImageButton ID="ContinuePushButton" runat="server" CssClass="blueButton" CausesValidation="False" CommandName="Submit" 
                        ImageUrl="~/images/btn_ok.png" OnClick="ContinueButton_OnClick"/>
                </div>
                <div class="loginFooter">
                    <p>
                        <%--<uc:ManagementContactDetails id="managementContactDetailsBox" runat="server" />/>--%>
                    </p>
                </div>
            </SuccessTemplate>
        </asp:ChangePassword>
    </div>
</div>