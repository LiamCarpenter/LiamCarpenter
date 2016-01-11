Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class LV

    Public Sub New( _
        ByVal pInvoicenumber As String, _
        ByVal pInvoiceamount As Nullable(Of Double), _
        ByVal pInvoicedate As Nullable(Of DateTime), _
        ByVal pInvoicedescription As String, _
        ByVal pSuppliernumber As String, _
        ByVal pSuppliersitecode As String, _
        ByVal pLinenumber As Nullable(Of Integer), _
        ByVal pExpense As Nullable(Of Double), _
        ByVal pExpensevat As Nullable(Of Double), _
        ByVal pOthercharge As Nullable(Of Double), _
        ByVal pOthervat As Nullable(Of Double), _
        ByVal pServicecharge As Nullable(Of Double), _
        ByVal pAirlinecharge As Nullable(Of Double), _
        ByVal pLinedescription As String, _
        ByVal pCompany As String, _
        ByVal pCostcentre As String, _
        ByVal pAccount As String, _
        ByVal pProduct As String, _
        ByVal pProjectcode As String, _
        ByVal pAttributecategory As String, _
        ByVal pAttribute1 As String, _
        ByVal pAttribute2 As String, _
        ByVal pAttribute3 As String, _
        ByVal pAttribute4 As String, _
        ByVal pAttribute5 As String, _
        ByVal pAttribute6 As String, _
        ByVal pAttribute7 As String)
        mInvoicenumber = pInvoicenumber
        mInvoiceamount = pInvoiceamount
        mInvoicedate = pInvoicedate
        mInvoicedescription = pInvoicedescription
        mSuppliernumber = pSuppliernumber
        mSuppliersitecode = pSuppliersitecode
        mLinenumber = pLinenumber
        mExpense = pExpense
        mExpensevat = pExpensevat
        mOthercharge = pOthercharge
        mOthervat = pOthervat
        mServicecharge = pServicecharge
        mAirlinecharge = pAirlinecharge
        mLinedescription = pLinedescription
        mCompany = pCompany
        mCostcentre = pCostcentre
        mAccount = pAccount
        mProduct = pProduct
        mProjectcode = pProjectcode
        mAttributecategory = pAttributecategory
        mAttribute1 = pAttribute1
        mAttribute2 = pAttribute2
        mAttribute3 = pAttribute3
        mAttribute4 = pAttribute4
        mAttribute5 = pAttribute5
        mAttribute6 = pAttribute6
        mAttribute7 = pAttribute7
    End Sub

    Public Sub New(ByVal pInvoicenumber As String)
        mInvoicenumber = pInvoicenumber
    End Sub

    Public Sub New( _
)
    End Sub

    Private mInvoicenumber As String
    Private mInvoiceamount As Nullable(Of Double)
    Private mInvoicedate As Nullable(Of DateTime)
    Private mInvoicedescription As String
    Private mSuppliernumber As String
    Private mSuppliersitecode As String
    Private mLinenumber As Nullable(Of Integer)
    Private mExpense As Nullable(Of Double)
    Private mExpensevat As Nullable(Of Double)
    Private mOthercharge As Nullable(Of Double)
    Private mOthervat As Nullable(Of Double)
    Private mServicecharge As Nullable(Of Double)
    Private mAirlinecharge As Nullable(Of Double)
    Private mLinedescription As String
    Private mCompany As String
    Private mCostcentre As String
    Private mAccount As String
    Private mProduct As String
    Private mProjectcode As String
    Private mAttributecategory As String
    Private mAttribute1 As String
    Private mAttribute2 As String
    Private mAttribute3 As String
    Private mAttribute4 As String
    Private mAttribute5 As String
    Private mAttribute6 As String
    Private mAttribute7 As String

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

    Public Property Invoicedescription() As String
        Get
            Return mInvoicedescription
        End Get
        Set(ByVal value As String)
            mInvoicedescription = value
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

    Public Property Suppliersitecode() As String
        Get
            Return mSuppliersitecode
        End Get
        Set(ByVal value As String)
            mSuppliersitecode = value
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

    Public Property Company() As String
        Get
            Return mCompany
        End Get
        Set(ByVal value As String)
            mCompany = value
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

    Public Property Account() As String
        Get
            Return mAccount
        End Get
        Set(ByVal value As String)
            mAccount = value
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

    Public Property Projectcode() As String
        Get
            Return mProjectcode
        End Get
        Set(ByVal value As String)
            mProjectcode = value
        End Set
    End Property

    Public Property Attributecategory() As String
        Get
            Return mAttributecategory
        End Get
        Set(ByVal value As String)
            mAttributecategory = value
        End Set
    End Property

    Public Property Attribute1() As String
        Get
            Return mAttribute1
        End Get
        Set(ByVal value As String)
            mAttribute1 = value
        End Set
    End Property

    Public Property Attribute2() As String
        Get
            Return mAttribute2
        End Get
        Set(ByVal value As String)
            mAttribute2 = value
        End Set
    End Property

    Public Property Attribute3() As String
        Get
            Return mAttribute3
        End Get
        Set(ByVal value As String)
            mAttribute3 = value
        End Set
    End Property

    Public Property Attribute4() As String
        Get
            Return mAttribute4
        End Get
        Set(ByVal value As String)
            mAttribute4 = value
        End Set
    End Property

    Public Property Attribute5() As String
        Get
            Return mAttribute5
        End Get
        Set(ByVal value As String)
            mAttribute5 = value
        End Set
    End Property

    Public Property Attribute6() As String
        Get
            Return mAttribute6
        End Get
        Set(ByVal value As String)
            mAttribute6 = value
        End Set
    End Property

    Public Property Attribute7() As String
        Get
            Return mAttribute7
        End Get
        Set(ByVal value As String)
            mAttribute7 = value
        End Set
    End Property

    Private Shared Function makeLVFromRow( _
            ByVal r As IDataReader _
        ) As LV
        Return New LV( _
                toStr(r.Item("invoicenumber")), _
                toNullableFloat(r.Item("invoiceamount")), _
                toNullableDate(r.Item("invoicedate")), _
                toStr(r.Item("invoicedescription")), _
                toStr(r.Item("suppliernumber")), _
                toStr(r.Item("suppliersitecode")), _
                toNullableInteger(r.Item("linenumber")), _
                toNullableFloat(r.Item("expense")), _
                toNullableFloat(r.Item("expensevat")), _
                toNullableFloat(r.Item("othercharge")), _
                toNullableFloat(r.Item("othervat")), _
                toNullableFloat(r.Item("servicecharge")), _
                toNullableFloat(r.Item("airlinecharge")), _
                toStr(r.Item("linedescription")), _
                toStr(r.Item("company")), _
                toStr(r.Item("costcentre")), _
                toStr(r.Item("account")), _
                toStr(r.Item("product")), _
                toStr(r.Item("projectcode")), _
                toStr(r.Item("attributecategory")), _
                toStr(r.Item("attribute1")), _
                toStr(r.Item("attribute2")), _
                toStr(r.Item("attribute3")), _
                toStr(r.Item("attribute4")), _
                toStr(r.Item("attribute5")), _
                toStr(r.Item("attribute6")), _
                toStr(r.Item("attribute7")))
    End Function

    Private Shared Function makeLVCheckerFromRow( _
            ByVal r As IDataReader _
        ) As LV
        Return New LV(toStr(r.Item("invoicenumber")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String, ByVal pextradate As String) As List(Of LV)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of LV)()
            Using r As IDataReader = dbh.callSP("FeederFile_LV", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate, _
                                                "@extradate", pextradate)
                While r.Read()
                    ret.Add(makeLVFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function checker(ByVal pstartdate As String, ByVal penddate As String, ByVal pextradate As String) As List(Of LV)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of LV)()
            Using r As IDataReader = dbh.callSP("FeederFileChecker_LV", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate, _
                                                "@extradate", pextradate)
                While r.Read()
                    ret.Add(makeLVCheckerFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function
End Class
