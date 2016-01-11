Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Partial Public Class IAAirTTAllClients
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
                    'setAjax()

                    Dim dtstart As Date
                    Dim dtend As Date
                    'btnrefresh.ImageUrl = "~/images/run_out.gif"
                    'btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    'btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    'btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")

                    'If Not Session.Item("TTstartdate") Is Nothing Then
                    '    dtstart = CDate(Session.Item("TTstartdate"))
                    '    dtend = CDate(Session.Item("TTenddate"))
                    '    btnrefresh.ImageUrl = "~/images/refresh_out.gif"
                    '    btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                    '    btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")
                    '    'Dim cityParameter As New ReportParameter("City", "testx") 'this sets whether the column is visible or not testing
                    '    'rvAirTT.LocalReport.SetParameters((New ReportParameter() {cityParameter}))
                    '    'rvAirTT.LocalReport.Refresh()
                    '    'rvAirTT.Visible = True
                    'Else
                    dtstart = Now
                    dtend = Now
                    'End If

                    txtlocation.Text = ""
                    txthidden_location.Text = " "
                    txthidden_locationCode.Text = " "
                    txtairline.Text = ""
                    txthidden_airline.Text = " "
                    txtfrom.Text = Format(dtstart, "dd/MM/yyyy")
                    txtto.Text = Format(dtend, "dd/MM/yyyy")
                    txtbossname.Text = CStr(Session.Item("clientnameshort"))
                    txtbossname2.Text = CStr(Session.Item("clientnameshort2"))
                    txtclientid.Text = CStr(Session.Item("clientid"))
                    lblBookingDetails.Text = labelText("Traveller Tracking - Air - All Clients", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)

                End If
                setAjax()

                Me.Title = "All Clients Management Information by NYS Corporate"

            Catch ex As Exception
                handleexception(ex, "IAAirTTAllClients", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub setAjax()
        Using New clslogger(log, className, "setAjax")
            cexFrom.CssClass = "cal_Theme1"
            cexFrom.Format = "dd/MM/yyyy"
            cexTo.CssClass = "cal_Theme1"
            cexTo.Format = "dd/MM/yyyy"

            extLocationSearch.EnableCaching = True
            extLocationSearch.CompletionListCssClass = "list2"
            extLocationSearch.CompletionListHighlightedItemCssClass = "hoverlistitem2"
            extLocationSearch.CompletionListItemCssClass = "listitem2"
            extLocationSearch.ServiceMethod = "airportSearch"
            extLocationSearch.ServicePath = "AjaxService.asmx"
            extLocationSearch.MinimumPrefixLength = 1

            extAirlineSearch.EnableCaching = True
            extAirlineSearch.CompletionListCssClass = "list2"
            extAirlineSearch.CompletionListHighlightedItemCssClass = "hoverlistitem2"
            extAirlineSearch.CompletionListItemCssClass = "listitem2"
            extAirlineSearch.ServiceMethod = "airlineSearch"
            extAirlineSearch.ServicePath = "AjaxService.asmx"
            extAirlineSearch.MinimumPrefixLength = 1
        End Using
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    If btnrefresh.ImageUrl = "~/images/run_out.gif" Then
                        Throw New EvoFriendlyException("Please complete both date fields to run report!", "Check Details")
                    Else
                        Throw New EvoFriendlyException("Please complete both date fields to refresh report!", "Check Details")
                    End If
                End If
                If CDate(txtto.Text) < CDate(txtfrom.Text) Then
                    If btnrefresh.ImageUrl = "~/images/run_out.gif" Then
                        Throw New EvoFriendlyException("Please ensure the 'From' date precedes or is the same as the 'To' date to run report!", "Check Details")
                    Else
                        Throw New EvoFriendlyException("Please ensure the 'From' date precedes or is the same as the 'To' date to refresh report!", "Check Details")
                    End If
                End If
                If txtlocation.Text.Trim = "" Then
                    txthidden_location.Text = " "
                    txthidden_locationCode.Text = " "
                Else
                    If txtlocation.Text.Contains("(") Then
                        'R2.17 CR - fix the code selection
                        'txthidden_locationCode.Text = Trim(Replace(Replace(Mid(txtlocation.Text, txtlocation.Text.IndexOf("(")), ")", ""), "(", ""))
                        'txthidden_location.Text = " "
                        Dim intFirstBracket As Integer = txtlocation.Text.IndexOf("(")
                        Dim intCodeLength As Integer = txtlocation.Text.Substring(txtlocation.Text.IndexOf("(")).IndexOf(")")

                        txthidden_locationCode.Text = Trim(Replace(Replace(txtlocation.Text.Substring(intFirstBracket, intCodeLength), ")", ""), "(", ""))
                        txthidden_location.Text = " "

                    ElseIf txtlocation.Text.Contains(":") Then
                        txthidden_location.Text = Mid(txtlocation.Text, 1, txtlocation.Text.IndexOf(":"))
                        txthidden_locationCode.Text = " "
                    Else
                        txthidden_location.Text = Trim(txtlocation.Text)
                        txthidden_locationCode.Text = " "
                    End If
                End If

                If txtairline.Text.Trim = "" Then
                    txthidden_airline.Text = " "
                Else
                    txthidden_airline.Text = txtairline.Text.Trim
                End If

                Dim dtstart As Date = CDate(txtfrom.Text)
                Dim dtend As Date = CDate(txtto.Text)
                lblBookingDetails.Text = labelText("Traveller Tracking - Air - All Clients", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)
                Session.Item("TTstartdate") = Format(dtstart, "dd/MM/yyyy")
                Session.Item("TTenddate") = Format(dtend, "dd/MM/yyyy")
                'Dim cityParameter As New ReportParameter("City", "testx")
                'rvAirTT.LocalReport.SetParameters((New ReportParameter() {cityParameter}))

                btnrefresh.ImageUrl = "~/images/refresh_out.gif"
                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")
                rvAirTT.LocalReport.Refresh()
                rvAirTT.Visible = True
            Catch ex As Exception
                handleexception(ex, "IAAirTTAllClients", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAAirTTAllClients_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAAirTTAllClients_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAAirTTAllClients"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAAirTTAllClients", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub
End Class