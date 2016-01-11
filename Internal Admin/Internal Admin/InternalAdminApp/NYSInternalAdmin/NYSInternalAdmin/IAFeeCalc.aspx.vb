Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports EvoUtilities.CollectionUtils
Imports BossData

Partial Public Class IAFeeCalc
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
                    cexTo.CssClass = "cal_Theme1"
                    cexTo.Format = "dd/MM/yyyy"

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    btnRun.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    btnRun.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    btnRun.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                End If
                Me.Title = "Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAFeeCalc", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Private Sub IAFeeCalc_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAFeeCalc_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAFeeCalc"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAFeeCalc", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnRun_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnRun.Click
        Using New clslogger(log, className, "btnRun_Click")
            Try
                If txtfrom.Text = "" Or txtto.Text = "" Then
                    Throw New EvoFriendlyException("Please complete both date fields to continue!", "Check details")
                End If
                Try
                    Dim dTemp As Date = CDate(txtfrom.Text)
                Catch ex As Exception
                    Throw New EvoFriendlyException("Please check From date is in the correct format 'dd/mm/yyyy' to continue!", "Check details")
                End Try
                Try
                    Dim dTemp As Date = CDate(txtto.Text)
                Catch ex As Exception
                    Throw New EvoFriendlyException("Please check To date is in the correct format 'dd/mm/yyyy' to continue!", "Check details")
                End Try
                If CDate(txtfrom.Text) > CDate(txtto.Text) Then
                    Throw New EvoFriendlyException("The From date must be set before to To date to continue!", "Check details")
                End If

                'first lets get all invoices from BOSS and update SQL
                If Session.Item("FeederFileClient") IsNot Nothing Then
                    If CStr(Session.Item("FeederFileClient")).ToUpper <> "NONE" Then
                        Dim strRet As String = runInvoiceNumberCheck(txtfrom.Text, txtto.Text, CStr(Session.Item("FeederFileClient")))
                        If strRet <> "" Then
                            Throw New EvoFriendlyException(strRet, "Check details")
                        End If
                    End If
                End If

                Dim retDetails As New getBossIDs
                retDetails = checkClientName(CStr(Session.Item("FeederFileClient")))

                Dim intCount As Integer = BOSSinvmain.invoiceCount(txtfrom.Text, txtto.Text, retDetails.strBossID1, retDetails.strBossID2)

                lblResult.Text = "There are " & CStr(intCount) & " invoices for client: " & CStr(Session.Item("FeederFileClient")) & " between " & txtfrom.Text & " and " & txtto.Text & ".<br><br>" & _
                                    "Please create an invoice in BOSS for the print fee, value= £" & CStr(Math.Round(CDec(intCount * CDbl(getConfig("PrintFee"))), 2)) & ".<br><br>" & _
                                    "Please ensure the invoice note has the following text: <br><br>" & _
                                    "Printing fee invoice for " & CStr(intCount) & " invoices @ £" & CStr(getConfig("PrintFee")) & " each."
            Catch ex As Exception
                handleexception(ex, "IAFeeCalc", Me.Page)
            End Try
        End Using
    End Sub
End Class