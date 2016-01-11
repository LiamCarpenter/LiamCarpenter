﻿Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Public Class IAOtherTTAllClients
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

                    Dim dtstart As Date
                    Dim dtend As Date
                    dtstart = Now
                    dtend = Now

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")

                    txtfrom.Text = Format(dtstart, "dd/MM/yyyy")
                    txtto.Text = Format(dtend, "dd/MM/yyyy")
                    lblBookingDetails.Text = labelText("Ancillaries Report - All Clients", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)
                End If
                setAjax()
                Me.Title = "All Clients Management Information by NYS Corporate"

            Catch ex As Exception
                handleexception(ex, "IAOtherTTAllClients", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub setAjax()
        Using New clslogger(log, className, "setAjax")
            cexFrom.CssClass = "cal_Theme1"
            cexFrom.Format = "dd/MM/yyyy"
            cexTo.CssClass = "cal_Theme1"
            cexTo.Format = "dd/MM/yyyy"
        End Using
    End Sub

    Protected Sub btnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
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

                Dim dtstart As Date = CDate(txtfrom.Text)
                Dim dtend As Date = CDate(txtto.Text)

                lblBookingDetails.Text = labelText("Ancillaries Report - Car/Coach/Ferry/International Rail - All Clients", dtstart.Day, dtstart.Month, dtstart.Year, dtend.Day, dtend.Month, dtend.Year)

                Session.Item("TTstartdate") = Format(dtstart, "dd/MM/yyyy")
                Session.Item("TTenddate") = Format(dtend, "dd/MM/yyyy")

                btnrefresh.ImageUrl = "~/images/refresh_out.gif"
                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")

                'rvOtherTT.LocalReport.Refresh()
                'rvOtherTT.Visible = True
            Catch ex As Exception
                handleexception(ex, "IAOtherTTAllClients", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IAOtherTTAllClients_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAOtherTTAllClients_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAOtherTTAllClients"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAOtherTTAllClients", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub
End Class