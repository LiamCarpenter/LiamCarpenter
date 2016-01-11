<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FeedMain.aspx.vb" Inherits="Cubit.FeedMain" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="avg" Assembly="ScrollingGrid" Namespace="AvgControls" %>
<%@ Register Assembly="skmDataGrid" Namespace="skmDataGrid" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CUBIT - View</title>
    <link href="Cubit.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="ScrollingGrid.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#ddgroup option").each(function () {
                //alert($(this).text() + " : " + $(this).val());
                //alert($(this).text().substring(1, 2))
                var tester = $(this).text().substring(0, 2);

                if (tester == '(0') {
                    $(this).css('color', 'Red');
                }
                else {
                    $(this).css('color', 'Green');
                }

            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainer">
        <asp:ScriptManager runat="server">
            <Services>
                <asp:ServiceReference Path="AjaxService.asmx" />
            </Services>
        </asp:ScriptManager>
        <asp:Panel ID="pnconfirm" runat="server" BackColor="Transparent" BackImageUrl="~/images/delete_dialog.gif"
            Height="665px" Style="z-index: 1000; left: 8px; position: absolute; top: 692px"
            Visible="False" Width="994px">
            <asp:Button ID="btnConfrim" runat="server" Style="z-index: 100; left: 416px; position: absolute;
                top: 352px" TabIndex="74" Text="Yes" Width="60px" />
            <asp:Button ID="btnCancel" runat="server" Style="z-index: 101; left: 504px; position: absolute;
                top: 352px" TabIndex="74" Text="No" Width="60px" />
            <asp:Label ID="lblAccomDateCheck" runat="server" Style="z-index: 101; left: 362px;
                position: absolute; top: 304px; height: 39px; width: 269px;" BackColor="White"
                CssClass="black_text">Are you sure you want to delete this record?</asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnmain" runat="server" Height="665px" Style="z-index: 499; left: 8px;
            position: absolute; top: 8px" Width="994px" CssClass="nyspanels">
            <asp:Label ID="lbluserinitials" runat="server" CssClass="nysitaliclabel" Style="z-index: 113; left: 393px; position: absolute; top: 627px; width: 10px; right: 591px;" Visible="False"></asp:Label>
            <asp:Label ID="lbluserid" runat="server" CssClass="nysitaliclabel" Style="z-index: 113; left: 393px; position: absolute; top: 627px; width: 10px;" Visible="False"></asp:Label>
            <asp:Label ID="lblcount" runat="server" CssClass="nysitaliclabel" Style="z-index: 113; left: 7px; position: absolute; top: 629px" Width="376px"></asp:Label>
            <asp:Label ID="lbluser" runat="server" CssClass="nysitaliclabel" Style="z-index: 113; left: 7px; position: absolute; top: 645px" Width="376px"></asp:Label>
            <asp:Label ID="lblversion" runat="server" CssClass="nysitaliclabel" Style="z-index: 115; left: 396px; position: absolute; top: 645px; width: 83px;"></asp:Label>
            <asp:Panel ID="pnsearch" runat="server" Style="z-index: 501; left: 1012px; position: absolute;
                top: 4px; width: 984px; right: -69px; height: 42px;" TabIndex="-1" Visible="False"
                CssClass="nyspanelwhite">
                <asp:TextBox ID="txtname" runat="server" CssClass="nystextbox" MaxLength="50" Style="z-index: 555; position: absolute; top: 18px; right: 512px; width: 254px;" TabIndex="7"></asp:TextBox>
                <asp:DropDownList ID="ddstatus" runat="server" CssClass="nysdropdown" Style="z-index: 115;
                    position: absolute; top: 18px; right: 775px; width: 200px;" Width="206px" TabIndex="6" Height="20px">
                </asp:DropDownList>
                <asp:ImageButton ID="btncancelsearch" runat="server" Style="z-index: 115; left: 896px; position: absolute; top: 14px; height: 24px;" ImageUrl="~/images/cancel_trans.gif" CssClass="imageButton" AlternateText="Cancel" TabIndex="12" />
                <asp:ImageButton ID="btnsearch" runat="server" AlternateText="Search" CssClass="imageButton"
                    ImageUrl="~/images/search.gif" Style="z-index: 115; left: 810px; position: absolute;
                    top: 14px; height: 24px;" TabIndex="11" />
                <asp:TextBox ID="txtinvoice" runat="server" CssClass="nystextbox" MaxLength="5" Style="z-index: 115;
                    left: 478px; position: absolute; top: 18px; width: 103px;" TabIndex="8"></asp:TextBox>
                <asp:TextBox ID="txtguestname" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 699px; position: absolute; top: 18px; width: 103px;"
                    TabIndex="10"></asp:TextBox>
                <asp:TextBox ID="txtguestpnr" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 588px; position: absolute; top: 18px; width: 104px;"
                    TabIndex="9"></asp:TextBox>
                <asp:Label ID="lblgroup3" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 700px; position: absolute; top: 4px; width: 98px; height: 14px;">Guest 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; name:</asp:Label>
                <asp:Label ID="lblgroup2" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 589px; position: absolute; top: 4px; width: 61px; height: 14px;">Guest 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; PNR:</asp:Label>
                <asp:Label ID="lblgroup1" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 479px; position: absolute; top: 4px; width: 79px; height: 14px;">Venue inv 
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; no:</asp:Label>
                <asp:Label ID="lblgroup5" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 9px; position: absolute; top: 4px; width: 78px; height: 14px;">Status:</asp:Label>
                <asp:Label ID="lblgroup0" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 219px; position: absolute; top: 4px; width: 75px; height: 14px;">Venue 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; name:</asp:Label>
                <cc2:AutoCompleteExtender ID="extVenueName" runat="server" TargetControlID="txtname">
                </cc2:AutoCompleteExtender>
                <cc2:AutoCompleteExtender ID="extInvoice" runat="server" TargetControlID="txtinvoice">
                </cc2:AutoCompleteExtender>
                <cc2:AutoCompleteExtender ID="extguestname" runat="server" TargetControlID="txtguestname">
                </cc2:AutoCompleteExtender>
                <cc2:AutoCompleteExtender ID="extguestPNRSearch" runat="server" TargetControlID="txtguestpnr">
                </cc2:AutoCompleteExtender>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" CssClass="nyspanelwhite" Style="z-index: 500;
                left: 4px; position: absolute; top: 72px; height: 544px;" Width="982px">
                <avg:ScrollingGrid ID="pndata" runat="server" Width="982px" Height="430px" HorizontalAlign="NotSet"
                    ScrollBars="Auto">
                    <cc1:PrettyDataGrid ID="grdData" runat="server" AutoGenerateColumns="False" Width="970px"
                        AllowSorting="True" TabIndex="3" RowHighlightColor="">
                        <SelectedItemStyle CssClass="gridSelected" ForeColor="Red" />
                        <AlternatingItemStyle CssClass="gridAlternate" />
                        <ItemStyle CssClass="gridItem" />
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="Dataid"></asp:BoundColumn>
                            <asp:ButtonColumn CommandName="edit" Text="Edit"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="transactionnumber" HeaderText="TRANS No" SortExpression="transactionnumber">
                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                <ItemStyle Width="60px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="EXC EXPORT">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkExcludeFromExport" Enabled="false" /></ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="costcode" HeaderText="AICOSTCODE" SortExpression="costcode">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="venuebosscode" HeaderText="VENUE BOSS CODE" SortExpression="venuebosscode">
                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                <ItemStyle Width="120px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Actualcomm" HeaderText="COMM %" SortExpression="Actualcomm">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="bookerinitials" HeaderText="BOOKERS INI" SortExpression="bookerinitials">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="bookeddate" HeaderText="BOOKED DATE" SortExpression="bookeddate"
                                DataFormatString="{0:dd-MM-yyyy}">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="transactioncode" HeaderText="TRANS CODE" SortExpression="transactioncode">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="transactionvalue" HeaderText="IMPORT VALUE" SortExpression="transactionvalue">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="transactionvaluenew" HeaderText="EDITED VALUE" SortExpression="transactionvaluenew">
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="venuename" HeaderText="VENUE NAME" SortExpression="venuename">
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="transactionlinenumber" HeaderText="TRANS LINE No" SortExpression="transactionlinenumber"
                                Visible="False">
                                <HeaderStyle HorizontalAlign="Left" Width="28px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="transactiondate" HeaderText="TRANS DATE" SortExpression="transactiondate"
                                DataFormatString="{0:dd-MM-yyyy}">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="arrivaldate" HeaderText="ARRIVAL DATE" SortExpression="arrivaldate"
                                DataFormatString="{0:dd-MM-yyyy}">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="departuredate" HeaderText="DEP DATE" SortExpression="departuredate"
                                DataFormatString="{0:dd-MM-yyyy}">
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="passengername" HeaderText="GUEST NAME" SortExpression="passengername">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ref1" HeaderText="REF1" SortExpression="ref1" Visible="False">
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ref2" HeaderText="REF2" SortExpression="ref2" Visible="False">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ref3" HeaderText="REF3" SortExpression="ref3" Visible="False">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="dept" HeaderText="DEPT" SortExpression="dept" Visible="false">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="booker" HeaderText="BOOKERS NAME" SortExpression="booker">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="nettamount" HeaderText="ITEM NETT" SortExpression="nettamount">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="vatamount" HeaderText="ITEM VAT" SortExpression="vatamount">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="vatrate" HeaderText="VAT RATE" SortExpression="vatrate"
                                Visible="False">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="totalamount" HeaderText="ITEM TOTAL" SortExpression="totalamount">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="venuedetails" HeaderText="VENUE DETAILS" SortExpression="venuedetails"
                                Visible="False">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="roomdetails" HeaderText="ROOM DETAILS" SortExpression="roomdetails"
                                Visible="False">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="groupid" HeaderText="GROUP ID" SortExpression="groupid"
                                Visible="False">
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="confermainvoicenumber" HeaderText="CONFERMA INV No" SortExpression="confermainvoicenumber"
                                Visible="False">
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="categoryid" Visible="False" HeaderText="categoryid" SortExpression="categoryid">
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="categoryname" HeaderText="CATEGORY" SortExpression="categoryname">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="categorybosscode" HeaderText="categorybosscode" SortExpression="categorybosscode" Visible="False">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="supplierinvoice" HeaderText="SUPPLIER INV" SortExpression="supplierinvoice">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="guestPNR" HeaderText="GUEST PNR" SortExpression="guestPNR">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="parameterid" HeaderText="parameterid" SortExpression="parameterid"
                                Visible="false">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="venuereference" HeaderText="venuereference" SortExpression="venuereference"
                                Visible="false">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="venueEX" Visible="false" HeaderText="venueEX" SortExpression="venueEX">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="venueDD" HeaderText="venueDD" SortExpression="venueDD"
                                Visible="False">
                                <ItemStyle Width="82px" HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="statusid" HeaderText="statusid" SortExpression="statusid"
                                Visible="False">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="parameterstart" HeaderText="parameterstart" SortExpression="parameterstart"
                                Visible="false">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="parameterend" HeaderText="parameterend" SortExpression="parameterend"
                                Visible="False">
                                <HeaderStyle HorizontalAlign="Left" Width="82px" />
                                <ItemStyle HorizontalAlign="Left" Width="82px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="transactionid" HeaderText="transactionid" SortExpression="transactionid"
                                Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="statusname" HeaderText="STATUS" SortExpression="statusname">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="invoiceid" HeaderText="invoiceid" SortExpression="invoiceid"
                                Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="invoicedate" HeaderText="invoiceexportdate" SortExpression="invoicedate"
                                Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="currency" HeaderText="CURRENCY" SortExpression="currency"
                                Visible="true"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ExcludeFromExport" HeaderText="EXCLUDE FROM EXPORT" SortExpression="ExcludeFromExport"
                                Visible="False">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="aicol6" HeaderText="AICOL6" SortExpression="aicol6">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="aicol7" HeaderText="AICOL7" SortExpression="aicol7">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="aicol8" HeaderText="AICOL8" SortExpression="aicol8">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="aicol9" HeaderText="AICOL9" SortExpression="aicol9">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="aicol10" HeaderText="AICOL10" SortExpression="aicol10">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TravellerEmail" HeaderText="TravellerEmail" SortExpression="TravellerEmail"
                                Visible="false">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="transactionnumber" HeaderText="TRANS No" SortExpression="transactionnumber">
                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                <ItemStyle Width="60px" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:ButtonColumn CommandName="remove" Text="Delete" Visible="false"></asp:ButtonColumn>
                        </Columns>
                        <HeaderStyle CssClass="gridHead" />
                    </cc1:PrettyDataGrid>
                    <asp:Label ID="lblexportresult" runat="server" Text="Last Export Result" Style="z-index: 115;
                        position: absolute; top: 463px; left: 6px; width: 115px; height: 8px;" CssClass="nyslabel">
                    </asp:Label>
                </avg:ScrollingGrid>
                <asp:Button ID="btnTestFileImport" runat="server" Style="z-index: 115; left: 881px;
                position: absolute; top: 466px; width: 90px; " Text="Import" />
                <asp:Button ID="btnTestFileImportBooked" runat="server" Style="z-index: 115; left: 880px;
                position: absolute; top: 492px; width: 90px; height: 19px;" Text="Import Booked" />
                <asp:Button ID="btnTestFileImporTrans" runat="server" Style="z-index: 115; left: 879px;
                position: absolute; top: 518px; width: 90px; height: 19px;" Text="Import Trans" />

                <asp:TextBox ID="txtexportresult" runat="server" ReadOnly="True" Style="z-index: 115; position: absolute;
                        top: 451px; left: 3px; width: 849px; height: 51px; margin-top: 29px;" 
                    TextMode="MultiLine">
                    </asp:TextBox>

            </asp:Panel>
            <asp:ImageButton ID="btnusesearch" runat="server" AlternateText="Use Search" CssClass="imageButton"
                ImageUrl="~/images/usesearch.gif" Style="z-index: 115; left: 354px; position: absolute;
                top: 39px; height: 24px;" Visible="False" TabIndex="2" />
            <%'Key Table %>
            <table width="100px" style="z-index: 115; left: 500px; position: absolute;">
                <tr><td class="gridBookingOnly">Booking Only</td></tr>
                <tr><td class="gridCurrency">Currency not GBP</td></tr>
                <tr><td class="gridREFUND">Refund</td></tr>
                <tr><td class="gridZero">Zero Commission</td></tr>
            </table>
            <asp:TextBox ID="txtdataid" runat="server" CssClass="evotextbox" Height="16px" Style="z-index: 132;
                position: absolute; visibility: hidden; top: 9px; left: 961px;" Width="20px"></asp:TextBox>
            <asp:Label ID="lblGroupView" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 7px; position: absolute; top: 6px; width: 254px;" Font-Bold="True"></asp:Label>
            <asp:Label ID="lblgroup" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 5px; position: absolute; top: 26px; width: 281px;">Select to view groups 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; unexported 
             transactions: </asp:Label>
            <asp:TextBox ID="txtscrolltop" runat="server" CssClass="nystextbox" MaxLength="50"
                Style="z-index: 113; left: 21px; position: absolute; top: 51px; height: 4px;
                width: 2px;"></asp:TextBox>
            <asp:TextBox ID="txtscrollleft" runat="server" CssClass="nystextbox" MaxLength="50"
                Style="z-index: 113; left: 19px; position: absolute; top: 49px; height: 2px;
                width: 1px;"></asp:TextBox>
            <asp:ImageButton ID="btnBossImport" runat="server" AlternateText="Export" CssClass="imageButton"
                ImageUrl="~/images/boss.gif" Style="z-index: 113; left: 824px; position: absolute;
                top: 635px; height: 24px;" TabIndex="4" Visible="False" />
            <asp:ImageButton ID="btnexport" runat="server" AlternateText="Export" CssClass="imageButton"
                ImageUrl="~/images/export.gif" Style="z-index: 113; left: 741px; position: absolute;
                top: 635px; height: 24px;" TabIndex="5" Visible="False" />
            <asp:ImageButton ID="btnadmin" runat="server" CssClass="imageButton" ImageUrl="~/images/admin.gif"
                Style="z-index: 115; left: 908px; position: absolute; top: 635px; height: 24px;"
                AlternateText="Admin" Visible="False" TabIndex="6" />
            <%--<asp:Button ID="btnTestFileImport" runat="server" Style="z-index: 115; left: 477px;
                position: absolute; top: 643px; width: 53px; height: 19px;" Text="Import" />
                <asp:Button ID="btnTestFileImportBooked" runat="server" Style="z-index: 115; left: 540px;
                position: absolute; top: 642px; width: 53px; height: 19px;" Text="Import Booked" />
                <asp:Button ID="btnTestFileImporTrans" runat="server" Style="z-index: 115; left: 606px;
                position: absolute; top: 642px; width: 53px; height: 19px;" Text="Import Trans" />--%>
            <asp:Button ID="btnTestDelete" runat="server" Style="z-index: 115; left: 602px; position: absolute;
                top: 636px; height: 24px; width: 40px;" Text="Del" />
            <asp:Button ID="btnEdit" runat="server" Style="z-index: 115; left: 671px; position: absolute;
                top: 635px; height: 24px; width: 62px;" Text="Edit Grid" />
            <asp:Button ID="btnSaveGrid" runat="server" Style="z-index: 115; left: 671px; position: absolute;
                top: 635px; height: 24px; width: 62px;" Text="Save Grid" Visible="false" />
            <asp:Button ID="btnTestFileDownload" runat="server" Style="z-index: 115; left: 539px;
                position: absolute; top: 637px; height: 24px; width: 40px;" Text="Dload" />
            <asp:Panel ID="pnsure" runat="server" BackColor="Transparent" BackImageUrl="~/images/discount_dialogV2.gif"
                Height="665px" Style="z-index: 512; left: 2020px; position: absolute; top: 16px"
                Visible="False" Width="994px">
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblfeed" runat="server" BackColor="White" CssClass="nyslabelcentre"
                    Height="32px" Style="z-index: 111; left: 351px; position: absolute; top: 302px;
                    width: 296px;"></asp:Label>
                <asp:ImageButton ID="btnno" runat="server" AlternateText="No" CssClass="imageButton"
                    ImageUrl="~/images/no.gif" Style="z-index: 115; left: 508px; position: absolute;
                    top: 350px; height: 24px;" TabIndex="14" />
                <asp:ImageButton ID="btnyes" runat="server" AlternateText="Yes" CssClass="imageButton"
                    ImageUrl="~/images/yes.gif" Style="z-index: 115; left: 411px; position: absolute;
                    top: 350px; height: 24px; right: 504px;" TabIndex="13" />
            </asp:Panel>
            <asp:DropDownList ID="ddgroup" runat="server" AutoPostBack="True" CssClass="nysdropdown"
                Style="z-index: 114; left: 4px; position: absolute; top: 42px; width: 337px;"
                TabIndex="1" Height="20px">
            </asp:DropDownList>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
