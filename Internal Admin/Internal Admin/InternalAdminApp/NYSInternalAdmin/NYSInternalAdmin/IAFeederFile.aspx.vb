Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports NysDat
Imports System.Net
Imports System.IO
Imports System.Globalization

Partial Public Class IAFeederFile
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
                    btnCima.Visible = False
                    btnO2.Visible = False
                    btnToCsv.Visible = False
                    pndates.Style.Item("TOP") = "4px"

                    Dim strRet As String = setUser()

                    If strRet.StartsWith("ERROR") Then
                        Response.Redirect("IALogonAdmin.aspx?User=falseX")
                    End If
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "BS-DWP" Then
                        lbldwp.Visible = True
                        lblcmec.Visible = True
                        txtresult2.Visible = True
                        txtresult.Style.Item("HEIGHT") = "280px"
                        'R2.21.3 SA - BUG FIX
                    ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "NHS LITIGATION AUTHIORITY" Then
                        lbldwp.Visible = False
                        lblcmec.Visible = False
                        txtresult2.Visible = False
                        txtresult.Style.Item("HEIGHT") = "494px"

                    ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "NCAS" Then
                        lbldwp.Visible = False
                        lblcmec.Visible = False
                        txtresult2.Visible = False
                        txtresult.Style.Item("HEIGHT") = "494px"
                    Else
                        lbldwp.Visible = False
                        lblcmec.Visible = False
                        txtresult2.Visible = False
                        txtresult.Style.Item("HEIGHT") = "594px"
                    End If
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "LV=" Then
                        txtextra.Style.Item("TOP") = "13px"
                        lblextra.Style.Item("TOP") = "16px"
                    Else
                        txtextra.Style.Item("TOP") = "-100px"
                        lblextra.Style.Item("TOP") = "-100px"
                    End If
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "O2" Then
                        pndates.Style.Item("TOP") = "-100px"
                        createFile()
                    End If
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "CIMA" Then
                        lblCimaDate.Text = "Date last files sent: <br />" & clsCIMALastSentDate.getDate.ToString
                        lbldwp.Visible = True
                        lbldwp.Text = "Invoices"
                        lblcmec.Visible = True
                        lblcmec.Text = "Credits"
                        txtresult2.Visible = True
                        txtresult.Style.Item("HEIGHT") = "280px"
                    End If

                    'R2.13 CR
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "ROYAL COLLEGE OF NURSING" Then
                        lblextra.Text = "Direct Debit Date"
                        lblextra.Visible = True
                        txtextra.Visible = True
                        txtextra.Style.Item("TOP") = "13px"
                        lblextra.Style.Item("TOP") = "16px"
                        txtextra.Text = Date.Now.AddDays(5).ToString("dd/MM/yyyy")
                    End If

                    'R2.21 SA
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "ROYAL COLLEGE OF NURSING G" Then
                        lblextra.Text = "Direct Debit Date"
                        lblextra.Visible = True
                        txtextra.Visible = True
                        txtextra.Style.Item("TOP") = "13px"
                        lblextra.Style.Item("TOP") = "16px"
                        txtextra.Text = Date.Now.AddDays(5).ToString("dd/MM/yyyy")
                    End If

                    'R2.21 CR
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "INSTITUTION OF CIVIL ENGINEERS" Then
                        lblextra.Text = "Direct Debit Date"
                        lblextra.Visible = True
                        txtextra.Visible = True
                        txtextra.Style.Item("TOP") = "13px"
                        lblextra.Style.Item("TOP") = "16px"
                        txtextra.Text = Date.Now.AddDays(5).ToString("dd/MM/yyyy")
                    End If

                    setAjax()

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please complete both date fields to continue!", "Check details")
                End If
                Try
                    Dim dTemp As Date = CDate(txtfrom.Text)
                Catch ex As Exception
                    Throw New EvoFriendlyException("Please check From date is in the correct format 'dd/mm/yyyy' to continue!", "Check details")
                End Try
                Try
                    Dim dTemp As Date = CDate(txtto.Text)
                Catch ex As Exception
                    Throw New EvoFriendlyException("Please check To date is in the correct format 'dd/mm/yyyy' to continue!", "Check details")
                End Try
                If CDate(txtfrom.Text) > CDate(txtto.Text) Then
                    Throw New EvoFriendlyException("The From date must be set before to To date to continue!", "Check details")
                End If
                If CStr(Session.Item("FeederFileClient")).ToUpper = "LV=" Then
                    If txtextra.Text <> "" Then
                        Try
                            Dim dTemp As Date = CDate(txtextra.Text)
                        Catch ex As Exception
                            Throw New EvoFriendlyException("Please check Extra date is in the correct format 'dd/mm/yyyy' to continue!", "Check details")
                        End Try
                    End If
                End If
                createFile()

            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub createFile()
        Using New clslogger(log, className, "createFile")

            If Session.Item("FeederFileClient") IsNot Nothing Then

                If CStr(Session.Item("FeederFileClient")).ToUpper <> "NONE" Then
                    If txtfrom.Text = "" Then
                        txtfrom.Text = Format(Now.AddDays(-40), "dd/MM/yyyy")
                    End If
                    If txtto.Text = "" Then
                        txtto.Text = Format(Now, "dd/MM/yyyy")
                    End If

                    'R2.17 CR
                    Dim strBossClientCode As String = CStr(Session.Item("FeederFileClient"))
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "NCAS" Then
                        strBossClientCode = "NPSA"
                    End If

                    'R2.21.3 SA- BUG FIX                              
                    If CStr(Session.Item("FeederFileClient")).ToUpper = "NHS LITIGATION AUTHORITY" Then
                        strBossClientCode = "NPSA"
                    End If

                    Dim strRet As String = "" 'runInvoiceNumberCheck(txtfrom.Text, txtto.Text, CStr(Session.Item("FeederFileClient")))
                    If strRet <> "" Then
                        Throw New EvoFriendlyException(strRet, "Check details")
                    End If
                End If

                If CStr(Session.Item("FeederFileClient")).ToUpper = "LV=" Then
                    txtresult.Text = FeederFileCreation.createFileLV(txtfrom.Text, txtto.Text, txtextra.Text)
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "CIMA" Then
                    Dim o As New FeederFileCreation
                    txtresult.Text = o.createFileCIMA(txtfrom.Text, txtto.Text)

                    'lets go see if there are any credits
                    Dim oCredits As New List(Of CIMA)
                    oCredits = CIMA.FeederFileCIMACredits(txtfrom.Text, txtto.Text)

                    'R1.2 NM
                    Dim intDayDiff As Integer = notInteger(Math.Ceiling(DateDiff(DateInterval.Day, CDate(txtfrom.Text), CDate(txtto.Text))))
                    'go find PDF and move it to the XML folder so we can send them

                    For x As Integer = 0 To intDayDiff
                        Dim localDate As Date = CDate(txtfrom.Text).AddDays(x)

                        Dim strDatePath As String = "_" & CStr(localDate.Year) & CStr(IIf(localDate.Month < 10, "0" & CStr(localDate.Month), CStr(localDate.Month)))
                        Dim strReadFromFolder As String = getConfig("PdfFilesInput").Replace("###", strDatePath)
                        Dim strSentFolder As String = getConfig("PdfFilesSent").Replace("###", strDatePath)
                        Dim strNotSentFolder As String = getConfig("PdfFilesNotSent").Replace("###", strDatePath)
                        Dim strErrorFolder As String = getConfig("PdfFilesError").Replace("###", strDatePath)
                        Dim strAwaitingFolder As String = getConfig("PdfFilesAwaiting")

                        makeFolderExist(getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy"))
                        makeFolderExist(getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\SENT\")

                        For Each oCredit As CIMA In oCredits
                            If File.Exists(strReadFromFolder & oCredit.Invoicenumber & "M.pdf") Then
                                Try
                                    File.Copy(strReadFromFolder & oCredit.Invoicenumber & "M.pdf", getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\" & oCredit.Invoicenumber & "M.pdf")
                                Catch ex As Exception
                                    log.Error("CIMA File not found:" & oCredit.Invoicenumber & "M.pdf")
                                End Try
                            ElseIf File.Exists(strSentFolder & oCredit.Invoicenumber & "M.pdf") Then
                                Try
                                    File.Copy(strSentFolder & oCredit.Invoicenumber & "M.pdf", getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\" & oCredit.Invoicenumber & "M.pdf")
                                Catch ex As Exception
                                    log.Error("CIMA File not found:" & oCredit.Invoicenumber & "M.pdf")
                                End Try
                            ElseIf File.Exists(strNotSentFolder & oCredit.Invoicenumber & "M.pdf") Then
                                Try
                                    File.Copy(strNotSentFolder & oCredit.Invoicenumber & "M.pdf", getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\" & oCredit.Invoicenumber & "M.pdf")
                                Catch ex As Exception
                                    log.Error("CIMA File not found:" & oCredit.Invoicenumber & "M.pdf")
                                End Try
                            ElseIf File.Exists(strErrorFolder & oCredit.Invoicenumber & "M.pdf") Then
                                Try
                                    File.Copy(strErrorFolder & oCredit.Invoicenumber & "M.pdf", getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\" & oCredit.Invoicenumber & "M.pdf")
                                Catch ex As Exception
                                    log.Error("CIMA File not found:" & oCredit.Invoicenumber & "M.pdf")
                                End Try
                            ElseIf File.Exists(strAwaitingFolder & oCredit.Invoicenumber & "M.pdf") Then
                                Try
                                    File.Copy(strAwaitingFolder & oCredit.Invoicenumber & "M.pdf", getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\" & oCredit.Invoicenumber & "M.pdf")
                                Catch ex As Exception
                                    log.Error("CIMA File not found:" & oCredit.Invoicenumber & "M.pdf")
                                End Try
                            End If
                        Next
                    Next

                    'double check pdf files have been copied to correct place
                    Dim notFoundCount As Integer = 0
                    For Each oCredit2 As CIMA In oCredits
                        If File.Exists(getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\" & oCredit2.Invoicenumber & "M.pdf") Then
                            txtresult2.Text = txtresult2.Text & oCredit2.Invoicenumber & " found" & vbCrLf
                        Else
                            txtresult2.Text = txtresult2.Text & oCredit2.Invoicenumber & " NOT FOUND - please F8 the invoice in BOSS to produce PDF file" & vbCrLf
                            notFoundCount += 1
                        End If
                    Next

                    If notFoundCount > 0 Then
                        btnCima.Visible = False
                    Else
                        btnCima.Visible = True
                    End If

                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "NHS BUSINESS SERVICES AUTHORITY" Then
                    Dim o As New FeederFileCreation
                    txtresult.Text = o.createFileNHSBSA(txtfrom.Text, txtto.Text)
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "O2" Then
                    btnO2.Visible = True
                    Dim o As New FeederFileCreation
                    txtresult.Text = o.runO2POCheck()
                    'R2.23.1 AI
                    btnO2AdquiraInstructions.Visible = True
                    hlFeederFiles.Visible = True
                    hlFeederFiles.Text = "View the new feeder files"
                    hlFeederFiles.NavigateUrl = getConfig("XMLFilePath") & "\O2ADQUIRA\" & Format(Now, "dd-MM-yyyy") & "\"

                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "HIGHWAYS AGENCY" Then
                    txtresult.Text = FeederFileCreation.createFileHighways(txtfrom.Text, txtto.Text, "")
                    btnToCsv.Visible = True
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "BS-DWP" Then
                    txtresult.Text = FeederFileCreation.createFileDWP(txtfrom.Text, txtto.Text, "DWP")
                    txtresult2.Text = FeederFileCreation.createFileDWP(txtfrom.Text, txtto.Text, "CMEC")
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "BS - ANCHOR TRUST" Then
                    txtresult.Text = FeederFileCreation.createFileAnchor(txtfrom.Text, txtto.Text)
                    btnToCsv.Visible = True
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "NPSA" Then
                    txtresult.Text = FeederFileCreation.createFileNPSA(txtfrom.Text, txtto.Text)
                    btnToCsv.Visible = True


                    'R2.17 CR
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "NCAS" Then

                    'Dim blnRecordsUpdated As Boolean = False
                    'Dim strError As String = ""
                    'Try
                    '    Dim oTest As New List(Of NCAS)
                    '    oTest = NCAS.list(txtfrom.Text, txtto.Text)
                    '    For Each oBiscuit As NCAS In oTest
                    '        'Update SQL before running!!
                    '        clsBoss.runMainSelect(False, oBiscuit.InvoiceRef, "", "", "")
                    '    Next

                    '    blnRecordsUpdated = True
                    'Catch ex As Exception
                    '    blnRecordsUpdated = False
                    '    strError = ex.Message
                    'End Try

                    'If blnRecordsUpdated Then
                    'Now attempt the creation
                    txtresult.Text = FeederFileCreation.createFileNCAS(txtfrom.Text, txtto.Text)
                    btnToCsv.Visible = True
                    btnToCsv.Text = "Send Feeder File"
                    hlFeederFiles.Visible = True
                    hlFeederFiles.Text = "View the new feeder files"
                    hlFeederFiles.NavigateUrl = getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy") & "\"
                    'Else
                    '    Throw New EvoFriendlyException("Records for feeder file were unable to update - " & strError, "Error")
                    'End If


                    'R2.21.3 SA - BUG FIX!!!
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "NHS LITIGATION AUTHORITY" Then
                    txtresult.Text = FeederFileCreation.createFileNCAS(txtfrom.Text, txtto.Text)
                    btnToCsv.Visible = True
                    btnToCsv.Text = "Send Feeder File"
                    hlFeederFiles.Visible = True
                    hlFeederFiles.Text = "View the new feeder files"
                    hlFeederFiles.NavigateUrl = getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy") & "\"

                    'R2.11 CR
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "ROYAL COLLEGE OF NURSING" Then
                    txtresult.Text = FeederFileCreation.createFileRCN(txtfrom.Text, txtto.Text, txtextra.Text)
                    btnToCsv.Visible = True
                    'R2.21 SA
                    btnToCsv.Text = "Send Feeder File"
                    hlFeederFiles.Visible = True
                    hlFeederFiles.Text = "View the new feeder files"
                    hlFeederFiles.NavigateUrl = getConfig("XMLFilePath") & "\RCN100\" & Format(Now, "dd-MM-yyyy") & "\"

                    'R2.21 SA 
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "ROYAL COLLEGE OF NURSING G" Then
                    txtresult.Text = FeederFileCreation.createFileRCNG(txtfrom.Text, txtto.Text, txtextra.Text)
                    btnToCsv.Visible = True
                    'R2.21 SA
                    btnToCsv.Text = "Send Feeder File"
                    hlFeederFiles.Visible = True
                    hlFeederFiles.Text = "View the new feeder files"
                    hlFeederFiles.NavigateUrl = getConfig("XMLFilePath") & "\RCNG100\" & Format(Now, "dd-MM-yyyy") & "\"

                    'R2.12 CR
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "INSTITUTION OF CIVIL ENGINEERS" Then
                    txtresult.Text = FeederFileCreation.createFileICE(txtfrom.Text, txtto.Text, txtextra.Text)
                    btnToCsv.Visible = True
                    btnToCsv.Text = "Send Feeder File"
                    hlFeederFiles.Visible = True
                    hlFeederFiles.Text = "View the new feeder files"
                    hlFeederFiles.NavigateUrl = getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\"

                Else
                    Throw New EvoFriendlyException("There is no Feeder file creation set up for the selected client!", "Check details")
                End If
            Else
                Throw New EvoFriendlyException("There is no Feeder file creation set up for the selected client!", "Check details")
            End If
        End Using
    End Sub

    Private Sub setAjax()
        Using New clslogger(log, className, "setAjax")
            cexFrom.CssClass = "cal_Theme1"
            cexFrom.Format = "dd/MM/yyyy"
            cexTo.CssClass = "cal_Theme1"
            cexTo.Format = "dd/MM/yyyy"
            cexExtra.CssClass = "cal_Theme1"
            cexExtra.Format = "dd/MM/yyyy"
        End Using
    End Sub

    Private Sub IAFeederFile_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAFeederFile_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IAFeederFile"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
        End Using
    End Sub

    Public Shared Function sendCimaXml(ByVal pXml As String, ByVal pblnCEL As Boolean) As String
        Using New clslogger(log, className, "sendCimaXml")
            Dim url As String = getConfig("ProactisURLCIMA")
            If pblnCEL Then
                url = getConfig("ProactisURLCEL")
            End If

            Dim postRequest As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
            Dim response As String = ""
            postRequest.Method = "POST"

            Dim postBuffer As Byte() = Text.Encoding.ASCII.GetBytes(pXml)
            postRequest.ContentLength = postBuffer.Length

            Dim postRequestStream As New StreamWriter(postRequest.GetRequestStream(), Text.Encoding.ASCII)
            postRequestStream.Write(pXml)
            postRequestStream.Close()

            Dim postResponse As HttpWebResponse = DirectCast(postRequest.GetResponse(), HttpWebResponse)

            Using sr As New StreamReader(postResponse.GetResponseStream())
                response = sr.ReadToEnd()
            End Using
            Return response
        End Using
    End Function

    Protected Sub btnCima_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCima.Click
        Using New clslogger(log, className, "btnCima_Click")
            Try
                'do CIMA first
                Dim strPathX As String = getConfig("XMLFilePath") & "\CIMA\CIMA\" & Format(Now, "dd-MM-yyyy") & "\"
                Dim strPathXMove As String = getConfig("XMLFilePath") & "\CIMA\CIMA\" & Format(Now, "dd-MM-yyyy") & "\SENT\"
                Dim ofileX As System.IO.StreamReader
                Dim strError As New StringBuilder
                Dim intCimaCount As Integer = 0
                Dim intCelCount As Integer = 0

                If IO.Directory.Exists(strPathX) Then
                    For Each strfileX As String In IO.Directory.GetFiles(strPathX)
                        Dim strJustFileX As String = New System.IO.FileInfo(strfileX).Name

                        ofileX = New System.IO.StreamReader(strPathX & strJustFileX)
                        Dim strRepNameX As String = ofileX.ReadToEnd
                        ofileX.Close()
                        strRepNameX = strRepNameX.Trim
                        If strRepNameX <> "" Then
                            Dim strRet As String = ""
                            If getConfig("BatchTest") = "true" Then
                                strRet = "Status code=""200"" text=""OK"""
                            Else
                                strRet = sendCimaXml(strRepNameX, False)
                            End If

                            If Not strRet.Contains("Status code=""200"" text=""OK""") Then
                                strError.Append(strJustFileX & " - " & strRet & vbCrLf)
                            Else
                                intCimaCount += 1
                                IO.File.Move(strPathX & strJustFileX, strPathXMove & strJustFileX)

                                'R2.5 CR
                                'Save invoice to db table for records
                                Dim oCIMAInvoice As New clsCIMASentInvoice
                                oCIMAInvoice.CIMASentInvoiceID = 0
                                oCIMAInvoice.DateSent = Date.Now().ToString("dd/MM/yyyy")
                                oCIMAInvoice.InvoiceRef = strJustFileX.Replace("NYS", "").Replace(".xml", "")
                                oCIMAInvoice.save()
                                oCIMAInvoice = Nothing
                            End If
                        End If
                    Next
                End If

                'now do CEL 
                Dim strPathCEL As String = getConfig("XMLFilePath") & "\CIMA\CEL\" & Format(Now, "dd-MM-yyyy") & "\"
                Dim strPathCELMove As String = getConfig("XMLFilePath") & "\CIMA\CEL\" & Format(Now, "dd-MM-yyyy") & "\SENT\"
                Dim ofileCEL As System.IO.StreamReader

                If IO.Directory.Exists(strPathCEL) Then
                    For Each strfileCEL As String In IO.Directory.GetFiles(strPathCEL)
                        Dim strJustFileCEL As String = New System.IO.FileInfo(strfileCEL).Name

                        ofileCEL = New System.IO.StreamReader(strPathCEL & strJustFileCEL)
                        Dim strRepNameCEL As String = ofileCEL.ReadToEnd
                        ofileCEL.Close()
                        strRepNameCEL = strRepNameCEL.Trim
                        If strRepNameCEL <> "" Then
                            Dim strRet As String = ""
                            If getConfig("BatchTest") = "true" Then
                                strRet = "Status code=""200"" text=""OK"""
                            Else
                                strRet = sendCimaXml(strRepNameCEL, True)
                            End If

                            If Not strRet.Contains("Status code=""200"" text=""OK""") Then
                                strError.Append(strJustFileCEL & " - " & strRet & vbCrLf)
                            Else
                                intCelCount += 1
                                IO.File.Move(strPathCEL & strJustFileCEL, strPathCELMove & strJustFileCEL)

                                'R2.5 CR
                                'Save invoice to db table for records
                                Dim oCIMAInvoice As New clsCIMASentInvoice
                                oCIMAInvoice.CIMASentInvoiceID = 0
                                oCIMAInvoice.DateSent = Date.Now().ToString("dd/MM/yyyy")
                                oCIMAInvoice.InvoiceRef = strJustFileCEL.Replace("NYS", "").Replace(".xml", "")
                                oCIMAInvoice.save()
                                oCIMAInvoice = Nothing
                            End If
                        End If
                    Next
                End If

                'now do CREDITS 
                Dim strPathCREDITS As String = getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\"
                Dim strPathCREDITSMove As String = getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\SENT\"

                Dim intFileCount As Integer = 0
                If IO.Directory.Exists(strPathCREDITS) Then
                    For Each strfileCREDITS As String In IO.Directory.GetFiles(strPathCREDITS)
                        intFileCount += 1
                    Next
                End If

                Dim strCreditText As String = ""

                If intFileCount > 0 Then
                    If SendEmailMessage("accounts@nysgroup.com", getConfig("CIMAEmail"), _
                                         "Credit Invoices submitted from NYS Corporate:" & Format(Now, "dd/MM/yyyy"), _
                                         "Please find attached the credit invoices", "", "", "", getConfig("CIMABcc"), "", "", strPathCREDITS) Then
                        For Each strfileCREDITS As String In IO.Directory.GetFiles(strPathCREDITS)
                            Dim strJustFileCREDITS As String = New System.IO.FileInfo(strfileCREDITS).Name
                            Dim oCIMAInvoice As New clsCIMASentInvoice
                            oCIMAInvoice.CIMASentInvoiceID = 0
                            oCIMAInvoice.DateSent = Date.Now().ToString("dd/MM/yyyy")
                            oCIMAInvoice.InvoiceRef = strJustFileCREDITS.Replace("M.pdf", "")
                            oCIMAInvoice.save()
                            oCIMAInvoice = Nothing

                            IO.File.Copy(strPathCREDITS & strJustFileCREDITS, strPathCREDITSMove & strJustFileCREDITS)
                            Try
                                IO.File.Delete(strPathCREDITS & strJustFileCREDITS)
                            Catch ex As Exception

                            End Try
                        Next
                    Else
                        strError.Append("Error sending email with attachments" & vbCrLf)
                    End If
                Else
                    log.Info("No credits to send on: " & Date.Now().ToString("dd/MM/yyyy"))
                End If

                If strError.Length > 0 Then
                    txtresult.Text = "The following errors occurred when submitting the files: " & vbCrLf & strError.ToString
                Else
                    Try
                        Dim strMessage As String = "All files submitted successfully." & vbCrLf & _
                                                    "CIMA files: " & intCimaCount & vbCrLf & _
                                                    "CEL files: " & intCelCount & "<br>" & _
                                                    "Credit files: " & intFileCount

                        SendEmailMessage("accounts@nysgroup.com", getConfig("CIMABcc"), _
                                         "Invoices submitted from NYS Corporate:" & Format(Now, "dd/MM/yyyy"), _
                                         strMessage, "", "", "", "", "", "", "")

                        'R2.5 CR
                        clsCIMALastSentDate.save()
                        lblCimaDate.Text = "Date last files sent: <br />" & clsCIMALastSentDate.getDate.ToString

                    Catch ex As Exception
                        log.Error("CIMA EMAIL ERROR: " & ex.Message)
                    End Try
                    Throw New EvoFriendlyException("All files submitted successfully.<br>" & _
                                                   "CIMA files: " & intCimaCount & "<br>" & _
                                                   "CEL files: " & intCelCount & "<br>" & _
                                                   "Credit files: " & intFileCount, "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnO2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnO2.Click
        Using New clslogger(log, className, "btnO2_Click")
            Try
                'first need to get a list of the invoices that are needing to go.

                'then we have to check the database for that PO number

                'check to see if the PO is Steria or Adquira then put it on the relevant file
                ' if steria: check the PO again to see if it's HQ or UK
                ' if adquira: they all go on the same file, but it cant be sent automatically so show the file path to the user & give instructions

                'get the list of invoices that need to be sent - just get them all
                Dim rs As List(Of O2)
                rs = O2.FeederFileO2InvoiceList("%")

                Dim strSteriaUK As New StringBuilder
                Dim strSteriaHQ As New StringBuilder
                Dim strAdquira As New StringBuilder

                Dim strSteriaUKFailures As New StringBuilder
                Dim strSteriaHQFailures As New StringBuilder
                Dim strAdquiraFailures As New StringBuilder

                Dim intSteriaUKLineCount As Integer = 0
                Dim intSteriaHQLineCount As Integer = 0

                Dim dictAdquiraValidInvoices As New Dictionary(Of String, String)
                Dim dictSteriaUKValidInvoices As New Dictionary(Of String, String)
                Dim dictSteriaHQValidInvoices As New Dictionary(Of String, String)

                'now work out where they are going & build the lines
                For Each oInvoice In rs


                    'populate the full odetails of the order
                    Dim oPurchaseOrder As New clsO2Po
                    oPurchaseOrder.mstrOrderNumber = oInvoice.Po
                    oPurchaseOrder = oPurchaseOrder.GetByOrderNumber()

                    Dim oResults As New FeederFileCreation.o2details

                    'check which system the PO originated from
                    If notString(getConfig("O2SteriaActive")).ToLower = "true" AndAlso _
                        oPurchaseOrder.mstrSourceSystemName.ToLower = "steria" Then
                        'go and do all the steria stuff

                        If oInvoice.Po.StartsWith("401") Then
                            'add to the UK file
                            If intSteriaUKLineCount = 0 Then
                                intSteriaUKLineCount = 1
                            End If
                            oResults = FeederFileCreation.createPhysicalFileO2_Steria(oInvoice.Invoicenumber, intSteriaUKLineCount)
                            intSteriaUKLineCount = oResults.intlinecount

                            strSteriaUK.Append(oResults.strDetails)

                            If oResults.vatfailure Then
                                strSteriaUKFailures.Append("VAT failure on file creation for invoice " & oResults.invoice & ". " & vbCrLf)
                            End If
                            If oResults.totalsfailure Then
                                strSteriaUKFailures.Append("Totals failure on file creation for invoice " & oResults.invoice & ". " & vbCrLf)
                            End If

                            'if no failures at all add the invoice & po to the list for saving as sent later 
                            '(don't do it now otherwise OK invoices wont get sent when failures are fixed and re-run)
                            If oResults.vatfailure = False AndAlso oResults.totalsfailure = False Then
                                dictSteriaUKValidInvoices.Add(oInvoice.Invoicenumber, oInvoice.Po)
                            End If

                        ElseIf oInvoice.Po.StartsWith("415") Then
                            'add to the HQ file
                            If intSteriaHQLineCount = 0 Then
                                intSteriaHQLineCount = 1
                            End If
                            oResults = FeederFileCreation.createPhysicalFileO2_Steria(oInvoice.Invoicenumber, intSteriaHQLineCount)
                            intSteriaHQLineCount = oResults.intlinecount

                            strSteriaHQ.Append(oResults.strDetails)

                            If oResults.vatfailure Then
                                strSteriaHQFailures.Append("VAT failure on file creation for invoice " & oResults.invoice & ". " & vbCrLf)
                            End If
                            If oResults.totalsfailure Then
                                strSteriaHQFailures.Append("Totals failure on file creation for invoice " & oResults.invoice & ". " & vbCrLf)
                            End If

                            'if no failures at all add the invoice & po to the list for saving as sent later 
                            '(don't do it now otherwise OK invoices wont get sent when failures are fixed and re-run)
                            If oResults.vatfailure = False AndAlso oResults.totalsfailure = False Then
                                dictSteriaHQValidInvoices.Add(oInvoice.Invoicenumber, oInvoice.Po)
                            End If

                        End If

                        'ElseIf oPurchaseOrder.mstrSourceSystemName.ToLower = "adquira" Then
                    Else
                        'go and do all the adquira stuff

                        'attempt to build the invoice lines and check the results
                        oResults = FeederFileCreation.createPhysicalFileO2_Adquira(oInvoice.Invoicenumber)

                        'add the details to the file
                        strAdquira.Append(oResults.strDetails)

                        'if any failures add them to the failures string
                        If oResults.vatfailure Then
                            strAdquiraFailures.Append("Vat failure for invoice " & oInvoice.Invoicenumber & ". " & vbCrLf)
                        End If
                        If oResults.totalsfailure Then
                            strAdquiraFailures.Append("Totals failure for invoice " & oInvoice.Invoicenumber & ". " & vbCrLf)
                        End If

                        'if no failures at all add the invoice & po to the list for saving as sent later 
                        '(don't do it now otherwise OK invoices wont get sent when failures are fixed and re-run)
                        If oResults.vatfailure = False AndAlso oResults.totalsfailure = False Then
                            dictAdquiraValidInvoices.Add(oInvoice.Invoicenumber, oInvoice.Po)
                        End If

                        'Else
                        '    'do something else - error is probably best rather than assuming its one of the above
                        '    'would mean an option hasn't been selected
                    End If
                Next



                'create unfailed files

                'sort the adquira file
                If strAdquiraFailures.ToString = "" Then
                    If strAdquira.ToString <> "" Then

                        Try
                            'create the physical file
                            makeFolderExist(getConfig("XMLFilePath") & "\O2 Adquira")
                            makeFolderExist(getConfig("XMLFilePath") & "\O2 Adquira\" & Format(Now, "dd-MM-yyyy"))
                            Dim strFileName As String = ""

                            strFileName = "NYS Invoice List " & Format(Date.Now, "dd-MM-yyyy") & ".csv"

                            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\O2 Adquira\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

                            ofiler.Write(strAdquira.ToString)
                            ofiler.Flush()
                            ofiler.Close()

                            strAdquiraFailures.Append("Adquira: File created successfully. ** Please MANUALLY upload this document to Adquira using steps below. **")
                            ClientScript.RegisterStartupScript(Me.GetType(), "AdquiraInstructions", "<script>window.open('IAFeederAdquiraInstructions.aspx','AdquiraInstructions','titlebar=no,toolbar=no,directories=no,status=yes,scrollbars=yes,resizable=yes,resize=no,menubar=no,height=550,width=750,top=0,left=0'')</script>")

                            'now save the invoices as sent in the database
                            For Each dictInvoice In dictAdquiraValidInvoices
                                Dim oAdmin As New O2PoAdmin(0, dictInvoice.Key, dictInvoice.Value, "Sent in Adquira Feeder File", Format(Now, "dd/MM/yyyy"))
                                oAdmin.save()
                            Next
                        Catch ex As Exception
                            strAdquiraFailures.Append("Adquira: File Creation Error " & ex.Message)
                        End Try

                    Else
                        'nothing to send on this run
                        strAdquiraFailures.Append("Adquira: Nothing to send on this run.")
                    End If
                End If


                'sort the steria HQ file
                If strSteriaHQFailures.ToString = "" Then
                    'create the physical file
                    If strSteriaHQ.ToString <> "" Then

                        Try
                            makeFolderExist(getConfig("XMLFilePath") & "\O2")
                            makeFolderExist(getConfig("XMLFilePath") & "\O2\" & Format(Now, "dd-MM-yyyy"))

                            Dim strFileName As String = ""
                            Dim intVersionNo As Integer = 0
                            intVersionNo = FeederFileCreation.BatchNoGet("O2HQ")

                            'add the header to the file
                            Dim strHeadCsv As New StringBuilder
                            strHeadCsv.Append("H~")
                            strHeadCsv.Append("NYSHQPO~")
                            strHeadCsv.Append(intVersionNo & "~") 'version no
                            strHeadCsv.Append("0~") 'revision no
                            strHeadCsv.Append(Format(Date.Now, "dd-MM-yyyy"))
                            strHeadCsv.Append("^" & vbNewLine)

                            'increment the line count one last time
                            intSteriaHQLineCount += 1

                            'add the trailer to the file
                            strSteriaHQ.Append("T~")
                            strSteriaHQ.Append(intSteriaHQLineCount)
                            strSteriaHQ.Append("^" & vbNewLine)

                            strFileName = "AP_EI_HQ_NYSHQPO_" & Format(Date.Now, "dd-MM-yyyy") & "_" & intVersionNo.ToString & ".dat"

                            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\O2\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

                            ofiler.Write(strHeadCsv.ToString & strSteriaHQ.ToString)
                            ofiler.Flush()
                            ofiler.Close()

                            strSteriaHQFailures.Append("Steria HQ: File created successfully.")

                            'now save the invoices as sent in the database
                            For Each dictInvoice In dictSteriaHQValidInvoices
                                Dim oAdmin As New O2PoAdmin(0, dictInvoice.Key, dictInvoice.Value, "Sent in Steria Feeder File HQ" & intVersionNo.ToString, Format(Now, "dd/MM/yyyy"))
                                oAdmin.save()
                            Next
                        Catch ex As Exception
                            strSteriaHQFailures.Append("Steria HQ: File Creation Error " & ex.Message)
                        End Try
                        

                    Else
                        'nothing to send on this run
                        strSteriaHQFailures.Append("Steria HQ: Nothing to send on this run.")
                    End If
                End If

                'sort the steria UK file
                If strSteriaUKFailures.ToString = "" Then
                    'create the physical file
                    If strSteriaUK.ToString <> "" Then

                        Try
                            makeFolderExist(getConfig("XMLFilePath") & "\O2")
                            makeFolderExist(getConfig("XMLFilePath") & "\O2\" & Format(Now, "dd-MM-yyyy"))

                            Dim strFileName As String = ""
                            Dim intVersionNo As Integer = 0
                            intVersionNo = FeederFileCreation.BatchNoGet("O2HQ")

                            'add the header to the file
                            Dim strHeadCsv As New StringBuilder
                            strHeadCsv.Append("H~")
                            strHeadCsv.Append("NYSUKPO~")
                            strHeadCsv.Append(intVersionNo & "~") 'version no
                            strHeadCsv.Append("0~") 'revision no
                            strHeadCsv.Append(Format(Date.Now, "dd-MM-yyyy"))
                            strHeadCsv.Append("^" & vbNewLine)

                            'increment the line count one last time
                            intSteriaUKLineCount += 1

                            'add the trailer to the file
                            strSteriaUK.Append("T~")
                            strSteriaUK.Append(intSteriaUKLineCount)
                            strSteriaUK.Append("^" & vbNewLine)

                            strFileName = "AP_EI_UK_NYSUKPO_" & Format(Date.Now, "dd-MM-yyyy") & "_" & intVersionNo.ToString & ".dat"

                            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\O2\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

                            ofiler.Write(strHeadCsv.ToString & strSteriaUK.ToString)
                            ofiler.Flush()
                            ofiler.Close()

                            strSteriaUKFailures.Append("Steria UK: File created successfully.")

                            'now save the invoices as sent in the database
                            For Each dictInvoice In dictSteriaUKValidInvoices
                                Dim oAdmin As New O2PoAdmin(0, dictInvoice.Key, dictInvoice.Value, "Sent in Steria Feeder File UK" & intVersionNo.ToString, Format(Now, "dd/MM/yyyy"))
                                oAdmin.save()
                            Next
                        Catch ex As Exception
                            strSteriaUKFailures.Append("Steria UK: File Creation Error " & ex.Message)
                        End Try

                    Else
                        'nothing to send on this run
                        strSteriaUKFailures.Append("Steria UK: Nothing to send on this run.")
                    End If
                End If

                btnO2Emails.Visible = True

                'now throw a friendly exception to show the results of this run
                Throw New EvoFriendlyException(strAdquiraFailures.ToString & "<br/>" & strSteriaUKFailures.ToString & "<br/>" & strSteriaHQFailures.ToString, "Info")

            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
            
        End Using
    End Sub

    'Protected Sub btnO2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnO2.Click
    '    Using New clslogger(log, className, "btnO2_Click")
    '        Try
    '            Dim strHQRet As String = ""

    '            'need to populate this before creating the file otherwise it will be empty!
    '            Dim oList As New List(Of O2)
    '            oList = O2.FeederFileO2Email()
    '            ViewState.Item("FeederFileO2Email") = oList
    '            oList = Nothing

    '            Dim oA As New FeederFileCreation
    '            strHQRet = oA.createO2Files("415%", True)
    '            If strHQRet = "" Then
    '                strHQRet = oA.createO2Files("415%", False)
    '            End If

    '            Dim strUKRet As String = ""
    '            Dim oB As New FeederFileCreation
    '            strUKRet = oB.createO2Files("401%", True)
    '            If strUKRet = "" Then
    '                strUKRet = oA.createO2Files("401%", False)
    '            End If

    '            btnO2Emails.Visible = True
    '            If strHQRet = "" And strUKRet = "" Then
    '                Throw New EvoFriendlyException("Both files created successfully.", "Info")
    '            ElseIf strHQRet <> "" And strUKRet = "" Then
    '                Throw New EvoFriendlyException(strHQRet & "<br>UK file created successfully.", "Info")
    '            ElseIf strHQRet = "" And strUKRet <> "" Then
    '                Throw New EvoFriendlyException("HQ file created successfully<br>" & strUKRet, "Info")
    '            Else
    '                Throw New EvoFriendlyException(strHQRet & "<br>" & strUKRet, "Info")
    '            End If

    '        Catch ex As Exception
    '            handleexception(ex, "IAFeederFile", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    Private Sub btnToCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToCsv.Click
        Using New clslogger(log, className, "btnToCsv_Click")
            Try
                If CStr(Session.Item("clientname")).ToLower = "npsa" Then
                    sendToFile(True, "")

                    'R2.17 CR
                ElseIf CStr(Session.Item("clientname")).ToLower = "ncas" Then

                    'get attach the new files
                    Dim strFileRootPath As String = getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy") & "\"
                    Dim strFeederFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & ".csv"
                    Dim strCreditFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " Credits.csv"
                    Dim strCoverSheetName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " CoverSheet.pdf"

                    'create an email message
                    Dim ofile As New System.IO.StreamReader(getConfig("XMLFilePath") & "\NCAS\Template\NcasInvoice.htm")
                    Dim strMessage As String = ofile.ReadToEnd
                    ofile.Close()

                    'get the to address
                    Dim strTo As String = getConfig("NCASEmail")
                    If getConfig("EmailTest").ToLower = "true" Then
                        strTo = "craig.rickell@nysgroup.com"
                    End If

                    'send the email to NCAS/NYS Contact
                    SendEmailMessage("accounts@nysgroup.com", _
                                     strTo, _
                                     "NCAS Feeder Files from NYS Corporate", _
                                     strMessage, _
                                     strFileRootPath & strCoverSheetName, _
                                     "", _
                                     "", _
                                     getConfig("NCASBcc"), _
                                     strFileRootPath & strFeederFileName, _
                                     strFileRootPath & strCreditFileName, _
                                     "")

                    btnToCsv.Visible = False

                    Throw New EvoFriendlyException("Email has been sent successfully", "Info")

                    'R2.21.3 SA - BUG FIX!!
                ElseIf CStr(Session.Item("clientname")).ToUpper = "NHS LITIGATION AUTHORITY" Then
                    'get attach the new files
                    Dim strFileRootPath As String = getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy") & "\"
                    Dim strFeederFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & ".csv"
                    Dim strCreditFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " Credits.csv"
                    Dim strCoverSheetName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " CoverSheet.pdf"

                    'create an email message
                    Dim ofile As New System.IO.StreamReader(getConfig("XMLFilePath") & "\NCAS\Template\NcasInvoice.htm")
                    Dim strMessage As String = ofile.ReadToEnd
                    ofile.Close()

                    'get the to address
                    Dim strTo As String = getConfig("NCASEmail")
                    If getConfig("EmailTest").ToLower = "true" Then
                        strTo = "craig.rickell@nysgroup.com"
                    End If

                    'send the email to NHS litigation Authority/NYS Contact
                    SendEmailMessage("accounts@nysgroup.com", _
                                     strTo, _
                                     "NHS Litigation Authority Feeder Files from NYS Corporate", _
                                     strMessage, _
                                     strFileRootPath & strCoverSheetName, _
                                     "", _
                                     "", _
                                     getConfig("NCASBcc"), _
                                     strFileRootPath & strFeederFileName, _
                                     strFileRootPath & strCreditFileName, _
                                     "")

                    btnToCsv.Visible = False

                    Throw New EvoFriendlyException("Email has been sent successfully", "Info")

                ElseIf CStr(Session.Item("clientname")).ToLower = "bs - anchor trust" Then
                    sendToFile(False, "")
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "HIGHWAYS AGENCY" Then
                    makeFolderExist(getConfig("XMLFilePath") & "\HA")
                    makeFolderExist(getConfig("XMLFilePath") & "\HA\" & Format(Now, "dd-MM-yyyy"))

                    'Highways don't get conference or hotels through on the feeder file
                    'instead those products are sent via f8 on BOSS

                    'do rail
                    Dim strFileName As String = "RAIL-" & Format(Date.Now, "dd-MM-yyyy") & ".csv"
                    Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\HA\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
                    ofiler.Write(FeederFileCreation.createFileHighways(txtfrom.Text, txtto.Text, "RR"))
                    ofiler.Flush()
                    ofiler.Close()

                    'do air
                    Dim strFileName2 As String = "AIR-" & Format(Date.Now, "dd-MM-yyyy") & ".csv"
                    Dim ofiler2 As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\HA\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName2, False, Encoding.Default)
                    ofiler2.Write(FeederFileCreation.createFileHighways(txtfrom.Text, txtto.Text, "A"))
                    ofiler2.Flush()
                    ofiler2.Close()

                    'R2.11 CR
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "ROYAL COLLEGE OF NURSING" Then
                    'sendToFile(False, "", "NYS100")

                    'R2.21 SA 
                    Dim strFileRootPath As String = getConfig("XMLFilePath") & "\RCN100\" & Format(Date.Now, "dd-MM-yyyy") & "\"
                    Dim strFeederFileName As String = "NYS100.csv"
                    Dim strCreditFileName As String = "NYS100-Credits.csv"
                    'R?? SA Bug Fix - add cover sheet 
                    Dim strCoverSheetName As String = "NYS100-" & Format(Date.Now, "dd-MM-yyyy") & " CoverSheet.pdf"

                    'create an email message
                    Dim ofile As New System.IO.StreamReader(getConfig("XMLFilePath") & "\RCN100\Template\RCNInvoice.htm")
                    Dim strMessage As String = ofile.ReadToEnd
                    ofile.Close()

                    ''get the to address
                    Dim strTo As String = getConfig("RCNEmail")
                    If getConfig("EmailTest").ToLower = "true" Then
                        strTo = "sarab.azzouz@nysgroup.com"
                    End If

                    'R?? SA - Bug fix, cover sheet was not added to the email
                    'send the email
                    SendEmailMessage("accounts@nysgroup.com", _
                                     strTo, _
                                     "RCN Feeder Files from NYS Corporate", _
                                     strMessage, _
                                     strFileRootPath & strCoverSheetName, _
                                     getConfig("RCNCc"), _
                                     "", _
                                     getConfig("RCNBcc"), _
                                     strFileRootPath & strFeederFileName, _
                                     strFileRootPath & strCreditFileName, _
                                     "")
                    btnToCsv.Visible = False

                    Throw New EvoFriendlyException("Email has been sent successfully", "Info")


                    'R2.21 SA
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "ROYAL COLLEGE OF NURSING G" Then
                    Dim strFileRootPath As String = getConfig("XMLFilePath") & "\RCNG100\" & Format(Now, "dd-MM-yyyy") & "\"
                    Dim strFeederFileName As String = "NYSG100.csv"
                    Dim strCreditFileName As String = "NYSG100-Credits.csv"
                    Dim strCoverSheetName As String = "NYSG100-" & Format(Date.Now, "dd-MM-yyyy") & "CoverSheet.pdf"

                    'create an email message
                    Dim ofile As New System.IO.StreamReader(getConfig("XMLFilePath") & "\RCNG100\Template\RCNGInvoice.htm")
                    Dim strMessage As String = ofile.ReadToEnd
                    ofile.Close()

                    'get the to address
                    Dim strTo As String = getConfig("RCNGEmail")
                    If getConfig("EmailTest").ToLower = "true" Then
                        strTo = "sarab.azzouz@nysgroup.com"
                    End If
                    SendEmailMessage("accounts@nysgroup.com", _
                                     strTo, _
                                     "RCN Conference/events Feeder Files from NYS Corporate", _
                                     strMessage, _
                                     strFileRootPath & strCoverSheetName, _
                                     getConfig("RCNGCc"), _
                                     "", _
                                     getConfig("RCNGBcc"), _
                                     strFileRootPath & strFeederFileName, _
                                     strFileRootPath & strCreditFileName, _
                                     "")
                    btnToCsv.Visible = False

                    Throw New EvoFriendlyException("Email has been sent successfully", "Info")

                    'R2.12 CR
                ElseIf CStr(Session.Item("FeederFileClient")).ToUpper = "INSTITUTION OF CIVIL ENGINEERS" Then
                    'sendToFile(False, "", "ICE FeederFile " & Date.Now.ToString("dd-MM-yyyy"))
                    'get attach the new files
                    Dim strFileRootPath As String = getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\"
                    Dim strFeederFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & ".csv"
                    Dim strCreditFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & "_Fees.csv"
                    Dim strCoverSheetName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " CoverSheet.pdf"

                    'create an email message
                    Dim ofile As New System.IO.StreamReader(getConfig("XMLFilePath") & "\ICE\Template\IceInvoice.htm")
                    Dim strMessage As String = ofile.ReadToEnd
                    ofile.Close()

                    'get the to address
                    Dim strTo As String = getConfig("ICEEmail")
                    If getConfig("EmailTest").ToLower = "true" Then
                        strTo = "craig.rickell@nysgroup.com"
                    End If

                    'send the email to ICE/NYS Contact
                    SendEmailMessage("accounts@nysgroup.com", _
                                     strTo, _
                                     "ICE Feeder Files from NYS Corporate", _
                                     strMessage, _
                                     strFileRootPath & strCoverSheetName, _
                                     getConfig("ICECc"), _
                                     "", _
                                     getConfig("ICEBcc"), _
                                     strFileRootPath & strFeederFileName, _
                                     strFileRootPath & strCreditFileName, _
                                     "")

                    btnToCsv.Visible = False

                    Throw New EvoFriendlyException("Email has been sent successfully", "Info")

                End If
            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
        End Using
    End Sub

    'R2.11 CR - added optional filename
    Private Sub sendToFile(ByVal pbText As Boolean, ByVal strReport As String, Optional ByVal strFileName As String = "Export")
        Using New clslogger(log, className, "sendToFile")
            Response.Charset = ""
            Response.ContentEncoding = System.Text.Encoding.Default

            If strReport = "" Then
                strReport = txtresult.Text
            End If

            If pbText Then
                Response.AddHeader("Content-Type", "text/plain")
                Response.AddHeader("content-disposition", "attachment; filename=" & strFileName & ".txt")
                Response.Write(strReport)
            Else
                Response.AddHeader("Content-Type", "application/csv")
                Response.AddHeader("content-disposition", "attachment; filename=" & strFileName & ".csv")
                Response.Write(strReport.Replace(",", " ").Replace("|", ","))
            End If

            Response.End()
        End Using
    End Sub

    Private Sub btnO2Emails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnO2Emails.Click
        Using New clslogger(log, className, "btnO2Emails_Click")
            Try
                Dim strRet As String = ""
                'Dim strRet As String = sendPOEmails()

                Dim strRet2 As String = sendInvoiceEmail()

                If strRet <> "" Then
                    If strRet2 <> "" Then
                        Throw New EvoFriendlyException("The following PO emails failed to send:" & strRet & vbCrLf & strRet2, "Info")
                    Else
                        Throw New EvoFriendlyException("The following emails failed to send:" & strRet & vbCrLf & "The invoices file/s sent successfully", "Info")
                    End If
                Else
                    If strRet2 <> "" Then
                        Throw New EvoFriendlyException("PO emails failed to send." & vbCrLf & strRet2, "Info")
                    Else
                        Throw New EvoFriendlyException("PO emails sent successfully." & vbCrLf & "The invoices file/s sent successfully", "Info")
                    End If
                End If
            Catch ex As Exception
                handleexception(ex, "IAFeederFile", Me.Page)
            End Try
        End Using
    End Sub

    Private Function sendInvoiceEmail() As String
        Using New clslogger(log, className, "sendInvoiceEmail")
            Dim ofile As New System.IO.StreamReader(Server.MapPath("docs\o2Invoice.htm"))
            Dim strread As String = ofile.ReadToEnd
            Dim emailTo As String = getConfig("o2InvoiceEmail")
            Dim emailcc1 As String = getConfig("o2InvoiceEmailcc1")
            Dim emailcc2 As String = getConfig("o2InvoiceEmailcc2")
            Dim emailbcc As String = getConfig("o2InvoiceEmailbcc")
            Dim o2File1 As String = ""
            Dim o2File2 As String = ""

            ofile.Close()

            If getConfig("EmailTest") = "True" Then
                emailTo = "nick.massarella@nysgroup.com"
                emailcc1 = "alex.cockram@nysgroup.com"
                emailcc2 = "michael.weeks@nysgroup.com"
                emailbcc = "craig.rickell@nysgroup.com"
            End If

            Dim strPath As String = getConfig("XMLFilePath") & "\O2\" & Format(Now, "dd-MM-yyyy") & "\"

            'will only be 2 files in this folder
            If IO.Directory.Exists(strPath) Then
                For Each myFile As String In IO.Directory.GetFiles(strPath)
                    Dim strFile As String = IO.Path.GetFileName(myFile)
                    If IO.File.Exists(strPath & strFile) Then
                        If o2File1 = "" Then
                            o2File1 = strPath & strFile
                        Else
                            o2File2 = strPath & strFile
                        End If
                    End If
                Next
            End If

            'make sure there are some files, if no file at all then tell the user and skip the email send
            If o2File1 = "" AndAlso o2File2 = "" Then
                Return "Nothing to send to Steria"
            End If

            Try
                SendEmailMessage("accounts@nysgroup.com", emailTo, "Invoice files", strread, "", emailcc1, emailcc2, emailbcc, o2File1, o2File2, "")
                Return ""
            Catch ex As Exception
                Return "Failed invoice email to:" & emailTo
            End Try
        End Using
    End Function

    Private Function sendPOEmails() As String
        Using New clslogger(log, className, "sendPOEmails")

            Dim originalList As List(Of O2) = DirectCast(ViewState.Item("FeederFileO2Email"), List(Of O2))

            Dim currentPO As String = ""
            Dim lastPO As String = ""
            Dim currentEmail As String = ""
            Dim lastEmail As String = ""
            Dim strDetails As String = ""
            Dim runningNet As Decimal = 0
            Dim runningGross As Decimal = 0
            Dim strPOX As String = ""
            Dim strFails As String = ""

            For Each oitem As O2 In originalList

                currentPO = oitem.Po
                currentEmail = oitem.RequesterEmail

                If lastEmail = "" Then
                    lastEmail = currentEmail
                End If
                If lastPO = "" Then
                    lastPO = currentPO
                End If

                If currentEmail.ToUpper <> lastEmail.ToUpper Then 'do send
                    Dim ofile As New System.IO.StreamReader(Server.MapPath("docs\goodsreceipt-po.htm"))
                    Dim strread As String = ofile.ReadToEnd
                    ofile.Close()

                    strPOX = strDetails & "<br /><b>PO " & lastPO & ": Total Net=&pound;" & runningNet & " Total Gross=&pound;" & runningGross & "</b>"
                    runningNet = 0
                    runningGross = 0

                    Dim firstname As String = Mid(lastEmail, 1, lastEmail.IndexOf("."))
                    firstname = StrConv(LCase$(firstname), vbProperCase)
                    strread = strread.Replace("#firstname#", firstname)

                    strread = strread.Replace("#details#", strPOX)
                    Dim emailTo As String = lastEmail

                    If getConfig("EmailTest") = "True" Then
                        emailTo = "nick.massarella@nysgroup.com"
                    End If
                    Try
                        SendEmailMessage("accounts@nysgroup.com", emailTo, "Purchase Order action required", strread, "", "", "", "", "", "", "")

                    Catch ex As Exception
                        strFails = strFails & "Failed email to:" & emailTo & ", "
                    End Try

                    strDetails = ""
                    strPOX = ""
                End If

                If lastPO.ToUpper = currentPO.ToUpper Or strDetails = "" Then

                    Dim venue As String = StrConv(LCase$(oitem.Venue.Trim), vbProperCase)

                    strDetails = strDetails & oitem.Invoicenumber & ": " & CStr(IIf(oitem.Ref.StartsWith("c"), "Accommodation @ ", "Meeting/Event @ ")) & venue & _
                                " on " & oitem.Start & "." & "Net=&pound;" & oitem.TotalNett & " Gross=&pound;" & oitem.tot_billed & "<br />"

                    runningNet = runningNet + oitem.TotalNett
                    runningGross = runningGross + oitem.tot_billed
                Else
                    Dim venue As String = StrConv(LCase$(oitem.Venue.Trim), vbProperCase)

                    strDetails = strDetails & "<br /><b>PO " & lastPO & ": Total Net=&pound;" & runningNet & " Total Gross=&pound;" & runningGross & "</b><br /><br />" & _
                                oitem.Invoicenumber & ": " & CStr(IIf(oitem.Ref.StartsWith("c"), "Accommodation @ ", "Meeting/Event @ ")) & venue & _
                                " on " & oitem.Start & "." & "Net=&pound;" & oitem.TotalNett & " Gross=&pound;" & oitem.tot_billed & "<br />"
                    'strPO = strPO & strDetails & "<br /><b>PO " & lastPO & ": Total Net=&pound;" & runningNet & " Total Gross=&pound;" & runningGross & "</b>"
                    runningNet = oitem.TotalNett
                    runningGross = oitem.tot_billed
                End If

                lastPO = currentPO
                lastEmail = currentEmail
            Next

            If originalList.Count > 0 Then
                'do last one
                Dim ofileX As New System.IO.StreamReader(Server.MapPath("docs\goodsreceipt-po.htm"))
                Dim strreadX As String = ofileX.ReadToEnd
                ofileX.Close()

                strPOX = strDetails & "<br /><b>PO " & lastPO & ": Total Net=&pound;" & runningNet & " Total Gross=&pound;" & runningGross & "</b>"
                Dim firstnameX As String = Mid(lastEmail, 1, lastEmail.IndexOf("."))
                firstnameX = StrConv(LCase$(firstnameX), vbProperCase)
                strreadX = strreadX.Replace("#firstname#", firstnameX)

                strreadX = strreadX.Replace("#details#", strPOX)
                Dim emailToX As String = lastEmail

                If getConfig("EmailTest") = "True" Then
                    emailToX = "nick.massarella@nysgroup.com"
                End If
                Try
                    SendEmailMessage("accounts@nysgroup.com", emailToX, "Purchase Order action required", strreadX, "", "", "", "", "", "", "")
                Catch ex As Exception
                    strFails = strFails & "Failed email to:" & emailToX & ", "
                End Try
            End If
            Return strFails
        End Using

    End Function

    Protected Sub btnO2AdquiraInstructions_click(sender As Object, e As EventArgs) Handles btnO2AdquiraInstructions.Click
        Using New clslogger(log, className, "btnO2AdquiraInstructions_click")
            Try
                If pnO2Adquira.Visible = False Then
                    pnO2Adquira.Style("LEFT") = "10px"
                    pnO2Adquira.Style("TOP") = "25px"
                    pnO2Adquira.Visible = True
                Else
                    pnO2Adquira.Style("LEFT") = "1200px"
                    pnO2Adquira.Style("TOP") = "25px"
                    pnO2Adquira.Visible = True

                End If
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Protected Sub txtresult_TextChanged(sender As Object, e As EventArgs)

    End Sub
End Class

Partial Public Class dataImport

    Public Sub New( _
        ByVal pintID As Integer, _
        ByVal pstrtot_invno As String)
        mintID = pintID
        mstrtot_invno = pstrtot_invno
    End Sub

    Private mintID As Integer
    Private mstrtot_invno As String

    Public Property ID() As Integer
        Get
            Return mintID
        End Get
        Set(ByVal value As Integer)
            mintID = value
        End Set
    End Property

    Public Property tot_invno() As String
        Get
            Return mstrtot_invno
        End Get
        Set(ByVal value As String)
            mstrtot_invno = value
        End Set
    End Property

End Class