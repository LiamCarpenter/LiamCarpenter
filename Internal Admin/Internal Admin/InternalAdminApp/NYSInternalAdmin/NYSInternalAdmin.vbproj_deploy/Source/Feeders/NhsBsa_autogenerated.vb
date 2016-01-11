Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
Public Class NhsBsa
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
            ByVal pTotAmount As Nullable(Of Double))
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
    End Sub

    Public Sub New( _
        ByVal pInvoicenumber As String)
        mInvoicenumber = pInvoicenumber
    End Sub

    Public Sub New( _
)
    End Sub

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

    Private Shared Function makeNhsBsaFromRow( _
            ByVal r As IDataReader _
        ) As NhsBsa
        Return New NhsBsa( _
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
                toNullableFloat(r.Item("tot_amount")))
    End Function

    Private Shared Function makeNhsBsaFromRowIndividual( _
            ByVal r As IDataReader _
        ) As NhsBsa
        Return New NhsBsa( _
                toStr(r.Item("invoicenumber")))
    End Function

    Public Shared Function listIndividual(ByVal pstartdate As String, ByVal penddate As String) As List(Of NhsBsa)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of NhsBsa)()
            Using r As IDataReader = dbh.callSP("FeederFile_NHSBSAFirstRun", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeNhsBsaFromRowIndividual(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function getDetails(ByVal pInvno As String) As List(Of NhsBsa)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of NhsBsa)()
            Using r As IDataReader = dbh.callSP("FeederFile_NHSBSAIndividual", _
                                                "@invno", pInvno)
                While r.Read()
                    ret.Add(makeNhsBsaFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String) As List(Of NhsBsa)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of NhsBsa)()
            Using r As IDataReader = dbh.callSP("FeederFile_NHSBSA", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeNhsBsaFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function
End Class
