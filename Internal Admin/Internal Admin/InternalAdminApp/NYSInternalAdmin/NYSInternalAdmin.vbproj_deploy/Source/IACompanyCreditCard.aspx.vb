﻿Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
''Imports NysDat

Partial Public Class IACompanyCreditCard
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
                    PopulateCards()

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

                    Dim dtstart As Date = CDate(Session.Item("startdate"))
                    Dim dtend As Date = CDate(Session.Item("enddate"))
                    txtfrom.Text = Format(dtstart, "dd/MM/yyyy")
                    txtto.Text = Format(dtend, "dd/MM/yyyy")
                    txtcompany.Text = CStr(Session.Item("clientid"))
                    lblBookingDetails.Text = labelText("Company Credit Card Purchasing", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)
                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IACompanyCreditCard", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
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
                lblBookingDetails.Text = labelText("Company Credit Card Purchasing", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)
                Session.Item("startdate") = Format(dtstart, "dd/MM/yyyy")
                Session.Item("enddate") = Format(dtend, "dd/MM/yyyy")

                rvCompanyCreditCard.LocalReport.Refresh()
            Catch ex As Exception
                handleexception(ex, "IACompanyCreditCard", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IACompanyCreditCard_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IACompanyCreditCard_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IACompanyCreditCard"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IACompanyCreditCard", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub PopulateCards()
        ddCard.Items.Clear()

        'double underscore for value so SQL does wildcard characters
        ddCard.Items.Add(New ListItem("All", "__"))

        ddCard.Items.Add(New ListItem("Daryl Pinnington", "DP"))
        ddCard.Items.Add(New ListItem("Nicky Traynor", "NT"))
    End Sub
End Class