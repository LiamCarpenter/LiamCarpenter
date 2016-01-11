Option Explicit On
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Globalization
Imports DatabaseObjects
Imports EvoUtilities.ConfigUtils
Imports ReportDownloader.Utility
Imports CSVParser

Public Class CSVReaderTrans

    Private Shared ReadOnly className As String

    Protected Shared log As log4net.ILog = _
      log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    ''' <summary>
    ''' Function Main - downloads today's CSV file if it hasn't already been dowloaded, then passes to 
    ''' importFile method.
    ''' </summary>
    ''' <returns>
    ''' True/False depending upon validity of data downloaded
    ''' </returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    ''' '
    Public Function Main(ByVal pOurFileDate As String) As Boolean
        'check if file exists
        Try
            '
            Dim blnOk As Boolean = True
            If IO.File.Exists(getConfig("downloadedfilesTrans") & pOurFileDate & ".csv") Then
                If Not importFile(getConfig("downloadedfilesTrans") & pOurFileDate & ".csv", pOurFileDate & ".csv") Then
                    blnOk = False
                End If
            End If
            'End If

            Return blnOk
        Catch ex As Exception
            log.Error(ex.Message)
            Return False
        End Try
    End Function 'Main

    ''' <summary>
    ''' Function readAllText - reads the passed in file if it exists
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <returns>An empty string denoting the file doesn't exist</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Public Shared Function readAllText(ByVal filename As String, ByVal pOurFileDate As String) As String
        Try

            Dim fr As StreamReader = Nothing
            Dim frW As New StringWriter(CultureInfo.CurrentCulture)

            Dim FileString As String = ""
            Dim LineItemsArr() As String

            Dim FilePath As String = filename

            fr = New System.IO.StreamReader(FilePath)

            While fr.Peek <> -1
                FileString = fr.ReadLine.Trim

                If String.IsNullOrEmpty(FileString) Then Continue While 'Empty Line

                LineItemsArr = FileString.Split(CChar(","))

                For Each Item As String In LineItemsArr
                    'If every item will have a beginning and closing " (quote) then you can just
                    'cut the first and last characters of the string here.
                    'i.e.  UpdatedItems = Item. remove first and last character

                    'R2.17 CR
                    'with splitting by comma you can end up with part fields in this section acting like full fields
                    'so we need to check if the filed starts with a double quote - if it does then make sure it has them when being written
                    'if not then dont add them in
                    Dim strStartingChar As String = ""
                    Dim strEndingChar As String = ""
                    If Item.Trim.StartsWith(Chr(34)) Then
                        'we need to add a starting double quote
                        strStartingChar = Chr(34)
                    End If
                    If Item.Trim.EndsWith(Chr(34)) Then
                        'we need to add an ending double quote
                        strEndingChar = Chr(34)
                    End If

                    'Then stick the data into your Generic List (Of String()?)
                    Dim strTestS As String = Item.Replace("""", "")
                    strTestS = strTestS.Replace(vbCrLf, "")
                    strTestS = strTestS.Replace(vbCr, "")
                    strTestS = strTestS.Replace(vbLf, "")

                    'R2.17 CR - add the quotes around the outside of the values
                    frW.Write(strStartingChar & ToCsvCell(clsStuff.notString(strTestS)) & strEndingChar)
                    frW.Write(",")
                Next
                frW.WriteLine("")
            End While
            fr.Close()


            Dim ofiler As New System.IO.StreamWriter(getConfig("downloadedfilesTrans") & _
                                                     pOurFileDate & ".csv", False, Encoding.Default)
            ofiler.Write(frW.ToString)
            ofiler.Flush()
            ofiler.Close()
            frW.Close()


            'Dim afile As FileIO.TextFieldParser = New FileIO.TextFieldParser(filename)
            'Dim CurrentRecord As String() ' this array will hold each line of data
            'afile.TextFieldType = FileIO.FieldType.Delimited
            'afile.Delimiters = New String() {","}
            'afile.HasFieldsEnclosedInQuotes = True

            '' parse the actual file
            'Do While Not afile.EndOfData
            '    Try

            '        CurrentRecord = afile.ReadFields
            '    Catch ex As FileIO.MalformedLineException
            '        Stop
            '    End Try
            'Loop

            Dim strTest As String = My.Computer.FileSystem.ReadAllText(filename)
            'strTest = strTest.Replace("""", "")
            Return strTest
        Catch ex As Exception
            log.Error(ex.Message)
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Class ErrorLineDetails - used to add CSV lines that fail import so a report can be created
    ''' </summary>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Private Class ErrorLineDetails
        Public ReadOnly lineNo As Integer
        Public ReadOnly line As IList(Of String)
        Public ReadOnly ex As Exception
        Public Sub New(ByVal lineNo As Integer, ByVal ex As Exception)
            Me.lineNo = lineNo
            Me.line = line
            Me.ex = ex
        End Sub
    End Class

    ''' <summary>
    ''' Function checkLength - tests the length of the passed in value
    ''' </summary>
    ''' <param name="fieldName"></param>
    ''' <param name="a"></param>
    ''' <param name="l"></param>
    ''' <returns>An exception if the test fails</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Private Function checkLength(ByVal fieldName As String, ByVal a As String, ByVal l As Integer) As Boolean
        'skip check if array is too short: return true by default
        If a.Length > l Then
            Throw New Exception("field too long: " & fieldName)
        End If
    End Function

    ''' <summary>
    ''' Function item- return item at index trimmed, if array too short then return empty string
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="index"></param>
    ''' <returns>A trimmed value  if OK, otherwise an empty string</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Private Shared Function item(ByVal a As IList(Of String), ByVal index As Integer) As String
        If a.Count - 1 < index Then
            Return ""
        Else
            Return CStr(a.Item(index)).Trim
        End If
    End Function

    ''' <summary>
    ''' Enum FieldIndexes -  determines the column of each required field
    ''' </summary>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Private Enum FieldIndexes
        TransactionReference
        TransactionDate
        TransactionAccountAmount
        TransactionAccountCurrency
        TransactionMerchantAmount
        TransactionMerchantCurrency
        TransactionMerchantNarrative
        TransactionMCC
        TransactionCardLastFourDigits
        ClientID
        ClientName
        DeploymentID
        DeploymentPlatform
        DeploymentChargeStart
        DeploymentChargeEnd
        DeploymentAmount
        DeploymentCurrency
        DeploymentName
        DeploymentCreatedDate
        DeploymentIdentifiers
        DeploymentConsumerReference
        DeploymentCustomerID
        DeploymentCustomerName
        SupplierName
        SupplierAddress1
        SupplierAddress2
        SupplierCity
        SupplierCountryCode
        SupplierPostalOrZipCode
        SupplierTelephoneNumber
        FacsimilieNumber
        SupplierConsumerID
        HotelCheckIn
        HotelCheckOut
        HotelRoomType
        HotelRateInformation
        HotelCancellationPolicy
        HotelCancellationReference
        HotelPaymentRestrictions
        HotelCareOfName
        HotelContactName
        HotelComments
        HotelBookingPlatformName
        Travellers
    End Enum

    Public Function importFile(ByVal pstrFile As String, ByVal pOurFileDate As String) As Boolean
        Try

            Dim parser As New CSVParser.CSVParser(readAllText(pstrFile, pOurFileDate))
            Dim line As IList(Of String) = parser.readLine()
            Dim dinosaurs As New List(Of TransactionData)

            Do While Not line Is Nothing
                If line.Count = 1 Then
                    Dim Values() As String = Split(item(line, 0), ",")
                    If item(Values, 0).ToLower <> "TransactionReference".ToLower Then

                        'First let's add everything in the csv file to a new list so we can sort on bookingID
                        ' this will allow us to group the records so it's easier to load into the DB

                        Dim v As New TransactionData(0, CType(item(Values, FieldIndexes.TransactionReference), Integer?), _
                                                item(Values, FieldIndexes.TransactionDate).Replace("""", "").Replace("'", "").Trim, _
                                                CDec(item(Values, FieldIndexes.TransactionAccountAmount)), _
                                                item(Values, FieldIndexes.TransactionAccountCurrency).Replace("""", "").Replace("'", ""), _
                                                CDec(item(Values, FieldIndexes.TransactionMerchantAmount)), _
                                                item(Values, FieldIndexes.TransactionMerchantCurrency).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.TransactionMerchantNarrative).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.TransactionMCC).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.TransactionCardLastFourDigits).Replace("""", "").Replace("'", ""), _
                                                CType(item(Values, FieldIndexes.ClientID), Integer?), _
                                                item(Values, FieldIndexes.ClientName).Replace("""", "").Replace("'", ""), _
                                                CType(item(Values, FieldIndexes.DeploymentID), Integer?), _
                                                item(Values, FieldIndexes.DeploymentPlatform).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.DeploymentChargeStart).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.DeploymentChargeEnd).Replace("""", "").Replace("'", ""), _
                                                CDec(item(Values, FieldIndexes.DeploymentAmount)), _
                                                item(Values, FieldIndexes.DeploymentCurrency).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.DeploymentName).Replace("""", "").Replace("'", ""), _
                                                CType(item(Values, FieldIndexes.DeploymentCreatedDate), Date?), _
                                                item(Values, FieldIndexes.DeploymentIdentifiers).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.DeploymentConsumerReference).Replace("""", "").Replace("'", ""), _
                                                CType(item(Values, FieldIndexes.DeploymentCustomerID), Integer?), _
                                                item(Values, FieldIndexes.DeploymentCustomerName).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.SupplierName).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.SupplierAddress1).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.SupplierAddress2).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.SupplierCity).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.SupplierCountryCode).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.SupplierPostalOrZipCode).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.SupplierTelephoneNumber).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.FacsimilieNumber).Replace("""", "").Replace("'", ""), _
                                                CType(item(Values, FieldIndexes.SupplierConsumerID), Integer?), _
                                                item(Values, FieldIndexes.HotelCheckIn).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelCheckOut).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelRoomType).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelRateInformation).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelCancellationPolicy).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelCancellationReference).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelPaymentRestrictions).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelCareOfName).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelContactName).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelComments).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.HotelBookingPlatformName).Replace("""", "").Replace("'", ""), _
                                                item(Values, FieldIndexes.Travellers).Replace("""", "").Replace("'", ""))
                        dinosaurs.Add(v)
                    End If
                Else
                    If line.Count > 1 Then
                        If item(line, 0).ToLower <> "TransactionReference".ToLower Then

                            'First let's add everything in the csv file to a new list so we can sort on bookingID
                            ' this will allow us to group the records so it's easier to load into the DB
                            Dim v As New TransactionData(0, CType(item(line, FieldIndexes.TransactionReference), Integer?), _
                                                item(line, FieldIndexes.TransactionDate).Replace("""", "").Replace("'", ""), _
                                                CDec(item(line, FieldIndexes.TransactionAccountAmount)), _
                                                item(line, FieldIndexes.TransactionAccountCurrency).Replace("""", "").Replace("'", ""), _
                                                CDec(item(line, FieldIndexes.TransactionMerchantAmount)), _
                                                item(line, FieldIndexes.TransactionMerchantCurrency).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.TransactionMerchantNarrative).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.TransactionMCC).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.TransactionCardLastFourDigits).Replace("""", "").Replace("'", ""), _
                                                CType(item(line, FieldIndexes.ClientID), Integer?), _
                                                item(line, FieldIndexes.ClientName).Replace("""", "").Replace("'", ""), _
                                                CType(item(line, FieldIndexes.DeploymentID), Integer?), _
                                                item(line, FieldIndexes.DeploymentPlatform).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.DeploymentChargeStart).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.DeploymentChargeEnd).Replace("""", "").Replace("'", ""), _
                                                CDec(item(line, FieldIndexes.DeploymentAmount)), _
                                                item(line, FieldIndexes.DeploymentCurrency).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.DeploymentName).Replace("""", "").Replace("'", ""), _
                                                CType(item(line, FieldIndexes.DeploymentCreatedDate), Date?), _
                                                item(line, FieldIndexes.DeploymentIdentifiers).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.DeploymentConsumerReference).Replace("""", "").Replace("'", ""), _
                                                CType(item(line, FieldIndexes.DeploymentCustomerID), Integer?), _
                                                item(line, FieldIndexes.DeploymentCustomerName).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.SupplierName).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.SupplierAddress1).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.SupplierAddress2).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.SupplierCity).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.SupplierCountryCode).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.SupplierPostalOrZipCode).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.SupplierTelephoneNumber).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.FacsimilieNumber).Replace("""", "").Replace("'", ""), _
                                                CType(item(line, FieldIndexes.SupplierConsumerID), Integer?), _
                                                item(line, FieldIndexes.HotelCheckIn).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelCheckOut).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelRoomType).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelRateInformation).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelCancellationPolicy).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelCancellationReference).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelPaymentRestrictions).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelCareOfName).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelContactName).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelComments).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.HotelBookingPlatformName).Replace("""", "").Replace("'", ""), _
                                                item(line, FieldIndexes.Travellers).Replace("""", "").Replace("'", ""))
                            dinosaurs.Add(v)
                        End If
                    End If
                End If
                line = parser.readLine
            Loop

            Dim Sorter As New Utility.Sorter(Of TransactionData)

            Sorter.SortString = "TransactionReference"
            dinosaurs.Sort(Sorter)

            If saveData(dinosaurs, pOurFileDate) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            log.Error(ex.Message)
            Return False
        End Try
    End Function

    Private Structure getGroupDetailsRet
        Public pintgroupid, pintcompanyid As Integer
    End Structure

    Private Function getGroupDetails(ByVal pstrName As String) As getGroupDetailsRet
        'Dim gps As List(Of clsGroup)
        Dim ret As New getGroupDetailsRet

        'gps = clsGroup.getGroupIDsFromName(pstrName)

        'Dim intcompanyid As Integer = 0
        Dim intgroupid As Integer = 0

        'For Each gp As clsGroup In gps
        '    intcompanyid = gp.companyid
        '    intgroupid = gp.groupid
        'Next

        intgroupid = clsStuff.notWholeNumber(clsGroup.getGroupIDFromName(pstrName))

        ret.pintcompanyid = 0
        ret.pintgroupid = intgroupid
        Return ret
    End Function

    ''' <summary>
    ''' Function saveData - checks data for validity, if OK saves to database and copies CSV file to Archive, 
    ''' if not each related record is removed from the database and written to a report for chacking
    ''' </summary>
    ''' <param name="dinosaurs"></param>
    ''' <returns>True/false dependant upon failures</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Private Function saveData(ByVal dinosaurs As List(Of TransactionData), ByVal pOurFileDate As String) As Boolean
        Dim strErrorBookingIDs As String = ""
        Dim strCurrentBookingID As String = ""
        Dim strLastBookingID As String = ""
        Dim strCurrentGuestPNR As String = ""
        Dim strLastGuestPNR As String = ""
        Dim strerror As String = ""
        Dim failures As New List(Of ErrorLineDetails)
        Dim intcounts As Integer = 0
        Dim fr As New StringWriter(CultureInfo.CurrentCulture)
        Dim blncancel As Boolean = False

        'R17 CR
        Dim blnDuplicatesAdded As Boolean = False

        fr.WriteLine("TransactionReference,TransactionDate,TransactionAccountAmount,TransactionAccountCurrency,TransactionMerchantAmount,TransactionMerchantCurrency,TransactionMerchantNarrative," & _
                     "TransactionMCC,TransactionCardLastFourDigits,ClientID,ClientName,DeploymentID,DeploymentPlatform,DeploymentChargeStart,DeploymentChargeEnd,DeploymentAmount,DeploymentCurrency," & _
                     "DeploymentName,DeploymentCreatedDate,DeploymentIdentifiers,DeploymentConsumerReference,DeploymentCustomerID,DeploymentCustomerName,SupplierName,SupplierAddress1,SupplierAddress2," & _
                     "SupplierCity,SupplierCountryCode,SupplierPostalOrZipCode,SupplierTelephoneNumber,FacsimilieNumber,SupplierConsumerID,HotelCheckIn,HotelCheckOut,HotelRoomType,HotelRateInformation," & _
                     "HotelCancellationPolicy,HotelCancellationReference,HotelPaymentRestrictions,HotelCareOfName,HotelContactName,HotelComments,HotelBookingPlatformName,Travellers,Error Message")

        Dim blnExtrasAdded As Boolean = False

        For Each dino As TransactionData In dinosaurs
            If dino.TransactionMerchantNarrative.ToLower = "TransactionMerchantNarrative".ToLower Then
                'TODO make sure It was a header
            Else
                Try
                    Try
                        Dim inttest As Integer = CInt(dino.TransactionReference)
                    Catch ex As Exception
                        Throw New Exception("TransactionReference is not a number.")
                    End Try
                    checkLength("TransactionDate", dino.TransactionDate, 20)
                    Try
                        Dim inttest As Decimal = CDec(dino.TransactionAccountAmount)
                    Catch ex As Exception
                        Throw New Exception("TransactionAccountAmount is not a number.")
                    End Try

                    checkLength("TransactionAccountCurrency", dino.TransactionAccountCurrency, 20)
                    Try
                        Dim inttest As Decimal = CDec(dino.TransactionMerchantAmount)
                    Catch ex As Exception
                        Throw New Exception("TransactionMerchantAmount is not a number.")
                    End Try
                    checkLength("TransactionMerchantCurrency", dino.TransactionMerchantCurrency, 20)
                    checkLength("TransactionMerchantNarrative", dino.TransactionMerchantNarrative, 200)
                    checkLength("TransactionMcc", dino.TransactionMCC, 20)
                    checkLength("TransactionCardLastFourDigits", dino.TransactionCardLastFourDigits, 10)
                    Try
                        Dim inttest As Integer = CInt(dino.ClientID)
                    Catch ex As Exception
                        Throw New Exception("ClientID is not a number.")
                    End Try
                    checkLength("ClientName", dino.ClientName, 300)
                    Try
                        Dim inttest As Integer = CInt(dino.DeploymentID)
                    Catch ex As Exception
                        Throw New Exception("DeploymentID is not a number.")
                    End Try
                    checkLength("DeploymentPlatform", dino.DeploymentPlatform, 200)
                    checkLength("DeploymentChargeStart", dino.DeploymentChargeStart, 20)
                    checkLength("DeploymentChargeEnd", dino.DeploymentChargeEnd, 20)
                    Try
                        Dim inttest As Decimal = CDec(dino.DeploymentAmount)
                    Catch ex As Exception
                        Throw New Exception("DeploymentAmount is not a number.")
                    End Try
                    checkLength("DeploymentCurrency", dino.DeploymentCurrency, 20)
                    Try
                        Dim inttest As Date = CDate(dino.DeploymentCreatedDate)
                    Catch ex As Exception
                        Throw New Exception("DeploymentCreatedDate is not a date.")
                    End Try
                    Try
                        If dino.DeploymentConsumerReference.Contains("-") Then
                            dino.DeploymentConsumerReference = Mid(dino.DeploymentConsumerReference, 1, dino.DeploymentConsumerReference.IndexOf("-"))
                        End If
                        Dim inttest As Integer = CInt(dino.DeploymentConsumerReference)
                    Catch ex As Exception
                        Throw New Exception("DeploymentConsumerReference is not a number.")
                    End Try
                    Try
                        Dim inttest As Integer = CInt(dino.DeploymentCustomerID)
                    Catch ex As Exception
                        Throw New Exception("DeploymentCustomerID is not a number.")
                    End Try
                    checkLength("DeploymentCustomerName", dino.DeploymentCustomerName, 200)
                    checkLength("SupplierName", dino.SupplierName, 400)
                    checkLength("SupplierAddress1", dino.SupplierAddress1, 200)
                    checkLength("SupplierAddress2", dino.SupplierAddress2, 200)
                    checkLength("SupplierCity", dino.SupplierCity, 200)
                    checkLength("SupplierCountryCode", dino.SupplierCountryCode, 10)
                    checkLength("SupplierPostalOrZipCode", dino.SupplierPostalOrZipCode, 20)
                    checkLength("SupplierTelephoneNumber", dino.SupplierTelephoneNumber, 50)
                    checkLength("FacsimilieNumber", dino.FacsimilieNumber, 50)
                    Try
                        Dim inttest As Integer = CInt(dino.SupplierConsumerID)
                    Catch ex As Exception
                        Throw New Exception("SupplierConsumerID is not a number.")
                    End Try
                    checkLength("HotelCheckIn", dino.HotelCheckIn, 20)
                    checkLength("HotelCheckOut", dino.HotelCheckOut, 20)
                    checkLength("HotelRoomType", dino.HotelRoomType, 200)
                    checkLength("HotelCancellationReference", dino.HotelCancellationReference, 200)
                    checkLength("HotelPaymentRestrictions", dino.HotelPaymentRestrictions, 200)
                    checkLength("HotelCareOfName", dino.HotelCareOfName, 200)
                    checkLength("HotelContactName", dino.HotelContactName, 200)
                    checkLength("HotelBookingPlatformName", dino.HotelBookingPlatformName, 200)
                    checkLength("Travellers", dino.Travellers, 200)


                    If dino.save = 0 Then
                        intcounts += 1
                        strerror = "Failure in feedimportdata_save stored procedure"
                    Else
                        strerror = "OK"
                    End If
                    'always write back to csv so can archive if all OK, or show errors if not
                    For Each s As String In New String() {CStr(dino.TransactionReference), _
                                                        dino.TransactionDate, _
                                                        CStr(dino.TransactionAccountAmount), _
                                                        dino.TransactionAccountCurrency, _
                                                        CStr(dino.TransactionMerchantAmount), _
                                                        dino.TransactionMerchantCurrency, _
                                                        dino.TransactionMerchantNarrative, _
                                                        dino.TransactionMCC, _
                                                        dino.TransactionCardLastFourDigits, _
                                                        CStr(dino.ClientID), _
                                                        dino.ClientName, _
                                                        CStr(dino.DeploymentID), _
                                                        dino.DeploymentPlatform, _
                                                        dino.DeploymentChargeStart, _
                                                        dino.DeploymentChargeEnd, _
                                                        CStr(dino.DeploymentAmount), _
                                                        dino.DeploymentCurrency, _
                                                        dino.DeploymentName, _
                                                        CStr(dino.DeploymentCreatedDate), _
                                                        dino.DeploymentIdentifiers, _
                                                        dino.DeploymentConsumerReference, _
                                                        CStr(dino.DeploymentCustomerID), _
                                                        dino.DeploymentCustomerName, _
                                                        dino.SupplierName, _
                                                        dino.SupplierAddress1, _
                                                        dino.SupplierAddress2, _
                                                        dino.SupplierCity, _
                                                        dino.SupplierCountryCode, _
                                                        dino.SupplierPostalOrZipCode, _
                                                        dino.SupplierTelephoneNumber, _
                                                        dino.FacsimilieNumber, _
                                                        CStr(dino.SupplierConsumerID), _
                                                        dino.HotelCheckIn, _
                                                        dino.HotelCheckOut, _
                                                        dino.HotelRoomType, _
                                                        dino.HotelRateInformation, _
                                                        dino.HotelCancellationPolicy, _
                                                        dino.HotelCancellationReference, _
                                                        dino.HotelPaymentRestrictions, _
                                                        dino.HotelCareOfName, _
                                                        dino.HotelContactName, _
                                                        dino.HotelComments, _
                                                        dino.HotelBookingPlatformName, _
                                                        dino.Travellers, _
                                                        strerror}

                        fr.Write(ToCsvCell(clsStuff.notString(s)))
                        fr.Write(",")
                    Next
                    fr.WriteLine("")

                Catch ex As Exception
                    'add to failures list so can check when run complete if there are any failures
                    For Each s As String In New String() {CStr(dino.TransactionReference), _
                                                        dino.TransactionDate, _
                                                        CStr(dino.TransactionAccountAmount), _
                                                        dino.TransactionAccountCurrency, _
                                                        CStr(dino.TransactionMerchantAmount), _
                                                        dino.TransactionMerchantCurrency, _
                                                        dino.TransactionMerchantNarrative, _
                                                        dino.TransactionMCC, _
                                                        dino.TransactionCardLastFourDigits, _
                                                        CStr(dino.ClientID), _
                                                        dino.ClientName, _
                                                        CStr(dino.DeploymentID), _
                                                        dino.DeploymentPlatform, _
                                                        dino.DeploymentChargeStart, _
                                                        dino.DeploymentChargeEnd, _
                                                        CStr(dino.DeploymentAmount), _
                                                        dino.DeploymentCurrency, _
                                                        dino.DeploymentName, _
                                                        CStr(dino.DeploymentCreatedDate), _
                                                        dino.DeploymentIdentifiers, _
                                                        dino.DeploymentConsumerReference, _
                                                        CStr(dino.DeploymentCustomerID), _
                                                        dino.DeploymentCustomerName, _
                                                        dino.SupplierName, _
                                                        dino.SupplierAddress1, _
                                                        dino.SupplierAddress2, _
                                                        dino.SupplierCity, _
                                                        dino.SupplierCountryCode, _
                                                        dino.SupplierPostalOrZipCode, _
                                                        dino.SupplierTelephoneNumber, _
                                                        dino.FacsimilieNumber, _
                                                        CStr(dino.SupplierConsumerID), _
                                                        dino.HotelCheckIn, _
                                                        dino.HotelCheckOut, _
                                                        dino.HotelRoomType, _
                                                        dino.HotelRateInformation, _
                                                        dino.HotelCancellationPolicy, _
                                                        dino.HotelCancellationReference, _
                                                        dino.HotelPaymentRestrictions, _
                                                        dino.HotelCareOfName, _
                                                        dino.HotelContactName, _
                                                        dino.HotelComments, _
                                                        dino.HotelBookingPlatformName, _
                                                        dino.Travellers, ex.Message}
                        'dino.Currency, _

                        fr.Write(ToCsvCell(clsStuff.notString(s)))
                        fr.Write(",")
                    Next
                    fr.WriteLine("")
                    intcounts = intcounts + 1
                End Try


            End If
        Next
        'archive OK records to disk

        If intcounts > 0 Then
            'write failed records to disk
            Dim ofiler As New System.IO.StreamWriter(getConfig("downloadedfilesTransERROR") & _
                                                     "Error_" & pOurFileDate & _
                                                     ".csv", False, Encoding.Default)
            ofiler.Write(fr.ToString)
            ofiler.Flush()
            ofiler.Close()
            fr.Close()
        Else
            Dim ofiler As New System.IO.StreamWriter(getConfig("downloadedfilesTransOK") & _
                                                     pOurFileDate & _
                                                     ".csv", False, Encoding.Default)
            ofiler.Write(fr.ToString)
            ofiler.Flush()
            ofiler.Close()
            fr.Close()
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function ToCsvCell - checks for invalid data and removes if found
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns>Cleaned up string value</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Public Shared Function ToCsvCell(ByVal s As String) As String
        If s.IndexOf(",", StringComparison.Ordinal) <> -1 Or _
            s.IndexOf("""", StringComparison.Ordinal) <> -1 Or _
            s.IndexOf(vbCr, StringComparison.Ordinal) <> -1 Or _
            s.IndexOf(vbLf, StringComparison.Ordinal) <> -1 Then
            Return """" & s.Replace("""", """""") & """"
        Else
            Return s
        End If
    End Function

End Class 'CSVReader
