<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IACommissionStatement.aspx.vb"
    Inherits="NYSInternalAdmin.IACommissionStatement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
    <script language="JavaScript" type="text/javascript">
        function toggleDiv(element) {
            if (document.getElementById(element).style.display === 'none') {
                document.getElementById(element).style.display = 'block';
                document.getElementById(element).innerHTML = "<img style='position: absolute; top: 301px; left: 0px;' src='images/white.gif'><img style='position: absolute; top: 310px; left: 425px;' src='images/ajax-loader.gif'><p style='position:absolute; top: 330px; left:370px; font-family: Verdana; font-size: small; font-weight: bold; font-style: normal; color: #515151; background-color: #FFFFFF;'>Please wait while the files are built......</p>";
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
        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; z-index: 1001;
            top: 20px; width: 1010px; height: 652px; display: none;" CssClass="nyspanelwhite"
            BackColor="Transparent" BackImageUrl="~/images/trans_light.gif">
        </asp:Panel>
        <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; z-index: 1000;
            top: 2000px; width: 398px; height: 520px;" CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" ImageUrl="~/images/logout_out.gif"
                Style="z-index: 5000; left: 310px; position: absolute; top: 486px" TabIndex="3"
                BorderStyle="None" />
            <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="pnNav"
                TargetControlID="imHover" OffsetY="-12" OffsetX="-11">
            </cc1:HoverMenuExtender>
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
        <asp:Label ID="lblBookingDetails" runat="server" CssClass="nyslabelHeaderCentre"
            Style="z-index: 101; left: 4px; position: absolute; top: -2px; width: 1010px;"
            Text="Commission Statements"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" Style="left: 4px; position: absolute; z-index: 100;
            top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">
            <asp:Panel ID="pndates" runat="server" CssClass="nyspanels" Style="left: 101px; position: absolute;
                top: 4px; width: 828px; height: 37px; z-index: 10000;">
                <asp:Label ID="lblinvoice" runat="server" CssClass="nyslabel" Style="z-index: 103;
                    left: 13px; position: absolute; top: 8px; width: 95px; right: 720px; height: 16px;"
                    Text="Venues:"></asp:Label>
                <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" ImageUrl="~/images/run_over.gif"
                    Style="z-index: 109; left: 733px; position: absolute; top: 5px; height: 24px;"
                    TabIndex="4" BorderStyle="None" />
                <asp:DropDownList ID="ddVenues" runat="server" Style="z-index: 99; left: 70px; position: absolute;
                    top: 8px; width: 654px;" CssClass="nysdropdown">
                </asp:DropDownList>
            </asp:Panel>
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif" />
            <asp:TextBox ID="txtResult2" runat="server" CssClass="nystextbox" Style="z-index: 99;
                left: 51px; position: absolute; top: 351px; width: 891px; height: 283px;" TabIndex="1"
                TextMode="MultiLine"></asp:TextBox>
            <asp:HyperLink ID="hlStatement" runat="server" CssClass="nyshyperlink" Style="z-index: 113;
                left: 101px; position: absolute; top: 73px" TabIndex="9" Target="_blank" Visible="False">View statement</asp:HyperLink>
            <asp:Label ID="lblEmail" Style="z-index: 113; left: 103px; position: absolute; top: 51px"
                runat="server" Text=""></asp:Label>
            <asp:Button ID="btnSendEmail" Style="position: absolute; top: 107px; left: 98px; width:110px;"
                runat="server" Text="Send Email" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
