Imports Microsoft.Reporting.WebForms

Partial Public Class IAInactiveMevisContacts
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

                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")
                btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")
                If txtfrom.Text = "" Then
                    txtfrom.Text = " "
                End If
                
                txtcompany.Text = CStr(Session.Item("clientid"))
                txtbossname.Text = CStr(Session.Item("clientnameshort"))
                txtbossname2.Text = CStr(Session.Item("clientnameshort2"))
                lblBookingDetails.Text = "Inactive Mevis Users"
            End If
            Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
        Catch ex As Exception
            handleexception(ex, "IAInactiveMevisContacts", Me.Page)
        End Try
    End Sub

    Protected Sub btnlogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Protected Sub btnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If txtfrom.Text = " " Then
                    txtfrom.Text = ""
                End If

                If txtfrom.Text = "" Then
                    Throw New EvoFriendlyException("Please complete the date field to refresh report!", "Check Details")
                End If

                If txtfrom.Text = "" Then
                    txtfrom.Text = " "
                    lblBookingDetails.Text = "Inactive Mevis Contacts"
                Else
                    Dim dtstart As Date = CDate(txtfrom.Text)
                    lblBookingDetails.Text = "Inactive Mevis Users since " & dtstart.Day & "/" & dtstart.Month & "/" & dtstart.Year
                End If

                rvInactiveMevisContacts.LocalReport.Refresh()
            Catch ex As Exception
                handleexception(ex, "IAInactiveMevisContacts", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAInactiveMevisContacts_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAInactiveMevisContacts_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAInactiveMevisContacts"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAInactiveMevisContacts", Me.Page)
            End Try
        End Using
    End Sub

End Class