<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IALogonAdmin.aspx.vb" Inherits="NYSInternalAdmin.IALogonAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/SSOHeader.gif"
            Style="left: 8px; position: absolute; top: 8px" Width="974px" />
        <asp:Panel ID="pnmain" runat="server" CssClass="nyspanels" 
            Style="left: 8px; position: absolute; top: 55px; height: 598px;" 
            Width="972px">
            <asp:Label ID="lbluser" runat="server" CssClass="nysitaliclabel" 
                Style="z-index: 113; left: 6px; position: absolute; top: 578px; width: 784px;"></asp:Label>
            <asp:Panel ID="pnlogin" runat="server" 
                BackImageUrl="~/images/IALogonPanel2.gif" 
                
                Style="z-index: 100; left: 242px; position: absolute; top: 143px; height: 273px; width: 502px;">
            <asp:Label ID="Label14" runat="server" 
                    CssClass="nyslabelHeader" Style="z-index: 103; left: 244px;
                position: absolute; top: 25px; width: 226px;" Text="NYS Internal Admin"></asp:Label>
                <asp:DropDownList ID="ddgroups" runat="server" CssClass="nysdropdown" 
                    Style="z-index: 102; left: 232px; position: absolute; top: 129px; width: 238px;" 
                    tabindex="1">
                </asp:DropDownList>
                <asp:Label ID="Label4" runat="server" CssClass="nyslabellogonlg" 
                    Font-Bold="False" Style="z-index: 103; left: 233px;
                position: absolute; top: 107px; width: 153px;" Text="Please select a client:"></asp:Label>
                <asp:ImageButton ID="btnlogin" runat="server" CssClass="btnclasshand" 
                    ImageUrl="~/images/continue_out.gif" 
                    Style="z-index: 109; left: 392px; position: absolute; top: 224px" 
                    TabIndex="2" />
                <asp:Label ID="lblversion" runat="server" CssClass="nysitaliclabel" Style="z-index: 115;
                left: 12px; position: absolute; top: 247px; width: 131px;"></asp:Label>
            </asp:Panel>
            <asp:Label ID="lblNoAccess" runat="server" CssClass="nysitaliclabel" 
                ForeColor="Red" Style="z-index: 115;
                left: 334px; position: absolute; top: 247px; width: 320px;">Access not validated, please contact system support</asp:Label>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
