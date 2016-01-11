Option Strict Off

Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
Imports System.Security.Principal
Imports System.IO
Imports System.Globalization
'R2.21 SA
Imports NysDat

Public Class FeederFileCreation

    'Public Shared Sub removeDuplicates()
    '    Using dbh As New SqlDatabaseHandle(getConnection)
    '        dbh.callSPSingleValueCanReturnNothing("BossDuplicates_remove")
    '        dbh.callSPSingleValueCanReturnNothing("BossRouteDuplicates_remove")
    '        dbh.callSPSingleValueCanReturnNothing("BossTotDuplicates_remove")
    '    End Using
    'End Sub

    Public Shared Function Pack(ByVal str As String) As String
        Dim words As Object
        Dim x As Long
        Dim temp As String = ""

        str = str.Replace(vbCrLf, " ")
        words = str.Split(CChar(" "))

        For x = LBound(words) To UBound(words)
            If words(x) <> "" Then
                temp = temp & " " & words(x).ToString.Trim
            End If
        Next x
        Pack = temp
    End Function

    'R2.5 CR - added pVATrate for new VAT increase Jan 2011
    Public Shared Function testVat(ByVal plocalVExpense As Double, ByVal plocalVExpenseVat As Double, _
                                   ByVal plocalVService As Double, ByVal plocalVServiceVat As Double, _
                                   ByVal pOther As Double, ByVal pOtherVat As Double, ByVal pVATrate As Double) As Boolean
        Dim blnRet As Boolean = True
        If plocalVExpense <> 0 And plocalVExpenseVat <> 0 Then
            If Math.Round(plocalVExpense * pVATrate, 2) <> Math.Round(plocalVExpenseVat, 2) Then
                Dim dbldiff As Double = Math.Abs(Math.Round(Math.Round(plocalVExpense * pVATrate, 2) - Math.Round(plocalVExpenseVat, 2), 2))
                If dbldiff > 0.02 Then
                    blnRet = False
                End If
            End If
        ElseIf plocalVService <> 0 And plocalVServiceVat <> 0 Then
            If Math.Round(plocalVService * pVATrate, 2) <> Math.Round(plocalVServiceVat, 2) Then
                Dim dbldiff As Double = Math.Abs(Math.Round(Math.Round(plocalVService * pVATrate, 2) - Math.Round(plocalVServiceVat, 2), 2))
                If dbldiff > 0.02 Then
                    blnRet = False
                End If
            End If
        ElseIf pOther <> 0 And pOtherVat <> 0 Then
            If Math.Round(pOther * pVATrate, 2) <> Math.Round(pOtherVat, 2) Then
                Dim dbldiff As Double = Math.Abs(Math.Round(Math.Round(pOther * pVATrate, 2) - Math.Round(pOtherVat, 2), 2))
                If dbldiff > 0.02 Then
                    blnRet = False
                End If
            End If
        Else
            blnRet = True
        End If
        Return blnRet
    End Function

    Public Shared Function BatchNoGet(ByVal pClient As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = 0
            If getConfig("BatchTest") = "true" Then
                ret = 1
            Else
                ret = CInt(dbh.callSPSingleValueCanReturnNothing("Batchno_get", "@client", pClient))
            End If

            Return ret
        End Using
    End Function

    Public Shared Function createFileHighways(ByVal pstartdate As String, ByVal penddate As String, ByVal pstrType As String) As String

        'check to remove duplicates


        Dim intBachNo As Integer = BatchNoGet("Highways")
        Dim strBatchID As String = "NYS" & intBachNo.ToString.PadLeft(6, CChar("0"))

        Dim strInvoiceHeaders As New StringBuilder
        Dim strInvoiceLineStart As New StringBuilder

        Dim intInvoiceHeaderCount As Integer = 0
        Dim intInvoiceLineCount As Integer = 0
        'Dim dblInvoiceHeaderTotal As Double = 0
        Dim dblInvoiceHeaderTotal As Double = 0

        Dim rs As List(Of Highway)
        rs = Highway.list(pstartdate, penddate)
        Dim intLineNo As Integer = 1
        Dim nextInvoiceNumber As String = ""
        Dim lastInvoiceNumber As String = ""
        Dim strLineDescription As String = ""

        Dim intCCTest As Integer = 0
        'now for final run through if all else is OK
        Dim invoiceAMOUNTER As Double = 0
        Dim invoiceTOTALER As Double = 0
        Dim invoiceTOTALER2 As Double = 0
        Dim headersTotal As Double = 0
        Dim linesTotal As Double = 0
        Dim lines2Total As Double = 0
        Dim lineCounter As Integer = 0
        Dim localfee As Double = 0

        strInvoiceLineStart.Append("statement_period,")
        strInvoiceLineStart.Append("statement_lineno,")
        strInvoiceLineStart.Append("hamis_seqno,")
        strInvoiceLineStart.Append("traveller,")
        strInvoiceLineStart.Append("departure_date,")
        strInvoiceLineStart.Append("ticket_amount,")
        strInvoiceLineStart.Append("ticket_number,")
        strInvoiceLineStart.Append("billing_ref,")
        strInvoiceLineStart.Append("route,")
        strInvoiceLineStart.Append("mileage,")
        strInvoiceLineStart.Append("nys_ref" & vbCrLf)

        For Each r As Highway In rs

            If r.Invoicenumber = lastInvoiceNumber Or (r.ProductType.ToUpper = pstrType.ToUpper Or pstrType = "") Then
                'If r.Invoicenumber = "N487643" Then
                '    Dim ints As Integer = 0
                'End If
                lineCounter += 1

                nextInvoiceNumber = r.Invoicenumber
                If nextInvoiceNumber <> lastInvoiceNumber Then
                    intCCTest += 1
                End If

                r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " "))

                strInvoiceLineStart.Append(CStr(CDate(pstartdate).Year) & CStr(IIf(CDate(pstartdate).Month < 10, "0" & CDate(pstartdate).Month, CDate(pstartdate).Month)) & ",") 'colA
                'Dim strType As String = ""
                'If r.ProductType.ToUpper = "RR" Then
                '    strType = "T"
                'ElseIf r.ProductType.ToUpper = "H" Then
                '    strType = "H"
                'ElseIf r.ProductType.ToUpper = "A" Then
                '    strType = "A"
                'Else
                '    strType = r.Product
                'End If

                strInvoiceLineStart.Append(lineCounter & ",") 'colB
                strInvoiceLineStart.Append(r.Po & ",") 'colC
                'strInvoiceLineStart.Append("TM/" & intCCTest.ToString.PadLeft(6, CChar("0")) & "/" & strType & "1,") 'colC

                Dim strTraveller As String = r.Traveller
                Dim strFirstName As String = ""
                Dim strLastName As String = ""
                Dim strTempPO As String = r.Po
                Dim strTempPO2 As String = r.Po

                If r.Po <> "" And r.Traveller <> "" Then
                    strTempPO = strTempPO.ToUpper.Replace("T1", "T/1")
                    strTempPO2 = strTempPO2.ToUpper.Replace("T1", "T/1")


                    strTraveller = strTraveller.ToUpper.Replace(r.Po, "")
                    strTraveller = strTraveller.ToUpper.Replace("#", "")
                    strTraveller = strTraveller.ToUpper.Replace(" ", "")
                    strTraveller = strTraveller.ToUpper.Replace(r.Po, "")

                    If strTempPO <> "" Then
                        strTempPO = strTempPO.ToUpper.Replace("TM/", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO, "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO = strTempPO.ToUpper.Replace("TM", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO, "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO2 = strTempPO2.ToUpper.Replace("TM/00", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO2, "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO = strTempPO.ToUpper.Replace("/T/1", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO, "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO = strTempPO.ToUpper.Replace("T/1", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO, "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO = strTempPO.ToUpper.Replace("T1", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO, "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO = strTempPO.ToUpper.Replace("-1", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO, "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO = strTempPO.ToUpper.Replace("00", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO, "")

                        strTraveller = strTraveller.ToUpper.Replace("/T/1", "")
                        strTraveller = strTraveller.ToUpper.Replace("#", "")
                        strTraveller = strTraveller.ToUpper.Replace("-1", "")
                    End If
                    If strTempPO <> "" Then
                        strTempPO2 = strTempPO2.ToUpper.Replace("#", "")
                        strTempPO2 = strTempPO2.ToUpper.Replace("-1", "")
                        strTempPO2 = strTempPO2.ToUpper.Replace("/T/1", "")
                    End If
                    If strTraveller <> "" And strTempPO <> "" Then
                        strTraveller = strTraveller.ToUpper.Replace(strTempPO2, "")
                        strTraveller = strTraveller.ToUpper.Replace("TM/", "")
                        strTraveller = strTraveller.ToUpper.Replace("TM", "")
                    End If
                    If strTraveller <> "" Then
                        If strTraveller.LastIndexOf("/") > 0 Then
                            strFirstName = Mid(strTraveller, strTraveller.LastIndexOf("/") + 2)
                            strFirstName = strFirstName.ToUpper.Replace("MR", "")
                            strFirstName = strFirstName.ToUpper.Replace("MS", "")
                            strFirstName = strFirstName.ToUpper.Replace("MRS", "")
                            strFirstName = strFirstName.ToUpper.Replace("MISS", "")
                            strFirstName = strFirstName.ToUpper.Replace("DR", "")
                            strFirstName = strFirstName.ToUpper.Replace("PROF", "")

                            If strFirstName.Length > 1 Then
                                strFirstName = Mid(strFirstName, 1, 1)
                            End If
                            If strTraveller.ToUpper.Contains(" ") Then
                                strLastName = Mid(strTraveller, 1, strTraveller.IndexOf(" "))
                            Else
                                If strTraveller.IndexOf("/") > 0 Then
                                    strLastName = Mid(strTraveller, 1, strTraveller.IndexOf("/"))
                                Else
                                    strLastName = strTraveller
                                End If
                            End If
                            strTraveller = strFirstName & "_" & strLastName
                        End If
                    End If
                End If
                strInvoiceLineStart.Append(strTraveller & ",") 'colD
                strInvoiceLineStart.Append(Format(CDate(r.Departuredate), "yyyyMMdd") & ",") 'colE
                strInvoiceLineStart.Append(r.Invoiceamount & ",") 'total for invoice'colF

                If r.ProductType.ToUpper = "RR" Then 'colG???
                    strInvoiceLineStart.Append(r.Railticketref & ",")
                ElseIf r.ProductType.ToUpper = "H" Then
                    strInvoiceLineStart.Append(r.Hotelticketref & ",")
                ElseIf r.ProductType.ToUpper = "A" Then
                    strInvoiceLineStart.Append(r.Airticketref & ",")
                Else
                    strInvoiceLineStart.Append(",")
                End If

                If r.ProductType.ToUpper = "RR" Then 'colG???
                    strInvoiceLineStart.Append(r.Railtransactionref & ",")
                ElseIf r.ProductType.ToUpper = "H" Then
                    strInvoiceLineStart.Append(r.Hoteltransactionref & ",")
                ElseIf r.ProductType.ToUpper = "A" Then
                    strInvoiceLineStart.Append(r.Airtransactionref & ",")
                Else
                    strInvoiceLineStart.Append(",")
                End If

                If r.ProductType.ToUpper = "RR" Then 'colI
                    Dim strOrigin As String = r.EvolviOrigin
                    Dim strDestination As String = r.EvolviDestination
                    If strOrigin = "" Then
                        strOrigin = r.BossOrigin
                    End If
                    If strDestination = "" Then
                        strDestination = r.BossDestination
                    End If
                    strInvoiceLineStart.Append(strOrigin & " - " & strDestination & ",")
                ElseIf r.ProductType.ToUpper = "H" Then
                    strInvoiceLineStart.Append(r.VenueName & ",")
                ElseIf r.ProductType.ToUpper = "A" Then
                    strInvoiceLineStart.Append(r.BossOrigin & " - " & r.BossDestination & ",")
                Else
                    strInvoiceLineStart.Append(",")
                End If

                If r.ProductType.ToUpper = "RR" Then 'colJ
                    strInvoiceLineStart.Append(r.Raildistance & ",")
                Else
                    strInvoiceLineStart.Append("N/A" & ",")
                End If

                strInvoiceLineStart.Append(r.Invoicenumber & vbCrLf) 'colH???
                lastInvoiceNumber = r.Invoicenumber
            End If
        Next

        Return strInvoiceLineStart.ToString

    End Function

    Public Shared Function createFileLV(ByVal pstartdate As String, ByVal penddate As String, ByVal pextradate As String) As String

        'check to remove duplicates

        'first lets run checker to ensure each invoice total matches the total of the line values
        Dim strCheckerLines As New StringBuilder
        Dim oCheckers As List(Of LV)
        oCheckers = LV.checker(pstartdate, penddate, pextradate)
        For Each oChecker As LV In oCheckers
            'If oChecker.Invoicenumber = "N385755" Then
            '    Dim str As String = "test"
            'End If
            strCheckerLines.Append(oChecker.Invoicenumber & vbCrLf)
        Next

        Dim strInvoiceHeaders As New StringBuilder
        Dim strInvoiceLines As New StringBuilder
        Dim strErrorLines As New StringBuilder
        Dim intInvoiceHeaderCount As Integer = 0
        Dim intInvoiceLineCount As Integer = 0
        Dim dblInvoiceHeaderTotal As Double = 0

        Dim rs As List(Of LV)
        rs = LV.list(pstartdate, penddate, pextradate)
        Dim intLineNo As Integer = 1
        Dim nextInvoiceNumber As String = ""
        Dim lastInvoiceNumber As String = ""
        Dim strLineDescription As String = ""


        'first run through to check for incorrect employee numbers
        Dim strEmployeeErrors As New StringBuilder
        Dim strBookerTravellerErrors As New StringBuilder

        'R2.21 CR
        Dim strPOInvoices As New StringBuilder

        For Each r As LV In rs

            Dim strEmployeeNumber As String = r.Attribute7.Trim.Replace(".", "")
            Dim strBooker As String = r.Attribute2.Trim
            Dim strTraveller As String = r.Attribute3.Trim

            'R2.21 CR - temp comment out - doesn't allow PO's through!!!
            'R2.21 SA 
            Dim intTravellerID As Integer

            'R2.25 CR - recode the format check, now we use a regex!!
            ' just code 5 character ones in their own way as might be a PO
            If strEmployeeNumber.Length = 5 Then
                'log anything that looks like it has been billed to a PO
                strPOInvoices.Append(r.Invoicenumber & ":" & strEmployeeNumber & vbCrLf)
            Else
                If Regex.IsMatch(strEmployeeNumber, getConfig("LVEmployeeNumberFormat")) Then
                    'format is valid so check for a traveller in the DB
                    intTravellerID = clsTravellerProfiles.checkCode2(strEmployeeNumber)
                    If notInteger(intTravellerID) = 0 Then
                        strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
                    End If
                Else
                    'format is not valid so error
                    strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
                End If
            End If


            'If strEmployeeNumber.Length = 7 Then

            '    'R2.21 CR - add the orig back in for now
            '    'original check
            '    'Try
            '    '    Dim inttest As Integer = CInt(strEmployeeNumber)
            '    'Catch ex As Exception
            '    '    strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '    'End Try

            '    'R2.21 CR - temp comment out
            '    'R2.21 SA
            '    intTravellerID = clsTravellerProfiles.checkCode2(strEmployeeNumber)
            '    'R2.21 SA 
            '    If intTravellerID > 0 Then
            '        'original check
            '        Try
            '            Dim inttest As Integer = CInt(strEmployeeNumber)
            '        Catch ex As Exception
            '            strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '        End Try
            '    Else
            '        strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '    End If

            '    'R2.21 CR  - temp comment out
            'ElseIf strEmployeeNumber.Length = 8 Then
            '    If strEmployeeNumber.ToLower.StartsWith("m") Then
            '        strEmployeeNumber = strEmployeeNumber.ToLower.Replace("m", "")
            '        'R2.21 SA 
            '        intTravellerID = clsTravellerProfiles.checkCode2(strEmployeeNumber)
            '        If intTravellerID > 0 Then
            '            Try
            '                Dim inttest As Integer = CInt(strEmployeeNumber)
            '            Catch ex As Exception
            '                strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '            End Try
            '        Else
            '            strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '        End If
            '    Else
            '        strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '    End If

            '    'R2.21 CR - put old crap back in
            '    'ElseIf strEmployeeNumber.Length = 8 Then
            '    '    If strEmployeeNumber.ToLower.StartsWith("m") Then
            '    '        strEmployeeNumber = strEmployeeNumber.ToLower.Replace("m", "")
            '    '        Try
            '    '            Dim inttest As Integer = CInt(strEmployeeNumber)
            '    '        Catch ex As Exception
            '    '            strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '    '        End Try
            '    '    Else
            '    '        strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            '    '    End If

            '    'R2.21 CR - log anything that looks like it has been billed to a PO
            'ElseIf strEmployeeNumber.Length = 5 Then
            '    strPOInvoices.append(r.Invoicenumber & ":" & strEmployeeNumber & vbCrLf)

            'Else
            '    strEmployeeErrors.Append(r.Invoicenumber & " : " & r.Attribute7 & vbCrLf)
            'End If


            'now do booker & traveller
            If strBooker = "" And strTraveller = "" Then
                strBookerTravellerErrors.Append(r.Invoicenumber & vbCrLf)
            ElseIf strBooker.ToLower.Contains("group") Then
                strBookerTravellerErrors.Append(r.Invoicenumber & vbCrLf)
            ElseIf strTraveller.ToLower.Contains("group") Then
                strBookerTravellerErrors.Append(r.Invoicenumber & vbCrLf)
            ElseIf strBooker.ToLower.Contains("advised") Then
                strBookerTravellerErrors.Append(r.Invoicenumber & vbCrLf)
            ElseIf strTraveller.ToLower.Contains("advised") Then
                strBookerTravellerErrors.Append(r.Invoicenumber & vbCrLf)
            End If
        Next

        'R2.21 CR - temp comment out
        'R2.21 SA - skip checking if project code is provided 
        Dim strProjectCode As String
        'R2.21 SA - check ACK code
        Dim strACKCodeErrors As New StringBuilder
        For Each r As LV In rs
            strProjectCode = r.Projectcode.Trim

            Dim strACKCode As String = r.Company & r.Costcentre
            strACKCode = strACKCode.Trim
            Dim intTravellerID As Integer

            If strACKCode.Length = 7 Then
                'If project code is provided do not check ACK code
                If strProjectCode = "" Then
                    'check ACK code exists
                    intTravellerID = clsTravellerProfiles.checkCode1(strACKCode)
                    If intTravellerID > 0 Then
                        Try
                            Dim inttest As Integer = CInt(strACKCode)
                        Catch ex As Exception
                            strACKCodeErrors.Append(r.Invoicenumber & " : " & r.Company & r.Costcentre & vbCrLf)
                        End Try
                    Else
                        strACKCodeErrors.Append(r.Invoicenumber & " : " & r.Company & r.Costcentre & vbCrLf)
                    End If
                End If
            Else
                strACKCodeErrors.Append(r.Invoicenumber & " : " & r.Company & r.Costcentre & vbCrLf)
            End If
        Next

        'now run through to check for incorrect VAT amounts
        Dim strVATErrors As New StringBuilder

        For Each r As LV In rs
            If r.Invoicenumber = "N520379" Then
                Dim iistop As Integer = 0
            End If
            Dim localVExpense As Double = CDbl(r.Expense)
            Dim localVExpenseVat As Double = CDbl(r.Expensevat)
            Dim localVService As Double = CDbl(r.Servicecharge)
            Dim localVServiceVat As Double = 0
            Dim localVOk As Boolean = True
            'lets check the vat !!!!
            If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localVExpense = Ret.pexpense
                localVExpenseVat = Ret.pexpensevat
                localVService = Ret.pservice
                localVServiceVat = Ret.pservicevat
                localVOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localVExpenseVat > 0 Then
                    If localVExpense = 0 Then
                        If localVService = 0 Then
                            localVOk = False
                        Else
                            localVServiceVat = localVExpenseVat
                            localVExpenseVat = 0
                        End If
                    End If
                End If
            End If

            If localVOk Then
                'R2.5 CR - check VAT rate
                Dim dblVAT As Double = 0

                If CDate(r.Invoicedate) >= CDate("04/01/2011") Then
                    dblVAT = 0.2
                Else
                    dblVAT = 0.175
                End If

                If Not testVat(localVExpense, localVExpenseVat, 0, 0, 0, 0, dblVAT) Then
                    strVATErrors.Append(r.Invoicenumber & " line: " & r.Linenumber & vbCrLf)
                End If
                If Not testVat(0, 0, 0, 0, CDbl(r.Othercharge), CDbl(r.Othervat), dblVAT) Then
                    strVATErrors.Append(r.Invoicenumber & " line: " & r.Linenumber & vbCrLf)
                End If
                If Not testVat(0, 0, localVService, localVServiceVat, 0, 0, dblVAT) Then
                    strVATErrors.Append(r.Invoicenumber & " line: " & r.Linenumber & vbCrLf)
                End If
            Else
                strVATErrors.Append(r.Invoicenumber & " line: " & r.Linenumber & vbCrLf)
            End If
        Next

        Dim strRet As String = ""
        If strVATErrors.ToString <> "" Then
            strRet = "THE FOLLOWING INVOICES HAVE INCORRECT VAT AMOUNTS:" & vbCrLf & strVATErrors.ToString
        End If

        If strBookerTravellerErrors.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING INVOICES HAVE NO/INCORRECT BOOKER/TRAVELLER NAMES:" & vbCrLf & strBookerTravellerErrors.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING INVOICES HAVE NO/INCORRECT BOOKER/TRAVELLER NAMES:" & vbCrLf & strBookerTravellerErrors.ToString
            End If
        End If
        If strEmployeeErrors.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING INVOICES HAVE INCORRECT EMPLOYEE NUMBERS:" & vbCrLf & strEmployeeErrors.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING INVOICES HAVE INCORRECT EMPLOYEE NUMBERS:" & vbCrLf & strEmployeeErrors.ToString
            End If
        End If

        If strCheckerLines.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING INVOICE TOTAL DOES NOT MATCH THE TOTAL OF THE INVOICE LINES:" & vbCrLf & strCheckerLines.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING INVOICE TOTAL DOES NOT MATCH THE TOTAL OF THE INVOICE LINES:" & vbCrLf & strCheckerLines.ToString
            End If
        End If

        'R2.21 CR - temp comment out
        'R2.21 SA
        If strACKCodeErrors.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING INVOICES HAVE INCORRECT ACK CODES:" & vbCrLf & strACKCodeErrors.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING INVOICES HAVE INCORRECT ACK CODES:" & vbCrLf & strACKCodeErrors.ToString
            End If
        End If

        'R2.21 CR
        If strPOInvoices.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING INVOICES APPEAR TO HAVE BEEN BILLED TO A PO, THESE MUST BE BILLED BACK TO LVCONF AS FEEDER FILE DOES NOT SUPPORT PO'S:" & vbCrLf & strPOInvoices.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING INVOICES APPEAR TO HAVE BEEN BILLED TO A PO, THESE MUST BE BILLED BACK TO LVCONF AS FEEDER FILE DOES NOT SUPPORT PO'S:" & vbCrLf & strPOInvoices.ToString
            End If
        End If

        If strRet <> "" Then
            Return strRet
        End If

        Dim intBachNo As Integer = BatchNoGet("LV")
        Dim strBatchID As String = "NYS" & intBachNo.ToString.PadLeft(6, CChar("0"))

        Dim strBatch As New StringBuilder
        strBatch.Append("B|" & strBatchID & "|" & Format(Now, "dd-MM-yyyy") & vbCrLf)

        'now for final run through if all else is OK
        Dim invoiceAMOUNTER As Double = 0
        Dim invoiceTOTALER As Double = 0
        Dim invoiceTOTALER2 As Double = 0
        Dim headersTotal As Double = 0
        Dim linesTotal As Double = 0
        Dim lines2Total As Double = 0
        Dim linegroupnumber As Integer = 0
        For Each r As LV In rs
            If r.Invoicenumber = "N549724" Then
                Dim iTest As Integer = 0
            End If
            nextInvoiceNumber = r.Invoicenumber
            If nextInvoiceNumber <> lastInvoiceNumber Then
                intLineNo = 1
                'R1 NM
                linegroupnumber = 0
            End If

            Dim localExpense As Double = CDbl(r.Expense)
            Dim localExpenseVat As Double = CDbl(r.Expensevat)
            Dim localService As Double = CDbl(r.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            'lets check the vat !!!!
            If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localService = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat <> 0 Then
                    If localExpense = 0 Then
                        localServiceVat = localExpenseVat
                        localExpenseVat = 0
                    End If
                End If
            End If

            'add sanity check for minus figures for airline charges as NYS don't want to show cancel charges for air tickets
            If r.Airlinecharge < 0 Then
                localExpense = CDbl(localExpense + r.Airlinecharge)
            End If
            If Not localOk Then
                strErrorLines.Append(r.Invoicenumber & vbCrLf)
            End If

            r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " "))

            If intLineNo = 1 Then 'first line so add header details first
                strInvoiceHeaders.Append("H|")
                strInvoiceHeaders.Append(strBatchID & "|")
                strInvoiceHeaders.Append(r.Invoicenumber.Trim & "|")
                strInvoiceHeaders.Append(Format(r.Invoiceamount, "0.00") & "|")
                strInvoiceHeaders.Append(Format(r.Invoicedate, "dd-MM-yyyy") & "|")
                strInvoiceHeaders.Append(r.Invoicedescription & "|")
                strInvoiceHeaders.Append(r.Suppliernumber.Trim & "|")
                strInvoiceHeaders.Append(r.Suppliersitecode.Trim & vbCrLf)
                intInvoiceHeaderCount += 1
                If r.Linedescription = "" Then
                    strLineDescription = r.Product
                Else
                    strLineDescription = r.Linedescription
                End If
                'do some adding up to compare later
                invoiceAMOUNTER = CDbl(r.Invoiceamount)
                headersTotal = headersTotal + CDbl(r.Invoiceamount)
            End If

            If localExpense <> 0 And localExpenseVat <> 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("ITEM" & "|")
                strInvoiceLines.Append(Format(localExpense, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                'R1 NM
                'strInvoiceLines.Append(r.Attribute7.Trim & vbCrLf)
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)

                intInvoiceLineCount += 1

                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("TAX" & "|")
                strInvoiceLines.Append(Format(localExpenseVat, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                'R1 NM
                'strInvoiceLines.Append(r.Attribute7.Trim & vbCrLf)
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            ElseIf localExpense <> 0 And localExpenseVat = 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("ITEM" & "|")
                strInvoiceLines.Append(Format(localExpense, "0.00") & "|")
                strInvoiceLines.Append("EXEMPT" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                'R1 NM
                'strInvoiceLines.Append(r.Attribute7.Trim & vbCrLf)
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            ElseIf localExpense = 0 And localExpenseVat = 0 Then
                'not needed anymore
            ElseIf localExpense = 0 And localExpenseVat <> 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("TAX" & "|")
                strInvoiceLines.Append(Format(localExpenseVat, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                'R1 NM
                'strInvoiceLines.Append(r.Attribute7.Trim & vbCrLf)
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            Else
                Dim inn As Integer = 1
            End If

            If r.Othercharge <> 0 And r.Othervat <> 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("ITEM" & "|")
                strInvoiceLines.Append(Format(r.Othercharge, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1

                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("TAX" & "|")
                strInvoiceLines.Append(Format(r.Othervat, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            ElseIf r.Othercharge <> 0 And r.Othervat = 0 Then
                'intLineNo = 1
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("ITEM" & "|")
                strInvoiceLines.Append(Format(r.Othercharge, "0.00") & "|")
                strInvoiceLines.Append("EXEMPT" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            ElseIf r.Othercharge = 0 And r.Othervat = 0 Then
            Else
                Dim inn As Integer = 1
            End If
            If localService <> 0 And localServiceVat <> 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("ITEM" & "|")
                strInvoiceLines.Append(Format(localService, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1

                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("TAX" & "|")
                strInvoiceLines.Append(Format(localServiceVat, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            ElseIf localService <> 0 And localServiceVat = 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("ITEM" & "|")
                strInvoiceLines.Append(Format(localService, "0.00") & "|")
                strInvoiceLines.Append("EXEMPT" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            ElseIf localService = 0 And localServiceVat = 0 Then
                'not needed anymore

            ElseIf localService = 0 And localServiceVat <> 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("TAX" & "|")
                strInvoiceLines.Append(Format(localServiceVat, "0.00") & "|")
                strInvoiceLines.Append("STD" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            Else
                Dim inn As Integer = 1
            End If
            If r.Airlinecharge > 0 Then
                strInvoiceLines.Append("L|")
                strInvoiceLines.Append(strBatchID & "|")
                strInvoiceLines.Append(r.Invoicenumber.Trim & "|")
                strInvoiceLines.Append(intLineNo & "|")
                intLineNo += 1
                strInvoiceLines.Append("ITEM" & "|")
                strInvoiceLines.Append(Format(r.Airlinecharge, "0.00") & "|")
                strInvoiceLines.Append("EXEMPT" & "|")
                If r.Linedescription = "" Then
                    strInvoiceLines.Append(strLineDescription & "|")
                Else
                    strInvoiceLines.Append(r.Linedescription & "|")
                End If
                strInvoiceLines.Append(r.Company & "|")
                strInvoiceLines.Append(r.Costcentre & "|")
                strInvoiceLines.Append(r.Account.Trim & "|")
                strInvoiceLines.Append(r.Product.Trim & "|")
                strInvoiceLines.Append(r.Projectcode.Trim & "|")
                strInvoiceLines.Append(r.Attributecategory.Trim & "|")
                strInvoiceLines.Append(r.Attribute1.Trim & "|")
                strInvoiceLines.Append(r.Attribute2.Trim & "|")
                strInvoiceLines.Append(r.Attribute3.Trim & "|")
                strInvoiceLines.Append(r.Attribute4.Trim & "|")
                strInvoiceLines.Append(r.Attribute5.Trim & "|")
                strInvoiceLines.Append(r.Attribute6.Trim & "|")
                strInvoiceLines.Append(r.Attribute7.Trim & "|")
                If linegroupnumber = 0 Then
                    linegroupnumber = 1
                Else
                    linegroupnumber += 1
                End If
                strInvoiceLines.Append(linegroupnumber & vbCrLf)
                intInvoiceLineCount += 1
            End If
            lastInvoiceNumber = r.Invoicenumber
            'don't add airline charge if less than zero as it has already been added
            If r.Airlinecharge < 0 Then
                dblInvoiceHeaderTotal = CDbl(dblInvoiceHeaderTotal + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat)
            Else
                dblInvoiceHeaderTotal = CDbl(dblInvoiceHeaderTotal + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge)
            End If

        Next

        Dim strInvoiceTrailer As New StringBuilder
        strInvoiceTrailer.Append("T|" & strBatchID & "|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|" & Format(dblInvoiceHeaderTotal, "0.00"))

        strBatch.Append(strInvoiceHeaders.ToString)
        strBatch.Append(strInvoiceLines.ToString)
        strBatch.Append(strInvoiceTrailer.ToString)

        If strErrorLines.ToString <> "" Then
            strErrorLines.Append("The above Invoice/s have incorrect VAT values for the fare and/or service charges")
            Return strErrorLines.ToString
        Else
            Return strBatch.ToString
        End If

    End Function

    Public Shared Function createFileAnchor(ByVal pstartdate As String, ByVal penddate As String) As String

        Dim strCheckerLines As New StringBuilder
        Dim strLines As New StringBuilder

        Dim rs As List(Of Anchor)
        rs = Anchor.list(pstartdate, penddate)

        'run through and check account and cc values
        For Each r As Anchor In rs
            If r.Account.Length <> 5 Then
                strCheckerLines.Append(r.Invoice & " - Account:" & r.Account & " is incorrect" & vbCrLf)
            Else
                Try
                    Dim lngTest As Long = CLng(r.Account)
                Catch ex As Exception
                    strCheckerLines.Append(r.Invoice & " - Account:" & r.Account & " is incorrect" & vbCrLf)
                End Try
            End If
            If r.Costc.Length <> 8 Then
                strCheckerLines.Append(r.Invoice & " - CostC:" & r.Costc & " is incorrect" & vbCrLf)
            Else
                Try
                    Dim lngTest As Long = CLng(r.Costc)
                Catch ex As Exception
                    strCheckerLines.Append(r.Invoice & " - CostC:" & r.Costc & " is incorrect" & vbCrLf)
                End Try
            End If
            'R?? SA -if not a refun and it's not hotel -check employee number is 8 char long 
            If r.Amount > 0 AndAlso r.Product <> "H" AndAlso r.EmployeeNumber.Length <> 8 Then
                strCheckerLines.Append(r.Invoice & " - EmployeeNumber:" & r.EmployeeNumber & " is incorrect" & vbCrLf)
            ElseIf r.EmployeeNumber.Length = 8 Then
                Try
                    Dim lngTest As Long = CLng(r.EmployeeNumber)
                Catch ex As Exception
                    strCheckerLines.Append(r.Invoice & " - EmployeeNumber:" & r.EmployeeNumber & " is incorrect" & vbCrLf)
                End Try
            End If
        Next

        If strCheckerLines.ToString <> "" Then
            Return "ERROR: THE FOLLOWING INVOICE/S HAVE INCORRECT ACCOUNT or COSTC or EmployeeNumber:" & vbCrLf & strCheckerLines.ToString
        End If

        'R?? - Employee number, and name to have be in seperate columns 
        'all ok, so let's go
        'R1.1 SA - added inm_ldname
        strLines.Append("account|costc|project|asset|resource|businessid|cur_amount|amount|employeenumber|name|descript|invoice|vat|disp_vat" & vbCrLf)
        For Each r As Anchor In rs
            'R1.17 SA
            Dim strName As String
            strName = nameSwitch(r.Inm_ldname)

            'R?? Anchor want employee number and name to be in seperate columns
            'strName = strName & " " & r.EmployeeNumber

            strLines.Append(r.Account & "|" & r.Costc & "|" & r.Project & "|" & r.Asset & "|" & r.Resource & "|" & _
                            r.Businessid & "|" & r.Cur_amount & "|" & r.Amount & "|" & r.EmployeeNumber & "|" & strName & "|" & " " & r.Descript.Replace(vbCrLf, " ") & _
                            "|" & r.Invoice & "|" & r.Vat & "|" & r.DispVat & vbCrLf)
        Next

        Return strLines.ToString()

    End Function

    'R1.17 SA 
    Public Shared Function nameSwitch(ByVal pName As String) As String
        Dim strName As String = pName.ToUpper
        'do MRS first - otherwise removing MR might leave a single S
        Dim titles() As String = New String() {"MRS", "MR", "MS", "MISS", "DR", "PROF"}
        Dim strFirstName As String = ""
        Dim strLastName As String = ""
        Dim intIndex As Integer = 0

        For Each strSalutation As String In titles
            If strName.EndsWith(strSalutation) Then
                strName = strName.Remove(strName.LastIndexOf(strSalutation))
                Exit For
            End If
        Next
        If strName.Contains("/") Then
            intIndex = strName.IndexOf("/")
            strFirstName = strName.Substring(intIndex + 1)
            strLastName = strName.Substring(0, intIndex)
            strFirstName = strFirstName.Trim()
            strLastName = strLastName.Trim()
            strName = strFirstName & " " & strLastName
        End If
        Return strName
    End Function

    Public Shared Function checkNPSAProjectCode(ByVal pCode As String) As Boolean
        Using dbh As New SqlDatabaseHandle(getCubitConnection)
            If clsNYS.notNumber(dbh.callSPSingleValueCanReturnNothing("NPSAProjectCode_Check", "@ProjectCode", pCode)) > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    'R2.17 CR
    Public Shared Function createFileNCAS(ByVal pstartdate As String, ByVal penddate As String) As String

        Dim strCheckerLines As New StringBuilder

        Dim rs As List(Of NCAS)
        rs = NCAS.list(pstartdate, penddate)

        'first run through and check validity
        For Each r As NCAS In rs
            If r.Memo.Length > 0 Then
                strCheckerLines.Append(r.InvoiceRef & " - Memo is too long (should be blank) - '" & r.Memo & "'" & vbCrLf)
            End If
            If r.SupplierName.Length > 0 Then
                strCheckerLines.Append(r.InvoiceRef & " - SupplierName is too long (should be blank) - '" & r.SupplierName & "'" & vbCrLf)
            End If
            If r.TrxNum.Length > 12 Then
                strCheckerLines.Append(r.InvoiceRef & " - TrxNum is too long(12) - '" & r.TrxNum & "'" & vbCrLf)
            End If
            If r.HeaderDescription.Length > 240 Then
                strCheckerLines.Append(r.InvoiceRef & " - HeaderDescription is too long(240) - '" & r.HeaderDescription & "'" & vbCrLf)
            End If
            If r.VsrRef.Length > 0 Then
                strCheckerLines.Append(r.InvoiceRef & " - VsrRef is too long (should be blank) - '" & r.VsrRef & "'" & vbCrLf)
            End If
            If r.ReceivedDate.Length > 0 Then
                strCheckerLines.Append(r.InvoiceRef & " - ReceivedDate is too long (should be blank) - '" & r.ReceivedDate & "'" & vbCrLf)
            End If
            If r.CostCentre.Length > 6 Then
                strCheckerLines.Append(r.InvoiceRef & " - CostCentre is too long(6 max) - '" & r.CostCentre & "'" & vbCrLf)
            End If

            'R2.21.1 CR - NCAS change to NHS Lita
            'subjective now 6 chars instead of 4
            If r.Subjective.Length > 6 Then
                strCheckerLines.Append(r.InvoiceRef & " - Subjective is too long(6 max) - '" & r.Subjective & "'" & vbCrLf)
            End If

            'R2.21.1 CR - no longer need Analysis codes
            'If r.Analysis1.Length > 5 Then
            '    strCheckerLines.Append(r.InvoiceRef & " - Analysis1 is too long(5 max) - '" & r.Analysis1 & "'" & vbCrLf)
            'End If
            'If r.Analysis2.Length > 5 Then
            '    strCheckerLines.Append(r.InvoiceRef & " - Analysis2 is too long(5 max) - '" & r.Analysis2 & "'" & vbCrLf)
            'End If

            If r.Type.ToUpper <> "ITEM" Then
                strCheckerLines.Append(r.InvoiceRef & " - Type is incorrect (should always be 'ITEM') - '" & r.Type & "'" & vbCrLf)
            End If
            If r.LineDescription.Length > 240 Then
                strCheckerLines.Append(r.InvoiceRef & " - LineDescription is too long(240) - '" & r.LineDescription & "'" & vbCrLf)
            End If

            'R2.17 CR - DO NOT NEED THIS!! - project codes now not used
            'now check Project code is good
            'If Not checkNPSAProjectCode(r.Dim_2) Then
            '    strCheckerLines.Append(r.Ext_ref & " Incorrect Project code: - " & r.Dim_2 & vbCrLf)
            'End If
        Next

        If strCheckerLines.Length > 0 Then
            Return strCheckerLines.ToString
        End If

        Dim dblTotalGross As Double = 0
        Dim dblTotalVat As Double = 0
        Dim dblTotalDisVat As Double = 0
        Dim strInvoiceRef As String = ""
        Dim strInvoiceDate As String = ""

        Dim dblTotalCreditGross As Double = 0
        Dim dblTotalCreditVat As Double = 0
        Dim dblTotalCreditDisVat As Double = 0
        Dim strCreditRef As String = ""
        Dim strCreditDate As String = ""

        'strLines - for invoice lines only (no credit notes)
        Dim strLines As New StringBuilder

        'strCreditLines - for credit notes (have to be seperate)
        Dim strCreditLines As New StringBuilder

        'Add the headings!!
        strLines.Append("Memo|")
        strLines.Append("Supplier Name|")
        strLines.Append("Trx Num|")
        strLines.Append("Header Desc|")
        strLines.Append("VSR REF|")
        strLines.Append("Trx Date|")
        strLines.Append("Received|")
        strLines.Append("Cost Centre|")
        strLines.Append("Subjective|")
        'strLines.Append("Analysis 1|")
        'strLines.Append("Analysis 2|")
        strLines.Append("Type|")
        strLines.Append("TAX|")
        strLines.Append("Description|")
        strLines.Append("Line £|")
        strLines.Append("Unit £")
        strLines.Append(vbCrLf)

        'don't add the headings for the credit notes just yet!
        'do it later so that we can check to see if any credit are on this run or not by the length of the stringbuilder

        For Each r As NCAS In rs
            If r.CreditNote Then
                strCreditLines.Append(r.Memo & "|")
                strCreditLines.Append(r.SupplierName & "|")
                strCreditLines.Append(r.TrxNum & "|")
                strCreditLines.Append(r.HeaderDescription & "|")
                strCreditLines.Append(r.VsrRef & "|")
                strCreditLines.Append(r.TrxDate & "|")
                strCreditLines.Append(r.ReceivedDate & "|")
                strCreditLines.Append(" " & r.CostCentre & "|")
                strCreditLines.Append(r.Subjective & "|")
                'strCreditLines.Append(r.Analysis1 & "|")
                'strCreditLines.Append(r.Analysis2 & "|")
                strCreditLines.Append(r.Type & "|")
                strCreditLines.Append(r.Tax & "|")
                strCreditLines.Append(r.LineDescription & "|")
                strCreditLines.Append(r.LineAmount & "|")
                strCreditLines.Append(r.UnitAmount)
                strCreditLines.Append(vbCrLf)

                strCreditRef = r.TrxNum
                strCreditDate = r.TrxDate
                dblTotalCreditGross += r.LineAmount
                dblTotalCreditVat += r.LineVat
                dblTotalCreditDisVat += r.LineDisVat
            Else
                strLines.Append(r.Memo & "|")
                strLines.Append(r.SupplierName & "|")
                strLines.Append(r.TrxNum & "|")
                strLines.Append(r.HeaderDescription & "|")
                strLines.Append(r.VsrRef & "|")
                strLines.Append(r.TrxDate & "|")
                strLines.Append(r.ReceivedDate & "|")
                strLines.Append(" " & r.CostCentre & "|")
                strLines.Append(r.Subjective & "|")
                'strLines.Append(r.Analysis1 & "|")
                'strLines.Append(r.Analysis2 & "|")
                strLines.Append(r.Type & "|")
                strLines.Append(r.Tax & "|")
                strLines.Append(r.LineDescription & "|")
                strLines.Append(r.LineAmount & "|")
                strLines.Append(r.UnitAmount)
                strLines.Append(vbCrLf)

                strInvoiceRef = r.TrxNum
                strInvoiceDate = r.TrxDate
                dblTotalGross += r.LineAmount
                dblTotalVat += r.LineVat
                dblTotalDisVat += r.LineDisVat
            End If
        Next

        'create the new folders for today
        makeFolderExist(getConfig("XMLFilePath") & "\NCAS")
        makeFolderExist(getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy"))

        Try
            'create the feeder file
            Dim strFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & ".csv"
            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
            ofiler.Write(strLines.ToString.Replace(",", " ").Replace("|", ","))
            ofiler.Flush()
            ofiler.Close()
        Catch ex As Exception
            Return "Unable to create feeder CSV file, please speak to development team"
        End Try


        If strCreditLines.Length > 0 Then
            'Add the headings!! 
            'have to do it backwards though - insert(0,"") will add to start of string every time
            ' alternative is to mess around with string lengths - bugger that!
            strCreditLines.Insert(0, vbCrLf)
            strCreditLines.Insert(0, "Unit £")
            strCreditLines.Insert(0, "Line £|")
            strCreditLines.Insert(0, "Description|")
            strCreditLines.Insert(0, "TAX|")
            strCreditLines.Insert(0, "Type|")
            'strCreditLines.Insert(0, "Analysis 2|")
            'strCreditLines.Insert(0, "Analysis 1|")
            strCreditLines.Insert(0, "Subjective|")
            strCreditLines.Insert(0, "Cost Centre|")
            strCreditLines.Insert(0, "Received|")
            strCreditLines.Insert(0, "Trx Date|")
            strCreditLines.Insert(0, "VSR REF|")
            strCreditLines.Insert(0, "Header Desc|")
            strCreditLines.Insert(0, "Trx Num|")
            strCreditLines.Insert(0, "Supplier Name|")
            strCreditLines.Insert(0, "Memo|")

            Try
                'create the credit note feeder file
                Dim strCreditFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " Credits.csv"
                'Dim strCreditFileName As String = strCreditRef & ".csv"
                Dim ofiler2 As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy") & "\" & strCreditFileName, False, Encoding.Default)
                ofiler2.Write(strCreditLines.ToString.Replace(",", " ").Replace("|", ","))
                ofiler2.Flush()
                ofiler2.Close()
            Catch ex As Exception
                Return "Unable to create credit note feeder CSV file, please speak to development team"
            End Try
        End If

        Try
            'create the cover sheet file
            Dim strCoverFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " CoverSheet.rtf"
            Dim strRtfFilePath As String = getConfig("XMLFilePath") & "\NCAS\" & Format(Now, "dd-MM-yyyy") & "\" & strCoverFileName

            Dim ofiler3 As New System.IO.StreamWriter(strRtfFilePath, False, Encoding.Default)
            Dim strCoverSheet As String = clsNYS.readText(getConfig("XMLFilePath") & "\NCAS\Template\CoverSheet.rtf")

            strCoverSheet = strCoverSheet.Replace("#TRXNUM#", strInvoiceRef)
            strCoverSheet = strCoverSheet.Replace("#TRXCREDITNUM#", strCreditRef)
            strCoverSheet = strCoverSheet.Replace("#TRXDATE#", strInvoiceDate)
            strCoverSheet = strCoverSheet.Replace("#STARTDATE#", pstartdate)
            strCoverSheet = strCoverSheet.Replace("#ENDDATE#", penddate)

            If strCoverSheet.Contains("#TOTALNET#") Then
                Dim strtest As String = ""
                strtest = "yes"
            End If

            dblTotalGross = FormatNumber(dblTotalGross, 2)
            dblTotalVat = FormatNumber(dblTotalVat, 2)
            dblTotalCreditGross = FormatNumber(dblTotalCreditGross, 2)
            dblTotalCreditVat = FormatNumber(dblTotalCreditVat, 2)
            dblTotalCreditDisVat = FormatNumber(dblTotalCreditDisVat, 2)

            strCoverSheet = strCoverSheet.Replace("#TOTALNET#", "£" & FormatNumber(dblTotalGross, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALVAT#", "£" & FormatNumber(dblTotalVat, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALGROSS#", "£" & FormatNumber(dblTotalGross, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALDISVAT#", "£" & FormatNumber(dblTotalDisVat + dblTotalCreditDisVat, 2))

            strCoverSheet = strCoverSheet.Replace("#TOTALCREDITNET#", "£" & FormatNumber((dblTotalCreditGross - dblTotalCreditVat), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALCREDITVAT#", "£" & FormatNumber(dblTotalCreditVat, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALCREDITGROSS#", "£" & FormatNumber(dblTotalCreditGross, 2))
            'strCoverSheet = strCoverSheet.Replace("#TOTALCREDITDISVAT#", "£" & FormatNumber(dblTotalCreditDisVat, 2))

            strCoverSheet = strCoverSheet.Replace("#INVOICETOTALNET#", "£" & FormatNumber((dblTotalGross - dblTotalVat) + (dblTotalCreditGross - dblTotalCreditVat), 2))
            strCoverSheet = strCoverSheet.Replace("#INVOICETOTALVAT#", "£" & FormatNumber(dblTotalVat + dblTotalCreditVat, 2))
            strCoverSheet = strCoverSheet.Replace("#INVOICETOTALGROSS#", "£" & FormatNumber(dblTotalGross + dblTotalCreditGross, 2))

            strCoverSheet = strCoverSheet.Replace("#INVOICEDUE#", "£" & FormatNumber(dblTotalGross + dblTotalCreditGross, 2))

            ofiler3.Write(strCoverSheet)
            ofiler3.Flush()
            ofiler3.Close()

            If Not ConvertToPDF(strRtfFilePath, strRtfFilePath.Replace(".rtf", ".pdf"), False) Then
                Throw New Exception("Unable to convert file to PDF")
            End If

        Catch ex As Exception
            Return "Unable to create cover sheet file, please speak to development team"
        End Try

        Return strLines.ToString()

    End Function


    'R2.18 CR
    Private Shared Function ConvertToPDF(ByVal inFile As String, ByVal outFile As String, ByVal pLandscape As Boolean) As Boolean
        Try
            Dim p As New SautinSoft.PdfMetamorphosis()
            p.Serial = "10022099295"

            'specify Metamorphosis options
            If pLandscape Then
                p.PageStyle.PageOrientation.Landscape()
            Else
                p.PageStyle.PageOrientation.Portrait()
            End If

            'Convert RTF file to PDF file
            If p IsNot Nothing Then
                Dim rtfFile As String = inFile '"d:\test.rtf"
                Dim pdfFile As String = outFile '"d:\test.pdf"

                Dim result As Integer = p.RtfToPdfConvertFile(rtfFile, pdfFile)
            End If

            If (File.Exists(outFile)) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function createFileNPSA(ByVal pstartdate As String, ByVal penddate As String) As String

        Dim strCheckerLines As New StringBuilder
        Dim strLines As New StringBuilder

        Dim rs As List(Of Npsa)
        rs = Npsa.list(pstartdate, penddate)

        'first run through and check lengths
        For Each r As Npsa In rs
            If r.Batch_id.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - batch_id is too long(25)- " & r.Batch_id & vbCrLf)
            End If
            If r.Interface.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Interface is too long(25)- " & r.Interface & vbCrLf)
            End If
            If r.Voucher_type.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Voucher_type is too long(25)- " & r.Voucher_type & vbCrLf)
            End If
            If r.Trans_type.Length > 2 Then
                strCheckerLines.Append(r.Ext_ref & " - Trans_type is too long(25)- " & r.Trans_type & vbCrLf)
            End If
            If r.Client.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Client is too long(25)- " & r.Client & vbCrLf)
            End If
            If r.Account.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Account is too long(25)- " & r.Account & vbCrLf)
            End If
            If r.Dim_1.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Dim_1 is too long(25)- " & r.Dim_1 & vbCrLf)
            End If
            If r.Dim_2.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Dim_2 is too long(25)- " & r.Dim_2 & vbCrLf)
            End If
            If r.Dim_3.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Dim_3 is too long(25)- " & r.Dim_3 & vbCrLf)
            End If
            If r.Dim_4.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Dim_4 is too long(25)- " & r.Dim_4 & vbCrLf)
            End If
            If r.Dim_5.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Dim_5 is too long(25)- " & r.Dim_5 & vbCrLf)
            End If
            If r.Dim_6.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Dim_6 is too long(25)- " & r.Dim_6 & vbCrLf)
            End If
            If r.Dim_7.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Dim_7 is too long(25)- " & r.Dim_7 & vbCrLf)
            End If
            If r.Tax_code.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Tax_code is too long(25)- " & r.Tax_code & vbCrLf)
            End If
            If r.Tax_system.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Tax_system is too long(25)- " & r.Tax_system & vbCrLf)
            End If
            If r.Currency.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Currency is too long(25)- " & r.Currency & vbCrLf)
            End If
            If r.Dc_flag.Length > 2 Then
                strCheckerLines.Append(r.Ext_ref & " - Dc_flag is too long(2)- " & r.Dc_flag & vbCrLf)
            End If
            If r.Cur_amount.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Cur_amount is too long(20)- " & r.Cur_amount & vbCrLf)
            End If
            If r.Amount.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Amount is too long(20)- " & r.Amount & vbCrLf)
            End If
            If r.Number_1.Length > 11 Then
                strCheckerLines.Append(r.Ext_ref & " - Number_1 is too long(11)- " & r.Number_1 & vbCrLf)
            End If
            If r.Value_1.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Value_1 is too long(20)- " & r.Value_1 & vbCrLf)
            End If
            If r.Value_2.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Value_2 is too long(20)- " & r.Value_2 & vbCrLf)
            End If
            If r.Value_3.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Value_3 is too long(20)- " & r.Value_3 & vbCrLf)
            End If
            r.Descript = Pack(r.Descript)
            If r.Descript.Replace(vbCrLf, " ").Length > 255 Then
                strCheckerLines.Append(r.Ext_ref & " - Descript is too long(255)- " & r.Descript & vbCrLf)
            End If
            If r.Trans_date.Length > 8 Then
                strCheckerLines.Append(r.Ext_ref & " - Trans_date is too long(8)- " & r.Trans_date & vbCrLf)
            End If
            If r.Voucher_date.Length > 8 Then
                strCheckerLines.Append(r.Ext_ref & " - Voucher_date is too long(8)- " & r.Voucher_date & vbCrLf)
            End If
            If r.Voucher_no.Length > 15 Then
                strCheckerLines.Append(r.Ext_ref & " - Voucher_no is too long(15)- " & r.Voucher_no & vbCrLf)
            End If
            If r.Period.Length > 6 Then
                strCheckerLines.Append(r.Ext_ref & " - Period is too long(6)- " & r.Period & vbCrLf)
            End If
            If r.Tax_id.Length > 1 Then
                strCheckerLines.Append(r.Ext_ref & " - Tax_id is too long(1)- " & r.Tax_id & vbCrLf)
            End If
            If r.Ext_inv_ref.Length > 100 Then
                strCheckerLines.Append(r.Ext_ref & " - Ext_inv_ref is too long(100)- " & r.Ext_inv_ref & vbCrLf)
            End If
            If r.Ext_ref.Length > 255 Then
                strCheckerLines.Append(r.Ext_ref & " - Ext_ref is too long(255)- " & r.Ext_ref & vbCrLf)
            End If
            If r.Due_date.Length > 8 Then
                strCheckerLines.Append(r.Ext_ref & " - Due_date is too long(8)- " & r.Due_date & vbCrLf)
            End If
            If r.Disc_date.Length > 8 Then
                strCheckerLines.Append(r.Ext_ref & " - Disc_date is too long(8)- " & r.Disc_date & vbCrLf)
            End If
            If r.Discount.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Discount is too long(20)- " & r.Discount & vbCrLf)
            End If
            If r.Commitment.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Commitment is too long(25)- " & r.Commitment & vbCrLf)
            End If
            If r.Order_id.Length > 15 Then
                strCheckerLines.Append(r.Ext_ref & " - Order_id is too long(15)- " & r.Order_id & vbCrLf)
            End If
            If r.Kid.Length > 27 Then
                strCheckerLines.Append(r.Ext_ref & " - Kid is too long(27)- " & r.Kid & vbCrLf)
            End If
            If r.Pay_transfer.Length > 2 Then
                strCheckerLines.Append(r.Ext_ref & " - Pay_transfer is too long(2)- " & r.Pay_transfer & vbCrLf)
            End If
            If r.Status.Length > 1 Then
                strCheckerLines.Append(r.Ext_ref & " - Status is too long(1)- " & r.Status & vbCrLf)
            End If
            If r.Apar_type.Length > 1 Then
                strCheckerLines.Append(r.Ext_ref & " - Apar_type is too long(1)- " & r.Apar_type & vbCrLf)
            End If
            If r.Apar_id.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Apar_id is too long(25)- " & r.Apar_id & vbCrLf)
            End If
            If r.Pay_flag.Length > 1 Then
                strCheckerLines.Append(r.Ext_ref & " - Pay_flag is too long(1)- " & r.Pay_flag & vbCrLf)
            End If
            If r.Voucher_ref.Length > 15 Then
                strCheckerLines.Append(r.Ext_ref & " - Voucher_ref is too long(15)- " & r.Voucher_ref & vbCrLf)
            End If
            If r.Sequence_ref.Length > 9 Then
                strCheckerLines.Append(r.Ext_ref & " - Sequence_ref is too long(9)- " & r.Sequence_ref & vbCrLf)
            End If
            If r.Intrule_id.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Intrule_id is too long(25)- " & r.Intrule_id & vbCrLf)
            End If
            If r.Factor_short.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Factor_short is too long(25)- " & r.Factor_short & vbCrLf)
            End If
            If r.Responsible.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Responsible is too long(25)- " & r.Responsible & vbCrLf)
            End If
            If r.Apar_name.Length > 255 Then
                strCheckerLines.Append(r.Ext_ref & " - Apar_name is too long(255)- " & r.Apar_name & vbCrLf)
            End If
            If r.Address.Length > 160 Then
                strCheckerLines.Append(r.Ext_ref & " - Address is too long(160)- " & r.Address & vbCrLf)
            End If
            If r.Province.Length > 40 Then
                strCheckerLines.Append(r.Ext_ref & " - Province is too long(40)- " & r.Province & vbCrLf)
            End If
            If r.Place.Length > 40 Then
                strCheckerLines.Append(r.Ext_ref & " - Place is too long(40)- " & r.Place & vbCrLf)
            End If
            If r.Bank_account.Length > 35 Then
                strCheckerLines.Append(r.Ext_ref & " - Bank_account is too long(35)- " & r.Bank_account & vbCrLf)
            End If
            If r.Pay_method.Length > 2 Then
                strCheckerLines.Append(r.Ext_ref & " - Pay_method is too long(2)- " & r.Interface & vbCrLf)
            End If
            If r.Vat_reg_no.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Vat_reg_no is too long(25)- " & r.Vat_reg_no & vbCrLf)
            End If
            If r.Zip_code.Length > 15 Then
                strCheckerLines.Append(r.Ext_ref & " - Zip_code is too long(15)- " & r.Zip_code & vbCrLf)
            End If
            If r.Curr_licence.Length > 3 Then
                strCheckerLines.Append(r.Ext_ref & " - Curr_licence is too long(3)- " & r.Curr_licence & vbCrLf)
            End If
            If r.Account2.Length > 25 Then
                strCheckerLines.Append(r.Ext_ref & " - Account2 is too long(25)- " & r.Account2 & vbCrLf)
            End If
            If r.Base_amount.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Base_amount is too long(20)- " & r.Base_amount & vbCrLf)
            End If
            If r.Base_curr.Length > 20 Then
                strCheckerLines.Append(r.Ext_ref & " - Base_curr is too long(20)- " & r.Base_curr & vbCrLf)
            End If
            If r.Pay_temp_id.Length > 4 Then
                strCheckerLines.Append(r.Ext_ref & " - Pay_temp_id is too long(4)- " & r.Pay_temp_id & vbCrLf)
            End If
            If r.Allocation_key.Length > 3 Then
                strCheckerLines.Append(r.Ext_ref & " - Allocation_key is too long(3)- " & r.Allocation_key & vbCrLf)
            End If
            If r.Period_no.Length > 2 Then
                strCheckerLines.Append(r.Ext_ref & " - Period_no is too long(2)- " & r.Period_no & vbCrLf)
            End If
            If r.Clearing_code.Length > 13 Then
                strCheckerLines.Append(r.Ext_ref & " - Clearing_code is too long(13)- " & r.Clearing_code & vbCrLf)
            End If
            If r.Swift.Length > 11 Then
                strCheckerLines.Append(r.Ext_ref & " - Swift is too long(11)- " & r.Swift & vbCrLf)
            End If
            If r.Arrive_id.Length > 15 Then
                strCheckerLines.Append(r.Ext_ref & " - Arrive_id is too long(15)- " & r.Arrive_id & vbCrLf)
            End If
            If r.Bank_acc_type.Length > 2 Then
                strCheckerLines.Append(r.Ext_ref & " - Bank_acc_type is too long(2)- " & r.Bank_acc_type & vbCrLf)
            End If
            'now check Project code is good
            If Not checkNPSAProjectCode(r.Dim_2) Then
                strCheckerLines.Append(r.Ext_ref & " Incorrect Project code: - " & r.Dim_2 & vbCrLf)
            End If
        Next

        If strCheckerLines.Length > 0 Then
            Return strCheckerLines.ToString
        End If

        Dim dblTotalAmount As Double = 0
        Dim strBatchID As String = ""
        Dim strInterface As String = ""
        Dim strVoucherType As String = ""
        Dim strTransType As String = ""
        Dim strClient As String = ""
        Dim strTransDate As String = ""
        Dim strVoucherDate As String = ""
        Dim strExtInvRef As String = ""
        Dim strDueDate As String = ""
        Dim strDiscDate As String = ""
        Dim strLastDescription As String = ""
        Dim strCurrentDescription As String = ""
        Dim intcount As Integer = 0

        For Each r As Npsa In rs
            If intcount = 0 Then
                strBatchID = r.Batch_id
                strInterface = r.Interface
                strVoucherType = r.Voucher_type
                strTransType = r.Trans_type
                strClient = r.Client
                strTransDate = r.Trans_date
                strVoucherDate = r.Voucher_date
                strExtInvRef = r.Ext_inv_ref
                strDueDate = r.Due_date
                strDiscDate = r.Disc_date
                strLastDescription = r.Descript
            End If

            If r.Descript = "help" Then
                strCurrentDescription = Pack(strLastDescription)
            Else
                strCurrentDescription = Pack(r.Descript)
            End If

            strLines.Append(r.Batch_id.PadRight(25, " ")) '25
            strLines.Append(r.Interface.PadRight(25, " ")) '25
            strLines.Append(r.Voucher_type.PadRight(25, " ")) '25
            strLines.Append(r.Trans_type) '2
            strLines.Append(r.Client.PadRight(25, " ")) '25
            strLines.Append(r.Account.PadRight(25, " ")) '25
            strLines.Append(r.Dim_1.PadRight(25, " ")) '25
            strLines.Append(r.Dim_2.PadRight(25, " ")) '25
            strLines.Append(r.Dim_3.PadRight(25, " ")) '25
            strLines.Append(r.Dim_4.PadRight(25, " ")) '25
            strLines.Append(r.Dim_5.PadRight(25, " ")) '25
            strLines.Append(r.Dim_6.PadRight(25, " ")) '25
            strLines.Append(r.Dim_7.PadRight(25, " ")) '25
            strLines.Append(r.Tax_code.PadRight(25, " ")) '25
            strLines.Append(r.Tax_system.PadRight(25, " ")) '25
            strLines.Append(r.Currency.PadRight(25, " ")) '25
            strLines.Append(r.Dc_flag.PadLeft(2, " ")) '2 left
            strLines.Append(r.Cur_amount.Replace(".", "").PadLeft(20, " ")) '20 left
            strLines.Append(r.Amount.Replace(".", "").PadLeft(20, " ")) '20 left
            strLines.Append(r.Number_1.PadLeft(11, " ")) '11 left
            strLines.Append(r.Value_1.PadLeft(20, " ")) '20 left
            strLines.Append(r.Value_2.PadLeft(20, " ")) '20 left
            strLines.Append(r.Value_3.PadLeft(20, " ")) '20 left
            strLines.Append(strCurrentDescription.Replace(vbCrLf, " ").Replace(vbTab, " ").PadRight(255, " ")) '255
            strLines.Append(r.Trans_date.PadRight(8, " ")) '8
            strLines.Append(r.Voucher_date.PadRight(8, " ")) '8
            strLines.Append(r.Voucher_no.PadLeft(15, " ")) '15 left
            strLines.Append(r.Period.PadLeft(6, " ")) '6 left
            strLines.Append(r.Tax_id.PadRight(1, " ")) '1
            strLines.Append(r.Ext_inv_ref.PadRight(100, " ")) '100
            strLines.Append(r.Ext_ref.PadRight(255, " ")) '255
            strLines.Append(r.Due_date.PadRight(8, " ")) '8
            strLines.Append(r.Disc_date.PadRight(8, " ")) '8
            strLines.Append(r.Discount.PadLeft(20, " ")) '20 left
            strLines.Append(r.Commitment.PadRight(25, " ")) '25
            strLines.Append(r.Order_id.PadLeft(15, " ")) '15 left
            strLines.Append(r.Kid.PadRight(27, " ")) '27
            strLines.Append(r.Pay_transfer.PadRight(2, " ")) '2
            strLines.Append(r.Status) '1
            strLines.Append(r.Apar_type) '1
            strLines.Append(r.Apar_id.PadRight(25, " ")) '25
            strLines.Append(r.Pay_flag) '1
            strLines.Append(r.Voucher_ref.PadLeft(15, " ")) '15 left
            strLines.Append(r.Sequence_ref.PadLeft(9, " ")) '9 left
            strLines.Append(r.Intrule_id.PadRight(25, " ")) '25
            strLines.Append(r.Factor_short.PadRight(25, " ")) '25
            strLines.Append(r.Responsible.PadRight(25, " ")) '25
            strLines.Append(r.Apar_name.PadRight(255, " ")) '255
            strLines.Append(r.Address.PadRight(160, " ")) '160
            strLines.Append(r.Province.PadRight(40, " ")) '40
            strLines.Append(r.Place.PadRight(40, " ")) '40
            strLines.Append(r.Bank_account.PadRight(35, " ")) '35
            strLines.Append(r.Pay_method.PadRight(2, " ")) '2
            strLines.Append(r.Vat_reg_no.PadRight(25, " ")) '25
            strLines.Append(r.Zip_code.PadRight(15, " ")) '15
            strLines.Append(r.Curr_licence.PadRight(3, " ")) '3
            strLines.Append(r.Account2.PadRight(25, " ")) '25
            strLines.Append(r.Base_amount.PadLeft(20, " ")) '20 left
            strLines.Append(r.Base_curr.PadLeft(20, " ")) '20 left
            strLines.Append(r.Pay_temp_id.PadRight(4, " ")) '4
            strLines.Append(r.Allocation_key.PadLeft(3, " ")) '3 left
            strLines.Append(r.Period_no.PadLeft(2, " ")) '2 left
            strLines.Append(r.Clearing_code.PadRight(13, " ")) '13
            strLines.Append(r.Swift.PadRight(11, " ")) '11
            strLines.Append(r.Arrive_id.PadLeft(15, " ")) '15 left
            strLines.Append(r.Bank_acc_type.PadRight(2, " ")) '2
            strLines.Append(vbCrLf)
            intcount += 1
            dblTotalAmount = dblTotalAmount + CDbl(r.Cur_amount)
            If r.Descript <> "help" Then
                strLastDescription = r.Descript
            End If
        Next

        'now add final line
        Dim strBlank As String = " "
        Dim strZero As String = "0"
        Dim strDescription As String = "Invoice " & Format(Now, "ddMMMyy")

        strLines.Append(strBatchID.PadRight(25, " ")) '25
        strLines.Append(strInterface.PadRight(25, " ")) '25
        strLines.Append(strVoucherType.PadRight(25, " ")) '25
        strLines.Append("AP") '2
        strLines.Append(strClient.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank & "0".PadRight(24, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append("GBP".PadRight(25, " ")) '25
        strLines.Append("-1".PadLeft(2, " ")) '2 left
        strLines.Append(CStr(-dblTotalAmount).Replace(".", "").PadLeft(20, " ")) '20 left
        strLines.Append(CStr(-dblTotalAmount).Replace(".", "").PadLeft(20, " ")) '20 left
        strLines.Append(strZero.PadLeft(11, " ")) '11 left
        strLines.Append(strZero.PadLeft(20, " ")) '20 left
        strLines.Append(strZero.PadLeft(20, " ")) '20 left
        strLines.Append(strZero.PadLeft(20, " ")) '20 left
        strLines.Append(strDescription.PadRight(255, " ")) '255
        strLines.Append(strTransDate.PadRight(8, " ")) '8
        strLines.Append(strVoucherDate.PadRight(8, " ")) '8
        strLines.Append(strZero.PadLeft(15, " ")) '15 left
        strLines.Append(strZero.PadLeft(6, " ")) '6 left
        strLines.Append(" ") '1
        strLines.Append(strExtInvRef.PadRight(100, " ")) '100
        strLines.Append(strExtInvRef.PadRight(255, " ")) '255
        strLines.Append(strDueDate.PadRight(8, " ")) '8
        strLines.Append(strDiscDate.PadRight(8, " ")) '8
        strLines.Append(strZero.PadLeft(20, " ")) '20 left
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strZero.PadLeft(15, " ")) '15 left
        strLines.Append(strBlank.PadRight(27, " ")) '27
        strLines.Append(strBlank.PadRight(2, " ")) '2
        strLines.Append("N") '1
        strLines.Append("P") '1
        strLines.Append("25720".PadRight(25, " ")) '25
        strLines.Append(strZero) '1
        strLines.Append(strZero.PadLeft(15, " ")) '15 left
        strLines.Append(strZero.PadLeft(9, " ")) '9 left
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append("VANPER".PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(255, " ")) '255
        strLines.Append(strBlank.PadRight(160, " ")) '160
        strLines.Append(strBlank.PadRight(40, " ")) '40
        strLines.Append(strBlank.PadRight(40, " ")) '40
        strLines.Append(strBlank.PadRight(35, " ")) '35
        strLines.Append(strBlank.PadRight(2, " ")) '2
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strBlank.PadRight(15, " ")) '15
        strLines.Append(strBlank.PadRight(3, " ")) '3
        strLines.Append(strBlank.PadRight(25, " ")) '25
        strLines.Append(strZero.PadLeft(20, " ")) '20 left
        strLines.Append(strZero.PadLeft(20, " ")) '20 left
        strLines.Append(strBlank.PadRight(4, " ")) '4
        strLines.Append(strZero.PadLeft(3, " ")) '3 left
        strLines.Append(strZero.PadLeft(2, " ")) '2 left
        strLines.Append(strBlank.PadRight(13, " ")) '13
        strLines.Append(strBlank.PadRight(11, " ")) '11
        strLines.Append(strZero.PadLeft(15, " ")) '15 left
        strLines.Append(strBlank.PadRight(2, " ")) '2
        strLines.Append(vbCrLf)

        Return strLines.ToString()

    End Function

    Public Shared Function createFileDWP(ByVal pstartdate As String, ByVal penddate As String, ByVal pstrType As String) As String

        'check to remove duplicates

        'first lets run checker to ensure each invoice total matches the total of the line values
        Dim strCheckerLines As New StringBuilder
        Dim strCheckerCCLines As New StringBuilder
        Dim oCheckers As List(Of DWP)
        oCheckers = DWP.checker(pstartdate, penddate, pstrType)
        For Each oChecker As DWP In oCheckers
            strCheckerLines.Append(oChecker.Invoicenumber & vbCrLf)
        Next

        'now check CCs
        Dim oCheckersCC As List(Of DWP)
        oCheckersCC = DWP.checkerCC(pstartdate, penddate, pstrType)

        For Each oCheckerCC As DWP In oCheckersCC
            strCheckerCCLines.Append(oCheckerCC.Invoicenumber & " - " & oCheckerCC.Result & vbCrLf)
        Next

        Dim rs As List(Of DWP)
        rs = DWP.list(pstartdate, penddate, pstrType)

        'now check VAT
        'now run through to check for incorrect VAT amounts
        Dim strVATErrors As New StringBuilder
        Dim strBUnitErrors As New StringBuilder

        For Each r As DWP In rs
            If r.Invoicenumber = "N579616" Then
                Dim istop As Integer = 0
            End If
            Dim localVExpense As Double = CDbl(r.Expense)
            Dim localVExpenseVat As Double = CDbl(r.Expensevat)
            Dim localVService As Double = CDbl(r.Servicecharge)
            Dim localVServiceVat As Double = 0
            Dim localVOk As Boolean = True
            'lets check the vat !!!!
            If CDbl(r.Expense) > 0 And CDbl(r.Expensevat) > 0 And CDbl(r.Servicecharge) > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localVExpense = Ret.pexpense
                localVExpenseVat = Ret.pexpensevat
                localVService = Ret.pservice
                localVServiceVat = Ret.pservicevat
                localVOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localVExpenseVat > 0 Then
                    If localVExpense = 0 Then
                        If localVService = 0 Then
                            localVOk = False
                        Else
                            localVServiceVat = localVExpenseVat
                            localVExpenseVat = 0
                        End If
                    End If
                End If
            End If
            If localVOk Then

                'R2.5 CR - check VAT rate
                Dim dblVAT As Double = 0
                If CDate(r.Inm_end) >= CDate("04/01/2011") Then
                    dblVAT = 0.2
                Else
                    dblVAT = 0.175
                End If
                Dim strDWPDodgyinvoices As String = getConfig("DWPInvoiceIDs")
                If strDWPDodgyinvoices <> "" Then
                    If strDWPDodgyinvoices.Contains(r.Invoicenumber) Then
                        dblVAT = 0.2
                    End If
                End If

                If Not testVat(localVExpense, localVExpenseVat, localVService, localVServiceVat, CDbl(r.Othercharge), CDbl(r.Othervat), dblVAT) Then
                    strVATErrors.Append(r.Invoicenumber & vbCrLf)
                End If
            Else
                strVATErrors.Append(r.Invoicenumber & " line: " & r.Linenumber & vbCrLf)
            End If

            If r.Company = "04" Or r.Company = "4" Then
                Try
                    Dim inttest As Integer = CInt(r.Tot_cref1)
                    'R2.17 CR - DWP Bug Fix: new cost centre ranges
                    'If inttest < 100000 Or inttest > 114999 Then
                    '    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & CStr(inttest) & " should be in the range 100000 to 114999 for Business Unit:" & r.Company & vbCrLf)
                    'End If
                    If inttest < 170000 Or inttest > 189999 Then
                        strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & CStr(inttest) & " should be in the range 170000 to 189999 for Business Unit:" & r.Company & vbCrLf)
                    End If
                Catch ex As Exception
                    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & r.Tot_cref1 & " is not a number" & vbCrLf)
                End Try
            ElseIf r.Company = "07" Or r.Company = "7" Then
                Try
                    'R2.17 CR - DWP Bug Fix: new cost centre ranges
                    'Dim inttest As Integer = CInt(r.Tot_cref1)
                    'If inttest < 116800 Or inttest > 117998 Then
                    '    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & CStr(inttest) & " should be in the range 116800 to 117998 for Business Unit:" & r.Company & vbCrLf)
                    'End If
                    strBUnitErrors.Append(r.Invoicenumber & " - Business Unit:" & r.Company & " - nothing should now be billed against this business unit." & vbCrLf)
                Catch ex As Exception
                    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & r.Tot_cref1 & " is not a number" & vbCrLf)
                End Try
            ElseIf r.Company = "20" Then
                Try
                    Dim inttest As Integer = CInt(r.Tot_cref1)
                    If inttest < 120000 Or inttest > 139999 Then
                        strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & CStr(inttest) & " should be in the range 120000 to 139999 for Business Unit:" & r.Company & vbCrLf)
                    End If
                Catch ex As Exception
                    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & r.Tot_cref1 & " is not a number" & vbCrLf)
                End Try
            ElseIf r.Company = "09" Or r.Company = "9" Then
                Try
                    'R2.17 CR - DWP Bug Fix: new cost centre ranges
                    'Dim inttest As Integer = CInt(r.Tot_cref1)
                    'If inttest < 140000 Or inttest > 159999 Then
                    '    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & CStr(inttest) & " should be in the range 140000 to 159999 for Business Unit:" & r.Company & vbCrLf)
                    'End If
                    strBUnitErrors.Append(r.Invoicenumber & " - Business Unit:" & r.Company & " - nothing should now be billed against this business unit." & vbCrLf)
                Catch ex As Exception
                    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & r.Tot_cref1 & " is not a number" & vbCrLf)
                End Try

                'R2.17 CR - DWP Bug Fix: new cost centre ranges
            ElseIf r.Company = "05" Or r.Company = "5" Then
                Try
                    Dim inttest As Integer = CInt(r.Tot_cref1)
                    If inttest < 198000 Or inttest > 199999 Then
                        strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & CStr(inttest) & " should be in the range 198000 to 199999 for Business Unit:" & r.Company & vbCrLf)
                    End If
                Catch ex As Exception
                    strBUnitErrors.Append(r.Invoicenumber & " - Cost centre:" & r.Tot_cref1 & " is not a number" & vbCrLf)
                End Try

            Else
                strBUnitErrors.Append(r.Invoicenumber & " - Business unit:" & r.Company & " is incorrect" & vbCrLf)
            End If
        Next

        Dim strRet As String = ""
        If strCheckerLines.ToString <> "" Then
            strRet = "THE FOLLOWING INVOICE TOTAL/S DOES NOT MATCH THE TOTAL OF THE INVOICE LINES:" & vbCrLf & strCheckerLines.ToString
        End If

        If strCheckerCCLines.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING INVOICE/S HAVE INCORRECT/MISSING COST CENTRES:" & vbCrLf & strCheckerCCLines.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING INVOICE/S HAVE INCORRECT/MISSING COST CENTRES:" & vbCrLf & strCheckerCCLines.ToString
            End If
        End If

        If strVATErrors.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING INVOICE/S HAVE INCORRECT VAT AMOUNTS:" & vbCrLf & strVATErrors.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING INVOICE/S HAVE INCORRECT VAT AMOUNTS:" & vbCrLf & strVATErrors.ToString
            End If
        End If

        If strBUnitErrors.ToString <> "" Then
            If strRet = "" Then
                strRet = "THE FOLLOWING HAVE INCORRECT COST CENTRES/BUSINESS UNITS:" & vbCrLf & strBUnitErrors.ToString
            Else
                strRet = strRet & vbCrLf & vbCrLf & "THE FOLLOWING HAVE INCORRECT COST CENTRES/BUSINESS UNITS:" & vbCrLf & strBUnitErrors.ToString
            End If
        End If

        If strRet <> "" Then
            Return strRet
        End If

        'now do it for real
        Dim intBachNo As Integer = BatchNoGet("DWP")
        Dim strBatchID As String = "NYSCorporate" & intBachNo.ToString.PadLeft(8, CChar("0"))

        Dim strBatch As New StringBuilder
        strBatch.Append("B|" & strBatchID & "|" & Format(Now, "dd-MMM-yyyy") & vbCrLf)

        Dim strInvoiceHeaders As New StringBuilder
        ' Dim strInvoiceLines As New StringBuilder
        Dim strErrorLines As New StringBuilder

        Dim intInvoiceHeaderCount As Integer = 0
        Dim intInvoiceLineCount As Integer = 0
        Dim dblInvoiceHeaderTotal As Double = 0

        Dim intLineNo As Integer = 1
        Dim nextInvoiceNumber As String = ""
        Dim lastInvoiceNumber As String = ""

        'now for final run through if all else is OK
        Dim invoiceAMOUNTER As Double = 0
        Dim invoiceTOTALER As Double = 0
        Dim invoiceTOTALER2 As Double = 0
        Dim headersTotal As Double = 0
        Dim linesTotal As Double = 0
        Dim lines2Total As Double = 0
        Dim linegroupnumber As Integer = 0
        Dim strSuppliernumber As String = ""
        Dim strCompany As String = ""
        Dim strTotCref1 As String = ""
        Dim dblTotDiscnt As Double = 0
        Dim strMeetingcode As String = ""
        Dim dblInvoiceamount As Double = 0

        For Each r As DWP In rs
            nextInvoiceNumber = r.Invoicenumber
            If r.Invoicenumber = "N579616" Then
                Dim IStop As String = ""
            End If
            If nextInvoiceNumber <> lastInvoiceNumber Then
                If lastInvoiceNumber <> "" And dblTotDiscnt <> 0 Then
                    strInvoiceHeaders.Append("L|")
                    strInvoiceHeaders.Append(strBatchID & "|")
                    strInvoiceHeaders.Append(strSuppliernumber.Trim & "|")
                    strInvoiceHeaders.Append(lastInvoiceNumber.Trim & "|")
                    If dblInvoiceamount < 0 Then
                        strInvoiceHeaders.Append(Math.Abs(dblTotDiscnt) & "|")
                    Else
                        strInvoiceHeaders.Append(-Math.Abs(dblTotDiscnt) & "|")
                    End If
                    strInvoiceHeaders.Append("ZERO" & "|")
                    strInvoiceHeaders.Append("Discount amount for invoice||||||||")
                    strInvoiceHeaders.Append(strCompany & "|")
                    strInvoiceHeaders.Append(strTotCref1 & "|")
                    If pstrType.ToUpper = "DWP" Then
                        'R1.1 NM
                        If strMeetingcode = "35016" Then
                            strInvoiceHeaders.Append(strMeetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Outreach|||||||||" & vbCrLf)
                        Else
                            strInvoiceHeaders.Append(strMeetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Mgmt Conferences|||||||||" & vbCrLf)
                        End If
                    Else
                        strInvoiceHeaders.Append(strMeetingcode & "|00000000|000|||||||||||||||" & vbCrLf)
                    End If
                    intInvoiceLineCount += 1
                End If
                intLineNo = 1
                'R1 NM
                linegroupnumber = 0

            End If
            If r.Invoicenumber = "N436271" Then
                Dim uTest As Integer = 0
            End If
            Dim localExpense As Double = CDbl(r.Expense)
            Dim localExpenseVat As Double = CDbl(r.Expensevat)
            Dim localService As Double = CDbl(r.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            'lets check the vat !!!!
            If CDbl(r.Expense) > 0 And CDbl(r.Expensevat) > 0 And CDbl(r.Servicecharge) > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localService = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat <> 0 Then
                    If localExpense = 0 Then
                        localServiceVat = localExpenseVat
                        localExpenseVat = 0
                    End If
                End If
            End If

            'add sanity check for minus figures for airline charges as NYS don't want to show cancel charges for air tickets
            If Not localOk Then
                strErrorLines.Append(r.Invoicenumber & vbCrLf)
            End If

            r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " "))
            If nextInvoiceNumber <> lastInvoiceNumber Then ' as first line may have been paid direct so wouldn't exist!
                'If CDbl(r.Linenumber) = 1 Then 'first line so add header details first
                strInvoiceHeaders.Append("H|")
                strInvoiceHeaders.Append(strBatchID & "|")
                strInvoiceHeaders.Append(r.Invoicenumber.Trim & "|")
                strInvoiceHeaders.Append(Format(CDbl(r.Invoiceamount), "0.00") & "|")
                strInvoiceHeaders.Append(Format(CDate(r.Invoicedate), "dd-MMM-yyyy") & "|Travel services|")
                strInvoiceHeaders.Append(r.Suppliernumber.Trim & "|")
                strInvoiceHeaders.Append(r.Suppliersitecode.Trim & vbCrLf)
                intInvoiceHeaderCount += 1
                'do some adding up to compare later
                invoiceAMOUNTER = CDbl(r.Invoiceamount)
                headersTotal = headersTotal + CDbl(r.Invoiceamount)
                dblInvoiceHeaderTotal = CDbl(dblInvoiceHeaderTotal + CDbl(r.Invoiceamount))
            End If

            If localExpense <> 0 And localExpenseVat <> 0 Then
                strInvoiceHeaders.Append("L|")
                strInvoiceHeaders.Append(strBatchID & "|")
                strInvoiceHeaders.Append(r.Suppliernumber.Trim & "|")
                strInvoiceHeaders.Append(r.Invoicenumber.Trim & "|")
                'strInvoiceHeaders.Append(Format(localExpense - CDbl(r.Inm_discnt), "0.00") & "|")
                strInvoiceHeaders.Append(Format(localExpense, "0.00") & "|")
                strInvoiceHeaders.Append("STD" & "|")
                strInvoiceHeaders.Append(r.Tot_fileno & "-")
                strInvoiceHeaders.Append(r.Inm_supnam & "-")
                strInvoiceHeaders.Append(r.Sup_add3 & "--")
                strInvoiceHeaders.Append(Format(CDate(r.Inm_start), "dd-MMM-yyyy") & "-")
                strInvoiceHeaders.Append(r.Inm_ldname & "-")
                strInvoiceHeaders.Append(r.Tot_cref2 & "-")
                strInvoiceHeaders.Append(r.Linedescription & "||||||||")
                strInvoiceHeaders.Append(r.Company & "|")
                strInvoiceHeaders.Append(r.Tot_cref1 & "|")
                If pstrType.ToUpper = "DWP" Then
                    'R1.1 NM
                    If r.Meetingcode = "35016" Then
                        strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Outreach|||||||||" & vbCrLf)
                    Else
                        strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Mgmt Conferences|||||||||" & vbCrLf)
                    End If
                Else
                    strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000|||||||||||||||" & vbCrLf)
                End If
                intInvoiceLineCount += 1
            ElseIf localExpense <> 0 And localExpenseVat = 0 Then
                strInvoiceHeaders.Append("L|")
                strInvoiceHeaders.Append(strBatchID & "|")
                strInvoiceHeaders.Append(r.Suppliernumber.Trim & "|")
                strInvoiceHeaders.Append(r.Invoicenumber.Trim & "|")
                'strInvoiceHeaders.Append(Format(localExpense - CDbl(r.Inm_discnt), "0.00") & "|")
                strInvoiceHeaders.Append(Format(localExpense, "0.00") & "|")
                strInvoiceHeaders.Append("ZERO" & "|")
                strInvoiceHeaders.Append(r.Tot_fileno & "-")
                strInvoiceHeaders.Append(r.Inm_supnam & "-")
                strInvoiceHeaders.Append(r.Sup_add3 & "--")
                strInvoiceHeaders.Append(r.Inm_start & "-")
                strInvoiceHeaders.Append(r.Inm_ldname & "-")
                strInvoiceHeaders.Append(r.Tot_cref2 & "-")
                strInvoiceHeaders.Append(r.Linedescription & "||||||||")
                strInvoiceHeaders.Append(r.Company & "|")
                strInvoiceHeaders.Append(r.Tot_cref1 & "|")
                If pstrType.ToUpper = "DWP" Then
                    'R1.1 NM
                    If r.Meetingcode = "35016" Then
                        strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Outreach|||||||||" & vbCrLf)
                    Else
                        strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Mgmt Conferences|||||||||" & vbCrLf)
                    End If
                Else
                    strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000|||||||||||||||" & vbCrLf)
                End If
                intInvoiceLineCount += 1
            ElseIf localExpense = 0 And localExpenseVat = 0 Then
                'not needed anymore
                If CDbl(r.Othercharge) <> 0 And CDbl(r.Othervat) <> 0 Then
                    strInvoiceHeaders.Append("L|")
                    strInvoiceHeaders.Append(strBatchID & "|")
                    strInvoiceHeaders.Append(r.Suppliernumber.Trim & "|")
                    strInvoiceHeaders.Append(r.Invoicenumber.Trim & "|")
                    'strInvoiceHeaders.Append(Format(localExpense - CDbl(r.Inm_discnt), "0.00") & "|")
                    strInvoiceHeaders.Append(Format(CDbl(r.Othercharge), "0.00") & "|")
                    strInvoiceHeaders.Append("STD" & "|")
                    strInvoiceHeaders.Append(r.Tot_fileno & "-")
                    strInvoiceHeaders.Append(r.Inm_supnam & "-")
                    strInvoiceHeaders.Append(r.Sup_add3 & "--")
                    strInvoiceHeaders.Append(Format(CDate(r.Inm_start), "dd-MMM-yyyy") & "-")
                    strInvoiceHeaders.Append(r.Inm_ldname & "-")
                    strInvoiceHeaders.Append(r.Tot_cref2 & "-")
                    strInvoiceHeaders.Append(r.Linedescription & "||||||||")
                    strInvoiceHeaders.Append(r.Company & "|")
                    strInvoiceHeaders.Append(r.Tot_cref1 & "|")
                    If pstrType.ToUpper = "DWP" Then
                        'R1.1 NM
                        If r.Meetingcode = "35016" Then
                            strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Outreach|||||||||" & vbCrLf)
                        Else
                            strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Mgmt Conferences|||||||||" & vbCrLf)
                        End If
                    Else
                        strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000|||||||||||||||" & vbCrLf)
                    End If
                    intInvoiceLineCount += 1
                ElseIf CDbl(r.Othercharge) <> 0 And CDbl(r.Othervat) = 0 Then
                    strInvoiceHeaders.Append("L|")
                    strInvoiceHeaders.Append(strBatchID & "|")
                    strInvoiceHeaders.Append(r.Suppliernumber.Trim & "|")
                    strInvoiceHeaders.Append(r.Invoicenumber.Trim & "|")
                    'strInvoiceHeaders.Append(Format(localExpense - CDbl(r.Inm_discnt), "0.00") & "|")
                    strInvoiceHeaders.Append(Format(CDbl(r.Othercharge), "0.00") & "|")
                    strInvoiceHeaders.Append("ZERO" & "|")
                    strInvoiceHeaders.Append(r.Tot_fileno & "-")
                    strInvoiceHeaders.Append(r.Inm_supnam & "-")
                    strInvoiceHeaders.Append(r.Sup_add3 & "--")
                    strInvoiceHeaders.Append(r.Inm_start & "-")
                    strInvoiceHeaders.Append(r.Inm_ldname & "-")
                    strInvoiceHeaders.Append(r.Tot_cref2 & "-")
                    strInvoiceHeaders.Append(r.Linedescription & "||||||||")
                    strInvoiceHeaders.Append(r.Company & "|")
                    strInvoiceHeaders.Append(r.Tot_cref1 & "|")
                    If pstrType.ToUpper = "DWP" Then
                        'R1.1 NM
                        If r.Meetingcode = "35016" Then
                            strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Outreach|||||||||" & vbCrLf)
                        Else
                            strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Mgmt Conferences|||||||||" & vbCrLf)
                        End If
                    Else
                        strInvoiceHeaders.Append(r.Meetingcode & "|00000000|000|||||||||||||||" & vbCrLf)
                    End If
                    intInvoiceLineCount += 1
                End If
            Else
                Dim inn As Integer = 1
            End If
            strSuppliernumber = r.Suppliernumber
            strCompany = r.Company
            strTotCref1 = r.Tot_cref1
            lastInvoiceNumber = r.Invoicenumber
            dblTotDiscnt = r.Tot_discnt
            strMeetingcode = r.Meetingcode
            dblInvoiceamount = r.Invoiceamount
        Next

        'need to check if last invoice has discount so can add line
        If lastInvoiceNumber <> "" And dblTotDiscnt <> 0 Then
            strInvoiceHeaders.Append("L|")
            strInvoiceHeaders.Append(strBatchID & "|")
            strInvoiceHeaders.Append(strSuppliernumber.Trim & "|")
            strInvoiceHeaders.Append(lastInvoiceNumber.Trim & "|")
            If dblInvoiceamount < 0 Then
                strInvoiceHeaders.Append(Math.Abs(dblTotDiscnt) & "|")
            Else
                strInvoiceHeaders.Append(-Math.Abs(dblTotDiscnt) & "|")
            End If
            strInvoiceHeaders.Append("ZERO" & "|")
            strInvoiceHeaders.Append("Discount amount for invoice||||||||")
            strInvoiceHeaders.Append(strCompany & "|")
            strInvoiceHeaders.Append(strTotCref1 & "|")
            If pstrType.ToUpper = "DWP" Then
                'R1.1 NM
                If strMeetingcode = "35016" Then
                    strInvoiceHeaders.Append(strMeetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Outreach|||||||||" & vbCrLf)
                Else
                    strInvoiceHeaders.Append(strMeetingcode & "|00000000|000||||||Travel & Events.Events Organisation.Venue Hire Mgmt Conferences|||||||||" & vbCrLf)
                End If
            Else
                strInvoiceHeaders.Append(strMeetingcode & "|00000000|000|||||||||||||||" & vbCrLf)
            End If
            intInvoiceLineCount += 1
        End If

        Dim strInvoiceTrailer As New StringBuilder
        strInvoiceTrailer.Append("T|" & strBatchID & "|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|" & Format(dblInvoiceHeaderTotal, "0.00"))

        strBatch.Append(strInvoiceHeaders.ToString)
        strBatch.Append(strInvoiceTrailer.ToString)

        If strErrorLines.ToString <> "" Then
            strErrorLines.Append("The above Invoice/s have incorrect VAT values for the fare and/or service charges")
            Return strErrorLines.ToString
        Else
            Return strBatch.ToString
        End If

    End Function

    Public Structure getCorrectVat
        Public pexpense, pexpensevat, pservice, pservicevat As Double
        Public ok As Boolean
    End Structure

    Public Shared Function checkVAT(ByVal expense As Double, ByVal expensevat As Double, ByVal servicecharge As Double) As getCorrectVat

        Dim ret As New getCorrectVat
        Dim dblAll20 As Double = Math.Round((expense + servicecharge) * 0.2, 2)
        Dim dblAll175 As Double = Math.Round((expense + servicecharge) * 0.175, 2)
        Dim dblAll15 As Double = Math.Round((expense + servicecharge) * 0.15, 2)

        Dim dblExp20 As Double = Math.Round(expense * 0.2, 2)
        Dim dblExp175 As Double = Math.Round(expense * 0.175, 2)
        Dim dblExp15 As Double = Math.Round(expense * 0.15, 2)

        Dim dblSer20 As Double = Math.Round(servicecharge * 0.2, 2)
        Dim dblSer175 As Double = Math.Round(servicecharge * 0.175, 2)
        Dim dblSer15 As Double = Math.Round(servicecharge * 0.15, 2)

        ret.ok = True
        If Math.Round(expensevat, 2) = dblAll175 Then
            'need to split vat @ 17.5%
            ret.pexpense = expense
            ret.pexpensevat = expense * 0.175
            ret.pservice = servicecharge
            ret.pservicevat = servicecharge * 0.175
            If ret.pexpensevat + ret.pservicevat <> expensevat Then
                Dim dbldiff As Double = (CDbl(ret.pexpensevat + ret.pservicevat)) - CDbl(expensevat)
                dbldiff = Math.Abs(dbldiff)
                ret.pexpensevat = Math.Round(ret.pexpensevat, 2)
                ret.pservicevat = expensevat - ret.pexpensevat
            End If
        ElseIf expensevat = dblAll15 Then
            'need to split vat @ 15%
            ret.pexpense = expense
            ret.pexpensevat = expense * 0.15
            ret.pservice = servicecharge
            ret.pservicevat = servicecharge * 0.15
            If ret.pexpensevat + ret.pservicevat <> expensevat Then
                Dim dbldiff As Double = (CDbl(ret.pexpensevat + ret.pservicevat)) - CDbl(expensevat)
                dbldiff = Math.Abs(dbldiff)
                ret.pexpensevat = Math.Round(ret.pexpensevat, 2)
                ret.pservicevat = expensevat - ret.pexpensevat
            End If
        ElseIf expensevat = dblAll20 Then
            'need to split vat @ 20%
            ret.pexpense = expense
            ret.pexpensevat = expense * 0.2
            ret.pservice = servicecharge
            ret.pservicevat = servicecharge * 0.2
            If ret.pexpensevat + ret.pservicevat <> expensevat Then
                Dim dbldiff As Double = (CDbl(ret.pexpensevat + ret.pservicevat)) - CDbl(expensevat)
                dbldiff = Math.Abs(dbldiff)
                ret.pexpensevat = Math.Round(ret.pexpensevat, 2)
                ret.pservicevat = expensevat - ret.pexpensevat
            End If
        ElseIf Math.Round(expensevat, 2) = dblExp175 Then
            'all vat belongs to expense @ 17.5%
            ret.pexpense = expense
            ret.pexpensevat = expensevat
            ret.pservice = servicecharge
            ret.pservicevat = 0
        ElseIf Math.Round(expensevat, 2) = dblExp15 Then
            'all vat belongs to expense @ 15%
            ret.pexpense = expense
            ret.pexpensevat = expensevat
            ret.pservice = servicecharge
            ret.pservicevat = 0
        ElseIf Math.Round(expensevat, 2) = dblExp20 Then
            'all vat belongs to expense @ 20%
            ret.pexpense = expense
            ret.pexpensevat = expensevat
            ret.pservice = servicecharge
            ret.pservicevat = 0
        ElseIf Math.Round(expensevat, 2) = dblSer175 Then
            'all vat belongs to service @ 17.5%
            ret.pexpense = expense
            ret.pexpensevat = 0
            ret.pservice = servicecharge
            ret.pservicevat = expensevat
        ElseIf Math.Round(expensevat, 2) = dblSer15 Then
            'all vat belongs to service @ 15%
            ret.pexpense = expense
            ret.pexpensevat = 0
            ret.pservice = servicecharge
            ret.pservicevat = expensevat
        ElseIf Math.Round(expensevat, 2) = dblSer20 Then
            'all vat belongs to service @ 20%
            ret.pexpense = expense
            ret.pexpensevat = 0
            ret.pservice = servicecharge
            ret.pservicevat = expensevat
        Else
            'oh no we have a problem!
            ret.ok = False
        End If
        'sanity checks
        If Math.Round(expense + expensevat + servicecharge, 2) <> Math.Round(ret.pexpense + ret.pexpensevat + ret.pservice + ret.pservicevat, 2) Then
            ret.ok = False
        End If
        Return ret
    End Function

    'CR
    Public Function createFileNHSBSA(ByVal pstartdate As String, ByVal penddate As String) As String
        'check to remove duplicates


        'sends details to screen, shows errors too
        Dim strRet As String = createFileNHSBSAToView(pstartdate, penddate)
        If strRet.StartsWith("ERROR") Then
            Return strRet
        End If

        ' get list of distinct Invoice numbers
        Dim rs As List(Of NhsBsa)
        rs = NhsBsa.listIndividual(pstartdate, penddate)
        'now cycle through and create individual files
        For Each r As NhsBsa In rs
            createPhysicalFileNHSBSA(r.Invoicenumber)
        Next

        Return strRet

    End Function

    'CR
    Public Shared Function createFileNHSBSAToView(ByVal pstartdate As String, ByVal penddate As String) As String

        'go get details of invoices here
        Dim strBatch As New StringBuilder
        Dim strBatchHeaderX As New StringBuilder
        Dim strBatchFooter As New StringBuilder
        Dim strErrorLines As New StringBuilder
        Dim strPOErrors As New StringBuilder

        Dim intLineNo As Integer = 1

        'do loop here
        Dim rs As List(Of NhsBsa)
        rs = NhsBsa.list(pstartdate, penddate)

        Dim nextInvoiceNumber As String = ""
        Dim lastInvoiceNumber As String = ""
        Dim strLineDescription As String = ""

        Dim totalnett As Double = 0
        Dim totalvat As Double = 0
        Dim dblInvoiceHeaderTotal As Double = 0
        Dim dblInvoiceHeaderTotal2 As Double = 0
        Dim intInvoiceHeaderCount As Integer = 0
        Dim intInvoiceLineCount As Integer = 0

        If strPOErrors.ToString <> "" Then
            Return "ERROR - THE FOLLOWING INVOICES HAVE INCORRECT PROJECT CODES:" & vbCrLf & strPOErrors.ToString
        End If

        For Each r As NhsBsa In rs
            nextInvoiceNumber = r.Invoicenumber
            Dim localExpense As Double = CDbl(r.Expense)
            Dim localExpenseVat As Double = CDbl(r.Expensevat)
            Dim localService As Double = CDbl(r.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            Dim localDiscount As Double = CDbl(r.LineDiscount)

            'lets check the vat !!!!
            If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localService = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat > 0 Then
                    If localExpense = 0 Then
                        If localService = 0 Then
                            localOk = False
                        Else
                            localServiceVat = localExpenseVat
                            localExpenseVat = 0
                        End If
                    End If
                End If
            End If

            'add sanity check for minus figures for airline charges as NYS don't want to show cancel charges for air tickets
            If r.Airlinecharge < 0 Then
                localExpense = CDbl(localExpense + r.Airlinecharge)
            End If

            If Not localOk Then
                strErrorLines.Append(r.Invoicenumber & " line: " & r.Linenumber & vbCrLf)
            End If
            r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " "))
            If r.Linenumber = 1 Then
                strBatchHeaderX.Append("H|")
                strBatchHeaderX.Append(r.Invoicenumber & "|")
                strBatchHeaderX.Append("Total billed=|" & r.Invoiceamount & "|")
                strBatchHeaderX.Append("Total amount=|" & r.TotAmount & "|")
                strBatchHeaderX.Append("Discount=|" & r.TotalDiscount & "|")
                strBatchHeaderX.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                strBatchHeaderX.Append(Format(CDate(r.Invoicedate).AddDays(28), "yyyy-MM-dd") & "|")
                strBatchHeaderX.Append(r.Po & "|" & vbCrLf)
                intInvoiceHeaderCount += 1
                If r.Linedescription = "" Then
                    strLineDescription = "ZZZZZ"
                Else
                    strLineDescription = Replace(r.Linedescription, "£", "")
                End If
            End If

            If localExpense <> 0 And localExpenseVat <> 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(intLineNo & "|")
                intLineNo += 1
                strBatch.Append(localExpense & "|")
                strBatch.Append("17.5|")
                strBatch.Append(localExpenseVat & "|")
                strBatch.Append(localExpense + localExpenseVat & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Linedescription = "" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
                End If
                strBatch.Append("Price=|" & (localExpense - localDiscount) + localExpenseVat & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            ElseIf localExpense <> 0 And localExpenseVat = 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(intLineNo & "|")
                intLineNo += 1
                strBatch.Append(localExpense & "|")
                strBatch.Append("0|")
                strBatch.Append("0|")
                strBatch.Append(localExpense & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Linedescription = "" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
                End If
                strBatch.Append("Price=|" & (localExpense - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            End If
            If r.Othercharge <> 0 And r.Othervat <> 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(intLineNo & "|")
                intLineNo += 1
                strBatch.Append(r.Othercharge & "|")
                strBatch.Append("17.5|")
                strBatch.Append(r.Othervat & "|")
                strBatch.Append(r.Othercharge + r.Othervat & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Linedescription = "" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
                End If
                strBatch.Append("Price=|" & (r.Othercharge - localDiscount) + r.Othervat & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            ElseIf r.Othercharge <> 0 And r.Othervat = 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(intLineNo & "|")
                intLineNo += 1
                strBatch.Append(r.Othercharge & "|")
                strBatch.Append("0|")
                strBatch.Append("0|")
                strBatch.Append(r.Othercharge & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Linedescription = "" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
                End If
                strBatch.Append("Price=|" & (r.Othercharge - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            End If
            If localService <> 0 And localServiceVat <> 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(intLineNo & "|")
                intLineNo += 1
                strBatch.Append(localService & "|")
                strBatch.Append("17.5|")
                strBatch.Append(localServiceVat & "|")
                strBatch.Append(localService + localServiceVat & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Linedescription = "" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
                End If
                strBatch.Append("Price=|" & (localService - localDiscount) + localServiceVat & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            ElseIf localService <> 0 And localServiceVat = 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(intLineNo & "|")
                intLineNo += 1
                strBatch.Append(localService & "|")
                strBatch.Append("|")
                strBatch.Append("0|")
                strBatch.Append(localService & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Linedescription = "" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
                End If
                strBatch.Append("Price=|" & (localService - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            Else
                Dim inn As Integer = 1
            End If
            If r.Airlinecharge > 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(intLineNo & "|")
                intLineNo += 1
                strBatch.Append(r.Airlinecharge & "|")
                strBatch.Append("|")
                strBatch.Append("0|")
                strBatch.Append(r.Airlinecharge & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Linedescription = "" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
                End If
                strBatch.Append("Price=|" & (r.Airlinecharge - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            End If
            lastInvoiceNumber = r.Invoicenumber
            dblInvoiceHeaderTotal = CDbl(dblInvoiceHeaderTotal + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge)
            dblInvoiceHeaderTotal2 = CDbl((dblInvoiceHeaderTotal2 + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge) - r.LineDiscount)

        Next

        Dim strInvoiceTrailer As New StringBuilder
        Dim strReturn As New StringBuilder
        strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total Amount=|" & Format(dblInvoiceHeaderTotal, "0.00") & vbCrLf)
        strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total Billed=|" & Format(dblInvoiceHeaderTotal2, "0.00"))
        'strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total 1 Billed=|" & Format(dblInvoiceHeaderTotal4, "0.00"))
        'strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total 1 Amount=|" & Format(dblInvoiceHeaderTotal3, "0.00"))

        strReturn.Append(strBatchHeaderX.ToString)
        strReturn.Append(strBatch.ToString)
        strReturn.Append(strInvoiceTrailer.ToString)

        'strBatch.Append("</Details>" & vbCrLf)
        'strBatch.Append("<Summary>" & vbCrLf)
        'strBatch.Append("<TotalExclTax>" & totalnett & "</TotalExclTax>" & vbCrLf)
        'strBatch.Append("<TotalTax>" & totalvat & "</TotalTax>" & vbCrLf)
        'strBatch.Append("<TotalInclTax>" & totalnett + totalvat & "</TotalInclTax>" & vbCrLf)
        'strBatch.Append("</Summary>" & vbCrLf)
        'strBatch.Append("</Invoice>" & vbCrLf)
        'strBatch.Append("</ABWInvoice>" & vbCrLf)
        Return strReturn.ToString
    End Function

    'Private Sub createPhysicalFileNHSInstitute(ByVal pInvno As String)

    '    'go get details of invoices here

    '    Dim strBatch As New StringBuilder
    '    Dim strBatchHeader As New StringBuilder
    '    Dim strBatchFooter As New StringBuilder
    '    Dim strErrorLines As New StringBuilder

    '    Dim intLineNo As Integer = 1

    '    strBatch.Append("<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?>" & vbNewLine)
    '    strBatch.Append("<ABWInvoice xmlns=""http://services.agresso.com/schema/ABWInvoice/2007/12/24"" xmlns:agrlib=""http://services.agresso.com/schema/ABWSchemaLib/2007/12/24"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://services.agresso.com/schema/ABWInvoice/2007/12/24 http://services.agresso.com/schema/ABWInvoice/2007/12/24/ABWInvoice.xsd"">" & vbNewLine)

    '    'do loop here
    '    Dim rs As List(Of Nhsi)
    '    rs = Nhsi.getDetails(pInvno)

    '    Dim strLineDescription As String = ""

    '    Dim totalnett As Double = 0
    '    Dim totalvat As Double = 0

    '    For Each r As Nhsi In rs
    '        'nextInvoiceNumber = r.Invoicenumber
    '        Dim localExpense As Double = CDbl(r.Expense)
    '        Dim localExpenseVat As Double = CDbl(r.Expensevat)
    '        Dim localService As Double = CDbl(r.Servicecharge)
    '        Dim localServiceVat As Double = 0
    '        Dim localOk As Boolean = True
    '        Dim localDiscount As Double = CDbl(r.LineDiscount)

    '        'lets check the vat !!!!
    '        If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
    '            Dim Ret As New getCorrectVat
    '            Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
    '            localExpense = Ret.pexpense
    '            localExpenseVat = Ret.pexpensevat
    '            localService = Ret.pservice
    '            localServiceVat = Ret.pservicevat
    '            localOk = Ret.ok
    '        Else
    '            'need to swap VAT from expense to service if ex
    '            If localExpenseVat > 0 Then
    '                If localExpense = 0 Then
    '                    localServiceVat = localExpenseVat
    '                    localExpenseVat = 0
    '                End If
    '            End If
    '        End If

    '        If r.Airlinecharge < 0 Then
    '            localExpense = CDbl(localExpense + r.Airlinecharge)
    '        End If

    '        If Not localOk Then
    '            strErrorLines.Append(r.Invoicenumber & vbNewLine)
    '        End If

    '        totalnett = CDbl((totalnett + localExpense + localService + r.Othercharge + r.Airlinecharge) - localDiscount)
    '        totalvat = CDbl(totalvat + localExpenseVat + localServiceVat + r.Othervat)

    '        If r.Linenumber = 1 Then
    '            strBatch.Append("<Invoice>" & vbNewLine)
    '            strBatch.Append("<InvoiceNo>" & r.Invoicenumber & "</InvoiceNo>" & vbNewLine)
    '            strBatch.Append("<Header>" & vbNewLine)
    '            strBatch.Append("<agrlib:InvoiceDate>" & Format(r.Invoicedate, "yyyy-MM-dd") & "</agrlib:InvoiceDate>" & vbNewLine)
    '            strBatch.Append("<agrlib:DueDate>" & Format(CDate(r.Invoicedate).AddDays(28), "yyyy-MM-dd") & "</agrlib:DueDate>" & vbNewLine)
    '            strBatch.Append("<OrderRef>" & r.Po & "</OrderRef>" & vbNewLine)
    '            strBatch.Append("<OrderNo>" & r.Po & "</OrderNo>" & vbNewLine)
    '            strBatch.Append("<agrlib:ContractId />" & vbNewLine)
    '            strBatch.Append("<Currency>GBP</Currency>" & vbNewLine)
    '            strBatch.Append("<Seller>" & vbNewLine)
    '            strBatch.Append("<agrlib:Name>NYS</agrlib:Name>" & vbNewLine)
    '            strBatch.Append("<agrlib:AddressInfo>" & vbNewLine)
    '            strBatch.Append("<agrlib:Address>Quantum House</agrlib:Address>" & vbNewLine)
    '            strBatch.Append("<agrlib:Place>Innovation Way</agrlib:Place>" & vbNewLine)
    '            strBatch.Append("<agrlib:Province>York</agrlib:Province>" & vbNewLine)
    '            strBatch.Append("<agrlib:ZipCode>YO10 5BR</agrlib:ZipCode>" & vbNewLine)
    '            strBatch.Append("</agrlib:AddressInfo>" & vbNewLine)
    '            strBatch.Append("<agrlib:SellerNo>12512</agrlib:SellerNo>" & vbNewLine)
    '            strBatch.Append("</Seller>" & vbNewLine)
    '            strBatch.Append("<Buyer>" & vbNewLine)
    '            strBatch.Append("<agrlib:Name>NHS Shared Business Services</agrlib:Name>" & vbNewLine)
    '            strBatch.Append("<agrlib:AddressInfo>" & vbNewLine)
    '            strBatch.Append("<agrlib:Address>T54 Payables 4635</agrlib:Address>" & vbNewLine)
    '            strBatch.Append("<agrlib:Place>Coventry House</agrlib:Place>" & vbNewLine)
    '            strBatch.Append("<agrlib:Province>University of Warwick Campus Coventry</agrlib:Province>" & vbNewLine)
    '            strBatch.Append("<agrlib:ZipCode>CV4 7AL</agrlib:ZipCode>" & vbNewLine)
    '            strBatch.Append("</agrlib:AddressInfo>" & vbNewLine)
    '            strBatch.Append("<Accountable>NHS Institute</Accountable>" & vbNewLine)
    '            strBatch.Append("</Buyer>" & vbNewLine)
    '            strBatch.Append("<agrlib:ReferenceCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Code />" & vbNewLine)
    '            strBatch.Append("<agrlib:Value />" & vbNewLine)
    '            strBatch.Append("<agrlib:Description />" & vbNewLine)
    '            strBatch.Append("</agrlib:ReferenceCode>" & vbNewLine)
    '            strBatch.Append("</Header>" & vbNewLine)
    '            strBatch.Append("<Details>" & vbNewLine)

    '            If r.Linedescription = "" Then
    '                strLineDescription = "ZZZZZZ"
    '            Else
    '                strLineDescription = Replace(r.Linedescription, "£", "")
    '            End If
    '        End If

    '        If localExpense <> 0 And localExpenseVat <> 0 Then
    '            strBatch.Append("<Detail>" & vbNewLine)
    '            strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
    '            intLineNo += 1
    '            strBatch.Append("<LineTotExclTax>" & localExpense & "</LineTotExclTax>" & vbNewLine)
    '            strBatch.Append("<TaxPercent>17.5</TaxPercent>" & vbNewLine) 'work this out later
    '            strBatch.Append("<TaxAmount>" & localExpenseVat & "</TaxAmount>" & vbNewLine)
    '            strBatch.Append("<LineTotInclTax>" & localExpense + localExpenseVat & "</LineTotInclTax>" & vbNewLine)
    '            strBatch.Append("<agrlib:TaxCode></agrlib:TaxCode>" & vbNewLine)
    '            strBatch.Append("<Products>" & vbNewLine)
    '            strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
    '            If r.Linedescription = "" Then
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
    '            Else
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
    '            End If
    '            strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
    '            strBatch.Append("<Price>" & (localExpense - localDiscount) + localExpenseVat & "</Price>" & vbNewLine)
    '            localDiscount = 0
    '            strBatch.Append("</Products>" & vbNewLine)
    '            strBatch.Append("</Detail>" & vbNewLine)
    '        ElseIf localExpense <> 0 And localExpenseVat = 0 Then
    '            strBatch.Append("<Detail>" & vbNewLine)
    '            strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
    '            intLineNo += 1
    '            strBatch.Append("<LineTotExclTax>" & localExpense & "</LineTotExclTax>" & vbNewLine)
    '            strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
    '            strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
    '            strBatch.Append("<LineTotInclTax>" & localExpense & "</LineTotInclTax>" & vbNewLine)
    '            strBatch.Append("<agrlib:TaxCode></agrlib:TaxCode>" & vbNewLine)
    '            strBatch.Append("<Products>" & vbNewLine)
    '            strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
    '            If r.Linedescription = "" Then
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
    '            Else
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
    '            End If
    '            strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
    '            strBatch.Append("<Price>" & (localExpense - localDiscount) & "</Price>" & vbNewLine)
    '            localDiscount = 0
    '            strBatch.Append("</Products>" & vbNewLine)
    '            strBatch.Append("</Detail>" & vbNewLine)
    '        End If
    '        If r.Othercharge <> 0 And r.Othervat <> 0 Then
    '            strBatch.Append("<Detail>" & vbNewLine)
    '            strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
    '            intLineNo += 1
    '            strBatch.Append("<LineTotExclTax>" & r.Othercharge & "</LineTotExclTax>" & vbNewLine)
    '            strBatch.Append("<TaxPercent>17.5</TaxPercent>" & vbNewLine) 'work this out later
    '            strBatch.Append("<TaxAmount>" & r.Othervat & "</TaxAmount>" & vbNewLine)
    '            strBatch.Append("<LineTotInclTax>" & r.Othercharge + r.Othervat & "</LineTotInclTax>" & vbNewLine)
    '            strBatch.Append("<agrlib:TaxCode></agrlib:TaxCode>" & vbNewLine)
    '            strBatch.Append("<Products>" & vbNewLine)
    '            strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
    '            If r.Linedescription = "" Then
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
    '            Else
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
    '            End If
    '            strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
    '            strBatch.Append("<Price>" & (r.Othercharge - localDiscount) + r.Othervat & "</Price>" & vbNewLine)
    '            localDiscount = 0
    '            strBatch.Append("</Products>" & vbNewLine)
    '            strBatch.Append("</Detail>" & vbNewLine)
    '        ElseIf r.Othercharge <> 0 And r.Othervat = 0 Then
    '            strBatch.Append("<Detail>" & vbNewLine)
    '            strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
    '            intLineNo += 1
    '            strBatch.Append("<LineTotExclTax>" & r.Othercharge & "</LineTotExclTax>" & vbNewLine)
    '            strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
    '            strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
    '            strBatch.Append("<LineTotInclTax>" & r.Othercharge & "</LineTotInclTax>" & vbNewLine)
    '            strBatch.Append("<agrlib:TaxCode></agrlib:TaxCode>" & vbNewLine)
    '            strBatch.Append("<Products>" & vbNewLine)
    '            strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
    '            If r.Linedescription = "" Then
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
    '            Else
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
    '            End If
    '            strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
    '            strBatch.Append("<Price>" & (r.Othercharge - localDiscount) & "</Price>" & vbNewLine)
    '            localDiscount = 0
    '            strBatch.Append("</Products>" & vbNewLine)
    '            strBatch.Append("</Detail>" & vbNewLine)
    '        End If
    '        If localService <> 0 And localServiceVat <> 0 Then
    '            strBatch.Append("<Detail>" & vbNewLine)
    '            strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
    '            intLineNo += 1
    '            strBatch.Append("<LineTotExclTax>" & localService & "</LineTotExclTax>" & vbNewLine)
    '            strBatch.Append("<TaxPercent>17.5</TaxPercent>" & vbNewLine) 'work this out later
    '            strBatch.Append("<TaxAmount>" & localServiceVat & "</TaxAmount>" & vbNewLine)
    '            strBatch.Append("<LineTotInclTax>" & localService + localServiceVat & "</LineTotInclTax>" & vbNewLine)
    '            strBatch.Append("<agrlib:TaxCode></agrlib:TaxCode>" & vbNewLine)
    '            strBatch.Append("<Products>" & vbNewLine)
    '            strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
    '            If r.Linedescription = "" Then
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
    '            Else
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
    '            End If
    '            strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
    '            strBatch.Append("<Price>" & (localService - localDiscount) + localServiceVat & "</Price>" & vbNewLine)
    '            localDiscount = 0
    '            strBatch.Append("</Products>" & vbNewLine)
    '            strBatch.Append("</Detail>" & vbNewLine)
    '        ElseIf localService <> 0 And localServiceVat = 0 Then
    '            strBatch.Append("<Detail>" & vbNewLine)
    '            strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
    '            intLineNo += 1
    '            strBatch.Append("<LineTotExclTax>" & localService & "</LineTotExclTax>" & vbNewLine)
    '            strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
    '            strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
    '            strBatch.Append("<LineTotInclTax>" & localService & "</LineTotInclTax>" & vbNewLine)
    '            strBatch.Append("<agrlib:TaxCode></agrlib:TaxCode>" & vbNewLine)
    '            strBatch.Append("<Products>" & vbNewLine)
    '            strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
    '            If r.Linedescription = "" Then
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
    '            Else
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
    '            End If
    '            strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
    '            strBatch.Append("<Price>" & (localService - localDiscount) & "</Price>" & vbNewLine)
    '            localDiscount = 0
    '            strBatch.Append("</Products>" & vbNewLine)
    '            strBatch.Append("</Detail>" & vbNewLine)
    '        Else
    '            Dim inn As Integer = 1
    '        End If
    '        If r.Airlinecharge > 0 Then
    '            strBatch.Append("<Detail>" & vbNewLine)
    '            strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
    '            intLineNo += 1
    '            strBatch.Append("<LineTotExclTax>" & r.Airlinecharge & "</LineTotExclTax>" & vbNewLine)
    '            strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
    '            strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
    '            strBatch.Append("<LineTotInclTax>" & r.Airlinecharge & "</LineTotInclTax>" & vbNewLine)
    '            strBatch.Append("<agrlib:TaxCode></agrlib:TaxCode>" & vbNewLine)
    '            strBatch.Append("<Products>" & vbNewLine)
    '            strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
    '            If r.Linedescription = "" Then
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
    '            Else
    '                strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
    '            End If
    '            strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
    '            strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
    '            strBatch.Append("<Price>" & (r.Airlinecharge - localDiscount) & "</Price>" & vbNewLine)
    '            localDiscount = 0
    '            strBatch.Append("</Products>" & vbNewLine)
    '            strBatch.Append("</Detail>" & vbNewLine)
    '        End If
    '        'lastInvoiceNumber = r.Invoicenumber
    '    Next
    '    strBatch.Append("</Details>" & vbNewLine)
    '    strBatch.Append("<Summary>" & vbNewLine)
    '    strBatch.Append("<TotalExclTax>" & totalnett & "</TotalExclTax>" & vbNewLine)
    '    strBatch.Append("<TotalTax>" & totalvat & "</TotalTax>" & vbNewLine)
    '    strBatch.Append("<TotalInclTax>" & totalnett + totalvat & "</TotalInclTax>" & vbNewLine)
    '    strBatch.Append("</Summary>" & vbNewLine)
    '    strBatch.Append("</Invoice>" & vbNewLine)
    '    strBatch.Append("</ABWInvoice>" & vbNewLine)

    '    makeFolderExist(poServer.MapPath("XMLFILES/NHSI"))
    '    makeFolderExist(poServer.MapPath("XMLFILES/NHSI/") & Format(Now, "dd-MM-yyyy"))

    '    Dim ofiler As New System.IO.StreamWriter(poServer.MapPath("XMLFILES/NHSI/" & Format(Now, "dd-MM-yyyy") & "/NYS" & pInvno & ".xml"), False)

    '    'Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\XMLFILES\NHSI\NYS" & pInvno & ".xml", False)
    '    'undoImpersonation()

    '    ofiler.Write(strBatch.ToString)
    '    ofiler.Flush()
    '    ofiler.Close()
    '    Try
    '        IO.File.Move(poServer.MapPath("XMLFILES/NHSI/" & Format(Now, "dd-MM-yyyy") & "/NYS" & pInvno & ".xml"), _
    '                            getConfig("XMLFilePath") & "\XMLFILES\NHSI\NYS" & pInvno & ".xml")
    '    Catch ex As Exception

    '    End Try


    'End Sub

    Private Sub createPhysicalFileNHSBSA(ByVal pInvno As String)

        'go get details of invoices here

        Dim strBatch As New StringBuilder
        Dim strBatchHeader As New StringBuilder
        Dim strBatchFooter As New StringBuilder
        Dim strErrorLines As New StringBuilder

        Dim intLineNo As Integer = 1

        strBatch.Append("<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?>" & vbNewLine)
        strBatch.Append("<ABWInvoice xmlns=""http://services.agresso.com/schema/ABWInvoice/2007/12/24"" xmlns:agrlib=""http://services.agresso.com/schema/ABWSchemaLib/2007/12/24"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://services.agresso.com/schema/ABWInvoice/2007/12/24 http://services.agresso.com/schema/ABWInvoice/2007/12/24/ABWInvoice.xsd"">" & vbNewLine)

        'do loop here
        Dim rs As List(Of NhsBsa)
        rs = NhsBsa.getDetails(pInvno)

        Dim strLineDescription As String = ""

        Dim totalnett As Double = 0
        Dim totalvat As Double = 0

        For Each r As NhsBsa In rs
            'nextInvoiceNumber = r.Invoicenumber
            Dim localExpense As Double = CDbl(r.Expense)
            Dim localExpenseVat As Double = CDbl(r.Expensevat)
            Dim localService As Double = CDbl(r.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            Dim localDiscount As Double = CDbl(r.LineDiscount)

            'lets check the vat !!!!
            If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localService = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat > 0 Then
                    If localExpense = 0 Then
                        If localService = 0 Then
                            localOk = False
                        Else
                            localServiceVat = localExpenseVat
                            localExpenseVat = 0
                        End If
                    End If
                End If
            End If

            If r.Airlinecharge < 0 Then
                localExpense = CDbl(localExpense + r.Airlinecharge)
            End If

            If Not localOk Then
                strErrorLines.Append(r.Invoicenumber & " line: " & r.Linenumber & vbNewLine)
            End If

            totalnett = CDbl((totalnett + localExpense + localService + r.Othercharge + r.Airlinecharge) - localDiscount)
            totalvat = CDbl(totalvat + localExpenseVat + localServiceVat + r.Othervat)

            r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " "))

            If r.Linenumber = 1 Then
                strBatch.Append("<Invoice>" & vbNewLine)
                strBatch.Append("<InvoiceNo>" & r.Invoicenumber & "</InvoiceNo>" & vbNewLine)
                strBatch.Append("<Header>" & vbNewLine)
                strBatch.Append("<agrlib:InvoiceDate>" & Format(r.Invoicedate, "yyyy-MM-dd") & "</agrlib:InvoiceDate>" & vbNewLine)
                strBatch.Append("<agrlib:DueDate>" & Format(CDate(r.Invoicedate).AddDays(28), "yyyy-MM-dd") & "</agrlib:DueDate>" & vbNewLine)
                strBatch.Append("<OrderRef>" & r.Po & "</OrderRef>" & vbNewLine)
                strBatch.Append("<OrderNo>" & r.Po & "</OrderNo>" & vbNewLine)
                strBatch.Append("<agrlib:ContractId />" & vbNewLine)
                strBatch.Append("<Currency>GBP</Currency>" & vbNewLine)
                strBatch.Append("<Seller>" & vbNewLine)
                strBatch.Append("<agrlib:Name>NYS</agrlib:Name>" & vbNewLine)
                strBatch.Append("<agrlib:AddressInfo>" & vbNewLine)
                strBatch.Append("<agrlib:Address>Quantum House</agrlib:Address>" & vbNewLine)
                strBatch.Append("<agrlib:Place>Innovation Way</agrlib:Place>" & vbNewLine)
                strBatch.Append("<agrlib:Province>York</agrlib:Province>" & vbNewLine)
                strBatch.Append("<agrlib:ZipCode>YO10 5BR</agrlib:ZipCode>" & vbNewLine)
                strBatch.Append("</agrlib:AddressInfo>" & vbNewLine)
                strBatch.Append("<agrlib:SellerNo>12512</agrlib:SellerNo>" & vbNewLine)
                strBatch.Append("</Seller>" & vbNewLine)
                strBatch.Append("<Buyer>" & vbNewLine)
                strBatch.Append("<agrlib:Name>NHS Shared Business Services</agrlib:Name>" & vbNewLine)
                strBatch.Append("<agrlib:AddressInfo>" & vbNewLine)
                strBatch.Append("<agrlib:Address>T54 Payables 4635</agrlib:Address>" & vbNewLine)
                strBatch.Append("<agrlib:Place>Coventry House</agrlib:Place>" & vbNewLine)
                strBatch.Append("<agrlib:Province>University of Warwick Campus Coventry</agrlib:Province>" & vbNewLine)
                strBatch.Append("<agrlib:ZipCode>CV4 7AL</agrlib:ZipCode>" & vbNewLine)
                strBatch.Append("</agrlib:AddressInfo>" & vbNewLine)
                strBatch.Append("<Accountable>NHS Institute</Accountable>" & vbNewLine)
                strBatch.Append("</Buyer>" & vbNewLine)
                strBatch.Append("<agrlib:ReferenceCode>" & vbNewLine)
                strBatch.Append("<agrlib:Code />" & vbNewLine)
                strBatch.Append("<agrlib:Value />" & vbNewLine)
                strBatch.Append("<agrlib:Description />" & vbNewLine)
                strBatch.Append("</agrlib:ReferenceCode>" & vbNewLine)
                strBatch.Append("</Header>" & vbNewLine)
                strBatch.Append("<Details>" & vbNewLine)

                If r.Linedescription = "" Then
                    strLineDescription = "ZZZZZZ"
                Else
                    strLineDescription = Replace(r.Linedescription, "£", "")
                End If
            End If

            If localExpense <> 0 And localExpenseVat <> 0 Then
                strBatch.Append("<Detail>" & vbNewLine)
                strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
                intLineNo += 1
                strBatch.Append("<LineTotExclTax>" & localExpense & "</LineTotExclTax>" & vbNewLine)
                strBatch.Append("<TaxPercent>17.5</TaxPercent>" & vbNewLine) 'work this out later
                strBatch.Append("<TaxAmount>" & localExpenseVat & "</TaxAmount>" & vbNewLine)
                strBatch.Append("<LineTotInclTax>" & localExpense + localExpenseVat & "</LineTotInclTax>" & vbNewLine)
                strBatch.Append("<agrlib:TaxCode>S</agrlib:TaxCode>" & vbNewLine)
                strBatch.Append("<Products>" & vbNewLine)
                strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
                If r.Linedescription = "" Then
                    strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
                Else
                    strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
                End If
                strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
                strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
                strBatch.Append("<Price>" & (localExpense - localDiscount) + localExpenseVat & "</Price>" & vbNewLine)
                localDiscount = 0
                strBatch.Append("</Products>" & vbNewLine)
                strBatch.Append("</Detail>" & vbNewLine)
            ElseIf localExpense <> 0 And localExpenseVat = 0 Then
                strBatch.Append("<Detail>" & vbNewLine)
                strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
                intLineNo += 1
                strBatch.Append("<LineTotExclTax>" & localExpense & "</LineTotExclTax>" & vbNewLine)
                strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
                strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
                strBatch.Append("<LineTotInclTax>" & localExpense & "</LineTotInclTax>" & vbNewLine)
                strBatch.Append("<agrlib:TaxCode>Z</agrlib:TaxCode>" & vbNewLine)
                strBatch.Append("<Products>" & vbNewLine)
                strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
                If r.Linedescription = "" Then
                    strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
                Else
                    strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
                End If
                strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
                strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
                strBatch.Append("<Price>" & (localExpense - localDiscount) & "</Price>" & vbNewLine)
                localDiscount = 0
                strBatch.Append("</Products>" & vbNewLine)
                strBatch.Append("</Detail>" & vbNewLine)
            End If
            If r.Othercharge <> 0 And r.Othervat <> 0 Then
                strBatch.Append("<Detail>" & vbNewLine)
                strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
                intLineNo += 1
                strBatch.Append("<LineTotExclTax>" & r.Othercharge & "</LineTotExclTax>" & vbNewLine)
                strBatch.Append("<TaxPercent>17.5</TaxPercent>" & vbNewLine) 'work this out later
                strBatch.Append("<TaxAmount>" & r.Othervat & "</TaxAmount>" & vbNewLine)
                strBatch.Append("<LineTotInclTax>" & r.Othercharge + r.Othervat & "</LineTotInclTax>" & vbNewLine)
                strBatch.Append("<agrlib:TaxCode>S</agrlib:TaxCode>" & vbNewLine)
                strBatch.Append("<Products>" & vbNewLine)
                strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
                If r.Linedescription = "" Then
                    strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
                Else
                    strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
                End If
                strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
                strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
                strBatch.Append("<Price>" & (r.Othercharge - localDiscount) + r.Othervat & "</Price>" & vbNewLine)
                localDiscount = 0
                strBatch.Append("</Products>" & vbNewLine)
                strBatch.Append("</Detail>" & vbNewLine)
            ElseIf r.Othercharge <> 0 And r.Othervat = 0 Then
                strBatch.Append("<Detail>" & vbNewLine)
                strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
                intLineNo += 1
                strBatch.Append("<LineTotExclTax>" & r.Othercharge & "</LineTotExclTax>" & vbNewLine)
                strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
                strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
                strBatch.Append("<LineTotInclTax>" & r.Othercharge & "</LineTotInclTax>" & vbNewLine)
                strBatch.Append("<agrlib:TaxCode>Z</agrlib:TaxCode>" & vbNewLine)
                strBatch.Append("<Products>" & vbNewLine)
                strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
                If r.Linedescription = "" Then
                    strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
                Else
                    strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
                End If
                strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
                strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
                strBatch.Append("<Price>" & (r.Othercharge - localDiscount) & "</Price>" & vbNewLine)
                localDiscount = 0
                strBatch.Append("</Products>" & vbNewLine)
                strBatch.Append("</Detail>" & vbNewLine)
            End If
            If localService <> 0 And localServiceVat <> 0 Then
                strBatch.Append("<Detail>" & vbNewLine)
                strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
                intLineNo += 1
                strBatch.Append("<LineTotExclTax>" & localService & "</LineTotExclTax>" & vbNewLine)
                strBatch.Append("<TaxPercent>17.5</TaxPercent>" & vbNewLine) 'work this out later
                strBatch.Append("<TaxAmount>" & localServiceVat & "</TaxAmount>" & vbNewLine)
                strBatch.Append("<LineTotInclTax>" & localService + localServiceVat & "</LineTotInclTax>" & vbNewLine)
                strBatch.Append("<agrlib:TaxCode>S</agrlib:TaxCode>" & vbNewLine)
                strBatch.Append("<Products>" & vbNewLine)
                strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
                If r.Linedescription = "" Then
                    strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
                Else
                    strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
                End If
                strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
                strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
                strBatch.Append("<Price>" & (localService - localDiscount) + localServiceVat & "</Price>" & vbNewLine)
                localDiscount = 0
                strBatch.Append("</Products>" & vbNewLine)
                strBatch.Append("</Detail>" & vbNewLine)
            ElseIf localService <> 0 And localServiceVat = 0 Then
                strBatch.Append("<Detail>" & vbNewLine)
                strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
                intLineNo += 1
                strBatch.Append("<LineTotExclTax>" & localService & "</LineTotExclTax>" & vbNewLine)
                strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
                strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
                strBatch.Append("<LineTotInclTax>" & localService & "</LineTotInclTax>" & vbNewLine)
                strBatch.Append("<agrlib:TaxCode>Z</agrlib:TaxCode>" & vbNewLine)
                strBatch.Append("<Products>" & vbNewLine)
                strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
                If r.Linedescription = "" Then
                    strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
                Else
                    strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
                End If
                strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
                strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
                strBatch.Append("<Price>" & (localService - localDiscount) & "</Price>" & vbNewLine)
                localDiscount = 0
                strBatch.Append("</Products>" & vbNewLine)
                strBatch.Append("</Detail>" & vbNewLine)
            Else
                Dim inn As Integer = 1
            End If
            If r.Airlinecharge > 0 Then
                strBatch.Append("<Detail>" & vbNewLine)
                strBatch.Append("<LineNo>" & intLineNo & "</LineNo>" & vbNewLine)
                intLineNo += 1
                strBatch.Append("<LineTotExclTax>" & r.Airlinecharge & "</LineTotExclTax>" & vbNewLine)
                strBatch.Append("<TaxPercent>0</TaxPercent>" & vbNewLine) 'work this out later
                strBatch.Append("<TaxAmount>0</TaxAmount>" & vbNewLine)
                strBatch.Append("<LineTotInclTax>" & r.Airlinecharge & "</LineTotInclTax>" & vbNewLine)
                strBatch.Append("<agrlib:TaxCode>Z</agrlib:TaxCode>" & vbNewLine)
                strBatch.Append("<Products>" & vbNewLine)
                strBatch.Append("<SellerProductCode>" & getProduct(r.Product, r.identifier) & "</SellerProductCode>" & vbNewLine)
                If r.Linedescription = "" Then
                    strBatch.Append("<SellerProductDescr><![CDATA[" & strLineDescription & "]]></SellerProductDescr>" & vbNewLine)
                Else
                    strBatch.Append("<SellerProductDescr><![CDATA[" & Replace(r.Linedescription, "£", "") & "]]></SellerProductDescr>" & vbNewLine)
                End If
                strBatch.Append("<agrlib:UnitCode>EA</agrlib:UnitCode>" & vbNewLine)
                strBatch.Append("<agrlib:Quantity>1</agrlib:Quantity>" & vbNewLine)
                strBatch.Append("<Price>" & (r.Airlinecharge - localDiscount) & "</Price>" & vbNewLine)
                localDiscount = 0
                strBatch.Append("</Products>" & vbNewLine)
                strBatch.Append("</Detail>" & vbNewLine)
            End If
            'lastInvoiceNumber = r.Invoicenumber
        Next
        strBatch.Append("</Details>" & vbNewLine)
        strBatch.Append("<Summary>" & vbNewLine)
        strBatch.Append("<TotalExclTax>" & totalnett & "</TotalExclTax>" & vbNewLine)
        strBatch.Append("<TotalTax>" & totalvat & "</TotalTax>" & vbNewLine)
        strBatch.Append("<TotalInclTax>" & totalnett + totalvat & "</TotalInclTax>" & vbNewLine)
        strBatch.Append("</Summary>" & vbNewLine)
        strBatch.Append("</Invoice>" & vbNewLine)
        strBatch.Append("</ABWInvoice>" & vbNewLine)

        makeFolderExist(getConfig("XMLFilePath") & "\NHSBSA")
        makeFolderExist(getConfig("XMLFilePath") & "\NHSBSA\" & Format(Now, "dd-MM-yyyy"))

        Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\NHSBSA\" & Format(Now, "dd-MM-yyyy") & "/NYS" & pInvno & ".xml", False)

        ofiler.Write(strBatch.ToString)
        ofiler.Flush()
        ofiler.Close()
        Try
            IO.File.Move(getConfig("XMLFilePath") & "\NHSBSA\" & Format(Now, "dd-MM-yyyy") & "/NYS" & pInvno & ".xml", _
                         getConfig("XMLFilePath") & "\XMLFILES\NHSBSA\NYS" & pInvno & ".xml")
        Catch ex As Exception

        End Try


    End Sub

    Public Shared Sub makeFolderExist(ByVal folder As String)
        If Not IO.Directory.Exists(folder) Then
            IO.Directory.CreateDirectory(folder)
        End If
    End Sub

    'Public Shared Function createFileNHSInstituteToView(ByVal pstartdate As String, ByVal penddate As String) As String

    '    'go get details of invoices here
    '    Dim strBatch As New StringBuilder
    '    Dim strBatchHeaderX As New StringBuilder
    '    Dim strBatchFooter As New StringBuilder
    '    Dim strErrorLines As New StringBuilder
    '    Dim strPOErrors As New StringBuilder

    '    Dim intLineNo As Integer = 1

    '    'do loop here
    '    Dim rs As List(Of Nhsi)
    '    rs = Nhsi.list(pstartdate, penddate)

    '    Dim nextInvoiceNumber As String = ""
    '    Dim lastInvoiceNumber As String = ""
    '    Dim strLineDescription As String = ""

    '    Dim totalnett As Double = 0
    '    Dim totalvat As Double = 0
    '    Dim dblInvoiceHeaderTotal As Double = 0
    '    Dim dblInvoiceHeaderTotal2 As Double = 0
    '    Dim intInvoiceHeaderCount As Integer = 0
    '    Dim intInvoiceLineCount As Integer = 0

    '    'lets cycle through and check Project Code is correct
    '    For Each r As Nhsi In rs
    '        If r.Po.Length < 5 Or r.Po.Length > 9 Then
    '            strPOErrors.Append(r.Invoicenumber & " : " & r.Po & vbCrLf)
    '        End If
    '    Next

    '    If strPOErrors.ToString <> "" Then
    '        Return "ERROR - THE FOLLOWING INVOICES HAVE INCORRECT PROJECT CODES:" & vbCrLf & strPOErrors.ToString
    '    End If

    '    For Each r As Nhsi In rs
    '        nextInvoiceNumber = r.Invoicenumber
    '        Dim localExpense As Double = CDbl(r.Expense)
    '        Dim localExpenseVat As Double = CDbl(r.Expensevat)
    '        Dim localService As Double = CDbl(r.Servicecharge)
    '        Dim localServiceVat As Double = 0
    '        Dim localOk As Boolean = True
    '        Dim localDiscount As Double = CDbl(r.LineDiscount)

    '        'lets check the vat !!!!
    '        If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
    '            Dim Ret As New getCorrectVat
    '            Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
    '            localExpense = Ret.pexpense
    '            localExpenseVat = Ret.pexpensevat
    '            localService = Ret.pservice
    '            localServiceVat = Ret.pservicevat
    '            localOk = Ret.ok
    '        Else
    '            'need to swap VAT from expense to service if ex
    '            If localExpenseVat > 0 Then
    '                If localExpense = 0 Then
    '                    localServiceVat = localExpenseVat
    '                    localExpenseVat = 0
    '                End If
    '            End If
    '        End If

    '        'add sanity check for minus figures for airline charges as NYS don't want to show cancel charges for air tickets
    '        If r.Airlinecharge < 0 Then
    '            localExpense = CDbl(localExpense + r.Airlinecharge)
    '        End If

    '        If Not localOk Then
    '            strErrorLines.Append(r.Invoicenumber & vbCrLf)
    '        End If

    '        If r.Linenumber = 1 Then
    '            strBatchHeaderX.Append("H|")
    '            strBatchHeaderX.Append(r.Invoicenumber & "|")
    '            strBatchHeaderX.Append("Total billed=|" & r.Invoiceamount & "|")
    '            strBatchHeaderX.Append("Total amount=|" & r.TotAmount & "|")
    '            strBatchHeaderX.Append("Discount=|" & r.TotalDiscount & "|")
    '            strBatchHeaderX.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
    '            strBatchHeaderX.Append(Format(CDate(r.Invoicedate).AddDays(28), "yyyy-MM-dd") & "|")
    '            strBatchHeaderX.Append(r.Po & "|" & vbCrLf)
    '            intInvoiceHeaderCount += 1
    '            If r.Linedescription = "" Then
    '                strLineDescription = "ZZZZZ"
    '            Else
    '                strLineDescription = Replace(r.Linedescription, "£", "")
    '            End If
    '        End If

    '        If localExpense <> 0 And localExpenseVat <> 0 Then
    '            strBatch.Append("L|")
    '            strBatch.Append(r.Invoicenumber & "|")
    '            strBatch.Append(intLineNo & "|")
    '            intLineNo += 1
    '            strBatch.Append(localExpense & "|")
    '            strBatch.Append("17.5|")
    '            strBatch.Append(localExpenseVat & "|")
    '            strBatch.Append(localExpense + localExpenseVat & "|")
    '            strBatch.Append(getProduct(r.Product, r.identifier) & "|")
    '            If r.Linedescription = "" Then
    '                strBatch.Append(strLineDescription & "|")
    '            Else
    '                strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
    '            End If
    '            strBatch.Append("Price=|" & (localExpense - localDiscount) + localExpenseVat & "|" & vbCrLf)
    '            localDiscount = 0
    '            intInvoiceLineCount += 1

    '        ElseIf localExpense <> 0 And localExpenseVat = 0 Then
    '            strBatch.Append("L|")
    '            strBatch.Append(r.Invoicenumber & "|")
    '            strBatch.Append(intLineNo & "|")
    '            intLineNo += 1
    '            strBatch.Append(localExpense & "|")
    '            strBatch.Append("0|")
    '            strBatch.Append("0|")
    '            strBatch.Append(localExpense & "|")
    '            strBatch.Append(getProduct(r.Product, r.identifier) & "|")
    '            If r.Linedescription = "" Then
    '                strBatch.Append(strLineDescription & "|")
    '            Else
    '                strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
    '            End If
    '            strBatch.Append("Price=|" & (localExpense - localDiscount) & "|" & vbCrLf)
    '            localDiscount = 0
    '            intInvoiceLineCount += 1

    '        End If
    '        If r.Othercharge <> 0 And r.Othervat <> 0 Then
    '            strBatch.Append("L|")
    '            strBatch.Append(r.Invoicenumber & "|")
    '            strBatch.Append(intLineNo & "|")
    '            intLineNo += 1
    '            strBatch.Append(r.Othercharge & "|")
    '            strBatch.Append("17.5|")
    '            strBatch.Append(r.Othervat & "|")
    '            strBatch.Append(r.Othercharge + r.Othervat & "|")
    '            strBatch.Append(getProduct(r.Product, r.identifier) & "|")
    '            If r.Linedescription = "" Then
    '                strBatch.Append(strLineDescription & "|")
    '            Else
    '                strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
    '            End If
    '            strBatch.Append("Price=|" & (r.Othercharge - localDiscount) + r.Othervat & "|" & vbCrLf)
    '            localDiscount = 0
    '            intInvoiceLineCount += 1

    '        ElseIf r.Othercharge <> 0 And r.Othervat = 0 Then
    '            strBatch.Append("L|")
    '            strBatch.Append(r.Invoicenumber & "|")
    '            strBatch.Append(intLineNo & "|")
    '            intLineNo += 1
    '            strBatch.Append(r.Othercharge & "|")
    '            strBatch.Append("0|")
    '            strBatch.Append("0|")
    '            strBatch.Append(r.Othercharge & "|")
    '            strBatch.Append(getProduct(r.Product, r.identifier) & "|")
    '            If r.Linedescription = "" Then
    '                strBatch.Append(strLineDescription & "|")
    '            Else
    '                strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
    '            End If
    '            strBatch.Append("Price=|" & (r.Othercharge - localDiscount) & "|" & vbCrLf)
    '            localDiscount = 0
    '            intInvoiceLineCount += 1

    '        End If
    '        If localService <> 0 And localServiceVat <> 0 Then
    '            strBatch.Append("L|")
    '            strBatch.Append(r.Invoicenumber & "|")
    '            strBatch.Append(intLineNo & "|")
    '            intLineNo += 1
    '            strBatch.Append(localService & "|")
    '            strBatch.Append("17.5|")
    '            strBatch.Append(localServiceVat & "|")
    '            strBatch.Append(localService + localServiceVat & "|")
    '            strBatch.Append(getProduct(r.Product, r.identifier) & "|")
    '            If r.Linedescription = "" Then
    '                strBatch.Append(strLineDescription & "|")
    '            Else
    '                strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
    '            End If
    '            strBatch.Append("Price=|" & (localService - localDiscount) + localServiceVat & "|" & vbCrLf)
    '            localDiscount = 0
    '            intInvoiceLineCount += 1

    '        ElseIf localService <> 0 And localServiceVat = 0 Then
    '            strBatch.Append("L|")
    '            strBatch.Append(r.Invoicenumber & "|")
    '            strBatch.Append(intLineNo & "|")
    '            intLineNo += 1
    '            strBatch.Append(localService & "|")
    '            strBatch.Append("|")
    '            strBatch.Append("0|")
    '            strBatch.Append(localService & "|")
    '            strBatch.Append(getProduct(r.Product, r.identifier) & "|")
    '            If r.Linedescription = "" Then
    '                strBatch.Append(strLineDescription & "|")
    '            Else
    '                strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
    '            End If
    '            strBatch.Append("Price=|" & (localService - localDiscount) & "|" & vbCrLf)
    '            localDiscount = 0
    '            intInvoiceLineCount += 1

    '        Else
    '            Dim inn As Integer = 1
    '        End If
    '        If r.Airlinecharge > 0 Then
    '            strBatch.Append("L|")
    '            strBatch.Append(r.Invoicenumber & "|")
    '            strBatch.Append(intLineNo & "|")
    '            intLineNo += 1
    '            strBatch.Append(r.Airlinecharge & "|")
    '            strBatch.Append("|")
    '            strBatch.Append("0|")
    '            strBatch.Append(r.Airlinecharge & "|")
    '            strBatch.Append(getProduct(r.Product, r.identifier) & "|")
    '            If r.Linedescription = "" Then
    '                strBatch.Append(strLineDescription & "|")
    '            Else
    '                strBatch.Append(Replace(r.Linedescription, "£", "") & "|")
    '            End If
    '            strBatch.Append("Price=|" & (r.Airlinecharge - localDiscount) & "|" & vbCrLf)
    '            localDiscount = 0
    '            intInvoiceLineCount += 1

    '        End If
    '        lastInvoiceNumber = r.Invoicenumber
    '        dblInvoiceHeaderTotal = CDbl(dblInvoiceHeaderTotal + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge)
    '        dblInvoiceHeaderTotal2 = CDbl((dblInvoiceHeaderTotal2 + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge) - r.LineDiscount)

    '    Next

    '    Dim strInvoiceTrailer As New StringBuilder
    '    Dim strReturn As New StringBuilder
    '    strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total Amount=|" & Format(dblInvoiceHeaderTotal, "0.00") & vbCrLf)
    '    strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total Billed=|" & Format(dblInvoiceHeaderTotal2, "0.00"))
    '    'strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total 1 Billed=|" & Format(dblInvoiceHeaderTotal4, "0.00"))
    '    'strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total 1 Amount=|" & Format(dblInvoiceHeaderTotal3, "0.00"))

    '    strReturn.Append(strBatchHeaderX.ToString)
    '    strReturn.Append(strBatch.ToString)
    '    strReturn.Append(strInvoiceTrailer.ToString)

    '    'strBatch.Append("</Details>" & vbCrLf)
    '    'strBatch.Append("<Summary>" & vbCrLf)
    '    'strBatch.Append("<TotalExclTax>" & totalnett & "</TotalExclTax>" & vbCrLf)
    '    'strBatch.Append("<TotalTax>" & totalvat & "</TotalTax>" & vbCrLf)
    '    'strBatch.Append("<TotalInclTax>" & totalnett + totalvat & "</TotalInclTax>" & vbCrLf)
    '    'strBatch.Append("</Summary>" & vbCrLf)
    '    'strBatch.Append("</Invoice>" & vbCrLf)
    '    'strBatch.Append("</ABWInvoice>" & vbCrLf)
    '    Return strReturn.ToString
    'End Function

    Public Shared Function getProduct(ByVal pProduct As String, ByVal pIdentifier As String) As String
        pProduct = pProduct.Trim
        If pProduct.ToLower = "rail" Then
            Return "RAIL"
        ElseIf pProduct.ToLower = "hotel" Then
            If pIdentifier = "" Or pIdentifier.StartsWith("c") Then
                Return "HOTEL"
            Else
                Return "CONFER"
            End If
        ElseIf pProduct.ToLower = "fees" Or pProduct.ToLower = "non travel expense" Then
            Return "TRAVELFEES"
        ElseIf pProduct.ToLower = "air" Then
            Return "FLIGHTS"
        ElseIf pProduct.ToLower = "car rental" Then
            Return "VEHHIRE"
        ElseIf pProduct.ToLower = "other" Then
            Return "VISA"
        Else
            Return ""
        End If
    End Function

    Public Function runO2POCheck() As String

        ' get list of distinct Invoice numbers
        Dim oFirstRuns As List(Of O2)
        oFirstRuns = O2.list()

        'First run through and check pos etc
        Dim strPoValueErrors As New StringBuilder
        Dim strNoPoErrors As New StringBuilder
        Dim strErrorLength As New StringBuilder
        Dim strErrorLinesVAT As New StringBuilder
        Dim currentInvoice As String = ""
        Dim lastInvoice As String = ""

        'R2.16 CR
        Dim strCompanyErrors As New StringBuilder
        Dim strPOErrors As New StringBuilder

        'for each invoice number
        For Each oFirstRun As O2 In oFirstRuns
            Dim dblCurrentPrice As Double = 0

            If oFirstRun.Invoicenumber = "N552723" Then
                Dim istop As Integer = 0
            End If
            currentInvoice = oFirstRun.Invoicenumber

            'R2.16 CR - Refine the skip, always skip PO value check if booking is not RTD and not a blanket PO
            Dim blnRTDBooking As Boolean = False
            Dim blnBlanketPO As Boolean = False


            Dim oO2PO As New NysDat.clsO2Po
            oO2PO.mstrOrderNumber = oFirstRun.OrderNumber
            oO2PO = oO2PO.GetByOrderNumber()
            blnBlanketPO = oO2PO.mblnIsBlanket


            If oFirstRun.EnquiryID > 0 Then
                'we have an enquiryid, get the booking details and check the companyid
                Dim oEnquiry As New NysDat.clsEnquiryDat
                oEnquiry.Populate(oFirstRun.EnquiryID, 0, 0, "", "", 0, "", 0, 0, False)

                blnRTDBooking = getConfig("O2RTDCompanyID").Contains(toStr(oEnquiry.companyid))

            Else
                'No enquiryid, so should only be conferma bookings
                Dim blnBookerNameCheck As Boolean = False
                If oFirstRun.identifier.StartsWith("c") Then
                    Dim oCubitContact As New NysDat.clsCorporateFeedbackDat
                    oCubitContact = NysDat.clsCorporateFeedbackDat.getCubitBooker(oFirstRun.Invoicenumber, Replace(oFirstRun.identifier, "c", ""))

                    If oCubitContact.mstrBookerEmail <> "" Then
                        'we have an email address to match on!
                        Dim oContacts As New NysDat.clsContactDat
                        oContacts.getByEmail(oCubitContact.mstrBookerEmail)

                        'R2.18 CR - could be more than one contact, loop through them!
                        If oContacts.mcolContactsX.Count > 0 Then
                            For Each oContact In oContacts.mcolContactsX
                                blnRTDBooking = getConfig("O2RTDCompanyID").Contains(toStr(oContact.mintcompanyid))
                            Next
                        Else
                            'no contacts found - check them by name then!
                            blnBookerNameCheck = True
                        End If

                        'R2.18 CR - dont do this anymore
                        'If oContact.mintcontactid > 0 Then
                        '    blnRTDBooking = getConfig("O2RTDCompanyID").Contains(toStr(oContact.mintcompanyid))
                        'Else
                        '    blnBookerNameCheck = True
                        'End If
                    Else
                        If oFirstRun.BookerName = "" Then
                            'replace it with what we have found in CUBIT
                            oFirstRun.BookerName = oCubitContact.mstrBookerName
                            blnBookerNameCheck = True
                        End If
                    End If
                Else
                    'no idea what has happened here - but still possible!
                    'try to work it out by bookername
                    'could be a manual invoice
                    blnBookerNameCheck = True
                End If


                If blnBookerNameCheck Then
                    Dim oContactList As New List(Of NysDat.clsContactDat)

                    If oFirstRun.BookerName <> "" Then
                        If oFirstRun.BookerName.Contains(" ") Then

                            'R2.18 CR - just search RTD for the contact
                            oContactList = NysDat.clsContactDat.search(0, getConfig("O2RTDCompanyID"), oFirstRun.BookerName.Split(" ")(0), oFirstRun.BookerName.Split(" ")(1))

                            'R2.18 CR - dont do this anymore
                            ''get the O2 contacts
                            'oContactList = NysDat.clsContactDat.search(100058, 0, oFirstRun.BookerName.Split(" ")(0), oFirstRun.BookerName.Split(" ")(1))
                            ''add the groupbookings contacts to the end of the collection
                            'oContactList.AddRange(NysDat.clsContactDat.search(0, 100252, oFirstRun.BookerName.Split(" ")(0), oFirstRun.BookerName.Split(" ")(1)))
                        Else

                            'R2.18 CR - just search RTD for the contact
                            oContactList = NysDat.clsContactDat.search(0, getConfig("O2RTDCompanyID"), oFirstRun.BookerName, "")

                            'R2.18 CR - dont do this anymore
                            ''get the O2 contacts
                            'oContactList = NysDat.clsContactDat.search(100058, 0, oFirstRun.BookerName, "")
                            ''add the groupbookings contacts to the end of the collection
                            'oContactList.AddRange(NysDat.clsContactDat.search(0, 100252, oFirstRun.BookerName, ""))
                        End If
                    Else
                        'no booker name, no enquiryid, no cubit booker email..... all is lost, time to error!
                        strCompanyErrors.Append("INVOICE " & oFirstRun.Invoicenumber & " Unable to resolve this to an O2 company - no booker name found and can't resolve to a MEVIS enquiry." & vbCrLf)
                    End If


                    'R2.21 CR - BUG FIX, dont let it check for count = 0. 
                    'If you do then you can cause errors for bookers that are in other companies (i.e. NOT RTD)
                    'Dim intPreviousCompanyID As Integer = 0
                    'If oContactList.Count = 0 Then
                    '    strCompanyErrors.Append("INVOICE " & oFirstRun.Invoicenumber & " booker '" & oFirstRun.BookerName & "' - was not found in any O2 company on MEVIS." & vbCrLf)
                    'End If

                    For Each oContact In oContactList
                        If oContact.mstrcontactstatus.ToLower = "active" Then

                            'R2.18 CR - just set the boolean to true (they have an active profile in RTD)
                            blnRTDBooking = True

                            'R2.18 CR - dont do this anymore
                            'If intPreviousCompanyID = 0 OrElse oContact.mintcompanyid = intPreviousCompanyID Then
                            '    'check the companyid
                            '    blnRTDBooking = getConfig("O2CompanyIdsPOCheck").Contains(toStr(oContact.mintcompanyid))
                            'Else
                            '    'we have multiple contacts with different companies
                            '    strCompanyErrors.Append("BOOKER " & oFirstRun.BookerName & " - was found in multiple O2 companies on MEVIS, please ammend." & vbCrLf)
                            'End If

                            ''if multiple contacts found then check them all - if all the same company then no probs!
                            ''if different companyies then we have a problem - error to user!
                            'intPreviousCompanyID = oContact.mintcompanyid
                        End If
                    Next
                End If
            End If

            'if no BookerName or EnquiryID then chances are the PO check will be skipped
            'because we cant work out if RTD or not.... code assumes not

            'R2.16 CR - skip some invoices
            'skip all that are not RTD and not blanket PO's
            'also skip any invoice specified in the config file
            Dim blnSkipInvoicePOCheck As Boolean = False
            If blnBlanketPO Or blnRTDBooking Then
                If getConfig("O2InvoicesSkipPOCheck").Contains(oFirstRun.Invoicenumber) OrElse getConfig("O2InvoicesSkipPOCheck") = "all" Then
                    blnSkipInvoicePOCheck = True
                Else
                    blnSkipInvoicePOCheck = False
                End If
            Else
                blnSkipInvoicePOCheck = True
            End If



            If currentInvoice <> lastInvoice Then
                If oFirstRun.OrderNumber <> "" Then
                    If blnSkipInvoicePOCheck = False Then
                        'select a PO from O2 PO table and all its details
                        Dim oO2PoX As New NysDat.clsO2Po
                        oO2PoX.mstrOrderNumber = oFirstRun.Po.Trim
                        oO2PoX = oO2PoX.GetByOrderNumber()

                        'check to see if PO price has been used up
                        Dim oO2PoChecks As New List(Of O2)
                        oO2PoChecks = O2.FeederFileO2PoList(oFirstRun.Po)
                        Dim strAssociatedInvoicesX As String = ""
                        Dim PoValueInBoss As Double = 0

                        For Each oO2PoCheck As O2 In oO2PoChecks

                            Dim localExpensePO As Double = Math.Round(CDbl(oO2PoCheck.TotalNett), 2)

                            PoValueInBoss = PoValueInBoss + localExpensePO
                            If Not strAssociatedInvoicesX.Contains(oO2PoCheck.Invoicenumber) Then
                                strAssociatedInvoicesX = strAssociatedInvoicesX & vbTab & "Inv ref:" & vbTab & oO2PoCheck.Invoicenumber & vbTab & _
                                                        "Nett value= " & oO2PoCheck.TotalNett & vbTab & _
                                                        "Billed= " & oO2PoCheck.tot_billed & vbTab & _
                                                        "Recvd= " & oO2PoCheck.tot_recvd & vbCrLf
                            End If
                        Next

                        If Math.Round(oO2PoX.mdblTotalPrice, 2) > 0 AndAlso Math.Round(PoValueInBoss, 2) > Math.Round(oO2PoX.mdblTotalPrice, 2) Then
                            'let's try 10%
                            If Math.Round(oO2PoX.mdblTotalPrice, 2) > 0 AndAlso Math.Round(PoValueInBoss, 2) > (Math.Round(oO2PoX.mdblTotalPrice, 2) * 1.1) Then
                                strPoValueErrors.Append("PO " & oFirstRun.OrderNumber & " IS OVER VALUE. PO VALUE=" & vbTab & CStr(Math.Round(oO2PoX.mdblTotalPrice, 2)) & vbCrLf & _
                                                        strAssociatedInvoicesX & vbCrLf & vbTab & "TOTAL CURRENT NETT VALUE=" & vbTab & PoValueInBoss & vbCrLf & vbCrLf)
                            End If
                        End If
                    End If
                Else
                    If oFirstRun.Po.ToLower.Trim <> "n/a pay direct" Then
                        If Not strNoPoErrors.ToString.Contains(oFirstRun.Invoicenumber) Then
                            strNoPoErrors.Append(oFirstRun.Invoicenumber & ", ")
                        End If
                    End If
                End If

            End If

            Dim localExpense As Decimal = CDec(oFirstRun.Expense)
            Dim localExpenseVat As Decimal = CDec(oFirstRun.Expensevat)
            Dim localService As Decimal = CDec(oFirstRun.Servicecharge)
            Dim localServiceVat As Decimal = 0
            Dim localOk As Boolean = True
            Dim localDiscount As Decimal = CDec(oFirstRun.LineDiscount)

            'lets check the vat !!!!
            If localExpense > 0 And localExpenseVat > 0 And localService > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(localExpense, localExpenseVat, localService)
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localExpense = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat > 0 Then
                    If localExpense = 0 Then
                        If localService = 0 Then
                            localOk = False
                        Else
                            localServiceVat = localExpenseVat
                            localExpenseVat = 0
                        End If
                    End If
                End If
            End If

            Dim dblVAT As Double = 0

            If CDate(oFirstRun.Invoicedate) >= CDate("04/01/2011") Then
                dblVAT = 0.2
            Else
                dblVAT = 0.175
            End If

            If Not testVat(localExpense, localExpenseVat, 0, 0, 0, 0, dblVAT) Then
                strErrorLinesVAT.Append(oFirstRun.Invoicenumber & " line: " & oFirstRun.Linenumber & ", ")
            End If

            'add sanity check for minus figures for airline charges as NYS don't want to show cancel charges for air tickets
            If oFirstRun.Airlinecharge < 0 Then
                localExpense = CDbl(localExpense + oFirstRun.Airlinecharge)
            End If

            If Not localOk Then
                strErrorLinesVAT.Append(oFirstRun.Invoicenumber & " line: " & oFirstRun.Linenumber & ", ")
            End If

            oFirstRun.Linedescription = Pack(oFirstRun.Linedescription.Trim.Replace(vbCrLf, " ")).Trim

            'R2.26 CR - new line description length
            'If oFirstRun.Linedescription.Length > 225 Then
            '    strErrorLength.Append(oFirstRun.Invoicenumber & ", ")
            'End If
            If oFirstRun.Linedescription.Length > 4000 Then
                strErrorLength.Append(oFirstRun.Invoicenumber & ", ")
            End If

            lastInvoice = currentInvoice
        Next

        Dim strRetErrors As String = ""

        'R2.16 CR
        'Identifying company errors
        If strCompanyErrors.length > 0 Then
            strRetErrors = "THE FOLLOWING BOOKERS WERE NOT FOUND/FOUND MULTIPLE TIMES IN MEVIS: " & vbCrLf & strCompanyErrors.ToString.Trim
        End If

        'PO errors
        If strPoValueErrors.Length > 0 Then
            strRetErrors = "THE FOLLOWING INVOICES HAVE EXCEEDED THEIR PO LIMIT: " & vbCrLf & strPoValueErrors.ToString.Trim '.Remove(strPoValueErrors.ToString.LastIndexOf(","))
        End If

        'Description too long
        If strErrorLength.Length > 0 Then
            If strRetErrors <> "" Then
                strRetErrors = strRetErrors & vbCrLf & vbCrLf & "THE FOLLOWING HAVE TOO MANY CHARACTERS IN THE DESCRIPTION: " & vbCrLf & _
                            strErrorLength.ToString.Trim.Remove(strErrorLength.ToString.LastIndexOf(","))
            Else
                strRetErrors = "THE FOLLOWING HAVE TOO MANY CHARACTERS IN THE DESCRIPTION: " & vbCrLf & strErrorLength.ToString.Trim.Remove(strErrorLength.ToString.LastIndexOf(","))
            End If
        End If

        'No PO attached
        If strNoPoErrors.Length > 0 Then
            If strRetErrors <> "" Then
                strRetErrors = strRetErrors & vbCrLf & vbCrLf & "THE FOLLOWING INVOICES PO NUMBERS DO NOT EXIST IN THE DATABASE TABLE: " & vbCrLf & strNoPoErrors.ToString.Trim.Remove(strNoPoErrors.ToString.LastIndexOf(","))
            Else
                strRetErrors = "THE FOLLOWING INVOICES PO NUMBERS DO NOT EXIST IN THE DATABSE TABLE: " & vbCrLf & strNoPoErrors.ToString.Trim.Remove(strNoPoErrors.ToString.LastIndexOf(","))
            End If
        End If

        'VAT Errors
        If strErrorLinesVAT.Length > 0 Then
            If strRetErrors <> "" Then
                strRetErrors = strRetErrors & vbCrLf & vbCrLf & "THE FOLLOWING INVOICES HAVE VAT ISSUES: " & vbCrLf & strErrorLinesVAT.ToString.Trim.Remove(strErrorLinesVAT.ToString.LastIndexOf(","))
            Else
                strRetErrors = "THE FOLLOWING INVOICES HAVE VAT ISSUES: " & vbCrLf & strErrorLinesVAT.ToString.Trim.Remove(strErrorLinesVAT.ToString.LastIndexOf(","))
            End If
        End If

        'exits if errors found
        If strRetErrors <> "" Then
            Return strRetErrors
        End If

        'R2.26 CR - do the view slightly different, show the results for both O2 systems
        'sends details to screen, shows errors too
        'Dim strRet As String = runO2ToView()
        Dim strRet As String = ""
        strRet = "Steria File:" & vbCrLf & runO2ToView_Steria()
        strRet &= vbCrLf & vbCrLf & "Adquira File:" & vbCrLf & runO2ToView_Adquira()

        Return strRet

    End Function

    Public Function createO2Files(ByVal pPOstartsWith As String, ByVal pTest As Boolean) As String
        Try

            Dim rs As List(Of O2)

            Dim strWhichType As String = "HQ"
            If pPOstartsWith.StartsWith("401") Then
                strWhichType = "UK"
            End If

            rs = O2.FeederFileO2InvoiceList(pPOstartsWith)

            Dim strCSV As New StringBuilder
            Dim strHeadCSV As New StringBuilder
            Dim dicPoNumbers As New Dictionary(Of String, String)
            Dim intVersionNo As Integer = 0
            Dim intLineCount As Integer = 0

            If rs.Count > 0 Then
                intLineCount += 1
            Else
                Return "No " & strWhichType & " invoices this run."
            End If

            For Each r As O2 In rs
                Dim retVals As o2details = createPhysicalFileO2_Steria(r.Invoicenumber, intLineCount)

                strCSV.Append(retVals.strDetails)
                intLineCount = retVals.intlinecount
                If retVals.vatfailure Then
                    Return "VAT failure on file creation for Invoice: " & retVals.invoice
                End If
                If retVals.totalsfailure Then
                    Return "Totals failure on file creation for Invoice: " & retVals.invoice
                End If
                'save to O2Admin table so we can see what's ahppening later
                If Not pTest Then
                    Dim oAdmin As New O2PoAdmin(0, r.Invoicenumber, r.Po, "Sent in Feeder File", Format(Now, "dd/MM/yyyy"))
                    oAdmin.save()
                End If
            Next

            If Not pTest Then
                dicPoNumbers = Nothing

                If strCSV.Length > 0 Then
                    Try
                        intVersionNo = BatchNoGet("O2" & strWhichType)
                        'create file header line
                        strHeadCSV.Append("H~")
                        strHeadCSV.Append("NYS" & strWhichType & "PO~")
                        strHeadCSV.Append(intVersionNo & "~") 'version no
                        strHeadCSV.Append("0~") 'revision no
                        strHeadCSV.Append(Format(Date.Now, "dd-MM-yyyy"))
                        strHeadCSV.Append("^" & vbNewLine)

                        intLineCount += 1
                        'add trailer row
                        strCSV.Append("T~")
                        strCSV.Append(intLineCount)
                        strCSV.Append("^" & vbNewLine)

                        makeFolderExist(getConfig("XMLFilePath") & "\O2")
                        makeFolderExist(getConfig("XMLFilePath") & "\O2\" & Format(Now, "dd-MM-yyyy"))
                        Dim strFileName As String = ""

                        strFileName = "AP_EI_" & strWhichType & "_NYS" & strWhichType & "PO_" & Format(Date.Now, "dd-MM-yyyy") & "_" & intVersionNo.ToString & ".dat"

                        Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\O2\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

                        ofiler.Write(strHeadCSV.ToString & strCSV.ToString)
                        ofiler.Flush()
                        ofiler.Close()

                    Catch ex As Exception
                        Return "ERROR CREATING " & strWhichType & " FILE:" & ex.Message
                    End Try
                End If
            End If
            Return ""
        Catch ex As Exception
            Return "ERROR:" & ex.Message
        End Try
    End Function

    Public Function createFileCIMA(ByVal pstartdate As String, ByVal penddate As String) As String
        'check to remove duplicates
        'sends details to screen, shows errors too
        Dim strRet As String = createFileCIMAToView(pstartdate, penddate)
        If strRet.StartsWith("ERROR") Then
            Return strRet
        End If

        ' get list of distinct Invoice numbers
        Dim rs As List(Of CIMA)
        rs = CIMA.FeederFileCIMAFirstRun(pstartdate, penddate)
        'now cycle through and create individual files
        For Each r As CIMA In rs
            'R2.13 CR - CIMA testing, needs to be removed before live!!
            'If r.Invoicenumber = "N614594" _
            '    Or r.Invoicenumber = "N614595" Then
            createPhysicalFileCIMA(r.Invoicenumber)
            'End If
        Next

        Return strRet

    End Function


    'R? CR - Test fix for Sherry, invoice come through as negative gross but positive nett
    Public Shared Function createSingleFileCIMA(pstrInvoiceNumber As String) As String
        Dim ret As String = ""
        'WARNING: NO ERROR HANDLING ON THIS FUNCTION!!!

        Dim oFeeder As New FeederFileCreation
        oFeeder.createPhysicalFileCIMA(pstrInvoiceNumber)

        ret = "All submitted fine!"
        Return ret
    End Function

    'R2.26 CR - new O2 invoicing system
    Public Shared Function runO2ToView_Adquira() As String
        Dim strBatch As New StringBuilder

        Dim nextInvoiceNumberX As String = ""
        Dim intLocalLineNumber As Integer = 0

        Dim rs As List(Of O2)
        rs = O2.list()

        'need to check the PO to see if it's steria

        Dim dblVAT As Double = 0

        For Each invoice In rs
            'populate the full details of the order
            Dim oPurchaseOrder As New clsO2Po
            oPurchaseOrder.mstrOrderNumber = invoice.Po
            oPurchaseOrder = oPurchaseOrder.GetByOrderNumber()
            If oPurchaseOrder.mstrSourceSystemName.ToLower = "adquira" Then
                'hold the values
                Dim localExpense As Double = CDbl(invoice.Expense)
                Dim localExpenseVat As Double = CDbl(invoice.Expensevat)
                Dim localService As Double = CDbl(invoice.Servicecharge)
                Dim localServiceVat As Double = 0
                Dim localOk As Boolean = True
                Dim localDiscount As Double = CDbl(invoice.LineDiscount)

                'work out the VAT rate for this invoice
                If CDate(invoice.Invoicedate) >= CDate("04/01/2011") Then
                    dblVAT = 0.2
                Else
                    dblVAT = 0.175
                End If

                'tidy up the line description
                invoice.Linedescription = Pack(invoice.Linedescription.Trim.Replace(vbCrLf, " ")).Trim
                invoice.Linedescription = invoice.Linedescription.Replace("PLUS VAT = ", "=")
                invoice.Linedescription = invoice.Linedescription.Replace("PLUS VAT ", " ")
                invoice.Linedescription = invoice.Linedescription.Replace(" PLUS VAT . ", " PLUS VAT.")
                invoice.Linedescription = invoice.Linedescription.Replace(" = ", "=")
                invoice.Linedescription = invoice.Linedescription.Replace(" . ", ".")
                invoice.Linedescription = invoice.Linedescription.Replace(" X ", "x")
                invoice.Linedescription = invoice.Linedescription.Replace(" x ", "x")
                invoice.Linedescription = invoice.Linedescription.Replace(". ", ".")
                invoice.Linedescription = invoice.Linedescription.Replace(" .", ".")
                invoice.Linedescription = invoice.Linedescription.Replace(";", ",")
                invoice.Linedescription = Pack(invoice.Linedescription.Trim.Replace(vbCrLf, " ")).Trim

                'sort out the line numbers
                If nextInvoiceNumberX <> invoice.Invoicenumber Then
                    intLocalLineNumber = 0
                    nextInvoiceNumberX = invoice.Invoicenumber
                End If

                'lets check the vat !!!!
                If invoice.Expense > 0 And invoice.Expensevat > 0 And invoice.Servicecharge > 0 Then
                    Dim Ret As New getCorrectVat
                    Ret = checkVAT(CDbl(invoice.Expense), CDbl(invoice.Expensevat), CDbl(invoice.Servicecharge))
                    localExpense = Ret.pexpense
                    localExpenseVat = Ret.pexpensevat
                    localExpense = Ret.pservice
                    localServiceVat = Ret.pservicevat
                    localOk = Ret.ok
                Else
                    'need to swap VAT from expense to service if ex
                    If localExpenseVat > 0 Then
                        If localExpense = 0 Then
                            If localService = 0 Then
                                localOk = False
                            Else
                                localServiceVat = localExpenseVat
                                localExpenseVat = 0
                            End If
                        End If
                    End If
                End If


                'sort out the header line if we are on the first invoice line
                If intLocalLineNumber = 0 Then


                    strBatch.Append("CF;") 'CF = Invoice, CFR = Invoice Amendment, CFRA = Invoice Annulment
                    strBatch.Append(invoice.Invoicenumber & ";") 'Invoice no
                    strBatch.Append(Format(invoice.Invoicedate, "dd/MM/yyyy") & ";") 'invoice date
                    strBatch.Append("GBP;") 'curency code
                    strBatch.Append(invoice.Po & ";") 'purchase order number
                    strBatch.Append("090D;") 'payment terms
                    strBatch.Append(Replace(invoice.Costcentre, ".", "") & ";") 'Project code
                    strBatch.Append(vbCrLf)

                    strBatch.Append("DC;")
                    strBatch.Append(oPurchaseOrder.mstrCompanyName & ";") 'corporate buyer name
                    strBatch.Append(oPurchaseOrder.mstrCompanyTaxId & ";") 'tax id
                    strBatch.Append(";") 'cost centre
                    strBatch.Append(";") 'email (optional)
                    strBatch.Append(vbCrLf)

                    strBatch.Append("DP;")
                    strBatch.Append("NYS Corporate Ltd;") 'corporate seller name
                    strBatch.Append("371439840;") 'tax id
                    strBatch.Append(";") 'supplier comments on invoice
                    strBatch.Append(vbCrLf)

                    intLocalLineNumber += 1
                End If


                Dim dblQuantity As Double = 0
                Dim dblPrice As Double = 0
                If oPurchaseOrder.mdblQuantity = 1 Then
                    'price = invoice line price
                    'quantity = 1
                    dblPrice = localExpense
                    dblQuantity = 1
                Else
                    'price = 1
                    'quantity = invoice line price
                    dblPrice = 1
                    dblQuantity = localExpense
                End If

                'now line information
                strBatch.Append("LF;")
                strBatch.Append(intLocalLineNumber & ";") 'line number
                strBatch.Append(invoice.Linedescription & ";") 'line description
                strBatch.Append(";") 'supplier ref
                strBatch.Append(";") 'buyer ref
                strBatch.Append(";") 'out of taxable total options(SI = out of total, NO [or blank] = line in total)
                strBatch.Append(vbCrLf)

                strBatch.Append(";") 'blank field
                strBatch.Append(";") 'receipt date
                strBatch.Append(dblQuantity & ";") 'Quantity (should actually be the price of the invoice for us, just as long as the PO has been raised that way around)
                strBatch.Append(dblPrice & ";") 'Price
                strBatch.Append("EA;") 'unit of measure (ISO code)
                strBatch.Append(";") 'delivery note number
                strBatch.Append(";") 'tracking number
                strBatch.Append(";") 'receipt number
                strBatch.Append(";") 'receipt place
                If localExpenseVat = 0 Then
                    strBatch.Append("Zero Rate;") 'tax name ("Standard Rate", or "Zero Rate")
                    strBatch.Append((dblVAT * 100) & ";") 'tax percentage
                Else
                    strBatch.Append("Standard Rate;") 'tax name ("Standard Rate", or "Zero Rate")
                    strBatch.Append((dblVAT * 100) & ";") 'tax percentage
                End If
                strBatch.Append(vbCrLf)

                strBatch.Append(";") 'blank field
                strBatch.Append(";") 'delivery date
                strBatch.Append(";") 'delivery place

                intLocalLineNumber += 1
            End If
        Next

        Return strBatch.ToString
    End Function

    Public Shared Function runO2ToView_Steria() As String

        Dim strBatch As New StringBuilder
        Dim strBatchHeaderX As New StringBuilder
        Dim strBatchFooter As New StringBuilder

        Dim rs As List(Of O2)
        rs = O2.list()

        'need to check the PO to see if it's steria

        Dim nextInvoiceNumberX As String = ""
        Dim lastInvoiceNumberX As String = ""
        Dim strLineDescription As String = ""

        Dim totalnett As Double = 0
        Dim totalvat As Double = 0
        Dim dblInvoiceHeaderTotal As Double = 0
        Dim dblInvoiceHeaderTotal2 As Double = 0
        Dim intInvoiceHeaderCount As Integer = 0
        Dim intInvoiceLineCount As Integer = 0
        Dim intLocalLineNumber As Integer = 0

        For Each r As O2 In rs
            'populate the full details of the order
            Dim oPurchaseOrder As New clsO2Po
            oPurchaseOrder.mstrOrderNumber = r.Po
            oPurchaseOrder = oPurchaseOrder.GetByOrderNumber()
            If oPurchaseOrder.mstrSourceSystemName.ToLower = "steria" Then

                r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " ")).Trim

                r.Linedescription = r.Linedescription.Replace("PLUS VAT = ", "=")
                r.Linedescription = r.Linedescription.Replace("PLUS VAT ", " ")
                r.Linedescription = r.Linedescription.Replace(" PLUS VAT . ", " PLUS VAT.")
                r.Linedescription = r.Linedescription.Replace(" = ", "=")
                r.Linedescription = r.Linedescription.Replace(" . ", ".")
                r.Linedescription = r.Linedescription.Replace(" X ", "x")
                r.Linedescription = r.Linedescription.Replace(" x ", "x")
                r.Linedescription = r.Linedescription.Replace(". ", ".")
                r.Linedescription = r.Linedescription.Replace(" .", ".")
                r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " ")).Trim

                If nextInvoiceNumberX <> r.Invoicenumber Then
                    intLocalLineNumber = 0
                    nextInvoiceNumberX = r.Invoicenumber
                End If

                Dim localExpense As Double = CDbl(r.Expense)
                Dim localExpenseVat As Double = CDbl(r.Expensevat)
                Dim localService As Double = CDbl(r.Servicecharge)
                Dim localServiceVat As Double = 0
                Dim localOk As Boolean = True
                Dim localDiscount As Double = CDbl(r.LineDiscount)

                'lets check the vat !!!!
                If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                    Dim Ret As New getCorrectVat
                    Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                    localExpense = Ret.pexpense
                    localExpenseVat = Ret.pexpensevat
                    localExpense = Ret.pservice
                    localServiceVat = Ret.pservicevat
                    localOk = Ret.ok
                Else
                    'need to swap VAT from expense to service if ex
                    If localExpenseVat > 0 Then
                        If localExpense = 0 Then
                            If localService = 0 Then
                                localOk = False
                            Else
                                localServiceVat = localExpenseVat
                                localExpenseVat = 0
                            End If
                        End If
                    End If
                End If

                If intLocalLineNumber = 0 Then
                    strBatchHeaderX.Append("H,")
                    strBatchHeaderX.Append(r.Invoicenumber & ",")
                    strBatchHeaderX.Append("Total billed=," & r.Invoiceamount & ",")
                    strBatchHeaderX.Append("Total amount=," & r.TotAmount & ",")
                    strBatchHeaderX.Append("Discount=," & r.TotalDiscount & ",")
                    strBatchHeaderX.Append(Format(r.Invoicedate, "yyyy-MM-dd") & ",")
                    strBatchHeaderX.Append(Format(CDate(r.Invoicedate).AddDays(28), "yyyy-MM-dd") & ",")
                    strBatchHeaderX.Append(r.Po & "," & vbCrLf)
                    intInvoiceHeaderCount += 1
                    If r.Linedescription = "" Then
                        strLineDescription = "ZZZZZ"
                    Else
                        strLineDescription = Replace(r.Linedescription, "£", "")
                    End If
                    intLocalLineNumber += 1
                End If

                If localExpense <> 0 And localExpenseVat <> 0 Then
                    strBatch.Append("L,")
                    strBatch.Append(r.Invoicenumber & ",")
                    strBatch.Append(intLocalLineNumber & ",")

                    strBatch.Append(localExpense & ",")
                    If CDate(Format(Now, "dd/MM/yyyy")) > CDate("31/12/2010") Then
                        If CDate(r.Invoicedate) < CDate("04/01/2011") Then
                            strBatch.Append("17.5,")
                        Else
                            strBatch.Append("20,")
                        End If
                    Else
                        strBatch.Append("17.5,")
                    End If

                    strBatch.Append(localExpenseVat & ",")
                    strBatch.Append(localExpense + localExpenseVat & ",")
                    strBatch.Append(getProduct(r.Product, r.identifier) & ",")
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & ",")
                    Else
                        strBatch.Append(Replace(r.Linedescription, "£", "") & ",")
                    End If
                    strBatch.Append("Price=," & (localExpense - localDiscount) + localExpenseVat & "," & vbCrLf)
                    localDiscount = 0
                    intInvoiceLineCount += 1
                    intLocalLineNumber += 1
                ElseIf localExpense <> 0 And localExpenseVat = 0 Then
                    strBatch.Append("L,")
                    strBatch.Append(r.Invoicenumber & ",")
                    strBatch.Append(intLocalLineNumber & ",")

                    strBatch.Append(localExpense & ",")
                    strBatch.Append("0,")
                    strBatch.Append("0,")
                    strBatch.Append(localExpense & ",")
                    strBatch.Append(getProduct(r.Product, r.identifier) & ",")
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & ",")
                    Else
                        strBatch.Append(Replace(r.Linedescription, "£", "") & ",")
                    End If
                    strBatch.Append("Price=," & (localExpense - localDiscount) & "," & vbCrLf)
                    localDiscount = 0
                    intInvoiceLineCount += 1
                    intLocalLineNumber += 1
                End If
                If r.Othercharge <> 0 And r.Othervat <> 0 Then
                    strBatch.Append("L,")
                    strBatch.Append(r.Invoicenumber & ",")
                    strBatch.Append(intLocalLineNumber & ",")

                    strBatch.Append(r.Othercharge & ",")
                    If CDate(Format(Now, "dd/MM/yyyy")) > CDate("31/12/2010") Then
                        If CDate(r.Invoicedate) < CDate("04/01/2011") Then
                            strBatch.Append("17.5,")
                        Else
                            strBatch.Append("20,")
                        End If
                    Else
                        strBatch.Append("17.5,")
                    End If
                    strBatch.Append(r.Othervat & ",")
                    strBatch.Append(r.Othercharge + r.Othervat & ",")
                    strBatch.Append(getProduct(r.Product, r.identifier) & ",")
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & ",")
                    Else
                        strBatch.Append(Replace(r.Linedescription, "£", "") & ",")
                    End If
                    strBatch.Append("Price=," & (r.Othercharge - localDiscount) + r.Othervat & "," & vbCrLf)
                    localDiscount = 0
                    intInvoiceLineCount += 1
                    intLocalLineNumber += 1
                ElseIf r.Othercharge <> 0 And r.Othervat = 0 Then
                    strBatch.Append("L,")
                    strBatch.Append(r.Invoicenumber & ",")
                    strBatch.Append(intLocalLineNumber & ",")

                    strBatch.Append(r.Othercharge & ",")
                    strBatch.Append("0,")
                    strBatch.Append("0,")
                    strBatch.Append(r.Othercharge & ",")
                    strBatch.Append(getProduct(r.Product, r.identifier) & ",")
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & ",")
                    Else
                        strBatch.Append(Replace(r.Linedescription, "£", "") & ",")
                    End If
                    strBatch.Append("Price=," & (r.Othercharge - localDiscount) & "," & vbCrLf)
                    localDiscount = 0
                    intInvoiceLineCount += 1
                    intLocalLineNumber += 1
                End If
                If localService <> 0 And localServiceVat <> 0 Then
                    strBatch.Append("L,")
                    strBatch.Append(r.Invoicenumber & ",")
                    strBatch.Append(intLocalLineNumber & ",")

                    strBatch.Append(localService & ",")
                    If CDate(Format(Now, "dd/MM/yyyy")) > CDate("31/12/2010") Then
                        If CDate(r.Invoicedate) < CDate("04/01/2011") Then
                            strBatch.Append("17.5,")
                        Else
                            strBatch.Append("20,")
                        End If
                    Else
                        strBatch.Append("17.5,")
                    End If
                    strBatch.Append(localServiceVat & ",")
                    strBatch.Append(localService + localServiceVat & ",")
                    strBatch.Append(getProduct(r.Product, r.identifier) & ",")
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & ",")
                    Else
                        strBatch.Append(Replace(r.Linedescription, "£", "") & ",")
                    End If
                    strBatch.Append("Price=," & (localService - localDiscount) + localServiceVat & "," & vbCrLf)
                    localDiscount = 0
                    intInvoiceLineCount += 1
                    intLocalLineNumber += 1
                ElseIf localService <> 0 And localServiceVat = 0 Then
                    strBatch.Append("L,")
                    strBatch.Append(r.Invoicenumber & ",")
                    strBatch.Append(intLocalLineNumber & ",")

                    strBatch.Append(localService & ",")
                    strBatch.Append(",")
                    strBatch.Append("0,")
                    strBatch.Append(localService & ",")
                    strBatch.Append(getProduct(r.Product, r.identifier) & ",")
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & ",")
                    Else
                        strBatch.Append(Replace(r.Linedescription, "£", "") & ",")
                    End If
                    strBatch.Append("Price=," & (localService - localDiscount) & "," & vbCrLf)
                    localDiscount = 0
                    intInvoiceLineCount += 1
                    intLocalLineNumber += 1
                Else
                    Dim inn As Integer = 1
                End If
                If r.Airlinecharge > 0 Then
                    strBatch.Append("L,")
                    strBatch.Append(r.Invoicenumber & ",")
                    strBatch.Append(intLocalLineNumber & ",")

                    strBatch.Append(r.Airlinecharge & ",")
                    strBatch.Append(",")
                    strBatch.Append("0,")
                    strBatch.Append(r.Airlinecharge & ",")
                    strBatch.Append(getProduct(r.Product, r.identifier) & ",")

                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & ",")
                    Else
                        strBatch.Append(Replace(r.Linedescription, "£", "") & ",")
                    End If

                    strBatch.Append("Price=," & (r.Airlinecharge - localDiscount) & "," & vbCrLf)
                    localDiscount = 0
                    intInvoiceLineCount += 1
                    intLocalLineNumber += 1
                End If
                lastInvoiceNumberX = r.Invoicenumber
                dblInvoiceHeaderTotal = CDbl(dblInvoiceHeaderTotal + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge)
                dblInvoiceHeaderTotal2 = CDbl((dblInvoiceHeaderTotal2 + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge) - r.LineDiscount)
            End If

        Next

        Dim strInvoiceTrailer As New StringBuilder
        Dim strReturn As New StringBuilder
        strInvoiceTrailer.Append("T," & CStr(intInvoiceHeaderCount) & "," & CStr(intInvoiceLineCount) & ",Total Amount=," & Format(dblInvoiceHeaderTotal, "0.00") & vbCrLf)
        strInvoiceTrailer.Append("T," & CStr(intInvoiceHeaderCount) & "," & CStr(intInvoiceLineCount) & ",Total Billed=," & Format(dblInvoiceHeaderTotal2, "0.00"))

        strReturn.Append(strBatchHeaderX.ToString)
        strReturn.Append(strBatch.ToString)
        strReturn.Append(strInvoiceTrailer.ToString)

        Return strReturn.ToString
    End Function

    Public Shared Function getCima(ByVal pcimacode As Integer) As String
        Using dbh As New SqlDatabaseHandle(getCubitConnection)
            Using r As IDataReader = dbh.callSP("cima_get", "@cimacode", pcimacode)
                Dim strRet As String = ""
                If r.Read() Then
                    strRet = clsNYS.notString(r.Item("directorate"))
                End If
                Return strRet
            End Using
        End Using
    End Function

    'CR
    Public Shared Function createFileCIMAToView(ByVal pstartdate As String, ByVal penddate As String) As String

        'go get details of invoices here
        Dim strBatch As New StringBuilder
        Dim strBatchHeaderX As New StringBuilder
        Dim strBatchFooter As New StringBuilder
        Dim strErrorLines As New StringBuilder
        Dim strPOErrors As New StringBuilder
        Dim strProductErrors As New StringBuilder
        Dim intLineNo As Integer = 1

        'do loop here
        Dim rs As List(Of CIMA)
        rs = CIMA.FeederFileCIMA(pstartdate, penddate)

        Dim nextInvoiceNumber As String = ""
        Dim lastInvoiceNumber As String = ""
        Dim strLineDescription As String = ""

        Dim totalnett As Double = 0
        Dim totalvat As Double = 0
        Dim dblInvoiceHeaderTotal As Double = 0
        Dim dblInvoiceHeaderTotal2 As Double = 0
        Dim intInvoiceHeaderCount As Integer = 0
        Dim intInvoiceLineCount As Integer = 0

        'lets cycle through and check Cost Code is correct
        For Each r As CIMA In rs
            Try
                Dim intTest As Integer = CInt(Mid(r.Costcentre, 1, 3))
                If getCima(intTest) = "" Then
                    strPOErrors.Append(r.Invoicenumber & " : " & r.Costcentre & vbCrLf)
                End If
            Catch ex As Exception
                strPOErrors.Append(r.Invoicenumber & " : " & r.Costcentre & vbCrLf)
            End Try
            'check for a correct product code
            If getProduct(r.Product, r.identifier) = "" Then
                strProductErrors.Append(r.Invoicenumber & " : " & r.Product & vbCrLf)
            End If
        Next

        If strPOErrors.ToString <> "" Then
            If strProductErrors.ToString <> "" Then
                Return "ERROR - THE FOLLOWING INVOICES HAVE INCORRECT COST CODES:" & vbCrLf & strPOErrors.ToString & vbCrLf & vbCrLf & _
                        "ERROR - THE FOLLOWING INVOICES HAVE NOT MATCHED THE PRODUCT CODE CORRECTLY:" & vbCrLf & strProductErrors.ToString
            Else
                Return "ERROR - THE FOLLOWING INVOICES HAVE INCORRECT COST CODES:" & vbCrLf & strPOErrors.ToString
            End If
        Else
            If strProductErrors.ToString <> "" Then
                Return "ERROR - THE FOLLOWING INVOICES HAVE NOT MATCHED THE PRODUCT CODE CORRECTLY:" & vbCrLf & strProductErrors.ToString
            End If
        End If

        For Each r As CIMA In rs
            If r.Invoicenumber = "N508459" Or r.Invoicenumber = "N487279" Then
                Dim test As Integer = 0
            End If
            nextInvoiceNumber = r.Invoicenumber
            Dim localExpense As Double = CDbl(r.Expense)
            Dim localExpenseVat As Double = CDbl(r.Expensevat)
            Dim localService As Double = CDbl(r.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            Dim localDiscount As Double = CDbl(r.LineDiscount)

            r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " ")).Trim
            r.Linedescription = r.Linedescription.Replace("&", "and")

            'need to split the cost centre
            Dim strCCLocal As String = r.Costcentre
            Dim strCCValue As String = Mid(strCCLocal, 1, 3)
            Dim strActivityValue As String = Mid(strCCLocal, 10, 5)
            Dim strNatAccountValue As String = Mid(strCCLocal, 5, 4)

            'R2.13 CR - if new format cost centre
            ' new tactic number, change activity number
            Dim strCCTacticNumber As String = ""
            If strCCLocal.Length > 14 Then
                Dim strRemainingCC As String = Mid(strCCLocal, 10)
                strCCTacticNumber = Mid(strRemainingCC, 1, strRemainingCC.IndexOf("-"))
                strActivityValue = Mid(strRemainingCC, strRemainingCC.IndexOf("-") + 2, 5)
            End If


            'lets check the vat !!!!
            If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localService = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat > 0 Then
                    If localExpense = 0 Then
                        If localService = 0 Then
                            localOk = False
                        Else
                            localServiceVat = localExpenseVat
                            localExpenseVat = 0
                        End If
                    End If
                End If
            End If

            'add sanity check for minus figures for airline charges as NYS don't want to show cancel charges for air tickets
            If r.Airlinecharge < 0 Then
                localExpense = CDbl(localExpense + r.Airlinecharge)
                r.Airlinecharge = 0
            End If

            If Not localOk Then
                strErrorLines.Append(r.Invoicenumber & " line: " & r.Linenumber & vbCrLf)
            End If

            If r.Linenumber = 1 Then
                strBatchHeaderX.Append("H|")
                strBatchHeaderX.Append(r.Invoicenumber & "|")
                strBatchHeaderX.Append("Total billed=|" & r.Invoiceamount & "|")
                strBatchHeaderX.Append("Total amount=|" & r.TotAmount & "|")
                strBatchHeaderX.Append("Discount=|" & r.TotalDiscount & "|")
                strBatchHeaderX.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                strBatchHeaderX.Append(Format(CDate(r.Invoicedate).AddDays(28), "yyyy-MM-dd") & "|")
                strBatchHeaderX.Append(r.Po & "|" & vbCrLf)
                intInvoiceHeaderCount += 1
                If r.Linedescription = "" Then
                    strLineDescription = getProduct(r.Product, r.identifier)
                Else
                    If r.StartDate = r.EndDate Then
                        strLineDescription = Mid(Replace(r.Linedescription, "£", "") & " Date:" & r.StartDate, 1, 500)
                    Else
                        strLineDescription = Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate, 1, 500)
                    End If
                End If
                If r.Product.ToLower = "other" Then
                    strLineDescription = strLineDescription & " visa"
                End If
            End If

            If localExpense <> 0 And localExpenseVat <> 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(r.Linenumber & "|")
                strBatch.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                intLineNo += 1
                strBatch.Append(localExpense & "|")
                strBatch.Append("17.5|")
                strBatch.Append(localExpenseVat & "|")
                strBatch.Append(localExpense + localExpenseVat & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Product.ToLower = "other" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & "|")
                    Else
                        strBatch.Append(Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate & "|", 1, 500))
                    End If
                End If
                strBatch.Append("CostCentre=|" & r.Costcentre & "|")
                strBatch.Append("CC=|" & strCCValue & "|")
                strBatch.Append("Activity=|" & strActivityValue & "|")
                strBatch.Append("NatAccount=|" & strNatAccountValue & "|")

                'R2.13 CR
                strBatch.Append("Tactic=|" & strCCTacticNumber & "|")

                strBatch.Append("Price=|" & (localExpense - localDiscount) + localExpenseVat & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            ElseIf localExpense <> 0 And localExpenseVat = 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(r.Linenumber & "|")
                strBatch.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                intLineNo += 1
                strBatch.Append(localExpense & "|")
                strBatch.Append("0|")
                strBatch.Append("0|")
                strBatch.Append(localExpense & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Product.ToLower = "other" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & "|")
                    Else
                        strBatch.Append(Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate & "|", 1, 500))
                    End If
                End If
                strBatch.Append("CostCentre=|" & r.Costcentre & "|")
                strBatch.Append("CC=|" & strCCValue & "|")
                strBatch.Append("Activity=|" & strActivityValue & "|")
                strBatch.Append("NatAccount=|" & strNatAccountValue & "|")

                'R2.13 CR
                strBatch.Append("Tactic=|" & strCCTacticNumber & "|")

                strBatch.Append("Price=|" & (localExpense - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            End If
            If r.Othercharge <> 0 And r.Othervat <> 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(r.Linenumber & "|")
                strBatch.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                intLineNo += 1
                strBatch.Append(r.Othercharge & "|")
                strBatch.Append("17.5|")
                strBatch.Append(r.Othervat & "|")
                strBatch.Append(r.Othercharge + r.Othervat & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Product.ToLower = "other" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & "|")
                    Else
                        strBatch.Append(Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate & "|", 1, 500))
                    End If
                End If
                strBatch.Append("CostCentre=|" & r.Costcentre & "|")
                strBatch.Append("CC=|" & strCCValue & "|")
                strBatch.Append("Activity=|" & strActivityValue & "|")
                strBatch.Append("NatAccount=|" & strNatAccountValue & "|")

                'R2.13 CR
                strBatch.Append("Tactic=|" & strCCTacticNumber & "|")

                strBatch.Append("Price=|" & (r.Othercharge - localDiscount) + r.Othervat & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            ElseIf r.Othercharge <> 0 And r.Othervat = 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(r.Linenumber & "|")
                strBatch.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                intLineNo += 1
                strBatch.Append(r.Othercharge & "|")
                strBatch.Append("0|")
                strBatch.Append("0|")
                strBatch.Append(r.Othercharge & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Product.ToLower = "other" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & "|")
                    Else
                        strBatch.Append(Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate & "|", 1, 500))
                    End If
                End If
                strBatch.Append("CostCentre=|" & r.Costcentre & "|")
                strBatch.Append("CC=|" & strCCValue & "|")
                strBatch.Append("Activity=|" & strActivityValue & "|")
                strBatch.Append("NatAccount=|" & strNatAccountValue & "|")

                'R2.13 CR
                strBatch.Append("Tactic=|" & strCCTacticNumber & "|")

                strBatch.Append("Price=|" & (r.Othercharge - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            End If
            If localService <> 0 And localServiceVat <> 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(r.Linenumber & "|")
                strBatch.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                intLineNo += 1
                strBatch.Append(localService & "|")
                strBatch.Append("17.5|")
                strBatch.Append(localServiceVat & "|")
                strBatch.Append(localService + localServiceVat & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Product.ToLower = "other" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & "|")
                    Else
                        strBatch.Append(Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate & "|", 1, 500))
                    End If
                End If
                strBatch.Append("CostCentre=|" & r.Costcentre & "|")
                strBatch.Append("CC=|" & strCCValue & "|")
                strBatch.Append("Activity=|" & strActivityValue & "|")
                strBatch.Append("NatAccount=|" & strNatAccountValue & "|")

                'R2.13 CR
                strBatch.Append("Tactic=|" & strCCTacticNumber & "|")

                strBatch.Append("Price=|" & (localService - localDiscount) + localServiceVat & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            ElseIf localService <> 0 And localServiceVat = 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(r.Linenumber & "|")
                strBatch.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                intLineNo += 1
                strBatch.Append(localService & "|")
                strBatch.Append("|")
                strBatch.Append("0|")
                strBatch.Append(localService & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Product.ToLower = "other" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & "|")
                    Else
                        strBatch.Append(Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate & "|", 1, 500))
                    End If
                End If
                strBatch.Append("CostCentre=|" & r.Costcentre & "|")
                strBatch.Append("CC=|" & strCCValue & "|")
                strBatch.Append("Activity=|" & strActivityValue & "|")
                strBatch.Append("NatAccount=|" & strNatAccountValue & "|")

                'R2.13 CR
                strBatch.Append("Tactic=|" & strCCTacticNumber & "|")

                strBatch.Append("Price=|" & (localService - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            Else
                Dim inn As Integer = 1
            End If
            If r.Airlinecharge > 0 Then
                strBatch.Append("L|")
                strBatch.Append(r.Invoicenumber & "|")
                strBatch.Append(r.Linenumber & "|")
                strBatch.Append(Format(r.Invoicedate, "yyyy-MM-dd") & "|")
                intLineNo += 1
                strBatch.Append(r.Airlinecharge & "|")
                strBatch.Append("|")
                strBatch.Append("0|")
                strBatch.Append(r.Airlinecharge & "|")
                strBatch.Append(getProduct(r.Product, r.identifier) & "|")
                If r.Product.ToLower = "other" Then
                    strBatch.Append(strLineDescription & "|")
                Else
                    If r.Linedescription = "" Then
                        strBatch.Append(strLineDescription & "|")
                    Else
                        strBatch.Append(Mid(Replace(r.Linedescription, "£", "") & " Dates:" & r.StartDate & "-" & r.EndDate & "|", 1, 500))
                    End If
                End If
                strBatch.Append("CostCentre=|" & r.Costcentre & "|")
                strBatch.Append("CC=|" & strCCValue & "|")
                strBatch.Append("Activity=|" & strActivityValue & "|")
                strBatch.Append("NatAccount=|" & strNatAccountValue & "|")

                'R2.13 CR
                strBatch.Append("Tactic=|" & strCCTacticNumber & "|")

                strBatch.Append("Price=|" & (r.Airlinecharge - localDiscount) & "|" & vbCrLf)
                localDiscount = 0
                intInvoiceLineCount += 1

            End If
            lastInvoiceNumber = r.Invoicenumber
            dblInvoiceHeaderTotal = CDbl(dblInvoiceHeaderTotal + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge)
            dblInvoiceHeaderTotal2 = CDbl((dblInvoiceHeaderTotal2 + localExpense + localExpenseVat + r.Othercharge + r.Othervat + localService + localServiceVat + r.Airlinecharge) - r.LineDiscount)

        Next

        If strErrorLines.ToString <> "" Then
            Return "ERROR - THE FOLLOWING INVOICES INCORRECT VAT ISSUES:" & vbCrLf & strErrorLines.ToString
        End If

        Dim strInvoiceTrailer As New StringBuilder
        Dim strReturn As New StringBuilder
        strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total Amount=|" & Format(dblInvoiceHeaderTotal, "0.00") & vbCrLf)
        strInvoiceTrailer.Append("T|" & CStr(intInvoiceHeaderCount) & "|" & CStr(intInvoiceLineCount) & "|Total Billed=|" & Format(dblInvoiceHeaderTotal2, "0.00"))

        strReturn.Append(strBatchHeaderX.ToString)
        strReturn.Append(strBatch.ToString)
        strReturn.Append(strInvoiceTrailer.ToString)

        Return strReturn.ToString
    End Function

    'CR
    Private Sub createPhysicalFileCIMA(ByVal pInvno As String)
        'TODO: change class ref and Stored proceedure call
        'add stored proceedure to db

        'go get details of invoices here

        If pInvno = "N520286" Then
            Dim ist As Integer = 0
        End If
        Dim strBatch As New StringBuilder
        Dim strBatchHeader As New StringBuilder
        Dim strBatchFooter As New StringBuilder
        Dim strErrorLines As New StringBuilder

        Dim intLineNoX As Integer = 1

        strBatch.Append("<cXML payloadID=""uniquedocumentid@nystravel.co.uk"" timestamp=""" & Format(Now, "yyyy-MM-dd") & "T" & Format(Now, "hh:mm:ss") & "+00:00"" xml:lang=""en-GB"" version=""1.2.011"">" & vbNewLine)
        'strBatch.Append("<ABWInvoice xmlns=""http://services.agresso.com/schema/ABWInvoice/2007/12/24"" xmlns:agrlib=""http://services.agresso.com/schema/ABWSchemaLib/2007/12/24"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://services.agresso.com/schema/ABWInvoice/2007/12/24 http://services.agresso.com/schema/ABWInvoice/2007/12/24/ABWInvoice.xsd"">" & vbNewLine)
        strBatch.Append("<Header>" & vbNewLine)
        strBatch.Append("<From>" & vbNewLine)
        strBatch.Append("<Credential domain=""DUNS"">" & vbNewLine)
        strBatch.Append("<Identity>DUNS Number If Applicable</Identity>" & vbNewLine)
        strBatch.Append("</Credential>" & vbNewLine)
        strBatch.Append("</From>" & vbNewLine)

        strBatch.Append("<To>" & vbNewLine)
        strBatch.Append("<Credential domain=""NetworkId"">" & vbNewLine)
        strBatch.Append("<Identity>NYS Company ID</Identity>" & vbNewLine)
        strBatch.Append("</Credential>" & vbNewLine)
        strBatch.Append("</To>" & vbNewLine)

        strBatch.Append("<Sender>" & vbNewLine)
        strBatch.Append("<Credential domain=""DUNS"">" & vbNewLine)
        strBatch.Append("<Identity>DUNS Number If Applicable</Identity>" & vbNewLine)
        strBatch.Append("<SharedSecret>password</SharedSecret>" & vbNewLine)
        strBatch.Append("</Credential>" & vbNewLine)
        strBatch.Append("<UserAgent>NYS Travel cXML Invoicing</UserAgent>" & vbNewLine)
        strBatch.Append("</Sender>" & vbNewLine)
        strBatch.Append("</Header>" & vbNewLine)

        strBatch.Append("<Request deploymentMode=""test"">" & vbNewLine)
        strBatch.Append("<InvoiceDetailRequest>" & vbNewLine)

        If pInvno = "N472057" Then
            Dim i As Integer = 90
        End If
        'do loop here
        Dim rs As List(Of CIMA)
        rs = CIMA.FeederFileCIMAIndividual(pInvno)

        pInvno = pInvno & getConfig("CIMATestValue")
        Dim strLineDescription As String = ""

        Dim totalnett As Double = 0
        Dim totalvat As Double = 0

        Dim blnCEL As Boolean = False

        For Each r As CIMA In rs

            If r.Costcentre.StartsWith("201") Or _
                r.Costcentre.StartsWith("204") Or _
                r.Costcentre.StartsWith("207") Or _
                r.Costcentre.StartsWith("214") Or _
                r.Costcentre.StartsWith("512") Then
                blnCEL = True
            End If

            Dim localExpense As Double = CDbl(r.Expense)
            Dim localExpenseVat As Double = CDbl(r.Expensevat)
            Dim localService As Double = CDbl(r.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            Dim localDiscount As Double = CDbl(r.LineDiscount)

            If r.Linedescription.Trim = "" Then
                If strLineDescription = "" Then
                    strLineDescription = getProduct(r.Product, r.identifier)
                End If
            Else
                strLineDescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " "))
                strLineDescription = Mid(strLineDescription.Replace("&", "and"), 1, 500)
            End If
            If r.Product.ToLower = "other" Then
                strLineDescription = strLineDescription & " visa payment"
            End If
            'r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " "))
            'strLineDescription = r.Linedescription
            'strLineDescription = strLineDescription.Replace("&", "and")

            'need to split the cost centre
            Dim strCCLocal As String = r.Costcentre
            Dim strCCValue As String = Mid(strCCLocal, 1, 3)
            Dim strNatAccountValue As String = Mid(strCCLocal, 5, 4)
            Dim strActivityValue As String = Mid(strCCLocal, 10, 5)

            'R2.13 CR - if new format cost centre
            ' new tactic number, change activity number
            Dim strCCTacticNumber As String = ""
            If strCCLocal.Length > 15 Then
                'R2.16 CR - CIMA changed the format without telling us, now 123-1234-12345-(optional 12- OR 123- OR 1234-)
                'Dim strRemainingCC As String = Mid(strCCLocal, 10)
                'strCCTacticNumber = Mid(strRemainingCC, 1, strRemainingCC.IndexOf("-"))
                'strActivityValue = Mid(strRemainingCC, strRemainingCC.IndexOf("-") + 2, 5)
                strCCTacticNumber = Replace(Mid(strCCLocal, 16), "-", "")
            End If


            'lets check the vat !!!!
            If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localService = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat > 0 Then
                    If localExpense = 0 Then
                        If localService = 0 Then
                            localOk = False
                        Else
                            localServiceVat = localExpenseVat
                            localExpenseVat = 0
                        End If
                    End If
                End If
            End If

            If localExpense > 0 And localDiscount > 0 Then
                localExpense = localExpense - localDiscount
            End If

            If r.Airlinecharge < 0 Then
                localExpense = CDbl(localExpense + r.Airlinecharge)
            End If

            If Not localOk Then
                strErrorLines.Append(r.Invoicenumber & " line: " & r.Linenumber & vbNewLine)
            End If

            totalnett = CDbl(totalnett + localExpense + localService + r.Othercharge + r.Airlinecharge) ' - localDiscount)
            totalvat = CDbl(totalvat + localExpenseVat + localServiceVat + r.Othervat)

            'R2.16 CR - BUG FIX
            'If r.LineNumber = 1 Then
            If intLineNoX = 1 Then
                strBatch.Append("<InvoiceDetailRequestHeader invoiceID=""" & pInvno & """ purpose=""standard"" operation=""new"" invoiceDate=""" & Format(CDate(r.Invoicedate), "yyyy-MM-dd") & """>" & vbNewLine)
                strBatch.Append("<InvoiceDetailHeaderIndicator/>" & vbNewLine)
                strBatch.Append("<InvoiceDetailLineIndicator isTaxInLine=""yes"" isSpecialHandlingInLine="""" isShippingInLine="""" isAccountingInLine="""" />" & vbNewLine)
                strBatch.Append("</InvoiceDetailRequestHeader>" & vbNewLine)
                strBatch.Append("<InvoiceDetailOrder>" & vbNewLine)
                strBatch.Append("<InvoiceDetailOrderInfo>" & vbNewLine)
                strBatch.Append("<OrderReference orderID=""" & r.Po & """ orderDate=""" & Format(r.orderdate, "yyyy-MM-dd") & """ />" & vbNewLine)
                strBatch.Append("<OrderIDInfo orderID=""" & r.Po & """ />" & vbNewLine)
                strBatch.Append("<SupplierOrderInfo orderID=""" & r.Po & """ />" & vbNewLine)
                strBatch.Append("</InvoiceDetailOrderInfo>" & vbNewLine)
            End If

            'R2.17 CR - FIX (not a bug from our end)
            ' CIMA have accidentally changed the HOTEL product code at their end 
            Dim strProductCode As String = getProduct(r.Product, r.identifier)
            If strProductCode = "HOTEL" Then
                strProductCode = "NYSHOTEL"
            End If

            If localExpense <> 0 Then
                'R2.13 CR - changed strNatAccountValue to Element5, added strCCTacticNumber as Element4
                strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strCCTacticNumber & """ AccountingElement5=""" & strNatAccountValue & """>" & vbNewLine)
                'strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strNatAccountValue & """>" & vbNewLine)
                strBatch.Append("<UnitOfMeasure>EA</UnitOfMeasure>" & vbNewLine)

                strBatch.Append("<UnitPrice>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localExpense & "</Money>" & vbNewLine)
                strBatch.Append("</UnitPrice>" & vbNewLine)

                strBatch.Append("<InvoiceDetailItemReference lineNumber=""" & intLineNoX & """>" & vbNewLine)

                strBatch.Append("<ItemID>" & vbNewLine)
                strBatch.Append("<SupplierPartID>" & strProductCode & "</SupplierPartID>" & vbNewLine)
                strBatch.Append("</ItemID>" & vbNewLine)

                strBatch.Append("<Description xml:lang=""en-GB""><![CDATA[" & strLineDescription & "]]></Description>" & vbNewLine)
                strBatch.Append("</InvoiceDetailItemReference>" & vbNewLine)

                strBatch.Append("<SubtotalAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localExpense & "</Money>" & vbNewLine)
                strBatch.Append("</SubtotalAmount>" & vbNewLine)

                strBatch.Append("<Tax>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localExpenseVat & "</Money>" & vbNewLine)
                strBatch.Append("<Description xml:lang=""en-GB"">Value Added Tax</Description>" & vbNewLine)

                'R2.17 - BUG FIX, take the VAT rate from the config file
                'strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""17.5"">" & vbNewLine)
                strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""" & getConfig("VatRate") & """>" & vbNewLine)

                strBatch.Append("<TaxableAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localExpense & "</Money>" & vbNewLine)
                strBatch.Append("</TaxableAmount>" & vbNewLine)
                strBatch.Append("<TaxAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localExpenseVat & "</Money>" & vbNewLine)
                strBatch.Append("</TaxAmount>" & vbNewLine)
                strBatch.Append("</TaxDetail>" & vbNewLine)
                strBatch.Append("</Tax>" & vbNewLine)

                strBatch.Append("<GrossAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localExpense + localExpenseVat & "</Money>" & vbNewLine)
                strBatch.Append("</GrossAmount>" & vbNewLine)

                strBatch.Append("<NetAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localExpense & "</Money>" & vbNewLine)
                strBatch.Append("</NetAmount>" & vbNewLine)

                strBatch.Append("</InvoiceDetailItem>" & vbNewLine)
                intLineNoX += 1
            End If

            If r.Othercharge <> 0 Then
                'R2.13 CR - changed strNatAccountValue to Element5, added strCCTacticNumber as Element4
                strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strCCTacticNumber & """ AccountingElement5=""" & strNatAccountValue & """>" & vbNewLine)
                'strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strNatAccountValue & """>" & vbNewLine)
                strBatch.Append("<UnitOfMeasure>EA</UnitOfMeasure>" & vbNewLine)

                strBatch.Append("<UnitPrice>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Othercharge & "</Money>" & vbNewLine)
                strBatch.Append("</UnitPrice>" & vbNewLine)

                strBatch.Append("<InvoiceDetailItemReference lineNumber=""" & intLineNoX & """>" & vbNewLine)

                strBatch.Append("<ItemID>" & vbNewLine)
                strBatch.Append("<SupplierPartID>" & strProductCode & "</SupplierPartID>" & vbNewLine)
                strBatch.Append("</ItemID>" & vbNewLine)

                strBatch.Append("<Description xml:lang=""en-GB""><![CDATA[" & strLineDescription & "]]></Description>" & vbNewLine)
                strBatch.Append("</InvoiceDetailItemReference>" & vbNewLine)

                strBatch.Append("<SubtotalAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Othercharge & "</Money>" & vbNewLine)
                strBatch.Append("</SubtotalAmount>" & vbNewLine)

                strBatch.Append("<Tax>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Othervat & "</Money>" & vbNewLine)
                strBatch.Append("<Description xml:lang=""en-GB"">Value Added Tax</Description>" & vbNewLine)

                'R2.17 - BUG FIX, take the VAT rate from the config file
                'strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""17.5"">" & vbNewLine)
                strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""" & getConfig("VatRate") & """>" & vbNewLine)

                strBatch.Append("<TaxableAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Othercharge & "</Money>" & vbNewLine)
                strBatch.Append("</TaxableAmount>" & vbNewLine)
                strBatch.Append("<TaxAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Othervat & "</Money>" & vbNewLine)
                strBatch.Append("</TaxAmount>" & vbNewLine)
                strBatch.Append("</TaxDetail>" & vbNewLine)
                strBatch.Append("</Tax>" & vbNewLine)

                strBatch.Append("<GrossAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Othercharge + r.Othervat & "</Money>" & vbNewLine)
                strBatch.Append("</GrossAmount>" & vbNewLine)

                strBatch.Append("<NetAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Othercharge & "</Money>" & vbNewLine)
                strBatch.Append("</NetAmount>" & vbNewLine)

                strBatch.Append("</InvoiceDetailItem>" & vbNewLine)
                intLineNoX += 1
            End If

            If localService <> 0 Then
                'R2.13 CR - changed strNatAccountValue to Element5, added strCCTacticNumber as Element4
                strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strCCTacticNumber & """ AccountingElement5=""" & strNatAccountValue & """>" & vbNewLine)
                'strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strNatAccountValue & """>" & vbNewLine)
                strBatch.Append("<UnitOfMeasure>EA</UnitOfMeasure>" & vbNewLine)

                strBatch.Append("<UnitPrice>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localService & "</Money>" & vbNewLine)
                strBatch.Append("</UnitPrice>" & vbNewLine)

                strBatch.Append("<InvoiceDetailItemReference lineNumber=""" & intLineNoX & """>" & vbNewLine)

                strBatch.Append("<ItemID>" & vbNewLine)
                strBatch.Append("<SupplierPartID>" & strProductCode & "</SupplierPartID>" & vbNewLine)
                strBatch.Append("</ItemID>" & vbNewLine)

                strBatch.Append("<Description xml:lang=""en-GB""><![CDATA[" & strLineDescription & "]]></Description>" & vbNewLine)
                strBatch.Append("</InvoiceDetailItemReference>" & vbNewLine)

                strBatch.Append("<SubtotalAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localService & "</Money>" & vbNewLine)
                strBatch.Append("</SubtotalAmount>" & vbNewLine)

                strBatch.Append("<Tax>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localServiceVat & "</Money>" & vbNewLine)
                strBatch.Append("<Description xml:lang=""en-GB"">Value Added Tax</Description>" & vbNewLine)

                'R2.17 - BUG FIX, take the VAT rate from the config file
                'strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""17.5"">" & vbNewLine)
                strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""" & getConfig("VatRate") & """>" & vbNewLine)

                strBatch.Append("<TaxableAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localService & "</Money>" & vbNewLine)
                strBatch.Append("</TaxableAmount>" & vbNewLine)
                strBatch.Append("<TaxAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localServiceVat & "</Money>" & vbNewLine)
                strBatch.Append("</TaxAmount>" & vbNewLine)
                strBatch.Append("</TaxDetail>" & vbNewLine)
                strBatch.Append("</Tax>" & vbNewLine)

                strBatch.Append("<GrossAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localService + localServiceVat & "</Money>" & vbNewLine)
                strBatch.Append("</GrossAmount>" & vbNewLine)

                strBatch.Append("<NetAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & localService & "</Money>" & vbNewLine)
                strBatch.Append("</NetAmount>" & vbNewLine)

                strBatch.Append("</InvoiceDetailItem>" & vbNewLine)
                intLineNoX += 1
            End If

            If r.Airlinecharge > 0 Then
                'R2.13 CR - changed strNatAccountValue to Element5, added strCCTacticNumber as Element4
                strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strCCTacticNumber & """ AccountingElement5=""" & strNatAccountValue & """>" & vbNewLine)
                'strBatch.Append("<InvoiceDetailItem invoiceLineNumber=""" & intLineNoX & """ quantity="""" AccountingElement1=""" & strCCValue & """ AccountingElement2 = """" AccountingElement3=""" & strActivityValue & """ AccountingElement4=""" & strNatAccountValue & """>" & vbNewLine)
                strBatch.Append("<UnitOfMeasure>EA</UnitOfMeasure>" & vbNewLine)

                strBatch.Append("<UnitPrice>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Airlinecharge & "</Money>" & vbNewLine)
                strBatch.Append("</UnitPrice>" & vbNewLine)

                strBatch.Append("<InvoiceDetailItemReference lineNumber=""" & intLineNoX & """>" & vbNewLine)

                strBatch.Append("<ItemID>" & vbNewLine)
                strBatch.Append("<SupplierPartID>" & strProductCode & "</SupplierPartID>" & vbNewLine)
                strBatch.Append("</ItemID>" & vbNewLine)

                strBatch.Append("<Description xml:lang=""en-GB""><![CDATA[" & strLineDescription & "]]></Description>" & vbNewLine)
                strBatch.Append("</InvoiceDetailItemReference>" & vbNewLine)

                strBatch.Append("<SubtotalAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Airlinecharge & "</Money>" & vbNewLine)
                strBatch.Append("</SubtotalAmount>" & vbNewLine)

                strBatch.Append("<Tax>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">0</Money>" & vbNewLine)
                strBatch.Append("<Description xml:lang=""en-GB"">Value Added Tax</Description>" & vbNewLine)
                strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""0"">" & vbNewLine)
                strBatch.Append("<TaxableAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Airlinecharge & "</Money>" & vbNewLine)
                strBatch.Append("</TaxableAmount>" & vbNewLine)
                strBatch.Append("<TaxAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">0</Money>" & vbNewLine)
                strBatch.Append("</TaxAmount>" & vbNewLine)
                strBatch.Append("</TaxDetail>" & vbNewLine)
                strBatch.Append("</Tax>" & vbNewLine)

                strBatch.Append("<GrossAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Airlinecharge & "</Money>" & vbNewLine)
                strBatch.Append("</GrossAmount>" & vbNewLine)

                strBatch.Append("<NetAmount>" & vbNewLine)
                strBatch.Append("<Money currency=""GBP"">" & r.Airlinecharge & "</Money>" & vbNewLine)
                strBatch.Append("</NetAmount>" & vbNewLine)

                strBatch.Append("</InvoiceDetailItem>" & vbNewLine)
                intLineNoX += 1

            End If

        Next

        strBatch.Append("</InvoiceDetailOrder>" & vbNewLine)
        strBatch.Append("<InvoiceDetailSummary>" & vbNewLine)
        strBatch.Append("<SubtotalAmount>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & totalnett & "</Money>" & vbNewLine)
        strBatch.Append("</SubtotalAmount>" & vbNewLine)

        strBatch.Append("<Tax>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & totalvat & "</Money>" & vbNewLine)
        strBatch.Append("<Description xml:lang=""en-GB"" />" & vbNewLine)

        'R2.17 - BUG FIX, take the VAT rate from the config file
        'strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""17.5"">" & vbNewLine)
        strBatch.Append("<TaxDetail purpose=""tax"" category=""Standard"" percentageRate=""" & getConfig("VatRate") & """>" & vbNewLine)

        strBatch.Append("<TaxableAmount>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & totalnett & "</Money>" & vbNewLine)
        strBatch.Append("</TaxableAmount>" & vbNewLine)
        strBatch.Append("<TaxAmount>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & totalvat & "</Money>" & vbNewLine)
        strBatch.Append("</TaxAmount>" & vbNewLine)
        strBatch.Append("</TaxDetail>" & vbNewLine)
        strBatch.Append("</Tax>" & vbNewLine)

        strBatch.Append("<ShippingAmount>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & "</Money>" & vbNewLine)
        strBatch.Append("</ShippingAmount>" & vbNewLine)

        strBatch.Append("<GrossAmount>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & totalnett + totalvat & "</Money>" & vbNewLine)
        strBatch.Append("</GrossAmount>" & vbNewLine)

        strBatch.Append("<NetAmount>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & totalnett & "</Money>" & vbNewLine)
        strBatch.Append("</NetAmount>" & vbNewLine)

        strBatch.Append("<DueAmount>" & vbNewLine)
        strBatch.Append("<Money currency=""GBP"">" & totalnett + totalvat & "</Money>" & vbNewLine)
        strBatch.Append("</DueAmount>" & vbNewLine)

        strBatch.Append("</InvoiceDetailSummary>" & vbNewLine)
        strBatch.Append("</InvoiceDetailRequest>" & vbNewLine)
        strBatch.Append("</Request>" & vbNewLine)
        strBatch.Append("</cXML>" & vbNewLine)

        makeFolderExist(getConfig("XMLFilePath") & "\CIMA")
        makeFolderExist(getConfig("XMLFilePath") & "\CIMA\CIMA\" & Format(Now, "dd-MM-yyyy"))
        makeFolderExist(getConfig("XMLFilePath") & "\CIMA\CIMA\" & Format(Now, "dd-MM-yyyy") & "\SENT\")
        makeFolderExist(getConfig("XMLFilePath") & "\CIMA\CEL\" & Format(Now, "dd-MM-yyyy"))
        makeFolderExist(getConfig("XMLFilePath") & "\CIMA\CEL\" & Format(Now, "dd-MM-yyyy") & "\SENT\")

        Dim ofiler As System.IO.StreamWriter

        If blnCEL Then
            ofiler = New System.IO.StreamWriter(getConfig("XMLFilePath") & "\CIMA\CEL\" & Format(Now, "dd-MM-yyyy") & "\NYS" & pInvno & ".xml", False)
        Else
            ofiler = New System.IO.StreamWriter(getConfig("XMLFilePath") & "\CIMA\CIMA\" & Format(Now, "dd-MM-yyyy") & "\NYS" & pInvno & ".xml", False)
        End If

        ofiler.Write(strBatch.ToString)
        ofiler.Flush()
        ofiler.Close()

    End Sub

    'R2.26 CR - changed structure to be public
    Public Structure o2details
        Public strDetails As String
        Public intlinecount As Integer
        Public vatfailure, totalsfailure As Boolean
        Public invoice As String
    End Structure

    'R2.26 CR
    'O2's new system - create a physical file for sending to Adquira Marketplace
    Public Shared Function createPhysicalFileO2_Adquira(ByVal pInvno As String) As o2details
        Dim retStruct As New o2details
        Dim strBatch As New StringBuilder

        Dim nextInvoiceNumberX As String = ""
        Dim intLocalLineNumber As Integer = 0
        Dim totalVatable As Decimal = 0
        Dim totalNonVatable As Decimal = 0
        Dim dblVAT As Double = 0
        Dim strInvoicedate As String = ""
        Dim totalnett As Double = 0
        Dim totalvat As Double = 0

        Dim InvoiceTotal As Decimal = 0

        Dim rs As List(Of O2)
        rs = O2.getDetails(pInvno)

        For Each invoice As O2 In rs
            'hold the values
            Dim localExpense As Double = CDbl(invoice.Expense)
            Dim localExpenseVat As Double = CDbl(invoice.Expensevat)
            Dim localService As Double = CDbl(invoice.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            Dim localDiscount As Double = CDbl(invoice.LineDiscount)

            strInvoicedate = invoice.Invoicedate
            totalnett = CDbl((totalnett + localExpense + localService + invoice.Othercharge + invoice.Airlinecharge) - localDiscount)
            totalvat = CDbl(totalvat + localExpenseVat + localServiceVat + invoice.Othervat)

            'work out the VAT rate for this invoice
            If CDate(invoice.Invoicedate) >= CDate("04/01/2011") Then
                dblVAT = 0.2
            Else
                dblVAT = 0.175
            End If

            'tidy up the line description
            invoice.Linedescription = Pack(invoice.Linedescription.Trim.Replace(vbCrLf, " ")).Trim
            invoice.Linedescription = invoice.Linedescription.Replace("PLUS VAT = ", "=")
            invoice.Linedescription = invoice.Linedescription.Replace("PLUS VAT ", " ")
            invoice.Linedescription = invoice.Linedescription.Replace(" PLUS VAT . ", " PLUS VAT.")
            invoice.Linedescription = invoice.Linedescription.Replace(" = ", "=")
            invoice.Linedescription = invoice.Linedescription.Replace(" . ", ".")
            invoice.Linedescription = invoice.Linedescription.Replace(" X ", "x")
            invoice.Linedescription = invoice.Linedescription.Replace(" x ", "x")
            invoice.Linedescription = invoice.Linedescription.Replace(". ", ".")
            invoice.Linedescription = invoice.Linedescription.Replace(" .", ".")
            invoice.Linedescription = invoice.Linedescription.Replace(";", ",")
            invoice.Linedescription = Pack(invoice.Linedescription.Trim.Replace(vbCrLf, " ")).Trim

            'limit the length of the line desc (4000 chars max)
            If invoice.Linedescription.Length > 4000 Then
                invoice.Linedescription = invoice.Linedescription.Substring(0, 4000)
            End If

            'if we are on a fee line then there might not be a description, so add one if there isn't one
            If invoice.Linedescription.Length = 0 AndAlso invoice.Product.ToLower = "fees" Then
                invoice.Linedescription = "Transaction Fee"
            End If

            'sort out the line numbers
            If nextInvoiceNumberX <> invoice.Invoicenumber Then
                intLocalLineNumber = 0
                nextInvoiceNumberX = invoice.Invoicenumber
            End If

            'lets check the vat !!!!
            If invoice.Expense > 0 And invoice.Expensevat > 0 And invoice.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(invoice.Expense), CDbl(invoice.Expensevat), CDbl(invoice.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localExpense = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat > 0 Then
                    If localExpense = 0 Then
                        If localService = 0 Then
                            localOk = False
                        Else
                            localServiceVat = localExpenseVat
                            localExpenseVat = 0
                        End If
                    End If
                End If
            End If

            'check PO against O2Po table
            'if PO not found code will assume quantity = line price (or unit price = 1.00)
            Dim oO2Pos As New NysDat.clsO2Po
            Dim oO2PoRet As New NysDat.clsO2Po
            oO2Pos.mstrOrderNumber = invoice.Po
            oO2PoRet = oO2Pos.GetByOrderNumber()

            'for forcing steria invoices through Adquira, default the company and tax code
            If oO2PoRet.mstrCompanyName = "" Then
                oO2PoRet.mstrCompanyName = "Telefonica UK Limited"
            End If
            If oO2PoRet.mstrCompanyTaxId = "" Then
                oO2PoRet.mstrCompanyTaxId = "01743099"
            End If

            'sort out the header line if we are on the first invoice line
            If intLocalLineNumber = 0 Then
                strBatch.Append("CF;") 'CF = Invoice, CFR = Invoice Amendment, CFRA = Invoice Annulment
                strBatch.Append(invoice.Invoicenumber & ";") 'Invoice no
                strBatch.Append(Format(invoice.Invoicedate, "dd/MM/yyyy") & ";") 'invoice date
                strBatch.Append("GBP;") 'curency code
                strBatch.Append(invoice.Po & ";") 'purchase order number
                strBatch.Append("090D;") 'payment terms
                strBatch.Append(Replace(invoice.Costcentre, ".", "") & ";") 'Project code
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(vbCrLf)

                strBatch.Append("DC;")
                strBatch.Append(oO2PoRet.mstrCompanyName & ";") 'corporate buyer name
                strBatch.Append(oO2PoRet.mstrCompanyTaxId & ";") 'tax id
                strBatch.Append(";") 'cost centre
                strBatch.Append(";") 'email (optional)
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(vbCrLf)

                strBatch.Append("DP;")
                strBatch.Append("NYS Corporate Ltd;") 'corporate seller name
                strBatch.Append("371439840;") 'tax id
                strBatch.Append(";") 'supplier comments on invoice
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(";")
                strBatch.Append(vbCrLf)

                intLocalLineNumber += 1

                InvoiceTotal = invoice.Invoiceamount
            End If

            Dim dblQuantity As Double = 0
            Dim dblPrice As Double = 0
            If oO2PoRet.mdblQuantity = 1 Then
                'price = invoice line price
                'quantity = 1
                dblPrice = localExpense
                dblQuantity = 1
            Else
                'price = 1
                'quantity = invoice line price
                dblPrice = 1
                dblQuantity = localExpense
            End If

            'now line information
            strBatch.Append("LF;")
            strBatch.Append(intLocalLineNumber & ";") 'line number
            strBatch.Append(invoice.Linedescription & ";") 'line description
            strBatch.Append(";") 'supplier ref
            strBatch.Append(";") 'buyer ref
            strBatch.Append(";") 'out of taxable total options(SI = out of total, NO [or blank] = line in total)
            strBatch.Append(";")
            strBatch.Append(";")
            strBatch.Append(";")
            strBatch.Append(";")
            strBatch.Append(vbCrLf)

            strBatch.Append(";") 'blank field
            strBatch.Append(";") 'receipt date
            strBatch.Append(dblQuantity & ";") 'Quantity
            strBatch.Append(dblPrice & ";") 'price
            strBatch.Append("EA;") 'unit of measure (ISO code)
            strBatch.Append(";") 'delivery note number
            'SA - added 1 to tracking number to save us going into each invoice
            'strBatch.Append(";") 'tracking number
            strBatch.Append("1;") 'tracking number
            strBatch.Append(";") 'receipt number
            strBatch.Append(";") 'receipt place
            If localExpenseVat = 0 Then
                strBatch.Append("Zero Rate;") 'tax name ("Standard Rate", or "Zero Rate")
                strBatch.Append("0") 'tax percentage
                totalNonVatable = totalNonVatable + localExpense
            Else
                strBatch.Append("Standard Rate;") 'tax name ("Standard Rate", or "Zero Rate")
                strBatch.Append((dblVAT * 100) & "") 'tax percentage
                totalVatable = totalVatable + localExpense
            End If
            strBatch.Append(vbCrLf)

            'strBatch.Append(";") 'blank field
            'strBatch.Append(";") 'delivery date
            'strBatch.Append(";") 'delivery place
            'strBatch.Append(vbCrLf)

            'Line Tax info (optional)
            'Line number
            'Tax Type (I or R)
            'Country of tax (ISO code)
            'Tax Name
            'Base Total
            'Percentage
            'Tax Total
            'included in invoice total (yes, no)
            intLocalLineNumber += 1
        Next


        'first check VAT is OK
        If Math.Round(totalVatable * dblVAT, 2) <> Math.Round(totalvat, 2) Then
            Dim dbldiff As Double = Math.Abs(Math.Round(Math.Round(totalVatable * dblVAT, 2) - Math.Round(totalvat, 2), 2))
            If dbldiff > 0.04 Then
                retStruct.vatfailure = True
                retStruct.invoice = pInvno
            Else
                retStruct.vatfailure = False
            End If
        Else
            retStruct.vatfailure = False
        End If

        'now check all values add up correctly
        If Math.Round((totalVatable * dblVAT) + totalNonVatable + totalVatable, 2) <> Math.Round(InvoiceTotal, 2) Then
            Dim dbldiff As Double = Math.Abs(Math.Round(Math.Round((totalVatable * dblVAT) + totalNonVatable + totalVatable, 2) - Math.Round(InvoiceTotal, 2), 2))
            If dbldiff > 0.04 Then
                retStruct.totalsfailure = True
                retStruct.invoice = pInvno
            Else
                retStruct.totalsfailure = False
            End If
        Else
            retStruct.totalsfailure = False
        End If

        retStruct.strDetails = strBatch.ToString

        Return retStruct
    End Function

    'R2.26 CR - changed function to be public shared
    Public Shared Function createPhysicalFileO2_Steria(ByVal pInvno As String, ByRef pintLineCount As Integer) As o2details
        Dim retStruct As New o2details

        Dim strCSV As New StringBuilder
        Dim strBatchHeader As New StringBuilder
        Dim strBatchFooter As New StringBuilder
        Dim intLineNo As Integer = 1
        Dim intInnerLineNo As Integer = 1

        'get the invoice details
        Dim rs As List(Of O2)
        rs = O2.getDetails(pInvno)

        Dim strLineDescription As String = ""

        Dim totalnett As Decimal = 0
        Dim totalvat As Decimal = 0

        'CR
        Dim strPoNumberx As String = ""
        Dim intPoLineNumberx As Integer = 0
        Dim strLastPoNumberx As String = ""
        Dim strInvoicedate As String = ""

        Dim totalVatable As Decimal = 0
        Dim totalNonVatable As Decimal = 0
        Dim InvoiceTotal As Decimal = 0

        For Each r As O2 In rs

            strInvoicedate = r.Invoicedate

            r.Linedescription = Pack(r.Linedescription.Trim.Replace(vbCrLf, " ")).Trim

            r.Linedescription = r.Linedescription.Replace("PLUS VAT = ", "=")
            r.Linedescription = r.Linedescription.Replace("PLUS VAT ", " ")
            r.Linedescription = r.Linedescription.Replace(" PLUS VAT . ", " PLUS VAT.")
            r.Linedescription = r.Linedescription.Replace(" = ", "=")
            r.Linedescription = r.Linedescription.Replace(" . ", ".")
            r.Linedescription = r.Linedescription.Replace(" X ", "x")
            r.Linedescription = r.Linedescription.Replace(" x ", "x")
            r.Linedescription = r.Linedescription.Replace(". ", ".")
            r.Linedescription = r.Linedescription.Replace(" .", ".")

            'limit the description (225 chars max)
            If r.Linedescription.Length > 225 Then
                r.Linedescription = r.Linedescription.Substring(0, 225)
            End If

            Dim localExpense As Double = CDbl(r.Expense)
            Dim localExpenseVat As Double = CDbl(r.Expensevat)
            Dim localService As Double = CDbl(r.Servicecharge)
            Dim localServiceVat As Double = 0
            Dim localOk As Boolean = True
            Dim localDiscount As Double = CDbl(r.LineDiscount)

            'lets check the vat !!!!
            If r.Expense > 0 And r.Expensevat > 0 And r.Servicecharge > 0 Then
                Dim Ret As New getCorrectVat
                Ret = checkVAT(CDbl(r.Expense), CDbl(r.Expensevat), CDbl(r.Servicecharge))
                localExpense = Ret.pexpense
                localExpenseVat = Ret.pexpensevat
                localService = Ret.pservice
                localServiceVat = Ret.pservicevat
                localOk = Ret.ok
            Else
                'need to swap VAT from expense to service if ex
                If localExpenseVat > 0 Then
                    If localExpense = 0 Then
                        If localService = 0 Then
                            localOk = False
                        Else
                            localServiceVat = localExpenseVat
                            localExpenseVat = 0
                        End If
                    End If
                End If
            End If

            If r.Airlinecharge < 0 Then
                localExpense = CDbl(localExpense + r.Airlinecharge)
            End If

            totalnett = CDbl((totalnett + localExpense + localService + r.Othercharge + r.Airlinecharge) - localDiscount)
            totalvat = CDbl(totalvat + localExpenseVat + localServiceVat + r.Othervat)

            If intLineNo = 1 Then
                pintLineCount += 1

                'create PO header line
                strCSV.Append("D1~")
                strCSV.Append(r.Invoicenumber & "~")
                strCSV.Append("T83270~") 'VENDOR NO - provided by O2
                strCSV.Append("YO105BR~") 'VENDOR SITE - provided by O2
                InvoiceTotal = r.Invoiceamount
                strCSV.Append(r.Invoiceamount & "~")
                strCSV.Append(Format(r.Invoicedate, "dd-MMM-yyyy") & "~")
                strCSV.Append("### PO Line(s) Invoice~") 'DESCRIPTION
                strCSV.Append("~") 'Blank field - used to be for PO but not used
                strCSV.Append("GBP~") 'Currency Code 
                strCSV.Append(r.TotalDiscount & "~")
                strCSV.Append("GBP~") 'PAYMENT CURRENCY CODE
                strCSV.Append("EFT~") 'PAYMENT METHOD - Optional, EFT or CHECK
                strCSV.Append("~") 'EMPLOYEE ID - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'ARIBA NUMBER
                strCSV.Append("^" & vbNewLine)
            End If

            'CR
            'check PO against O2Po table
            'if PO not found code will assume quantity = line price (or unit price = 1.00)
            Dim oO2Pos As New NysDat.clsO2Po
            Dim oO2PoRet As New NysDat.clsO2Po
            oO2Pos.mstrOrderNumber = r.Po
            oO2PoRet = oO2Pos.GetByOrderNumber()

            If localExpense <> 0 Then
                'CR
                Dim dblUnitPrice As Double = 0
                Dim dblQuantity As Double = 0
                If oO2PoRet.mintPoID > 0 Then
                    If oO2PoRet.mdblQuantity = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = oO2PoRet.mdblUnitPrice
                        dblQuantity = localExpense
                    ElseIf oO2PoRet.mdblUnitPrice = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = localExpense
                        dblQuantity = 1
                    End If
                End If

                pintLineCount += 1

                strCSV.Append("D2~")
                strCSV.Append(intInnerLineNo & "~")
                strCSV.Append("ITEM~")
                strCSV.Append(localExpense & "~")
                strCSV.Append(Math.Round(dblUnitPrice, 2) & "~") 'unit price
                If localExpenseVat = 0 Then
                    strCSV.Append("ZR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
                    totalNonVatable = totalNonVatable + localExpense
                Else
                    strCSV.Append("SR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
                    totalVatable = totalVatable + localExpense
                End If
                strCSV.Append(dblQuantity & "~") 'QUANTITY
                strCSV.Append(r.Po & "~") 'PURCHASE ORDER NUMBER
                strCSV.Append("1~") 'PURCHASE ORDER LINE NO always 1 apparently!
                strCSV.Append(r.Linedescription & "~")
                strCSV.Append("~") 'Project No - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'TASK CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'EXP CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("^" & vbNewLine)
                intInnerLineNo += 1
            End If

            If r.Othercharge <> 0 Then
                'CR
                Dim dblUnitPrice As Double = 0
                Dim dblQuantity As Double = 0
                If oO2PoRet.mintPoID > 0 Then
                    If oO2PoRet.mdblQuantity = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = oO2PoRet.mdblUnitPrice
                        dblQuantity = r.Othercharge
                    ElseIf oO2PoRet.mdblUnitPrice = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = r.Othercharge
                        dblQuantity = 1
                    End If
                End If

                pintLineCount += 1

                strCSV.Append("D2~")
                strCSV.Append(intInnerLineNo & "~")
                strCSV.Append("ITEM~")
                strCSV.Append(r.Othercharge & "~")
                strCSV.Append(Math.Round(dblUnitPrice, 2) & "~") 'unit price
                If r.Othervat = 0 Then
                    strCSV.Append("ZR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
                    totalNonVatable = totalNonVatable + localExpense
                Else
                    strCSV.Append("SR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
                    totalVatable = totalVatable + localExpense
                End If
                strCSV.Append(dblQuantity & "~") 'QUANTITY
                strCSV.Append(r.Po & "~") 'PURCHASE ORDER NUMBER
                strCSV.Append("1~") 'PURCHASE ORDER LINE NO
                strCSV.Append(r.Linedescription & "~")
                strCSV.Append("~") 'Project No - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'TASK CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'EXP CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("^" & vbNewLine)
                intInnerLineNo += 1
            End If

            If localService <> 0 Then
                'CR
                Dim dblUnitPrice As Double = 0
                Dim dblQuantity As Double = 0
                If oO2PoRet.mintPoID > 0 Then
                    If oO2PoRet.mdblQuantity = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = oO2PoRet.mdblUnitPrice
                        dblQuantity = localService
                    ElseIf oO2PoRet.mdblUnitPrice = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = localService
                        dblQuantity = 1
                    End If
                End If

                pintLineCount += 1

                strCSV.Append("D2~")
                strCSV.Append(intInnerLineNo & "~")
                strCSV.Append("ITEM~")
                strCSV.Append(localService & "~")
                strCSV.Append(Math.Round(dblUnitPrice, 2) & "~") 'unit price
                If localServiceVat = 0 Then
                    strCSV.Append("ZR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
                    totalNonVatable = localExpense
                    totalNonVatable = totalNonVatable + localExpense
                Else
                    strCSV.Append("SR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
                    totalVatable = totalVatable + localExpense
                End If
                strCSV.Append(dblQuantity & "~") 'QUANTITY
                strCSV.Append(r.Po & "~") 'PURCHASE ORDER NUMBER
                strCSV.Append("1~") 'PURCHASE ORDER LINE NO
                strCSV.Append(r.Linedescription & "~")
                strCSV.Append("~") 'Project No - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'TASK CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'EXP CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("^" & vbNewLine)
                intInnerLineNo += 1
            End If

            If r.Airlinecharge > 0 Then
                'CR
                Dim dblUnitPrice As Double = 0
                Dim dblQuantity As Double = 0
                If oO2PoRet.mintPoID > 0 Then
                    If oO2PoRet.mdblQuantity = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = oO2PoRet.mdblUnitPrice
                        dblQuantity = r.Airlinecharge
                    ElseIf oO2PoRet.mdblUnitPrice = oO2PoRet.mdblTotalPrice Then
                        dblUnitPrice = r.Airlinecharge
                        dblQuantity = 1
                    End If
                End If

                pintLineCount += 1

                strCSV.Append("D2~")
                strCSV.Append(intInnerLineNo & "~")
                strCSV.Append("ITEM~")
                strCSV.Append(r.Airlinecharge & "~")
                strCSV.Append(Math.Round(dblUnitPrice, 2) & "~") 'unit price
                strCSV.Append("ZR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
                totalNonVatable = totalNonVatable + localExpense
                strCSV.Append(dblQuantity & "~") 'QUANTITY
                strCSV.Append(r.Po & "~") 'PURCHASE ORDER NUMBER
                strCSV.Append("1~") 'PURCHASE ORDER LINE NO
                strCSV.Append(r.Linedescription & "~")
                strCSV.Append("~") 'Project No - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'TASK CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("~") 'EXP CODE - not needed if PO Number exists (keep blank field though)
                strCSV.Append("^" & vbNewLine)
                intInnerLineNo += 1
            End If

            intLineNo += 1
        Next

        Dim dblVAT As Double = 0

        If CDate(strInvoicedate) >= CDate("04/01/2011") Then
            dblVAT = 0.2
        Else
            dblVAT = 0.175
        End If

        'first check VAT is OK
        If Math.Round(totalVatable * dblVAT, 2) <> Math.Round(totalvat, 2) Then
            Dim dbldiff As Double = Math.Abs(Math.Round(Math.Round(totalVatable * dblVAT, 2) - Math.Round(totalvat, 2), 2))
            If dbldiff > 0.04 Then
                retStruct.vatfailure = True
                retStruct.invoice = pInvno
            Else
                retStruct.vatfailure = False
            End If
        Else
            retStruct.vatfailure = False
        End If

        'now check all values add up correctly
        If Math.Round((totalVatable * dblVAT) + totalNonVatable + totalVatable, 2) <> Math.Round(InvoiceTotal, 2) Then
            Dim dbldiff As Double = Math.Abs(Math.Round(Math.Round((totalVatable * dblVAT) + totalNonVatable + totalVatable, 2) - Math.Round(InvoiceTotal, 2), 2))
            If dbldiff > 0.04 Then
                retStruct.totalsfailure = True
                retStruct.invoice = pInvno
            Else
                retStruct.totalsfailure = False
            End If
        Else
            retStruct.totalsfailure = False
        End If

        If strCSV.Length > 0 Then
            pintLineCount += 1

            strCSV.Append("D2~")
            strCSV.Append(intInnerLineNo & "~")
            strCSV.Append("TAX~")
            strCSV.Append(totalvat & "~")
            strCSV.Append("~") 'leave blank
            If totalvat = 0 Then
                strCSV.Append("ZR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
            Else
                strCSV.Append("SR~") 'SR – standard rate i.e. 17.5%, ZR – Zero Rated, NR – Non-reclaimable, OS – Out of Scope, EX - Exempt
            End If

            strCSV.Append("~") 'leave blank
            strCSV.Append("~") 'leave blank
            strCSV.Append("~") 'leave blank
            strCSV.Append("~") 'leave blank
            strCSV.Append("~") 'leave blank
            strCSV.Append("~") 'leave blank
            strCSV.Append("~") 'leave blank
            strCSV.Append("^" & vbNewLine)
        End If
        retStruct.strDetails = strCSV.ToString.Replace("###", intInnerLineNo - 1)
        retStruct.intlinecount = pintLineCount
        Return retStruct
    End Function

    'R2.11 CR
    Public Shared Function createFileRCN(ByVal pstartdate As String, ByVal penddate As String, ByVal pstrDebitDate As String) As String

        Dim strBookerErrorLines As New StringBuilder
        Dim strTravellerErrorLines As New StringBuilder
        Dim strInvoiceDateErrorLines As New StringBuilder
        Dim strTravelDateErrorLines As New StringBuilder
        Dim strActivityCodeErrorLines As New StringBuilder
        Dim strProjectErrorLines As New StringBuilder
        Dim strResourceErrorLines As New StringBuilder
        Dim strReasonErrorLines As New StringBuilder
        Dim strTaxCodeErrorLines As New StringBuilder
        Dim strAllErrorLines As New StringBuilder

        Dim strLines As New StringBuilder
        'R2.21 SA
        Dim strLinesCredit As New StringBuilder

        Dim rs As List(Of RCN)
        rs = RCN.list(pstartdate, penddate)

        'R2.23.1 AI
        Dim strShortName As String = ""

        'run through and check all fields are ok
        For Each r As RCN In rs

            'check the booker name is valid
            If r.BookerEmail = "" AndAlso (r.BookerName = "" OrElse r.BookerName.ToLower = "bookers name") Then
                strBookerErrorLines.Append(r.InvoiceNo & " - " & r.BookerName & vbCrLf)
            End If

            'check the travller name is valid
            If r.TravellerFirstname = "" OrElse r.TravellerLastname = "" Then
                strTravellerErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check there is an invoice date
            If r.DirectDebitDate = "" Then
                strInvoiceDateErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check there is a travel date
            If r.TravelDate = "" Then
                strTravelDateErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we can get an activity code
            Dim oRCNActivity As New NysDat.clsRCNActivityCode
            oRCNActivity = NysDat.clsRCNActivityCode.getByBossProduct(r.Product)
            If oRCNActivity.ActivityCode = "" Then
                strActivityCodeErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we have a project code
            If r.ProjectCode = "" Then
                strProjectErrorLines.Append(r.InvoiceNo & vbCrLf)
            Else
                If Not r.ProjectCode.Contains("-") Then
                    If r.ProjectCode.Length = 7 And IsNumeric(r.ProjectCode) Then
                        'do the formatting automatically - someone has forgotten to put the dash in
                        r.ProjectCode = r.ProjectCode.Substring(0, 2) & "-" & r.ProjectCode.Substring(2)
                    End If
                End If

                'R2.12a CR - fix RCN project code, set to get from config file
                If Not Regex.IsMatch(r.ProjectCode, getConfig("RCNProjectCodeFormat")) Then
                    strProjectErrorLines.Append(r.InvoiceNo & " - " & r.ProjectCode & vbCrLf)
                End If
            End If

            'check we have a resource code
            If r.ResourceCode = "" Then
                strResourceErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we have a reason for travel
            If r.Reason = "" Then
                strReasonErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we have a tax code
            If oRCNActivity.TaxCode = "" Then
                strTaxCodeErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If
        Next

        If strBookerErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT BOOKER NAME:" & vbCrLf & strBookerErrorLines.ToString & vbCrLf)
        End If

        If strTravellerErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT TRAVELLER NAME:" & vbCrLf & strTravellerErrorLines.ToString & vbCrLf)
        End If

        If strInvoiceDateErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE A BLANK INVOICE/DIRECT DEBIT DATE:" & vbCrLf & strInvoiceDateErrorLines.ToString & vbCrLf)
        End If

        If strTravelDateErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE A BLANK TRAVEL DATE:" & vbCrLf & strTravelDateErrorLines.ToString & vbCrLf)
        End If

        If strActivityCodeErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: UNABLE TO FIND RCN ACTIVITY CODES FOR THE FOLLOWING INVOICE/S:" & vbCrLf & strActivityCodeErrorLines.ToString & vbCrLf)
        End If

        If strProjectErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT PROJECT CODE (tot_pono):" & vbCrLf & strProjectErrorLines.ToString & vbCrLf)
        End If

        If strResourceErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT RESOURCE CODE (tot_costc):" & vbCrLf & strResourceErrorLines.ToString & vbCrLf)
        End If

        If strReasonErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN EMPTY REASON FOR TRAVEL:" & vbCrLf & strReasonErrorLines.ToString & vbCrLf)
        End If

        If strTaxCodeErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: UNABLE TO FIND RCN TAX CODE FOR THE FOLLOWING INVOICE/S:" & vbCrLf & strTaxCodeErrorLines.ToString & vbCrLf)
        End If

        'if there are any errors, return them all
        If strAllErrorLines.ToString <> "" Then
            Return strAllErrorLines.ToString
        End If

        'add the header line
        strLines.Append("Booker ID|Claim Reference|Direct Debit Date|Travel Date|Activity Code|Project Code|Resource|Location or Journey|Line Description|Unit Value|Tax Code|Tax Value|Invoice Number|Notes" & vbCrLf)
        'R2.21 SA - header for credit file
        strLinesCredit.Append("Booker ID|Claim Reference|Direct Debit Date|Travel Date|Activity Code|Project Code|Resource|Location or Journey|Line Description|Unit Value|Tax Code|Tax Value|Invoice Number|Notes" & vbCrLf)

        'R2.13 CR
        Dim dblRailFees As Double = 0
        Dim dblRailFeesVat As Double = 0
        Dim dblAirFees As Double = 0
        Dim dblAirFeesVat As Double = 0
        Dim dblHotelFees As Double = 0
        Dim dblHotelFeesVat As Double = 0
        Dim dblCarFees As Double = 0
        Dim dblCarFeesVat As Double = 0
        Dim dblNonTravelFees As Double = 0
        Dim dblNonTravelFeesVat As Double = 0
        Dim dblPassportFees As Double = 0
        Dim dblPassportFeesVat As Double = 0
        Dim dblTourFees As Double = 0
        Dim dblTourFeesVat As Double = 0
        Dim dblOtherFees As Double = 0
        Dim dblOtherFeesVat As Double = 0

        'R2.21 SA
        Dim dblRailFeesCredit As Double = 0
        Dim dblRailFeesVatCredit As Double = 0
        Dim dblAirFeesCredit As Double = 0
        Dim dblAirFeesVatCredit As Double = 0
        Dim dblHotelFeesCredit As Double = 0
        Dim dblHotelFeesVatCredit As Double = 0
        Dim dblCarFeesCredit As Double = 0
        Dim dblCarFeesVatCredit As Double = 0
        Dim dblNonTravelFeesCredit As Double = 0
        Dim dblNonTravelFeesVatCredit As Double = 0
        Dim dblPassportFeesCredit As Double = 0
        Dim dblPassportFeesVatCredit As Double = 0
        Dim dblTourFeesCredit As Double = 0
        Dim dblTourFeesVatCredit As Double = 0
        Dim dblOtherFeesCredit As Double = 0
        Dim dblOtherFeesVatCredit As Double = 0
        'dont do Conference - RCN100 dont do conf bookings.... yet
        'Dim dblConferenceFees As Double = 0
        'Dim dblConferenceFeesVat As Double = 0

        Dim strLastInvoiceProductType As String = ""
        Dim strLastInvoiceNo As String = ""
        Dim strHotelFeeErrors As New StringBuilder
        Dim strEbisIDErrors As New StringBuilder

        'R2.21 SA declare the total variables
        Dim dblGross As Double = 0
        Dim dblNett As Double = 0
        Dim dblVat As Double = 0
        Dim dblDispVat As Double = 0
        Dim dblTax As Double = 0
        Dim dblDiscount As Double = 0
        Dim dblCreditGross As Double = 0
        Dim dblCreditNett As Double = 0
        Dim dblCreditVat As Double = 0
        Dim dblCreditDispVat As Double = 0
        Dim dblCreditTax As Double = 0
        Dim dblCreditDiscount As Double = 0

        'R2.?? SA 
        'get rail charges from interceptor
        Dim dblRailChargesFromInterceptor As Double = 0
        dblRailChargesFromInterceptor = RCN.getRailCharges(pstartdate, penddate)

        For Each r As RCN In rs

            Dim strBookerFirstname As String = ""
            Dim strBookerLastname As String = ""
            'If r.InvoiceNo = "N592415" Then
            '    Dim iStop As Integer = 0
            'End If

            If r.BookerName.ToUpper.StartsWith("MR ") Or r.BookerName.ToUpper.StartsWith("MS ") Or r.BookerName.ToUpper.StartsWith("DR ") Then
                r.BookerName = Trim(r.BookerName.Substring(3))
            ElseIf r.BookerName.ToUpper.StartsWith("MRS ") Then
                r.BookerName = Trim(r.BookerName.Substring(4))
            ElseIf r.BookerName.ToUpper.StartsWith("MISS ") Then
                r.BookerName = Trim(r.BookerName.Substring(5))
            End If

            If r.BookerName.Contains(" ") Then
                strBookerFirstname = r.BookerName.Substring(0, r.BookerName.IndexOf(" ")).ToUpper
                strBookerLastname = r.BookerName.Substring(r.BookerName.IndexOf(" ") + 1).ToUpper
            Else
                strBookerFirstname = r.BookerName.ToUpper
                strBookerLastname = r.BookerName.ToUpper
            End If

            'R2.16 CR - dont create the ID anymore, try and select it from SSO!!
            'create the Ebis ID
            'Dim strEbisID As String = strBookerLastname.ToLower & strBookerFirstname.Substring(0, 1).ToLower
            Dim strEbisID As String = ""
            Dim strBookerEmail As String = r.BookerEmail

            Dim oSsoUser As New clsSSOUser
            oSsoUser = clsSSOUser.findEbisID(strBookerEmail, strBookerFirstname, strBookerLastname)

            'R2.23.1 AI
            Dim strShortFirstname As String = ""
            Dim strShortLastname As String = ""

            If oSsoUser.SsoUserEbisID = "" Then
                strShortName = ""
                strShortName = RCN.GetRCNShortenedName(strBookerFirstname & " " & strBookerLastname)
                If ((strShortName = "") AndAlso (Not strEbisIDErrors.ToString.Contains(r.InvoiceNo))) Then
                    strEbisIDErrors.Append(r.InvoiceNo & " - BookerName= " & r.BookerName & " BookerEmail= " & strBookerEmail & vbCrLf)
                Else
                    If strShortName.Contains(" ") Then
                        strShortFirstname = strShortName.Substring(0, strShortName.IndexOf(" ")).ToUpper
                        strShortLastname = strShortName.Substring(strShortName.IndexOf(" ") + 1).ToUpper
                    Else
                        strShortFirstname = strShortName.ToUpper
                        strShortLastname = strShortName.ToUpper
                    End If
                    strEbisID = clsSSOUser.GetRCNSsoUserEbisID(strShortFirstname, strShortLastname)
                End If
            Else
                strEbisID = oSsoUser.SsoUserEbisID.ToLower
            End If

            'SA - old code
            'If oSsoUser.SsoUserEbisID = "" Then
            '    If Not strEbisIDErrors.ToString.Contains(r.InvoiceNo) Then
            '        strEbisIDErrors.Append(r.InvoiceNo & " - BookerName= " & r.BookerName & " BookerEmail= " & strBookerEmail & vbCrLf)
            '    End If
            'Else
            '    strEbisID = oSsoUser.SsoUserEbisID.ToLower
            'End If


            'R2.17 CR
            'If name couldn't be split earlier but a booker has been found in sso, use sso booker names.
            ' in all other cases use the name from BOSS
            If strBookerFirstname = strBookerLastname AndAlso oSsoUser.SsoUserEbisID <> "" Then
                strBookerFirstname = oSsoUser.SsoUserFirstname.ToUpper
                strBookerLastname = oSsoUser.SsoUserLastName.ToUpper
            End If


            'create the Claim Ref - truncate if over 40 chars
            Dim strClaimRef As String = "NYS " & strBookerFirstname & " " & strBookerLastname
            If strClaimRef.Length > 40 Then
                'over size - truncate
                strClaimRef = strClaimRef.Substring(0, 40)
            End If


            'R2.17 CR
            'change the product code if hotel is inside london
            Dim strCurrentProduct As String = r.Product
            If strCurrentProduct = "H" Then
                If NysDat.clsRCNLondonPostcodes.containsCode(r.SupplierPostcode1) Then
                    strCurrentProduct = "HLon"
                End If
            End If


            'get the activity code & tax code
            Dim oRCNActivity As New NysDat.clsRCNActivityCode
            oRCNActivity = NysDat.clsRCNActivityCode.getByBossProduct(strCurrentProduct)


            'create the line desc
            Dim strLineDesc As New StringBuilder
            'Traveller last name no greater than 7 chars
            If r.TravellerLastname.Length > 7 Then
                strLineDesc.Append(r.TravellerLastname.Substring(0, 7).ToUpper)
            Else
                strLineDesc.Append(r.TravellerLastname)
            End If
            'add only the 1st letter of the firstname
            strLineDesc.Append(" " & r.TravellerFirstname.Substring(0, 1).ToUpper)
            'add the trip reason
            strLineDesc.Append(" " & r.Reason)
            'truncate if over 30 chars
            If strLineDesc.Length > 30 Then
                strLineDesc.Remove(31, strLineDesc.Length - 31)
            End If

            'FIX: TAX codes wrong for international hotels
            Dim strTaxCode As String = cleanRCNString(oRCNActivity.TaxCode)
            If r.Product = "H" And r.ExpenseVat = 0 Then
                'check to see if hotel is International
                'if international then tax code = "O"
                'otherwise it is tax code = "Z"
                If r.IsSupplierInternational Then
                    strTaxCode = "O"
                Else
                    strTaxCode = "Z"
                End If
            End If

            'R2.21 CR - removed RM product check - added to db table now
            'FIX: delivery tax code
            Dim strActivityCode As String = cleanRCNString(oRCNActivity.ActivityCode)
            If ((cleanRCNString(r.Location).ToLower.Contains("delivery") And cleanRCNString(r.Location).ToLower.Contains("special"))) And r.ExpenseVat = 0 Then
                'Exempt
                strTaxCode = "Z"
                strActivityCode = "5520-01"
            End If

            'add the row - transaction fees do NOT come in here
            If r.Expense <> 0 Or r.ExpenseVat <> 0 Then
                'R2.21 SA 
                If r.Expense >= 0 Then 'debit 
                    strLines.Append(cleanRCNString(strEbisID) & "|" & cleanRCNString(strClaimRef) & "|" & pstrDebitDate & "|" & cleanRCNString(r.TravelDate) & "|" & _
            strActivityCode & "|" & cleanRCNString(r.ProjectCode) & "|" & cleanRCNString(r.ResourceCode) & "|" & cleanRCNString(r.Location) & _
            "|" & cleanRCNString(strLineDesc.ToString) & "|" & r.Expense & "|" & strTaxCode & "|" & r.ExpenseVat & "|" & r.InvoiceNo & "|" & Trim(cleanRCNString(r.LineNotes)) & vbCrLf)
                    dblNett += r.Expense
                    dblVat += r.Vat
                    dblDispVat += r.DispVat
                    dblTax += r.Tax
                    dblDiscount += r.Discount
                Else 'credit
                    strLinesCredit.Append(cleanRCNString(strEbisID) & "|" & cleanRCNString(strClaimRef) & "|" & pstrDebitDate & "|" & cleanRCNString(r.TravelDate) & "|" & _
            strActivityCode & "|" & cleanRCNString(r.ProjectCode) & "|" & cleanRCNString(r.ResourceCode) & "|" & cleanRCNString(r.Location) & _
            "|" & cleanRCNString(strLineDesc.ToString) & "|" & r.Expense & "|" & strTaxCode & "|" & r.ExpenseVat & "|" & r.InvoiceNo & "|" & Trim(cleanRCNString(r.LineNotes)) & vbCrLf)
                    dblCreditNett += r.Expense
                    dblCreditVat += r.Vat
                    dblCreditDispVat += r.DispVat
                    dblCreditTax += r.Tax
                    dblCreditDiscount += r.Discount
                End If
            End If

            'ourcharge > 0 or ourvat > 0 = Transaction Fee line!!
            ' add the fees to the relevant total - RCN don't want single lines of fees (for some weird reason)
            If r.OurCharge <> 0 Or r.OurVat <> 0 Then
                Dim strFeeProdType As String = r.ProductType

                If r.ProductType = "TF" Then
                    ' need to work out the product on the invoice that the trans fee is against
                    If r.InvoiceNo = strLastInvoiceNo Then
                        'same invoice = fee for same product.... hopefully!
                        strFeeProdType = strLastInvoiceProductType
                    End If
                End If

                'R2.21 SA 
                If r.OurCharge > 0 Then 'debit

                    'determine which product it relates to!
                    If strFeeProdType = "RR" Then
                        dblRailFees += r.OurCharge
                        dblRailFeesVat += r.OurVat
                    ElseIf strFeeProdType = "A" Then
                        dblAirFees += r.OurCharge
                        dblAirFeesVat += r.OurVat
                    ElseIf strFeeProdType = "H" Then

                        If r.SupplierID = "AERO24OOH" Then
                            'it's a hotel fee, but it's for OOH service - so it needs billing back on the feeder
                            dblHotelFees += r.OurCharge
                            dblHotelFeesVat += r.OurVat
                        Else
                            'all other hotel fees are not to be billed back to RCN!
                            strHotelFeeErrors.Append(r.InvoiceNo & " Fee= £" & r.OurCharge & " FeeVAT= £" & r.OurVat & vbCrLf)
                        End If

                    ElseIf strFeeProdType = "CR" Then
                        dblCarFees += r.OurCharge
                        dblCarFeesVat += r.OurVat
                    ElseIf strFeeProdType = "NT" Then
                        dblNonTravelFees += r.OurCharge
                        dblNonTravelFeesVat += r.OurVat
                    ElseIf strFeeProdType = "PV" Then
                        dblPassportFees += r.OurCharge
                        dblPassportFeesVat += r.OurVat
                    ElseIf strFeeProdType = "TP" Then
                        dblTourFees += r.OurCharge
                        dblTourFeesVat += r.OurVat
                    Else
                        'Only things to come in here should be those defined as OTHER product types
                        ' OR any TF where the previous invoice line is not present
                        dblOtherFees += r.OurCharge
                        dblOtherFeesVat += r.OurVat
                    End If

                Else 'Credit

                    'determine which product it relates to!
                    If strFeeProdType = "RR" Then
                        dblRailFeesCredit += r.OurCharge
                        dblRailFeesVatCredit += r.OurVat
                    ElseIf strFeeProdType = "A" Then
                        dblAirFeesCredit += r.OurCharge
                        dblAirFeesVatCredit += r.OurVat
                    ElseIf strFeeProdType = "H" Then

                        If r.SupplierID = "AERO24OOH" Then
                            'it's a hotel fee, but it's for OOH service - so it needs billing back on the feeder
                            dblHotelFeesCredit += r.OurCharge
                            dblHotelFeesVatCredit += r.OurVat
                        Else
                            'all other hotel fees are not to be billed back to RCN!
                            strHotelFeeErrors.Append(r.InvoiceNo & " Fee= £" & r.OurCharge & " FeeVAT= £" & r.OurVat & vbCrLf)
                        End If

                    ElseIf strFeeProdType = "CR" Then
                        dblCarFeesCredit += r.OurCharge
                        dblCarFeesVatCredit += r.OurVat
                    ElseIf strFeeProdType = "NT" Then
                        dblNonTravelFeesCredit += r.OurCharge
                        dblNonTravelFeesVatCredit += r.OurVat
                    ElseIf strFeeProdType = "PV" Then
                        dblPassportFeesCredit += r.OurCharge
                        dblPassportFeesVatCredit += r.OurVat
                    ElseIf strFeeProdType = "TP" Then
                        dblTourFeesCredit += r.OurCharge
                        dblTourFeesVatCredit += r.OurVat
                    Else
                        'Only things to come in here should be those defined as OTHER product types
                        ' OR any TF where the previous invoice line is not present
                        dblOtherFeesCredit += r.OurCharge
                        dblOtherFeesVatCredit += r.OurVat
                    End If
                End If

            End If

            strLastInvoiceNo = r.InvoiceNo
            If r.ProductType <> "TF" Then
                strLastInvoiceProductType = r.ProductType
            End If
        Next

        If strHotelFeeErrors.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THERE APPEARS TO BE HOTEL FEES ON THE FOLLOWING INVOICE/S, THERE SHOULD NOT BE ANY FOR RCN:" & vbCrLf & strHotelFeeErrors.ToString & vbCrLf)
        End If

        If strEbisIDErrors.ToString <> "" Then
            strAllErrorLines.Append("ERROR: UNABLE TO MATCH BOOKER ON THE FOLLOWING INVOICE/S AGAINST AN EBIS ID:" & vbCrLf & strEbisIDErrors.ToString & vbCrLf)
        End If

        If strAllErrorLines.ToString <> "" Then
            Return strAllErrorLines.ToString
        End If

        Dim oRCNActivity2 As New NysDat.clsRCNActivityCode
        oRCNActivity2 = NysDat.clsRCNActivityCode.getByBossProduct("TF")

        'R2.?? SA 
        'add interceptor charges to the rail charges 
        dblRailFees += dblRailChargesFromInterceptor

        'R2.21 SA - change from <> 0 to > 0 for positive values only
        If dblRailFees > 0 Or dblRailFeesVat > 0 Then
            'Rail are booking fees - therefore non-vat able
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "BOOKING FEES" & _
                               "|" & "RAIL" & "|" & dblRailFees & "|" & "Z" & "|" & dblRailFeesVat & "|" & "0" & "|Sum of Booking Fees for Rail Travel" & vbCrLf)
            'R2.21 SA
            dblNett += dblRailFees
            dblVat += dblRailFeesVat
        End If

        If dblAirFees > 0 Or dblAirFeesVat > 0 Then
            'Air are booking fees - therefore non-vat able
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "BOOKING FEES" & _
                               "|" & "AIR" & "|" & dblAirFees & "|" & "Z" & "|" & dblAirFeesVat & "|" & "0" & "|Sum of Booking Fees for Air Travel" & vbCrLf)
            'R2.21 SA
            dblNett += dblAirFees
            dblVat += dblAirFeesVat
        End If

        'SHOULD NEVER BE HOTEL FEES FOR RCN!!!!
        'EXCEPT: when OOH charges are applied - the only amounts coming into here should be for OOH
        If dblHotelFees <> 0 Or dblHotelFeesVat <> 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "OUT OF HOURS BOOKING FEES" & _
                               "|" & "HOTEL" & "|" & dblHotelFees & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblHotelFeesVat & "|" & "0" & "|Sum of out of hours booking fees for Hotel stays" & vbCrLf)
            dblNett += dblHotelFees
            dblVat += dblHotelFeesVat
        End If

        'if hotel fees found then error to the user!!

        If dblCarFees > 0 Or dblCarFeesVat > 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
               "|" & "CAR RENTAL" & "|" & dblCarFees & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblCarFeesVat & "|" & "0" & "|Sum of transaction fees for Car Hire" & vbCrLf)
            'R2.21 SA
            dblNett += dblCarFees
            dblVat += dblCarFeesVat
        End If

        If dblNonTravelFees > 0 Or dblNonTravelFeesVat > 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
                               "|" & "NON TRAVEL EXPENSE" & "|" & dblNonTravelFees & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblNonTravelFeesVat & "|" & "0" & "|Sum of transaction fees for Non Travel products" & vbCrLf)
            'R2.21 SA
            dblNett += dblNonTravelFees
            dblVat += dblNonTravelFeesVat
        End If

        If dblPassportFees > 0 Or dblPassportFeesVat > 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
               "|" & "PASSPORT & VISA" & "|" & dblPassportFees & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblPassportFeesVat & "|" & "0" & "|Sum of transaction fees for Passports and Visas" & vbCrLf)
            'R2.21 SA
            dblNett += dblPassportFees
            dblVat += dblPassportFeesVat
        End If

        If dblTourFees > 0 Or dblTourFeesVat > 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
               "|" & "TOUR PACKAGE" & "|" & dblTourFees & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblTourFeesVat & "|" & "0" & "|Sum of transaction fees for Tour Packages" & vbCrLf)
            'R2.21 SA
            dblNett += dblTourFees
            dblVat += dblTourFeesVat
        End If

        If dblOtherFees > 0 Or dblOtherFeesVat > 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
               "|" & "OTHER" & "|" & dblOtherFees & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblOtherFeesVat & "|" & "0" & "|Sum of transaction fees for Miscellaneous Items" & vbCrLf)
            'R2.21 SA
            dblNett += dblOtherFees
            dblVat += dblOtherFeesVat
        End If


        'R2.21 SA
        If dblRailFeesCredit < 0 Or dblRailFeesVatCredit < 0 Then
            'Rail are booking fees - therefore non-vat able
            strLinesCredit.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "BOOKING FEES" & _
                               "|" & "RAIL" & "|" & dblRailFeesCredit & "|" & "Z" & "|" & dblRailFeesVatCredit & "|" & "0" & "|Sum of Booking Fees for Rail Travel" & vbCrLf)
            dblCreditNett += dblRailFeesCredit
            dblCreditVat += dblRailFeesVatCredit
        End If

        If dblAirFeesCredit < 0 Or dblAirFeesVatCredit < 0 Then
            'Air are booking fees - therefore non-vat able
            strLinesCredit.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "BOOKING FEES" & _
                               "|" & "AIR" & "|" & dblAirFeesCredit & "|" & "Z" & "|" & dblAirFeesVatCredit & "|" & "0" & "|Sum of Booking Fees for Air Travel" & vbCrLf)
            dblCreditNett += dblAirFeesCredit
            dblCreditVat += dblAirFeesVatCredit
        End If

        'add in the hotel fees - these should only be for OOH hotel fees
        If dblHotelFeesCredit <> 0 Or dblHotelFeesVatCredit <> 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "OUT OF HOURS BOOKING FEES" & _
                               "|" & "HOTEL" & "|" & dblHotelFeesCredit & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblHotelFeesVatCredit & "|" & "0" & "|Sum of out of hours booking fees for Hotel stays" & vbCrLf)
            dblNett += dblHotelFeesCredit
            dblVat += dblHotelFeesVatCredit
        End If

        If dblCarFeesCredit < 0 Or dblCarFeesVatCredit < 0 Then
            strLinesCredit.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
               cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
               "|" & "CAR RENTAL" & "|" & dblCarFeesCredit & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblCarFeesVatCredit & "|" & "0" & "|Sum of transaction fees for Car Hire" & vbCrLf)
            dblCreditNett += dblCarFeesCredit
            dblCreditVat += dblCarFeesVatCredit
        End If

        If dblNonTravelFeesCredit < 0 Or dblNonTravelFeesVatCredit < 0 Then
            strLines.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
                   cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
                   "|" & "NON TRAVEL EXPENSE" & "|" & dblNonTravelFeesCredit & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblNonTravelFeesVatCredit & "|" & "0" & "|Sum of transaction fees for Non Travel products" & vbCrLf)
            dblCreditNett += dblNonTravelFeesCredit
            dblCreditVat += dblNonTravelFeesVatCredit
        End If

        If dblPassportFeesCredit < 0 Or dblPassportFeesVatCredit < 0 Then
            strLinesCredit.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
   cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
   "|" & "PASSPORT & VISA" & "|" & dblPassportFeesCredit & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblPassportFeesVatCredit & "|" & "0" & "|Sum of transaction fees for Passports and Visas" & vbCrLf)
            dblCreditNett += dblPassportFeesCredit
            dblCreditVat += dblPassportFeesVatCredit
        End If

        If dblTourFeesCredit < 0 Or dblTourFeesVatCredit < 0 Then
            strLinesCredit.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
   cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
   "|" & "TOUR PACKAGE" & "|" & dblTourFeesCredit & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblTourFeesVatCredit & "|" & "0" & "|Sum of transaction fees for Tour Packages" & vbCrLf)
            dblCreditNett += dblTourFeesCredit
            dblCreditVat += dblTourFeesVatCredit
        End If

        If dblOtherFeesCredit < 0 Or dblOtherFeesVatCredit < 0 Then
            strLinesCredit.Append("sirrj" & "|" & "NYS Jude Sirr" & "|" & pstrDebitDate & "|" & pstrDebitDate & "|" & _
   cleanRCNString(oRCNActivity2.ActivityCode) & "|" & "10-10140" & "|" & "00R" & "|" & "TRANSACTION FEES" & _
   "|" & "OTHER" & "|" & dblOtherFeesCredit & "|" & cleanRCNString(oRCNActivity2.TaxCode) & "|" & dblOtherFeesVatCredit & "|" & "0" & "|Sum of transaction fees for Miscellaneous Items" & vbCrLf)
            dblCreditNett += dblOtherFeesCredit
            dblCreditVat += dblOtherFeesVatCredit
        End If

        'R2.21 SA
        'create file here
        'now create the files and show a link to view them
        'create the new folders for today
        makeFolderExist(getConfig("XMLFilePath") & "\RCN100")
        makeFolderExist(getConfig("XMLFilePath") & "\RCN100\" & Format(Now, "dd-MM-yyyy"))

        'Create debit file
        Try
            'create the feeder file
            Dim strFileName As String = "NYS100.csv"
            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\RCN100\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
            ofiler.Write(strLines.ToString.Replace(",", " ").Replace("|", ","))
            ofiler.Flush()
            ofiler.Close()
        Catch ex As Exception
            Return "Unable to create feeder CSV file, please speak to development team"
        End Try

        'Create credit file
        Try
            'create the feeder file
            Dim strFileName As String = "NYS100-Credits.csv"
            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\RCN100\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
            ofiler.Write(strLinesCredit.ToString.Replace(",", " ").Replace("|", ","))
            ofiler.Flush()
            ofiler.Close()
        Catch ex As Exception
            Return "Unable to create feeder CSV file, please speak to development team"
        End Try


        'create the cover sheet file
        Try
            Dim strCoverFileName As String = "NYS100-" & Format(Date.Now, "dd-MM-yyyy") & " CoverSheet.rtf"
            Dim strRtfFilePath As String = getConfig("XMLFilePath") & "\RCN100\" & Format(Now, "dd-MM-yyyy") & "\" & strCoverFileName

            Dim ofiler3 As New System.IO.StreamWriter(strRtfFilePath, False, Encoding.Default)
            Dim strCoverSheet As String = clsNYS.readText(getConfig("XMLFilePath") & "\RCN100\Template\CoverSheet.rtf")

            strCoverSheet = strCoverSheet.Replace("#dddate#", pstrDebitDate)
            strCoverSheet = strCoverSheet.Replace("#startdate#", pstartdate)
            strCoverSheet = strCoverSheet.Replace("#enddate#", penddate)

            dblNett = FormatNumber(dblNett + dblDiscount - dblTax, 2)
            dblCreditNett = FormatNumber(dblCreditNett + dblCreditDiscount - dblCreditTax, 2)

            dblGross = FormatNumber(dblNett + dblVat + dblDispVat + dblTax - dblDiscount, 2)
            dblCreditGross = FormatNumber(dblCreditNett + dblCreditVat + dblCreditDispVat + dblCreditTax - dblCreditDiscount, 2)

            strCoverSheet = strCoverSheet.Replace("#dnet#", "£" & FormatNumber(dblNett, 2))
            strCoverSheet = strCoverSheet.Replace("#dtax#", "£" & FormatNumber(dblTax, 2))
            strCoverSheet = strCoverSheet.Replace("#dvat#", "£" & FormatNumber(dblVat, 2))
            strCoverSheet = strCoverSheet.Replace("#ddvat#", "£" & FormatNumber(dblDispVat, 2))
            'strCoverSheet = strCoverSheet.Replace("#ddis#", "£" & FormatNumber(dblDiscount, 2))
            strCoverSheet = strCoverSheet.Replace("#dgross#", "£" & dblGross)

            strCoverSheet = strCoverSheet.Replace("#cnet#", "£" & FormatNumber(dblCreditNett, 2))
            strCoverSheet = strCoverSheet.Replace("#ctax#", "£" & FormatNumber(dblCreditTax, 2))
            strCoverSheet = strCoverSheet.Replace("#cvat#", "£" & FormatNumber(dblCreditVat, 2))
            strCoverSheet = strCoverSheet.Replace("#cdvat#", "£" & FormatNumber(dblCreditDispVat, 2))
            'strCoverSheet = strCoverSheet.Replace("#cdis#", "£" & FormatNumber(dblCreditDiscount, 2))
            strCoverSheet = strCoverSheet.Replace("#cgross#", "£" & dblCreditGross)

            strCoverSheet = strCoverSheet.Replace("#TOTALNET#", "£" & FormatNumber((dblNett + dblCreditNett), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALTAX#", "£" & FormatNumber((dblTax + dblCreditTax), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALVAT#", "£" & FormatNumber((dblVat + dblCreditVat), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALDVAT#", "£" & FormatNumber((dblDispVat + dblCreditDispVat), 2))
            'strCoverSheet = strCoverSheet.Replace("#TOTALDISCOUNT#", "£" & FormatNumber((dblDiscount + dblCreditDiscount), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALGROSS#", "£" & FormatNumber(dblGross, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALCREDIT#", "£" & FormatNumber(dblCreditGross, 2))

            strCoverSheet = strCoverSheet.Replace("#TOTALDUE#", "£" & FormatNumber((dblGross + dblCreditGross), 2))

            strCoverSheet = strCoverSheet.Replace("#TOTALDISVAT#", "£" & FormatNumber((dblDispVat + dblCreditDispVat), 2))

            ofiler3.Write(strCoverSheet)
            ofiler3.Flush()
            ofiler3.Close()

            If Not ConvertToPDF(strRtfFilePath, strRtfFilePath.Replace(".rtf", ".pdf"), False) Then
                Throw New Exception("Unable to convert file to PDF")
            End If

            'R2.? SA - send an email to Fiona    
            clsNYS.SendEmailMessage("accounts@nysgroup.com", getConfig("FeederRailFeesEmailTo"), _
                                    "RCN100 Manual invoice for the period between" & pstartdate & " and " & penddate, _
                                    "The value for RCN100 manual invoice should be £" & dblRailChargesFromInterceptor, _
                                    "", "", "", "", "", "", "")

        Catch ex As Exception
            Return "Unable to create cover sheet file, please speak to development team"
        End Try



        Return strLines.ToString() & vbCrLf & strLinesCredit.ToString()

    End Function

    Public Shared Function cleanRCNString(ByVal pValue As String) As String
        Dim strRet As String = pValue
        If strRet <> "" Then
            strRet = strRet.Replace("|", " ")
            strRet = strRet.Replace("?", " ")
            strRet = strRet.Replace("/", "-")
            strRet = strRet.Replace("@", " ")
            strRet = strRet.Replace("~", " ")
            strRet = strRet.Replace("#", " ")
            strRet = strRet.Replace("`", " ")
            strRet = strRet.Replace("!", " ")
            strRet = strRet.Replace("""", " ")
            strRet = strRet.Replace("^", " ")
            strRet = strRet.Replace("&", " ")
            strRet = strRet.Replace("*", " ")
            strRet = strRet.Replace("\", "-")
            strRet = strRet.Replace("amp;", " ")
            strRet = strRet.Replace(";", " ")
            strRet = strRet.Replace(".", " ")
            strRet = strRet.Replace(",", " ")
            strRet = strRet.Replace("'", " ")
            strRet = strRet.Replace("(", " ")
            strRet = strRet.Replace(")", " ")
            strRet = strRet.Replace(vbCrLf, " ")
        End If
        Return strRet
    End Function

    'R2.21 SA
    Public Shared Function createFileRCNG(ByVal pstartdate As String, ByVal penddate As String, ByVal pstrDebitDate As String) As String

        Dim strBookerErrorLines As New StringBuilder
        Dim strTravellerErrorLines As New StringBuilder
        Dim strInvoiceDateErrorLines As New StringBuilder
        Dim strTravelDateErrorLines As New StringBuilder
        Dim strActivityCodeErrorLines As New StringBuilder
        Dim strProjectErrorLines As New StringBuilder
        Dim strResourceErrorLines As New StringBuilder
        Dim strReasonErrorLines As New StringBuilder
        Dim strTaxCodeErrorLines As New StringBuilder
        Dim strAllErrorLines As New StringBuilder

        Dim strLines As New StringBuilder
        'R2.21 SA
        Dim strLinesCredit As New StringBuilder

        Dim rs As List(Of RCN)
        rs = RCN.listConf(pstartdate, penddate)

        'run through and check all fields are ok
        For Each r As RCN In rs

            'check the booker name is valid
            If r.BookerEmail = "" AndAlso (r.BookerName = "" OrElse r.BookerName.ToLower = "bookers name") Then
                strBookerErrorLines.Append(r.InvoiceNo & " - " & r.BookerName & vbCrLf)
            End If

            'check the travller name is valid
            If r.TravellerFirstname = "" OrElse r.TravellerLastname = "" Then
                strTravellerErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check there is an invoice date
            If r.DirectDebitDate = "" Then
                strInvoiceDateErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check there is a travel date
            If r.TravelDate = "" Then
                strTravelDateErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we can get an activity code
            Dim oRCNActivity As New NysDat.clsRCNActivityCode
            oRCNActivity = NysDat.clsRCNActivityCode.getByBossProduct(r.Product)
            If oRCNActivity.ActivityCode = "" Then
                strActivityCodeErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we have a project code
            If r.ProjectCode = "" Then
                strProjectErrorLines.Append(r.InvoiceNo & vbCrLf)
            Else
                'R2.12a CR - fix RCN project code, set to get from config file
                If Not Regex.IsMatch(r.ProjectCode, getConfig("RCNProjectCodeFormat")) Then
                    strProjectErrorLines.Append(r.InvoiceNo & " - " & r.ProjectCode & vbCrLf)
                End If
            End If

            'check we have a resource code
            If r.ResourceCode = "" Then
                strResourceErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we have a reason for travel
            If r.Reason = "" Then
                strReasonErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If

            'check we have a tax code
            If oRCNActivity.TaxCode = "" Then
                strTaxCodeErrorLines.Append(r.InvoiceNo & vbCrLf)
            End If
        Next

        If strBookerErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT BOOKER NAME:" & vbCrLf & strBookerErrorLines.ToString & vbCrLf)
        End If

        If strTravellerErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT TRAVELLER NAME:" & vbCrLf & strTravellerErrorLines.ToString & vbCrLf)
        End If

        If strInvoiceDateErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE A BLANK INVOICE/DIRECT DEBIT DATE:" & vbCrLf & strInvoiceDateErrorLines.ToString & vbCrLf)
        End If

        If strTravelDateErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE A BLANK TRAVEL DATE:" & vbCrLf & strTravelDateErrorLines.ToString & vbCrLf)
        End If

        If strActivityCodeErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: UNABLE TO FIND RCN ACTIVITY CODES FOR THE FOLLOWING INVOICE/S:" & vbCrLf & strActivityCodeErrorLines.ToString & vbCrLf)
        End If

        If strProjectErrorLines.ToString <> "" Then
            'strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT PROJECT CODE (tot_pono):" & vbCrLf & strProjectErrorLines.ToString & vbCrLf)
        End If

        If strResourceErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN INCORRECT RESOURCE CODE (tot_costc):" & vbCrLf & strResourceErrorLines.ToString & vbCrLf)
        End If

        If strReasonErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THE FOLLOWING INVOICE/S HAVE AN EMPTY REASON FOR TRAVEL:" & vbCrLf & strReasonErrorLines.ToString & vbCrLf)
        End If

        If strTaxCodeErrorLines.ToString <> "" Then
            strAllErrorLines.Append("ERROR: UNABLE TO FIND RCN TAX CODE FOR THE FOLLOWING INVOICE/S:" & vbCrLf & strTaxCodeErrorLines.ToString & vbCrLf)
        End If

        'if there are any errors, return them all
        If strAllErrorLines.ToString <> "" Then
            Return strAllErrorLines.ToString
        End If

        'add the header line
        strLines.Append("Booker ID|Claim Reference|Direct Debit Date|Travel Date|Activity Code|Project Code|Resource|Location or Journey|Line Description|Unit Value|Tax Code|Tax Value|Invoice Number|Notes" & vbCrLf)
        'R2.21 SA - header for credit file
        strLinesCredit.Append("Booker ID|Claim Reference|Direct Debit Date|Travel Date|Activity Code|Project Code|Resource|Location or Journey|Line Description|Unit Value|Tax Code|Tax Value|Invoice Number|Notes" & vbCrLf)


        'Dim strLastInvoiceNo As String = ""
        Dim strHotelFeeErrors As New StringBuilder
        Dim strEbisIDErrors As New StringBuilder

        'declare the total variables
        Dim dblGross As Double = 0
        Dim dblNett As Double = 0
        Dim dblVat As Double = 0
        Dim dblDispVat As Double = 0
        Dim dblTax As Double = 0
        Dim dblCreditGross As Double = 0
        Dim dblCreditNett As Double = 0
        Dim dblCreditVat As Double = 0
        Dim dblCreditDispVat As Double = 0
        Dim dblCreditTax As Double = 0

        'R2.23.2 SA 
        Dim dblRailCharges As Double = 0

        For Each r As RCN In rs

            Dim strBookerFirstname As String = ""
            Dim strBookerLastname As String = ""

            If r.BookerName.ToUpper.StartsWith("MR ") Or r.BookerName.ToUpper.StartsWith("MS ") Or r.BookerName.ToUpper.StartsWith("DR ") Then
                r.BookerName = Trim(r.BookerName.Substring(3))
            ElseIf r.BookerName.ToUpper.StartsWith("MRS ") Then
                r.BookerName = Trim(r.BookerName.Substring(4))
            ElseIf r.BookerName.ToUpper.StartsWith("MISS ") Then
                r.BookerName = Trim(r.BookerName.Substring(5))
            End If

            If r.BookerName.Contains(" ") Then
                strBookerFirstname = r.BookerName.Substring(0, r.BookerName.IndexOf(" ")).ToUpper
                strBookerLastname = r.BookerName.Substring(r.BookerName.IndexOf(" ") + 1).ToUpper
            Else
                strBookerFirstname = r.BookerName.ToUpper
                strBookerLastname = r.BookerName.ToUpper
            End If

            'R2.16 CR - dont create the ID anymore, try and select it from SSO!!
            'create the Ebis ID
            'Dim strEbisID As String = strBookerLastname.ToLower & strBookerFirstname.Substring(0, 1).ToLower
            Dim strEbisID As String = ""
            Dim strBookerEmail As String = r.BookerEmail

            Dim oSsoUser As New clsSSOUser
            oSsoUser = clsSSOUser.findEbisID(strBookerEmail, strBookerFirstname, strBookerLastname)
            If oSsoUser.SsoUserEbisID = "" Then
                If Not strEbisIDErrors.ToString.Contains(r.InvoiceNo) Then
                    strEbisIDErrors.Append(r.InvoiceNo & " - BookerName= " & r.BookerName & " BookerEmail= " & strBookerEmail & vbCrLf)
                End If
            Else
                strEbisID = oSsoUser.SsoUserEbisID.ToLower
            End If

            'R2.17 CR
            'If name couldn't be split earlier but a booker has been found in sso, use sso booker names.
            ' in all other cases use the name from BOSS
            If strBookerFirstname = strBookerLastname AndAlso oSsoUser.SsoUserEbisID <> "" Then
                strBookerFirstname = oSsoUser.SsoUserFirstname.ToUpper
                strBookerLastname = oSsoUser.SsoUserLastName.ToUpper
            End If

            'create the Claim Ref - truncate if over 40 chars
            Dim strClaimRef As String = "NYS " & strBookerFirstname & " " & strBookerLastname
            If strClaimRef.Length > 40 Then
                'over size - truncate
                strClaimRef = strClaimRef.Substring(0, 40)
            End If


            'R2.17 CR
            'change the product code if hotel is inside london
            Dim strCurrentProduct As String = r.Product
            If strCurrentProduct = "H" Then
                If NysDat.clsRCNLondonPostcodes.containsCode(r.SupplierPostcode1) Then
                    strCurrentProduct = "HLon"
                End If
                'R2.21 SA
                Dim strRet As String = NysDat.clsRCNActivityCode.IsConference(r.Identifier)
                If strRet.ToLower = "royal college of nursing" Then
                    strCurrentProduct = "Conf"
                End If
            End If

            'R2.21.2 SA - BUG FIX: project code is not always in the right format 
            'need to check it contains a - 
            If Not r.ProjectCode.Contains("-") Then
                r.ProjectCode = r.ProjectCode.Substring(0, 2) & "-" & r.ProjectCode.Substring(2)
            End If

            'get the activity code & tax code
            Dim oRCNActivity As New NysDat.clsRCNActivityCode
            oRCNActivity = NysDat.clsRCNActivityCode.getByBossProduct(strCurrentProduct)


            'create the line desc
            Dim strLineDesc As New StringBuilder
            'Traveller last name no greater than 7 chars
            If r.TravellerLastname.Length > 7 Then
                strLineDesc.Append(r.TravellerLastname.Substring(0, 7).ToUpper)
            Else
                strLineDesc.Append(r.TravellerLastname)
            End If
            'add only the 1st letter of the firstname
            strLineDesc.Append(" " & r.TravellerFirstname.Substring(0, 1).ToUpper)
            'add the trip reason
            strLineDesc.Append(" " & r.Reason)
            'truncate if over 30 chars
            If strLineDesc.Length > 30 Then
                strLineDesc.Remove(31, strLineDesc.Length - 31)
            End If

            'FIX: TAX codes wrong for international hotels
            Dim strTaxCode As String = cleanRCNString(oRCNActivity.TaxCode)
            If r.Product = "H" And r.ExpenseVat = 0 Then
                'check to see if hotel is International
                'if international then tax code = "O"
                'otherwise it is tax code = "Z"
                If r.IsSupplierInternational Then
                    strTaxCode = "O"
                Else
                    strTaxCode = "Z"
                End If
            End If

            'FIX: delivery tax code
            Dim strActivityCode As String = cleanRCNString(oRCNActivity.ActivityCode)
            If (r.Product = "RM" Or (cleanRCNString(r.Location).ToLower.Contains("delivery") And cleanRCNString(r.Location).ToLower.Contains("special"))) And r.ExpenseVat = 0 Then
                'Exempt
                strTaxCode = "E"
                strActivityCode = "5520-01"
            End If

            'add the row 
            If r.Expense <> 0 Or r.ExpenseVat <> 0 Then
                If r.Expense >= 0 Then 'debit 
                    strLines.Append(cleanRCNString(strEbisID) & "|" & cleanRCNString(strClaimRef) & "|" & pstrDebitDate & "|" & cleanRCNString(r.TravelDate) & "|" & _
            strActivityCode & "|" & cleanRCNString(r.ProjectCode) & "|" & cleanRCNString(r.ResourceCode) & "|" & cleanRCNString(r.Location) & _
            "|" & cleanRCNString(strLineDesc.ToString) & "|" & r.Expense & "|" & strTaxCode & "|" & r.ExpenseVat & "|" & r.InvoiceNo & "|" & Trim(cleanRCNString(r.LineNotes)) & vbCrLf)
                    dblNett += r.Expense
                    dblVat += r.Vat
                    dblDispVat += r.DispVat
                    dblTax += r.Tax
                Else 'credit
                    strLinesCredit.Append(cleanRCNString(strEbisID) & "|" & cleanRCNString(strClaimRef) & "|" & pstrDebitDate & "|" & cleanRCNString(r.TravelDate) & "|" & _
            strActivityCode & "|" & cleanRCNString(r.ProjectCode) & "|" & cleanRCNString(r.ResourceCode) & "|" & cleanRCNString(r.Location) & _
            "|" & cleanRCNString(strLineDesc.ToString) & "|" & r.Expense & "|" & strTaxCode & "|" & r.ExpenseVat & "|" & r.InvoiceNo & "|" & Trim(cleanRCNString(r.LineNotes)) & vbCrLf)
                    dblCreditNett += r.Expense
                    dblCreditVat += r.Vat
                    dblCreditDispVat += r.DispVat
                    dblCreditTax += r.Tax
                End If
            End If

            'R2.23.2 SA 
            dblRailCharges += r.RailCharges
        Next

        If strHotelFeeErrors.ToString <> "" Then
            strAllErrorLines.Append("ERROR: THERE APPEARS TO BE HOTEL FEES ON THE FOLLOWING INVOICE/S, THERE SHOULD NOT BE ANY FOR RCN:" & vbCrLf & strHotelFeeErrors.ToString & vbCrLf)
        End If

        If strEbisIDErrors.ToString <> "" Then
            strAllErrorLines.Append("ERROR: UNABLE TO MATCH BOOKER ON THE FOLLOWING INVOICE/S AGAINST AN EBIS ID:" & vbCrLf & strEbisIDErrors.ToString & vbCrLf)
        End If

        If strAllErrorLines.ToString <> "" Then
            Return strAllErrorLines.ToString
        End If


        'create file here
        'now create the files and show a link to view them
        'create the new folders for today
        makeFolderExist(getConfig("XMLFilePath") & "\RCNG100")
        makeFolderExist(getConfig("XMLFilePath") & "\RCNG100\" & Format(Now, "dd-MM-yyyy"))

        'Create debit file
        Try
            'create the feeder file
            Dim strFileName As String = "NYSG100.csv"
            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\RCNG100\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
            ofiler.Write(strLines.ToString.Replace(",", " ").Replace("|", ","))
            ofiler.Flush()
            ofiler.Close()
        Catch ex As Exception
            Return "Unable to create feeder CSV file, please speak to development team"
        End Try

        'Create credit file
        Try
            'create the feeder file
            Dim strFileName As String = "NYSG100-Credits.csv"
            Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\RCNG100\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
            ofiler.Write(strLinesCredit.ToString.Replace(",", " ").Replace("|", ","))
            ofiler.Flush()
            ofiler.Close()
        Catch ex As Exception
            Return "Unable to create feeder CSV file, please speak to development team"
        End Try


        'create the cover sheet file
        Try
            Dim strCoverFileName As String = "NYSG100-" & Format(Date.Now, "dd-MM-yyyy") & "CoverSheet.rtf"
            Dim strRtfFilePath As String = getConfig("XMLFilePath") & "\RCNG100\" & Format(Now, "dd-MM-yyyy") & "\" & strCoverFileName

            Dim ofiler3 As New System.IO.StreamWriter(strRtfFilePath, False, Encoding.Default)
            Dim strCoverSheet As String = clsNYS.readText(getConfig("XMLFilePath") & "\RCNG100\Template\CoverSheet.rtf")

            strCoverSheet = strCoverSheet.Replace("#dddate#", pstrDebitDate)
            strCoverSheet = strCoverSheet.Replace("#startdate#", pstartdate)
            strCoverSheet = strCoverSheet.Replace("#enddate#", penddate)

            'dblGross = FormatNumber(dblGross, 2)
            'dblNett = dblGross - (dblVat + dblDispVat + dblTax)
            dblVat = FormatNumber(dblVat, 2)
            dblDispVat = FormatNumber(dblDispVat, 2)
            dblTax = FormatNumber(dblTax, 2)
            dblNett = FormatNumber(dblNett, 2) - FormatNumber(dblTax, 2)
            dblGross = FormatNumber((dblNett + dblVat + dblDispVat + dblTax), 2)

            'dblCreditGross = FormatNumber(dblCreditGross, 2)
            'dblCreditNett = dblCreditGross - (dblCreditVat + dblCreditDispVat + dblCreditTax)
            dblCreditVat = FormatNumber(dblCreditVat, 2)
            dblCreditDispVat = FormatNumber(dblCreditDispVat, 2)
            dblCreditTax = FormatNumber(dblCreditTax, 2)
            dblCreditNett = FormatNumber(dblCreditNett, 2) - FormatNumber(dblCreditTax, 2)
            dblCreditGross = FormatNumber((dblCreditNett + dblCreditVat + dblCreditDispVat + dblCreditTax), 2)


            strCoverSheet = strCoverSheet.Replace("#dnet#", "£" & dblNett)
            strCoverSheet = strCoverSheet.Replace("#dtax#", "£" & dblTax)
            strCoverSheet = strCoverSheet.Replace("#dvat#", "£" & dblVat)
            strCoverSheet = strCoverSheet.Replace("#ddvat#", "£" & dblDispVat)
            strCoverSheet = strCoverSheet.Replace("#dgross#", "£" & dblGross)

            strCoverSheet = strCoverSheet.Replace("#cnet#", "£" & dblCreditNett)
            strCoverSheet = strCoverSheet.Replace("#ctax#", "£" & dblCreditTax)
            strCoverSheet = strCoverSheet.Replace("#cvat#", "£" & dblCreditVat)
            strCoverSheet = strCoverSheet.Replace("#cdvat#", "£" & dblCreditDispVat)
            strCoverSheet = strCoverSheet.Replace("#cgross#", "£" & dblCreditGross)

            strCoverSheet = strCoverSheet.Replace("#TOTALNET#", "£" & FormatNumber((dblNett + dblCreditNett), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALTAX#", "£" & FormatNumber((dblTax + dblCreditTax), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALVAT#", "£" & FormatNumber((dblVat + dblCreditVat), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALDVAT#", "£" & FormatNumber((dblDispVat + dblCreditDispVat), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALGROSS#", "£" & FormatNumber(dblGross, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALCREDIT#", "£" & FormatNumber(dblCreditGross, 2))

            strCoverSheet = strCoverSheet.Replace("#TOTALDUE#", "£" & FormatNumber((dblGross + dblCreditGross), 2))

            strCoverSheet = strCoverSheet.Replace("#TOTALDISVAT#", "£" & FormatNumber((dblDispVat + dblCreditDispVat), 2))

            ofiler3.Write(strCoverSheet)
            ofiler3.Flush()
            ofiler3.Close()

            If Not ConvertToPDF(strRtfFilePath, strRtfFilePath.Replace(".rtf", ".pdf"), False) Then
                Throw New Exception("Unable to convert file to PDF")
            End If

            'R2.23 SA 
            clsNYS.SendEmailMessage("accounts@nysgroup.com", getConfig("FeederRailFeesEmailTo"), _
                                    "RCNG100 Manual invoice for the period between" & pstartdate & " and " & penddate, _
                                    "The value for RCNG100 manual invoice should be £" & dblRailCharges, _
                                    "", "", "", "", "", "", "")


        Catch ex As Exception
            Return "Unable to create cover sheet file, please speak to development team"
        End Try

        Return strLines.ToString() & vbCrLf & strLinesCredit.ToString()

    End Function

    'R2.12 CR
    Public Shared Function createFileICE(ByVal pstartdate As String, ByVal penddate As String, pDirectDebitDate As String) As String
        Dim strBookerErrorLines As New StringBuilder
        Dim strTravellerErrorLines As New StringBuilder
        Dim strInvoiceDateErrorLines As New StringBuilder
        Dim strTravelDateErrorLines As New StringBuilder
        Dim strActivityCodeErrorLines As New StringBuilder
        Dim strProjectErrorLines As New StringBuilder
        Dim strResourceErrorLines As New StringBuilder
        Dim strReasonErrorLines As New StringBuilder
        Dim strTaxCodeErrorLines As New StringBuilder
        Dim strAllErrorLines As New StringBuilder

        Dim strLines As New StringBuilder
        Dim strFeeLines As New StringBuilder

        Dim strRet As String = ""

        Dim rs As List(Of ICE)
        rs = ICE.list(pstartdate, penddate)


        'run through and check all fields are ok
        For Each oRow In rs
            If oRow.CostCentreDesc Is Nothing OrElse oRow.CostCentreDesc = "" Then
                strAllErrorLines.Append(vbCrLf & "Invoice " & oRow.InvoiceNo & " line" & oRow.InvoiceLine & ": Cost Centre couldn't be matched to a description. Please re-sync SSO database.")
            End If
            If oRow.ExpenseTypeDesc Is Nothing OrElse oRow.ExpenseTypeDesc = "" Then
                strAllErrorLines.Append(vbCrLf & "Invoice " & oRow.InvoiceNo & " line" & oRow.InvoiceLine & ": Expense Type couldn't be matched to a description. Please re-sync SSO database.")
            End If
            If oRow.ProjectCodeDesc Is Nothing OrElse oRow.ProjectCodeDesc = "" Then
                strAllErrorLines.Append(vbCrLf & "Invoice " & oRow.InvoiceNo & " line" & oRow.InvoiceLine & ": Project Code couldn't be matched to a description. Please re-sync SSO database.")
            End If
        Next

        If strAllErrorLines.ToString <> "" Then
            Return strAllErrorLines.ToString
        End If


        'no errors so build the file
        'add the headers first
        strLines.Append("Nominal Code|")
        strLines.Append("Project|")
        strLines.Append("Costcentre|")
        strLines.Append("Costcentre Desc|")
        strLines.Append("Expense Type Desc|")
        strLines.Append("Detail|")
        strLines.Append("Dr|")
        strLines.Append("Cr|")
        strLines.Append("Vat|")
        strLines.Append("Dis Vat|")
        strLines.Append("Transaction Key 1|")
        strLines.Append("Transaction Key 2|")
        strLines.Append("Transaction Key 3|")
        strLines.Append("Reference|")


        strFeeLines.Append("Nominal Code|")
        strFeeLines.Append("Project|")
        strFeeLines.Append("Costcentre|")
        strFeeLines.Append("Costcentre Desc|")
        strFeeLines.Append("Expense Type Desc|")
        strFeeLines.Append("Detail|")
        strFeeLines.Append("Dr|")
        strFeeLines.Append("Cr|")
        strFeeLines.Append("Vat|")
        strFeeLines.Append("Dis Vat|")
        strFeeLines.Append("Transaction Key 1|")
        strFeeLines.Append("Transaction Key 2|")
        strFeeLines.Append("Transaction Key 3|")
        strFeeLines.Append("Reference|")

        Dim intFeeLinesCount As Integer = 0
        Dim intInvoiceLinesCount As Integer = 0

        'declare the total variables
        Dim dblTotalFeeGross As Double = 0
        Dim dblTotalFeeVat As Double = 0
        Dim dblTotalFeeDisVat As Double = 0
        Dim dblTotalGross As Double = 0
        Dim dblTotalVat As Double = 0
        Dim dblTotalDisVat As Double = 0

        'declare the reference variables
        Dim strInvoiceRef As String = ""
        'Dim strInvoiceDate As String = Date.Now.ToString("dd/MM/yyyy")
        Dim strInvoiceDate As String = pDirectDebitDate

        'ICE want invocie charges to be split: all fees on fee file, all other charges (tickets, etc..) on invoice file. 
        'Cancellation: need to refund full amount to ICE (negative value - credit): on the invoice file 
        'cancellation: need to charge cancellation fee (positive value - debit): on fees file 

        'now the lines
        For Each oRow In rs

            'R2.21c SA
            Dim dbOurcharges As Decimal = 0
            Dim dbOurChargesC As Decimal = 0

            If oRow.OurCharges >= 0 Then
                dbOurcharges = oRow.OurCharges
            Else
                dbOurChargesC = oRow.OurCharges
            End If

            'all trancaction fees to be added to the fee file 
            If oRow.ProductCode = "MF" Or oRow.ProductCode = "TF" Or oRow.ProductTypeCode = "TF" Then
                strFeeLines.Append(vbCrLf)
                strFeeLines.Append(oRow.NominalCode & "|")
                strFeeLines.Append(oRow.ProjectCode & "|")
                strFeeLines.Append(oRow.CostCentre & "|")
                strFeeLines.Append(oRow.CostCentreDesc & "|")
                strFeeLines.Append(oRow.ExpenseTypeDesc & "|")
                strFeeLines.Append(oRow.Description & "|")
                strFeeLines.Append(oRow.Dr & "|")
                strFeeLines.Append(oRow.Cr & "|")
                strFeeLines.Append(oRow.Vat & "|")
                strFeeLines.Append(oRow.DisVat & "|")
                strFeeLines.Append(oRow.TransactionKey1 & "|")
                strFeeLines.Append(oRow.TransactionKey2 & "|")
                strFeeLines.Append(oRow.TransactionKey3 & "|")
                strFeeLines.Append(oRow.Reference & "|")

                intFeeLinesCount += 1

                dblTotalFeeGross += (oRow.Dr + oRow.Cr)
                dblTotalFeeVat += oRow.Vat
                dblTotalFeeDisVat += oRow.DisVat

                'Air: split cancellation charge for all air cancelled bookings. 
            ElseIf oRow.ProductCode = "A" AndAlso oRow.Total < 0 Then

                'Add cancellation charge to the fees file (this needs to be positive value) 
                strFeeLines.Append(vbCrLf)
                strFeeLines.Append(oRow.NominalCode & "|")
                strFeeLines.Append(oRow.ProjectCode & "|")
                strFeeLines.Append(oRow.CostCentre & "|")
                strFeeLines.Append(oRow.CostCentreDesc & "|")
                strFeeLines.Append(oRow.ExpenseTypeDesc & "|")
                strFeeLines.Append(oRow.Description & "|")
                strFeeLines.Append(Math.Abs(oRow.CancellationCharge) & "|") 'it's negative value in DB, change to positive charge and add to file 
                strFeeLines.Append("0.0" & "|")
                strFeeLines.Append(oRow.Vat & "|")
                strFeeLines.Append(oRow.DisVat & "|")
                strFeeLines.Append(oRow.TransactionKey1 & "|")
                strFeeLines.Append(oRow.TransactionKey2 & "|")
                strFeeLines.Append(oRow.TransactionKey3 & "|")
                strFeeLines.Append(oRow.Reference & "|")

                intFeeLinesCount += 1

                dblTotalFeeGross += (Math.Abs(oRow.CancellationCharge))
                dblTotalFeeVat += oRow.Vat
                dblTotalFeeDisVat += oRow.DisVat

                'Add total refunded value to the invoice file (negative value - we are giving them back) 
                strLines.Append(vbCrLf)
                strLines.Append(oRow.NominalCode & "|")
                strLines.Append(oRow.ProjectCode & "|")
                strLines.Append(oRow.CostCentre & "|")
                strLines.Append(oRow.CostCentreDesc & "|")
                strLines.Append(oRow.ExpenseTypeDesc & "|")
                strLines.Append(oRow.Description & "|")
                strLines.Append("0.0" & "|") 'debit is zero 
                strLines.Append(Math.Abs(oRow.Total) & "|") 'add total refunded, as charge is added to the fee file 
                strLines.Append(oRow.Vat & "|")
                strLines.Append(oRow.DisVat & "|")
                strLines.Append(oRow.TransactionKey1 & "|")
                strLines.Append(oRow.TransactionKey2 & "|")
                strLines.Append(oRow.TransactionKey3 & "|")
                strLines.Append(oRow.Reference & "|")

                intInvoiceLinesCount += 1

                dblTotalGross += (oRow.Total)
                dblTotalVat += oRow.Vat
                dblTotalDisVat += oRow.DisVat

                'Rail cancellations are pulled from Boss
                'Rail: split cancellation charge for all air cancelled bookings. 
            ElseIf oRow.ProductTypeCode = "RR" AndAlso oRow.Total < 0 Then

                'Add cancellation charge to the fees file (this needs to be positive value)
                strFeeLines.Append(vbCrLf)
                strFeeLines.Append(oRow.NominalCode & "|")
                strFeeLines.Append(oRow.ProjectCode & "|")
                strFeeLines.Append(oRow.CostCentre & "|")
                strFeeLines.Append(oRow.CostCentreDesc & "|")
                strFeeLines.Append(oRow.ExpenseTypeDesc & "|")
                strFeeLines.Append(oRow.Description & "|")
                strFeeLines.Append(Math.Abs(oRow.CancellationCharge) & "|") 'it's negative value in DB, change to positive charge and add to file 
                strFeeLines.Append("0.0" & "|")
                strFeeLines.Append(oRow.Vat & "|")
                strFeeLines.Append(oRow.DisVat & "|")
                strFeeLines.Append(oRow.TransactionKey1 & "|")
                strFeeLines.Append(oRow.TransactionKey2 & "|")
                strFeeLines.Append(oRow.TransactionKey3 & "|")
                strFeeLines.Append(oRow.Reference & "|")

                intFeeLinesCount += 1

                dblTotalFeeGross += (Math.Abs(oRow.CancellationCharge))
                dblTotalFeeVat += oRow.Vat
                dblTotalFeeDisVat += oRow.DisVat

                'Add total refunded value to the invoice file (negative value - we are giving them back)
                strLines.Append(vbCrLf)
                strLines.Append(oRow.NominalCode & "|")
                strLines.Append(oRow.ProjectCode & "|")
                strLines.Append(oRow.CostCentre & "|")
                strLines.Append(oRow.CostCentreDesc & "|")
                strLines.Append(oRow.ExpenseTypeDesc & "|")
                strLines.Append(oRow.Description & "|")
                strLines.Append("0.0" & "|") 'debit is zero 
                strLines.Append(Math.Abs(oRow.Total) & "|") 'add total refunded, as charge is added to the fee file 
                strLines.Append(oRow.Vat & "|")
                strLines.Append(oRow.DisVat & "|")
                strLines.Append(oRow.TransactionKey1 & "|")
                strLines.Append(oRow.TransactionKey2 & "|")
                strLines.Append(oRow.TransactionKey3 & "|")
                strLines.Append(oRow.Reference & "|")

                intInvoiceLinesCount += 1

                dblTotalGross += (oRow.Total)
                dblTotalVat += oRow.Vat
                dblTotalDisVat += oRow.DisVat

                'R2.21c SA
                'Rail invoices to be added to the invoice file -no fees in Boss - it's all done through interceptor 
                'ElseIf oRow.ProductTypeCode = "RR" AndAlso oRow.Total > 0 Then

                '    'Comment after 1st march feeder run - fees will be saved in DB table not in Boss anymore. 
                '    'first fees file - add ourCharges as fees 
                '    strFeeLines.Append(vbCrLf)
                '    strFeeLines.Append(oRow.NominalCode & "|")
                '    strFeeLines.Append(oRow.ProjectCode & "|")
                '    strFeeLines.Append(oRow.CostCentre & "|")
                '    strFeeLines.Append(oRow.CostCentreDesc & "|")
                '    strFeeLines.Append(oRow.ExpenseTypeDesc & "|")
                '    strFeeLines.Append(oRow.Description & "|")
                '    strFeeLines.Append(dbOurcharges & "|")
                '    strFeeLines.Append(dbOurChargesC & "|")
                '    strFeeLines.Append(oRow.Vat & "|")
                '    strFeeLines.Append(oRow.DisVat & "|")
                '    strFeeLines.Append(oRow.TransactionKey1 & "|")
                '    strFeeLines.Append(oRow.TransactionKey2 & "|")
                '    strFeeLines.Append(oRow.TransactionKey3 & "|")
                '    strFeeLines.Append(oRow.Reference & "|")

                '    intFeeLinesCount += 1

                '    dblTotalFeeGross += (dbOurcharges + dbOurChargesC)
                '    dblTotalFeeVat += oRow.Vat
                '    dblTotalFeeDisVat += oRow.DisVat

                '    'now invoice file - add ticket charge without our charges 
                '    strLines.Append(vbCrLf)
                '    strLines.Append(oRow.NominalCode & "|")
                '    strLines.Append(oRow.ProjectCode & "|")
                '    strLines.Append(oRow.CostCentre & "|")
                '    strLines.Append(oRow.CostCentreDesc & "|")
                '    strLines.Append(oRow.ExpenseTypeDesc & "|")
                '    strLines.Append(oRow.Description & "|")
                '    strLines.Append(oRow.Dr - dbOurcharges & "|")
                '    strLines.Append(oRow.Cr - dbOurChargesC & "|")
                '    strLines.Append(oRow.Vat & "|")
                '    strLines.Append(oRow.DisVat & "|")
                '    strLines.Append(oRow.TransactionKey1 & "|")
                '    strLines.Append(oRow.TransactionKey2 & "|")
                '    strLines.Append(oRow.TransactionKey3 & "|")
                '    strLines.Append(oRow.Reference & "|")

                '    intInvoiceLinesCount += 1

                '    dblTotalGross += ((oRow.Dr - dbOurcharges) + (oRow.Cr - dbOurChargesC))
                '    dblTotalVat += oRow.Vat
                '    dblTotalDisVat += oRow.DisVat

            Else
                'R2.12 SA
                strLines.Append(vbCrLf)

                strLines.Append(oRow.NominalCode & "|")
                strLines.Append(oRow.ProjectCode & "|")
                strLines.Append(oRow.CostCentre & "|")
                strLines.Append(oRow.CostCentreDesc & "|")
                strLines.Append(oRow.ExpenseTypeDesc & "|")
                strLines.Append(oRow.Description & "|")
                strLines.Append(oRow.Dr & "|")
                strLines.Append(Math.Abs(oRow.Cr) & "|")
                strLines.Append(oRow.Vat & "|")
                strLines.Append(oRow.DisVat & "|")
                strLines.Append(oRow.TransactionKey1 & "|")
                strLines.Append(oRow.TransactionKey2 & "|")
                strLines.Append(oRow.TransactionKey3 & "|")
                strLines.Append(oRow.Reference & "|")

                intInvoiceLinesCount += 1

                dblTotalGross += (oRow.Dr + oRow.Cr)
                dblTotalVat += oRow.Vat
                dblTotalDisVat += oRow.DisVat

            End If

            'reset this variable every row - so that the invoice ref that pulls through to the cover sheet is the ref of the last invoice in the list
            strInvoiceRef = oRow.InvoiceNo
        Next

        'Uncomment after the 1st March feeder run 
        'R2.21c SA Finally:: 
        'From friday 1st March, evolvi will not show any fees for ICE, and all fees will be added through Interceptor.
        'Rail fees needs to be are pulled from (InterceptorCharge table) 
        Dim rlc As List(Of ICE)
        rlc = ICE.listRail(pstartdate, penddate)

        'R2.23 SA 
        Dim dblRailCharges As Double = 0
        'for all rail booking 
        For Each oRow In rlc

            strFeeLines.Append(vbCrLf)

            strFeeLines.Append(oRow.NominalCode & "|")
            strFeeLines.Append(oRow.ProjectCode & "|")
            strFeeLines.Append(oRow.CostCentre & "|")
            strFeeLines.Append(oRow.CostCentreDesc & "|")
            strFeeLines.Append(oRow.ExpenseTypeDesc & "|")
            strFeeLines.Append(oRow.Description & "|")
            strFeeLines.Append(oRow.Dr & "|")
            strFeeLines.Append(oRow.Cr & "|")
            strFeeLines.Append(oRow.Vat & "|")
            strFeeLines.Append(oRow.DisVat & "|")
            strFeeLines.Append(oRow.TransactionKey1 & "|")
            strFeeLines.Append(oRow.TransactionKey2 & "|")
            strFeeLines.Append(oRow.TransactionKey3 & "|")
            strFeeLines.Append(oRow.Reference & "|")

            intFeeLinesCount += 1

            dblTotalFeeGross += (oRow.Dr + oRow.Cr)
            dblTotalFeeVat += oRow.Vat
            dblTotalFeeDisVat += oRow.DisVat

            'reset this variable every row - so that the invoice ref that pulls through to the cover sheet is the ref of the last invoice in the list
            strInvoiceRef = oRow.InvoiceNo

            'R2.23 SA
            dblRailCharges += oRow.Dr
        Next

        'now create the files and show a link to view them
        'create the new folders for today
        makeFolderExist(getConfig("XMLFilePath") & "\ICE")
        makeFolderExist(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy"))

        'create the invoice file
        If intInvoiceLinesCount > 0 Then
            Try
                'create the feeder file
                Dim strFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & ".csv"

                'check it doesn't already exist first - if it does then delete it
                If IO.File.Exists(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName) Then
                    IO.File.Delete(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                End If

                Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
                ofiler.Write(strLines.ToString.Replace(",", " ").Replace("|", ","))
                ofiler.Flush()
                ofiler.Close()
            Catch ex As Exception
                Return "Unable to create feeder CSV file, please speak to development team"
            End Try

            strRet = strLines.ToString
        Else
            'tell the user there are no invoices on this run
            strRet = "**** There are no invoices on this run ****" & vbCrLf
        End If

        'create the fees file
        If intFeeLinesCount > 0 Then
            Try
                'create the feeder file
                Dim strFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & "_Fees.csv"

                'check it doesn't already exist first - if it does then delete it
                If IO.File.Exists(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName) Then
                    IO.File.Delete(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                End If

                Dim ofiler As New System.IO.StreamWriter(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)
                ofiler.Write(strFeeLines.ToString.Replace(",", " ").Replace("|", ","))
                ofiler.Flush()
                ofiler.Close()
            Catch ex As Exception
                Return "Unable to create feeder CSV file, please speak to development team"
            End Try

            strRet &= strLines.ToString
        Else
            'tell the user there are no fees on this run
            strRet &= "**** There are no fees on this run ****" & vbCrLf
        End If

        'create the cover sheet file
        Try
            Dim strCoverFileName As String = "NYS-" & Format(Date.Now, "dd-MM-yyyy") & " CoverSheet.rtf"
            Dim strRtfFilePath As String = getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strCoverFileName

            Dim ofiler3 As New System.IO.StreamWriter(strRtfFilePath, False, Encoding.Default)
            Dim strCoverSheet As String = clsNYS.readText(getConfig("XMLFilePath") & "\ICE\Template\CoverSheet.rtf")

            'check it doesn't already exist first - if it does then delete it
            If IO.File.Exists(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strRtfFilePath.Replace(".rtf", ".pdf")) Then
                IO.File.Delete(getConfig("XMLFilePath") & "\ICE\" & Format(Now, "dd-MM-yyyy") & "\" & strRtfFilePath.Replace(".rtf", ".pdf"))
            End If

            strCoverSheet = strCoverSheet.Replace("#TRXNUM#", strInvoiceRef)
            strCoverSheet = strCoverSheet.Replace("#TRXDATE#", strInvoiceDate)
            strCoverSheet = strCoverSheet.Replace("#STARTDATE#", pstartdate)
            strCoverSheet = strCoverSheet.Replace("#ENDDATE#", penddate)

            dblTotalGross = FormatNumber(dblTotalGross, 2)
            dblTotalVat = FormatNumber(dblTotalVat, 2)
            dblTotalFeeGross = FormatNumber(dblTotalFeeGross, 2)
            dblTotalFeeVat = FormatNumber(dblTotalFeeVat, 2)
            dblTotalFeeDisVat = FormatNumber(dblTotalFeeDisVat, 2)

            strCoverSheet = strCoverSheet.Replace("#TOTALNET#", "£" & FormatNumber(dblTotalGross - dblTotalVat, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALVAT#", "£" & FormatNumber(dblTotalVat, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALGROSS#", "£" & FormatNumber(dblTotalGross, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALDISVAT#", "£" & FormatNumber(dblTotalDisVat + dblTotalFeeDisVat, 2))

            strCoverSheet = strCoverSheet.Replace("#TOTALFEENET#", "£" & FormatNumber((dblTotalFeeGross - dblTotalFeeVat), 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALFEEVAT#", "£" & FormatNumber(dblTotalFeeVat, 2))
            strCoverSheet = strCoverSheet.Replace("#TOTALFEEGROSS#", "£" & FormatNumber(dblTotalFeeGross, 2))
            'strCoverSheet = strCoverSheet.Replace("#TOTALCREDITDISVAT#", "£" & FormatNumber(dblTotalCreditDisVat, 2))

            strCoverSheet = strCoverSheet.Replace("#INVOICETOTALNET#", "£" & FormatNumber((dblTotalGross - dblTotalVat) + (dblTotalFeeGross - dblTotalFeeVat), 2))
            strCoverSheet = strCoverSheet.Replace("#INVOICETOTALVAT#", "£" & FormatNumber(dblTotalVat + dblTotalFeeVat, 2))
            strCoverSheet = strCoverSheet.Replace("#INVOICETOTALGROSS#", "£" & FormatNumber(dblTotalGross + dblTotalFeeGross, 2))

            strCoverSheet = strCoverSheet.Replace("#INVOICEDUE#", "£" & FormatNumber(dblTotalGross + dblTotalFeeGross, 2))

            ofiler3.Write(strCoverSheet)
            ofiler3.Flush()
            ofiler3.Close()

            If Not ConvertToPDF(strRtfFilePath, strRtfFilePath.Replace(".rtf", ".pdf"), False) Then
                Throw New Exception("Unable to convert file to PDF")
            End If

            'R2.23.1.AI
            clsNYS.SendEmailMessage("accounts@nysgroup.com", getConfig("FeederRailFeesEmailTo"), _
                                             "ICE Manual invoice for the period between" & pstartdate & " and " & penddate, _
                                             "The value for ICE manual invoice should be " & dblRailCharges, _
                                             "", "", "", "", "", "", "")
        Catch ex As Exception
            Return "Unable to create cover sheet file, please speak to development team"
        End Try

        Return strRet
    End Function
End Class
