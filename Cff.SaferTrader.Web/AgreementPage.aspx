﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgreementPage.aspx.cs" 
Inherits="Cff.SaferTrader.Web.AgreementPage" Title="Cashflow Funding Limited | Debtor Management | Dashboard" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>Cashflow Funding Limited | Debtor Management</title>
    <link rel="stylesheet" href="~/css/style.css" type="text/css" />
    <% 
        if (Request.ServerVariables["HTTPS"] == "off" || Request.ServerVariables["HTTPS"] == "")
       {%>
            <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
       <% } else { %>
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <% } %>    
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <center><br />
        <div>
            <table border="0" width="60%" style="background-color: White; padding: 10px;">                <tr>                <td width="100%">&nbsp;                <div id="utilities">
                    <h1>Cashflow Funding Limited</h1>
                </div>                <h3 style="text-align:center">CLIENT ON LINE&nbsp; INFORMATION SERVICE AGREEMENT</h3>
                               <p style="text-align:left;"><font color="Green"><strong>If you are not fully authorised to view this                information then you must sign off immediately. </strong></font></p>                <p style="text-align:justify;">                    <big><font color="#000000">This Agreement is between you and Cashflow Funding Limited (<b>CFL</b>).                 Please read this agreement carefully. By accepting and accessing the  on-line information service in                 respect of your account and related information (<b>Service</b>)  offered to you by CFL, you are                 acknowledging that you have read and understood this Agreement, and that you agree to be bound by the                 following terms and conditions:</font></big></p>                                 <p style="text-align:justify;"><big><font color="#000000">1. Your and CFL&#146;s rights and obligations                to each other continue to be governed by the terms of the debt factoring facility                agreement between you and CFL, and any related security documents or other related                documents. Nothing arising out of your accessing or receiving this Service and nothing                contained in this Agreement will alter, reduce or otherwise affect such rights and                obligations.</font></big></p>                <p style="text-align:left;"><big><font color="#000000">2. CFL reserves the right to alter, update,                upgrade or terminate the Service at any time.</font></big></p>                                <p style="text-align:left;"><big><font color="#000000">3. CFL does not guarantee the availability of                the web-site or its content and CFL reserves the right to terminate immediately access at                any time without prior notice to you.</font></big></p>                               <p style="text-align:justify;"><big><font color="#000000">4. The Service is provided to you free of                charge as a customer service on an <i>all care, no responsibility</i> basis. </font></big></p>                               <p style="text-align:justify;"><big><font color="#000000">5. Although all reasonable care may be taken                by CFL in the preparation and provision of the Service to you, and storage of information                relating to the Service, CFL does not warrant or guarantee:</font></big></p>                                    <blockquote>                  <p style="text-align:justify;"><big><font color="#000000">5.1 The accuracy, adequacy, quality,                  currentness, validity, completeness or secure storage, of any information included in the                  Service;</font></big></p>                </blockquote>                <blockquote>                  <p style="text-align:justify;"><big><font color="#000000">5.2 Its suitability for any particular                  purpose; </font></big></p>                </blockquote>                <blockquote>                  <p style="text-align:justify;"><big><font color="#000000">5.3 The availability of the Service at any                  particular time or times; or</font></big></p>                  <p style="text-align:justify;"><big><font color="#000000">5.4 That the Service will be free of                  computer viruses or similar contaminations or destructive on-line properties.</font></big></p>                </blockquote>               <p style="text-align:justify;"><big><font color="#000000">6. CFL will not, under any circumstances,                whether in contract, tort or otherwise in law, be liable to you or any third party for any                loss, damages, costs or delays arising out of or resulting from:</font></big></p>                <blockquote>                 <p style="text-align:left;"><big><font color="#000000">6.1 Any inaccuracy, inadequacy, defect in                  quality, lack of currentness, invalidity or incompleteness of any information provided to                  you as part of the Service;</font></big></p>                   <p style="text-align:left;"><big><font color="#000000">6.2 (Subject to the requirements of the Privacy                  Act 1993 only), any disclosure of any information to any unauthorised party and whether                  due to inadvertence or due to a third party breaching any security or access measures CFL                  may institute in relation to the Service from time to time, or otherwise;</font></big></p>                   <p style="text-align:left;"><big><font color="#000000">6.3 Delays in access to, or unavailability of                  the Service;</font></big></p>                  <p style="text-align:left;"><big><font color="#000000">6.4 The existence of any computer viruses or                  similar contamination or destructive on-line properties.</font></big></p>                </blockquote>                <p style="text-align:justify;"><big><font color="#000000">7. The exclusion of liability contained in the                preceding clauses shall extend to and include any and all losses, damages or costs of                whatsoever nature, and whether direct or indirect, physical (including injury to persons)                or financial (including loss of profits and business interruption), loss of programmes or                other information, and the like.</font></big></p>                 <p style="text-align:left;"><big><font color="#000000">8. You are responsible for checking and                verifying the accuracy of all information included in the Service.</font></big></p>                 <p style="text-align:left;"><big><font color="#000000">9. You agree to indemnify CFL against all                claims, demands, losses, damages or costs which CFL may face, suffer or incur from any                third party as a result of CFL providing the Service to you.</font></big></p>                 <p style="text-align:justify;"><big><font color="#000000">10. This Agreement shall be governed by the                laws of New Zealand. You and CFL agree to submit to the exclusive jurisdiction of the                courts of New Zealand in relation to all matters concerning the implementation of this                Agreement, including its interpretation.</font></big></p>                                     <p style="text-align:left;"></p>                 <p style="text-align:left;"><b>IF YOU DO NOT ACCEPT AND AGREE TO THE TERMS OF THIS AGREEMENT,                YOU SHOULD <a href="LogOn.aspx">SIGN OFF NOW</a>. </b></p>                <div id="AcceptanceField" runat="server">                    <center>                        <table border="0"><tr>                            <td><p><big><font color="#000000"><input type="checkbox" id="Acceptance" style="margin: -3px 5px 0 0;" runat="server"/> I Accept and Agree to the Terms and Conditions of this agreement.</font></big></p></td>                            <td style="vertical-align:middle; margin-left: 10px; width: 150px; text-align: right;">                                <asp:ImageButton ID="ContinueButton" runat="server" ImageUrl="~/images/btn_gcontinue.png" OnClick="OnContinueClick" runat="server"></asp:ImageButton>                            </td>                            </tr>                        </table>                    </center>                </div>                </td>              </tr>            </table>        </div>
    </center>
    </form>
</body>
</html>

