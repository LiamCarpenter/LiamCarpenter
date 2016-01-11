Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class IAO2Invoice
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
                    populateDropDowns()
                    populateListofInvoices("0")
                    populateAwaitingInvoices()
                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAO2Invoice", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateDropDowns()
        Using New clslogger(log, className, "populateListofInvoices")
            Dim oStatuses As List(Of O2status)
            oStatuses = O2status.list
            ddtype.Items.Clear()
            ddstatus.Items.Clear()
            ddtype.Items.Add(New ListItem("All Statuses", "0"))
            ddtype.Items.Add(New ListItem("All Except Approved", "1"))
            ddstatus.Items.Add(New ListItem(" ", "0"))
            For Each oStatusX As O2status In oStatuses
                ddtype.Items.Add(New ListItem(oStatusX.O2status, oStatusX.O2statusid))
                ddstatus.Items.Add(New ListItem(oStatusX.O2status, oStatusX.O2statusid))
            Next
        End Using
    End Sub

    Private Sub populateAwaitingInvoices()
        Using New clslogger(log, className, "populateAwaitingInvoices")
            Dim oInvoices As List(Of O2PoAdmin)
            oInvoices = O2PoAdmin.O2PoAdminGetAwaiting
            txtInvoicesAwaiting.Text = ""

            For Each oInvoice As O2PoAdmin In oInvoices
                txtInvoicesAwaiting.Text = txtInvoicesAwaiting.Text & oInvoice.Invoiceref & " - " & oInvoice.PO & " - " & oInvoice.crsref & vbCrLf
            Next
        End Using
    End Sub

    Private Sub populateListofInvoices(ByVal pStatus As String)
        Using New clslogger(log, className, "populateListofInvoices")
            Dim oInvoices As List(Of O2PoAdmin)
            oInvoices = O2PoAdmin.list(pStatus)
            lstInvoicesGone.Items.Clear()
            For Each oInvoice As O2PoAdmin In oInvoices
                lstInvoicesGone.Items.Add(New ListItem(oInvoice.Invoiceref & " - " & oInvoice.PO & " - " & oInvoice.Invoicestatus & " - " & oInvoice.Datesent, oInvoice.O2PoAdminID))
            Next
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IAO2Invoice.aspx")
            Catch ex As Exception
                handleexception(ex, "IAO2Invoice", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If ddtype.SelectedIndex > -1 Then
                    If ddtype.SelectedIndex = 0 Then
                        populateListofInvoices("0")
                    ElseIf ddtype.SelectedIndex = 1 Then
                        populateListofInvoices("1")
                    Else
                        populateListofInvoices(ddtype.SelectedItem.Text)
                    End If
                Else
                    Throw New EvoFriendlyException("Please select a status to search on.", "Check details")
                End If
            Catch ex As Exception
                handleexception(ex, "IAO2Invoice", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAO2Invoice_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAO2Invoice_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAO2Invoice"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAO2Invoice", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub lstInvoicesGone_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstInvoicesGone.SelectedIndexChanged
        Using New clslogger(log, className, "IAO2Invoice_PreRender")
            Try
                If lstInvoicesGone.SelectedIndex > -1 Then
                    populateInvoice(lstInvoicesGone.SelectedItem.Value)
                End If
            Catch ex As Exception
                handleexception(ex, "IAO2Invoice", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateInvoice(ByVal pID As Integer)
        Using New clslogger(log, className, "populateInvoice")
            Dim oInvoice As O2PoAdmin
            oInvoice = O2PoAdmin.get(pID)
            txtinvoiceref.Text = oInvoice.Invoiceref
            txtpo.Text = oInvoice.PO
            txtdatesent.Text = oInvoice.Datesent
            changeDropDowns(ddstatus, "", True, False)
            changeDropDowns(ddstatus, oInvoice.Invoicestatus, False, True)
        End Using
    End Sub

    Private Sub changeDropDowns(ByVal dd As System.Web.UI.WebControls.DropDownList, _
                                ByVal pstrvalue As String, ByVal pblnclear As Boolean, _
                                ByVal pblntext As Boolean)
        Using New clslogger(log, className, "changeDropDowns")
            If pblnclear = True Then
                For intcount As Integer = 0 To dd.Items.Count - 1
                    If dd.Items(intcount).Selected = True Then
                        dd.Items(intcount).Selected = False
                    End If
                Next
                dd.SelectedIndex = 0
            Else
                If pblntext = True Then
                    For intcount As Integer = 0 To dd.Items.Count - 1
                        If dd.Items(intcount).Text.ToLower = pstrvalue.ToLower Then
                            dd.SelectedIndex = intcount
                            Exit For
                        End If
                    Next
                Else
                    For intcount As Integer = 0 To dd.Items.Count - 1
                        If dd.Items(intcount).Value.ToLower = pstrvalue.ToLower Then
                            dd.SelectedIndex = intcount
                            Exit For
                        End If
                    Next
                End If
            End If
        End Using
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Using New clslogger(log, className, "btnUpdate_Click")
            Try
                If ddstatus.SelectedIndex > 0 Then
                    O2PoAdmin.saveStatus(lstInvoicesGone.SelectedItem.Value, ddstatus.SelectedItem.Text)
                    txtinvoiceref.Text = ""
                    txtpo.Text = ""
                    txtdatesent.Text = ""
                    changeDropDowns(ddstatus, "", True, False)
                    If ddtype.SelectedIndex = 0 Then
                        populateListofInvoices("0")
                    ElseIf ddtype.SelectedIndex = 1 Then
                        populateListofInvoices("1")
                    Else
                        populateListofInvoices(ddtype.SelectedItem.Text)
                    End If
                Else
                    Throw New EvoFriendlyException("Please select a status for this invoice.", "Check details")
                End If
            Catch ex As Exception
                handleexception(ex, "IAO2Invoice", Me.Page)
            End Try
        End Using
    End Sub
End Class
