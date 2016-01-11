Option Explicit On
Option Strict On

Imports Cubit.NSConfigUtils
Imports DatabaseObjects
Imports System.Net.Mail

Partial Public Class FeedEdit
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

    ''' <summary>
    ''' Sub Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' 
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load")
            Try
                If Not Me.IsPostBack Then
                    setUser()

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

                    Dim tab2 As New Data.DataTable
                    grdBoss.DataSource = tab2
                    grdBoss.DataBind()

                    Dim intID As Integer = 0
                    Try
                        intID = CInt(Request.QueryString("dataid"))
                    Catch ex As Exception
                        Throw New NYSFriendlyException("Data ID doesn't exist, please return to View screen and reselect", _
                                                       "Info")
                    End Try
                    If intID > 0 Then
                        populateData(intID)

                        'R15 CR
                        'needs to come after populate to check txtgroupname and txtguestname value
                        setAjax()
                    Else
                        Throw New NYSFriendlyException("Data ID doesn't exist, please return to View screen and reselect", _
                                                                          "Info")
                    End If
                Else
                    setAjax()
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub setUser
    ''' </summary>
    ''' <remarks>
    ''' Created 17/03/2009 Nick Massarella
    ''' Sets up logged in user and warns if not allowed to view application
    ''' </remarks>
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
                If oUser.Systemnysuserloginname.ToLower = "nickmassarella" Then
                    btnshowhidden.Visible = True
                Else
                    btnshowhidden.Visible = False
                End If
            Else
                Response.Redirect("FeedMain.aspx")
            End If

        End Using
    End Sub

    ''' <summary>
    ''' Sub populateData
    ''' </summary>
    ''' <param name="pintid"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Connects to DatabaseObjects to retrieve a FeedData record for selected id
    ''' </remarks>
    Private Sub populateData(ByVal pintid As Integer)
        Using New clslogger(log, className, "populateData")
            Dim dr As FeedImportData
            dr = FeedImportData.getForEdit(pintid)
            If dr IsNot Nothing Then
                lblGroupView.Text = dr.GroupName
                txtdataid.Text = notString(dr.Dataid)
                txttransactionnumber.Text = notString(dr.Transactionnumber)
                txttransactionlinenumber.Text = notString(dr.Transactionlinenumber)
                txttransactiondate.Text = Format(dr.Transactiondate, "dd-MM-yyyy")
                txtarrivaldate.Text = Format(dr.Arrivaldate, "dd-MM-yyyy")
                txtdeparturedate.Text = Format(dr.Departuredate, "dd-MM-yyyy")
                txtbookeddate.Text = Format(dr.BookedDate, "dd-MM-yyyy")
                txtguestname.Text = dr.Passengername
                txtguestPNR.Text = dr.GuestPNR
                txtnettamount.Text = notString(dr.Nettamount)
                txtvatamount.Text = notString(dr.Vatamount)
                txtvatrate.Text = notString(dr.Vatrate)
                txttotalamount.Text = notString(dr.Totalamount)
                txtTransactioncode.Text = dr.Transactioncode
                'txtTransactionvalue.Text = notString(dr.Transactionvalue)
                txtTransactionvaluenew.Text = dr.Transactionvaluenew
                Dim fs As New FeedStatus
                fs = FeedStatus.get(CInt(dr.Statusid))
                txtstatusname.Text = fs.Statusname
                txtCategoryname.Text = dr.Categoryname
                txtCategorybosscode.Text = dr.Categorybosscode
                txtVenuereferencehidden.Text = notString(dr.Venuereference)
                txtVenuereference.Text = notString(dr.Venuereference)
                txtvenuename.Text = dr.Venuename
                txtvenuenamehidden.Text = dr.Venuename
                txtgroupid.Text = CStr(dr.Groupid)
                txtgroupname.Text = dr.GroupName
                txtEX.Text = notString(dr.VenueEX)
                txtDD.Text = notString(dr.VenueDD)
                txtVenuebosscode.Text = dr.Venuebosscode
                txtVenuebosscodehidden.Text = dr.Venuebosscode
                txtvenuedetails.Text = dr.Venuedetails.Replace("/", vbCrLf).Replace(";", vbCrLf)
                txtconfermainvoiceno.Text = notString(dr.Confermainvoicenumber)
                txtsupplierinvoice.Text = dr.Supplierinvoice
                txtref1.Text = dr.Ref1
                txtdept.Text = dr.Dept
                txtroomdetails.Text = dr.Roomdetails

                'R2.21 CR
                txttravellerEmail.Text = dr.TravellerEmail

                If txtCategorybosscode.Text = "AC" Then
                    txtVenuecommision.Text = txtDD.Text
                Else
                    txtVenuecommision.Text = txtEX.Text
                End If

                If dr.Transactionvalue.HasValue Then
                    txtTransactionvalue.Text = notString(dr.Transactionvalue)
                End If
                txtTransactionvaluenew.Text = CStr(dr.Transactionvaluenew)

                btnfind.Visible = True

                If txtVenuebosscode.Text = "" Then
                    txtVenuebosscode.ReadOnly = False
                End If

                If txtstatusname.Text.ToLower = "feed import error" Or _
                    txtstatusname.Text.ToLower = "boss export error" Or _
                    txtstatusname.Text.ToLower = "incomplete" Then
                    txtfail.Text = dr.Failreason
                    txtfail.Visible = True
                    lblfail.Visible = True
                Else
                    txtfail.Visible = False
                    lblfail.Visible = False
                End If
                If txtstatusname.Text = "Completed" Or _
                    txtstatusname.Text = "Incomplete" Then
                    pnhide.Visible = True
                    btnclose.Visible = True
                    btnsave.Visible = False
                    btncancel.Visible = False
                    btnfind.Visible = False
                    getBossValues(CInt(dr.Transactionnumber))
                    pnboss.Visible = True
                Else
                    pnhide.Visible = False
                    btnclose.Visible = False
                    btnsave.Visible = True
                    btncancel.Visible = True
                    btnfind.Visible = True
                End If

                If txtstatusname.Text.ToLower = "boss export error" Then
                    btnreset.Visible = True
                    txtstatusname.Style.Item("width") = "270px"
                Else
                    btnreset.Visible = False
                    txtstatusname.Style.Item("width") = "360px"
                End If

                txtaicol6.Text = dr.AICol6
                txtaicol7.Text = dr.AICol7
                txtaicol8.Text = dr.AICol8
                txtaicol9.Text = dr.AICol9
                txtaicol10.Text = dr.AICol10
                txtaicostcode.Text = dr.Costcode
                txtcostcodehidden.Text = dr.Costcode

                'set the labels up so all looks OK to user, but we're not actually swapping any fields
                populateClientOptions(CInt(dr.Groupid))

                txtbooker.Text = dr.Booker
                txtbookerinitials.Text = dr.Bookerinitials
                txtref2.Text = dr.Ref2 'bill instructions
                txtref3.Text = dr.Ref3 'comments
                txtcurrency.Text = dr.Currency

                chkExcludeExport.Checked = notBoolean(dr.ExcludeFromExport)


                If dr.GroupName.ToLower = "university of york" Then
                    If dr.ccVal.ToUpper = "YNIC" Then
                        txtaicostcode.Text = "UNI YORK"
                        txtcostcodehidden.Text = "UNI YORK"
                    ElseIf dr.ccVal.ToUpper = "YEMC" Or dr.ccVal.ToUpper = "EMC" Then
                        txtaicostcode.Text = "EMC"
                        txtcostcodehidden.Text = "EMC"
                    End If
                ElseIf dr.GroupName = "LV=" Then
                    'R2.20D CR - lv traveller updates
                    'just disable the boxes

                    If getConfig("LVTravellerCheckLive") = "true" Then
                        txtaicol6.Enabled = False
                        txtaicol7.Enabled = False
                        txtaicostcode.Enabled = False
                    End If

                ElseIf dr.GroupName.ToLower = "cima" Then
                    'Dim ret As New getCimaRet
                    'If dr.ccVal <> "" Then
                    '    Try
                    '        Dim intTest As Integer = CInt(Mid(dr.ccVal, 1, 3))
                    '        ret = getCimaDetails(CInt(Mid(dr.ccVal, 1, 3)))
                    '        txtaicol6.Text = ret.strcref1
                    '        txtcref2.Text = ret.strcref2
                    '    Catch ex As Exception
                    '        txtaicol6.Text = ""
                    '        txtcref2.Text = ""
                    '    End Try
                    'End If
                ElseIf dr.GroupName.ToLower = "npsa" Then
                    If txtaicol6.Text.ToLower = "n/a" Or _
                        txtaicol6.Text.ToLower = "" Or _
                        txtaicol6.Text.ToLower = "." Or _
                        txtaicol6.Text.ToLower = "-" Or _
                        txtaicol6.Text.ToLower = "_" Or _
                        txtaicol6.Text.ToLower = "na" Then
                        txtaicol6.Text = "none"
                    End If
                    'txtpo.ReadOnly = False
                ElseIf dr.GroupName.ToLower = "herefordshire council" Then
                    'R15 CR
                    ' txtpo.ReadOnly = False
                ElseIf dr.GroupName.ToLower = "east of england" Then
                    If dr.ccVal.ToLower.StartsWith("cgz79") Then

                    ElseIf dr.ccVal.ToLower = "e9279" Then
                        ' txtcref2.Text = dr.AICol6
                    Else
                        ' txtcref2.Text = ""
                    End If
                ElseIf dr.GroupName.ToLower = "university hospital of south manchester" Then
                    'If dr.ccVal <> "" Then
                    '    txtaicol6.Text = clsGroup.UHSMCostCodeCheck(Mid(dr.Costcode, 1, 1), CInt(getConfig("DBType")))
                    'End If
                ElseIf dr.GroupName.ToLower = "o2" Then
                    '  txtpo.ReadOnly = False
                End If

                If dr.complaintsText <> "" Then
                    lblComplaints.Text = dr.complaintsText
                End If

                'R2.15 NM
                'added to override the commission rate if the booking comes from LateRooms.com
                'R2.21.3 SA - changed label name from lblLR to lblVenueComm
                lblVenueComm.Visible = False
                Dim strRet As String = FeedImportData.isLateRooms(CInt(dr.Transactionnumber))
                If strRet.ToUpper = "LATEROOMS.COM" Then
                    If getConfig("LateRoomsFee") <> "" Then
                        lblVenueComm.Text = "LR"
                        lblVenueComm.Visible = True
                    End If
                    'R2.21.3 SA 
                ElseIf strRet.ToUpper = "EXPEDIA" Or strRet.ToUpper = "EXPEDIA AFFILIATE" Then
                    If getConfig("ExpediaFee") <> "" Then
                        lblVenueComm.Text = "EXP"
                        lblVenueComm.Visible = True
                    End If
                End If

                'No need to do this! it casused problems!!
                ''R2.21.3 SA - 
                ''disable commission box for laterooms and expedia
                'If strRet.ToUpper = "LATEROOMS.COM" Or strRet.ToUpper = "EXPEDIA" Then
                '    txtVenuecommision.Enabled = False
                'End If

            End If
        End Using
    End Sub

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

    Private Sub populateClientOptions(ByVal pGroupID As Integer)
        Using New clslogger(log, className, "populateClientOptions")
            Dim oP As clsClientOption
            oP = clsClientOption.get(pGroupID)

            If Not oP Is Nothing Then
                If oP.aicostcodeValue <> "" Then
                    lblaicostcode.Text = oP.aicostcodeValue
                    lblaicostcode.Visible = True
                    txtaicostcode.Visible = True
                Else
                    lblaicostcode.Visible = False
                    txtaicostcode.Visible = False
                End If
                If oP.aicol6Value <> "" Then
                    lblaicol6.Text = oP.aicol6Value
                    lblaicol6.Visible = True
                    txtaicol6.Visible = True
                Else
                    lblaicol6.Visible = False
                    txtaicol6.Visible = False
                End If
                If oP.aicol7Value <> "" Then
                    lblaicol7.Text = oP.aicol7Value
                    lblaicol7.Visible = True
                    txtaicol7.Visible = True
                Else
                    lblaicol7.Visible = False
                    txtaicol7.Visible = False
                End If
                If oP.aicol8Value <> "" Then
                    lblaicol8.Text = oP.aicol8Value
                    lblaicol8.Visible = True
                    txtaicol8.Visible = True
                Else
                    lblaicol8.Visible = False
                    txtaicol8.Visible = False
                End If
                If oP.aicol9Value <> "" Then
                    lblaicol9.Text = oP.aicol9Value
                    lblaicol9.Visible = True
                    txtaicol9.Visible = True
                Else
                    lblaicol9.Visible = False
                    txtaicol9.Visible = False
                End If
                If oP.aicol10Value <> "" Then
                    lblaicol10.Text = oP.aicol10Value
                    lblaicol10.Visible = True
                    txtaicol10.Visible = True
                Else
                    lblaicol10.Visible = False
                    txtaicol10.Visible = False
                End If
            End If

        End Using
    End Sub

    Private Sub getBossValues(ByVal pintTransactionnumber As Integer)
        Using New clslogger(log, className, "getBossValues")
            Dim fi As New FeedInvoice
            fi = FeedInvoice.FeedInvoiceBossGet(pintTransactionnumber)
            txtbossid.Text = fi.bossid
            txtbosstotal.Text = CStr(clsNYS.notNumber(fi.bosstotal))
            txtbosscomNett.Text = CStr(clsNYS.notNumber(fi.bosscomNett))
            txtbosscomVat.Text = CStr(clsNYS.notNumber(fi.bosscomVat))

            Dim tab As Data.DataTable
            tab = NSUtils.GetTable(FeedInvoice.FeedInvoiceBossGetAll(pintTransactionnumber))

            Dim dv As New Data.DataView(tab)
            grdBoss.DataSource = tab
            grdBoss.DataBind()

            Dim ocol As New System.Drawing.ColorConverter
            Dim col As System.Drawing.Color
            col = CType(ocol.ConvertFromString("#FF3300"), Drawing.Color)
            grdBoss.RowHighlightColor = col

            For Each dgiCount As DataGridItem In grdBoss.Items
                If grdBoss.Items(dgiCount.ItemIndex).Cells(0).Text = "Totals:" Then
                    grdBoss.Items(dgiCount.ItemIndex).CssClass = "gridAlternateTwo"
                End If
            Next
        End Using
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btncancel.Click
        Using New clslogger(log, className, "btncancel_Click")
            Try
                Response.Redirect("FeedMain.aspx?dataid=" & txtdataid.Text)
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnsave.Click
        Using New clslogger(log, className, "btnsave_Click")
            Try
                If Not chkExcludeExport.Checked Then
                    Try
                        If txtTransactionvaluenew.Text <> "" Then
                            Dim dbltemp As Double = CDbl(txtTransactionvaluenew.Text)
                        End If
                    Catch ex As Exception
                        Throw New NYSFriendlyException("Please ensure 'Edited transaction value'" & _
                                                       " is a correct numeric value to save!", "Info")
                    End Try
                    Try
                        Dim dbltemp As Double = CDbl(txtVenuecommision.Text)
                    Catch ex As Exception
                        Throw New NYSFriendlyException("Please ensure 'Venue commission'" & _
                                                       " is a correct numeric value to save!", "Info")
                    End Try

                    If txtgroupname.Text.ToLower = "east of england" Then
                        If clsGroup.EOECostCentreCheck(txtaicostcode.Text) = "" Then
                            Throw New NYSFriendlyException("The Costcode is not in the EOE Costcentre table, please check the Cost centre. If correct please contact the Helpdesk to add the Cost centre to the database.", "Info")
                        End If
                    ElseIf txtgroupname.Text.ToLower.Contains("anchor trust") Then
                        If clsGroup.AnchorCostCodeCheck(txtaicostcode.Text, CInt(getConfig("DBType"))) <> 1 Then
                            Throw New NYSFriendlyException("The Costcode is not in the Anchor Costcode table, please check the Costcode. If correct please contact the Helpdesk to add the Costcode to the database.", "Info")
                        End If
                        'R16 NM
                    ElseIf txtgroupname.Text.ToLower = "university hospital of south manchester" Then
                        If txtaicostcode.Text <> "" Then
                            'first check costcode exists
                            If clsGroup.UHSMCostCodeCheckExists(txtaicostcode.Text) = 0 Then
                                Throw New NYSFriendlyException("Costcode is not in the UHSM Costcode table, please check the Costcode. If correct please contact the Helpdesk to add the Costcode to the database.", "Info")
                            Else
                                If clsGroup.UHSMCostCodeCheck(Mid(txtaicostcode.Text, 1, 1), CInt(getConfig("DBType"))) = "" Then
                                    Throw New NYSFriendlyException("The first character of the Costcode is not in the UHSM Costcode Division table, please contact the Helpdesk to check the Costcode!", "Info")
                                End If
                            End If
                        Else
                            Throw New NYSFriendlyException("The Costcode must have at least one character to map the Division for CREF1, please contact the Helpdesk to check the Costcode!", "Info")
                        End If

                        'R2.20G SA 
                    ElseIf txtgroupname.Text.ToLower = "nhs leadership academy" Then
                        If txtaicostcode.Text <> "" Then
                            'check if the code exists 
                            If clsGroup.NHSLACostCodeCheckExists(txtaicostcode.Text) = 0 Then
                                Throw New NYSFriendlyException("Costcode is not in the NHSLA Costcode table, please check the Costcode. If correct please contact the Helpdesk to add the Costcode to the database.", "Info")
                            End If
                        Else
                            Throw New NYSFriendlyException("The Costcode must have a value, please provide a Costcode!", "Info")
                        End If
                        If txtaicol6.Text <> "" Then
                            'check if expense type exists
                            If clsGroup.NHSLAExpenseTypeExists(txtaicol6.Text) = 0 Then
                                Throw New NYSFriendlyException("Expense type is not in the NHSLA Expense type table, please check the Expense type. If correct please contact the Helpdesk to add the Expense type to the database.", "Info")
                            End If
                        Else
                            Throw New NYSFriendlyException("The expense type must have a vlaue, please provide an Expense Type!", "Info")
                        End If

                    ElseIf txtgroupname.Text.ToLower.Contains("lv=") Then

                        Dim blnError As Boolean = False

                        'if the new traveller system is active for LV, disable the boxes and enter some text to keep the user's mind at ease!
                        If getConfig("LVTravellerCheckLive") = "true" Then
                            'no error checking - aicol6, aicol7, aicostcentre all get populated on export

                            'otherwise do the old validation checks
                        Else
                            If txtaicol6.Text.Length = 7 Then
                                Try
                                    Dim inttest As Integer = CInt(txtaicol6.Text)
                                Catch ex As Exception
                                    blnError = True
                                End Try
                            ElseIf txtaicol6.Text.Length = 8 Then
                                Dim strEmployeeNumber As String = txtaicol6.Text
                                If strEmployeeNumber.ToLower.StartsWith("m") Then
                                    strEmployeeNumber = strEmployeeNumber.ToLower.Replace("m", "")
                                    Try
                                        Dim inttest As Integer = CInt(strEmployeeNumber)
                                    Catch ex As Exception
                                        blnError = True
                                    End Try
                                Else
                                    blnError = True
                                End If
                            Else
                                blnError = True
                            End If

                            If blnError Then
                                Throw New NYSFriendlyException("The Employee Number is incorrect, it should be 7 numbers or 7 numbers prefixed with M, please contact the Helpdesk to check the Costcode!", "Info")
                            End If

                            'R2.5 NM
                            If clsCode.checkLVCode(txtaicostcode.Text) = 1 Then
                                If txtaicol7.Text.ToUpper.Trim = "" Or txtaicol7.Text.ToUpper.Trim = "." Or txtaicol7.Text.ToUpper.Trim = "N/A" Or txtaicol7.Text.ToUpper.Trim = "NONE" Then
                                    Throw New NYSFriendlyException("This booking has an ACK code that requires a valid Project Code.", "Info")
                                End If
                            End If

                        End If

                        'R15 CR
                        'validation for herefordshire council Po Code
                    ElseIf txtgroupname.Text.ToLower = "herefordshire council" Then

                        If txtaicostcode.Text.Length <> 6 Then
                            Throw New NYSFriendlyException("The Cost centre is incorrect, it should be 6 characters only, please contact the Helpdesk to check the code!", "Info")
                        End If

                    ElseIf txtgroupname.Text.ToLower = "nhs institute for innovation and improvement" Then

                        'R2.17 CR
                        'If txtaicostcode.Text.Length < 5 Or txtaicostcode.Text.Length > 9 Then
                        '    Throw New NYSFriendlyException("The Project Code must be between 5 & 9 alphanumeric characters long!", "Info")
                        'End If
                        If txtaicostcode.Text.Length < 1 Or txtaicostcode.Text.Length > 12 Then
                            Throw New NYSFriendlyException("The Project Code must be up to a maximum of 12 alphanumeric characters long!", "Info")
                        End If

                    ElseIf txtgroupname.Text.ToLower = "cima" Then
                        'first check cost centre exists
                        Dim ret As New getCimaRet
                        If txtaicostcode.Text <> "" Then
                            Try
                                Dim intTest As Integer = CInt(Mid(txtaicostcode.Text, 1, 3))
                                ret = getCimaDetails(CInt(Mid(txtaicostcode.Text, 1, 3)))
                                If ret.strcref1 = "" Then
                                    Throw New NYSFriendlyException("CIMA cost centre (first 3 digits of the cost code) doesn't exist in the database, please ensure cost code is correct!", "Info")
                                End If
                            Catch ex As Exception
                                Throw New NYSFriendlyException("CIMA cost centre (first 3 digits of the cost code) doesn't exist in the database, please ensure cost code is correct!", "Info")
                            End Try

                            'now check format
                            Dim mRegExp As New Regex(clsGroup.CostCodeExpGet(CInt(txtgroupid.Text)))

                            If Not mRegExp.IsMatch(txtaicostcode.Text) Then
                                Throw New NYSFriendlyException("Please ensure the cost code is in the " & _
                                            "correct format, i.e. '" & clsGroup.CostCodeExpFormatGet _
                                            (CInt(txtgroupid.Text)) & "'!", "Info")
                            End If
                        Else
                            Throw New NYSFriendlyException("CIMA cost code cannot be empty!", "Info")
                        End If
                    ElseIf txtgroupname.Text.ToLower = "o2" Then
                        If Not clsPo.checkPOExists(txtaicostcode.Text) Then
                            Throw New NYSFriendlyException("The PO does not exist in the O2 PO table, please check.", "Info")
                        End If
                        Dim strRet As String = clsNYS.checkPo(txtaicostcode.Text, True, True, FeedImportData.exportValueGet(CInt(txttransactionnumber.Text)))
                        If strRet <> "" Then
                            Throw New NYSFriendlyException(strRet, "Info")
                        End If
                        'R2.5 NM
                    ElseIf txtgroupname.Text.ToLower = "doncaster council" Then
                        If Not clsGroup.DCCostCodeExists(txtaicostcode.Text) Then
                            Throw New NYSFriendlyException("The Cost code does not exist in the Doncaster Council CC table, please check.", "Info")
                        End If
                    Else
                        'R2.12 CR - NPSA don't want this check anymore
                        'If txtgroupname.Text.ToLower = "npsa" Then
                        '    If clsGroup.NPSAProjectCodeExists(txtaicol6.Text) = 0 Then
                        '        Throw New NYSFriendlyException("The Project Code does not exist, check if correct, if so contact the helpdesk to add value to database!", "Info")
                        '    End If
                        'End If
                        Dim mRegExp As New Regex(clsGroup.CostCodeExpGet(CInt(txtgroupid.Text)))

                        If Not mRegExp.IsMatch(txtaicostcode.Text) Then
                            Throw New NYSFriendlyException("Please ensure the cost code is in the " & _
                                        "correct format, i.e. '" & clsGroup.CostCodeExpFormatGet _
                                        (CInt(txtgroupid.Text)) & "'!", "Info")
                        End If
                    End If
                End If

                'R2.21.3 SA changed label name from lblLR to lblVenueComm
                'R2.15 NM
                'added to override the commission rate if the booking comes from LateRooms.com
                If lblVenueComm.Visible Then
                    If getConfig("LateRoomsFee") <> "" Then
                        If CDec(txtVenuecommision.Text) <> CDec(getConfig("LateRoomsFee")) Then
                            Throw New NYSFriendlyException("This is a LateRooms.com booking, the commission rate must be " & getConfig("LateRoomsFee") & "%.", "Info")
                        End If
                    End If
                    'R2.21.3 SA 
                    If getConfig("ExpediaFee") <> "" Then
                        If CDec(txtVenuecommision.Text) <> CDec(getConfig("ExpediaFee")) Then
                            Throw New NYSFriendlyException("This is an Expedia booking, the commission rate must be " & getConfig("ExpediaFee") & "%.", "Info")
                        End If
                    End If

                End If

                Dim commissionEX As Double = 0
                Dim commissionDD As Double = 0
                If txtCategorybosscode.Text = "AC" Then
                    commissionDD = CDbl(txtVenuecommision.Text)
                    commissionEX = CDbl(txtEX.Text)
                Else
                    commissionEX = CDbl(txtVenuecommision.Text)
                    commissionDD = CDbl(txtDD.Text)
                End If

                'R2.7 NM avoid venues with IBIS in name
                If Not chkExcludeExport.Checked Then
                    If txtVenuereference.Text = "0" And Not txtvenuename.Text.ToLower.Contains("ibis") Then
                        lblfeed.Text = "VENUE - This venue has not been matched to a venue in VenuesDB, are you sure you want to save this?"
                        pnsure.Visible = True
                        Exit Sub
                    End If
                End If

                If txtvenuename.Text.ToLower.Contains("premier inn") Or _
                   txtvenuename.Text.ToLower.Contains("travelodge") Or _
                   txtvenuename.Text.ToLower.Contains("premier travel inn") Or _
                   txtvenuename.Text.ToLower.Contains("ibis") Then

                Else
                    'R2.7 NM avoid venues with IBIS in name and then check if NON COMM in VDB
                    If Not chkExcludeExport.Checked Then
                        If txtVenuecommision.Text = "0" Then
                            If Not clsVenue.venueCheckNonComm(CInt(txtVenuereference.Text)) Then
                                lblfeed.Text = "COMMISSION - are you sure this Venue is non-commissionable?"
                                pnsure.Visible = True
                                Exit Sub
                            End If
                        End If
                    End If
                End If

                'R15 CR
                Dim hasSaved As Boolean
                Dim savepo As Integer = 0


                hasSaved = saveValue(txtTransactionvaluenew.Text, txtvenuename.Text, CInt(txtVenuereference.Text), txtVenuebosscode.Text, _
                            commissionEX, commissionDD, txtaicostcode.Text, txtaicol6.Text, txtaicol7.Text, txtaicol8.Text, txtaicol9.Text, txtaicol10.Text, txttravellerEmail.Text)


                If hasSaved Then
                    'CR
                    'Set all records with same transaction number to same export status
                    FeedImportData.saveExcludeFromExport(txtdataid.Text, chkExcludeExport.Checked)

                    'now check if VenueDB and Conferma names are same, 
                    'if not save to Alternate table so don't have to look up again
                    checkVenueName(commissionEX, commissionDD)
                    'now check VenuesDB bosscode, if blank add newly entered value
                    If txtVenuebosscodehidden.Text = "" And _
                        txtVenuebosscode.Text <> "" And _
                        CInt(txtVenuereference.Text) <> 0 Then
                        clsVenue.saveBossCode(CInt(txtVenuereference.Text), txtVenuebosscode.Text)
                    End If
                    'R16 NM
                    'update all records where the transaction number & passenger match- saves users having to do each line
                    FeedImportData.saveAllRelated(CInt(txtdataid.Text), savepo)

                    Response.Redirect("FeedMain.aspx?dataid=" & txtdataid.Text)
                Else
                    Throw New NYSFriendlyException("Data didn't save correctly, please check " & _
                                                    "values and try again", "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub checkVenueName
    ''' </summary>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Checks if venuename hidden and venuename fields have same value
    '''  if not the data is saved in the VenueAlternate table if it doesn't already exist
    ''' </remarks>
    Private Sub checkVenueName(ByVal pdblEX As Double, ByVal pdblDD As Double)
        Using (New clslogger(log, className, "checkVenueName"))
            If txtvenuenamehidden.Text <> txtvenuename.Text Or _
               txtVenuereferencehidden.Text <> txtVenuereference.Text Then
                If clsVenue.save(CInt(txtVenuereference.Text), txtvenuename.Text, _
                                 txtvenuenamehidden.Text, txtVenuebosscode.Text, _
                                 CInt(lbluserid.Text), pdblDD, pdblEX) > 0 Then
                    'cool
                Else
                    log.Error(txtvenuename.Text & " did not save to database")
                End If
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Function saveValue
    ''' </summary>
    ''' <param name="pstrvalue"></param>
    ''' <param name="pstrname"></param>
    ''' <param name="pintreference"></param>
    ''' <param name="pstrbosscode"></param>
    ''' <param name="pdblcommissionEX"></param>
    ''' <param name="pdblcommissionDD"></param>
    ''' <param name="pstrcostcode"></param>
    ''' <returns>
    ''' True if data saved correctly
    ''' </returns>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Saves user entered values to database and checks if data saved correctly
    ''' R15 CR - added optional pstrpocode 
    ''' </remarks>
    Private Function saveValue(ByVal pstrvalue As String, ByVal pstrname As String, ByVal pintreference As Integer, ByVal pstrbosscode As String, _
                              ByVal pdblcommissionEX As Double, ByVal pdblcommissionDD As Double, ByVal pstrcostcode As String, ByVal pstraicol6 As String, _
                              ByVal pstraicol7 As String, ByVal pstraicol8 As String, ByVal pstraicol9 As String, ByVal pstraicol10 As String, ByVal pstrTravellerEmail As String) As Boolean
        Using (New clslogger(log, className, "saveValue"))
            If FeedImportData.savevalue(CInt(lbluserid.Text), CInt(txtdataid.Text), pstrvalue, pstrname, _
                                  pintreference, pstrbosscode, pdblcommissionEX, pdblcommissionDD, _
                                  pstrcostcode, pstraicol6, pstraicol7, pstraicol8, pstraicol9, pstraicol10, pstrTravellerEmail) > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    ''' <summary>
    ''' Sub btnfind_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Button to locate Venue from edit screen, by name from VenuesDB and display possible matches
    ''' </remarks>
    Protected Sub btnfind_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnfind.Click
        Using New clslogger(log, className, "btnfind_Click")
            Try
                txtDatavenuenameFind.Text = txtvenuename.Text
                findVenue(Me.txtvenuename.Text, True)
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnfind2_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Button to locate Venue from find screen, by name from VenuesDB and display possible matches
    ''' </remarks>
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
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Function findVenue
    ''' </summary>
    ''' <param name="pstrvenuename"></param>
    ''' <param name="pblnfromedit"></param>
    ''' <remarks>
    ''' Created 16/03/2009 Nick Massarella
    ''' Connects to dababaseobjects to search for venues using data from screen or user inputted
    ''' </remarks>
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
            tab = NSUtils.GetTable(clsVenue.Populate(txtDatavenuename1.Text.Replace("'", "''"), txtDatavenuename2.Text.Replace("'", "''"), _
                                                     btnAndOr.Text, txtgroupid.Text))
            Dim dv As New Data.DataView(tab)
            If sortCriteria() IsNot Nothing AndAlso sortCriteria <> "" Then
                dv.Sort = sortCriteria & " " & sortDir '& ", enquiryeventdate"
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
                    txtVenuebosscode.Text = e.Item.Cells(7).Text
                    txtVenuebosscode.Text = txtVenuebosscode.Text.Replace("&nbsp;", "")
                    'todo need to get correct commission rate
                    If txtCategorybosscode.Text = "DD" Or txtCategorybosscode.Text = "AC" Then
                        If e.Item.Cells(14).Text.Replace("&nbsp;", "") <> "" Then
                            txtVenuecommision.Text = e.Item.Cells(14).Text.Replace("&nbsp;", "")
                        ElseIf e.Item.Cells(15).Text.Replace("&nbsp;", "") <> "" Then
                            txtVenuecommision.Text = e.Item.Cells(15).Text.Replace("&nbsp;", "")
                        ElseIf e.Item.Cells(16).Text.Replace("&nbsp;", "") <> "" Then
                            txtVenuecommision.Text = e.Item.Cells(16).Text.Replace("&nbsp;", "")
                        Else
                            txtVenuecommision.Text = e.Item.Cells(11).Text
                        End If
                    Else
                        txtVenuecommision.Text = e.Item.Cells(12).Text
                    End If
                    txtVenuecommision.Text = txtVenuecommision.Text.Replace("&nbsp;", "")

                    'put values in hidden fields so can use when updating other records
                    txtDD.Text = e.Item.Cells(11).Text
                    txtEX.Text = e.Item.Cells(12).Text

                    If txtVenuebosscode.Text = "" Then
                        txtVenuebosscode.ReadOnly = False
                    End If
                    If txtVenuecommision.Text = "" Then
                        txtVenuecommision.ReadOnly = False
                    End If
                    txtDatavenuenameFind.Text = ""
                    txtDatavenuename1.Text = ""
                    txtDatavenuename2.Text = ""
                    pnvenue.Visible = False
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
    ''' Sub grdVenue_SortCommand
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 17/03/2009 Nick Massarella
    ''' Sorts the grdVenue depending upon last sort request
    ''' </remarks>
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
                    handleException(ex, "FeedEdit", Page)
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

    ''' <summary>
    ''' Sub btnfindcancel_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 17/03/2009 Nick Massarella
    ''' Hides venue search panel
    ''' </remarks>
    Protected Sub btnfindcancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnfindcancel.Click
        Using New clslogger(log, className, "btnfindcancel_Click")
            Try
                txtDatavenuenameFind.Text = ""
                txtDatavenuename1.Text = ""
                txtDatavenuename2.Text = ""
                pnvenue.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnAndOr_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 17/03/2009 Nick Massarella
    ''' Allows user to search on either And/Or depending on button text
    ''' </remarks>
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
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnnovenue_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 17/03/2009 Nick Massarella
    ''' Hides venue search panel and puts venue fields into edit mode to allow user to manually change values
    ''' </remarks>
    Protected Sub btnnovenue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnnovenue.Click
        Using New clslogger(log, className, "btnnovenue_Click")
            Try
                txtVenuereference.Text = "0"
                txtVenuebosscode.Text = ""
                txtVenuebosscode.ReadOnly = False
                txtVenuecommision.Text = ""
                txtVenuecommision.ReadOnly = False

                txtDatavenuenameFind.Text = ""
                txtDatavenuename1.Text = ""
                txtDatavenuename2.Text = ""
                pnvenue.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnclose_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 30/03/2009 Nick Massarella
    ''' Returns user to FeedMain - only available when viewing completed records
    ''' </remarks>
    Private Sub btnclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnclose.Click
        Using New clslogger(log, className, "btnclose_Click")
            Try
                Session.Remove("Sent")
                Response.Redirect("FeedMain.aspx?dataid=" & txtdataid.Text)
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btnshowhidden_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 30/03/2009 Nick Massarella
    ''' User button to show hidden fields - button only visible to nickmassarella
    ''' </remarks>
    Protected Sub btnshowhidden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnshowhidden.Click
        Using New clslogger(log, className, "btnshowhidden_Click")
            Try
                If txtdataid.Visible = True Then
                    txtdataid.Visible = False
                    txtVenuebosscodehidden.Visible = False
                    txtDD.Visible = False
                    txtEX.Visible = False
                    txtcostcodehidden.Visible = False
                    txtvenuenamehidden.Visible = False
                Else
                    txtdataid.Visible = True
                    txtVenuebosscodehidden.Visible = True
                    txtDD.Visible = True
                    txtEX.Visible = True
                    txtcostcodehidden.Visible = True
                    txtvenuenamehidden.Visible = True
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnreset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnreset.Click
        Using New clslogger(log, className, "btnreset_Click")
            Try
                FeedImportData.saveStatus(CInt(txtdataid.Text), FeedStatus.getStatusID("Feed Import OK"))
                txtstatusname.Text = "Feed Import OK"
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnno_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnno.Click
        Using New clslogger(log, className, "btnno_Click")
            Try
                Session.Remove("Sent")
                pnsure.Visible = False
                lblfeed.Text = ""
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnyes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnyes.Click
        Using New clslogger(log, className, "btnyes_Click")
            Try
                Dim commissionEX As Double = 0
                Dim commissionDD As Double = 0
                If txtCategorybosscode.Text = "AC" Then
                    commissionDD = CDbl(txtVenuecommision.Text)
                    commissionEX = CDbl(txtEX.Text)
                Else
                    commissionEX = CDbl(txtVenuecommision.Text)
                    commissionDD = CDbl(txtDD.Text)
                End If
                If lblfeed.Text.StartsWith("VENUE") Then
                    sendVenueEmail("webforms@nysgroup.com", getConfig("VenueEmail"), _
                                        "CUBIT - venue not matched", _
                                           "Venue: " & txtvenuename.Text & vbCrLf & _
                                           "Client: " & lblGroupView.Text & vbCrLf & _
                                           "Transaction No: " & txttransactionnumber.Text & vbCrLf & _
                                           "has not been matched or does not exist in VenuesDB")
                    If txtVenuecommision.Text = "0" Then
                        lblfeed.Text = "COMMISSION - are you sure this Venue is non-commissionable?"
                        pnsure.Visible = True
                        Exit Sub
                    End If

                    If saveValue(txtTransactionvaluenew.Text, txtvenuename.Text, CInt(txtVenuereference.Text), txtVenuebosscode.Text, commissionEX, _
                                 commissionDD, txtaicostcode.Text, txtaicol6.Text, txtaicol7.Text, txtaicol8.Text, txtaicol9.Text, txtaicol10.Text, txttravellerEmail.Text) Then
                        'R17 CR
                        'Set all records with same transaction number to same export status
                        FeedImportData.saveExcludeFromExport(txtdataid.Text, chkExcludeExport.Checked)

                        'now check if VenueDB and Conferma names are same, 
                        'if not save to Alternate table so don't have to look up again
                        checkVenueName(commissionEX, commissionDD)
                        'now check VenuesDB bosscode, if blank add newly entered value
                        If txtVenuebosscodehidden.Text = "" And _
                            txtVenuebosscode.Text <> "" And _
                            CInt(txtVenuereference.Text) <> 0 Then
                            clsVenue.saveBossCode(CInt(txtVenuereference.Text), txtVenuebosscode.Text)
                        End If
                        Response.Redirect("FeedMain.aspx?dataid=" & txtdataid.Text)
                    Else
                        Throw New NYSFriendlyException("Data didn't save correctly, please check " & _
                                                       "values and try again", "Info")
                    End If
                Else
                    If saveValue(txtTransactionvaluenew.Text, txtvenuename.Text, CInt(txtVenuereference.Text), txtVenuebosscode.Text, commissionEX, _
                                 commissionDD, txtaicostcode.Text, txtaicol6.Text, txtaicol7.Text, txtaicol8.Text, txtaicol9.Text, txtaicol10.Text, txttravellerEmail.Text) Then
                        'R17 CR
                        'Set all records with same transaction number to same export status
                        FeedImportData.saveExcludeFromExport(txtdataid.Text, chkExcludeExport.Checked)

                        'now check if VenueDB and Conferma names are same, 
                        'if not save to Alternate table so don't have to look up again
                        checkVenueName(commissionEX, commissionDD)
                        'now check VenuesDB bosscode, if blank add newly entered value
                        If txtVenuebosscodehidden.Text = "" And _
                            txtVenuebosscode.Text <> "" And _
                            CInt(txtVenuereference.Text) <> 0 Then
                            clsVenue.saveBossCode(CInt(txtVenuereference.Text), txtVenuebosscode.Text)
                        End If
                        'R15 NM
                        'added category name so email only goes on rooms with no commission
                        If txtVenuecommision.Text = "0" And txtCategoryname.Text.Trim.ToLower = "room" Then
                            sendVenueEmail("webforms@nysgroup.com", getConfig("VenueEmail"), _
                                           "CUBIT - venue not matched", _
                                           "Venue: " & txtvenuename.Text & vbCrLf & _
                                           "Client: " & lblGroupView.Text & vbCrLf & _
                                           "Transaction No: " & txttransactionnumber.Text & vbCrLf & _
                                           "has a zero commission rate")
                        End If
                        Response.Redirect("FeedMain.aspx?dataid=" & txtdataid.Text)
                    Else
                        Throw New NYSFriendlyException("Data didn't save correctly, please check " & _
                                                       "values and try again", "Info")
                    End If
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedEdit", Page)
                End If
            End Try
        End Using
    End Sub

    Public Shared Function sendVenueEmail(ByVal From As String, _
            ByVal [To] As String, ByVal Subject As String, _
            ByVal Message As String, Optional ByVal Bcc As String = Nothing) As Boolean
        'For each to address create a mail message
        Dim MailMsg As New MailMessage(New MailAddress(From.Trim()), New MailAddress([To]))
        If Bcc IsNot Nothing Then
            Dim bccaddr As New MailAddress(Bcc)
            MailMsg.Bcc.Add(bccaddr)
        End If
        MailMsg.BodyEncoding = Encoding.Default
        MailMsg.Subject = Subject.Trim()
        MailMsg.Body = Message.Trim() & vbCrLf

        'Smtpclient to send the mail message
        Dim SmtpMail As New SmtpClient
        Try
            SmtpMail.Send(MailMsg)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
    'R15 CR
    ''' <summary>
    ''' Sub setAjax
    ''' </summary>
    ''' <remarks>
    ''' Created on 17/03/2010 by Craig Rickell
    ''' Sets AJAX for autocomplete on txtpo
    ''' </remarks>
    Private Sub setAjax()
        Using New clslogger(log, className, "setAjax")
            If txtgroupname.Text.ToLower = "lv=" Then
                extPoCode.EnableCaching = True
                extPoCode.CompletionListCssClass = "list3"
                extPoCode.CompletionListHighlightedItemCssClass = "hoverlistitem3"
                extPoCode.CompletionListItemCssClass = "listitem3"
                extPoCode.ServiceMethod = "poCodeSearch_LV"
                extPoCode.ServicePath = "AjaxService.asmx"
                extPoCode.MinimumPrefixLength = 1
                extPoCode.ContextKey = txtguestname.Text
                extPoCode.TargetControlID = "txtaicol6"
            ElseIf txtgroupname.Text.ToLower = "herefordshire council" Then
                extPoCode.EnableCaching = True
                extPoCode.CompletionListCssClass = "list3"
                extPoCode.CompletionListHighlightedItemCssClass = "hoverlistitem3"
                extPoCode.CompletionListItemCssClass = "listitem3"
                extPoCode.ServiceMethod = "poCodeSearch_HerefordshireCouncil"
                extPoCode.ServicePath = "AjaxService.asmx"
                extPoCode.MinimumPrefixLength = 1
                extPoCode.ContextKey = txtguestname.Text
                extPoCode.TargetControlID = "txtaicol6"
            Else
                extPoCode.TargetControlID = "txtholder"
                extPoCode.ServiceMethod = "anything"
            End If
        End Using
    End Sub

End Class