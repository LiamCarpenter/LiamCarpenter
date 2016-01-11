Option Strict On
Option Explicit On

Imports Cubit.NSConfigUtils
Imports ReportDownloader
Imports DatabaseObjects
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Globalization
Imports EvoUtilities.ConfigUtils
Imports ReportDownloader.Utility
Imports System.Math
Imports System.Linq
Imports System.Data.Linq
Imports System.Xml.Linq

''' <summary>
''' Public Class FeedMain
''' </summary>
''' <remarks>
''' Created 12/03/2009 Nick Massarella
''' Main page class
''' </remarks>
Partial Public Class FeedMain
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

    ''' <summary>
    ''' Sub Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Page load for FeedMain
    ''' 1. tests for encrypted connection string and encrypts if Encrypt 
    ''' key = true and no already encrypted
    ''' 2. Checks users windows login against database to see if allowed to use application
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Write("Page Loading")

        Using New clslogger(log, className, "Page_Load")

            Try
                If Not Me.IsPostBack Then

                    If Not Session.Item("MainsortCriteria") Is Nothing Then
                        Me.sortCriteria = CStr(Session.Item("MainsortCriteria"))
                    End If
                    If Not Session.Item("MainsortDir") Is Nothing Then
                        Me.sortDir = CStr(Session.Item("MainsortDir"))
                    End If

                    'R2.11 SA
                    If Not Session.Item("LastExportResult") Is Nothing Then
                        txtexportresult.Text = CStr(Session.Item("LastExportResult"))
                    End If

                    setPanels()

                    Dim ocol As New System.Drawing.ColorConverter
                    Dim col As System.Drawing.Color
                    col = CType(ocol.ConvertFromString(getConfig("CPBackgroundColour")), Drawing.Color)
                    pnmain.BackColor = col

                    'When the config value is set to true the connection string in the config file will be encrypted
                    If getConfig("Encrypt") = "true" Then
                        clsEncryption.EncryptConnStr("DataProtectionConfigurationProvider")
                    End If

                    'uncomment next line to decrypt connection string
                    'clsEncryption.DecryptConnStr()

                    lblversion.Text = "Cubit " & getConfig("Version")

                    checkLoginstatus(Mid(Request.ServerVariables("LOGON_USER"), _
                                        InStr(Request.ServerVariables("LOGON_USER"), "\") + 1))
                    setUser()
                    'set ajax properties as this crappy solutions will not allow in the designer
                    setAjax()
                    Dim strdataid As String = ""
                    Dim intgroupid As Integer = 0

                    If Request.QueryString("dataid") IsNot Nothing Then
                        strdataid = Request.QueryString("dataid")
                    End If

                    Dim intstatus As Integer = 0

                    If CBool(getConfig("Testing")) = True Then
                        btnTestFileImport.Visible = True
                        'R2.21.6 SA 
                        btnTestFileImportBooked.Visible = True
                        btnTestFileImporTrans.Visible = True
                    End If

                    If Session.Item("groupid") IsNot Nothing Then
                        If Session.Item("search") IsNot Nothing Then
                            If CStr(Session.Item("search")) = "true" Then
                                pnsearch.Visible = True
                                If Session.Item("venuename") IsNot Nothing Then
                                    txtname.Text = CStr(Session.Item("venuename"))
                                End If
                                If Session.Item("invoiceno") IsNot Nothing Then
                                    txtinvoice.Text = CStr(Session.Item("invoiceno"))
                                End If
                                If Session.Item("guestpnr") IsNot Nothing Then
                                    txtguestpnr.Text = CStr(Session.Item("guestpnr"))
                                End If
                                If Session.Item("guestname") IsNot Nothing Then
                                    txtguestname.Text = CStr(Session.Item("guestname"))
                                End If
                                If Session.Item("status") IsNot Nothing Then
                                    If ddstatus.Items.Count = 0 Then
                                        populateStatus()
                                    Else
                                        changeDropDowns(ddstatus, "", True, False)
                                    End If
                                    changeDropDowns(ddstatus, CStr(Session.Item("status")), False, False)
                                    intstatus = CInt(ddstatus.SelectedItem.Value)
                                End If
                            Else
                                btnusesearch.Visible = True
                                intstatus = FeedStatus.getStatusID("Feed Import OK")
                            End If
                        Else
                            btnusesearch.Visible = True
                            intstatus = FeedStatus.getStatusID("Feed Import OK")
                        End If
                        populateData(CInt(Session.Item("groupid")), strdataid, txtname.Text, txtinvoice.Text, _
                             txtguestpnr.Text, txtguestname.Text, intstatus)
                        intgroupid = CInt(Session.Item("groupid"))
                    Else
                        populateData(0, "", "", "", "", "", 0)
                    End If

                    populateGroups(intgroupid)

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
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub setAjax()
        Using New clslogger(log, className, "setAjax")
            extVenueName.EnableCaching = True
            extVenueName.CompletionListCssClass = "list2"
            extVenueName.CompletionListHighlightedItemCssClass = "hoverlistitem2"
            extVenueName.CompletionListItemCssClass = "listitem2"
            extVenueName.ServiceMethod = "venueNameSearch"
            extVenueName.ServicePath = "AjaxService.asmx"
            extVenueName.MinimumPrefixLength = 2

            extInvoice.EnableCaching = True
            extInvoice.CompletionListCssClass = "list2"
            extInvoice.CompletionListHighlightedItemCssClass = "hoverlistitem2"
            extInvoice.CompletionListItemCssClass = "listitem2"
            extInvoice.ServiceMethod = "venueInvoiceSearch"
            extInvoice.ServicePath = "AjaxService.asmx"
            extInvoice.MinimumPrefixLength = 1

            extguestname.EnableCaching = True
            extguestname.CompletionListCssClass = "list2"
            extguestname.CompletionListHighlightedItemCssClass = "hoverlistitem2"
            extguestname.CompletionListItemCssClass = "listitem2"
            extguestname.ServiceMethod = "guestNameSearch"
            extguestname.ServicePath = "AjaxService.asmx"
            extguestname.MinimumPrefixLength = 1

            extguestPNRSearch.EnableCaching = True
            extguestPNRSearch.CompletionListCssClass = "list2"
            extguestPNRSearch.CompletionListHighlightedItemCssClass = "hoverlistitem2"
            extguestPNRSearch.CompletionListItemCssClass = "listitem2"
            extguestPNRSearch.ServiceMethod = "guestPNRSearch"
            extguestPNRSearch.ServicePath = "AjaxService.asmx"
            extguestPNRSearch.MinimumPrefixLength = 1
        End Using
    End Sub

    ''' <summary>
    ''' Positions various panels to correct location so when they are made visible the 
    ''' appear in the correct place
    ''' </summary>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' </remarks>
    ''' 
    Private Sub setPanels()
        Using New clslogger(log, className, "setPanels")
            pnsearch.Style.Item("Top") = "24px"
            pnsearch.Style.Item("Left") = "4px"
            pnsearch.Style.Item("width") = "982px"
            pnsure.Style.Item("Top") = "0px"
            pnsure.Style.Item("Left") = "0px"
            'R2.21.4 SA 
            pnconfirm.Style.Item("Top") = "8px"
            pnconfirm.Style.Item("Left") = "8px"
        End Using
    End Sub

    ''' <summary>
    ''' Gets/sets the column name for grdData to be sorted on
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created 27/03/2009 Nick Massarella
    ''' </remarks>
    Public Property sortCriteria() As String
        Get
            Return CStr(ViewState("sortCriteria"))
        End Get
        Set(ByVal Value As String)
            ViewState("sortCriteria") = Value
        End Set
    End Property

    ''' <summary>
    ''' Property sortDir
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created 27/03/2009 Nick Massarella
    ''' Gets/sets the sort direction for grdData to be sorted on
    ''' </remarks>
    Public Property sortDir() As String
        Get
            Return CStr(ViewState("sortDir"))
        End Get
        Set(ByVal Value As String)
            ViewState("sortDir") = Value
        End Set
    End Property

    ''' <summary>
    ''' Sub bind
    ''' </summary>
    ''' <param name="pintgroupid"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Populate datagrid with outstanding transaction records for selected group
    ''' </remarks>
    Private Sub populateData(ByVal pintgroupid As Integer, ByVal pstrdataid As String, _
                    ByVal pstrvenuename As String, ByVal pstrinvoiceno As String, _
                    ByVal pstrguestpnr As String, ByVal pstrguestname As String, _
                    ByVal pintstatus As Integer)
        Dim tab As New Data.DataTable
        Dim dv As Data.DataView
        If pintgroupid > 0 Then
            tab = NSUtils.GetTable(FeedImportData.list(pintgroupid, pstrvenuename, pstrinvoiceno, _
                                                        pstrguestpnr, pstrguestname, pintstatus))
        Else
            dv = New Data.DataView(tab)
            grdData.DataSource = dv
            grdData.DataBind()
            lblcount.Text = ""
            Exit Sub
        End If

        dv = New Data.DataView(tab)

        If sortCriteria() IsNot Nothing AndAlso sortCriteria <> "" Then
            dv.Sort = sortCriteria & " " & sortDir
        End If

        grdData.DataSource = dv
        grdData.DataBind()

        'set properties as VS doesn't like them in the designer
        'grdData.RowClickEventCommandName = "edit" 'R2.21.4 SA commented out 
        Dim ocol As New System.Drawing.ColorConverter
        Dim col As System.Drawing.Color
        col = CType(ocol.ConvertFromString("#FF3300"), Drawing.Color)
        grdData.RowHighlightColor = col
        lblcount.Text = "Record count = " & CStr(dv.Count)

        'only show if there are rows to export
        If dv.Count > 0 And pnsearch.Visible = False Then
            btnexport.Visible = True
        Else
            btnexport.Visible = False
        End If

        'select correct row if user was editing
        Dim firstid As String = ""
        Dim lastid As String = ""
        Dim intcount As Integer = 0
        Dim blnitem As Boolean = True
        For Each dgiCount As DataGridItem In grdData.Items
            If pstrdataid <> "" Then
                If grdData.Items(dgiCount.ItemIndex).Cells(0).Text = pstrdataid Then
                    grdData.SelectedIndex = dgiCount.ItemIndex
                End If
            End If
            'R2.21.4 SA - incremented cell numbers by one, as new column is added at index 1
            If grdData.Items(dgiCount.ItemIndex).Cells(33).Text = "AC" Then
                grdData.Items(dgiCount.ItemIndex).Cells(6).Text = _
                grdData.Items(dgiCount.ItemIndex).Cells(39).Text
            Else
                grdData.Items(dgiCount.ItemIndex).Cells(6).Text = _
                grdData.Items(dgiCount.ItemIndex).Cells(38).Text
            End If

            'R2.21.4 SA - incremented cell numbers by one, as new column is added at index 1
            'now try and highlight rows in groups of transactions
            firstid = grdData.Items(dgiCount.ItemIndex).Cells(2).Text

            If intcount = 0 Then
                grdData.Items(dgiCount.ItemIndex).CssClass = "gridItem"
            Else
                If firstid = lastid Then
                    If blnitem Then
                        grdData.Items(dgiCount.ItemIndex).CssClass = "gridItem"
                    Else
                        grdData.Items(dgiCount.ItemIndex).CssClass = "gridAlternate"
                    End If
                Else
                    blnitem = Not blnitem
                    If blnitem Then
                        grdData.Items(dgiCount.ItemIndex).CssClass = "gridItem"
                    Else
                        grdData.Items(dgiCount.ItemIndex).CssClass = "gridAlternate"
                    End If
                End If
            End If
            'R2.21.4 SA - incremented cell numbers by one, as new column is added at index 1
            If grdData.Items(dgiCount.ItemIndex).Cells(47).Text <> "GBP" Then
                grdData.Items(dgiCount.ItemIndex).CssClass = "gridCurrency"
            End If
            'R2.21.4 SA - incremented cell numbers by one,as new column is added at index 1
            If grdData.Items(dgiCount.ItemIndex).Cells(6).Text = "0" Then
                If Not grdData.Items(dgiCount.ItemIndex).Cells(12).Text.ToLower.Contains("premier inn") And _
                    Not grdData.Items(dgiCount.ItemIndex).Cells(12).Text.ToLower.Contains("travelodge") Then
                    grdData.Items(dgiCount.ItemIndex).CssClass = "gridZero"
                End If
            End If
            'R2.21.4 SA - incremented cell numbers by one,as new column is added at index 1
            lastid = grdData.Items(dgiCount.ItemIndex).Cells(2).Text
            intcount += 1
            'R2.21.4 SA - incremented cell numbers by one, as new column is added at index 1
            'R2.6 NM
            If grdData.Items(dgiCount.ItemIndex).Cells(48).Text.ToUpper = "TRUE" Then
                CType(dgiCount.FindControl("chkExcludeFromExport"), CheckBox).Checked = True
            Else
                CType(dgiCount.FindControl("chkExcludeFromExport"), CheckBox).Checked = False
            End If

            'AM
            If grdData.Items(dgiCount.ItemIndex).Cells(26).Text.Substring(0, 1) = "-" Then
                grdData.Items(dgiCount.ItemIndex).CssClass = "gridREFUND"
                'Avoid automatic check  as data persistent issues
                'Dim tn As String = grdData.Items(dgiCount.ItemIndex).Cells(0).Text
                'FeedImportData.saveExcludeFromExport(CStr(Convert.ToInt32(tn)), True)
                'CType(dgiCount.FindControl("chkExcludeFromExport"), CheckBox).Checked = True
            End If

            'Add a BookingOnly Check here
            Dim TempDataID = CDbl(grdData.Items(dgiCount.ItemIndex).Cells(0).Text)

            Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                Dim EFBookingOnly = (From x In Cubit.FeedImportData Where x.dataid = TempDataID Select x.BookingOnlyEF).FirstOrDefault

                If EFBookingOnly = True Then
                    grdData.Items(dgiCount.ItemIndex).CssClass = "gridBookingOnly"
                End If

            End Using


        Next

    End Sub

    Private Sub setUser()
        Using New clslogger(log, className, "setUser")

            Dim oUser As clsSystemNYSUser
            oUser = CType(Session.Item("loggedinuser"), clsSystemNYSUser)

            If oUser IsNot Nothing Then
                If CBool(oUser.SystemnysuserInactive) = False Then
                    lbluser.Text = "Current user: " & oUser.Systemnysuserfirstname & " " & oUser.Systemnysuserlastname
                    lbluserid.Text = CStr(oUser.Systemnysuserid)
                    lbluserinitials.Text = oUser.Systemnysuserinitials
                    If oUser.Systemnysusergroup.ToUpper = "ADMIN" Then
                        btnadmin.Visible = True
                    Else
                        btnadmin.Visible = False
                    End If

                    'R2.21.4 SA - display delete command *** HARDCODED !!!!
                    If oUser.Systemnysuserlastname.ToLower = "weeks" Or _
                        oUser.Systemnysuserlastname.ToLower = "keller" Or _
                        oUser.Systemnysuserlastname.ToLower = "marron" Or _
                        oUser.Systemnysuserlastname.ToLower = "johnson" Or _
                        oUser.Systemnysuserlastname.ToLower = "devlin" Then
                        grdData.Columns(56).Visible = True
                    Else
                        grdData.Columns(56).Visible = False
                    End If

                    If getConfig("Testing") = "true" Then
                        btnTestFileImport.Visible = True
                        btnTestDelete.Visible = True
                        btnTestFileDownload.Visible = True
                        'R2.21.6 SA 
                        btnTestFileImportBooked.Visible = True
                        btnTestFileImporTrans.Visible = True
                    Else
                        btnTestFileImport.Visible = False
                        btnTestDelete.Visible = False
                        btnTestFileDownload.Visible = False
                        'R2.21.6 SA 
                        btnTestFileImportBooked.Visible = False
                        btnTestFileImporTrans.Visible = False
                    End If
                Else
                    lbluser.Text = Request.ServerVariables("LOGON_USER") & " user account is inactive, please contact the system administrator."
                End If
            Else
                lbluser.Text = Request.ServerVariables("LOGON_USER") & " is not a registered user of this system, please contact the system administrator."
            End If

            log.Info(lbluser.Text)

        End Using
    End Sub

    ''' <summary>
    ''' Sub checkLoginstatus
    ''' </summary>
    ''' <param name="pstrUserName"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Check users windows login against database
    ''' </remarks>
    Private Sub checkLoginstatus(ByVal pstrUserName As String)
        Using New clslogger(log, className, "checkLoginstatus")
            Dim oUsers As List(Of clsSystemNYSUser)

            If Session.Item("loggedinuser") Is Nothing Then
                oUsers = clsSystemNYSUser.Populate(pstrUserName)
                'oUsers = clsSystemNYSUser.Populate("carllaw") ' Comment this out when released.

                For Each oUser As clsSystemNYSUser In oUsers
                    If oUser.Systemnysuserloginname.ToUpper = pstrUserName.ToUpper Then
                        Session.Item("loggedinuser") = oUser
                        Exit For
                    End If
                Next
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Sub grdData_ItemCommand
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Captures grdData events:
    ''' 1. Sort command when user clicks headers
    ''' 2. Row selection command which sends user to edit screen with selected row dataid
    ''' </remarks>
    Private Sub grdData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdData.ItemCommand
        Try
            If btnSaveGrid.Visible Then
                Return
            End If
            If e.CommandName = "Sort" Then
                Return
            End If

            Select Case e.CommandName
                Case "edit"
                    setSearchSessions()
                    Session.Item("MainsortCriteria") = ViewState("sortCriteria")
                    Session.Item("MainsortDir") = ViewState("sortDir")
                    Response.Redirect("FeedEdit.aspx?dataid=" & e.Item.Cells(0).Text)
                    'R2.21.4 SA 
                Case "remove"
                    txtdataid.Text = CStr(e.Item.Cells(0).Text)
                    pnconfirm.Visible = True
                Case Else
                    Throw New Exception("Unrecognised command name: " & e.CommandName)
            End Select
        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedMain", Page)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Sub grdData_SortCommand
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Sorts the grdData depending upon last sort request
    ''' </remarks>
    Private Sub grdData_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdData.SortCommand
        Try
            If Me.sortCriteria = e.SortExpression AndAlso Me.sortDir = "asc" Then
                Me.sortDir = "desc"
            Else
                Me.sortDir = "asc"
            End If
            Me.sortCriteria = e.SortExpression
            Dim intstatus As Integer = 0
            If pnsearch.Visible Then
                If ddstatus.Items.Count > 0 Then
                    intstatus = CInt(ddstatus.SelectedItem.Value)
                End If
            Else
                intstatus = FeedStatus.getStatusID("Feed Import OK")
            End If
            populateData(CInt(ddgroup.SelectedItem.Value), "", txtname.Text, txtinvoice.Text, _
                    txtguestpnr.Text, txtguestname.Text, intstatus)

        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedMain", Page)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Sub populateGroups
    ''' </summary>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Connects to DatabaseObjects and populates ddgroup with all groups in database
    ''' </remarks>
    Private Sub populateGroups(ByVal pintgroupid As Integer)
        Using New clslogger(log, className, "populateGroups")
            ddgroup.Items.Clear()

            Dim gps As List(Of clsGroup)
            gps = clsGroup.list

            Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                'Load a list of all records where Status = 1000 and get the GroupIDs
                Dim EFList = (From x In Cubit.FeedImportData Where x.statusid = 1000 And x.categoryid <> 0 Select x.groupid, x.groupname).ToList

                'add default line
                Dim oItem As New ListItem
                oItem.Value = "0"
                oItem.Text = "Please Select"
                ddgroup.Items.Add(oItem)
                oItem = Nothing

                'Dim list As ICollection(Of String) = Split(getConfig("TrackerGroups"), ";")

                For Each gp As clsGroup In gps

                    'R15 CR
                    Dim code As New clsCode
                    code = clsCode.getByName(gp.groupname)

                    If code.Codeid > 0 Then
                        If code.Customername.ToUpper = gp.groupname.ToUpper Then
                            oItem = New ListItem
                            oItem.Value = CStr(gp.groupid)

                            Dim TempGroupID = gp.groupid
                            'Count how many records are associated to this Group and display
                            Dim RecordCount = (From x In EFList Where x.groupid = TempGroupID).Count

                            If RecordCount = 0 Then
                                oItem.Attributes.Add("Style", "color:Red;")
                            Else
                                oItem.Attributes.Add("Style", "color:Green;")
                            End If

                            oItem.Text = "(" & RecordCount & " Records) " & gp.groupname

                            ddgroup.Items.Add(oItem)
                            oItem = Nothing
                            'Exit For
                        End If
                    End If

                    'For Each file As String In list
                    'If File.ToUpper = gp.groupname.ToUpper Then
                    'oItem = New ListItem
                    'oItem.Value = CStr(gp.groupid)
                    'oItem.Text = gp.groupname
                    'ddgroup.Items.Add(oItem)
                    'oItem = Nothing
                    'Exit For
                    'End If
                    'Next
                Next


            End Using
            If pintgroupid > 0 Then
                changeDropDowns(ddgroup, CStr(pintgroupid), False, False)
                If CStr(Session.Item("search")) = "true" Then
                    lblGroupView.Text = ddgroup.SelectedItem.Text
                End If
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Sub ddgroup_SelectedIndexChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Allows user to select a group to view current outstanding transaction
    '''</remarks>
    Protected Sub ddgroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddgroup.SelectedIndexChanged
        Using New clslogger(log, className, "ddgroup_SelectedIndexChanged")
            Try
                'R2.9 SA - need a fresh start!
                txtexportresult.Text = ""

                If ddgroup.SelectedIndex > 0 Then
                    txtguestname.Text = ""
                    txtguestpnr.Text = ""
                    txtinvoice.Text = ""
                    txtname.Text = ""
                    'first get correct group & company
                    If ddgroup.SelectedItem.Value = ddgroup.SelectedItem.Text Then

                    Else

                    End If
                    populateData(CInt(ddgroup.SelectedItem.Value), "", "", "", "", "", _
                             FeedStatus.getStatusID("Feed Import OK"))
                    Session.Item("groupid") = ddgroup.SelectedItem.Value
                    btnusesearch.Visible = True
                    If btnSaveGrid.Visible Then
                        btnSaveGrid.Visible = False
                        btnEdit.Visible = True
                    End If
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnadmin_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Captures btnadmin click event and sends user to FeedAdmin screen
    ''' </remarks>
    Protected Sub btnadmin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnadmin.Click
        Using New clslogger(log, className, "btnadmin_Click")
            Try
                If btnSaveGrid.Visible Then
                    btnSaveGrid.Visible = False
                    btnEdit.Visible = True
                End If
                Session.Remove("MainsortCriteria")
                Session.Remove("MainsortDir")
                setSearchSessions()
                Response.Redirect("FeedAdmin.aspx")
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub setSearchSessions
    ''' </summary>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' Sets up sessions so can return to same point 
    ''' </remarks>
    Private Sub setSearchSessions()
        Using New clslogger(log, className, "setSearchSessions")
            If pnsearch.Visible = True Then
                Session.Item("venuename") = txtname.Text
                Session.Item("invoiceno") = txtinvoice.Text
                Session.Item("guestpnr") = txtguestpnr.Text
                Session.Item("guestname") = txtguestname.Text
                Session.Item("status") = ddstatus.SelectedItem.Value
                Session.Item("search") = "true"
            Else
                Session.Remove("venuename")
                Session.Remove("invoiceno")
                Session.Remove("guestpnr")
                Session.Remove("guestname")
                Session.Remove("status")
                Session.Item("search") = "false"
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnsearch_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' Rebinds grdData using criteria user inputted
    ''' </remarks>
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnsearch.Click
        Using New clslogger(log, className, "btnsearch_Click")
            Try
                If ddgroup.SelectedIndex < 1 Then
                    Throw New NYSFriendlyException("Please select a group before using the" & _
                                                   " search function!", "Info")
                End If
                If txtname.Text = "" And txtinvoice.Text = "" And _
                    txtguestpnr.Text = "" And txtguestname.Text = "" And _
                    ddstatus.SelectedIndex < 1 Then
                    Throw New NYSFriendlyException("Please enter a value for at least one" & _
                                                    " of the search fields!", "Info")
                End If
                populateData(CInt(ddgroup.SelectedItem.Value), "", txtname.Text, txtinvoice.Text, _
                        txtguestpnr.Text, txtguestname.Text, CInt(ddstatus.SelectedItem.Value))
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub


    'R2.11 SA -
    Protected Sub btnexport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnexport.Click
        Using New clslogger(log, className, "btnexport_Click")

            If btnSaveGrid.Visible = True Then 'cant export till save
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('you must save grid responses first');", True)

                Exit Sub
            End If

            Try
                'R2.11 SA 
                txtexportresult.Text = ""
                Session.Item("LastExportResult") = ""

                If btnSaveGrid.Visible Then
                    btnSaveGrid.Visible = False
                    btnEdit.Visible = True
                End If

                If ddgroup.SelectedIndex < 1 Then
                    Throw New NYSFriendlyException("Please select a group from the list to export!", "Info")
                End If

                Dim es As List(Of FeedImportData)
                es = FeedImportData.exportList(CInt(ddgroup.SelectedItem.Value), _
                                               FeedStatus.getStatusID("feed import ok"))
                If es.Count = 0 Then
                    Throw New NYSFriendlyException("There are no records ready for export for: " & ddgroup.SelectedItem.Text & " !", "Info")
                Else
                    'R2.21.4 SA - check if category is EX, and change warning label if so
                    Dim lstSingleRecordHolder As New List(Of FeedImportData)
                    Dim blnNoRoom As Boolean = False

                    For Each oitem In es
                        lstSingleRecordHolder.Add(oitem)
                        Dim strCheckCategoryStatus As String = checkCategoryStatus(lstSingleRecordHolder)
                        If strCheckCategoryStatus = "" Then
                            blnNoRoom = True
                        End If
                    Next

                    If blnNoRoom = True Then
                        lblfeed.Text = "Some records don't have room charge against them, Are you sure you wish to Export all exportable records for: " & _
                                  ddgroup.SelectedItem.Text & " ?"
                    Else
                        lblfeed.Text = "Are you sure you wish to Export all exportable records for: " & _
                                  ddgroup.SelectedItem.Text & " ?"
                    End If

                    pnsure.Visible = True

                End If

            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try

        End Using
    End Sub

    'R2.11 SA - added ByRef possibleExports As List(Of Integer) to header and possibleExports.Remove(CInt(cc.Transactionnumber)) to body
    Private Function checkCostCodes(ByVal pintgroupid As Integer, ByVal pintstatus As Integer, ByVal pstrGroupID As Double, ByRef plstErrors As List(Of clsExportError)) As String
        Using New clslogger(log, className, "checkCostCodes")
            Dim strRet As String = ""
            Dim ccs As List(Of FeedImportData)

            Dim strMessage As String = "The following Transactions have incorrectly formatted codes:" & vbCrLf

            '##### east of england
            If pstrGroupID = 100047 Then
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 1)
                For Each cc As FeedImportData In ccs
                    If clsGroup.EOECostCentreCheck(cc.Costcode) = "" Then
                        strRet = strRet & cc.Transactionnumber & ";"
                        'R2.11 SA 
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                    End If
                Next
                '##### Anchor Trust
            ElseIf pstrGroupID = 100043 Then
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 0)
                For Each cc As FeedImportData In ccs
                    If clsGroup.AnchorCostCodeCheck(cc.Costcode, CInt(getConfig("DBType"))) <> 1 Then
                        strRet = strRet & cc.Transactionnumber & ";"
                        'R2.11 SA 
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                    End If
                Next
                'R16 NM
                'Michael : 01-05-2014
                'This is okay to take out as boss is handling the error checking
                'ElseIf pstrGroupName.ToLower = "university hospital of south manchester" Then
                '    ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 0)
                '    For Each cc As FeedImportData In ccs
                '        If cc.Costcode <> "" Then
                '            'R17 NM first check costcode exists
                '            If clsGroup.UHSMCostCodeCheckExists(cc.Costcode) = 0 Then
                '                strRet = strRet & cc.Transactionnumber & ";"
                '                'R2.11 SA 
                '                plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                '            Else
                '                'R17 NM then check it has a division
                '                If clsGroup.UHSMCostCodeCheck(Mid(cc.Costcode, 1, 1), CInt(getConfig("DBType"))) = "" Then
                '                    strRet = strRet & cc.Transactionnumber & ";"
                '                    'R2.11 SA 
                '                    plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                '                End If
                '            End If
                '        Else
                '            strRet = strRet & cc.Transactionnumber & ";"
                '            'R2.11 SA 
                '            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                '        End If
                '    Next

                'R2.20G SA
                '####nhs leadership academy
            ElseIf pstrGroupID = 100114 Then
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 0)
                For Each cc As FeedImportData In ccs
                    If cc.Costcode <> "" Then
                        If clsGroup.NHSLACostCodeCheckExists(cc.Costcode) = 0 Then
                            strRet = strRet & cc.Transactionnumber & ";"
                            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                        End If
                    Else
                        strRet = strRet & cc.Transactionnumber & ";"
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                    End If

                    If cc.AICol6 <> "" Then
                        If clsGroup.NHSLAExpenseTypeExists(cc.AICol6) = 0 Then
                            strRet = strRet & cc.Transactionnumber & ";"
                            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 12))
                        End If
                    Else
                        strRet = strRet & cc.Transactionnumber & ";"
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 12))
                    End If
                Next

                '###### lv=
            ElseIf pstrGroupID = 100041 Then
                Dim blnError As Boolean = False
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 2)

                Dim blnProjError As Boolean = False
                Dim strProjErrors As String = ""
                For Each cc As FeedImportData In ccs
                    If cc.Expression.Length = 7 Then
                        Try
                            Dim inttest As Integer = CInt(cc.Expression)
                        Catch ex As Exception
                            strRet = strRet & cc.Transactionnumber & ";"
                            'R2.11 SA 
                            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                        End Try
                    ElseIf cc.Expression.Length = 8 Then
                        Dim strEmployeeNumber As String = cc.Expression
                        If strEmployeeNumber.ToLower.StartsWith("m") Then
                            strEmployeeNumber = strEmployeeNumber.ToLower.Replace("m", "")
                            Try
                                Dim inttest As Integer = CInt(strEmployeeNumber)
                            Catch ex As Exception
                                strRet = strRet & cc.Transactionnumber & ";"
                                'R2.11 SA 
                                plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                            End Try
                        Else
                            strRet = strRet & cc.Transactionnumber & ";"
                            'R2.11 SA 
                            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                        End If
                    Else
                        strRet = strRet & cc.Transactionnumber & ";"
                        'R2.11 SA 
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                    End If

                    'R2.5 NM
                    'R2.10 CR - note: LV ACKCode in CostCentre field, only if record is in database do we check for a project code
                    ' if record is NOT in database then just let it through - BOSS checks ACK Code is a valid one!
                    If clsCode.checkLVCode(cc.Costcode) = 1 Then
                        If cc.Po.ToUpper.Trim = "" Or cc.Po.ToUpper.Trim = "." Or cc.Po.ToUpper.Trim = "N/A" Or cc.Po.ToUpper.Trim = "NONE" Then
                            strProjErrors = strProjErrors & cc.Transactionnumber & ";"
                            'R2.11 SA 
                            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 5))
                        End If
                    End If
                Next

                If strRet <> "" Then
                    strRet = "The following Transactions have incorrectly formatted codes:" & vbCrLf & strRet
                    strMessage = ""
                End If
                If strProjErrors <> "" Then
                    strRet = strRet & vbCrLf & "The following transaction numbers have invalid project codes:" & vbCrLf & strProjErrors
                    strMessage = ""
                End If

                'R15 CR
                '####### Herefordshire Council
            ElseIf pstrGroupID = 100051 Then
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 2)
                For Each cc As FeedImportData In ccs
                    If cc.Costcode.Length <> 6 Then
                        strRet = strRet & cc.Transactionnumber & ";"
                        'R2.11 SA 
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                    End If
                Next
                'R2.1 NM
                '##### 02
            ElseIf pstrGroupID = 100058 Then
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 0)
                Dim strNotRet As String = ""
                For Each cc As FeedImportData In ccs
                    If clsPo.checkPOExists(cc.Costcode) Then
                        If checkPo(cc.Costcode, True, True, FeedImportData.exportValueGet(CInt(cc.Transactionnumber))) <> "" Then
                            strRet = strRet & cc.Transactionnumber & ";"
                            'R2.11 SA 
                            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 6))
                        End If
                    Else
                        strNotRet = strNotRet & cc.Transactionnumber & ";"
                        'R2.11 SA 
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 7))
                    End If
                Next
                If strRet <> "" Then
                    strRet = "The following Transactions PO's are over value:" & vbCrLf & strRet
                End If
                If strNotRet <> "" Then
                    strRet = strRet & vbCrLf & _
                        "The following Transactions PO's don't exist in the O2 PO table:" & vbCrLf & strNotRet
                End If
                strMessage = ""
                'R2.10 CR
                '##### E-Act
            ElseIf pstrGroupID = 100046 Then
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 0)
                For Each cc As FeedImportData In ccs
                    'only check the code if it is not blank - allow blanks to go through
                    If cc.Po <> "" Then
                        If Not clsGroup.EACTCostCodeExists(cc.Po) Then
                            strRet = strRet & cc.Transactionnumber & ";"
                            'R2.11 SA 
                            plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                        End If
                    End If
                Next

            Else
                ccs = FeedImportData.CostCodeCheck(pintgroupid, pintstatus, 0)
                For Each cc As FeedImportData In ccs
                    Dim mRegExp As New Regex(cc.Expression)
                    If Not mRegExp.IsMatch(cc.Costcode) Then
                        strRet = strRet & cc.Transactionnumber & ";"
                        'R2.11 SA 
                        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                    End If
                    'R2.11 CR - commented out, NCAS (aka NPSA) no longer need the check
                    'If pstrGroupName.ToLower = "npsa" Then
                    '    Dim localPO As String = cc.Po
                    '    If localPO.ToLower = "n/a" Or _
                    '    localPO.ToLower = "" Or _
                    '    localPO.ToLower = "." Or _
                    '    localPO.ToLower = "-" Or _
                    '    localPO.ToLower = "_" Or _
                    '    localPO.ToLower = "na" Then
                    '        localPO = "none"
                    '    End If

                    '    If clsGroup.NPSAProjectCodeExists(localPO) = 0 Then
                    '        strRet = strRet & cc.Transactionnumber & ";"
                    '        'R2.11 SA 
                    '        plstErrors.Add(New clsExportError(CInt(cc.Transactionnumber), 4))
                    '    End If
                    'End If
                Next
            End If

            If strRet = "" Then
                Return ""
            Else
                'R2.10 CR - add line breaks to the error message!! it can't be read otherwise!
                Dim x As Integer = 0
                Dim strNewRet As String = ""
                Dim intSeperator As Integer = 0
                For x = 0 To strRet.Length - 1
                    If strRet(x) = ";" Then
                        intSeperator += 1
                    End If
                    If intSeperator = 10 Then
                        strNewRet &= strRet(x) & vbCrLf
                        intSeperator = 0
                    Else
                        strNewRet &= strRet(x)
                    End If
                Next

                Return strMessage & strNewRet
            End If

        End Using
    End Function

    Private Function checkExportStatus(ByVal pList As List(Of FeedImportData)) As String
        Using New clslogger(log, className, "checkExportStatus")
            Dim strtans As String = ""
            For Each es As FeedImportData In pList
                Dim intTrans As Integer = FeedImportData.exportCheck(CInt(es.Transactionnumber), _
                                            FeedStatus.getStatusID("Feed Import OK"))
                If intTrans > 0 Then
                    'R2.3 NM do another check to see if they are only extras, if so zero will be returned and allow extras to be exported
                    Dim intTrans2 As Integer = FeedImportData.exportCheckExtra(CInt(es.Transactionnumber))
                    If intTrans2 > 0 Then
                        strtans = strtans & CStr(intTrans2) & ","
                    End If
                End If
            Next
            Return strtans
        End Using
    End Function

    Private Function checkCategoryStatus(ByVal pList As List(Of FeedImportData)) As String
        Using New clslogger(log, className, "checkCategoryStatus")
            Dim strtans As String = ""
            For Each es As FeedImportData In pList
                Dim intTrans As Integer = clsCode.testCategory(CInt(es.Transactionnumber))
                If intTrans > 1 Then
                    strtans = strtans & CStr(intTrans) & ","
                End If
            Next
            Return strtans
        End Using
    End Function

    ''' <summary>
    ''' Sub SetBookingIDtoExportStatus
    ''' </summary>
    ''' <param name="pinttransactionnumber"></param>
    ''' <param name="pblnOk"></param>
    ''' <remarks>
    ''' Created 27/03/2009 Nick Massarella
    ''' Updates FeedData record status on export to BOSS setting correct status if failure occurs
    ''' </remarks>
    Private Sub SetBookingIDtoExportStatus(ByVal pDataid As Integer, ByVal pblnOk As Boolean)
        Using New clslogger(log, className, "SetBookingIDtoExportStatus")


            If pblnOk Then
                FeedImportData.SetBookingIDtoExportStatus(pDataid, FeedStatus.getStatusID("BOSS Export OK"))

            Else

                FeedImportData.SetBookingIDtoExportStatus(pDataid, FeedStatus.getStatusID("BOSS Export Error"))
            End If

        End Using
    End Sub
    Private Sub SetBookingIDtoExportStatusAM(ByVal pDataid As Integer, ByVal value As Integer) '1000 ok --- 1003 error
        Using New clslogger(log, className, "SetBookingIDtoExportStatusAM")

            Try
                FeedImportData.SetBookingIDtoExportStatus(pDataid, value)
            Catch
                log.Error("SetBookingIDtoExportStatusAM: Error DataID=" + pDataid.ToString() + " Sataus=" + value.ToString())
            End Try


        End Using
    End Sub


    Private Sub MesgBox(ByVal sMessage As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" & sMessage & "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub


    Private Function createInvoice(ByVal pinttransactionnumber As Integer) As Boolean
        Using New clslogger(log, className, "createInvoice")
            Dim ts As FeedInvoice
            ts = FeedInvoice.bookingTotalValue(pinttransactionnumber, 0)
            Dim inv As New FeedInvoice(0, pinttransactionnumber, CInt(lbluserid.Text), ts.dataitemnett, _
                                       ts.dataitemvat, ts.dataitemgross, False, Date.Now, _
                                       ts.transactionfee)
            If inv.save() > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Private Function createInvoiceAM(ByVal pinttransactionnumber As Integer) As Integer
        Using New clslogger(log, className, "createInvoice")
            Dim ts As FeedInvoice
            ts = FeedInvoice.bookingTotalValue(pinttransactionnumber, 0)
            'MK Used for testing
            lbluserid.Text = "100181"
            Dim inv As New FeedInvoice(0, pinttransactionnumber, CInt(lbluserid.Text), ts.dataitemnett, _
                                       ts.dataitemvat, ts.dataitemgross, False, Date.Now, _
                                       ts.transactionfee)
            Dim ret As Integer
            ret = inv.save()
            Return ret
        End Using
    End Function
    Public Trans As List(Of Integer) = New List(Of Integer)

    Private Function ExportSALEREFUND(ByVal pinttransactionnumber As Integer) As Boolean

        Using New clslogger(log, className, "ExportSALEREFUND")
            Dim ret As Boolean = False
            Dim exs As List(Of FeedImportData)

            exs = FeedImportData.exportByBookingIDList(pinttransactionnumber, _
                                                       FeedStatus.getStatusID("Feed Import OK"))
            Dim intTransactionValue As Integer
            intTransactionValue = FeedImportData.getOverallTransactionValue(CInt(pinttransactionnumber))
            If intTransactionValue <> 0 Then

                Dim exsPositive As List(Of FeedImportData) = New List(Of FeedImportData)
                Dim exsNegative As List(Of FeedImportData) = New List(Of FeedImportData)

                For Each ex As FeedImportData In exs
                    Dim Addrecord As Boolean = True
                    'MK 29-04-2013
                    'Do a Check for Booking only and Zero Commission
                    'If it is Skip the record
                    '01-05-2014 It has now been requested that these are left in

                    'Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                    '    Dim TempDataID = ex.Dataid

                    '    Dim EFBookingOnly = (From x In Cubit.FeedImportData Where x.dataid = TempDataID Select x.BookingOnlyEF).FirstOrDefault

                    '    If EFBookingOnly Is Nothing Or EFBookingOnly = False Then
                    '    Else

                    '        If ex.VenueDD = 0 Then
                    '            Dim EFFeedImportData = (From x In Cubit.FeedImportData Where x.dataid = TempDataID).FirstOrDefault
                    '            EFFeedImportData.statusid = 1006 'Zero Commission Booking Only
                    '            Cubit.SaveChanges()
                    '            Addrecord = False
                    '        End If

                    '    End If

                    'End Using

                    If Addrecord = True Then
                        If ex.Nettamount < 0 Then
                            exsNegative.Add(ex)
                        Else
                            exsPositive.Add(ex)
                        End If
                    End If
                Next


                If exsPositive.Count() > 0 Then

                    Dim invoiceID As Integer

                    'Todo Add in a new calculations for working out the ParaneterID 
                    'Based on international and commissionable
                    'Need to work out how we work out if the items is comm or non comm
                    'Internation or UK

                    '##########################
                    '##### From Lost work #####
                    '##########################

                    '22 - 05 - 2015 MK
                    'Before Creating the invoice we need to check the ParameterID
                    'This is to allow for the new options of international and commissionable

                    'Dim FeedSub As New FeedMainSubRoutines
                    'FeedSub.ValidateAndUpateParameterID(pinttransactionnumber)

                    '############################
                    '##### End of Lost work #####
                    '############################

                    'This routine must have saved the ParameterID before entering the export routine
                    'This is then Loaded later if the ParameterID is different for the current loaded one

                    'Load the original Parameter,
                    'Check to see if the comm is 0 or not
                    'FeedImportData.VenueDD = 0
                    'Check to see if it is GB or Not
                    'ConfirmaImport.CountryCode <> GB

                    Using Database As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                        Dim FeedDataList = (From x In Database.FeedImportData Where x.transactionnumber = pinttransactionnumber)

                        For Each FeedData In FeedDataList

                            'InvoiceLineDetailID
                            'transactionlinenumber take the last 7 Strings

                            Dim InvoiceLineDetailID As String = Right(CStr(FeedData.transactionlinenumber), 7)
                            Dim ConfirmaData = (From x In Database.ConfirmaImport Where x.InvoiceLineDetailID = InvoiceLineDetailID).FirstOrDefault
                            Dim CurrentParameter = (From x In Database.FeedParameter Where x.parameterid = FeedData.parameterid).FirstOrDefault

                            If CurrentParameter IsNot Nothing Then

                            Dim ParameterID As Integer = 0

                            Dim CurrentTransaction = (From x In Database.FeedTransaction Where x.transactionid = CurrentParameter.transactionid).FirstOrDefault
                                Dim ParamaterList = (From x In Database.FeedParameter Where x.groupid = FeedData.groupid).ToList

                                Dim TransactionList As New List(Of Cubit.FeedTransactionEF)

                                For Each x In ParamaterList
                                    If Date.Now >= x.parameterstart And Date.Now <= x.parameterend Then
                                        Dim TempTransaction = (From data In Database.FeedTransaction Where data.transactionid = x.transactionid).FirstOrDefault
                                        TransactionList.Add(TempTransaction)
                                    End If
                                Next

                                'Dim TransactionList = (From x In Database.FeedParameter Where x.groupid = FeedData.groupid Join TransactionJoin In Database.FeedTransaction On x.transactionid Equals TransactionJoin.transactionid Select TransactionJoin).ToList

                            Dim Comm As Boolean = True
                            Dim CountryCode As String = "GB"
                                Dim TransactionType As String = CurrentTransaction.transactiontype
                                Dim TransactionCode As String = CurrentTransaction.transactioncode

                            If FeedData.venueDD = 0 Then
                                Comm = False
                            End If

                            If ConfirmaData Is Nothing Then
                                'If we can't get the data go to the default option of UK
                            Else
                                    If ConfirmaData.CountryCode.ToLower <> "gb" Then
                                        CountryCode = "Intl"
                                    End If
                            End If

                            'Options
                            'Transaction Code = online offline
                            'Transaction Type = room all extras
                            'CountryCode = GB Intl
                            'Commisonalble = True False

                            'Check for Exact Match
                                Dim ExactMatch = (From x In TransactionList Where x.transactioncode = TransactionCode And x.transactiontype = TransactionType And x.CountryCode = CountryCode And x.Commissionable = Comm).FirstOrDefault

                            'Check for a Match on Non Comm
                            If ExactMatch IsNot Nothing Then
                                'We found a exactmatch update the transation
                                'Load the parameter 
                                Dim LoadedParameter = (From x In ParamaterList Where x.transactionid = ExactMatch.transactionid).FirstOrDefault
                                ParameterID = LoadedParameter.parameterid
                            End If

                            If ParameterID = 0 Then
                                'Check for a Match on International
                                    Dim InternationalMatch = (From x In TransactionList Where x.transactioncode = TransactionCode And x.transactiontype = TransactionType And x.CountryCode = CountryCode).FirstOrDefault

                                If InternationalMatch IsNot Nothing Then
                                    'We found a exactmatch update the transation
                                    'Load the parameter 
                                    Dim LoadedParameter = (From x In ParamaterList Where x.transactionid = InternationalMatch.transactionid).FirstOrDefault
                                    ParameterID = LoadedParameter.parameterid
                                End If
                            End If

                            If ParameterID = 0 Then
                                'Check on comm vs none comm
                                    Dim CommMatch = (From x In TransactionList Where x.transactioncode = TransactionCode And x.transactiontype = TransactionType And x.Commissionable = Comm).FirstOrDefault

                                    'Load the parameter 
                                    If CommMatch IsNot Nothing Then
                                        Dim LoadedParameter = (From x In ParamaterList Where x.transactionid = CommMatch.transactionid).FirstOrDefault
                                        ParameterID = LoadedParameter.parameterid
                                    End If
                                End If

                            If ParameterID <> 0 Then
                                'Assign it to the record and save
                                    FeedData.parameterid = ParameterID
                            End If

                            End If

                        Next

                        Database.SaveChanges()
                    End Using

                    invoiceID = createInvoiceAM(CInt(pinttransactionnumber))
                    If invoiceID > 0 Then

                        ret = exportData(pinttransactionnumber, exsPositive, invoiceID, "C") 'SALE XML
                        If ret = True Then

                            'set status for all items in transaction
                            For Each ex As FeedImportData In exsPositive
                                SetBookingIDtoExportStatusAM(CInt(ex.Dataid), 1002)
                                log.Info("exsPositive(SetBookingIDtoExportStatusAM) - Transaction=" & pinttransactionnumber.ToString() & ", DataId=" & CInt(ex.Dataid).ToString())
                            Next
                        Else
                            log.Error("exsPositive (exportData return false) - Transaction=" & pinttransactionnumber.ToString() & ", Invoice=" & invoiceID.ToString())
                        End If

                    End If
                End If

                'ret is false still if no positives
                If exsNegative.Count() > 0 Then
                    'OK do refund
                    Dim invoiceID As Integer
                    invoiceID = createInvoiceAM(CInt(pinttransactionnumber))
                    If invoiceID > 0 Then
                        ret = exportData(pinttransactionnumber, exsNegative, invoiceID, "CR") 'REFUND XML
                        If ret = True Then
                            For Each ex As FeedImportData In exsNegative
                                SetBookingIDtoExportStatusAM(CInt(ex.Dataid), 1002)
                                log.Info("exsPositive(SetBookingIDtoExportStatusAM) - Transaction=" & pinttransactionnumber.ToString() & ", DataId=" & CInt(ex.Dataid).ToString())
                            Next
                        Else
                            log.Error("exsNegative (exportData return false) - Transaction=" & pinttransactionnumber.ToString() & ", Invoice=" & invoiceID.ToString())
                        End If
                    End If
                End If

            Else 'intTransactionValue = 0 push as if exported in any case
                For Each ex As FeedImportData In exs
                    SetBookingIDtoExportStatusAM(CInt(ex.Dataid), 1002)
                Next
                ret = True
            End If

            Return ret
        End Using


    End Function

    Private Function exportData(ByVal pinttransactionnumber As Integer, ByRef exs As List(Of FeedImportData), ByVal InvoiceID As Integer, ByVal prefix As String) As Boolean
        Using New clslogger(log, className, "exportData")
            Dim intID As Integer = 0
            Dim xml As New StringBuilder

            Try
                'first lets get all records associated with passed in bookingID
                'Dim exs As List(Of FeedImportData)

                'exs = FeedImportData.exportByBookingIDList(pinttransactionnumber, _
                '                                           FeedStatus.getStatusID("Feed Import OK"))

                Dim intcount As Integer = 0
                Dim intInvoiceID As Integer = 0
                Dim dblTotalGros As Double = 0
                Dim commission As Double = 0

                Dim strcatcode As String = ""
                Dim blnTransaction As Boolean = False
                Dim dblTransaction As Decimal = 0
                Dim dbldiscountnett As Double = 0
                Dim dbldiscountvat As Double = 0
                Dim dbldiscounttotal As Double = 0
                Dim ExportList As New List(Of dataExportItems)
                Dim strCref1 As String = ""
                Dim strCref2 As String = ""
                Dim strCref3 As String = ""
                Dim strCref4 As String = ""
                Dim strCref5 As String = ""
                Dim strCref6 As String = ""
                Dim strCref7 As String = ""
                Dim strCref8 As String = ""
                Dim strCref9 As String = ""
                Dim strPO As String = ""
                Dim strCostCentre As String = ""
                Dim strSavingsCode As String = ""
                Dim blnExtraAdded As Boolean = False
                Dim BookingOnly As String = "N"
                Dim dblVatCalcRate As Double = CDbl(getConfig("VATCalcRate"))
                'R2.5 NM
                Dim blnAlreadyAddedExtrasFee As Boolean = False
                Dim blnAlreadyAddedInvoiceFee As Boolean = False
                Dim extrafee As Decimal = 0
                Dim CurrentPassenger As String = ""
                Dim LastPassenger As String = ""
                Dim intCounter As Integer = 0
                Dim strtransType As String = ""

                'R2.21 SA 
                Dim blnOOHAdded As Boolean = False

                'R2.21.1 SA 
                Dim blnAPTAdded As Boolean = False
                Dim blnCXAdded As Boolean = False

                Dim IsZeroCommissionBookingOnly As Boolean = False

                'now lets loop through and create the export file
                For Each ex As FeedImportData In exs

                    If ex.ExcludeFromExport = False Then

                        'MK Added Check for BookingOnly
                        'Note: This is UGLY but the files to auto generate the classes are missing
                        'No time to code them all in and change all storedprocedures
                        'The data transfer will be kept to a minmum by only selecting one bit field based on a primary key

                        Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                            Dim TempDataID = ex.Dataid
                            Dim EFBookingOnly = (From x In Cubit.FeedImportData Where x.dataid = TempDataID Select x.BookingOnlyEF).FirstOrDefault

                            If EFBookingOnly Is Nothing Or EFBookingOnly = False Then
                                BookingOnly = "N"
                            Else
                                BookingOnly = "Y"
                            End If

                        End Using

                        Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                            Dim TempDataID = ex.Dataid
                            Dim EFBookingOnly = (From x In Cubit.FeedImportData Where x.dataid = TempDataID Select x.BookingOnlyEF, x.parameterid).FirstOrDefault

                            If EFBookingOnly IsNot Nothing Then
                                If EFBookingOnly.BookingOnlyEF = True Then
                                    BookingOnly = "Y"
                                Else
                                    BookingOnly = "N"
                                End If
                            End If

                            If EFBookingOnly IsNot Nothing Then
                                If ex.Parameterid <> EFBookingOnly.parameterid Then
                                    ex.Parameterid = EFBookingOnly.parameterid
                                    'If Theme ParameterID is different we need to reload the transaction value
                                    'ex.tra()
                                    'End If
                                    Dim Transaction = (From x In Cubit.FeedParameter Where x.parameterid = ex.Parameterid Join TransactionJoin In Cubit.FeedTransaction On x.transactionid Equals TransactionJoin.transactionid Select TransactionJoin).FirstOrDefault

                                    ex.Transactionvalue = Transaction.transactionvalue
                                End If
                            End If

                        End Using

                        ex.Invoiceid = InvoiceID

                        If intCounter = 0 Then
                            strtransType = ex.TransactionType
                        End If

                        intCounter += 1
                        CurrentPassenger = ex.Passengername

                        'If CurrentGuestPNR = "" Then
                        '    CurrentGuestPNR = CStr(intGuestPNRCounter)
                        'Else
                        '    If ex.GuestPNR = "" Then
                        '        CurrentGuestPNR = CStr(intGuestPNRCounter)
                        '    Else
                        '        intGuestPNRCounter += 1
                        '    End If
                        'End If

                        If LastPassenger = "" Then
                            LastPassenger = CurrentPassenger
                        End If

                        'If LastGuestPNR = "" Then
                        '    If ex.GuestPNR = "" Then
                        '        LastGuestPNR = CStr(intGuestPNRCounter)
                        '    Else
                        '        LastGuestPNR = ex.GuestPNR
                        '    End If
                        'End If

                        If CurrentPassenger <> LastPassenger Then
                            blnAlreadyAddedExtrasFee = False
                        End If

                        Dim intItemsCount As Integer = exs.Count
                        'set id outside so can use if errors
                        If ex.Currency = "GBP" Then
                            intID = CInt(ex.Transactionnumber)

                            'VAT rate change for commission calcs
                            If CDate(ex.Arrivaldate) < CDate("04/01/2011") Then
                                dblVatCalcRate = 1.175
                            End If

                            'R?? SA - BUG FIX!!
                            ''R17 CR
                            ''allow multiple negative rows
                            ''check to see if all rows are negative
                            'Dim blnAllNegative As Boolean = False
                            'blnAllNegative = FeedImportData.ExportIsAllNegative(intID)

                            strPO = ex.poVal
                            strCostCentre = ex.ccVal
                            strCref1 = ex.CREF1Val
                            strCref2 = ex.CREF2Val
                            strCref3 = ex.CREF3Val
                            strCref4 = ex.CREF4Val
                            strCref5 = ex.CREF5Val
                            strCref6 = ex.CREF6Val
                            strCref7 = ex.CREF7Val
                            strCref8 = ex.CREF8Val
                            strCref9 = ex.CREF9Val

                            Dim strcustomercode As String = FeedInvoice.getCode(ex.GroupName)

                            'R2.21 CR - do the re-mapping of the BOSS code here
                            Dim strBOSSCodeColumn As String = clsClientOption.getBossOption(clsStuff.notWholeNumber(ex.Groupid))
                            If strBOSSCodeColumn <> "" Then
                                'they have a column specified to take the BOSS code from, use it!
                                strcustomercode = FeedImportData.getBookingColumnValue(ex.Dataid, strBOSSCodeColumn)
                            End If

                            '##### Chiesi ##########
                            If ex.GroupName.ToLower = "chiesi" Then
                                'Load the traveller profile and get the codes
                                Dim strTravellerEmail As String = ex.TravellerEmail
                                Dim intEmailExist As Integer = clsTravellerProfiles.checkEmail(strTravellerEmail)

                                If intEmailExist = 1 Then
                                    Dim oTraveller As New clsTravellerProfiles
                                    oTraveller.populate(0, strTravellerEmail)
                                    strCostCentre = oTraveller.Code1 'Cost Centre
                                    'strPO = oTraveller.Code2 'Department Code
                                    'strCref1 = oTraveller.Code3 'Area Code

                                    Try
                                        Dim codes() As String
                                        codes = ex.Costcode.Split(CChar("/"))

                                        strCref5 = codes(0) 'GL code
                                        strCref6 = codes(1) 'IO, adds a dot if value isn't present.

                                        If strCref6 = "" Then
                                            strCref6 = "."
                                        End If
                                    Catch exc As Exception
                                        Throw New NYSFriendlyException("Chiesi case does not contain the appropriately formatted GL/IO code, no cases will export.", "Export Error")
                                    End Try

                                Else
                                    'should not reach this stage, as validation is done before. 
                                End If

                            End If

                            If ex.GroupName.ToLower = "university of york" Then
                                If ex.ccVal.ToUpper = "YNIC" Then
                                    strcustomercode = "YNC"
                                    strCostCentre = "UNI YORK"
                                ElseIf ex.ccVal.ToUpper = "YEMC" Or ex.ccVal.ToUpper = "EMC" Then
                                    strcustomercode = "EMC"
                                    strCostCentre = "EMC"

                                    'R2.10 CR
                                ElseIf ex.ccVal.ToUpper = "CENLCF" Then
                                    strcustomercode = "CENLCF"
                                    strCostCentre = "CENLCF"
                                End If

                            ElseIf ex.GroupName.ToLower = "cima" Then
                                Dim ret As New getCimaRet
                                ret = getCimaDetails(CInt(Mid(ex.ccVal, 1, 3)))
                                strCref1 = ret.strcref1.Replace("&", "&amp;")
                                strCref2 = ret.strcref2.Replace("&", "&amp;")
                            ElseIf ex.GroupName.ToLower = "npsa" Then
                                If ex.poVal.ToLower = "n/a" Or _
                                    ex.poVal.ToLower = "" Or _
                                    ex.poVal.ToLower = "." Or _
                                    ex.poVal.ToLower = "-" Or _
                                    ex.poVal.ToLower = "_" Or _
                                    ex.poVal.ToLower = "na" Then
                                    strPO = "none"
                                End If
                                strSavingsCode = Mid(ex.AICol8, 1, 1)
                                'Michael: 24-04-2014 Comment out as no longer needed
                                ' ElseIf ex.GroupName.ToLower = "lincs council" Then
                                '  If ex.AICol9 = "" Then
                                '    Dim mRegExp As New Regex("^[A-Z,a-z][A-Z,a-z][0-9][0-9][0-9][0-9][0-9]$")
                                '     If mRegExp.IsMatch(strPO) Then
                                '        strcustomercode = "NLC"
                                '     Else
                                '        strcustomercode = "NELC"
                                '     End If
                                '   Else
                                '       If ex.AICol9.ToLower = "ne lincs" Then
                                '           strcustomercode = "NELC"
                                '       ElseIf ex.AICol9.ToLower = "north lincs" Then
                                '            strcustomercode = "NLC"
                                '        End If
                                '    End If
                            ElseIf ex.GroupName.ToLower = "east of england" Then
                                'R2.4 CR
                                'strCostCentre = "EOE"

                                strcustomercode = clsGroup.EOECostCentreCheck(ex.ccVal)

                                'R2.??? CR - comment out because billing method has changed and all departments now need booker name
                                'If Not ex.ccVal.ToLower.StartsWith("cgz79") Then
                                '    strCref2 = ""
                                'End If
                            ElseIf ex.GroupName.ToLower = "university hospital of south manchester" Then
                                strCref1 = clsGroup.UHSMCostCodeCheck(Mid(ex.Costcode, 1, 1), CInt(getConfig("DBType")))
                                'R2.20G SA 
                            ElseIf ex.GroupName.ToLower = "nhs leadership academy" Then
                                strCref1 = strCostCentre
                            ElseIf ex.GroupName.ToLower = "demontfort university".ToLower Or ex.GroupName.ToLower = "city of york council" Or ex.GroupName.ToLower = "yhip" Then
                                strcustomercode = ex.AICol6
                            ElseIf ex.GroupName.ToLower = "E-act".ToLower Then
                                If CStr(getConfig("EACTReplacements")).ToUpper.Contains(strCostCentre.ToUpper) Then
                                    strCref1 = strCostCentre
                                    strCostCentre = "HDO"
                                End If
                                'R2.5 NM
                            ElseIf ex.GroupName.ToLower = "NHS Business Services Authority".ToLower Then
                                If ex.AICol8 <> "" Then
                                    strcustomercode = ex.AICol8
                                End If
                            ElseIf ex.GroupName.ToLower = "NHS Business Services Authority".ToLower Then
                                If ex.AICol8 <> "" Then
                                    strcustomercode = ex.AICol8
                                End If
                                'R2.5 NM
                            ElseIf ex.GroupName.ToLower = "HomeGroup".ToLower Then
                                If getConfig("HomeGroupBoss").ToLower.Contains(ex.AICol10.ToLower) Then
                                    strcustomercode = getConfig("HomeGroupBossCode2")
                                Else
                                    strcustomercode = getConfig("HomeGroupBossCode1")
                                End If
                                'R2.13 NM RCN have 13 different customer codes so the value will be in AICol7
                            ElseIf ex.GroupName.ToLower = "Royal College of Nursing".ToLower Then
                                'R2.21.2 SA - BUG FIX!! 
                                'for RCN customer code in in ALCOl8 now..
                                strcustomercode = ex.AICol8
                                '  strcustomercode = ex.AICol7

                                'R2.11 CR
                            ElseIf ex.GroupName.ToLower = "Royal College of Nursing pod".ToLower Then
                                BookingOnly = "Y"
                                ex.Supplierinvoice = CStr(ex.Transactionnumber)

                                'R2.11 SA 
                            ElseIf ex.GroupName.ToLower = "Herefordshire Council".ToLower Then
                                If strCostCentre.StartsWith("J") Then
                                    strcustomercode = "HOOPLE"
                                Else
                                    strcustomercode = "HERECC"
                                End If

                                'R2.5 SA 
                            ElseIf ex.GroupName.ToLower = "piksel".ToLower Then
                                Dim strTravellerEmail As String = ex.TravellerEmail
                                Dim intEmailExist As Integer = clsTravellerProfiles.checkEmail(strTravellerEmail)

                                If intEmailExist = 1 Then
                                    Dim oTraveller As New clsTravellerProfiles
                                    oTraveller.populate(0, strTravellerEmail)
                                    strCostCentre = oTraveller.Code1 'Employee Numeber 
                                    strCref1 = oTraveller.Code3 'Business Unit
                                    strCref3 = oTraveller.Code5 ' Department 
                                    strCref4 = oTraveller.Code6 'Legal Entity
                                    strCref5 = oTraveller.Location ' location
                                Else
                                    'should not reach this stage, as validation is done before. 
                                End If
                            ElseIf ex.GroupName.ToLower = "kcom".ToLower Then
                                Dim strTravellerEmail As String = ex.TravellerEmail
                                Dim intEmailExist As Integer = clsTravellerProfiles.checkEmail(strTravellerEmail)

                                If intEmailExist = 1 Then
                                    Dim oTraveller As New clsTravellerProfiles
                                    oTraveller.populate(0, strTravellerEmail)
                                    strCostCentre = oTraveller.Code1 'Employee Numeber 
                                Else
                                    'should not reach this stage, as validation is done before. 
                                End If
                                'MK - SPIE Cubit Cost Centre processing
                            ElseIf ex.GroupName.ToLower = "spie" Then
                                'Check if there is a cost centre
                                'CL - removing this to test as I don't believe it is needed.... 20-08-2014
                                'If ex.Costcode <> "" Then
                                '    strCostCentre = ex.Costcode
                                'Else
                                '    Dim strTravellerEmail As String = ex.TravellerEmail
                                '    Dim intEmailExist As Integer = clsTravellerProfiles.checkEmail(strTravellerEmail)

                                '    If intEmailExist = 1 Then
                                '        Dim oTraveller As New clsTravellerProfiles
                                '        oTraveller.populate(0, strTravellerEmail)
                                '        strCostCentre = oTraveller.Code1 'Provided Cost Centre 
                                '    Else
                                '        'should not reach this stage, as validation is done before. 
                                '        'REMOVE ONCE LAUNCHED
                                '        'strCostCentre = "LOADED FROM TRAVELLER PROFILE"
                                '    End If
                                'End If
                            End If

                            If (ex.Totalamount + dbldiscounttotal) <> 0 Then
                                If intcount = 0 Then
                                    intInvoiceID = ex.Invoiceid
                                    xml.Append("<MEVISINVOICEBOSSEXPORT>\n<INVOICE>\n")

                                    'R2.18 CR - TODO: let credit notes come through - just uncomment and test!
                                    'Dim strInvoiceType As String = "I"
                                    'Dim strInvoiceToCredit As String = ""
                                    'If ex.Nettamount < 0 Then
                                    '    'Invoice type N means credit note, type I means invoice
                                    '    strInvoiceType = "N"
                                    '    'try to get previous invoice number
                                    '    strInvoiceToCredit = FeedImportData.getPreviousBOSSNumber("C" & ex.Invoiceid)
                                    'End If
                                    'xml.Append("<INVOICETYPE>" & strInvoiceType & "</INVOICETYPE>\n")
                                    'xml.Append("<INVOICETOCREDIT>" & strInvoiceToCredit & "</INVOICETOCREDIT>\n")


                                    'R2.21a SA - UNCOMMENT WHEN BOSS IS READY! TESTED!
                                    Dim strInvoiceType As String = "SALE"

                                    Dim strInvoiceToCredit As String = ""
                                    If ex.Nettamount < 0 Then
                                        'try to get previous invoice number

                                        strInvoiceToCredit = FeedImportData.getPreviousBOSSNumber("C" & ex.Invoiceid)
                                        strInvoiceType = "REFUND"
                                    End If
                                    xml.Append("<INVOICETYPE>" & strInvoiceType & strInvoiceToCredit & "</INVOICETYPE>\n")
                                    xml.Append("<INVOICEID>C" & ex.Invoiceid & "</INVOICEID>\n")
                                    'xml.Append("<INVOICEID>C" & pinttransactionnumber & "</INVOICEID>\n")

                                    'xml.Append("<INVOICEDATE>" & ex.InvoiceDate & "</INVOICEDATE>\n")
                                    xml.Append("<INVOICEDATE>" & Date.Now & "</INVOICEDATE>\n")
                                    xml.Append("<LINES>\n")
                                End If

                                If ex.Categorybosscode = "AC" Then
                                    strcatcode = "DD"
                                    commission = CDbl(ex.VenueDD)
                                Else
                                    strcatcode = ex.Categorybosscode
                                    commission = CDbl(ex.VenueEX)
                                End If

                                Dim comNett As Double = 0
                                Dim comVat As Double = 0
                                Dim comTot As Double = 0

                                'R2.21.3 SA - calculate comm for all treasactions
                                If commission > 0 And (ex.Totalamount + dbldiscounttotal) <> 0 Then
                                    comTot = CDbl(((ex.Totalamount + dbldiscounttotal) / 100) * commission)
                                    comNett = comTot / dblVatCalcRate
                                    comVat = comTot - comNett
                                End If
                                'R2.3 NM
                                If ex.Venuename.ToLower.Contains("ibis") Then
                                    'R2.6 NM
                                    'need to calculate the number of nights first
                                    Dim diff As Integer = CInt(DateDiff(DateInterval.Day, CDate(ex.Arrivaldate), CDate(ex.Departuredate)))
                                    comNett = Math.Round((CDbl(getConfig("IBISCommission")) * diff) / dblVatCalcRate, 2)
                                    comVat = Math.Round((CDbl(getConfig("IBISCommission")) * diff) - comNett, 2)
                                    'R2.6 NM 
                                    'only allow commission on the room
                                    If ex.Categoryid <> 1000 Then
                                        comNett = 0
                                        comVat = 0
                                    End If
                                End If
                                'R2.4 NM
                                Dim strVATText As String = "Standard VAT"
                                If ex.Vatamount + dbldiscountvat = 0 Then
                                    strVATText = "Zero VAT"
                                End If

                                Dim totalAmount As Double = CDbl(IIf(ex.Totalamount Is Nothing, 0.0, ex.Totalamount))

                                'R2.2 NM
                                Dim v As New dataExportItems(CStr(ex.Dataid), _
                                                        ex.Bookerinitials, _
                                                        strCref1, _
                                                        strCref2, _
                                                        strCref3, _
                                                        "N", _
                                                        "1", _
                                                        ex.Invoiceid & "-" & exs.Count, _
                                                        strcustomercode, _
                                                        strCostCentre, _
                                                        strPO, _
                                                        "", _
                                                        "H", _
                                                        ex.Venuebosscode, _
                                                        "", _
                                                        "", _
                                                        ex.Supplierinvoice, _
                                                        Format(ex.Arrivaldate, "dd/MM/yyyy"), _
                                                        Format(ex.Departuredate, "dd/MM/yyyy"), _
                                                        ex.Passengername, _
                                                        strcatcode & " - " & ex.Categoryname & " @ £" & Math.Round((totalAmount + dbldiscounttotal), 2) & " - " & strVATText, _
                                                        notString(ex.Nettamount + dbldiscountnett + ex.Vatamount + dbldiscountvat).Replace("£", ""), _
                                                        "", _
                                                        notString(ex.Vatamount + dbldiscountvat).Replace("£", ""), _
                                                        "", "", "", "", _
                                                        comNett.ToString("N2"), _
                                                        "", _
                                                        comVat.ToString("N2"), _
                                                        BookingOnly, _
                                                        strSavingsCode, _
                                                        strCref4, _
                                                        strCref5, _
                                                        strCref6, _
                                                        strCref7, _
                                                        strCref8, _
                                                        strCref9)
                                ExportList.Add(v)

                                If ex.GroupName = "LV=" And (ex.Venuereference = 3406 Or ex.Venuereference = 4086) And Not blnExtraAdded Then

                                    'R2.5 NM
                                    'Add extras extra fee just once for this booking
                                    If blnAlreadyAddedExtrasFee Then
                                        extrafee = 0
                                    Else
                                        'need to check if this booking has lines on that will qualify as an extras extra fee
                                        If clsClientOption.checkCategory(CInt(ex.Transactionnumber), ex.GuestPNR) > 0 Then
                                            extrafee = clsClientOption.getFee(CInt(ex.Groupid))
                                        Else
                                            extrafee = 0
                                        End If
                                        blnAlreadyAddedExtrasFee = True
                                    End If

                                    Dim dblNett As Double = 0
                                    Dim dblVAT As Double = 0
                                    Dim dblGross As Double = 0
                                    Dim dblExtraTransaction As Double = 8

                                    If ex.TransactionExVat Then
                                        dblNett = dblExtraTransaction + extrafee
                                        dblGross = CDbl(Math.Round(dblNett * dblVatCalcRate, 2))
                                        dblVAT = dblGross - dblNett
                                    Else
                                        dblGross = dblExtraTransaction + extrafee
                                        dblNett = CDbl(Math.Round(dblGross / dblVatCalcRate, 2))
                                        dblVAT = dblGross - dblNett
                                    End If

                                    Dim v3 As New dataExportItems(CStr(ex.Dataid), _
                                                                ex.Bookerinitials, _
                                                                strCref1, _
                                                                strCref2, _
                                                                strCref3, _
                                                                "N", _
                                                                "1", _
                                                                ex.Invoiceid & "-" & exs.Count, _
                                                                strcustomercode, _
                                                                strCostCentre, _
                                                                strPO, _
                                                                "", _
                                                                "MF", _
                                                                "TF", _
                                                                "", _
                                                                "", _
                                                                ex.Supplierinvoice, _
                                                                Format(ex.Arrivaldate, "dd/MM/yyyy"), _
                                                                Format(ex.Departuredate, "dd/MM/yyyy"), _
                                                                ex.Passengername, _
                                                                "TF" & " - transaction fee @ £" & dblGross, _
                                                                "", _
                                                                "", _
                                                                "", _
                                                                dblNett.ToString("N2"), _
                                                                dblVAT.ToString("N2"), _
                                                                "", _
                                                                "", _
                                                                "", _
                                                                "", _
                                                                "", _
                                                                "N", _
                                                                strSavingsCode, _
                                                                strCref4, _
                                                                strCref5, _
                                                                strCref6, _
                                                                strCref7, _
                                                                strCref8, _
                                                                strCref9)
                                    ExportList.Add(v3)
                                    blnExtraAdded = True
                                End If

                                'R2.21 SA - add out of hours charge - check for all clients 
                                If ex.Bookerinitials = "OOH" And Not blnOOHAdded Then
                                    Dim blnOOH As Boolean
                                    Dim dblOOHFeeGross As Decimal
                                    Dim dblOOHFeeNett As Decimal
                                    Dim dblOOHVAT As Decimal

                                    blnOOH = clsClientOption.checkOOHFee(CInt(ex.Groupid))
                                    If blnOOH = True Then
                                        dblOOHFeeNett = clsClientOption.getOOHFee(CInt(ex.Groupid))
                                        dblOOHVAT = CDec((ex.Vatrate * dblOOHFeeNett) / 100)
                                        dblOOHFeeGross = Round(dblOOHFeeNett + dblOOHVAT, 2)

                                        'R2.??? CR - BUG FIX. changed the product to TF, supplier to AERO24OOH
                                        ' this was going through to MF, TF which is wrong
                                        Dim v4 As New dataExportItems(CStr(ex.Dataid), _
                                                               ex.Bookerinitials, _
                                                               strCref1, _
                                                               strCref2, _
                                                               strCref3, _
                                                               "N", _
                                                               "1", _
                                                               ex.Invoiceid & "-" & exs.Count, _
                                                               strcustomercode, _
                                                               strCostCentre, _
                                                               strPO, _
                                                               "", _
                                                               "TF", _
                                                               "AERO24OOH", _
                                                               "", _
                                                               "", _
                                                               ex.Supplierinvoice, _
                                                               Format(ex.Arrivaldate, "dd/MM/yyyy"), _
                                                               Format(ex.Departuredate, "dd/MM/yyyy"), _
                                                               ex.Passengername, _
                                                               "TF" & " - out of hours fee @ £" & dblOOHFeeGross, _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               CStr(dblOOHFeeNett), _
                                                               CStr(dblOOHVAT), _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               "N", _
                                                               strSavingsCode, _
                                                               strCref4, _
                                                               strCref5, _
                                                               strCref6, _
                                                               strCref7, _
                                                               strCref8, _
                                                               strCref9)
                                        ExportList.Add(v4)
                                        blnOOHAdded = True
                                    End If
                                End If

                                'R2.21.1 SA 
                                'if client has APT fee against it then add fee 
                                If ex.Bookerinitials = "APT" And Not blnAPTAdded Then
                                    Dim blnAPT As Boolean
                                    Dim dblAPTFeeGross As Decimal
                                    Dim dblAPTFeeNett As Decimal
                                    Dim dblAPTVAT As Decimal

                                    blnAPT = clsClientOption.checkAPTFee(CInt(ex.Groupid))
                                    If blnAPT = True Then
                                        dblAPTFeeNett = clsClientOption.getAPTFee(CInt(ex.Groupid))
                                        dblAPTVAT = CDec((ex.Vatrate * dblAPTFeeNett) / 100)
                                        dblAPTFeeGross = Round(dblAPTFeeNett + dblAPTVAT, 2)

                                        Dim v5 As New dataExportItems(CStr(ex.Dataid), _
                                                               ex.Bookerinitials, _
                                                               strCref1, _
                                                               strCref2, _
                                                               strCref3, _
                                                               "N", _
                                                               "1", _
                                                               ex.Invoiceid & "-" & exs.Count, _
                                                               strcustomercode, _
                                                               strCostCentre, _
                                                               strPO, _
                                                               "", _
                                                               "TF", _
                                                               "APT", _
                                                               "", _
                                                               "", _
                                                               ex.Supplierinvoice, _
                                                               Format(ex.Arrivaldate, "dd/MM/yyyy"), _
                                                               Format(ex.Departuredate, "dd/MM/yyyy"), _
                                                               ex.Passengername, _
                                                               "TF" & " - serviced apartments  fee @ £" & dblAPTFeeGross, _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               CStr(dblAPTFeeNett), _
                                                               CStr(dblAPTVAT), _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               "", _
                                                               "N", _
                                                               strSavingsCode, _
                                                               strCref4, _
                                                               strCref5, _
                                                               strCref6, _
                                                               strCref7, _
                                                               strCref8, _
                                                               strCref9)
                                        ExportList.Add(v5)
                                        blnAPTAdded = True
                                    End If
                                End If

                                'R2.21.1 SA - BUG FIX: changed Product to MF, supplier to TF
                                'cx: cancellation. if it applies for a client then add it 
                                If ex.Categorybosscode = "CX" AndAlso Not blnCXAdded AndAlso _
                                    (ex.TTransactionType.ToUpper = "ROOM" Or ex.GroupName.ToLower = "nys corporate clients") Then
                                    Dim blnCX As Boolean
                                    Dim dblCXFeeGross As Decimal = 0
                                    Dim dblCXFeeNett As Decimal = 0
                                    Dim dblCXVAT As Decimal = 0

                                    blnCX = clsClientOption.checkCXFee(CInt(ex.Groupid))
                                    If blnCX = True AndAlso (ex.Parameterid > 0 Or ex.GroupName.ToLower = "nys corporate clients") Then
                                        'R2.21.5 SA 
                                        If ex.GroupName.ToLower = "nys corporate clients" Then
                                            'get cx gee based on sub-client 
                                            dblCXFeeNett = clsSubClientOptions.getSubClientFee(strcustomercode)
                                            dblCXVAT = CDec((ex.Vatrate * dblCXFeeNett) / 100)
                                            dblCXFeeGross = Round(dblCXFeeNett + dblCXVAT, 2)
                                        Else
                                            dblCXFeeNett = clsClientOption.getCXFee(CInt(ex.Groupid), CInt(ex.Parameterid), CInt(ex.Transactionnumber))
                                            dblCXVAT = CDec((ex.Vatrate * dblCXFeeNett) / 100)
                                            dblCXFeeGross = Round(dblCXFeeNett + dblCXVAT, 2)
                                        End If

                                        Dim v6 As New dataExportItems(CStr(ex.Dataid), _
                                                             ex.Bookerinitials, _
                                                            strCref1, _
                                                            strCref2, _
                                                            strCref3, _
                                                            "N", _
                                                            "1", _
                                                            ex.Invoiceid & "-" & exs.Count, _
                                                            strcustomercode, _
                                                            strCostCentre, _
                                                            strPO, _
                                                            "", _
                                                            "MF", _
                                                            "TF", _
                                                            "", _
                                                            "", _
                                                            ex.Supplierinvoice, _
                                                            Format(ex.Arrivaldate, "dd/MM/yyyy"), _
                                                            Format(ex.Departuredate, "dd/MM/yyyy"), _
                                                            ex.Passengername, _
                                                            "CX" & " - booking cancellation fee @ £" & dblCXFeeGross, _
                                                            "", _
                                                            "", _
                                                            "", _
                                                            CStr(dblCXFeeNett), _
                                                            CStr(dblCXVAT), _
                                                            "", _
                                                            "", _
                                                            "", _
                                                            "", _
                                                            "", _
                                                            "N", _
                                                            strSavingsCode, _
                                                            strCref4, _
                                                            strCref5, _
                                                            strCref6, _
                                                            strCref7, _
                                                            strCref8, _
                                                            strCref9)
                                        ExportList.Add(v6)
                                        blnCXAdded = True
                                    End If

                                End If

                                ''Or (ex.GroupName.ToLower = "nys corporate clients" AndAlso ex.Categorybosscode = "AC")

                                'R2.21.2 SA - BUG FIX!! Only add transaction fee if sale booking not a refund! - added blnAllNegative = false
                                'add the transaction line if there hasn't been one already added and the 
                                'fee is greater than 0
                                If ((ex.Parameterid > 0 And ex.SendGross = False) AndAlso ((ex.Totalamount + dbldiscounttotal) > 0) _
                                    AndAlso ex.Categorybosscode <> "CX") Then 'And blnTransaction = False Then

                                    'R2.5 NM
                                    'Add extras extra fee just once for this booking
                                    If blnAlreadyAddedExtrasFee Then
                                        extrafee = 0
                                    Else
                                        'need to check if this booking has lines on that will qualify as an extras extra fee
                                        If clsClientOption.checkCategory(CInt(ex.Transactionnumber), ex.GuestPNR) > 0 Then
                                            extrafee = clsClientOption.getFee(CInt(ex.Groupid))
                                        Else
                                            extrafee = 0
                                        End If
                                        blnAlreadyAddedExtrasFee = True
                                    End If

                                    'R2.21.5 SA - add transaction charge from subClientOptions for NYS Corporate clients
                                    If ex.GroupName.ToLower = "nys corporate clients" Then
                                        dblTransaction = clsSubClientOptions.getSubClientFee(strcustomercode)
                                    Else
                                        If notString(ex.Transactionvaluenew) <> "" Then
                                            dblTransaction = CDec(ex.Transactionvaluenew)
                                        Else
                                            dblTransaction = CDec(ex.Transactionvalue)
                                        End If
                                    End If

                                    If dblTransaction > 0 Then
                                        Dim dblNett As Decimal = 0
                                        Dim dblVAT As Decimal = 0
                                        Dim dblGross As Decimal = 0

                                        If ex.TransactionExVat Then
                                            dblNett = dblTransaction + extrafee
                                            dblGross = CDec(Math.Round(dblNett * dblVatCalcRate, 2))
                                            dblVAT = dblGross - dblNett
                                        Else
                                            dblGross = dblTransaction + extrafee
                                            dblNett = CDec(Math.Round(dblGross / dblVatCalcRate, 2))
                                            dblVAT = dblGross - dblNett
                                        End If

                                        'R2.??? CR - BUG FIX. changed Product to MF, supplier to TF
                                        ' The OOH values were in here which is incorrect
                                        Dim v2 As New dataExportItems(CStr(ex.Dataid), _
                                                        ex.Bookerinitials, _
                                                        strCref1, _
                                                        strCref2, _
                                                        strCref3, _
                                                        "N", _
                                                        "1", _
                                                        ex.Invoiceid & "-" & exs.Count, _
                                                        strcustomercode, _
                                                        strCostCentre, _
                                                        strPO, _
                                                        "", _
                                                        "MF", _
                                                        "TF", _
                                                        "", _
                                                        "", _
                                                        ex.Supplierinvoice, _
                                                        Format(ex.Arrivaldate, "dd/MM/yyyy"), _
                                                        Format(ex.Departuredate, "dd/MM/yyyy"), _
                                                        ex.Passengername, _
                                                        "TF" & " - transaction fee @ £" & dblGross, _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        dblNett.ToString("N2"), _
                                                        dblVAT.ToString("N2"), _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        "N", _
                                                        strSavingsCode, _
                                                        strCref4, _
                                                        strCref5, _
                                                        strCref6, _
                                                        strCref7, _
                                                        strCref8, _
                                                        strCref9)
                                        ExportList.Add(v2)

                                        dblTotalGros = dblTotalGros + dblTransaction
                                    End If
                                    blnTransaction = True
                                End If
                                dblTotalGros = dblTotalGros + notNumber(notString((ex.Totalamount + dbldiscounttotal)).Replace("£", ""))
                                intcount += 1
                                dbldiscountnett = 0
                                dbldiscountvat = 0
                                dbldiscounttotal = 0

                                'R2.13 NM Add fees line for additional always check
                                If ex.invoicefee > 0 Then 'And blnTransaction = False Then

                                    'R2.5 NM
                                    'Add extras extra fee just once for this booking
                                    If blnAlreadyAddedInvoiceFee Then
                                        extrafee = 0
                                    Else
                                        extrafee = ex.invoicefee
                                        If strtransType.ToUpper = "BKG" Then
                                            blnAlreadyAddedInvoiceFee = True
                                        End If
                                    End If

                                    If extrafee > 0 Then
                                        Dim dblNett As Decimal = 0
                                        Dim dblVAT As Decimal = 0
                                        Dim dblGross As Decimal = 0

                                        dblGross = extrafee
                                        dblNett = CDec(Math.Round(dblGross / dblVatCalcRate, 2))
                                        dblVAT = dblGross - dblNett

                                        Dim v2 As New dataExportItems(CStr(ex.Dataid), _
                                                        ex.Bookerinitials, _
                                                        strCref1, _
                                                        strCref2, _
                                                        strCref3, _
                                                        "N", _
                                                        "1", _
                                                        ex.Invoiceid & "-" & exs.Count, _
                                                        strcustomercode, _
                                                        strCostCentre, _
                                                        strPO, _
                                                        "", _
                                                        "BF", _
                                                        "TF", _
                                                        "", _
                                                        "", _
                                                        ex.Supplierinvoice, _
                                                        Format(ex.Arrivaldate, "dd/MM/yyyy"), _
                                                        Format(ex.Departuredate, "dd/MM/yyyy"), _
                                                        ex.Passengername, _
                                                        "TF" & " - transaction fee @ £" & dblGross, _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        dblNett.ToString("N2"), _
                                                        dblVAT.ToString("N2"), _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        "N", _
                                                        strSavingsCode, _
                                                        strCref4, _
                                                        strCref5, _
                                                        strCref6, _
                                                        strCref7, _
                                                        strCref8, _
                                                        strCref9)
                                        ExportList.Add(v2)

                                        dblTotalGros = dblTotalGros + extrafee
                                    End If
                                    'blnTransaction = True
                                End If
                                dblTotalGros = dblTotalGros + notNumber(notString((ex.Totalamount + dbldiscounttotal)).Replace("£", ""))
                                intcount += 1
                                dbldiscountnett = 0
                                dbldiscountvat = 0
                                dbldiscounttotal = 0

                            Else
                                dbldiscountnett = CDbl(dbldiscountnett + ex.Nettamount)
                                dbldiscountvat = CDbl(dbldiscountvat + ex.Vatamount)
                                dbldiscounttotal = CDbl(dbldiscounttotal + ex.Totalamount)
                            End If



                            'R2.21.2 SA - BUG FIX!! 
                            'Exporting credit notes to Boss - comment all check .. export all records  
                            'If blnAllNegative Then
                            '    GoTo startxml
                            'ELSEIf ex.Totalamount < 0 And intItemsCount > 1 Then 'we have a discount!!!!
                            '    If dbldiscounttotal = 0 Then
                            '        dbldiscountnett = CDbl(ex.Nettamount)
                            '        dbldiscountvat = CDbl(ex.Vatamount)
                            '        dbldiscounttotal = CDbl(ex.Totalamount)
                            '    Else
                            '        dbldiscountnett += CDbl(ex.Nettamount)
                            '        dbldiscountvat += CDbl(ex.Vatamount)
                            '        dbldiscounttotal += CDbl(ex.Totalamount)
                            '    End If
                            'Else

                            'R2.21.5 SA - commented out not needed any more 
                            'startxml:

                            'R2.21.2 SA - BUG FIX!! Export credit notes.. remove Or blnAllNegative from If statement 
                            'If (ex.Totalamount + dbldiscounttotal) > 0 Or intItemsCount = 1 Then ''old check 

                            'R2.13 NM

                            ' End If 'R2.21.2 SA - BUG FIX!!! Export Credit notes 
                        End If
                        LastPassenger = ex.Passengername

                    End If 'Exclude

                Next

                Dim Sorter As New ReportDownloader.Utility.Sorter(Of dataExportItems)

                Sorter.SortString = "Passengername,notes"
                ExportList.Sort(Sorter)

                For Each dino As dataExportItems In ExportList
                    xml.Append("<LINE>\n")
                    xml.Append("<ID>" & dino.Dataid & "</ID>\n")
                    xml.Append("<OPS>" & dino.Bookerinitials & "</OPS>\n")
                    xml.Append("<CREF1>" & dino.strCref1 & "</CREF1>\n")
                    xml.Append("<CREF2>" & dino.strCref2 & "</CREF2>\n")
                    xml.Append("<CREF3>" & dino.strCref3 & "</CREF3>\n")
                    xml.Append("<CREF4>" & dino.strCref4 & "</CREF4>\n")
                    xml.Append("<CREF5>" & dino.strCref5 & "</CREF5>\n")
                    xml.Append("<CREF6>" & dino.strCref6 & "</CREF6>\n")
                    xml.Append("<CREF7>" & dino.strCref7 & "</CREF7>\n")
                    xml.Append("<CREF8>" & dino.strCref8 & "</CREF8>\n")
                    xml.Append("<CREF9>" & dino.strCref9 & "</CREF9>\n")
                    xml.Append("<PAYNET>" & dino.PAYNET & "</PAYNET>\n")
                    xml.Append("<PAXNO>" & dino.PAXNO & "</PAXNO>\n")
                    xml.Append("<CRSREF>" & dino.CRSREF & "</CRSREF>\n")
                    xml.Append("<CUSTOMERCODE>" & dino.strcustomercode & "</CUSTOMERCODE>\n")
                    xml.Append("<COSTCENTRE>" & dino.Costcode & "</COSTCENTRE>\n")
                    xml.Append("<PURCHASEORDERNO>" & dino.Po & "</PURCHASEORDERNO>\n")
                    xml.Append("<FILEREF>" & dino.FILEREF & "</FILEREF>\n")
                    xml.Append("<PRODUCTCODE>" & dino.PRODUCTCODE & "</PRODUCTCODE>\n")
                    xml.Append("<SUPPLIERCODE>" & dino.Venuebosscode & "</SUPPLIERCODE>\n")
                    xml.Append("<HOTELPHONENO>" & dino.HOTELPHONENO & "</HOTELPHONENO>\n")
                    xml.Append("<CITYOFSERVICE>" & dino.CITYOFSERVICE & "</CITYOFSERVICE>\n")
                    xml.Append("<DOCUMENTNO>" & dino.Supplierinvoice & "</DOCUMENTNO>\n")
                    xml.Append("<STARTDATE>" & dino.Arrivaldate & "</STARTDATE>\n")
                    xml.Append("<ENDDATE>" & dino.Departuredate & "</ENDDATE>\n")
                    xml.Append("<LEADNAME>" & dino.Passengername & "</LEADNAME>\n")
                    xml.Append("<NOTES>" & dino.notes & "</NOTES>\n")
                    xml.Append("<SUPPLIERFARE>" & dino.SUPPLIERFARE & "</SUPPLIERFARE>\n")
                    xml.Append("<SUPPLIERCHARGE>" & dino.SUPPLIERCHARGE & "</SUPPLIERCHARGE>\n")
                    xml.Append("<SUPPLIERVAT>" & dino.SUPPLIERVAT & "</SUPPLIERVAT>\n")
                    xml.Append("<AGENCYCHARGE>" & dino.AGENCYCHARGE & "</AGENCYCHARGE>\n")
                    xml.Append("<AGENCYVAT>" & dino.AGENCYVAT & "</AGENCYVAT>\n")
                    xml.Append("<DISCOUNTAMT>" & dino.DISCOUNTAMT & "</DISCOUNTAMT>\n")
                    xml.Append("<DISCOUNTPERC>" & dino.DISCOUNTPERC & "</DISCOUNTPERC>\n")
                    xml.Append("<COMMISSION>" & dino.COMMISSION & "</COMMISSION>\n")
                    xml.Append("<COMMISSIONPERC>" & dino.COMMISSIONPERC & "</COMMISSIONPERC>\n")
                    xml.Append("<COMMISSIONVAT>" & dino.COMMISSIONVAT & "</COMMISSIONVAT>\n")
                    xml.Append("<BOOKINGONLY>" & dino.BOOKINGONLY & "</BOOKINGONLY>\n")
                    xml.Append("<SAVINGSCODE>" & dino.SavingsCode & "</SAVINGSCODE>\n")
                    xml.Append("</LINE>\n")
                Next

                If xml.ToString <> "" Then
                    xml.Append("</LINES>\n</INVOICE>\n</MEVISINVOICEBOSSEXPORT>\n")
                End If
            Catch ex As Exception
                'don't write the files if there is an error
                FeedImportData.saveFail(intID, ex.Message)
                Return False
            End Try

            Try  'Once the file is created there must be a status change
                Dim ExportFile As String = getConfig("BossFilesExportDir") & prefix & pinttransactionnumber & ".xml"
                Dim oFileXml As New System.IO.StreamWriter(ExportFile, False)
                oFileXml.Write(Xml.ToString.Replace("\n", vbNewLine))
                oFileXml.Flush()
                oFileXml.Close()
                Dim ExportFile2 As String = getConfig("BossFilesExportDir") & "CUBITBackup\" & prefix & pinttransactionnumber & ".xml"
                Dim oFileXml2 As New System.IO.StreamWriter(ExportFile2, False)
                oFileXml2.Write(xml.ToString.Replace("\n", vbNewLine))
                oFileXml2.Flush()
                oFileXml2.Close()
                'once the file is exported the status must change to 1002
                Return True
            Catch ex As Exception '' cannot change status if there is a problem writing file
                'first save message to record
                Throw (ex)
                Return False
            End Try


        End Using
    End Function
    Protected Structure getCimaRet
        Public strcref1, strcref2 As String
    End Structure

    Private Function getCimaDetails(ByVal pintcode As Integer) As getCimaRet
        Using New clslogger(log, className, "getCimaDetails")
            Dim ret As New getCimaRet
            Dim ci As New FeedInvoice
            ci = FeedInvoice.getCima(pintcode)
            ret.strcref1 = ci.department
            ret.strcref2 = ci.directorate
            Return ret
        End Using
    End Function

    Protected Sub btnusesearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnusesearch.Click
        Using New clslogger(log, className, "btnusesearch_Click")
            Try
                pnsearch.Visible = True
                lblGroupView.Text = ddgroup.SelectedItem.Text.ToUpper
                If ddstatus.Items.Count = 0 Then
                    populateStatus()
                End If
                If btnSaveGrid.Visible Then
                    btnSaveGrid.Visible = False
                    btnEdit.Visible = True
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub populateStatus
    ''' </summary>
    ''' <remarks>
    ''' Created 24/03/2009 Nick Massarella
    ''' Calls FeedStatus databaseobjects method to retrieve a list os statuses
    ''' </remarks>
    Private Sub populateStatus()
        Using New clslogger(log, className, "populateStatus")
            Dim fs As List(Of FeedStatus)
            fs = FeedStatus.list

            ddstatus.Items.Clear()
            ddstatus.Visible = True
            Dim oitem As ListItem
            oitem = New ListItem
            oitem.Value = "0"
            oitem.Text = "Please Select"
            ddstatus.Items.Add(oitem)
            oitem = Nothing

            For Each s As FeedStatus In fs
                oitem = New ListItem
                oitem.Value = CStr(s.Statusid)
                oitem.Text = s.Statusname
                ddstatus.Items.Add(oitem)
                oitem = Nothing
            Next
        End Using
    End Sub

    ''' <summary>
    ''' Sub btncancelsearch_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 24/03/2009 Nick Massarella
    ''' Hides search options panel
    ''' </remarks>
    Private Sub btncancelsearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btncancelsearch.Click
        Using New clslogger(log, className, "btncancelsearch_Click")
            Try
                'rebind grid to show Exportable records only
                lblGroupView.Text = ""
                populateData(CInt(Session.Item("groupid")), "", "", "", _
                     "", "", FeedStatus.getStatusID("Feed Import OK"))
                pnsearch.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnno_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 27/03/2009 Nick Massarella
    ''' Cancels export button click and hides warning panel
    ''' </remarks>
    Protected Sub btnno_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnno.Click
        Using New clslogger(log, className, "btnno_Click")
            Try
                pnsure.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.11 CR
    Private Sub NewExportAttempt()

        pnsure.Visible = False

        'get the import OK status
        Dim intStatusID As Integer = 1000 'FeedStatus.getStatusID("feed import ok")

        'Create a list of all records ready for export 
        Dim lstToExport As List(Of FeedImportData)
        lstToExport = FeedImportData.exportList(CInt(ddgroup.SelectedItem.Value), intStatusID)

        'Create a new list to hold the transactionnumber for errored records, and the reason for error
        Dim lstErrors As New List(Of clsExportError)


        'now check each record for validity

        '1st - check the boss code errors -- 1). NO VENUE CODE
        Dim lstBossCodeErrors As List(Of FeedImportData)
        lstBossCodeErrors = FeedImportData.exportListBossCodeCheck(CInt(ddgroup.SelectedItem.Value), intStatusID)
        For Each oItem In lstBossCodeErrors
            'we have an error with a transaction, add it to the list of errors
            lstErrors.Add(New clsExportError(CInt(oItem.Transactionnumber), 1))
        Next


        '2nd and 3rd - check the export status and category
        Dim lstSingleRecordHolder As New List(Of FeedImportData)
        For Each oItem In lstToExport
            lstSingleRecordHolder.Add(oItem)

            'check for export status and category errors
            If getConfig("AvoidCheck2").ToString = "true" Then
                Dim strCheckExportStatusError As String = checkExportStatus(lstSingleRecordHolder)

                'R2.21.4 SA - comment out! need to export all categories
                ' Dim strCheckCategoryStatus As String = checkCategoryStatus(lstSingleRecordHolder)
                'if oItem passes the first three checks: add it to export list tuen check on and off
                If strCheckExportStatusError <> "" Then
                    lstErrors.Add(New clsExportError(CInt(oItem.Transactionnumber), 2))
                End If
            End If
            lstSingleRecordHolder.Clear() 'clear list to add next item, keep the list as single records
        Next

        '4th - check the cost codes
        Dim strCostCodesError As String = checkCostCodes(CInt(ddgroup.SelectedItem.Value), _
                                                        intStatusID, CDbl(ddgroup.SelectedItem.Value), lstErrors)

        'R2.12 CR
        '5th check the LV traveller
        'if traveller not found then error to user
        'if the traveller IS found, change the relevant fields
        '############ LV= '############
        If CDbl(ddgroup.SelectedItem.Value) = 100041 And getConfig("LVTravellerCheckLive").ToString = "true" Then
            'LV= import data key:
            'AICol6: Employee Number
            'AICol7: Project Code
            'CostCode: ACKCode
            For Each oItem In lstToExport
                checkAndChangeLVTravellers(clsStuff.notWholeNumber(oItem.Transactionnumber), intStatusID, lstErrors)
            Next
        End If

        'R2.5 SA 
        '6th check the Piksle Traveller
        'if traveller not found then error to user
        '############ Piksel '############
        If CDbl(ddgroup.SelectedItem.Value) = 100130 Then
            Dim intEmailExist As Integer = 0
            'For each record
            For Each oItem In lstToExport
                'get details - we want traveller email
                Dim oItemList As New List(Of FeedImportData)
                oItemList = FeedImportData.exportByBookingIDList(clsStuff.notWholeNumber(oItem.Transactionnumber), intStatusID)

                For Each oExportItem In oItemList
                    'check if traveller exist
                    intEmailExist = clsTravellerProfiles.checkEmail(oExportItem.TravellerEmail)
                    If intEmailExist <> 1 Then
                        lstErrors.Add(New clsExportError(CInt(oItem.Transactionnumber), 13))
                    End If
                Next
            Next
        End If

        '############ Kcom '############
        If CDbl(ddgroup.SelectedItem.Value) = 100125 Then
            Dim intEmailExist As Integer = 0
            'For each record
            For Each oItem In lstToExport
                'get details - we want traveller email
                Dim oItemList As New List(Of FeedImportData)
                oItemList = FeedImportData.exportByBookingIDList(clsStuff.notWholeNumber(oItem.Transactionnumber), intStatusID)

                For Each oExportItem In oItemList
                    'check if traveller exist
                    intEmailExist = clsTravellerProfiles.checkEmail(oExportItem.TravellerEmail)
                    If intEmailExist <> 1 Then
                        lstErrors.Add(New clsExportError(CInt(oItem.Transactionnumber), 15))
                    End If
                Next
            Next
        End If

        'Checking all done now

        'work out how many error messages we have to display & compile the transactionnumbers
        Dim dictCompiledErrorTransactions As New Dictionary(Of Integer, String)
        For Each oError In lstErrors
            If dictCompiledErrorTransactions.ContainsKey(oError.ErrorCode) Then
                'update it
                dictCompiledErrorTransactions(oError.ErrorCode) &= oError.TransactionNumber.ToString & "; "
            Else
                'add it
                dictCompiledErrorTransactions.Add(oError.ErrorCode, oError.TransactionNumber.ToString & "; ")
            End If
        Next


            'Export anything that doesnt have an error

            Dim strCheckBossExport As String = ""

            Trans.Clear()

        For Each oItem In lstToExport

            If Not recordHasErrors(CInt(oItem.Transactionnumber), lstErrors) Then
                'R2.9 SA - check if the value of all lines for that transaction is not zero

                'no errors and nt cancelling amounts try exporting
                If exportSingle(CInt(oItem.Transactionnumber), CInt(oItem.Dataid_null)) Then 'Exports the entire transaction if not already sent

                Else 'Export Unscucessful

                End If
            Else 'has errors
                If strCheckBossExport.Contains(CStr(oItem.Transactionnumber)) = False Then
                    strCheckBossExport &= CStr(oItem.Transactionnumber) & "  "
                End If
                ''change status code to error --> Do not as live CUBIT appears not to do this in FACT!!
                'SetBookingIDtoExportStatus(CInt(oItem.Dataid_null), False)


            End If

        Next '' lstToExport

        If strCheckBossExport <> "" Then
            'add the wordy bit
            strCheckBossExport = " There were BOSS Export errors for the following transactions:" & vbCrLf & strCheckBossExport
            'change the status we are going to be looking at
            ' why change at this point
            intStatusID = FeedStatus.getStatusID("BOSS Export Error")
        End If

        'Finally display the errors
        'if there are no errors at all (I know, I'm suprised too!) - then tell them everything is OK
        Dim strResult As String = ""
        For Each oItem In dictCompiledErrorTransactions
            strResult &= vbCrLf & clsExportError.getErrorMessage(oItem.Key) & oItem.Value.ToString & vbCrLf
        Next
        strResult &= strCheckBossExport

        Dim strMessage As String = ""
        If strResult <> "" Then

            strMessage = "Valid detail lines from the following transaction numbers were passed to export: "
            For Each ex As Integer In Trans
                strMessage += ex.ToString() + ", "
            Next
            strMessage = strMessage.Substring(0, strMessage.Length - 2)
        Else
            strMessage = "All records exported OK - no errors"
        End If

        'now add the group name to the error results - if any errors    
        If strResult <> "" Then
            strResult = "Export Errors for " & ddgroup.SelectedItem.Text & strResult
        End If

        'reselect the status
        'If strCheckExportStatusError <> "" Or strCheckCategoryStatus <> "" Or strCheckCostCodes <> ""  Then
        '    changeDropDowns(ddstatus, CStr(intStatusID), False, False)
        'ElseIf strCheckBossExport <> "" Then
        'changeDropDowns(ddstatus, CStr(intStatusID), False, False) -- AM dont need to change dropdowns yet
        'Else
        '    changeDropDowns(ddstatus, "", True, False)
        'End If

        'display details of failer messages 
        txtexportresult.Text = strResult

        'repopulate the grid
        'populateData(CInt(ddgroup.SelectedValue), "", "", "", "", "", intStatusID)
        'AM keep at 1000
        populateData(CInt(ddgroup.SelectedValue), "", "", "", "", "", 1000)
        Throw New NYSFriendlyException(strMessage, "Info")


    End Sub

    'R2.12 CR
    Private Sub checkAndChangeLVTravellers(ByVal pintTransactionNumber As Integer, ByVal pintStatusID As Integer, ByRef lstErrors As List(Of clsExportError))

        'get a list of all possible rows for the current transaction number
        Dim oItemList As New List(Of FeedImportData)
        oItemList = FeedImportData.exportByBookingIDList(pintTransactionNumber, pintStatusID)

        For Each oExportItem In oItemList

            Dim oTraveller As New clsTravellerProfiles
            'oTraveller.populate(0, oExportItem.TravellerEmail)
            oTraveller = clsTravellerProfiles.getByEmail(oExportItem.TravellerEmail)

            If oTraveller.Email = Nothing OrElse oTraveller.Email = "" Then
                'cant find the traveller - error to user
                lstErrors.Add(New clsExportError(pintTransactionNumber, 8))
            Else
                'CAN find the traveller, change the relevant fields
                Dim blnError As Boolean = False

                'check there is an employee number
                If oTraveller.Code2 = "" Then
                    'if there isn't, error - we need it
                    lstErrors.Add(New clsExportError(pintTransactionNumber, 9))
                    blnError = True
                Else
                    'if there is, save it to col6 ready for export
                    oExportItem.AICol6 = oTraveller.Code2
                End If

                'if project code provided then let the details go through as they are
                If oExportItem.AICol7 <> "" Then
                    'leave the details as they are!
                    'but check there is also an ACK!
                    If oExportItem.Costcode = "" Then
                        'error
                        lstErrors.Add(New clsExportError(pintTransactionNumber, 10))
                        blnError = True
                    End If
                Else
                    'if no project code provided, use the default ACK and set the project code to blank

                    oExportItem.AICol7 = ""

                    If oTraveller.Code1 <> "" Then
                        oExportItem.Costcode = oTraveller.Code1
                    Else
                        'error
                        lstErrors.Add(New clsExportError(pintTransactionNumber, 11))
                        blnError = True
                    End If
                End If

                'Save the changes - if no error
                If Not blnError Then
                    FeedImportData.savevalue(clsStuff.notWholeNumber(lbluserid.Text), oExportItem.Dataid, oExportItem.Transactionvaluenew, oExportItem.Venuename, clsStuff.notWholeNumber(oExportItem.Venuereference), oExportItem.Venuebosscode, _
                                             clsStuff.notDouble(oExportItem.VenueEX), clsStuff.notDouble(oExportItem.VenueDD), oExportItem.Costcode, oExportItem.AICol6, oExportItem.AICol7, oExportItem.AICol8, oExportItem.AICol9, oExportItem.AICol10, oExportItem.TravellerEmail)
                End If
            End If
        Next


    End Sub

    'R2.11 CR - re-write the re-write!
    'results weren't getting displayed properly and errored records actually still exported
    Protected Sub btnyes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnyes.Click
        Using New clslogger(log, className, "btnyes_Click")
            pnsure.Visible = False
            Try
                NewExportAttempt()
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
        populateGroups(CInt(ddgroup.SelectedItem.Value))
        'Response.Redirect("FeedMain.aspx")
    End Sub

    'R2.11 CR
    Private Function recordHasErrors(pintTransactionnumber As Integer, lstErrors As List(Of clsExportError)) As Boolean
        Dim ret As Boolean = False

        For Each oItem In lstErrors
            If oItem.TransactionNumber = pintTransactionnumber Then
                ret = True
            End If
        Next

        Return ret
    End Function

    'R2.11 CR
    'Define a class for using during export error checking
    Private Class clsExportError
        Private mTransactionNumber As Integer
        Private mErrorCode As Integer
        Public Property TransactionNumber As Integer
            Get
                Return mTransactionNumber
            End Get
            Set(value As Integer)
                mTransactionNumber = value
            End Set
        End Property
        Public Property ErrorCode As Integer
            Get
                Return mErrorCode
            End Get
            Set(value As Integer)
                mErrorCode = value
            End Set
        End Property
        Public Shared Function getErrorMessage(pintErrorCode As Integer) As String
            Dim strErrorMessage As String = ""
            Select Case pintErrorCode
                Case 1
                    strErrorMessage = "The following Transactions don't have Venue Bosscodes set:" & vbCrLf
                Case 2
                    strErrorMessage = "The following Transactions have differing statuses:" & vbCrLf
                    ' check 3 no longer applicable - it was assume that a transaction must have at least a room booking, but room could have been booked in 
                    ' an earlier export
                Case 4
                    strErrorMessage = "The following Transactions have incorrectly formatted codes:" & vbCrLf
                Case 5
                    strErrorMessage = "The following Transactions have invalid project codes:" & vbCrLf
                Case 6
                    strErrorMessage = "The following Transactions have PO's that are over value:" & vbCrLf
                Case 7
                    strErrorMessage = "The following Transactions have PO's that dont exist in our records:" & vbCrLf
                Case 8
                    strErrorMessage = "The following LV Transactions can't match to a valid LV Traveller:" & vbCrLf
                Case 9
                    strErrorMessage = "The following LV Transactions HAVE MATCHED to a valid LV Traveller, but the traveller does not have an Employee Number:" & vbCrLf
                Case 10
                    strErrorMessage = "The following LV Transactions have provided a project code, but no ACK Code:" & vbCrLf
                Case 11
                    strErrorMessage = "The following LV Transactions HAVE MATCHED to a valid LV Traveller, but the traveller does not have an ACK Code:" & vbCrLf
                    'R2.20G SA
                Case 12
                    strErrorMessage = "The following Transactions have invalid expense types:" & vbCrLf
                    'R2.5 SA 
                Case 13
                    strErrorMessage = "The following Piksel Transactions can't match to a valid Piksel Traveller:" & vbCrLf
                    'R2.9 SA 
                Case 14
                    strErrorMessage = "The following Transactions have a value of Zero! No records to export!" & vbCrLf
                Case 15
                    strErrorMessage = "The following Kcom Transactions can't match to a valid Kcom Traveller:" & vbCrLf
                Case Else
                    strErrorMessage = ""
            End Select

            Return strErrorMessage
        End Function
        Public Sub New()
        End Sub
        Public Sub New(pintTransactionNumber As Integer, _
                       pintErrorCode As Integer)
            mTransactionNumber = pintTransactionNumber
            mErrorCode = pintErrorCode
        End Sub
    End Class

    Private Function InTrans(ByVal pTransactionnumber As Integer) As Boolean
        Dim ret As Boolean = False
        For Each ex As Integer In Trans
            If pTransactionnumber = ex Then
                ret = True
            End If
        Next
        Return ret
    End Function

    'R2.11 SA
    Private Function exportSingle(ByVal pTransactionnumber As Integer, ByVal pDataid As Integer) As Boolean
        Dim TransationDealtWith As Boolean = False 'either exported or nullified
        Using New clslogger(log, className, "exportSingle")
            If Not (InTrans(pTransactionnumber)) Then
                If ExportSALEREFUND(CInt(pTransactionnumber)) Then
                    Trans.Add(pTransactionnumber)
                    TransationDealtWith = True
                Else
                    'not dealt with
                    TransationDealtWith = False
                End If
            Else
                TransationDealtWith = True 'already dealt with
            End If

            Return TransationDealtWith
        End Using
    End Function

    Protected Sub btnTestFileDownload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestFileDownload.Click
        Try
            'Dim strmessage As String = WebRetrieve.Main()
            Dim strmessage As String = WebRetrieve.TransData("2012-03-11", "12-03-2012")

            If strmessage = "" Then
                Throw New NYSFriendlyException("File download OK", "Info")
            Else
                Throw New NYSFriendlyException("File was not downloaded - " & strmessage, "Info")
            End If
        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedMain", Page)
            End If
        End Try
    End Sub

    Protected Sub btnTestFileImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestFileImport.Click
        Try
            'Dim otests As New List(Of clsCode)
            'otests = clsCode.testlist
            'Dim strResult As String = ""
            'For Each otest As clsCode In otests
            '    Dim intRet As Integer = clsCode.testCategory(otest.transactionnumber)
            '    If intRet > 1 Then
            '        strResult = strResult & CStr(intRet) & ";"
            '    End If
            'Next
            'Dim istop As Integer = 0


            Dim csv As New CSVReader
            'Dim csv As New CSVReaderBooked
            'Dim csv As New CSVReaderTrans

            If csv.Main() Then
                Throw New NYSFriendlyException("CSV file import OK", "Info")
            Else
                Throw New NYSFriendlyException("CSV file import ERROR " + getConfig("downloadedfiles"), "Info")
            End If
        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedMain", Page)
            End If
        End Try
    End Sub

    'R2.4 NM removed
    'Protected Sub btnBossImport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBossImport.Click
    '    Try
    '        Dim intStatusID As Integer = 0
    '        Dim strRet As String = checkBOSSReturnedFiles()
    '        If strRet = "OK" Then
    '            intStatusID = FeedStatus.getStatusID("Completed")
    '        Else
    '            intStatusID = FeedStatus.getStatusID("Incomplete")
    '        End If
    '        bindData(CInt(Session.Item("groupid")), "", "", "", "", "", intStatusID)
    '        Throw New NYSFriendlyException("BOSS file import " & strRet, "Info")
    '    Catch ex As Exception
    '        If Not TypeOf ex Is System.Threading.ThreadAbortException Then
    '            handleException(ex, "FeedMain", Page)
    '        End If
    '    End Try
    'End Sub

    Protected Sub btnTestDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestDelete.Click
        FeedImportData.deleteAll()
    End Sub

    'R2.4 NM
    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Using New clslogger(log, className, "btnEdit_Click")
            Try
                For Each dgiCount As DataGridItem In grdData.Items
                    CType(dgiCount.FindControl("chkExcludeFromExport"), CheckBox).Enabled = True
                Next
                btnEdit.Visible = False
                btnSaveGrid.Visible = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnSaveGrid_Click(sender As Object, e As EventArgs) Handles btnSaveGrid.Click
        Using New clslogger(log, className, "btnSaveGrid_Click")
            Try
                Dim currentTransID As Integer = 0
                Dim lastTransID As Integer = 0

                For Each dgiCount As DataGridItem In grdData.Items
                    'R2.21.4 SA - incremented cell numbers by one, as new column is added at index 1
                    'FeedImportData.saveExcludeFromExport(grdData.Items(dgiCount.ItemIndex).Cells(2).Text, CType(dgiCount.FindControl("chkExcludeFromExport"), CheckBox).Checked)
                    FeedImportData.saveExcludeFromExport(grdData.Items(dgiCount.ItemIndex).Cells(0).Text, CType(dgiCount.FindControl("chkExcludeFromExport"), CheckBox).Checked)
                Next

                'if all is well disable grid
                For Each dgiCount As DataGridItem In grdData.Items
                    CType(dgiCount.FindControl("chkExcludeFromExport"), CheckBox).Enabled = False
                Next

                btnEdit.Visible = True
                btnSaveGrid.Visible = False
                Dim intstatus As Integer = 0
                If pnsearch.Visible Then
                    If ddstatus.Items.Count > 0 Then
                        intstatus = CInt(ddstatus.SelectedItem.Value)
                    End If
                Else
                    intstatus = FeedStatus.getStatusID("Feed Import OK")
                End If
                populateData(CInt(ddgroup.SelectedItem.Value), "", txtname.Text, txtinvoice.Text, _
                        txtguestpnr.Text, txtguestname.Text, intstatus)
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.4 SA
    Protected Sub btnConfrim_Click(sender As Object, e As EventArgs) Handles btnConfrim.Click
        Using New clslogger(log, className, "btnConfrim_Click")
            Try
                Dim intIDConfirm As Integer
                intIDConfirm = FeedImportData.deleteSingle(CInt(txtdataid.Text))
                If intIDConfirm = 1 Then
                    pnconfirm.Visible = False
                    populateData(CInt(ddgroup.SelectedItem.Value), "", "", "", "", "", _
                            FeedStatus.getStatusID("Feed Import OK"))
                    Throw New NYSFriendlyException("Record deleted successfully!", "Info")
                Else
                    pnconfirm.Visible = False
                    Throw New NYSFriendlyException("Record didn't delete! Please try again! ", "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.4 SA
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Using New clslogger(log, className, "btnCancel_Click")
            Try
                txtdataid.Text = ""
                pnconfirm.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.6 SA 
    Protected Sub btnTestFileImportBooked_Click(sender As Object, e As EventArgs) Handles btnTestFileImportBooked.Click
        Using New clslogger(log, className, "btnTestFileImportBooked_Click")
            Try

                Dim csv As New CSVReaderBooked

                If csv.Main() Then
                    Throw New NYSFriendlyException("CSV Booked file import OK", "Info")
                Else
                    Throw New NYSFriendlyException("CSV Booked file import ERROR", "Info")
                End If

            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.6 SA 
    Protected Sub btnTestFileImporTrans_Click(sender As Object, e As EventArgs) Handles btnTestFileImporTrans.Click
        Using New clslogger(log, className, "btnTestFileImporTrans_Click")
            Try
                Dim csv As New CSVReaderTrans

                If csv.Main(Format(Now, "dd-MM-yyyy")) Then
                    Throw New NYSFriendlyException("CSV Trans file import OK", "Info")
                Else
                    Throw New NYSFriendlyException("CSV Trans file import ERROR", "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub
End Class

Partial Public Class dataExportItems
    Public Sub New( _
        ByVal pDataid As String, _
        ByVal pBookerinitials As String, _
        ByVal pstrCref1 As String, _
        ByVal pstrCref2 As String, _
        ByVal pstrCref3 As String, _
        ByVal pPAYNET As String, _
        ByVal pPAXNO As String, _
        ByVal pCRSREF As String, _
        ByVal pstrcustomercode As String, _
        ByVal pCostcode As String, _
        ByVal pPo As String, _
        ByVal pFILEREF As String, _
        ByVal pPRODUCTCODE As String, _
        ByVal pVenuebosscode As String, _
        ByVal pHOTELPHONENO As String, _
        ByVal pCITYOFSERVICE As String, _
        ByVal pSupplierinvoice As String, _
        ByVal pArrivaldate As String, _
        ByVal pDeparturedate As String, _
        ByVal pPassengername As String, _
        ByVal pnotes As String, _
        ByVal pSUPPLIERFARE As String, _
        ByVal pSUPPLIERCHARGE As String, _
        ByVal pSUPPLIERVAT As String, _
        ByVal pAGENCYCHARGE As String, _
        ByVal pAGENCYVAT As String, _
        ByVal pDISCOUNTAMT As String, _
        ByVal pDISCOUNTPERC As String, _
        ByVal pCOMMISSION As String, _
        ByVal pCOMMISSIONPERC As String, _
        ByVal pCOMMISSIONVAT As String, _
        ByVal pBOOKINGONLY As String, _
        ByVal pSavingsCode As String, _
        ByVal pstrCref4 As String, _
        ByVal pstrCref5 As String, _
        ByVal pstrCref6 As String, _
        ByVal pstrCref7 As String, _
        ByVal pstrCref8 As String, _
        ByVal pstrCref9 As String)

        mSavingsCode = pSavingsCode
        mDataid = pDataid
        mBookerinitials = pBookerinitials
        mstrCref1 = pstrCref1
        mstrCref2 = pstrCref2
        mstrCref3 = pstrCref3
        mPAYNET = pPAYNET
        mPAXNO = pPAXNO
        mCRSREF = pCRSREF
        mstrcustomercode = pstrcustomercode
        mCostcode = pCostcode
        mPo = pPo
        mFILEREF = pFILEREF
        mPRODUCTCODE = pPRODUCTCODE
        mVenuebosscode = pVenuebosscode
        mHOTELPHONENO = pHOTELPHONENO
        mCITYOFSERVICE = pCITYOFSERVICE
        mSupplierinvoice = pSupplierinvoice
        mArrivaldate = pArrivaldate
        mDeparturedate = pDeparturedate
        mPassengername = pPassengername
        mnotes = pnotes
        mSUPPLIERFARE = pSUPPLIERFARE
        mSUPPLIERCHARGE = pSUPPLIERCHARGE
        mSUPPLIERVAT = pSUPPLIERVAT
        mAGENCYCHARGE = pAGENCYCHARGE
        mAGENCYVAT = pAGENCYVAT
        mDISCOUNTAMT = pDISCOUNTAMT
        mDISCOUNTPERC = pDISCOUNTPERC
        mCOMMISSION = pCOMMISSION
        mCOMMISSIONPERC = pCOMMISSIONPERC
        mCOMMISSIONVAT = pCOMMISSIONVAT
        mBOOKINGONLY = pBOOKINGONLY
        mstrCref4 = pstrCref4
        mstrCref5 = pstrCref5
        mstrCref6 = pstrCref6
        mstrCref7 = pstrCref7
        mstrCref8 = pstrCref8
        mstrCref9 = pstrCref9
    End Sub

    Private mSavingsCode As String
    Private mDataid As String
    Private mBookerinitials As String
    Private mstrCref1 As String
    Private mstrCref2 As String
    Private mstrCref3 As String
    Private mPAYNET As String
    Private mPAXNO As String
    Private mCRSREF As String
    Private mstrcustomercode As String
    Private mCostcode As String
    Private mPo As String
    Private mFILEREF As String
    Private mPRODUCTCODE As String
    Private mVenuebosscode As String
    Private mHOTELPHONENO As String
    Private mCITYOFSERVICE As String
    Private mSupplierinvoice As String
    Private mArrivaldate As String
    Private mDeparturedate As String
    Private mPassengername As String
    Private mnotes As String
    Private mSUPPLIERFARE As String
    Private mSUPPLIERCHARGE As String
    Private mSUPPLIERVAT As String
    Private mAGENCYCHARGE As String
    Private mAGENCYVAT As String
    Private mDISCOUNTAMT As String
    Private mDISCOUNTPERC As String
    Private mCOMMISSION As String
    Private mCOMMISSIONPERC As String
    Private mCOMMISSIONVAT As String
    Private mBOOKINGONLY As String
    Private mstrCref4 As String
    Private mstrCref5 As String
    Private mstrCref6 As String
    Private mstrCref7 As String
    Private mstrCref8 As String
    Private mstrCref9 As String

    Public Property SavingsCode() As String
        Get
            Return mSavingsCode
        End Get
        Set(ByVal value As String)
            mSavingsCode = value
        End Set
    End Property

    Public Property BOOKINGONLY() As String
        Get
            Return mBOOKINGONLY
        End Get
        Set(ByVal value As String)
            mBOOKINGONLY = value
        End Set
    End Property

    Public Property COMMISSIONVAT() As String
        Get
            Return mCOMMISSIONVAT
        End Get
        Set(ByVal value As String)
            mCOMMISSIONVAT = value
        End Set
    End Property

    Public Property COMMISSIONPERC() As String
        Get
            Return mCOMMISSIONPERC
        End Get
        Set(ByVal value As String)
            mCOMMISSIONPERC = value
        End Set
    End Property

    Public Property COMMISSION() As String
        Get
            Return mCOMMISSION
        End Get
        Set(ByVal value As String)
            mCOMMISSION = value
        End Set
    End Property

    Public Property DISCOUNTPERC() As String
        Get
            Return mDISCOUNTPERC
        End Get
        Set(ByVal value As String)
            mDISCOUNTPERC = value
        End Set
    End Property

    Public Property DISCOUNTAMT() As String
        Get
            Return mDISCOUNTAMT
        End Get
        Set(ByVal value As String)
            mDISCOUNTAMT = value
        End Set
    End Property

    Public Property AGENCYVAT() As String
        Get
            Return mAGENCYVAT
        End Get
        Set(ByVal value As String)
            mAGENCYVAT = value
        End Set
    End Property

    Public Property AGENCYCHARGE() As String
        Get
            Return mAGENCYCHARGE
        End Get
        Set(ByVal value As String)
            mAGENCYCHARGE = value
        End Set
    End Property

    Public Property SUPPLIERVAT() As String
        Get
            Return mSUPPLIERVAT
        End Get
        Set(ByVal value As String)
            mSUPPLIERVAT = value
        End Set
    End Property

    Public Property SUPPLIERCHARGE() As String
        Get
            Return mSUPPLIERCHARGE
        End Get
        Set(ByVal value As String)
            mSUPPLIERCHARGE = value
        End Set
    End Property

    Public Property SUPPLIERFARE() As String
        Get
            Return mSUPPLIERFARE
        End Get
        Set(ByVal value As String)
            mSUPPLIERFARE = value
        End Set
    End Property

    Public Property notes() As String
        Get
            Return mnotes
        End Get
        Set(ByVal value As String)
            mnotes = value
        End Set
    End Property

    Public Property Passengername() As String
        Get
            Return mPassengername
        End Get
        Set(ByVal value As String)
            mPassengername = value
        End Set
    End Property

    Public Property Departuredate() As String
        Get
            Return mDeparturedate
        End Get
        Set(ByVal value As String)
            mDeparturedate = value
        End Set
    End Property

    Public Property Arrivaldate() As String
        Get
            Return mArrivaldate
        End Get
        Set(ByVal value As String)
            mArrivaldate = value
        End Set
    End Property

    Public Property Supplierinvoice() As String
        Get
            Return mSupplierinvoice
        End Get
        Set(ByVal value As String)
            mSupplierinvoice = value
        End Set
    End Property

    Public Property CITYOFSERVICE() As String
        Get
            Return mCITYOFSERVICE
        End Get
        Set(ByVal value As String)
            mCITYOFSERVICE = value
        End Set
    End Property

    Public Property HOTELPHONENO() As String
        Get
            Return mHOTELPHONENO
        End Get
        Set(ByVal value As String)
            mHOTELPHONENO = value
        End Set
    End Property

    Public Property Venuebosscode() As String
        Get
            Return mVenuebosscode
        End Get
        Set(ByVal value As String)
            mVenuebosscode = value
        End Set
    End Property

    Public Property PRODUCTCODE() As String
        Get
            Return mPRODUCTCODE
        End Get
        Set(ByVal value As String)
            mPRODUCTCODE = value
        End Set
    End Property

    Public Property FILEREF() As String
        Get
            Return mFILEREF
        End Get
        Set(ByVal value As String)
            mFILEREF = value
        End Set
    End Property

    Public Property Po() As String
        Get
            Return mPo
        End Get
        Set(ByVal value As String)
            mPo = value
        End Set
    End Property

    Public Property Costcode() As String
        Get
            Return mCostcode
        End Get
        Set(ByVal value As String)
            mCostcode = value
        End Set
    End Property

    Public Property strcustomercode() As String
        Get
            Return mstrcustomercode
        End Get
        Set(ByVal value As String)
            mstrcustomercode = value
        End Set
    End Property

    Public Property CRSREF() As String
        Get
            Return mCRSREF
        End Get
        Set(ByVal value As String)
            mCRSREF = value
        End Set
    End Property

    Public Property PAXNO() As String
        Get
            Return mPAXNO
        End Get
        Set(ByVal value As String)
            mPAXNO = value
        End Set
    End Property

    Public Property PAYNET() As String
        Get
            Return mPAYNET
        End Get
        Set(ByVal value As String)
            mPAYNET = value
        End Set
    End Property

    Public Property strCref3() As String
        Get
            Return mstrCref3
        End Get
        Set(ByVal value As String)
            mstrCref3 = value
        End Set
    End Property

    Public Property strCref2() As String
        Get
            Return mstrCref2
        End Get
        Set(ByVal value As String)
            mstrCref2 = value
        End Set
    End Property

    Public Property strCref1() As String
        Get
            Return mstrCref1
        End Get
        Set(ByVal value As String)
            mstrCref1 = value
        End Set
    End Property

    Public Property Dataid() As String
        Get
            Return mDataid
        End Get
        Set(ByVal value As String)
            mDataid = value
        End Set
    End Property

    Public Property Bookerinitials() As String
        Get
            Return mBookerinitials
        End Get
        Set(ByVal value As String)
            mBookerinitials = value
        End Set
    End Property

    Public Property strCref4() As String
        Get
            Return mstrCref4
        End Get
        Set(ByVal value As String)
            mstrCref4 = value
        End Set
    End Property

    Public Property strCref5() As String
        Get
            Return mstrCref5
        End Get
        Set(ByVal value As String)
            mstrCref5 = value
        End Set
    End Property

    Public Property strCref6() As String
        Get
            Return mstrCref6
        End Get
        Set(ByVal value As String)
            mstrCref6 = value
        End Set
    End Property

    Public Property strCref7() As String
        Get
            Return mstrCref7
        End Get
        Set(ByVal value As String)
            mstrCref7 = value
        End Set
    End Property

    Public Property strCref8() As String
        Get
            Return mstrCref8
        End Get
        Set(ByVal value As String)
            mstrCref8 = value
        End Set
    End Property

    Public Property strCref9() As String
        Get
            Return mstrCref9
        End Get
        Set(ByVal value As String)
            mstrCref9 = value
        End Set
    End Property

End Class