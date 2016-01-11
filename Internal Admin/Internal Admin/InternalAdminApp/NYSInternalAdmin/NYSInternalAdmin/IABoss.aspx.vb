Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports NysDat
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports BossData

Partial Public Class IABoss
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
                    populateClients()
                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IABoss_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IABoss_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IABoss"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnToday_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnToday.Click
        Using New clslogger(log, className, "btnToday_Click")
            Try
                txtresult.Text = clsBoss.runMainSelect(True, "", "", "", "")
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnClient_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClient.Click
        Using New clslogger(log, className, "btnClient_Click")
            Try
                If ddclients.SelectedIndex > 0 Then
                    txtresult.Text = clsBoss.runMainSelect(False, "", ddclients.SelectedItem.Text, "", "")
                Else
                    Throw New EvoFriendlyException("Please select a Client from the drop down.", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub


    Protected Sub btnClient2_Click(sender As Object, e As EventArgs) Handles btnClient2.Click
        Using New clslogger(log, className, "btnClient2_Click")
            Try
                If ddclients.SelectedIndex > 0 Then
                    txtresult.Text = clsBoss.runMainSelect(False, "", ddclients.SelectedItem.Text, txtstart.Text, txtend.Text)
                Else
                    txtresult.Text = clsBoss.runMainSelect(True, "", "All", txtstart.Text, txtend.Text)
                End If
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnInvoice_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInvoice.Click
        Using New clslogger(log, className, "btnInvoice_Click")
            Try
                If txtInvRef.Text <> "" Then
                    txtresult.Text = clsBoss.runMainSelect(False, txtInvRef.Text, "", "", "") & " for " & txtInvRef.Text
                Else
                    Throw New EvoFriendlyException("Please enter a value in the Invoice textbox.", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateClients()
        Using New clslogger(log, className, "populateClients")

            Dim oCusts As List(Of BOSSinvtot)
            oCusts = BOSSinvtot.listCustomer

            ddclients.Items.Add(New ListItem("Please select", "0"))

            Dim intcount As Integer = 1
            For Each oCust As BOSSinvtot In oCusts
                If oCust.customerid.Trim <> "" Then
                    ddclients.Items.Add(New ListItem(oCust.customerid, CStr(intcount)))
                    intcount += 1
                End If
            Next
        End Using
    End Sub

    Protected Sub btnCash_Click(sender As Object, e As EventArgs) Handles btnCash.Click
        Using New clslogger(log, className, "btnCash_Click")
            Try
                If txtInvRef.Text <> "" Then
                    'try cash header first then cash
                    txtresult.Text = clsBoss.runCashHdr(txtInvRef.Text, "")

                    'now try other way around as BOSS is poo!
                    txtresult.Text = clsBoss.runCashSelect("", txtInvRef.Text)
                Else
                    Throw New EvoFriendlyException("Please enter a value in the Invoice textbox.", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        Using New clslogger(log, className, "btnPay_Click")
            Try
                If txtInvRef.Text <> "" Then
                    txtresult.Text = clsBoss.runPaydet(txtInvRef.Text)
                Else
                    Throw New EvoFriendlyException("Please enter a value in the Invoice textbox.", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IABoss", Me.Page)
            End Try
        End Using
    End Sub
End Class