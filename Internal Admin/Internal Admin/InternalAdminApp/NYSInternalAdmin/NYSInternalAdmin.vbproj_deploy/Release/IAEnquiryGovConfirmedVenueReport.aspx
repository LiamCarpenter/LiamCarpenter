<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAEnquiryGovConfirmedVenueReport.aspx.vb"
    Inherits="NYSInternalAdmin.IAEnquiryGovConfirmedVenueReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; z-index: 1001;
            top: 20px; width: 1010px; height: 652px; display: none;" CssClass="nyspanelwhite"
            BackColor="Transparent" BackImageUrl="~/images/trans_light.gif">
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

        <asp:Label ID="lbldetails" runat="server" CssClass="nyslabelHeaderCentre" Style="z-index: 101;
            left: 4px; position: absolute; top: -2px; width: 1010px;" Text="Govement Confirmed Venues Report"
            Font-Size="Medium"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" Style="left: 4px; position: absolute; z-index: 100;
            top: 20px; height: 652px; width: 1010px;" CssClass="nyspanels">
              <asp:TextBox ID="txtcompany" runat="server" CssClass="nystextbox" Style="z-index: 98; left: 242px; position: absolute;
                top: 9px; width: 8px; height: 10px;" TabIndex="1" Visible="False"></asp:TextBox>
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif" />
            <rsweb:ReportViewer ID="rvGovConfiredvenueReport" runat="server" BackColor="White"
                CssClass="reportView" DocumentMapWidth="100%" Font-Names="Verdana" Font-Size="8pt"
                Height="600px" InternalBorderStyle="None" InternalBorderWidth="2px" ShowCredentialPrompts="False"
                ShowDocumentMapButton="False" ShowPageNavigationControls="False" ShowParameterPrompts="False"
                ShowPromptAreaButton="False" ShowZoomControl="False" Style="left: 4px; position: absolute;
                z-index: 100; top: 48px;" ToolBarItemHoverBackColor="Silver" Width="995px" LinkDisabledColor="LightGray"
                ToolBarItemBorderStyle="None" LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink"
                ShowBackButton="False" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="Reports\repEnquiryGovCancellationVenueReport.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>

            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                TypeName="NYSInternalAdmin.dsEnquiryGovConfirmedVenueReportTableAdapters.EnquiryGovConfirmedVenueReportTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtcompany" Name="groupID" PropertyName="Text" 
                        Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

      </asp:Panel>
    </div>
    </form>
</body>
</html>
