<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IATestEvolvi.aspx.vb" Inherits="NYSInternalAdmin.IATestEvolvi" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
    <script language="JavaScript" type="text/javascript">
        function toggleDiv(element) {
            if (document.getElementById(element).style.display === 'none') {
                document.getElementById(element).style.display = 'block';
                document.getElementById(element).innerHTML = "<img style='position: absolute; top: 301px; left: 0px;' src='images/white.gif'><img style='position: absolute; top: 310px; left: 425px;' src='images/ajax-loader.gif'><p style='position:absolute; top: 330px; left:370px; font-family: Verdana; font-size: small; font-weight: bold; font-style: normal; color: #515151; background-color: #FFFFFF;'>Please wait while the report is compiled......</p>";
            }
            else if (document.getElementById(element).style.display === 'block') {
                document.getElementById(element).style.display = 'none';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; z-index: 1001; top: 20px; width: 1010px; height: 652px; display: none;" CssClass="nyspanelwhite"
            BackColor="Transparent" BackImageUrl="~/images/trans_light.gif">
        </asp:Panel>
        <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; z-index: 1000; top: 2000px; width: 398px; height: 520px;" CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" ImageUrl="~/images/logout_out.gif" Style="z-index: 5000; left: 310px; position: absolute;
                top: 486px" TabIndex="3" BorderStyle="None" />
            <cc1:hovermenuextender id="HoverMenuExtender1" runat="server" popupcontrolid="pnNav" targetcontrolid="imHover" offsety="-12" offsetx="-11">
            </cc1:hovermenuextender>
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlMain" Style="left: 4px; position: absolute; z-index: 100; top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute; top: 10px;" ImageUrl="~/images/menu.gif" />
            <asp:Panel runat="server" ID="pnlInputs" Style="position: absolute; top: 76px; left: 8px;" CssClass="nyspanels">
                <asp:Literal runat="server" ID="ltrOutputString"></asp:Literal>
                <table>
                    <tr>
                        <td>
                            Location of XML Files for Guardian Input:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtGuardianLocation"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Location of XML Files for Guardian Output:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtGuardianOutLocation"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Location of XML Files for Interceptor:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFolderLocation"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Folder to send BOSS Output:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtBossOutputFolder"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Folder to send SQL Output:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSqlOutputFolder"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btnSubmit" Text="Process XML Files" />
                        </td>
                    </tr>
                </table>
              <%--  <table>
                  <%--  <tr>
                        <td>
                            Put employee number
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtinput"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td>
                            Display employee number
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtoutput"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="dowork" Text="Get employee number" />
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                            <asp:Button runat="server" ID="btnAutoInvoice" Text="Process Auto Invoices" />
                    </td>
                    </tr>--%>
                </table>--%>
            </asp:Panel>
            <asp:Label ID="lblBookingDetails" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 8px; position: absolute; top: 46px; width: 123px;"
                Text="Evolvi Test"></asp:Label>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
