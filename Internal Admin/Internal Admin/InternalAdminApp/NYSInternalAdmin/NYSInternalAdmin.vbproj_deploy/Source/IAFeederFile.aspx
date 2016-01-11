<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAFeederFile.aspx.vb"
    Inherits="NYSInternalAdmin.IAFeederFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $find("cexFrom").add_showing(changeZIndex);
            $find("cexTo").add_showing(changeZIndex1);
            $find("cexExtra").add_showing(changeZIndex2);
        }
        function changeZIndex() {
            $find("cexFrom")._popupDiv.style.zIndex = 100000000;
        }
        function changeZIndex1() {
            $find("cexTo")._popupDiv.style.zIndex = 100000000;
        }
        function changeZIndex2() {
            $find("cexExtra")._popupDiv.style.zIndex = 100000000;
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
    <cc1:calendarextender id="cexTo" runat="server" targetcontrolid="txtto">
        </cc1:calendarextender>
    <cc1:calendarextender id="cexFrom" runat="server" targetcontrolid="txtfrom">
        </cc1:calendarextender>
    <cc1:calendarextender id="cexExtra" runat="server" targetcontrolid="txtextra">
        </cc1:calendarextender>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="AjaxService.asmx" />
            </Services>
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
                targetcontrolid="imHover" offsety="-12" offsetx="-11" popdelay="1">
            </cc1:hovermenuextender>
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
        <asp:Label ID="lblBookingDetails" runat="server" CssClass="nyslabelHeaderCentre"
            Style="z-index: 101; left: 4px; position: absolute; top: -2px; width: 1010px;"
            Text="Feeder File Generation"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" Style="left: 4px; position: absolute; z-index: 100;
            top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">

            <asp:Panel ID="pndates" runat="server" CssClass="nyspanels" Style="left: 297px; position: absolute;
                top: 4px; width: 414px; height: 34px; z-index: 10000;">
                <asp:Label ID="lblto" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 212px;
                    position: absolute; top: 10px; width: 18px; right: 341px;" Text="to:"></asp:Label>
                <asp:Label ID="lblfrom" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 8px;
                    position: absolute; top: 10px; width: 114px; right: 449px;" Text="Enter dates from:"></asp:Label>
                <asp:TextBox ID="txtfrom" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 122px; position: absolute; top: 8px; width: 77px;" TabIndex="1"></asp:TextBox>
                <asp:TextBox ID="txtto" runat="server" CssClass="nystextbox" Style="z-index: 99;
                    left: 233px; position: absolute; top: 8px; width: 77px;" TabIndex="1"></asp:TextBox>
                <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" ImageUrl="~/images/run_out.gif"
                    Style="z-index: 109; left: 320px; position: absolute; top: 6px" TabIndex="3"
                    BorderStyle="None" />
            </asp:Panel>
            <asp:Label ID="lblextra" runat="server" CssClass="nyslabel" Style="z-index: 103;
                left: 720px; position: absolute; top: 16px; width: 75px; right: 215px;" Text="Extra date:"></asp:Label>
            <asp:TextBox ID="txtextra" runat="server" CssClass="nystextbox" Style="z-index: 99;
                left: 793px; position: absolute; top: 13px; width: 77px;" TabIndex="1"></asp:TextBox>
            <asp:Button ID="btnCima" runat="server" CssClass="btnclasshand" Text="Send to CIMA"
                Style="z-index: 109; left: 896px; position: absolute; top: 14px; width: 100px;"
                OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
            <asp:Button ID="btnToCsv" runat="server" CssClass="btnclasshand" Text="Create file"
                Style="z-index: 109; left: 883px; position: absolute; top: 14px; width: 113px;" />
            <asp:Label ID="lblCimaDate" runat="server" CssClass="nyslabel" Style="z-index: 103;
                left: 154px; position: absolute; top: 6px; width: 139px; right: 717px;" Text=""></asp:Label>
            <asp:Button ID="btnO2" runat="server" CssClass="btnclasshand" Text="Create files"
                Style="z-index: 109; left: 780px; position: absolute; top: 14px; width: 105px;" />
            <asp:Button ID="btnO2Emails" runat="server" CssClass="btnclasshand" Text="Send O2 emails to Steria"
                Style="z-index: 109; left: 891px; position: absolute; top: 14px; width: 105px;"
                Visible="False" />
            <asp:TextBox ID="txtresult" runat="server" CssClass="nystextbox" Style="z-index: 99;
                left: 4px; position: absolute; top: 51px; width: 998px; height: 280px;" TabIndex="5"
                TextMode="MultiLine"></asp:TextBox>
            
            <asp:TextBox ID="txtresult2" runat="server" CssClass="nystextbox" Style="z-index: 99;
                left: 4px; position: absolute; top: 365px; width: 998px; height: 280px;" TabIndex="5"
                TextMode="MultiLine"></asp:TextBox>
            <asp:HyperLink runat="server" ID="hlFeederFiles" Visible="false" Style="z-index: 111;
                left: 28px; position: absolute; top: 620px;"></asp:HyperLink>
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif" />
            <asp:Label ID="lbldwp" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 5px;
                position: absolute; top: 33px; width: 114px; right: 891px;" Text="DWP:"></asp:Label>
            <asp:Label ID="lblcmec" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 5px;
                position: absolute; top: 347px; width: 114px; right: 891px;" Text="CMEC:"></asp:Label>
        </asp:Panel>
                    <asp:Button ID="btnO2AdquiraInstructions" runat="server" Style="z-index: 1000; left: 804px; position: absolute;
                top: 626px;" Text="Adquira Instructions" Visible="False" />
        <asp:Panel ID="pnO2Adquira" runat="server" 
            Style="z-index: 110; left: 1039px; position: absolute; top: 25px; width: 880px; height: 526px; padding: 60px;">
                Please follow these steps for Uploading a feeder file to Adquira for O2:

        <ol>
            <li>Sign in to <a href="https://mkpes.adquira.com/">https://mkpes.adquira.com/</a></li>
            <li>Click on 'Raised Invoices'</li>
            <li>Click on 'New Invoice'</li>
            <li>Choose 'Create invoices WITHOUT purchase order reference' > 'Load invoice file' > 'Load CSV invoice file'</li>
            <li>Click 'Browse' then paste this path into the file browser and select the CSV file from the folder: <asp:literal runat="server" ID="litAdquiraFilePath"></asp:literal></li>
            <li>Press the Send button to begin processing the file</li>
            <li>Now go to 'Raised Invoices' again and check all invoices have been uploaded</li>
            <li>If an exclaimation mark is against any invoice it means there is an error - these need correcting now</li>
            <li>Still on 'Raised Invoices', tick all invoices and press the 'Sign' button</li>
            <li>This will now show you all signed invoices waiting to be sent, tick all the invoices and press the 'Send' button</li>
            <li>The invoices will now be marked as "Sending"</li>
        </ol>

        If an invoice fails to send an email will be received in the O2 Invoicing inbox detailing the problem. This can take a few hours to come through.
        </asp:Panel>
    </div>
    </form>
</body>
</html>
