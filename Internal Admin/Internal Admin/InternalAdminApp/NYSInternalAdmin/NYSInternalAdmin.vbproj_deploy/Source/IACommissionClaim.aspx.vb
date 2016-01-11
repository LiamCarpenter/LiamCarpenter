Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports BossData

Partial Public Class IACommissionClaim
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

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                    btnEmail.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                    btnPrinter.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                    btnPrintInvoice.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                
                If txtinvoiceno.Text.Trim = "" Then
                    Throw New EvoFriendlyException("Please enter an Invoice number.", "Check Details")
                End If

                If Not txtinvoiceno.Text.Trim.Contains("#") Then
                    If Not Regex.IsMatch(txtinvoiceno.Text, getConfig("InvoiceFormat")) Then
                        Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                    End If
                End If

                If ddtype.SelectedIndex < 1 Then
                    Throw New EvoFriendlyException("Please select a type.", "Check Details")
                End If

                Dim strDirectoryToSaveTo As String = ""

                For intDirCount As Integer = 1 To 10000
                    If Not IO.Directory.Exists(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)) Then
                        IO.Directory.CreateDirectory(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount))
                        strDirectoryToSaveTo = getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)
                        Exit For
                    End If
                Next

                Dim strError As String = ""

                If txtinvoiceno.Text.Trim.Contains("#") Then
                    Dim list As ICollection(Of String) = Split(txtinvoiceno.Text.Trim, "#")
                    For Each strInv As String In list
                        strInv = strInv.Trim
                        If strInv <> "" Then
                            If Regex.IsMatch(strInv, getConfig("InvoiceFormat")) Then
                                'R?? 
                                Dim strResult As String = clsBoss.runMainSelect(False, txtinvoiceno.Text.Trim, "", "", "")
                                If clsBoss.runMainSelect(False, strInv, "", "", "") = "Successful run" Then
                                    If ddtype.SelectedIndex = 1 Then
                                        buildAndSendOne(False, False, False, False, True, False, False, False, False, True, False, strInv, "", strDirectoryToSaveTo)
                                    ElseIf ddtype.SelectedIndex = 2 Then
                                        buildAndSendOne(False, False, False, False, False, True, False, False, False, True, False, strInv, "", strDirectoryToSaveTo)
                                    ElseIf ddtype.SelectedIndex = 3 Then
                                        buildAndSendOne(False, False, False, False, False, False, True, False, False, True, False, strInv, "", strDirectoryToSaveTo)
                                    ElseIf ddtype.SelectedIndex = 4 Then
                                        buildAndSendOne(False, False, False, False, False, False, False, True, False, True, False, strInv, "", strDirectoryToSaveTo)
                                    ElseIf ddtype.SelectedIndex = 5 Then
                                        buildAndSendOne(False, False, False, False, False, False, False, False, True, True, False, strInv, "", strDirectoryToSaveTo)
                                    End If
                                Else
                                    ' Throw New EvoFriendlyException("SQL did not update correctly from BOSS, see Nick.", "Check Details")
                                    Throw New EvoFriendlyException("SQL did not update correctly from BOSS - " & strResult, "Check Details")
                                End If
                            Else
                                strError = strError & strInv & ";"
                            End If
                        End If
                    Next
                Else
                    If txtinvoiceno.Text.Trim <> "" Then
                        'R?? SA 
                        Dim strResult As String = clsBoss.runMainSelect(False, txtinvoiceno.Text.Trim, "", "", "")
                        If clsBoss.runMainSelect(False, txtinvoiceno.Text.Trim, "", "", "") = "Successful run" Then
                            If ddtype.SelectedIndex = 1 Then
                                buildAndSendOne(False, False, False, False, True, False, False, False, False, True, False, txtinvoiceno.Text.Trim, "", strDirectoryToSaveTo)
                            ElseIf ddtype.SelectedIndex = 2 Then
                                buildAndSendOne(False, False, False, False, False, True, False, False, False, True, False, txtinvoiceno.Text.Trim, "", strDirectoryToSaveTo)
                            ElseIf ddtype.SelectedIndex = 3 Then
                                buildAndSendOne(False, False, False, False, False, False, True, False, False, True, False, txtinvoiceno.Text.Trim, "", strDirectoryToSaveTo)
                            ElseIf ddtype.SelectedIndex = 4 Then
                                buildAndSendOne(False, False, False, False, False, False, False, True, False, True, False, txtinvoiceno.Text.Trim, "", strDirectoryToSaveTo)
                            ElseIf ddtype.SelectedIndex = 5 Then
                                'buildAndSendOne(True, False, False, False, False, False, False, False, True, False, False, txtinvoiceno.Text.Trim, "", strDirectoryToSaveTo)
                                buildAndSendOne(False, False, False, True, False, False, False, False, True, False, False, txtinvoiceno.Text.Trim, "", strDirectoryToSaveTo)
                            End If
                        Else
                            'Throw New EvoFriendlyException("SQL did not update correctly from BOSS, see Nick.", "Check Details")
                            Throw New EvoFriendlyException("SQL did not update correctly from BOSS - " & strResult, "Check Details")
                        End If
                    End If
                End If
                If strError <> "" Then
                    Throw New EvoFriendlyException("The following invoices were not in the correct format:" & strError, "Check Details")
                End If
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub buildAndSendOne(ByVal pbSpecial As Boolean, _
                           ByVal pbSpecial2 As Boolean, _
                           ByVal pbSpecialFinal As Boolean, _
                           ByVal pbSpeciallaw As Boolean, _
                           ByVal pbClaimInitial As Boolean, _
                           ByVal pbClaim2 As Boolean, _
                           ByVal pbClaim3 As Boolean, _
                           ByVal pbClaim4 As Boolean, _
                           ByVal pbClaim5 As Boolean, _
                           ByVal pbIndividual As Boolean, _
                           ByVal pbJustPrint As Boolean, _
                           ByVal pstrInvoiceNo As String, _
                           ByVal pstrEmail As String, _
                           ByVal pstrSaveDirctory As String)
        Using New clslogger(log, className, "buildAndSendOne")
            Dim oClaims As List(Of BOSSCommissionClaim)
            If pbSpecial Then
                oClaims = BOSSCommissionClaim.commissionClaimSpecial()
            ElseIf pbIndividual Then
                oClaims = BOSSCommissionClaim.commissionClaimIndiv(pstrInvoiceNo)
            Else
                oClaims = BOSSCommissionClaim.commissionClaim(pbSpecial2, pbSpecialFinal, pbSpeciallaw, pbClaimInitial, pbClaim2, pbClaim3, pbClaim4, pbClaim5)
            End If

            Dim strDetails As String = ""
            Dim ofiler As System.IO.StreamWriter
            Dim straddress As String = ""
            Dim strFailedToPrint As String = ""

            If oClaims.Count > 0 Then
                For Each oClaim As BOSSCommissionClaim In oClaims

                    Dim strLeadNameSplit As String = oClaim.leadname
                    Dim stremailtouseX As String = ""

                    If oClaim.leadname.Contains("&") Then
                        Dim list As ICollection(Of String) = Split(oClaim.leadname, "&")

                        'R2.17 CR - BUG FIX, reset the leadName so that it doesn't repeat the names on the file.
                        strLeadNameSplit = ""

                        For Each str As String In list
                            If str.Contains("/") Then
                                str = str.Trim
                                Dim strFirst As String = Mid(str, str.IndexOf("/") + 2)
                                Dim strLast As String = Mid(str, 1, str.IndexOf("/"))
                                If strLeadNameSplit = "" Then
                                    strLeadNameSplit = strFirst & " " & strLast
                                Else
                                    strLeadNameSplit = strLeadNameSplit & " & " & strFirst & " " & strLast
                                End If
                            Else
                                If strLeadNameSplit = "" Then
                                    strLeadNameSplit = str
                                Else
                                    strLeadNameSplit = strLeadNameSplit & " & " & str
                                End If
                            End If
                        Next
                    Else
                        If oClaim.leadname.Contains("/") Then
                            Dim strFirst As String = Mid(oClaim.leadname, oClaim.leadname.IndexOf("/") + 2)
                            Dim strLast As String = Mid(oClaim.leadname, 1, oClaim.leadname.IndexOf("/"))
                            strLeadNameSplit = strFirst & " " & strLast
                        End If
                    End If

                    strDetails = readText(getConfig("MIDocsPath") & "Payments\head.rtf")
                    strDetails = strDetails & readText(getConfig("MIDocsPath") & "Payments\line.rtf")

                    strDetails = strDetails.Replace("#invref#", oClaim.invoiceid)
                    strDetails = strDetails.Replace("#invdate#", oClaim.invoicedate)
                    strDetails = strDetails.Replace("#evdate#", oClaim.eventdate)
                    strDetails = strDetails.Replace("#ref#", oClaim.ref)
                    strDetails = strDetails.Replace("#type#", oClaim.product)
                    strDetails = strDetails.Replace("#lead#", strLeadNameSplit)

                    'R2.16 CR - use the returned currency if it is not blank
                    Dim strCurrency As String = "GBP"
                    If oClaim.Currency <> "" Then
                        strCurrency = oClaim.Currency
                    End If

                    strDetails = strDetails.Replace("#amt#", " GBP " & CStr(oClaim.invoiceamount))
                    strDetails = strDetails.Replace("#cnet#", " " & strCurrency & " " & CStr(oClaim.commnet))
                    strDetails = strDetails.Replace("#cvat#", " " & strCurrency & " " & CStr(oClaim.commvat))
                    strDetails = strDetails.Replace("#ctot#", " " & strCurrency & " " & CStr(oClaim.commtotal))
                    'strDetails = strDetails.Replace("#cnet#", " GBP " & CStr(oClaim.commnet))
                    'strDetails = strDetails.Replace("#cvat#", " GBP " & CStr(oClaim.commvat))
                    'strDetails = strDetails.Replace("#ctot#", " GBP " & CStr(oClaim.commtotal))

                    If oClaim.accountsemail <> "" Then
                        If Regex.IsMatch(oClaim.accountsemail, emailRegex) Then
                            stremailtouseX = oClaim.accountsemail
                        End If
                    End If
                    If stremailtouseX = "" And oClaim.venueemail <> "" Then
                        If Regex.IsMatch(oClaim.venueemail, emailRegex) Then
                            stremailtouseX = oClaim.venueemail
                        End If
                    End If
                    If stremailtouseX = "" And oClaim.latestcontactemail <> "" Then
                        If Regex.IsMatch(oClaim.latestcontactemail, emailRegex) Then
                            stremailtouseX = oClaim.latestcontactemail
                        End If
                    End If

                    'R2.17 CR
                    If stremailtouseX = "" Then
                        'email still blank - could be a supplier NOT in VDB, so check systemadminparameters
                        Dim strSupplierCode As String = BOSSsupplier.getSupplierBoss(oClaim.invoiceid)

                        If strSupplierCode.Trim <> "" Then
                            Dim oSysParams As New List(Of NysDat.clsSystemparameter)
                            oSysParams = NysDat.clsSystemparameter.getSupplierByBossCode(strSupplierCode)
                            For Each oSysParam In oSysParams
                                If oSysParam.Systemparameteremail.Trim <> "" Then
                                    'we have something!!! might not be a valid email though...
                                    'check for validity
                                    If Regex.IsMatch(oSysParam.Systemparameteremail.Trim, clsNYS.emailRegex) Then
                                        stremailtouseX = oSysParam.Systemparameteremail.Trim

                                        'No need to keep looping now - we have a valid email
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If

                    If pstrEmail <> "" Then
                        stremailtouseX = pstrEmail
                    End If

                    'save each invoice details
                    Dim intId As Integer = 0
                    If getConfig("EmailTest") <> "True" Then

                        Dim strClaimInitial As String = ""

                        If pbClaimInitial Then
                            strClaimInitial = Format(Date.Now, "dd/MM/yyyy")
                        End If

                        Dim oSaver As New CommissionChase(0, oClaim.invoiceid, Format(Date.Now, "dd/MM/yyyy"), "", "", "", "", "", oClaim.venuename, CStr(oClaim.invoiceamount), _
                                                            CStr(oClaim.commnet), CStr(oClaim.commvat), CStr(oClaim.commtotal), oClaim.invoicedate, oClaim.eventdate, stremailtouseX, "")
                        intId = oSaver.save()

                        If pbClaim2 Then
                            CommissionChase.saveDate(4, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
                        ElseIf pbClaim3 Then
                            CommissionChase.saveDate(5, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
                        ElseIf pbClaim4 Then
                            CommissionChase.saveDate(6, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
                        ElseIf pbClaim5 Then
                            CommissionChase.saveDate(7, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
                        ElseIf pbClaimInitial Then
                            CommissionChase.saveDate(8, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
                        End If
                    End If

                    Dim strFile As String = ""
                    If pbSpecial Then
                        strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSpecial.rtf")
                    Else
                        If pbSpecial2 Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSecond.rtf")
                        ElseIf pbSpecialFinal Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFinal.rtf")
                        ElseIf pbSpeciallaw Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionLaw.rtf")
                        ElseIf pbClaimInitial Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionInitial.rtf")
                        ElseIf pbClaim2 Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFirst.rtf")
                        ElseIf pbClaim3 Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSecond.rtf")
                        ElseIf pbClaim4 Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFinal.rtf")
                        ElseIf pbClaim5 Then
                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionLaw.rtf")
                        End If
                    End If

                    Dim sVenue As String = oClaim.venuename
                    sVenue = StrConv(LCase$(sVenue), vbProperCase)

                    'straddress = CStr(IIf(oClaim.address1 <> "", StrConv(LCase$(oClaim.address1), vbProperCase) & ", \line ", ""))
                    'straddress = straddress & CStr(IIf(oClaim.address2 <> "", StrConv(LCase$(oClaim.address2), vbProperCase) & ", \line ", ""))
                    'straddress = straddress & CStr(IIf(oClaim.address3 <> "", StrConv(LCase$(oClaim.address3), vbProperCase) & ", \line ", ""))
                    'straddress = straddress & CStr(IIf(oClaim.address4 <> "", StrConv(LCase$(oClaim.address4), vbProperCase) & ", \line ", ""))

                    'R2.6 SA 
                    If oClaim.venuename.ToLower = "Royal Society Of Arts".ToLower Then
                        straddress = straddress & CStr("Harbour and Jones C/O RSA House" & ", \line ")
                        straddress = straddress & CStr(IIf(oClaim.address1 <> "", StrConv(LCase$(oClaim.address2), vbProperCase) & ", \line ", ""))
                        straddress = straddress & CStr("Royal Society of Arts" & ", \line ")
                        straddress = straddress & CStr(IIf(oClaim.address2 <> "", StrConv(LCase$(oClaim.address2), vbProperCase) & ", \line ", ""))
                        straddress = straddress & CStr(IIf(oClaim.address3 <> "", StrConv(LCase$(oClaim.address3), vbProperCase) & ", \line ", ""))
                        straddress = straddress & CStr(IIf(oClaim.address4 <> "", StrConv(LCase$(oClaim.address4), vbProperCase) & ", \line ", ""))
                        'R2.6 SA   
                    ElseIf oClaim.venuename.ToLower = "Peterborough United".ToLower Then
                        straddress = straddress & CStr("C/O Peterborough United" & ", \line ")
                        straddress = straddress & CStr("Venue Catering Partner" & ", \line ")
                        straddress = straddress & CStr("Cheshire House" & ", \line ")
                        straddress = straddress & CStr("Murhall Street" & ", \line ")
                        straddress = straddress & CStr("Burslam" & ", \line ")
                        straddress = straddress & CStr("Stoke-on-Trent" & ", \line ")
                        straddress = straddress & CStr("ST6 4BL" & ", \line ")
                        straddress = straddress & CStr("Tel: 01782 816394" & ", \line ")
                    Else
                        straddress = CStr(IIf(oClaim.address1 <> "", StrConv(LCase$(oClaim.address1), vbProperCase) & ", \line ", ""))
                        straddress = straddress & CStr(IIf(oClaim.address2 <> "", StrConv(LCase$(oClaim.address2), vbProperCase) & ", \line ", ""))
                        straddress = straddress & CStr(IIf(oClaim.address3 <> "", StrConv(LCase$(oClaim.address3), vbProperCase) & ", \line ", ""))
                        straddress = straddress & CStr(IIf(oClaim.address4 <> "", StrConv(LCase$(oClaim.address4), vbProperCase) & ", \line ", ""))
                    End If

                    'R2.6 SA - add post code for all venues but not Peterborough United
                    If straddress <> "" AndAlso oClaim.venuename.ToLower <> "Peterborough United" Then
                        straddress = Mid(straddress, 1, Len(straddress) - 6)
                        straddress = straddress & CStr(IIf(oClaim.postcode <> "", oClaim.postcode & ". \line ", ""))
                    End If

                    'R2.6 SA - add details for all venues but not Peterborough United
                    If oClaim.venuename.ToLower <> "Peterborough United" Then
                        straddress = straddress & CStr(IIf(oClaim.telephone <> "", "Tel: " & oClaim.telephone & " \line ", ""))
                        straddress = straddress & CStr(IIf(oClaim.fax <> "", "Fax: " & oClaim.fax & " \line ", ""))
                        straddress = straddress & CStr(IIf(stremailtouseX <> "", "Email: " & stremailtouseX & " \line ", ""))
                    End If
                   
                    If straddress <> "" Then
                        straddress = Mid(straddress, 1, Len(straddress) - 7)
                    End If

                    strFile = strFile.Replace("#venuename#", sVenue & ".")
                    strFile = strFile.Replace("#address#", straddress)
                    strFile = strFile.Replace("#details#", strDetails)
                    strFile = strFile.Replace("#invoiceno#", oClaim.invoiceid)

                    'R2.16 CR
                    'strFile = strFile.Replace("#ctnet#", " GBP " & CStr(oClaim.commnet))
                    'strFile = strFile.Replace("#ctvat#", " GBP " & CStr(oClaim.commvat))
                    'strFile = strFile.Replace("#cttot#", " GBP " & CStr(oClaim.commtotal))
                    strFile = strFile.Replace("#ctnet#", " " & strCurrency & " " & CStr(oClaim.commnet))
                    strFile = strFile.Replace("#ctvat#", " " & strCurrency & " " & CStr(oClaim.commvat))
                    strFile = strFile.Replace("#cttot#", " " & strCurrency & " " & CStr(oClaim.commtotal))
                    strFile = strFile.Replace("#date#", Format(Date.Now, "dd MMMM yyyy"))

                    ofiler = New System.IO.StreamWriter(pstrSaveDirctory & "\CommInvoice-" & oClaim.invoiceid & ".rtf", False)
                    ofiler.Write(strFile)
                    ofiler.Flush()
                    ofiler.Close()

                    Dim strRtfFile As String = pstrSaveDirctory & "\CommInvoice-" & oClaim.invoiceid & ".rtf"
                    Dim strPdfFile As String = pstrSaveDirctory & "\CommInvoice-" & oClaim.invoiceid & ".pdf"
                    Dim strFileToUse As String = "" '

                    If ConvertToPDF(strRtfFile, strPdfFile) Then
                        strFileToUse = strPdfFile
                    Else
                        strFileToUse = strRtfFile
                    End If

                    Dim blnEmailSent As Boolean = False
                    'if email exists and is in correct format then email, otherwise send to printer
                    If Not pbJustPrint Then
                        If Regex.IsMatch(stremailtouseX, emailRegex) Then
                            Dim strEmailTo As String = ""
                            Dim strMessage As String = readText(getConfig("MIDocsPath") & "Payments\Body.htm")
                            If getConfig("EmailTest") = "True" Then
                                strEmailTo = "sarab.azzouz@nysgroup.com"
                                strMessage = "Email would have been sent to: " & stremailtouseX & vbCrLf & strMessage
                            Else
                                If stremailtouseX = "info@java-hotel.com" Then
                                    strEmailTo = "fm.wetherby@ramadajarvis.co.uk"
                                ElseIf stremailtouseX = "reservationsglasgow@adobehotels.co.uk" Then
                                    strEmailTo = "assistantfinancialcontrollerglasgow@abodehotels.co.uk"
                                Else
                                    strEmailTo = stremailtouseX
                                End If
                            End If
                            Try
                                If SendEmailMessage("commissions@nysgroup.com", strEmailTo, "Commission invoice", strMessage, _
                                                 strFileToUse, "", "", "", "", "", "") Then

                                    If getConfig("EmailTest") <> "True" Then
                                        CommissionChase.updateSentBy(intId, "Emailed")
                                    End If
                                    blnEmailSent = True
                                    If strFileToUse.EndsWith(".pdf") Then
                                        txtResult.Text = txtResult.Text & "Emailed " & oClaim.invoiceid & " as PDF file to " & strEmailTo & vbCrLf
                                    Else
                                        txtResult.Text = txtResult.Text & "Emailed " & oClaim.invoiceid & " as RTF file to " & strEmailTo & vbCrLf
                                    End If
                                Else
                                    blnEmailSent = False
                                End If

                            Catch ex As Exception
                                blnEmailSent = False
                            End Try
                        Else
                            blnEmailSent = False
                        End If
                    End If

                    If Not blnEmailSent Then
                        If Not printDoc(strFileToUse) Then
                            strFailedToPrint = strFailedToPrint & vbCrLf & strFileToUse
                        End If

                        If getConfig("EmailTest") <> "True" Then
                            CommissionChase.updateSentBy(intId, "Printed")
                        End If
                        txtResult.Text = txtResult.Text & "Printed " & oClaim.invoiceid & vbCrLf
                    End If
                Next
            End If

            If strFailedToPrint <> "" Then
                txtResult.Text = txtResult.Text & vbCrLf & vbCrLf & "The following invoices failed to print:" & vbTab & strFailedToPrint
            End If
        End Using
    End Sub

    Private Function ConvertToPDF(ByVal inFile As String, ByVal outFile As String) As Boolean
        Using New clslogger(log, className, "ConvertToPDF")
            Try
                Dim p As New SautinSoft.PdfMetamorphosis()

                p.Serial = "10022099295"

                p.PageStyle.PageOrientation.Portrait()

                If p IsNot Nothing Then
                    Dim rtfFile As String = inFile
                    Dim pdfFile As String = outFile

                    Dim result As Integer = p.RtfToPdfConvertFile(rtfFile, pdfFile)
                End If

                If IO.File.Exists(outFile) Then
                    IO.File.Delete(inFile)
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                log.Error(ex.Message)
                Return False
            End Try

        End Using
    End Function


    'Private Sub buildAndSend(ByVal pbSpecial As Boolean, _
    '                         ByVal pbSpecial2 As Boolean, _
    '                         ByVal pbSpecialFinal As Boolean, _
    '                         ByVal pbSpeciallaw As Boolean, _
    '                         ByVal pbClaimInitial As Boolean, _
    '                         ByVal pbClaim2 As Boolean, _
    '                         ByVal pbClaim3 As Boolean, _
    '                         ByVal pbClaim4 As Boolean, _
    '                         ByVal pbClaim5 As Boolean, _
    '                         ByVal pbIndividual As Boolean, _
    '                         ByVal pbJustPrint As Boolean, _
    '                         ByVal pstrInvoiceNo As String, _
    '                         ByVal pstrEmail As String)
    '    Using New clslogger(log, className, "buildAndSend")
    '        Dim oClaims As List(Of BOSSinvmain)
    '        If pbSpecial Then
    '            oClaims = BOSSinvmain.commissionClaimSpecial()
    '        ElseIf pbIndividual Then
    '            oClaims = BOSSinvmain.commissionClaimIndiv(pstrInvoiceNo)
    '        Else
    '            oClaims = BOSSinvmain.commissionClaim(pbSpecial2, pbSpecialFinal, pbSpeciallaw, pbClaimInitial, pbClaim2, pbClaim3, pbClaim4, pbClaim5)
    '        End If
    '        Dim currentVenue As String = ""
    '        Dim lastVenue As String = ""
    '        Dim strDetails As String = ""
    '        Dim runningComNet As Decimal = 0
    '        Dim runningComVat As Decimal = 0
    '        Dim runningComTotal As Decimal = 0
    '        Dim ofiler As System.IO.StreamWriter
    '        Dim intcount As Integer = 0
    '        Dim straddress As String = ""
    '        Dim stremailtouse As String = ""
    '        Dim strDirectoryToSaveTo As String = ""
    '        Dim strIdsX As String = ""
    '        Dim strInvoiceIds As String = ""
    '        Dim strFailedToPrint As String = ""

    '        Dim intLineCount As Integer = 1

    '        If oClaims.Count > 0 Then

    '            'see if there is a directory for today
    '            For intDirCount As Integer = 1 To 10000
    '                If Not IO.Directory.Exists(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)) Then
    '                    IO.Directory.CreateDirectory(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount))
    '                    strDirectoryToSaveTo = getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)
    '                    Exit For
    '                End If
    '            Next

    '            For Each oClaim As BOSSinvmain In oClaims
    '                'If oClaim.invoiceid = "N442477" Then
    '                '    Dim istop As Integer = 0
    '                'End If
    '                currentVenue = oClaim.venuename
    '                If currentVenue <> lastVenue Then
    '                    'first finish off last venue
    '                    If lastVenue <> "" Then

    '                        Dim strFile As String = ""
    '                        If pbSpecial Then
    '                            strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSpecial.rtf")
    '                        Else
    '                            If pbSpecial2 Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSecond.rtf")
    '                            ElseIf pbSpecialFinal Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFinal.rtf")
    '                            ElseIf pbSpeciallaw Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionLaw.rtf")
    '                            ElseIf pbClaimInitial Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionInitial.rtf")
    '                            ElseIf pbClaim2 Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFirst.rtf")
    '                            ElseIf pbClaim3 Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSecond.rtf")
    '                            ElseIf pbClaim4 Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFinal.rtf")
    '                            ElseIf pbClaim5 Then
    '                                strFile = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionLaw.rtf")
    '                            End If
    '                        End If
    '                        Dim sVenue As String = lastVenue
    '                        sVenue = StrConv(LCase$(sVenue), vbProperCase)

    '                        strFile = strFile.Replace("#venuename#", sVenue & ".")
    '                        strFile = strFile.Replace("#address#", straddress)
    '                        strFile = strFile.Replace("#details#", strDetails)

    '                        strFile = strFile.Replace("#ctnet#", " \£" & CStr(runningComNet))
    '                        strFile = strFile.Replace("#ctvat#", " \£" & CStr(runningComVat))
    '                        strFile = strFile.Replace("#cttot#", " \£" & CStr(runningComTotal))
    '                        strFile = strFile.Replace("#date#", Format(Date.Now, "dd MMMM yyyy"))

    '                        ofiler = New System.IO.StreamWriter(strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf", False)
    '                        ofiler.Write(strFile)
    '                        ofiler.Flush()
    '                        ofiler.Close()

    '                        Dim blnEmailSent As Boolean = False
    '                        'if email exists and is in correct format then email, otherwise send to printer
    '                        If Not pbJustPrint Then
    '                            If Regex.IsMatch(stremailtouse, emailRegex) Then
    '                                Dim strEmailTo As String = ""
    '                                Dim strMessage As String = readText(getConfig("MIDocsPath") & "Payments\Body.htm")
    '                                If getConfig("EmailTest") = "True" Then
    '                                    strEmailTo = "nick.massarella@nysgroup.com"
    '                                    strMessage = "Email would have been sent to: " & stremailtouse & vbCrLf & strMessage
    '                                Else
    '                                    If stremailtouse = "info@java-hotel.com" Then
    '                                        strEmailTo = "fm.wetherby@ramadajarvis.co.uk"
    '                                    ElseIf stremailtouse = "reservationsglasgow@adobehotels.co.uk" Then
    '                                        strEmailTo = "assistantfinancialcontrollerglasgow@abodehotels.co.uk"
    '                                    Else
    '                                        strEmailTo = stremailtouse
    '                                    End If
    '                                End If
    '                                Try

    '                                    SendEmailMessage("commissions@nysgroup.com", strEmailTo, "Commission invoice", strMessage, _
    '                                                         strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf", "nick.massarella@nysgroup.com")

    '                                    'if successfull then save sentBy as email
    '                                    If getConfig("EmailTest") <> "True" Then
    '                                        If strIdsX.Length > 0 Then
    '                                            strIdsX = Mid(strIdsX, 1, Len(strIdsX) - 1)
    '                                            CommissionChase.updateSentBy(strIdsX, "Emailed")
    '                                        End If
    '                                    End If
    '                                    txtResult.Text = txtResult.Text & "Emailed " & strInvoiceIds & " to " & strEmailTo & vbCrLf
    '                                    strIdsX = ""
    '                                    strInvoiceIds = ""
    '                                    blnEmailSent = True
    '                                Catch ex As Exception
    '                                    blnEmailSent = False
    '                                End Try
    '                            Else
    '                                blnEmailSent = False
    '                            End If
    '                        End If

    '                        If Not blnEmailSent Then
    '                            If Not printDoc(strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf") Then
    '                                strFailedToPrint = strFailedToPrint & vbCrLf & strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf"
    '                            End If

    '                            If getConfig("EmailTest") <> "True" Then
    '                                If strIdsX.Length > 0 Then
    '                                    strIdsX = Mid(strIdsX, 1, Len(strIdsX) - 1)
    '                                    CommissionChase.updateSentBy(strIdsX, "Printed")
    '                                End If
    '                            End If
    '                            txtResult.Text = txtResult.Text & "Printed " & strInvoiceIds & vbCrLf
    '                            strIdsX = ""
    '                            strInvoiceIds = ""
    '                        End If
    '                        stremailtouse = ""
    '                        intcount += 1
    '                    End If

    '                    If oClaim.accountsemail <> "" Then
    '                        If Regex.IsMatch(oClaim.accountsemail, emailRegex) Then
    '                            stremailtouse = oClaim.accountsemail
    '                        End If
    '                    End If
    '                    If stremailtouse = "" And oClaim.venueemail <> "" Then
    '                        If Regex.IsMatch(oClaim.venueemail, emailRegex) Then
    '                            stremailtouse = oClaim.venueemail
    '                        End If
    '                    End If
    '                    If stremailtouse = "" And oClaim.latestcontactemail <> "" Then
    '                        If Regex.IsMatch(oClaim.latestcontactemail, emailRegex) Then
    '                            stremailtouse = oClaim.latestcontactemail
    '                        End If
    '                    End If
    '                    If pstrEmail <> "" Then
    '                        stremailtouse = pstrEmail
    '                    End If

    '                    straddress = CStr(IIf(oClaim.address1 <> "", StrConv(LCase$(oClaim.address1), vbProperCase) & ", \line ", ""))
    '                    straddress = straddress & CStr(IIf(oClaim.address2 <> "", StrConv(LCase$(oClaim.address2), vbProperCase) & ", \line ", ""))
    '                    straddress = straddress & CStr(IIf(oClaim.address3 <> "", StrConv(LCase$(oClaim.address3), vbProperCase) & ", \line ", ""))
    '                    straddress = straddress & CStr(IIf(oClaim.address4 <> "", StrConv(LCase$(oClaim.address4), vbProperCase) & ", \line ", ""))

    '                    If straddress <> "" Then
    '                        straddress = Mid(straddress, 1, Len(straddress) - 6)
    '                        straddress = straddress & CStr(IIf(oClaim.postcode <> "", oClaim.postcode & ". \line ", ""))
    '                    End If

    '                    straddress = straddress & CStr(IIf(oClaim.telephone <> "", "Tel: " & oClaim.telephone & " \line ", ""))
    '                    straddress = straddress & CStr(IIf(oClaim.fax <> "", "Fax: " & oClaim.fax & " \line ", ""))
    '                    straddress = straddress & CStr(IIf(stremailtouse <> "", "Email: " & stremailtouse & " \line ", ""))
    '                    If straddress <> "" Then
    '                        straddress = Mid(straddress, 1, Len(straddress) - 7)
    '                    End If
    '                    'now do new venue
    '                    strDetails = readText(getConfig("MIDocsPath") & "Payments\head.rtf")
    '                    strDetails = strDetails & readText(getConfig("MIDocsPath") & "Payments\line.rtf")
    '                    runningComNet = 0
    '                    runningComVat = 0
    '                    runningComTotal = 0
    '                    intLineCount = 1
    '                Else
    '                    strDetails = strDetails & readText(getConfig("MIDocsPath") & "Payments\line.rtf")
    '                End If

    '                Dim strLeadNameSplit As String = oClaim.leadname

    '                If oClaim.leadname.Contains("&") Then
    '                    Dim list As ICollection(Of String) = Split(oClaim.leadname, "&")
    '                    For Each str As String In list
    '                        If str.Contains("/") Then
    '                            str = str.Trim
    '                            Dim strFirst As String = Mid(str, str.IndexOf("/") + 2)
    '                            Dim strLast As String = Mid(str, 1, str.IndexOf("/"))
    '                            If strLeadNameSplit = "" Then
    '                                strLeadNameSplit = strFirst & " " & strLast
    '                            Else
    '                                strLeadNameSplit = strLeadNameSplit & " & " & strFirst & " " & strLast
    '                            End If
    '                        Else
    '                            If strLeadNameSplit = "" Then
    '                                strLeadNameSplit = str
    '                            Else
    '                                strLeadNameSplit = strLeadNameSplit & " & " & str
    '                            End If
    '                        End If
    '                    Next
    '                Else
    '                    If oClaim.leadname.Contains("/") Then
    '                        Dim strFirst As String = Mid(oClaim.leadname, oClaim.leadname.IndexOf("/") + 2)
    '                        Dim strLast As String = Mid(oClaim.leadname, 1, oClaim.leadname.IndexOf("/"))
    '                        strLeadNameSplit = strFirst & " " & strLast
    '                    End If
    '                End If

    '                strDetails = strDetails.Replace("#invref#", oClaim.invoiceid)
    '                strDetails = strDetails.Replace("#invdate#", oClaim.invoicedate)
    '                strDetails = strDetails.Replace("#evdate#", oClaim.eventdate)
    '                strDetails = strDetails.Replace("#ref#", oClaim.ref)
    '                strDetails = strDetails.Replace("#type#", oClaim.product)
    '                strDetails = strDetails.Replace("#lead#", strLeadNameSplit)
    '                strDetails = strDetails.Replace("#amt#", " \£" & CStr(oClaim.invoiceamount))
    '                strDetails = strDetails.Replace("#cnet#", " \£" & CStr(oClaim.commnet))
    '                strDetails = strDetails.Replace("#cvat#", " \£" & CStr(oClaim.commvat))
    '                strDetails = strDetails.Replace("#ctot#", " \£" & CStr(oClaim.commtotal))

    '                'first page break
    '                If intLineCount = 9 Then
    '                    strDetails = strDetails & " \pard \insrsid \page \par "
    '                    intLineCount = 20
    '                ElseIf intLineCount > 9 Then
    '                    If intLineCount Mod 20 = 0 Then
    '                        strDetails = strDetails & " \pard \insrsid \page \par "
    '                    End If
    '                End If

    '                runningComNet = runningComNet + oClaim.commnet
    '                runningComVat = runningComVat + oClaim.commvat
    '                runningComTotal = runningComTotal + oClaim.commtotal
    '                lastVenue = currentVenue

    '                'save each invoice details
    '                If getConfig("EmailTest") <> "True" Then

    '                    'Dim strSpecialDate As String = ""
    '                    Dim strClaimInitial As String = ""
    '                    'If pbSpecial Then
    '                    '    strSpecialDate = Format(Date.Now, "dd/MM/yyyy")
    '                    'End If
    '                    If pbClaimInitial Then
    '                        strClaimInitial = Format(Date.Now, "dd/MM/yyyy")
    '                    End If

    '                    Dim oSaver As New CommissionChase(0, oClaim.invoiceid, Format(Date.Now, "dd/MM/yyyy"), "", "", "", "", "", oClaim.venuename, CStr(oClaim.invoiceamount), _
    '                                                        CStr(oClaim.commnet), CStr(oClaim.commvat), CStr(oClaim.commtotal), oClaim.invoicedate, oClaim.eventdate, stremailtouse, "")
    '                    Dim intId As Integer = oSaver.save()

    '                    'If pbSpecial2 Then
    '                    '    CommissionChase.saveDate(1, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    'ElseIf pbSpecialFinal Then
    '                    '    CommissionChase.saveDate(2, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    'ElseIf pbSpeciallaw Then
    '                    '    CommissionChase.saveDate(3, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    'Else
    '                    If pbClaim2 Then
    '                        CommissionChase.saveDate(4, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    ElseIf pbClaim3 Then
    '                        CommissionChase.saveDate(5, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    ElseIf pbClaim4 Then
    '                        CommissionChase.saveDate(6, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    ElseIf pbClaim5 Then
    '                        CommissionChase.saveDate(7, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    ElseIf pbClaimInitial Then
    '                        CommissionChase.saveDate(8, Format(Date.Now, "dd/MM/yyyy"), oClaim.invoiceid)
    '                    End If


    '                    strIdsX = strIdsX & CStr(intId) & ","
    '                    strInvoiceIds = strInvoiceIds & oClaim.invoiceid & ","
    '                End If
    '                intLineCount += 1
    '            Next

    '            'do last one
    '            Dim strFile2 As String = ""
    '            If pbSpecial Then
    '                strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSpecial.rtf")
    '            Else
    '                If pbSpecial2 Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSecond.rtf")
    '                ElseIf pbSpecialFinal Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFinal.rtf")
    '                ElseIf pbSpeciallaw Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionLaw.rtf")
    '                ElseIf pbClaimInitial Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionInitial.rtf")
    '                ElseIf pbClaim2 Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFirst.rtf")
    '                ElseIf pbClaim3 Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionSecond.rtf")
    '                ElseIf pbClaim4 Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionFinal.rtf")
    '                ElseIf pbClaim5 Then
    '                    strFile2 = readText(getConfig("MIDocsPath") & "Payments\Templates\CommissionLaw.rtf")
    '                End If
    '            End If

    '            Dim sVenue2 As String = lastVenue
    '            sVenue2 = StrConv(LCase$(sVenue2), vbProperCase)

    '            strFile2 = strFile2.Replace("#venuename#", sVenue2 & ".")
    '            strFile2 = strFile2.Replace("#address#", straddress)
    '            strFile2 = strFile2.Replace("#details#", strDetails)

    '            strFile2 = strFile2.Replace("#ctnet#", " \£" & CStr(runningComNet))
    '            strFile2 = strFile2.Replace("#ctvat#", " \£" & CStr(runningComVat))
    '            strFile2 = strFile2.Replace("#cttot#", " \£" & CStr(runningComTotal))
    '            strFile2 = strFile2.Replace("#date#", Format(Date.Now, "dd MMMM yyyy"))

    '            ofiler = New System.IO.StreamWriter(strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf", False)
    '            ofiler.Write(strFile2)
    '            ofiler.Flush()
    '            ofiler.Close()

    '            Dim blnEmailSent2 As Boolean = False
    '            'if email exists and is in correct format then email, otherwise send to printer
    '            If Not pbJustPrint Then
    '                If Regex.IsMatch(stremailtouse, emailRegex) Then
    '                    Dim strEmailTo As String = ""
    '                    Dim strMessage As String = readText(getConfig("MIDocsPath") & "Payments\Body.htm")
    '                    If getConfig("EmailTest") = "True" Then
    '                        strEmailTo = "nick.massarella@nysgroup.com"
    '                        strMessage = "Email would have been sent to: " & stremailtouse & vbCrLf & strMessage
    '                    Else
    '                        If stremailtouse = "info@java-hotel.com" Then
    '                            strEmailTo = "fm.wetherby@ramadajarvis.co.uk"
    '                        ElseIf stremailtouse = "reservationsglasgow@adobehotels.co.uk" Then
    '                            strEmailTo = "assistantfinancialcontrollerglasgow@abodehotels.co.uk"
    '                        Else
    '                            strEmailTo = stremailtouse
    '                        End If
    '                    End If
    '                    Try
    '                        SendEmailMessage("commissions@nysgroup.com", strEmailTo, "Commission invoice", strMessage, _
    '                                         strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf", "nick.massarella@nysgroup.com")
    '                        'if successfull then save sentBy as email
    '                        If getConfig("EmailTest") <> "True" Then
    '                            If strIdsX.Length > 0 Then
    '                                strIdsX = Mid(strIdsX, 1, Len(strIdsX) - 1)
    '                                CommissionChase.updateSentBy(strIdsX, "Emailed")
    '                            End If
    '                        End If
    '                        blnEmailSent2 = True
    '                        txtResult.Text = txtResult.Text & "Emailed " & strInvoiceIds & " to " & strEmailTo & vbCrLf
    '                    Catch ex As Exception
    '                        blnEmailSent2 = False
    '                    End Try
    '                Else
    '                    blnEmailSent2 = False
    '                End If
    '            End If

    '            If Not blnEmailSent2 Then
    '                If Not printDoc(strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf") Then
    '                    strFailedToPrint = strFailedToPrint & vbCrLf & strDirectoryToSaveTo & "\CommInvoice" & CStr(intcount) & ".rtf"
    '                End If

    '                If getConfig("EmailTest") <> "True" Then
    '                    If strIdsX.Length > 0 Then
    '                        strIdsX = Mid(strIdsX, 1, Len(strIdsX) - 1)
    '                        CommissionChase.updateSentBy(strIdsX, "Printed")
    '                    End If
    '                End If
    '                txtResult.Text = txtResult.Text & "Printed " & strInvoiceIds & vbCrLf
    '            End If
    '        End If

    '        If strFailedToPrint <> "" Then
    '            txtResult.Text = txtResult.Text & vbCrLf & vbCrLf & "The following invoices failed to print:" & vbTab & strFailedToPrint
    '        End If
    '    End Using
    'End Sub

    Private Function printDoc(ByVal pstrPath As Object) As Boolean
        Using New clslogger(log, className, "printDoc")
            Try
                Dim MyProcess As New Diagnostics.Process
                MyProcess.StartInfo.CreateNoWindow = False
                MyProcess.StartInfo.Verb = "printto"
                MyProcess.StartInfo.FileName = CStr(pstrPath)
                MyProcess.Start()

                Dim intCount As Integer = 0
                Do While Not MyProcess.HasExited
                    If intCount < 11 Then
                        MyProcess.WaitForExit(2000)
                    Else
                        Exit Do
                    End If
                    intCount += 1
                Loop
                If CStr(pstrPath).EndsWith(".pdf") Then
                    MyProcess.Kill()
                End If
                MyProcess.Dispose()
                MyProcess.Close()
                Return (True)
            Catch ex As Exception
                log.Error(ex.Message)
                Return False
            End Try
        End Using
    End Function

    Private Sub IACommissionClaim_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IACommissionClaim_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IACommissionClaim"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnPrinter_Click(sender As Object, e As EventArgs) Handles btnPrinter.Click
        Using New clslogger(log, className, "btnPrinter_Click")
            Try
                Dim intErrorCount = 0
                If txtdate.Text <> "" Then
                    Try
                        Dim dtTest As Date = CDate(txtdate.Text)
                        If txtdate.Text.Contains("/") Then
                            txtdate.Text = Format(dtTest, "dd-MM-yyyy")
                        End If
                        For intDirCount As Integer = 1 To 100
                            If IO.Directory.Exists(getConfig("MIDocsPath") & "PaymentDocs\" & txtdate.Text & "-" & CStr(intDirCount) & "\ToPrint") Then

                                If Not IO.Directory.Exists(getConfig("MIDocsPath") & "PaymentDocs\" & txtdate.Text & "-" & CStr(intDirCount) & "\ToPrint\Printed") Then
                                    IO.Directory.CreateDirectory(getConfig("MIDocsPath") & "PaymentDocs\" & txtdate.Text & "-" & CStr(intDirCount) & "\ToPrint\Printed")
                                End If
                                For Each myFile As String In IO.Directory.GetFiles(getConfig("MIDocsPath") & "PaymentDocs\" & txtdate.Text & "-" & CStr(intDirCount) & "\ToPrint")
                                    Dim strFile As String = IO.Path.GetFileName(myFile)
                                    If strFile <> "" Then
                                        If strFile.Contains("CommInvoice") Then
                                            Threading.Thread.Sleep(1000)
                                            If printDoc(myFile) Then
                                                Dim blnMoved As Boolean = True
                                                Try
                                                    IO.File.Move(myFile, getConfig("MIDocsPath") & "PaymentDocs\" & txtdate.Text & "-" & CStr(intDirCount) & "\ToPrint\Printed\" & strFile)
                                                Catch ex As Exception
                                                    blnMoved = False
                                                End Try
                                                If Not blnMoved Then
                                                    Try
                                                        IO.File.Copy(myFile, getConfig("MIDocsPath") & "PaymentDocs\" & txtdate.Text & "-" & CStr(intDirCount) & "\ToPrint\Printed\" & strFile)
                                                        IO.File.Delete(myFile)
                                                    Catch ex As Exception
                                                        Throw New EvoFriendlyException("Error moving files, please restart.", "Info")
                                                    End Try
                                                End If
                                            Else
                                                intErrorCount += 1
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    Catch ex As Exception
                        txtResult.Text = "Not a date!"
                    End Try
                End If
               
                If intErrorCount > 0 Then
                    Throw New EvoFriendlyException("Errors: " & intErrorCount, "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnPrintInvoice_Click(sender As Object, e As EventArgs) Handles btnPrintInvoice.Click
        Using New clslogger(log, className, "btnPrintInvoice_Click")
            Try
                If txtinvoice.Text.Trim <> "" Then
                    If Regex.IsMatch(txtinvoice.Text, getConfig("InvoiceFormat")) Then
                        If clsBoss.runMainSelect(False, txtinvoice.Text.Trim, "", "", "") = "Successful run" Then
                            Dim strDirectoryToSaveTo As String = ""

                            For intDirCount As Integer = 1 To 10000
                                If Not IO.Directory.Exists(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)) Then
                                    IO.Directory.CreateDirectory(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount))
                                    strDirectoryToSaveTo = getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)
                                    Exit For
                                End If
                            Next
                            buildAndSendOne(False, False, False, False, True, False, False, False, False, True, True, txtinvoice.Text.Trim, "", strDirectoryToSaveTo)
                        Else
                            Throw New EvoFriendlyException("SQL did not update correctly from BOSS, see Nick.", "Check Details")
                        End If
                    Else
                        Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                    End If
                Else
                    Throw New EvoFriendlyException("Please complete the invoice number field.", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnEmail_Click(sender As Object, e As EventArgs) Handles btnEmail.Click
        Using New clslogger(log, className, "btnEmail_Click")
            Try
                If txtinvoiceEmail.Text.Trim <> "" And txtemail.Text.Trim <> "" Then
                    If Regex.IsMatch(txtemail.Text.Trim, emailRegex) Then
                        If txtinvoiceEmail.Text.Trim.Contains("#") Then
                            Dim list As ICollection(Of String) = Split(txtinvoiceEmail.Text.Trim, "#")
                            For Each strInv As String In list
                                strInv = strInv.Trim
                                If strInv <> "" Then
                                    If Regex.IsMatch(strInv, getConfig("InvoiceFormat")) Then
                                        Dim strResult As String = clsBoss.runMainSelect(False, strInv, "", "", "")
                                        If clsBoss.runMainSelect(False, strInv, "", "", "") = "Successful run" Then
                                            Dim strDirectoryToSaveTo As String = ""

                                            For intDirCount As Integer = 1 To 10000
                                                If Not IO.Directory.Exists(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)) Then
                                                    IO.Directory.CreateDirectory(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount))
                                                    strDirectoryToSaveTo = getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)
                                                    Exit For
                                                End If
                                            Next
                                            buildAndSendOne(False, False, False, False, True, False, False, False, False, True, False, strInv, txtemail.Text.Trim, strDirectoryToSaveTo)
                                        Else
                                            ' Throw New EvoFriendlyException("SQL did not update correctly from BOSS, see Nick.", "Check Details")
                                            Throw New EvoFriendlyException("SQL did not update correctly from BOSS" & strResult, "Check Details")
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            Dim strError As String = ""
                            If Regex.IsMatch(txtinvoiceEmail.Text.Trim, getConfig("InvoiceFormat")) Then
                                strError = clsBoss.runMainSelect(False, txtinvoiceEmail.Text.Trim, "", "", "")
                                If strError = "Successful run" Then
                                    Dim strDirectoryToSaveTo As String = ""

                                    For intDirCount As Integer = 1 To 10000
                                        If Not IO.Directory.Exists(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)) Then
                                            IO.Directory.CreateDirectory(getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount))
                                            strDirectoryToSaveTo = getConfig("MIDocsPath") & "PaymentDocs\" & Format(Now, "dd-MM-yyyy") & "-" & CStr(intDirCount)
                                            Exit For
                                        End If
                                    Next
                                    buildAndSendOne(False, False, False, False, True, False, False, False, False, True, False, txtinvoiceEmail.Text.Trim, txtemail.Text.Trim, strDirectoryToSaveTo)
                                Else
                                    Throw New EvoFriendlyException("You or another BOSS user may have locked this invoice record. Release by closing BOSS window accessing the invoice and try again later  " + strError, "Check Details")
                                End If
                            Else
                                Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                            End If
                        End If
                    Else
                        Throw New EvoFriendlyException("Email is not in correct format, please amend.", "Check Details")
                    End If
                Else
                    Throw New EvoFriendlyException("Please complete both Invoice no & email fields.", "Check Details")
                End If
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnsearch_Click(sender As Object, e As EventArgs) Handles btnsearch.Click
        Using New clslogger(log, className, "btnsearch_Click")
            Try
                If txtsearch.Text.Trim <> "" Then
                    Dim oSs As List(Of BOSSCommissionClaim)
                    oSs = BOSSCommissionClaim.bossCommissionSearch(txtsearch.Text.Trim)
                    For Each oS As BOSSCommissionClaim In oSs
                        txtResult2.Text = txtResult2.Text & oS.invoiceid & " - " & oS.venuename & " - " & oS.emailto & vbCrLf
                    Next
                End If
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub
End Class
