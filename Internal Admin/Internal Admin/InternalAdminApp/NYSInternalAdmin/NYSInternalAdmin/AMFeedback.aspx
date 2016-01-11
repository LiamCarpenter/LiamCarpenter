<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AMFeedback.aspx.vb" Inherits="NYSInternalAdmin.AMFeedback" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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


        //On scrolling of DIV tag.
        function OnDivScroll(tag) {
            var lstCollegeNames = document.getElementById(tag);

            if (lstCollegeNames.options.length > 8) {
                lstCollegeNames.size = lstCollegeNames.options.length;
            }
            else {
                lstCollegeNames.size = 8;
            }
        }

        function OnSelectFocus(divtag,tag) {

            if (document.getElementById(divtag).scrollLeft != 0) {
                document.getElementById(divtag).scrollLeft = 0;
            }

            var lstCollegeNames = document.getElementById(tag);

            if (lstCollegeNames.options.length > 8) {
                lstCollegeNames.focus();
                lstCollegeNames.size = 8;
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 180px;
        }
        .style4
        {
            width: 114px;
            background-color: #CCCCCC;
        }
        .style5
        {
            width: 173px;
        }
        .style6
        {
            width: 162px;
        }
        .style7
        {
            width: 110px;
        }
        .style9
        {
            width: 109px;
        }
        .style10
        {
            background-color: #CCCCCC;
        }
        .style11
        {
            width: 119px;
        }
    </style>
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
            <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="pnNav"
                TargetControlID="imHover" OffsetY="-12" OffsetX="-11">
            </cc1:HoverMenuExtender>
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
         <asp:Image ID="imHover" runat="server" 
                Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
               <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index: 100;
                                                        top: 40px; height: 900px; width: 1200px; bottom: 232px;" 
            CssClass="nyspanels">

        <h2>
<%--            <asp:Button  ID="butMainMenu" runat="server" Text="<< Back to Main Menu" />--%>
<%--

           <centre>Feedback email selection</centre> --%>
        </h2>
        <table align="center"> 
     <tr>
         <td style="border: medium groove #000000;" align="center">Clients</td>
         <td class="style11" style="border: medium groove #000000;" align="center">Hotel</td>
         <td class="style9" style="border: medium groove #000000;" align="center">Rail</td>
         <td class="style9" style="border: medium groove #000000;" align="center">Air</td>

         <td class="style5" style="border: medium groove #000000;" align="center">From (MM/DD/YY)</td>
         <td class="style5" style="border: medium groove #000000;" align="center">To (From+1/01/YY)</td>

         <td class="style10" style="border: medium groove #000000;" align="center">Test (no external emails)</td>
         <td class="style10" style="border: medium groove #000000;" align="center">Test mails generate</td>

     </tr>
        
        <tr>
        <td style="border: medium groove #000000;" align="center"><asp:DropDownList ID="ddClients" runat="server"  CssClass="nysdropdown" AutoPostBack="true"></asp:DropDownList>
        </td>
  
        <td class="style11" style="border: medium groove #000000;" align="center"> <asp:CheckBox runat="server" id="chkHotel" ></asp:CheckBox></td>
        <td class="style11" style="border: medium groove #000000;" align="center"> <asp:CheckBox runat="server" id="chkRail" ></asp:CheckBox></td>
        <td class="style11" style="border: medium groove #000000;" align="center"> <asp:CheckBox runat="server" id="chkAir" ></asp:CheckBox></td>
            
         
           
        </td>
      
      <td class="style6" style="border: medium groove #000000;" align="center">  <asp:TextBox  id="txtFrom"  runat="server" width="100px" >03/01/14</asp:TextBox></td>
       <td class="style5" style="border: medium groove #000000;" align="center">   <asp:TextBox id="txtTo" runat="server"  width="100px" >04/01/14</asp:TextBox></td>
      
        
     

        <td class="style4" style="border: medium groove #000000;" align="center">
         <asp:CheckBox runat="server" id="chkTest"  ></asp:CheckBox>
           </td>
           <td class="style10" style="border: medium groove #000000;" align="center">            <asp:TextBox id="txtTest" runat="server" value="0" width="50px"></asp:TextBox>
           </td>

      
        </tr></table>

   


               
        <p>
            <asp:Button ID="butFeedbackEmails" runat="server" 
                Text="Generate Feedback Emails" />
            <asp:DropDownList ID="ddlPercent" runat="server">
                <asp:ListItem Value="10">10%</asp:ListItem>
                <asp:ListItem Value="20">20%</asp:ListItem>
                <asp:ListItem Value="30">30%</asp:ListItem>
                <asp:ListItem Value="40">40%</asp:ListItem>
                <asp:ListItem Value="50">50%</asp:ListItem>
                <asp:ListItem Value="999" Selected="True">As per values in DB</asp:ListItem>
                <asp:ListItem Value="100">100%</asp:ListItem>
            </asp:DropDownList>
        </p>
        <p>
            <table style="height: 449px; width: 756px">
                <tr>
                    <td>
                        Hotel 
                    </td>
                    <td>
                        Rail
                    </td>
                    <td class="style1">
                        Air
                    </td>
                    <td>
                       Boss
                    </td>
                </tr>
    <!--------->
    <tr>
                    <td>
               <div id='divHotel'   style="OVERFLOW: auto;WIDTH: 180px;HEIGHT: 355px"   onscroll="OnDivScroll('lstHotel');" >

                         <asp:ListBox ID="lstHotel" style="HEIGHT: 350px" runat="server"  onfocus="OnSelectFocus('divHotel','lstHotel');"   ></asp:ListBox>

                    </div>

                    </td>
                    <td>
                        <div id='divRail'   style="OVERFLOW: auto;WIDTH: 180px;HEIGHT: 355px"   onscroll="OnDivScroll('lstRail');" >
                        <asp:ListBox ID="lstRail" runat="server"  Height="350px" onfocus="OnSelectFocus('divRail','lstRail');"></asp:ListBox>
                  </div>
                    </td>
                    <td class="style1">
                        <div id='divAir'   style="OVERFLOW: auto;WIDTH: 180px;HEIGHT: 355px"   onscroll="OnDivScroll('lstAir');" >
                        <asp:ListBox ID="lstAir" runat="server"  Height="350px" onfocus="OnSelectFocus('divAir','lstAir');"   ></asp:ListBox>
                 </div>
                    </td>
                    <td>
                        <asp:ListBox ID="lstBoss" runat="server"  Height="350px" Width="600px"   ></asp:ListBox>
                    </td>
                </tr>



                  <tr>
                    <td class="style1">
                        <asp:ListBox ID="lstHotelSel" runat="server" Height="300px" Width="180px" 
                            BackColor="#99FF66"></asp:ListBox>
   
                       
                    </td>
                    <td class="style1">
                        <asp:ListBox ID="lstRailSel" runat="server"  Height="300px" Width="180px"  BackColor="#99FF66"></asp:ListBox>
 
                    </td>
                    <td class="style1">
                            <asp:ListBox ID="lstAirSel" runat="server"  Height="300px" Width="180px"  BackColor="#99FF66"></asp:ListBox>
      
                    </td>
                    <td>

                    </td>
                </tr>

            </table>
        </p>
    </asp:Panel>
    </div>
    </form>
</body>
</html>
