Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail
Imports NysDat
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Xml
Imports System.IO
Partial Public Class IAFeraInvoices
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            cexFrom.CssClass = "cal_Theme1"
            cexFrom.Format = "dd/MM/yyyy"

            If Not IsPostBack Then
                Dim strRet As String = setUser()
                If strRet.StartsWith("ERROR") Then
                    Response.Redirect("IALogonAdmin.aspx?User=falseX")
                End If
                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
            End If
        Catch ex As Exception
            handleexception(ex, "IAFeraInvoices", Me.Page)
        End Try
    End Sub

    Protected Sub btnGetResults_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetResults.Click
        Try


            txtRecordsMissingFiles.Text = ""

            'check date is valid
            Dim dteRecordsFrom As Date
            Try
                Dim test As String = ""
                test = Format(CDate(txtDateFrom.Text), "dd/MM/yyyy")

                dteRecordsFrom = CDate(test)
            Catch ex As Exception
                'do something
                Throw New EvoFriendlyException("Date entered is not valid, please try again", "IAFeraInvoices Date")
            End Try


            'get records from db
            Dim oBossRecords As List(Of clsInvoicingDat)
            oBossRecords = clsInvoicingDat.getFeraInvoices(dteRecordsFrom)


            'display the count of records
            litBossInvoices.Text = oBossRecords.Count.ToString

            'work out the month span of selected date
            Dim intMonths As Integer = 0
            intMonths = notInteger(Math.Ceiling(DateDiff(DateInterval.Month, dteRecordsFrom, Date.Now)))


            Dim intMatchCount As Integer = 0
            'loop through records
            For Each oInvoice As clsInvoicingDat In oBossRecords
                'check to see if pdf file exists in any of the folders
                Dim x As Integer = 0
                Dim blnFoundFile As Boolean = False
                For x = 0 To intMonths
                    Dim intYear As Integer = 0
                    Dim intMonth As Integer = 0

                    intYear = dteRecordsFrom.AddMonths(x).Year
                    intMonth = dteRecordsFrom.AddMonths(x).Month

                    Dim strDatePath As String = "_" & CStr(intYear) & CStr(IIf(intMonth < 10, "0" & CStr(intMonth), CStr(intMonth)))
                    Dim strReadFromFolder As String = getConfig("FeraInput").Replace("###", strDatePath)
                    Dim strSentFolder As String = getConfig("FeraSent").Replace("###", strDatePath)
                    Dim strNotSentFolder As String = getConfig("FeraNotSent").Replace("###", strDatePath)
                    Dim strErrorFolder As String = getConfig("FeraError").Replace("###", strDatePath)
                    Dim strAwaitingFolder As String = getConfig("FeraAwaiting")

                    If File.Exists(strReadFromFolder & oInvoice.mstrinvoicenumber & "M.pdf") Then
                        'count the number of records that have a file
                        blnFoundFile = True

                        'check the other folder
                    ElseIf File.Exists(strSentFolder & oInvoice.mstrinvoicenumber & "M.pdf") Then
                        blnFoundFile = True
                    ElseIf File.Exists(strNotSentFolder & oInvoice.mstrinvoicenumber & "M.pdf") Then
                        blnFoundFile = True
                    ElseIf File.Exists(strErrorFolder & oInvoice.mstrinvoicenumber & "M.pdf") Then
                        blnFoundFile = True
                    ElseIf File.Exists(strAwaitingFolder & oInvoice.mstrinvoicenumber & "M.pdf") Then
                        blnFoundFile = True
                    End If
                Next

                If blnFoundFile Then
                    intMatchCount += 1
                Else
                    'add any record id without file to txtRecordsMissingFiles
                    txtRecordsMissingFiles.Text = txtRecordsMissingFiles.Text & vbNewLine & oInvoice.mstrinvoicenumber
                End If
            Next

            litBossMatching.Text = intMatchCount.ToString
        Catch ex As Exception
            handleexception(ex, "IAFeraInvoices", Me.Page)
        End Try
    End Sub

    Private Sub IAFeraInvoices_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAFeraInvoices_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IAFeraInvoices"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAFeraInvoices", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub
End Class