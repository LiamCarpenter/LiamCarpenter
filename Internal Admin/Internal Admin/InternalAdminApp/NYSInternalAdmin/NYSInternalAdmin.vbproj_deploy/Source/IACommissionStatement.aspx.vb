Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports BossData
Imports NysDat
Imports EvoUtilities.CollectionUtils

Partial Public Class IACommissionStatement
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
                    populateVenues()
                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IACommissionClaim", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateVenues()
        Using New clslogger(log, className, "populateVenues")
            Dim oVenues As New List(Of clsCommission)
            oVenues = clsCommission.bossOutCommVenues
            ddVenues.Items.Clear()
            ddVenues.Items.Add(New ListItem("All venues", "0"))

            For Each oVenu As clsCommission In oVenues
                ddVenues.Items.Add(New ListItem(oVenu.BossCode & " # " & oVenu.Venue & " # " & oVenu.Email, oVenu.Ref))
            Next
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IACommissionStatement", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If ddVenues.SelectedIndex < 0 Then
                    Throw New EvoFriendlyException("Please select from the venues list.", "Info")
                ElseIf ddVenues.SelectedIndex = 0 Then 'all
                    getVenues(True)
                Else
                    getVenues(False)
                End If
            Catch ex As Exception
                handleexception(ex, "IACommissionStatement", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub getVenues(ByVal pbAll As Boolean)
        Using New clslogger(log, className, "createStatement")
            Dim strBoss As String = ""
            Dim strEmail As String = ""
            If Not pbAll Then
                Dim list As ICollection(Of String) = split(ddVenues.SelectedItem.Text, "#")
                Dim intCount As Integer = 1

                For Each str As String In list
                    If intCount = 1 Then
                        strBoss = str.Trim
                    ElseIf intCount = 3 Then
                        strEmail = str.Trim
                    End If
                    intCount += 1
                Next

                If Regex.IsMatch(strEmail, emailRegex) Then

                    'R2.21.1 CR
                    Dim strStatsPath As String = getConfig("CommStatspath")
                    If strStatsPath.Contains("C:\") Then
                        strStatsPath = strStatsPath.Replace("C:\", "\\" & Environment.MachineName & "\")
                    End If

                    Dim strFile As String = createStatements(strBoss, strEmail)
                    hlStatement.NavigateUrl = strStatsPath & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFile
                    hlStatement.Text = "View statement for: " & strBoss
                    hlStatement.Visible = True

                    'show the email address
                    lblEmail.Text = "Selected Email: " & strEmail
                Else
                    txtResult2.Text = "Email not valid for: " & ddVenues.SelectedItem.Text
                End If

            Else
                hlStatement.NavigateUrl = ""
                hlStatement.Text = ""
                hlStatement.Visible = False
                txtResult2.Text = "Emails not valid for: " & vbCrLf
                For iCount As Integer = 1 To ddVenues.Items.Count - 1
                    Dim list As ICollection(Of String) = split(ddVenues.Items(iCount).Text, "#")
                    Dim intCount As Integer = 1

                    For Each str As String In list
                        If intCount = 1 Then
                            strBoss = str.Trim
                        ElseIf intCount = 3 Then
                            strEmail = str.Trim
                        End If
                        intCount += 1
                    Next
                    If Regex.IsMatch(strEmail, emailRegex) Then
                        createStatements(strBoss, strEmail)
                    Else
                        txtResult2.Text = txtResult2.Text & ddVenues.Items(iCount).Text & vbCrLf
                    End If
                Next
            End If

        End Using
    End Sub

    Private Function createStatements(ByVal pstrBoss As String, ByVal pstrEmail As String) As String
        Using New clslogger(log, className, "createStatements")
            Dim oVenues As New List(Of clsCommission)
            Dim strFileName As String = ""

            'R2.17 CR - update the SQL before running
            Dim oInvoices As New List(Of clsCommission)
            oInvoices = clsCommission.bossOutComm("", Format(Now, "dd/MM/yyyy"), "", pstrBoss)
            For Each oInvoice In oInvoices
                If clsBoss.runMainSelect(False, oInvoice.invoiceid, "", "", "") <> "Successful run" Then
                    Throw New EvoFriendlyException("SQL did not update correctly from BOSS, see Development Team.", "Check Details")
                End If
            Next
            oInvoices = Nothing


            oVenues = clsCommission.bossOutComm("", Format(Now, "dd/MM/yyyy"), "", pstrBoss)

            'R?? SA - check venue comm is not empty 
            If oVenues.Count > 0 Then

                Dim csv As New StringBuilder
                csv.Append("INVOICE ID,SUPPLIER NAME,INVOICE AMOUNT (GBP),COMM CURRENCY,COMM NETT,COMM VAT,COMM DUE,INVOICE DATE,LEAD NAME,REF,START/ARRIVAL DATE,PRODUCT" & vbNewLine)

                Dim strError As New StringBuilder


                'R2.17 CR
                'loop through the records and count the number of invoice lines per invoice
                Dim dictInvoiceLines As New Dictionary(Of String, Integer)
                For Each ovenu As clsCommission In oVenues
                    If dictInvoiceLines.ContainsKey(ovenu.invoiceid) Then
                        dictInvoiceLines(ovenu.invoiceid) += 1
                    Else
                        dictInvoiceLines.Add(ovenu.invoiceid, 1)
                    End If
                Next

                'R2.17 SA
                'Variables to calculate totals
                Dim totalnett As Decimal = 0
                Dim totalvat As Decimal = 0
                Dim totaldue As Decimal = 0

                'R2.17 CR
                Dim dblRunningCommOverrideDue As Double = 0
                Dim dblRunningCommOverrideNett As Double = 0
                Dim dblRunningCommOverrideVat As Double = 0
                Dim dblRunningCommOverrideReceived As Double = 0
                Dim strLastInvoiceID As String = ""
                Dim intInvoiceLineCount As Integer = 0

                Dim strCommCurrency As String = "GBP"
                Dim strPreviousCurrency As String = ""

                For Each oVenu As clsCommission In oVenues
                    'R2.17 CR - check for comission override
                    Dim oOverrides As New List(Of BossCommissionOverride)
                    oOverrides = BossCommissionOverride.getOverride(oVenu.invoiceid)
                    Dim commnett As Decimal = 0
                    Dim commvat As Decimal = 0
                    Dim commrec As Decimal = 0
                    Dim commdue As Decimal = 0
                    Dim strpaynet As String = ""


                    'R2.17 CR
                    If strLastInvoiceID <> oVenu.invoiceid Then
                        intInvoiceLineCount = 1
                        'reset the running override commission amounts
                        dblRunningCommOverrideDue = 0
                        dblRunningCommOverrideNett = 0
                        dblRunningCommOverrideVat = 0
                        dblRunningCommOverrideReceived = 0
                    Else
                        intInvoiceLineCount += 1
                    End If

                    'R2.17 CR
                    Dim blnCommissionOverride As Boolean = False

                    If oOverrides.Count > 0 Then
                        'R2.17 CR
                        blnCommissionOverride = True

                        For Each oSql As BossCommissionOverride In oOverrides
                            commnett = commnett + oSql.CommissionNett
                            commvat = commvat + oSql.CommissionVat
                            commrec = commrec + oSql.CommissionReceived
                            commdue = commdue + oSql.CommissionDue
                            strpaynet = strpaynet & CStr(oSql.PayNet) & ":"

                            'R2.16 CR
                            strCommCurrency = oSql.Currency
                        Next
                    Else
                        commnett = oVenu.inm_comamt
                        commvat = oVenu.inm_vtoncm
                        commrec = oVenu.inm_comrcv
                        commdue = oVenu.inm_comdue
                        strCommCurrency = "GBP"
                    End If

                    'R2.17 SA 
                    totalnett = totalnett + commnett
                    totalvat = totalvat + commvat
                    totaldue = totaldue + commdue

                    'R2.17 CR - if more than one invoice line and commission override has been done, then split the comm
                    ' Commission override is done per invoice - statements bring invoices through per line, so cant repeat the override values.
                    If dictInvoiceLines(oVenu.invoiceid) > 1 AndAlso blnCommissionOverride Then

                        If intInvoiceLineCount = dictInvoiceLines(oVenu.invoiceid) Then
                            'for the last line, subtract the running total from the full amount to keep decimals correct
                            commdue = commdue - dblRunningCommOverrideDue
                            commnett = commnett - dblRunningCommOverrideNett
                            commvat = commvat - dblRunningCommOverrideVat
                            commrec = commrec - dblRunningCommOverrideReceived
                        Else
                            commdue = FormatNumber(commdue / dictInvoiceLines(oVenu.invoiceid), 2)
                            commnett = FormatNumber(commnett / dictInvoiceLines(oVenu.invoiceid), 2)
                            commvat = FormatNumber(commvat / dictInvoiceLines(oVenu.invoiceid), 2)
                            commrec = FormatNumber(commrec / dictInvoiceLines(oVenu.invoiceid), 2)

                            dblRunningCommOverrideDue += commdue
                            dblRunningCommOverrideNett += commnett
                            dblRunningCommOverrideVat += commvat
                            dblRunningCommOverrideReceived += commrec
                        End If
                    End If

                    'R2.17 CR - change to use new values
                    'For Each s As String In makeList(oVenu.invoiceid, oVenu.sup_name, CStr(oVenu.inm_amount), CStr(oVenu.inm_comamt), CStr(oVenu.inm_vtoncm), CStr(oVenu.inm_comdue), _
                    '                                 oVenu.inm_invdt, oVenu.inm_ldname, oVenu.inm_docmnt, oVenu.inm_start, oVenu.product)
                    For Each s As String In makeList(oVenu.invoiceid, oVenu.sup_name, CStr(oVenu.inm_amount), strCommCurrency, CStr(commnett), CStr(commvat), CStr(commdue), _
                                                     oVenu.inm_invdt, oVenu.inm_ldname, oVenu.inm_docmnt, oVenu.inm_start, oVenu.product)
                        csv.Append(toCSVCell(s) & ",")
                    Next
                    csv.Append(vbNewLine)


                    'R2.17 CR
                    If strPreviousCurrency <> "" AndAlso strPreviousCurrency <> strCommCurrency Then
                        'ERROR - we have two different currencies for this invoice, can't put on statement.
                        strError.Replace(strLastInvoiceID & vbCrLf, "")
                        strError.Replace(oVenu.invoiceid & vbCrLf, "")
                        strError.Append(strLastInvoiceID & vbCrLf & oVenu.invoiceid & vbCrLf)
                    End If

                    'R2.17 CR
                    strLastInvoiceID = oVenu.invoiceid
                    strPreviousCurrency = strCommCurrency
                Next


                'R2.17 SA
                'Add an empty line to the statement followed by the totals 
                csv.Append(vbNewLine)
                csv.Append(", , ," & "TOTAL" & "," & CStr(totalnett) & "," & CStr(totalvat) & "," & CStr(totaldue) & ",,,,, " & vbNewLine)

                'R2.17 CR
                If strError.Length > 0 Then
                    Throw New EvoFriendlyException("The following invoices have multiple commission currencies specified, please ammend: " & strError.ToString, "Check Details")
                End If



                makeFolderExist(getConfig("CommStatspath") & "\" & Format(Now, "dd-MM-yyyy"))

                strFileName = pstrBoss & ".csv"

                If IO.File.Exists(getConfig("CommStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName) Then
                    IO.File.Delete(getConfig("CommStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                End If

                Dim ofiler As New System.IO.StreamWriter(getConfig("CommStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, False, Encoding.Default)

                ofiler.Write(csv.ToString)
                ofiler.Flush()
                ofiler.Close()

                'R2.16 CR - try to fix the 'readonly' issue Ann found
                ofiler = Nothing
                Threading.Thread.Sleep(2000)

                'R2.17 CR - don't send an email here!!
                ' new button to auto send the email
                'Dim strLocalEmail As String = pstrEmail
                'If getConfig("EmailTest") = "True" Then
                '    strLocalEmail = "nick.massarella@nysgroup.com"
                'End If

                'Dim strMessage As String = readText(getConfig("CommStatspath") & "\Statement.htm")

                'If SendEmailMessage("commissions@nysgroup.com", strLocalEmail, "Commission statement", strMessage, _
                '                    getConfig("CommStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, "", "", "", "", "", "") Then

                'End If

            Else
                Throw New EvoFriendlyException("No statement to generate!", "Info")
            End If

            Return strFileName
        End Using
    End Function

    'Private Function ConvertToPDF(ByVal inFile As String, ByVal outFile As String) As Boolean
    '    Using New clslogger(log, className, "ConvertToPDF")
    '        Try
    '            Dim p As New SautinSoft.PdfMetamorphosis()

    '            p.Serial = "10022099295"

    '            p.PageStyle.PageOrientation.Portrait()

    '            If p IsNot Nothing Then
    '                Dim rtfFile As String = inFile
    '                Dim pdfFile As String = outFile

    '                Dim result As Integer = p.RtfToPdfConvertFile(rtfFile, pdfFile)
    '            End If

    '            If IO.File.Exists(outFile) Then
    '                IO.File.Delete(inFile)
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        Catch ex As Exception
    '            log.Error(ex.Message)
    '            Return False
    '        End Try

    '    End Using
    'End Function

    Private Sub IACommissionStatement_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IACommissionStatement_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IACommissionStatement"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IACommissionStatement", Me.Page)
            End Try
        End Using
    End Sub

    'R2.17 CR
    Protected Sub btnSendEmail_Click(sender As Object, e As EventArgs) Handles btnSendEmail.Click
        Try
            Dim list As ICollection(Of String) = split(ddVenues.SelectedItem.Text, "#")
            Dim intCount As Integer = 1
            Dim strEmail As String = ""
            Dim strBoss As String = ""
            Dim strFileName As String = ""

            For Each str As String In list
                If intCount = 1 Then
                    strBoss = str.Trim
                ElseIf intCount = 3 Then
                    strEmail = str.Trim
                End If
                intCount += 1
            Next

            strfilename = strBoss & ".csv"

            Dim strLocalEmail As String = strEmail
            If getConfig("EmailTest") = "True" Then
                strLocalEmail = "nick.massarella@nysgroup.com"
            End If

            Dim strMessage As String = readText(getConfig("CommStatspath") & "\Statement.htm")

            If SendEmailMessage("commissions@nysgroup.com", strLocalEmail, "Commission statement", strMessage, _
                                getConfig("CommStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, "", "", "", "", "", "") Then
                Throw New EvoFriendlyException("Email Sent Successfully to " & strLocalEmail, "Information")
            End If
        Catch ex As Exception
            handleexception(ex, "IACommissionStatement", Me.Page)
        End Try
    End Sub
End Class
