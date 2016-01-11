<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IARSAReport.aspx.vb" Inherits="NYSInternalAdmin.IARSAReport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
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
        <asp:Label ID="lblBookingDetails" runat="server" 
             CssClass="nyslabelHeaderCentre" Style="z-index: 101; left: 4px; position: absolute;
                top: -2px; width: 1010px;" Text="RSA Report"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >
            <asp:Panel ID="pndates" runat="server" CssClass="nyspanels" 
                 Style="left: 297px; position: absolute; top: 4px; width: 414px; height: 34px; z-index: 10000;">
                <asp:Label ID="lblto" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 212px;
                        position: absolute; top: 10px; width: 18px; right: 341px;" Text="to:"></asp:Label>
                <asp:Label ID="lblfrom" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 8px;
                        position: absolute; top: 10px; width: 114px; right: 449px;" 
                    Text="Enter dates from:"></asp:Label>
                <asp:TextBox ID="txtfrom" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 122px; position: absolute;
                        top: 8px; width: 77px;" TabIndex="1"></asp:TextBox>
                <asp:TextBox ID="txtto" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 233px; position: absolute;
                        top: 8px; width: 77px; " TabIndex="1"></asp:TextBox>
                 <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" 
                     ImageUrl="~/images/run_out.gif" 
                     Style="z-index: 109; left: 320px; position: absolute; top: 6px" 
                     TabIndex="3" BorderStyle="None" /> 

            </asp:Panel>

            <asp:Image ID="imHover" runat="server" 
            Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>  
                  
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                TypeName="NYSInternalAdmin.dsRSAReport_DepositsTableAdapters.RSAReport_DepositsTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtfrom" Name="StartDate" PropertyName="Text" 
                        Type="String" />
                    <asp:ControlParameter ControlID="txtto" Name="EndDate" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            
            <asp:Label runat="server" ID="lblEmail" CssClass="nyslabel"
                style="position:absolute;top:15px; left:775px;">Send Files to:</asp:Label>
            <asp:TextBox runat="server" ID="txtEmail" CssClass="nystextbox"
                style="position:absolute;top:13px; left:866px;"></asp:TextBox>
            <asp:Button ID="btnSendFiles" runat="server" CssClass="Button" 
                Text="Send Invoice Files" 
                style="position:absolute;top:37px; left:829px; width:100px" />
            
            <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                TypeName="NYSInternalAdmin.dsRSAReport_InvoicesTableAdapters.RSAReport_InvoicesTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtfrom" Name="StartDate" PropertyName="Text" 
                        Type="String" />
                    <asp:ControlParameter ControlID="txtto" Name="EndDate" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            
            <asp:Label ID="Label1" runat="server" CssClass="nyslabelHeader" Text="Deposits" 
                style="position:absolute;top:44px; left:8px;"></asp:Label>
            
            
            <rsweb:ReportViewer ID="rvRSADeposits" runat="server" BackColor="White" 
                CssClass="reportView" DocumentMapWidth="100%" Font-Names="Verdana" 
                Font-Size="8pt" InternalBorderStyle="None" InternalBorderWidth="2px" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" 
                LinkDisabledColor="LightGray" ShowBackButton="False" 
                ShowCredentialPrompts="False" ShowDocumentMapButton="False" 
                ShowFindControls="True" 
                ShowParameterPrompts="False" ShowPromptAreaButton="False" 
                ShowZoomControl="False" 
                Style="left: 4px; position: absolute; z-index:100; top: 74px;" 
                ToolBarItemBorderStyle="None" ToolBarItemHoverBackColor="Silver" 
                Width="995px" Height="260px">
                <LocalReport ReportPath="Reports\repRSAReport_Deposits.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDatasource2" 
                            Name="dsRSAReport_Deposits" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>


            <rsweb:ReportViewer ID="rvRSAInvoices" runat="server" BackColor="White" 
                CssClass="reportView" DocumentMapWidth="100%" Font-Names="Verdana" 
                Font-Size="8pt" InternalBorderStyle="None" InternalBorderWidth="2px" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" 
                LinkDisabledColor="LightGray" ShowBackButton="False" 
                ShowCredentialPrompts="False" ShowDocumentMapButton="False" 
                ShowFindControls="True" 
                ShowParameterPrompts="False" ShowPromptAreaButton="False" 
                ShowZoomControl="False" 
                Style="left: 4px; position: absolute; z-index:100; top: 383px;" 
                ToolBarItemBorderStyle="None" ToolBarItemHoverBackColor="Silver" 
                Width="995px" Height="260px"><LocalReport ReportPath="Reports\repRSAReport_Invoices.rdlc"><DataSources><rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                            Name="dsRSAReport_Invoices" /></DataSources></LocalReport></rsweb:ReportViewer>
            <asp:Label ID="Label2" runat="server" CssClass="nyslabelHeader" 
                style="position:absolute;top:353px; left:9px;" Text="Invoices"></asp:Label>
        </asp:Panel>

         <cc1:CalendarExtender ID="cexTo" runat="server" TargetControlID="txtto">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="cexFrom" runat="server" TargetControlID="txtfrom">
        </cc1:CalendarExtender>
    </div>
    </form>
</body>
</html>
