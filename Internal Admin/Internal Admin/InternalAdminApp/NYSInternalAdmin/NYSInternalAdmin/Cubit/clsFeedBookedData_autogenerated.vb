Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class clsFeedBookedData

    Public Sub New( _
        ByVal pDataid As Integer, _
        ByVal pBookingId As Nullable(Of Integer), _
        ByVal pLineNumber As Nullable(Of Integer), _
        ByVal pBookedDate As Nullable(Of DateTime), _
        ByVal pArrivalDate As Nullable(Of DateTime), _
        ByVal pDepartureDate As Nullable(Of DateTime), _
        ByVal pAmendedDate As String, _
        ByVal pAmendedByPerson As String, _
        ByVal pCancellationDate As String, _
        ByVal pCancelledByPerson As String, _
        ByVal pLeadPassengerName As String, _
        ByVal pBookingReference As String, _
        ByVal pMajorDestination As String, _
        ByVal pRefBooker As String, _
        ByVal pTransactionCurrency As String, _
        ByVal pTotalSaleAmount As Nullable(Of Double), _
        ByVal pTotalBilledInGbp As Nullable(Of Double), _
        ByVal pPaymentMethod As String, _
        ByVal pSupplierID As Nullable(Of Integer), _
        ByVal pSupplierName As String, _
        ByVal pHotelDetails As String, _
        ByVal pRoomDetails As String, _
        ByVal pGroupid As Nullable(Of Integer), _
        ByVal pGroupname As String, _
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
        mDataid = pDataid
        mBookingId = pBookingId
        mLineNumber = pLineNumber
        mBookedDate = pBookedDate
        mArrivalDate = pArrivalDate
        mDepartureDate = pDepartureDate
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
        mRoomDetails = pRoomDetails
        mGroupid = pGroupid
        mGroupname = pGroupname
        mCountryCode = pCountryCode
        mGuestPNR = pGuestPNR
        mChannel = pChannel
        mCreator = pCreator
        mCreatorCompany = pCreatorCompany
        mCurrentStatus = pCurrentStatus
        mOutofPolicyReason = pOutofPolicyReason
        mLast4Digits = pLast4Digits
        mTravellerEmail = pTravellerEmail
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

    Public Sub New( _
)
    End Sub

    Public Sub New( _
        ByVal pDataid As Integer, _
        ByVal pBookingId As Nullable(Of Integer), _
        ByVal pBookedDate As Nullable(Of DateTime), _
        ByVal pArrivalDate As Nullable(Of DateTime), _
        ByVal pDepartureDate As Nullable(Of DateTime), _
        ByVal pLeadPassengerName As String, _
        ByVal pBookingReference As String, _
        ByVal pTotalBilledInGbp As Nullable(Of Double), _
        ByVal pSupplierName As String, _
        ByVal pHotelDetails As String, _
        ByVal pRoomDetails As String, _
        ByVal pGroupname As String, _
        ByVal pAIBkrName As String, _
        ByVal pGuestPNR As String, _
        ByVal punabletosendreason As String, _
        ByVal pLast4Digits As String, _
        ByVal psupplierinvoice As String)
        mDataid = pDataid
        mBookingId = pBookingId
        mBookedDate = pBookedDate
        mArrivalDate = pArrivalDate
        mDepartureDate = pDepartureDate
        mLeadPassengerName = pLeadPassengerName
        mBookingReference = pBookingReference
        mTotalBilledInGbp = pTotalBilledInGbp
        mSupplierName = pSupplierName
        mHotelDetails = pHotelDetails
        mRoomDetails = pRoomDetails
        mGroupname = pGroupname
        mAIBkrName = pAIBkrName
        mGuestPNR = pGuestPNR
        munabletosendreason = punabletosendreason
        mLast4Digits = pLast4Digits
        msupplierinvoice = psupplierinvoice
    End Sub

    'R2.20C CR
    Private mBookerEmail As String

    Private mTravellerEmail As String
    Private msupplierinvoice As String
    Private munabletosendreason As String
    Private mDataid As Integer
    Private mBookingId As Nullable(Of Integer)
    Private mLineNumber As Nullable(Of Integer)
    Private mBookedDate As Nullable(Of DateTime)
    Private mArrivalDate As Nullable(Of DateTime)
    Private mDepartureDate As Nullable(Of DateTime)
    Private mAmendedDate As String
    Private mAmendedByPerson As String
    Private mCancellationDate As String
    Private mCancelledByPerson As String
    Private mLeadPassengerName As String
    Private mBookingReference As String
    Private mMajorDestination As String
    Private mRefBooker As String
    Private mTransactionCurrency As String
    Private mTotalSaleAmount As Nullable(Of Double)
    Private mTotalBilledInGbp As Nullable(Of Double)
    Private mPaymentMethod As String
    Private mSupplierID As Nullable(Of Integer)
    Private mSupplierName As String
    Private mHotelDetails As String
    Private mRoomDetails As String
    Private mGroupid As Nullable(Of Integer)
    Private mGroupname As String
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

    Public Property supplierinvoice() As String
        Get
            Return msupplierinvoice
        End Get
        Set(ByVal value As String)
            msupplierinvoice = value
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

    Public Property unabletosendreason() As String
        Get
            Return munabletosendreason
        End Get
        Set(ByVal value As String)
            munabletosendreason = value
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

    Public Property Dataid() As Integer
        Get
            Return mDataid
        End Get
        Set(ByVal value As Integer)
            mDataid = value
        End Set
    End Property

    Public Property BookingId() As Nullable(Of Integer)
        Get
            Return mBookingId
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mBookingId = value
        End Set
    End Property

    Public Property LineNumber() As Nullable(Of Integer)
        Get
            Return mLineNumber
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mLineNumber = value
        End Set
    End Property

    Public Property BookedDate() As Nullable(Of DateTime)
        Get
            Return mBookedDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mBookedDate = value
        End Set
    End Property

    Public Property ArrivalDate() As Nullable(Of DateTime)
        Get
            Return mArrivalDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mArrivalDate = value
        End Set
    End Property

    Public Property DepartureDate() As Nullable(Of DateTime)
        Get
            Return mDepartureDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mDepartureDate = value
        End Set
    End Property

    Public Property LeadPassengerName() As String
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

    Public Property TotalSaleAmount() As Nullable(Of Double)
        Get
            Return mTotalSaleAmount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mTotalSaleAmount = value
        End Set
    End Property

    Public Property TotalBilledInGbp() As Nullable(Of Double)
        Get
            Return mTotalBilledInGbp
        End Get
        Set(ByVal value As Nullable(Of Double))
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

    Public Property SupplierID() As Nullable(Of Integer)
        Get
            Return mSupplierID
        End Get
        Set(ByVal value As Nullable(Of Integer))
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
            Return mRoomDetails
        End Get
        Set(ByVal value As String)
            mRoomDetails = value
        End Set
    End Property

    Public Property Groupid() As Nullable(Of Integer)
        Get
            Return mGroupid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mGroupid = value
        End Set
    End Property

    Public Property Groupname() As String
        Get
            Return mGroupname
        End Get
        Set(ByVal value As String)
            mGroupname = value
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

    Public Property AICostCode() As String
        Get
            Return mAICostCode
        End Get
        Set(ByVal value As String)
            mAICostCode = value
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

    Private Shared Function makeFeedBookedInvoiceEmailFromRowX( _
            ByVal r As IDataReader _
        ) As clsFeedBookedData
        Return New clsFeedBookedData( _
                CInt(r.Item("dataid")), _
                toNullableInteger(r.Item("BookingId")), _
                toNullableDate(r.Item("BookedDate")), _
                toNullableDate(r.Item("ArrivalDate")), _
                toNullableDate(r.Item("DepartureDate")), _
                toStr(r.Item("LeadPassengerName")), _
                toStr(r.Item("BookingReference")), _
                toNullableFloat(r.Item("TotalBilledInGbp")), _
                toStr(r.Item("SupplierName")), _
                toStr(r.Item("HotelDetails")), _
                toStr(r.Item("RoomDetails")), _
                toStr(r.Item("groupname")), _
                toStr(r.Item("AIBkrName")), _
                toStr(r.Item("GuestPNR")), _
                toStr(r.Item("unabletosendreason")), _
                toStr(r.Item("Last4Digits")), _
                toStr(r.Item("supplierinvoice")))
    End Function

    Private Shared Function makeFeedBookedDataFromRowX( _
            ByVal r As IDataReader _
        ) As clsFeedBookedData
        Return New clsFeedBookedData( _
                CInt(r.Item("dataid")), _
                toNullableInteger(r.Item("BookingId")), _
                toNullableInteger(r.Item("LineNumber")), _
                toNullableDate(r.Item("BookedDate")), _
                toNullableDate(r.Item("ArrivalDate")), _
                toNullableDate(r.Item("DepartureDate")), _
                toStr(r.Item("AmendedDate")), _
                toStr(r.Item("AmendedByPerson")), _
                toStr(r.Item("CancellationDate")), _
                toStr(r.Item("CancelledByPerson")), _
                toStr(r.Item("LeadPassengerName")), _
                toStr(r.Item("BookingReference")), _
                toStr(r.Item("MajorDestination")), _
                toStr(r.Item("RefBooker")), _
                toStr(r.Item("TransactionCurrency")), _
                toNullableFloat(r.Item("TotalSaleAmount")), _
                toNullableFloat(r.Item("TotalBilledInGbp")), _
                toStr(r.Item("PaymentMethod")), _
                toNullableInteger(r.Item("SupplierID")), _
                toStr(r.Item("SupplierName")), _
                toStr(r.Item("HotelDetails")), _
                toStr(r.Item("RoomDetails")), _
                toNullableInteger(r.Item("groupid")), _
                toStr(r.Item("groupname")), _
                toStr(r.Item("CountryCode")), _
                toStr(r.Item("GuestPNR")), _
                toStr(r.Item("Channel")), _
                toStr(r.Item("Creator")), _
                toStr(r.Item("CreatorCompany")), _
                toStr(r.Item("CurrentStatus")), _
                toStr(r.Item("OutofPolicyReason")), _
                toStr(r.Item("Last4Digits")), _
                toStr(r.Item("TravellerEmail")), _
                toStr(r.Item("BookerEmail")), _
                toStr(r.Item("AIBkrName")), _
                toStr(r.Item("AICostCode")), _
                toStr(r.Item("AIBillInstruct")), _
                toStr(r.Item("AIComments")), _
                toStr(r.Item("AIAgentBkr")), _
                toStr(r.Item("AICol6")), _
                toStr(r.Item("AICol7")), _
                toStr(r.Item("AICol8")), _
                toStr(r.Item("AICol9")), _
                toStr(r.Item("AICol10")))
    End Function

    Public Shared Function [missingInvoiceEmailCheckandsend]() As List(Of clsFeedBookedData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsFeedBookedData)()
            Using r As IDataReader = dbh.callSP("missingInvoiceEmail_checkandsend")
                While r.Read()
                    ret.Add(makeFeedBookedInvoiceEmailFromRowX(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function [missingInvoiceEmailNewemail]( _
            ByVal psuppliername As String _
        ) As List(Of clsFeedBookedData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsFeedBookedData)()
            Using r As IDataReader = dbh.callSP("missingInvoiceEmail_newemail", _
                                                    "@SupplierName", psuppliername)
                While r.Read()
                    ret.Add(makeFeedBookedInvoiceEmailFromRowX(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function [initialInvoiceEmailList]( _
            ByVal pDays As Integer _
        ) As List(Of clsFeedBookedData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsFeedBookedData)()
            Using r As IDataReader = dbh.callSP("missingInvoiceEmail_initial", "@days", pDays)
                While r.Read()
                    ret.Add(makeFeedBookedInvoiceEmailFromRowX(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function [repeatInvoiceEmailList]( _
            ByVal pDays As Integer _
        ) As List(Of clsFeedBookedData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsFeedBookedData)()
            Using r As IDataReader = dbh.callSP("missingInvoiceEmail_repeat", "@days", pDays)
                While r.Read()
                    ret.Add(makeFeedBookedInvoiceEmailFromRowX(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function unsentInvoiceEmailList() As List(Of clsFeedBookedData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsFeedBookedData)()
            Using r As IDataReader = dbh.callSP("unsentInvoiceEmail_list")
                While r.Read()
                    ret.Add(makeFeedBookedInvoiceEmailFromRowX(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function venueInvoiceEmailGet(ByVal pvereference As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim stremail As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing("venueAccountsEmail_get", _
                                                 "@ve_reference", pvereference, _
                                                 "@databasename", getConfig("databasename")))
            Return stremail
        End Using
    End Function


    Public Shared Function InvoiceEmailGet( _
            ByVal pDataid As Integer _
        ) As clsFeedBookedData
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("InvoiceEmail_get", "@dataid", pDataid)
                If Not r.Read() Then
                    Throw New Exception("No FeedBookedData with id " & pDataid)
                End If
                Dim ret As clsFeedBookedData = makeFeedBookedInvoiceEmailFromRowX(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function invoiceEmailsave(ByVal pdataid As Integer, _
                                     ByVal pdontsend As Boolean, _
                                     ByVal punabletosend As Boolean, _
                                     ByVal punabletosendreason As String, _
                                     ByVal pdateEmailSent As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedBookedDataInvoiceEmail_save", _
                                                            "@Dataid", pdataid, _
                                                            "@dontsend", pdontsend, _
                                                            "@unabletosend", punabletosend, _
                                                            "@unabletosendreason", punabletosendreason, _
                                                            "@dateEmailSent", pdateEmailSent))
            Return intRet
        End Using
    End Function

    Public Shared Function venueAccountsEmailSave(ByVal paccountsemail As String, _
                                                    ByVal pve_reference As Integer, _
                                                    ByVal pDb As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("venueAccountsEmail_save", _
                                                            "@accountsemail", paccountsemail, _
                                                            "@ve_reference", pve_reference, _
                                                            "@db", pDb))
            Return intRet
        End Using
    End Function

    'Public Shared Function [get]( _
    '        ByVal pDataid As Integer _
    '    ) As clsFeedBookedData
    '    Using dbh As New SqlDatabaseHandle(getConnection)
    '        Using r As IDataReader = dbh.callSP("FeedBookedData_get", "@dataid", pDataid)
    '            If Not r.Read() Then
    '                Throw New Exception("No FeedBookedData with id " & pDataid)
    '            End If
    '            Dim ret As clsFeedBookedData = makeFeedBookedDataFromRow(r)
    '            Return ret
    '        End Using
    '    End Using
    'End Function

    'Public Shared Function list() As List(Of clsFeedBookedData)
    '    Using dbh As New SqlDatabaseHandle(getConnection)
    '        Dim ret As New List(Of clsFeedBookedData)()
    '        Using r As IDataReader = dbh.callSP("FeedBookedData_list")
    '            While r.Read()
    '                ret.Add(makeFeedBookedDataFromRow(r))
    '            End While
    '        End Using
    '        Return ret
    '    End Using
    'End Function

   

    Public Function saveImport() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mDataid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedBookedData_save", "@Dataid", mDataid, "@BookingId", mBookingId, _
                                                 "@LineNumber", mLineNumber, "@BookedDate", mBookedDate, _
                                                 "@ArrivalDate", mArrivalDate, "@DepartureDate", mDepartureDate, _
                                                 "@LeadPassengerName", mLeadPassengerName, "@BookingReference", mBookingReference, _
                                                 "@MajorDestination", mMajorDestination, "@RefBooker", mRefBooker, _
                                                 "@TransactionCurrency", mTransactionCurrency, "@TotalSaleAmount", mTotalSaleAmount, _
                                                 "@TotalBilledInGbp", mTotalBilledInGbp, "@PaymentMethod", mPaymentMethod, _
                                                 "@SupplierID", mSupplierID, "@SupplierName", mSupplierName, "@HotelDetails", mHotelDetails, _
                                                 "@RoomDetails", mRoomDetails, "@Groupid", mGroupid, "@Groupname", mGroupname, _
                                                 "@CountryCode", mCountryCode, "@GuestPNR", mGuestPNR, "@Channel", mChannel, _
                                                 "@Creator", mCreator, "@CreatorCompany", mCreatorCompany, "@AIBkrName", mAIBkrName, _
                                                 "@AICostCode", mAICostCode, "@AIBillInstruct", mAIBillInstruct, "@AIComments", mAIComments, _
                                                 "@AIAgentBkr", mAIAgentBkr, "@AICol6", mAICol6, "@AICol7", mAICol7, _
                                                 "@AICol8", mAICol8, "@AICol9", mAICol9, "@AICol10", mAICol10, "@CurrentStatus", mCurrentStatus, _
                                                 "@AmendedDate", mAmendedDate, "@AmendedByPerson", mAmendedByPerson, _
                                                 "@CancellationDate", mCancellationDate, "@CancelledByPerson", mCancelledByPerson, _
                                                 "@OutofPolicyReason", mOutofPolicyReason, "@Last4Digits", mLast4Digits, _
                                                 "@TravellerEmail", mTravellerEmail, "@BookerEmail", mBookerEmail))
            Return mDataid
        End Using
    End Function

    Public Shared Function updateSupplierInvoice(ByVal pID As Integer, ByVal pstrSupplierInvoice As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("supplierInvoice_update", "@ID", pID, "@SupplierInvoice", pstrSupplierInvoice))
            Return intRet
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pDataid As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedBookedData_delete", "@Dataid", pDataid)
        End Using
    End Sub

    Public Sub delete()
        delete(mDataid)
    End Sub


End Class
