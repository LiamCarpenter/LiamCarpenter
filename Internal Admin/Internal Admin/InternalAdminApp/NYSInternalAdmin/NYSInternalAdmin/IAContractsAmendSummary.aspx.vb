Option Strict Off

Imports Microsoft.Reporting.WebForms
Imports System.Globalization

'Imports NysDat

Partial Public Class IAContractsAmendSummary
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

                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")
                btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                fillDropDown()
                Dim blnDefault As Boolean = False
                If Session.Item("contractsdate") IsNot Nothing Then
                    changeDropDowns(ddreport, CStr(Session.Item("contractsdate")), False, False)
                    Dim oSplit As Object
                    oSplit = CStr(Session.Item("contractsdate")).Split(CChar("#"))
                    Try
                        Dim iMonth As Integer = CInt(oSplit(0))
                        Dim iYear As Integer = CInt(oSplit(1))
                        Dim dt As New Date(iYear, iMonth, 1)
                        txtfrom.Text = Format(dt, "dd/MM/yyyy")
                        txtto.Text = Format(dt.AddMonths(1).AddDays(-1), "dd/MM/yyyy")
                    Catch ex As Exception
                        blnDefault = True
                    End Try
                Else
                    blnDefault = True
                End If
                If blnDefault Then
                    changeDropDowns(ddreport, "", True, False)
                    Dim dt As Date = Now.AddMonths(-1)
                    Dim dtfrom As New Date(dt.Year, dt.Month, 1)
                    txtfrom.Text = Format(dtfrom, "dd/MM/yyyy")
                    txtto.Text = Format(dtfrom.AddMonths(1).AddDays(-1), "dd/MM/yyyy")
                    changeDropDowns(ddreport, dt.Month & "#" & dt.Year, False, False)
                End If
                lblBookingDetails.Text = "Contract Amendments Summary - " & ddreport.SelectedItem.Text
            End If
            Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
        Catch ex As Exception
            handleexception(ex, "IAContractsAmendSummary", Me.Page)
        End Try
    End Sub

    Private Sub fillDropDown()
        Using New clslogger(log, className, "fillDropDown")
            ddreport.Items.Clear()
            ddreport.Items.Add(New ListItem("Please select", "0"))
            Dim ostart As New Date(Now.AddMonths(2).Year, Now.AddMonths(2).Month, 1)
            Dim intcount As Long = 24

            For intcounts As Long = 1 To intcount + 1
                Dim dt As Date = ostart.AddMonths(-CInt(intcounts - 1))
                ddreport.Items.Add(New ListItem(getMonthAsString(dt.Month) & " " & dt.Year.ToString(CultureInfo.CurrentCulture), _
                                                dt.Month.ToString(CultureInfo.CurrentCulture) & "#" & dt.Year.ToString(CultureInfo.CurrentCulture)))
            Next
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

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If ddreport.SelectedIndex < 1 Then
                    Throw New EvoFriendlyException("Please select a month!", "Check Details")
                End If

                Dim oSplit As Object
                oSplit = ddreport.SelectedItem.Value.Split(CChar("#"))
                Try
                    Dim iMonth As Integer = CInt(oSplit(0))
                    Dim iYear As Integer = CInt(oSplit(1))
                    Dim dt As New Date(iYear, iMonth, 1)
                    txtfrom.Text = Format(dt, "dd/MM/yyyy")
                    txtto.Text = Format(dt.AddMonths(1).AddDays(-1), "dd/MM/yyyy")
                    Session.Item("contractsdate") = ddreport.SelectedItem.Value
                Catch ex As Exception
                    Throw New EvoFriendlyException("Please reselect a month!", "Check Details")
                End Try

                lblBookingDetails.Text = "Contract Amendments Summary - " & ddreport.SelectedItem.Text
                rvAmendSummary.LocalReport.Refresh()
                rvAmendSummaryPerc.LocalReport.Refresh()
                rvUsersSummary.LocalReport.Refresh()
                rvAmendSummaryChart.LocalReport.Refresh()

            Catch ex As Exception
                handleexception(ex, "IAContractsAmendSummary", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAContractsAmendSummary_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAContractsAmendSummary_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAContractsAmendSummary"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAContractsAmendSummary", Me.Page)
            End Try
        End Using
    End Sub
End Class