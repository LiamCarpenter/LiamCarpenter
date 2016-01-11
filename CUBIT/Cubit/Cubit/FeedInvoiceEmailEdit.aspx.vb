Option Explicit On
Option Strict On

Imports Cubit.NSConfigUtils
Imports DatabaseObjects
Imports System.Net.Mail

Partial Public Class FeedInvoiceEmailEdit
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Private mSent As Boolean = False

    Public Property Sent() As Boolean
        Get
            Return mSent
        End Get
        Set(ByVal value As Boolean)
            mSent = value
        End Set
    End Property

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load")
            Try
                If Not Me.IsPostBack Then
                    setuser()
                    Dim ocol As New System.Drawing.ColorConverter
                    Dim col As System.Drawing.Color
                    col = CType(ocol.ConvertFromString(getConfig("CPBackgroundColour")), Drawing.Color)
                    pnmain.BackColor = col

                    pnhide.Style.Item("Top") = "0px"
                    pnhide.Style.Item("Left") = "0px"
                    pnsure.Style.Item("Top") = "0px"
                    pnsure.Style.Item("Left") = "0px"
                    pnvenue.Style.Item("TOP") = "-30px"
                    pnvenue.Style.Item("LEFT") = "-14px"
                    'don't know why the grid won't work if nothing is 
                    'so added this to stop error on load
                    Dim tab As New Data.DataTable
                    grdVenue.DataSource = tab
                    grdVenue.DataBind()

                    Dim intID As Integer = 0
                    Try
                        intID = CInt(Request.QueryString("dataid"))
                    Catch ex As Exception
                        Throw New NYSFriendlyException("Data ID doesn't exist, please return to View screen and reselect", _
                                                       "Info")
                    End Try
                    If intID > 0 Then
                        populateData(intID)
                    Else
                        Throw New NYSFriendlyException("Data ID doesn't exist, please return to View screen and reselect", _
                                                                          "Info")
                    End If
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub setUser()
        Using New clslogger(log, className, "setUser")
            Dim oUser As clsSystemNYSUser
            oUser = CType(Session.Item("loggedinuser"), clsSystemNYSUser)
            If oUser IsNot Nothing Then
                If CBool(oUser.SystemnysuserInactive) = False Then
                    lbluser.Text = "Current user: " & oUser.Systemnysuserfirstname & " " & oUser.Systemnysuserlastname
                    lbluserid.Text = CStr(oUser.Systemnysuserid)
                Else
                    Response.Redirect("FeedMain.aspx")
                End If
            Else
                Response.Redirect("FeedMain.aspx")
            End If

        End Using
    End Sub

    Private Sub populateData(ByVal pintid As Integer)
        Using New clslogger(log, className, "populateData")
            Dim dr As clsFeedBookedData
            dr = clsFeedBookedData.InvoiceEmailGet(pintid)
            If dr IsNot Nothing Then
                txtdataid.Text = notString(dr.Dataid)
                txtbookingid.Text = notString(dr.BookingId)
                txtbookeddate.Text = Format(dr.BookedDate, "dd-MM-yyyy")
                txtarrivaldate.Text = Format(dr.ArrivalDate, "dd-MM-yyyy")
                txtdeparturedate.Text = Format(dr.DepartureDate, "dd-MM-yyyy")
                txttraveller.Text = notString(dr.LeadPassengerName)
                txtbookingref.Text = notString(dr.BookingReference)
                txttotal.Text = Format(dr.TotalBilledInGbp, "0.00")

                txtvenuedetails.Text = dr.HotelDetails.Replace("/", vbCrLf).Replace(";", vbCrLf)
                txtroomdetails.Text = dr.RoomDetails
                txtclientname.Text = notString(dr.Groupname)
                txtbooker.Text = dr.AIBkrName
                txtguestPNR.Text = dr.GuestPNR
                txtfail.Text = dr.unabletosendreason
                txtoriginalvenuename.Text = dr.SupplierName

                txtsupplerinvoice.Text = dr.supplierinvoice
                txtsupplerinvoice_hidden.Text = dr.supplierinvoice

                Dim strVenueName As String = dr.SupplierName
                Dim strAlternativeName As String = clsStuff.notString(clsVenue.venueAlternativenameCheck _
                                                                 (strVenueName))
                If strAlternativeName <> "" Then
                    strVenueName = strAlternativeName
                End If

                'now go and see if the name exists in VenuesDB if it does bring back values
                Dim venues As List(Of clsVenue)
                venues = clsVenue.venueExactNameFind(strVenueName.Replace("'", "''"), CStr(0))
                Dim strreason As String = ""
                Dim stremailto As String = ""
                txtvenuename.Text = strVenueName
                If venues.Count = 0 Then
                    txtVenuereference.Text = "0"
                    txtemail.Text = ""
                ElseIf venues.Count = 1 Then
                    For Each venue As clsVenue In venues
                        txtVenuereference.Text = CStr(venue.vereference)
                        txtemail.Text = clsFeedBookedData.venueInvoiceEmailGet(CStr(venue.vereference))
                        txtbosscode.Text = venue.bosscode
                    Next
                ElseIf venues.Count > 1 Then
                    'see if we can match the ID before we give up
                    Dim strAlternativeRef As String = clsStuff.notString(clsVenue.venueAlternativeRefCheck _
                                                                                 (dr.SupplierName))

                    If strAlternativeRef <> "" And strAlternativeRef <> "0" Then
                        For Each venue As clsVenue In venues
                            If venue.vereference = CDbl(strAlternativeRef) Then
                                txtemail.Text = clsFeedBookedData.venueInvoiceEmailGet(CStr(venue.vereference))
                                txtVenuereference.Text = CStr(venue.vereference)
                                Exit For
                            End If
                        Next
                    End If

                    If txtemail.Text = "" Then
                        'too many matches
                        txtVenuereference.Text = "0"
                    End If
                End If
            End If
        End Using
    End Sub

    Protected Sub btnfind_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnfind.Click
        Using New clslogger(log, className, "btnfind_Click")
            Try
                txtDatavenuenameFind.Text = txtvenuename.Text
                findVenue(Me.txtvenuename.Text, True)
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub findVenue(ByVal pstrvenuename As String, ByVal pblnfromedit As Boolean)
        Using New clslogger(log, className, "findVenue")
            If pblnfromedit Then
                Dim list As ICollection(Of String) = Split(pstrvenuename, " ")
                Dim strNamesToAvoid As String = getConfig("NamesToAvoid")
                For Each file As String In list
                    If txtDatavenuename1.Text = "" Then
                        If Not strNamesToAvoid.Contains(file.ToLower) Then
                            txtDatavenuename1.Text = file
                        End If
                    ElseIf txtDatavenuename2.Text = "" Then
                        If Not strNamesToAvoid.Contains(file.ToLower) Then
                            txtDatavenuename2.Text = file
                            Exit For
                        End If
                    End If
                Next
                pnvenue.Visible = True
            End If

            Dim tab As Data.DataTable
            tab = NSUtils.GetTable(clsVenue.Populate(Replace(txtDatavenuename1.Text, "'", "''"), Replace(txtDatavenuename2.Text, "'", "''"), _
                                                     btnAndOr.Text, CStr(0)))
            Dim dv As New Data.DataView(tab)
            If sortCriteria() IsNot Nothing AndAlso sortCriteria <> "" Then
                dv.Sort = sortCriteria & " " & sortDir
            Else
                dv.Sort = "vename"
            End If

            grdVenue.DataSource = dv
            grdVenue.DataBind()

            'set properties as VS doesn't like them in the designer
            grdVenue.RowClickEventCommandName = "select"
            Dim ocol As New System.Drawing.ColorConverter
            Dim col As System.Drawing.Color
            col = CType(ocol.ConvertFromString("#FF3300"), Drawing.Color)
            grdVenue.RowHighlightColor = col

        End Using
    End Sub

    ''' <summary>
    ''' Sub grdVenue_ItemCommand
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Captures grdVenue events:
    ''' 1. Sort command when user clicks headers
    ''' 2. Row selection command which sends user to edit screen with selected row dataid
    ''' </remarks>
    Private Sub grdVenue_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdVenue.ItemCommand
        Try
            If e.CommandName = "Sort" Then
                Return
            End If

            Select Case e.CommandName
                Case "select"
                    txtVenuereference.Text = e.Item.Cells(0).Text
                    txtVenuereference.Text = txtVenuereference.Text.Replace("&nbsp;", "")
                    txtvenuename.Text = e.Item.Cells(1).Text
                    txtvenuename.Text = txtvenuename.Text.Replace("&nbsp;", "")

                    txtbosscode.Text = e.Item.Cells(7).Text
                    txtbosscode.Text = txtbosscode.Text.Replace("&nbsp;", "")

                    txtDatavenuenameFind.Text = ""
                    txtDatavenuename1.Text = ""
                    txtDatavenuename2.Text = ""
                    txtemail.Text = clsFeedBookedData.venueInvoiceEmailGet(e.Item.Cells(0).Text)
                    pnvenue.Visible = False
                Case Else
                    Throw New Exception("Unrecognised command name: " & e.CommandName)
            End Select
        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedInvoiceEmailEdit", Page)
            End If
        End Try
    End Sub

    Private Sub grdVenue_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdVenue.SortCommand
        Using New clslogger(log, className, "grdVenue_SortCommand")
            Try
                If Me.sortCriteria = e.SortExpression AndAlso Me.sortDir = "asc" Then
                    Me.sortDir = "desc"
                Else
                    Me.sortDir = "asc"
                End If
                Me.sortCriteria = e.SortExpression
                findVenue("", False)
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ' Holds the column name to be sorted on...
    Public Property sortCriteria() As String
        Get
            Return CStr(ViewState("sortCriteriaEdit"))
        End Get
        Set(ByVal Value As String)
            ViewState("sortCriteriaEdit") = Value
        End Set
    End Property

    ' Holds the direction to be sorted...
    Public Property sortDir() As String
        Get
            Return CStr(ViewState("sortDirEdit"))
        End Get
        Set(ByVal Value As String)
            ViewState("sortDirEdit") = Value
        End Set
    End Property

    Protected Sub btnAndOr_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAndOr.Click
        Using New clslogger(log, className, "btnAndOr_Click")
            Try
                If btnAndOr.Text = "And" Then
                    btnAndOr.Text = "Or"
                Else
                    btnAndOr.Text = "And"
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnfind2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnfind2.Click
        Using New clslogger(log, className, "btnfind2_Click")
            Try
                If txtDatavenuename1.Text = "" And txtDatavenuename2.Text = "" Then
                    Throw New NYSFriendlyException("Please enter at least one value to search on!", "Info")
                Else
                    findVenue("", False)
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnnovenue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnnovenue.Click
        Using New clslogger(log, className, "btnnovenue_Click")
            Try
                txtVenuereference.Text = "0"
                txtDatavenuenameFind.Text = ""
                txtDatavenuename1.Text = ""
                txtDatavenuename2.Text = ""
                pnvenue.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnfindcancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnfindcancel.Click
        Using New clslogger(log, className, "btnfindcancel_Click")
            Try
                txtDatavenuenameFind.Text = ""
                txtDatavenuename1.Text = ""
                txtDatavenuename2.Text = ""
                pnvenue.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btncancel.Click
        Using New clslogger(log, className, "btncancel_Click")
            Try
                Response.Redirect("FeedInvoiceEmail.aspx?")
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnsave.Click
        Using New clslogger(log, className, "btnsave_Click")
            Try
                If txtsupplerinvoice_hidden.Text.ToUpper = "N" Then
                    If txtsupplerinvoice.Text <> txtsupplerinvoice_hidden.Text Then
                        Dim intRet As Integer = clsFeedBookedData.updateSupplierInvoice(CInt(txtdataid.Text), txtsupplerinvoice.Text)
                    End If
                Else
                    If txtemail.Text <> "" Then
                        Dim mRegExp As New Regex(emailRegex)
                        If Not mRegExp.IsMatch(txtemail.Text) Then
                            Throw New NYSFriendlyException("Email address is not in the correct format, please amend.", "Info")
                        End If
                    End If
                    If txtemail.Text <> "" And txtVenuereference.Text <> "0" Then
                        If clsFeedBookedData.venueAccountsEmailSave(txtemail.Text, CInt(txtVenuereference.Text), CInt(getConfig("DBType"))) > 0 Then
                            'save values to alternate table so can use later
                            If txtoriginalvenuename.Text <> txtvenuename.Text Then
                                If clsVenue.saveInvoiceAlternate(CInt(txtVenuereference.Text), txtvenuename.Text, _
                                                 txtoriginalvenuename.Text, txtbosscode.Text, _
                                                 CInt(lbluserid.Text)) > 0 Then
                                    'cool
                                Else
                                    log.Error(txtvenuename.Text & " did not save to database")
                                End If
                            End If
                            'now try and send
                            tryandsend(txtemail.Text, txtVenuereference.Text)
                            Response.Redirect("FeedInvoiceEmail.aspx?")
                        Else
                            Throw New NYSFriendlyException("Details did not save correctly, please try again.", "Info")
                        End If
                    Else
                        If txtemail.Text = "" And txtVenuereference.Text <> "0" Then
                            Throw New NYSFriendlyException("An accounts email address must be added to the matched venue, either add an email address or cancel.", "Info")
                        Else
                            Throw New NYSFriendlyException("The venue needs to be matched to one in VenuesDB, please add venue first then try to rematch.", "Info")
                        End If
                    End If
                End If

            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmailEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub tryandsend(ByVal pstrEmail As String, ByVal pstrRef As String)
        Using New clslogger(log, className, "tryandsend")

            'need to get all other records where the venue matches and send them too
            Dim exs As List(Of clsFeedBookedData)
            exs = clsFeedBookedData.missingInvoiceEmailNewemail(txtoriginalvenuename.Text) '.Replace("'", "''"))

            Dim strCurrentSupplier As String = ""
            Dim strLastSupplier As String = ""
            Dim strreason As String = ""
            Dim strMessage As String = ""
            Dim intDataID As Integer = 0

            For Each ex As clsFeedBookedData In exs
                'try and match venue name with venuesdb
                intDataID = ex.Dataid

                strCurrentSupplier = ex.SupplierName
                strreason = ""
                If strLastSupplier = "" Then
                    strLastSupplier = strCurrentSupplier
                End If

                If strCurrentSupplier <> strLastSupplier Then
                    'do send 
                    Dim strFinalMessage As String = readText(Server.MapPath("docs/InvoiceMain.htm"))
                    strFinalMessage = strFinalMessage.Replace("#body#", strMessage)

                    Dim strResult As String = sendemail(pstrEmail, getConfig("EmailFrom"), "Urgent request for invoice/s", strFinalMessage)

                    If strResult <> "" Then
                        Dim intRet1 As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, True, True, strResult, "")
                    Else
                        Dim intRet2 As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, False, False, "", Format(Now, "dd/MM/yyyy"))
                    End If
                    strMessage = ""
                End If
                
                Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, True, True, strreason, "")
                'build the message
                strMessage = strMessage & readText(Server.MapPath("docs/InvoiceBody.htm"))
                strMessage = strMessage.Replace("#traveller#", ex.LeadPassengerName)
                strMessage = strMessage.Replace("#venuename#", strCurrentSupplier)
                strMessage = strMessage.Replace("#arrivaldate#", Format(ex.ArrivalDate, "dd/MM/yyyy"))
                strMessage = strMessage.Replace("#departuredate#", Format(ex.DepartureDate, "dd/MM/yyyy"))
                strMessage = strMessage.Replace("#bookingreference#", ex.BookingReference)
                strMessage = strMessage.Replace("#guestpnr#", ex.GuestPNR)
                strMessage = strMessage.Replace("#conferma#", CStr(ex.BookingId))
                strMessage = strMessage.Replace("#lastfour#", ex.Last4Digits)
                strMessage = strMessage.Replace("#totalcost#", "£" & Format(ex.TotalBilledInGbp, "0.00"))
                'strMessage = strMessage & "<br>"
                'strMessage = strMessage.Replace("#clientname#", ex.Groupname)
                strLastSupplier = ex.SupplierName
            Next

            'need to do a finally for last record
            If exs.Count > 0 Then
                Dim strFinalMessage As String = readText(Server.MapPath("docs/InvoiceMain.htm"))
                strFinalMessage = strFinalMessage.Replace("#body#", strMessage)

                Dim strResult As String = sendemail(pstrEmail, getConfig("EmailFrom"), "Urgent request for invoice/s", strFinalMessage)

                For Each ex As clsFeedBookedData In exs
                    If strResult <> "" Then
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(intDataID, True, True, strResult, "")
                    Else
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, False, False, "", Format(Now, "dd/MM/yyyy"))
                    End If
                Next
            End If
        End Using
    End Sub
End Class