<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checkEmail.aspx.cs" Inherits="Cff.SaferTrader.Web.checkEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 256px;
        }
        .style2
        {
            height: 28px;
        }
        .style4
        {
            height: 38px;
        }
    </style>
</head>
<body style="width:764px;">
    <dl><dt style="background-color:Green;width: 764px; color:White"><b>Check Email</b></dt></dl>
    <form id="form1" runat="server" width ="764px" >
    <div style="width: 764px; height: 463px;">
    <asp:panel runat="server" ID="EmailForm" Height="467px" Width="764px" style="background-color:#EEFCE4" UpdateMode="always">  
       <table border="0" style="width: 737px; height: 403px;">
            <tr>
                <td> To: </td>
                <td>
                    <asp:TextBox ID="txtEmailTo" runat="server" Width="421px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td> Cc: </td>
                <td>
                    <asp:TextBox ID="txtEmailCC" runat="server" Width="419px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td> Bcc: </td>
                <td><asp:Literal ID="EmailBCCLiteral" runat="server"></asp:Literal> </td>
            </tr>
            
            <tr>
                <td>Attachment:</td>
                <td><asp:Literal ID="EmailAttachment" runat="server"></asp:Literal></td>
             </tr>
             <tr>
                <td class="style2">Additional Attachment: </td>
                <td class="style2">
                    <asp:FileUpload ID="AttachmentFile" runat="server" Width="217px" />
                    <asp:Label ID="AttachmentDetail" runat="server" Width="217px" />
                </td>
            </tr>
            <tr>
                <td>Subject:</td>
                <td>
                    <asp:TextBox ID="txtEmailSubject" runat="server" Width="419px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style1">
                    Your Email Body:<br/>
                    <asp:TextBox runat="server" ID="txtBody" TextMode="MultiLine" Columns="55" 
                        Rows="10" Width="759px" Height="222px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4"><asp:Literal ID="sendStatusLiteral" runat="server"></asp:Literal>
                    <asp:HyperLink ID="sendEmailHyperLink" runat="server"></asp:HyperLink>
                </td>
                <td align="right" class="style4">
                    <asp:Button runat="server" autopostback="true" ID="SendEmail" Text="Send Email" 
                        onclick="SendEmail_Click" BackColor="#009933" ForeColor="White" />
                </td>
            </tr>
            
        </table>
        <asp:HiddenField ID="hCode" runat="server"/>
    </asp:Panel>
    </div>
    </form>
</body>
</html>
