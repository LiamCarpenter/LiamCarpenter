Imports EvoUtilities.ConfigUtils
Imports BossData
Imports System.Data
Imports Microsoft.VisualBasic

Public Class clsBoss
    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Shared log As log4net.ILog = _
    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Shared Sub bossSuppliers(ByVal pInvoiceID As String)
        Using New clslogger(log, className, "bossSuppliers")

            'log.Info("Start Connection to update Boss Supplier")
            'Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            'Dim myDataSet As New DataSet()

            log.Info("Start update Boss Supplier")
            Dim strSupplierID As String = BOSSsupplier.getSupplierBoss(pInvoiceID)
            log.Info("End update Boss Supplier for " & strSupplierID)

            If strSupplierID <> "" Then
                Try

                    log.Info("Start Connection to update Boss Supplier")
                    Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
                    Dim myDataSet As New DataSet()

                    log.Info("Select supplier info for " & strSupplierID)
                    Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT sup_id, sup_desc, sup_type, sup_grpid, sup_comfrm, sup_name, sup_add1, sup_add2, sup_add3, sup_add4, " & _
                                                                           "sup_pcode1, sup_pcode2, sup_cntry, sup_contac, sup_tel, sup_fax, sup_email, sup_email2, sup_cdate, sup_ldate, " &
                                                                           "sup_acode, sup_dccode, sup_atol, sup_atolag, sup_prefrd, sup_paygrs, sup_selfbl, sup_docmnt, cast(sup_dcomp1 as numeric(18,2)) as sup_dcomp1, " & _
                                                                           "cast(sup_dcomp2 as numeric(18,2)) as sup_dcomp2, sup_glcode, cast(sup_pcent as numeric(18,2)) as sup_pcent, sup_xpense, " & _
                                                                           "sup_cterms, sup_bdueto, sup_bduefm, sup_pmeth, sup_pmethc, sup_notes, sup_popup, cast(sup_dppc as numeric(18,2)) as sup_dppc, " & _
                                                                           "cast(sup_dpamt as numeric(18,2)) as sup_dpamt, sup_ubtax, sup_yqtax, sup_ouracc, sup_fullcl, sup_vatabl, sup_vatno " &
                                                                           "FROM supplier " & _
                                                                           "where sup_id = '" & strSupplierID & "'", dBaseConnection)

                    log.Info("Start second Connection to Boss")
                    Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                    myDataAdapter.Fill(myDataSet, "Supplier")
                    dBaseConnection.Close()
                    log.Info("First connection end")

                    For Each dr As DataRow In myDataSet.Tables("Supplier").Rows
                        'sp save will check if exists etc
                        Dim oSupplier As New BOSSsupplier(0, CStr(dr.Item("sup_id")), CStr(dr.Item("sup_desc")), CStr(dr.Item("sup_type")), CStr(dr.Item("sup_grpid")), CStr(dr.Item("sup_comfrm")), _
                                                          CStr(dr.Item("sup_name")), CStr(dr.Item("sup_add1")), CStr(dr.Item("sup_add2")), CStr(dr.Item("sup_add3")), CStr(dr.Item("sup_add4")), _
                                                          CStr(dr.Item("sup_pcode1")), CStr(dr.Item("sup_pcode2")), CStr(dr.Item("sup_cntry")), CStr(dr.Item("sup_contac")), CStr(dr.Item("sup_tel")), _
                                                          CStr(dr.Item("sup_fax")), CStr(dr.Item("sup_email")), CStr(dr.Item("sup_email2")), CType(dr.Item("sup_cdate"), Date?), CType(dr.Item("sup_ldate"), Date?), _
                                                          CStr(dr.Item("sup_acode")), CStr(dr.Item("sup_dccode")), CStr(dr.Item("sup_atol")), CType(dr.Item("sup_atolag"), Boolean?), CType(dr.Item("sup_prefrd"), Boolean?), _
                                                          CType(dr.Item("sup_paygrs"), Boolean?), CType(dr.Item("sup_selfbl"), Boolean?), CType(dr.Item("sup_docmnt"), Boolean?), CDec(dr.Item("sup_dcomp1")), _
                                                          CDec(dr.Item("sup_dcomp2")), CDec(dr.Item("sup_glcode")), CDec(dr.Item("sup_pcent")), CStr(dr.Item("sup_xpense")), _
                                                          CDec(dr.Item("sup_cterms")), CDec(dr.Item("sup_bdueto")), CDec(dr.Item("sup_bduefm")), CStr(dr.Item("sup_pmeth")), _
                                                          CStr(dr.Item("sup_pmethc")), CStr(dr.Item("sup_notes")), CStr(dr.Item("sup_popup")), CDec(dr.Item("sup_dppc")), CDec(dr.Item("sup_dpamt")), _
                                                          CType(dr.Item("sup_ubtax"), Boolean?), CType(dr.Item("sup_yqtax"), Boolean?), CStr(dr.Item("sup_ouracc")), CStr(dr.Item("sup_fullcl")), _
                                                          CType(dr.Item("sup_vatabl"), Boolean?), CStr(dr.Item("sup_vatno")))
                        oSupplier.save()
                        log.Info("Saved supplier " & CStr(dr.Item("sup_id")))
                    Next

                    myDataSet.Clear()
                    log.Info("BOSSSupplier completed successfully")
                Catch ex As Exception
                    log.Error("SUPPLIER ERROR: ", ex)
                End Try
            End If
        End Using
    End Sub

    Public Shared Function runBOSSSQL(ByVal Query As String) As DataSet
        Using New clslogger(log, className, "runBOSSSQL")
            Dim ds As New DataSet()
            Using connection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
                Using command As New System.Data.OleDb.OleDbCommand(Query, connection)
                    Using adapter As New System.Data.OleDb.OleDbDataAdapter(command)
                        adapter.Fill(ds)
                    End Using
                End Using
            End Using
            Return ds
        End Using
    End Function


    Public Shared Function runMainSelect(ByVal pbDate As Boolean, ByVal pstrInvoiceref As String, ByVal pstrClient As String, _
                                         ByVal pstrstart As String, ByVal pstrend As String) As String
        Using New clslogger(log, className, "runMainSelect")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim myDataSetInner As New DataSet()
            Dim strCurrentRef As String = ""
            Dim strLastRef As String = ""
            Dim intCount As Integer = 0
            Dim strSubject As String = "runMainSelect hourly "

            Dim strTest As String = ""

            'first update supplier details as they may have changed
            Try
                bossSuppliers(pstrInvoiceref)
            Catch ex As Exception
                'just carry one
            End Try
            Try
                Dim strWhere As String = ""
                If pbDate Then
                    strWhere = "WHERE (inm_dla = DATE() or inm_invdt = DATE() or inm_cdate = DATE())"
                ElseIf pstrInvoiceref <> "" Then
                    strWhere = "WHERE inm_no = '" & pstrInvoiceref & "'"
                ElseIf pstrClient <> "" Then
                    If pstrstart = "" Then
                        strWhere = "WHERE inm_custid = '" & pstrClient & "' and (inm_dla = DATE() or inm_invdt = DATE() or inm_cdate = DATE())"
                    Else
                        Dim diff1 As Integer = DateDiff(DateInterval.Day, CDate(pstrstart), Now)
                        Dim diff2 As Integer = DateDiff(DateInterval.Day, CDate(pstrend), Now)
                        If pstrClient = "All" Then
                            strWhere = "WHERE (" & _
                                            "(inm_dla >= DATE() - " & diff1 & " and inm_dla <= DATE() - " & diff2 & ") or " & _
                                            "(inm_invdt >= DATE() - " & diff1 & " and inm_invdt <= DATE() - " & diff2 & ") or " & _
                                            "(inm_cdate >= DATE() - " & diff1 & " and inm_cdate <= DATE() - " & diff2 & "))"
                        Else
                            strWhere = "WHERE inm_custid = '" & pstrClient & "' and (" & _
                                                                                "(inm_dla >= DATE() - " & diff1 & " and inm_dla <= DATE() - " & diff2 & ") or " & _
                                                                                "(inm_invdt >= DATE() - " & diff1 & " and inm_invdt <= DATE() - " & diff2 & ") or " & _
                                                                                "(inm_cdate >= DATE() - " & diff1 & " and inm_cdate <= DATE() - " & diff2 & "))"
                        End If

                    End If
                End If

                dBaseConnection.Open()
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("set deleted on", dBaseConnection)
                dBaseCommand.ExecuteNonQuery()

                Dim sql As String = "SELECT DISTINCT inm_no FROM invmain " & strWhere
                dBaseCommand = New System.Data.OleDb.OleDbCommand(sql, dBaseConnection)
                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()

                If myDataSet.Tables("Main").Rows.Count > 0 Then
                    For Each dr As DataRow In myDataSet.Tables("Main").Rows
                        If dr.Item("inm_no").ToString.Contains(Chr(187)) = False Then

                            If selectInvMainRecords(dr.Item("inm_no").ToString) Then
                                If selectInvtotRecord(dr.Item("inm_no").ToString) Then
                                    If selectInvRouteRecords(dr.Item("inm_no").ToString) Then

                                    Else
                                        myDataSet.Clear()
                                        Return "selectInvRouteRecords ERROR, try again"
                                    End If
                                Else
                                    myDataSet.Clear()
                                    Return "selectInvtotRecord ERROR, try again"
                                End If
                            Else
                                myDataSet.Clear()
                                Return "selectInvMainRecords ERROR, try again"
                            End If

                            'R1.2 NM
                            If runCashHdr(dr.Item("inm_no").ToString, "") <> "Successful run" Then
                                myDataSet.Clear()
                                Return "runCashHdr ERROR, try again"
                            End If

                        End If
                    Next
                Else
                    myDataSet.Clear()
                    Return "Nothing to update, try again"
                End If

                myDataSet.Clear()
                Return "Successful run"

            Catch ex As Exception
                log.Error(ex.Message)
                Return "Invoice errors2: " & vbCrLf & strCurrentRef & ex.Message
            End Try
        End Using
    End Function


    'R2.?? CR - commented out... not in use??
    ' also searching for invoices that have N inside which is wrong
    'Public Shared Function runHotelInvoice(ByVal pstrFrom As String, ByVal pstrVenue As String) As String
    '    Using New clslogger(log, className, "runHotelInvoice")

    '        'Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
    '        'Dim myDataSet As New DataSet()
    '        'Dim myDataSetInner As New DataSet()

    '        Try

    '            'Dim diff1 As Integer = DateDiff(DateInterval.Day, CDate(pstrFrom), Now)

    '            'Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT DISTINCT invmain.inm_no " & _
    '            '                                                       "FROM invmain,supplier " & _
    '            '                                                       "where  invmain.inm_suppid = supplier.sup_id " & _
    '            '                                                       "and invmain.inm_invdt <= DATE() - " & diff1 & _
    '            '                                                       " and supplier.sup_name LIKE '" & pstrVenue.ToUpper & "%' " & _
    '            '                                                       " and inm_comdue > 0 " _
    '            '                                                      , dBaseConnection)

    '            'Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
    '            'myDataAdapter.Fill(myDataSet, "Main")
    '            'dBaseConnection.Close()

    '            If pstrVenue = " " Then
    '                pstrVenue = ""
    '            End If
    '            Dim oResults As List(Of CommissionChase)
    '            oResults = CommissionChase.outstandingCommissionInvoice(pstrFrom, pstrVenue)
    '            For Each oResult As CommissionChase In oResults
    '                ' For Each dr As DataRow In myDataSet.Tables("Main").Rows
    '                If oResult.Invoiceid.Contains("N") Then

    '                    If selectInvMainRecords(oResult.Invoiceid) Then
    '                        If selectInvtotRecord(oResult.Invoiceid) Then
    '                            If selectInvRouteRecords(oResult.Invoiceid) Then

    '                            End If
    '                        End If
    '                    End If
    '                    runCashHdr(oResult.Invoiceid, "")
    '                    runCashSelect("", oResult.Invoiceid)
    '                End If
    '            Next

    '            Return "Successful run"
    '        Catch ex As Exception
    '            log.Error(ex.Message)
    '            Return ex.Message
    '        End Try
    '    End Using
    'End Function

    'R2.?? CR - commented out... not in use??
    ' also searching for invoices that have N inside which is wrong
    'Public Shared Function runUnpaidInvoice() As String
    '    Using New clslogger(log, className, "runUnpaidInvoice")

    '        Try

    '            Dim oResults As List(Of CommissionChase)
    '            oResults = CommissionChase.bossCommissionClaimUnpaidInvoice
    '            For Each oResult As CommissionChase In oResults
    '                ' For Each dr As DataRow In myDataSet.Tables("Main").Rows
    '                If oResult.Invoiceid.Contains("N") Then

    '                    'If selectInvMainRecords(oResult.Invoiceid) Then
    '                    '    If selectInvtotRecord(oResult.Invoiceid) Then
    '                    '        If selectInvRouteRecords(oResult.Invoiceid) Then

    '                    '        End If
    '                    '    End If
    '                    'End If
    '                    runCashHdr(oResult.Invoiceid, "")
    '                    runCashSelect("", oResult.Invoiceid)
    '                End If
    '            Next

    '            Return "Successful run"
    '        Catch ex As Exception
    '            log.Error(ex.Message)
    '            Return ex.Message
    '        End Try
    '    End Using
    'End Function

    Public Shared Function selectInvtotRecord(ByVal pstrInvoiceRef As String) As Boolean
        Using New clslogger(log, className, "selectInvtotRecord")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strLocalRef As String = ""

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT tot_ukey,tot_custid,tot_invno,tot_change,tot_crsref,tot_br,tot_pono,tot_costc," & _
                                                                                   "tot_type,tot_invdt,tot_duedt,tot_fare,tot_tax,tot_srvchg,tot_ourchg,tot_ourvat," & _
                                                                                   "tot_supvat,tot_amtvat,tot_bilvat,tot_amount,tot_billed,tot_discnt,tot_ccamt," & _
                                                                                   "tot_recvd,tot_dposit,tot_comm,tot_comdue,tot_vtoncm,tot_disput,tot_reason," & _
                                                                                   "tot_noerrs,tot_note,tot_raddr,tot_rtelno,tot_paxs,tot_morcrs,tot_retail," & _
                                                                                   "tot_bdm,tot_print,tot_errs,tot_atol,tot_atolx,tot_cref1,tot_cref2,tot_agcomm," & _
                                                                                   "tot_agvat,tot_fileno,tot_nofee,tot_crstyp,tot_noprnt,tot_email,tot_curncy," & _
                                                                                   "tot_roe,tot_cursym " & _
                                                                                   "FROM Invtot where tot_invno = '" & pstrInvoiceRef & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()

                Dim dtCreated As Date = BOSSinvmain.BOSSinvmainCdate(pstrInvoiceRef)

                For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    'delete any existing record
                    BOSSinvtot.delete(0, CStr(dr.Item("tot_invno")))
                    'save tot record
                    strLocalRef = dr.Item("tot_invno").ToString

                    If dr.Item("tot_invno").ToString.Contains(Chr(187)) = False Then
                        Dim oTot As New BOSSinvtot(0, CStr(dr.Item("tot_ukey")), CStr(dr.Item("tot_custid")), CStr(dr.Item("tot_invno")), CInt(dr.Item("tot_change")), CStr(dr.Item("tot_crsref")), _
                                              CStr(dr.Item("tot_br")), CStr(dr.Item("tot_pono")), CStr(dr.Item("tot_costc")), CStr(dr.Item("tot_type")), CDate(dr.Item("tot_invdt")), CDate(dr.Item("tot_duedt")), _
                                              CDec(dr.Item("tot_fare")), CDec(dr.Item("tot_tax")), CDec(dr.Item("tot_srvchg")), CDec(dr.Item("tot_ourchg")), CDec(dr.Item("tot_ourvat")), _
                                              CDec(dr.Item("tot_supvat")), CDec(dr.Item("tot_amtvat")), CDec(dr.Item("tot_bilvat")), CDec(dr.Item("tot_amount")), CDec(dr.Item("tot_billed")), _
                                              CDec(dr.Item("tot_discnt")), CDec(dr.Item("tot_ccamt")), CDec(dr.Item("tot_recvd")), CDec(dr.Item("tot_dposit")), CDec(dr.Item("tot_comm")), _
                                              CDec(dr.Item("tot_comdue")), CDec(dr.Item("tot_vtoncm")), CBool(dr.Item("tot_disput")), CStr(dr.Item("tot_reason")), CBool(dr.Item("tot_noerrs")), _
                                              CStr(dr.Item("tot_note")), CStr(dr.Item("tot_raddr")), CStr(dr.Item("tot_rtelno")), CInt(dr.Item("tot_paxs")), CStr(dr.Item("tot_morcrs")), _
                                              CBool(dr.Item("tot_retail")), CStr(dr.Item("tot_bdm")), CInt(dr.Item("tot_print")), CBool(dr.Item("tot_errs")), CBool(dr.Item("tot_atol")), _
                                              CStr(dr.Item("tot_atolx")), CStr(dr.Item("tot_cref1")), CStr(dr.Item("tot_cref2")), CDec(dr.Item("tot_agcomm")), CDec(dr.Item("tot_agvat")), _
                                              CStr(dr.Item("tot_fileno")), CBool(dr.Item("tot_nofee")), CStr(dr.Item("tot_crstyp")), CBool(dr.Item("tot_noprnt")), CStr(dr.Item("tot_email")), _
                                              CStr(dr.Item("tot_curncy")), CDec(dr.Item("tot_roe")), CStr(dr.Item("tot_cursym")), dtCreated)
                        oTot.save()
                    End If
                Next
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function



    Public Shared Function selectInvMainRecords(ByVal pstrInvoiceRef As String) As Boolean
        Using New clslogger(log, className, "selectInvMainRecords")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_no, inm_line, inm_retail, inm_br, inm_type, inm_invdt, inm_entry, inm_custid, inm_costc, inm_pono, inm_cdate, inm_prod, inm_suppid, inm_supnam," & _
                                         "inm_htltel, inm_bsp, inm_docmnt, inm_etkt, inm_books, inm_exch, inm_origtk, inm_online, inm_bspdte, inm_start, inm_end, inm_ldname, inm_savecd," & _
                                         "cast(inm_mxfare as numeric(18,2)) as inm_mxfare," & _
                                         "cast(inm_lwfare as numeric(18,2)) as inm_lwfare," & _
                                         "inm_bookng, inm_fcrncy," & _
                                         "cast(inm_ffare as numeric(18,2)) as inm_ffare," & _
                                         "cast(inm_drate as numeric(18,2)) as inm_drate," & _
                                         "cast(inm_days as numeric(18,2)) as inm_days," & _
                                         "inm_curncy," & _
                                         "cast(inm_fare as numeric(18,2)) as inm_fare," & _
                                         "cast(inm_srvchg as numeric(18,2)) as inm_srvchg," & _
                                         "cast(inm_ubtax as numeric(18,2)) as inm_ubtax," & _
                                         "cast(inm_yqtax as numeric(18,2)) as inm_yqtax," & _
                                         "cast(inm_othtax as numeric(18,2)) as inm_othtax," & _
                                         "cast(inm_tax as numeric(18,2)) as inm_tax," & _
                                         "cast(inm_ourchg as numeric(18,2)) as inm_ourchg," & _
                                         "cast(inm_amtvat as numeric(18,2)) as inm_amtvat," & _
                                         "cast(inm_supvat as numeric(18,2)) as inm_supvat," & _
                                         "cast(inm_ourvat as numeric(18,2)) as inm_ourvat," & _
                                         "cast(inm_amount as numeric(18,2)) as inm_amount," & _
                                         "cast(inm_dscpct as numeric(18,2)) as inm_dscpct," & _
                                         "cast(inm_discnt as numeric(18,2)) as inm_discnt," & _
                                         "cast(inm_billed as numeric(18,2)) as inm_billed," & _
                                         "cast(inm_bilvat as numeric(18,2)) as inm_bilvat," & _
                                         "cast(inm_compct as numeric(18,2)) as inm_compct," & _
                                         "cast(inm_comvat as numeric(18,2)) as inm_comvat," & _
                                         "cast(inm_vtoncm as numeric(18,2)) as inm_vtoncm," & _
                                         "cast(inm_trucom as numeric(18,2)) as inm_trucom," & _
                                         "cast(inm_othcom as numeric(18,2)) as inm_othcom," & _
                                         "cast(inm_comamt as numeric(18,2)) as inm_comamt," & _
                                         "cast(inm_comrcv as numeric(18,2)) as inm_comrcv," & _
                                         "cast(inm_comdue as numeric(18,2)) as inm_comdue," & _
                                         "inm_paynet, inm_vatinv, inm_cominv, inm_morpax, inm_dposit, inm_depok, inm_depbr, inm_depbnk," & _
                                         "inm_baldue, inm_paytyp, inm_ccid, inm_ccno, inm_ccstdt, inm_ccexp, inm_ccauth, inm_cccvv, inm_issue, inm_merch, inm_mfee, inm_ccmeth, inm_ccamt," & _
                                         "inm_ccxmit, inm_print, inm_con1pd, inm_con2pd, inm_orig, inm_locked, inm_erflag, inm_erdesc, inm_ukey, inm_change, inm_who, inm_note, inm_crsref, inm_bdm," & _
                                         "inm_pcity, inm_ino, inm_cos, inm_domint, inm_apok, inm_arok, inm_voided, inm_agcomm, inm_agvat, inm_agdcpc, inm_atol, inm_abond, inm_afare, inm_aheads," & _
                                         "inm_nfare, inm_rebate, inm_fee, inm_feevt, inm_feebas, inm_dla, inm_bywho, inm_cinvrf, tmpfld, inm_3rdpty, inm_ourcc, inm_gcid, inm_gcno, inm_itcode," & _
                                         "inm_bkcur, inm_bkroe, inm_miles1, inm_miles2, inm_km1, inm_km2, cast(inm_disvat as numeric(18,2)) as inm_disvat, chksum " & _
                                         "FROM invmain " & _
                                         "WHERE inm_no = '" & pstrInvoiceRef & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()
            Catch ex As Exception
                'clsUseful.sendEmailLocal("ashley.marron@nysgroup.com", "ashley.marron@nysgroup.com", "IA - BOSS OLE Fall through", ex.Message)
                'log the error
                log.Error("INVOICE ERROR FROM BOSS FOR " & pstrInvoiceRef & ": " & ex.Message)
                Return False
            End Try

            Try
                'first delete any records found in SQL
                BOSSinvmain.delete(0, pstrInvoiceRef)
            Catch ex As Exception
                clsUseful.sendEmailLocal("ashley.marron@nysgroup.com", "ashley.marron@nysgroup.com", "IA - BOSS DELETE look at SqlServerMevisConnStr in webconfig ", "BOSSinvmain_delete 0, " + pstrInvoiceRef.ToString() + ex.Message)
                'log the error
                log.Error("INVOICE ERROR DELETE  " & pstrInvoiceRef & ": " & ex.Message)
                Return False
            End Try

            Try

                For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    'then save main record to SQL

                    Dim oIMain As New BOSSinvmain(0, dr.Item("inm_no").ToString, CDbl(dr.Item("inm_line")), CBool(dr.Item("inm_retail")), CStr(dr.Item("inm_br")), _
                                                    CStr(dr.Item("inm_type")), CDate(dr.Item("inm_invdt")), CStr(dr.Item("inm_entry")), CStr(dr.Item("inm_custid")), _
                                                    CStr(dr.Item("inm_costc")), CStr(dr.Item("inm_pono")), CDate(dr.Item("inm_cdate")), CStr(dr.Item("inm_prod")), _
                                                    CStr(dr.Item("inm_suppid")), CStr(dr.Item("inm_supnam")), CStr(dr.Item("inm_htltel")), CBool(dr.Item("inm_bsp")), _
                                                    CStr(dr.Item("inm_docmnt")), CStr(dr.Item("inm_etkt")), CDbl(dr.Item("inm_books")), CStr(dr.Item("inm_exch")), _
                                                    CStr(dr.Item("inm_origtk")), CBool(dr.Item("inm_online")), CDate(dr.Item("inm_bspdte")), CDate(dr.Item("inm_start")), _
                                                    CDate(dr.Item("inm_end")), CStr(dr.Item("inm_ldname")), CStr(dr.Item("inm_savecd")), CDec(dr.Item("inm_mxfare")), _
                                                    CDec(dr.Item("inm_lwfare")), CBool(dr.Item("inm_bookng")), CStr(dr.Item("inm_fcrncy")), CDec(dr.Item("inm_ffare")), _
                                                    CDec(dr.Item("inm_drate")), CDec(dr.Item("inm_days")), CStr(dr.Item("inm_curncy")), CDec(dr.Item("inm_fare")), _
                                                    CDec(dr.Item("inm_srvchg")), CDec(dr.Item("inm_ubtax")), CDec(dr.Item("inm_yqtax")), CDec(dr.Item("inm_othtax")), _
                                                    CDec(dr.Item("inm_tax")), CDec(dr.Item("inm_ourchg")), CDec(dr.Item("inm_amtvat")), CDec(dr.Item("inm_supvat")), _
                                                    CDec(dr.Item("inm_ourvat")), CDec(dr.Item("inm_amount")), CDec(dr.Item("inm_dscpct")), CDec(dr.Item("inm_discnt")), _
                                                    CDec(dr.Item("inm_billed")), CDec(dr.Item("inm_bilvat")), CDec(dr.Item("inm_compct")), CDec(dr.Item("inm_comvat")), _
                                                    CDec(dr.Item("inm_vtoncm")), CDec(dr.Item("inm_trucom")), CDec(dr.Item("inm_othcom")), CDec(dr.Item("inm_comamt")), _
                                                    CDec(dr.Item("inm_comrcv")), CDec(dr.Item("inm_comdue")), CBool(dr.Item("inm_paynet")), CBool(dr.Item("inm_vatinv")), _
                                                    CBool(dr.Item("inm_cominv")), CStr(dr.Item("inm_morpax")), CDec(dr.Item("inm_dposit")), CBool(dr.Item("inm_depok")), _
                                                    CStr(dr.Item("inm_depbr")), CStr(dr.Item("inm_depbnk")), CDate(dr.Item("inm_baldue")), CStr(dr.Item("inm_paytyp")), _
                                                    CStr(dr.Item("inm_ccid")), CStr(dr.Item("inm_ccno")), CStr(dr.Item("inm_ccstdt")), CStr(dr.Item("inm_ccexp")), _
                                                    CStr(dr.Item("inm_ccauth")), CDbl(dr.Item("inm_issue")), CBool(dr.Item("inm_merch")), CDec(dr.Item("inm_mfee")), _
                                                    CStr(dr.Item("inm_ccmeth")), CDec(dr.Item("inm_ccamt")), CDate(dr.Item("inm_ccxmit")), CDbl(dr.Item("inm_print")), _
                                                    CDate(dr.Item("inm_con1pd")), CDate(dr.Item("inm_con2pd")), CStr(dr.Item("inm_orig")), CBool(dr.Item("inm_locked")), _
                                                    CBool(dr.Item("inm_erflag")), CStr(dr.Item("inm_erdesc")), CStr(dr.Item("inm_ukey")), CBool(dr.Item("inm_change")), _
                                                    CStr(dr.Item("inm_who")), CStr(dr.Item("inm_note")), CStr(dr.Item("inm_crsref")), CStr(dr.Item("inm_bdm")), _
                                                    CStr(dr.Item("inm_pcity")), CStr(dr.Item("inm_ino")), CStr(dr.Item("inm_cos")), CStr(dr.Item("inm_domint")), _
                                                    CBool(dr.Item("inm_apok")), CBool(dr.Item("inm_arok")), CStr(dr.Item("inm_voided")), CDec(dr.Item("inm_agcomm")), _
                                                    CDec(dr.Item("inm_agvat")), CDec(dr.Item("inm_agdcpc")), CBool(dr.Item("inm_atol")), CStr(dr.Item("inm_abond")), _
                                                    CDec(dr.Item("inm_afare")), CDec(dr.Item("inm_aheads")), CBool(dr.Item("inm_nfare")), CDec(dr.Item("inm_rebate")), _
                                                    CDec(dr.Item("inm_fee")), CDec(dr.Item("inm_feevt")), CStr(dr.Item("inm_feebas")), CDate(dr.Item("inm_dla")), _
                                                    CStr(dr.Item("inm_bywho")), CStr(dr.Item("inm_cinvrf")), CStr(dr.Item("tmpfld")), CBool(dr.Item("inm_3rdpty")), _
                                                    CStr(dr.Item("inm_ourcc")), CStr(dr.Item("inm_itcode")), CStr(dr.Item("inm_bkcur")), CDec(dr.Item("inm_bkroe")), _
                                                    CStr(dr.Item("chksum")), Now, CStr(dr.Item("inm_miles1")), CStr(dr.Item("inm_miles2")), CStr(dr.Item("inm_km1")), _
                                                    CStr(dr.Item("inm_km2")), CDec(dr.Item("inm_disvat")), CStr(dr.Item("inm_cccvv")), CStr(dr.Item("inm_gcid")), _
                                                    CStr(dr.Item("inm_gcno")))
                    oIMain.save()
                Next
                Return True
            Catch ex As Exception

                'R2.21.2 - log the error, the boss sync service has done this since go live - no idea why this hasn't
                'send a local email if possible
                'AM this is really not a useful time to be sending hundreds of 
                'clsUseful.sendEmailLocal("craig.rickell@nysgroup.com", "craig.rickell@nysgroup.com", "selectInvRefs had an error", "Do something:" & ex.Message)
                clsUseful.sendEmailLocal("ashley.marron@nysgroup.com", "ashley.marron@nysgroup.com", "MAKE and SAVE L and oIMain.save", ex.Message)

                'log the error
                log.Error("INVOICE MAKE and SAVE  " & pstrInvoiceRef & ": " & ex.Message)

                Return False
            End Try
        End Using
    End Function

    Public Shared Function selectInvRouteRecords(ByVal pstrInvoiceRef As String) As Boolean
        Using New clslogger(log, className, "selectInvRouteRecords")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inr_key, inr_invno, inr_prod, inr_bookno, inr_segno, inr_from, inr_to, inr_sttime," & _
                                                                         "inr_start, inr_stterm, inr_etime, inr_end, inr_eterm, inr_flwncr, inr_flight, inr_class," & _
                                                                         "inr_bpmt, inr_fbasis, inr_fare, inr_dest, inr_miles, inr_fee, inr_feevt, inr_feebas,inr_status " & _
                                                                         "FROM invroute " & _
                                                                         "WHERE inr_invno = '" & pstrInvoiceRef & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()

                'first delete any records found in SQL
                BOSSinvroute.delete(0, pstrInvoiceRef)

                For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    'then save main record to SQL
                    Dim oRt As New BOSSinvroute(0, CStr(dr.Item("inr_key")), CStr(dr.Item("inr_invno")), CStr(dr.Item("inr_prod")), CInt(dr.Item("inr_bookno")), CInt(dr.Item("inr_segno")), _
                                                CStr(dr.Item("inr_from")), CStr(dr.Item("inr_to")), CStr(dr.Item("inr_sttime")), CDate(dr.Item("inr_start")), CStr(dr.Item("inr_stterm")), _
                                                CStr(dr.Item("inr_etime")), CDate(dr.Item("inr_end")), CStr(dr.Item("inr_eterm")), CStr(dr.Item("inr_flwncr")), CStr(dr.Item("inr_flight")), _
                                                CStr(dr.Item("inr_class")), CDec(dr.Item("inr_bpmt")), CStr(dr.Item("inr_fbasis")), CDec(dr.Item("inr_fare")), CBool(dr.Item("inr_dest")), _
                                                CInt(dr.Item("inr_miles")), CDec(dr.Item("inr_fee")), CDec(dr.Item("inr_feevt")), CStr(dr.Item("inr_feebas")), CStr(dr.Item("inr_status")), _
                                                Now)
                    oRt.save()
                Next
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function

    Public Shared Function runCashHdr(ByVal pstrInvoiceRef As String, ByVal pstrCSD_KEY As String) As String
        Using New clslogger(log, className, "runCashHdr")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim strWhere As String = "where CSH_REF = '" & pstrInvoiceRef & "'"
                If pstrCSD_KEY <> "" Then
                    strWhere = "where Csh_key = '" & pstrCSD_KEY & "'"
                End If
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT Csh_key,Csh_br,Csh_bnkdte,Csh_batch,Csh_type,Csh_from,Csh_id,Csh_ref,Csh_fop,Csh_vrfnd,Csh_rfnd," & _
                                                        "Csh_prod,Csh_bank,Csh_curncy,Csh_amount,Csh_used,Csh_remain,Csh_atol,Csh_rdate,Csh_note,Csh_locked,Csh_recon,Csh_recdte," & _
                                                        "Csh_status,Csh_who,Csh_cctxn,Csh_ccid,Csh_ccno,Csh_ccauth,Csh_ccexp,Csh_issue,Csh_merch,Csh_ccxmit,Csh_cshamt,Csh_ccamt," & _
                                                        "Csh_chqamt,Csh_vchamt,Csh_othamt,Csh_ccchg,Csh_mfee,Csh_bal,Csh_chqgte,Csh_chqno,Csh_vchid,Csh_vchref,Csh_vchnte,Csh_othid," & _
                                                        "Csh_othref,Csh_othnte,Chksum " & _
                                                        "FROM cashhdr " & _
                                                        strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "runCashHdr")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("runCashHdr").Rows
                    Dim oCashhdr As New BOSScashhdr(0, CStr(dr.Item("Csh_key")), CStr(dr.Item("Csh_br")), CType(dr.Item("Csh_bnkdte"), Date?), CStr(dr.Item("Csh_batch")), CStr(dr.Item("Csh_type")), _
                                                    CStr(dr.Item("Csh_from")), CStr(dr.Item("Csh_id")), CStr(dr.Item("Csh_ref")), CDec(dr.Item("Csh_fop")), CType(dr.Item("Csh_vrfnd"), Boolean?), _
                                                    CType(dr.Item("Csh_rfnd"), Boolean?), CStr(dr.Item("Csh_prod")), CStr(dr.Item("Csh_bank")), CStr(dr.Item("Csh_curncy")), CDec(dr.Item("Csh_amount")), _
                                                    CDec(dr.Item("Csh_used")), CDec(dr.Item("Csh_remain")), CDec(dr.Item("Csh_atol")), CType(dr.Item("Csh_rdate"), Date?), CStr(dr.Item("Csh_note")), _
                                                    CType(dr.Item("Csh_locked"), Boolean?), CType(dr.Item("Csh_recon"), Boolean?), CType(dr.Item("Csh_recdte"), Date?), CStr(dr.Item("Csh_status")), _
                                                    CStr(dr.Item("Csh_who")), CStr(dr.Item("Csh_cctxn")), CStr(dr.Item("Csh_ccid")), CStr(dr.Item("Csh_ccno")), CStr(dr.Item("Csh_ccauth")), _
                                                    CStr(dr.Item("Csh_ccexp")), CDec(dr.Item("Csh_issue")), CType(dr.Item("Csh_merch"), Boolean?), CType(dr.Item("Csh_ccxmit"), Date?), _
                                                    CDec(dr.Item("Csh_cshamt")), CDec(dr.Item("Csh_ccamt")), CDec(dr.Item("Csh_chqamt")), CDec(dr.Item("Csh_vchamt")), CDec(dr.Item("Csh_othamt")), _
                                                    CDec(dr.Item("Csh_ccchg")), CDec(dr.Item("Csh_mfee")), CDec(dr.Item("Csh_bal")), CStr(dr.Item("Csh_chqgte")), CStr(dr.Item("Csh_chqno")), _
                                                    CStr(dr.Item("Csh_vchid")), CStr(dr.Item("Csh_vchref")), CStr(dr.Item("Csh_vchnte")), CStr(dr.Item("Csh_othid")), CStr(dr.Item("Csh_othref")), _
                                                    CStr(dr.Item("Csh_othnte")), CStr(dr.Item("Chksum")))
                    oCashhdr.save()
                    If pstrCSD_KEY = "" Then
                        runCashSelect(CStr(dr.Item("Csh_key")), "")
                    End If
                Next

                myDataSet.Clear()
                Return "Successful run"
            Catch ex As Exception
                log.Error("INVOICE ERROR: " & pstrInvoiceRef, ex)
                Return ex.Message
            End Try
        End Using
    End Function

    Public Shared Function runCashSelect(ByVal pstrCsh_key As String, ByVal pstrInvoiceRef As String) As String
        Using New clslogger(log, className, "runCashSelect")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim strWhere As String = "where csd_key = '" & pstrCsh_key & "'"
                If pstrInvoiceRef <> "" Then
                    strWhere = "where csd_appkey = '" & pstrInvoiceRef & "'"
                End If
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT csd_key, csd_appid, csd_to, csd_applyd, csd_appkey, csd_invkey, csd_applgr, csd_arkey, " & _
                                                                        "csd_cdate, csd_today, csd_suppid, csd_prdkey, csd_ok, csd_final, csd_note, csd_locked, csd_status, " & _
                                                                        "csd_who, csd_ukey, chksum " & _
                                                                        "FROM cash " & _
                                                                        strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Cash")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("Cash").Rows
                    Dim oCash As New BOSScash(0, CStr(dr.Item("csd_key")), CStr(dr.Item("csd_appid")), CStr(dr.Item("csd_to")), CStr(dr.Item("csd_applyd")), CStr(dr.Item("csd_appkey")), CStr(dr.Item("csd_invkey")), _
                                                  CStr(dr.Item("csd_applgr")), CStr(dr.Item("csd_arkey")), CType(dr.Item("csd_cdate"), Date?), CType(dr.Item("csd_today"), Date?), CStr(dr.Item("csd_suppid")), CStr(dr.Item("csd_prdkey")), _
                                                  CStr(dr.Item("csd_ok")), CStr(dr.Item("csd_final")), CStr(dr.Item("csd_note")), CStr(dr.Item("csd_locked")), CStr(dr.Item("csd_status")), CStr(dr.Item("csd_who")), _
                                                  CStr(dr.Item("csd_ukey")), CStr(dr.Item("chksum")), Now)
                    oCash.save()
                    If pstrInvoiceRef <> "" Then
                        runCashHdr("", CStr(dr.Item("csd_key")))
                    End If
                Next

                myDataSet.Clear()
                Return "Successful run"
            Catch ex As Exception
                log.Error("INVOICE ERROR: " & pstrCsh_key, ex)
                Return ex.Message
            End Try
        End Using
    End Function

    Public Shared Function runPaydet(ByVal pstrInvoiceNumber As String) As Boolean
        Using New clslogger(log, className, "runPaydet")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT pyd_key, pyd_line, pyd_ctrl, pyd_cdate, pyd_today, pyd_branch, pyd_ledger, pyd_applyd, pyd_dr," & _
                                                                        "pyd_cr, pyd_apkey, pyd_ukey, pyd_locked, pyd_recon, pyd_recdte, pyd_ok, pyd_who " & _
                                                                        "FROM paydetl where pyd_applyd = '" & pstrInvoiceNumber & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "selectPaydet")
                dBaseConnection.Close()

                'first get rid of old records

                BOSSpaydetl.delete(0, "", pstrInvoiceNumber)

                For Each dr As DataRow In myDataSet.Tables("selectPaydet").Rows
                    Dim oPaydet As New BOSSpaydetl(0, CStr(dr.Item("pyd_key")), CStr(dr.Item("pyd_line")), CType(dr.Item("pyd_ctrl"), Boolean?), CStr(dr.Item("pyd_cdate")), CStr(dr.Item("pyd_today")), _
                                                   CStr(dr.Item("pyd_branch")), CStr(dr.Item("pyd_ledger")), CStr(dr.Item("pyd_applyd")), CDec(dr.Item("pyd_dr")), CDec(dr.Item("pyd_cr")), _
                                                   CStr(dr.Item("pyd_apkey")), CStr(dr.Item("pyd_ukey")), CType(dr.Item("pyd_locked"), Boolean?), CType(dr.Item("pyd_recon"), Boolean?), _
                                                   CStr(dr.Item("pyd_recdte")), CType(dr.Item("pyd_ok"), Boolean?), CStr(dr.Item("pyd_who")))
                    oPaydet.save()

                    runPayhdr(dr.Item("pyd_key"))
                Next

                myDataSet.Clear()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function

    Public Shared Function runPayhdr(ByVal ppyd_key As String) As Boolean
        Using New clslogger(log, className, "runPayhdr")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT pyh_key, pyh_type, pyh_cdate, pyh_date, pyh_bnkac, pyh_bnklgr, pyh_bnkref, pyh_branch, pyh_payee, pyh_suptyp, " & _
                                                                       "pyh_paynam, pyh_net, pyh_vat, pyh_bnkchg, pyh_amt, pyh_used, pyh_remain, pyh_who, pyh_recon, pyh_recdte, pyh_note, pyh_locked, " & _
                                                                       "pyh_recur, pyh_reckey, pyh_freq, pyh_rstart, pyh_howmny " & _
                                                                       "FROM payhdr where pyh_key = '" & ppyd_key & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "runPayhdr")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("runPayhdr").Rows
                    Dim oPayhdr As New BOSSpayhdr(0, CStr(dr.Item("pyh_key")), CStr(dr.Item("pyh_type")), CType(dr.Item("pyh_cdate"), Date?), CType(dr.Item("pyh_date"), Date?), _
                                                  CStr(dr.Item("pyh_bnkac")), CStr(dr.Item("pyh_bnklgr")), CStr(dr.Item("pyh_bnkref")), CStr(dr.Item("pyh_branch")), CStr(dr.Item("pyh_payee")), _
                                                  CStr(dr.Item("pyh_suptyp")), CStr(dr.Item("pyh_paynam")), CDec(dr.Item("pyh_net")), CDec(dr.Item("pyh_vat")), CDec(dr.Item("pyh_bnkchg")), _
                                                  CDec(dr.Item("pyh_amt")), CDec(dr.Item("pyh_used")), CDec(dr.Item("pyh_remain")), CStr(dr.Item("pyh_who")), CType(dr.Item("pyh_recon"), Boolean?), _
                                                  CType(dr.Item("pyh_recdte"), Date?), CStr(dr.Item("pyh_note")), CType(dr.Item("pyh_locked"), Boolean?), CType(dr.Item("pyh_recur"), Boolean?), _
                                                  CStr(dr.Item("pyh_reckey")), CStr(dr.Item("pyh_freq")), CType(dr.Item("pyh_rstart"), Date?), CDec(dr.Item("pyh_howmny")))
                    oPayhdr.save()
                Next

                myDataSet.Clear()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function


    Public Shared Function runHotelInvoice(ByVal pstrFrom As String, ByVal pstrVenue As String) As String
        Using New clslogger(log, className, "runHotelInvoice")

            'Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            'Dim myDataSet As New DataSet()
            'Dim myDataSetInner As New DataSet()

            Try

                'Dim diff1 As Integer = DateDiff(DateInterval.Day, CDate(pstrFrom), Now)

                'Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT DISTINCT invmain.inm_no " & _
                '                                                       "FROM invmain,supplier " & _
                '                                                       "where  invmain.inm_suppid = supplier.sup_id " & _
                '                                                       "and invmain.inm_invdt <= DATE() - " & diff1 & _
                '                                                       " and supplier.sup_name LIKE '" & pstrVenue.ToUpper & "%' " & _
                '                                                       " and inm_comdue > 0 " _
                '                                                      , dBaseConnection)

                'Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                'myDataAdapter.Fill(myDataSet, "Main")
                'dBaseConnection.Close()

                If pstrVenue = " " Then
                    pstrVenue = ""
                End If
                Dim oResults As List(Of CommissionChase)
                oResults = CommissionChase.outstandingCommissionInvoice(pstrFrom, pstrVenue)
                For Each oResult As CommissionChase In oResults
                    ' For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    If oResult.Invoiceid.Contains(Chr(187)) = False Then

                        If selectInvMainRecords(oResult.Invoiceid) Then
                            If selectInvtotRecord(oResult.Invoiceid) Then
                                If selectInvRouteRecords(oResult.Invoiceid) Then

                                End If
                            End If
                        End If
                        runCashHdr(oResult.Invoiceid, "")
                        runCashSelect("", oResult.Invoiceid)
                    End If
                Next

                Return "Successful run"
            Catch ex As Exception
                log.Error(ex.Message)
                Return ex.Message
            End Try
        End Using
    End Function

    Public Shared Function runUnpaidInvoice() As String
        Using New clslogger(log, className, "runUnpaidInvoice")

            Try

                Dim oResults As List(Of CommissionChase)
                oResults = CommissionChase.bossCommissionClaimUnpaidInvoice
                For Each oResult As CommissionChase In oResults
                    ' For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    If oResult.Invoiceid.Contains(Chr(187)) = False Then

                        'If selectInvMainRecords(oResult.Invoiceid) Then
                        '    If selectInvtotRecord(oResult.Invoiceid) Then
                        '        If selectInvRouteRecords(oResult.Invoiceid) Then

                        '        End If
                        '    End If
                        'End If
                        runCashHdr(oResult.Invoiceid, "")
                        runCashSelect("", oResult.Invoiceid)
                    End If
                Next

                Return "Successful run"
            Catch ex As Exception
                log.Error(ex.Message)
                Return ex.Message
            End Try
        End Using
    End Function


End Class
