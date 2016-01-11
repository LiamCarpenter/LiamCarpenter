<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAAdminFlightClass.aspx.vb"
    Inherits="NYSInternalAdmin.IAAdminFlightClass" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; 
            z-index: 1001; top: 20px; width: 1010px; height: 652px; display: none;" 
            CssClass="nyspanelwhite" BackColor="Transparent" 
             BackImageUrl="~/images/trans_light.gif">
        </asp:Panel> 
        <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; 
            z-index: 1000; top: 2000px; width: 398px; height: 520px;" 
            CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" 
                 ImageUrl="~/images/logout_out.gif" 
                  Style="z-index: 5000; left: 310px; position: absolute; top: 486px"  
                 TabIndex="3" BorderStyle="None" />
            <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" 
                 PopupControlID="pnNav" TargetControlID="imHover" OffsetY="-12" OffsetX="-11">
            </cc1:HoverMenuExtender> 
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel> 
        <asp:Panel runat="server" ID="pnlMain" Style="left: 4px; position: absolute; z-index: 100;
            top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif" />
                <asp:Panel runat="server" ID="pnlSearch" Visible="true" 
                style="position:absolute;top:76px; left:8px;">
                    <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblGroup" Text="Edit Flight Class: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddClasses" CssClass="nysdropdown">
                    </asp:DropDownList>
                    <asp:LinkButton runat="server" ID="lnkAddNewClass" Text="Add New Flight Class" OnClick="lnkAddNewClass_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
                </asp:Panel>
                
            <asp:Panel runat="server" ID="pnlDetails"  Visible="false"  
                style="position:absolute;top:140px; left:8px;">
                <table>
                    <tr>
                        <td>Flight Class Code: </td>
                        <td><asp:TextBox runat="server" ID="txtFlightClassCode" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Default Class: </td>
                        <td><asp:TextBox runat="server" ID="txtDefaultClass"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>British Airways Class: </td>
                        <td><asp:TextBox runat="server" ID="txtBritishAirwaysClass"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Air France Class: </td>
                        <td><asp:TextBox runat="server" ID="txtAirFranceClass"></asp:TextBox></td>
                    </tr>
                </table>
            </asp:Panel>
            
            <asp:Panel runat="server" ID="pnlButtons"  Visible="false" 
                style="position:absolute;top:260px; left:8px;">
                    <asp:Button runat="server" ID="btnSave" Visible="false" Text="Save" />
                    <asp:Button runat="server" ID="btnAdd" Visible="false" Text="Add" />
                    <asp:Button runat="server" ID="btnCancel" Visible="false" Text="Cancel" />
            </asp:Panel>
            <asp:Label ID="lblBookingDetails" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 8px; position: absolute;
                top: 46px; width: 208px;" Text="Flight Class Admin"></asp:Label>
        </asp:Panel>
    </div>
    
     <asp:Panel runat="server" ID="pnlClassID" Visible="false">
        <asp:HiddenField runat="server" ID="hdnClassID" />
    </asp:Panel>
    </form>
</body>
</html>
