<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="IAAdmin.aspx.vb" Inherits="NYSInternalAdmin.IAAdmin" %>

<%--<%@ Register assembly="skmDataGrid" namespace="skmDataGrid" TagPrefix="cc1" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="skmDataGrid" Namespace="skmDataGrid" TagPrefix="cc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <cc2:hovermenuextender id="HoverMenuExtender1" runat="server" popupcontrolid="pnNav" targetcontrolid="imHover" offsety="-12" offsetx="-11">
        </cc2:hovermenuextender>
        <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; z-index: 1000; top: 2000px; width: 398px; height: 520px;" CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" ImageUrl="~/images/logout_out.gif" Style="z-index: 5000; left: 310px; position: absolute; top: 486px" TabIndex="3" BorderStyle="None" />
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
        <asp:Label ID="lblTitle" runat="server" CssClass="nyslabelHeaderCentre" Style="z-index: 101; left: 4px; position: absolute; top: -2px; width: 1010px;" Text="Client Statements - Admin"></asp:Label>
        <asp:Panel runat="server" ID="pnlMain" Style="left: 4px; position: absolute; z-index: 100; top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels" Font-Size="Small">
            <asp:Panel ID="pnlMessage" runat="server" Style="z-index: 101; left: 804px; position: absolute; top: 387px; width: 314px; height: 164px;" CssClass="evopanels" Visible="false" BorderStyle="Solid" BackColor="#E8E6DA">
                <asp:Label ID="lblMessage" runat="server" Text="" Style="z-index: 100; position: absolute; top: 42px; left: 67px; width: 205px; height: 15px;" Visible="false"></asp:Label>
                <asp:Button ID="btnOk" runat="server" Text="OK" Style="z-index: 100; position: absolute; top: 96px; left: 78px; height: 31px; width: 79px; right: 157px;" Visible="false" Font-Size="Small" Enabled="False" />
                <asp:Button ID="btnClose" runat="server" Text="Close" Style="z-index: 100; position: absolute; top: 96px; left: 193px; height: 31px; width: 79px; right: 42px;" Visible="false" Font-Size="Small" Enabled="False" />
            </asp:Panel>
            <asp:Panel ID="pnlEdit" runat="server" Style="z-index: 100; position: absolute; top: 60px; left: 45px; width: 788px; height: 539px;" CssClass="evopanels" BackColor="White" BorderStyle="Solid" BorderWidth="4px">
                
                <asp:Label ID="lblBossRef" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 43px; width: 127px; height: 12px;" Text="Select Client:" Font-Size="Small"></asp:Label>
                
                <asp:Label ID="lblHiddenBossID" runat="server" Text="0" Visible="True" ></asp:Label>
                <asp:DropDownList ID="ddBossRef" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 39px; height: 25px; width: 250px;" AutoPostBack="true"></asp:DropDownList>

                <asp:TextBox ID="txtBossRef" runat="server" Style="z-index: 250; left: 230px; position: absolute; top: 75px; width: 250px;" AutoPostBack="True" Enabled="False" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtEmailAddress" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 111px; width: 250px;" Enabled="False"></asp:TextBox>
                <asp:Label ID="lblreference" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 149px; width: 127px; height: 15px;" Text="Reference 1:" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label1" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 180px; width: 127px; height: 15px;" Text="Reference 2:" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label2" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 216px; width: 127px; height: 15px;" Text="Reference 3:" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label3" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 250px; width: 127px; height: 15px;" Text="Reference 4:" Font-Size="Small"></asp:Label>
                
                <asp:DropDownList ID="ddReference" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 147px; width: 150px;" Enabled="False"></asp:DropDownList>
                <asp:DropDownList ID="ddReference2" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 177px; width: 150px;" Enabled="False"></asp:DropDownList>
                <asp:DropDownList ID="ddReference3" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 214px; width: 150px;" Enabled="False"></asp:DropDownList>
                <asp:DropDownList ID="ddReference4" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 250px; width: 150px;" Enabled="False"></asp:DropDownList>

                <asp:TextBox ID="txtReference1" runat="server" Style="z-index: 200; left: 510px; position: absolute; top: 143px; width: 131px;"></asp:TextBox>
                <asp:TextBox ID="txtReference2" runat="server" Style="z-index: 200; left: 510px; position: absolute; top: 180px; width: 133px;"></asp:TextBox>
                <asp:TextBox ID="txtReference3" runat="server" Style="z-index: 200; left: 510px; position: absolute; top: 215px; width: 134px;"></asp:TextBox>
                <asp:TextBox ID="txtReference4" runat="server" Style="z-index: 200; left: 510px; position: absolute; top: 251px; width: 135px;"></asp:TextBox>
                
                <asp:CheckBox ID="chkSendtoEmailAddress" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 447px;" Font-Size="Small" Enabled="False" />
                <%--<asp:CheckBox ID="chkonConinvoices" runat="server" Style="z-index: 200; left: 230px; position: absolute; top: 484px;" Font-Size="Small" Enabled="False" />
                --%><asp:Label ID="lblBossRefNew" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 75px; width: 158px; height: 12px;" Text="Boss Code:" Font-Size="Small" Visible="False"></asp:Label>
                <asp:Label ID="lblemail" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 114px; width: 127px; height: 12px;" Font-Size="Small" Text="Email:"></asp:Label>
                <asp:Label ID="Label4" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 410px; position: absolute; top: 149px; width: 127px; height: 15px;" Text="Display as:" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label5" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 410px; position: absolute; top: 180px; width: 127px; height: 15px;" Text="Display as:" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label6" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 410px; position: absolute; top: 215px; width: 127px; height: 15px;" Text="Display as:" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label7" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 408px; position: absolute; top: 250px; width: 127px; height: 15px;" Text="Display as:" Font-Size="Small"></asp:Label>

                <asp:Label ID="lblsend" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 448px; width: 155px; height: 12px;" Font-Size="Small" Text="Automatic:"></asp:Label>
                <%--<asp:Label ID="Label8" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 60px; position: absolute; top: 483px; width: 171px; height: 12px;" Font-Size="Small" Text="On consolidated Invoicing?"></asp:Label>
                --%><asp:Button ID="btnChange" runat="server" Text="Update Client" Style="z-index: 101; left: 508px; position: absolute; top: 37px; width: 115px; height: 25px;" Font-Size="Small" />
                <asp:Button ID="btnAdd" runat="server" Text="Add New Client" Style="z-index: 101; left: 647px; position: absolute; top: 37px; width: 115px; height: 25px;" Font-Size="Small" />
                <asp:Button ID="btnSave" runat="server" Text="Save" Style="z-index: 101; left: 641px; position: absolute; top: 481px; width: 115px; height: 25px;"  Font-Size="Small" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Style="z-index: 101; left: 501px; position: absolute; top: 481px; width: 115px; height: 25px;" Font-Size="Small" />
            </asp:Panel>
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute; top: 10px; right: 921px;" ImageUrl="~/images/menu.gif" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
