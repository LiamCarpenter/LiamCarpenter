Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
Public Class ICE
    Public Sub New()
    End Sub

    'R2.21c SA - added pOurCharge, pdecCancellationCharge, pdecTotal
    Public Sub New(pstrNominalCode As String, _
                   pstrProjectCode As String, _
                   pstrCostCentre As String, _
                   pstrDescription As String, _
                   pdecDr As Decimal, _
                   pdecCr As Decimal, _
                   pdecVat As Decimal, _
                   pdecDisVat As Decimal, _
                   pstrTransactionKey1 As String, _
                   pstrTransactionKey2 As String, _
                   pstrTransactionKey3 As String, _
                   pstrReference As String, _
                   pstrCostCentreDesc As String, _
                   pstrExpenseTypeDesc As String, _
                   pstrProjectCodeDesc As String, _
                   pstrInvoiceNo As String, _
                   pstrInvoiceLine As String, _
                   pstrProductCode As String, _
                   pstrProductTypeCode As String, _
                   pdecOurCharges As Decimal, _
                   pdecCancellationCharge As Decimal,
                   pdecTotal As Decimal)
        mNominalCode = pstrNominalCode
        mProjectCode = pstrProjectCode
        mCostCentre = pstrCostCentre
        mDescription = pstrDescription
        mDr = pdecDr
        mCr = pdecCr
        mVat = pdecVat
        mDisVat = pdecDisVat
        mTransactionKey1 = pstrTransactionKey1
        mTransactionKey2 = pstrTransactionKey2
        mTransactionKey3 = pstrTransactionKey3
        mReference = pstrReference
        mCostCentreDesc = pstrCostCentreDesc
        mExpenseTypeDesc = pstrExpenseTypeDesc
        mProjectCodeDesc = pstrProjectCodeDesc
        mInvoiceNo = pstrInvoiceNo
        mInvoiceLine = pstrInvoiceLine
        mProductCode = pstrProductCode
        mProductTypeCode = pstrProductTypeCode
        'R2.21c SA 
        mOurCharges = pdecOurCharges
        mCancellationCharge = pdecCancellationCharge
        mTotal = pdecTotal
    End Sub

    Private mNominalCode As String
    Private mProjectCode As String
    Private mCostCentre As String
    Private mDescription As String
    Private mDr As Decimal
    Private mCr As Decimal
    Private mVat As Decimal
    Private mDisVat As Decimal
    Private mTransactionKey1 As String
    Private mTransactionKey2 As String
    Private mTransactionKey3 As String
    Private mReference As String
    Private mCostCentreDesc As String
    Private mExpenseTypeDesc As String
    Private mProjectCodeDesc As String
    Private mInvoiceNo As String
    Private mInvoiceLine As String
    Private mProductCode As String
    Private mProductTypeCode As String
    'R2.21c SA
    Private mOurCharges As Decimal
    Private mCancellationCharge As Decimal
    Private mTotal As Decimal

    Public Property NominalCode As String
        Get
            Return mNominalCode
        End Get
        Set(value As String)
            mNominalCode = value
        End Set
    End Property

    Public Property ProjectCode As String
        Get
            Return mProjectCode
        End Get
        Set(value As String)
            mProjectCode = value
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

    Public Property Description As String
        Get
            Return mDescription
        End Get
        Set(value As String)
            mDescription = value
        End Set
    End Property

    Public Property Dr As Decimal
        Get
            Return mDr
        End Get
        Set(value As Decimal)
            mDr = value
        End Set
    End Property

    Public Property Cr As Decimal
        Get
            Return mCr
        End Get
        Set(value As Decimal)
            mCr = value
        End Set
    End Property

    Public Property Vat As Decimal
        Get
            Return mVat
        End Get
        Set(value As Decimal)
            mVat = value
        End Set
    End Property

    Public Property DisVat As Decimal
        Get
            Return mDisVat
        End Get
        Set(value As Decimal)
            mDisVat = value
        End Set
    End Property

    Public Property TransactionKey1 As String
        Get
            Return mTransactionKey1
        End Get
        Set(value As String)
            mTransactionKey1 = value
        End Set
    End Property

    Public Property TransactionKey2 As String
        Get
            Return mTransactionKey2
        End Get
        Set(value As String)
            mTransactionKey2 = value
        End Set
    End Property

    Public Property TransactionKey3 As String
        Get
            Return mTransactionKey3
        End Get
        Set(value As String)
            mTransactionKey3 = value
        End Set
    End Property

    Public Property Reference As String
        Get
            Return mReference
        End Get
        Set(value As String)
            mReference = value
        End Set
    End Property

    Public Property CostCentreDesc As String
        Get
            Return mCostCentreDesc
        End Get
        Set(value As String)
            mCostCentreDesc = value
        End Set
    End Property

    Public Property ExpenseTypeDesc As String
        Get
            Return mExpenseTypeDesc
        End Get
        Set(value As String)
            mExpenseTypeDesc = value
        End Set
    End Property

    Public Property ProjectCodeDesc As String
        Get
            Return mProjectCodeDesc
        End Get
        Set(value As String)
            mProjectCodeDesc = value
        End Set
    End Property

    Public Property InvoiceNo As String
        Get
            Return mInvoiceNo
        End Get
        Set(value As String)
            mInvoiceNo = value
        End Set
    End Property

    Public Property InvoiceLine As String
        Get
            Return mInvoiceLine
        End Get
        Set(value As String)
            mInvoiceLine = value
        End Set
    End Property

    Public Property ProductCode As String
        Get
            Return mProductCode
        End Get
        Set(value As String)
            mProductCode = value
        End Set
    End Property

    Public Property ProductTypeCode As String
        Get
            Return mProductTypeCode
        End Get
        Set(value As String)
            mProductTypeCode = value
        End Set
    End Property

    'R2.21c SA
    Public Property OurCharges As Decimal
        Get
            Return mOurCharges
        End Get
        Set(ByVal value As Decimal)
            mOurCharges = value
        End Set
    End Property

    'R2.21c SA 
    Public Property CancellationCharge As Decimal
        Get
            Return mCancellationCharge
        End Get
        Set(ByVal value As Decimal)
            mCancellationCharge = value
        End Set
    End Property

    'R2.21c SA 
    Public Property Total As Decimal
        Get
            Return mTotal
        End Get
        Set(ByVal value As Decimal)
            mTotal = value
        End Set
    End Property

    'R2.21c SA - added OurCharges, CancellationCharge, Total
    Private Shared Function makeICEFromRow( _
            ByVal r As IDataReader _
        ) As ICE
        Return New ICE( _
                clsNYS.notString(r.Item("NominalCode")), _
                clsNYS.notString(r.Item("ProjectCode")), _
                clsNYS.notString(r.Item("CostCentre")), _
                clsNYS.notString(r.Item("Description")), _
                clsNYS.notDecimal(r.Item("Dr")), _
                clsNYS.notDecimal(r.Item("Cr")), _
                clsNYS.notDecimal(r.Item("VAT")), _
                clsNYS.notDecimal(r.Item("DisVat")), _
                clsNYS.notString(r.Item("TransactionKey1")), _
                clsNYS.notString(r.Item("TransactionKey2")), _
                clsNYS.notString(r.Item("TransactionKey3")), _
                clsNYS.notString(r.Item("Reference")), _
                clsNYS.notString(r.Item("CostCentreDesc")), _
                clsNYS.notString(r.Item("ExpenseTypeDesc")), _
                clsNYS.notString(r.Item("ProjectCodeDesc")), _
                clsNYS.notString(r.Item("InvoiceNo")), _
                clsNYS.notString(r.Item("InvoiceLine")), _
                clsNYS.notString(r.Item("ProductCode")), _
                clsNYS.notString(r.Item("ProductTypeCode")), _
                clsNYS.notDecimal(r.Item("OurCharges")), _
                clsNYS.notDecimal(r.Item("CancellationCharge")), _
                clsNYS.notDecimal(r.Item("Total")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String) As List(Of ICE)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As New List(Of ICE)()
            Using r As IDataReader = dbh.callSP("FeederFile_ICE", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeICEFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function listRail(ByVal pstartdate As String, ByVal penddate As String) As List(Of ICE)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As New List(Of ICE)()
            Using r As IDataReader = dbh.callSP("FeederFile_ICEGetRailCharges", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeICEFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function
End Class
