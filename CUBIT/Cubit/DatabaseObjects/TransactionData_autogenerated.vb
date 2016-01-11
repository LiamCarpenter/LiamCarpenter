Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class TransactionData

    Public Sub New( _
        ByVal pDataid As Integer, _
        ByVal pTransactionReference As Nullable(Of Integer), _
        ByVal pTransactionDate As String, _
        ByVal pTransactionAccountAmount As Decimal, _
        ByVal pTransactionAccountCurrency As String, _
        ByVal pTransactionMerchantAmount As Decimal, _
        ByVal pTransactionMerchantCurrency As String, _
        ByVal pTransactionMerchantNarrative As String, _
        ByVal pTransactionMCC As String, _
        ByVal pTransactionCardLastFourDigits As String, _
        ByVal pClientID As Nullable(Of Integer), _
        ByVal pClientName As String, _
        ByVal pDeploymentID As Nullable(Of Integer), _
        ByVal pDeploymentPlatform As String, _
        ByVal pDeploymentChargeStart As String, _
        ByVal pDeploymentChargeEnd As String, _
        ByVal pDeploymentAmount As Decimal, _
        ByVal pDeploymentCurrency As String, _
        ByVal pDeploymentName As String, _
        ByVal pDeploymentCreatedDate As Nullable(Of DateTime), _
        ByVal pDeploymentIdentifiers As String, _
        ByVal pDeploymentConsumerReference As String, _
        ByVal pDeploymentCustomerID As Nullable(Of Integer), _
        ByVal pDeploymentCustomerName As String, _
        ByVal pSupplierName As String, _
        ByVal pSupplierAddress1 As String, _
        ByVal pSupplierAddress2 As String, _
        ByVal pSupplierCity As String, _
        ByVal pSupplierCountryCode As String, _
        ByVal pSupplierPostalOrZipCode As String, _
        ByVal pSupplierTelephoneNumber As String, _
        ByVal pFacsimilieNumber As String, _
        ByVal pSupplierConsumerID As Nullable(Of Integer), _
        ByVal pHotelCheckIn As String, _
        ByVal pHotelCheckOut As String, _
        ByVal pHotelRoomType As String, _
        ByVal pHotelRateInformation As String, _
        ByVal pHotelCancellationPolicy As String, _
        ByVal pHotelCancellationReference As String, _
        ByVal pHotelPaymentRestrictions As String, _
        ByVal pHotelCareOfName As String, _
        ByVal pHotelContactName As String, _
        ByVal pHotelComments As String, _
        ByVal pHotelBookingPlatformName As String, _
        ByVal pTravellers As String)
        mDataid = pDataid
        mTransactionReference = pTransactionReference
        mTransactionDate = pTransactionDate
        mTransactionAccountAmount = pTransactionAccountAmount
        mTransactionAccountCurrency = pTransactionAccountCurrency
        mTransactionMerchantAmount = pTransactionMerchantAmount
        mTransactionMerchantCurrency = pTransactionMerchantCurrency
        mTransactionMerchantNarrative = pTransactionMerchantNarrative
        mTransactionMCC = pTransactionMCC
        mTransactionCardLastFourDigits = pTransactionCardLastFourDigits
        mClientID = pClientID
        mClientName = pClientName
        mDeploymentID = pDeploymentID
        mDeploymentPlatform = pDeploymentPlatform
        mDeploymentChargeStart = pDeploymentChargeStart
        mDeploymentChargeEnd = pDeploymentChargeEnd
        mDeploymentAmount = pDeploymentAmount
        mDeploymentCurrency = pDeploymentCurrency
        mDeploymentName = pDeploymentName
        mDeploymentCreatedDate = pDeploymentCreatedDate
        mDeploymentIdentifiers = pDeploymentIdentifiers
        mDeploymentConsumerReference = pDeploymentConsumerReference
        mDeploymentCustomerID = pDeploymentCustomerID
        mDeploymentCustomerName = pDeploymentCustomerName
        mSupplierName = pSupplierName
        mSupplierAddress1 = pSupplierAddress1
        mSupplierAddress2 = pSupplierAddress2
        mSupplierCity = pSupplierCity
        mSupplierCountryCode = pSupplierCountryCode
        mSupplierPostalOrZipCode = pSupplierPostalOrZipCode
        mSupplierTelephoneNumber = pSupplierTelephoneNumber
        mFacsimilieNumber = pFacsimilieNumber
        mSupplierConsumerID = pSupplierConsumerID
        mHotelCheckIn = pHotelCheckIn
        mHotelCheckOut = pHotelCheckOut
        mHotelRoomType = pHotelRoomType
        mHotelRateInformation = pHotelRateInformation
        mHotelCancellationPolicy = pHotelCancellationPolicy
        mHotelCancellationReference = pHotelCancellationReference
        mHotelPaymentRestrictions = pHotelPaymentRestrictions
        mHotelCareOfName = pHotelCareOfName
        mHotelContactName = pHotelContactName
        mHotelComments = pHotelComments
        mHotelBookingPlatformName = pHotelBookingPlatformName
        mTravellers = pTravellers
    End Sub

    Public Sub New( _
)
    End Sub

    Private mDataid As Integer
    Private mTransactionReference As Nullable(Of Integer)
    Private mTransactionDate As String
    Private mTransactionAccountAmount As Decimal
    Private mTransactionAccountCurrency As String
    Private mTransactionMerchantAmount As Decimal
    Private mTransactionMerchantCurrency As String
    Private mTransactionMerchantNarrative As String
    Private mTransactionMCC As String
    Private mTransactionCardLastFourDigits As String
    Private mClientID As Nullable(Of Integer)
    Private mClientName As String
    Private mDeploymentID As Nullable(Of Integer)
    Private mDeploymentPlatform As String
    Private mDeploymentChargeStart As String
    Private mDeploymentChargeEnd As String
    Private mDeploymentAmount As Decimal
    Private mDeploymentCurrency As String
    Private mDeploymentName As String
    Private mDeploymentCreatedDate As Nullable(Of DateTime)
    Private mDeploymentIdentifiers As String
    Private mDeploymentConsumerReference As String
    Private mDeploymentCustomerID As Nullable(Of Integer)
    Private mDeploymentCustomerName As String
    Private mSupplierName As String
    Private mSupplierAddress1 As String
    Private mSupplierAddress2 As String
    Private mSupplierCity As String
    Private mSupplierCountryCode As String
    Private mSupplierPostalOrZipCode As String
    Private mSupplierTelephoneNumber As String
    Private mFacsimilieNumber As String
    Private mSupplierConsumerID As Nullable(Of Integer)
    Private mHotelCheckIn As String
    Private mHotelCheckOut As String
    Private mHotelRoomType As String
    Private mHotelRateInformation As String
    Private mHotelCancellationPolicy As String
    Private mHotelCancellationReference As String
    Private mHotelPaymentRestrictions As String
    Private mHotelCareOfName As String
    Private mHotelContactName As String
    Private mHotelComments As String
    Private mHotelBookingPlatformName As String
    Private mTravellers As String

    Public Property Dataid() As Integer
        Get
            Return mDataid
        End Get
        Set(ByVal value As Integer)
            mDataid = value
        End Set
    End Property

    Public Property TransactionReference() As Nullable(Of Integer)
        Get
            Return mTransactionReference
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTransactionReference = value
        End Set
    End Property

    Public Property TransactionDate() As String
        Get
            Return mTransactionDate
        End Get
        Set(ByVal value As String)
            mTransactionDate = value
        End Set
    End Property

    Public Property TransactionAccountAmount() As Decimal
        Get
            Return mTransactionAccountAmount
        End Get
        Set(ByVal value As Decimal)
            mTransactionAccountAmount = value
        End Set
    End Property

    Public Property TransactionAccountCurrency() As String
        Get
            Return mTransactionAccountCurrency
        End Get
        Set(ByVal value As String)
            mTransactionAccountCurrency = value
        End Set
    End Property

    Public Property TransactionMerchantAmount() As Decimal
        Get
            Return mTransactionMerchantAmount
        End Get
        Set(ByVal value As Decimal)
            mTransactionMerchantAmount = value
        End Set
    End Property

    Public Property TransactionMerchantCurrency() As String
        Get
            Return mTransactionMerchantCurrency
        End Get
        Set(ByVal value As String)
            mTransactionMerchantCurrency = value
        End Set
    End Property

    Public Property TransactionMerchantNarrative() As String
        Get
            Return mTransactionMerchantNarrative
        End Get
        Set(ByVal value As String)
            mTransactionMerchantNarrative = value
        End Set
    End Property

    Public Property TransactionMCC() As String
        Get
            Return mTransactionMCC
        End Get
        Set(ByVal value As String)
            mTransactionMCC = value
        End Set
    End Property

    Public Property TransactionCardLastFourDigits() As String
        Get
            Return mTransactionCardLastFourDigits
        End Get
        Set(ByVal value As String)
            mTransactionCardLastFourDigits = value
        End Set
    End Property

    Public Property ClientID() As Nullable(Of Integer)
        Get
            Return mClientID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mClientID = value
        End Set
    End Property

    Public Property ClientName() As String
        Get
            Return mClientName
        End Get
        Set(ByVal value As String)
            mClientName = value
        End Set
    End Property

    Public Property DeploymentID() As Nullable(Of Integer)
        Get
            Return mDeploymentID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mDeploymentID = value
        End Set
    End Property

    Public Property DeploymentPlatform() As String
        Get
            Return mDeploymentPlatform
        End Get
        Set(ByVal value As String)
            mDeploymentPlatform = value
        End Set
    End Property

    Public Property DeploymentChargeStart() As String
        Get
            Return mDeploymentChargeStart
        End Get
        Set(ByVal value As String)
            mDeploymentChargeStart = value
        End Set
    End Property

    Public Property DeploymentChargeEnd() As String
        Get
            Return mDeploymentChargeEnd
        End Get
        Set(ByVal value As String)
            mDeploymentChargeEnd = value
        End Set
    End Property

    Public Property DeploymentAmount() As Decimal
        Get
            Return mDeploymentAmount
        End Get
        Set(ByVal value As Decimal)
            mDeploymentAmount = value
        End Set
    End Property

    Public Property DeploymentCurrency() As String
        Get
            Return mDeploymentCurrency
        End Get
        Set(ByVal value As String)
            mDeploymentCurrency = value
        End Set
    End Property

    Public Property DeploymentName() As String
        Get
            Return mDeploymentName
        End Get
        Set(ByVal value As String)
            mDeploymentName = value
        End Set
    End Property

    Public Property DeploymentCreatedDate() As Nullable(Of DateTime)
        Get
            Return mDeploymentCreatedDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mDeploymentCreatedDate = value
        End Set
    End Property

    Public Property DeploymentIdentifiers() As String
        Get
            Return mDeploymentIdentifiers
        End Get
        Set(ByVal value As String)
            mDeploymentIdentifiers = value
        End Set
    End Property

    Public Property DeploymentConsumerReference() As String
        Get
            Return mDeploymentConsumerReference
        End Get
        Set(ByVal value As String)
            mDeploymentConsumerReference = value
        End Set
    End Property

    Public Property DeploymentCustomerID() As Nullable(Of Integer)
        Get
            Return mDeploymentCustomerID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mDeploymentCustomerID = value
        End Set
    End Property

    Public Property DeploymentCustomerName() As String
        Get
            Return mDeploymentCustomerName
        End Get
        Set(ByVal value As String)
            mDeploymentCustomerName = value
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

    Public Property SupplierAddress1() As String
        Get
            Return mSupplierAddress1
        End Get
        Set(ByVal value As String)
            mSupplierAddress1 = value
        End Set
    End Property

    Public Property SupplierAddress2() As String
        Get
            Return mSupplierAddress2
        End Get
        Set(ByVal value As String)
            mSupplierAddress2 = value
        End Set
    End Property

    Public Property SupplierCity() As String
        Get
            Return mSupplierCity
        End Get
        Set(ByVal value As String)
            mSupplierCity = value
        End Set
    End Property

    Public Property SupplierCountryCode() As String
        Get
            Return mSupplierCountryCode
        End Get
        Set(ByVal value As String)
            mSupplierCountryCode = value
        End Set
    End Property

    Public Property SupplierPostalOrZipCode() As String
        Get
            Return mSupplierPostalOrZipCode
        End Get
        Set(ByVal value As String)
            mSupplierPostalOrZipCode = value
        End Set
    End Property

    Public Property SupplierTelephoneNumber() As String
        Get
            Return mSupplierTelephoneNumber
        End Get
        Set(ByVal value As String)
            mSupplierTelephoneNumber = value
        End Set
    End Property

    Public Property FacsimilieNumber() As String
        Get
            Return mFacsimilieNumber
        End Get
        Set(ByVal value As String)
            mFacsimilieNumber = value
        End Set
    End Property

    Public Property SupplierConsumerID() As Nullable(Of Integer)
        Get
            Return mSupplierConsumerID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mSupplierConsumerID = value
        End Set
    End Property

    Public Property HotelCheckIn() As String
        Get
            Return mHotelCheckIn
        End Get
        Set(ByVal value As String)
            mHotelCheckIn = value
        End Set
    End Property

    Public Property HotelCheckOut() As String
        Get
            Return mHotelCheckOut
        End Get
        Set(ByVal value As String)
            mHotelCheckOut = value
        End Set
    End Property

    Public Property HotelRoomType() As String
        Get
            Return mHotelRoomType
        End Get
        Set(ByVal value As String)
            mHotelRoomType = value
        End Set
    End Property

    Public Property HotelRateInformation() As String
        Get
            Return mHotelRateInformation
        End Get
        Set(ByVal value As String)
            mHotelRateInformation = value
        End Set
    End Property

    Public Property HotelCancellationPolicy() As String
        Get
            Return mHotelCancellationPolicy
        End Get
        Set(ByVal value As String)
            mHotelCancellationPolicy = value
        End Set
    End Property

    Public Property HotelCancellationReference() As String
        Get
            Return mHotelCancellationReference
        End Get
        Set(ByVal value As String)
            mHotelCancellationReference = value
        End Set
    End Property

    Public Property HotelPaymentRestrictions() As String
        Get
            Return mHotelPaymentRestrictions
        End Get
        Set(ByVal value As String)
            mHotelPaymentRestrictions = value
        End Set
    End Property

    Public Property HotelCareOfName() As String
        Get
            Return mHotelCareOfName
        End Get
        Set(ByVal value As String)
            mHotelCareOfName = value
        End Set
    End Property

    Public Property HotelContactName() As String
        Get
            Return mHotelContactName
        End Get
        Set(ByVal value As String)
            mHotelContactName = value
        End Set
    End Property

    Public Property HotelComments() As String
        Get
            Return mHotelComments
        End Get
        Set(ByVal value As String)
            mHotelComments = value
        End Set
    End Property

    Public Property HotelBookingPlatformName() As String
        Get
            Return mHotelBookingPlatformName
        End Get
        Set(ByVal value As String)
            mHotelBookingPlatformName = value
        End Set
    End Property

    Public Property Travellers() As String
        Get
            Return mTravellers
        End Get
        Set(ByVal value As String)
            mTravellers = value
        End Set
    End Property

    Private Shared Function makeTransactionDataFromRow( _
            ByVal r As IDataReader _
        ) As TransactionData
        Return New TransactionData( _
                clsStuff.notWholeNumber(r.Item("dataid")), _
                toNullableInteger(r.Item("TransactionReference")), _
                clsStuff.notString(r.Item("TransactionDate")), _
                clsStuff.notDecimal(r.Item("TransactionAccountAmount")), _
                clsStuff.notString(r.Item("TransactionAccountCurrency")), _
                clsStuff.notDecimal(r.Item("TransactionMerchantAmount")), _
                clsStuff.notString(r.Item("TransactionMerchantCurrency")), _
                clsStuff.notString(r.Item("TransactionMerchantNarrative")), _
                clsStuff.notString(r.Item("TransactionMCC")), _
                clsStuff.notString(r.Item("TransactionCardLastFourDigits")), _
                toNullableInteger(r.Item("ClientID")), _
                clsStuff.notString(r.Item("ClientName")), _
                toNullableInteger(r.Item("DeploymentID")), _
                clsStuff.notString(r.Item("DeploymentPlatform")), _
                clsStuff.notString(r.Item("DeploymentChargeStart")), _
                clsStuff.notString(r.Item("DeploymentChargeEnd")), _
                clsStuff.notDecimal(r.Item("DeploymentAmount")), _
                clsStuff.notString(r.Item("DeploymentCurrency")), _
                clsStuff.notString(r.Item("DeploymentName")), _
                toNullableDate(r.Item("DeploymentCreatedDate")), _
                clsStuff.notString(r.Item("DeploymentIdentifiers")), _
                clsStuff.notString(r.Item("DeploymentConsumerReference")), _
                toNullableInteger(r.Item("DeploymentCustomerID")), _
                clsStuff.notString(r.Item("DeploymentCustomerName")), _
                clsStuff.notString(r.Item("SupplierName")), _
                clsStuff.notString(r.Item("SupplierAddress1")), _
                clsStuff.notString(r.Item("SupplierAddress2")), _
                clsStuff.notString(r.Item("SupplierCity")), _
                clsStuff.notString(r.Item("SupplierCountryCode")), _
                clsStuff.notString(r.Item("SupplierPostalOrZipCode")), _
                clsStuff.notString(r.Item("SupplierTelephoneNumber")), _
                clsStuff.notString(r.Item("FacsimilieNumber")), _
                toNullableInteger(r.Item("SupplierConsumerID")), _
                clsStuff.notString(r.Item("HotelCheckIn")), _
                clsStuff.notString(r.Item("HotelCheckOut")), _
                clsStuff.notString(r.Item("HotelRoomType")), _
                clsStuff.notString(r.Item("HotelRateInformation")), _
                clsStuff.notString(r.Item("HotelCancellationPolicy")), _
                clsStuff.notString(r.Item("HotelCancellationReference")), _
                clsStuff.notString(r.Item("HotelPaymentRestrictions")), _
                clsStuff.notString(r.Item("HotelCareOfName")), _
                clsStuff.notString(r.Item("HotelContactName")), _
                clsStuff.notString(r.Item("HotelComments")), _
                clsStuff.notString(r.Item("HotelBookingPlatformName")), _
                clsStuff.notString(r.Item("Travellers")))
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intret As Integer = CInt(dbh.callSPSingleValue("TransactionData_save", "@Dataid", mDataid, _
                                                 "@TransactionReference", mTransactionReference, _
                                                 "@TransactionDate", mTransactionDate, _
                                                 "@TransactionAccountAmount", mTransactionAccountAmount, _
                                                 "@TransactionAccountCurrency", mTransactionAccountCurrency, _
                                                 "@TransactionMerchantAmount", mTransactionMerchantAmount, _
                                                 "@TransactionMerchantCurrency", mTransactionMerchantCurrency, _
                                                 "@TransactionMerchantNarrative", mTransactionMerchantNarrative, _
                                                 "@TransactionMCC", mTransactionMCC, _
                                                 "@TransactionCardLastFourDigits", mTransactionCardLastFourDigits, _
                                                 "@ClientID", mClientID, _
                                                 "@ClientName", mClientName, _
                                                 "@DeploymentID", mDeploymentID, _
                                                 "@DeploymentPlatform", mDeploymentPlatform, _
                                                 "@DeploymentChargeStart", mDeploymentChargeStart, _
                                                 "@DeploymentChargeEnd", mDeploymentChargeEnd, _
                                                 "@DeploymentAmount", mDeploymentAmount, _
                                                 "@DeploymentCurrency", mDeploymentCurrency, _
                                                 "@DeploymentName", mDeploymentName, _
                                                 "@DeploymentCreatedDate", mDeploymentCreatedDate, _
                                                 "@DeploymentIdentifiers", mDeploymentIdentifiers, _
                                                 "@DeploymentConsumerReference", mDeploymentConsumerReference, _
                                                 "@DeploymentCustomerID", mDeploymentCustomerID, _
                                                 "@DeploymentCustomerName", mDeploymentCustomerName, _
                                                 "@SupplierName", mSupplierName, _
                                                 "@SupplierAddress1", mSupplierAddress1, _
                                                 "@SupplierAddress2", mSupplierAddress2, _
                                                 "@SupplierCity", mSupplierCity, _
                                                 "@SupplierCountryCode", mSupplierCountryCode, _
                                                 "@SupplierPostalOrZipCode", mSupplierPostalOrZipCode, _
                                                 "@SupplierTelephoneNumber", mSupplierTelephoneNumber, _
                                                 "@FacsimilieNumber", mFacsimilieNumber, _
                                                 "@SupplierConsumerID", mSupplierConsumerID, _
                                                 "@HotelCheckIn", mHotelCheckIn, _
                                                 "@HotelCheckOut", mHotelCheckOut, _
                                                 "@HotelRoomType", mHotelRoomType, _
                                                 "@HotelRateInformation", mHotelRateInformation, _
                                                 "@HotelCancellationPolicy", mHotelCancellationPolicy, _
                                                 "@HotelCancellationReference", mHotelCancellationReference, _
                                                 "@HotelPaymentRestrictions", mHotelPaymentRestrictions, _
                                                 "@HotelCareOfName", mHotelCareOfName, _
                                                 "@HotelContactName", mHotelContactName, _
                                                 "@HotelComments", mHotelComments, _
                                                 "@HotelBookingPlatformName", mHotelBookingPlatformName, _
                                                 "@Travellers", mTravellers))
            Return intret
        End Using
    End Function

    'R2.11 SA 
    Public Function check(ByVal pBookingID As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("TransactionData_check",
                                                                               "@BookingID", pBookingID))
            Return intRet
        End Using
    End Function

End Class
