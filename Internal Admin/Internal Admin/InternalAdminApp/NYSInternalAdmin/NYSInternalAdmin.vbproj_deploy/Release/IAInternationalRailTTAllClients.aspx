<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAInternationalRailTTAllClients.aspx.vb"
    Inherits="NYSInternalAdmin.IAInternationalRailTTAllClients" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $find("cexFrom").add_showing(changeZIndex);
            $find("cexTo").add_showing(changeZIndex1);
        }
        function changeZIndex() {
            $find("cexFrom")._popupDiv.style.zIndex = 100000000;
        }
        function changeZIndex1() {
            $find("cexTo")._popupDiv.style.zIndex = 100000000;
        } 
        
    </script>
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
        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; z-index: 1001;
            top: 20px; width: 1010px; height: 652px; display: none;" CssClass="nyspanelwhite"
            BackColor="Transparent" BackImageUrl="~/images/trans_light.gif">
        </asp:Panel>
        <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; z-index: 1000;
            top: 2000px; width: 398px; height: 520px;" CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" ImageUrl="~/images/logout_out.gif"
                Style="z-index: 5000; left: 310px; position: absolute; top: 486px" TabIndex="3"
                BorderStyle="None" />
            <cc1:hovermenuextender id="HoverMenuExtender1" runat="server" popupcontrolid="pnNav"
                targetcontrolid="imHover" offsety="-12" offsetx="-11">
             </cc1:hovermenuextender>
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
        <asp:Label ID="lblBookingDetails" runat="server" CssClass="nyslabelHeaderCentre"
            Style="z-index: 101; left: 4px; position: absolute; top: -2px; width: 1010px;"
            Text="Traveller Tracking - International Rail - All Clients"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" Style="left: 4px; position: absolute; z-index: 100;
            top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">
            <asp:Panel ID="pndates" runat="server" CssClass="nyspanelsSM" Style="left: 114px;
                position: absolute; top: 4px; width: 782px; height: 34px; z-index: 10000;">
                <asp:Label ID="lblto" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 393px;
                    position: absolute; top: 10px; width: 18px; right: 371px;" Text="to:"></asp:Label>
                <asp:Label ID="lblfrom" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 188px;
                    position: absolute; top: 10px; width: 111px; right: 483px;" Text="Enter dates from:"></asp:Label>
                <asp:TextBox ID="txtfrom" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 302px; position: absolute; top: 8px; width: 77px;" TabIndex="1"></asp:TextBox>
                <asp:TextBox ID="txtto" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 414px; position: absolute; top: 8px; width: 77px;" TabIndex="1"></asp:TextBox>
                <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" ImageUrl="~/images/refresh_out.gif"
                    Style="z-index: 109; left: 512px; position: absolute; top: 6px" TabIndex="3"
                    BorderStyle="None" />
            </asp:Panel>
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif" />
            <rsweb:reportviewer id="rvRailChannelTT" runat="server" backcolor="White" cssclass="reportView"
                documentmapwidth="100%" font-names="Verdana" font-size="8pt" internalborderstyle="None"
                internalborderwidth="2px" showcredentialprompts="False" showdocumentmapbutton="False"
                showfindcontrols="True" showpagenavigationcontrols="False" showparameterprompts="False"
                showpromptareabutton="False" showzoomcontrol="False" style="left: 4px; position: absolute;
                z-index: 100; top: 44px;" toolbaritemhoverbackcolor="Silver" linkdisabledcolor="LightGray"
                toolbaritemborderstyle="None" linkactivecolor="Blue" linkactivehovercolor="DeepPink"
                height="600px" showbackbutton="False" width="995px">
           
            <LocalReport ReportPath="Reports\repEurostarTTAllClients.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsEurostarTTAllClients_EurostarTTAllClients" />
                </DataSources>
            </LocalReport>
           
        </rsweb:reportviewer>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetData" TypeName="NYSInternalAdmin.dsEurostarTTAllClientsTableAdapters.EurostarTTAllClientsTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtfrom" Name="startdate" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtto" Name="enddate" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <cc1:calendarextender id="cexTo" runat="server" targetcontrolid="txtto">
        </cc1:calendarextender>
            <cc1:calendarextender id="cexFrom" runat="server" targetcontrolid="txtfrom">
        </cc1:calendarextender>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
