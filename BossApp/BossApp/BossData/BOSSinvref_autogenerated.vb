Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSinvref

    Public Sub New( _
        ByVal pInvrefsID As Integer, _
        ByVal pRef_ukey As String, _
        ByVal pRef_invno As String, _
        ByVal pRef_3 As String, _
        ByVal pRef_4 As String, _
        ByVal pRef_5 As String, _
        ByVal pRef_6 As String, _
        ByVal pRef_7 As String, _
        ByVal pRef_8 As String, _
        ByVal pRef_9 As String)
        mInvrefsID = pInvrefsID
        mRef_ukey = pRef_ukey
        mRef_invno = pRef_invno
        mRef_3 = pRef_3
        mRef_4 = pRef_4
        mRef_5 = pRef_5
        mRef_6 = pRef_6
        mRef_7 = pRef_7
        mRef_8 = pRef_8
        mRef_9 = pRef_9
    End Sub

    Public Sub New( _
)
    End Sub

    Private mInvrefsID As Integer
    Private mRef_ukey As String
    Private mRef_invno As String
    Private mRef_3 As String
    Private mRef_4 As String
    Private mRef_5 As String
    Private mRef_6 As String
    Private mRef_7 As String
    Private mRef_8 As String
    Private mRef_9 As String

    Public Property InvrefsID() As Integer
        Get
            Return mInvrefsID
        End Get
        Set(ByVal value As Integer)
            mInvrefsID = value
        End Set
    End Property

    Public Property Ref_ukey() As String
        Get
            Return mRef_ukey
        End Get
        Set(ByVal value As String)
            mRef_ukey = value
        End Set
    End Property

    Public Property Ref_invno() As String
        Get
            Return mRef_invno
        End Get
        Set(ByVal value As String)
            mRef_invno = value
        End Set
    End Property

    Public Property Ref_3() As String
        Get
            Return mRef_3
        End Get
        Set(ByVal value As String)
            mRef_3 = value
        End Set
    End Property

    Public Property Ref_4() As String
        Get
            Return mRef_4
        End Get
        Set(ByVal value As String)
            mRef_4 = value
        End Set
    End Property

    Public Property Ref_5() As String
        Get
            Return mRef_5
        End Get
        Set(ByVal value As String)
            mRef_5 = value
        End Set
    End Property

    Public Property Ref_6() As String
        Get
            Return mRef_6
        End Get
        Set(ByVal value As String)
            mRef_6 = value
        End Set
    End Property

    Public Property Ref_7() As String
        Get
            Return mRef_7
        End Get
        Set(ByVal value As String)
            mRef_7 = value
        End Set
    End Property

    Public Property Ref_8() As String
        Get
            Return mRef_8
        End Get
        Set(ByVal value As String)
            mRef_8 = value
        End Set
    End Property

    Public Property Ref_9() As String
        Get
            Return mRef_9
        End Get
        Set(ByVal value As String)
            mRef_9 = value
        End Set
    End Property

    Private Shared Function makeBOSSinvrefFromRow( _
            ByVal r As IDataReader _
        ) As BOSSinvref
        Return New BOSSinvref( _
                clsUseful.notInteger(r.Item("invrefsID")), _
                clsUseful.notString(r.Item("ref_ukey")), _
                clsUseful.notString(r.Item("ref_invno")), _
                clsUseful.notString(r.Item("ref_3")), _
                clsUseful.notString(r.Item("ref_4")), _
                clsUseful.notString(r.Item("ref_5")), _
                clsUseful.notString(r.Item("ref_6")), _
                clsUseful.notString(r.Item("ref_7")), _
                clsUseful.notString(r.Item("ref_8")), _
                clsUseful.notString(r.Item("ref_9")))
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = clsUseful.notString(dbh.callSPSingleValueCanReturnNothing("BOSSinvrefs_save", "@InvrefsID", mInvrefsID, "@Ref_ukey", mRef_ukey, "@Ref_invno", mRef_invno, _
                                                                                             "@Ref_3", mRef_3, "@Ref_4", mRef_4, "@Ref_5", mRef_5, "@Ref_6", mRef_6, "@Ref_7", mRef_7, _
                                                                                             "@Ref_8", mRef_8, "@Ref_9", mRef_9))
            Return strRet
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSinvref)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSinvref)()
            Using r As IDataReader = dbh.callSP("BOSSinvrefs_list")
                While r.Read()
                    ret.Add(makeBOSSinvrefFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function
End Class
