Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

<Serializable()> _
Public Class O2
    Public Sub New( _
        ByVal pInvoicenumber As String, _
        ByVal pInvoiceamount As Nullable(Of Double), _
        ByVal pInvoicedate As Nullable(Of DateTime), _
        ByVal pSuppliernumber As String, _
        ByVal pLinenumber As Nullable(Of Integer), _
        ByVal pExpense As Nullable(Of Double), _
        ByVal pExpensevat As Nullable(Of Double), _
        ByVal pOthercharge As Nullable(Of Double), _
        ByVal pOthervat As Nullable(Of Double), _
        ByVal pServicecharge As Nullable(Of Double), _
        ByVal pAirlinecharge As Nullable(Of Double), _
        ByVal pLinedescription As String, _
        ByVal pProduct As String, _
        ByVal pPo As String, _
        ByVal pCostcentre As String, _
        ByVal pidentifier As String, _
        ByVal pLineDiscount As Nullable(Of Double), _
        ByVal pTotalDiscount As Nullable(Of Double), _
        ByVal pTotAmount As Nullable(Of Double), _
        ByVal pOrderDate As Date, _
        ByVal pOrderNumber As String, _
        ByVal pEnquiryID As Integer, _
        ByVal pBookerName As String)
        mInvoicenumber = pInvoicenumber
        mInvoiceamount = pInvoiceamount
        mInvoicedate = pInvoicedate
        mSuppliernumber = pSuppliernumber
        mLinenumber = pLinenumber
        mExpense = pExpense
        mExpensevat = pExpensevat
        mOthercharge = pOthercharge
        mOthervat = pOthervat
        mServicecharge = pServicecharge
        mAirlinecharge = pAirlinecharge
        mLinedescription = pLinedescription
        mProduct = pProduct
        mPo = pPo
        mCostcentre = pCostcentre
        midentifier = pidentifier
        mLineDiscount = pLineDiscount
        mTotalDiscount = pTotalDiscount
        mTotAmount = pTotAmount
        mOrderDate = pOrderDate
        mOrderNumber = pOrderNumber

        'R2.16 CR
        mEnquiryID = pEnquiryID
        mBookerName = pBookerName
    End Sub

    'Public Sub New( _
    '    ByVal pInvoicenumber As String, _
    '    ByVal pPoNo As String, _
    '    ByVal pOrderNumber As String, _
    '    ByVal pLinedescription As String)
    '    mInvoicenumber = pInvoicenumber
    '    mPo = pPoNo
    '    mOrderNumber = pOrderNumber
    '    mLinedescription = pLinedescription
    'End Sub

    Public Sub New( _
        ByVal pInvoicenumber As String, _
        ByVal pPo As String, _
        ByVal pOrderNumber As String)
        mInvoicenumber = pInvoicenumber
        mPo = pPo
        mOrderNumber = pOrderNumber
    End Sub

    Public Sub New( _
        ByVal pInvoicenumber As String, _
        ByVal pTotalNett As Decimal, _
        ByVal ptot_billed As Decimal, _
        ByVal ptot_bilvat As Decimal, _
        ByVal ptot_recvd As Decimal)
        mInvoicenumber = pInvoicenumber
        mTotalNett = pTotalNett
        mtot_billed = ptot_billed
        mtot_bilvat = ptot_bilvat
        mtot_recvd = ptot_recvd
    End Sub

    Public Sub New( _
        ByVal pInvoicenumber As String, _
        ByVal pPo As String, _
        ByVal pTotalNett As Decimal, _
        ByVal ptot_billed As Decimal, _
        ByVal pRequesterEmail As String, _
        ByVal pRef As String, _
        ByVal pStart As String, _
        ByVal pVenue As String)
        mInvoicenumber = pInvoicenumber
        mPo = pPo
        mTotalNett = pTotalNett
        mtot_billed = ptot_billed
        mRequesterEmail = pRequesterEmail
        mRef = pRef
        mStart = pStart
        mVenue = pVenue
    End Sub

    Public Sub New( _
)
    End Sub

    Private mRequesterEmail As String
    Private mRef As String
    Private mStart As String
    Private mVenue As String
    Private mtot_billed As Decimal
    Private mtot_bilvat As Decimal
    Private mtot_recvd As Decimal
    Private mTotalNett As Decimal
    Private mOrderNumber As String
    Private mInvoicenumber As String
    Private mInvoiceamount As Nullable(Of Double)
    Private mInvoicedate As Nullable(Of DateTime)
    Private mSuppliernumber As String
    Private mLinenumber As Nullable(Of Integer)
    Private mExpense As Nullable(Of Double)
    Private mExpensevat As Nullable(Of Double)
    Private mOthercharge As Nullable(Of Double)
    Private mOthervat As Nullable(Of Double)
    Private mServicecharge As Nullable(Of Double)
    Private mAirlinecharge As Nullable(Of Double)
    Private mLinedescription As String
    Private mProduct As String
    Private mPo As String
    Private mCostcentre As String
    Private midentifier As String
    Private mLineDiscount As Nullable(Of Double)
    Private mTotalDiscount As Nullable(Of Double)
    Private mTotAmount As Nullable(Of Double)
    Private mOrderDate As Date

    'R2.16 CR
    Private mEnquiryID As Integer
    Private mBookerName As String

    Public Property tot_billed() As Decimal
        Get
            Return mtot_billed
        End Get
        Set(ByVal value As Decimal)
            mtot_billed = value
        End Set
    End Property

    Public Property tot_bilvat() As Decimal
        Get
            Return mtot_bilvat
        End Get
        Set(ByVal value As Decimal)
            mtot_bilvat = value
        End Set
    End Property

    Public Property tot_recvd() As Decimal
        Get
            Return mtot_recvd
        End Get
        Set(ByVal value As Decimal)
            mtot_recvd = value
        End Set
    End Property

    Public Property TotalNett() As Decimal
        Get
            Return mTotalNett
        End Get
        Set(ByVal value As Decimal)
            mTotalNett = value
        End Set
    End Property

    Public Property RequesterEmail() As String
        Get
            Return mRequesterEmail
        End Get
        Set(ByVal value As String)
            mRequesterEmail = value
        End Set
    End Property

    Public Property Ref() As String
        Get
            Return mRef
        End Get
        Set(ByVal value As String)
            mRef = value
        End Set
    End Property

    Public Property Start() As String
        Get
            Return mStart
        End Get
        Set(ByVal value As String)
            mStart = value
        End Set
    End Property

    Public Property Venue() As String
        Get
            Return mVenue
        End Get
        Set(ByVal value As String)
            mVenue = value
        End Set
    End Property

    Public Property OrderNumber() As String
        Get
            Return mOrderNumber
        End Get
        Set(ByVal value As String)
            mOrderNumber = value
        End Set
    End Property

    Public Property LineDiscount() As Nullable(Of Double)
        Get
            Return mLineDiscount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mLineDiscount = value
        End Set
    End Property

    Public Property TotalDiscount() As Nullable(Of Double)
        Get
            Return mTotalDiscount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mTotalDiscount = value
        End Set
    End Property

    Public Property TotAmount() As Nullable(Of Double)
        Get
            Return mTotAmount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mTotAmount = value
        End Set
    End Property

    Public Property Invoicenumber() As String
        Get
            Return mInvoicenumber
        End Get
        Set(ByVal value As String)
            mInvoicenumber = value
        End Set
    End Property

    Public Property Invoiceamount() As Nullable(Of Double)
        Get
            Return mInvoiceamount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mInvoiceamount = value
        End Set
    End Property

    Public Property Invoicedate() As Nullable(Of DateTime)
        Get
            Return mInvoicedate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInvoicedate = value
        End Set
    End Property

    Public Property Suppliernumber() As String
        Get
            Return mSuppliernumber
        End Get
        Set(ByVal value As String)
            mSuppliernumber = value
        End Set
    End Property

    Public Property Linenumber() As Nullable(Of Integer)
        Get
            Return mLinenumber
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mLinenumber = value
        End Set
    End Property

    Public Property Expense() As Nullable(Of Double)
        Get
            Return mExpense
        End Get
        Set(ByVal value As Nullable(Of Double))
            mExpense = value
        End Set
    End Property

    Public Property Expensevat() As Nullable(Of Double)
        Get
            Return mExpensevat
        End Get
        Set(ByVal value As Nullable(Of Double))
            mExpensevat = value
        End Set
    End Property

    Public Property Othercharge() As Nullable(Of Double)
        Get
            Return mOthercharge
        End Get
        Set(ByVal value As Nullable(Of Double))
            mOthercharge = value
        End Set
    End Property

    Public Property Othervat() As Nullable(Of Double)
        Get
            Return mOthervat
        End Get
        Set(ByVal value As Nullable(Of Double))
            mOthervat = value
        End Set
    End Property

    Public Property Servicecharge() As Nullable(Of Double)
        Get
            Return mServicecharge
        End Get
        Set(ByVal value As Nullable(Of Double))
            mServicecharge = value
        End Set
    End Property

    Public Property Airlinecharge() As Nullable(Of Double)
        Get
            Return mAirlinecharge
        End Get
        Set(ByVal value As Nullable(Of Double))
            mAirlinecharge = value
        End Set
    End Property

    Public Property Linedescription() As String
        Get
            Return mLinedescription
        End Get
        Set(ByVal value As String)
            mLinedescription = value
        End Set
    End Property

    Public Property Product() As String
        Get
            Return mProduct
        End Get
        Set(ByVal value As String)
            mProduct = value
        End Set
    End Property

    Public Property Po() As String
        Get
            Return mPo
        End Get
        Set(ByVal value As String)
            mPo = value
        End Set
    End Property

    Public Property Costcentre() As String
        Get
            Return mCostcentre
        End Get
        Set(ByVal value As String)
            mCostcentre = value
        End Set
    End Property

    Public Property identifier() As String
        Get
            Return midentifier
        End Get
        Set(ByVal value As String)
            midentifier = value
        End Set
    End Property

    Public Property orderdate() As Date
        Get
            Return mOrderDate
        End Get
        Set(ByVal value As Date)
            mOrderDate = value
        End Set
    End Property

    'R2.16 CR
    Public Property EnquiryID As Integer
        Get
            Return mEnquiryID
        End Get
        Set(value As Integer)
            mEnquiryID = value
        End Set
    End Property

    'R2.16 CR
    Public Property BookerName As String
        Get
            Return mBookerName
        End Get
        Set(value As String)
            mBookerName = value
        End Set
    End Property

    Private Shared Function makeO2FromRow( _
               ByVal r As IDataReader _
           ) As O2
        Return New O2( _
                toStr(r.Item("invoicenumber")), _
                toNullableFloat(r.Item("invoiceamount")), _
                toNullableDate(r.Item("invoicedate")), _
                toStr(r.Item("suppliernumber")), _
                toNullableInteger(r.Item("linenumber")), _
                toNullableFloat(r.Item("expense")), _
                toNullableFloat(r.Item("expensevat")), _
                toNullableFloat(r.Item("othercharge")), _
                toNullableFloat(r.Item("othervat")), _
                toNullableFloat(r.Item("servicecharge")), _
                toNullableFloat(r.Item("airlinecharge")), _
                toStr(r.Item("linedescription")), _
                toStr(r.Item("product")), _
                toStr(r.Item("po")), _
                toStr(r.Item("costcentre")), _
                toStr(r.Item("identifier")), _
                toNullableFloat(r.Item("linediscount")), _
                toNullableFloat(r.Item("totaldiscount")), _
                toNullableFloat(r.Item("tot_amount")), _
                CDate(r.Item("orderdate")), _
                clsNYS.notString(r.Item("ordernumber")), _
                toNullableInteger(r.Item("enquiryID")), _
                toStr(r.Item("BookerName")))
    End Function

    'Private Shared Function makeO2FromRowIndividual1( _
    '       ByVal r As IDataReader _
    '   ) As O2
    '    Return New O2( _
    '            clsNYS.notString(r.Item("invoicenumber")), _
    '            clsNYS.notString(r.Item("inm_pono")), _
    '            clsNYS.notString(r.Item("ordernumber")), _
    '            clsNYS.notString(r.Item("linedescription")))
    'End Function

    Private Shared Function makeO2FromRowIndividual( _
           ByVal r As IDataReader _
       ) As O2
        Return New O2( _
                clsNYS.notString(r.Item("invoicenumber")), _
                clsNYS.notString(r.Item("inm_pono")), _
                clsNYS.notString(r.Item("ordernumber")))
    End Function

    Private Shared Function makeO2FromRowNett( _
           ByVal r As IDataReader _
       ) As O2
        Return New O2( _
                clsNYS.notString(r.Item("invoicenumber")), _
                CDec(r.Item("totalnett")), _
                CDec(r.Item("tot_billed")), _
                CDec(r.Item("tot_bilvat")), _
                CDec(r.Item("tot_recvd")))
    End Function

    Private Shared Function makeO2FromRowEmail( _
           ByVal r As IDataReader _
       ) As O2
        Return New O2( _
                clsNYS.notString(r.Item("Invoicenumber")), _
                clsNYS.notString(r.Item("Po")), _
                CDec(r.Item("TotalNett")), _
                CDec(r.Item("tot_billed")), _
                clsNYS.notString(r.Item("RequesterEmail")), _
                clsNYS.notString(r.Item("Ref")), _
                clsNYS.notString(r.Item("Start")), _
                clsNYS.notString(r.Item("Venue")))
    End Function

    Public Shared Function FeederFileO2Email() As List(Of O2)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2)()
            Using r As IDataReader = dbh.callSP("FeederFile_O2Email")
                While r.Read()
                    ret.Add(makeO2FromRowEmail(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    'Public Shared Function FeederFileO2FirstRun() As List(Of O2)
    '    Using dbh As New SqlDatabaseHandle(getConnection)
    '        Dim ret As New List(Of O2)()
    '        Using r As IDataReader = dbh.callSP("FeederFile_O2FirstRun")
    '            While r.Read()
    '                ret.Add(makeO2FromRowIndividual1(r))
    '            End While
    '        End Using
    '        Return ret
    '    End Using
    'End Function

    Public Shared Function FeederFileO2InvoiceList(ByVal pPOStartsWith As String) As List(Of O2)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2)()
            Using r As IDataReader = dbh.callSP("FeederFile_O2InvoiceList", "@po", pPOStartsWith)
                While r.Read()
                    ret.Add(makeO2FromRowIndividual(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function getDetails(ByVal pInvno As String) As List(Of O2)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2)()
            Using r As IDataReader = dbh.callSP("FeederFile_O2Individual", _
                                                "@invno", pInvno)
                While r.Read()
                    ret.Add(makeO2FromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    'CR
    Public Shared Function FeederFileO2PoList(ByVal pPONumber As String) As List(Of O2)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2)()
            Using r As IDataReader = dbh.callSP("FeederFile_O2PoList", _
                                                "@PoNo", pPONumber)
                While r.Read()
                    ret.Add(makeO2FromRowNett(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function list() As List(Of O2)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2)()
            Using r As IDataReader = dbh.callSP("FeederFile_O2")
                While r.Read()
                    ret.Add(makeO2FromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function O2VersionNoGet() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("O2Versionno_get"))
            Return ret
        End Using
    End Function
End Class
