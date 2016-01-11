Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Partial Public Class IAUnpaidCommission
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
                    
                    'go and get all the invoice numbers first so we can update SQL from BOSS for each one so we know we have an up to date set of records!!!!
                    If Not clsBoss.runUnpaidInvoice = "Successful run" Then
                        Throw New EvoFriendlyException("SQL failed to update from BOSS, see Nick.", "Check Details")
                    End If

                End If

                Me.Title = "Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAUnpaidCommission", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Private Sub IAUnpaidCommission_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAUnpaidCommission_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAUnpaidCommission"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAUnpaidCommission", Me.Page)
            End Try
        End Using
    End Sub

    'Protected Sub btnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
    '    Using New clslogger(log, className, "btnrefresh_Click")
    '        Try

    '            'go and get all the invoice numbers first so we can update SQL from BOSS for each one so we know we have an up to date set of records!!!!
    '            If Not clsBoss.runUnpaidInvoice = "Successful run" Then
    '                Throw New EvoFriendlyException("SQL failed to update from BOSS, see Nick.", "Check Details")
    '            End If

    '            rvDetails.Visible = True
    '            rvDetails.LocalReport.Refresh()

    '        Catch ex As Exception
    '            handleexception(ex, "IAOutstandingCommission", Me.Page)
    '        End Try
    '    End Using
    'End Sub
End Class