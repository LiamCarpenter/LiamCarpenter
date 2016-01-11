Imports NysDat
Imports System.Net.Mail
Imports CSVParser

Imports System.Data
Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Imports System.Xml
Imports System.IO
Imports System.Net
Imports System.Text

Imports Microsoft.Win32

Public Class AMVenueChaser
    Inherits clsNYS

    Private Shared ReadOnly className As String

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
                handleexception(ex, "AMVenueChaser", Me.Page)
            End Try
        End Using
    End Sub
    Private Sub IAClientStatements_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAClientStatements_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "AMVenueChaser"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "AMVenueChaser", Me.Page)
            End Try
        End Using
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Using New clslogger(log, className, "Page_Load")
            Try
                If Not IsPostBack Then
                    'Dim strRet As String = setUser()
                    'If strRet.StartsWith("ERROR") Then
                    '    Response.Redirect("IALogonAdmin.aspx?User=falseX")
                    'End If

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    'but.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    'but.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    'but.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub but_Click(sender As Object, e As EventArgs) Handles but.Click

        Dim oEnqs As New List(Of clsEnquiryDat)
        oEnqs = clsEnquiryDat.checkETPenquiryAM(0)

        'Response.Write("Chasers " + oEnqs.Count.ToString())

        For Each o As clsEnquiryDat In oEnqs
          
            Dim blnFirstSend As Boolean = True
            Dim process As String = ""
            If o.venueQuoteType = 0 Then 'then ddr 
                process = "VenueType 0"
                If clsEnquiryDat.paidDirectCheckDdr(o.enquiryvenueid) = 0 Then
                    blnFirstSend = False
                    process += " paidDirectCheckDdr "
                End If
            ElseIf o.venueQuoteType = 1 Then 'the room & cat
                process = "VenueType 1"
                If clsEnquiryDat.paidDirectCheckRc(o.enquiryvenueid) = 0 Then
                    blnFirstSend = False
                    process += " paidDirectCheckRc "
                End If
            Else 'error!
                blnFirstSend = False
                process += " error "
            End If

            lstEnquiry.Items.Add(o.groupname + "  " + o.eventname + " " + o.eventdate + " " + o.eventdateend + " " + o.duration.ToString() + " " + o.durationtype + " " + o.enquiryvenueid.ToString() + " " + o.venueaccountsemail + " " + process)

            If blnFirstSend Then
                'R2.6 CR - added oEnq.companyaccountstel
                If sendETPemailToVenue(o.enquiryid, _
                                        o.groupid, _
                                        o.groupname, _
                                        o.eventname, _
                                        o.companyid, _
                                        o.companyname, _
                                        o.companyaccountsemail, _
                                        o.venueaccountsemail, _
                                        o.eventdate, _
                                        o.nysref, _
                                        o.companyaccountstel) Then

                    'update invoice record
                    'clsInvoicingDat.invoiceAutoSave(o.enquiryid, Now)
                Else
                    'update invoice record so can find later
                    'clsInvoicingDat.invoiceAutoSave(o.enquiryid, CDate("01/01/2999"))
                End If
            Else
                'update invoice record so can find later
                'clsInvoicingDat.invoiceAutoSave(o.enquiryid, CDate("01/01/2999"))
            End If

        Next

    End Sub


    Private Function sendETPemailToVenue(ByVal penquiryid As Integer, _
                                  ByVal pgroupid As Integer, _
                                  ByVal pgroupname As String, _
                                  ByVal peventname As String, _
                                  ByVal pcompanyid As Integer, _
                                  ByVal pcompanyname As String, _
                                  ByVal pCompanyAccountsEmailFrom As String, _
                                  ByVal pVenueAccountsEmailTo As String, _
                                  ByVal peventdate As String, _
                                  ByVal pnysref As String, _
                                  ByVal pcompanyaccountstel As String) As Boolean
        Try

            Dim ofile As New System.IO.StreamReader(getConfig("HomeAbsolutepath") & "userdocs\NYS\ETPVenue.htm")

            Dim strreadtest As String = ofile.ReadToEnd & "<p>"
            ofile.Close()

            strreadtest = strreadtest.Replace("#nysref#", pnysref)
            strreadtest = strreadtest.Replace("#eventname#", peventname)
            strreadtest = strreadtest.Replace("#groupname#", pgroupname)
            strreadtest = strreadtest.Replace("#eventdate#", peventdate)

            'R2.6 CR
            strreadtest = strreadtest.Replace("#companyaccountstel#", pcompanyaccountstel)

            Dim strfrom As String = "ashley.marron@nysgroup.com" 'pCompanyAccountsEmailFrom

            If strfrom = "" Then
                strfrom = "accounts@nysgroup.com"
            End If

            'Dim strSavePath As String = getConfig("HomeAbsolutepath") & "userdocs\" & CStr(pgroupid) & "-" & _
            '                                CStr(pgroupname) & "\" & CStr(pcompanyid) & _
            '                                "-" & CStr(pcompanyname) & "\emails\body\"

            Dim strMessage As String = strreadtest
            Dim strTo As String = "ashley.marron@nysgroup.com" 'pVenueAccountsEmailTo

            'If strTo = "" Then
            '    SendEmail.send(strfrom, "craig.rickell@nysgroup.com", "Auto invoice error: No venue email", strMessage, "", "", "")
            '    Return False
            'End If

            'If getConfig("ReleaseEmailTest") = "true" Then
            '    strTo = getConfig("ReleaseEmailTestSend")
            '    strMessage = "Email would have been sent to: " & pVenueAccountsEmailTo & " - " & strMessage
            'End If


            Dim oMail As New MailMessage()
            oMail.From = New MailAddress("ashley.marron@nysgroup.com")
            oMail.To.Add(strTo)
            oMail.IsBodyHtml = True
            oMail.BodyEncoding = System.Text.Encoding.Default
            oMail.Subject = getConfig("ETPSubject")




            oMail.Body =strMessage 

           


            Dim oClient As New SmtpClient
     
            oClient.Send(oMail)

            ' SendEmail.send(strfrom, strTo, getConfig("ETPSubject"), strMessage, "", "", "")

                'saveHistory(penquiryid, Format(Date.Now, "dd/MM/yyyy"), _
                '    getConfig("ETPSubject") & " email sent to:" & strTo, "", "email", "", "Auto email", _
                '    "Complete", 3, strMessage, strSavePath)
                Return True
        Catch ex As Exception
            log.Error("AUTO INVOICE EMAIL ERROR: " & ex.Message)
            Return False
        End Try
    End Function
    '-------------------------------------------------------------------------------------------------------

    Private CubitTime As Long

    'R2.21.7 CR
    Private CubitAfternoonTime As Long
    Private CubitBookedAfternoonDisabled As Boolean = False
    Private CubitTransDisabled As Boolean = False
    Private CubitFinishTime As Long
    Private lastCubitDay As DateTime


    Protected Sub WebGet(ByVal strday As String, ByVal strdayConf As String, ByVal ToLocation As String, ByVal Ending As String)
        If Not IO.File.Exists(ToLocation & strday & ".csv") Then 'And _   ''brute force 
            ' Not IO.File.Exists(getConfig("downloadedfilesOK") & Format(Now, "dd-MM-yyyy") & ".csv") And _
            ' Not IO.File.Exists(getConfig("downloadedfilesERROR") & "Error_" & Format(Now, "dd-MM-yy") & ".csv") Then
            Dim wr As HttpWebRequest = CType(WebRequest.Create(getConfig("ConfermaFileLocation") & _
                                        strdayConf & Ending), HttpWebRequest)
            Dim ws As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)
            Dim str As Stream = ws.GetResponseStream()
            Dim inBuf(2000000) As Byte
            Dim bytesToRead As Integer = CInt(inBuf.Length)
            Dim bytesRead As Integer = 0
            While bytesToRead > 0
                Dim n As Integer = str.Read(inBuf, bytesRead, bytesToRead)
                If n = 0 Then
                    Exit While
                End If
                bytesRead += n
                bytesToRead -= n
            End While

            Dim fstr As New FileStream(ToLocation & strday & _
                                       ".csv", FileMode.OpenOrCreate, FileAccess.Write)
            fstr.Write(inBuf, 0, bytesRead)
            str.Close()
            fstr.Close()
            txtError.Text = "OK " + Ending
        Else
            txtError.Text = "File Exists " + Ending + " " + ToLocation & strday & ".csv"
        End If
    End Sub
    Protected Sub butCubitImport_Click(sender As Object, e As EventArgs) Handles butCubitImport.Click

        CubitTime = getCubitTimeFromConfig()

        'R2.21.7 CR
        CubitAfternoonTime = getCubitAfternoonTimeFromConfig()

        CubitFinishTime = getCubitFinishTimeFromConfig()

        '        '1
        'To:    \\nys-accounts\downloadedfiles\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20140513-booking.csv

        '        '2
        'To:    \\nys-accounts\downloadedfilesBooked\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20140513-bookingsMadeToday.csv

        '        '3
        'To:    \\nys-accounts\downloadedfilesBookedAfternoon\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20140513-bookingsMadeToday%20between%2012am%20and%204pm.csv

        '        '4
        'To:    \\nys-accounts\downloadedfiles\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/2014-05-13-Hotel_DailyTransactions_Agency1438.csv




        Debug.Print("Grab ----> WebRetrieve")

        Debug.Print("Import")
        '

        '        '**1  confermaLauncher()
        'To:    \\nys-accounts\downloadedfiles\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20140513-booking.csv
        Debug.Print("confermaLauncher Main: " + (getConfig("downloadedfiles") & Format(Now, "dd-MM-yyyy") & ".csv"))
        Debug.Print((getConfig("ConfermaFileLocation") & Format(Now, "yyyyMMdd") & "-booking.csv"))


        '        '**2 confermaBookedLaunche
        'To:    \\nys-accounts\downloadedfilesBooked\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20140513-bookingsMadeToday.csv
        Debug.Print("confermaBookedLauncher" + (getConfig("downloadedfilesBooked") & Format(Now, "dd-MM-yyyy") & ".csv"))
        Debug.Print(getConfig("ConfermaFileLocation") & Format(Now, "yyyyMMdd") & "-bookingsMadeToday.csv")


        '        '3  confermaBookedAftLauncher ---- already....
        'To:    \\nys-accounts\downloadedfilesBookedAfternoon\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20140513-bookingsMadeToday%20between%2012am%20and%204pm.csv
        'Debug.Print("confermaBookedAftLauncher" + (getConfig("downloadedfilesBookedAft") & Format(Now, "dd-MM-yyyy") & ".csv"))
        'Debug.Print(getConfig("ConfermaFileLocation") & Format(Now, "yyyyMMdd") & "-bookingsMadeToday%20between%2012am%20and%204pm.csv")

        '        '4   confermaTransLauncher
        'To:    \\nys-accounts\downloadedfiles\13-05-2014.csv
        'From:  https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/2014-05-13-Hotel_DailyTransactions_Agency1438.csv
        Debug.Print("confermaTransLauncher" + (getConfig("downloadedfilesTrans") & Format(Now, "dd-MM-yyyy") & ".csv"))
        Debug.Print(getConfig("ConfermaFileLocation") & Format(Now, "yyyy-MM-dd") & "-Hotel_DailyTransactions_Agency1438.csv")


        Debug.Print(getConfig("CubitConnStr"))

        Dim strday As String = Me.txtStrDate.Text ' "10-05-2014" 
        Dim strdayConf As String = strday.Substring(6, 4) + strday.Substring(3, 2) + strday.Substring(0, 2)   'Me.txtStrDateConf.Text '20140510'
        Dim strdayConf4 As String = strday.Substring(6, 4) + "-" + strday.Substring(3, 2) + "-" + strday.Substring(0, 2)   '"2014-05-13" 'changes for filesTrans


        'WebGet(strday, strdayConf, getConfig("downloadedfiles"), "-booking.csv")
        'Dim csv1 As New CSVReader
        ''**1
        'If csv1.Main(strday) Then
        '    log.Info("CSV file import OK")
        'Else
        '    log.Info("CSV file import ERROR " + getConfig("downloadedfiles"))
        'End If

        '**2
        WebGet(strday, strdayConf, getConfig("downloadedfilesBooked"), "-bookingsMadeToday.csv")
        Dim csv2 As CSVReaderBooked
        csv2 = New CSVReaderBooked
        If csv2.Main(strday) Then
            log.Info("CSV file import OK")
        Else
            log.Info("CSV file import ERROR " + getConfig("downloadedfilesBooked"))
        End If

        ''**3 ----> afternoon already doe
        'WebGet(strday, strdayConf, getConfig("downloadedfilesBookedAft"), "-bookingsMadeToday%20between%2012am%20and%204pm.csv")
        'Dim csv3 As CSVReaderBooked
        'csv3 = New CSVReaderBooked
        ''call the afternoon import instead of the main - so that it looks in the other file location
        'If csv3.Afternoon(strday) Then
        '    log.Info("CSV file import OK")
        'Else
        '    log.Info("CSV file import ERROR " + getConfig("downloadedfilesBookedAft"))
        'End If

        ''**4
        'WebGet(strday, strdayConf4, getConfig("downloadedfilesTrans"), "-Hotel_DailyTransactions_Agency1438.csv")
        'Dim csv4 As CSVReaderTrans
        'csv4 = New CSVReaderTrans
        'If csv4.Main(strday) Then
        '    log.Info("CSV file import OK")
        'Else
        '    log.Info("CSV file import ERROR " + getConfig("downloadedfilesTrans"))
        'End If

        txtError.Text = "....... Ends .........."
    End Sub




    Private Function yesterday() As DateTime
        Return stripTime(DateTime.Now) - New TimeSpan(24, 0, 0)
    End Function
    Private Function getCurrentTime() As Long
        Return minutesInDay(DateTime.Now)
    End Function

    Private Function minutesInDay(ByVal dt As DateTime) As Long
        Return dt.Hour * 60 + dt.Minute
    End Function

    Private Function getCurrentDay() As DateTime
        Return stripTime(DateTime.Now)
    End Function
    Private Function stripTime(ByVal dt As DateTime) As DateTime
        Return New DateTime(dt.Year, dt.Month, dt.Day)
    End Function

    Private Function getCubitTimeFromConfig() As Long
        log.Info("Cubit time is: " & getConfig("CubitTime"))
        Dim m As Match = Regex.Match(getConfig("CubitTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    'R2.21.7 CR
    Private Function getCubitAfternoonTimeFromConfig() As Long
        log.Info("CUBIT Afternoon time is: " & getConfig("CubitAfternoonTime"))
        Dim m As Match = Regex.Match(getConfig("CubitAfternoonTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Function getCubitFinishTimeFromConfig() As Long
        log.Info("Cubit Finish time is: " & getConfig("CubitFinishTime"))
        Dim m As Match = Regex.Match(getConfig("CubitFinishTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function


    '-------------------------------------------------

    Public Sub EvolviReader(ByVal pDayOfWeek As String)
        Try
            Dim strEvolviFilesLocation As String = "\\nys-filestore\EvolviHandoffs\intercept\SQL\" 'getConfig("EvolviHandoffs") ' \\nysmgmt\EvolviHandoffs\intercept\SQL\
            Dim strDay As String = pDayOfWeek
            'If getConfig("EvolviDay") <> "" Then  '   <add key="EvolviDay" value=""/>
            '    strDay = getConfig("EvolviDay") 'but in the app it is ""
            'End If
            Dim fileCount As Integer = System.IO.Directory.GetFiles(strEvolviFilesLocation).Length   '  \\nysmgmt\EvolviHandoffs\intercept\SQL\
            Dim arrFiles() As String = System.IO.Directory.GetFiles(strEvolviFilesLocation, "*.xml") ' into an array to manage


            For i As Integer = 0 To fileCount - 1
                Dim Ref As Integer = 0
                Dim ExternalRef As String = ""
                Dim IssueRef As Integer = 0
                Dim TransactionType As String = ""
                Dim TransactionDate As String = ""
                Dim MachineType As Integer = 0
                Dim MachineNumber As String = ""
                Dim CurrencyCode As String = ""
                Dim PersonalCCUsed As String = ""
                Dim FulfilmentType As String = ""
                Dim AccountRef As Integer = 0
                Dim AccountExternalRef As String = ""
                Dim AccountType As String = ""
                Dim Period As String = ""
                Dim Immediate As String = ""
                Dim EvolviInvoice As String = ""
                Dim EvolviPayment As String = ""
                Dim EvolviFinancial As String = ""
                Dim BookingAgentRef As Integer = 0
                Dim BookingAgentExternalRef As String = ""
                Dim BranchExternalRef As String = ""
                Dim Unit As String = ""
                Dim BookingAgentFirstName As String = ""
                Dim BookingAgentLastName As String = ""
                Dim BookingAgentTitle As String = ""
                Dim BookingAgentEmailAddress As String = ""
                Dim OnBehalfRef As Integer = 0
                Dim OnBehalfExternalRef As String = ""
                Dim OnBehalfBranchExternalRef As String = ""
                Dim OnBehalfUnit As String = ""
                Dim OnBehalfFirstName As String = ""
                Dim OnBehalfLastName As String = ""
                Dim OnBehalfTitle As String = ""
                Dim OnBehalfEmailAddress As String = ""

                Dim TicketingAgentRef As Integer = 0
                Dim TicketingAgentExternalRef As String = ""
                Dim TicketingAgentBranchExternalRef As String = ""
                Dim TicketingAgentUnit As String = ""
                Dim LeadRef As Integer = 0
                Dim AccountContactRef As Integer = 0
                Dim AccountContactFirstName As String = ""
                Dim AccountContactLastName As String = ""
                Dim AccountContactTitle As String = ""
                Dim AccountContactNumber As String = ""
                Dim AccountContactEmail As String = ""
                Dim AccountContactOrganisation As String = ""
                Dim AccountContactAddress1 As String = ""
                Dim AccountContactAddress2 As String = ""
                Dim AccountContactAddress3 As String = ""
                Dim AccountContactCity As String = ""
                Dim AccountContactCounty As String = ""
                Dim AccountContactPostcode As String = ""
                Dim DeliveryContactRef As Integer = 0
                Dim DeliveryContactFirstName As String = ""
                Dim DeliveryContactLastName As String = ""
                Dim DeliveryContactTitle As String = ""
                Dim DeliveryContactNumber As String = ""
                Dim DeliveryContactEmail As String = ""
                Dim DeliveryContactOrganisation As String = ""
                Dim DeliveryContactAddress1 As String = ""
                Dim DeliveryContactAddress2 As String = ""
                Dim DeliveryContactAddress3 As String = ""
                Dim DeliveryContactCity As String = ""
                Dim DeliveryContactCounty As String = ""
                Dim DeliveryContactPostcode As String = ""
                Try
                    Dim odoc As New System.Xml.XmlDocument

                    odoc.Load(arrFiles(i))

                    ' XML Check through to load variables above 
                    Try
                        'R2.23 CR - BUG FIX: apparently the new "SignOnToken" tag isn't ALWAYS there...grrr
                        'so, we take the firstchild name and check it - if it's wrong then twe take the next sibling name and check it
                        'then if BOTH fail we error
                        Dim ndeFirstChild As XmlNode = odoc.LastChild.FirstChild
                        If ndeFirstChild.Name <> "Agency" Then
                            ndeFirstChild = odoc.LastChild.FirstChild.NextSibling
                        End If


                        'R2.23 CR - BUG FIX: added .nextsibling to skip new "SignOnToken" tag
                        If ndeFirstChild.Name = "Agency" Then
                            For Each at As XmlAttribute In ndeFirstChild.Attributes
                                If at.Name = "Ref" Then
                                    Ref = notInteger(at.Value)
                                ElseIf at.Name = "ExternalRef" Then
                                    ExternalRef = at.Value
                                End If
                            Next
                        Else
                            log.Error("No Agency as first child:" & arrFiles(i).ToString)
                        End If

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!! So below comment is wrong
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        Dim oNodeToProcess As XmlNode
                        If odoc.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odoc.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odoc.LastChild.LastChild
                        End If

                        'R2.23 CR - BUG FIX: ImmediateDetail is no longer last child, now there is "FeeOrderRule"
                        'PreviousSibling added; should move it back to the correct node
                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            For Each at As XmlAttribute In oNodeToProcess.Attributes
                                If at.Name = "IssueRef" Then
                                    IssueRef = notInteger(at.Value)
                                ElseIf at.Name = "TransactionType" Then
                                    TransactionType = at.Value
                                ElseIf at.Name = "TransactionDate" Then
                                    TransactionDate = Mid(at.Value, 1, 10)
                                    TransactionDate = Format(CDate(TransactionDate), "dd/MM/yyyy")
                                ElseIf at.Name = "MachineType" Then
                                    MachineType = notInteger(at.Value)
                                ElseIf at.Name = "MachineNumber" Then
                                    MachineNumber = at.Value
                                ElseIf at.Name = "CurrencyCode" Then
                                    CurrencyCode = at.Value
                                ElseIf at.Name = "PersonalCCUsed" Then
                                    PersonalCCUsed = at.Value
                                ElseIf at.Name = "FulfilmentType" Then
                                    FulfilmentType = at.Value
                                End If
                            Next
                            If oNodeToProcess.HasChildNodes Then
                                For iI As Integer = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Account" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                AccountRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                AccountExternalRef = at.Value
                                            ElseIf at.Name = "Type" Then
                                                AccountType = at.Value
                                            ElseIf at.Name = "Period" Then
                                                Period = at.Value
                                            ElseIf at.Name = "Immediate" Then
                                                Immediate = at.Value
                                            ElseIf at.Name = "EvolviInvoice" Then
                                                EvolviInvoice = at.Value
                                            ElseIf at.Name = "EvolviPayment" Then
                                                EvolviPayment = at.Value
                                            ElseIf at.Name = "EvolviFinancial" Then
                                                EvolviFinancial = at.Value
                                            End If
                                        Next
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "BookingAgent" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                BookingAgentRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                BookingAgentExternalRef = at.Value
                                            ElseIf at.Name = "BranchExternalRef" Then
                                                BranchExternalRef = at.Value
                                            ElseIf at.Name = "Unit" Then
                                                Unit = at.Value
                                            End If
                                        Next

                                        'R1
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            BookingAgentFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            BookingAgentLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            BookingAgentTitle = at.Value
                                                        End If
                                                    Next
                                                End If
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            BookingAgentEmailAddress = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                        'end of R1

                                        'new bit r2
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "OnBehalfOf" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                OnBehalfRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                OnBehalfExternalRef = at.Value
                                            ElseIf at.Name = "BranchExternalRef" Then
                                                OnBehalfBranchExternalRef = at.Value
                                            ElseIf at.Name = "Unit" Then
                                                OnBehalfUnit = at.Value
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            OnBehalfFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            OnBehalfLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            OnBehalfTitle = at.Value
                                                        End If
                                                    Next
                                                End If
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            OnBehalfEmailAddress = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                        'end of R2

                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "TicketingAgent" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                TicketingAgentRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                TicketingAgentExternalRef = at.Value
                                            ElseIf at.Name = "BranchExternalRef" Then
                                                TicketingAgentBranchExternalRef = at.Value
                                            ElseIf at.Name = "Unit" Then
                                                TicketingAgentUnit = at.Value
                                            End If
                                        Next
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "PassengerGroup" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "LeadRef" Then
                                                LeadRef = notInteger(at.Value)
                                            End If
                                        Next
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "AccountContact" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                AccountContactRef = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            AccountContactFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            AccountContactLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            AccountContactTitle = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Phone" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Number" Then
                                                            AccountContactNumber = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            AccountContactEmail = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Address" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Organisation" Then
                                                            AccountContactOrganisation = at.Value
                                                        ElseIf at.Name = "Address1" Then
                                                            AccountContactAddress1 = at.Value
                                                        ElseIf at.Name = "Address2" Then
                                                            AccountContactAddress2 = at.Value
                                                        ElseIf at.Name = "Address3" Then
                                                            AccountContactAddress3 = at.Value
                                                        ElseIf at.Name = "City" Then
                                                            AccountContactCity = at.Value
                                                        ElseIf at.Name = "County" Then
                                                            AccountContactCounty = at.Value
                                                        ElseIf at.Name = "Postcode" Then
                                                            AccountContactPostcode = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "DeliveryContact" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                DeliveryContactRef = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            DeliveryContactFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            DeliveryContactLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            DeliveryContactTitle = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Phone" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Number" Then
                                                            DeliveryContactNumber = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            DeliveryContactEmail = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Address" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Organisation" Then
                                                            DeliveryContactOrganisation = at.Value
                                                        ElseIf at.Name = "Address1" Then
                                                            DeliveryContactAddress1 = at.Value
                                                        ElseIf at.Name = "Address2" Then
                                                            DeliveryContactAddress2 = at.Value
                                                        ElseIf at.Name = "Address3" Then
                                                            DeliveryContactAddress3 = at.Value
                                                        ElseIf at.Name = "City" Then
                                                            DeliveryContactCity = at.Value
                                                        ElseIf at.Name = "County" Then
                                                            DeliveryContactCounty = at.Value
                                                        ElseIf at.Name = "Postcode" Then
                                                            DeliveryContactPostcode = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        Else
                            log.Error("No Immediate as Last child:" & arrFiles(i).ToString)
                        End If

                        'first save all main data 
                        ' load clsEvolvi in MEVIS --- NYSDAT
                        Dim intEvolviID As Integer = NysDat.clsEvolvi.saveEvolvi(Ref, ExternalRef, _
                          IssueRef, TransactionType, TransactionDate _
                        , MachineType, MachineNumber, CurrencyCode, PersonalCCUsed, FulfilmentType _
                        , AccountRef, AccountExternalRef, AccountType, Period, Immediate _
                        , EvolviInvoice, EvolviPayment, EvolviFinancial, BookingAgentRef _
                        , BookingAgentExternalRef, BranchExternalRef, Unit, BookingAgentFirstName _
                        , BookingAgentLastName, BookingAgentTitle, BookingAgentEmailAddress _
                        , OnBehalfRef, OnBehalfExternalRef, OnBehalfBranchExternalRef _
                        , OnBehalfUnit, OnBehalfFirstName, OnBehalfLastName, OnBehalfTitle _
                        , OnBehalfEmailAddress, TicketingAgentRef _
                        , TicketingAgentExternalRef, TicketingAgentBranchExternalRef _
                        , TicketingAgentUnit, LeadRef, AccountContactRef, AccountContactFirstName _
                        , AccountContactLastName, AccountContactTitle, AccountContactNumber _
                        , AccountContactEmail, AccountContactOrganisation, AccountContactAddress1 _
                        , AccountContactAddress2, AccountContactAddress3, AccountContactCity _
                        , AccountContactCounty, AccountContactPostcode, DeliveryContactRef _
                        , DeliveryContactFirstName, DeliveryContactLastName, DeliveryContactTitle _
                        , DeliveryContactNumber, DeliveryContactEmail, DeliveryContactOrganisation _
                        , DeliveryContactAddress1, DeliveryContactAddress2, DeliveryContactAddress3 _
                        , DeliveryContactCity, DeliveryContactCounty, DeliveryContactPostcode)

                        odoc = Nothing

                        'now do Custom fields
                        Dim odocCustom As New System.Xml.XmlDocument
                        odocCustom.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocCustom.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocCustom.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocCustom.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI As Integer = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "CustomField" Then
                                        Dim CustomCode As String = ""
                                        Dim CustomValue As String = ""
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Code" Then
                                                CustomCode = at.Value
                                            ElseIf at.Name = "Value" Then
                                                CustomValue = at.Value
                                            End If
                                        Next
                                        NysDat.clsEvolvi.saveCustom(intEvolviID, CustomCode, CustomValue)
                                    End If
                                Next
                            End If
                        End If

                        odocCustom = Nothing
                        'now do Passengers
                        Dim odocPassenger As New System.Xml.XmlDocument
                        odocPassenger.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocPassenger.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocPassenger.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocPassenger.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI As Integer = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "PassengerGroup" Then
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                Dim PassengerRef As Integer = 0
                                                Dim AdultChild As String = ""
                                                Dim FirstName As String = ""
                                                Dim LastName As String = ""
                                                Dim Title As String = ""
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Passenger" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Ref" Then
                                                            PassengerRef = notInteger(at.Value)
                                                        ElseIf at.Name = "AdultChild" Then
                                                            AdultChild = at.Value
                                                        End If
                                                    Next
                                                End If
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                    Dim PassengerID As Integer = 0
                                                    For iIII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.Count - 1
                                                        If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "Person" Then
                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                If at.Name = "FirstName" Then
                                                                    FirstName = at.Value
                                                                ElseIf at.Name = "LastName" Then
                                                                    LastName = at.Value
                                                                ElseIf at.Name = "Title" Then
                                                                    Title = at.Value
                                                                End If
                                                            Next
                                                            PassengerID = NysDat.clsEvolvi.savePassenger(intEvolviID, LeadRef, _
                                                                                   PassengerRef, AdultChild, _
                                                                                   FirstName, LastName, Title)
                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "CustomField" Then
                                                            Dim CustomCode As String = ""
                                                            Dim CustomLabel As String = ""
                                                            Dim CustomValue As String = ""
                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                If at.Name = "Code" Then
                                                                    CustomCode = at.Value
                                                                ElseIf at.Name = "Label" Then
                                                                    CustomLabel = at.Value
                                                                ElseIf at.Name = "Value" Then
                                                                    CustomValue = at.Value
                                                                End If
                                                            Next
                                                            NysDat.clsEvolvi.savePassengerCustom(intEvolviID, PassengerID, _
                                                                                   CustomCode, CustomLabel, CustomValue)
                                                        End If
                                                    Next
                                                End If

                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If

                        odocPassenger = Nothing

                        'now do segments

                        Dim odocSegment As New System.Xml.XmlDocument
                        Dim SegmentRef As Integer = 0
                        Dim Distance As Double = 0
                        Dim DistanceUnits As String = ""
                        Dim JourneyTime As Double = 0

                        odocSegment.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocSegment.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocSegment.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocSegment.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI As Integer = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                SegmentRef = notInteger(at.Value)
                                            ElseIf at.Name = "Distance" Then
                                                Distance = notNumber(at.Value)
                                            ElseIf at.Name = "DistanceUnits" Then
                                                DistanceUnits = notString(at.Value)
                                            ElseIf at.Name = "JourneyTime" Then
                                                JourneyTime = notNumber(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            Dim OriginUICCode As String = ""
                                            Dim OriginNLCCode As String = ""
                                            Dim OriginCRSCode As String = ""
                                            Dim OriginName As String = ""
                                            Dim DestinationUICCode As String = ""
                                            Dim DestinationNLCCode As String = ""
                                            Dim DestinationCRSCode As String = ""
                                            Dim DestinationName As String = ""
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Origin" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "UICCode" Then
                                                            OriginUICCode = at.Value
                                                        ElseIf at.Name = "NLCCode" Then
                                                            OriginNLCCode = at.Value
                                                        ElseIf at.Name = "CRSCode" Then
                                                            OriginCRSCode = at.Value
                                                        ElseIf at.Name = "Name" Then
                                                            OriginName = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Destination" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "UICCode" Then
                                                            DestinationUICCode = at.Value
                                                        ElseIf at.Name = "NLCCode" Then
                                                            DestinationNLCCode = at.Value
                                                        ElseIf at.Name = "CRSCode" Then
                                                            DestinationCRSCode = at.Value
                                                        ElseIf at.Name = "Name" Then
                                                            DestinationName = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                            NysDat.clsEvolvi.saveSegment(SegmentRef, Distance, DistanceUnits, JourneyTime, _
                                                                         intEvolviID, OriginUICCode, OriginNLCCode, _
                                                                         OriginCRSCode, OriginName, DestinationUICCode, _
                                                                         DestinationNLCCode, DestinationCRSCode, DestinationName)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                        odocSegment = Nothing

                        'now do legs in segments
                        Dim odocLeg As New System.Xml.XmlDocument
                        Dim SegmentRefLeg As Integer = 0

                        odocLeg.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocLeg.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocLeg.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocLeg.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI As Integer = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                SegmentRefLeg = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            Dim LegRef As Integer = 0
                                            Dim Direction As String = ""
                                            Dim Departure As String = ""
                                            Dim Arrival As String = ""
                                            Dim TransportMode As String = ""
                                            Dim LegOriginUICCode As String = ""
                                            Dim LegOriginNLCCode As String = ""
                                            Dim LegOriginCRSCode As String = ""
                                            Dim LegOriginName As String = ""
                                            Dim LegDestinationUICCode As String = ""
                                            Dim LegDestinationNLCCode As String = ""
                                            Dim LegDestinationCRSCode As String = ""
                                            Dim LegDestinationName As String = ""
                                            Dim TOCCode As String = ""
                                            Dim TOCName As String = ""
                                            Dim PassengerRef As String = ""
                                            Dim AccomodationUnit As String = ""
                                            Dim TrainRouteRef As String = ""
                                            Dim TrainRouteOriginDeparture As String = ""
                                            Dim TrainRouteDestinationArrival As String = ""
                                            Dim TrainRouteOriginUICCode As String = ""
                                            Dim TrainRouteOriginNLCCode As String = ""
                                            Dim TrainRouteOriginCRSCode As String = ""
                                            Dim TrainRouteOriginName As String = ""
                                            Dim TrainRouteDestinationUICCode As String = ""
                                            Dim TrainRouteDestinationNLCCode As String = ""
                                            Dim TrainRouteDestinationCRSCode As String = ""
                                            Dim TrainRouteDestinationName As String = ""

                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Leg" Then
                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Ref" Then
                                                            LegRef = notInteger(at.Value)
                                                        ElseIf at.Name = "Direction" Then
                                                            Direction = at.Value
                                                        ElseIf at.Name = "Departure" Then
                                                            Departure = Mid(at.Value, 1, 10)
                                                            Departure = Format(CDate(Departure), "dd/MM/yyyy")
                                                        ElseIf at.Name = "Arrival" Then
                                                            Arrival = Mid(at.Value, 1, 10)
                                                            Arrival = Format(CDate(Arrival), "dd/MM/yyyy")
                                                        ElseIf at.Name = "TransportMode" Then
                                                            TransportMode = at.Value
                                                        End If
                                                    Next
                                                    If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                        For iIII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.Count - 1
                                                            If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "Origin" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "UICCode" Then
                                                                        LegOriginUICCode = at.Value
                                                                    ElseIf at.Name = "NLCCode" Then
                                                                        LegOriginNLCCode = at.Value
                                                                    ElseIf at.Name = "CRSCode" Then
                                                                        LegOriginCRSCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        LegOriginName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "Destination" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "UICCode" Then
                                                                        LegDestinationUICCode = at.Value
                                                                    ElseIf at.Name = "NLCCode" Then
                                                                        LegDestinationNLCCode = at.Value
                                                                    ElseIf at.Name = "CRSCode" Then
                                                                        LegDestinationCRSCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        LegDestinationName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "TOC" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "Code" Then
                                                                        TOCCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        TOCName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "Reservation" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "PassengerRef" Then
                                                                        PassengerRef = at.Value
                                                                    ElseIf at.Name = "AccomodationUnit" Then
                                                                        AccomodationUnit = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "TrainRoute" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "Ref" Then
                                                                        TrainRouteRef = at.Value
                                                                    ElseIf at.Name = "OriginDeparture" Then
                                                                        TrainRouteOriginDeparture = Mid(at.Value, 1, 10)
                                                                        TrainRouteOriginDeparture = Format(CDate(TrainRouteOriginDeparture), "dd/MM/yyyy")
                                                                    ElseIf at.Name = "DestinationArrival" Then
                                                                        TrainRouteDestinationArrival = Mid(at.Value, 1, 10)
                                                                        TrainRouteDestinationArrival = Format(CDate(TrainRouteOriginDeparture), "dd/MM/yyyy")
                                                                    End If
                                                                Next
                                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).HasChildNodes Then
                                                                    For iIIII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes.Count - 1
                                                                        If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "Origin" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "UICCode" Then
                                                                                    TrainRouteOriginUICCode = at.Value
                                                                                ElseIf at.Name = "NLCCode" Then
                                                                                    TrainRouteOriginNLCCode = at.Value
                                                                                ElseIf at.Name = "CRSCode" Then
                                                                                    TrainRouteOriginCRSCode = at.Value
                                                                                ElseIf at.Name = "Name" Then
                                                                                    TrainRouteOriginName = at.Value
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "Destination" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "UICCode" Then
                                                                                    TrainRouteDestinationUICCode = at.Value
                                                                                ElseIf at.Name = "NLCCode" Then
                                                                                    TrainRouteDestinationNLCCode = at.Value
                                                                                ElseIf at.Name = "CRSCode" Then
                                                                                    TrainRouteDestinationCRSCode = at.Value
                                                                                ElseIf at.Name = "Name" Then
                                                                                    TrainRouteDestinationName = at.Value
                                                                                End If
                                                                            Next
                                                                        End If
                                                                    Next
                                                                End If
                                                            End If
                                                        Next
                                                        NysDat.clsEvolvi.saveSegmentLeg(LegRef, SegmentRefLeg, intEvolviID, _
                                                                                    Direction, Departure, Arrival, TransportMode, _
                                                                                    LegOriginUICCode, LegOriginNLCCode, LegOriginCRSCode, _
                                                                                    LegOriginName, LegDestinationUICCode, LegDestinationNLCCode, _
                                                                                    LegDestinationCRSCode, LegDestinationName, TOCCode, TOCName, _
                                                                                    PassengerRef, AccomodationUnit, _
                                                                                    TrainRouteRef, TrainRouteOriginDeparture, TrainRouteDestinationArrival, _
                                                                                    TrainRouteOriginUICCode, TrainRouteOriginNLCCode, TrainRouteOriginCRSCode, _
                                                                                    TrainRouteOriginName, TrainRouteDestinationUICCode, TrainRouteDestinationNLCCode, _
                                                                                    TrainRouteDestinationCRSCode, TrainRouteDestinationName)
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If

                        odocLeg = Nothing

                        'now do emissions in segments
                        Dim oEmissionLeg As New System.Xml.XmlDocument
                        Dim SegmentRefEmission As Integer = 0
                        Dim TransportType As String = ""
                        Dim Emissions As Double = 0

                        oEmissionLeg.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If oEmissionLeg.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = oEmissionLeg.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = oEmissionLeg.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI As Integer = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                SegmentRefEmission = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "CarbonEmissionDetails" Then
                                                    If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                        For iIII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.Count - 1
                                                            If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "CarbonEmissions" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "TransportType" Then
                                                                        TransportType = at.Value
                                                                    ElseIf at.Name = "Emissions" Then
                                                                        Emissions = notNumber(at.Value)
                                                                    End If
                                                                Next
                                                                NysDat.clsEvolvi.saveSegmentEmissions(intEvolviID, SegmentRefEmission, TransportType, Emissions)
                                                            End If
                                                        Next
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If

                        oEmissionLeg = Nothing
                        'now do tickets in segments
                        Dim odocTicket As New System.Xml.XmlDocument
                        Dim SegmentRefTicket As Integer = 0

                        odocTicket.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocTicket.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocTicket.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocTicket.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI As Integer = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                SegmentRefTicket = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Ticket" Then
                                                    Dim TicketRef As Integer = 0
                                                    Dim TicketPassengerRef As Integer = 0
                                                    Dim TicketTCN As Integer = 0
                                                    Dim TicketAdultChild As String = ""
                                                    Dim TicketCode As String = ""
                                                    Dim TicketName As String = ""
                                                    Dim TicketClass As String = ""
                                                    Dim TicketSingleReturn As String = ""
                                                    Dim TicketRouteCode As String = ""
                                                    Dim TicketRoute As String = ""
                                                    Dim TicketDistance As Double = 0
                                                    Dim FareTotalAmount As Double = 0
                                                    Dim FareVATAmount As Double = 0
                                                    Dim FareVATCode As Integer = 0
                                                    Dim DiscountTotalAmount As Double = 0
                                                    Dim DiscountVATAmount As Double = 0
                                                    Dim DiscountVATCode As Integer = 0
                                                    Dim TransactionChargeTotalAmount As Double = 0
                                                    Dim TransactionChargeVATAmount As Double = 0
                                                    Dim TransactionChargeVATCode As Integer = 0
                                                    Dim FulfilmentFeeTotalAmount As Double = 0
                                                    Dim FulfilmentFeeVATAmount As Double = 0
                                                    Dim FulfilmentFeeVATCode As Integer = 0
                                                    Dim CreditCardChargeTotalAmount As Double = 0
                                                    Dim CreditCardChargeVATAmount As Double = 0
                                                    Dim CreditCardChargeVATCode As Integer = 0
                                                    Dim NormalFare As Double = 0
                                                    Dim OfferedFare As Double = 0
                                                    Dim RailcardCode As String = ""
                                                    Dim RailcardName As String = ""
                                                    Dim GrossRefundTotalAmount As Double = 0
                                                    Dim GrossRefundVATAmount As Double = 0
                                                    Dim GrossRefundVATCode As Integer = 0
                                                    Dim DiscountReclaimedTotalAmount As Double = 0
                                                    Dim DiscountReclaimedVATAmount As Double = 0
                                                    Dim DiscountReclaimedVATCode As Integer = 0
                                                    Dim ATOCCancellationChargeTotalAmount As Double = 0
                                                    Dim ATOCCancellationChargeVATAmount As Double = 0
                                                    Dim ATOCCancellationChargeVATCode As Integer = 0
                                                    Dim AgencyCancellationChargeTotalAmount As Double = 0
                                                    Dim AgencyCancellationChargeVATAmount As Double = 0
                                                    Dim AgencyCancellationChargeVATCode As Integer = 0
                                                    Dim ExgratiaPaymentTotalAmount As Double = 0
                                                    Dim ExgratiaPaymentVATAmount As Double = 0
                                                    Dim ExgratiaPaymentVATCode As Integer = 0

                                                    For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Ref" Then
                                                            TicketRef = notInteger(at.Value)
                                                        ElseIf at.Name = "PassengerRef" Then
                                                            TicketPassengerRef = notInteger(at.Value)
                                                        ElseIf at.Name = "TCN" Then
                                                            TicketTCN = notInteger(at.Value)
                                                        ElseIf at.Name = "AdultChild" Then
                                                            TicketAdultChild = at.Value
                                                        ElseIf at.Name = "Code" Then
                                                            TicketCode = at.Value
                                                        ElseIf at.Name = "Name" Then
                                                            TicketName = at.Value
                                                        ElseIf at.Name = "Class" Then
                                                            TicketClass = at.Value
                                                        ElseIf at.Name = "SingleReturn" Then
                                                            TicketSingleReturn = at.Value
                                                        ElseIf at.Name = "RouteCode" Then
                                                            TicketRouteCode = at.Value
                                                        ElseIf at.Name = "Route" Then
                                                            TicketRoute = at.Value
                                                        ElseIf at.Name = "Distance" Then
                                                            TicketDistance = notInteger(at.Value)
                                                        End If
                                                    Next
                                                    If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                        For iIII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.Count - 1
                                                            If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "Sale" Then
                                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).HasChildNodes Then
                                                                    For iIIII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes.Count - 1
                                                                        If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "Fare" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    FareTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    FareVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    FareVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "Discount" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    DiscountTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    DiscountVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    DiscountVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "TransactionCharge" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    TransactionChargeTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    TransactionChargeVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    TransactionChargeVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "FulfilmentFee" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    FulfilmentFeeTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    FulfilmentFeeVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    FulfilmentFeeVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "CreditCardCharge" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    CreditCardChargeTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    CreditCardChargeVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    CreditCardChargeVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        End If
                                                                    Next
                                                                End If
                                                            ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "Railcard" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "Code" Then
                                                                        RailcardCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        RailcardName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "Refund" Then
                                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).HasChildNodes Then
                                                                    For iIIII As Integer = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes.Count - 1
                                                                        If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "GrossRefund" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    GrossRefundTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    GrossRefundVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    GrossRefundVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "DiscountReclaimed" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    DiscountReclaimedTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    DiscountReclaimedVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    DiscountReclaimedVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "ATOCCancellationCharge" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    ATOCCancellationChargeTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    ATOCCancellationChargeVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    ATOCCancellationChargeVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "AgencyCancellationCharge" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    AgencyCancellationChargeTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    AgencyCancellationChargeVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    AgencyCancellationChargeVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Name = "ExgratiaPayment" Then
                                                                            For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).ChildNodes(iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    ExgratiaPaymentTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    ExgratiaPaymentVATAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    ExgratiaPaymentVATCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        End If
                                                                    Next
                                                                End If
                                                            ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Name = "FareException" Then
                                                                For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(iIII).Attributes
                                                                    If at.Name = "NormalFare" Then
                                                                        NormalFare = notNumber(at.Value)
                                                                    ElseIf at.Name = "OfferedFare" Then
                                                                        OfferedFare = notNumber(at.Value)
                                                                    End If
                                                                Next
                                                            End If
                                                        Next
                                                    End If
                                                    NysDat.clsEvolvi.saveTicket(SegmentRefTicket, intEvolviID, TicketRef, TicketPassengerRef, _
                                                                                TicketTCN, TicketAdultChild, TicketCode, TicketName, _
                                                                                TicketClass, TicketSingleReturn, TicketRouteCode, _
                                                                                TicketRoute, TicketDistance, FareTotalAmount, _
                                                                                FareVATAmount, FareVATCode, DiscountTotalAmount, _
                                                                                DiscountVATAmount, DiscountVATCode, TransactionChargeTotalAmount, _
                                                                                TransactionChargeVATAmount, TransactionChargeVATCode, FulfilmentFeeTotalAmount, _
                                                                                FulfilmentFeeVATAmount, FulfilmentFeeVATCode, CreditCardChargeTotalAmount, _
                                                                                CreditCardChargeVATAmount, CreditCardChargeVATCode, NormalFare, _
                                                                                OfferedFare, RailcardCode, RailcardName, GrossRefundTotalAmount, _
                                                                                GrossRefundVATAmount, GrossRefundVATCode, DiscountReclaimedTotalAmount, DiscountReclaimedVATAmount, DiscountReclaimedVATCode, _
                                                                                ATOCCancellationChargeTotalAmount, ATOCCancellationChargeVATAmount, ATOCCancellationChargeVATCode, _
                                                                                AgencyCancellationChargeTotalAmount, AgencyCancellationChargeVATAmount, AgencyCancellationChargeVATCode, _
                                                                                ExgratiaPaymentTotalAmount, ExgratiaPaymentVATAmount, ExgratiaPaymentVATCode)
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If
                        'move the file so it doesn't get read again
                        If IO.File.Exists(arrFiles(i)) Then
                            Dim strFileName As String = System.IO.Path.GetFileName(arrFiles(i))
                            IO.File.Move(arrFiles(i), strEvolviFilesLocation & strDay & "-PASS\" & strFileName)
                        End If
                    Catch ex As Exception
                        If IO.File.Exists(arrFiles(i)) Then
                            Dim strFileName As String = System.IO.Path.GetFileName(arrFiles(i))
                            IO.File.Move(arrFiles(i), strEvolviFilesLocation & strDay & "-FAIL\" & strFileName)
                        End If
                    End Try
                Catch ex As Exception
                    If IO.File.Exists(arrFiles(i)) Then
                        Dim strFileName As String = System.IO.Path.GetFileName(arrFiles(i))
                        IO.File.Move(arrFiles(i), strEvolviFilesLocation & strDay & "-FAIL\" & strFileName)
                    End If
                End Try
            Next
        Catch ex As Exception
            log.Error("Evolvi file load error:" & ex.Message)
        End Try
    End Sub

    Protected Sub butEvolviReader_Click(sender As Object, e As EventArgs) Handles butEvolviReader.Click
        Dim strDay As String = Mid(Now.DayOfWeek.ToString, 1, 3)
        EvolviReader(strDay)
    End Sub
    '--------------------------------------------------------

    Private Function CheckConfermaEmail(ByVal pLocator As String) As String
        Return ConfermaEmail(pLocator)
    End Function

    Public Shared Function ConfermaEmail(ByVal pref As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
            Dim ret As String = ""
            Using r As IDataReader = dbh.callSP("AutoInvoiceConferma_email", "@ref", pref)
                While r.Read()
                    ret = r.GetString(0).ToString
                End While
            End Using
            Return ret
        End Using
    End Function

    Private Function CheckEvolviEmail(ByVal pLocator As String) As String
        Dim strFirstName As String = ""
        Dim strLastName As String = ""
        Dim strEmailTo As String = ""
        Dim oEvolviEmails As New List(Of clsAutoInvoice)

        oEvolviEmails = clsAutoInvoice.EvolviEmail(pLocator)
        If oEvolviEmails.Count > 0 Then
            For Each oEmail As clsAutoInvoice In oEvolviEmails
                strFirstName = oEmail.FirstName.Trim
                strLastName = oEmail.LastName.Trim
                strEmailTo = oEmail.EmailAddress.Trim
            Next
            If strEmailTo = "" Then
                'try fera/YDH  email table next
                strEmailTo = clsAutoInvoice.EmailCheck(strFirstName, strLastName)
            End If
        End If

        Return strEmailTo
    End Function

    Private Function CheckAirEmail(ByVal pLocator As String) As String
        Dim strFirstName As String = ""
        Dim strLastName As String = ""
        Dim strEmailTo As String = ""
        Dim oAirEmails As New List(Of clsAutoInvoice)

        oAirEmails = clsAutoInvoice.AirEmail(pLocator)
        If oAirEmails.Count > 0 Then
            For Each oEmail As clsAutoInvoice In oAirEmails
                Dim oSplit As Object
                If oEmail.FullName.Trim <> "" Then
                    oSplit = oEmail.FullName.Split(" ")
                    strFirstName = oSplit(0).ToString.Trim
                    strLastName = oSplit(1).ToString.Trim
                End If
            Next
            If strFirstName <> "" Or strLastName <> "" Then
                strEmailTo = clsAutoInvoice.EmailCheck(strFirstName, strLastName)
                'if stll nothing then try switching
                If strEmailTo = "" Then
                    strEmailTo = clsAutoInvoice.EmailCheck(strLastName, strFirstName)
                End If
            End If
        End If

        Return strEmailTo
    End Function


    Protected Sub butAutoInvoiceProcess_Click(sender As Object, e As EventArgs) Handles butAutoInvoiceProcess.Click



        'Dim strcompanyid As String = ""
        'Dim strgroupid As String = Me.txtGroup.Text
        'strcompanyid = NSConfigUtils.GetCompany(CInt(Me.txtCompany.Text), strgroupid)

        'Dim t As String = strgroupid ' Me.txtGroup.Text '"100040"
        'Dim ret As Decimal = 0
        ''Dim FeePerc As Decimal = NSConfigUtils.ReadConfigRateWithVenueEnquiry(pintenquiryvenueid, )
        'Dim Rates As String
        'Rates = getConfig("NYSNonCommClientsRates")
        ''  <add key="NYSNonCommClientsRates" value="100040=004.7500;100076=006.0000;100101=000.0000"/>  e.g. 100101 non at group but some at company
        'If (Rates.IndexOf(t) <> -1) Then
        '    Dim st As Integer = Rates.IndexOf(t) + 7
        '    ret = CDec(Rates.Substring(st, 8))
        'Else
        '    ret = 0
        'End If

        'Dim tg As String = strcompanyid 'Me.txtCompany.Text  ' "100167"
        'Rates = getConfig("NYSNonCommClientsCompanyRates")
        'If (Rates.IndexOf(tg) <> -1) Then
        '    Dim st As Integer = Rates.IndexOf(tg) + 7
        '    ret = CDec(Rates.Substring(st, 8))
        'Else 'default back to group rate
        '    Rates = getConfig("NYSNonCommClientsRates")
        '    If (Rates.IndexOf(t) <> -1) Then
        '        Dim st As Integer = Rates.IndexOf(t) + 7
        '        ret = CDec(Rates.Substring(st, 8))
        '    Else
        '        ret = 0
        '    End If
        'End If
        'Me.txtRate.Text = ret.ToString()


        Dim decCalcdNet As Decimal = 0
        Dim decCalcdVat As Decimal = 0

        Dim enqeventid As Integer = 358850
        Dim envid As Integer = 209770
        Dim FeePerc As Decimal = 2.75 'NSConfigUtils.ReadConfigRateWithVenueEnquiry(enqeventid, envid)

        Dim oCosts As New List(Of clsEnquiryVenue)
        oCosts = clsEnquiryVenue.enquiryVenueValueGet(enqeventid)

        For Each oCost As clsEnquiryVenue In oCosts
            decCalcdNet = CDec(Math.Round((oCost.nett / 100) * FeePerc, 2))
            decCalcdVat = (decCalcdNet / 100) * NysDat.clsEnquiryVenueDat.getVenueVatRate(enqeventid, False)

        Next


        Exit Sub

        'Dim strEvolviFilesLocation As String = getConfig("EvolviHandoffs")

        'Dim fileCount As Integer = System.IO.Directory.GetFiles(strEvolviFilesLocation).Length   '  \\nysmgmt\EvolviHandoffs\intercept\SQL\
        'Dim fileCount1 As Integer = System.IO.Directory.GetFiles(strEvolviFilesLocation, "*.xml").Length   '  \\nysmgmt\EvolviHandoffs\intercept\SQL\

        'Dim strDatePath As String = "_" & CStr(Date.Now.Year) & IIf(Date.Now.Month < 10, "0" & CStr(Date.Now.Month), CStr(Date.Now.Month))
        'Dim strReadFromFolder As String = getConfig("InvoiceInput")

        'strReadFromFolder = "\\nys-boss\BOSS\32bitboss\PDFfiles\awaiting\"
        'Dim strSentFolder As String = getConfig("InvoiceSent").Replace("###", strDatePath)
        'Dim strNotSentFolder As String = getConfig("InvoiceNotSent").Replace("###", strDatePath)
        'Dim strErrorFolder As String = getConfig("InvoiceError").Replace("###", strDatePath)
        'Dim strAwaitingFolder As String = getConfig("InvoiceAwaiting")

        'Dim strCompanyName As String = ""

        'Dim diInputFolder As New DirectoryInfo(strReadFromFolder)
        'For Each aFile As FileSystemInfo In diInputFolder.GetFiles
        '    Dim strFileName As String = aFile.Name
        '    Dim oInvoices As New List(Of clsAutoInvoice)
        '    'Dim oEvolviEmails As New List(Of clsAutoInvoice)
        '    Dim oAirEmails As New List(Of clsAutoInvoice)
        '    'R2.21 
        '    Dim oConfermaEmails As New List(Of clsAutoInvoice)
        '    If strFileName.ToLower.Contains(".pdf") Then
        '        oInvoices = clsAutoInvoice.checkInvoice(strFileName.ToLower.Replace(".pdf", ""))

        '        If oInvoices.Count > 0 Then 'should actually only be one!
        '            If IO.File.Exists(strSentFolder & strFileName) Then 'file has already been sent so don't bother again

        '                'IO.File.Copy(strReadFromFolder & strFileName, strSentFolder & strFileName, True)
        '                'IO.File.Delete(strReadFromFolder & strFileName)
        '            Else
        '                For Each oInvoice As clsAutoInvoice In oInvoices
        '                    Try
        '                        'R2.21 SA 
        '                        strCompanyName = oInvoice.Client.ToUpper.Trim

        '                        If strCompanyName = "FERA" Or strCompanyName = "YDH R" Or strCompanyName = "YDH" Then
        '                            Dim strFirstName As String = ""
        '                            Dim strLastName As String = ""
        '                            Dim strEmailTo As String = ""

        '                            If strCompanyName = "FERA" AndAlso oInvoice.PONumber.Trim <> "" AndAlso IsNumeric(oInvoice.PONumber.Trim.ToLower.Replace("p", "")) Then
        '                                Try
        '                                    'Dim inttest As Integer = CInt(oInvoice.PONumber.Trim.ToLower.Replace("p", ""))
        '                                    'Dim strtest As String = oInvoice.PONumber.Trim.ToLower.Replace("p", "")
        '                                    ' If IsNumeric(strtest) Then
        '                                    'all good so send to proc email
        '                                    strEmailTo = getConfig("FeraProcurementEmail")
        '                                    'End If

        '                                Catch ex As Exception
        '                                    'rail 
        '                                    strEmailTo = CheckEvolviEmail(oInvoice.RecordLocator.Trim)
        '                                    'Air
        '                                    If strEmailTo = "" Then
        '                                        strEmailTo = CheckAirEmail(oInvoice.RecordLocator.Trim)
        '                                    End If
        '                                End Try
        '                            Else
        '                                If oInvoice.RecordLocator.Trim <> "" Then
        '                                    'R2.21 SA
        '                                    If IsNumeric(oInvoice.RecordLocator.Trim) Then  'RAIL
        '                                        strEmailTo = CheckEvolviEmail(oInvoice.RecordLocator.Trim)
        '                                    Else
        '                                        'CHECK AIR
        '                                        strEmailTo = CheckAirEmail(oInvoice.RecordLocator.Trim)
        '                                        'CHECK HOTEL
        '                                        If strEmailTo = "" Then
        '                                            strEmailTo = CheckConfermaEmail(oInvoice.RecordLocator.Trim)
        '                                        End If
        '                                    End If
        '                                ElseIf oInvoice.RecordLocator.Trim = "" AndAlso strCompanyName = "YDH R" AndAlso oInvoice.Supplier = "GENPOST" Then
        '                                    strEmailTo = getConfig("YDHRPostChargeEmail")
        '                                Else
        '                                    'no locator  
        '                                    strEmailTo = ""
        '                                End If
        '                            End If

        '                            'make sure email is in the correct format
        '                            Dim mRegExp As New Regex(emailRegex)
        '                            If Not mRegExp.IsMatch(strEmailTo) Then
        '                                strEmailTo = "" ' getConfig("FeraProcurementEmail")
        '                            End If

        '                            If strEmailTo <> "" Then

        '                                Dim blnSent As Boolean = sendAutoInvoiceEmail(strEmailTo, getConfig("AutoInvoiceEmailFrom"), strReadFromFolder & strFileName, strFirstName, False)
        '                                If blnSent Then

        '                                    ''move to sent folder
        '                                    'If IO.File.Exists(strSentFolder & strFileName) Then
        '                                    '    IO.File.Copy(strReadFromFolder & strFileName, strSentFolder & strFileName, True)
        '                                    '    IO.File.Delete(strReadFromFolder & strFileName)
        '                                    'Else
        '                                    '    IO.File.Move(strReadFromFolder & strFileName, strSentFolder & strFileName)
        '                                    '    'IO.File.Delete(strReadFromFolder & strFileName)
        '                                    'End If
        '                                Else
        '                                    'R2.21 SA - log per company name 
        '                                    lstEmail.Items.Add("1" + strCompanyName + " " + oInvoice.InvoiceNumber.ToString + " " + strEmailTo)

        '                                    'sendAutoInvoiceEmail("", "", strFileName, "", True)
        '                                    'move to error folder
        '                                    'If IO.File.Exists(strErrorFolder & strFileName) Then
        '                                    '    IO.File.Copy(strReadFromFolder & strFileName, strErrorFolder & strFileName, True)
        '                                    '    IO.File.Delete(strReadFromFolder & strFileName)
        '                                    'Else
        '                                    '    IO.File.Move(strReadFromFolder & strFileName, strErrorFolder & strFileName)
        '                                    '    ' IO.File.Delete(strReadFromFolder & strFileName)
        '                                    'End If
        '                                End If
        '                            Else
        '                                'R2.21 SA - log per company name 
        '                                lstEmail.Items.Add("2" + strCompanyName + " " + oInvoice.InvoiceNumber.ToString + " " + strEmailTo)
        '                                'move to error folder
        '                                'If IO.File.Exists(strErrorFolder & strFileName) Then
        '                                '    IO.File.Copy(strReadFromFolder & strFileName, strErrorFolder & strFileName, True)
        '                                '    IO.File.Delete(strReadFromFolder & strFileName)
        '                                'Else
        '                                '    IO.File.Move(strReadFromFolder & strFileName, strErrorFolder & strFileName)
        '                                '    'IO.File.Delete(strReadFromFolder & strFileName)
        '                                'End If
        '                            End If
        '                        Else
        '                            'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
        '                            'If DateTime.Now.Hour >= notInteger(getConfig("StartMovePDFFilesHour")) Then
        '                            '    'definately not Fera so move
        '                            '    If IO.File.Exists(strNotSentFolder & strFileName) Then
        '                            '        IO.File.Copy(strReadFromFolder & strFileName, strNotSentFolder & strFileName, True)
        '                            '        IO.File.Delete(strReadFromFolder & strFileName)
        '                            '    Else
        '                            '        IO.File.Move(strReadFromFolder & strFileName, strNotSentFolder & strFileName)

        '                            '        ' IO.File.Delete(strReadFromFolder & strFileName)
        '                            '    End If
        '                            'End If
        '                        End If

        '                    Catch ex As Exception
        '                        lstEmail.Items.Add("3" + strCompanyName + " " + oInvoice.InvoiceNumber.ToString + " ")
        '                        'R2.21 SA - log for company name instead of just for FERA
        '                        'log.Error("Failure retrieving email - " & strCompanyName & ": " & strFileName & " **Ex.Message=" & ex.Message)
        '                        'sendAutoInvoiceEmail("", "", strFileName, "", True)
        '                        'move to error folder
        '                        'If IO.File.Exists(strErrorFolder & strFileName) Then
        '                        '    IO.File.Copy(strReadFromFolder & strFileName, strErrorFolder & strFileName, True)
        '                        '    IO.File.Delete(strReadFromFolder & strFileName)
        '                        'Else
        '                        '    IO.File.Move(strReadFromFolder & strFileName, strErrorFolder & strFileName)
        '                        '    ' IO.File.Delete(strReadFromFolder & strFileName)
        '                        'End If
        '                    End Try
        '                Next
        '            End If
        '        Else
        '            'could still be Fera / YDH , but record not yet transfered to SQL from BOSS
        '            'need to move to diff folder so can poll that at a later date

        '            'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
        '            'If DateTime.Now.Hour >= notInteger(getConfig("StartMovePDFFilesHour")) Then
        '            '    If IO.File.Exists(strAwaitingFolder & strFileName) Then
        '            '        IO.File.Delete(strAwaitingFolder & strFileName)
        '            '        IO.File.Move(strReadFromFolder & strFileName, strAwaitingFolder & strFileName)
        '            '        'IO.File.Delete(strReadFromFolder & strFileName)
        '            '    Else
        '            '        IO.File.Move(strReadFromFolder & strFileName, strAwaitingFolder & strFileName)
        '            '        'IO.File.Delete(strReadFromFolder & strFileName)
        '            '    End If
        '            'End If
        '        End If
        '    Else
        '        'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
        '        'If DateTime.Now.Hour >= notInteger(getConfig("StartMovePDFFilesHour")) Then
        '        '    'definately not Fera so move
        '        '    If IO.File.Exists(strNotSentFolder & strFileName) Then
        '        '        IO.File.Copy(strReadFromFolder & strFileName, strNotSentFolder & strFileName, True)
        '        '        IO.File.Delete(strReadFromFolder & strFileName)
        '        '    Else
        '        '        IO.File.Move(strReadFromFolder & strFileName, strNotSentFolder & strFileName)
        '        '        'IO.File.Delete(strReadFromFolder & strFileName)
        '        '    End If
        '        'End If

        '    End If
        'Next
        ''    Else
        'lstEmail.DataBind()

        'Dim test1 As String = CheckConfermaEmail("B156239")
        'Dim test2 As String = CheckConfermaEmail("B139319")
        'Exit Sub


    End Sub

    Private Function sendAutoInvoiceEmail(ByVal pEmailTo As String, _
                                  ByVal pEmailFrom As String, _
                                  ByVal pFile As String, _
                                  ByVal pFirstName As String, _
                                  ByVal pError As Boolean) As Boolean

        '<add key="HomeAbsolutepath" value="\\NYSMevis\"/>


        If Not pError Then
            Dim ofile As New System.IO.StreamReader("\\NYSMevis\userdocs\NYS\AutoInvoiceEmail.htm") ' getConfig("HomeAbsolutepath") & "userdocs\NYS\AutoInvoiceEmail.htm")

            Dim strreadtest As String = ofile.ReadToEnd & "<p>"
            ofile.Close()

            If pFirstName <> "" Then
                strreadtest = strreadtest.Replace("#firstname#", pFirstName)
            Else
                strreadtest = strreadtest.Replace("Dear #firstname#&nbsp;<br />", "")
            End If

            Dim strMessage As String = strreadtest

            Dim strfrom As String = pEmailFrom
            Dim strto As String = pEmailTo
            If strfrom = "" Then
                strfrom = "rail@nysgroup.com"
            End If

            If getConfig("ReleaseEmailTest") = "true" Then
                strto = getConfig("ReleaseEmailTestSend")
                strMessage = "Email would have been sent to: " & pEmailTo & " - " & strMessage
            End If

            Try
                SendEmail.send(strfrom, strto, "PDF Invoice from NYS Corporate", strMessage, "conference3@nysgroup.com", "", "", pFile)
                Return True
            Catch ex As Exception
                log.Error("ERROR IN SENDAUTOINVOICEEMAIL: " & ex.Message)
                Return False
            End Try
        Else
            Try
                Dim strProduct As String = clsAutoInvoice.GetInvoiceType(pFile.Replace(".pdf", ""))
                If strProduct = "Rail" Then
                    SendEmail.send("RailBookings@nysgroup.com", "RailBookings@nysgroup.com", "FERA/YDH " & strProduct & " PDF Invoice could not be sent to the booker", "Invoice: " & pFile, "", "", "", "")
                Else
                    SendEmail.send("RailBookings@nysgroup.com", "sarab.azzouz@nysgroup.com", "FERA/YDH " & strProduct & " PDF Invoice could not be sent to the booker", "Invoice: " & pFile, "", "", "", "")
                End If

                Return True
            Catch ex As Exception
                log.Error("ERROR IN SENDAUTOINVOICEEMAIL: " & ex.Message)
                Return False
            End Try
        End If
    End Function


End Class