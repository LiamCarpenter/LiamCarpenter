<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAAdminEvolvi.aspx.vb" Inherits="NYSInternalAdmin.IAAdminEvolvi" %>

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
                document.getElementById(element).innerHTML = "<img style='position: absolute; top: 301px; left: 0px;' src='images/white.gif'><img style='position: absolute; top: 310px; left: 425px;' src='images/ajax-loader.gif'><p style='position:absolute; top: 330px; left:370px; font-family: Verdana; font-size: small; font-weight: bold; font-style: normal; color: #515151; background-color: #FFFFFF;'>Please wait while the report is compiled......</p>";
            }
            else if (document.getElementById(element).style.display === 'block') {
                document.getElementById(element).style.display = 'none';
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; z-index: 1001; top: 20px; width: 1010px; height: 652px; display: none;" CssClass="nyspanelwhite"
            BackColor="Transparent" BackImageUrl="~/images/trans_light.gif">
        </asp:Panel>
        <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; z-index: 1000; top: 2000px; width: 398px; height: 520px;" CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" ImageUrl="~/images/logout_out.gif" Style="z-index: 5000; left: 310px; position: absolute;
                top: 486px" TabIndex="3" BorderStyle="None" />
            <cc1:hovermenuextender id="HoverMenuExtender1" runat="server" popupcontrolid="pnNav" targetcontrolid="imHover" offsety="-12" offsetx="-11">
            </cc1:hovermenuextender>
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlMain" Style="left: 4px; position: absolute; z-index: 100; top: 20px; height: 780px; width: 1010px;" CssClass="nyspanels">
            <asp:Image ID="imHover" runat="server" Style="z-index: 100; left: 9px; position: absolute; top: 10px;" ImageUrl="~/images/menu.gif" />
            <asp:Panel runat="server" ID="pnlSearch" Visible="true" Style="position: absolute; top: 76px; left: 8px; width: 495px;">
                <table style="width: 497px">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblGroup" Text="Edit Client: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddGroup" Width="352px" CssClass="nysdropdown">
                            </asp:DropDownList>
                            <asp:LinkButton runat="server" ID="lnkAddNewClient" Text="Add New Client" OnClick="lnkAddNewClient_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <hr style="width: 483px;" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDetails" Visible="false" Style="position: absolute; top: 140px; left: 8px; width: 964px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblClientName" Text="Client Name: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtClientName" MaxLength="200" Width="369px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblBossCode" Text="BOSS Code: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtBossCode" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblInterceptorFeesActive" Text="Charge Fees through Interceptor: "></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkInterceptorFeesActive" Text="Active" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblInterceptorDiscountActive" Text="Apply discount through Interceptor: "></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkInterceptorDiscountActive" Text="Active" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label1" Text="Discount Percentage: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDiscountPercentage"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label2" Text="&#37"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPerTicket" Text="Per Ticket fee"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPerPax" Text="Per Passenger fee"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPerOrderItem" Text="Per Order Item fee"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPerbasket" Text="Per Basket fee"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblKisokOnlineFee" Text="Kiosk online Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOnlineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOnlineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOnlineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOnlineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblKisokOfflineFee" Text="Kiosk offline Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOfflineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOfflineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOfflineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKioskOfflineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label runat="server" ID="lblToDOnlineFee" Text="ToD online Fee: "></asp:Label>
                        </td>
                        <td class="style1">
                            <asp:TextBox runat="server" ID="txtToDOnlineFeePerTicket"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:TextBox runat="server" ID="txtToDOnlineFeePerPax"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:TextBox runat="server" ID="txtToDOnlineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:TextBox runat="server" ID="txtToDOnlineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblToDOfflineFee" Text="ToD offline Fee: "></asp:Label>
                        </td>
                        <td class="style6">
                            <asp:TextBox runat="server" ID="txtToDOfflineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtToDOfflineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtToDOfflineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtToDOfflineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblPostageOnlineFee" Text="1st Class Online Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOnlineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOnlineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOnlineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOnlineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblPostageOfflineFee" Text="1st Class Offline Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOfflineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOfflineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOfflineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostageOfflineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblCourierOnlineFee" Text="Courier Online Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOnlineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOnlineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOnlineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOnlineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblCourierOfflineFee" Text="Courier Offline Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOfflineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOfflineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOfflineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCourierOfflineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSpecialDeliveryOnlineFee" Text="Special Delivery Online Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOnlineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOnlineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOnlineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOnlineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSpecialDeliveryOfflineFee" Text="Special Delivery Offline Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOfflineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOfflineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOfflineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSDOfflineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSatSpecialDeliveryOnlineFee" Text="Saturday Special Delivery Online Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOnlineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOnlineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOnlineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOnlineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSatSpecialDeliveryOfflineFee" Text="Saturday Special Delivery Offline Fee: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOfflineFeePerTicket"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOfflineFeePerPax"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOfflineFeePerOrderItem"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSSDOfflineFeePerBasket"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblBooker" Text="Add Booker to invoice? "></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkBooker" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblCutomPO" Text="Custom PO: "></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkCustomPO" Text="Active" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCustomPoValue" Text="Value:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCustomPO"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSplitBy" Text="Split Bookings into multiple invoice files?"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddSplitNode" AutoPostBack="true">
                                <asp:ListItem Text="Don't Split" Value=""></asp:ListItem>
                                <asp:ListItem Text="By Passenger Node" Value="Passenger"></asp:ListItem>
                                <asp:ListItem Text="By CustomField Node" Value="CustomField"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSplitOnAttribute" Text="Field name:"></asp:Label>
                            <br />
                            <label>
                                (Case Sensitive)</label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSplitField" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <label>
                                Field Value:</label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSplitFieldValue" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <label>
                                Field name of unique value to split on:</label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtUniqueFieldName" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlButtons" Visible="false" Style="position: absolute; left: 9px; height: 52px; top: 707px; width: 406px;">
                <asp:Button runat="server" Style="z-index: 101; position: absolute; top: 14px; left: 13px; width: 85px;" ID="btnSave" Visible="false" Text="Save" />
                <asp:Button runat="server" Style="z-index: 101; position: absolute; top: 14px; left: 150px; width: 85px;" ID="btnAdd" Visible="false" Text="Add" />
                <asp:Button runat="server" Style="z-index: 101; position: absolute; top: 14px; left: 279px; width: 85px;" ID="btnCancel" Visible="false" Text="Cancel" />
            </asp:Panel>
            <asp:Label ID="lblBookingDetails" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 8px; position: absolute; top: 46px; width: 208px;"
                Text="Evolvi Client Admin"></asp:Label>
        </asp:Panel>
    </div>
    <asp:Panel runat="server" ID="pnlClientID" Visible="false">
        <asp:HiddenField runat="server" ID="hdnClientID" />
    </asp:Panel>
    </form>
</body>
</html>
