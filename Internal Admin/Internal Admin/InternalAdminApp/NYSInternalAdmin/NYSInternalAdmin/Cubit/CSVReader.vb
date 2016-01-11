Option Explicit On
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Globalization
Imports DatabaseObjects
Imports EvoUtilities.ConfigUtils
Imports Utility
Imports CSVParser


''' <summary>
''' Class dataImport - used to store imported data from the CSV file so it can be sorted correctly
''' </summary>
''' <remarks>
''' Created 13/03/2009 Nick Massarella
''' </remarks>
Partial Public Class dataImport
    Public Sub New( _
        ByVal pTransactionnumber As String, _
        ByVal pTransactionlinenumber As String, _
        ByVal pTransactiondate As String, _
        ByVal pDeparturedate As String, _
        ByVal pArrivaldate As String, _
        ByVal pPassengername As String, _
        ByVal pTotalamount As String, _
        ByVal pVatamount As String, _
        ByVal pVatrate As String, _
        ByVal pNettamount As String, _
        ByVal pVenuename As String, _
        ByVal pVenuedetails As String, _
        ByVal pRoomdetails As String, _
        ByVal pCompany As String, _
        ByVal pConfermainvoicenumber As String, _
        ByVal pCategory As String, _
        ByVal pSupplierinvoice As String, _
        ByVal pGuestPNR As String, _
        ByVal pRef1 As String, _
        ByVal pBooker As String, _
        ByVal pBookedDate As String, _
        ByVal pOutofPolicyReason As String, _
        ByVal pLast4Digits As String, _
        ByVal pTravellerEmail As String, _
        ByVal pBookerEmail As String, _
        ByVal pCostcode As String, _
        ByVal pRef2 As String, _
        ByVal pRef3 As String, _
        ByVal pBookerinitials As String, _
        ByVal pCategoryID As String, _
        ByVal pAICol6 As String, _
        ByVal pAICol7 As String, _
        ByVal pAICol8 As String, _
        ByVal pAICol9 As String, _
        ByVal pAICol10 As String, _
        ByVal pCurrency As String, _
        ByVal pInvoiceLineDetailID As String) 'R2.9.1 SA - added InvoiceLineDetailID
        mTransactionnumber = pTransactionnumber
        mTransactionlinenumber = pTransactionlinenumber
        mTransactiondate = pTransactiondate
        mArrivaldate = pArrivaldate
        mDeparturedate = pDeparturedate
        mPassengername = pPassengername
        mTotalamount = pTotalamount
        mVatamount = pVatamount
        mVatrate = pVatrate
        mNettamount = pNettamount
        mVenuename = pVenuename
        mVenuedetails = pVenuedetails
        mRoomdetails = pRoomdetails
        mCompany = pCompany
        mConfermainvoicenumber = pConfermainvoicenumber
        mCategory = pCategory
        mSupplierinvoice = pSupplierinvoice
        mGuestPNR = pGuestPNR
        mRef1 = pRef1
        mBooker = pBooker
        mBookedDate = pBookedDate
        mOutofPolicyReason = pOutofPolicyReason
        mLast4Digits = pLast4Digits
        mTravellerEmail = pTravellerEmail
        'R2.20C CR
        mBookerEmail = pBookerEmail
        mCostcode = pCostcode
        mRef2 = pRef2
        mRef3 = pRef3
        mBookerinitials = pBookerinitials
        mCategoryID = pCategoryID
        mAICol6 = pAICol6
        mAICol7 = pAICol7
        mAICol8 = pAICol8
        mAICol9 = pAICol9
        mAICol10 = pAICol10
        mCurrency = pCurrency
        'R2.9.1 SA 
        mInvoiceLineDetailID = pInvoiceLineDetailID
    End Sub

    'R2.20CR
    Private mBookerEmail As String

    Private mTravellerEmail As String
    Private mOutofPolicyReason As String
    Private mLast4Digits As String
    Private mAICol6 As String
    Private mAICol7 As String
    Private mAICol8 As String
    Private mAICol9 As String
    Private mAICol10 As String
    Private mTransactionnumber As String
    Private mTransactionlinenumber As String
    Private mTransactiondate As String
    Private mDeparturedate As String
    Private mArrivaldate As String
    Private mPassengername As String
    Private mTotalamount As String
    Private mVatamount As String
    Private mVatrate As String
    Private mNettamount As String
    Private mVenuename As String
    Private mVenuedetails As String
    Private mRoomdetails As String
    Private mCompany As String
    Private mConfermainvoicenumber As String
    Private mCategory As String
    Private mSupplierinvoice As String
    Private mGuestPNR As String
    Private mRef1 As String
    Private mBooker As String
    Private mBookedDate As String
    Private mCostcode As String
    Private mRef2 As String
    Private mRef3 As String
    Private mBookerinitials As String
    Private mCategoryID As String
    Private mCurrency As String
    'R2.9.1 SA 
    Private mInvoiceLineDetailID As String

    'R2.20C CR
    Public Property BookerEmail() As String
        Get
            Return mBookerEmail
        End Get
        Set(value As String)
            mBookerEmail = value
        End Set
    End Property

    Public Property TravellerEmail() As String
        Get
            Return mTravellerEmail
        End Get
        Set(ByVal value As String)
            mTravellerEmail = value
        End Set
    End Property

    Public Property OutofPolicyReason() As String
        Get
            Return mOutofPolicyReason
        End Get
        Set(ByVal value As String)
            mOutofPolicyReason = value
        End Set
    End Property

    Public Property Last4Digits() As String
        Get
            Return mLast4Digits
        End Get
        Set(ByVal value As String)
            mLast4Digits = value
        End Set
    End Property

    Public Property Currency() As String
        Get
            Return mCurrency
        End Get
        Set(ByVal value As String)
            mCurrency = value
        End Set
    End Property

    Public Property AICol6() As String
        Get
            Return mAICol6
        End Get
        Set(ByVal value As String)
            mAICol6 = value
        End Set
    End Property

    Public Property AICol7() As String
        Get
            Return mAICol7
        End Get
        Set(ByVal value As String)
            mAICol7 = value
        End Set
    End Property

    Public Property AICol8() As String
        Get
            Return mAICol8
        End Get
        Set(ByVal value As String)
            mAICol8 = value
        End Set
    End Property

    Public Property AICol9() As String
        Get
            Return mAICol9
        End Get
        Set(ByVal value As String)
            mAICol9 = value
        End Set
    End Property

    Public Property AICol10() As String
        Get
            Return mAICol10
        End Get
        Set(ByVal value As String)
            mAICol10 = value
        End Set
    End Property

    Public Property Transactionnumber() As String
        Get
            Return mTransactionnumber
        End Get
        Set(ByVal value As String)
            mTransactionnumber = value
        End Set
    End Property

    Public Property Transactionlinenumber() As String
        Get
            Return mTransactionlinenumber
        End Get
        Set(ByVal value As String)
            mTransactionlinenumber = value
        End Set
    End Property

    Public Property Transactiondate() As String
        Get
            Return mTransactiondate
        End Get
        Set(ByVal value As String)
            mTransactiondate = value
        End Set
    End Property

    Public Property Arrivaldate() As String
        Get
            Return mArrivaldate
        End Get
        Set(ByVal value As String)
            mArrivaldate = value
        End Set
    End Property

    Public Property Departuredate() As String
        Get
            Return mDeparturedate
        End Get
        Set(ByVal value As String)
            mDeparturedate = value
        End Set
    End Property

    Public Property Passengername() As String
        Get
            Return mPassengername
        End Get
        Set(ByVal value As String)
            mPassengername = value
        End Set
    End Property

    Public Property Ref1() As String
        Get
            Return mRef1
        End Get
        Set(ByVal value As String)
            mRef1 = value
        End Set
    End Property

    Public Property Ref2() As String
        Get
            Return mRef2
        End Get
        Set(ByVal value As String)
            mRef2 = value
        End Set
    End Property

    Public Property Ref3() As String
        Get
            Return mRef3
        End Get
        Set(ByVal value As String)
            mRef3 = value
        End Set
    End Property

    Public Property Booker() As String
        Get
            Return mBooker
        End Get
        Set(ByVal value As String)
            mBooker = value
        End Set
    End Property

    Public Property BookedDate() As String
        Get
            Return mBookedDate
        End Get
        Set(ByVal value As String)
            mBookedDate = value
        End Set
    End Property

    Public Property Bookerinitials() As String
        Get
            Return mBookerinitials
        End Get
        Set(ByVal value As String)
            mBookerinitials = value
        End Set
    End Property

    Public Property Nettamount() As String
        Get
            Return mNettamount
        End Get
        Set(ByVal value As String)
            mNettamount = value
        End Set
    End Property

    Public Property Vatamount() As String
        Get
            Return mVatamount
        End Get
        Set(ByVal value As String)
            mVatamount = value
        End Set
    End Property

    Public Property Vatrate() As String
        Get
            Return mVatrate
        End Get
        Set(ByVal value As String)
            mVatrate = value
        End Set
    End Property

    Public Property Totalamount() As String
        Get
            Return mTotalamount
        End Get
        Set(ByVal value As String)
            mTotalamount = value
        End Set
    End Property

    Public Property Venuename() As String
        Get
            Return mVenuename
        End Get
        Set(ByVal value As String)
            mVenuename = value
        End Set
    End Property

    Public Property Venuedetails() As String
        Get
            Return mVenuedetails
        End Get
        Set(ByVal value As String)
            mVenuedetails = value
        End Set
    End Property

    Public Property Roomdetails() As String
        Get
            Return mRoomdetails
        End Get
        Set(ByVal value As String)
            mRoomdetails = value
        End Set
    End Property

    Public Property Company() As String
        Get
            Return mCompany
        End Get
        Set(ByVal value As String)
            mCompany = value
        End Set
    End Property

    Public Property Confermainvoicenumber() As String
        Get
            Return mConfermainvoicenumber
        End Get
        Set(ByVal value As String)
            mConfermainvoicenumber = value
        End Set
    End Property

    Public Property Category() As String
        Get
            Return mCategory
        End Get
        Set(ByVal value As String)
            mCategory = value
        End Set
    End Property

    Public Property Supplierinvoice() As String
        Get
            Return mSupplierinvoice
        End Get
        Set(ByVal value As String)
            mSupplierinvoice = value
        End Set
    End Property

    Public Property GuestPNR() As String
        Get
            Return mGuestPNR
        End Get
        Set(ByVal value As String)
            mGuestPNR = value
        End Set
    End Property

    Public Property Costcode() As String
        Get
            Return mCostcode
        End Get
        Set(ByVal value As String)
            mCostcode = value
        End Set
    End Property

    Public Property CategoryID() As String
        Get
            Return mCategoryID
        End Get
        Set(ByVal value As String)
            mCategoryID = value
        End Set
    End Property

    'R2.9.1 SA 
    Public Property InvoiceLineDetailID() As String
        Get
            Return mInvoiceLineDetailID
        End Get
        Set(ByVal value As String)
            mInvoiceLineDetailID = value
        End Set
    End Property

