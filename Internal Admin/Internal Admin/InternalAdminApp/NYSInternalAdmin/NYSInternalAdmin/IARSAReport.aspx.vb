Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports NysDat
Partial Public Class IARSAReport
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            setAjax()

            txtfrom.Text = Date.Now.AddDays(-30).ToString("dd/MM/yyyy")
            txtto.Text = Date.Now.ToString("dd/MM/yyyy")
        End If
    End Sub

    Protected Sub btnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                rvRSADeposits.LocalReport.Refresh()
                rvRSAInvoices.LocalReport.Refresh()


                

            Catch ex As Exception
                handleexception(ex, "IARSAReport", Me.Page)
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

    Private Sub IARSAReport_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IARSAReport_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                       ucReportMenu)
                fp.pageName = "IARSAReport"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IARSAReport", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnSendFiles_Click(sender As Object, e As EventArgs) Handles btnSendFiles.Click
        Using New clslogger(log, className, "btnSendFiles_Click")
            Try
                If txtEmail.Text = "" Then
                    Throw New EvoFriendlyException("Please enter an email address to send to", "Check Details")
                End If

                Dim gvInvoices As New GridView
                gvInvoices.DataSource = ObjectDataSource2
                gvInvoices.DataBind()

                Dim strErrors As New StringBuilder
                Dim lstFilesToZip As New List(Of String)

                Dim strFilePath As String = Server.MapPath("userdocs/100004-" & _
                                              "RSA/100033" & _
                                              "-RSA/documents/")

                For Each row As GridViewRow In gvInvoices.Rows
                    Dim strEnquiryRef As String = ""
                    strEnquiryRef = row.Cells(0).Text

                    'check to see if the enquiry is for RSA UK
                    Dim oEnquiry As New clsEnquiryDat
                    oEnquiry.Populate(0, 0, 0, "", "", 0, strEnquiryRef, 0, 0, False)

                    If oEnquiry.companyname.ToUpper = "RSA" Then
                        'carry on

                        'now check for an invoice file on that booking
                        ' with description of "RSA UK Invoice"
                        Dim oHistory As New clsHistoryDat
                        oHistory.Populate(0, oEnquiry.enquiryid, 0)

                        Dim blnFoundFile As Boolean = False
                        For Each oHistoryItem In oHistory.mcolHistory
                            If oHistory.mstrhistorydesription.ToLower.StartsWith("rsa uk invoice") Or _
                                oHistory.mstrhistorydesription.ToLower.StartsWith("rsa uk supplier invoice") Then
                                'get the file

                                If IO.File.Exists(strFilePath & oHistory.mstrhistorydocumentname) Then
                                    'file found - add to zip list
                                    lstFilesToZip.Add(strFilePath & oHistory.mstrhistorydocumentname)
                                    blnFoundFile = True
                                Else
                                    'file missing - add to errors
                                    blnFoundFile = False
                                End If
                            End If
                        Next

                        If Not blnFoundFile Then
                            'file missing - add to errors
                            strErrors.Append(oEnquiry.nysref & " ")
                        End If
                    Else
                        'dont bother with this one! it's an RSA GCC booking
                    End If
                Next

                'data checking done, now send the files or error
                If strErrors.Length > 0 Then
                    'throw an error
                    Throw New EvoFriendlyException("Invoice files missing for enquiries: " & strErrors.ToString, "Missing Files")
                Else
                    If lstFilesToZip.Count > 0 Then
                        Dim attachmentAbsoluteFilename As String = ""
                        attachmentAbsoluteFilename = strFilePath & "TempInvoices.zip"

                        'zip everything up
                        EvoZipUtils.Zipper.createZipFile(attachmentAbsoluteFilename, lstFilesToZip, strFilePath)

                        SendEmailMessage("accounts@nysgroup.com", _
                                         txtEmail.Text, _
                                         "Invoice Files", _
                                         "Please find attached all requested RSA UK invoice files, dates between " & txtfrom.Text & "-" & txtto.Text, _
                                         attachmentAbsoluteFilename,
                                         "", _
                                         "", _
                                         "", _
                                         "", _
                                         "", _
                                         "")

                        IO.File.Delete(attachmentAbsoluteFilename)

                    Else
                        Throw New EvoFriendlyException("There are no files to zip", "Check Details")
                    End If
                End If

            Catch ex As Exception
                handleexception(ex, "IARSAReport", Me.Page)
            End Try
        End Using
    End Sub
End Class