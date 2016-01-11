<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAPoCheckJLR.aspx.vb" Inherits="NYSInternalAdmin.IAPoCheckJLR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">

    <asp:scriptmanager ID="ScriptManager1" runat="server">
    </asp:scriptmanager> 

    <div>
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
                 PopupControlID="pnNav" TargetControlID="imHover" OffsetY="-12" 
                OffsetX="-11" PopDelay="1">
            </cc1:HoverMenuExtender> 
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel> 
        <asp:Label ID="lblBookingDetails" runat="server" 
             CssClass="nyslabelHeaderCentre" Style="z-index: 101; left: 4px; position: absolute;
                top: -2px; width: 1010px;" Text="JLR PO Check"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >
            <asp:TextBox ID="txtresult" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 4px; position: absolute;
                top: 51px; width: 998px; height: 561px;" TabIndex="5" 
                TextMode="MultiLine"></asp:TextBox>
        <asp:Image ID="imHover" runat="server" 
            Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
        <asp:ImageButton ID="btnrefresh" runat="server" BorderStyle="None" 
                    CssClass="btnclasshand" ImageUrl="~/images/refresh_out.gif" 
                    Style="z-index: 109; left: 373px; position: absolute; top: 6px" TabIndex="3" />
            <asp:Label ID="Label1" runat="server" 
                style="position:absolute; top:3px; left:525px;" CssClass="nyslabel" >Please note:<br />**this will only work on PO's that have been used in MEVIS<br />**Values are all NETT</asp:Label>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