End Class

''' <summary>
''' Class CSVReader - checks for today's CSV file, if it hasn't already been downloaded, 
''' then it is downloaded. Then the data is checked for validity and if OK each line is 
''' imported into the database as a unique record. If a failure occurs a CSV report of
''' the failure is created
''' </summary>
''' <remarks>
''' Created 13/03/2009 Nick Massarella
''' </remarks>
Public Class CSVReader

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
    Public Function Main(ByVal strDate As String) As Boolean
        'check if file exists
        Try
            Dim blnOk As Boolean = True
            'If pbTest Then
            '    For Each strfile As String In IO.Directory.GetFiles(getConfig("downloadedfiles"))
            '        Dim strJustFile As String = New System.IO.FileInfo(strfile).Name
            '        Try
            '            If IO.File.Exists(getConfig("downloadedfiles") & strJustFile) Then
            '                If Not importFile(getConfig("downloadedfiles") & strJustFile) Then
            '                    blnOk = False
            '                End If
            '            End If
            '        Catch ex As Exception
            '            Dim str As String = ""
            '        End Try
            '    Next
            'Else

            '  
            If IO.File.Exists(getConfig("downloadedfiles") & strDate & ".csv") Then
                If Not importFile(getConfig("downloadedfiles") & strDate & ".csv") Then
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
    Public Shared Function readAllText(ByVal filename As String) As String
        Try
            Return My.Computer.FileSystem.ReadAllText(filename)
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
        Transactionnumber = 0
        Transactionlinenumber = 1
        Transactiondate = 2
        Departuredate = 3
        Arrivaldate = 4
        LeadPassengername = 5
        Currency = 11
        Totalamount = 12
        Vatamount = 13
        Vatrate = 14
        Nettamount = 15
        VenueName = 19
        Venuedetails = 20
        Roomdetails = 21
        Company = 22
        Confermainvoicenumber = 23
        Category = 24
        Supplierinvoice = 25
        GuestPNR = 27
        ref1 = 28 ' lastmodified
        BookedDate = 29
        OutofPolicyReason = 30
        Last4Digits = 31
        TravellerEmail = 32

        'R2.21 SA - if index change, pls change index in admin page 
        'for boss drop down list 

        'R2.20C - new field added by conferma
        BookerEmail = 33
        UserBooker = 34 'booker name
        Costcode = 35
        ref2 = 36 'now billinstructions
        ref3 = 37 'now comments
        NYSBookerinitials = 38
        AICol6 = 39
        AICol7 = 40
        AICol8 = 41
        AICol9 = 42
        AICol10 = 43

        'R2.9.1 SA 
        InvoiceLineDetailID = 44

    End Enum

    ''' <summary>
    ''' Function importFile - reads the new file, adds each line to a dataimport record. 
    ''' There are two different ways as the file may contain records in a single column or 
    ''' multiple columns. Sends sorted list to the saveData method.
    ''' </summary>
    ''' <param name="pstrFile"></param>
    ''' <returns>True/false depending upon any failures</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Public Function importFile(ByVal pstrFile As String) As Boolean
        Try

            Dim parser As New CSVParser.CSVParser(readAllText(pstrFile))
            Dim line As IList(Of String) = parser.readLine()
            Dim dinosaurs As New List(Of dataImport)

            Do While Not line Is Nothing
                If line.Count = 1 Then
                    Dim Values() As String = Split(item(line, 0), ",")
                    If item(Values, 0).ToLower <> "transactionnumber" Then

                        'First let's add everything in the csv file to a new list so we can sort on bookingID
                        'this will allow us to group the records so it's easier to load into the DB
                        Dim v As New dataImport(item(Values, FieldIndexes.Transactionnumber), _
                                                item(Values, FieldIndexes.Transactionlinenumber), _
                                                item(Values, FieldIndexes.Transactiondate), _
                                                item(Values, FieldIndexes.Departuredate), _
                                                item(Values, FieldIndexes.Arrivaldate), _
                                                item(Values, FieldIndexes.LeadPassengername), _
                                                item(Values, FieldIndexes.Totalamount).Replace("�", ""), _
                                                item(Values, FieldIndexes.Vatamount).Replace("�", ""), _
                                                item(Values, FieldIndexes.Vatrate), _
                                                item(Values, FieldIndexes.Nettamount).Replace("�", ""), _
                                                item(Values, FieldIndexes.VenueName), _
                                                item(Values, FieldIndexes.Venuedetails), _
                                                item(Values, FieldIndexes.Roomdetails), _
                                                item(Values, FieldIndexes.Company), _
                                                item(Values, FieldIndexes.Confermainvoicenumber), _
                                                item(Values, FieldIndexes.Category), _
                                                item(Values, FieldIndexes.Supplierinvoice), _
                                                item(Values, FieldIndexes.GuestPNR), _
                                                item(Values, FieldIndexes.ref1), _
                                                item(Values, FieldIndexes.UserBooker), _
                                                item(Values, FieldIndexes.BookedDate), _
                                                item(Values, FieldIndexes.OutofPolicyReason), _
                                                item(Values, FieldIndexes.Last4Digits), _
                                                item(Values, FieldIndexes.TravellerEmail), _
                                                item(Values, FieldIndexes.BookerEmail), _
                                                item(Values, FieldIndexes.Costcode), _
                                                item(Values, FieldIndexes.ref2), _
                                                item(Values, FieldIndexes.ref3), _
                                                item(Values, FieldIndexes.NYSBookerinitials), _
                                                CStr(clsStuff.notWholeNumber(FeedCategory.getCatergoryIDFromName(item(Values, FieldIndexes.Category)))), _
                                                item(Values, FieldIndexes.AICol6), _
                                                item(Values, FieldIndexes.AICol7), _
                                                item(Values, FieldIndexes.AICol8), _
                                                item(Values, FieldIndexes.AICol9), _
                                                item(Values, FieldIndexes.AICol10), _
                                                item(Values, FieldIndexes.Currency), _
                                                item(Values, FieldIndexes.InvoiceLineDetailID)) 'R2.9.1 SA added InvoiceLineDetailID
                        dinosaurs.Add(v)

                    End If
                Else
                    If line.Count > 1 Then
                        If item(line, 0).ToLower <> "transactionnumber" Then

                            '+-- test
                            Dim t As Integer = FeedCategory.getCatergoryIDFromName(item(line, FieldIndexes.Category))
                            Dim tst As String = CStr(clsStuff.notWholeNumber(FeedCategory.getCatergoryIDFromName(item(line, FieldIndexes.Category))))
                            'First let's add everything in the csv file to a new list so we can sort on bookingID
                            ' this will allow us to group the records so it's easier to load into the DB
                            Dim v As New dataImport(item(line, FieldIndexes.Transactionnumber), _
                                                    item(line, FieldIndexes.Transactionlinenumber), _
                                                    item(line, FieldIndexes.Transactiondate), _
                                                    item(line, FieldIndexes.Departuredate), _
                                                    item(line, FieldIndexes.Arrivaldate), _
                                                    item(line, FieldIndexes.LeadPassengername), _
                                                    item(line, FieldIndexes.Totalamount).Replace("�", ""), _
                                                    item(line, FieldIndexes.Vatamount).Replace("�", ""), _
                                                    item(line, FieldIndexes.Vatrate), _
                                                    item(line, FieldIndexes.Nettamount).Replace("�", ""), _
                                                    item(line, FieldIndexes.VenueName), _
                                                    item(line, FieldIndexes.Venuedetails), _
                                                    item(line, FieldIndexes.Roomdetails), _
                                                    item(line, FieldIndexes.Company), _
                                                    item(line, FieldIndexes.Confermainvoicenumber), _
                                                    item(line, FieldIndexes.Category), _
                                                    item(line, FieldIndexes.Supplierinvoice), _
                                                    item(line, FieldIndexes.GuestPNR), _
                                                    item(line, FieldIndexes.ref1), _
                                                    item(line, FieldIndexes.UserBooker), _
                                                    item(line, FieldIndexes.BookedDate), _
                                                    item(line, FieldIndexes.OutofPolicyReason), _
                                                    item(line, FieldIndexes.Last4Digits), _
                                                    item(line, FieldIndexes.TravellerEmail), _
                                                    item(line, FieldIndexes.BookerEmail), _
                                                    item(line, FieldIndexes.Costcode), _
                                                    item(line, FieldIndexes.ref2), _
                                                    item(line, FieldIndexes.ref3), _
                                                    item(line, FieldIndexes.NYSBookerinitials), _
                                                    CStr(clsStuff.notWholeNumber(FeedCategory.getCatergoryIDFromName(item(line, FieldIndexes.Category)))), _
                                                    item(line, FieldIndexes.AICol6), _
                                                    item(line, FieldIndexes.AICol7), _
                                                    item(line, FieldIndexes.AICol8), _
                                                    item(line, FieldIndexes.AICol9), _
                                                    item(line, FieldIndexes.AICol10), _
                                                    item(line, FieldIndexes.Currency), _
                                                    item(line, FieldIndexes.InvoiceLineDetailID)) 'R2.9.1 SA - added InvoiceLineDetailID
                            dinosaurs.Add(v)
                        End If
                    End If
                End If
                line = parser.readLine
            Loop

            Dim Sorter As New Utility.Sorter(Of dataImport)

            Sorter.SortString = "Transactionnumber,GuestPNR,CategoryID"
            dinosaurs.Sort(Sorter)

            If saveData(dinosaurs) Then
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
    Private Function saveData(ByVal dinosaurs As List(Of dataImport)) As Boolean
        Dim strErrorBookingIDs As String = ""
        Dim strCurrentBookingID As String = ""
        Dim strLastBookingID As String = ""
        Dim strCurrentGuestPNR As String = ""
        Dim strLastGuestPNR As String = ""
        Dim strerror As String = ""
        Dim failures As New List(Of ErrorLineDetails)
        Dim intcounts As Integer = 1
        Dim fr As New StringWriter(CultureInfo.CurrentCulture)
        Dim blncancel As Boolean = False

        'R17 CR
        Dim blnDuplicatesAdded As Boolean = False
        Dim swrDuplicates As New StringWriter(CultureInfo.CurrentCulture)

        fr.WriteLine("Transactionnumber,Transactionlinenumber,Transactiondate,Departuredate," & _
                "Arrivaldate,LeadPassengername,Totalamount,Vatamount,Vatrate,Nettamount," & _
                "VenueName,Venuedetails,Roomdetails,Company,Confermainvoicenumber," & _
                "Category,Supplierinvoice,GuestPNR,ref1,UserBooker,BookedDate,OutofPolicyReason,Last4Digits,TravellerEmail,BookerEmail,Costcode," & _
                "ref2,ref3,NYSBookerinitials,AICol6,AICol7,AICol8,AICol9,AICol10,Currency,Cancellation,Error Message")

        'R17 CR
        'Same headings for duplicates file
        swrDuplicates.WriteLine("Transactionnumber,Transactionlinenumber,Transactiondate,Departuredate," & _
                "Arrivaldate,LeadPassengername,Totalamount,Vatamount,Vatrate,Nettamount," & _
                "VenueName,Venuedetails,Roomdetails,Company,Confermainvoicenumber," & _
                "Category,Supplierinvoice,GuestPNR,ref1,UserBooker,BookedDate,OutofPolicyReason,Last4Digits,TravellerEmail,BookerEmail,Costcode," & _
                "ref2,ref3,NYSBookerinitials,AICol6,AICol7,AICol8,AICol9,AICol10,Currency,Cancellation,Error Message")

        Dim blnExtrasAdded As Boolean = False

        'R2.21 SA
        'Dim strBossCode As String = ""
        'Dim strCompanyName As String = ""
        'Dim intTransactionNo As Integer

        For Each dino As dataImport In dinosaurs
            If dino.Company.ToLower = "company" Then
                'TODO make sure It was a header
            Else
                Try
                    'R17 CR
                    Dim blnIsDuplicate As Boolean = False

                    'R17 CR
                    'Check to see if transaction is a duplicate
                    Dim strpassengername As String = dino.Passengername.Replace("NULL", "")
                    strpassengername = strpassengername.ToLower.Replace("mr ", "")
                    strpassengername = strpassengername.ToLower.Replace("mrs ", "")
                    strpassengername = strpassengername.ToLower.Replace("ms ", "")
                    strpassengername = strpassengername.ToLower.Replace("dr ", "")
                    strpassengername = strpassengername.ToLower.Replace("miss ", "")
                    strpassengername = strpassengername.ToLower.Replace("prof ", "")
                    strpassengername = strpassengername.Trim

                    Dim intIndexOFSpace As Integer = strpassengername.IndexOf(" ")

                    If intIndexOFSpace > 0 Then
                        Dim strfirstname As String = Mid(strpassengername, 1, intIndexOFSpace)
                        Dim strlastname As String = Mid(strpassengername, intIndexOFSpace + 1)

                        strpassengername = strlastname.Trim & "/" & strfirstname.Trim
                    End If


                    ''skip past other coding if current record is a duplicate
                    If Not blnIsDuplicate Then
                        'just check to see if headers have been included
                        'first lets check the group exists
                        strCurrentBookingID = dino.Transactionnumber
                        strCurrentGuestPNR = dino.GuestPNR
                        If dino.Company = "LV" Then
                            dino.Company = "LV="
                        ElseIf dino.Company.ToUpper = "ANCHOR TRUST" Then
                            dino.Company = "BS - ANCHOR TRUST"
                        ElseIf dino.Company.ToUpper = "CIMA" Then
                            'Dim str As String = ""
                        End If

                        'first try get group details from company name
                        Dim retValues As getGroupDetailsRet = getGroupDetails(dino.Company)

                        If retValues.pintgroupid = 0 Then
                            Throw New Exception("Group '" & dino.Company & "' does not exist.")
                        End If
                        'get transaction charge type, PP or BKG
                        Dim oPX As clsClientOption
                        oPX = clsClientOption.get(retValues.pintgroupid)
                        Dim strTType As String = "BKG"
                        If Not oPX Is Nothing Then
                            If Not oPX.TransactionType Is Nothing Then
                                strTType = oPX.TransactionType
                            End If
                        End If

                        'R2.7 NM
                        ' Need to get full venuename as Conferma chops off the name if it's too long
                        Dim NewVenueName As String = Mid(dino.Venuedetails, 1, dino.Venuedetails.IndexOf(";"))
                        dino.Venuename = NewVenueName
                        checkLength("Last4Digits", dino.Last4Digits, 4)
                        checkLength("OutofPolicyReason", dino.OutofPolicyReason, 200)
                        checkLength("Passengername", dino.Passengername, 200)
                        checkLength("Ref1", dino.Ref1, 500)
                        checkLength("Ref2", dino.Ref2, 500)
                        checkLength("Ref3", dino.Ref3, 500)
                        checkLength("Booker", dino.Booker, 100)
                        checkLength("Bookerinitials", dino.Bookerinitials, 20)
                        checkLength("VenueName", dino.Venuename, 200)
                        checkLength("Venuedetails", dino.Venuedetails, 3000)
                        checkLength("Roomdetails", dino.Roomdetails, 200)
                        checkLength("Category", dino.Category, 50)
                        checkLength("Supplierinvoice", dino.Supplierinvoice, 1000)
                        checkLength("GuestPNR", dino.GuestPNR, 200)
                        checkLength("Costcode", dino.Costcode, 1000)
                        checkLength("AICol6", dino.AICol6, 200)
                        checkLength("AICol7", dino.AICol7, 200)
                        checkLength("AICol8", dino.AICol8, 200)
                        checkLength("AICol9", dino.AICol9, 200)
                        checkLength("AICol10", dino.AICol10, 200)
                        checkLength("Currency", dino.Currency, 100)
                        checkLength("TravellerEmail", dino.TravellerEmail, 200)
                        'R2.20C CR
                        checkLength("BookerEmail", dino.bookeremail, 200)

                        'always set as should always be this
                        If dino.Costcode.ToLower.StartsWith("cgz79") Then
                            dino.Costcode = "CGZ79/4404"
                        End If
                        If dino.Company.ToLower = "nhs business services authority" Then
                            dino.Costcode = dino.Costcode & "11001"
                        End If

                        Try
                            Dim inttest As Integer = CInt(dino.Transactionnumber)
                        Catch ex As Exception
                            Throw New Exception("Transactionnumber is not a number.")
                        End Try

                        If NewVenueName = "" Then
                            Throw New Exception("There is no Venue name.")
                        End If

                        Try
                            Dim inttest As Integer = CInt(dino.Confermainvoicenumber)
                        Catch ex As Exception
                            Throw New Exception("Confermainvoicenumber is not a number.")
                        End Try

                        Try
                            If clsStuff.notString(dino.Transactiondate.Replace("NULL", "")) <> "" Then
                                Dim dttest As Date = CDate(dino.Transactiondate)
                            Else
                                dino.Transactiondate = dino.Arrivaldate
                            End If
                        Catch ex As Exception
                            Throw New Exception("Transactiondate is not a date.")
                        End Try
                        Try
                            Dim dttest As Date = CDate(dino.Departuredate)
                        Catch ex As Exception
                            Throw New Exception("Departuredate is not a date.")
                        End Try
                        Try
                            Dim dttest As Date = CDate(dino.Arrivaldate)
                        Catch ex As Exception
                            Throw New Exception("Arrivaldate is not a date.")
                        End Try
                        Try
                            Dim dttest As Date = CDate(dino.BookedDate)
                        Catch ex As Exception
                            Throw New Exception("BookedDate is not a date.")
                        End Try
                        Try
                            Dim dbltest As Double = CDbl(dino.Nettamount)
                        Catch ex As Exception
                            Throw New Exception("Nettamount is not a numeric value.")
                        End Try
                        Try
                            Dim dbltest As Double = CDbl(dino.Vatamount)
                        Catch ex As Exception
                            Throw New Exception("Vatamount is not a numeric value.")
                        End Try
                        Try
                            Dim dbltest As Double = CDbl(dino.Vatrate)
                        Catch ex As Exception
                            Throw New Exception("Vatrate is not a numeric value.")
                        End Try
                        Try
                            Dim dbltest As Double = CDbl(dino.Totalamount)
                        Catch ex As Exception
                            Throw New Exception("Totalamount is not a numeric value.")
                        End Try

                        'Added for VAT change NM
                        If CDate(dino.Arrivaldate) < CDate("04/01/2011") Then
                            If CDbl(dino.Vatrate) = 20 Then
                                dino.Vatrate = CStr(17.5)
                                dino.Nettamount = CStr(CDbl(CDbl(dino.Totalamount) / 1.175).ToString("N2"))
                                dino.Vatamount = CStr(CDbl(CDbl(dino.Totalamount) - CDbl(dino.Nettamount)).ToString("N2"))
                            End If
                        ElseIf CDate(dino.Arrivaldate) > CDate("03/01/2011") Then
                            If CDbl(dino.Vatrate) = 17.5 Then
                                dino.Vatrate = CStr(20)
                                dino.Nettamount = CStr(CDbl(CDbl(dino.Totalamount) / 1.2).ToString("N2"))
                                dino.Vatamount = CStr(CDbl(CDbl(dino.Totalamount) - CDbl(dino.Nettamount)).ToString("N2"))
                            End If
                        End If

                        Dim venues As List(Of clsVenue)
                        Dim venuereference As Integer = 0
                        Dim venueDD As Double = 0
                        Dim venueEX As Double = 0
                        Dim venueBOSScode As String = ""
                        Dim Importvenuename As String = ""
                        Dim venuename As String = ""
                        Dim venuetransient As String = ""
                        Dim venuetransientgroup As String = ""
                        Dim venuetransientdefault As String = ""

                        Importvenuename = dino.Venuename

                        'first check to see if hotel name used by Conferma has a different name in VenuesDB
                        Dim strAlternativeName As String = clsStuff.notString(clsVenue.venueAlternativenameCheck _
                                                                     (Importvenuename))
                        If strAlternativeName <> "" Then
                            venuename = strAlternativeName
                        Else
                            venuename = Importvenuename
                        End If

                        'now go and see if the name exists in VenuesDB if it does bring back values
                        venues = clsVenue.venueExactNameFind(venuename.Replace("'", "''''"), CStr(retValues.pintgroupid))
                        If venues.Count > 0 Then
                            For Each venue As clsVenue In venues
                                venuereference = venue.vereference
                                venueDD = venue.vedd
                                venueEX = venue.veex
                                venueBOSScode = venue.bosscode
                                venuename = venue.vename
                                venuetransient = venue.transient
                                venuetransientgroup = venue.transientgroup
                                venuetransientdefault = venue.transientdefault
                            Next
                        End If

                        'uses new transient values from Database, 
                        'individual overrides all, 
                        'then group, then default, then conference
                        If venuetransient <> "" Then
                            venueDD = CDbl(venuetransient)
                        ElseIf venuetransientgroup <> "" Then
                            venueDD = CDbl(venuetransientgroup)
                        ElseIf venuetransientdefault <> "" Then
                            venueDD = CDbl(venuetransientdefault)
                        End If

                        'R2.15 NM
                        'added to override the commission rate if the booking comes from LateRooms.com
                        Dim strRet As String = FeedImportData.isLateRooms(CInt(dino.Transactionnumber))
                        If strRet.ToUpper = "LATEROOMS.COM" Then
                            If getConfig("LateRoomsFee") <> "" Then
                                venueDD = CDbl(getConfig("LateRoomsFee"))
                            End If
                            'R2.21.3 SA - override commission rate if the booking comes from Expedia  
                        ElseIf strRet.ToUpper = "EXPEDIA" Then
                            If getConfig("ExpediaFee") <> "" Then
                                venueDD = CDbl(getConfig("ExpediaFee"))
                            End If
                        End If

                        'get parameterid and transaction value form bookerinitials
                        Dim strcode As String = ""
                        If dino.Bookerinitials.ToLower = "con" Or dino.Bookerinitials = "" Then
                            strcode = "online"
                            If dino.Bookerinitials = "" Or dino.Bookerinitials.ToLower = "nys corporate" Then
                                dino.Bookerinitials = "CON"
                            End If
                        ElseIf dino.Bookerinitials.ToLower = "man" Then
                            strcode = "offline"
                        Else
                            strcode = "offline"
                        End If



                        'R2.13 CR - move the below segment of code
                        'get categoryid from database
                        'If dino.Category.ToLower = "cancellation fee" Then
                        '    dino.Category = "Room"
                        '    blncancel = True
                        'End If

                        Dim intcatid As Integer = clsStuff.notWholeNumber _
                                                (FeedCategory.getCatergoryIDFromName(dino.Category))

                        'get parameterid from database
                        Dim dtBookedDate As Date = CDate(dino.BookedDate)
                        dtBookedDate = CDate(Format(dtBookedDate, "dd/MM/yyyy"))

                        'R2.13 CR - move code to here, so that category ID can be selected correctly for cancellation fees
                        '           Still need to change the dino.Category to room though so that it can match to the param record
                        If dino.Category.ToLower = "cancellation fee" Then
                            dino.Category = "room"
                            blncancel = True
                        End If

                        Dim strType As String = dino.Category

                        If dino.Category.ToLower = "room" Then
                            strType = "room"
                        ElseIf dino.Category.ToLower = "meals" Or _
                            dino.Category.ToLower = "beverages" Then
                            strType = "meals/beverages"
                        Else 'If dino.Category.ToLower <> "room" Then
                            strType = "all extras"
                        End If

                        Dim intExtrasCharges As Integer = FeedParameter.parameterCheck(retValues.pintgroupid)
                        Dim intparamid As Integer = 0
                        If strType = "room" Then
                            intparamid = FeedParameter.parameterValueGet(strType, _
                                                            strcode, dtBookedDate, retValues.pintgroupid)
                        ElseIf strType = "meals/beverages" Then
                            If intExtrasCharges = 1 Then
                                intparamid = FeedParameter.parameterValueGet(strType, _
                                                            strcode, dtBookedDate, retValues.pintgroupid)
                            ElseIf intExtrasCharges = 2 Then
                                intparamid = FeedParameter.parameterValueGet("all extras", _
                                                            strcode, dtBookedDate, retValues.pintgroupid)
                            End If
                        ElseIf strType = "all extras" Then
                            If intExtrasCharges = 1 Then
                                intparamid = FeedParameter.parameterValueGet("meals/beverages", _
                                                            strcode, dtBookedDate, retValues.pintgroupid)
                            ElseIf intExtrasCharges = 2 Then
                                intparamid = FeedParameter.parameterValueGet(strType, _
                                                            strcode, dtBookedDate, retValues.pintgroupid)
                            End If
                        End If
                        'this will add a transaction value to each person
                        'if group is set us as PP booking types
                        If intparamid > 0 Then
                            If strTType = "PP" Then
                                If strLastGuestPNR = strCurrentGuestPNR And strCurrentGuestPNR <> "" Then
                                    If dino.Category.ToLower = "room" Then
                                        intparamid = 0
                                    ElseIf strType = "meals/beverages" Then
                                        If intExtrasCharges = 1 Or intExtrasCharges = 2 Then
                                            If blnExtrasAdded Then
                                                intparamid = 0
                                            Else
                                                blnExtrasAdded = True
                                            End If
                                        End If
                                    ElseIf strType = "all extras" Then
                                        If intExtrasCharges = 1 Or intExtrasCharges = 2 Then
                                            If blnExtrasAdded Then
                                                intparamid = 0
                                            Else
                                                blnExtrasAdded = True
                                            End If
                                        End If
                                    End If
                                Else
                                    blnExtrasAdded = False
                                End If
                            Else
                                If strLastBookingID = strCurrentBookingID Then
                                    If dino.Category.ToLower = "room" Then
                                        intparamid = 0
                                    ElseIf strType = "meals/beverages" Then
                                        If intExtrasCharges = 1 Or intExtrasCharges = 2 Then
                                            If blnExtrasAdded Then
                                                intparamid = 0
                                            Else
                                                blnExtrasAdded = True
                                            End If
                                        End If
                                    ElseIf strType = "all extras" Then
                                        If intExtrasCharges = 1 Or intExtrasCharges = 2 Then
                                            If blnExtrasAdded Then
                                                intparamid = 0
                                            Else
                                                blnExtrasAdded = True
                                            End If
                                        End If
                                    End If
                                Else
                                    blnExtrasAdded = False
                                End If
                            End If
                        End If

                        Dim oP As ClientMapping
                        oP = ClientMapping.getClientMapping(retValues.pintgroupid)

                        'R2.21 SA 
                        'strCompanyName = dino.Company
                        'Dim intCompanyID As Integer = clsGroup.getGroupIDFromName(dino.Company)
                        'Dim strBossOption As String
                        'strBossOption = clsClientOption.getBossOption(intCompanyID)

                        'Select Case strBossOption
                        '    Case "35"
                        '        strBossCode = dino.Costcode
                        '    Case "34"
                        '        strBossCode = dino.Booker
                        '    Case "39"
                        '        strBossCode = dino.AICol6
                        '    Case "40"
                        '        strBossCode = dino.AICol7
                        '    Case "41"
                        '        strBossCode = dino.AICol8
                        '    Case "42"
                        '        strBossCode = dino.AICol9
                        '    Case "43"
                        '        strBossCode = dino.AICol10
                        '    Case Else
                        '        'do not do anything
                        'End Select
                        Dim strNewTransactionlinenumber As String = dino.Transactionlinenumber & dino.InvoiceLineDetailID


                        Dim strDinoCostCode As String = dino.Costcode.Replace("NULL", "").Replace(" ", "")
                        Dim strDinoBooker As String = dino.Booker.Replace("NULL", "")
                        Dim fd As New FeedImportData(0, clsStuff.notWholeNumber(dino.Transactionnumber), _
                                                    clsStuff.notTheWholeNumber(strNewTransactionlinenumber), _
                                                     CType(dino.Transactiondate, Date?), _
                                                     CType(dino.Arrivaldate, Date?), _
                                                     CType(dino.Departuredate, Date?), _
                                                     strpassengername, _
                                                     dino.Ref1.Replace("NULL", ""), _
                                                     dino.Ref2.Replace("NULL", ""), _
                                                     dino.Ref3.Replace("NULL", ""), _
                                                     "", _
                                                     strDinoBooker, _
                                                     dino.Bookerinitials.Replace("NULL", ""), _
                                                     CType(dino.Nettamount, Double?), _
                                                     CType(dino.Vatamount, Double?), _
                                                     CType(dino.Vatrate, Double?), _
                                                     CType(dino.Totalamount, Double?), _
                                                     venuename, _
                                                     dino.Venuedetails.Replace("NULL", ""), _
                                                     dino.Roomdetails.Replace("NULL", ""), _
                                                     retValues.pintgroupid, _
                                                     dino.Company, _
                                                     CType(dino.Confermainvoicenumber, Integer?), _
                                                     intcatid, _
                                                     "", "", _
                                                     dino.Supplierinvoice.Replace("NULL", ""), _
                                                     dino.GuestPNR.Replace("NULL", ""), _
                                                     strDinoCostCode, _
                                                     intparamid, venuereference, _
                                                     venueBOSScode, venueEX, venueDD, _
                                                     0, "", Now, Now, 0, "", 0, "", 0, "", "", 0, _
                                                     dtBookedDate, _
                                                     False, False, _
                                                     dino.AICol6, _
                                                     dino.AICol7, _
                                                     dino.AICol8, _
                                                     dino.AICol9, _
                                                     dino.AICol10, _
                                                     dino.Currency, _
                                                     blncancel, _
                                                     False, _
                                                     dino.OutofPolicyReason, _
                                                     dino.Last4Digits, _
                                                     dino.TravellerEmail, _
                                                     dino.BookerEmail)
                        'dino.Currency)

                        'R2.21 SA
                        'intTransactionNo = clsStuff.notWholeNumber(dino.Transactionnumber)

                        Dim icount As Integer = fd.saveImport()
                        If icount = 0 Then

                            log.Info("Failure in feedimportdata_save stored procedure")
                            failures.Add(New ErrorLineDetails(intcounts, Nothing))
                            strErrorBookingIDs = strErrorBookingIDs & strCurrentBookingID & ","
                            strerror = "Failure in feedimportdata_save stored procedure"
                        Else
                            log.Info("Imported " + icount.ToString())
                            strerror = "OK"
                        End If

                        'R2.20 SA
                        'If strBossCode <> "" Then
                        '    Dim intRet As Integer
                        '    intRet = clsCode.BossCodeSave(intTransactionNo, strCompanyName, strBossCode)
                        '    'Dim bosscode As New clsCode
                        '    'bosscode = clsCode.getByName(strCompanyName)

                        '    'If bosscode.Codeid > 0 Then
                        '    '    bosscode.Customercode = strBossCode
                        '    '    bosscode.save()
                        '    'End If
                        'End If


                        'always write back to csv so can archive if all OK, or show errors if not
                        For Each s As String In New String() {dino.Transactionnumber, _
                                                              CStr(dino.Transactionlinenumber & dino.InvoiceLineDetailID), _
                                                              dino.Transactiondate, _
                                                              dino.Departuredate, _
                                                              dino.Arrivaldate, _
                                                              dino.Passengername, _
                                                              dino.Totalamount, _
                                                              dino.Vatamount, _
                                                              dino.Vatrate, _
                                                              dino.Nettamount, _
                                                              dino.Venuename, _
                                                              dino.Venuedetails, _
                                                              dino.Roomdetails, _
                                                              dino.Company, _
                                                              dino.Confermainvoicenumber, _
                                                              dino.Category, _
                                                              dino.Supplierinvoice, _
                                                              dino.GuestPNR, _
                                                              dino.Ref1, _
                                                              dino.Booker, _
                                                              dino.BookedDate, _
                                                              dino.OutofPolicyReason, _
                                                              dino.Last4Digits, _
                                                              dino.TravellerEmail, _
                                                              dino.BookerEmail, _
                                                              dino.Costcode, _
                                                              dino.Ref2, _
                                                              dino.Ref3, _
                                                              dino.Bookerinitials, _
                                                              dino.AICol6, _
                                                              dino.AICol7, _
                                                              dino.AICol8, _
                                                              dino.AICol9, _
                                                              dino.AICol10, _
                                                              dino.Currency, _
                                                              CStr(blncancel), _
                                                              strerror}
                            'dino.Currency, _

                            fr.Write(ToCsvCell(clsStuff.notString(s)))
                            fr.Write(",")
                        Next

                        fr.WriteLine("")
                        strLastBookingID = dino.Transactionnumber
                        strLastGuestPNR = dino.GuestPNR
                    End If


                Catch ex As Exception
                    'add to failures list so can check when run complete if there are any failures
                    failures.Add(New ErrorLineDetails(intcounts, ex))
                    strErrorBookingIDs = strErrorBookingIDs & strCurrentBookingID & ","
                    For Each s As String In New String() {dino.Transactionnumber, _
                                                          CStr(dino.Transactionlinenumber & dino.InvoiceLineDetailID), _
                                                          dino.Transactiondate, _
                                                          dino.Departuredate, _
                                                          dino.Arrivaldate, _
                                                          dino.Passengername, _
                                                          dino.Totalamount, _
                                                          dino.Vatamount, _
                                                          dino.Vatrate, _
                                                          dino.Nettamount, _
                                                          dino.Venuename, _
                                                          dino.Venuedetails, _
                                                          dino.Roomdetails, _
                                                          dino.Company, _
                                                          dino.Confermainvoicenumber, _
                                                          dino.Category, _
                                                          dino.Supplierinvoice, _
                                                          dino.GuestPNR, _
                                                          dino.Ref1, _
                                                          dino.Booker, _
                                                          dino.BookedDate, _
                                                          dino.OutofPolicyReason, _
                                                          dino.Last4Digits, _
                                                          dino.TravellerEmail, _
                                                          dino.BookerEmail, _
                                                          dino.Costcode, _
                                                          dino.Ref2, _
                                                          dino.Ref3, _
                                                          dino.Bookerinitials, _
                                                          dino.AICol6, _
                                                          dino.AICol7, _
                                                          dino.AICol8, _
                                                          dino.AICol9, _
                                                          dino.AICol10, _
                                                          dino.Currency, _
                                                          CStr(blncancel), ex.Message}
                        'dino.Currency, _

                        fr.Write(ToCsvCell(clsStuff.notString(s)))
                        fr.Write(",")
                    Next
                    fr.WriteLine("")
                    strLastBookingID = dino.Transactionnumber
                    strLastGuestPNR = dino.GuestPNR
                End Try
                intcounts = intcounts + 1


            End If
        Next

        If failures.Count > 0 Then
            'delete bookingids from database as some rows of same bookingID have failed
            Try
                If strErrorBookingIDs <> "" Then
                    FeedImportData.delete(strErrorBookingIDs & "0")
                End If
            Catch ex As Exception
                log.Error(ex.Message)
                'not really bothered if this fails as IDs will not have been saved due 
                'to them being incorrect anyway
            End Try

            'write failed records to disk
            Dim ofiler As New System.IO.StreamWriter(getConfig("downloadedfilesERROR") & _
                                                     "Error_" & Format(Now, "dd-MM-yy") & _
                                                     ".csv", False, Encoding.Default)
            ofiler.Write(fr.ToString)
            ofiler.Flush()
            ofiler.Close()
            fr.Close()
            Return False
        Else
            'R17 CR
            'if some duplicates have been added then write the file
            If blnDuplicatesAdded Then
                Dim oDuplicatesFiler As New System.IO.StreamWriter(getConfig("downloadedfilesDuplicates") & _
                                                     Format(Now, "dd-MM-yy") & _
                                                     ".csv", False, Encoding.Default)
                oDuplicatesFiler.Write(swrDuplicates.ToString)
                oDuplicatesFiler.Flush()
                oDuplicatesFiler.Close()
            End If
            swrDuplicates.Close()

            'archive OK records to disk
            Dim ofiler As New System.IO.StreamWriter(getConfig("downloadedfilesOK") & _
                                                     Format(Now, "dd-MM-yy") & _
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
