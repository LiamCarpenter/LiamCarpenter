<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAAirTTAllClients.aspx.vb"
    Inherits="NYSInternalAdmin.IAAirTTAllClients" %>

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
            <Services>
                <asp:ServiceReference Path="AjaxService.asmx" />
            </Services>
        </asp:ScriptManager>
        <cc1:calendarextender id="cexTo" runat="server" targetcontrolid="txtto"></cc1:calendarextender>
        <cc1:calendarextender id="cexFrom" runat="server" targetcontrolid="txtfrom"></cc1:calendarextender>
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
            Text="Traveller Tracking - Air"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" Style="left: 4px; position: absolute; z-index: 100;
            top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">
            <asp:Panel ID="pndates" runat="server" CssClass="nyspanelsSM" Style="left: 114px;
                position: absolute; top: 4px; width: 782px; height: 59px; z-index: 10000;">
                <asp:Label ID="lblto" runat="server" CssClass="nyslabel" Style="z-index: 103; position: absolute;
                    top: 34px; width: 18px; right: 658px;" Text="to:"></asp:Label>
                <asp:Label ID="lblfrom" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 13px;
                    position: absolute; top: 11px; width: 111px; right: 658px;" Text="Enter dates from:"></asp:Label>
                <asp:TextBox ID="txtfrom" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 127px; position: absolute; top: 8px; width: 77px;" TabIndex="1"></asp:TextBox>
                <asp:TextBox ID="txtto" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 127px; position: absolute; top: 31px; width: 77px;" TabIndex="2"></asp:TextBox>
                <asp:TextBox ID="txtclientid" runat="server" CssClass="nystextbox" Style="z-index: 98;
                    left: 192px; position: absolute; top: 8px; width: 9px;" TabIndex="1" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtbossname" runat="server" CssClass="nystextbox" Style="z-index: 98;
                    left: 184px; position: absolute; top: 8px; width: 9px;" TabIndex="1" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtbossname2" runat="server" CssClass="nystextbox" Style="z-index: 98;
                    left: 184px; position: absolute; top: 8px; width: 9px;" TabIndex="1" Visible="False"></asp:TextBox>
                <asp:Label ID="lbllocation0" runat="server" CssClass="nyslabel" Style="z-index: 103;
                    position: absolute; top: 34px; width: 46px; right: 454px;" Text="Airline:"></asp:Label>
                <asp:Label ID="lbllocation" runat="server" CssClass="nyslabel" Style="z-index: 103;
                    left: 223px; position: absolute; top: 11px; width: 105px; right: 454px;" Text="Airport/Location:"></asp:Label>
                <asp:TextBox ID="txtairline" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 333px; position: absolute; top: 31px; width: 350px; right: 99px;" TabIndex="3"></asp:TextBox>
                <asp:TextBox ID="txtlocation" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 333px; position: absolute; top: 8px; width: 350px; right: 99px;" TabIndex="3"></asp:TextBox>
                <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" ImageUrl="~/images/refresh_out.gif"
                    Style="z-index: 109; left: 693px; position: absolute; top: 29px" TabIndex="4"
                    BorderStyle="None" />
            </asp:Panel>
            <cc1:autocompleteextender id="extLocationSearch" runat="server" targetcontrolid="txtlocation"></cc1:autocompleteextender>
            <cc1:autocompleteextender id="extAirlineSearch" runat="server" targetcontrolid="txtairline"></cc1:autocompleteextender>
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif" />
            <asp:TextBox ID="txthidden_location" runat="server" CssClass="nystextbox" Style="z-index: 99;
                left: 380px; position: absolute; top: 4px; width: 23px;" TabIndex="3"></asp:TextBox>
            <asp:TextBox ID="txthidden_airline" runat="server" CssClass="nystextbox" Style="z-index: 99;
                left: 380px; position: absolute; top: 4px; width: 23px;" TabIndex="3"></asp:TextBox>
            <asp:TextBox ID="txthidden_locationCode" runat="server" CssClass="nystextbox" Style="z-index: 99;
                left: 380px; position: absolute; top: 4px; width: 23px;" TabIndex="3"></asp:TextBox>
            <rsweb:reportviewer id="rvAirTT" runat="server" backcolor="White" cssclass="reportView"
                documentmapwidth="100%" font-names="Verdana" font-size="8pt" internalborderstyle="None"
                internalborderwidth="2px" showcredentialprompts="False" showdocumentmapbutton="False"
                showpagenavigationcontrols="False" showparameterprompts="False" showpromptareabutton="False"
                showzoomcontrol="False" style="left: 4px; position: absolute; z-index: 1000;
                top: 68px;" toolbaritemhoverbackcolor="Silver" linkdisabledcolor="LightGray"
                toolbaritemborderstyle="None" tabindex="5" linkactivecolor="Blue" linkactivehovercolor="DeepPink"
                height="575px" showbackbutton="False" width="995px" interactivedeviceinfos="(Collection)"
                waitmessagefont-names="Verdana" waitmessagefont-size="14pt"><LocalReport 
                reportpath="Reports\repAirTTAllClients_FromBOSS.rdlc"><datasources><rsweb:ReportDataSource 
                    DataSourceId="ObjectDataSource1" Name="dsAirTTAllClients_FromBOSS" /></datasources></LocalReport></rsweb:reportviewer>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                TypeName="NYSInternalAdmin.dsAirTTAllClients_FromBOSSTableAdapters.AirTTAllClientsTableAdapter">

                <SelectParameters>
                    <asp:ControlParameter ControlID="txtfrom" Name="startdate" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtto" Name="enddate" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txthidden_location" Name="location" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txthidden_locationCode" Name="locationcode" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txthidden_airline" Name="airlineName" PropertyName="Text"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
