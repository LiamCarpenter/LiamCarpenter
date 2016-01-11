Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
''Imports NysDat

Partial Public Class IAOutstandingCommission
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
                    cexFrom.CssClass = "cal_Theme1"
                    cexFrom.Format = "dd/MM/yyyy"

                    'R2.17 CR
                    cexTo.CssClass = "cal_Theme1"
                    cexTo.Format = "dd/MM/yyyy"

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                    btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")
                    btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                    Dim dtstart As Date

                    dtstart = New Date(Now.Year, Now.Month, 1)
                    dtstart = dtstart.AddMonths(-1)

                    
                    'R2.17 CR
                    Dim dtend As Date
                    dtend = New Date(Now.Year, Now.Month, 1)


                    txtVenue_hidden.Text = "0"
                    txtcompany.Text = CStr(Session.Item("clientid"))
                    txtcompanyid.Text = CStr(Session.Item("companyid"))
                    txtfrom.Text = Format(dtstart, "dd/MM/yyyy")

                    'R2.17 CR
                    txtto.Text = Format(dtend, "dd/MM/yyyy")
                    txtClientBossCode.Text = " "

                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAOutstandingCommission", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If txtfrom.Text = "" Then
                    Throw New EvoFriendlyException("Please complete the date field to refresh reports!", "Check Details")
                End If

                If txtVenue.Text.Trim = "" Then
                    txtVenue_hidden.Text = " "
                Else
                    txtVenue_hidden.Text = txtVenue.Text.Trim
                End If

                Dim dtstart As Date = CDate(txtfrom.Text)

                'R2.17 CR
                Dim dtend As Date = CDate(txtto.Text)
                txtClientBossCode.Text = " "

                'R2.17 CR - Commented Out
                'lblBookingDetails.Text = "Outstanding Commission - " & _
                '                            CStr(dtstart.Day) & " " & getMonthAsShortString(dtstart.Month) & " " & CStr(dtstart.Year)

                lblBookingDetails.Text = "Outstanding Commission between " & Format(dtstart, "dd/MM/yyyy") & " and " & Format(dtend, "dd/MM/yyyy")

                Session.Item("startdateCurrent") = Format(dtstart, "dd/MM/yyyy")
                'go and get all the invoice numbers first so we can update SQL from BOSS for each one so we know we have an up to date set of records!!!!
                If txtVenue_hidden.Text <> "" And txtVenue_hidden.Text <> " " Then
                    If Not clsBoss.runHotelInvoice(txtfrom.Text, txtVenue_hidden.Text) = "Successful run" Then
                        Throw New EvoFriendlyException("SQL failed to update from BOSS, see Nick.", "Check Details")
                    End If
                End If
                rvDetails.Visible = True
                rvDetails.LocalReport.Refresh()

            Catch ex As Exception
                handleexception(ex, "IAOutstandingCommission", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAOutstandingCommission_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAOutstandingCommission_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAOutstandingCommission"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAOutstandingCommission", Me.Page)
            End Try
        End Using
    End Sub
End Class