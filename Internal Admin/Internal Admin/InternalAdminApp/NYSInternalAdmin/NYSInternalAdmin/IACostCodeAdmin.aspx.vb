Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class IACostCodeAdmin
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

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    btnDwp.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    btnDwp.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    btnDwp.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                End If

            Catch ex As Exception
                handleexception(ex, "IACostCodeAdmin", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IACostCodeAdmin", Me.Page)
            End Try
        End Using
    End Sub

    Public Shared Function readAllText(ByVal filename As String) As String
        Try
            Return My.Computer.FileSystem.ReadAllText(filename)
        Catch ex As Exception
            log.Error(ex.Message)
            Return ""
        End Try
    End Function

    Private Sub IACostCodeAdmin_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IACostCodeAdmin_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IACostCodeAdmin"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IACostCodeAdmin", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnDwp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDwp.Click
        Using New clslogger(log, className, "btnDwp_Click")
            Try
                If txtdwp.Text = "" Or txtcmec.Text = "" Then
                    Throw New EvoFriendlyException("Please ensure you have files for both DWP and CMEC to import!", "Check Details")
                Else
                    If txtdwp.Text <> "" Then
                        If Not IO.File.Exists(txtdwp.Text) Then
                            Throw New EvoFriendlyException("The file path for the DWP file is not correct, please check.", "Check Details")
                        End If
                        If Not txtdwp.Text.ToLower.Contains(".csv") Then
                            Throw New EvoFriendlyException("The DWP file does not seem to be a CSV file, please check.", "Check Details")
                        End If
                    End If
                    If txtcmec.Text <> "" Then
                        If Not IO.File.Exists(txtcmec.Text) Then
                            Throw New EvoFriendlyException("The file path for the CMEC file is not correct, please check.", "Check Details")
                        End If
                        If Not txtcmec.Text.ToLower.Contains(".csv") Then
                            Throw New EvoFriendlyException("The DWP file does not seem to be a CSV file, please check.", "Check Details")
                        End If
                    End If

                    'all looks good so lets get rid of the old data first
                    DWPCostcentre.delete()

                    If importFile(txtdwp.Text) Then
                        If importFile(txtcmec.Text) Then
                            Throw New EvoFriendlyException("Both files imported correctly.", "Check Details")
                        Else
                            Throw New EvoFriendlyException("The CMEC file failed to import, please try again.", "Check Details")
                        End If
                    Else
                        Throw New EvoFriendlyException("The DWP file failed to import, please try again.", "Check Details")
                    End If
                End If
            Catch ex As Exception
                handleexception(ex, "IACostCodeAdmin", Me.Page)
            End Try
        End Using
    End Sub

    Private Shared Function item(ByVal a As IList(Of String), ByVal index As Integer) As String
        If a.Count - 1 < index Then
            Return ""
        Else
            Return CStr(a.Item(index)).Trim
        End If
    End Function

    Private Enum FieldIndexes
        businessunit = 0
        costcentre
        description
    End Enum

    Public Function importFile(ByVal pstrFile As String) As Boolean
        Try

            Dim parser As New CSVParser.CSVParser(readAllText(pstrFile))
            Dim line As IList(Of String) = parser.readLine()

            Do While Not line Is Nothing
                If line.Count = 1 Then
                    Dim Values() As String = Split(item(line, 0), ",")
                    If item(Values, 0).ToLower <> "business unit codes" Then
                        If item(Values, FieldIndexes.costcentre) <> "" Then
                            Dim oDwp As New DWPCostcentre(0, item(Values, FieldIndexes.businessunit), item(Values, FieldIndexes.costcentre), item(Values, FieldIndexes.description))
                            oDwp.save()
                        End If
                    End If
                Else
                    If line.Count > 1 Then
                        If item(line, 0).ToLower <> "business unit codes" Then
                            If item(line, FieldIndexes.costcentre) <> "" Then
                                Dim oDwp As New DWPCostcentre(0, item(line, FieldIndexes.businessunit), item(line, FieldIndexes.costcentre), item(line, FieldIndexes.description))
                                oDwp.save()
                            End If
                            End If
                        End If
                    End If
                    line = parser.readLine
            Loop

            Return True
        Catch ex As Exception
            log.Error(ex.Message)
            Return False
        End Try
    End Function
End Class



