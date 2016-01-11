Option Explicit On
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Globalization
Imports DatabaseObjects
Imports EvoUtilities.ConfigUtils
Imports ReportDownloader.Utility

''' <summary>
''' Class dataImport - used to store imported data from the CSV file so it can be sorted correctly
''' </summary>
''' <remarks>
''' Created 13/03/2009 Nick Massarella
''' </remarks>
Partial Public Class dataImportBooked

    Public Sub New( _
        ByVal pBookingId As String, _
        ByVal pLineNumber As String, _
        ByVal pBookedDate As String, _
        ByVal pArrivaldate As String, _
        ByVal pDeparturedate As String, _
        ByVal pAmendedDate As String, _
        ByVal pAmendedByPerson As String, _
        ByVal pCancellationDate As String, _
        ByVal pCancelledByPerson As String, _
        ByVal pLeadPassengerName As String, _
        ByVal pBookingReference As String, _
        ByVal pMajorDestination As String, _
        ByVal pRefBooker As String, _
        ByVal pTransactionCurrency As String, _
        ByVal pTotalSaleAmount As String, _
        ByVal pTotalBilledInGbp As String, _
        ByVal pPaymentMethod As String, _
        ByVal pSupplierID As String, _
        ByVal pSupplierName As String, _
        ByVal pHotelDetails As String, _
        ByVal pRoomDetails As String, _
        ByVal pCompany As String, _
        ByVal pCountryCode As String, _
        ByVal pGuestPNR As String, _
        ByVal pChannel As String, _
        ByVal pCreator As String, _
        ByVal pCreatorCompany As String, _
        ByVal pCurrentStatus As String, _
        ByVal pOutofPolicyReason As String, _
        ByVal pLast4Digits As String, _
        ByVal pTravellerEmail As String, _
        ByVal pBookerEmail As String, _
        ByVal pAIBkrName As String, _
        ByVal pAICostCode As String, _
        ByVal pAIBillInstruct As String, _
        ByVal pAIComments As String, _
        ByVal pAIAgentBkr As String, _
        ByVal pAICol6 As String, _
        ByVal pAICol7 As String, _
        ByVal pAICol8 As String, _
        ByVal pAICol9 As String, _
        ByVal pAICol10 As String)
        mBookingId = pBookingId
        mLineNumber = pLineNumber
        mBookedDate = pBookedDate
        mArrivaldate = pArrivaldate
        mDeparturedate = pDeparturedate
        mAmendedDate = pAmendedDate
        mAmendedByPerson = pAmendedByPerson
        mCancellationDate = pCancellationDate
        mCancelledByPerson = pCancelledByPerson
        mLeadPassengerName = pLeadPassengerName
        mBookingReference = pBookingReference
        mMajorDestination = pMajorDestination
        mRefBooker = pRefBooker
        mTransactionCurrency = pTransactionCurrency
        mTotalSaleAmount = pTotalSaleAmount
        mTotalBilledInGbp = pTotalBilledInGbp
        mPaymentMethod = pPaymentMethod
        mSupplierID = pSupplierID
        mSupplierName = pSupplierName
        mHotelDetails = pHotelDetails
        mRoomdetails = pRoomDetails
        mCompany = pCompany
        mCountryCode = pCountryCode
        mGuestPNR = pGuestPNR
        mChannel = pChannel
        mCreator = pCreator
        mCreatorCompany = pCreatorCompany
        mCurrentStatus = pCurrentStatus
        mOutofPolicyReason = pOutofPolicyReason
        mLast4Digits = pLast4Digits
        mTravellerEmail = pTravellerEmail
        mBookerEmail = pBookerEmail
        mAIBkrName = pAIBkrName
        mAICostCode = pAICostCode
        mAIBillInstruct = pAIBillInstruct
        mAIComments = pAIComments
        mAIAgentBkr = pAIAgentBkr
        mAICol6 = pAICol6
        mAICol7 = pAICol7
        mAICol8 = pAICol8
        mAICol9 = pAICol9
        mAICol10 = pAICol10

        'R2.20C CR
        mBookerEmail = pBookerEmail
    End Sub

    'R2.20C CR
    Private mBookerEmail As String

    Private mTravellerEmail As String
    Private mBookingId As String
    Private mLineNumber As String
    Private mBookedDate As String
    Private mArrivaldate As String
    Private mDeparturedate As String
    Private mAmendedDate As String
    Private mAmendedByPerson As String
    Private mCancellationDate As String
    Private mCancelledByPerson As String
    Private mLeadPassengerName As String
    Private mBookingReference As String
    Private mMajorDestination As String
    Private mRefBooker As String
    Private mTransactionCurrency As String
    Private mTotalSaleAmount As String
    Private mTotalBilledInGbp As String
    Private mPaymentMethod As String
    Private mSupplierID As String
    Private mSupplierName As String
    Private mHotelDetails As String
    Private mRoomdetails As String
    Private mCompany As String
    Private mCountryCode As String
    Private mGuestPNR As String
    Private mChannel As String
    Private mCreator As String
    Private mCreatorCompany As String
    Private mCurrentStatus As String
    Private mOutofPolicyReason As String
    Private mLast4Digits As String
    Private mAIBkrName As String
    Private mAICostCode As String
    Private mAIBillInstruct As String
    Private mAIComments As String
    Private mAIAgentBkr As String
    Private mAICol6 As String
    Private mAICol7 As String
    Private mAICol8 As String
    Private mAICol9 As String
    Private mAICol10 As String

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

    Public Property Last4Digits() As String
        Get
            Return mLast4Digits
        End Get
        Set(ByVal value As String)
            mLast4Digits = value
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

    Public Property CurrentStatus() As String
        Get
            Return mCurrentStatus
        End Get
        Set(ByVal value As String)
            mCurrentStatus = value
        End Set
    End Property

    Public Property BookingId() As String
        Get
            Return mBookingId
        End Get
        Set(ByVal value As String)
            mBookingId = value
        End Set
    End Property

    Public Property LineNumber() As String
        Get
            Return mLineNumber
        End Get
        Set(ByVal value As String)
            mLineNumber = value
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

    Public Property Arrivaldate() As String
        Get
            Return mArrivaldate
        End Get
        Set(ByVal value As String)
            mArrivaldate = value
        End Set
    End Property

    Public Property DepartureDate() As String
        Get
            Return mDeparturedate
        End Get
        Set(ByVal value As String)
            mDeparturedate = value
        End Set
    End Property

    Public Property AmendedDate() As String
        Get
            Return mAmendedDate
        End Get
        Set(ByVal value As String)
            mAmendedDate = value
        End Set
    End Property

    Public Property AmendedByPerson() As String
        Get
            Return mAmendedByPerson
        End Get
        Set(ByVal value As String)
            mAmendedByPerson = value
        End Set
    End Property

    Public Property CancellationDate() As String
        Get
            Return mCancellationDate
        End Get
        Set(ByVal value As String)
            mCancellationDate = value
        End Set
    End Property

    Public Property CancelledByPerson() As String
        Get
            Return mCancelledByPerson
        End Get
        Set(ByVal value As String)
            mCancelledByPerson = value
        End Set
    End Property

    Public Property LeadPassengername() As String
        Get
            Return mLeadPassengerName
        End Get
        Set(ByVal value As String)
            mLeadPassengerName = value
        End Set
    End Property

    Public Property BookingReference() As String
        Get
            Return mBookingReference
        End Get
        Set(ByVal value As String)
            mBookingReference = value
        End Set
    End Property

    Public Property MajorDestination() As String
        Get
            Return mMajorDestination
        End Get
        Set(ByVal value As String)
            mMajorDestination = value
        End Set
    End Property

    Public Property RefBooker() As String
        Get
            Return mRefBooker
        End Get
        Set(ByVal value As String)
            mRefBooker = value
        End Set
    End Property

    Public Property TransactionCurrency() As String
        Get
            Return mTransactionCurrency
        End Get
        Set(ByVal value As String)
            mTransactionCurrency = value
        End Set
    End Property

    Public Property TotalSaleAmount() As String
        Get
            Return mTotalSaleAmount
        End Get
        Set(ByVal value As String)
            mTotalSaleAmount = value
        End Set
    End Property

    Public Property TotalBilledInGbp() As String
        Get
            Return mTotalBilledInGbp
        End Get
        Set(ByVal value As String)
            mTotalBilledInGbp = value
        End Set
    End Property

    Public Property PaymentMethod() As String
        Get
            Return mPaymentMethod
        End Get
        Set(ByVal value As String)
            mPaymentMethod = value
        End Set
    End Property

    Public Property SupplierID() As String
        Get
            Return mSupplierID
        End Get
        Set(ByVal value As String)
            mSupplierID = value
        End Set
    End Property

    Public Property SupplierName() As String
        Get
            Return mSupplierName
        End Get
        Set(ByVal value As String)
            mSupplierName = value
        End Set
    End Property

    Public Property HotelDetails() As String
        Get
            Return mHotelDetails
        End Get
        Set(ByVal value As String)
            mHotelDetails = value
        End Set
    End Property

    Public Property RoomDetails() As String
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

    Public Property CountryCode() As String
        Get
            Return mCountryCode
        End Get
        Set(ByVal value As String)
            mCountryCode = value
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

    Public Property Channel() As String
        Get
            Return mChannel
        End Get
        Set(ByVal value As String)
            mChannel = value
        End Set
    End Property

    Public Property Creator() As String
        Get
            Return mCreator
        End Get
        Set(ByVal value As String)
            mCreator = value
        End Set
    End Property

    Public Property CreatorCompany() As String
        Get
            Return mCreatorCompany
        End Get
        Set(ByVal value As String)
            mCreatorCompany = value
        End Set
    End Property

    Public Property AIBkrName() As String
        Get
            Return mAIBkrName
        End Get
        Set(ByVal value As String)
            mAIBkrName = value
        End Set
    End Property

    Public Property AIBillInstruct() As String
        Get
            Return mAIBillInstruct
        End Get
        Set(ByVal value As String)
            mAIBillInstruct = value
        End Set
    End Property

    Public Property AICostCode() As String
        Get
            Return mAICostCode
        End Get
        Set(ByVal value As String)
            mAICostCode = value
        End Set
    End Property

    Public Property AIComments() As String
        Get
            Return mAIComments
        End Get
        Set(ByVal value As String)
            mAIComments = value
        End Set
    End Property

    Public Property AIAgentBkr() As String
        Get
            Return mAIAgentBkr
        End Get
        Set(ByVal value As String)
            mAIAgentBkr = value
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
Public Class CSVReaderBooked

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
    Public Function Main() As Boolean
        'check if file exists
        Try
            Dim blnOk As Boolean = True
            '
            Dim TempFileName = getConfig("downloadedfilesBooked") & Format(Now, "dd-MM-yyyy") & ".csv"

            If IO.File.Exists(getConfig("downloadedfilesBooked") & Format(Now, "dd-MM-yyyy") & ".csv") Then
                If Not importFile(getConfig("downloadedfilesBooked") & Format(Now, "dd-MM-yyyy") & ".csv", _
                                  getConfig("downloadedfilesBookedERROR"), _
                                  getConfig("downloadedfilesBookedDuplicates"), _
                                  getConfig("downloadedfilesBookedOK")) Then
                    blnOk = False
                End If
            End If

            Return blnOk
        Catch ex As Exception
            log.Error(ex.Message)
            Return False
        End Try
    End Function 'Main

    'R2.21.7 CR
    'new function for afternoon file
    Public Function Afternoon(ByVal pOurFileDate As String) As Boolean
        Try
            Dim blnOK As Boolean = True

            If IO.File.Exists(getConfig("downloadedfilesBookedAft") & pOurFileDate & ".csv") Then
                If Not importFile(getConfig("downloadedfilesBookedAft") & pOurFileDate & ".csv", _
                                  getConfig("downloadedfilesBookedAftERROR"), _
                                  getConfig("downloadedfilesBookedAftDuplicates"), _
                                  getConfig("downloadedfilesBookedAftOK")) Then
                    blnOK = False
                End If
            End If

            Return blnOK
        Catch ex As Exception
            log.Error(ex.Message)
            Return False
        End Try
    End Function

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
        BookingId = 0
        LineNumber
        BookedDate
        Arrivaldate
        Departuredate
        AmendedDate
        AmendedByPerson
        CancellationDate
        CancelledByPerson
        LeadPassengerName
        BookingReference
        MajorDestination
        RefBooker
        TransactionCurrency
        TotalSaleAmount
        TotalBilledInGbp
        PaymentMethod
        SupplierID
        SupplierName
        HotelDetails
        RoomDetails
        Company
        CountryCode
        GuestPNR
        Channel
        Creator
        CreatorCompany
        CurrentStatus
        OutofPolicyReason
        Last4Digits
        TravellerEmail 

        'R2.20C CR - new booker email address
        BookerEmail

        AIBkrName
        AICostCode
        AIBillInstruct
        AIComments
        AIAgentBkr
        AICol6
        AICol7
        AICol8
        AICol9
        AICol10
    End Enum

    'R2.21.7 CR - pass in the error, duplicate, and OK folder paths
    ''' <summary>
    ''' Function importFile - reads the new file, adds each line to a dataimport record. 
    ''' There are two different ways as the file may contain records in a single column or 
    ''' multiple columns. Sends sorted list to the saveData method.
    ''' </summary>
    ''' <param name="pstrFile"></param>
    ''' <returns>True/false depending upon any failures</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Public Function importFile(ByVal pstrFile As String,
                               ByVal pstrErrorFolder As String,
                               ByVal pstrDuplicateFolder As String,
                               ByVal pstrOKFolder As String) As Boolean
        Try

            Dim parser As New CSVParser.CSVParser(readAllText(pstrFile))
            Dim line As IList(Of String) = parser.readLine()
            Dim dinosaurs As New List(Of dataImportBooked)

            Do While Not line Is Nothing
                If line.Count = 1 Then
                    Dim Values() As String = Split(item(line, 0), ",")
                    If item(Values, 0).ToLower <> "bookingid" Then

                        'First let's add everything in the csv file to a new list so we can sort on bookingID
                        ' this will allow us to group the records so it's easier to load into the DB
                        Dim v As New dataImportBooked(item(Values, FieldIndexes.BookingId), _
                                                item(Values, FieldIndexes.LineNumber), _
                                                item(Values, FieldIndexes.BookedDate), _
                                                item(Values, FieldIndexes.Arrivaldate), _
                                                item(Values, FieldIndexes.Departuredate), _
                                                item(Values, FieldIndexes.AmendedDate), _
                                                item(Values, FieldIndexes.AmendedByPerson), _
                                                item(Values, FieldIndexes.CancellationDate), _
                                                item(Values, FieldIndexes.CancelledByPerson), _
                                                item(Values, FieldIndexes.LeadPassengerName), _
                                                item(Values, FieldIndexes.BookingReference), _
                                                item(Values, FieldIndexes.MajorDestination), _
                                                item(Values, FieldIndexes.RefBooker), _
                                                item(Values, FieldIndexes.TransactionCurrency), _
                                                item(Values, FieldIndexes.TotalSaleAmount), _
                                                item(Values, FieldIndexes.TotalBilledInGbp), _
                                                item(Values, FieldIndexes.PaymentMethod), _
                                                item(Values, FieldIndexes.SupplierID), _
                                                item(Values, FieldIndexes.SupplierName), _
                                                item(Values, FieldIndexes.HotelDetails), _
                                                item(Values, FieldIndexes.RoomDetails), _
                                                item(Values, FieldIndexes.Company), _
                                                item(Values, FieldIndexes.CountryCode), _
                                                item(Values, FieldIndexes.GuestPNR), _
                                                item(Values, FieldIndexes.Channel), _
                                                item(Values, FieldIndexes.Creator), _
                                                item(Values, FieldIndexes.CreatorCompany), _
                                                item(Values, FieldIndexes.CurrentStatus), _
                                                item(Values, FieldIndexes.OutofPolicyReason), _
                                                item(Values, FieldIndexes.Last4Digits), _
                                                item(Values, FieldIndexes.TravellerEmail), _
                                                item(Values, FieldIndexes.BookerEmail), _
                                                item(Values, FieldIndexes.AIBkrName), _
                                                item(Values, FieldIndexes.AICostCode), _
                                                item(Values, FieldIndexes.AIBillInstruct), _
                                                item(Values, FieldIndexes.AIComments), _
                                                item(Values, FieldIndexes.AIAgentBkr), _
                                                item(Values, FieldIndexes.AICol6), _
                                                item(Values, FieldIndexes.AICol7), _
                                                item(Values, FieldIndexes.AICol8), _
                                                item(Values, FieldIndexes.AICol9), _
                                                item(Values, FieldIndexes.AICol10))
                        dinosaurs.Add(v)
                    End If
                Else
                    If line.Count > 1 Then
                        If item(line, 0).ToLower <> "BookingId" Then

                            'First let's add everything in the csv file to a new list so we can sort on bookingID
                            ' this will allow us to group the records so it's easier to load into the DB
                            Dim v As New dataImportBooked(item(line, FieldIndexes.BookingId), _
                                                item(line, FieldIndexes.LineNumber), _
                                                item(line, FieldIndexes.BookedDate), _
                                                item(line, FieldIndexes.Arrivaldate), _
                                                item(line, FieldIndexes.Departuredate), _
                                                item(line, FieldIndexes.AmendedDate), _
                                                item(line, FieldIndexes.AmendedByPerson), _
                                                item(line, FieldIndexes.CancellationDate), _
                                                item(line, FieldIndexes.CancelledByPerson), _
                                                item(line, FieldIndexes.LeadPassengerName), _
                                                item(line, FieldIndexes.BookingReference), _
                                                item(line, FieldIndexes.MajorDestination), _
                                                item(line, FieldIndexes.RefBooker), _
                                                item(line, FieldIndexes.TransactionCurrency), _
                                                item(line, FieldIndexes.TotalSaleAmount), _
                                                item(line, FieldIndexes.TotalBilledInGbp), _
                                                item(line, FieldIndexes.PaymentMethod), _
                                                item(line, FieldIndexes.SupplierID), _
                                                item(line, FieldIndexes.SupplierName), _
                                                item(line, FieldIndexes.HotelDetails), _
                                                item(line, FieldIndexes.RoomDetails), _
                                                item(line, FieldIndexes.Company), _
                                                item(line, FieldIndexes.CountryCode), _
                                                item(line, FieldIndexes.GuestPNR), _
                                                item(line, FieldIndexes.Channel), _
                                                item(line, FieldIndexes.Creator), _
                                                item(line, FieldIndexes.CreatorCompany), _
                                                item(line, FieldIndexes.CurrentStatus), _
                                                item(line, FieldIndexes.OutofPolicyReason), _
                                                item(line, FieldIndexes.Last4Digits), _
                                                item(line, FieldIndexes.TravellerEmail), _
                                                item(line, FieldIndexes.BookerEmail), _
                                                item(line, FieldIndexes.AIBkrName), _
                                                item(line, FieldIndexes.AICostCode), _
                                                item(line, FieldIndexes.AIBillInstruct), _
                                                item(line, FieldIndexes.AIComments), _
                                                item(line, FieldIndexes.AIAgentBkr), _
                                                item(line, FieldIndexes.AICol6), _
                                                item(line, FieldIndexes.AICol7), _
                                                item(line, FieldIndexes.AICol8), _
                                                item(line, FieldIndexes.AICol9), _
                                                item(line, FieldIndexes.AICol10))
                            dinosaurs.Add(v)
                        End If
                    End If
                End If
                line = parser.readLine
            Loop

            Dim Sorter As New Sorter(Of dataImportBooked)

            Sorter.SortString = "BookingId,LineNumber,GuestPNR"
            dinosaurs.Sort(Sorter)

            'R2.21.7 CR - pass in the error, duplicate, and OK folder paths
            If saveData(dinosaurs, pstrErrorFolder, pstrDuplicateFolder, pstrOKFolder) Then
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
        Dim ret As New getGroupDetailsRet
        Dim intgroupid As Integer = 0

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
    Private Function saveData(ByVal dinosaurs As List(Of dataImportBooked), _
                              ByVal pstrErrorFolder As String, ByVal pstrDuplicateFolder As String, ByVal pstrOKFolder As String) As Boolean
        Dim strErrorBookingIDs As String = ""
        Dim strCurrentBookingID As String = ""
        Dim strLastBookingID As String = ""
        Dim strCurrentGuestPNR As String = ""
        Dim strLastGuestPNR As String = ""
        Dim strerror As String = ""
        Dim failures As New List(Of ErrorLineDetails)
        Dim intcounts As Integer = 1
        Dim fr As New StringWriter(CultureInfo.CurrentCulture)

        'R17 CR
        Dim blnDuplicatesAdded As Boolean = False
        Dim swrDuplicates As New StringWriter(CultureInfo.CurrentCulture)

        fr.WriteLine("BookingId,LineNumber,BookedDate,Arrivaldate,Departuredate,AmendedDate,AmendedByPerson,CancellationDate,CancelledByPerson," & _
                    "LeadPassengerName, BookingReference, MajorDestination,RefBooker,TransactionCurrency,TotalSaleAmount,TotalBilledInGbp," & _
                    "PaymentMethod,SupplierID,SupplierName,HotelDetails,RoomDetails,Company,CountryCode,GuestPNR,Channel,Creator,CreatorCompany," & _
                    "CurrentStatus,OutofPolicyReason,Last4Digits,TravellerEmail,BookerEmail,AIBkrName,AICostCode,AIBillInstruct,AIComments,AIAgentBkr,AICol6,AICol7,AICol8,AICol9,AICol10,Error Message")

        'R17 CR
        'Same headings for duplicates file
        swrDuplicates.WriteLine("BookingId,LineNumber,BookedDate,Arrivaldate,Departuredate,AmendedDate,AmendedByPerson,CancellationDate,CancelledByPerson," & _
                    "LeadPassengerName, BookingReference, MajorDestination,RefBooker,TransactionCurrency,TotalSaleAmount,TotalBilledInGbp," & _
                    "PaymentMethod,SupplierID,SupplierName,HotelDetails,RoomDetails,Company,CountryCode,GuestPNR,Channel,Creator,CreatorCompany," & _
                    "CurrentStatus,OutofPolicyReason,Last4Digits,TravellerEmail,BookerEmail,AIBkrName,AICostCode,AIBillInstruct,AIComments,AIAgentBkr,AICol6,AICol7,AICol8,AICol9,AICol10,Error Message")



        Dim blnExtrasAdded As Boolean = False

        For Each dino As dataImportBooked In dinosaurs
            If dino.Company.ToLower = "company" Then
                'TODO make sure It was a header
            Else
                Try
                    'R17 CR
                    Dim blnIsDuplicate As Boolean = False

                    strCurrentBookingID = dino.BookingId
                    strCurrentGuestPNR = dino.GuestPNR
                    If dino.Company = "LV" Then
                        dino.Company = "LV="
                    ElseIf dino.Company.ToUpper = "ANCHOR TRUST" Then
                        dino.Company = "BS - ANCHOR TRUST"
                    ElseIf dino.Company = "CIMA" Then
                        'Dim str As String = ""
                    End If

                    'first try get group details from company name
                    Dim retValues As getGroupDetailsRet = getGroupDetails(dino.Company)

                    If retValues.pintgroupid = 0 Then
                        Throw New Exception("Group '" & dino.Company & "' does not exist.")
                    End If

                    'R2.7 NM
                    ' Need to get full venuename as Conferma chops off the name if it's too long
                    Dim NewSupplierName As String = Mid(dino.HotelDetails, 1, dino.HotelDetails.IndexOf(";"))
                    dino.SupplierName = NewSupplierName
                    checkLength("Last4Digits", dino.Last4Digits, 4)
                    checkLength("AmendedByPerson", dino.AmendedByPerson, 200)
                    checkLength("AmendedDate", dino.AmendedDate, 200)
                    checkLength("CancelledByPerson", dino.CancelledByPerson, 200)
                    checkLength("CancellationDate", dino.CancellationDate, 200)
                    checkLength("OutofPolicyReason", dino.OutofPolicyReason, 200)
                    checkLength("LeadPassengername", dino.LeadPassengername, 200)
                    checkLength("BookingReference", dino.BookingReference, 200)
                    checkLength("MajorDestination", dino.MajorDestination, 200)
                    checkLength("RefBooker", dino.RefBooker, 200)
                    checkLength("TransactionCurrency", dino.TransactionCurrency, 20)
                    checkLength("PaymentMethod", dino.PaymentMethod, 100)
                    checkLength("SupplierName", dino.SupplierName, 200)
                    checkLength("HotelDetails", dino.HotelDetails, 3000)
                    checkLength("RoomDetails", dino.RoomDetails, 500)
                    checkLength("CountryCode", dino.CountryCode, 50)
                    checkLength("GuestPNR", dino.GuestPNR, 50)
                    checkLength("Channel", dino.Channel, 50)
                    checkLength("Creator", dino.Creator, 100)
                    checkLength("CreatorCompany ", dino.CreatorCompany, 100)
                    checkLength("CurrentStatus", dino.CurrentStatus, 1)
                    checkLength("AIBkrName", dino.AIBkrName, 200)
                    checkLength("AICostCode", dino.AICostCode, 200)
                    checkLength("AIBillInstruct", dino.AIBillInstruct, 200)
                    checkLength("AIComments", dino.AIComments, 200)
                    checkLength("AIAgentBkr", dino.AIAgentBkr, 200)
                    checkLength("AICol6", dino.AICol6, 200)
                    checkLength("AICol7", dino.AICol7, 200)
                    checkLength("AICol8", dino.AICol8, 200)
                    checkLength("AICol9", dino.AICol9, 200)
                    checkLength("AICol10", dino.AICol10, 200)
                    checkLength("TravellerEmail", dino.TravellerEmail, 200)

                    'R2.20C CR
                    checkLength("BookerEmail", dino.BookerEmail, 200)


                    Try
                        Dim inttest As Integer = CInt(dino.BookingId)
                    Catch ex As Exception
                        Throw New Exception("BookingId is not a number.")
                    End Try
                    Try
                        Dim inttest As Integer = CInt(dino.LineNumber)
                    Catch ex As Exception
                        Throw New Exception("LineNumber is not a number.")
                    End Try
                    Try
                        Dim dttest As Date = CDate(dino.BookedDate)
                    Catch ex As Exception
                        Throw New Exception("BookedDate is not a date.")
                    End Try
                    Try
                        Dim dttest As Date = CDate(dino.Arrivaldate)
                    Catch ex As Exception
                        Throw New Exception("Arrivaldate is not a date.")
                    End Try
                    Try
                        Dim dttest As Date = CDate(dino.DepartureDate)
                    Catch ex As Exception
                        Throw New Exception("Departuredate is not a date.")
                    End Try
                    Try
                        Dim dbltest As Double = CDbl(dino.TotalSaleAmount)
                    Catch ex As Exception
                        Throw New Exception("TotalSaleAmount is not a numeric value.")
                    End Try
                    Try
                        Dim dbltest As Double = CDbl(dino.TotalBilledInGbp)
                    Catch ex As Exception
                        Throw New Exception("TotalBilledInGbp is not a numeric value.")
                    End Try
                    Try
                        Dim inttest As Integer = CInt(dino.SupplierID)
                    Catch ex As Exception
                        Throw New Exception("SupplierID is not a number.")
                    End Try

                    Dim fd As New clsFeedBookedData(0, CType(dino.BookingId, Integer?), _
                                                 CType(dino.LineNumber, Integer?), _
                                                 CType(dino.BookedDate, Date?), _
                                                 CType(dino.Arrivaldate, Date?), _
                                                 CType(dino.DepartureDate, Date?), _
                                                 dino.AmendedDate, _
                                                 dino.AmendedByPerson, _
                                                 dino.CancellationDate, _
                                                 dino.CancelledByPerson, _
                                                 dino.LeadPassengername, _
                                                 dino.BookingReference, _
                                                 dino.MajorDestination, _
                                                 dino.RefBooker, _
                                                 dino.TransactionCurrency, _
                                                 CType(dino.TotalSaleAmount, Double?), _
                                                 CType(dino.TotalBilledInGbp, Double?), _
                                                 dino.PaymentMethod, _
                                                 CType(dino.SupplierID, Integer?), _
                                                 dino.SupplierName, _
                                                 dino.HotelDetails, _
                                                 dino.RoomDetails, _
                                                 retValues.pintgroupid, _
                                                 dino.Company, _
                                                 dino.CountryCode, _
                                                 dino.GuestPNR, _
                                                 dino.Channel, _
                                                 dino.Creator, _
                                                 dino.CreatorCompany, _
                                                 dino.CurrentStatus, _
                                                 dino.OutofPolicyReason, _
                                                 dino.Last4Digits, _
                                                 dino.TravellerEmail, _
                                                 dino.BookerEmail, _
                                                 dino.AIBkrName, _
                                                 dino.AICostCode, _
                                                 dino.AIBillInstruct, _
                                                 dino.AIComments, _
                                                 dino.AIAgentBkr, _
                                                 dino.AICol6, _
                                                 dino.AICol7, _
                                                 dino.AICol8, _
                                                 dino.AICol9, _
                                                 dino.AICol10)

                    If fd.saveImport() = 0 Then
                        failures.Add(New ErrorLineDetails(intcounts, Nothing))
                        strErrorBookingIDs = strErrorBookingIDs & strCurrentBookingID & ","
                        strerror = "Failure in FeedBookedData_save stored procedure"
                    Else
                        strerror = "OK"
                    End If
                    'always write back to csv so can archive if all OK, or show errors if not
                    For Each s As String In New String() {dino.BookingId, _
                                                         dino.LineNumber, _
                                                         dino.BookedDate, _
                                                         dino.Arrivaldate, _
                                                         dino.DepartureDate, _
                                                         dino.AmendedDate, _
                                                         dino.AmendedByPerson, _
                                                         dino.CancellationDate, _
                                                         dino.CancelledByPerson, _
                                                         dino.LeadPassengername, _
                                                         dino.BookingReference, _
                                                         dino.MajorDestination, _
                                                         dino.RefBooker, _
                                                         dino.TransactionCurrency, _
                                                         dino.TotalSaleAmount, _
                                                         dino.TotalBilledInGbp, _
                                                         dino.PaymentMethod, _
                                                         dino.SupplierID, _
                                                         dino.SupplierName, _
                                                         dino.HotelDetails, _
                                                         dino.RoomDetails, _
                                                         dino.Company, _
                                                         dino.CountryCode, _
                                                         dino.GuestPNR, _
                                                         dino.Channel, _
                                                         dino.Creator, _
                                                         dino.CreatorCompany, _
                                                         dino.CurrentStatus, _
                                                         dino.OutofPolicyReason, _
                                                         dino.Last4Digits, _
                                                         dino.TravellerEmail, _
                                                         dino.BookerEmail, _
                                                         dino.AIBkrName, _
                                                         dino.AICostCode, _
                                                         dino.AIBillInstruct, _
                                                         dino.AIComments, _
                                                         dino.AIAgentBkr, _
                                                         dino.AICol6, _
                                                         dino.AICol7, _
                                                         dino.AICol8, _
                                                         dino.AICol9, _
                                                         dino.AICol10, _
                                                         strerror}

                        fr.Write(ToCsvCell(clsStuff.notString(s)))
                        fr.Write(",")
                    Next
                    strLastBookingID = dino.BookingId
                    strLastGuestPNR = dino.GuestPNR
                    fr.WriteLine("")
                    'End If
                Catch ex As Exception
                    'add to failures list so can check when run complete if there are any failures
                    failures.Add(New ErrorLineDetails(intcounts, ex))
                    strErrorBookingIDs = strErrorBookingIDs & strCurrentBookingID & ","
                    For Each s As String In New String() {dino.BookingId, _
                                                         dino.LineNumber, _
                                                         dino.BookedDate, _
                                                         dino.Arrivaldate, _
                                                         dino.DepartureDate, _
                                                         dino.AmendedDate, _
                                                         dino.AmendedByPerson, _
                                                         dino.CancellationDate, _
                                                         dino.CancelledByPerson, _
                                                         dino.LeadPassengername, _
                                                         dino.BookingReference, _
                                                         dino.MajorDestination, _
                                                         dino.RefBooker, _
                                                         dino.TransactionCurrency, _
                                                         dino.TotalSaleAmount, _
                                                         dino.TotalBilledInGbp, _
                                                         dino.PaymentMethod, _
                                                         dino.SupplierID, _
                                                         dino.SupplierName, _
                                                         dino.HotelDetails, _
                                                         dino.RoomDetails, _
                                                         dino.Company, _
                                                         dino.CountryCode, _
                                                         dino.GuestPNR, _
                                                         dino.Channel, _
                                                         dino.Creator, _
                                                         dino.CreatorCompany, _
                                                         dino.CurrentStatus, _
                                                         dino.OutofPolicyReason, _
                                                         dino.Last4Digits, _
                                                         dino.TravellerEmail, _
                                                         dino.BookerEmail, _
                                                         dino.AIBkrName, _
                                                         dino.AICostCode, _
                                                         dino.AIBillInstruct, _
                                                         dino.AIComments, _
                                                         dino.AIAgentBkr, _
                                                         dino.AICol6, _
                                                         dino.AICol7, _
                                                         dino.AICol8, _
                                                         dino.AICol9, _
                                                         dino.AICol10, _
                                                         ex.Message}

                        fr.Write(ToCsvCell(clsStuff.notString(s)))
                        fr.Write(",")
                    Next
                    strLastBookingID = dino.BookingId
                    strLastGuestPNR = dino.GuestPNR
                    fr.WriteLine("")
                End Try
                intcounts = intcounts + 1

            End If
        Next

        If failures.Count > 0 Then
            'delete bookingids from database as some rows of same bookingID have failed
            Try
                If strErrorBookingIDs <> "" Then
                    'FeedImportData.delete(strErrorBookingIDs & "0")
                End If
            Catch ex As Exception
                log.Error(ex.Message)
                'not really bothered if this fails as IDs will not have been saved due 
                'to them being incorrect anyway
            End Try

            'R2.21.7 CR - use the passed in folders
            'write failed records to disk
            'Dim ofiler As New System.IO.StreamWriter(getConfig("downloadedfilesBookedERROR") & _
            '                                         "Error_" & Format(Now, "dd-MM-yy") & _
            '                                         ".csv", False, Encoding.Default)
            Dim ofiler As New System.IO.StreamWriter(pstrErrorFolder & _
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
                'R2.21.7 CR - use the passed in folder
                'Dim oDuplicatesFiler As New System.IO.StreamWriter(getConfig("downloadedfilesDuplicates") & _
                '                                     Format(Now, "dd-MM-yy") & _
                '                                     ".csv", False, Encoding.Default)
                Dim oDuplicatesFiler As New System.IO.StreamWriter(pstrDuplicateFolder & _
                                                     Format(Now, "dd-MM-yy") & _
                                                     ".csv", False, Encoding.Default)

                oDuplicatesFiler.Write(swrDuplicates.ToString)
                oDuplicatesFiler.Flush()
                oDuplicatesFiler.Close()
            End If
            swrDuplicates.Close()


            'R2.21.7 CR - use the passed in folder
            'archive OK records to disk
            'Dim ofiler As New System.IO.StreamWriter(getConfig("downloadedfilesBookedOK") & _
            '                                         Format(Now, "dd-MM-yy") & _
            '                                         ".csv", False, Encoding.Default)
            Dim ofiler As New System.IO.StreamWriter(pstrOKFolder & _
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

