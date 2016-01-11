Option Strict On
Option Explicit On

Imports Cubit.NSConfigUtils
Imports DatabaseObjects
Imports System.IO
Imports System.Globalization
Imports ReportDownloader
Imports EvoUtilities.ConfigUtils
Imports ReportDownloader.Utility
Imports System.Text
Imports System.Math
Imports System.Linq
Imports System.Data.Linq
Imports System.Xml.Linq

Partial Public Class FeedAdmin
    Inherits clsNYS

    Private Shared ReadOnly className As String

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
    ''' Page load for FeedAdmin
    ''' 1. 
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

                    populateGroups()
                    populateTransactions(True, 0)

                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub setUser()
        Using New clslogger(log, className, "setUser")
            If Request.Url().ToString().ToLower.StartsWith("http://cubit.dev") Or Request.Url().ToString().ToLower.StartsWith("http://localhost") Then
                'Allows loading the Admin panel in my dev enviroment
            Else
                Dim oUser As clsSystemNYSUser
                oUser = CType(Session.Item("loggedinuser"), clsSystemNYSUser)
                If oUser IsNot Nothing Then
                    If CBool(oUser.SystemnysuserInactive) = False Then
                        lbluser.Text = "Current user: " & oUser.Systemnysuserfirstname & " " & oUser.Systemnysuserlastname
                    Else
                        Response.Redirect("FeedMain.aspx")
                    End If
                    If oUser.Systemnysuserlastname.ToLower = "massarella" Or _
                        oUser.Systemnysuserlastname.ToLower = "richardson" Then
                        pnAnchor.Visible = True
                        If oUser.Systemnysuserlastname.ToLower = "massarella" Then
                            btnBookedDownLoad.Visible = True
                            btnTransDownLoad.Visible = True
                        Else
                            btnBookedDownLoad.Visible = False
                            btnTransDownLoad.Visible = False
                        End If
                    Else
                        pnAnchor.Visible = False
                    End If
                Else
                    Response.Redirect("FeedMain.aspx")
                End If
            End If

        End Using
    End Sub

    Private Sub populateGroups()
        Using New clslogger(log, className, "populateGroups")
            ddgroup.Items.Clear()

            Dim gps As List(Of clsGroup)
            gps = clsGroup.list

            'add default line
            Dim oItem As New ListItem
            oItem.Value = "0"
            oItem.Text = "Please Select"
            ddgroup.Items.Add(oItem)
            oItem = Nothing

            For Each gp As clsGroup In gps

                'R15 CR
                Dim code As New clsCode
                code = clsCode.getByName(gp.groupname)

                If code.Codeid > 0 Then
                    If code.Customername.ToUpper = gp.groupname.ToUpper Then
                        oItem = New ListItem
                        oItem.Value = CStr(gp.groupid)
                        oItem.Text = gp.groupname
                        ddgroup.Items.Add(oItem)
                        oItem = Nothing
                        'Exit For
                    End If
                End If
            Next
        End Using
    End Sub

    Protected Sub ddgroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddgroup.SelectedIndexChanged
        Using New clslogger(log, className, "ddgroup_SelectedIndexChanged")
            Try
                If ddgroup.SelectedIndex > 0 Then
                    populateParameters(CInt(ddgroup.SelectedItem.Value))
                    btnadd.Visible = True
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub populateParameters(ByVal pgroupid As Integer)
        Using New clslogger(log, className, "populateParameters")
            lstparameters.Items.Clear()

            'Dim ps As List(Of FeedParameter)
            'ps = FeedParameter.listforgroup(pgroupid)

            Dim oItem As New ListItem
            oItem.Value = "0"
            oItem.Text = "Client Options"
            lstparameters.Items.Add(oItem)
            oItem = Nothing

            'For Each p As FeedParameter In ps
            '    oItem = New ListItem
            '    oItem.Value = CStr(p.Parameterid)
            '    oItem.Text = p.Parameterstart & " to " & p.Parameterend & " - " & _
            '        p.Transactiontype & ":" & p.Transactioncode & ":" & p.Transactionvalue
            '    lstparameters.Items.Add(oItem)
            '    oItem = Nothing
            'Next

            'R2.21.5 SA 
            If pgroupid = 100119 Then 'NYS Corporate Clients - hard coded in
                Dim ooItem As New ListItem
                ooItem.Value = "1"
                ooItem.Text = "SubClient Options"
                lstparameters.Items.Add(ooItem)
                ooItem = Nothing
            End If


            Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                Dim ParmeterList = (From Parameter In Cubit.FeedParameter Join Transaction In Cubit.FeedTransaction On Parameter.transactionid Equals Transaction.transactionid Where Parameter.groupid = pgroupid _
                Select Parameter.parameterid, Parameter.parameterstart, Parameter.parameterend, Transaction.Commissionable, Transaction.transactiontype, Transaction.transactioncode, Transaction.transactionvalue, Transaction.CountryCode).ToList

                Dim FilterList = From x In ParmeterList

                If FiltersActiveList.SelectedValue = "Active" Then
                    FilterList = From x In ParmeterList Where x.parameterstart < Date.Now And x.parameterend > Date.Now
                End If

                If FiltersActiveList.SelectedValue = "InActive" Then
                    FilterList = From x In ParmeterList Where x.parameterstart > Date.Now Or x.parameterend < Date.Now
                End If

                If FiltersBookingType.SelectedValue.ToLower = "room" Then
                    FilterList = From x In FilterList Where x.transactiontype = "room"
                End If

                If FiltersBookingType.SelectedValue.ToLower = "food/beverages" Then
                    FilterList = From x In FilterList Where x.transactiontype = "meals/beverages"
                End If

                If FiltersBookingType.SelectedValue.ToLower = "extras" Then
                    FilterList = From x In FilterList Where x.transactiontype = "all extras"
                End If

                If FilterOnlineOffline.SelectedValue = "Online" Then
                    FilterList = From x In FilterList Where x.transactioncode = "online"
                End If

                If FilterOnlineOffline.SelectedValue = "Offline" Then
                    FilterList = From x In FilterList Where x.transactioncode = "offline"
                End If

                If FiltersCommission.SelectedValue = "Comm" Then
                    FilterList = From x In FilterList Where x.Commissionable = True Or x.Commissionable Is Nothing
                End If

                If FiltersCommission.SelectedValue = "NonComm" Then
                    FilterList = From x In FilterList Where x.Commissionable = False
                End If

                If FiltersCountry.SelectedValue = "GB" Then
                    FilterList = From x In FilterList Where x.CountryCode = "GB"
                End If

                If FiltersCountry.SelectedValue = "International" Then
                    FilterList = From x In FilterList Where x.CountryCode = "Intl"
                End If

                For Each Parameter In FilterList

                    Dim TempCommText As String = "Comm"

                    If Parameter.Commissionable = False Then
                        TempCommText = "Non Comm"
                    End If

                    Dim TempText As String = Parameter.parameterstart & " to " & Parameter.parameterend & " - " & Parameter.transactiontype & " : " & Parameter.transactioncode & " : " & Parameter.transactionvalue & " : " & Parameter.CountryCode & " : " & TempCommText

                    lstparameters.Items.Add(New ListItem With {.Value = CStr(Parameter.parameterid), .Text = TempText})

                Next

            End Using

        End Using
    End Sub

    Private Sub populateTransactions(ByVal pblnedit As Boolean, ByVal pintid As Integer)
        Using New clslogger(log, className, "populateParameters")
            If pintid > 0 Then
                If pintid = 1 Then
                    txtvalue.Text = ""
                    changeDropDowns(ddtransactionsedit, "", True, False)
                Else
                    'Dim t As FeedTransaction
                    't = FeedTransaction.get(pintid)
                    'If t.Transactioncode = "online" Then
                    '    ddcode.SelectedIndex = 0
                    'Else
                    '    ddcode.SelectedIndex = 1
                    'End If
                    'If t.Transactiontype = "extras" Then
                    '    ddtype.SelectedIndex = 1
                    'Else
                    '    ddtype.SelectedIndex = 0
                    'End If
                    'ddCountry.SelectedIndex = 1
                    'txtvalue.Text = CStr(t.Transactionvalue)

                    Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                        Dim Transaction = (From x In Cubit.FeedTransaction Where x.transactionid = pintid).FirstOrDefault

                        If Transaction.transactioncode = "online" Then
                            ddcode.SelectedIndex = 0
                        Else
                            ddcode.SelectedIndex = 1
                        End If
                        If Transaction.transactiontype = "extras" Then
                            ddtype.SelectedIndex = 1
                        Else
                            ddtype.SelectedIndex = 0
                        End If
                        If Transaction.CountryCode = "GB" Then
                            ddCountry.SelectedIndex = 0
                        Else
                            ddCountry.SelectedIndex = 1
                        End If
                        If Transaction.Commissionable = True Then
                            ddCommissionable.SelectedIndex = 0
                        Else
                            ddCommissionable.SelectedIndex = 1
                        End If
                        txtvalue.Text = CStr(Transaction.transactionvalue)

                    End Using

                End If
            Else
                If pblnedit Then
                    ddtransactionsedit.Items.Clear()

                    ddtransactionsedit.Items.Add(New ListItem With {.Value = "0", .Text = "Please Select"})
                    Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                        Dim TransactionList = (From x In Cubit.FeedTransaction).ToList

                        For Each Transaction In TransactionList
                            Dim TempCommText As String = "Comm"

                            If Transaction.Commissionable = False Then
                                TempCommText = "Non Comm"
                            End If

                            Dim TempItemText = Transaction.transactiontype & ": " & Transaction.transactioncode & " : " & Transaction.transactionvalue & " : " & Transaction.CountryCode & " : " & TempCommText
                            ddtransactionsedit.Items.Add(New ListItem With {.Value = CStr(Transaction.transactionid), .Text = TempItemText})
                        Next

                    End Using

                    ''add default line
                    'Dim oItem As New ListItem
                    'oItem.Value = "0"
                    'oItem.Text = "Please Select"
                    'ddtransactionsedit.Items.Add(oItem)
                    'oItem = Nothing

                    'Dim ts As List(Of FeedTransaction)
                    'ts = FeedTransaction.list

                    'For Each t As FeedTransaction In ts
                    '    oItem = New ListItem
                    '    oItem.Value = CStr(t.Transactionid)
                    '    oItem.Text = t.Transactiontype & ": " & t.Transactioncode & ": " & t.Transactionvalue
                    '    ddtransactionsedit.Items.Add(oItem)
                    '    oItem = Nothing
                    'Next
                Else
                    ddtransactions.Items.Clear()

                    ddtransactions.Items.Add(New ListItem With {.Value = "0", .Text = "Please Select"})
                    Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                        Dim TransactionList = (From x In Cubit.FeedTransaction).ToList

                        For Each Transaction In TransactionList

                            Dim TempCommText As String = "Comm"

                            If Transaction.Commissionable = False Then
                                TempCommText = "Non Comm"
                            End If

                            Dim TempItemText = Transaction.transactiontype & ": " & Transaction.transactioncode & " : " & Transaction.transactionvalue & " : " & Transaction.CountryCode & " : " & TempCommText
                            ddtransactions.Items.Add(New ListItem With {.Value = CStr(Transaction.transactionid), .Text = TempItemText})
                        Next

                    End Using

                    ''add default line
                    'Dim oItem As New ListItem
                    'oItem.Value = "0"
                    'oItem.Text = "Please Select"
                    'ddtransactions.Items.Add(oItem)
                    'oItem = Nothing

                    'Dim ts As List(Of FeedTransaction)
                    'ts = FeedTransaction.list

                    'For Each t As FeedTransaction In ts
                    '    oItem = New ListItem
                    '    oItem.Value = CStr(t.Transactionid)
                    '    oItem.Text = t.Transactiontype & ": " & t.Transactioncode & ": " & t.Transactionvalue
                    '    ddtransactions.Items.Add(oItem)
                    '    oItem = Nothing
                    'Next
                End If
            End If

        End Using
    End Sub

    Protected Sub lstparameters_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstparameters.SelectedIndexChanged
        Using New clslogger(log, className, "lstparameters_SelectedIndexChanged")
            Try
                If lstparameters.SelectedIndex > -1 Then
                    If lstparameters.SelectedValue = "0" Then
                        populateClientOptions(CInt(ddgroup.SelectedItem.Value))
                        populateClientMappings(CInt(ddgroup.SelectedItem.Value))
                        pnoptionsouter.Visible = True
                        pnoptionsouter.Style.Item("TOP") = "2px"
                        pnoptionsouter.Style.Item("LEFT") = "2px"

                        'R2.21.5 SA
                    ElseIf lstparameters.SelectedValue = "1" AndAlso ddgroup.SelectedItem.Text.ToLower = "nys corporate clients" Then
                        'clear subclients
                        clearSubClientBoxes()
                        'populate subclients
                        populateSubClientOptions(CInt(ddgroup.SelectedItem.Value))
                        pnSubClientOptionsOuter.Style.Item("TOP") = "2px"
                        pnSubClientOptionsOuter.Style.Item("LEFT") = "2px"
                        pnSubClientOptionsOuter.Visible = True

                    Else
                        If ddtransactions.Items.Count < 2 Then
                            populateTransactions(False, 0)
                        Else
                            changeDropDowns(ddtransactions, "", True, False)
                        End If
                        pnedit.Visible = True
                        populateParameter(CInt(lstparameters.SelectedItem.Value))
                    End If
                    btnclose.Visible = False

                    'R15 CR
                    btnCopyFromMevis.Visible = False
                    btnMissingInvoices.Visible = False
                    btntransadd.Visible = False
                    ddtransactionsedit.Enabled = False
                    lstparameters.Enabled = False
                    ddgroup.Enabled = False
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub
    'R2.21.5 SA 
    Private Sub clearSubClientBoxes()
        Using New clslogger(log, className, "clearSubClientBoxes")
            txtsubclientID.Text = ""
            txtSubClientName.Text = ""
            txtSubclientBossCode.Text = ""
            txtSubClientFee.Text = ""
            chkSubClientActive.Checked = False
        End Using
    End Sub


    'R2.21.5 SA 
    Private Sub populateSubClientOptions(ByVal pGroupID As Integer)
        Using New clslogger(log, className, "populateSubClientOptions")

            Dim tab As New Data.DataTable
            Dim dv As Data.DataView
            tab = NSUtils.GetTable(clsSubClientOptions.list(pGroupID))

            dv = New Data.DataView(tab)

            grdData.DataSource = dv
            grdData.DataBind()

            Dim ocol As New System.Drawing.ColorConverter
            Dim col As System.Drawing.Color
            col = CType(ocol.ConvertFromString("#FF3300"), Drawing.Color)
            grdData.RowHighlightColor = col
        End Using
    End Sub

    'R2.21.5 SA 
    Private Function saveSubClientOptions(ByVal pintSubClientID As Integer, ByVal pstrSubClientName As String, _
                                          ByVal pstrSubClientBossCode As String, ByVal pintGroupID As Integer,
                                          ByVal pdecSubClientFee As Decimal, ByVal pblnSubClientActive As Boolean) As Boolean
        Using New clslogger(log, className, "saveSubClientOptions")
            Dim p As New clsSubClientOptions(pintSubClientID, pstrSubClientName, pstrSubClientBossCode, _
                                                pintGroupID, pdecSubClientFee, pblnSubClientActive)
            If p.save > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    'R2.21.5 SA 
    Private Sub getSubClientDetails(ByVal pintSubClientID As Integer)
        Using New clslogger(log, className, "getSubClientDetails")
            Dim details As New clsSubClientOptions
            details = clsSubClientOptions.get(pintSubClientID)

            txtSubClientName.Text = details.SubClientName
            txtSubclientBossCode.Text = details.SubClientBossCode
            txtSubClientFee.Text = CStr(details.SubClientFee)
            chkSubClientActive.Checked = CBool(details.SubClientActive)

        End Using
    End Sub

    Private Sub populateClientOptions(ByVal pGroupID As Integer)
        Using New clslogger(log, className, "populateClientOptions")
            Dim oP As clsClientOption
            oP = clsClientOption.get(pGroupID)

            txtoptionsid.Text = "0"
            txtCostCentreValue.Text = ""
            txtAICol6value.Text = ""
            txtAICol7value.Text = ""
            txtAICol8value.Text = ""
            txtAICol9value.Text = ""
            txtAICol10value.Text = ""
            ddcharge.SelectedIndex = 0
            chkgross.Checked = False
            chkexvat.Checked = False
            chkadditional.Checked = False
            rdextras.SelectedIndex = 0
            txtadditionalfee.Text = ""
            'R2.13 NM
            txtinvoicefee.Text = ""

            'R2.21 SA
            ddBossCode.SelectedIndex = 0
            chkOOH.Checked = False
            txtOOHfee.Text = ""
            txtBossCode.Text = ""

            'R2.21.1 SA 
            chkAPT.Checked = False
            txtAPTFee.Text = ""
            chkCX.Checked = False

            If Not oP Is Nothing And oP.ClientOptionsID > 0 Then
                txtoptionsid.Text = CStr(oP.ClientOptionsID)
                txtCostCentreValue.Text = oP.aicostcodeValue
                txtAICol6value.Text = oP.aicol6Value
                txtAICol7value.Text = oP.aicol7Value
                txtAICol8value.Text = oP.aicol8Value
                txtAICol9value.Text = oP.aicol9Value
                txtAICol10value.Text = oP.aicol10Value

                If oP.TransactionType = "PP" Then
                    ddcharge.SelectedIndex = 1
                Else
                    ddcharge.SelectedIndex = 0
                End If
                chkgross.Checked = CBool(oP.SendGross)
                chkexvat.Checked = CBool(oP.TransactionExVat)
                If oP.ExtrasCharges = 1 Then
                    rdextras.SelectedIndex = 1
                ElseIf oP.ExtrasCharges = 2 Then
                    rdextras.SelectedIndex = 2
                End If
                chkadditional.Checked = CBool(oP.additional)
                If oP.additionalfee > 0 Then
                    txtadditionalfee.Text = CStr(oP.additionalfee)
                Else
                    txtadditionalfee.Text = ""
                End If
                'R2.13 NM
                If oP.invoicefee > 0 Then
                    txtinvoicefee.Text = notString(oP.invoicefee)
                Else
                    txtinvoicefee.Text = ""
                End If

                'R2.21 SA
                chkOOH.Checked = notBoolean(oP.OOH)
                If oP.OOHFee > 0 Then
                    txtOOHfee.Text = notString(oP.OOHFee)
                Else
                    txtOOHfee.Text = ""
                End If

                'R2.21.1 SA
                chkCX.Checked = notBoolean(oP.CX)
                chkAPT.Checked = notBoolean(oP.APT)
                If oP.APTFee > 0 Then
                    txtAPTFee.Text = notString(oP.APTFee)
                Else
                    txtAPTFee.Text = ""
                End If

            End If

            'R15 CR
            Dim code As clsCode
            code = clsCode.getByName(ddgroup.SelectedItem.Text)

            'R2.21 SA 
            'Please remember to change ddBossCode Items value if changed elsewhere
            If oP.BossOption <> "" Then
                'R?? SA bug Fix!
                'Dim strBossCode As String = ""
                'Select Case oP.BossOption
                '    Case "35"
                '        strBossCode = "CostCode"
                '    Case "34"
                '        strBossCode = "Booker"
                '    Case "39"
                '        strBossCode = "AIcol6"
                '    Case "40"
                '        strBossCode = "AIcol7"
                '    Case "41"
                '        strBossCode = "AIcol8"
                '    Case "42"
                '        strBossCode = "AIcol9"
                '    Case "43"
                '        strBossCode = "AIcol10"
                'End Select
                'changeDropDowns(ddBossCode, strBossCode, False, True)
                changeDropDowns(ddBossCode, oP.BossOption, False, True)
            Else
                'R15 CR
                If code.Codeid > 0 Then
                    txtBossCode.Text = code.Customercode
                End If
            End If

        End Using
    End Sub

    'R2.3 NM
    Private Sub populateClientMappings(ByVal pGroupID As Integer)
        Using New clslogger(log, className, "populateClientMappings")
            Dim oP As ClientMapping
            oP = ClientMapping.getClientMapping(pGroupID)

            txtmappingsid.Text = "0"
            txtccdefault.Text = ""
            txtpodefault.Text = ""
            txtcref1default.Text = ""
            txtcref2default.Text = ""
            txtcref3default.Text = ""
            txtcref4default.Text = ""
            txtcref5default.Text = ""
            txtcref6default.Text = ""
            txtcref7default.Text = ""
            txtcref8default.Text = ""
            txtcref9default.Text = ""

            ddcc.SelectedIndex = 0
            ddpo.SelectedIndex = 0
            ddcref1.SelectedIndex = 0
            ddcref2.SelectedIndex = 0
            ddcref3.SelectedIndex = 0
            ddcref4.SelectedIndex = 0
            ddcref5.SelectedIndex = 0
            ddcref6.SelectedIndex = 0
            ddcref7.SelectedIndex = 0
            ddcref8.SelectedIndex = 0
            ddcref9.SelectedIndex = 0

            If Not oP Is Nothing And oP.ClientMappingsID > 0 Then
                txtmappingsid.Text = CStr(oP.ClientMappingsID)
                txtccdefault.Text = oP.CostCentreDefault
                txtpodefault.Text = oP.PODefault
                txtcref1default.Text = oP.CREF1Default
                txtcref2default.Text = oP.CREF2Default
                If oP.CostCentreValue <> "" Then
                    changeDropDowns(ddcc, oP.CostCentreValue, False, True)
                End If
                If oP.POValue <> "" Then
                    changeDropDowns(ddpo, oP.POValue, False, True)
                End If
                If oP.CREF1Value <> "" Then
                    changeDropDowns(ddcref1, oP.CREF1Value, False, True)
                End If
                If oP.CREF2Value <> "" Then
                    changeDropDowns(ddcref2, oP.CREF2Value, False, True)
                End If
                If oP.CREF3Value <> "" Then
                    changeDropDowns(ddcref3, oP.CREF3Value, False, True)
                End If
                If oP.CREF4Value <> "" Then
                    changeDropDowns(ddcref4, oP.CREF4Value, False, True)
                End If
                If oP.CREF5Value <> "" Then
                    changeDropDowns(ddcref5, oP.CREF5Value, False, True)
                End If
                If oP.CREF6Value <> "" Then
                    changeDropDowns(ddcref6, oP.CREF6Value, False, True)
                End If
                If oP.CREF7Value <> "" Then
                    changeDropDowns(ddcref7, oP.CREF7Value, False, True)
                End If
                If oP.CREF8Value <> "" Then
                    changeDropDowns(ddcref8, oP.CREF8Value, False, True)
                End If
                If oP.CREF9Value <> "" Then
                    changeDropDowns(ddcref9, oP.CREF9Value, False, True)
                End If
            End If
        End Using
    End Sub

    Private Sub populateParameter(ByVal pintparameterid As Integer)
        Using New clslogger(log, className, "populateParameter")
            If pintparameterid = 0 Then
                txtfrom.Value = ""
                txtto.Value = ""
                If ddtransactions.Items.Count < 2 Then
                    populateTransactions(False, 0)
                Else
                    changeDropDowns(ddtransactions, "", True, False)
                End If
            Else
                Dim p As FeedParameter
                p = FeedParameter.get(pintparameterid)
                txtfrom.Value = Format(p.Parameterstart, "dd/MM/yyyy")
                txtto.Value = Format(p.Parameterend, "dd/MM/yyyy")
                changeDropDowns(ddtransactions, CStr(p.Transactionid), False, False)
            End If
            btnadd.Visible = False
        End Using
    End Sub

    Private Function checkParameters(ByVal pGroupID As Integer, ByVal pstrType As String) As String
        Using New clslogger(log, className, "checkParameters")
            Dim intExtrasCharges As Integer = FeedParameter.parameterCheck(pGroupID)
            Dim strRes As String = ""

            If pstrType <> "room" And intExtrasCharges = 0 Then
                strRes = "Client is not allowed any extras charges"
            ElseIf pstrType = "meals/beverages" Then
                If intExtrasCharges = 0 Then
                    strRes = "Client is not allowed any extras charges"
                ElseIf intExtrasCharges = 2 Then
                    strRes = "Client is only allowed All extras charges"
                End If
            ElseIf pstrType = "all extras" Then
                If intExtrasCharges = 0 Then
                    strRes = "Client is not allowed any extras charges"
                ElseIf intExtrasCharges = 1 Then
                    strRes = "Client is only allowed Meals & Beverages extras charges"
                End If
            End If
            Return strRes
        End Using
    End Function

    ''' <summary>
    ''' Sub btnsave_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Button to save parameter details
    ''' 1. Checks user has entered correct details before attempting to save
    ''' </remarks>
    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnsave.Click
        Using New clslogger(log, className, "btnsave_Click")
            Try
                If txtfrom.Value = "" Or txtto.Value = "" Then
                    Throw New NYSFriendlyException("Please complete both date fields to continue!", "Info")
                End If
                If ddtransactions.SelectedIndex < 1 Then
                    Throw New NYSFriendlyException("Please select an option from the Transactions list " & _
                                                   "to continue!", "Info")
                End If
                Dim strMessage As String = checkParameters(CInt(ddgroup.SelectedItem.Value), Mid(ddtransactions.SelectedItem.Text, 1, ddtransactions.SelectedItem.Text.IndexOf(":")))
                If strMessage <> "" Then
                    Throw New NYSFriendlyException(strMessage, "Info")
                End If
                If saveParameter(txtfrom.Value, txtto.Value, CInt(ddtransactions.SelectedItem.Value)) Then
                    Dim intid As Integer = 0
                    If lstparameters.SelectedIndex > 0 Then
                        intid = CInt(lstparameters.SelectedItem.Value)
                    End If
                    populateParameters(CInt(ddgroup.SelectedItem.Value))
                    If intid > 0 Then
                        changelists(lstparameters, CStr(intid), False)
                    End If
                    pnedit.Visible = False
                    btnadd.Visible = True
                    btnclose.Visible = True

                    'R15 CR
                    btnCopyFromMevis.Visible = True
                    btnMissingInvoices.Visible = True
                    btntransadd.Visible = True
                    ddtransactionsedit.Enabled = True
                    lstparameters.Enabled = True
                    ddgroup.Enabled = True
                    Throw New NYSFriendlyException("Parameter details saved", "Info")
                Else
                    Throw New NYSFriendlyException("Parameter details did not save correctly, " & _
                                                   "please try again!", "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Function saveParameter
    ''' </summary>
    ''' <param name="pstrfrom"></param>
    ''' <param name="pstrto"></param>
    ''' <param name="pinttransactionid"></param>
    ''' <returns>
    ''' True is saved correctly
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Connects to DatabaseObjects and saves user entered values
    ''' </remarks>
    Private Function saveParameter(ByVal pstrfrom As String, ByVal pstrto As String, _
                                    ByVal pinttransactionid As Integer) As Boolean
        Using New clslogger(log, className, "saveParameter")
            Dim intid As Integer = 0
            If lstparameters.SelectedIndex > -1 Then
                intid = CInt(lstparameters.SelectedItem.Value)
            End If
            Dim p As New FeedParameter(intid, CInt(ddgroup.SelectedItem.Value), CDate(pstrfrom), _
                                       CDate(pstrto), pinttransactionid)

            If p.save > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    ''' <summary>
    ''' Sub btnadd_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Clears lstparameters and calls populateparameter method to clear fields
    ''' </remarks>
    Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnadd.Click
        Using New clslogger(log, className, "btnadd_Click")
            Try
                changelists(lstparameters, "", True)
                populateParameter(0)
                pnedit.Visible = True
                btnadd.Visible = False
                btnclose.Visible = False
                'R15 NM
                btnCopyFromMevis.Visible = False
                btnMissingInvoices.Visible = False
                btntransadd.Visible = False
                ddtransactionsedit.Enabled = False
                lstparameters.Enabled = False
                ddgroup.Enabled = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btncancel_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Cancels current edit of parameter
    ''' </remarks>
    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btncancel.Click
        Using New clslogger(log, className, "btncancel_Click")
            Try
                pnedit.Visible = False
                btnadd.Visible = True
                btnclose.Visible = True
                btntransadd.Visible = True
                ddtransactionsedit.Enabled = True
                lstparameters.Enabled = True
                ddgroup.Enabled = True
                'R15 NM
                btnCopyFromMevis.Visible = True
                btnMissingInvoices.Visible = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub ddtransactionsedit_SelectedIndexChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 13/03/2009 Nick Massarella
    ''' Selects a transaction record for edit
    ''' </remarks>
    Protected Sub ddtransactionsedit_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddtransactionsedit.SelectedIndexChanged
        Using New clslogger(log, className, "ddtransactionsedit_SelectedIndexChanged")
            Try
                If ddtransactionsedit.SelectedIndex > 0 Then
                    populateTransactions(False, CInt(ddtransactionsedit.SelectedItem.Value))
                    pntransactionedit.Visible = True
                    btntransadd.Visible = False
                    btnclose.Visible = False
                    btnadd.Visible = False
                    ddtransactionsedit.Enabled = False
                    lstparameters.Enabled = False
                    ddgroup.Enabled = False
                    'R15 NM
                    btnCopyFromMevis.Visible = False
                    btnMissingInvoices.Visible = False
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btntransadd_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 13/03/2009 Nick Massarella
    ''' Deselects ddtransactionsedit, shows transactionsedit panel and clears fields 
    ''' </remarks>
    Protected Sub btntransadd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btntransadd.Click
        Using New clslogger(log, className, "btntransadd_Click")
            Try
                populateTransactions(False, 1)
                pntransactionedit.Visible = True
                btntransadd.Visible = False
                btnclose.Visible = False
                btnadd.Visible = False
                ddtransactionsedit.Enabled = False
                lstparameters.Enabled = False
                ddgroup.Enabled = False
                'R15 NM
                btnCopyFromMevis.Visible = False
                btnMissingInvoices.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btntranscancel_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 13/03/2009 Nick Massarella
    ''' Cancels current transaction record edit mode
    ''' </remarks>
    Protected Sub btntranscancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btntranscancel.Click
        Using New clslogger(log, className, "btntranscancel_Click")
            Try
                pntransactionedit.Visible = False
                btntransadd.Visible = True
                btnclose.Visible = True
                btnadd.Visible = True
                ddtransactionsedit.Enabled = True
                lstparameters.Enabled = True
                ddgroup.Enabled = True
                'R15 NM
                btnCopyFromMevis.Visible = True
                btnMissingInvoices.Visible = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Sub btntranssave_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Created 13/03/2009 Nick Massarella
    ''' Checks user entered values for validity, if OK calls savetransaction method
    ''' </remarks>
    Protected Sub btntranssave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btntranssave.Click
        Using New clslogger(log, className, "btntranssave_Click")
            Try
                If txtvalue.Text = "" Then
                    Throw New NYSFriendlyException("Please enter a value in the 'Value' field to continue!", _
                                                   "Info")
                End If
                Try
                    Dim dbltest As Double = CDbl(txtvalue.Text)
                Catch ex As Exception
                    Throw New NYSFriendlyException("Please enter a numeric value in the 'Value' field to continue!", _
                                                                   "Info")
                End Try

                'MK Added 23-05-2014 to allow for new International Field saving using EF
                If AddNewTransaction(ddcode.SelectedItem.Text, CDbl(txtvalue.Text), ddtype.SelectedItem.Text, ddCountry.SelectedItem.Text, ddCommissionable.SelectedItem.Text) Then
                    Dim intid As Integer = 0
                    If ddtransactionsedit.SelectedIndex > 0 Then
                        intid = CInt(ddtransactionsedit.SelectedItem.Value)
                    End If
                    'clear transactions so when user next views it will repoluate with updated values
                    ddtransactions.Items.Clear()

                    populateTransactions(True, 0)
                    If intid > 0 Then
                        changeDropDowns(ddtransactionsedit, CStr(intid), False, False)
                    End If
                    pntransactionedit.Visible = False
                    btntransadd.Visible = True
                    btnclose.Visible = True
                    btnadd.Visible = True
                    ddtransactionsedit.Enabled = True
                    lstparameters.Enabled = True
                    ddgroup.Enabled = True
                    'R15 NM
                    btnCopyFromMevis.Visible = True
                    btnMissingInvoices.Visible = True
                    If lstparameters.Items.Count > 0 And ddgroup.SelectedIndex > 0 Then
                        populateParameters(CInt(ddgroup.SelectedItem.Value))
                    End If
                    Throw New NYSFriendlyException("Transaction saved", "Info")
                Else
                    'It didn't save the transaction
                    Throw New NYSFriendlyException("Transaction did not save correctly, please try again!", "Info")
                End If

                'OLD METHOD
                'If savetransaction(ddcode.SelectedItem.Text, CDbl(txtvalue.Text), ddtype.SelectedItem.Text) Then

                '    Dim intid As Integer = 0
                '    If ddtransactionsedit.SelectedIndex > 0 Then
                '        intid = CInt(ddtransactionsedit.SelectedItem.Value)
                '    End If
                '    'clear transactions so when user next views it will repoluate with updated values
                '    ddtransactions.Items.Clear()

                '    populateTransactions(True, 0)
                '    If intid > 0 Then
                '        changeDropDowns(ddtransactionsedit, CStr(intid), False, False)
                '    End If
                '    pntransactionedit.Visible = False
                '    btntransadd.Visible = True
                '    btnclose.Visible = True
                '    btnadd.Visible = True
                '    ddtransactionsedit.Enabled = True
                '    lstparameters.Enabled = True
                '    ddgroup.Enabled = True
                '    'R15 NM
                '    btnCopyFromMevis.Visible = True
                '    btnMissingInvoices.Visible = True
                '    If lstparameters.Items.Count > 0 And ddgroup.SelectedIndex > 0 Then
                '        populateParameters(CInt(ddgroup.SelectedItem.Value))
                '    End If
                '    Throw New NYSFriendlyException("Transaction saved", "Info")
                'Else
                '    Throw New NYSFriendlyException("Transaction did not save correctly, please try again!", _
                '                                                                   "Info")
                'End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Private Function AddNewTransaction(ByVal Code As String, ByVal Value As Double, ByVal Type As String, ByVal CountryCode As String, ByVal Commissionable As String) As Boolean

        'Create a new transaction
        Try
            Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities

                Dim TempTransaction As New Cubit.FeedTransactionEF

                TempTransaction.CountryCode = CountryCode
                TempTransaction.transactiontype = Type
                TempTransaction.transactionvalue = Value
                TempTransaction.transactioncode = Code
                TempTransaction.datecreated = Date.Now
                TempTransaction.lastupdated = Date.Now
                If Commissionable.ToLower = "commissionable" Then
                    TempTransaction.Commissionable = True
                Else
                    TempTransaction.Commissionable = False
                End If

                'Save it to the database
                Cubit.FeedTransaction.Add(TempTransaction)
                Cubit.SaveChanges()
            End Using

        Catch ex As Exception
            Return False
        End Try

        'Got to the end

        Return True

    End Function


    ''' <summary>
    ''' Function savetransaction
    ''' </summary>
    ''' <param name="pstrcode"></param>
    ''' <param name="pdblvalue"></param>
    ''' <returns>
    ''' True is saved correctly
    ''' </returns>
    ''' <remarks>
    ''' Created 13/03/2009 Nick Massarella
    ''' Save user edit transaction record
    ''' </remarks>
    Private Function savetransaction(ByVal pstrcode As String, ByVal pdblvalue As Double, _
                                     ByVal pstrtype As String) As Boolean
        Using New clslogger(log, className, "savetransaction")
            Dim intid As Integer = 0
            If ddtransactionsedit.SelectedIndex > 0 Then
                intid = CInt(ddtransactionsedit.SelectedItem.Value)
            End If
            Dim p As New FeedTransaction(intid, pstrcode, pdblvalue, pstrtype)

            If p.save > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnclose.Click
        Using New clslogger(log, className, "btnclose_Click")
            Response.Redirect("FeedMain.aspx")
        End Using
    End Sub

    Protected Sub btncanceloptions_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btncanceloptions.Click
        Using New clslogger(log, className, "btncanceloptions_Click")
            Try
                pnoptionsouter.Visible = False
                btnadd.Visible = True
                btnclose.Visible = True
                btntransadd.Visible = True
                ddtransactionsedit.Enabled = True
                lstparameters.Enabled = True
                ddgroup.Enabled = True
                'R15 NM
                btnCopyFromMevis.Visible = True
                btnMissingInvoices.Visible = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnsaveoptions_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnsaveoptions.Click
        Using New clslogger(log, className, "btnsaveoptions_Click")
            Try
                If txtccdefault.Text <> "" And ddcc.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cost centre drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtpodefault.Text <> "" And ddpo.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The PO drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref1default.Text <> "" And ddcref1.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref1 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref2default.Text <> "" And ddcref2.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref2 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref3default.Text <> "" And ddcref3.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref3 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref4default.Text <> "" And ddcref4.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref4 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref5default.Text <> "" And ddcref5.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref5 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref6default.Text <> "" And ddcref6.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref6 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref7default.Text <> "" And ddcref7.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref7 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref8default.Text <> "" And ddcref8.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref8 drop down cannot be selected when the default box has a value!", "Info")
                End If
                If txtcref9default.Text <> "" And ddcref9.SelectedIndex > 0 Then
                    Throw New NYSFriendlyException("The Cref9 drop down cannot be selected when the default box has a value!", "Info")
                End If

                Dim intID As Integer = 0
                If txtoptionsid.Text <> "" Then
                    Try
                        Dim inttemp As Integer = CInt(txtoptionsid.Text)
                        intID = CInt(txtoptionsid.Text)
                    Catch ex As Exception
                        intID = 0
                    End Try
                End If

                Dim intMapID As Integer = 0
                If txtmappingsid.Text <> "" Then
                    Try
                        Dim inttemp As Integer = CInt(txtmappingsid.Text)
                        intMapID = CInt(txtmappingsid.Text)
                    Catch ex As Exception
                        intMapID = 0
                    End Try
                End If

                If txtadditionalfee.Text <> "" Then
                    Try
                        Dim iTest As Decimal = CDec(txtadditionalfee.Text)
                    Catch ex As InvalidCastException
                        Throw New NYSFriendlyException("Please enter Additional Fee Value as a numeric value only!", "Info")
                    End Try
                End If

                If txtinvoicefee.Text <> "" Then
                    Try
                        Dim iTest As Decimal = CDec(txtinvoicefee.Text)
                    Catch ex As InvalidCastException
                        Throw New NYSFriendlyException("Please enter Invoice Fee Value as a numeric value only!", "Info")
                    End Try
                End If

                If chkadditional.Checked And txtadditionalfee.Text = "" Then
                    Throw New NYSFriendlyException("Please add an Additional Fee Value or uncheck the Additional fee box!", "Info")
                ElseIf Not chkadditional.Checked And txtadditionalfee.Text <> "" Then
                    Throw New NYSFriendlyException("Please check the Additional fee box or clear the Additioanl Fee Value box!", "Info")
                End If

                If txtadditionalfee.Text = "" Then
                    txtadditionalfee.Text = "0"
                End If

                If txtinvoicefee.Text = "" Then
                    txtinvoicefee.Text = "0"
                End If

                'R2.21 SA
                If txtOOHfee.Text <> "" Then
                    Try
                        Dim iTest As Decimal = CDec(txtOOHfee.Text)
                    Catch ex As Exception
                        Throw New NYSFriendlyException("Please enter OOH Fee Value as a numeric value only!", "Info")
                    End Try
                End If
                If chkOOH.Checked And txtOOHfee.Text = "" Then
                    Throw New NYSFriendlyException("Please add an OOH Fee Value or uncheck the OOH Fee box!", "Info")
                ElseIf Not chkOOH.Checked And txtOOHfee.Text <> "" Then
                    Throw New NYSFriendlyException("Please check the OOH Fee box or clear the OOH Fee Value box!", "Info")
                End If
                If txtOOHfee.Text = "" Then
                    txtOOHfee.Text = "0"
                End If

                'R2.21.1 SA 
                If txtAPTFee.Text <> "" Then
                    Try
                        Dim iTest As Decimal = CDec(txtAPTFee.Text)
                    Catch ex As Exception
                        Throw New NYSFriendlyException("Please enter APT Fee Value as a numeric value only!", "Info")
                    End Try
                End If
                If chkAPT.Checked And txtAPTFee.Text = "" Then
                    Throw New NYSFriendlyException("Please add an APT Fee Value or uncheck the OOH Fee box!", "Info")
                ElseIf Not chkAPT.Checked And txtAPTFee.Text <> "" Then
                    Throw New NYSFriendlyException("Please add an APT Fee Value or uncheck the OOH Fee box!", "Info")
                End If
                If txtAPTFee.Text = "" Then
                    txtAPTFee.Text = "0"
                End If

                'R2.21.1 SA - added chkAPT.Checked, CDec(txtAPTFee.Text), chkCX.Checked
                'R2.21 SA - added chkOOH.Checked, CDec(txtOOHfee.Text)
                'R2.13 NM
                If saveOptions(intID, ddcharge.SelectedValue, chkgross.Checked, chkexvat.Checked, CInt(rdextras.SelectedItem.Value), _
                               txtCostCentreValue.Text, txtAICol6value.Text, txtAICol7value.Text, txtAICol8value.Text, txtAICol9value.Text, txtAICol10value.Text, _
                                chkadditional.Checked, CDec(txtadditionalfee.Text), CDec(txtinvoicefee.Text), chkOOH.Checked, CDec(txtOOHfee.Text), _
                                chkAPT.Checked, CDec(txtAPTFee.Text), chkCX.Checked) Then

                    'R2.3 NM
                    If saveMappings(intMapID, CInt(ddgroup.SelectedItem.Value), ddcc.SelectedItem.Value, txtccdefault.Text, _
                                 ddpo.SelectedItem.Value, txtpodefault.Text, ddcref1.SelectedItem.Value, txtcref1default.Text, _
                                 ddcref2.SelectedItem.Value, txtcref2default.Text, ddcref3.SelectedItem.Value, txtcref3default.Text, _
                                 ddcref4.SelectedItem.Value, txtcref4default.Text, ddcref5.SelectedItem.Value, txtcref5default.Text, _
                                 ddcref6.SelectedItem.Value, txtcref6default.Text, ddcref7.SelectedItem.Value, txtcref7default.Text, _
                                 ddcref8.SelectedItem.Value, txtcref8default.Text, ddcref9.SelectedItem.Value, txtcref9default.Text) Then

                        pnoptionsouter.Visible = False
                        btnadd.Visible = True
                        btnclose.Visible = True

                        'R15 CR
                        btnCopyFromMevis.Visible = True
                        btnMissingInvoices.Visible = True
                        btntransadd.Visible = True
                        ddtransactionsedit.Enabled = True
                        lstparameters.Enabled = True
                        ddgroup.Enabled = True

                        Throw New NYSFriendlyException("Client options saved", "Info")
                    Else
                        Throw New NYSFriendlyException("Client options did not save correctly, please try again!", "Info")
                    End If
                Else
                    Throw New NYSFriendlyException("Client options did not save correctly, please try again!", "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Private Function saveMappings(ByVal pintMappingsID As Integer, ByVal pClientID As Integer, _
                                 ByVal pCostCentreValue As String, ByVal pCostCentreDefault As String, _
                                 ByVal pPOValue As String, ByVal pPODefault As String, _
                                 ByVal pCREF1Value As String, ByVal pCREF1Default As String, _
                                 ByVal pCREF2Value As String, ByVal pCREF2Default As String, _
                                 ByVal pCREF3Value As String, ByVal pCREF3Default As String, _
                                 ByVal pCREF4Value As String, ByVal pCREF4Default As String, _
                                 ByVal pCREF5Value As String, ByVal pCREF5Default As String, _
                                 ByVal pCREF6Value As String, ByVal pCREF6Default As String, _
                                 ByVal pCREF7Value As String, ByVal pCREF7Default As String, _
                                 ByVal pCREF8Value As String, ByVal pCREF8Default As String, _
                                 ByVal pCREF9Value As String, ByVal pCREF9Default As String) As Boolean
        Using New clslogger(log, className, "saveOptions")

            Dim oP As New ClientMapping()

            If pCostCentreValue.ToLower = "n/a" Then
                pCostCentreValue = ""
            End If
            If pPOValue.ToLower = "n/a" Then
                pPOValue = ""
            End If
            If pCREF1Value.ToLower = "n/a" Then
                pCREF1Value = ""
            End If
            If pCREF2Value.ToLower = "n/a" Then
                pCREF2Value = ""
            End If
            If pCREF3Value.ToLower = "n/a" Then
                pCREF3Value = ""
            End If
            If pCREF4Value.ToLower = "n/a" Then
                pCREF4Value = ""
            End If
            If pCREF5Value.ToLower = "n/a" Then
                pCREF5Value = ""
            End If
            If pCREF6Value.ToLower = "n/a" Then
                pCREF6Value = ""
            End If
            If pCREF7Value.ToLower = "n/a" Then
                pCREF7Value = ""
            End If
            If pCREF8Value.ToLower = "n/a" Then
                pCREF8Value = ""
            End If
            If pCREF9Value.ToLower = "n/a" Then
                pCREF9Value = ""
            End If

            If oP.save(pintMappingsID, pClientID, _
                       pCostCentreValue, pCostCentreDefault, _
                       pPOValue, pPODefault, _
                       pCREF1Value, pCREF1Default, _
                       pCREF2Value, pCREF2Default, _
                       pCREF3Value, pCREF3Default, _
                       pCREF4Value, pCREF4Default, _
                       pCREF5Value, pCREF5Default, _
                       pCREF6Value, pCREF6Default, _
                       pCREF7Value, pCREF7Default, _
                       pCREF8Value, pCREF8Default, _
                       pCREF9Value, pCREF9Default) > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    'R2.21.1 SA - added pbAPT As Boolean, pdecAPTFee As Decimal, pbCX As Boolean
    'R2.21 SA - added  pbOOH As Boolean, pdecOOHeFee As Decimal
    Private Function saveOptions(ByVal pintOptionsID As Integer, ByVal pstrType As String, ByVal pbGross As Boolean, _
                                 ByVal pbExVat As Boolean, ByVal pintExtrasCharges As Integer, ByVal pstraicostcodevalue As String, ByVal pstraicol6value As String, _
                                 ByVal pstraicol7value As String, ByVal pstraicol8value As String, ByVal pstraicol9value As String, ByVal pstraicol10value As String, _
                                 ByVal pbAdditional As Boolean, ByVal pdecAdditionalFee As Decimal, ByVal pInvoiceFee As Decimal, _
                                 ByVal pbOOH As Boolean, ByVal pdecOOHFee As Decimal, _
                                 ByVal pbAPT As Boolean, ByVal pdecAPTFee As Decimal, ByVal pbCX As Boolean) As Boolean
        Using New clslogger(log, className, "saveOptions")


            'R2.21 SA 
            Dim strBossOption As String = ""

            'R2.21 SA 
            If ddBossCode.Visible = True Then
                If ddBossCode.SelectedIndex > 0 AndAlso txtBossCode.Text <> "" Then
                    Throw New NYSFriendlyException("Please only provide ONE Boss code option", "Info")
                ElseIf ddBossCode.SelectedIndex > 0 AndAlso txtBossCode.Text = "" Then
                    strBossOption = ddBossCode.SelectedItem.Value
                    'R15 CR
                ElseIf txtBossCode.Text <> "" AndAlso ddBossCode.SelectedIndex = 0 Then
                    saveBossCode(txtBossCode.Text, ddgroup.SelectedItem.Text)
                End If
            Else
                saveBossCode(txtBossCode.Text, ddgroup.SelectedItem.Text)
            End If

            'R2.21.1 SA - added pbAPT, pdecAPTFee, pCX
            'R2.21 SA - added strBossOption - pbOOH, pdecOOHFee
            'R2.13 NM
            Dim oP As New clsClientOption(pintOptionsID, CType(ddgroup.SelectedItem.Value, Integer?), pstrType, pbGross, pbExVat, pintExtrasCharges,
                                          pstraicostcodevalue, pstraicol6value, pstraicol7value, pstraicol8value, _
                                          pstraicol9value, pstraicol10value, pbAdditional, pdecAdditionalFee, pInvoiceFee, strBossOption, pbOOH, pdecOOHFee, _
                                          pbAPT, pdecAPTFee, pbCX)


            If oP.save > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    'R15 CR
    Protected Sub saveBossCode(ByVal pstrbosscode As String, ByVal pstrgroupname As String)
        Using New clslogger(log, className, "saveBossCode")
            Dim bosscode As New clsCode
            bosscode = clsCode.getByName(pstrgroupname)

            If bosscode.Codeid > 0 Then
                bosscode.Customercode = pstrbosscode

                bosscode.save()
            End If
        End Using
    End Sub
    Protected Sub btnexport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnexport.Click
        Using New clslogger(log, className, "btnexport_Click")
            Try
                If Not flUpload.PostedFile Is Nothing Then
                    If flUpload.PostedFile.ContentLength > 0 Then
                        'check file extension
                        Dim strEXT As String = Path.GetExtension(flUpload.PostedFile.FileName)
                        If strEXT.ToUpper <> ".CSV" Then
                            Throw New NYSFriendlyException("Please select a file with a '.CSV' file extension!", "Info")
                        End If
                        Dim strFileName As String = Format(Now, "ddMMyyyyhhmmss") & ".csv"
                        flUpload.PostedFile.SaveAs(getConfig("uploadedfiles") & strFileName)
                        Dim filer As String = importFile(getConfig("uploadedfiles") & strFileName)
                        sendCSVToClient(Response, "Export.csv", filer)
                    Else
                        Throw New NYSFriendlyException("Please select a file to export!", "Info")
                    End If
                Else
                    Throw New NYSFriendlyException("Please select a file to export!", "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Public Shared Function readAllText(ByVal filename As String) As String
        Try
            Return My.Computer.FileSystem.ReadAllText(filename)
        Catch ex As Exception
            log.Error(ex.Message)
            Return ""
        End Try
    End Function

    Private Shared Function item(ByVal a As IList(Of String), ByVal index As Integer) As String
        If a.Count - 1 < index Then
            Return ""
        Else
            Return CStr(a.Item(index)).Trim
        End If
    End Function

    Public Function importFile(ByVal pstrFile As String) As String
        Try
            '"C:\Development\VS9\NYS9\CUBIT_APP\Cubit\Cubit\downloadedfiles\test.csv"
            Dim parser As New CSVParser.CSVParser(readAllText(pstrFile))
            Dim line As IList(Of String) = parser.readLine()
            Dim fr As New StringWriter(CultureInfo.CurrentCulture)

            Dim intstart As Integer = 0
            Do While Not line Is Nothing
                If line.Count > 1 Then
                    Dim strs As String = ""
                    Dim costcode As String = ""
                    For x As Integer = 0 To line.Count - 1
                        If intstart = 0 Then
                            If x = 1 Then
                                strs = strs & line(x).ToString & ",Anchor Code,"
                            Else
                                strs = strs & line(x).ToString & ","
                            End If
                        Else
                            If x = 1 Then
                                If line(x).ToUpper = getConfig("AnchorBossCode") Then
                                    Dim strCode As String = FeedInvoice.getAnchorCode(costcode)
                                    strs = strs & line(x).ToString & "," & strCode & ","
                                Else
                                    strs = strs & line(x).ToString & ",,"
                                End If
                            Else
                                strs = strs & line(x).ToString & ","
                            End If
                        End If
                        If x = 0 Then
                            costcode = line(x).ToString
                        End If
                    Next
                    fr.WriteLine(strs)
                End If
                line = parser.readLine
                intstart += 1
            Loop

            Return fr.ToString()

        Catch ex As Exception
            log.Error(ex.Message)
            Return ""
        End Try
    End Function

    Public Sub sendCSVToClient(ByVal response As HttpResponse, ByVal FileName As String, ByVal csvData As String)
        response.AddHeader("Content-Type", "application/csv")
        response.AddHeader("content-disposition", "attachment; filename=" & FileName)
        response.Charset = "iso-8859-1"
        response.Write(csvData)
        response.End()
    End Sub


    'R15 CR
    Protected Sub btnCopyFromMevis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopyFromMevis.Click
        Using New clslogger(log, className, "btnCopyFromMevis_Click")
            Try
                'get all groups from Mevis
                Dim gps As List(Of clsGroup)
                gps = clsGroup.listwithcubit

                Dim strRemoved As String = ""
                Dim strAdded As String = ""

                For Each gp As clsGroup In gps
                    'check if already exists in cubit db, code table
                    Dim code As New clsCode
                    code = clsCode.getByName(gp.groupname)

                    If code.Codeid > 0 Then
                        If Not CBool(gp.cubit) Then
                            'remove codes records
                            strRemoved = strRemoved & gp.groupname & "; "
                            clsCode.delete(code.Codeid)
                        End If
                    Else
                        If CBool(gp.cubit) Then
                            'insert into CUBIT db if they dont already exist and 'Cubit' field is TRUE
                            Dim clsNewCode As New clsCode
                            clsNewCode.Customername = gp.groupname
                            clsNewCode.Customercode = ""
                            strAdded = strAdded & gp.groupname & "; "
                            clsNewCode.save()
                        End If
                    End If
                Next
                If strRemoved <> "" Then
                    strRemoved = "Clients removed: " & strRemoved
                End If
                If strAdded <> "" Then
                    strAdded = "Clients added: " & strAdded
                End If
                populateGroups()
                If strRemoved <> "" Or strAdded <> "" Then
                    Throw New NYSFriendlyException(strRemoved & vbNewLine & vbNewLine & strAdded, "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnMissingInvoices_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMissingInvoices.Click
        Using New clslogger(log, className, "btnMissingInvoices_Click")
            Response.Redirect("FeedInvoiceEmail.aspx")
        End Using
    End Sub



    Protected Sub btnBookedDownLoad_Click(sender As Object, e As EventArgs) Handles btnBookedDownLoad.Click
        Using New clslogger(log, className, "btnBookedDownLoad_Click")
            Try
                Dim strmessage As String = ""
                If Not IO.File.Exists(getConfig("downloadedfilesBooked") & Format(Now, "dd-MM-yyyy") & ".csv") Then
                    strmessage = WebRetrieve.BookedData
                End If
                If strmessage <> "" Then
                    Throw New NYSFriendlyException(strmessage, "Info")
                End If
                Dim csv As New CSVReaderBooked
                If csv.Main Then
                    Throw New NYSFriendlyException("CSV file import OK", "Info")
                Else
                    Throw New NYSFriendlyException("CSV file import ERROR " + getConfig("downloadedfilesBooked"), "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnTransDownLoad_Click(sender As Object, e As EventArgs) Handles btnTransDownLoad.Click
        Using New clslogger(log, className, "btnTransDownLoad_Click")
            Try
                'If getConfig("GetAll") = "TRUE" Then
                '    Dim start As Date = CDate("01/11/2011")
                '    Dim dNow As Date = CDate(Format(Now, "dd/MM/yyyy"))
                '    Do While start <= dNow
                '        Dim strmessage As String = ""
                '        If Not IO.File.Exists(getConfig("downloadedfilesTrans") & Format(start, "dd-MM-yyyy") & ".csv") Then
                '            strmessage = WebRetrieve.TransData(Format(start, "yyyy-MM-dd"), Format(start, "dd-MM-yyyy"))
                '        End If
                '        If strmessage <> "" Then
                '            Throw New NYSFriendlyException(strmessage, "Info")
                '        End If
                '        Dim csv As New CSVReaderTrans
                '        If csv.Main(Format(start, "dd-MM-yyyy")) Then
                '            'Throw New NYSFriendlyException("CSV file import OK", "Info")
                '        Else
                '            Throw New NYSFriendlyException("CSV file import ERROR", "Info")
                '        End If
                '        start = start.AddDays(1)
                '    Loop
                'Else
                '    Dim strmessage As String = ""
                '    Dim dNow As Date = CDate(Format(Now, "dd/MM/yyyy"))

                '    If Not IO.File.Exists(getConfig("downloadedfilesTrans") & "14-09-2011.csv") Then
                '        strmessage = WebRetrieve.TransData("2011-09-14", "14-09-2011")
                '    End If
                '    If strmessage <> "" Then
                '        Throw New NYSFriendlyException(strmessage, "Info")
                '    End If
                '    Dim csv As New CSVReaderTrans
                '    If csv.Main("01-09-2011") Then
                '        Throw New NYSFriendlyException("CSV file import OK", "Info")
                '    Else
                '        Throw New NYSFriendlyException("CSV file import ERROR", "Info")
                '    End If

                '    Dim strmessage As String = ""
                '    If Not IO.File.Exists(getConfig("downloadedfilesTrans") & Format(Now.AddDays(-1), "dd-MM-yyyy") & ".csv") Then
                '        strmessage = WebRetrieve.TransData(Format(Now.AddDays(-1), "yyyy-MM-dd"), Format(Now.AddDays(-1), "dd-MM-yyyy"))
                '    End If
                '    If strmessage <> "" Then
                '        Throw New NYSFriendlyException(strmessage, "Info")
                '    End If
                '    Dim csv As New CSVReaderTrans
                '    If csv.Main(Format(Now.AddDays(-1), "dd-MM-yyyy")) Then
                '        Throw New NYSFriendlyException("CSV file import OK", "Info")
                '    Else
                '        Throw New NYSFriendlyException("CSV file import ERROR", "Info")
                '    End If
                'End If


            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.5 SA
    Protected Sub btnSubClientAdd_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSubClientAdd.Click
        Using New clslogger(log, className, "btnSubClientAdd_Click")
            Try
                pnSubClientAdd.Visible = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.5 SA
    Protected Sub btnSubClientClose_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSubClientClose.Click
        Using New clslogger(log, className, "btnSubClientClose_Click")
            Try
                pnSubClientOptionsOuter.Visible = False
                lstparameters.Enabled = True
                ddgroup.Enabled = True
                btnCopyFromMevis.Visible = True
                btnMissingInvoices.Visible = True
                btnclose.Visible = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.5 SA
    Protected Sub btnSubClientSave_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSubClientSave.Click
        Using New clslogger(log, className, "btnSubClientSave_Click")
            Try
                'validate
                If txtsubclientID.Text = "" Then
                    txtsubclientID.Text = "0"
                End If

                If txtSubClientFee.Text <> "" Then
                    Try
                        Dim iTest As Decimal = CDec(txtSubClientFee.Text)
                    Catch ex As Exception
                        Throw New NYSFriendlyException("Please enter Fee Value as a numeric value only!", "Info")
                    End Try
                End If

                'save 
                If saveSubClientOptions(CInt(txtsubclientID.Text), txtSubClientName.Text, txtSubclientBossCode.Text, CInt(ddgroup.SelectedItem.Value), _
                                        CDec(txtSubClientFee.Text), chkSubClientActive.Checked) Then

                    'clear boxes
                    clearSubClientBoxes()

                    're-populate gird
                    populateSubClientOptions(CInt(ddgroup.SelectedItem.Value))

                    'hide pnadd
                    pnSubClientAdd.Visible = False

                    Throw New NYSFriendlyException("Sub-Client details saved", "Info")
                Else
                    Throw New NYSFriendlyException("Sub-Client details did not saved! Please try again!", "Info")
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.5 SA
    Protected Sub btnSubClientCancel_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSubClientCancel.Click
        Using New clslogger(log, className, "btnSubClientCancel_Click")
            Try
                'clear
                clearSubClientBoxes()
                'hide panel
                pnSubClientAdd.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleException(ex, "FeedMain", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.21.5 SA
    Private Sub grdData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdData.ItemCommand
        Try
            Select Case e.CommandName
                Case "edit"
                    txtsubclientID.Text = CStr(e.Item.Cells(0).Text)
                    getSubClientDetails(CInt(txtsubclientID.Text))
                    pnSubClientAdd.Visible = True
                Case Else
                    Throw New Exception("Unrecognised command name: " & e.CommandName)
            End Select
        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                handleException(ex, "FeedMain", Page)
            End If
        End Try
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        populateParameters(CInt(ddgroup.SelectedItem.Value))

        If FiltersActiveList.SelectedValue = "All" Or FiltersActiveList.SelectedValue = "Inactive" Or FiltersBookingType.SelectedValue = "All" Or FilterOnlineOffline.SelectedValue = "All" Or FiltersCountry.SelectedValue = "All" Or FiltersCommission.SelectedValue = "All" Then
            'Do Nothing
        Else
            'Calculate the fee we would charge
            'Show the parameter that would be picked based on the filtered Selection
            Using Cubit As CUBITFeedImportDataEntities = New CUBITFeedImportDataEntities
                Dim pgroupID = CInt(ddgroup.SelectedItem.Value)

                Dim ParmeterList = (From Parameter In Cubit.FeedParameter Join Transaction In Cubit.FeedTransaction On Parameter.transactionid Equals Transaction.transactionid Where Parameter.groupid = pgroupID _
                Select Parameter.parameterid, Parameter.parameterstart, Parameter.parameterend, Transaction.Commissionable, Transaction.transactiontype, Transaction.transactioncode, Transaction.transactionvalue, Transaction.CountryCode).ToList

                Dim ActiveList = From x In ParmeterList Where x.parameterstart < Date.Now And x.parameterend > Date.Now

                Dim Comm As Boolean = True
                Dim SelectedCountryCode As String = "gb"
                Dim SelectedTransactionType As String = "room"
                Dim SelectedTransactionCode As String = "online"


                If FiltersBookingType.SelectedValue = "Food/Beverages" Then
                    SelectedTransactionType = "meals/beverages"
                End If

                If FiltersBookingType.SelectedValue = "Extras" Then
                    SelectedTransactionType = "all extras"
                End If

                If FilterOnlineOffline.SelectedValue = "Offline" Then
                    SelectedTransactionCode = "offline"
                End If

                If FiltersCountry.SelectedValue = "International" Then
                    SelectedCountryCode = "Intl"
                End If

                If FiltersCommission.SelectedValue = "NonComm" Then
                    Comm = False
                End If

                Dim SelectedParameterID As Integer = 0

                'Check for Exact Match
                Dim ExactMatch = (From x In ActiveList Where x.transactioncode = SelectedTransactionCode And x.transactiontype = SelectedTransactionType And x.CountryCode = SelectedCountryCode And x.Commissionable = Comm).FirstOrDefault

                If ExactMatch IsNot Nothing Then
                    SelectedParameterID = ExactMatch.parameterid
                End If

                If SelectedParameterID = 0 Then
                    'Check for a Match on International
                    Dim InternationalMatch = (From x In ActiveList Where x.transactioncode = SelectedTransactionCode And x.transactiontype = SelectedTransactionType And x.CountryCode = SelectedCountryCode).FirstOrDefault

                    If InternationalMatch IsNot Nothing Then
                        SelectedParameterID = InternationalMatch.parameterid
                    End If
                End If


                If SelectedParameterID = 0 Then
                    'Check on comm vs none comm
                    Dim CommMatch = (From x In ActiveList Where x.transactioncode = SelectedTransactionCode And x.transactiontype = SelectedTransactionType And x.Commissionable = Comm).FirstOrDefault

                    'Load the parameter 
                    If CommMatch IsNot Nothing Then
                        SelectedParameterID = CommMatch.parameterid
                    End If
                End If

                If SelectedParameterID = 0 Then
                    'Check on only Transaction Code and Type
                    Dim BaseMatch = (From x In ActiveList Where x.transactioncode = SelectedTransactionCode And x.transactiontype = SelectedTransactionType).FirstOrDefault

                    If BaseMatch IsNot Nothing Then
                        SelectedParameterID = BaseMatch.parameterid
                    End If

                End If

                If SelectedParameterID <> 0 Then
                    Dim SelectedParameter = (From x In ActiveList Where x.parameterid = SelectedParameterID).FirstOrDefault
                    'Display The Fee
                    Dim TempCommText As String
                    If SelectedParameter.Commissionable = False Then
                        TempCommText = "Non Comm"
                    Else
                        TempCommText = "Comm"
                    End If
                    Dim TempText As String = SelectedParameter.transactiontype & " : " & SelectedParameter.transactioncode & " : " & SelectedParameter.transactionvalue & " : " & SelectedParameter.CountryCode & " : " & TempCommText

                    lblSelectedFee.Text = "Applied fee for selection : " & SelectedParameter.transactionvalue & "<br>" & TempText
                Else
                    lblSelectedFee.Text = "No Parameter Selected :("
                End If

            End Using


        End If

    End Sub
End Class