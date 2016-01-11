Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports EvoUtilities.CollectionUtils

Partial Public Class IAMIBuilder
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load")
            Try
                If Not IsPostBack Then
                    Dim strRet As String = setUser()
                    If strRet.StartsWith("ERROR") Then
                        Response.Redirect("IALogonAdmin.aspx?User=falseX")
                    End If
                    cexFrom.CssClass = "cal_Theme1"
                    cexFrom.Format = "dd/MM/yyyy"
                    cexTo.CssClass = "cal_Theme1"
                    cexTo.Format = "dd/MM/yyyy"

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    btnRail.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                    btnIntRail.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                    btnBedNights.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                    'default to last month
                    Dim dt As Date = Now.AddMonths(-1)
                    Dim dtfrom As New Date(dt.Year, dt.Month, 1)
                    txtfrom.Text = Format(dtfrom, "dd/MM/yyyy")
                    txtto.Text = Format(dtfrom.AddMonths(1).AddDays(-1), "dd/MM/yyyy")

                End If
                Me.Title = "Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAMIBuilder", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub


    Private Sub IAMIBuilder_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAMIBuilder_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAMIBuilder"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAMIBuilder", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnRail_Click(sender As Object, e As EventArgs) Handles btnRail.Click
        Using New clslogger(log, className, "btnRail_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please select a date range to continue.", "CheckDetails")
                End If

                buildRailFile(txtfrom.Text, txtto.Text)

            Catch ex As Exception
                handleexception(ex, "IAMIBuilder", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub buildRailFile(ByVal pstrStartdate As String, ByVal pstrEnddate As String)
        Using New clslogger(log, className, "buildRailFile")

            Dim oEverything As New List(Of RailBuilder)

            'first go get all sales
            oEverything = RailBuilder.railSalesWithEvolviRecords(pstrStartdate, pstrEnddate, "", "")
            ' need to go through and match up the journey legg stuff first

            Dim oJLs As New List(Of RailBuilder)
            oJLs = RailBuilder.railSalesJourneyLegs(pstrStartdate, pstrEnddate)

            For Each oSales As RailBuilder In oEverything
                For Each oJl As RailBuilder In oJLs
                    If oJl.Bookingref = oSales.Bookingref And oJl.TravellerFirstName = oSales.TravellerFirstName And oJl.TravellerLastName = oSales.TravellerLastName Then
                        If oSales.Singlereturn = "Single(Return)" And oJl.NumOccurrences = 2 Then
                            oSales.Journeyleg = "2 of " & CStr(oJl.NumOccurrences)
                        Else
                            oSales.Journeyleg = "1 of " & CStr(oJl.NumOccurrences)
                        End If
                        Exit For
                    End If
                Next
                If oSales.Singlereturn = "Single(Return)" And oSales.Journeyleg = "1 of 1" Then
                    oSales.Singlereturn = "Single"
                ElseIf oSales.Singlereturn = "Single(Outbound)" And oSales.Journeyleg = "1 of 1" Then
                    oSales.Singlereturn = "Single"
                End If
            Next

            ' now get sales records without evolvi records & add to Everything
            Dim oSales2 As New List(Of RailBuilder)
            oSales2 = RailBuilder.railSalesWithoutEvolviRecords(pstrStartdate, pstrEnddate, "", "")
            For Each oSale2 As RailBuilder In oSales2
                oEverything.Add(oSale2)
            Next

            ' now get credit records & add to Everything
            Dim oCredits As New List(Of RailBuilder)
            oCredits = RailBuilder.railCreditsWithEvolviRecords(pstrStartdate, pstrEnddate, "", "")
            For Each oCredit As RailBuilder In oCredits
                oEverything.Add(oCredit)
            Next

            ' now get credit records & add to Everything
            Dim oCredits2 As New List(Of RailBuilder)
            oCredits2 = RailBuilder.railCreditsWithNoRefundButSaleEvolviRecords(pstrStartdate, pstrEnddate, "", "")
            For Each oCredit2 As RailBuilder In oCredits2
                oEverything.Add(oCredit2)
            Next

            ' now get credit records & add to Everything
            Dim oCredits3 As New List(Of RailBuilder)
            oCredits3 = RailBuilder.railCreditsWithNonIssueEvolviRecords(pstrStartdate, pstrEnddate, "", "")
            For Each oCredit3 As RailBuilder In oCredits3
                oEverything.Add(oCredit3)
            Next

            ' now get credit records & add to Everything
            Dim oCredits4 As New List(Of RailBuilder)
            oCredits4 = RailBuilder.railCreditsWithoutEvolviRecords(pstrStartdate, pstrEnddate, "", "")
            For Each oCredit4 As RailBuilder In oCredits4
                oEverything.Add(oCredit4)
            Next

            'now sort the collections
            Dim Sorter As New Utility.Sorter(Of RailBuilder)

            Sorter.SortString = "Inm_custid,TravellerFirstName,TravellerLastName,Journeyleg"
            oEverything.Sort(Sorter)

            Dim csv As New StringBuilder
            csv.Append("Customer,Transaction Type,Booking Ref,Ticket Ref,Transaction Number,Machine,cost centre,cref1,cref2,purchase order,Booker Location," & _
                       "Booker Forename,Booker Surname,Passenger Title,Passenger Forename,Passenger Surname,division1,division2,division3,From,To,Route," & _
                       "Class of Travel,Ticket Type,Code,Ticket Type (full),Ticket Type (for analysis),Single / Return,Journey Leg,Train Operator,Date of Travel," & _
                       "Date of Transaction,Lead Time (Working Days),Reason for Lowest Fare Not Taken,Gross Accepted Fare,Discount,Fees,Net,Lowest Possible Fare," & _
                       "Fully Flexible Fare,Saving,Saving Declined,(EOE) Positive Savings,(EOE) Negative Savings,(EOE) Further Savings Declined,% Saving Made," & _
                       "% Overspend above Minimum,Distance (miles),Rail CO2 (kg),Journey Time,Invoice,Fulfilment Type,Booking Method,Reason For Travel," & _
                       "Internal / External Mail,Transaction Fee,Postal/Handling Charge,Commission Claimed" & vbNewLine)

            For Each oEv As RailBuilder In oEverything
                For Each s As String In makeList(oEv.Inm_custid, oEv.Transactiontype, oEv.Bookingref, oEv.Ticketref, oEv.Transactionnumber, oEv.Machine, _
                                                 oEv.Costcentre, oEv.Tot_cref1, oEv.Tot_cref2, oEv.Tot_pono, oEv.BookerLocation, oEv.BookingAgentFirstName, _
                                                 oEv.BookingAgentLastName, oEv.TravellerTitle, oEv.TravellerFirstName, oEv.TravellerLastName, _
                                                 oEv.Division1, oEv.Division2, oEv.Division3, oEv.OriginName, oEv.DestinationName, oEv.TicketRoute, _
                                                 oEv.TicketClass, oEv.Tickettype, oEv.TicketCode, oEv.TicketName, oEv.Tickettype, _
                                                 oEv.Singlereturn, oEv.Journeyleg, oEv.TOCName, oEv.Departure, oEv.TransactionDate, oEv.Leadtime, oEv.Lowestreason, _
                                                 oEv.Grossacceptedfare, oEv.Discount, oEv.Fees, oEv.Net, oEv.Lowestpossiblefare, oEv.FullyFlexibleFare, _
                                                 oEv.Saving, oEv.Savingdeclined, oEv.EOEPositiveSaving, oEv.EOENegativeSaving, oEv.EOEFurtherSavingsDeclined, _
                                                 oEv.Savingsperc, oEv.Overspend, oEv.Distance, oEv.Emissions, oEv.Journeytime, oEv.Inm_no, oEv.FulfilmentType, _
                                                 oEv.Bookingmethod, oEv.Reasonfortravel, oEv.Mail, oEv.Transfee, oEv.Postfee, oEv.Commission)
                    csv.Append(toCSVCell(s) & ",")
                Next
                csv.Append(vbNewLine)
            Next

            Dim strFileName As String = ""
            Dim dt As Date = CDate(txtfrom.Text)

            makeFolderExist(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy"))

            strFileName = "Rail-" & getMonthAsString(dt.Month) & "_" & CStr(dt.Year) & ".csv"

            Dim ofiler As New System.IO.StreamWriter(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

            ofiler.Write(csv.ToString)
            ofiler.Flush()
            ofiler.Close()

            hlRail.NavigateUrl = getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName
            hlRail.Visible = True
        End Using
    End Sub

    Protected Sub btnIntRail_Click(sender As Object, e As EventArgs) Handles btnIntRail.Click
        Using New clslogger(log, className, "btnIntRail_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please select a date range to continue.", "CheckDetails")
                End If

                buildIntRailFile(txtfrom.Text, txtto.Text)

            Catch ex As Exception
                handleexception(ex, "IAMIBuilder", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub buildIntRailFile(ByVal pstrStartdate As String, ByVal pstrEnddate As String)
        Using New clslogger(log, className, "buildIntRailFile")

            Dim oEverything As New List(Of RailBuilder)

            oEverything = RailBuilder.intRailSalesWithoutEvolviRecords(pstrStartdate, pstrEnddate, "", "")
           
            ' now get credit records & add to Everything
            Dim oCredits As New List(Of RailBuilder)
            oCredits = RailBuilder.intRailCreditsWithoutEvolviRecords(pstrStartdate, pstrEnddate, "", "")
            For Each oCredit As RailBuilder In oCredits
                oEverything.Add(oCredit)
            Next

            'now sort the collections
            Dim Sorter As New Utility.Sorter(Of RailBuilder)

            Sorter.SortString = "Inm_custid,TravellerFirstName,TravellerLastName,Journeyleg"
            oEverything.Sort(Sorter)

            Dim csv As New StringBuilder
            csv.Append("Customer,Transaction Type,Booking Ref,Ticket Ref,Transaction Number,Machine,cost centre,cref1,cref2,purchase order,Booker Location," & _
                       "Booker Forename,Booker Surname,Passenger Title,Passenger Forename,Passenger Surname,division1,division2,division3,From,To,Route," & _
                       "Class of Travel,Ticket Type,Code,Ticket Type (full),Ticket Type (for analysis),Single / Return,Journey Leg,Train Operator,Date of Travel," & _
                       "Date of Transaction,Lead Time (Working Days),Reason for Lowest Fare Not Taken,Gross Accepted Fare,Discount,Fees,Net,Lowest Possible Fare," & _
                       "Fully Flexible Fare,Saving,Saving Declined,(EOE) Positive Savings,(EOE) Negative Savings,(EOE) Further Savings Declined,% Saving Made," & _
                       "% Overspend above Minimum,Distance (miles),Rail CO2 (kg),Journey Time,Invoice,Fulfilment Type,Booking Method,Reason For Travel," & _
                       "Internal / External Mail,Transaction Fee,Postal/Handling Charge,Commission Claimed" & vbNewLine)

            For Each oEv As RailBuilder In oEverything
                For Each s As String In makeList(oEv.Inm_custid, oEv.Transactiontype, oEv.Bookingref, oEv.Ticketref, oEv.Transactionnumber, oEv.Machine, _
                                                 oEv.Costcentre, oEv.Tot_cref1, oEv.Tot_cref2, oEv.Tot_pono, oEv.BookerLocation, oEv.BookingAgentFirstName, _
                                                 oEv.BookingAgentLastName, oEv.TravellerTitle, oEv.TravellerFirstName, oEv.TravellerLastName, _
                                                 oEv.Division1, oEv.Division2, oEv.Division3, oEv.OriginName, oEv.DestinationName, oEv.TicketRoute, _
                                                 oEv.TicketClass, oEv.Tickettype, oEv.TicketCode, oEv.TicketName, oEv.Tickettype, _
                                                 oEv.Singlereturn, oEv.Journeyleg, oEv.TOCName, oEv.Departure, oEv.TransactionDate, oEv.Leadtime, oEv.Lowestreason, _
                                                 oEv.Grossacceptedfare, oEv.Discount, oEv.Fees, oEv.Net, oEv.Lowestpossiblefare, oEv.FullyFlexibleFare, _
                                                 oEv.Saving, oEv.Savingdeclined, oEv.EOEPositiveSaving, oEv.EOENegativeSaving, oEv.EOEFurtherSavingsDeclined, _
                                                 oEv.Savingsperc, oEv.Overspend, oEv.Distance, oEv.Emissions, oEv.Journeytime, oEv.Inm_no, oEv.FulfilmentType, _
                                                 oEv.Bookingmethod, oEv.Reasonfortravel, oEv.Mail, oEv.Transfee, oEv.Postfee, oEv.Commission)
                    csv.Append(toCSVCell(s) & ",")
                Next
                csv.Append(vbNewLine)
            Next


            Dim strFileName As String = ""
            Dim dt As Date = CDate(txtfrom.Text)

            makeFolderExist(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy"))

            strFileName = "IntRail-" & getMonthAsString(dt.Month) & "_" & CStr(dt.Year) & ".csv"

            Dim ofiler As New System.IO.StreamWriter(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

            ofiler.Write(csv.ToString)
            ofiler.Flush()
            ofiler.Close()

            hlIntRail.NavigateUrl = getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName
            hlIntRail.Visible = True
        End Using
    End Sub

    Protected Sub btnBedNights_Click(sender As Object, e As EventArgs) Handles btnBedNights.Click
        Using New clslogger(log, className, "btnBedNights_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please select a date range to continue.", "CheckDetails")
                End If

                bedNightsFile(txtfrom.Text, txtto.Text)

            Catch ex As Exception
                handleexception(ex, "IAMIBuilder", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub bedNightsFile(ByVal pstrStartdate As String, ByVal pstrEnddate As String)
        Using New clslogger(log, className, "bedNightsFile")

            Dim oBednights As New List(Of Bednight)

            'first go get all sales
            oBednights = Bednight.bedNightSalesWithCubitRecords(pstrStartdate, pstrEnddate)
            ' need to go through and match up the extras stuff first

            Dim oExtras As New List(Of Bednight)
            oExtras = Bednight.bedNightExtras(pstrStartdate, pstrEnddate)
            For Each oBednight As Bednight In oBednights
                For Each oExtra As Bednight In oExtras
                    If oExtra.Inm_ldname = oBednight.Inm_ldname And oExtra.Invoiceno = oBednight.Invoiceno And oExtra.transactionnumber = oBednight.Bookingref Then
                        oBednight.Extrasdetails = oBednight.Extrasdetails & oExtra.Extrasdetails & "; "
                    End If
                Next
            Next

            'do records without cubit records
            Dim oBednightsnoCubits As New List(Of Bednight)
            oBednightsnoCubits = Bednight.bedNightSalesWithOutCubitRecords(pstrStartdate, pstrEnddate)
            For Each oBednightsnoCubit As Bednight In oBednightsnoCubits
                oBednights.Add(oBednightsnoCubit)
            Next

            'do credits
            Dim oBedNightCredits As New List(Of Bednight)
            oBedNightCredits = Bednight.bedNightCredits(pstrStartdate, pstrEnddate)
            For Each oBedNightCredit As Bednight In oBedNightCredits
                oBednights.Add(oBedNightCredit)
            Next

            'do group bookings from Mevis
            Dim oBedNightGroups As New List(Of Bednight)
            oBedNightGroups = Bednight.bedNightGroups(pstrStartdate, pstrEnddate)
            For Each oBedNightGroup As Bednight In oBedNightGroups
                oBednights.Add(oBedNightGroup)
            Next

            'now sort the collections
            Dim Sorter As New Utility.Sorter(Of Bednight)

            Sorter.SortString = "Inm_custid,Invoiceno"
            oBednights.Sort(Sorter)

            Dim csv As New StringBuilder
            csv.Append("Customer,Traveller (BOSS),Traveller (Conferma),Booker (Conferma),Booking Date,cost centre,cref1,cref2,ref_3,ref_4,ref_5,purchase order,division1," & _
                       "division2,division3,Date of Arrival,Date of Departure,No. Nights,Hotel Booked (VDB),Hotel Booked (BOSS),Hotel Chain,Location (VDB),Location (BOSS)," & _
                       "Post Code,Room Type,Total Cost Ex VAT,Total Cost Inc VAT,Room Cost Ex VAT,Room Cost Inc VAT,Room Cost Per Night,Extras Cost Ex VAT,Extras Cost Inc VAT," & _
                       "Extra Items Description,Booking Fee (Invoice) Ex VAT,Booking Fee (Invoice) Inc VAT,Booking Fee (MI),Extras Fee (MI),Comparison Cost,Savings Made," & _
                       "Commission Claimed,Invoice,Booking Reference,Op,Online/Offline,Reason For Travel,Reason for Out of Policy,Invoice Date,Invoice Total (Inc Fees)," & _
                       "No. Travellers,Canx?,Cust Group (MI)" & vbNewLine)

            For Each oEv As Bednight In oBednights
                For Each s As String In makeList(oEv.Inm_custid, oEv.Inm_ldname, oEv.Passenger, oEv.Booker, oEv.BookedDate, oEv.Inm_costc, _
                                                 oEv.Tot_cref1, oEv.Tot_cref2, oEv.Ref_3, oEv.Ref_4, oEv.Ref_5, oEv.Tot_pono, _
                                                 oEv.Division1, oEv.Division2, oEv.Division3, oEv.Arrivaldate, _
                                                 oEv.Departuredate, oEv.Nights, oEv.Venuename, oEv.Sup_name, oEv.Hotelchain, oEv.To_name, _
                                                 oEv.Sup_add2, oEv.Postcode, oEv.Roomdetails, oEv.Nett, oEv.Gross, _
                                                 oEv.Roomnett, oEv.Roomgross, oEv.Roomcostpernight, oEv.Extrasnett, oEv.Extrasgross, oEv.Extrasdetails, oEv.Bookingfeenett, _
                                                 oEv.Bookingfeegross, oEv.Transfee, oEv.Extrastransfee, oEv.Comparison, oEv.Saving, oEv.Commission, _
                                                 oEv.Invoiceno, oEv.Bookingref, oEv.Bookerinitials, oEv.Bookingtype, oEv.AICol6, _
                                                 oEv.OutofPolicyReason, oEv.Inm_invdt, oEv.Totalincfees, oEv.Travellers, oEv.CancellationDate, oEv.ClientMI)
                    csv.Append(toCSVCell(s) & ",")
                Next
                csv.Append(vbNewLine)
            Next

            Dim strFileName As String = ""
            Dim dt As Date = CDate(txtfrom.Text)

            makeFolderExist(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy"))

            strFileName = "Bed-" & getMonthAsString(dt.Month) & "_" & CStr(dt.Year) & ".csv"

            Dim ofiler As New System.IO.StreamWriter(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

            ofiler.Write(csv.ToString)
            ofiler.Flush()
            ofiler.Close()

            hlbeds.NavigateUrl = getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName
            hlbeds.Visible = True
        End Using
    End Sub

    Protected Sub btnFlights_Click(sender As Object, e As EventArgs) Handles btnFlights.Click
        Using New clslogger(log, className, "btnFlights_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please select a date range to continue.", "CheckDetails")
                End If

                flightsFile(txtfrom.Text, txtto.Text)

            Catch ex As Exception
                handleexception(ex, "IAMIBuilder", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub flightsFile(ByVal pstrStartdate As String, ByVal pstrEnddate As String)
        Using New clslogger(log, className, "flightsFile")

            Dim oFlights As New List(Of FlightBuilder)

            oFlights = FlightBuilder.flightDataByTicket(pstrStartdate, pstrEnddate)
            
            Dim csv As New StringBuilder
            csv.Append("Customer,Booked Date,Traveller,Booker,Cost Centre,Cref1,Cref2,Ref_3,Ref_4,Ref_5,Ref_6,Ref_7,Ref_8,Ref_9,Fileno,PO,Division1,Division2,Division3," & _
                       "Departure Date,Supplier,Travel Class,Fare,Taxes,Fees,Billed,Comparison Fare,Saving,Save code,Routing,Fly From,Fly To,IATA From,IATA To,Total Miles," & _
                       "CO2,NYS Invoice Ref,Crsref,Etravel,Single/Return,International/Domestic,Ticket Ref,First Leg Miles,Reason Code,GroupCompany" & vbNewLine)

            For Each oFl As FlightBuilder In oFlights
                For Each s As String In makeList(oFl.Customer, oFl.Bookeddate, oFl.Traveller, oFl.Booker, oFl.Costcentre, oFl.Tot_cref1, oFl.Tot_cref2, oFl.Ref_3, oFl.Ref_4, _
                                          oFl.Ref_5, oFl.Ref_6, oFl.Ref_7, oFl.Ref_8, oFl.Ref_9, oFl.Tot_fileno, oFl.Tot_pono, oFl.Division1, oFl.Division2, oFl.Division3, _
                                          oFl.Departdate, oFl.Supplier, oFl.Travelclass, CStr(oFl.Fare), CStr(oFl.Taxes), CStr(oFl.Fees), CStr(oFl.Billed), CStr(oFl.Compfare), _
                                          CStr(oFl.Saving), oFl.Inm_savecd, oFl.Routing, oFl.Flyfrom, oFl.Flyto, oFl.Iatafrom, oFl.Iatato, CStr(oFl.TotalMiles), _
                                          CStr(oFl.Co2), oFl.Tot_invno, oFl.Tot_crsref, oFl.Etravel, _
                                          oFl.SingleReturn, oFl.IntDom, oFl.TicketRef, oFl.FirstLegMiles, oFl.Reasoncode, oFl.GroupCompany)
                    csv.Append(toCSVCell(s) & ",")
                Next
                csv.Append(vbNewLine)
            Next

            Dim strFileName As String = ""
            Dim dt As Date = CDate(txtfrom.Text)

            makeFolderExist(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy"))

            strFileName = "Flight-" & getMonthAsString(dt.Month) & "_" & CStr(dt.Year) & ".csv"

            Dim ofiler As New System.IO.StreamWriter(getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

            ofiler.Write(csv.ToString)
            ofiler.Flush()
            ofiler.Close()

            hlFlights.NavigateUrl = getConfig("MIFilePath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName
            hlFlights.Visible = True
        End Using
    End Sub
End Class