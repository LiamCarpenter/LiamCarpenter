Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class RCN
    'R2.21 SA - added pVat, pDisVat, pTax
    Public Sub New( _
        ByVal pInvoiceNo As String, _
        ByVal pBookerName As String, _
        ByVal pDirectDebitDate As String, _
        ByVal pTravelDate As String, _
        ByVal pReason As String, _
        ByVal pProjectCode As String, _
        ByVal pResourceCode As String, _
        ByVal pLocation As String, _
        ByVal pExpense As Decimal, _
        ByVal pExpenseVat As Decimal, _
        ByVal pOurCharge As Decimal, _
        ByVal pOurVat As Decimal, _
        ByVal pProduct As String, _
        ByVal pTravellerFirstname As String, _
        ByVal pTravellerLastname As String, _
        ByVal pProductType As String, _
        ByVal pDiscount As Decimal, _
        ByVal pSupplierID As String, _
        ByVal pIsSupplierInterantional As Boolean, _
        ByVal pBookerEmail As String, _
        ByVal pSupplierPostcode1 As String, _
        ByVal pLineNotes As String, _
        ByVal pVat As Decimal, _
        ByVal pDispVat As Decimal, _
        ByVal pTax As Decimal)
        mInvoiceNo = pInvoiceNo
        mBookerName = pBookerName
        mDirectDebitDate = pDirectDebitDate
        mTravelDate = pTravelDate
        mReason = pReason
        mProjectCode = pProjectCode
        mResourceCode = pResourceCode
        mLocation = pLocation
        mExpense = pExpense
        mExpenseVat = pExpenseVat
        'mDispVat = pDispVat
        mOurCharge = pOurCharge
        mOurVat = pOurVat
        mProduct = pProduct
        mTravellerFirstname = pTravellerFirstname
        mTravellerLastname = pTravellerLastname
        mProductType = pProductType
        mDiscount = pDiscount
        mIsSupplierInterantional = pIsSupplierInterantional

        'R2.16 CR
        mBookerEmail = pBookerEmail

        'R2.17 CR
        mSupplierPostcode1 = pSupplierPostcode1
        mLineNotes = pLineNotes

        'R2.21 SA
        mVat = pVat
        mDispVat = pDispVat
        mTax = pTax

        'R2.22 CR
        mSupplierID = pSupplierID
    End Sub

    'R2.23.2 SA - added rail charges
    'R2.21 SA
    Public Sub New( _
    ByVal pInvoiceNo As String, _
    ByVal pBookerName As String, _
    ByVal pDirectDebitDate As String, _
    ByVal pTravelDate As String, _
    ByVal pReason As String, _
    ByVal pProjectCode As String, _
    ByVal pResourceCode As String, _
    ByVal pLocation As String, _
    ByVal pExpense As Decimal, _
    ByVal pExpenseVat As Decimal, _
    ByVal pProduct As String, _
    ByVal pTravellerFirstname As String, _
    ByVal pTravellerLastname As String, _
    ByVal pProductType As String, _
    ByVal pSupplierID As String, _
    ByVal pIsSupplierInterantional As Boolean, _
    ByVal pBookerEmail As String, _
    ByVal pSupplierPostcode1 As String, _
    ByVal pLineNotes As String, _
    ByVal pIdentifier As String, _
    ByVal pVat As Decimal, _
    ByVal pDispVat As Decimal, _
    ByVal pTax As Decimal, _
    ByVal pRailCharges As Decimal)
        mInvoiceNo = pInvoiceNo
        mBookerName = pBookerName
        mDirectDebitDate = pDirectDebitDate
        mTravelDate = pTravelDate
        mReason = pReason
        mProjectCode = pProjectCode
        mResourceCode = pResourceCode
        mLocation = pLocation
        mExpense = pExpense
        mExpenseVat = pExpenseVat
        mProduct = pProduct
        mTravellerFirstname = pTravellerFirstname
        mTravellerLastname = pTravellerLastname
        mProductType = pProductType
        mIsSupplierInterantional = pIsSupplierInterantional
        mBookerEmail = pBookerEmail
        mSupplierPostcode1 = pSupplierPostcode1
        mLineNotes = pLineNotes
        mIdentifier = pIdentifier
        mVat = pVat
        mDispVat = pDispVat
        mTax = pTax
        mSupplierID = pSupplierID
        'R2.23.2 SA 
        mRailCharges = pRailCharges

    End Sub

    Public Sub New( _
)
    End Sub

    Private mInvoiceNo As String
    Private mBookerName As String
    Private mDirectDebitDate As String
    Private mTravelDate As String
    Private mReason As String
    Private mProjectCode As String
    Private mResourceCode As String
    Private mLocation As String
    Private mExpense As Decimal
    Private mExpenseVat As Decimal
    Private mOurCharge As Decimal
    Private mOurVat As Decimal
    Private mProduct As String
    Private mTravellerFirstname As String
    Private mTravellerLastname As String
    Private mProductType As String
    Private mDiscount As Decimal
    Private mIsSupplierInterantional As Boolean

    'R2.16 CR
    Private mBookerEmail As String

    'R2.17 CR
    Private mSupplierPostcode1 As String
    Private mLineNotes As String

    'R2.21 SA
    Private mIdentifier As String
    Private mVat As Decimal
    Private mDispVat As Decimal
    Private mTax As Decimal
    'R2.22 CR
    Private mSupplierID As String
    'R2.23.2 SA 
    Private mRailCharges As Decimal

    'R2.21 SA
    Public Property Identifier() As String
        Get
            Return mIdentifier
        End Get
        Set(ByVal value As String)
            mIdentifier = value
        End Set
    End Property

    'R2.21 SA
    Public Property Vat() As Decimal
        Get
            Return mVat
        End Get
        Set(ByVal value As Decimal)
            mVat = value
        End Set
    End Property

    'R2.21 SA
    Public Property DispVat() As Decimal
        Get
            Return mDispVat
        End Get
        Set(ByVal value As Decimal)
            mDispVat = value
        End Set
    End Property

    'R2.21 SA
    Public Property Tax() As Decimal
        Get
            Return mTax
        End Get
        Set(ByVal value As Decimal)
            mTax = value
        End Set
    End Property

    Public Property InvoiceNo() As String
        Get
            Return mInvoiceNo
        End Get
        Set(ByVal value As String)
            mInvoiceNo = value
        End Set
    End Property

    Public Property BookerName() As String
        Get
            Return mBookerName
        End Get
        Set(ByVal value As String)
            mBookerName = value
        End Set
    End Property

    Public Property DirectDebitDate() As String
        Get
            Return mDirectDebitDate
        End Get
        Set(ByVal value As String)
            mDirectDebitDate = value
        End Set
    End Property

    Public Property TravelDate() As String
        Get
            Return mTravelDate
        End Get
        Set(ByVal value As String)
            mTravelDate = value
        End Set
    End Property

    Public Property Reason() As String
        Get
            Return mReason
        End Get
        Set(ByVal value As String)
            mReason = value
        End Set
    End Property

    Public Property ProjectCode() As String
        Get
            Return mProjectCode
        End Get
        Set(ByVal value As String)
            mProjectCode = value
        End Set
    End Property

    Public Property ResourceCode() As String
        Get
            Return mResourceCode
        End Get
        Set(ByVal value As String)
            mResourceCode = value
        End Set
    End Property

    Public Property Location() As String
        Get
            Return mLocation
        End Get
        Set(ByVal value As String)
            mLocation = value
        End Set
    End Property

    Public Property Expense As Decimal
        Get
            Return mExpense
        End Get
        Set(value As Decimal)
            mExpense = value
        End Set
    End Property

    Public Property ExpenseVat() As Decimal
        Get
            Return mExpenseVat
        End Get
        Set(ByVal value As Decimal)
            mExpenseVat = value
        End Set
    End Property



    Public Property OurCharge As Decimal
        Get
            Return mOurCharge
        End Get
        Set(value As Decimal)
            mOurCharge = value
        End Set
    End Property

    Public Property OurVat As Decimal
        Get
            Return mOurVat
        End Get
        Set(ByVal value As Decimal)
            mOurVat = value
        End Set
    End Property

    Public Property Product As String
        Get
            Return mProduct
        End Get
        Set(value As String)
            mProduct = value
        End Set
    End Property

    Public Property TravellerFirstname As String
        Get
            Return mTravellerFirstname
        End Get
        Set(value As String)
            mTravellerFirstname = value
        End Set
    End Property

    Public Property TravellerLastname As String
        Get
            Return mTravellerLastname
        End Get
        Set(value As String)
            mTravellerLastname = value
        End Set
    End Property

    Public Property ProductType As String
        Get
            Return mProductType
        End Get
        Set(value As String)
            mProductType = value
        End Set
    End Property

    Public Property Discount() As Decimal
        Get
            Return mDiscount
        End Get
        Set(ByVal value As Decimal)
            mDiscount = value
        End Set
    End Property

    Public Property IsSupplierInternational() As Boolean
        Get
            Return mIsSupplierInterantional
        End Get
        Set(ByVal value As Boolean)
            mIsSupplierInterantional = value
        End Set
    End Property

    'R2.16 CR
    Public Property BookerEmail As String
        Get
            Return mBookerEmail
        End Get
        Set(value As String)
            mBookerEmail = value
        End Set
    End Property

    'R2.17 CR
    Public Property SupplierPostcode1 As String
        Get
            Return mSupplierPostcode1
        End Get
        Set(value As String)
            mSupplierPostcode1 = value
        End Set
    End Property

    'R2.17 CR
    Public Property LineNotes As String
        Get
            Return mLineNotes
        End Get
        Set(value As String)
            mLineNotes = value
        End Set
    End Property

    'R2.22 CR
    Public Property SupplierID As String
        Get
            Return mSupplierID
        End Get
        Set(value As String)
            mSupplierID = value
        End Set
    End Property

    'R2.23.2 SA 
    Public Property RailCharges As Decimal
        Get
            Return mRailCharges
        End Get
        Set(ByVal value As Decimal)
            mRailCharges = value
        End Set
    End Property

    Private Shared Function makeRCNFromRow( _
            ByVal r As IDataReader _
        ) As RCN
        Return New RCN( _
                clsNYS.notString(r.Item("InvoiceNo")), _
                clsNYS.notString(r.Item("BookerName")), _
                clsNYS.notString(r.Item("DDDate")), _
                clsNYS.notString(r.Item("TravelDate")), _
                clsNYS.notString(r.Item("Reason")), _
                clsNYS.notString(r.Item("ProjectCode")), _
                clsNYS.notString(r.Item("ResourceCode")), _
                clsNYS.notString(r.Item("Location")), _
                clsNYS.notDecimal(r.Item("Expense")), _
                clsNYS.notDecimal(r.Item("ExpenseVat")), _
                clsNYS.notDecimal(r.Item("OurCharge")), _
                clsNYS.notDecimal(r.Item("OurVat")), _
                clsNYS.notString(r.Item("Product")), _
                clsNYS.notString(r.Item("TravellerFirstname")), _
                clsNYS.notString(r.Item("TravellerLastname")), _
                clsNYS.notString(r.Item("ProductType")), _
                clsNYS.notDecimal(r.Item("Discount")), _
                clsNYS.notString(r.Item("inm_suppid")), _
                clsNYS.notBoolean(r.Item("IsSupplierInternational")), _
                clsNYS.notString(r.Item("BookerEmail")), _
                clsNYS.notString(r.Item("SupplierPostcode1")), _
                clsNYS.notString(r.Item("LineNotes")), _
                clsNYS.notDecimal(r.Item("Vat")), _
                clsNYS.notDecimal(r.Item("dispVat")), _
                clsNYS.notDecimal(r.Item("Tax")))
    End Function

    'R2.21 SA
    Private Shared Function makeRCNConfFromRow( _
        ByVal r As IDataReader _
    ) As RCN
        Return New RCN( _
                clsNYS.notString(r.Item("InvoiceNo")), _
                clsNYS.notString(r.Item("BookerName")), _
                clsNYS.notString(r.Item("DDDate")), _
                clsNYS.notString(r.Item("TravelDate")), _
                clsNYS.notString(r.Item("Reason")), _
                clsNYS.notString(r.Item("ProjectCode")), _
                clsNYS.notString(r.Item("ResourceCode")), _
                clsNYS.notString(r.Item("Location")), _
                clsNYS.notDecimal(r.Item("Expense")), _
                clsNYS.notDecimal(r.Item("ExpenseVat")), _
                clsNYS.notString(r.Item("Product")), _
                clsNYS.notString(r.Item("TravellerFirstname")), _
                clsNYS.notString(r.Item("TravellerLastname")), _
                clsNYS.notString(r.Item("ProductType")), _
                clsNYS.notString(r.Item("inm_suppid")), _
                clsNYS.notBoolean(r.Item("IsSupplierInternational")), _
                clsNYS.notString(r.Item("BookerEmail")), _
                clsNYS.notString(r.Item("SupplierPostcode1")), _
                clsNYS.notString(r.Item("LineNotes")), _
                clsNYS.notString(r.Item("Identifier")), _
                clsNYS.notDecimal(r.Item("Vat")), _
                clsNYS.notDecimal(r.Item("dispVat")), _
                clsNYS.notDecimal(r.Item("Tax")), _
                clsNYS.notDecimal(r.Item("RailCharges")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String) As List(Of RCN)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RCN)()
            Using r As IDataReader = dbh.callSP("FeederFile_RCN", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeRCNFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function listConf(ByVal pstartdate As String, ByVal penddate As String) As List(Of RCN)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RCN)()
            Using r As IDataReader = dbh.callSP("FeederFile_RCNCONF", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeRCNConfFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    'R2.23.1 AI
    Public Shared Function GetRCNShortenedName(ByVal pShortName As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
            Dim strRet As String = clsNYS.notString(dbh.callSPSingleValueCanReturnNothing("RCNShortenedName_get", _
                                                                                   "@shortname", pShortName))
            Return strRet
        End Using
    End Function

    'R2.?? SA 
    Public Shared Function getRailCharges(ByVal pStartdate As String, ByVal pEnddate As String) As Double
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Double = dbh.callSPSingleValueCanReturnNothing("FeederFile_RCNGetRailCharges", _
                                                                       "@startdate", pStartdate, _
                                                                       "@enddate", pEnddate)
            Return ret
        End Using
    End Function

    'R2.23 SA 
    Public Shared Function getConfRailCharges(ByVal pStartdate As String, ByVal pEnddate As String) As Double
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Double = dbh.callSPSingleValueCanReturnNothing("FeederFile_RCNCONFGetRailCharges", _
                                                                       "@startdate", pStartdate, _
                                                                       "@enddate", pEnddate)
            Return ret
        End Using
    End Function

End Class
