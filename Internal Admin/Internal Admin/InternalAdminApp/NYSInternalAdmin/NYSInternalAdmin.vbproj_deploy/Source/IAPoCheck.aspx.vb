Imports NysDat

Partial Public Class IAPoCheck
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
                    populatePOOwners()
                    checkPO("")
                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAPoCheck", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populatePOOwners()
        Using New clslogger(log, className, "populatePOOwners")
            Dim oOwners As New List(Of clsO2Po)
            oOwners = clsO2Po.RequesterNameList(1)

            ddpoOwners.Items.Clear()
            ddpoOwners.Items.Add(New ListItem("All PO Owners", ""))

            Dim intCount As Integer = 0
            For Each oOwner As clsO2Po In oOwners
                ddpoOwners.Items.Add(New ListItem(oOwner.RequesterName, CStr(intCount)))
                intCount += 1
            Next
        End Using
    End Sub

    Private Sub IAPoCheck_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAPoCheck_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IAPoCheck"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAPoCheck", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IAPoCheck", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub checkPO(ByVal pstrRequesterName As String)
        Try
            Using New clslogger(log, className, "checkPO")

                Dim oPos As New List(Of clsPo)
                oPos = clsPo.poReportList(pstrRequesterName)


                Dim strCSV As New StringBuilder
                'add the headings to the CSV
                strCSV.Append("Requester,PO Number,Accommodation Confirmed,Accommodation Potential,Invoiced Value,Conference Confirmed,Conference Potential,Total Value,PO Value,Remaining" & vbCrLf)


                Dim strResult As String = ""
                For Each oPoX As clsPo In oPos
                    Dim dcBossValue As Double = Math.Round(runBossLivePoCheck(oPoX.OrderNumber), 2)
                    Dim dcConfMevisValue As Double = Math.Round(CDec(clsPo.confirmedMevisValueGet(oPoX.OrderNumber)), 2)
                    Dim dcPotMevisValue As Double = Math.Round(CDec(clsPo.potentialMevisValueGet(oPoX.OrderNumber)), 2)
                    Dim dcConfCubitValue As Double = Math.Round(CDec(clsPo.importedCubitValueGet(oPoX.OrderNumber)), 2)
                    Dim dcPotCubitValue As Double = Math.Round(CDec(clsPo.potentialCubitValueGet(oPoX.OrderNumber)), 2)

                    'bulid the text version of the results
                    strResult = strResult & oPoX.RequesterName & ": " & vbTab & oPoX.OrderNumber & vbCrLf & _
                                vbTab & vbTab & "Accommodation Confirmed= " & vbTab & "£" & CStr(dcConfCubitValue) & vbCrLf & _
                                vbTab & vbTab & "Accommodation Potential= " & vbTab & "£" & CStr(dcPotCubitValue) & vbCrLf & _
                                vbTab & vbTab & "Invoiced value= " & vbTab & vbTab & "£" & CStr(dcBossValue) & vbCrLf & _
                                vbTab & vbTab & "Conference Confirmed= " & vbTab & "£" & CStr(dcConfMevisValue) & vbCrLf & _
                                vbTab & vbTab & "Conference Potential= " & vbTab & "£" & CStr(dcPotMevisValue) & vbCrLf & _
                                vbTab & vbTab & "TOTAL VALUE= " & vbTab & vbTab & "£" & CStr(Math.Round(dcBossValue + dcConfMevisValue + dcPotMevisValue + dcConfCubitValue + dcPotCubitValue, 2)) & vbCrLf & _
                                vbTab & vbTab & "PO VALUE= " & vbTab & vbTab & "£" & CStr(Math.Round(oPoX.TotalPrice, 2)) & vbCrLf & _
                                vbTab & vbTab & "REMAINING= " & vbTab & vbTab & "£" & CStr(Math.Round(Math.Round(oPoX.TotalPrice, 2) - Math.Round(dcBossValue + dcConfMevisValue + dcPotMevisValue + dcConfCubitValue + dcPotCubitValue, 2), 2)) & vbCrLf & vbCrLf

                    'also build a csv of the results
                    strCSV.Append("""" & oPoX.RequesterName & """,")
                    strCSV.Append(oPoX.OrderNumber & ",")
                    strCSV.Append(CStr(dcConfCubitValue) & ",")
                    strCSV.Append(CStr(dcPotCubitValue) & ",")
                    strCSV.Append(CStr(dcBossValue) & ",")
                    strCSV.Append(CStr(dcConfMevisValue) & ",")
                    strCSV.Append(CStr(dcPotMevisValue) & ",")
                    strCSV.Append(CStr(Math.Round(dcBossValue + dcConfMevisValue + dcPotMevisValue + dcConfCubitValue + dcPotCubitValue, 2)) & ",")
                    strCSV.Append(CStr(Math.Round(oPoX.TotalPrice, 2)) & ",")
                    strCSV.Append(CStr(Math.Round(Math.Round(oPoX.TotalPrice, 2) - Math.Round(dcBossValue + dcConfMevisValue + dcPotMevisValue + dcConfCubitValue + dcPotCubitValue, 2), 2)) & vbCrLf)
                Next


                hdnResults.Value = strCSV.ToString

                ''check Booked records
                'Dim oBPOs As New List(Of clsPo)
                'oBPOs = clsPo.POBookedCheck

                'If oBPOs.Count > 0 Then
                '    strResult = strResult & vbCrLf & vbCrLf & "Cubit booked records where PO is not a blanket PO:" & vbCrLf
                'End If
                'For Each oBPO As clsPo In oBPOs
                '    strResult = strResult & vbTab & oBPO.OrderNumber & vbCrLf
                'Next

                ''check Imported records
                'Dim oBPXs As New List(Of clsPo)
                'oBPXs = clsPo.POImportedCheck

                'If oBPXs.Count > 0 Then
                '    strResult = strResult & vbCrLf & vbCrLf & "Cubit imported records where PO is not a blanket PO:" & vbCrLf
                'End If
                'For Each oBPX As clsPo In oBPXs
                '    strResult = strResult & vbTab & oBPX.OrderNumber & vbCrLf
                'Next
                txtresult.Text = strResult
            End Using
        Catch ex As Exception
            Dim strtest As String = ex.Message
        End Try
    End Sub

    Public Function runBossLivePoCheck(ByVal pstrPO As String) As Decimal

        Dim dblRet As Decimal = 0

        Try
            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            dBaseConnection.Open()

            Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_billed as inm_billed, " & _
                                                                            "inm_bilvat as inm_bilvat, " & _
                                                                            "inm_disvat as inm_disvat " & _
                                                                            "FROM Invmain where inm_pono = '" & pstrPO & "'", dBaseConnection)
            Dim dBaseDataReader As System.Data.OleDb.OleDbDataReader = dBaseCommand.ExecuteReader()

            While dBaseDataReader.Read
                Dim dblGross As Decimal = CDec(dBaseDataReader("inm_billed").ToString)
                Dim dblNormalVat As Decimal = CDec(dBaseDataReader("inm_bilvat").ToString)
                Dim dblDispVat As Decimal = CDec(dBaseDataReader("inm_disvat").ToString)
                Dim dblNett As Decimal = dblGross - (dblNormalVat + dblDispVat)
                dblRet = dblRet + dblNett
            End While

            dBaseDataReader.Close()
            dBaseConnection.Close()
        Catch ex As Exception
            dblRet = -1
        End Try

        Return dblRet
    End Function

    Protected Sub btnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
        Using New clslogger(log, className, "btnrefresh_Click")
            Try
                If ddpoOwners.SelectedIndex < 1 Then
                    checkPO("")
                Else
                    checkPO(ddpoOwners.SelectedItem.Text)
                End If
            Catch ex As Exception
                handleexception(ex, "IAPoCheck", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click
        'send them the results of the last refresh
        Response.Charset = ""
        Response.ContentEncoding = System.Text.Encoding.Default
        Response.AddHeader("Content-Type", "application/csv")
        Response.AddHeader("content-disposition", "attachment; filename=O2 PO Report " & Date.Now.ToString("dd-MM-yyyy") & ".csv")
        Response.Write(hdnResults.Value)
        Response.End()
    End Sub
End Class