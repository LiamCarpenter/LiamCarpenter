<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAAdminCCP.aspx.vb" Inherits="NYSInternalAdmin.IAAdminCCP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NYS Corporate - Management Information</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
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
        <asp:Label ID="Label1" runat="server" CssClass="nyslabelHeaderCentre" Style="z-index: 101;
         position: absolute; top: -2px; width: 1013px; left: 7px;" 
            Text="Credit Card Payment - Admin" Font-Size="Larger"></asp:Label>
        <asp:Panel runat="server" ID="pnlMain" Style="left: 4px; position: absolute; z-index: 100;
            top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif" />
            <asp:Panel ID="pnEdit" runat="server" BackColor="#E8E6DA" BorderStyle="Solid" Style="z-index: 100;
                position: absolute; top: 5px; left: 154px; height: 517px; width: 686px; margin-top: 4px;"
                Font-Size="Medium">
                <asp:TextBox ID="txtname" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 49px; left: 215px; width: 408px;"></asp:TextBox>
                <asp:TextBox ID="txtnysref" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 19px; left: 215px; width: 270px;"></asp:TextBox>
                <asp:TextBox ID="txtcardnumber1" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 79px; left: 215px; width: 200px;" TabIndex="20" 
                    MaxLength="8"></asp:TextBox>
                <asp:TextBox ID="txtcardnumber2" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 79px; left: 431px; width: 200px;" MaxLength="8"></asp:TextBox>
                <asp:TextBox ID="txtcvv" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 107px; left: 215px; width: 50px;" TabIndex="3" MaxLength="3"></asp:TextBox>
                <asp:DropDownList ID="ddexdateMM" runat="server" Style="z-index: 200; position: absolute;
                    top: 107px; left: 445px; height: 40px; width: 50px;" TabIndex="4" Height="40px"
                    Width="60px">
                    <asp:ListItem>--</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddexdateYYYY" runat="server" Style="z-index: 200; position: absolute;
                    top: 107px; left: 525px; height: 24px; width: 57px;" TabIndex="4" Width="60px"
                    Height="40px">
                    <asp:ListItem>--</asp:ListItem>
                    <asp:ListItem>2012</asp:ListItem>
                    <asp:ListItem>2013</asp:ListItem>
                    <asp:ListItem>2014</asp:ListItem>
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>
                    <asp:ListItem>2017</asp:ListItem>
                    <asp:ListItem>2018</asp:ListItem>
                    <asp:ListItem>2019</asp:ListItem>
                    <asp:ListItem>2020</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtemail" runat="server" CssClass="evotextbox" Style="z-index: 50;
                    position: absolute; top: 142px; left: 215px; width: 408px;" TabIndex="5"></asp:TextBox>
                <asp:TextBox ID="txtpostalline1" runat="server" CssClass="evotextbox" Style="z-index: 50;
                    position: absolute; top: 172px; left: 215px; width: 408px; height: 22px;" TabIndex="6"></asp:TextBox>
                <asp:TextBox ID="txtpostalline2" runat="server" CssClass="evotextbox" Style="z-index: 50;
                    position: absolute; top: 201px; left: 215px; width: 408px; height: 22px;" TabIndex="6"></asp:TextBox>
                <asp:TextBox ID="txtpostalcity" runat="server" CssClass="evotextbox" Style="z-index: 50;
                    position: absolute; top: 230px; left: 215px; width: 408px; height: 22px;" TabIndex="6"></asp:TextBox>
                <asp:TextBox ID="txtpostalpostcode" runat="server" CssClass="evotextbox" Style="z-index: 50;
                    position: absolute; top: 260px; left: 215px; width: 135px; height: 22px;" TabIndex="6"
                    MaxLength="8"></asp:TextBox>
                <asp:TextBox ID="txtaddressline1" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 292px; left: 215px; width: 408px; height: 22px;" TabIndex="7"></asp:TextBox>
                <asp:TextBox ID="txtaddressLine2" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 322px; left: 215px; width: 408px; height: 22px;" TabIndex="7"></asp:TextBox>
                <asp:TextBox ID="txtaddresscity" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 352px; left: 215px; width: 408px; height: 22px;" TabIndex="7"></asp:TextBox>
                <asp:Label ID="lblnysref" runat="server" Text="Enquiry NYS reference:" CssClass="evolabel"
                    Style="z-index: 119; position: absolute; top: 19px; left: 31px; width: 153px;"></asp:Label>
                <asp:Label ID="lblname" runat="server" Text="Card Holder Name:" CssClass="evolabel"
                    Style="z-index: 119; position: absolute; top: 49px; left: 31px; width: 153px;"></asp:Label>
                <asp:Label ID="lblcardnumber" runat="server" Text="Card Number:" CssClass="evolabel"
                    Style="z-index: 119; position: absolute; top: 78px; left: 31px; width: 153px;"></asp:Label>
                <asp:Label ID="lblcvv" runat="server" Text="CVV:" CssClass="evolabel" Style="z-index: 119;
                    position: absolute; top: 107px; left: 31px; width: 153px;"></asp:Label>
                <asp:Label ID="lblprocessed" runat="server" Text="Processed" 
                    CssClass="evolabel" Style="z-index: 119;
                    position: absolute; top: 430px; left: 31px; width: 153px;"></asp:Label>
                <asp:Label ID="lbldateprocessed" runat="server" Text="Date Processed" 
                    CssClass="evolabel" 
                    Style="z-index: 119; position: absolute; top: 461px; left: 31px; width: 153px;"></asp:Label>
                <asp:Label ID="lblexpirydate" runat="server" Text="Expiry Date:" CssClass="evolabel"
                    Style="z-index: 119; position: absolute; top: 107px; left: 359px; width: 82px;"></asp:Label>
                <asp:Label ID="lblemail" runat="server" Text="Email address for receipt:" CssClass="evolabel"
                    Style="z-index: 119; position: absolute; top: 142px; left: 31px; width: 153px;"></asp:Label>
                <asp:Label ID="lbladdress" runat="server" Text="Registered address of the card (optional):"
                    CssClass="evolabel" Style="z-index: 119; position: absolute; top: 287px; left: 25px;
                    width: 187px;"></asp:Label>
                <asp:Label ID="lblpostaladdress" runat="server" Text="Postal address for receipt:"
                    CssClass="evolabel" Style="z-index: 119; position: absolute; top: 166px; left: 31px;
                    width: 161px;"></asp:Label>
                <asp:Button ID="btnsave" runat="server" CssClass="evostandardbutton" Style="z-index: 120;
                    position: absolute; top: 418px; left: 519px; width: 114px;" Text="Save" TabIndex="10"
                    Visible="False" />
                <asp:Button ID="btnClearboxes" runat="server" CssClass="evostandardbutton" Style="z-index: 120;
                    position: absolute; top: 466px; left: 519px; width: 114px;" Text="Clear Boxes"
                    TabIndex="10" Visible="False" />
                <asp:Button ID="btnpopulate" runat="server" CssClass="evostandardbutton" Style="z-index: 120;
                    position: absolute; top: 17px; left: 515px; width: 114px;" Text="Populate Details"
                    TabIndex="10" Visible="False" />
                <asp:TextBox ID="txtaddresspostcode" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 382px; left: 215px; width: 120px; height: 22px;" TabIndex="7"
                    MaxLength="8"></asp:TextBox>
                <asp:TextBox ID="txtprocessedDate" runat="server" CssClass="evotextbox" Style="z-index: 102;
                    position: absolute; top: 462px; left: 215px; width: 120px; height: 22px;" TabIndex="7"
                    MaxLength="8"></asp:TextBox>
                <asp:Label ID="lbladresspc" runat="server" Text="Post Code" CssClass="nyslabel" Style="z-index: 119;
                    position: absolute; top: 264px; left: 148px; width: 81px;" ForeColor="Gray" Font-Size="Small"></asp:Label>
                <asp:Label ID="lbladdresstown" runat="server" Text="City/Town" CssClass="nyslabel"
                    Style="z-index: 119; position: absolute; top: 234px; left: 147px; width: 58px;"
                    ForeColor="Gray" Font-Size="Small"></asp:Label>
                <asp:Label ID="lblpostalpc" runat="server" Text="PostCode" CssClass="nyslabel" Style="z-index: 119;
                    position: absolute; top: 387px; left: 144px; width: 32px;" ForeColor="Gray" Font-Size="Small"></asp:Label>
                <asp:Label ID="lblpostaltown" runat="server" Text="City/Town" CssClass="nyslabel"
                    Style="z-index: 119; position: absolute; top: 355px; left: 145px; width: 32px;"
                    ForeColor="Gray" Font-Size="Small"></asp:Label>
                <asp:CheckBox ID="chkProcessed" runat="server" 
                    style="z-index:100; position:absolute; top: 431px; left: 212px;"/>
             <cc1:CalendarExtender ID="cexdate" runat="server" TargetControlID="txtprocessedDate">
            </cc1:CalendarExtender>
            </asp:Panel>
            <asp:Panel ID="pnDelete" runat="server" BackColor="#E8E6DA" Style="z-index: 40;
                position: absolute; top: 538px; left: 153px; height: 60px; width: 686px;" 
                BorderStyle="Solid">
                <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="evostandardbutton"
                    Style="z-index: 120; position: absolute; top: 16px; left: 524px; width: 114px;"
                    Enabled="False" Visible="False" />
                <asp:Label ID="lbldelete" runat="server" Text="No records to delete!" CssClass="evolabel"
                    Style="z-index: 119; position: absolute; top: 12px; left: 12px; width: 389px;
                    height: 38px; right: 285px;"></asp:Label>
            </asp:Panel>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
