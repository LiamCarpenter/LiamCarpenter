Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
'Imports MySql.Data.MySqlClient

Partial Public Class clsCommission

    Public Sub New( _
    ByVal pBossCode As String, _
    ByVal pVenue As String, _
    ByVal pRef As Integer, _
    ByVal pEmail As String)
        mBossCode = pBossCode
        mVenue = pVenue
        mRef = pRef
        mEmail = pEmail
    End Sub

    Public Sub New( _
)
    End Sub

    Public Sub New( _
    ByVal pinvoiceid As String, _
    ByVal pinm_suppid As String, _
    ByVal psup_name As String, _
    ByVal pinm_amount As Decimal, _
    ByVal pinm_comamt As Decimal, _
    ByVal pinm_vtoncm As Decimal, _
    ByVal pinm_invdt As String, _
    ByVal pinm_ldname As String, _
    ByVal pinm_curncy As String, _
    ByVal pinm_docmnt As String, _
    ByVal pve_reference As Integer, _
    ByVal pinm_comdue As Decimal, _
    ByVal pinm_comrcv As Decimal, _
    ByVal pinm_start As String, _
    ByVal pproduct As String, _
    ByVal phg_code As String, _
    ByVal pgroupid As Integer)
        minvoiceid = pinvoiceid
        minm_suppid = pinm_suppid
        msup_name = psup_name
        minm_amount = pinm_amount
        minm_comamt = pinm_comamt
        minm_vtoncm = pinm_vtoncm
        minm_invdt = pinm_invdt
        minm_ldname = pinm_ldname
        minm_curncy = pinm_curncy
        minm_docmnt = pinm_docmnt
        mve_reference = pve_reference
        minm_comdue = pinm_comdue
        minm_comrcv = pinm_comrcv
        minm_start = pinm_start
        mproduct = pproduct
        mhg_code = phg_code
        mgroupid = pgroupid
    End Sub

    Private mBossCode As String
    Private mVenue As String
    Private mRef As Integer
    Private mEmail As String

    Private minvoiceid As String
    Private minm_suppid As String
    Private msup_name As String
    Private minm_amount As Decimal
    Private minm_comamt As Decimal
    Private minm_vtoncm As Decimal
    Private minm_invdt As String
    Private minm_ldname As String
    Private minm_curncy As String
    Private minm_docmnt As String
    Private mve_reference As Integer
    Private minm_comdue As Decimal
    Private minm_comrcv As Decimal
    Private minm_start As String
    Private mproduct As String
    Private mhg_code As String
    Private mgroupid As Integer

    Public Property invoiceid() As String
        Get
            Return minvoiceid
        End Get
        Set(ByVal value As String)
            minvoiceid = value
        End Set
    End Property

    Public Property inm_suppid() As String
        Get
            Return minm_suppid
        End Get
        Set(ByVal value As String)
            minm_suppid = value
        End Set
    End Property

    Public Property sup_name() As String
        Get
            Return msup_name
        End Get
        Set(ByVal value As String)
            msup_name = value
        End Set
    End Property

    Public Property inm_amount() As Decimal
        Get
            Return minm_amount
        End Get
        Set(ByVal value As Decimal)
            minm_amount = value
        End Set
    End Property

    Public Property inm_comamt() As Decimal
        Get
            Return minm_comamt
        End Get
        Set(ByVal value As Decimal)
            minm_comamt = value
        End Set
    End Property

    Public Property inm_vtoncm() As Decimal
        Get
            Return minm_vtoncm
        End Get
        Set(ByVal value As Decimal)
            minm_vtoncm = value
        End Set
    End Property

    Public Property inm_invdt() As String
        Get
            Return minm_invdt
        End Get
        Set(ByVal value As String)
            minm_invdt = value
        End Set
    End Property

    Public Property inm_ldname() As String
        Get
            Return minm_ldname
        End Get
        Set(ByVal value As String)
            minm_ldname = value
        End Set
    End Property

    Public Property inm_curncy() As String
        Get
            Return minm_curncy
        End Get
        Set(ByVal value As String)
            minm_curncy = value
        End Set
    End Property

    Public Property inm_docmnt() As String
        Get
            Return minm_docmnt
        End Get
        Set(ByVal value As String)
            minm_docmnt = value
        End Set
    End Property

    Public Property ve_reference() As Integer
        Get
            Return mve_reference
        End Get
        Set(ByVal value As Integer)
            mve_reference = value
        End Set
    End Property

    Public Property inm_comdue() As Decimal
        Get
            Return minm_comdue
        End Get
        Set(ByVal value As Decimal)
            minm_comdue = value
        End Set
    End Property

    Public Property inm_comrcv() As Decimal
        Get
            Return minm_comrcv
        End Get
        Set(ByVal value As Decimal)
            minm_comrcv = value
        End Set
    End Property

    Public Property inm_start() As String
        Get
            Return minm_start
        End Get
        Set(ByVal value As String)
            minm_start = value
        End Set
    End Property

    Public Property product() As String
        Get
            Return mproduct
        End Get
        Set(ByVal value As String)
            mproduct = value
        End Set
    End Property

    Public Property hg_code() As String
        Get
            Return mhg_code
        End Get
        Set(ByVal value As String)
            mhg_code = value
        End Set
    End Property

    Public Property groupid As Integer
        Get
            Return mgroupid
        End Get
        Set(ByVal value As Integer)
            mgroupid = value
        End Set
    End Property

    Public Property BossCode() As String
        Get
            Return mBossCode
        End Get
        Set(ByVal value As String)
            mBossCode = value
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

    Public Property Email() As String
        Get
            Return mEmail
        End Get
        Set(ByVal value As String)
            mEmail = value
        End Set
    End Property

    Public Property Ref() As Integer
        Get
            Return mRef
        End Get
        Set(ByVal value As Integer)
            mRef = value
        End Set
    End Property

    Private Shared Function makeSummaryFromRow( _
           ByVal r As IDataReader _
           ) As clsCommission
        Return New clsCommission( _
            clsNYS.notString(r.Item("BossCode")), _
            clsNYS.notString(r.Item("Venue")), _
            clsNYS.notNumber(r.Item("Ref")), _
            clsNYS.notString(r.Item("Email")))
    End Function

    Private Shared Function makeSummaryVFromRow( _
           ByVal r As IDataReader _
           ) As clsCommission
        Return New clsCommission( _
            clsNYS.notString(r.Item("invoiceid")), _
            clsNYS.notString(r.Item("inm_suppid")), _
            clsNYS.notString(r.Item("sup_name")), _
            clsNYS.notDecimal(r.Item("inm_amount")), _
            clsNYS.notDecimal(r.Item("inm_comamt")), _
            clsNYS.notDecimal(r.Item("inm_vtoncm")), _
            clsNYS.notString(r.Item("inm_invdt")), _
            clsNYS.notString(r.Item("inm_ldname")), _
            clsNYS.notString(r.Item("inm_curncy")), _
            clsNYS.notString(r.Item("inm_docmnt")), _
            clsNYS.notInteger(r.Item("ve_reference")), _
            clsNYS.notDecimal(r.Item("inm_comdue")), _
            clsNYS.notDecimal(r.Item("inm_comrcv")), _
            clsNYS.notString(r.Item("inm_start")), _
            clsNYS.notString(r.Item("product")), _
            clsNYS.notString(r.Item("hg_code")), _
            clsNYS.notInteger(r.Item("groupid")))
    End Function

    Public Shared Function bossOutCommVenues() As List(Of clsCommission)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsCommission)()
            Using r As IDataReader = dbh.callSP("boss_OutCommVenues")
                While r.Read()
                    ret.Add(makeSummaryFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function bossOutComm(ByVal pStartDate As String, ByVal pEndDate As String, ByVal pVenue As String, ByVal pBoss As String) As List(Of clsCommission)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsCommission)()
            Using r As IDataReader = dbh.callSP("boss_outstandingcommissionV2", _
                                                "@startdate", pStartDate, _
                                                "@enddate", pEndDate, _
                                                "@venuename", pVenue, _
                                                "@bosscode", pBoss)
                While r.Read()
                    ret.Add(makeSummaryVFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function
   
End Class

