Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms


Partial Public Class IAOOP
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

                setAjax()
                btnrefresh.ImageUrl = "~/images/run_out.gif"
                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")

                Dim dtstart As New Date(Now.Year, Now.Month, 1)
                Dim dtend As Date = Now.AddDays(-1)
                txtfrom.Text = Format(dtstart, "dd/MM/yyyy")
                txtto.Text = Format(dtend, "dd/MM/yyyy")
                txtcompany.Text = CStr(Session.Item("clientid"))
                txtbossname.Text = CStr(Session.Item("clientnameshort"))
                txtbossname2.Text = CStr(Session.Item("clientnameshort2"))
                lblBookingDetails.Text = labelText("Out of Policy", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)
            End If
            Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
        Catch ex As Exception
            handleexception(ex, "IAOOP", Me.Page)
        End Try
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Private Sub setAjax()
        Using New clslogger(log, className, "setAjax")
            cexFrom.CssClass = "cal_Theme1"
            cexFrom.Format = "dd/MM/yyyy"
            cexTo.CssClass = "cal_Theme1"
            cexTo.Format = "dd/MM/yyyy"
        End Using
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please complete both date fields to refresh reports!", "Check Details")
                End If
                If CDate(txtto.Text) < CDate(txtfrom.Text) Then
                    Throw New EvoFriendlyException("Please ensure the 'From' date precedes or is the same as the 'To' date to refresh reports!", "Check Details")
                End If

                Dim dtstart As Date = CDate(txtfrom.Text)
                Dim dtend As Date = CDate(txtto.Text)
                lblBookingDetails.Text = labelText("Out of Policy", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)


                rvConferma.LocalReport.Refresh()
                btnrefresh.ImageUrl = "~/images/refresh_out.gif"
                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")

                rvConferma.Visible = True
            Catch ex As Exception
                handleexception(ex, "IAOOP", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAOOP_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAOOP_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                       ucReportMenu)
                fp.pageName = "IAOOP"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAOOP", Me.Page)
            End Try
        End Using
    End Sub
End Class