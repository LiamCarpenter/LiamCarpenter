Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class NCAS

    Public Sub New( _
        ByVal pMemo As String, _
        ByVal pSupplierName As String, _
        ByVal pTrxNum As String, _
        ByVal pHeaderDesc As String, _
        ByVal pVsrRef As String, _
        ByVal pTrxDate As String, _
        ByVal pReceivedDate As String, _
        ByVal pCostCentre As String, _
        ByVal pSubjective As String, _
        ByVal pAnalysis1 As String, _
        ByVal pAnalysis2 As String, _
        ByVal pType As String, _
        ByVal pTax As String, _
        ByVal pLineDesc As String, _
        ByVal pLineAmount As Double, _
        ByVal pUnitAmount As Double, _
        ByVal pCreditNote As Boolean, _
        ByVal pInvoiceRef As String, _
        ByVal pLineDisVat As Double, _
        ByVal pLineVat As Double)
        mMemo = pMemo
        mSupplierName = pSupplierName
        mTrxNum = pTrxNum
        mHeaderDesc = pHeaderDesc
        mVsrRef = pVsrRef
        mTrxDate = pTrxDate
        mReceivedDate = pReceivedDate
        mCostCentre = pCostCentre
        mSubjective = pSubjective
        mAnalysis1 = pAnalysis1
        mAnalysis2 = pAnalysis2
        mType = pType
        mTax = pTax
        mLineDesc = pLineDesc
        mLineAmount = pLineAmount
        mUnitAmount = pUnitAmount
        mCreditNote = pCreditNote
        mInvoiceRef = pInvoiceRef
        mLineDisVat = pLineDisVat
        mLineVat = pLineVat
    End Sub

    Public Sub New( _
)
    End Sub

    Private mMemo As String
    Private mSupplierName As String
    Private mTrxNum As String
    Private mHeaderDesc As String
    Private mVsrRef As String
    Private mTrxDate As String
    Private mReceivedDate As String
    Private mCostCentre As String
    Private mSubjective As String
    Private mAnalysis1 As String
    Private mAnalysis2 As String
    Private mType As String
    Private mTax As String
    Private mLineDesc As String
    Private mLineAmount As Double
    Private mUnitAmount As Double
    Private mCreditNote As Boolean
    Private mInvoiceRef As String
    Private mLineDisVat As Double
    Private mLineVat As Double

    Public Property Memo As String
        Get
            Return mMemo
        End Get
        Set(value As String)
            mMemo = value
        End Set
    End Property

    Public Property SupplierName As String
        Get
            Return mSupplierName
        End Get
        Set(value As String)
            mSupplierName = value
        End Set
    End Property

    Public Property TrxNum As String
        Get
            Return mTrxNum
        End Get
        Set(value As String)
            mTrxNum = value
        End Set
    End Property

    Public Property HeaderDescription As String
        Get
            Return mHeaderDesc
        End Get
        Set(value As String)
            mHeaderDesc = value
        End Set
    End Property

    Public Property VsrRef As String
        Get
            Return mVsrRef
        End Get
        Set(value As String)
            mVsrRef = value
        End Set
    End Property

    Public Property TrxDate As String
        Get
            Return mTrxDate
        End Get
        Set(value As String)
            mTrxDate = value
        End Set
    End Property

    Public Property ReceivedDate As String
        Get
            Return mReceivedDate
        End Get
        Set(value As String)
            mReceivedDate = value
        End Set
    End Property

    Public Property CostCentre As String
        Get
            Return mCostCentre
        End Get
        Set(value As String)
            mCostCentre = value
        End Set
    End Property

    Public Property Subjective As String
        Get
            Return mSubjective
        End Get
        Set(value As String)
            mSubjective = value
        End Set
    End Property

    Public Property Analysis1 As String
        Get
            Return mAnalysis1
        End Get
        Set(value As String)
            mAnalysis1 = value
        End Set
    End Property

    Public Property Analysis2 As String
        Get
            Return mAnalysis2
        End Get
        Set(value As String)
            mAnalysis2 = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return mType
        End Get
        Set(value As String)
            mType = value
        End Set
    End Property

    Public Property Tax As String
        Get
            Return mTax
        End Get
        Set(value As String)
            mTax = value
        End Set
    End Property

    Public Property LineDescription As String
        Get
            Return mLineDesc
        End Get
        Set(value As String)
            mLineDesc = value
        End Set
    End Property

    Public Property LineAmount As Double
        Get
            Return mLineAmount
        End Get
        Set(value As Double)
            mLineAmount = value
        End Set
    End Property

    Public Property UnitAmount As Double
        Get
            Return mUnitAmount
        End Get
        Set(value As Double)
            mUnitAmount = value
        End Set
    End Property

    Public Property CreditNote As Boolean
        Get
            Return mCreditNote
        End Get
        Set(value As Boolean)
            mCreditNote = value
        End Set
    End Property

    Public Property InvoiceRef As String
        Get
            Return mInvoiceRef
        End Get
        Set(value As String)
            mInvoiceRef = value
        End Set
    End Property

    Public Property LineDisVat As Double
        Get
            Return mLineDisVat
        End Get
        Set(value As Double)
            mLineDisVat = value
        End Set
    End Property

    Public Property LineVat As Double
        Get
            Return mLineVat
        End Get
        Set(value As Double)
            mLineVat = value
        End Set
    End Property

    Private Shared Function makeNcasFromRow( _
            ByVal r As IDataReader _
        ) As NCAS
        Return New NCAS( _
                clsNYS.notString(r.Item("Memo")), _
                clsNYS.notString(r.Item("SupplierName")), _
                clsNYS.notString(r.Item("TrxNum")), _
                clsNYS.notString(r.Item("HeaderDesc")), _
                clsNYS.notString(r.Item("VsrRef")), _
                clsNYS.notString(r.Item("TrxDate")), _
                clsNYS.notString(r.Item("ReceivedDate")), _
                clsNYS.notString(r.Item("CostCentre")), _
                clsNYS.notString(r.Item("Subjective")), _
                clsNYS.notString(r.Item("Analysis1")), _
                clsNYS.notString(r.Item("Analysis2")), _
                clsNYS.notString(r.Item("Type")), _
                clsNYS.notString(r.Item("Tax")), _
                clsNYS.notString(r.Item("Description")), _
                clsNYS.notNumber(r.Item("LineAmount")), _
                clsNYS.notNumber(r.Item("UnitAmount")), _
                clsNYS.notBoolean(r.Item("CreditNote")), _
                clsNYS.notString(r.Item("InvoiceRef")), _
                clsNYS.notNumber(r.Item("LineDisVat")), _
                clsNYS.notNumber(r.Item("LineVat")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String) As List(Of NCAS)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of NCAS)()
            Using r As IDataReader = dbh.callSP("FeederFile_NCAS", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeNcasFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
