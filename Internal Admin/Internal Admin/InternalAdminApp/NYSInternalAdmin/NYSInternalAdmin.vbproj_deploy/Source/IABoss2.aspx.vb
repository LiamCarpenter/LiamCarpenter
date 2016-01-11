Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports NysDat
Imports System.Net
Imports System.IO
Imports System.Globalization

Partial Public Class IABoss2
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
                    btnInvoice.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IABoss2", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IABoss2_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IABoss2_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IABoss2"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IABoss2", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IABoss2", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnInvoice_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInvoice.Click
        Using New clslogger(log, className, "btnInvoice_Click")
            Try
                If txtInvRef.Text <> "" Then
                    If Regex.IsMatch(txtInvRef.Text, getConfig("InvoiceFormat")) Then
                        txtresult.Text = clsBoss.runMainSelect(False, txtInvRef.Text.ToUpper, "", "", "") & " for " & txtInvRef.Text
                    Else
                        Throw New EvoFriendlyException("Invoice no. must be in the format 'N111111' where 1 is any number.", "Info")
                    End If
                Else
                    Throw New EvoFriendlyException("Please enter a value in the Invoice textbox.", "Info")
                End If
            Catch ex As Exception
                handleexception(ex, "IABoss2", Me.Page)
            End Try
        End Using
    End Sub

End Class