Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class DWP

    Public Sub New( _
        ByVal pInvoicenumber As String, _
        ByVal pInvoiceamount As String, _
        ByVal pInvoicedate As String, _
        ByVal pSuppliernumber As String, _
        ByVal pSuppliersitecode As String, _
        ByVal pLinenumber As String, _
        ByVal pExpense As String, _
        ByVal pExpensevat As String, _
        ByVal pOthercharge As String, _
        ByVal pOthervat As String, _
        ByVal pServicecharge As String, _
        ByVal pAirlinecharge As String, _
        ByVal pLinedescription As String, _
        ByVal pCompany As String, _
        ByVal pTot_fileno As String, _
        ByVal pInm_supnam As String, _
        ByVal pSup_add3 As String, _
        ByVal pInm_start As String, _
        ByVal pInm_ldname As String, _
        ByVal pTot_cref2 As String, _
        ByVal pTot_cref1 As String, _
        ByVal pMeetingcode As String, _
        ByVal pTot_crsref As String, _
        ByVal pInm_discnt As String, _
        ByVal pTot_discnt As Double, _
        ByVal pInm_end As String)
        mInvoicenumber = pInvoicenumber
        mInvoiceamount = pInvoiceamount
        mInvoicedate = pInvoicedate
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
        mTot_fileno = pTot_fileno
        mInm_supnam = pInm_supnam
        mSup_add3 = pSup_add3
        mInm_start = pInm_start
        mInm_ldname = pInm_ldname
        mTot_cref2 = pTot_cref2
        mTot_cref1 = pTot_cref1
        mMeetingcode = pMeetingcode
        mTot_crsref = pTot_crsref
        mInm_discnt = pInm_discnt
        mTot_discnt = pTot_discnt
        mInm_end = pInm_end
    End Sub

    Public Sub New(ByVal pInvoicenumber As String)
        mInvoicenumber = pInvoicenumber
    End Sub

    Public Sub New(ByVal pInvoicenumber As String, _
                   ByVal pResult As String)
        mInvoicenumber = pInvoicenumber
        mResult = pResult
    End Sub

    Public Sub New( _
)
    End Sub

    Private mResult As String
    Private mInvoicenumber As String
    Private mInvoiceamount As String
    Private mInvoicedate As String
    Private mSuppliernumber As String
    Private mSuppliersitecode As String
    Private mLinenumber As String
    Private mExpense As String
    Private mExpensevat As String
    Private mOthercharge As String
    Private mOthervat As String
    Private mServicecharge As String
    Private mAirlinecharge As String
    Private mLinedescription As String
    Private mCompany As String
    Private mTot_fileno As String
    Private mInm_supnam As String
    Private mSup_add3 As String
    Private mInm_start As String
    Private mInm_ldname As String
    Private mTot_cref2 As String
    Private mTot_cref1 As String
    Private mMeetingcode As String
    Private mTot_crsref As String
    Private mInm_discnt As String
    Private mTot_discnt As Double
    Private mInm_end As String

    Public Property Result() As String
        Get
            Return mResult
        End Get
        Set(ByVal value As String)
            mResult = value
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

    Public Property Invoiceamount() As String
        Get
            Return mInvoiceamount
        End Get
        Set(ByVal value As String)
            mInvoiceamount = value
        End Set
    End Property

    Public Property Invoicedate() As String
        Get
            Return mInvoicedate
        End Get
        Set(ByVal value As String)
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

    Public Property Suppliersitecode() As String
        Get
            Return mSuppliersitecode
        End Get
        Set(ByVal value As String)
            mSuppliersitecode = value
        End Set
    End Property

    Public Property Linenumber() As String
        Get
            Return mLinenumber
        End Get
        Set(ByVal value As String)
            mLinenumber = value
        End Set
    End Property

    Public Property Expense() As String
        Get
            Return mExpense
        End Get
        Set(ByVal value As String)
            mExpense = value
        End Set
    End Property

    Public Property Expensevat() As String
        Get
            Return mExpensevat
        End Get
        Set(ByVal value As String)
            mExpensevat = value
        End Set
    End Property

    Public Property Othercharge() As String
        Get
            Return mOthercharge
        End Get
        Set(ByVal value As String)
            mOthercharge = value
        End Set
    End Property

    Public Property Othervat() As String
        Get
            Return mOthervat
        End Get
        Set(ByVal value As String)
            mOthervat = value
        End Set
    End Property

    Public Property Servicecharge() As String
        Get
            Return mServicecharge
        End Get
        Set(ByVal value As String)
            mServicecharge = value
        End Set
    End Property

    Public Property Airlinecharge() As String
        Get
            Return mAirlinecharge
        End Get
        Set(ByVal value As String)
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

    Public Property Tot_fileno() As String
        Get
            Return mTot_fileno
        End Get
        Set(ByVal value As String)
            mTot_fileno = value
        End Set
    End Property

    Public Property Inm_supnam() As String
        Get
            Return mInm_supnam
        End Get
        Set(ByVal value As String)
            mInm_supnam = value
        End Set
    End Property

    Public Property Sup_add3() As String
        Get
            Return mSup_add3
        End Get
        Set(ByVal value As String)
            mSup_add3 = value
        End Set
    End Property

    Public Property Inm_start() As String
        Get
            Return mInm_start
        End Get
        Set(ByVal value As String)
            mInm_start = value
        End Set
    End Property

    Public Property Inm_end() As String
        Get
            Return mInm_end
        End Get
        Set(ByVal value As String)
            mInm_end = value
        End Set
    End Property

    Public Property Inm_ldname() As String
        Get
            Return mInm_ldname
        End Get
        Set(ByVal value As String)
            mInm_ldname = value
        End Set
    End Property

    Public Property Tot_cref2() As String
        Get
            Return mTot_cref2
        End Get
        Set(ByVal value As String)
            mTot_cref2 = value
        End Set
    End Property

    Public Property Tot_cref1() As String
        Get
            Return mTot_cref1
        End Get
        Set(ByVal value As String)
            mTot_cref1 = value
        End Set
    End Property

    Public Property Meetingcode() As String
        Get
            Return mMeetingcode
        End Get
        Set(ByVal value As String)
            mMeetingcode = value
        End Set
    End Property

    Public Property Tot_crsref() As String
        Get
            Return mTot_crsref
        End Get
        Set(ByVal value As String)
            mTot_crsref = value
        End Set
    End Property

    Public Property Inm_discnt() As String
        Get
            Return mInm_discnt
        End Get
        Set(ByVal value As String)
            mInm_discnt = value
        End Set
    End Property

    Public Property Tot_discnt() As Double
        Get
            Return mTot_discnt
        End Get
        Set(ByVal value As Double)
            mTot_discnt = value
        End Set
    End Property

    Private Shared Function makeDWPFromRow( _
            ByVal r As IDataReader _
        ) As DWP
        Return New DWP( _
                clsNYS.notString(r.Item("invoicenumber")), _
                clsNYS.notString(r.Item("invoiceamount")), _
                clsNYS.notString(r.Item("invoicedate")), _
                clsNYS.notString(r.Item("suppliernumber")), _
                clsNYS.notString(r.Item("suppliersitecode")), _
                clsNYS.notString(r.Item("linenumber")), _
                clsNYS.notString(r.Item("expense")), _
                clsNYS.notString(r.Item("expensevat")), _
                clsNYS.notString(r.Item("othercharge")), _
                clsNYS.notString(r.Item("othervat")), _
                clsNYS.notString(r.Item("servicecharge")), _
                clsNYS.notString(r.Item("airlinecharge")), _
                clsNYS.notString(r.Item("linedescription")), _
                clsNYS.notString(r.Item("company")), _
                clsNYS.notString(r.Item("tot_fileno")), _
                clsNYS.notString(r.Item("inm_supnam")), _
                clsNYS.notString(r.Item("sup_add3")), _
                clsNYS.notString(r.Item("inm_start")), _
                clsNYS.notString(r.Item("inm_ldname")), _
                clsNYS.notString(r.Item("tot_cref2")), _
                clsNYS.notString(r.Item("tot_cref1")), _
                clsNYS.notString(r.Item("meetingcode")), _
                clsNYS.notString(r.Item("tot_crsref")), _
                clsNYS.notString(r.Item("inm_discnt")), _
                clsNYS.notNumber(r.Item("tot_discnt")), _
                clsNYS.notString(r.Item("inm_end")))
    End Function

    Private Shared Function makeDWPCheckerFromRow( _
            ByVal r As IDataReader _
        ) As DWP
        Return New DWP(clsNYS.notString(r.Item("invoicenumber")))
    End Function

    Private Shared Function makeDWPCheckerCCFromRow( _
            ByVal r As IDataReader _
        ) As DWP
        Return New DWP(clsNYS.notString(r.Item("invoicenumber")), _
                       clsNYS.notString(r.Item("result")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String, ByVal pType As String) As List(Of DWP)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of DWP)()
            Using r As IDataReader = dbh.callSP("FeederFile_DWP", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate, _
                                                "@type", pType)
                While r.Read()
                    ret.Add(makeDWPFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function checker(ByVal pstartdate As String, ByVal penddate As String, ByVal pType As String) As List(Of DWP)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of DWP)()
            Using r As IDataReader = dbh.callSP("FeederFileChecker_DWP", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate, _
                                                "@type", pType)
                While r.Read()
                    ret.Add(makeDWPCheckerFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function checkerCC(ByVal pstartdate As String, ByVal penddate As String, ByVal pType As String) As List(Of DWP)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of DWP)()
            Using r As IDataReader = dbh.callSP("FeederFileChecker_DWPCC", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate, _
                                                "@type", pType)
                While r.Read()
                    ret.Add(makeDWPCheckerCCFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
