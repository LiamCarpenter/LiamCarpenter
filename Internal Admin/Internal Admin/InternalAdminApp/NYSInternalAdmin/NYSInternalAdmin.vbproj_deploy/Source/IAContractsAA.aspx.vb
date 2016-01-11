Imports Microsoft.Reporting.WebForms
'Imports NysDat

Partial Public Class IAContractsAA
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim strRet As String = setUser()
                If strRet.StartsWith("ERROR") Then
                    Response.Redirect("IALogonAdmin.aspx?User=falseX")
                End If
                cexFrom.CssClass = "cal_Theme1"
                cexFrom.Format = "dd/MM/yyyy"
                cexTo.CssClass = "cal_Theme1"
                cexTo.Format = "dd/MM/yyyy"

                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")
                btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                If txtfrom.Text = "" Then
                    txtfrom.Text = Format(Now, "dd/MM/yyyy")
                End If
                If txtto.Text = "" Then
                    txtto.Text = Format(Now, "dd/MM/yyyy")
                End If

            End If
        Catch ex As Exception
            handleexception(ex, "IAContractsAA", Me.Page)
        End Try
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If txtfrom.Text = " " Then
                    txtfrom.Text = ""
                End If
                If txtto.Text = " " Then
                    txtto.Text = ""
                End If

                If txtfrom.Text = "" And txtto.Text <> "" Then
                    Throw New EvoFriendlyException("Please complete both date fields to refresh reports!", "Check Details")
                ElseIf txtfrom.Text <> "" And txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please complete both date fields to refresh reports!", "Check Details")
                ElseIf txtfrom.Text <> "" And txtto.Text <> "" Then
                    If CDate(txtto.Text) < CDate(txtfrom.Text) Then
                        Throw New EvoFriendlyException("Please ensure the 'From' date precedes or is the same as the 'To' date to refresh reports!", "Check Details")
                    End If
                End If

                If txtfrom.Text = "" Then
                    txtfrom.Text = " "
                    txtto.Text = " "
                Else
                    Dim dtstart As Date = CDate(txtfrom.Text)
                    Dim dtend As Date = CDate(txtto.Text)
                End If

                rvContractsAA.LocalReport.Refresh()
            Catch ex As Exception
                handleexception(ex, "IAContractsAA", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAContractsAA_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAContractsAA_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAContractsAA"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAContractsAA", Me.Page)
            End Try
        End Using
    End Sub
End Class