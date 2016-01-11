Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail
Imports NysDat
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Xml
Imports System.IO

Partial Public Class IATestEvolvi
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
                End If
                'ProcessFolder(getConfig("InterceptorProcessFolder"), getConfig("InterceptorOutputFolder"))
            Catch ex As Exception
                handleexception(ex, "IATestEvolvi", Me.Page)
            End Try
        End Using
    End Sub

    'R2.16 CR
    Private Sub InterceptorGuardian(ByVal pstrReadFromFolder As String, ByVal pstrWriteToFolder As String)
        'get hold of the files
        Dim fstOriginalFile As FileStream
        Dim xrdOriginalFile As XmlReader
        Dim fstNewFile As FileStream
        Dim xwrNewFile As XmlTextWriter


        Try
            Dim diInputFolder As New DirectoryInfo(pstrReadFromFolder)
            Dim diOutputFolder As New DirectoryInfo(pstrWriteToFolder)

            If diInputFolder.Exists And diOutputFolder.Exists Then
                'loop through all the files in the folder
                For Each File As FileSystemInfo In diInputFolder.GetFiles
                    'open the file and read it's contents
                    fstOriginalFile = New FileStream(File.FullName, FileMode.Open)
                    xrdOriginalFile = XmlReader.Create(fstOriginalFile)

                    Dim dblSchemaVersion As Double = 0
                    Dim intNumberOfBookings As Integer = 0
                    Dim strOrderRef As String = ""

                    'R2.22 CR - count the refunds instead
                    'R2.21 CR
                    'Dim blnIsRefund As Boolean = False
                    Dim intRefunds As Integer = 0
                    Dim blnInsideRefund As Boolean = False
                    Dim blnIsValidRefund As Boolean = False
                    Dim lstInvalidRefunds As New List(Of Integer)

                    'loop through the xml nodes
                    While xrdOriginalFile.Read
                        If xrdOriginalFile.Name.ToLower <> "xml" Then
                            If xrdOriginalFile.Name.ToLower = "my:handoff" Then
                                If xrdOriginalFile.IsStartElement Then
                                    'get the version number
                                    dblSchemaVersion = notDouble(xrdOriginalFile.GetAttribute("SchemaVersion"))
                                End If
                            End If

                            'R2.21 CR
                            If xrdOriginalFile.Name.ToLower = "immediatedetail" AndAlso _
                                xrdOriginalFile.IsStartElement AndAlso _
                                xrdOriginalFile.GetAttribute("TransactionType").ToLower = "refund" Then
                                'blnIsRefund = True
                                intRefunds += 1

                                'R2.22 CR - open tag so set the boolean
                                blnInsideRefund = True
                            End If

                            'R2.22 CR - check to see if the refund has a refund tag, if it doesn't then it's invalid
                            If blnInsideRefund AndAlso xrdOriginalFile.Name.ToLower = "refund" Then
                                blnIsValidRefund = True
                            End If

                            If xrdOriginalFile.Name.ToLower = "immediatedetail" And xrdOriginalFile.IsStartElement Then
                                intNumberOfBookings += 1
                            End If

                            'R2.22 CR - close tag, so release the booleans
                            If xrdOriginalFile.Name.ToLower = "immediatedetail" AndAlso _
                                Not xrdOriginalFile.IsStartElement Then
                                'check to see if this refund was valid or not
                                'if not valid add the index to the invalid list for skipping later
                                If blnInsideRefund AndAlso blnIsValidRefund = False Then
                                    lstInvalidRefunds.Add(intNumberOfBookings - 1)
                                End If

                                'reset the booleans
                                blnInsideRefund = False
                                blnIsValidRefund = False
                            End If
                        End If
                    End While

                    'R2.21 CR
                    If intRefunds > 0 Then
                        'take a copy of the file, we need to see it in it's original form
                        IO.File.Copy(File.FullName, getConfig("EvolviGuardianRefundCopies") & File.Name, True)
                    End If

                    If dblSchemaVersion >= 4.0 Then
                        'uh-oh! we've got to do some work; "Visual Basic powers..... activate!"

                        'split the file by the number of immediate detail nodes
                        Dim x As Integer = 0
                        For x = 0 To intNumberOfBookings - 1

                            'R2.22 CR - stop any invalid refunds from getting created
                            If lstInvalidRefunds.Count > 0 AndAlso lstInvalidRefunds.Contains(x) Then
                                'do nothing
                                'skip this immediatedetail node - it's an invalid refund
                            Else
                                'release and re-load the original file
                                xrdOriginalFile.Close()
                                xrdOriginalFile = Nothing
                                fstOriginalFile.Close()
                                fstOriginalFile = Nothing
                                fstOriginalFile = New FileStream(File.FullName, FileMode.Open)
                                xrdOriginalFile = XmlReader.Create(fstOriginalFile)

                                Dim strFileName As String = ""
                                Dim strNewSequenceNo As String = ""

                                strFileName = File.Name

                                If x = 0 Then
                                    'first file, keep the name the same!
                                Else
                                    'work out the file name, need to alter the sequence no
                                    Dim strFileLetter As String = getInterceptorFileLetter(x)

                                    'place the letter at the start of the sequence number
                                    strNewSequenceNo = Replace(strFileLetter & strFileName.Substring((strFileName.LastIndexOf("_") + 1) + strFileLetter.Length), ".xml", "")

                                    strFileName = strFileName.Substring(0, strFileName.LastIndexOf("_") + 1) & strNewSequenceNo & ".xml"
                                End If

                                fstNewFile = New FileStream(pstrWriteToFolder & strFileName, FileMode.Create)
                                xwrNewFile = New XmlTextWriter(fstNewFile, System.Text.Encoding.UTF8)
                                xwrNewFile.Formatting = Formatting.Indented
                                xwrNewFile.WriteStartDocument()

                                Dim intImmediateDetailCount As Integer = 0
                                While xrdOriginalFile.Read
                                    If xrdOriginalFile.Name.ToLower <> "xml" Then
                                        If xrdOriginalFile.Name.ToLower = "my:handoff" AndAlso xrdOriginalFile.IsStartElement Then
                                            'Replace the sequence number!
                                            xwrNewFile.WriteRaw(GuardianProcessXML(xrdOriginalFile, 0, False, strNewSequenceNo, False, "SequenceNumber"))
                                            xrdOriginalFile.MoveToElement()

                                            'R2.26 SA 
                                        ElseIf xrdOriginalFile.Name.ToLower = "my:Handoff" AndAlso xrdOriginalFile.IsStartElement Then
                                            xwrNewFile.WriteRaw(GuardianProcessXML(xrdOriginalFile, 0, False, "3.9", False, "SchemaVersion"))
                                            xrdOriginalFile.MoveToElement()

                                        ElseIf xrdOriginalFile.Name.ToLower = "immediatedetail" AndAlso xrdOriginalFile.IsStartElement Then
                                            intImmediateDetailCount += 1

                                            If intImmediateDetailCount = x + 1 Then
                                                'write the details
                                                'also rename the "OrderItemRef" attribute to "IssueRef"
                                                xwrNewFile.WriteRaw(Replace(GuardianProcessXML(xrdOriginalFile, 0, False, "", False), "OrderItemRef", "IssueRef"))
                                                xrdOriginalFile.MoveToElement()
                                            Else
                                                'skip past everything inside this immediate detail node - including all children
                                                readToNextSibling(xrdOriginalFile)
                                            End If
                                        Else
                                            'write the raw data
                                            xwrNewFile.WriteRaw(GuardianProcessXML(xrdOriginalFile, 0, False, "", False))
                                            xrdOriginalFile.MoveToElement()
                                        End If
                                    End If
                                End While

                                xwrNewFile.Close()
                                fstNewFile.Close()
                                xrdOriginalFile.Close()
                                fstOriginalFile.Close()
                            End If
                        Next

                    Else
                        'release and re-load the original file
                        xrdOriginalFile.Close()
                        xrdOriginalFile = Nothing
                        fstOriginalFile.Close()
                        fstOriginalFile = Nothing
                        fstOriginalFile = New FileStream(File.FullName, FileMode.Open)
                        xrdOriginalFile = XmlReader.Create(fstOriginalFile)

                        'old file verion, just send it through to interceptor - no work to be done by Guardian
                        fstNewFile = New FileStream(pstrWriteToFolder & File.Name, FileMode.Create)
                        xwrNewFile = New XmlTextWriter(fstNewFile, System.Text.Encoding.UTF8)
                        xwrNewFile.Formatting = Formatting.Indented
                        xwrNewFile.WriteStartDocument()

                        While xrdOriginalFile.Read
                            If xrdOriginalFile.Name <> "xml" Then
                                xwrNewFile.WriteRaw(GuardianProcessXML(xrdOriginalFile, 0, False, "", False))
                                xrdOriginalFile.MoveToElement()
                            End If
                        End While

                        xwrNewFile.Close()
                        fstNewFile.Close()
                        xrdOriginalFile.Close()
                        fstOriginalFile.Close()
                    End If

                    'bin when done
                    IO.File.Delete(File.FullName)
                Next
            Else
                log.Error("Interceptor Guardian - One of the specified folders does not exist")
            End If

        Catch ex As Exception
            xrdOriginalFile = Nothing
            xrdOriginalFile.Close()
            xwrNewFile = Nothing
            xwrNewFile.Close()
            fstNewFile = Nothing
            fstNewFile.Close()
            fstOriginalFile = Nothing
            fstOriginalFile.Close()

            log.Error("Read Failure - Interceptor Guardian: " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' pass in the order reference to save to database 
    ''' pass client boss code and offline (true/false) to work out the correct fees 
    ''' </summary>
    ''' <param name="pstrOrderRef"></param>
    ''' <param name="pstrBossCode"></param>
    ''' <param name="pblnOffline"></param>
    ''' <remarks></remarks>
    Private Sub SaveCurdianCharges(ByVal pstrOrderRef As String, ByVal pstrDeliverymethod As String, ByVal pstrBossCode As String, ByVal pblnOffline As Boolean)

        Dim strIssueDate As String = Date.Now.ToString(getConfig("RailIssueDateFormat"))
        Dim dblOnlineFeeAdded As Boolean = False
        Dim dblOfflineFeeAdded As Boolean = False
        Dim dblKioskOnlineFeeAdded As Boolean = False
        Dim dblKioskofflineFeeAdded As Boolean = False
        Dim dblToDOnlineFeeAdded As Boolean = False
        Dim dblPostageFeeAdded As Boolean = False
        Dim dblSpecialDeliveryFeeAdded As Boolean = False
        Dim dblSaturdaySpecialDeliveryFeeAdded As Boolean = False

        'get client details
        Dim oInterceptorClient As clsInterceptorClient
        oInterceptorClient = clsInterceptorClient.getByBossCode(pstrBossCode)

        'check fees are active 
        If oInterceptorClient.InterceptorFeesActive Then
            'If pblnOffline = True Then 'charge offline fee 
            '    If oInterceptorClient.OfflineFee > 0 AndAlso oInterceptorClient.OfflineFeePerBasket = True Then
            '        clsInterceptorCharges.saveGuardianCharges(pstrOrderRef, "Offline", oInterceptorClient.OfflineFee, strIssueDate, pstrBossCode)
            '    End If
            'Else 'cherge online fee
            '    If oInterceptorClient.OnlineFee > 0 AndAlso oInterceptorClient.OnlineFeePerBasket = True Then
            '        clsInterceptorCharges.saveGuardianCharges(pstrOrderRef, "Online", oInterceptorClient.OnlineFee, strIssueDate, pstrBossCode)
            '    End If

            'End If


            ''Kiosk Offline Fee 
            'If oInterceptorClient.KioskOfflineFee > 0 AndAlso oInterceptorClient.KioskOfflineFeePerBasket = True Then
            '    clsInterceptorCharges.saveGuardianCharges(pstrOrderRef, "KioskOffline", oInterceptorClient.KioskOfflineFee, strIssueDate, pstrBossCode)
            'End If

            ''ToD offline Fee 
            'If oInterceptorClient.ToDOfflineFee > 0 AndAlso oInterceptorClient.ToDOfflineFeePerBasket = True Then
            '    clsInterceptorCharges.saveGuardianCharges(pstrOrderRef, "ToDOffline", oInterceptorClient.ToDOfflineFee, strIssueDate, pstrBossCode)
            'End If




            ''Kiosk Online Fee 
            'If oInterceptorClient.KioskOfflineFee > 0 AndAlso oInterceptorClient.KioskOfflineFeePerBasket = True Then
            '    clsInterceptorCharges.saveGuardianCharges(pstrOrderRef, "KioskOnline", oInterceptorClient.KioskOfflineFee, strIssueDate, pstrBossCode)
            'End If

            ''ToD online Fee 
            'If oInterceptorClient.ToDOfflineFee > 0 AndAlso oInterceptorClient.ToDOfflineFeePerBasket = True Then
            '    clsInterceptorCharges.saveGuardianCharges(pstrOrderRef, "ToDOnline", oInterceptorClient.ToDOfflineFee, strIssueDate, pstrBossCode)
            'End If





        End If




    End Sub



    ''' <summary>
    ''' Pass in a number, upper case letter of the alphabet is returned (zero based). eg: 0 = "", 1 = "A", 2 = "B"
    ''' For values over 26 double letters are returned.
    ''' For Values over 52 "error" is returned.
    ''' </summary>
    ''' <param name="pintNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getInterceptorFileLetter(pintNumber As Integer) As String
        If pintNumber <= 52 Then

            'Create a collection to hold the letters
            Dim lstLetters(52) As String
            lstLetters(0) = ""

            Dim intTotalLoops As Integer = 1
            If pintNumber > 26 Then
                intTotalLoops = 2
            End If

            Dim intSkip As Integer = 0

            'loop for number of characters needed
            For y = 1 To intTotalLoops
                intSkip = ((y - 1) * 26)
                'go through all 26 letters
                For i = 1 To 26
                    'add the letter, or if on second loop add an additional letter
                    lstLetters(i + intSkip) += lstLetters(i) & (Convert.ToChar(i + 96)).ToString.ToUpper
                Next
            Next

            Return lstLetters(pintNumber)
        Else
            Return "error"
        End If

    End Function

    'R2.10 CR
    ''' <summary>
    ''' Advances Xml reader to the close tag of the current node. Child nodes are skipped. 
    ''' If current element is empty then reader does not advance.
    ''' If no end tag is found then it will advance to the end of the file.
    ''' </summary>
    ''' <param name="pxmlReader"></param>
    ''' <remarks></remarks>
    Private Sub readToNextSibling(ByRef pxmlReader As XmlReader)
        'get the node name that we are skipping
        Dim strNodeName As String = pxmlReader.Name

        'Check to see if it is an empty element
        If Not pxmlReader.IsEmptyElement Then
            'move to the next node
            pxmlReader.Read()

            'advance to the close tag, or end of file
            While Not pxmlReader.EOF AndAlso pxmlReader.Name <> strNodeName
                pxmlReader.Read()
            End While
        End If

    End Sub

    'Private Function workOutFee(ByVal dblTargetTotal As Double, _
    '                            ByVal intNumberToSplit As Integer, _
    '                            ByVal intCurrentStage As Integer, _
    '                            ByRef dblRunningTot As Double, _
    '                            ByVal blnIsChargePerTicket As Boolean, _
    '                            Optional ByVal intPassengerTickets As Integer = 0)

    '    Dim dblTest As Double = 0

    '    If intNumberToSplit <> intCurrentStage Then
    '        dblTest = (dblTargetTotal / intNumberToSplit)

    '        'if passenger has more than 1 ticket and the charge is per booking
    '        ' then the fee needs splitting accross all tickets & files
    '        If intPassengerTickets > 1 AndAlso blnIsChargePerTicket = False Then
    '            dblTest = (dblTest / intPassengerTickets)
    '        End If

    '        dblTest = Math.Round(dblTest, 2)
    '    Else
    '        'on last stage
    '        'the value will be the difference between the target total and the running tot
    '        dblTest = dblTargetTotal - dblRunningTot
    '    End If

    '    dblRunningTot += dblTest

    '    Return dblTest
    'End Function

    Private Function workOutFee(ByVal dblFeeAmount As Double, _
                           ByVal intTotalTickets As Integer, _
                           ByRef dblRunningTot As Double, _
                           ByVal blnChargeIsPerTicket As Boolean, _
                           ByVal intCurrentTicket As Integer, _
                           ByVal intTotalFiles As Integer) As Double
        Dim dblSplitFee As Double = 0

        If blnChargeIsPerTicket Then
            'charge is per ticket - just send back the charge
            dblSplitFee = dblFeeAmount
        Else
            'charge is per booking - need to split charge across all passengers & tickets
            'split the fee by number of files - so that all passengers are charged the same amount regardless of number of tickets
            If intCurrentTicket = intTotalTickets Then
                'on last ticket - do a subtract to round off the fee (just in case we have any odd fractions)
                dblSplitFee = dblFeeAmount - dblRunningTot
            Else
                dblSplitFee = dblFeeAmount / intTotalTickets
            End If
        End If

        'format to 2 dp
        dblSplitFee = Math.Round(dblSplitFee, 2)

        dblRunningTot += dblSplitFee

        Return dblSplitFee
    End Function

    Public Sub ProcessFolder(ByVal pstrReadFromFolder As String, ByVal pstrWriteToBossFolder As String, ByVal pstrWriteToSqlFolder As String)

        Dim fstOldFile As FileStream
        Dim fstBOSSFile As FileStream
        Dim xmlreader As XmlReader
        Dim xmlBOSSwriter As XmlTextWriter

        'R2.3 CR
        Dim fstSQLFile As FileStream
        Dim xmlSQLwriter As XmlTextWriter

        Try
            Dim diInputFolder As New DirectoryInfo(pstrReadFromFolder)
            Dim diOutputBossFolder As New DirectoryInfo(pstrWriteToBossFolder)

            'R2.3 CR
            Dim diOutputSqlFolder As New DirectoryInfo(pstrWriteToSqlFolder)

            If diInputFolder.Exists And diOutputBossFolder.Exists And diOutputSqlFolder.Exists Then
                Dim count As Integer = 0
                'loop through all files in folder
                For Each File As FileSystemInfo In diInputFolder.GetFiles
                    Dim strBossCode As String = ""
                    Dim blnAddOfflineFee As Boolean = False
                    Dim strMachineNumber As String = ""
                    Dim dblSpecialDelivery As Double = 0
                    Dim blnPostageFeeAdded As Boolean = False
                    Dim strCustomfieldCC As String = ""
                    Dim strCustomfieldCREF1 As String = ""
                    Dim blnSkipDeliveryCharge As Boolean = False
                    Dim dicPassengerSkipPost As New Dictionary(Of String, Boolean)
                    Dim strCurrentPassengerRef As String = ""
                    Dim strCurrentFulfilmentRef As String = ""

                    'R2.5 CR
                    Dim blnIsRefund As Boolean = False
                    Dim blnChangeAgencyCancellation As Boolean = False

                    'R2.3 CR
                    Dim dblRefundCancellationCharge As Double = 0

                    'R2.9.1 CR
                    Dim strCustomFieldPO As String = ""

                    'R2.10 CR
                    Dim dblRunningTotOfflineFee As Double = 0
                    Dim dblRunningTotPostageFee As Double = 0
                    Dim dblRunningTotSpecialDel As Double = 0
                    Dim dblRunningTotTransaction As Double = 0
                    Dim intTotalTickets As Integer = 0

                    'R2.13 CR
                    Dim strSplitNode As String = ""
                    Dim strSplitFieldName As String = ""
                    Dim strSplitFieldValue As String = ""
                    Dim strSplitUniqueFieldName As String = ""
                    Dim dictPassengerVSplitValue As New Dictionary(Of String, String)
                    Dim oInterceptorClient As New clsInterceptorClient

                    'R2.14 CR
                    Dim strIssueRef As String = ""

                    'R2.20B SA
                    Dim strExternalRefValue As String = ""
                    Dim blnHerfordshireCheck As Boolean = False

                    'R2.20C CR
                    Dim blnIsDataError As Boolean = False
                    Dim blnLvTravellerProvided As Boolean = False
                    Dim blnLvProjectCode As Boolean = False

                    'R2.20D SA
                    Dim strMckTravellerCostCentre As String = ""
                    Dim strMckTravellerHRNumber As String = ""
                    Dim blnMckOptionalCostCentre As Boolean = False
                    Dim strTransactionType As String = ""

                    'R1.3 SA 
                    Dim strPikselTravellerEmployeeNumber As String = ""
                    Dim strPikselTravellerPO As String = ""
                    Dim strPikselTravellerBussinessUnit As String = ""
                    Dim strPikselTravellerDepartment As String = ""
                    Dim strPikselTravellerLegalEntity As String = ""
                    Dim strPikselTravellerLocation As String = ""
                    Dim strPikselTravellerGLCode As String = ""

                    'R2.23 SA 
                    Dim blnNonIssue As Boolean = False

                    'R2.17 CR
                    Dim blnFulfilmentFeePresent As Boolean = False

                    'R2.22.1 SA 
                    Dim strOrderRef As String = ""
                    'R2.22 SA
                    Dim strFulilmentType As String = ""
                    Dim strIssueDate As String = Date.Now.ToString(getConfig("RailIssueDateFormat"))

                    'R2.21.9 SA 
                    Dim intTotalPassengers As Integer = 0
                    Dim blnFeeAdded As Boolean = False

                    'R2.25 SA 
                    Dim strKcomPayrollNumber As String = ""

                    count = count + 1
                    fstOldFile = New FileStream(File.FullName, FileMode.Open)
                    xmlreader = xmlreader.Create(fstOldFile)

                    'R2.16 CR
                    Dim dblSchemaVersion As Double = 0

                    'read through document and get attributes needed in next section
                    While xmlreader.Read
                        If xmlreader.Name = "Account" And xmlreader.IsStartElement Then
                            strBossCode = xmlreader.GetAttribute("ExternalRef")
                            'R2.13 CR
                            oInterceptorClient = clsInterceptorClient.checkBossCode(strBossCode, 0)
                            If oInterceptorClient.ClientID > 0 Then
                                strSplitNode = oInterceptorClient.SplitNodeName
                                strSplitFieldName = oInterceptorClient.SplitFieldName
                                strSplitFieldValue = oInterceptorClient.SplitFieldValue
                                strSplitUniqueFieldName = oInterceptorClient.SplitUniqueFieldName
                            End If
                            oInterceptorClient = Nothing
                            'R2.20B SA
                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "1" AndAlso strBossCode.ToUpper = "HERECC" Then
                            If xmlreader.GetAttribute("Value").StartsWith("J") Then
                                strExternalRefValue = "HOOPLE"
                            Else
                                strExternalRefValue = "HERECC"
                            End If
                            blnHerfordshireCheck = True

                            'R2.20C CR - check the traveller exists
                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "1" AndAlso strBossCode.ToUpper = "LV" AndAlso getConfig("LVTravellerEmailActive") = "true" Then
                            Dim strTravellerEmail As String = ""
                            strTravellerEmail = xmlreader.GetAttribute("Value")
                            strTravellerEmail = strTravellerEmail.Trim

                            blnLvTravellerProvided = True
                            'look in the database for the email address
                            'get the passenger details for the DB
                            Dim oCurrentLvTraveller As New clsTravellerProfiles
                            oCurrentLvTraveller = clsTravellerProfiles.getByEmail(strTravellerEmail)
                            If oCurrentLvTraveller.Email Is Nothing OrElse oCurrentLvTraveller.Email = "" Then
                                'copy the file to the errors folder, skip processing
                                'send an email to say it's been missed
                                blnIsDataError = True
                                log.Error("LV Traveller not found in file " & File.Name & ", email provided was: " & strTravellerEmail)

                                ''for live version:
                                'SendEmail.send("service@nysgroup.com", _
                                '   "nyshelpdesk@nysgroup.com", _
                                '   "Interceptor - LV Traveller Not Found", _
                                '   "Interceptor has encountered an input error.<br/>Traveller Email in the Handoff file was not found as an LV Traveller.<br/>" & _
                                '   "<p>Traveller Email in handoff file is: " & strTravellerEmail & "<br/>Ticket Issue Ref is: " & strIssueRef & "</p><p>File name is: " & File.Name & "</p>" & _
                                '   "Handoff will be moved to error files folder until corrected, please either chase the client for new traveller details or correct the incorrect email in the file (CustomCode 1)." & _
                                '   "<br/>Once corrected please move the handoff file back into the 'awaiting' directory for processing.", _
                                '   "craig.rickell@nysgroup.com", "", "")

                                'for testing version:
                                Throw New Exception("LV Traveller not found, email provided was: " & strTravellerEmail)
                            End If

                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "2" AndAlso strBossCode.ToUpper = "LV" AndAlso getConfig("LVTravellerEmailActive").ToLower = "true" Then
                            'they have given a project code
                            blnLvProjectCode = True
                            'R2.20D SA
                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "1" AndAlso strBossCode.ToUpper = "MCKESSON" Then
                            Dim strTravellerEmail As String = ""
                            Dim intEmailExist As Integer = 0
                            strTravellerEmail = xmlreader.GetAttribute("Value")
                            strTravellerEmail = strTravellerEmail.Trim

                            intEmailExist = clsTravellerProfiles.checkEmail(strTravellerEmail)
                            If intEmailExist = 1 Then
                                strMckTravellerCostCentre = clsTravellerProfiles.getCode1(strTravellerEmail)
                                strMckTravellerHRNumber = clsTravellerProfiles.getCode2(strTravellerEmail)
                            Else
                                blnIsDataError = True
                                log.Error("McKesson Traveller not found in file " & File.Name & ", email provided was: " & strTravellerEmail)

                                'for live version:
                                ' SendEmail.send("service@nysgroup.com", _
                                '"nyshelpdesk@nysgroup.com", _
                                '"Interceptor - McKesson Traveller Not Found", _
                                '"Interceptor has encountered an input error.<br/>Traveller Email in the Handoff file was not found as a McKesson Traveller.<br/>" & _
                                '"<p>Traveller Email in handoff file is: " & strTravellerEmail & "<br/>Ticket Issue Ref is: " & strIssueRef & "</p><p>File name is: " & File.Name & "</p>" & _
                                '"Handoff has been moved to error files folder until corrected, please either chase the client for new traveller details or correct the incorrect email in the file (CustomCode 1)." & _
                                '"<br/>Once corrected please move the handoff file back into the awaiting directory for processing.", _
                                '"craig.rickell@nysgroup.com", "", "")

                                'for testing version:
                                Throw New Exception("McKesson Traveller not found, email provided was: " & strTravellerEmail)
                            End If
                            'R2.20D SA - if optional Cost Centre is provided use value as Cost Centre
                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "3" AndAlso strBossCode = "MCKESSON" Then
                            If xmlreader.GetAttribute("Value") <> "" Then
                                blnMckOptionalCostCentre = True
                                strMckTravellerCostCentre = xmlreader.GetAttribute("Value")
                            End If

                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "1" AndAlso (strBossCode.ToUpper = "PIKSELUK0".ToUpper Or strBossCode.ToUpper = "PIKSELFR0".ToUpper Or strBossCode.ToUpper = "PIKSELIT0".ToUpper) Then
                            Dim strTravellerEmail As String = ""
                            Dim intEmailExist As Integer = 0
                            strTravellerEmail = xmlreader.GetAttribute("Value")
                            strTravellerEmail = strTravellerEmail.Trim

                            intEmailExist = clsTravellerProfiles.checkEmail(strTravellerEmail)
                            If intEmailExist = 1 Then
                                Dim oTraveller As New clsTravellerProfiles
                                oTraveller.populate(0, strTravellerEmail)

                                strPikselTravellerEmployeeNumber = oTraveller.Code1
                                strPikselTravellerBussinessUnit = oTraveller.Code3
                                strPikselTravellerDepartment = oTraveller.Code5
                                strPikselTravellerLegalEntity = oTraveller.Code6
                                strPikselTravellerLocation = oTraveller.Location

                            Else
                                blnIsDataError = True
                                log.Error("Piksel Traveller not found in file " & File.Name & ", email provided was: " & strTravellerEmail)

                                'for live version:
                                ' SendEmail.send("service@nysgroup.com", _
                                '"nyshelpdesk@nysgroup.com", _
                                '"Interceptor - Piksel Traveller Not Found", _
                                '"Interceptor has encountered an input error.<br/>Traveller Email in the Handoff file was not found as a Piksel Traveller.<br/>" & _
                                '"<p>Traveller Email in handoff file is: " & strTravellerEmail & "<br/>Ticket Issue Ref is: " & strIssueRef & "</p><p>File name is: " & File.Name & "</p>" & _
                                '"Handoff has been moved to error files folder until corrected, please either chase the client for new traveller details or correct the incorrect email in the file (CustomCode 1)." & _
                                '"<br/>Once corrected please move the handoff file back into the awaiting directory for processing.", _
                                '"developmentteam@nysgroup.com", "", "")

                                'for testing version:
                                Throw New Exception("Piksel Traveller not found, email provided was: " & strTravellerEmail)
                            End If
                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "2" AndAlso (strBossCode.ToUpper = "PIKSELUK0".ToUpper Or strBossCode.ToUpper = "PIKSELFR0".ToUpper Or strBossCode.ToUpper = "PIKSELIT0".ToUpper) Then
                            strPikselTravellerPO = xmlreader.GetAttribute("Value")

                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "3" AndAlso (strBossCode.ToUpper = "PIKSELUK0".ToUpper Or strBossCode.ToUpper = "PIKSELFR0".ToUpper Or strBossCode.ToUpper = "PIKSELIT0".ToUpper) Then
                            strPikselTravellerGLCode = xmlreader.GetAttribute("Value")
                            '    'R2.25 SA - Kcom
                            'ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "1" AndAlso (strBossCode.ToUpper = "KCOM0" Or strBossCode.ToUpper = "KC0") Then
                            '    Dim strTravellerEmail As String = ""
                            '    Dim intEmailExist As Integer = 0
                            '    strTravellerEmail = xmlreader.GetAttribute("Value")
                            '    strTravellerEmail = strTravellerEmail.Trim

                            '    intEmailExist = clsTravellerProfiles.checkEmail(strTravellerEmail)
                            '    If intEmailExist = 1 Then
                            '        strKcomPayrollNumber = clsTravellerProfiles.getCode1(strTravellerEmail)
                            '    Else
                            '        blnIsDataError = True
                            '        log.Error("McKesson Traveller not found in file " & File.Name & ", email provided was: " & strTravellerEmail)

                            '        'for live version:
                            '        ' SendEmail.send("service@nysgroup.com", _
                            '        '"nyshelpdesk@nysgroup.com", _
                            '        '"Interceptor - Kcom Traveller Not Found", _
                            '        '"Interceptor has encountered an input error.<br/>Traveller Email in the Handoff file was not found as a Kcom Traveller.<br/>" & _
                            '        '"<p>Traveller Email in handoff file is: " & strTravellerEmail & "<br/>Ticket Issue Ref is: " & strIssueRef & "</p><p>File name is: " & File.Name & "</p>" & _
                            '        '"Handoff has been moved to error files folder until corrected, please either chase the client for new traveller details or correct the incorrect email in the file (CustomCode 1)." & _
                            '        '"<br/>Once corrected please move the handoff file back into the awaiting directory for processing.", _
                            '        '"craig.rickell@nysgroup.com", "", "")

                            '        'for testing version:
                            '        Throw New Exception("Kcom Traveller not found, email provided was: " & strTravellerEmail)
                            '    End If
                        ElseIf xmlreader.Name = "BookingAgent" And xmlreader.IsStartElement Then
                            'Update by Craig - added <> "UNK" to if statement
                            If xmlreader.GetAttribute("ExternalRef") <> "EV" And xmlreader.GetAttribute("ExternalRef") <> "UNK" Then
                                blnAddOfflineFee = True
                            End If
                        ElseIf xmlreader.Name = "ImmediateDetail" And xmlreader.IsStartElement Then
                            strMachineNumber = xmlreader.GetAttribute("MachineNumber")
                            'R2.14 CR
                            strIssueRef = xmlreader.GetAttribute("IssueRef")
                            'R2.21 SA 
                            strTransactionType = xmlreader.GetAttribute("TransactionType")
                            'R2.23 SA 
                            If strTransactionType.ToLower = "nonissue" Then
                                blnNonIssue = True
                            End If

                        ElseIf xmlreader.Name = "Passenger" And xmlreader.IsStartElement Then
                            strCurrentPassengerRef = xmlreader.GetAttribute("Ref")
                            dicPassengerSkipPost.Add(strCurrentPassengerRef, False)
                            'R2.21.9 SA 
                            intTotalPassengers += 1
                        ElseIf xmlreader.Name = "CustomField" And xmlreader.IsStartElement Then
                            If xmlreader.GetAttribute("Code").StartsWith("SD") Then
                                If xmlreader.GetAttribute("Value").Length > 0 Then
                                    Try
                                        Dim dblTest As Double = CDbl(xmlreader.GetAttribute("Code").Replace("SD", ""))
                                        dblSpecialDelivery = dblSpecialDelivery + dblTest
                                    Catch ex As Exception
                                        dblSpecialDelivery = dblSpecialDelivery
                                        log.Error("SPECIAL DELIVERY IS NOT A NUMBER!")
                                    End Try
                                End If
                            ElseIf xmlreader.GetAttribute("Code").ToLower = "cc" Then

                                strCustomfieldCC = xmlreader.GetAttribute("Value")

                                'R2.17 CR - get these to be read from the config file
                                'R2.13 CR - also add the code for NHSBSAPRO
                                'R2.7 CR - also add the code for NHSBSAHOST
                                'alter customfield with code cc, add 11001 into value tag
                                'If strBossCode = "NHSBSA" Or strBossCode = "NHSBSAHOST" Or strBossCode = "NHSBSAPRO" Then
                                If strBossCode <> "" AndAlso notString(getConfig("CostCentreSuffixClients")).Contains(strBossCode) Then
                                    strCustomfieldCC = strCustomfieldCC & notString(getConfig("CostCentreSuffix")) ' "11001"
                                End If

                            ElseIf xmlreader.GetAttribute("Code") = "1" Then
                                blnSkipDeliveryCharge = False
                                If strBossCode = "UHSM" Then
                                    strCustomfieldCREF1 = xmlreader.GetAttribute("Value")
                                    strCustomfieldCREF1 = clsInterceptorCostCentre.findDivisionName(0, strCustomfieldCREF1.Substring(0, 1))
                                ElseIf strBossCode = "UOYR" Then
                                    'do not charge delivery for Archaeology department
                                    If xmlreader.GetAttribute("Value") = "ARCHAEOL" Then
                                        blnSkipDeliveryCharge = True
                                    End If
                                End If

                                If dicPassengerSkipPost.ContainsKey(strCurrentPassengerRef) Then
                                    dicPassengerSkipPost.Item(strCurrentPassengerRef) = blnSkipDeliveryCharge
                                End If

                                'R2.9.1 CR
                            ElseIf xmlreader.GetAttribute("Code").ToLower = "po" Then
                                strCustomFieldPO = clsInterceptorClient.getCustomPO(strBossCode)

                            End If

                            'R2.5 CR
                        ElseIf xmlreader.Name = "Refund" AndAlso xmlreader.IsStartElement Then
                            blnIsRefund = True
                        ElseIf xmlreader.Name = "AgencyCancellationCharge" AndAlso xmlreader.IsStartElement Then
                            If xmlreader.GetAttribute("TotalAmount") = "0.01" AndAlso blnIsRefund Then
                                blnChangeAgencyCancellation = True
                            End If

                            'R2.10 CR
                            'count how many tickets each passenger has - incase offline fee is charged per ticket
                        ElseIf xmlreader.Name = "Ticket" AndAlso xmlreader.IsStartElement Then
                            'count the number of tickets
                            intTotalTickets += 1
                            'R2.16 CR
                        ElseIf xmlreader.Name.ToLower = "my:handoff" AndAlso xmlreader.IsStartElement Then
                            dblSchemaVersion = notDouble(xmlreader.GetAttribute("SchemaVersion"))

                            'R2.21.9 SA read delivery method 
                            strFulilmentType = xmlreader.GetAttribute("DeliveryMethod")
                            'R2.22.1 SA - read the order ref 
                            strOrderRef = xmlreader.GetAttribute("OrderRef")

                            'R2.17 CR
                        ElseIf xmlreader.Name = "FulfilmentFee" Then
                            'we have a fulfilment fee node somewhere in the file!
                            'this isn't present on v4 handoffs, so if we have v4 files with a v3 schema version (i.e. Guardian has changed the version number to force fees to be added)
                            ' then we have a problem - but at least we know about it!
                            'So if it isn't present add the fees on to transaction charge instead
                            blnFulfilmentFeePresent = True

                        End If

                        'R2.13 CR - let it do all the above as normal. work out the splitting here
                        If strSplitNode <> "" Then
                            If xmlreader.Name = strSplitNode AndAlso xmlreader.IsStartElement Then
                                If strSplitFieldName <> "" Then
                                    'should have a fieldvalue too
                                    If xmlreader.GetAttribute(strSplitFieldName) = strSplitFieldValue Then
                                        If Not dictPassengerVSplitValue.ContainsKey(strCurrentPassengerRef) Then
                                            dictPassengerVSplitValue.Add(strCurrentPassengerRef, xmlreader.GetAttribute(strSplitUniqueFieldName))
                                        End If
                                    End If
                                Else
                                    'dealing with a record that only has a uniquefield - no other checks to do, the node should be unique itself!
                                    If Not dictPassengerVSplitValue.ContainsKey(strCurrentPassengerRef) Then
                                        dictPassengerVSplitValue.Add(strCurrentPassengerRef, xmlreader.GetAttribute(strSplitUniqueFieldName))
                                    End If
                                End If
                            End If
                        End If
                    End While

                    xmlreader.Close()
                    fstOldFile.Close()

                    xmlreader = Nothing
                    fstOldFile = Nothing

                    'R2.20C CR - if there is an error noticed above then skip the file & move it to errors
                    If blnIsDataError Then
                        GoTo skipfile
                    End If

                    'R2.21.8 SA 
                    'check file is nonissue before trying to process it
                    If blnNonIssue Then
                        GoTo skipfile
                    End If

                    Dim oClsInterceptorClient As New clsInterceptorClient
                    oClsInterceptorClient = clsInterceptorClient.checkBossCode(strBossCode, 0)

                    If oClsInterceptorClient.BossCode <> "" Then

                        'R2.13 CR - WARNING, items are CASE SENSITIVE
                        Dim lstDistinctSplitValues As New List(Of String)
                        For Each oItemValue As String In dictPassengerVSplitValue.Values
                            If Not lstDistinctSplitValues.Contains(oItemValue) Then
                                lstDistinctSplitValues.Add(oItemValue)
                            End If
                        Next

                        'R2.13 CR
                        Dim intNumberOfFiles As Integer = 1
                        If strSplitNode <> "" Then
                            'R2.13 CR FIX: this line of code works in test but not on live, DISTINCT needs to be done manually
                            'intNumberOfFiles = dictPassengerVSplitValue.Values.Distinct.Count()

                            'create one handoff file for each unique instance of the specified node/attribute
                            intNumberOfFiles = lstDistinctSplitValues.Count
                        End If

                        Dim intCurrentTicket As Integer = 0

                        'loop for number of files needed
                        Dim x As Integer = 0
                        For x = 0 To intNumberOfFiles - 1

                            Dim strFileName As String = File.Name
                            Dim strNewSeqNumber As String = ""

                            If intNumberOfFiles > 1 Then
                                'convert the current file number into a letter
                                Dim strFileLetter As String = getInterceptorFileLetter(x)

                                'R2.22 CR-  BUG FIX
                                'had a problem here where interceptor was naming the file the same as a file from guardian and overwriting files
                                'now first letter in sequence signifies Guardian file number
                                'second letter in sequence signifies Interceptor file number

                                'R2.22 CR - grab the starting character, put the new letters one char to the right
                                'place the letter at the start of the sequence number
                                Dim strStartingChar As String = ""
                                strStartingChar = strFileName.Substring((strFileName.LastIndexOf("_") + 1), 1)
                                strNewSeqNumber = Replace(strStartingChar & strFileLetter & strFileName.Substring((strFileName.LastIndexOf("_") + 2) + strFileLetter.Length), ".xml", "")

                                'Set the new file name
                                strFileName = strFileName.Substring(0, strFileName.LastIndexOf("_") + 1) & strNewSeqNumber & ".xml"
                            End If

                            fstOldFile = New FileStream(File.FullName, FileMode.Open)
                            xmlreader = xmlreader.Create(fstOldFile)

                            'R2.10 CR - replaced file.name with strFileName
                            fstBOSSFile = New FileStream(pstrWriteToBossFolder & strFileName, FileMode.Create)
                            xmlBOSSwriter = New XmlTextWriter(fstBOSSFile, System.Text.Encoding.UTF8)
                            xmlBOSSwriter.Formatting = Formatting.Indented
                            xmlBOSSwriter.WriteStartDocument()

                            'R2.3 CR
                            'R2.10 CR - replaced file.name with strFileName
                            fstSQLFile = New FileStream(pstrWriteToSqlFolder & strFileName, FileMode.Create)
                            xmlSQLwriter = New XmlTextWriter(fstSQLFile, System.Text.Encoding.UTF8)
                            xmlSQLwriter.Formatting = Formatting.Indented
                            xmlSQLwriter.WriteStartDocument()

                            'R2.3 CR
                            Dim blnInsideRefundOrSaleNode As Boolean = False

                            Dim strPreviousNode As String = ""

                            'R2.13 CR - comment out intPassengerLoopCount, rename strWritingPassengerRef and strWritingTicketRef
                            'R2.10 CR
                            'Dim intPassengerLoopCount As Integer = 0
                            Dim strThisPassengerRef As String = ""
                            Dim strThisTicketPassengerRef As String = ""

                            'R2.13 CR - set the value we are processing for the current file
                            Dim strWritingCurrentValue As String = ""
                            If strSplitNode <> "" Then

                                'R2.13 CR FIX: DISTINCT not working in service! use the string list lstDistinctSplitValues
                                strWritingCurrentValue = lstDistinctSplitValues.Item(x)
                                ' dictPassengerVSplitValue.Values.Distinct.ElementAt(x)
                            End If

                            'R2.18 CR
                            Dim strLvPassengerCustomFields As New StringBuilder

                            'R2.18 CR
                            Dim blnInsidePassengerNode As Boolean = False
                            Dim oCurrentLvTraveller As New clsTravellerProfiles

                            'R2.22 CR
                            Dim blnInsideTicketNode As Boolean = False

                            'R2.20D SA 
                            Dim strMckessonPassengerCustomFields As New StringBuilder

                            'R1.3 SA 
                            Dim strPikselPassengerCustomFields As New StringBuilder

                            ''R2.25 SA 
                            'Dim strKcomPassengerCustomFields As New StringBuilder

                            'R2.21.8 SA - check if it's an issue or refund 
                            'R2.16 CR
                            If dblSchemaVersion >= 4.0 AndAlso (blnNonIssue = False) Then
                                'V4/newer Evolvi Handoffs
                                While xmlreader.Read
                                    If xmlreader.Name <> "xml" Then

                                        'R?? bug fix 
                                        If xmlreader.Name = "my:Handoff" AndAlso xmlreader.IsStartElement Then
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "3.9", False, "SchemaVersion"))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "3.9", False, "SchemaVersion"))
                                            xmlreader.MoveToElement()

                                        ElseIf xmlreader.Name = "my:Handoff" AndAlso xmlreader.IsStartElement AndAlso intNumberOfFiles > 1 Then
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, strNewSeqNumber, False, "SequenceNumber"))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, strNewSeqNumber, False, "SequenceNumber"))
                                            xmlreader.MoveToElement()

                                            '    'R2.26 SA 
                                            'ElseIf xmlreader.Name = "my:Handoff" AndAlso xmlreader.IsStartElement Then
                                            '    xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "3.9", False, "SchemaVersion"))
                                            '    xmlreader.MoveToElement()
                                            '    xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "3.9", False, "SchemaVersion"))
                                            '    xmlreader.MoveToElement()


                                            'R2.22.1 SA - amend transaction date to drop as today's date
                                        ElseIf xmlreader.Name = "ImmediateDetail" AndAlso xmlreader.IsStartElement Then
                                            'declare new variable, get today's date in the rigth format, 
                                            Dim d As DateTime = Now
                                            Dim strDate As String = (d.ToString("yyyy-MM-dd")) & "T" & (d.ToString("hh:mm:ss"))
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, strDate, False, "TransactionDate"))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, strDate, False, "TransactionDate"))
                                            xmlreader.MoveToElement()

                                        ElseIf xmlreader.Name = "Passenger" AndAlso intNumberOfFiles > 1 Then
                                            If xmlreader.IsStartElement Then
                                                strThisPassengerRef = xmlreader.GetAttribute("Ref")
                                                'R2.18 CR
                                                blnInsidePassengerNode = True
                                            Else
                                                blnInsidePassengerNode = False
                                                oCurrentLvTraveller = Nothing
                                            End If

                                            If dictPassengerVSplitValue(strThisPassengerRef) = strWritingCurrentValue Then
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                            Else
                                                readToNextSibling(xmlreader)
                                            End If

                                            'R2.18 CR - also check the current passenger if only one file!!
                                        ElseIf xmlreader.Name = "Passenger" AndAlso intNumberOfFiles = 1 Then
                                            If xmlreader.IsStartElement Then
                                                strThisPassengerRef = xmlreader.GetAttribute("Ref")
                                                blnInsidePassengerNode = True
                                            Else
                                                blnInsidePassengerNode = False
                                                oCurrentLvTraveller = Nothing
                                            End If

                                            'write the nodes
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()


                                        ElseIf xmlreader.Name = "Ticket" And intNumberOfFiles > 1 Then
                                            If xmlreader.IsStartElement Then
                                                strThisTicketPassengerRef = xmlreader.GetAttribute("PassengerRef")
                                            End If

                                            If dictPassengerVSplitValue(strThisTicketPassengerRef) = strWritingCurrentValue Then
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                            Else
                                                readToNextSibling(xmlreader)
                                            End If
                                        ElseIf xmlreader.Name = "Reservation" And intNumberOfFiles > 1 Then
                                            If dictPassengerVSplitValue(xmlreader.GetAttribute("PassengerRef")) = strWritingCurrentValue Then
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                            End If

                                        ElseIf xmlreader.Name = "Person" AndAlso strPreviousNode = "BookingAgent" AndAlso oClsInterceptorClient.AddBookerToInvoice = False Then
                                            'do nothing - remove booker info in <Person> tag

                                            'only do nothing with BOSS file as this needs the Person tag removing, SQL needs it in
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()
                                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code").ToLower = "cc" And strCustomfieldCC <> "" Then
                                            'change values of CC
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, strCustomfieldCC, False))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, strCustomfieldCC, False))
                                            xmlreader.MoveToElement()
                                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code").ToLower = "cref1" And strCustomfieldCREF1 <> "" Then
                                            'change values of CREF1
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, strCustomfieldCREF1, False))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, strCustomfieldCREF1, False))
                                            xmlreader.MoveToElement()
                                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code").ToLower = "po" And strCustomFieldPO <> "" Then
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, strCustomFieldPO, False))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, strCustomFieldPO, False))
                                            xmlreader.MoveToElement()

                                            'R2.18 CR - LV changes
                                            'if inside passenger AND customfield AND client is LV AND Traveller is provided
                                        ElseIf xmlreader.Name = "CustomField" _
                                            AndAlso strBossCode.ToUpper = "LV" _
                                            AndAlso getConfig("LVTravellerEmailActive") = "true" _
                                            AndAlso blnInsidePassengerNode _
                                            AndAlso blnLvTravellerProvided Then

                                            'Input:
                                            'Code 1 = Traveller Email
                                            'Code 2 = Project Code
                                            'Code 3 = ACK

                                            'BOSS wants:
                                            'Code 1 = ACK (maps to inm_costc)
                                            'Code 2 = Employee No (maps to inm_pono)
                                            'Code 3 = Project Code (maps to tot_cref1)

                                            If xmlreader.GetAttribute("Code") = "1" Then
                                                'we're looking at the TravellerEmail

                                                Dim strTravellerEmail As String = ""
                                                strTravellerEmail = xmlreader.GetAttribute("Value")
                                                strTravellerEmail = strTravellerEmail.Trim

                                                'look in the database for the email address
                                                'get the passenger details for the DB
                                                'should already be checked against DB so no missing errors should occur here
                                                oCurrentLvTraveller = clsTravellerProfiles.getByEmail(strTravellerEmail)
                                                strLvPassengerCustomFields.Append("1")


                                            ElseIf xmlreader.GetAttribute("Code") = "2" Then
                                                'we're looking at the ProjectCode

                                                'if a project code provided then use that AND the ACK provided
                                                If xmlreader.GetAttribute("Value") <> "" Then
                                                    'assign the value to the currentTraveller class
                                                    oCurrentLvTraveller.ProjectCode = xmlreader.GetAttribute("Value")
                                                End If
                                                strLvPassengerCustomFields.Append("2")

                                            ElseIf xmlreader.GetAttribute("Code") = "3" Then
                                                'we're looking at the ACKCode

                                                'only need to worry about the value passed here if the project code has been used
                                                If oCurrentLvTraveller.ProjectCode <> "" Then
                                                    'they provided a ProjectCode, so use the ACK they also provided
                                                    oCurrentLvTraveller.Code1 = xmlreader.GetAttribute("Value")
                                                End If
                                                strLvPassengerCustomFields.Append("3")

                                            ElseIf xmlreader.GetAttribute("Code") = "4" Then
                                                'make sure it has a value before writing
                                                'if value is blank then DONT create the node!
                                                If xmlreader.GetAttribute("Value") = "" Then
                                                    'do nothing - dont even write the line to the output file - it will mess up BOSS
                                                Else
                                                    'write the node - we want the value in boss
                                                    xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                    xmlreader.MoveToElement()
                                                    xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                    xmlreader.MoveToElement()
                                                End If

                                            Else
                                                'not one of the customFields we are bothered about, just write it & let BOSS work it out
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                            End If

                                            'check if the above have all been looked at
                                            'OR - a project code hasn't been specified
                                            If strLvPassengerCustomFields.ToString = "123" _
                                                Or (blnLvProjectCode = False And xmlreader.GetAttribute("Code") = "1") Then
                                                'clear it first
                                                strLvPassengerCustomFields.Clear()

                                                're-create the fields
                                                strLvPassengerCustomFields.Append("<CustomField Code=""1"" Label=""ACK Code"" Value=""" & oCurrentLvTraveller.Code1 & """ />")
                                                strLvPassengerCustomFields.Append("<CustomField Code=""2"" Label=""Employee Number"" Value=""" & oCurrentLvTraveller.Code2 & """ />")
                                                If oCurrentLvTraveller.ProjectCode <> "" Then
                                                    strLvPassengerCustomFields.Append("<CustomField Code=""3"" Label=""Project Code"" Value=""" & oCurrentLvTraveller.ProjectCode & """ />")
                                                End If

                                                'write the customfields to the files
                                                xmlBOSSwriter.WriteRaw(strLvPassengerCustomFields.ToString)
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(strLvPassengerCustomFields.ToString)
                                                xmlreader.MoveToElement()

                                                'clear the string builder again - all done
                                                strLvPassengerCustomFields.Clear()
                                            End If
                                            'R2.20D SA
                                        ElseIf xmlreader.Name = "CustomField" AndAlso strBossCode.ToUpper = "MCKESSON" _
                                       AndAlso blnInsidePassengerNode Then

                                            'Input:
                                            'Code 1 = Traveller Email
                                            'Code 2 = 
                                            'Code 3 = Optional Cost Code

                                            'BOSS wants:
                                            'Code 1 = Cost Code (maps to inm_costc)
                                            'Code 2 = HR_Number (maps to inm_pono)

                                            If xmlreader.GetAttribute("Code") = "1" Then
                                                strMckessonPassengerCustomFields.Append("1")
                                                'ElseIf xmlreader.GetAttribute("Code") = "2" Then
                                                '    strMckessonPassengerCustomFields.Append("2")
                                            ElseIf xmlreader.GetAttribute("Code") = "3" Then
                                                strMckessonPassengerCustomFields.Append("3")
                                            Else
                                                'not one of the customFields we are bothered about, just write it & let BOSS work it out
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                            End If

                                            'check if the important custom fields have all been looked at
                                            If strMckessonPassengerCustomFields.ToString = "13" _
                                            Or (blnMckOptionalCostCentre = False And xmlreader.GetAttribute("Code") = "1") Then
                                                'clear it first
                                                strMckessonPassengerCustomFields.Clear()

                                                're-create the fields
                                                strMckessonPassengerCustomFields.Append("<CustomField Code=""1"" Label=""Cost Centre"" Value=""" & strMckTravellerCostCentre & """ />")
                                                strMckessonPassengerCustomFields.Append("<CustomField Code=""2"" Label=""HR Number"" Value=""" & strMckTravellerHRNumber & """ />")

                                                'write the customfields to the files
                                                xmlBOSSwriter.WriteRaw(strMckessonPassengerCustomFields.ToString)
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(strMckessonPassengerCustomFields.ToString)
                                                xmlreader.MoveToElement()

                                                'clear the string builder again - all done
                                                strMckessonPassengerCustomFields.Clear()
                                            End If


                                        ElseIf xmlreader.Name = "CustomField" AndAlso (strBossCode.ToUpper = "PIKSELUK0".ToUpper Or strBossCode.ToUpper = "PIKSELFR0".ToUpper Or strBossCode.ToUpper = "PIKSELIT0".ToUpper) _
                                                                     AndAlso blnInsidePassengerNode Then

                                            'Input:
                                            'Code 1 = Traveller Email
                                            'Code 2 = Project Code
                                            'Code 3 = GL Code

                                            'BOSS wants:
                                            'Code 1 = Employee Number (maps to inm_costc)
                                            'Code 2 = PO (user input. Don't do anything with it for now)
                                            'Code 3 = Business Unit (maps to inm_cref1)
                                            'Code 4 = Booker Name (Boss should generate. Don't do anything with it for now)
                                            'Code 5 = Department (maps to inm_cref3)
                                            'Code 6 = Legal Entitiy (maps to inm_cref4)
                                            'Code 7 = Location (maps to inm_cref5)
                                            'Code 8 = GL Code (maps to inm_cref6)

                                            'check Email 
                                            If xmlreader.GetAttribute("Code") = "1" Then
                                                strPikselPassengerCustomFields.Append("1")
                                            ElseIf xmlreader.GetAttribute("Code") = "2" Then 'Check PO
                                                strPikselPassengerCustomFields.Append("2")
                                            ElseIf xmlreader.GetAttribute("Code") = "3" Then 'Check GL code
                                                strPikselPassengerCustomFields.Append("3")
                                            Else
                                                'not one of the customFields we are bothered about, just write it & let BOSS work it out
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                                xmlreader.MoveToElement()
                                            End If

                                            'check if the important custom fields have all been looked at
                                            If strPikselPassengerCustomFields.ToString = "123" Then
                                                'clear it first
                                                strPikselPassengerCustomFields.Clear()

                                                're-create the fields
                                                strPikselPassengerCustomFields.Append("<CustomField Code=""1"" Label=""Employee Number"" Value=""" & strPikselTravellerEmployeeNumber & """ />")
                                                strPikselPassengerCustomFields.Append("<CustomField Code=""2"" Label=""Project Code"" Value=""" & strPikselTravellerPO & """ />")
                                                strPikselPassengerCustomFields.Append("<CustomField Code=""3"" Label=""Business Unit"" Value=""" & strPikselTravellerBussinessUnit & """ />")
                                                strPikselPassengerCustomFields.Append("<CustomField Code=""5"" Label=""Department"" Value=""" & strPikselTravellerDepartment & """ />")
                                                strPikselPassengerCustomFields.Append("<CustomField Code=""6"" Label=""Legal Entity"" Value=""" & strPikselTravellerLegalEntity & """ />")
                                                strPikselPassengerCustomFields.Append("<CustomField Code=""7"" Label=""Location"" Value=""" & strPikselTravellerLocation & """ />")
                                                strPikselPassengerCustomFields.Append("<CustomField Code=""8"" Label=""GL Code (click magnifying glass)"" Value=""" & strPikselTravellerGLCode & """ />")

                                                'write the customfields to the files
                                                xmlBOSSwriter.WriteRaw(strPikselPassengerCustomFields.ToString)
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(strPikselPassengerCustomFields.ToString)
                                                xmlreader.MoveToElement()

                                                'clear the string builder again - all done
                                                strPikselPassengerCustomFields.Clear()
                                            End If
                                            '           'R2.25 SA 
                                            '       ElseIf xmlreader.Name = "CustomField" AndAlso (strBossCode.ToUpper = "KCOM0" Or strBossCode.ToUpper = "KC0") _
                                            'AndAlso blnInsidePassengerNode Then

                                            '           'Input:
                                            '           'Code 1 = Traveller Email

                                            '           'BOSS wants:
                                            '           'Code 1 = Empolyee Payrll Number (maps to inm_pono)

                                            '           If xmlreader.GetAttribute("Code") = "1" Then
                                            '               strKcomPassengerCustomFields.Append("1")
                                            '           Else
                                            '               'not one of the customFields we are bothered about, just write it & let BOSS work it out
                                            '               xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            '               xmlreader.MoveToElement()
                                            '               xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            '               xmlreader.MoveToElement()
                                            '           End If

                                            '           'check if the important custom fields have all been looked at
                                            '           If strKcomPassengerCustomFields.ToString = "1" Then
                                            '               'clear it first
                                            '               strKcomPassengerCustomFields.Clear()

                                            '               're-create the fields
                                            '               strKcomPassengerCustomFields.Append("<CustomField Code=""1"" Label=""Employee Number"" Value=""" & strKcomPayrollNumber & """ />")

                                            '               'write the customfields to the files
                                            '               xmlBOSSwriter.WriteRaw(strKcomPassengerCustomFields.ToString)
                                            '               xmlreader.MoveToElement()
                                            '               xmlSQLwriter.WriteRaw(strKcomPassengerCustomFields.ToString)
                                            '               xmlreader.MoveToElement()

                                            '               'clear the string builder again - all done
                                            '               strKcomPassengerCustomFields.Clear()
                                            '           End If

                                            'R2.20B SA
                                        ElseIf strBossCode.ToUpper = "HERECC" AndAlso xmlreader.Name = "Account" AndAlso blnHerfordshireCheck Then
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, strExternalRefValue, False, "externalref"))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, strExternalRefValue, False, "externalref"))
                                            xmlreader.MoveToElement()


                                        ElseIf xmlreader.Name = "TransactionCharge" Then
                                            'OK, so.... we dont NORMALLY charge fees on v4 files (because they should already be on there)
                                            'BUT, McKesson are a law unto themselves and need us to check for applicable fees
                                            'then save them to the database because the files will not have anything included
                                            'SA: ICE and RCN don't want any fees showing on Evolvi, so we took the fees out and charge them here .. 
                                            'R2.25 SA - recoded adding fees 

                                            'R2.21.9 get client details again
                                            oClsInterceptorClient = clsInterceptorClient.checkBossCode(strBossCode, 0)
                                            'R2.21.9 SA - recoded adding fees 
                                            'AndAlso strTransactionType.ToUpper = "ISSUE"
                                            If oClsInterceptorClient.InterceptorFeesActive = True Then
                                                'Kiosk 
                                                If strFulilmentType.ToLower = "Client Ticket Printer (Kiosk)".ToLower AndAlso blnFeeAdded = False Then
                                                    'need total ToD charge
                                                    Dim dblTotalKioskFee As Double = 0
                                                    Dim dblTotalKioskPerTicket As Double = 0
                                                    Dim dblTotalKioskPerPax As Double = 0
                                                    Dim dblTotalKioskPerOrderItem As Double = 0

                                                    If blnAddOfflineFee Then 'offline booking i.e. offline Kiosk fees apply 
                                                        dblTotalKioskPerTicket = oClsInterceptorClient.KioskOfflineFeePerTicket * intTotalTickets
                                                        dblTotalKioskPerPax = oClsInterceptorClient.KioskOfflineFeePerTraveller * intTotalPassengers
                                                        dblTotalKioskPerOrderItem = oClsInterceptorClient.KioskOfflineFeePerOrderItem

                                                        dblTotalKioskFee = dblTotalKioskPerTicket + dblTotalKioskPerPax + dblTotalKioskPerOrderItem
                                                    Else
                                                        dblTotalKioskPerTicket = oClsInterceptorClient.KioskOnlineFeePerTicket * intTotalTickets
                                                        dblTotalKioskPerPax = oClsInterceptorClient.KioskOnlineFeePerTraveller * intTotalPassengers
                                                        dblTotalKioskPerOrderItem = oClsInterceptorClient.KioskOnlineFeePerOrderItem

                                                        dblTotalKioskFee = dblTotalKioskPerTicket + dblTotalKioskPerPax + dblTotalKioskPerOrderItem
                                                    End If
                                                    'save charges to database: "K" is for Kiosk
                                                    clsInterceptorCharges.saveCharges(strIssueRef, intTotalTickets, "Kiosk", dblTotalKioskFee, dblTotalKioskFee, strIssueDate, "ISSUE", strBossCode)
                                                    blnFeeAdded = True
                                                End If

                                                'Check if delievry method is TOD 
                                                If strFulilmentType.ToLower = "Ticket on Departure".ToLower AndAlso blnFeeAdded = False Then
                                                    'need total ToD charge
                                                    Dim dblTotalToDFee As Double = 0
                                                    Dim dblTotalToDPerTicket As Double = 0
                                                    Dim dblTotalToDPerPax As Double = 0
                                                    Dim dblTotalToDPerOrderItem As Double = 0

                                                    If blnAddOfflineFee Then 'offline booking i.e. offline ToD fees apply 
                                                        dblTotalToDPerTicket = oClsInterceptorClient.ToDOfflineFeePerTicket * intTotalTickets
                                                        dblTotalToDPerPax = oClsInterceptorClient.ToDOfflineFeePerTraveller * intTotalPassengers
                                                        dblTotalToDPerOrderItem = oClsInterceptorClient.ToDOfflineFeePerOrderItem

                                                        dblTotalToDFee = dblTotalToDPerTicket + dblTotalToDPerPax + dblTotalToDPerOrderItem
                                                    Else 'online 

                                                        dblTotalToDPerTicket = oClsInterceptorClient.ToDOnlineFeePerTicket * intTotalTickets
                                                        dblTotalToDPerPax = oClsInterceptorClient.ToDOnlineFeePerTraveller * intTotalPassengers
                                                        dblTotalToDPerOrderItem = oClsInterceptorClient.ToDOnlineFeePerOrderItem

                                                        dblTotalToDFee = dblTotalToDPerTicket + dblTotalToDPerPax + dblTotalToDPerOrderItem
                                                    End If
                                                    'save charges to database: "ToD" is for ToD
                                                    clsInterceptorCharges.saveCharges(strIssueRef, intTotalTickets, "TOD", dblTotalToDFee, dblTotalToDFee, strIssueDate, "ISSUE", strBossCode)
                                                    blnFeeAdded = True
                                                End If

                                                'postage 1st class 
                                                If strFulilmentType.ToLower = "First class post".ToLower AndAlso blnFeeAdded = False Then

                                                    Dim dblTotalPostage As Double = 0
                                                    Dim dblTotalPostagePerTicket As Double = 0
                                                    Dim dblTotalPostagePerPax As Double = 0
                                                    Dim dblTotalPostageperOrderItem As Double = 0

                                                    If blnAddOfflineFee Then
                                                        dblTotalPostagePerTicket = oClsInterceptorClient.PostageOfflineFeePerTicket * intTotalTickets
                                                        dblTotalPostagePerPax = oClsInterceptorClient.PostageOfflineFeePerTraveller * intTotalPassengers
                                                        dblTotalPostageperOrderItem = oClsInterceptorClient.PostageOfflineFeePerOrderItem

                                                        dblTotalPostage = dblTotalPostagePerTicket + dblTotalPostagePerPax + dblTotalPostageperOrderItem
                                                    Else
                                                        dblTotalPostagePerTicket = oClsInterceptorClient.PostageOnlineFeePerTicket * intTotalTickets
                                                        dblTotalPostagePerPax = oClsInterceptorClient.PostageOnlineFeePerTraveller * intTotalPassengers
                                                        dblTotalPostageperOrderItem = oClsInterceptorClient.PostageOnlineFeePerOrderItem

                                                        dblTotalPostage = dblTotalPostagePerTicket + dblTotalPostagePerPax + dblTotalPostageperOrderItem
                                                    End If
                                                    'save charges to database 
                                                    clsInterceptorCharges.saveCharges(strIssueRef, intTotalTickets, "Postage", dblTotalPostage, dblTotalPostage, strIssueDate, "ISSUE", strBossCode)
                                                    blnFeeAdded = True
                                                End If

                                                'Courier 
                                                If strFulilmentType.ToLower = "1pm Courier".ToLower AndAlso blnFeeAdded = False Then

                                                    Dim dblTotalCourier As Double = 0
                                                    Dim dblTotalCourierPerTicket As Double = 0
                                                    Dim dblTotalCourierPerPax As Double
                                                    Dim dblTotalCourierPerOrderItem As Double = 0

                                                    If blnAddOfflineFee Then
                                                        dblTotalCourierPerTicket = oClsInterceptorClient.CourierOfflineFeePerTicket * intTotalTickets
                                                        dblTotalCourierPerPax = oClsInterceptorClient.CourierOfflineFeePerTraveller * intTotalPassengers
                                                        dblTotalCourierPerOrderItem = oClsInterceptorClient.CourierOfflineFeePerOrderItem

                                                        dblTotalCourier = dblTotalCourierPerTicket + dblTotalCourierPerPax + dblTotalCourierPerOrderItem
                                                    Else
                                                        dblTotalCourierPerTicket = oClsInterceptorClient.CourierOnlineFeePerTicket * intTotalTickets
                                                        dblTotalCourierPerPax = oClsInterceptorClient.CourierOnlineFeePerTraveller * intTotalPassengers
                                                        dblTotalCourierPerOrderItem = oClsInterceptorClient.CourierOnlineFeePerOrderItem

                                                        dblTotalCourier = dblTotalCourierPerTicket + dblTotalCourierPerPax + dblTotalCourierPerOrderItem
                                                    End If
                                                    'save charges to database 
                                                    clsInterceptorCharges.saveCharges(strIssueRef, intTotalTickets, "Courier", dblTotalCourier, dblTotalCourier, strIssueDate, "ISSUE", strBossCode)
                                                    blnFeeAdded = True
                                                End If

                                                'Special delivery
                                                If strFulilmentType.ToLower = "Special Delivery".ToLower AndAlso blnFeeAdded = False Then
                                                    Dim dblTotalSDFee As Double = 0
                                                    Dim dblTotalSDPerTicket As Double = 0
                                                    Dim dblTotalSDPerPax As Double = 0
                                                    Dim dblTotalSDPerOrderItem As Double = 0

                                                    If blnAddOfflineFee Then
                                                        dblTotalSDPerTicket = oClsInterceptorClient.SDOfflineFeePerTicket * intTotalTickets
                                                        dblTotalSDPerPax = oClsInterceptorClient.SDOfflineFeePerTraveller * intTotalPassengers
                                                        dblTotalSDPerOrderItem = oClsInterceptorClient.SDOfflineFeePerOrderItem

                                                        dblTotalSDFee = dblTotalSDPerTicket + dblTotalSDPerPax + dblTotalSDPerOrderItem
                                                    Else
                                                        dblTotalSDPerTicket = oClsInterceptorClient.SDOnlineFeePerTicket * intTotalTickets
                                                        dblTotalSDPerPax = oClsInterceptorClient.SDOnlineFeePerTraveller * intTotalPassengers
                                                        dblTotalSDPerOrderItem = oClsInterceptorClient.SDOnlineFeePerOrderItem

                                                        dblTotalSDFee = dblTotalSDPerTicket + dblTotalSDPerPax + dblTotalSDPerOrderItem
                                                    End If
                                                    'save charges to database 
                                                    clsInterceptorCharges.saveCharges(strIssueRef, intTotalTickets, "SD", dblTotalSDFee, dblTotalSDFee, strIssueDate, "ISSUE", strBossCode)
                                                    blnFeeAdded = True
                                                End If

                                                'Saturday special delivery 
                                                If strFulilmentType.ToLower = "Saturday Special Delivery".ToLower AndAlso blnFeeAdded = False Then
                                                    Dim dblTotalSSDFee As Double = 0
                                                    Dim dblTotalSSDPerTicket As Double = 0
                                                    Dim dblTotalSSDPerPax As Double = 0
                                                    Dim dblTotalSSDPerOrderItem As Double = 0

                                                    If blnAddOfflineFee Then
                                                        dblTotalSSDPerTicket = oClsInterceptorClient.SSDOfflineFeePerTicket * intTotalTickets
                                                        dblTotalSSDPerPax = oClsInterceptorClient.SSDOfflineFeePerTraveller * intTotalPassengers
                                                        dblTotalSSDPerOrderItem = oClsInterceptorClient.SSDOfflineFeePerOrderItem

                                                        dblTotalSSDFee = dblTotalSSDPerTicket + dblTotalSSDPerPax + dblTotalSSDPerOrderItem
                                                    Else
                                                        dblTotalSSDPerTicket = oClsInterceptorClient.SSDOnlineFeePerTicket * intTotalTickets
                                                        dblTotalSSDPerPax = oClsInterceptorClient.SSDOnlineFeePerTraveller * intTotalPassengers
                                                        dblTotalSSDPerOrderItem = oClsInterceptorClient.SSDOnlineFeePerOrderItem

                                                        dblTotalSSDFee = dblTotalSSDPerTicket + dblTotalSSDPerPax + dblTotalSSDPerOrderItem
                                                    End If
                                                    'save charges to database 
                                                    clsInterceptorCharges.saveCharges(strIssueRef, intTotalTickets, "SSD", dblTotalSSDFee, dblTotalSSDFee, strIssueDate, "ISSUE", strBossCode)
                                                    blnFeeAdded = True
                                                End If

                                            End If

                                            'write the lines to the output and act like nothing has happened
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()

                                            'R2.24 CR: BUG FIX
                                            'rewrite the account ref if this is a refund
                                        ElseIf xmlreader.Name = "Account" AndAlso blnChangeAgencyCancellation Then
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, notString(getConfig("RefundExternalRefReplacement")), False, "externalref"))
                                            xmlreader.MoveToElement()

                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, notString(getConfig("RefundExternalRefReplacement")), False, "externalref"))
                                            xmlreader.MoveToElement()

                                            'R2.24 CR: BUG FIX
                                            'handle refunds
                                        ElseIf xmlreader.Name = "Refund" Or xmlreader.Name = "Sale" Then
                                            blnInsideRefundOrSaleNode = xmlreader.IsStartElement

                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()

                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()

                                        ElseIf xmlreader.Name = "DiscountReclaimed" AndAlso blnInsideRefundOrSaleNode Then
                                            dblRefundCancellationCharge += notDouble(xmlreader.GetAttribute("TotalAmount"))
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()

                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()
                                        ElseIf xmlreader.Name = "ATOCCancellationCharge" AndAlso blnInsideRefundOrSaleNode Then
                                            dblRefundCancellationCharge += notDouble(xmlreader.GetAttribute("TotalAmount"))
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", True))
                                            xmlreader.MoveToElement()

                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", True))
                                            xmlreader.MoveToElement()
                                        ElseIf xmlreader.Name = "AgencyCancellationCharge" AndAlso blnInsideRefundOrSaleNode Then

                                            'R2.5 CR - code inside else is all old coding
                                            If blnChangeAgencyCancellation Then
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, getConfig("RefundChargeReplacement"), False, "TotalAmount"))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, getConfig("RefundChargeReplacement"), False, "TotalAmount"))
                                                xmlreader.MoveToElement()
                                                dblRefundCancellationCharge = 0
                                            Else
                                                'write special for this, use the total dblRefundCancellationCharge
                                                'send new value in FulfilmentFee parameter, will write correctly in xml file

                                                'DO NOT add totalamount of this row to dblRefundCancellationCharge - processXML() function will
                                                ' add the value of dblRefundCancellationCharge to this nodes existing TotalAmount attribute automatically
                                                xmlBOSSwriter.WriteRaw(processXML(xmlreader, dblRefundCancellationCharge, False, "", False))
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(processXML(xmlreader, dblRefundCancellationCharge, False, "", False))
                                                xmlreader.MoveToElement()

                                                'Reset the cancellation charge in case another ticket occurance appears
                                                dblRefundCancellationCharge = 0
                                            End If

                                            'R2.5 CR
                                        ElseIf xmlreader.Name = "CustomField" AndAlso xmlreader.GetAttribute("Code") = "1" AndAlso blnChangeAgencyCancellation Then
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "NYS", False))
                                            xmlreader.MoveToElement()

                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "NYS", False))
                                            xmlreader.MoveToElement()

                                            'R2.5 CR
                                        ElseIf xmlreader.Name = "Account" AndAlso blnChangeAgencyCancellation Then
                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, notString(getConfig("RefundExternalRefReplacement")), False, "externalref"))
                                            xmlreader.MoveToElement()

                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, notString(getConfig("RefundExternalRefReplacement")), False, "externalref"))
                                            xmlreader.MoveToElement()

                                            'R2.23 SA 
                                        ElseIf xmlreader.Name.ToLower = "fare" AndAlso xmlreader.IsStartElement Then

                                            'write the fare first then sort out the discount
                                            Dim dblFare As Double = 0
                                            Dim strfare As String = ""
                                            dblFare = xmlreader.GetAttribute("TotalAmount")
                                            'strfare = ("<Fare VATAmount=""0.00"" VATCode=""1"" TotalAmount=""" & CStr(dblFare) & """ />")
                                            strfare = ("<Fare TotalAmount=""" & CStr(dblFare) & """ VATAmount=""0.00"" VATCode=""1"" />")


                                            xmlBOSSwriter.WriteRaw(strfare)
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(strfare)
                                            xmlreader.MoveToElement()

                                            'get client details 
                                            oClsInterceptorClient = clsInterceptorClient.checkBossCode(strBossCode, 0)
                                            If oClsInterceptorClient.InterceptorDiscountActive Then

                                                Dim dblDiscountPercent As Double = oClsInterceptorClient.InterceptorDiscountPercent
                                                Dim dblDiscount As Double = Math.Round((dblFare * dblDiscountPercent) / 100, 2)
                                                Dim strDiscount As String = ""
                                                'strDiscount = ("<Discount VATAmount=""0.00"" VATCode=""1"" TotalAmount=""" & CStr(dblDiscount) & """ />")
                                                strDiscount = ("<Discount TotalAmount=""" & CStr(dblDiscount) & """ VATAmount=""0.00"" VATCode=""1"" />")

                                                xmlBOSSwriter.WriteRaw(strDiscount)
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(strDiscount)
                                                xmlreader.MoveToElement()
                                            End If
                                        Else

                                            'R2.22.1 SA 
                                            Dim strRef As String = "" 'to write order ref
                                            'check it is end of segment
                                            If xmlreader.Name = "Segment" And Not xmlreader.IsStartElement Then
                                                strRef = ("<CustomField Code=""ORDERREF"" Value=""" & strOrderRef & """ />")
                                            End If

                                            If xmlreader.Name = "Ticket" And xmlreader.IsStartElement Then
                                                intCurrentTicket += 1
                                                strCurrentFulfilmentRef = xmlreader.GetAttribute("PassengerRef")

                                                'R2.22?? CR
                                                blnInsideTicketNode = True
                                            ElseIf xmlreader.Name = "Ticket" And Not xmlreader.IsStartElement Then
                                                blnInsideTicketNode = False
                                            End If

                                            xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()
                                            xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                            xmlreader.MoveToElement()

                                            'check if ref is not blank then write it to file 
                                            If strRef <> "" Then
                                                xmlBOSSwriter.WriteRaw(strRef)
                                                xmlreader.MoveToElement()
                                                xmlSQLwriter.WriteRaw(strRef)
                                                xmlreader.MoveToElement()
                                            End If

                                        End If
                                        End If
                                        strPreviousNode = xmlreader.Name
                                End While
                            End If

                            xmlreader.Close()
                            xmlBOSSwriter.Close()
                            'R2.3 CR
                            xmlSQLwriter.Close()
                            fstOldFile.Close()
                            fstBOSSFile.Close()
                            'R2.3 CR
                            fstSQLFile.Close()
                        Next

                        'dispose of dictionary
                        dicPassengerSkipPost.Clear()
                        dicPassengerSkipPost = Nothing
                    Else
                        'if BOSS Code is empty or not found - save file to output folder anyway
                        'BOSS can handle any reference code errors
                        Try
                            Dim fstCopyFileToBoss As New FileStream(pstrWriteToBossFolder & File.Name, FileMode.Create)
                            xmlBOSSwriter = New XmlTextWriter(fstCopyFileToBoss, System.Text.Encoding.UTF8)
                            xmlBOSSwriter.Formatting = Formatting.Indented
                            xmlBOSSwriter.WriteStartDocument()

                            fstOldFile = Nothing
                            fstOldFile = New FileStream(File.FullName, FileMode.Open)
                            xmlreader = xmlreader.Create(fstOldFile)

                            While xmlreader.Read
                                If xmlreader.Name <> "xml" Then
                                    'xmlBOSSwriter.WriteNode(xmlreader, True)
                                    xmlBOSSwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                    xmlreader.MoveToElement()
                                End If
                            End While

                            xmlBOSSwriter.Close()
                            fstCopyFileToBoss.Close()
                            xmlreader.Close()
                            fstOldFile.Close()

                        Catch ex As Exception
                            Throw New Exception("File copy to BOSS folder failed (" & ex.Message & ")")
                        End Try

                        'R2.3 CR
                        'copy file to SQL folder
                        Try
                            Dim fstCopyFileToSql As New FileStream(pstrWriteToSqlFolder & File.Name, FileMode.Create)
                            xmlSQLwriter = New XmlTextWriter(fstCopyFileToSql, System.Text.Encoding.UTF8)
                            xmlSQLwriter.Formatting = Formatting.Indented
                            xmlSQLwriter.WriteStartDocument()

                            fstOldFile = Nothing
                            fstOldFile = New FileStream(File.FullName, FileMode.Open)
                            xmlreader = xmlreader.Create(fstOldFile)
                            While xmlreader.Read
                                If xmlreader.Name <> "xml" Then
                                    'xmlSQLwriter.WriteNode(xmlreader, True)
                                    xmlSQLwriter.WriteRaw(processXML(xmlreader, 0, False, "", False))
                                    xmlreader.MoveToElement()
                                End If
                            End While

                            xmlSQLwriter.Close()
                            fstCopyFileToSql.Close()
                            xmlreader.Close()
                            fstOldFile.Close()
                        Catch ex As Exception
                            Throw New Exception("File copy to SQL folder failed (" & ex.Message & ")")
                        End Try

                        'R2.13 CR
                        ' send helpdesk an email about the missing client
                        Try
                            'email helpdesk - let them know that the client doesn't exist
                            'SendEmail.send("service@nysgroup.com", _
                            '               "nyshelpdesk@nysgroup.com", _
                            '               "Interceptor Client Missing", _
                            '               "Interceptor has encountered a data error.<br/>BOSS code in the Handoff file was not found as an interceptor client.<br/>" & _
                            '               "<p>BOSS code in handoff file is: " & strBossCode & "<br/>Ticket Issue Ref is: " & strIssueRef & "</p>" & _
                            '               "Handoff has been passed to BOSS as normal, but please enter this client for future bookings in case additional fees are needed.", _
                            '               "sarab.azzouz@nysgroup.com", "", "")
                            Throw New Exception("Interceptor Client Missing")
                        Catch ex As Exception
                            log.Error("Unable to send error email, client " & strBossCode & " missing from Interceptor")
                        End Try

                    End If

skipfile:
                    'R2.14 CR - error trapping
                    If strSplitNode <> "" _
                        And strSplitFieldName <> "" _
                        And strSplitFieldValue <> "" _
                        And strSplitUniqueFieldName <> "" _
                        And dictPassengerVSplitValue.Count = 0 Then
                        'means the file wont reach boss, splitting set up but hasnt found anything unique
                        ' copy it into errors folder
                        IO.File.Copy(File.FullName, getConfig("InterceptorErrorsFolder") & File.Name)

                        'R2.16 CR
                        log.Error("Splitting set up for client but couldn't find unique field, file copied to error folder")
                        'R2.20C CR
                    ElseIf blnIsDataError Then
                        IO.File.Copy(File.FullName, getConfig("InterceptorErrorsFolder") & File.Name)
                        'R2.21.8 SA 
                    ElseIf blnNonIssue Then
                        IO.File.Copy(File.FullName, getConfig("InterceptorNonIssueFolder") & File.Name)
                    End If

                    'bin when done
                    IO.File.Delete(File.FullName)

                Next
            Else
                log.Error("MI Evolvi - One of the specified folders does not exist")
            End If
        Catch ex As Exception
            xmlreader = Nothing
            xmlreader.Close()
            xmlBOSSwriter = Nothing
            xmlBOSSwriter.Close()
            fstOldFile = Nothing
            fstOldFile.Close()
            fstBOSSFile = Nothing
            fstBOSSFile.Close()

            'R2.3 CR
            xmlSQLwriter = Nothing
            xmlSQLwriter.Close()
            fstSQLFile = Nothing
            fstSQLFile.Close()

            log.Error("Read Failure - MI Evolvi: " & ex.Message)
        End Try
    End Sub
   

    Private Function GuardianProcessXML(ByVal prdrReader As XmlReader, _
                                      ByVal pdblFulfilmentFee As Double, _
                                      ByVal pblnFromBooker As Boolean, _
                                      ByVal pstrNewNodeValue As String, _
                                      ByVal pblnEmptyTotalAmount As Boolean, _
        Optional ByVal pstrNewValueName As String = "value") As String

        Dim strXml As New StringBuilder

        If prdrReader.IsStartElement Then
            If prdrReader.IsEmptyElement Then
                'finish with a />
                strXml.Append("<" & prdrReader.Name)
                If pdblFulfilmentFee > 0 Or pblnEmptyTotalAmount Then
                    If prdrReader.HasAttributes Or pblnEmptyTotalAmount Then
                        prdrReader.MoveToFirstAttribute()
                        If prdrReader.Name = "TotalAmount" Then

                            'R2.3 CR
                            Dim dblCurrentValue As Double = CDbl(prdrReader.Value)
                            If pblnEmptyTotalAmount Then
                                dblCurrentValue = 0
                            End If

                            strXml.Append(" " & prdrReader.Name & "=""" & FormatNumber(pdblFulfilmentFee + dblCurrentValue, 2) & """")

                        Else
                            strXml.Append(" " & prdrReader.Name & "=""" & prdrReader.Value & """")
                        End If

                        While prdrReader.MoveToNextAttribute
                            If prdrReader.Name = "TotalAmount" Then

                                'R2.3 CR
                                Dim dblCurrentValue As Double = CDbl(prdrReader.Value)
                                If pblnEmptyTotalAmount Then
                                    dblCurrentValue = 0
                                End If

                                strXml.Append(" " & prdrReader.Name & "=""" & FormatNumber(pdblFulfilmentFee + dblCurrentValue, 2) & """")

                            Else
                                strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                            End If
                        End While
                    End If
                Else
                    If prdrReader.HasAttributes Then
                        prdrReader.MoveToFirstAttribute()

                        'R2.18 CR - quick fix for evolvi v4 go live crossover
                        ' just so we can force Interceptor to add the fees on while using the v4 handoffs (in case we haven't set up the new structure)
                        Dim strSchemaReplacement As String = getConfig("HandoffSchemaVersionReplacement")

                        'R2.18 CR - if for quick fix
                        If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                            strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")

                        Else
                            'R2.5 CR - replaced "value" with pstrNewValueName.tolower
                            If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue, "&", "&amp;") & """")
                            Else
                                strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                            End If
                        End If

                        While prdrReader.MoveToNextAttribute
                            If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                                strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")

                            Else
                                'R2.5 CR - replaced "value" with pstrNewValueName.tolower
                                If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                    strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue, "&", "&amp;") & """")
                                Else
                                    strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                                End If
                            End If
                        End While
                    End If
                End If

                strXml = strXml.Append("/>")
            Else
                'finish with a >
                strXml.Append("<" & prdrReader.Name)

                'R2.18 CR - quick fix for evolvi v4 go live crossover
                ' just so we can force Interceptor to add the fees on while using the v4 handoffs (in case we haven't set up the new structure)
                Dim strSchemaReplacement As String = getConfig("HandoffSchemaVersionReplacement")

                If prdrReader.HasAttributes Then
                    prdrReader.MoveToFirstAttribute()

                    If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")
                    Else
                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                    End If

                    While prdrReader.MoveToNextAttribute
                        If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                            strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")
                        Else
                            strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                        End If
                    End While
                End If
                strXml.Append(">")
            End If
        Else
            If prdrReader.NodeType = XmlNodeType.EndElement Then
                strXml.Append("</" & prdrReader.Name & ">")
            End If
        End If

        Return strXml.ToString

    End Function

    'R2.5 CR - added optional pstrNewValueName, defaulted to "value"
    'pass name of attribute to this that you want the pstrNewNodeValue to go into
    Private Function processXML(ByVal prdrReader As XmlReader, _
                                ByVal pdblFulfilmentFee As Double, _
                                ByVal pblnFromBooker As Boolean, _
                                ByVal pstrNewNodeValue As String, _
                                ByVal pblnEmptyTotalAmount As Boolean, _
                                Optional ByVal pstrNewValueName As String = "value") As String
        Using New clslogger(log, className, "processXML")
            Dim strXml As New StringBuilder

            Dim blnCustomField As Boolean = False
            If prdrReader.Name = "CustomField" Then
                blnCustomField = True
            End If

            If blnCustomField _
                AndAlso prdrReader.GetAttribute("Value") = "" _
                AndAlso (pstrNewValueName.ToLower = "value" And pstrNewNodeValue = "") Then
                'skip it - we dont want it on the output otherwise BOSS will cry
            Else
                'its all good - keep going
                If prdrReader.IsStartElement Then
                    If prdrReader.IsEmptyElement Then
                        'finish with a />
                        strXml.Append("<" & prdrReader.Name)
                        If pdblFulfilmentFee > 0 Or pblnEmptyTotalAmount Then
                            If prdrReader.HasAttributes Or pblnEmptyTotalAmount Then
                                prdrReader.MoveToFirstAttribute()
                                If prdrReader.Name = "TotalAmount" Then

                                    'R2.3 CR
                                    Dim dblCurrentValue As Double = CDbl(prdrReader.Value)
                                    If pblnEmptyTotalAmount Then
                                        dblCurrentValue = 0
                                    End If

                                    strXml.Append(" " & prdrReader.Name & "=""" & FormatNumber(pdblFulfilmentFee + dblCurrentValue, 2) & """")

                                    'R2.23 CR - BUG FIX: Make the value attributes on CustomField nodes upper case
                                ElseIf blnCustomField AndAlso prdrReader.Name = "Value" Then
                                    strXml.Append(" " & prdrReader.Name & "=""" & prdrReader.Value.ToUpper & """")

                                Else
                                    strXml.Append(" " & prdrReader.Name & "=""" & prdrReader.Value & """")
                                End If

                                While prdrReader.MoveToNextAttribute
                                    If prdrReader.Name = "TotalAmount" Then

                                        'R2.3 CR
                                        Dim dblCurrentValue As Double = CDbl(prdrReader.Value)
                                        If pblnEmptyTotalAmount Then
                                            dblCurrentValue = 0
                                        End If

                                        strXml.Append(" " & prdrReader.Name & "=""" & FormatNumber(pdblFulfilmentFee + dblCurrentValue, 2) & """")

                                        'R2.23 CR - BUG FIX: Make the value attributes on CustomField nodes upper case
                                    ElseIf blnCustomField AndAlso prdrReader.Name = "Value" Then
                                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value.ToUpper, "&", "&amp;") & """")

                                    Else
                                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                                    End If
                                End While
                            End If
                        Else
                            If prdrReader.HasAttributes Then
                                prdrReader.MoveToFirstAttribute()

                                ''R2.18 CR - if for quick fix
                                'If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                                '    strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")

                                'R2.23 CR - BUG FIX: Make the value attributes on CustomField nodes upper case
                                If blnCustomField AndAlso prdrReader.Name = "Value" Then
                                    If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue.ToUpper, "&", "&amp;") & """")
                                    Else
                                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value.ToUpper, "&", "&amp;") & """")
                                    End If

                                Else
                                    'R2.5 CR - replaced "value" with pstrNewValueName.tolower
                                    If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue, "&", "&amp;") & """")
                                    Else
                                        strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                                    End If
                                End If

                                While prdrReader.MoveToNextAttribute
                                    'If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                                    '    strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")

                                    'R2.23 CR - BUG FIX: Make the value attributes on CustomField nodes upper case
                                    If blnCustomField AndAlso prdrReader.Name = "Value" Then
                                        If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                            strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue.ToUpper, "&", "&amp;") & """")
                                        Else
                                            strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value.ToUpper, "&", "&amp;") & """")
                                        End If

                                    Else
                                        'R2.5 CR - replaced "value" with pstrNewValueName.tolower
                                        If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                            strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue, "&", "&amp;") & """")
                                        Else
                                            strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                                        End If
                                    End If
                                End While
                            End If
                        End If

                        strXml = strXml.Append("/>")
                    Else
                        'finish with a >
                        strXml.Append("<" & prdrReader.Name)

                        'R2.18 CR - quick fix for evolvi v4 go live crossover
                        ' just so we can force Interceptor to add the fees on while using the v4 handoffs (in case we haven't set up the new structure)
                        'Dim strSchemaReplacement As String = getConfig("HandoffSchemaVersionReplacement")

                        If prdrReader.HasAttributes Then
                            prdrReader.MoveToFirstAttribute()

                            'If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                            '    strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")
                            'Else

                            'R2.22 CR - below code will fix sequence numbers not getting changed inside each file
                            'will also fix invoice date issue where it isn't being changed...
                            If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue.ToUpper, "&", "&amp;") & """")
                            Else
                                strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                            End If
                            'strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")

                            'End If

                            While prdrReader.MoveToNextAttribute
                                'If prdrReader.Name.ToLower = "schemaversion" AndAlso strSchemaReplacement <> "" Then
                                'strXml.Append(" " & prdrReader.Name & "=""" & Replace(strSchemaReplacement, "&", "&amp;") & """")
                                'Else

                                'R2.22 CR - below code will fix sequence numbers not getting changed inside each file
                                'will also fix invoice date issue where it isn't being changed...
                                If prdrReader.Name.ToLower = pstrNewValueName.ToLower And pstrNewNodeValue <> "" Then
                                    strXml.Append(" " & prdrReader.Name & "=""" & Replace(pstrNewNodeValue.ToUpper, "&", "&amp;") & """")
                                Else
                                    strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")
                                End If

                                'strXml.Append(" " & prdrReader.Name & "=""" & Replace(prdrReader.Value, "&", "&amp;") & """")

                                'End If
                            End While
                        End If
                        strXml.Append(">")
                    End If
                Else
                    If prdrReader.NodeType = XmlNodeType.EndElement Then
                        strXml.Append("</" & prdrReader.Name & ">")
                    End If
                End If


            End If
            Return strXml.ToString
        End Using
    End Function

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Using New clslogger(log, className, "btnSubmit_Click")
            Try
                If Not txtBossOutputFolder.Text.EndsWith("\") Then
                    txtBossOutputFolder.Text = txtBossOutputFolder.Text & "\"
                End If

                'R2.3 CR
                If Not txtSqlOutputFolder.Text.EndsWith("\") Then
                    txtSqlOutputFolder.Text = txtSqlOutputFolder.Text & "\"
                End If

                'R2.16 CR - do the guardian processing first, if specified
                If txtGuardianLocation.Text <> "" Then
                    InterceptorGuardian(txtGuardianLocation.Text, txtGuardianOutLocation.Text)
                End If

                ProcessFolder(txtFolderLocation.Text, txtBossOutputFolder.Text, txtSqlOutputFolder.Text)
                ltrOutputString.Text = "<p>Process Complete</p>"
            Catch ex As Exception
                handleexception(ex, "IATestEvolvi", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub IATestEvolvi_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IATestEvolvi_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IATestEvolvi"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IATestEvolvi", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    ''R
    'Protected Sub dowork_Click(sender As Object, e As EventArgs) Handles dowork.Click
    '    Using New clslogger(log, className, "dowork_Click")
    '        Try
    '            Dim strEmployeeNumber As String = txtinput.Text
    '            If strEmployeeNumber.Contains("/") Then
    '                strEmployeeNumber = strEmployeeNumber.Substring(1)
    '                'strEmployeeNumber = strEmployeeNumber.Replace("/", "")
    '                'If strEmployeeNumber.Length < 7 Then
    '                '    strEmployeeNumber = "0" + strEmployeeNumber
    '                'End If
    '            End If
    '            txtoutput.Text = strEmployeeNumber

    '        Catch ex As Exception
    '            handleexception(ex, "IATestEvolvi", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Protected Sub btnAutoInvoice_Click(sender As Object, e As EventArgs) Handles btnAutoInvoice.Click
    '    Using New clslogger(log, className, "btnAutoInvoice_Click")
    '        Try
    '            AutoInvoiceProcessFolder()
    '        Catch ex As Exception
    '            handleexception(ex, "IATestEvolvi", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    Public Sub AutoInvoiceProcessFolder()

        log.Info("AUTO Invoice Process Folder - Starting")
        Try

            Dim strDatePath As String = "_" & CStr(Date.Now.Year) & IIf(Date.Now.Month < 10, "0" & CStr(Date.Now.Month), CStr(Date.Now.Month))
            Dim strReadFromFolder As String = getConfig("InvoiceInput")
            Dim strSentFolder As String = getConfig("InvoiceSent").Replace("###", strDatePath)
            Dim strNotSentFolder As String = getConfig("InvoiceNotSent").Replace("###", strDatePath)
            Dim strErrorFolder As String = getConfig("InvoiceError").Replace("###", strDatePath)
            Dim strAwaitingFolder As String = getConfig("InvoiceAwaiting")

            Dim diInputFolder As New DirectoryInfo(strReadFromFolder)
            Dim diInputFolder1 As New DirectoryInfo(strSentFolder)
            Dim diInputFolder2 As New DirectoryInfo(strNotSentFolder)
            Dim diInputFolder3 As New DirectoryInfo(strErrorFolder)
            Dim diInputFolder4 As New DirectoryInfo(strAwaitingFolder)

            If Not diInputFolder.Exists Then
                diInputFolder.Create()
            End If
            If Not diInputFolder1.Exists Then
                diInputFolder1.Create()
            End If
            If Not diInputFolder2.Exists Then
                diInputFolder2.Create()
            End If
            If Not diInputFolder3.Exists Then
                diInputFolder3.Create()
            End If
            If Not diInputFolder4.Exists Then
                diInputFolder4.Create()
            End If

            Dim strCompanyName As String = ""

            If diInputFolder.Exists And diInputFolder1.Exists And diInputFolder2.Exists And diInputFolder3.Exists And diInputFolder4.Exists Then
                Dim count As Integer = 0
                'loop through all files in folder
                For Each aFile As FileSystemInfo In diInputFolder.GetFiles
                    Dim strFileName As String = aFile.Name
                    Dim oInvoices As New List(Of clsAutoInvoice)
                    'Dim oEvolviEmails As New List(Of clsAutoInvoice)
                    Dim oAirEmails As New List(Of clsAutoInvoice)
                    'R2.21 
                    Dim oConfermaEmails As New List(Of clsAutoInvoice)

                    'R?? SA file name dosen't contain M any more- after boss update on 29/10/2013
                    ' If strFileName.ToLower.Contains("m.pdf") Then
                    If strFileName.ToLower.Contains(".pdf") Then

                        'TODO: update SQL with BOSS values before running - make sure we have the SQL data!
                        'will need the server to have the foxpro driver installed

                        'R?? SA file name dosen't contain M any more- after boss update on 29/10/2013
                        'oInvoices = clsAutoInvoice.checkInvoice(strFileName.ToLower.Replace("m.pdf", ""))
                        oInvoices = clsAutoInvoice.checkInvoice(strFileName.ToLower.Replace(".pdf", ""))

                        If oInvoices.Count > 0 Then 'should actually only be one!
                            If IO.File.Exists(strSentFolder & strFileName) Then 'file has already been sent so don't bother again

                                IO.File.Copy(strReadFromFolder & strFileName, strSentFolder & strFileName, True)
                                IO.File.Delete(strReadFromFolder & strFileName)
                            Else
                                For Each oInvoice As clsAutoInvoice In oInvoices
                                    Try
                                        'R2.21 SA 
                                        strCompanyName = oInvoice.Client.ToUpper.Trim

                                        If strCompanyName = "FERA" Or strCompanyName = "YDH R" Or strCompanyName = "YDH" Then
                                            Dim strFirstName As String = ""
                                            Dim strLastName As String = ""
                                            Dim strEmailTo As String = ""

                                            If strCompanyName = "FERA" AndAlso oInvoice.PONumber.Trim <> "" AndAlso IsNumeric(oInvoice.PONumber.Trim.ToLower.Replace("p", "")) Then
                                                Try
                                                    'Dim inttest As Integer = CInt(oInvoice.PONumber.Trim.ToLower.Replace("p", ""))
                                                    'Dim strtest As String = oInvoice.PONumber.Trim.ToLower.Replace("p", "")
                                                    ' If IsNumeric(strtest) Then
                                                    'all good so send to proc email
                                                    strEmailTo = getConfig("FeraProcurementEmail")
                                                    'End If

                                                Catch ex As Exception
                                                    'rail 
                                                    strEmailTo = CheckEvolviEmail(oInvoice.RecordLocator.Trim)
                                                    'Air
                                                    If strEmailTo = "" Then
                                                        strEmailTo = CheckAirEmail(oInvoice.RecordLocator.Trim)
                                                    End If
                                                End Try
                                            Else
                                                If oInvoice.RecordLocator.Trim <> "" Then
                                                    'R2.21 SA
                                                    If IsNumeric(oInvoice.RecordLocator.Trim) Then  'RAIL
                                                        strEmailTo = CheckEvolviEmail(oInvoice.RecordLocator.Trim)
                                                    Else
                                                        'CHECK AIR
                                                        strEmailTo = CheckAirEmail(oInvoice.RecordLocator.Trim)
                                                        'CHECK HOTEL
                                                        If strEmailTo = "" Then
                                                            strEmailTo = CheckConfermaEmail(oInvoice.RecordLocator.Trim)
                                                        End If
                                                    End If
                                                ElseIf oInvoice.RecordLocator.Trim = "" AndAlso strCompanyName = "YDH R" AndAlso oInvoice.Supplier = "GENPOST" Then
                                                    strEmailTo = getConfig("YDHRPostChargeEmail")
                                                Else
                                                    'no locator  
                                                    strEmailTo = ""
                                                End If
                                            End If

                                            'make sure email is in the correct format
                                            Dim mRegExp As New Regex(emailRegex)
                                            If Not mRegExp.IsMatch(strEmailTo) Then
                                                strEmailTo = "" ' getConfig("FeraProcurementEmail")
                                            End If

                                            If strEmailTo <> "" Then

                                                Dim blnSent As Boolean = sendAutoInvoiceEmail(strEmailTo, getConfig("AutoInvoiceEmailFrom"), strReadFromFolder & strFileName, strFirstName, False)
                                                If blnSent Then

                                                    'move to sent folder
                                                    If IO.File.Exists(strSentFolder & strFileName) Then
                                                        IO.File.Copy(strReadFromFolder & strFileName, strSentFolder & strFileName, True)
                                                        IO.File.Delete(strReadFromFolder & strFileName)
                                                    Else
                                                        IO.File.Move(strReadFromFolder & strFileName, strSentFolder & strFileName)
                                                        'IO.File.Delete(strReadFromFolder & strFileName)
                                                    End If
                                                Else
                                                    'R2.21 SA - log per company name 
                                                    log.Error("Send Email failed - " & strCompanyName & ": " & strFileName)
                                                    sendAutoInvoiceEmail("", "", strFileName, "", True)
                                                    'move to error folder
                                                    If IO.File.Exists(strErrorFolder & strFileName) Then
                                                        IO.File.Copy(strReadFromFolder & strFileName, strErrorFolder & strFileName, True)
                                                        IO.File.Delete(strReadFromFolder & strFileName)
                                                    Else
                                                        IO.File.Move(strReadFromFolder & strFileName, strErrorFolder & strFileName)
                                                        ' IO.File.Delete(strReadFromFolder & strFileName)
                                                    End If
                                                End If
                                            Else
                                                'R2.21 SA - log per company name 
                                                log.Error("No email - " & strCompanyName & ": " & strFileName)
                                                sendAutoInvoiceEmail("", "", strFileName, "", True)
                                                'move to error folder
                                                If IO.File.Exists(strErrorFolder & strFileName) Then
                                                    IO.File.Copy(strReadFromFolder & strFileName, strErrorFolder & strFileName, True)
                                                    IO.File.Delete(strReadFromFolder & strFileName)
                                                Else
                                                    IO.File.Move(strReadFromFolder & strFileName, strErrorFolder & strFileName)
                                                    'IO.File.Delete(strReadFromFolder & strFileName)
                                                End If
                                            End If
                                        Else
                                            'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
                                            If DateTime.Now.Hour >= notInteger(getConfig("StartMovePDFFilesHour")) Then
                                                'definately not Fera so move
                                                If IO.File.Exists(strNotSentFolder & strFileName) Then
                                                    IO.File.Copy(strReadFromFolder & strFileName, strNotSentFolder & strFileName, True)
                                                    IO.File.Delete(strReadFromFolder & strFileName)
                                                Else
                                                    IO.File.Move(strReadFromFolder & strFileName, strNotSentFolder & strFileName)

                                                    ' IO.File.Delete(strReadFromFolder & strFileName)
                                                End If
                                            End If
                                        End If

                                    Catch ex As Exception
                                        'R2.21 SA - log for company name instead of just for FERA
                                        log.Error("Failure retrieving email - " & strCompanyName & ": " & strFileName & " **Ex.Message=" & ex.Message)
                                        sendAutoInvoiceEmail("", "", strFileName, "", True)
                                        'move to error folder
                                        If IO.File.Exists(strErrorFolder & strFileName) Then
                                            IO.File.Copy(strReadFromFolder & strFileName, strErrorFolder & strFileName, True)
                                            IO.File.Delete(strReadFromFolder & strFileName)
                                        Else
                                            IO.File.Move(strReadFromFolder & strFileName, strErrorFolder & strFileName)
                                            ' IO.File.Delete(strReadFromFolder & strFileName)
                                        End If
                                    End Try
                                Next
                            End If
                        Else
                            'could still be Fera / YDH , but record not yet transfered to SQL from BOSS
                            'need to move to diff folder so can poll that at a later date

                            'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
                            If DateTime.Now.Hour >= notInteger(getConfig("StartMovePDFFilesHour")) Then
                                If IO.File.Exists(strAwaitingFolder & strFileName) Then
                                    IO.File.Delete(strAwaitingFolder & strFileName)
                                    IO.File.Move(strReadFromFolder & strFileName, strAwaitingFolder & strFileName)
                                    'IO.File.Delete(strReadFromFolder & strFileName)
                                Else
                                    IO.File.Move(strReadFromFolder & strFileName, strAwaitingFolder & strFileName)
                                    'IO.File.Delete(strReadFromFolder & strFileName)
                                End If
                            End If
                        End If
                    Else
                        'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
                        If DateTime.Now.Hour >= notInteger(getConfig("StartMovePDFFilesHour")) Then
                            'definately not Fera so move
                            If IO.File.Exists(strNotSentFolder & strFileName) Then
                                IO.File.Copy(strReadFromFolder & strFileName, strNotSentFolder & strFileName, True)
                                IO.File.Delete(strReadFromFolder & strFileName)
                            Else
                                IO.File.Move(strReadFromFolder & strFileName, strNotSentFolder & strFileName)
                                'IO.File.Delete(strReadFromFolder & strFileName)
                            End If
                        End If

                    End If
                Next
            Else
                log.Error("Fera/YDH/YDH R - One of the specified folders does not exist")
            End If
        Catch ex As Exception
            log.Error("Read Failure - Fera/YDH/YDH R: " & ex.Message)
            log.Error("AUTO Invoice Process Folder - exception thrown: " & ex.Message)
        End Try

        log.Info("AUTO Invoice Process Folder - Ending")
    End Sub

    Private Function sendAutoInvoiceEmail(ByVal pEmailTo As String, _
                                ByVal pEmailFrom As String, _
                                ByVal pFile As String, _
                                ByVal pFirstName As String, _
                                ByVal pError As Boolean) As Boolean

        If Not pError Then
            Dim ofile As New System.IO.StreamReader(getConfig("HomeAbsolutepath") & "userdocs\NYS\AutoInvoiceEmail.htm")

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
                SendEmail.send("rail@nysgroup.com", "sarab.azzouz@nysgroup.com", "PDF Invoice could not be sent", "Invoice: " & pFile, "", "", "", "")
                Return True
            Catch ex As Exception
                log.Error("ERROR IN SENDAUTOINVOICEEMAIL: " & ex.Message)
                Return False
            End Try
        End If
    End Function

    'R2.21 SA
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

    'R2.21 SA
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

    'R2.21 SA 
    Private Function CheckConfermaEmail(ByVal pLocator As String) As String
        Dim strFirstName As String = ""
        Dim strLastName As String = ""
        Dim strEmailTo As String = ""
        Dim oConfermaEmails As New List(Of clsAutoInvoice)

        oConfermaEmails = clsAutoInvoice.ConfermaEmail(pLocator)
        If oConfermaEmails.Count > 0 Then
            For Each oEmail As clsAutoInvoice In oConfermaEmails
                strFirstName = oEmail.FirstName.Trim
                strLastName = oEmail.LastName.Trim
                strEmailTo = oEmail.EmailAddress.Trim
            Next
            If strEmailTo = "" Then
                strEmailTo = clsAutoInvoice.EmailCheck(strFirstName, strLastName)
            End If
        End If

        Return strEmailTo
    End Function

End Class