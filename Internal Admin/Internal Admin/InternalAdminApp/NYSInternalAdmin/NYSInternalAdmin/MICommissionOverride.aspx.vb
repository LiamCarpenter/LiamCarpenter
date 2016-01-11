Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports BossData

Partial Public Class MICommissionOverride
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
                    txtDue.Text = "0"
                    Dim strRet As String = setUser()
                    If strRet.StartsWith("ERROR") Then
                        Response.Redirect("IALogonAdmin.aspx?User=falseX")
                    End If
                    btnrefresh.Focus()
                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                End If
                Me.Title = "Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try

                Panel1.Visible = True
                'rvDue.Visible = False
                txtinvoiceno.Text = txtinvoiceno.Text.Trim
                If txtinvoiceno.Text = "" Then
                    Throw New EvoFriendlyException("Please enter an Invoice number.", "Check Details")
                Else
                    If Regex.IsMatch(txtinvoiceno.Text, getConfig("InvoiceFormat")) Then
                        clearBoxes(False)
                        selectBossData(txtinvoiceno.Text)
                        selectSqlData(txtinvoiceno.Text)
                        selectOverrideData(txtinvoiceno.Text)
                    Else
                        Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                    End If
                End If
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub clearBoxes(ByVal pbJustOverride As Boolean)
        Using New clslogger(log, className, "clearBoxes")

            If Not pbJustOverride Then
                txtBossCommNett.Text = ""
                txtBossCommVat.Text = ""
                txtBossCommTot.Text = ""
                txtBossCommRec.Text = ""
                txtBossCommDue.Text = ""
                txtBossTot.Text = ""
                txtBossPay.Text = ""

                txtSqlCommNet.Text = ""
                txtSqlCommVat.Text = ""
                txtSqlCommTot.Text = ""
                txtSqlCommRec.Text = ""
                txtSqlCommDue.Text = ""
                txtSqlTot.Text = ""
                txtSqlPay.Text = ""
            End If

            txtOverCommNet.Text = ""
            txtOverCommVat.Text = ""
            txtOverCommTot.Text = ""
            txtOverCommRec.Text = ""
            txtOverCommDue.Text = ""
            txtOverTot.Text = ""
            ddOverPay.SelectedIndex = 0

            'R2.16 CR
            txtCurrency.Text = ""

        End Using
    End Sub

    Public Sub selectOverrideData(ByVal pstrInvoiceRef As String)
        Using New clslogger(log, className, "selectOverrideData")
            Dim oSqls As New List(Of BossCommissionOverride)
            oSqls = BossCommissionOverride.getOverride(pstrInvoiceRef)

            Dim commnett As Decimal = 0
            Dim commvat As Decimal = 0
            Dim commrec As Decimal = 0
            Dim commdue As Decimal = 0
            Dim total As Decimal = 0
            Dim strpaynet As String = ""

            If oSqls.Count > 0 Then
                For Each oSql As BossCommissionOverride In oSqls
                    commnett = commnett + oSql.CommissionNett
                    commvat = commvat + oSql.CommissionVat
                    commrec = commrec + oSql.CommissionReceived
                    commdue = commdue + oSql.CommissionDue
                    total = total + oSql.BookingTotal
                    strpaynet = strpaynet & CStr(oSql.PayNet) & ":"

                    'R2.16 CR
                    txtCurrency.Text = oSql.Currency
                Next

                txtOverCommNet.Text = Math.Round(commnett, 2).ToString
                txtOverCommVat.Text = Math.Round(commvat, 2).ToString
                txtOverCommTot.Text = Math.Round(commnett + commvat, 2).ToString
                txtOverCommRec.Text = Math.Round(commrec, 2).ToString
                txtOverCommDue.Text = Math.Round(commdue, 2).ToString
                txtOverTot.Text = Math.Round(total, 2).ToString

                If strpaynet <> "" Then
                    If strpaynet.Contains("True") And strpaynet.Contains("False") Then
                        ddOverPay.SelectedIndex = 3
                    Else
                        If strpaynet.Contains("True") Then
                            ddOverPay.SelectedIndex = 1
                        Else
                            ddOverPay.SelectedIndex = 2
                        End If
                    End If
                End If
            Else
                Throw New EvoFriendlyException("There are no values set up in the Override table.", "Info")
            End If
        End Using
    End Sub

    Public Sub selectSqlData(ByVal pstrInvoiceRef As String)
        Using New clslogger(log, className, "selectSqlData")
            Dim oSqls As New List(Of BossCommissionOverride)
            oSqls = BossCommissionOverride.getSql(pstrInvoiceRef)

            Dim commnett As Decimal = 0
            Dim commvat As Decimal = 0
            Dim commrec As Decimal = 0
            Dim commdue As Decimal = 0
            Dim total As Decimal = 0
            Dim strpaynet As String = ""

            For Each oSql As BossCommissionOverride In oSqls
                commnett = commnett + oSql.CommissionNett
                commvat = commvat + oSql.CommissionVat
                commrec = commrec + oSql.CommissionReceived
                commdue = commdue + oSql.CommissionDue
                total = total + oSql.BookingTotal
                strpaynet = strpaynet & CStr(oSql.PayNet) & ":"
            Next

            txtSqlCommNet.Text = Math.Round(commnett, 2).ToString
            txtSqlCommVat.Text = Math.Round(commvat, 2).ToString
            txtSqlCommTot.Text = Math.Round(commnett + commvat, 2).ToString
            txtSqlCommRec.Text = Math.Round(commrec, 2).ToString
            txtSqlCommDue.Text = Math.Round(commdue, 2).ToString
            txtSqlTot.Text = Math.Round(total, 2).ToString

            If strpaynet <> "" Then
                If strpaynet.Contains("True") And strpaynet.Contains("False") Then
                    txtSqlPay.Text = "Yes & No"
                Else
                    If strpaynet.Contains("True") Then
                        txtSqlPay.Text = "Yes"
                    Else
                        txtSqlPay.Text = "No"
                    End If
                End If
            End If

        End Using
    End Sub

    Public Sub selectBossData(ByVal pstrInvoiceRef As String)
        Using New clslogger(log, className, "selectBossData")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_no, " & _
                                                                     "cast(inm_billed as numeric(18,2)) as inm_billed," & _
                                                                     "cast(inm_vtoncm as numeric(18,2)) as inm_vtoncm," & _
                                                                     "cast(inm_comamt as numeric(18,2)) as inm_comamt," & _
                                                                     "cast(inm_comrcv as numeric(18,2)) as inm_comrcv," & _
                                                                     "cast(inm_comdue as numeric(18,2)) as inm_comdue," & _
                                                                     "inm_paynet " & _
                                                                     "FROM invmain " & _
                                                                     "WHERE inm_no = '" & pstrInvoiceRef & "' and inm_voided <> 'V'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()

                Dim commnett As Decimal = 0
                Dim commvat As Decimal = 0
                Dim commrec As Decimal = 0
                Dim commdue As Decimal = 0
                Dim total As Decimal = 0
                Dim strpaynet As String = ""

                For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    commnett = commnett + CDec(dr.Item("inm_comamt"))
                    commvat = commvat + CDec(dr.Item("inm_vtoncm"))
                    commrec = commrec + CDec(dr.Item("inm_comrcv"))
                    commdue = commdue + CDec(dr.Item("inm_comdue"))
                    total = total + CDec(dr.Item("inm_billed"))
                    strpaynet = strpaynet & CStr(dr.Item("inm_paynet")) & ":"
                Next

                txtBossCommNett.Text = Math.Round(commnett, 2).ToString
                txtBossCommVat.Text = Math.Round(commvat, 2).ToString
                txtBossCommTot.Text = Math.Round(commnett + commvat, 2).ToString
                txtBossCommRec.Text = Math.Round(commrec, 2).ToString
                txtBossCommDue.Text = Math.Round(commdue, 2).ToString
                txtBossTot.Text = Math.Round(total, 2).ToString

                If strpaynet <> "" Then
                    If strpaynet.Contains("True") And strpaynet.Contains("False") Then
                        txtBossPay.Text = "Yes & No"
                    Else
                        If strpaynet.Contains("True") Then
                            txtBossPay.Text = "Yes"
                        Else
                            txtBossPay.Text = "No"
                        End If
                    End If
                End If
            Catch ex As Exception
                log.Error(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub MICommissionOverride_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "MICommissionOverride_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "MICommissionOverride"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Using New clslogger(log, className, "btnSave_Click")
            Try
                txtinvoiceno.Text = txtinvoiceno.Text.Trim
                If txtinvoiceno.Text = "" Then
                    Throw New EvoFriendlyException("Please enter an Invoice number.", "Check Details")
                End If
                If Not Regex.IsMatch(txtinvoiceno.Text, getConfig("InvoiceFormat")) Then
                    Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                End If
                If txtOverCommNet.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommNet.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission nett value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission nett", "Info")
                End If

                If txtOverCommVat.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommVat.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission vat value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission vat", "Info")
                End If

                If txtOverCommRec.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommRec.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission received value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission received", "Info")
                End If

                If txtOverCommDue.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommDue.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission due value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission due", "Info")
                End If

                If ddOverPay.SelectedIndex < 1 Then
                    Throw New EvoFriendlyException("Please select a pay net value", "Info")
                End If



                If saveOverride(False) Then
                    clearBoxes(True)
                    selectOverrideData(txtinvoiceno.Text)
                Else
                    Throw New EvoFriendlyException("Details did not save, please try again", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Private Function saveOverride(ByVal pbPaid As Boolean) As Boolean
        Using New clslogger(log, className, "saveOverride")

            Dim strDue As String = txtOverCommDue.Text
            Dim strRec As String = txtOverCommRec.Text
            If pbPaid Then
                Dim Rec As Decimal = Math.Round(CDec(txtOverCommRec.Text) + CDec(txtOverCommDue.Text), 2)
                strDue = "0.00"
                strRec = CStr(Rec)
            End If
            'R2.16 CR - add currency to save
            Dim oOver As New BossCommissionOverride(txtinvoiceno.Text, txtOverCommNet.Text, txtOverCommVat.Text, strRec, strDue, ddOverPay.SelectedItem.Text, txtSqlTot.Text, txtCurrency.Text)
            If oOver.save() > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Using New clslogger(log, className, "btnDelete_Click")
            Try
                txtinvoiceno.Text = txtinvoiceno.Text.Trim
                If txtinvoiceno.Text = "" Then
                    Throw New EvoFriendlyException("Please enter an Invoice number.", "Check Details")
                End If
                If Not Regex.IsMatch(txtinvoiceno.Text, getConfig("InvoiceFormat")) Then
                    Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                End If
                If BossCommissionOverride.delete(txtinvoiceno.Text) = 1 Then
                    clearBoxes(True)
                    selectOverrideData(txtinvoiceno.Text)
                    Throw New EvoFriendlyException("Override values deleted", "Info")
                Else
                    Throw New EvoFriendlyException("Override values FAILED to delete, please try again", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnPaid_Click(sender As Object, e As EventArgs) Handles btnPaid.Click
        Using New clslogger(log, className, "btnDelete_Click")
            Try
                txtinvoiceno.Text = txtinvoiceno.Text.Trim
                If txtinvoiceno.Text = "" Then
                    Throw New EvoFriendlyException("Please enter an Invoice number.", "Check Details")
                End If
                If Not Regex.IsMatch(txtinvoiceno.Text, getConfig("InvoiceFormat")) Then
                    Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                End If
                If txtOverCommNet.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommNet.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission nett value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission nett", "Info")
                End If

                If txtOverCommVat.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommVat.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission vat value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission vat", "Info")
                End If

                If txtOverCommRec.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommRec.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission received value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission received", "Info")
                End If

                If txtOverCommDue.Text <> "" Then
                    Try
                        Dim dtest As Decimal = CDec(txtOverCommDue.Text)
                    Catch ex As Exception
                        Throw New EvoFriendlyException("Commission due value is not numeric!", "Info")
                    End Try
                Else
                    Throw New EvoFriendlyException("Please enter a numeric value for Commission due", "Info")
                End If

                If ddOverPay.SelectedIndex < 1 Then
                    Throw New EvoFriendlyException("Please select a pay net value", "Info")
                End If

                If saveOverride(True) Then
                    clearBoxes(True)
                    selectOverrideData(txtinvoiceno.Text)
                Else
                    Throw New EvoFriendlyException("Details did not save, please try again", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnAll_Click(sender As Object, e As EventArgs) Handles btnAll.Click
        Using New clslogger(log, className, "btnAll_Click")
            Try
                txtDue.Text = "0"
                Panel1.Visible = False
                ' rvDue.Visible = True
                rvDue.LocalReport.Refresh()
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnDue_Click(sender As Object, e As EventArgs) Handles btnDue.Click
        Using New clslogger(log, className, "btnDue_Click")
            Try
                txtDue.Text = "1"
                Panel1.Visible = False
                'rvDue.Visible = True
                rvDue.LocalReport.Refresh()
            Catch ex As Exception
                handleexception(ex, "MICommissionOverride", Me.Page)
            End Try
        End Using
    End Sub
End Class
