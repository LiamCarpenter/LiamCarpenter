Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports BossData
Imports NysDat
Imports EvoUtilities.CollectionUtils
Imports Microsoft.Office.Interop.Excel
'R2.18 SA - created class
Partial Public Class IAClientStatements
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

                    populateBossRef()
                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateBossRef()
        Using New clslogger(log, className, "populateBossCode")
            Try
                ddClients.Items.Clear()
                ddClients.Items.Add(New ListItem("Please select", ""))
                Dim oBossRefList As List(Of clsClientStatementDetails)
                oBossRefList = clsClientStatementDetails.listBossRef()
                For Each gr As clsClientStatementDetails In oBossRefList
                    ddClients.Items.Add(gr.BossRef)
                Next
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAClientStatements", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If ddClients.SelectedValue = "" Then
                    btnSendEmail.Visible = False
                    Throw New EvoFriendlyException("Please select from the client list.", "Info")
                Else
                    lblEmail.Text = "Constructing File XLSX Attachment"
                    'R2.3 SA- for Piksel - different statement is required
                    If ddClients.SelectedValue.ToUpper = "PIKSELUK0" Or _
                        ddClients.SelectedValue.ToUpper = "PIKSELFR0" Or ddClients.SelectedValue.ToUpper = "PIKSELIT0" Then
                        getClientStatementPiksel(ddClients.SelectedValue, False)
                    Else
                        getClientStatements(False)
                    End If

                End If
            Catch ex As Exception
                handleexception(ex, "IAClientStatement", Me.Page)
            End Try
        End Using

        'AM Test against boss direct
        'BossDirect()


    End Sub

    Protected Sub BossDirect()
        'AM - to test against micheal's CSV
        Dim sql As String = "SELECT apar.* FROM apar INNER JOIN customer ON a_id = cus_id WHERE a_due <> 0 AND cus_grpid2 = '" + ddClients.SelectedItem.Text + "' ORDER BY a_invdate"
        Dim ds As DataSet = clsBoss.runBOSSSQL(sql)

        Dim sb As New StringBuilder()

        Dim columnNames As String() = ds.Tables(0).Columns.Cast(Of DataColumn)().[Select](Function(column) column.ColumnName).ToArray()
        sb.AppendLine(String.Join(",", columnNames))

        For Each row As DataRow In ds.Tables(0).Rows
            Dim fields As String() = row.ItemArray.[Select](Function(field) field.ToString()).ToArray()
            sb.AppendLine(String.Join(",", fields))
        Next

        Dim strDateForFileName As String = Date.Now.ToString(getConfig("ClientStatementFileDateFormat"))
        Dim strFileName = ddClients.SelectedItem.Text & "-" & strDateForFileName & "_check.csv"
        Dim fullpath = getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName

        If IO.File.Exists(fullpath) Then
            IO.File.Delete(fullpath)
        End If

        File.WriteAllText(fullpath, sb.ToString())
    End Sub

    Protected Sub btnCSV_Click(sender As Object, e As EventArgs) Handles btnCSV.Click
  
        If ddClients.SelectedValue = "" Then
            btnSendEmail.Visible = False
            Throw New EvoFriendlyException("Please select from the client list.", "Info")
        Else
            btnSendEmail.Visible = False
            hlStatement.Text = ""
            hlStatement.NavigateUrl = ""
            hlStatement.Visible = False
            lblEmail.Text = "Constructing File CSV Attachment"
            'R2.3 SA- for Piksel - different statement is required
            If ddClients.SelectedValue.ToUpper = "PIKSELUK0" Or ddClients.SelectedValue.ToUpper = "PIKSELFR0" Or ddClients.SelectedValue.ToUpper = "PIKSELIT0" Then
                getClientStatementPiksel(ddClients.SelectedValue, True)
            Else
                getClientStatements(True)
            End If
        End If

        'AM Test against boss direct
        BossDirect()
    End Sub
    Private Sub getClientStatements(ByVal csv As Boolean)
        Using New clsLoggerDat(log, className, "getClientStatement")

            Dim oStatements As New List(Of ClientStatement_autogenerated)
            oStatements = ClientStatement_autogenerated.getInvoices(ddClients.SelectedItem.Text)

            'R2.18 CR - populate all the client details, means we dont have to run multiple SQL queries!!
            Dim oStatementClient As New clsClientStatementDetails
            oStatementClient = clsClientStatementDetails.getDetails(ddClients.SelectedItem.Text)

            ''R2.21.2 SA 
            'for each invoice in ostatements run boss updater 
            Dim strResult As String = ""
            For Each item As ClientStatement_autogenerated In oStatements
                'update each invoice by running boss table updater 
                strResult = clsBoss.runMainSelect(False, item.InvoiceNumber, "", "", "")
                If strResult <> "Successful run" Then
                    log.Error("Error: " & strResult & " - Invoice sync failed for Invoice (This could be temporary network issues try again later) " & item.InvoiceNumber & " - for client " & ddClients.SelectedItem.Text)
                End If
                strResult = ""
            Next

            ' after updating all invoices re-generate the statement 
            oStatements = ClientStatement_autogenerated.getInvoices(ddClients.SelectedItem.Text)

            'R2.18 CR
            Dim strEmail As String = oStatementClient.EmailAddress
            'Admin_autogenerated.getEmail(ddClients.SelectedItem.Text)
            ''R2.18 CR
            If oStatementClient.SendEmail = False Then
                btnSendEmail.Visible = False
            Else
                btnSendEmail.Visible = True
            End If


            'R2.20C CR - check validity of multiple email addresses
            Dim blnAllEmailsOK As Boolean = True
            If strEmail.Contains(";") Then
                'multiple addresses - check them all
                For Each strSingleEmailAddress As String In strEmail.Split(";")
                    'check each email address for validity
                    If strSingleEmailAddress <> "" Then
                        If Not Regex.IsMatch(strSingleEmailAddress.Trim, emailRegex) Then
                            blnAllEmailsOK = False
                        End If
                    End If
                Next
            Else
                If Not Regex.IsMatch(strEmail, emailRegex) Then
                    blnAllEmailsOK = False
                End If
            End If

            'If Regex.IsMatch(strEmail, emailRegex) Then
            If blnAllEmailsOK = True Then
                If oStatements.Count > 0 Then
                    'R2.21.1 CR
                    Dim strStatsPath As String = getConfig("ClientStatspath")
                    If strStatsPath.Contains("C:\") Then
                        strStatsPath = strStatsPath.Replace("C:\", "\\" & Environment.MachineName & "\")
                    End If

                    'R2.23 SA - removed  blnShowSecondRef, False
                    'R2.21.1 SA - added oStatementClient.ShowBookerName
                    'Dim strFile As String = createStatement(oStatements)
                    Dim strFile As String = ""
                    If csv Then
                        strFile = createStatementCSV(oStatements)
                    Else
                        'strFile = createStatement(oStatements) or
                        If ddClients.SelectedValue.ToUpper = "PIKSELUK0" Or ddClients.SelectedValue.ToUpper = "PIKSELFR0" Or ddClients.SelectedValue.ToUpper = "PIKSELIT0" Then
                            getClientStatementPiksel(ddClients.SelectedValue, False)
                        Else
                            strFile = ASinServiceClientStatementCreateFile(oStatements, ddClients.SelectedItem.Text)
                        End If
                    End If

                    'R2.21.3 CR - if no file name then no lines are applicable on the statement
                    If strFile = "" Then
                        btnSendEmail.Visible = False
                        hlStatement.Text = ""
                        hlStatement.NavigateUrl = ""
                        hlStatement.Visible = False
                        lblEmail.Text = "No rows to send."
                    Else
                        hlStatement.NavigateUrl = strStatsPath & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFile
                        hlStatement.Text = "View statement for: " & ddClients.SelectedItem.Text & " " & strStatsPath & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFile
                        hlStatement.Visible = True
                        lblEmail.Text = "Selected Email: " & strEmail
                    End If

                    lblEmail.Visible = True

                Else
                    btnSendEmail.Visible = False
                    txtResult2.Text = "No Outstanding Invoices for: " & ddClients.SelectedItem.Text
                End If

            Else
                btnSendEmail.Visible = False
                txtResult2.Text = "Email not valid for: " & ddClients.SelectedItem.Text
            End If
        End Using
    End Sub

    Private Sub getClientStatementPiksel(ByVal pBossCode As String, ByVal IsCSV As Boolean)
        Using New clsLoggerDat(log, className, "getClientStatementPiksel")

            'get invoice details
            Dim oStatements As New List(Of ClientStatement_Piksel)
            oStatements = ClientStatement_Piksel.getInvoicesPiksel(ddClients.SelectedItem.Text)

            'for each invoice in ostatements run boss updater 
            Dim strResult As String = ""
            For Each item As ClientStatement_Piksel In oStatements
                'update each invoice by running boss table updater 
                strResult = clsBoss.runMainSelect(False, item.TransactionRef, "", "", "")
                If strResult <> "Successful run" Then
                    log.Error("Error: " & strResult & " - Invoice sync failed for Invoice " & item.TransactionRef & " - for client " & ddClients.SelectedItem.Text)
                End If
                strResult = ""
            Next

            'after updating all invoices re-generate the statement 
            oStatements = ClientStatement_Piksel.getInvoicesPiksel(ddClients.SelectedItem.Text)

            'get the email to send to 
            Dim strEmail As String = clsClientStatementDetails.getEmail(ddClients.SelectedItem.Text)
            Dim blnAllEmailsOK As Boolean = True

            'Check email address is valid 
            If Not Regex.IsMatch(strEmail, emailRegex) Then
                blnAllEmailsOK = False
            End If

            If blnAllEmailsOK = True Then
                If oStatements.Count > 0 Then
                    Dim strStatsPath As String = getConfig("ClientStatspath")
                    If strStatsPath.Contains("C:\") Then
                        strStatsPath = strStatsPath.Replace("C:\", "\\" & Environment.MachineName & "\")
                    End If

                    Dim strFile As String
                    If IsCSV Then
                        strFile = createStatementPikselCSV(oStatements)
                    Else
                        strFile = createStatementPiksel(oStatements)
                    End If

                    If strFile = "" Then
                        btnSendEmail.Visible = False
                        hlStatement.Text = ""
                        hlStatement.NavigateUrl = ""
                        hlStatement.Visible = False
                        lblEmail.Text = "No rows to send."
                    Else
                        hlStatement.NavigateUrl = strStatsPath & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFile
                        hlStatement.Text = "View statement for: " & ddClients.SelectedItem.Text
                        hlStatement.Visible = True
                        lblEmail.Text = "Selected Email: " & strEmail
                    End If

                    lblEmail.Visible = True

                Else
                    btnSendEmail.Visible = False
                    txtResult2.Text = "No Outstanding Invoices for: " & ddClients.SelectedItem.Text
                End If

            Else
                btnSendEmail.Visible = False
                txtResult2.Text = "Email not valid for: " & ddClients.SelectedItem.Text & ". Speak to development team to check Config file!"
            End If

        End Using
    End Sub

    Private Function createStatementCSV(ByVal pStatements As List(Of ClientStatement_autogenerated)) As String


      

        makeFolderExist(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy"))

        Dim strDateForFileName As String = Date.Now.ToString(getConfig("ClientStatementFileDateFormat"))
        Dim strFileName = ddClients.SelectedItem.Text & "-" & strDateForFileName & ".csv"
        Dim fullpath = getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName

        If IO.File.Exists(fullpath) Then
            IO.File.Delete(fullpath)
        End If


        Dim header As String = ""
        Dim i As Integer = 0
        For Each oStatement As ClientStatement_autogenerated In pStatements
            If i = 0 Then
                header = "Lead Name"
                header += "," + ToSafeString(oStatement.Reference1Title)
                header += "," + ToSafeString(oStatement.Reference2Title)
                header += "," + ToSafeString(oStatement.Reference3Title)
                header += "," + ToSafeString(oStatement.Reference4Title)
                header += "," + "Payment Terms"
                header += "," + "Invoice Amount"

                header += "," + ">" & CStr(oStatement.PaymentTerms) & " days"
                header += "," + CStr((oStatement.PaymentTerms + 1)) & "- 60 days"
                header += "," + "61-90 days"
                header += "," + "91-120 days"
                header += "," + "121-150 days"
                header += "," + "<151 days"

                header += "," + "Total Overdue"
                header += "," + "Invoice Date"
                header += "," + "Invoice Number"
                header += "," + "Consolidated Ref (if applicable)"
            End If
            i += 1
        Next
        Dim csvwrite As New StreamWriter(fullpath, True)

        csvwrite.WriteLine(header)


        i = 0
        For Each oStatement As ClientStatement_autogenerated In pStatements
            Dim line As String = ""
            Dim intDaysOverDue = DeterminNumberofDays(oStatement.InvoiceDate)

            line = oStatement.LeadName
            line += "," + oStatement.Reference1.ToString()
            line += "," + oStatement.Reference2.ToString()
            line += "," + oStatement.Reference3.ToString()
            line += "," + oStatement.Reference4.ToString()
            line += "," + oStatement.PaymentTerms.ToString()
            line += "," + oStatement.InvoiceAmont.ToString()
            line += "," + oStatement.WithinPaymentTerms.ToString()
            line += "," + oStatement.FirstOutstanding.ToString()
            line += "," + oStatement.SecondOutstanding.ToString()
            line += "," + oStatement.ThirdOutstanding.ToString()
            line += "," + oStatement.FourthOutstanding.ToString()
            line += "," + oStatement.FifthOutstanding.ToString()
            line += "," + oStatement.AmountOverdue.ToString()
            line += "," + oStatement.InvoiceDate.ToString()
            line += "," + oStatement.InvoiceNumber.ToString()
            line += "," + oStatement.ConsolidatedInvoiceNumber.ToString()

            'if amout outstanding is not paid withing payment terms, make rows red
            If intDaysOverDue > oStatement.PaymentTerms Then
                'appExcel.ActiveSheet.Range(("A" & x), ("Q" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
            End If

            'Check if HAMPASD and remove rows where invoicedate is not passed payment terms
            If ddClients.SelectedItem.Text.ToUpper = "HAMPASD" AndAlso (intDaysOverDue < oStatement.PaymentTerms) Then
                ' appExcel.ActiveSheet.range(("A" & x), ("Q" & x)).EntireRow.delete()
            Else
                csvwrite.WriteLine(line)
            End If

            i += 1
        Next
        csvwrite.Close()


        Return strFileName
    End Function



    'R2.23 SA - removed ByVal blnShowSecondRef As Boolean, blnShowBookerName As Boolean
    'R2.21.1 SA - added blnShowBookerName
    Private Function createStatement(ByVal pStatements As List(Of ClientStatement_autogenerated)) As String

        Using New clslogger(log, className, "createStatement")
            Try
                log.Info("Create Excel")
                Dim appExcel As New Application

                'R2.21.3 CR - count the rows as they get added, so we can make sure we aren't sending the client an empty sheet
                ' don't care about the header 
                Dim intActiveRows As Integer = 0

                'stop it asking if you want to overwrite - no-one will know its asking this
                appExcel.DisplayAlerts = False

                'appExcel.Workbooks.Add()
                Dim strFileName As String = ""
                Try
                    'Dim strOverdue As String
                    'Dim strCIno As String
                    log.Info("Start workbook")
                    Dim intDaysOverDue As Integer
                    appExcel.Workbooks.Add()

                    'loop through the statement and set headers 
                    For Each oStatement As ClientStatement_autogenerated In pStatements
                        appExcel.ActiveSheet.Range("A1").value = "Lead Name"
                        appExcel.ActiveSheet.Range("B1").value = oStatement.Reference1Title
                        appExcel.ActiveSheet.Range("C1").value = oStatement.Reference2Title
                        appExcel.ActiveSheet.Range("D1").value = oStatement.Reference3Title
                        appExcel.ActiveSheet.Range("E1").value = oStatement.Reference4Title
                        appExcel.ActiveSheet.Range("F1").value = "Payment Terms"
                        appExcel.ActiveSheet.Range("G1").value = "Invoice Amount"

                        appExcel.ActiveSheet.Range("H1").value = ">" & CStr(oStatement.PaymentTerms) & " days"
                        appExcel.ActiveSheet.Range("I1").value = CStr((oStatement.PaymentTerms + 1)) & "- 60 days"
                        appExcel.ActiveSheet.Range("J1").value = "61-90 days"
                        appExcel.ActiveSheet.Range("K1").value = "91-120 days"
                        appExcel.ActiveSheet.Range("L1").value = "121-150 days"
                        appExcel.ActiveSheet.Range("M1").value = "<151 days"

                        appExcel.ActiveSheet.Range("N1").value = "Total Overdue"
                        appExcel.ActiveSheet.Range("O1").value = "Invoice Date"
                        appExcel.ActiveSheet.Range("P1").value = "Invoice Number"
                        appExcel.ActiveSheet.Range("Q1").value = "Consolidated Ref (if applicable)"
                    Next

                    'loop again and set the rows
                    Dim x As Long
                    x = 2
                    For Each oStatement As ClientStatement_autogenerated In pStatements
                        intDaysOverDue = DeterminNumberofDays(oStatement.InvoiceDate)

                        appExcel.ActiveSheet.Range("A" & x).value = oStatement.LeadName
                        appExcel.ActiveSheet.Range("B" & x).value = oStatement.Reference1
                        appExcel.ActiveSheet.Range("C" & x).value = oStatement.Reference2
                        appExcel.ActiveSheet.Range("D" & x).value = oStatement.Reference3
                        appExcel.ActiveSheet.Range("E" & x).value = oStatement.Reference4
                        appExcel.ActiveSheet.Range("F" & x).value = oStatement.PaymentTerms
                        appExcel.ActiveSheet.Range("G" & x).value = oStatement.InvoiceAmont
                        appExcel.ActiveSheet.Range("H" & x).value = oStatement.WithinPaymentTerms
                        appExcel.ActiveSheet.Range("I" & x).value = oStatement.FirstOutstanding
                        appExcel.ActiveSheet.Range("J" & x).value = oStatement.SecondOutstanding
                        appExcel.ActiveSheet.Range("K" & x).value = oStatement.ThirdOutstanding
                        appExcel.ActiveSheet.Range("L" & x).value = oStatement.FourthOutstanding
                        appExcel.ActiveSheet.Range("M" & x).value = oStatement.FifthOutstanding
                        appExcel.ActiveSheet.Range("N" & x).value = oStatement.AmountOverdue
                        appExcel.ActiveSheet.Range("O" & x).value = oStatement.InvoiceDate
                        appExcel.ActiveSheet.Range("P" & x).value = oStatement.InvoiceNumber
                        appExcel.ActiveSheet.Range("Q" & x).value = oStatement.ConsolidatedInvoiceNumber

                        'if amout outstanding is not paid withing payment terms, make rows red
                        If intDaysOverDue > oStatement.PaymentTerms Then
                            appExcel.ActiveSheet.Range(("A" & x), ("Q" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                        End If

                        'Check if HAMPASD and remove rows where invoicedate is not passed payment terms
                        If ddClients.SelectedItem.Text.ToUpper = "HAMPASD" AndAlso (intDaysOverDue < oStatement.PaymentTerms) Then
                            appExcel.ActiveSheet.range(("A" & x), ("Q" & x)).EntireRow.delete()
                        End If

                        x = x + 1
                        'R2.21.3 CR
                        intActiveRows += 1
                    Next

                    'loop once again to check for empty column and delete them 
                    For Each oStatement As ClientStatement_autogenerated In pStatements
                        If appExcel.ActiveSheet.Range("B1").value = "" Then
                            appExcel.ActiveSheet.Columns("B").Delete()
                        End If
                        If appExcel.ActiveSheet.Range("C1").value = "" Then
                            appExcel.ActiveSheet.Columns("C").Delete()
                        End If
                        If appExcel.ActiveSheet.Range("D1").value = "" Then
                            appExcel.ActiveSheet.Columns("D").Delete()
                        End If
                        If appExcel.ActiveSheet.Range("E1").value = "" Then
                            appExcel.ActiveSheet.Columns("E").Delete()
                        End If
                    Next

                    'R2.21 SA - remove invoice column for EOE
                    If ddClients.SelectedItem.Text.ToLower = "eoecpft" Then
                        appExcel.ActiveSheet.Columns("P").Delete()
                    End If

                    appExcel.ActiveSheet.cells.entirecolumn.autofit()

                    makeFolderExist(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy"))

                    Dim strDateForFileName As String = Date.Now.ToString(getConfig("ClientStatementFileDateFormat"))
                    strFileName = ddClients.SelectedItem.Text & "-" & strDateForFileName & ".xlsx"

                    'check the file already exists
                    'if it does then delete the original (shouldn't happen often)
                    'only need to do this because excel interop doesn't like overwriting files apparently
                    If IO.File.Exists(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName) Then
                        IO.File.Delete(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                    End If

                    'R2.21.3 CR - check there are some lines to send
                    '       let it do the delete above anyway - so that we can correctly check that the file exists before sending later
                    If intActiveRows > 0 Then
                        appExcel.ActiveWorkbook.SaveAs(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                    Else
                        strFileName = ""
                    End If

                    'Threading.Thread.Sleep(2000)

                    log.Info("End workbook")
                Catch ex As Exception
                    handleexception(ex, "IAClientStatements", Me.Page)
                Finally
                    appExcel.Quit()
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel)
                    appExcel = Nothing
                End Try
                log.Info("Start return")
                Return strFileName
                log.Info("End Return")
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
            Return ""
        End Using
    End Function

    Private Function createStatementPikselCSV(ByVal pStatements As List(Of ClientStatement_Piksel)) As String


        makeFolderExist(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy"))

        Dim strDateForFileName As String = Date.Now.ToString(getConfig("ClientStatementFileDateFormat"))
        Dim strFileName = ddClients.SelectedItem.Text & "-" & strDateForFileName & ".csv"
        Dim fullpath = getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName

        If IO.File.Exists(fullpath) Then
            IO.File.Delete(fullpath)
        End If


        Dim header As String = ""
        Dim i As Integer = 0
        For Each oStatement As ClientStatement_Piksel In pStatements
            If i = 0 Then

                header = "Employee"
                header += "," + "Business Unit"
                header += "," + "Department"
                header += "," + "Project Code"
                header += "," + "Product"
                header += "," + "Departure Date"
                header += "," + "Arrival Date"
                header += "," + "Booker Name"
                header += "," + "Payment Terms"
                header += "," + "Invoice Amount"
                header += "," + ">" & CStr(oStatement.PaymentTerms) & " days"
                header += "," + CStr((oStatement.PaymentTerms + 1)) & "- 90 days"
                header += "," + "91-120 days"
                header += "," + "121-150 days"
                header += "," + "<151 days"
                header += "," + "Amount Overdue"
                header += "," + "Transaction Ref"
                header += "," + "Transaction Date"


            End If
            i += 1
        Next
        Dim csvwrite As New StreamWriter(fullpath, True)

        csvwrite.WriteLine(header)
     

        i = 0
        For Each oStatement As ClientStatement_Piksel In pStatements
            Dim line As String = ""
            Dim intDaysOverDue = DeterminNumberofDays(oStatement.TransactionDate)
            Dim strEmployee As String = oStatement.EmployeeNO & " " & getPikselEmployeeCSV(oStatement.EmployeeName)

            line = strEmployee
            line += "," + ToSafeString(oStatement.BusinessUnit)
            line += "," + ToSafeString(oStatement.Department)
            line += "," + ToSafeString(oStatement.ProjectCode)
            line += "," + ToSafeString(oStatement.Product)
            line += "," + ToSafeString(oStatement.DepartureDate)
            line += "," + ToSafeString(oStatement.ArrivalDate)
            line += "," + ToSafeString(oStatement.BookerName)

            line += "," + CStr(oStatement.PaymentTerms)
            line += "," + CStr(oStatement.InvoiceAmont)
            line += "," + CStr(oStatement.FirstOutstanding)
            line += "," + CStr(oStatement.SecondOutstanding)
            line += "," + CStr(oStatement.ThirdOutstanding)
            line += "," + CStr(oStatement.FourthOutstanding)
            line += "," + CStr(oStatement.FifthOutstanding)
            line += "," + CStr(oStatement.AmountOverdue)
            line += "," + ToSafeString(oStatement.TransactionRef)
            line += "," + ToSafeString(oStatement.TransactionDate)


            'if amout outstanding is not paid withing payment terms, make rows red
            If intDaysOverDue > oStatement.PaymentTerms Then
                'appExcel.ActiveSheet.Range(("A" & x), ("Q" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
            End If

            csvwrite.WriteLine(line)

            i += 1
        Next
        csvwrite.Close()
        Return strFileName

    End Function

    Public Shared Function ToSafeString(obj As Object) As String
        Return (If(obj, String.Empty)).ToString()
    End Function

    Private Function createStatementPiksel(ByVal pStatements As List(Of ClientStatement_Piksel)) As String
        Using New clslogger(log, className, "createStatementPiksel")
            Try
                log.Info("Create Excel Statement - Piksel")
                Dim strFileName As String = ""

                Dim appExcel As New Application
                Dim intActiveRows As Integer = 0 'don't care about the header 
                'stop it asking if you want to overwrite - no-one will know its asking this
                appExcel.DisplayAlerts = False

                Try
                    log.Info("Start workbook")
                    Dim intDaysOverDue As Integer
                    Dim strEmployee As String
                    appExcel.Workbooks.Add()

                    'loop through the statement and set headers 
                    Dim i As Integer = 0
                    For Each oStatement As ClientStatement_Piksel In pStatements

                        If i = 0 Then
                            appExcel.ActiveSheet.Range("A1").value = "Employee"
                            appExcel.ActiveSheet.Range("B1").value = "Business Unit"
                            appExcel.ActiveSheet.Range("C1").value = "Department"
                            appExcel.ActiveSheet.Range("D1").value = "Project Code"
                            appExcel.ActiveSheet.Range("E1").value = "Product"
                            appExcel.ActiveSheet.Range("F1").value = "Departure Date"
                            appExcel.ActiveSheet.Range("G1").value = "Arrival Date"
                            appExcel.ActiveSheet.Range("H1").value = "Booker Name"
                            appExcel.ActiveSheet.Range("I1").value = "Payment Terms"
                            appExcel.ActiveSheet.Range("J1").value = "Invoice Amount"
                            appExcel.ActiveSheet.Range("K1").value = ">" & CStr(oStatement.PaymentTerms) & " days"
                            appExcel.ActiveSheet.Range("L1").value = CStr((oStatement.PaymentTerms + 1)) & "- 90 days"
                            appExcel.ActiveSheet.Range("M1").value = "91-120 days"
                            appExcel.ActiveSheet.Range("N1").value = "121-150 days"
                            appExcel.ActiveSheet.Range("O1").value = "<151 days"
                            appExcel.ActiveSheet.Range("P1").value = "Amount Overdue"
                            appExcel.ActiveSheet.Range("Q1").value = "Transaction Ref"
                            appExcel.ActiveSheet.Range("R1").value = "Transaction Date"
                        End If

                        i += 1
                    Next
                    'loop again and set the rows
                    Dim x As Long
                    x = 2
                    For Each oStatement As ClientStatement_Piksel In pStatements
                        intDaysOverDue = DeterminNumberofDays(oStatement.TransactionDate)
                        strEmployee = oStatement.EmployeeNO & " " & getPikselEmployee(oStatement.EmployeeName)

                        appExcel.ActiveSheet.Range("A" & x).value = strEmployee
                        appExcel.ActiveSheet.Range("B" & x).value = oStatement.BusinessUnit
                        appExcel.ActiveSheet.Range("C" & x).value = oStatement.Department
                        appExcel.ActiveSheet.Range("D" & x).value = oStatement.ProjectCode
                        appExcel.ActiveSheet.Range("E" & x).value = oStatement.Product
                        appExcel.ActiveSheet.Range("F" & x).value = oStatement.DepartureDate
                        appExcel.ActiveSheet.Range("G" & x).value = oStatement.ArrivalDate
                        appExcel.ActiveSheet.Range("H" & x).value = oStatement.BookerName
                        appExcel.ActiveSheet.Range("I" & x).value = oStatement.PaymentTerms
                        appExcel.ActiveSheet.Range("J" & x).value = oStatement.InvoiceAmont
                        appExcel.ActiveSheet.Range("K" & x).value = oStatement.FirstOutstanding
                        appExcel.ActiveSheet.Range("L" & x).value = oStatement.SecondOutstanding
                        appExcel.ActiveSheet.Range("M" & x).value = oStatement.ThirdOutstanding
                        appExcel.ActiveSheet.Range("N" & x).value = oStatement.FourthOutstanding
                        appExcel.ActiveSheet.Range("O" & x).value = oStatement.FifthOutstanding
                        appExcel.ActiveSheet.Range("P" & x).value = oStatement.AmountOverdue
                        appExcel.ActiveSheet.Range("Q" & x).value = oStatement.TransactionRef
                        appExcel.ActiveSheet.Range("R" & x).value = oStatement.TransactionDate

                        'if amout outstanding is not paid withing payment terms, make rows red
                        If intDaysOverDue > oStatement.PaymentTerms Then
                            appExcel.ActiveSheet.Range(("A" & x), ("Q" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                        End If

                        x = x + 1
                        intActiveRows += 1
                    Next

                    appExcel.ActiveSheet.cells.entirecolumn.autofit()

                    makeFolderExist(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy"))

                    Dim strDateForFileName As String = Date.Now.ToString(getConfig("ClientStatementFileDateFormat"))
                    strFileName = ddClients.SelectedItem.Text & "-" & strDateForFileName & ".xlsx"

                    If IO.File.Exists(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName) Then
                        IO.File.Delete(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                    End If

                    If intActiveRows > 0 Then
                        appExcel.ActiveWorkbook.SaveAs(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                    Else
                        strFileName = ""
                    End If

                    Threading.Thread.Sleep(2000)
                    log.Info("End workbook")
                Catch ex As Exception
                    handleexception(ex, "IAClientStatements", Me.Page)
                Finally
                    appExcel.Quit()
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel)
                    appExcel = Nothing
                End Try

                log.Info("Start return")
                Return strFileName
                log.Info("End Return")
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
            Return ""
        End Using
    End Function

    Private Function getPikselEmployee(ByVal pEmployeeName As String) As String
        Using New clslogger(log, className, "getPikselEmployee")

            Dim strEmployee As String = pEmployeeName.Replace("/", ",")

            'Dim strFirstName As String = pEmployeeName.Substring(pEmployeeName.IndexOf("/") + 1)
            'Dim strLastName As String = pEmployeeName.Substring(0, pEmployeeName.IndexOf("/"))

            If strEmployee.ToUpper.EndsWith("MR") Or strEmployee.ToUpper.EndsWith("MS") Or strEmployee.ToUpper.EndsWith("DR") Then
                strEmployee = Trim(strEmployee.Substring(0, strEmployee.Length - 2))
            ElseIf strEmployee.ToUpper.StartsWith("MRS") Then
                strEmployee = Trim(strEmployee.Substring(0, strEmployee.Length - 3))
            ElseIf strEmployee.ToUpper.StartsWith("MISS") Then
                strEmployee = Trim(strEmployee.Substring(0, strEmployee.Length - 4))
            End If

            Return strEmployee
        End Using
    End Function

    Private Function getPikselEmployeeCSV(ByVal pEmployeeName As String) As String
        Using New clslogger(log, className, "getPikselEmployee")

            Dim strEmployee As String = pEmployeeName.Replace("/", " ")

            'Dim strFirstName As String = pEmployeeName.Substring(pEmployeeName.IndexOf("/") + 1)
            'Dim strLastName As String = pEmployeeName.Substring(0, pEmployeeName.IndexOf("/"))

            If strEmployee.ToUpper.EndsWith("MR") Or strEmployee.ToUpper.EndsWith("MS") Or strEmployee.ToUpper.EndsWith("DR") Then
                strEmployee = Trim(strEmployee.Substring(0, strEmployee.Length - 2))
            ElseIf strEmployee.ToUpper.StartsWith("MRS") Then
                strEmployee = Trim(strEmployee.Substring(0, strEmployee.Length - 3))
            ElseIf strEmployee.ToUpper.StartsWith("MISS") Then
                strEmployee = Trim(strEmployee.Substring(0, strEmployee.Length - 4))
            End If

            Return strEmployee
        End Using
    End Function

    Private Function DeterminNumberofDays(ByVal pInvoiceDate As Date) As Integer
        Using New clslogger(log, className, "DeterminNumberofDays")
            Dim tsTimeSpan As TimeSpan
            Dim intNumberOfDays As Integer
            tsTimeSpan = Now.Subtract(pInvoiceDate)
            intNumberOfDays = tsTimeSpan.Days
            Return intNumberOfDays
        End Using
    End Function

    Private Sub IAClientStatements_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAClientStatements_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAClientStatements"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnSendEmail_Click(sender As Object, e As EventArgs) Handles btnSendEmail.Click
        Try
            Dim intCount As Integer = 1
            Dim strBoss As String = ddClients.SelectedItem.Text
            Dim strEmail As String = clsClientStatementDetails.getEmail(strBoss)
            Dim strFileName As String = ""
            Dim details As Dictionary(Of String, String) = New Dictionary(Of String, String)
            Dim attachList As List(Of String) = New List(Of String)
            Dim oCIlist As New clsClientStatement_ConsolidatedInvoices
            Dim strInvoiceNo As String
            'Dim strFilelocation As String
            Dim oList As New List(Of ClientStatement_autogenerated)
            oList = ClientStatement_autogenerated.getInvoices(strBoss)

            For Each item In oList
                strInvoiceNo = item.InvoiceNumber
                oCIlist = clsClientStatement_ConsolidatedInvoices.getCIdetails(strInvoiceNo)
                If oCIlist.ConsolidatedRef <> "" AndAlso Not details.ContainsKey(oCIlist.ConsolidatedRef) Then
                    details.Add(oCIlist.ConsolidatedRef, " was sent to " & oCIlist.email & " on the " & oCIlist.dateSent)
                End If
                'strFilelocation = clsNYS.notString(Admin_autogenerated.getFileLocation(strBoss))
                'If strFilelocation <> "" AndAlso Directory.Exists(strFilelocation) Then
                '    If File.Exists(strFilelocation & "\" & oCIlist.fileName) Then
                '        attachList.Add(strFilelocation & "\" & oCIlist.fileName)
                '    End If
                'End If
            Next

            Dim strLocalEmail As String = strEmail
            Dim ofile As New System.IO.StreamReader(getConfig("ClientStatspath") & "\Statement.htm")
            Dim strMessage As String = ofile.ReadToEnd
            ofile.Close()

            If getConfig("EmailTest") = "True" Then
                strLocalEmail = "devtest@nysgroup.com"
                strMessage = strMessage & "<br/><strong>TEST MODE ACTIVE: Email would have been sent to " & strEmail & "</strong>"
            End If

            If details.Count <> 0 Then
                Dim repMessage As String = "The following Consolidated Invoices were sent " & "<br>"
                For Each item In details
                    repMessage = repMessage & "Invoice " & item.ToString() & "<br>"
                Next
                strMessage = strMessage.Replace("#MESSAGE#", repMessage)
            Else
                strMessage = strMessage.Replace("#MESSAGE#", "")
            End If

            Dim strDateForFileName As String = Date.Now.ToString(getConfig("ClientStatementFileDateFormat"))
            strFileName = ddClients.SelectedItem.Text & "-" & strDateForFileName & ".xlsx"

            If SendEmailMessage(getConfig("ClientStatementEmailFrom"), strLocalEmail, "Client Statement", strMessage, _
                                getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName, "", "", getConfig("ClientStatementEmailBcc"), "", "", "") Then
                'R2.22 SA 
                lblEmail.Visible = False
                hlStatement.Visible = False
                ddClients.SelectedValue = ""

                Throw New EvoFriendlyException("Email Sent Successfully to " & strLocalEmail, "Information")
            End If
        Catch ex As Exception
            handleexception(ex, "IAClientStatements", Me.Page)
        End Try
    End Sub

    Protected Sub ddClients_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddClients.SelectedIndexChanged
        Try
            lblEmail.Text = ""
            lblEmail.Visible = False
            hlStatement.NavigateUrl = ""
            hlStatement.Text = ""
            hlStatement.Visible = False

            btnSendEmail.Visible = False
        Catch ex As Exception
            handleexception(ex, "IAClientStatements", Me.Page)
        End Try
    End Sub

    Private Function ASinServiceClientStatementCreateFile(ByVal pStatements As List(Of ClientStatement_autogenerated), _
                                              pstrBossRef As String) As String

        Dim appExcel As New Microsoft.Office.Interop.Excel.Application

        'Count the rows as they get added, so we can make sure we aren't sending the client an empty sheet
        Dim intActiveRows As Integer = 0
        Dim strFileName As String = ""
        'stop it asking if you want to overwrite - no-one will know its asking this
        appExcel.DisplayAlerts = False

        Dim blnRef1Exist As Boolean = False
        Dim blnRef2Exist As Boolean = False
        Dim blnRef3Exist As Boolean = False
        Dim blnRef4Exist As Boolean = False

        Try
            Dim intDaysOverDue As Integer

            Dim oStatementClient As New clsClientStatementDetails
            oStatementClient = clsClientStatementDetails.getDetails(pstrBossRef)

            If oStatementClient.ReferenceTitle.Trim <> "" Then
                blnRef1Exist = True
            ElseIf oStatementClient.Reference2Title.Trim <> "" Then
                blnRef2Exist = True
            ElseIf oStatementClient.Reference3Title.Trim <> "" Then
                blnRef3Exist = True
            ElseIf oStatementClient.Reference4Title.Trim <> "" Then
                blnRef4Exist = True
            End If

            appExcel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet)

            If blnRef4Exist Then
                appExcel.ActiveSheet.Range("A1").value = "Lead Name"
                appExcel.ActiveSheet.Range("B1").value = oStatementClient.ReferenceTitle
                appExcel.ActiveSheet.Range("C1").value = oStatementClient.Reference2Title
                appExcel.ActiveSheet.Range("D1").value = oStatementClient.Reference3Title
                appExcel.ActiveSheet.Range("E1").value = oStatementClient.Reference4Title
                appExcel.ActiveSheet.Range("F1").value = "Payment Terms"
                appExcel.ActiveSheet.Range("G1").value = "Invoice Amount"
                appExcel.ActiveSheet.Range("H1").value = "Within payment Terms"
                appExcel.ActiveSheet.Range("I1").value = "30-60 days"
                appExcel.ActiveSheet.Range("J1").value = "61-90 days"
                appExcel.ActiveSheet.Range("K1").value = "91-120 days"
                appExcel.ActiveSheet.Range("L1").value = "121-150 days"
                appExcel.ActiveSheet.Range("M1").value = "<151 days"
                appExcel.ActiveSheet.Range("N1").value = "Total Overdue"
                appExcel.ActiveSheet.Range("O1").value = "Invoice Date"
                appExcel.ActiveSheet.Range("P1").value = "Invoice Number"
                appExcel.ActiveSheet.Range("Q1").value = "Consolidated Ref (if applicable)"

            ElseIf blnRef3Exist Then
                appExcel.ActiveSheet.Range("A1").value = "Lead Name"
                appExcel.ActiveSheet.Range("B1").value = oStatementClient.ReferenceTitle
                appExcel.ActiveSheet.Range("C1").value = oStatementClient.Reference2Title
                appExcel.ActiveSheet.Range("D1").value = oStatementClient.Reference3Title
                appExcel.ActiveSheet.Range("E1").value = "Payment Terms"
                appExcel.ActiveSheet.Range("F1").value = "Invoice Amount"
                appExcel.ActiveSheet.Range("G1").value = "Within payment Terms"
                appExcel.ActiveSheet.Range("H1").value = "30-60 days"
                appExcel.ActiveSheet.Range("I1").value = "61-90 days"
                appExcel.ActiveSheet.Range("J1").value = "91-120 days"
                appExcel.ActiveSheet.Range("K1").value = "121-150 days"
                appExcel.ActiveSheet.Range("L1").value = "<151 days"
                appExcel.ActiveSheet.Range("M1").value = "Total Overdue"
                appExcel.ActiveSheet.Range("N1").value = "Invoice Date"
                appExcel.ActiveSheet.Range("O1").value = "Invoice Number"
                appExcel.ActiveSheet.Range("P1").value = "Consolidated Ref (if applicable)"

            ElseIf blnRef2Exist Then
                appExcel.ActiveSheet.Range("A1").value = "Lead Name"
                appExcel.ActiveSheet.Range("B1").value = oStatementClient.ReferenceTitle
                appExcel.ActiveSheet.Range("C1").value = oStatementClient.Reference2Title
                appExcel.ActiveSheet.Range("D1").value = "Payment Terms"
                appExcel.ActiveSheet.Range("E1").value = "Invoice Amount"
                appExcel.ActiveSheet.Range("F1").value = "Within payment Terms"
                appExcel.ActiveSheet.Range("G1").value = "30-60 days"
                appExcel.ActiveSheet.Range("H1").value = "61-90 days"
                appExcel.ActiveSheet.Range("I1").value = "91-120 days"
                appExcel.ActiveSheet.Range("J1").value = "121-150 days"
                appExcel.ActiveSheet.Range("K1").value = "<151 days"
                appExcel.ActiveSheet.Range("L1").value = "Total Overdue"
                appExcel.ActiveSheet.Range("M1").value = "Invoice Date"
                appExcel.ActiveSheet.Range("N1").value = "Invoice Number"
                appExcel.ActiveSheet.Range("O1").value = "Consolidated Ref (if applicable)"

            ElseIf blnRef1Exist Then
                appExcel.ActiveSheet.Range("A1").value = "Lead Name"
                appExcel.ActiveSheet.Range("B1").value = oStatementClient.ReferenceTitle
                appExcel.ActiveSheet.Range("C1").value = "Payment Terms"
                appExcel.ActiveSheet.Range("D1").value = "Invoice Amount"
                appExcel.ActiveSheet.Range("E1").value = "Within payment Terms"
                appExcel.ActiveSheet.Range("F1").value = "30-60 days"
                appExcel.ActiveSheet.Range("G1").value = "61-90 days"
                appExcel.ActiveSheet.Range("H1").value = "91-120 days"
                appExcel.ActiveSheet.Range("I1").value = "121-150 days"
                appExcel.ActiveSheet.Range("J1").value = "<151 days"
                appExcel.ActiveSheet.Range("K1").value = "Total Overdue"
                appExcel.ActiveSheet.Range("L1").value = "Invoice Date"
                appExcel.ActiveSheet.Range("M1").value = "Invoice Number"
                appExcel.ActiveSheet.Range("N1").value = "Consolidated Ref (if applicable)"
            Else
                appExcel.ActiveSheet.Range("A1").value = "Lead Name"
                appExcel.ActiveSheet.Range("B1").value = "Payment Terms"
                appExcel.ActiveSheet.Range("C1").value = "Invoice Amount"
                appExcel.ActiveSheet.Range("D1").value = "Within payment Terms"
                appExcel.ActiveSheet.Range("E1").value = "30-60 days"
                appExcel.ActiveSheet.Range("F1").value = "61-90 days"
                appExcel.ActiveSheet.Range("G1").value = "91-120 days"
                appExcel.ActiveSheet.Range("H1").value = "121-150 days"
                appExcel.ActiveSheet.Range("I1").value = "<151 days"
                appExcel.ActiveSheet.Range("J1").value = "Total Overdue"
                appExcel.ActiveSheet.Range("K1").value = "Invoice Date"
                appExcel.ActiveSheet.Range("L1").value = "Invoice Number"
                appExcel.ActiveSheet.Range("M1").value = "Consolidated Ref (if applicable)"
            End If

            Dim x As Long
            x = 2

            For Each oStatement As ClientStatement_autogenerated In pStatements
                intDaysOverDue = DeterminNumberofDays(oStatement.InvoiceDate)

                If blnRef4Exist Then
                    appExcel.ActiveSheet.Range("A" & x).value = oStatement.LeadName
                    appExcel.ActiveSheet.Range("B" & x).value = oStatement.Reference1
                    appExcel.ActiveSheet.Range("C" & x).value = oStatement.Reference2
                    appExcel.ActiveSheet.Range("D" & x).value = oStatement.Reference3
                    appExcel.ActiveSheet.Range("E" & x).value = oStatement.Reference4
                    appExcel.ActiveSheet.Range("F" & x).value = oStatement.PaymentTerms
                    appExcel.ActiveSheet.Range("G" & x).value = oStatement.InvoiceAmont
                    appExcel.ActiveSheet.Range("H" & x).value = oStatement.WithinPaymentTerms
                    appExcel.ActiveSheet.Range("I" & x).value = oStatement.FirstOutstanding
                    appExcel.ActiveSheet.Range("J" & x).value = oStatement.SecondOutstanding
                    appExcel.ActiveSheet.Range("K" & x).value = oStatement.ThirdOutstanding
                    appExcel.ActiveSheet.Range("L" & x).value = oStatement.FourthOutstanding
                    appExcel.ActiveSheet.Range("M" & x).value = oStatement.FifthOutstanding
                    appExcel.ActiveSheet.Range("N" & x).value = oStatement.AmountOverdue
                    appExcel.ActiveSheet.Range("O" & x).value = oStatement.InvoiceDate
                    appExcel.ActiveSheet.Range("P" & x).value = oStatement.InvoiceNumber
                    appExcel.ActiveSheet.Range("Q" & x).value = oStatement.ConsolidatedInvoiceNumber

                    'Overdue invoices colour in red
                    If intDaysOverDue > oStatement.PaymentTerms Then
                        appExcel.ActiveSheet.Range(("A" & x), ("Q" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                    End If
                    'Check if HAMPASD and remove rows where invoice date is not passed payment terms
                    If pstrBossRef.ToUpper = "HAMPASD" AndAlso (intDaysOverDue < oStatement.PaymentTerms) Then
                        appExcel.ActiveSheet.range(("A" & x), ("Q" & x)).EntireRow.delete()
                    End If

                ElseIf blnRef3Exist Then
                    appExcel.ActiveSheet.Range("A" & x).value = oStatement.LeadName
                    appExcel.ActiveSheet.Range("B" & x).value = oStatement.Reference1
                    appExcel.ActiveSheet.Range("C" & x).value = oStatement.Reference2
                    appExcel.ActiveSheet.Range("D" & x).value = oStatement.Reference3
                    appExcel.ActiveSheet.Range("E" & x).value = oStatement.PaymentTerms
                    appExcel.ActiveSheet.Range("F" & x).value = oStatement.InvoiceAmont
                    appExcel.ActiveSheet.Range("G" & x).value = oStatement.WithinPaymentTerms
                    appExcel.ActiveSheet.Range("H" & x).value = oStatement.FirstOutstanding
                    appExcel.ActiveSheet.Range("I" & x).value = oStatement.SecondOutstanding
                    appExcel.ActiveSheet.Range("J" & x).value = oStatement.ThirdOutstanding
                    appExcel.ActiveSheet.Range("K" & x).value = oStatement.FourthOutstanding
                    appExcel.ActiveSheet.Range("L" & x).value = oStatement.FifthOutstanding
                    appExcel.ActiveSheet.Range("M" & x).value = oStatement.AmountOverdue
                    appExcel.ActiveSheet.Range("N" & x).value = oStatement.InvoiceDate
                    appExcel.ActiveSheet.Range("O" & x).value = oStatement.InvoiceNumber
                    appExcel.ActiveSheet.Range("P" & x).value = oStatement.ConsolidatedInvoiceNumber
                    'Overdue invoices colour in red
                    If intDaysOverDue > oStatement.PaymentTerms Then
                        appExcel.ActiveSheet.Range(("A" & x), ("P" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                    End If
                    'Check if HAMPASD and remove rows where invoice date is not passed payment terms
                    If pstrBossRef.ToUpper = "HAMPASD" AndAlso (intDaysOverDue < oStatement.PaymentTerms) Then
                        appExcel.ActiveSheet.range(("A" & x), ("P" & x)).EntireRow.delete()
                    End If

                ElseIf blnRef2Exist Then
                    appExcel.ActiveSheet.Range("A" & x).value = oStatement.LeadName
                    appExcel.ActiveSheet.Range("B" & x).value = oStatement.Reference1
                    appExcel.ActiveSheet.Range("C" & x).value = oStatement.Reference2
                    appExcel.ActiveSheet.Range("D" & x).value = oStatement.PaymentTerms
                    appExcel.ActiveSheet.Range("E" & x).value = oStatement.InvoiceAmont
                    appExcel.ActiveSheet.Range("F" & x).value = oStatement.WithinPaymentTerms
                    appExcel.ActiveSheet.Range("G" & x).value = oStatement.FirstOutstanding
                    appExcel.ActiveSheet.Range("H" & x).value = oStatement.SecondOutstanding
                    appExcel.ActiveSheet.Range("I" & x).value = oStatement.ThirdOutstanding
                    appExcel.ActiveSheet.Range("J" & x).value = oStatement.FourthOutstanding
                    appExcel.ActiveSheet.Range("K" & x).value = oStatement.FifthOutstanding
                    appExcel.ActiveSheet.Range("L" & x).value = oStatement.AmountOverdue
                    appExcel.ActiveSheet.Range("M" & x).value = oStatement.InvoiceDate
                    appExcel.ActiveSheet.Range("N" & x).value = oStatement.InvoiceNumber
                    appExcel.ActiveSheet.Range("O" & x).value = oStatement.ConsolidatedInvoiceNumber
                    'Overdue invoices colour in red
                    If intDaysOverDue > oStatement.PaymentTerms Then
                        appExcel.ActiveSheet.Range(("A" & x), ("O" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                    End If
                    'Check if HAMPASD and remove rows where invoice date is not passed payment terms
                    If pstrBossRef.ToUpper = "HAMPASD" AndAlso (intDaysOverDue < oStatement.PaymentTerms) Then
                        appExcel.ActiveSheet.range(("A" & x), ("O" & x)).EntireRow.delete()
                    End If

                ElseIf blnRef1Exist Then
                    appExcel.ActiveSheet.Range("A" & x).value = oStatement.LeadName
                    appExcel.ActiveSheet.Range("B" & x).value = oStatement.Reference1
                    appExcel.ActiveSheet.Range("C" & x).value = oStatement.PaymentTerms
                    appExcel.ActiveSheet.Range("D" & x).value = oStatement.InvoiceAmont
                    appExcel.ActiveSheet.Range("E" & x).value = oStatement.WithinPaymentTerms
                    appExcel.ActiveSheet.Range("F" & x).value = oStatement.FirstOutstanding
                    appExcel.ActiveSheet.Range("G" & x).value = oStatement.SecondOutstanding
                    appExcel.ActiveSheet.Range("H" & x).value = oStatement.ThirdOutstanding
                    appExcel.ActiveSheet.Range("I" & x).value = oStatement.FourthOutstanding
                    appExcel.ActiveSheet.Range("J" & x).value = oStatement.FifthOutstanding
                    appExcel.ActiveSheet.Range("K" & x).value = oStatement.AmountOverdue
                    appExcel.ActiveSheet.Range("L" & x).value = oStatement.InvoiceDate
                    appExcel.ActiveSheet.Range("M" & x).value = oStatement.InvoiceNumber
                    appExcel.ActiveSheet.Range("N" & x).value = oStatement.ConsolidatedInvoiceNumber
                    'Overdue invoices colour in red
                    If intDaysOverDue > oStatement.PaymentTerms Then
                        appExcel.ActiveSheet.Range(("A" & x), ("N" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                    End If
                    'Check if HAMPASD and remove rows where invoice date is not passed payment terms
                    If pstrBossRef.ToUpper = "HAMPASD" AndAlso (intDaysOverDue < oStatement.PaymentTerms) Then
                        appExcel.ActiveSheet.range(("A" & x), ("N" & x)).EntireRow.delete()
                    End If

                Else
                    appExcel.ActiveSheet.Range("A" & x).value = oStatement.LeadName
                    appExcel.ActiveSheet.Range("B" & x).value = oStatement.PaymentTerms
                    appExcel.ActiveSheet.Range("C" & x).value = oStatement.InvoiceAmont
                    appExcel.ActiveSheet.Range("D" & x).value = oStatement.WithinPaymentTerms
                    appExcel.ActiveSheet.Range("E" & x).value = oStatement.FirstOutstanding
                    appExcel.ActiveSheet.Range("F" & x).value = oStatement.SecondOutstanding
                    appExcel.ActiveSheet.Range("G" & x).value = oStatement.ThirdOutstanding
                    appExcel.ActiveSheet.Range("H" & x).value = oStatement.FourthOutstanding
                    appExcel.ActiveSheet.Range("I" & x).value = oStatement.FifthOutstanding
                    appExcel.ActiveSheet.Range("J" & x).value = oStatement.AmountOverdue
                    appExcel.ActiveSheet.Range("K" & x).value = oStatement.InvoiceDate
                    appExcel.ActiveSheet.Range("L" & x).value = oStatement.InvoiceNumber
                    appExcel.ActiveSheet.Range("M" & x).value = oStatement.ConsolidatedInvoiceNumber
                    'Overdue invoices colour in red
                    If intDaysOverDue > oStatement.PaymentTerms Then
                        appExcel.ActiveSheet.Range(("A" & x), ("M" & x)).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                    End If
                    'Check if HAMPASD and remove rows where invoice date is not passed payment terms
                    If pstrBossRef.ToUpper = "HAMPASD" AndAlso (intDaysOverDue < oStatement.PaymentTerms) Then
                        appExcel.ActiveSheet.range(("A" & x), ("M" & x)).EntireRow.delete()
                    End If
                End If

                x = x + 1
                intActiveRows += 1
            Next

            'remove invoice number column for EOE
            If blnRef4Exist Then
                If pstrBossRef.ToLower = "eoecpft" Then
                    appExcel.ActiveSheet.Columns("P").Delete()
                End If
            ElseIf blnRef3Exist Then
                If pstrBossRef.ToLower = "eoecpft" Then
                    appExcel.ActiveSheet.Columns("O").Delete()
                End If
            ElseIf blnRef2Exist Then
                If pstrBossRef.ToLower = "eoecpft" Then
                    appExcel.ActiveSheet.Columns("N").Delete()
                End If
            ElseIf blnRef1Exist Then
                If pstrBossRef.ToLower = "eoecpft" Then
                    appExcel.ActiveSheet.Columns("M").Delete()
                End If
            Else
                If pstrBossRef.ToLower = "eoecpft" Then
                    appExcel.ActiveSheet.Columns("L").Delete()
                End If
            End If

            appExcel.ActiveSheet.Cells.EntireColumn.AutoFit()
            makeFolderExist(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy"))


            Dim strDateForFileName As String = Date.Now.ToString(getConfig("ClientStatementFileDateFormat"))
            strFileName = pstrBossRef & "-" & strDateForFileName & ".xlsx"

            'check the file already exists
            'if it does then delete the original (shouldn't happen often)
            'only need to do this because excel interop doesn't like overwriting files apparently
            If IO.File.Exists(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName) Then
                IO.File.Delete(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
            End If

            log.Info("There are " & intActiveRows & " for " & pstrBossRef & " client statement")
            'let it do the delete above anyway - so that we can correctly check that the file exists before sending later
            If intActiveRows > 0 Then
                appExcel.ActiveSheet.SaveAs(getConfig("ClientStatspath") & "\" & Format(Now, "dd-MM-yyyy") & "\" & strFileName)
                log.Info(pstrBossRef & " client statement file is available")
            Else
                'set the file name to blank so that we know the file hasn't been generated
                strFileName = ""
                log.Info("No file has been created for " & pstrBossRef & " client statement")
            End If

            'Threading.Thread.Sleep(2000)
        Catch ex As Exception
            'logError(log, ex, "ClientStatementCreateFile")
        Finally
            'appExcel.Quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel)
            appExcel = Nothing
        End Try
        Return strFileName
    End Function

    
End Class