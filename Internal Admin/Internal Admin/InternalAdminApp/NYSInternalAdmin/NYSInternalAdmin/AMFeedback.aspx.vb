
Imports System.Xml
Imports System
Imports System.Data
Imports System.Diagnostics
Imports System.Threading
Imports EvoUtilities.ConfigUtils
Imports EvoUtilities.CollectionUtils
Imports EvoUtilities.log4netUtils
Imports EvoUtilities.SendEmailUtils

Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic

Imports System.IO
Imports NysDat.clsUsefulFunctions
Imports NysDat
Imports System.Text
Imports EvoDatabaseUtils
Imports System.Net.Mail

Imports System.Data.SqlClient



Public Class AMFeedback
    'Inherits System.Web.UI.Page
    Inherits clsNYS

    Private Shared ReadOnly className As String
    'Protected Shared log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType) 'in clsNYS

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "AMFeedback", Me.Page)
            End Try
        End Using
    End Sub
    Private Sub IAClientStatements_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAClientStatements_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "AMFeedback"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "AMFeedback", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load - Feedback")
            If Not IsPostBack Then

                ' butFeedbackEmails_Click(sender, e)
                'Dim strRet As String = setUser()
                'If strRet.StartsWith("ERROR") Then
                '    Response.Redirect("IALogonAdmin.aspx?User=falseX")
                'End If

                PopulateDates()
                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                butFeedbackEmails.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                butFeedbackEmails.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                butFeedbackEmails.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                populateBossRef()
                Me.chkTest.Checked = True
            End If
        End Using

    End Sub
    Private Sub PopulateDates()
        Dim dt = DateTime.ParseExact(Now.Date, "d", Nothing)
        'Date Tester
        'Dim dt = DateTime.ParseExact(Now.Date.AddMonths(+8), "d", Nothing)
        Dim currentMonth As Integer = dt.Month 'not this month but the last by default
        Dim currentYear As Integer = dt.Year
        Dim previousMonth As Integer
        Dim previousYear As Integer

        If currentMonth = 1 Then
            previousMonth = 12
            previousYear = currentYear - 1
        Else
            previousMonth = currentMonth - 1
            previousYear = currentYear
        End If
        ''us dates


        If previousMonth < 10 Then
            txtFrom.Text = "0" + previousMonth.ToString() + "/01/" + previousYear.ToString().Substring(2, 2)
        Else
            txtFrom.Text = previousMonth.ToString(+"/01/" + previousYear.ToString().Substring(2, 2))
        End If

        If currentMonth < 10 Then
            txtTo.Text = "0" + currentMonth.ToString() + "/01/" + currentYear.ToString().Substring(2, 2)
        Else
            txtTo.Text = currentMonth.ToString() + "/01/" + currentYear.ToString().Substring(2, 2)
        End If


        'uk dates
        'txtFrom.Text = "01/" + previousMonth.ToString() + "/" + previousYear.ToString()
        'txtTo.Text = "01/" + currentMonth.ToString() + "/" + currentYear.ToString()
    End Sub
    Private Sub populateBossRef()
        Using New clslogger(log, className, "populateBossCode")

            ddClients.Items.Clear()
            ddClients.Items.Add(New ListItem("Please select", ""))

            ddClients.Items.Add(New ListItem("All Clients (Air Travel Only)", "10"))
            ddClients.Items.Add(New ListItem("All", "999"))
            ddClients.Items.Add(New ListItem("Internal List", "9"))
            ddClients.Items.Add(New ListItem("ANCHOR / HERECC  / UOY / RCN", "9999"))
            Dim conn As New SqlConnection
            If conn.State = ConnectionState.Closed Then
                conn.ConnectionString = (getConfig("connectionString"))
            End If

            Try
                conn.Open()
                Dim sqlquery As String = "select c.ClientID as ClientID, "
                'sqlquery += " c.BossCode + ' Air=' + convert(VARCHAR(4),a.AirPercentage) + ' Rail=' + convert(VARCHAR(4),a.RailPercentage) + ' Hotel=' + convert(VARCHAR(4),a.HotelPercentage)    as BossCode from corporateFeedbackLimit a "
                'sqlquery += " left join ClientComplaintbosscode c on a.ClientID = c.ClientID "
                'sqlquery += " WHERE ((AirPercentage is not null and AirPercentage > 0 ) "
                'sqlquery += " or (RailPercentage is not null and RailPercentage > 0 ) "
                'sqlquery += " or (HotelPercentage is not null and HotelPercentage > 0 ));"

                'sqlquery = "with firstresults as (select c.ClientID as ClientID, convert(VARCHAR(10),c.ClientID) + ' '+ c.BossCode + ' Air=' + convert(VARCHAR(4),a.AirPercentage) "
                'sqlquery += " + ' Rail=' + convert(VARCHAR(4),a.RailPercentage) + ' Hotel=' + convert(VARCHAR(4),a.HotelPercentage) as BossCode "
                'sqlquery += " ,c.ClientComplaintBossCodeID as sub from corporateFeedbackLimit a  inner join ClientComplaintbosscode c on a.ClientID = c.ClientID  "
                'sqlquery += " WHERE((AirPercentage Is Not null And AirPercentage > 0) Or (RailPercentage Is Not null And RailPercentage > 0) Or (HotelPercentage Is Not null And HotelPercentage > 0)))"
                'sqlquery += " select ClientID, Max(BossCode) as bosscode,count(BossCode),MIN(sub) as Groupmembers from firstresults   group by ClientID"


                sqlquery = "select c.ClientID,  convert(VARCHAR(8),c.ClientID) + ' ' + c.ClientName "
                sqlquery += "+  ' Air=' + convert(VARCHAR(4),cf.AirPercentage) "
                sqlquery += "+ ' Rail=' + convert(VARCHAR(4),cf.RailPercentage) + ' Hotel=' + convert(VARCHAR(4),cf.HotelPercentage) as BossCode "
                sqlquery += " from ClientComplaint c 	  join corporateFeedbackLimit cf on c.ClientID = cf.ClientID"
                sqlquery += " group by c.ClientID, c.ClientName "
                sqlquery += ",cf.AirPercentage,cf.RailPercentage,cf.HotelPercentage order by c.ClientName "


                log.Info(sqlquery)
                Dim data As SqlDataReader
                Dim adapter As New SqlDataAdapter
                Dim parameter As New SqlParameter
                Dim command As SqlCommand = New SqlCommand(sqlquery, conn)
                'With command.Parameters
                '    .Add(New SqlParameter("@user", password_user.Text))
                'End With
                command.Connection = conn
                adapter.SelectCommand = command
                data = command.ExecuteReader()
                While data.Read
                    If data.HasRows = True Then
                        Dim newListItem As ListItem

                        newListItem = New ListItem(data.Item("BossCode").ToString(), data.Item("ClientID").ToString())
                        ddClients.Items.Add(newListItem)
                    End If

                End While
            Catch ex As Exception

            End Try


        End Using
    End Sub

    Protected Sub butFeedbackEmails_Click(sender As Object, e As EventArgs) Handles butFeedbackEmails.Click

        CountTestMail = 0
        lstAir.Items.Clear()
        lstRail.Items.Clear()

        lstHotel.Items.Clear()
        lstBoss.Items.Clear()

        lstAirSel.Items.Clear()
        lstRailSel.Items.Clear()
        lstHotelSel.Items.Clear()
        'checkCorporateFeedback()
        SingleOrAll()
        ' lstParam.DataBind()
        lstAir.DataBind()
        lstRail.DataBind()
        lstHotel.DataBind()
        lstBoss.DataBind()
        CountTestMail = 0

    End Sub
    Private Sub SingleOrAll()
        Using New clslogger(log, className, "SingleOrAll")
            ' Try

            If ddClients.SelectedValue = "" Then
                ' btnSendEmail.Visible = False
                ' Throw New EvoFriendlyException("Please select from the client list.", "Info")
            Else
                Select Case ddClients.SelectedValue

                    Case "10" 'all clients Air Only
                        Dim oClients As New List(Of clsCorporateFeedbackLimitDat)
                        oClients = clsCorporateFeedbackLimitDat.listWithValuesAM
                        For Each oClient As clsCorporateFeedbackLimitDat In oClients
                            checkCorporateFeedbackOne(oClient)
                        Next

                    Case "9" 'internal
                        Dim oClients As New List(Of clsCorporateFeedbackLimitDat)
                        oClients = clsCorporateFeedbackLimitDat.listWithValuesAM
                        For Each oClient As clsCorporateFeedbackLimitDat In oClients
                            Select Case oClient.ClientID


                                'Case 100000
                                '    checkCorporateFeedbackOne(oClient)
                                'Case 100007
                                '    checkCorporateFeedbackOne(oClient)
                                Case 100011
                                    checkCorporateFeedbackOne(oClient)
                                Case 100024
                                    checkCorporateFeedbackOne(oClient)
                                Case 100027
                                    checkCorporateFeedbackOne(oClient)
                                Case 100030
                                    checkCorporateFeedbackOne(oClient)


                                    'Case 100032 'Sharp
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100033 'HOMEGROUP
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100036 'UOY
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100037 'RCN
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100038
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100042
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100044
                                    '    checkCorporateFeedbackOne(oClient)

                                    'Case 100046
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100047
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100048
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100049
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100050
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100051
                                    '    checkCorporateFeedbackOne(oClient)







                                    'Case 100053
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100054
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100055
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100057
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100058
                                    '    checkCorporateFeedbackOne(oClient)

                                    '    '---------------------------------------------------
                                    'Case 100059
                                    '    checkCorporateFeedbackOne(oClient)
                                    'Case 100060
                                    '    checkCorporateFeedbackOne(oClient)
                            End Select

                        Next

                    Case "999" 'all
                        Dim oClients As New List(Of clsCorporateFeedbackLimitDat)
                        oClients = clsCorporateFeedbackLimitDat.listWithValuesAM
                        For Each oClient As clsCorporateFeedbackLimitDat In oClients
                            checkCorporateFeedbackOne(oClient)
                        Next


                    Case "9999" 'selected
                        Dim oClients As New List(Of clsCorporateFeedbackLimitDat)
                        oClients = clsCorporateFeedbackLimitDat.listWithValuesAM
                        For Each oClient As clsCorporateFeedbackLimitDat In oClients
                            Select Case oClient.ClientID
                                Case 100000 'ANCHOR
                                    checkCorporateFeedbackOne(oClient)
                                Case 100011 'HERECC
                                    checkCorporateFeedbackOne(oClient)
                                Case 100007 'UOY
                                    checkCorporateFeedbackOne(oClient)
                                Case 100033 'RCN
                                    checkCorporateFeedbackOne(oClient)
                            End Select

                        Next
                    Case Else

                        Dim oClients As New List(Of clsCorporateFeedbackLimitDat)
                        oClients = clsCorporateFeedbackLimitDat.listWithValuesAM
                        Dim HasLimits As Boolean = False
                        For Each oClient As clsCorporateFeedbackLimitDat In oClients
                            If oClient.ClientID = ddClients.SelectedValue Then
                                checkCorporateFeedbackOne(oClient)
                                HasLimits = True
                            End If
                        Next
                        If Not HasLimits Then
                            lstBoss.Items.Add("No feedback limits set for " + ddClients.SelectedItem.Text)
                        End If

                End Select


            End If
            '  Catch ex As Exception
            '     log.Error(ex.Message.ToString() & " " & ex.InnerException.ToString())
            'handleexception(ex, "SingleOrAll", Me.Page)
            'End Try
        End Using
    End Sub


    Private Sub checkCorporateFeedbackOne(ByVal client As clsCorporateFeedbackLimitDat)
        Dim strBossIDs As String = ""
        strBossIDs = clsCorporateFeedbackLimitDat.getClientBossCodes(client.ClientID)

        Dim strBossIdWhereQuotes As String = "OR"
        Dim ss As String = strBossIDs + " Air=" + client.AirPercentage.ToString() + "(" + client.SendAfterAir.ToString() + "-" + client.AirLastInvoice.ToString + ") Rail=" + client.RailPercentage.ToString() + "(" + client.SendAfterRail.ToString() + "-" + client.RailLastInvoice.ToString + ") Hotel=" + client.HotelPercentage.ToString() + "(" + client.SendAfterHotel.ToString() + "-" + client.HotelLastInvoice.ToString + ")"
        lstBoss.Items.Add(ss)
        log.Info(ss)
        '


        If strBossIDs IsNot Nothing AndAlso strBossIDs <> "" Then
            Dim strBossIdWhere As String = ""

            If strBossIDs.Contains(",") Then
                Dim strSplitIds As String() = strBossIDs.Split(",")
                Dim x As Integer = 0
                For x = 0 To strSplitIds.Length - 1
                    If strSplitIds(x).Trim <> "" Then
                        If strBossIdWhere <> "" Then
                            strBossIdWhere &= " or "
                        End If
                        strBossIdWhere &= "inm_custid LIKE '" & strSplitIds(x).Trim & "'"
                        strBossIdWhereQuotes &= "inm_custid LIKE '" & strSplitIds(x).Trim & "'"
                    End If
                Next
            Else
                strBossIdWhere = "inm_custid LIKE '" & strBossIDs.Trim & "'"
                strBossIdWhereQuotes = "inm_custid LIKE ''" & strBossIDs.Trim & "'''"
            End If

            Dim percent As Double
            percent = ddlPercent.SelectedValue

            If chkAir.Checked Then
                If client.AirPercentage > 0 Then
                    log.Info("--------------------------------------- AIR for  " + strBossIDs + " ----------------------------------------------")
                    Dim SendAfter As Integer = 0
                    If (client.SendAfterHotel < 0) Then
                        SendAfter = 0
                    Else
                        SendAfter = client.SendAfterAir
                    End If
                    If percent = 999 Then
                        CheckCorporateInvoices(strBossIdWhere, client.ClientID, "A", client.AirPercentage, client.AirLastInvoice, SendAfter, strBossIdWhereQuotes)
                    Else
                        ' Test threshold at 0 and 50% of invoices
                        CheckCorporateInvoices(strBossIdWhere, client.ClientID, "A", percent, client.AirLastInvoice, 0, strBossIdWhereQuotes)
                    End If
                    log.Info("-------------------------------------------------------------------------------------------------------------------------")
                End If
            End If

            If chkRail.Checked Then
                If client.RailPercentage > 0 Then
                    log.Info("--------------------------------------- RAIL for  " + strBossIDs + " ----------------------------------------------")
                    Dim SendAfter As Integer = 0
                    If (client.SendAfterHotel < 0) Then
                        SendAfter = 0
                    Else
                        SendAfter = client.SendAfterRail
                    End If
                    If percent = 999 Then
                        CheckCorporateInvoices(strBossIdWhere, client.ClientID, "RR", client.RailPercentage, client.RailLastInvoice, SendAfter, strBossIdWhereQuotes)
                    Else
                        CheckCorporateInvoices(strBossIdWhere, client.ClientID, "RR", percent, client.RailLastInvoice, 0, strBossIdWhereQuotes)
                    End If
                    log.Info("-------------------------------------------------------------------------------------------------------------------------")
                End If
            End If

            If chkHotel.Checked Then
                If client.HotelPercentage > 0 Then
                    log.Info("--------------------------------------- HOTEL for  " + strBossIDs + " ----------------------------------------------")
                    Dim SendAfter As Integer = 0
                    If (client.SendAfterHotel < 0) Then
                        SendAfter = 0
                    Else
                        SendAfter = client.SendAfterHotel
                    End If

                    If percent = 999 Then
                        CheckCorporateInvoices(strBossIdWhere, client.ClientID, "H", client.HotelPercentage, client.HotelLastInvoice, SendAfter, strBossIdWhereQuotes)
                    Else
                        CheckCorporateInvoices(strBossIdWhere, client.ClientID, "H", percent, client.HotelLastInvoice, 0, strBossIdWhereQuotes)
                    End If
                    log.Info("-------------------------------------------------------------------------------------------------------------------------")
                End If
            End If
        End If

    End Sub


    Private Sub checkCorporateFeedback()


        'Get all clients set up for corporate feedback sending
        Dim oClients As New List(Of clsCorporateFeedbackLimitDat)
        oClients = clsCorporateFeedbackLimitDat.listWithValues

        countRail = 0
        countAir = 0
        countHotel = 0

        For Each oClient As clsCorporateFeedbackLimitDat In oClients
            Dim strBossIDs As String = ""
            strBossIDs = clsCorporateFeedbackLimitDat.getClientBossCodes(oClient.ClientID)

            Dim strBossIdWhereQuotes As String = "OR"
            Dim ss As String = strBossIDs + " Air=" + oClient.AirPercentage.ToString() + "(" + oClient.SendAfterAir.ToString() + "-" + oClient.AirLastInvoice.ToString + ") Rail=" + oClient.RailPercentage.ToString() + "(" + oClient.SendAfterRail.ToString() + "-" + oClient.RailLastInvoice.ToString + ") Hotel=" + oClient.HotelPercentage.ToString() + "(" + oClient.SendAfterHotel.ToString() + "-" + oClient.HotelLastInvoice.ToString + ")"
            lstBoss.Items.Add(ss)
            log.Info(ss)
            '


            If strBossIDs IsNot Nothing AndAlso strBossIDs <> "" Then
                Dim strBossIdWhere As String = ""

                If strBossIDs.Contains(",") Then
                    Dim strSplitIds As String() = strBossIDs.Split(",")
                    Dim x As Integer = 0
                    For x = 0 To strSplitIds.Length - 1
                        If strSplitIds(x).Trim <> "" Then
                            If strBossIdWhere <> "" Then
                                strBossIdWhere &= " or "
                            End If
                            strBossIdWhere &= "inm_custid LIKE '" & strSplitIds(x).Trim & "'"
                            strBossIdWhereQuotes &= "inm_custid LIKE '" & strSplitIds(x).Trim & "'"
                        End If
                    Next
                Else
                    strBossIdWhere = "inm_custid LIKE '" & strBossIDs.Trim & "'"
                    strBossIdWhereQuotes = "inm_custid LIKE ''" & strBossIDs.Trim & "'''"
                End If

                Dim percent As Double
                percent = ddlPercent.SelectedValue


                If oClient.AirPercentage > 0 Then
                    log.Info("--------------------------------------- AIR for  " + strBossIDs + " ----------------------------------------------")
                    If percent = 999 Then
                        CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "A", oClient.AirPercentage, oClient.AirLastInvoice, oClient.SendAfterAir, strBossIdWhereQuotes)
                    Else
                        ' Test threshold at 0 and 50% of invoices
                        CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "A", percent, oClient.AirLastInvoice, 0, strBossIdWhereQuotes)
                    End If
                    log.Info("-------------------------------------------------------------------------------------------------------------------------")
                End If

                If oClient.RailPercentage > 0 Then
                    log.Info("--------------------------------------- RAIL for  " + strBossIDs + " ----------------------------------------------")
                    If percent = 999 Then
                        CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "RR", oClient.RailPercentage, oClient.RailLastInvoice, oClient.SendAfterRail, strBossIdWhereQuotes)
                    Else
                        CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "RR", percent, oClient.RailLastInvoice, 0, strBossIdWhereQuotes)
                    End If
                    log.Info("-------------------------------------------------------------------------------------------------------------------------")
                End If

                If oClient.HotelPercentage > 0 Then
                    log.Info("--------------------------------------- HOTEL for  " + strBossIDs + " ----------------------------------------------")
                    If percent = 999 Then
                        CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "H", oClient.HotelPercentage, oClient.HotelLastInvoice, oClient.SendAfterHotel, strBossIdWhereQuotes)
                    Else
                        CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "H", percent, oClient.HotelLastInvoice, 0, strBossIdWhereQuotes)
                    End If
                    log.Info("-------------------------------------------------------------------------------------------------------------------------")
                End If
            End If
        Next
        Session("countAir") = countAir
        Session("countRail") = countRail
        Session("countHotel") = countHotel
        'Session("list") = list
    End Sub
    Public Class EmailPossibles

        Public Property SourceCode As String
        Public Property ProductCode As String
        Public Property BossRef As String
        Public Property Name As String
        Public Property Email As String

        Public Property BookerFirstname As String
        Public Property BookerLastname As String
        Public Property Invoice As String

        Public Property LeadName As String
        Public Property StartDate As String
        Public Property Client As String

    End Class

    Dim list As List(Of EmailPossibles) = New List(Of EmailPossibles)

    Dim countRail As Integer = 0
    Dim countAir As Integer = 0
    Dim countHotel As Integer = 0




    Private Sub CheckCorporateInvoices(ByVal pstrClientBossID As String, _
                                     ByVal pintClientID As Integer, _
                                     ByVal pstrProductCode As String, _
                                     ByVal pdblPercentage As Double, _
                                     ByVal pstrLastInvoiceNoProcessed As String, _
                                     ByVal pintSendAfter As Integer,
                                     ByVal strBossIdWhereQuotes As String)

        Dim list As List(Of EmailPossibles) = New List(Of EmailPossibles)





        Dim dtNow As Date = Now()
        Dim pstrDateTo As String = dtNow.ToString("MM/dd/yy") '"03/15/14"
        dtNow = dtNow.AddMonths(-1)
        Dim pstrDate As String = dtNow.ToString("MM/dd/yy") '"03/15/14"

        If txtFrom.Text.Length = 8 And txtTo.Text.Length = 8 Then
            pstrDate = txtFrom.Text
            pstrDateTo = txtTo.Text
        End If

 
        Dim iInvoices As New List(Of clsInvoicingDat)
        iInvoices = clsInvoicingDat.checkFeedbackInvoiceAM(pstrClientBossID, pstrProductCode, pstrDate, pstrDateTo)
        Dim oInvoices As New List(Of clsInvoicingDat)
        Dim totalbook As Integer = iInvoices.Count
        Dim totalcandidate As Integer = 0
        'too many invoices will cause this to slow, take sample when above this level
        Dim Reduce As Boolean = getConfig("TooManyInvoices") < iInvoices.Count
        If Reduce Then
            Dim modulo As Integer = 2
            Select Case iInvoices.Count
                Case Is > 600
                    modulo = getConfig("TooManyInvoicesModulo600")
                Case Is > 500
                    modulo = getConfig("TooManyInvoicesModulo500")
                Case Is > 400
                    modulo = getConfig("TooManyInvoicesModulo400")
                Case Is > 300
                    modulo = getConfig("TooManyInvoicesModulo300")
                Case Is > 200
                    modulo = getConfig("TooManyInvoicesModulo200")
                Case Is > 100
                    modulo = getConfig("TooManyInvoicesModulo100")

            End Select

            Dim msg As String = pstrProductCode + " - Invoice limit of " + getConfig("TooManyInvoices") + " exceded (actually" + iInvoices.Count.ToString() + ")  reducing sample size by modulo " + modulo.ToString()
            log.Info(msg)
            lstBoss.Items.Add(msg)
            Dim i As Integer = 0
            Dim l As Integer = 0
            Dim limit As Integer = getConfig("CorporateFeedbackInvoiceCapLimit")
            For Each oInvoice As clsInvoicingDat In iInvoices
                If i Mod modulo = 0 Then
                    If i < limit Then
                        oInvoices.Add(oInvoice)
                        l = +1
                    End If
                End If
                i += 1
            Next
        Else
            oInvoices = iInvoices
        End If



        lstBoss.Items.Add(pstrProductCode + " invoices = " + oInvoices.Count().ToString() + ", print after limit of " + pintSendAfter.ToString())
        'Work out how many emails we need to send as a whole int
        Dim intTotalEmailsToSend As Integer = 0


        log.Info("exec boss_corporateFeedbackInvoicesAM '" + strBossIdWhereQuotes + "', '" + pstrProductCode + "', '" + pstrDate + "'")
        'exec boss_corporateFeedbackInvoices 'inm_custid LIKE ''ANCHOR''', 'A', 'B134424'
        'R2.5 CR - use passed in param pintSendAfter instead of hard-coded 50
        If totalbook >= pintSendAfter Then
            'lstBoss.Items.Add(pstrProductCode + " Over threshold of  " + oInvoices.Count.ToString())
            log.Info(pstrProductCode + " Over threshold of  " + oInvoices.Count.ToString())

            Dim intTotalInvoices As Integer = oInvoices.Count
            Dim strLastInvoiceID As String = ""
            intTotalEmailsToSend = Math.Ceiling((intTotalInvoices / 100) * pdblPercentage)

            'Dim list As List(Of EmailPossibles) = New List(Of EmailPossibles)
            Dim possibles As Integer = 0
            Dim SourceCode As String = "NF"
            Select Case pstrProductCode
                Case "RR"
                    countRail = 0
                    For Each oInvoice As clsInvoicingDat In oInvoices
                        'log.Info("Rail " + oInvoice.mstrinvoicenumber)
                        If countRail < getConfig("CorporateFeedbackCandidateCapLimit") Then

                            'trace a particular invoice
                            'If (oInvoice.mstrinvoicenumber = "B127609") Then
                            '    Dim Stophere As Boolean = True
                            'End If


                            Dim strBookerFirstname As String = ""
                            Dim strBookerLastname As String = ""
                            Dim strName As String = ""
                            Dim strTo As String = ""

                            Dim find As Boolean = False
                            Dim localCustRef2count As Integer = 0

                            '1 find by cust ref2
                            'Dim fullname As String = clsCorporateFeedbackDat.get_invtot_csref2(oInvoice.mstrinvoicenumber)
                            Dim fullname As String = oInvoice.mstrCommissionDocRef

                            If oInvoice.mstrbossid = "HOME" Then
                                fullname = oInvoice.mstrcostcode
                            End If


                            If fullname <> "" Then 'can attempt SSO nearly all AIR found this way!!
                                Dim sValueAsArray = fullname.ToCharArray()
                                If Not IsNumeric(sValueAsArray(0)) Then
                                    localCustRef2count += 1
                                    formatBookerNameCREF2(fullname, strBookerFirstname, strBookerLastname)

                                    Dim oBookerDetails As New clsCorporateFeedbackDat
                                    oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname, oInvoice.mstrbossid)
                                    If oBookerDetails.mstrBookerEmail <> "" Then
                                        SourceCode = "CREF2  "
                                        strTo = oBookerDetails.mstrBookerEmail
                                        find = True
                                    End If
                                End If
                            End If

                            If Not find Then
                                Dim oEvolviContact As clsEvolviData
                                oEvolviContact = clsEvolviData.getBooker(notInteger(oInvoice.mstrboss))
                                ' search on booking agent firstname/lastname - get back bookingagentemailaddress

                                If oEvolviContact.BookerEmail <> "" Then
                                    SourceCode = "Evolvi "
                                    strTo = oEvolviContact.BookerEmail
                                    formatBookerName(oEvolviContact.BookerName, strBookerFirstname, strBookerLastname)
                                    strName = oEvolviContact.BookerName
                                    find = True
                                ElseIf oEvolviContact.BookerName <> "" Then
                                    'Not found an email address but has found a name, so check SSO for possible email address
                                    formatBookerName(oEvolviContact.BookerName, strBookerFirstname, strBookerLastname)
                                    strName = oEvolviContact.BookerName
                                    Dim oBookerDetails As New clsCorporateFeedbackDat
                                    oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname, oInvoice.mstrbossid)
                                    SourceCode = "Evolvi to SSO "
                                    strTo = oBookerDetails.mstrBookerEmail
                                    find = True
                                    If oBookerDetails.mstrBookerName.Trim <> "" Then
                                        strBookerFirstname = oBookerDetails.mstrBookerName.Substring(0, oBookerDetails.mstrBookerName.IndexOf(" "))
                                        strBookerLastname = oBookerDetails.mstrBookerName.Substring(oBookerDetails.mstrBookerName.IndexOf(" ") + 1)
                                    End If
                                    oBookerDetails = Nothing
                                End If
                                oEvolviContact = Nothing
                            End If


                            If find Then
                                If clsCorporateFeedbackBlacklist.checkEmail(strTo) = 0 Then
                                    Dim EmailPossibles = New EmailPossibles
                                    EmailPossibles.SourceCode = SourceCode
                                    EmailPossibles.Name = strBookerFirstname + " " + strBookerLastname
                                    EmailPossibles.BookerFirstname = strBookerFirstname
                                    EmailPossibles.BookerLastname = strBookerLastname

                                    EmailPossibles.ProductCode = pstrProductCode
                                    EmailPossibles.Email = strTo
                                    EmailPossibles.BossRef = oInvoice.mstrinvoicenumber

                                    EmailPossibles.LeadName = oInvoice.mstrleadname
                                    EmailPossibles.StartDate = oInvoice.mstrstartdate
                                    EmailPossibles.Client = pintClientID
                                    EmailPossibles.Invoice = oInvoice.mstrinvoicenumber
                                    list.Add(EmailPossibles)
                                    possibles += 1
                                    countRail += 1
                                    lstRail.Items.Add(SourceCode + " " + strTo + " " + oInvoice.mstrinvoicenumber)
                                Else
                                    lstRail.Items.Add("BL" + strTo)
                                End If
                            End If
                            ' oEvolviContact = Nothing
                        End If
                    Next

                    totalcandidate = countRail
                    lstBoss.Items.Add("Rail Possibles = " + countRail.ToString() + " capped limit " + getConfig("CorporateFeedbackCandidateCapLimit"))
                    log.Info("Rail Possibles = " + countRail.ToString() + " capped limit " + getConfig("CorporateFeedbackCandidateCapLimit"))
                    log.Info(pstrProductCode + " Total  " + oInvoices.Count.ToString() + ", Emails to send " + intTotalEmailsToSend.ToString())

                Case "H"
                    countHotel = 0
                    For Each oInvoice As clsInvoicingDat In oInvoices
                        'log.Info("Hotel " + oInvoice.mstrinvoicenumber)
                        If countHotel < getConfig("CorporateFeedbackCandidateCapLimit") Then
                            'trace a particular invoice
                            'If (oInvoice.mstrinvoicenumber = "B112107") Then
                            '    Dim Stophere As Boolean = True
                            'End If

                            Dim strBookerFirstname As String = ""
                            Dim strBookerLastname As String = ""
                            Dim strTo As String = ""
                            Dim find As Boolean = False
                            Dim localCustRef2count As Integer = 0



                            '1 find by cust ref2
                            'Dim fullname As String = clsCorporateFeedbackDat.get_invtot_csref2(oInvoice.mstrinvoicenumber)
                            Dim fullname As String = oInvoice.mstrCommissionDocRef

                            If oInvoice.mstrbossid = "HOME" Then
                                fullname = oInvoice.mstrcostcode
                            End If

                            If fullname <> "" Then
                                Dim sValueAsArray = fullname.ToCharArray()
                                If Not IsNumeric(sValueAsArray(0)) Then
                                    localCustRef2count += 1
                                    formatBookerNameCREF2(fullname, strBookerFirstname, strBookerLastname)
                                    Dim oBookerDetails As New clsCorporateFeedbackDat
                                    oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname, oInvoice.mstrbossid)

                                    If oBookerDetails.mstrBookerEmail <> "" Then
                                        SourceCode = "CREF2 "
                                        strTo = oBookerDetails.mstrBookerEmail
                                        find = True
                                    End If
                                    oBookerDetails = Nothing
                                End If
                            End If


                            If Not find Then
                                Dim oCubitContact As New clsCorporateFeedbackDat
                                'oCubitContact = clsCorporateFeedbackDat.getCubitBooker(oInvoice.mstrinvoicenumber, oInvoice.mstrboss.Substring(1, oInvoice.mstrinvoicenumber.Length - 1)) ': "c140861")
                                If oInvoice.mstrboss <> "" Then
                                    If oInvoice.mstrboss.Substring(0, 1) = "C" Or oInvoice.mstrboss.Substring(0, 1) = "c" Then
                                        If Regex.IsMatch(oInvoice.mstrboss.Substring(1, oInvoice.mstrboss.Length - 1), "^[0-9]+$") Then
                                            oCubitContact = clsCorporateFeedbackDat.getCubitBooker(oInvoice.mstrinvoicenumber, oInvoice.mstrboss.Substring(1, oInvoice.mstrboss.Length - 1)) ': "c140861")
                                            '  log.Info("exec corporateFeedback_CubitBooker " + oInvoice.mstrinvoicenumber + "," + oInvoice.mstrboss.Substring(1, oInvoice.mstrinvoicenumber.Length - 1))
                                            If oCubitContact.mstrBookerEmail <> "" Then
                                                fullname = oCubitContact.mstrBookerName
                                                'formatBookerName(oCubitContact.mstrBookerName, strBookerFirstname, strBookerLastname)
                                                SourceCode = "CUBIT "
                                                strTo = oCubitContact.mstrBookerEmail
                                                find = True
                                            Else 'cannot find email so try get name for SSO
                                                If oCubitContact.mstrBookerName <> "" Then
                                                    formatBookerName(oCubitContact.mstrBookerName, strBookerFirstname, strBookerLastname)
                                                    Dim oBookerDetails As New clsCorporateFeedbackDat
                                                    oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname, oInvoice.mstrbossid)

                                                    If oBookerDetails.mstrBookerEmail <> "" Then
                                                        SourceCode = "CUBIT to SSO "
                                                        strTo = oBookerDetails.mstrBookerEmail
                                                        find = True
                                                    End If
                                                    oBookerDetails = Nothing
                                                End If
                                            End If
                                            oCubitContact = Nothing

                                        End If
                                    End If
                                End If
                            End If


                            If find Then
                                If clsCorporateFeedbackBlacklist.checkEmail(strTo) = 0 Then
                                    Dim EmailPossibles = New EmailPossibles
                                    EmailPossibles.SourceCode = SourceCode
                                    EmailPossibles.Name = fullname
                                    EmailPossibles.BookerFirstname = strBookerFirstname
                                    EmailPossibles.BookerLastname = strBookerLastname

                                    EmailPossibles.ProductCode = pstrProductCode
                                    EmailPossibles.Email = strTo
                                    EmailPossibles.BossRef = oInvoice.mstrinvoicenumber

                                    EmailPossibles.LeadName = oInvoice.mstrleadname
                                    EmailPossibles.StartDate = oInvoice.mstrstartdate
                                    EmailPossibles.Client = pintClientID
                                    EmailPossibles.Invoice = oInvoice.mstrinvoicenumber
                                    list.Add(EmailPossibles)
                                    possibles += 1
                                    countHotel += 1
                                    lstHotel.Items.Add(SourceCode + " " + strTo + " " + oInvoice.mstrinvoicenumber)

                                Else
                                    lstHotel.Items.Add("BL" + strTo)
                                End If
                            End If

                        End If

                    Next
                    totalcandidate = countHotel
                    lstBoss.Items.Add("Hotel Possibles = " + countHotel.ToString())
                    log.Info("Hotel Possibles = " + countHotel.ToString() + " capped limit " + getConfig("CorporateFeedbackCandidateCapLimit"))
                    log.Info(pstrProductCode + " Total  " + oInvoices.Count.ToString() + ", Emails to send " + intTotalEmailsToSend.ToString() + " capped limit " + getConfig("CorporateFeedbackCandidateCapLimit"))

                Case "A"
                    countAir = 0
                    For Each oInvoice As clsInvoicingDat In oInvoices
                        'log.Info("Air " + oInvoice.mstrinvoicenumber)
                        If countAir < getConfig("CorporateFeedbackCandidateCapLimit") Then
                            Dim strBookerFirstname As String = ""
                            Dim strBookerLastname As String = ""
                            Dim strTo As String = ""

                            Dim localCustRef2count As Integer = 0


                            Dim find As Boolean = False

                            '1 find by cust ref2
                            'Dim fullname As String = clsCorporateFeedbackDat.get_invtot_csref2(oInvoice.mstrinvoicenumber)
                            Dim fullname As String = oInvoice.mstrCommissionDocRef


                            If oInvoice.mstrbossid = "HOME" Then
                                fullname = oInvoice.mstrcostcode
                            End If

                            If fullname <> "" Then
                                Dim sValueAsArray = fullname.ToCharArray()
                                If Not IsNumeric(sValueAsArray(0)) Then
                                    localCustRef2count += 1
                                    formatBookerNameCREF2(fullname, strBookerFirstname, strBookerLastname)
                                Else 'if you cannot find email via this method reset
                                    fullname = ""
                                End If
                            End If


                            If fullname = "" Then
                                Dim oGIDS As New clsCorporateFeedbackDat
                                oGIDS = clsCorporateFeedbackDat.getGIDSBooker(oInvoice.mstrboss)
                                If oGIDS.mstrBookerName <> "" Then
                                    fullname = oGIDS.mstrBookerName
                                    ' formatBookerName(oGIDS.mstrBookerName, strBookerFirstname, strBookerLastname)
                                    formatBookerNameCREF2(fullname, strBookerFirstname, strBookerLastname)
                                    Dim oBookerDetails As New clsCorporateFeedbackDat
                                    oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname, oInvoice.mstrbossid)
                                    If oBookerDetails.mstrBookerEmail <> "" Then
                                        SourceCode = "GIDS "
                                        strTo = oBookerDetails.mstrBookerEmail
                                        find = True
                                        If oBookerDetails.mstrBookerName.Trim <> "" Then
                                            strBookerFirstname = oBookerDetails.mstrBookerName.Substring(0, oBookerDetails.mstrBookerName.IndexOf(" "))
                                            strBookerLastname = oBookerDetails.mstrBookerName.Substring(oBookerDetails.mstrBookerName.IndexOf(" ") + 1)
                                        End If
                                        oBookerDetails = Nothing
                                    End If
                                End If
                            End If


                            If fullname <> "" Then 'can attempt SSO nearly all AIR found this way!!
                                Dim oBookerDetails As New clsCorporateFeedbackDat
                                oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname, oInvoice.mstrbossid)
                                If oBookerDetails.mstrBookerEmail <> "" Then
                                    SourceCode = "CREF2 "
                                    strTo = oBookerDetails.mstrBookerEmail
                                    find = True
                                Else

                                End If

                            End If



                            'if not found try GIDS
                            If Not find Then
                                Dim oGIDS As New clsCorporateFeedbackDat
                                oGIDS = clsCorporateFeedbackDat.getGIDSBooker(oInvoice.mstrboss)
                                fullname = oGIDS.mstrBookerName
                                If fullname <> "" Then
                                    Dim email As String = fullname.Replace("//", "@")

                                    If Left(email, 13) = "EMAIL ADDRESS" Then
                                        email = email.Replace("EMAIL ADDRESS", "")
                                    End If
                                    If Left(email, 1) = ":" Then
                                        email = email.Replace(":", "")
                                    End If
                                    If Left(email, 6) = "EMAIL " Then
                                        email = email.Replace("EMAIL ", "")
                                    End If

                                    email = email.Trim()
                                    'formatBookerName(oGIDS.mstrBookerName, strBookerFirstname, strBookerLastname)
                                    SourceCode = "GIDS Notes "
                                    strTo = email ' oGIDS.mstrBookerEmail
                                    find = True
                                End If

                                oGIDS = Nothing
                            End If

                            'lstBoss.Items.Add("CurRef2 attempts = " + localCustRef2count.ToString())

                            If find Then
                                If clsCorporateFeedbackBlacklist.checkEmail(strTo) = 0 Then
                                    formatBookerName(fullname, strBookerFirstname, strBookerLastname)
                                    Dim EmailPossibles = New EmailPossibles
                                    EmailPossibles.SourceCode = SourceCode
                                    EmailPossibles.Name = fullname
                                    EmailPossibles.BookerFirstname = strBookerFirstname
                                    EmailPossibles.BookerLastname = strBookerLastname

                                    EmailPossibles.ProductCode = pstrProductCode
                                    EmailPossibles.Email = strTo
                                    EmailPossibles.BossRef = oInvoice.mstrinvoicenumber

                                    EmailPossibles.LeadName = oInvoice.mstrleadname
                                    EmailPossibles.StartDate = oInvoice.mstrstartdate
                                    EmailPossibles.Invoice = oInvoice.mstrinvoicenumber
                                    EmailPossibles.Client = pintClientID
                                    list.Add(EmailPossibles)
                                    possibles += 1
                                    countAir += 1
                                    lstAir.Items.Add(SourceCode + " " + strTo + " " + oInvoice.mstrinvoicenumber)
                                Else
                                    lstAir.Items.Add("BL" + strTo)
                                End If
                            End If
                        End If


                    Next
                    totalcandidate = countAir
                    lstBoss.Items.Add("Air Possibles = " + countAir.ToString() + " capped limit " + getConfig("CorporateFeedbackCandidateCapLimit"))
                    log.Info("Air Possibles = " + countAir.ToString() + " capped limit " + getConfig("CorporateFeedbackCandidateCapLimit"))
                    log.Info(pstrProductCode + " Total  " + oInvoices.Count.ToString() + ", Emails to send " + intTotalEmailsToSend.ToString())
                Case Else

            End Select

            ''---------------------------------------------------------------------------------------------------------------'

            If possibles > 0 Then
                '  GetRandoms(pstrProductCode, pdblPercentage, possibles, list)
                GetRandoms(pstrProductCode, pdblPercentage, totalbook, totalcandidate, list)
            End If

        Else
            log.Info("Not over threshold of " + pintSendAfter.ToString() + " actual invoices " + oInvoices.Count.ToString())
            lstBoss.Items.Add("Not over threshold of " + pintSendAfter.ToString() + " actual invoices " + oInvoices.Count.ToString())
        End If


    End Sub



    Private Sub GetRandoms(ByVal pstrProductCode As String, ByVal pdblPercentage As Double, ByVal intTotalInvoices As Integer, ByVal intTotalSample As Integer, ByRef list As List(Of EmailPossibles))
        'choose that many random numbers between 1 (& including) and intTotalEmailsToSend + 1 (only less than)
        Dim intTotalEmailsToSend As Integer = Math.Ceiling((intTotalInvoices / 100) * pdblPercentage)

        Dim intRandomList As New List(Of Integer)
        getRandomNumbers(1, intTotalSample + 1, intTotalEmailsToSend, intRandomList)

        Dim intCount As Integer = 0
        Dim sendCount As Integer = 0
        For Each e As EmailPossibles In list
            intCount += 1
            If intRandomList.Contains(intCount) Then
                sendCount += 1
                Select Case pstrProductCode
                    Case "RR"
                        lstRailSel.Items.Add(e.Email)

                    Case "H"
                        lstHotelSel.Items.Add(e.Email)

                    Case "A"
                        lstAirSel.Items.Add(e.Email)

                    Case Else

                End Select
                If Me.chkTest.Checked Then
                    SendEmail(e, True)
                Else
                    SendEmail(e, False)
                End If

            End If
        Next
        lstBoss.Items.Add(pstrProductCode + " send " + sendCount.ToString())
        lstAirSel.DataBind()
        lstRailSel.DataBind()
        lstHotelSel.DataBind()

    End Sub


    Dim CountTestMail As Integer = 0
    Private Sub SendEmail(ByRef EmailTo As EmailPossibles, ByVal LogRatherThanSend As Boolean)
        Dim strParams As String = ""
        Dim strBookerFullName As String = ""

        strBookerFullName = EmailTo.BookerFirstname & " " & EmailTo.BookerLastname

        strParams = "?InvoiceRef=" & EmailTo.Invoice ' oInvoice.mstrinvoicenumber
        strParams &= "&fao=" & EmailTo.BookerFirstname & " " & EmailTo.BookerLastname

        If EmailTo.BookerFirstname = "" And EmailTo.BookerLastname = "" Then
            strBookerFullName = "Sir/Madam"
        ElseIf EmailTo.BookerFirstname <> "" And EmailTo.BookerLastname = "" Then
            strBookerFullName = EmailTo.BookerFirstname
        End If

        Dim strOriginalEmail As String = ""
        Dim blnTesting As Boolean = True

        If Me.chkTest.Checked Then
            blnTesting = True
        Else
            blnTesting = False
        End If

        Dim blnFeedBacKRecord As Boolean = False

        'Dim oMail As New MailMessage(getConfig("CorporateFeedbackEmail"), "ashley.marron@nysgroup.com")
        Dim oMail As New MailMessage()
        oMail.From = New MailAddress(getConfig("CorporateFeedbackEmail"))
        If blnTesting Then
            '' oMail.To.Add("ashley.marron@nysgroup.com") 'send
            oMail.To.Add(getConfig("CorporateFeedbackEmailbcc"))
        Else

            oMail.To.Add(EmailTo.Email)
            oMail.Bcc.Add(getConfig("CorporateFeedbackEmailbcc"))
        End If


        oMail.Bcc.Add(getConfig("CorporateFeedbackEmailbcc"))
        oMail.IsBodyHtml = True
        oMail.BodyEncoding = System.Text.Encoding.Default
        oMail.Subject = "NYS Corporate Feedback Regarding Booking Ref: " & EmailTo.Invoice 'oInvoice.mstrinvoicenumber

        Dim strProductDesc As String = ""

        Select Case EmailTo.ProductCode
            Case "RR"
                strProductDesc = "Rail Travel"
            Case "H"
                strProductDesc = "Hotel Stay"
            Case "A"
                strProductDesc = "Flight booking"
            Case Else
        End Select

        strBookerFullName = "Sir/Madam" 'as default
        oMail.Body = "Reference " & EmailTo.Invoice & "<br /><br />Dear " & strBookerFullName & "<br/><br/>" & _
            "You recently made a booking through NYS Corporate, details of the travel are below.<br/><br/>" & _
            "<strong>" & strProductDesc & " for " & EmailTo.LeadName & " travelling on " & EmailTo.StartDate & "</strong><br/><br/>" & _
            "We would be grateful if you could spend a few moments of your time completing the questionnaire found by following the link below.<br/><br/><a href=""" & getConfig("CorporateFeedbackURL") & strParams & """>NYS FEEDBACK QUESTIONNAIRE</a><br/><br/>" & _
            "If the link above does not work please can you copy and paste the following into a web browser to enable you to complete the NYS Feedback questionnaire:<br />" & getConfig("CorporateFeedbackURL") & strParams & "<br/><br/>" & _
            "Please do not hesitate to contact us if you have any queries.<br/><br/>" & _
            "Kind Regards,<br /><br />" & _
            "Quality Department<br />" & _
            "NYS Corporate<br />" & _
            "Quantum House<br />" & _
            "Innovation Way<br />" & _
            "York YO10 5BR<br />" & _
            "Tel: 01904 420227<br />" & _
            "Fax: 0870 4582 757<br />"

        If blnTesting Then
            oMail.Body = "THIS IS A TEST EMAIL, original would have been sent to: " & EmailTo.Email & "<br/><br/>" & oMail.Body
        End If


        Dim oClient As New SmtpClient("office.nyscorporate.com", 25)
        If LogRatherThanSend Then
            Dim limit As Integer = CInt(Me.txtTest.Text)
            If CountTestMail < limit Then
                oClient.Send(oMail)
                'Dim oCorpFeedback As New clsCorporateFeedbackDat
                'oCorpFeedback.saveInitial(EmailTo.Invoice, EmailTo.Client)
                CountTestMail += 1
            End If
            'log.Info("----------------------------------------Email --------------------------------------------------")
            'log.Info("SourceCode " + EmailTo.SourceCode)
            'log.Info("Name " + EmailTo.Name)
            'log.Info("BookerFirstname " + EmailTo.BookerFirstname)
            'log.Info("BookerLastname " + EmailTo.BookerLastname)
            'log.Info("pstrProductCode " + EmailTo.ProductCode)
            'log.Info("Email " + EmailTo.Email)
            'log.Info("BossRef " + EmailTo.BossRef)

            'log.Info("LeadName " + EmailTo.LeadName)
            'log.Info("StartDate " + EmailTo.StartDate)
            'log.Info("Client " + EmailTo.Client)
            'log.Info(oMail.Subject)
            'log.Info(oMail.Body)

            'log.Info("----------------------------------------------------------------------------------------------")
        Else
            Try

                oClient.Send(oMail)
                log.Info("Sent to " + EmailTo.Email + " for " + strProductDesc + " Invoice " + EmailTo.Invoice)

                'save the intial feedback form to db to record send
                Dim oCorpFeedback As New clsCorporateFeedbackDat
                'Don't save if test or not real send
                Dim Entry As Integer = oCorpFeedback.saveInitial(EmailTo.Invoice, EmailTo.Client)
                log.Info("Entry " + Entry.ToString() + " in table corporateFeedback ")


            Catch ex As Exception
                log.Error("Failed to send " + ex.Message)
            End Try
        End If

    End Sub


    Private Sub getRandomNumbers(ByVal pintMinNumber As Integer, ByVal pintMaxNumber As Integer, ByVal intTotalNumbersNeeded As Integer, ByRef lstRandomNumbers As List(Of Integer))
        Dim rdmNumber As New Random
        Dim x As Integer = 0

        'First check to see if there any numbers remaining that are unselected, need to do this to avoid infinate loop
        Dim a As Integer = 0
        Dim blnRemainingFreeNumbers As Boolean = False
        For a = pintMinNumber To pintMaxNumber - 1
            If a > pintMinNumber Then
                If Not lstRandomNumbers.Contains(a) Then
                    blnRemainingFreeNumbers = True
                    Exit For
                End If
            End If
        Next

        If blnRemainingFreeNumbers Then
            For x = 0 To intTotalNumbersNeeded - 1
                Dim intNewRandomNumber As Integer = rdmNumber.Next(pintMinNumber, pintMaxNumber)

                Do Until Not lstRandomNumbers.Contains(intNewRandomNumber)
                    intNewRandomNumber = rdmNumber.Next(pintMinNumber, pintMaxNumber)
                Loop

                lstRandomNumbers.Add(intNewRandomNumber)
            Next
        End If

    End Sub


    Private Sub formatBookerNameCREF2(ByRef fullName As String, ByRef strFirstname As String, ByRef strLastname As String)


        Dim words As String()
        Dim boolsplit As Boolean = False


        If fullName.IndexOf("//") > -1 Then
            fullName = fullName.Substring(0, fullName.IndexOf("//"))
        End If


        If fullName.IndexOf("/") > -1 Then
            words = fullName.Split(New Char() {"/"c})
            strLastname = words(0)
            strFirstname = words(1)
            boolsplit = True
        ElseIf fullName.IndexOf(" ") > -1 Then
            words = fullName.Split(New Char() {" "c})
            strLastname = words(1)
            strFirstname = words(0)
            boolsplit = True
        ElseIf fullName.IndexOf(".") > -1 Then
            words = fullName.Split(New Char() {"."c})
            strLastname = words(1)
            strFirstname = words(0)
            boolsplit = True
        End If

        If boolsplit Then
            fullName = words(0) + " " + words(1)
        End If


    End Sub



    Private Sub formatBookerName(ByVal pstrBookerName As String, ByRef strFirstname As String, ByRef strLastname As String)
        'mess around with the booker name 
        pstrBookerName = Replace(pstrBookerName, ".", " ")

        'Dim strFirstname As String = ""
        'Dim strLastname As String = ""

        If pstrBookerName.Contains(" ") Then
            If split(pstrBookerName, " ").Count > 2 Then
                'they've added title!
                strFirstname = split(pstrBookerName, " ")(1)
                strLastname = split(pstrBookerName, " ")(2)
            Else
                strFirstname = split(pstrBookerName, " ")(0)
                strLastname = split(pstrBookerName, " ")(1)
            End If
        ElseIf pstrBookerName.Length = 2 Then
            'try as initials
            strFirstname = pstrBookerName.Substring(0, 1)
            strLastname = pstrBookerName.Substring(1)
        Else
            'either going to be:
            'full firstname + full lastname
            'firstname initial + full last name
            'firstname only
            'random string that is not a booker name

            'check to see if any of the characters after the 1st are upper case, then split on next occurrance of an upper character
            Dim x As Integer
            Dim intSplitNumber As Integer = 0
            Dim blnUpperSeperate As Boolean = False
            For x = 0 To pstrBookerName.Length - 1
                Dim strCurrentLetter As String = pstrBookerName(x)
                If strCurrentLetter = strCurrentLetter.ToUpper Then
                    'the letter is upper case
                    If x = 0 Then
                        blnUpperSeperate = True
                    Else
                        blnUpperSeperate = True
                        intSplitNumber = x + 1
                        Exit For
                    End If
                End If
            Next

            If blnUpperSeperate Then
                If intSplitNumber = 0 Then
                    intSplitNumber = 1
                End If
                strFirstname = pstrBookerName.Substring(0, intSplitNumber)
                strLastname = pstrBookerName.Substring(intSplitNumber)

            Else
                'if no uppers then just put the string in firstname as can't split fullname
                strFirstname = pstrBookerName
                strLastname = ""
            End If
        End If

        'Capitalise the first letter of both firstname and surname for presentation in email
        Dim strFirstletter As String = ""
        If strFirstname.Length > 1 Then
            strFirstletter = strFirstname.Substring(0, 1)
            strFirstname = strFirstletter.ToUpper & strFirstname.Remove(0, 1).ToLower
        ElseIf strFirstname.Length = 1 Then
            strFirstname = strFirstname.ToUpper
        End If

        strFirstletter = ""

        If strLastname.Length > 1 Then
            strFirstletter = strLastname.Substring(0, 1)
            strLastname = strFirstletter.ToUpper & strLastname.Remove(0, 1).ToLower

            If strLastname.Contains("-") Then
                'double barrelled last name
                Dim strFirstLastname As String = ""
                Dim strSecondLastname As String = ""
                strFirstLastname = strLastname.Substring(0, strLastname.IndexOf("-") + 1)
                strSecondLastname = strLastname.Substring(strLastname.IndexOf("-") + 1)

                Dim intSubstringValue As Integer = 0
                strFirstletter = strSecondLastname.Substring(intSubstringValue, 1)
                If strFirstletter = " " Then
                    intSubstringValue = 1
                    strFirstletter = strSecondLastname.Substring(intSubstringValue, 1)
                End If

                strLastname = strFirstLastname & strFirstletter.ToUpper & strSecondLastname.Remove(intSubstringValue, 1).ToLower

            End If
        ElseIf strLastname.Length = 1 Then
            strLastname = strLastname.ToUpper
        End If


    End Sub


  


    Protected Sub ddClients_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddClients.SelectedIndexChanged

        Select Case ddClients.SelectedItem.Value
            Case "10" 'all clients Air Only
                chkAir.Checked = True
                chkRail.Checked = False
                chkHotel.Checked = False
            Case "20" 'all clients Rail
                chkAir.Checked = False
                chkRail.Checked = True
                chkHotel.Checked = False
            Case "30" 'all clients Hotel
                chkAir.Checked = False
                chkRail.Checked = False
                chkHotel.Checked = True

        End Select
    End Sub
End Class