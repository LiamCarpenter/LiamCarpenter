Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class Anchor
    'R2.21.£ SA - added pEmployeeNumber, pProduct
    'R1.1 SA - added ByVal pName As String, _
    Public Sub New( _
        ByVal pAccount As String, _
        ByVal pCostc As String, _
        ByVal pProject As String, _
        ByVal pAsset As String, _
        ByVal pResource As String, _
        ByVal pBusinessid As String, _
        ByVal pCur_amount As Nullable(Of Double), _
        ByVal pAmount As Nullable(Of Double), _
        ByVal pInm_ldname As String, _
        ByVal pDescript As String, _
        ByVal pInvoice As String, _
        ByVal pVat As Nullable(Of Double), _
        ByVal pDispVat As Nullable(Of Double), _
        ByVal pEmployeeNumber As String, _
        ByVal pProduct As String)
        mAccount = pAccount
        mCostc = pCostc
        mProject = pProject
        mAsset = pAsset
        mResource = pResource
        mBusinessid = pBusinessid
        mCur_amount = pCur_amount
        mAmount = pAmount
        mDescript = pDescript
        mInvoice = pInvoice
        mVat = pVat
        mDispVat = pDispVat
        'R1.1 SA 
        mInm_ldname = pInm_ldname
        'R2.21.3 SA 
        mEmployeeNumber = pEmployeeNumber
        mProduct = pProduct
    End Sub

    Public Sub New( _
)
    End Sub

    Private mAccount As String
    Private mCostc As String
    Private mProject As String
    Private mAsset As String
    Private mResource As String
    Private mBusinessid As String
    Private mCur_amount As Nullable(Of Double)
    Private mAmount As Nullable(Of Double)
    Private mDescript As String
    Private mInvoice As String
    Private mVat As Nullable(Of Double)
    Private mDispVat As Nullable(Of Double)
    'R1.1 SA 
    Private mInm_ldname As String
    'R2.21.3 SA 
    Private mEmployeeNumber As String
    Private mProduct As String


    Public Property Account() As String
        Get
            Return mAccount
        End Get
        Set(ByVal value As String)
            mAccount = value
        End Set
    End Property

    Public Property Costc() As String
        Get
            Return mCostc
        End Get
        Set(ByVal value As String)
            mCostc = value
        End Set
    End Property

    Public Property Project() As String
        Get
            Return mProject
        End Get
        Set(ByVal value As String)
            mProject = value
        End Set
    End Property

    Public Property Asset() As String
        Get
            Return mAsset
        End Get
        Set(ByVal value As String)
            mAsset = value
        End Set
    End Property

    Public Property Resource() As String
        Get
            Return mResource
        End Get
        Set(ByVal value As String)
            mResource = value
        End Set
    End Property

    Public Property Businessid() As String
        Get
            Return mBusinessid
        End Get
        Set(ByVal value As String)
            mBusinessid = value
        End Set
    End Property

    Public Property Cur_amount() As Nullable(Of Double)
        Get
            Return mCur_amount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mCur_amount = value
        End Set
    End Property

    Public Property Amount() As Nullable(Of Double)
        Get
            Return mAmount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mAmount = value
        End Set
    End Property

    Public Property Descript() As String
        Get
            Return mDescript
        End Get
        Set(ByVal value As String)
            mDescript = value
        End Set
    End Property

    Public Property Invoice() As String
        Get
            Return mInvoice
        End Get
        Set(ByVal value As String)
            mInvoice = value
        End Set
    End Property

    Public Property Vat() As Nullable(Of Double)
        Get
            Return mVat
        End Get
        Set(ByVal value As Nullable(Of Double))
            mVat = value
        End Set
    End Property

    Public Property DispVat() As Nullable(Of Double)
        Get
            Return mDispVat
        End Get
        Set(ByVal value As Nullable(Of Double))
            mDispVat = value
        End Set
    End Property
    'R1.1 SA 
    Public Property Inm_ldname() As String
        Get
            Return mInm_ldname
        End Get
        Set(ByVal value As String)
            mInm_ldname = value
        End Set
    End Property

    'R2.21.3 SA 
    Public Property EmployeeNumber() As String
        Get
            Return mEmployeeNumber
        End Get
        Set(ByVal value As String)
            mEmployeeNumber = value
        End Set
    End Property

    'R2.21.3 SA 
    Public Property Product() As String
        Get
            Return mProduct
        End Get
        Set(ByVal value As String)
            mProduct = value
        End Set
    End Property

    Private Shared Function makeAnchorFromRow( _
            ByVal r As IDataReader _
        ) As Anchor
        'R2.21.3 SA - added employeenumber 
        'R1.1 SA - added clsNYS.notString(r.Item("inm_ldname"))
        Return New Anchor( _
                clsNYS.notString(r.Item("account")), _
                clsNYS.notString(r.Item("costc")), _
                clsNYS.notString(r.Item("project")), _
                clsNYS.notString(r.Item("asset")), _
                clsNYS.notString(r.Item("resource")), _
                clsNYS.notString(r.Item("businessid")), _
                clsNYS.notNumber(r.Item("cur_amount")), _
                clsNYS.notNumber(r.Item("amount")), _
                clsNYS.notString(r.Item("inm_ldname")), _
                clsNYS.notString(r.Item("descript")), _
                clsNYS.notString(r.Item("invoice")), _
                clsNYS.notNumber(r.Item("vat")), _
                clsNYS.notNumber(r.Item("dispVat")), _
                clsNYS.notString(r.Item("employeenumber")), _
                clsNYS.notString(r.Item("product")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String) As List(Of Anchor)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Anchor)()
            Using r As IDataReader = dbh.callSP("FeederFile_Anchor", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeAnchorFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
