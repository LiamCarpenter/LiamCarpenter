Imports EvoUtilities.ConfigUtils
Imports NysDat
Public Class IAPoCheckJLR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            loadReport()
        End If

    End Sub

    ''' <summary>
    ''' load the report on to the page as results. This will need recoding at some point - so that PO's and their values are logged in MEVIS
    ''' For now this will try and find all PO's used and sum up the booked and billed amounts. It can't tell if their over value.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub loadReport()
        'first grab all the PO's used in our system
        Dim oPoList As New List(Of String)
        oPoList = clsPo.getPOListForCustomer(0, 100090)

        Dim strResult As New StringBuilder

        For Each oPo In oPoList
            'load the current values for that PO
            'this code assumes that the PO is unique across all customers - rushed
            ' will need modifications in future to take company in to 'where' statements
            Dim dcBossValue As Double = Math.Round(runBossLivePoCheck(oPo), 2)
            Dim dcConfMevisValue As Double = Math.Round(CDec(clsPo.confirmedMevisValueGet(oPo)), 2)
            Dim dcPotMevisValue As Double = Math.Round(CDec(clsPo.potentialMevisValueGet(oPo)), 2)
            Dim dcConfCubitValue As Double = Math.Round(CDec(clsPo.importedCubitValueGet(oPo)), 2)
            Dim dcPotCubitValue As Double = Math.Round(CDec(clsPo.potentialCubitValueGet(oPo)), 2)


            strResult.Append(oPo & vbCrLf)
            strResult.Append("Accommodation Confirmed= " & vbTab & "£" & CStr(dcConfCubitValue) & vbCrLf)
            strResult.Append("Accommodation Potential= " & vbTab & "£" & CStr(dcPotCubitValue) & vbCrLf)
            strResult.Append("Invoiced value= " & vbTab & vbTab & "£" & CStr(dcBossValue) & vbCrLf)
            strResult.Append("Conference Confirmed= " & vbTab & "£" & CStr(dcConfMevisValue) & vbCrLf)
            strResult.Append("Conference Potential= " & vbTab & "£" & CStr(dcPotMevisValue) & vbCrLf)
            strResult.Append("TOTAL VALUE= " & vbTab & vbTab & "£" & CStr(Math.Round(dcBossValue + dcConfMevisValue + dcPotMevisValue + dcConfCubitValue + dcPotCubitValue, 2)) & vbCrLf)

            strResult.Append(vbCrLf & vbCrLf)

        Next

        txtresult.Text = strResult.ToString

    End Sub

    Public Function runBossLivePoCheck(ByVal pstrPO As String) As Decimal

        Dim dblRet As Decimal = 0

        Try
            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            dBaseConnection.Open()

            Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_billed as inm_billed, " & _
                                                                            "inm_bilvat as inm_bilvat, " & _
                                                                            "inm_disvat as inm_disvat " & _
                                                                            "FROM Invmain where inm_pono = '" & pstrPO & "' and (inm_custid = 'JLR' or inm_custid = 'LANDROVER')", dBaseConnection)
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
        loadReport()
    End Sub

    Private Sub IAPoCheckJLR_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                     ucReportMenu)
        fp.pageName = "IAPoCheckJLR"
        phMenu.Controls.Add(fp)
    End Sub
End Class