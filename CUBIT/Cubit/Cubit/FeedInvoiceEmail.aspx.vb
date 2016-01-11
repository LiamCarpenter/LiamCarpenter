Imports DatabaseObjects
Imports EvoUtilities.ConfigUtils

Partial Public Class FeedInvoiceEmail
    Inherits clsNYS

    Private Shared ReadOnly className As String

    ''' <summary>
    ''' Sub New
    ''' </summary>
    ''' <remarks>
    ''' Sets up page for logging
    ''' </remarks>
    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load")
            Try
                If Not Me.IsPostBack Then

                    If Not Session.Item("EmailsortCriteria") Is Nothing Then
                        Me.sortCriteria = CStr(Session.Item("EmailsortCriteria"))
                    End If
                    If Not Session.Item("EmailsortDir") Is Nothing Then
                        Me.sortDir = CStr(Session.Item("EmailsortDir"))
                    End If

                    setUser()
                    binddata()

                    If Session.Item("scrolltop") IsNot Nothing And _
                        Session.Item("scrollleft") IsNot Nothing Then
                        Dim a As New System.Drawing.Point(CInt(Session.Item("scrollleft")), CInt(Session.Item("scrolltop")))
                        pndata.StartScrollPos = a
                    End If
                Else
                    'store scroll position for later use
                    If txtscrolltop.Text <> "" Then
                        Session.Item("scrolltop") = txtscrolltop.Text
                    End If
                    If txtscrollleft.Text <> "" Then
                        Session.Item("scrollleft") = txtscrollleft.Text
                    End If
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmail", Page)
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

    Protected Sub btnSendUnsent_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendUnsent.Click
        Using New clslogger(log, className, "btnSendUnsent_Click")
            Try
                'first do ones that haven't ever been sent
                Dim intInitial As Integer = constructInvoiceEmails(1)
                Session.Item("InitialSuccess") = intInitial
                'now do ones that have been sent previously but still nothing
                Dim intRepeat As Integer = constructInvoiceEmails(2)
                'now sent all ones that can be sent, show ones that couldn't
                binddata()
                Throw New NYSFriendlyException("Successful initial emails sent: " & CStr(Session.Item("InitialSuccess")) & "<br>" & _
                                               "Successful repeat emails sent: " & intRepeat, "Info")
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmail", Page)
                End If
            End Try
        End Using
    End Sub

    Public Property sortCriteria() As String
        Get
            Return CStr(ViewState("sortCriteria"))
        End Get
        Set(ByVal Value As String)
            ViewState("sortCriteria") = Value
        End Set
    End Property

    Public Property sortDir() As String
        Get
            Return CStr(ViewState("sortDir"))
        End Get
        Set(ByVal Value As String)
            ViewState("sortDir") = Value
        End Set
    End Property

    Private Sub binddata()
        Using New clslogger(log, className, "binddata")
            Dim tab As New Data.DataTable
            Dim dv As Data.DataView
            tab = NSUtils.GetTable(clsFeedBookedData.unsentInvoiceEmailList)

            dv = New Data.DataView(tab)

            If sortCriteria() IsNot Nothing AndAlso sortCriteria <> "" Then
                dv.Sort = sortCriteria & " " & sortDir
            Else
                dv.Sort = "SupplierName"
            End If

            grdData.DataSource = dv
            grdData.DataBind()

            'set properties as VS doesn't like them in the designer
            grdData.RowClickEventCommandName = "edit"
            Dim ocol As New System.Drawing.ColorConverter
            Dim col As System.Drawing.Color
            col = CType(ocol.ConvertFromString("#FF3300"), Drawing.Color)
            grdData.RowHighlightColor = col
            lblcount.Text = "Record count = " & CStr(dv.Count)
            pndata.Visible = True
        End Using
    End Sub

    Private Sub grdData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdData.ItemCommand
        Try
            If e.CommandName = "Sort" Then
                Return
            End If

            Select Case e.CommandName
                Case "edit"
                    Session.Item("EmailsortCriteria") = ViewState("sortCriteria")
                    Session.Item("EmailsortDir") = ViewState("sortDir")
                    Response.Redirect("FeedInvoiceEmailEdit.aspx?dataid=" & e.Item.Cells(0).Text)
                Case Else
                    Throw New Exception("Unrecognised command name: " & e.CommandName)
            End Select
        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedInvoiceEmail", Page)
            End If
        End Try
    End Sub

    Private Sub grdData_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdData.SortCommand
        Try
            If Me.sortCriteria = e.SortExpression AndAlso Me.sortDir = "asc" Then
                Me.sortDir = "desc"
            Else
                Me.sortDir = "asc"
            End If
            Me.sortCriteria = e.SortExpression

            binddata

        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedInvoiceEmail", Page)
            End If
        End Try
    End Sub

    Private Function constructInvoiceEmails(ByVal ptype As Integer) As Integer
        Using New clslogger(log, className, "constructInvoiceEmails")
            Dim exs As New List(Of clsFeedBookedData)
            Dim intcount As Integer = 0

            If ptype = 1 Then
                exs = clsFeedBookedData.initialInvoiceEmailList(CInt(getConfig("InitialDays")))
            ElseIf ptype = 2 Then
                exs = clsFeedBookedData.repeatInvoiceEmailList(CInt(getConfig("RepeatDays")))
            ElseIf ptype = 3 Then
                exs = clsFeedBookedData.missingInvoiceEmailCheckandsend
            End If

            Dim strCurrentSupplier As String = ""
            Dim strLastSupplier As String = ""
            Dim strreason As String = ""
            Dim stremailto As String = ""
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
                    If stremailto <> "" Then
                        Dim strFinalMessage As String = readText(Server.MapPath("docs/InvoiceMain.htm"))
                        strFinalMessage = strFinalMessage.Replace("#body#", strMessage)

                        Dim strResult As String = sendemail(stremailto, getConfig("EmailFrom"), "Urgent request for invoice/s", strFinalMessage)

                        If strResult <> "" Then
                            Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, True, True, strResult, "")
                        Else
                            Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, False, False, "", Format(Now, "dd/MM/yyyy"))
                            intcount += 1
                        End If
                    End If
                    stremailto = ""
                    strMessage = ""
                End If
                Dim strAlternativeName As String = clsStuff.notString(clsVenue.venueAlternativenameCheck _
                                                                                 (strCurrentSupplier))
                If strAlternativeName <> "" Then
                    strCurrentSupplier = strAlternativeName
                End If

                'now go and see if the name exists in VenuesDB if it does bring back values
                Dim venues As List(Of clsVenue)
                venues = clsVenue.venueExactNameFind(strCurrentSupplier.Replace("'", "''''"), CStr(0))

                If venues.Count = 0 Then
                    strreason = "No match to any venue"

                ElseIf venues.Count = 1 Then
                    strreason = ""
                    For Each venue As clsVenue In venues
                        stremailto = clsFeedBookedData.venueInvoiceEmailGet(CStr(venue.vereference))
                    Next
                    'check email format
                    If stremailto <> "" Then
                        Dim mRegExp As New Regex(emailRegex)
                        If Not mRegExp.IsMatch(stremailto) Then
                            strreason = "Email address in not in the correct format"
                            stremailto = ""
                        End If
                    Else
                        strreason = "No email address present for venue"
                    End If
                ElseIf venues.Count > 1 Then

                    'see if we can match the ID before we give up
                    Dim strAlternativeRef As String = clsStuff.notString(clsVenue.venueAlternativeRefCheck _
                                                                                 (ex.SupplierName))

                    If strAlternativeRef <> "" And strAlternativeRef <> "0" Then
                        For Each venue As clsVenue In venues
                            If venue.vereference = CDbl(strAlternativeRef) Then
                                stremailto = clsFeedBookedData.venueInvoiceEmailGet(CStr(venue.vereference))
                                Exit For
                            End If
                        Next
                    End If

                    If stremailto = "" Then
                        'too many matches
                        strreason = "Venue name has matched to more than one venue: refs="
                        For Each venue As clsVenue In venues
                            strreason = strreason & venue.vereference & ";"
                        Next
                    End If
                End If
                If strreason <> "" Then
                    'save reason to db record
                    Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, True, True, strreason, "")
                Else
                    Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, False, False, "", Format(Now, "dd/MM/yyyy"))
                End If
                If stremailto <> "" Then
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
                End If

                strLastSupplier = ex.SupplierName
            Next

            'need to do a finally for last record
            If exs.Count > 0 Then
                If stremailto <> "" Then
                    Dim strFinalMessage As String = readText(Server.MapPath("docs/InvoiceMain.htm"))
                    strFinalMessage = strFinalMessage.Replace("#body#", strMessage)

                    Dim strResult As String = sendemail(stremailto, getConfig("EmailFrom"), "Urgent request for invoice/s", strFinalMessage)

                    If strResult <> "" Then
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(intDataID, True, True, strResult, "")
                    Else
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(intDataID, False, False, "", Format(Now, "dd/MM/yyyy"))
                        intcount += 1
                    End If
                End If
            End If

            Return intcount
        End Using
    End Function

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnclose.Click
        Using New clslogger(log, className, "btnclose_Click")
            Session.Remove("EmailsortCriteria")
            Session.Remove("EmailsortDir")
            Response.Redirect("FeedAdmin.aspx")
        End Using
    End Sub

    Protected Sub btnCheckAndSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCheckAndSend.Click
        Using New clslogger(log, className, "btnCheckAndSend_Click")
            Try
                'first do ones that haven't ever been sent
                Dim intCount As Integer = constructInvoiceEmails(3)
                binddata()
                Throw New NYSFriendlyException("Successful emails sent: " & intCount, "Info")
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedInvoiceEmail", Page)
                End If
            End Try
        End Using
    End Sub
End Class