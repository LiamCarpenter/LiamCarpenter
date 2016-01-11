Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSScash

    Public Sub New( _
        ByVal pCashID As Integer, _
        ByVal pCsd_key As String, _
        ByVal pCsd_appid As String, _
        ByVal pCsd_to As String, _
        ByVal pCsd_applyd As String, _
        ByVal pCsd_appkey As String, _
        ByVal pCsd_invkey As String, _
        ByVal pCsd_applgr As String, _
        ByVal pCsd_arkey As String, _
        ByVal pCsd_cdate As Nullable(Of DateTime), _
        ByVal pCsd_today As Nullable(Of DateTime), _
        ByVal pCsd_suppid As String, _
        ByVal pCsd_prdkey As String, _
        ByVal pCsd_ok As String, _
        ByVal pCsd_final As String, _
        ByVal pCsd_note As String, _
        ByVal pCsd_locked As String, _
        ByVal pCsd_status As String, _
        ByVal pCsd_who As String, _
        ByVal pCsd_ukey As String, _
        ByVal pChksum As String, _
        ByVal pDatecreated As Nullable(Of DateTime))
        mCashID = pCashID
        mCsd_key = pCsd_key
        mCsd_appid = pCsd_appid
        mCsd_to = pCsd_to
        mCsd_applyd = pCsd_applyd
        mCsd_appkey = pCsd_appkey
        mCsd_invkey = pCsd_invkey
        mCsd_applgr = pCsd_applgr
        mCsd_arkey = pCsd_arkey
        mCsd_cdate = pCsd_cdate
        mCsd_today = pCsd_today
        mCsd_suppid = pCsd_suppid
        mCsd_prdkey = pCsd_prdkey
        mCsd_ok = pCsd_ok
        mCsd_final = pCsd_final
        mCsd_note = pCsd_note
        mCsd_locked = pCsd_locked
        mCsd_status = pCsd_status
        mCsd_who = pCsd_who
        mCsd_ukey = pCsd_ukey
        mChksum = pChksum
        mDatecreated = pDatecreated
    End Sub

    Public Sub New( _
)
    End Sub

    Private mCashID As Integer
    Private mCsd_key As String
    Private mCsd_appid As String
    Private mCsd_to As String
    Private mCsd_applyd As String
    Private mCsd_appkey As String
    Private mCsd_invkey As String
    Private mCsd_applgr As String
    Private mCsd_arkey As String
    Private mCsd_cdate As Nullable(Of DateTime)
    Private mCsd_today As Nullable(Of DateTime)
    Private mCsd_suppid As String
    Private mCsd_prdkey As String
    Private mCsd_ok As String
    Private mCsd_final As String
    Private mCsd_note As String
    Private mCsd_locked As String
    Private mCsd_status As String
    Private mCsd_who As String
    Private mCsd_ukey As String
    Private mChksum As String
    Private mDatecreated As Nullable(Of DateTime)

    Public Property CashID() As Integer
        Get
            Return mCashID
        End Get
        Set(ByVal value As Integer)
            mCashID = value
        End Set
    End Property

    Public Property Csd_key() As String
        Get
            Return mCsd_key
        End Get
        Set(ByVal value As String)
            mCsd_key = value
        End Set
    End Property

    Public Property Csd_appid() As String
        Get
            Return mCsd_appid
        End Get
        Set(ByVal value As String)
            mCsd_appid = value
        End Set
    End Property

    Public Property Csd_to() As String
        Get
            Return mCsd_to
        End Get
        Set(ByVal value As String)
            mCsd_to = value
        End Set
    End Property

    Public Property Csd_applyd() As String
        Get
            Return mCsd_applyd
        End Get
        Set(ByVal value As String)
            mCsd_applyd = value
        End Set
    End Property

    Public Property Csd_appkey() As String
        Get
            Return mCsd_appkey
        End Get
        Set(ByVal value As String)
            mCsd_appkey = value
        End Set
    End Property

    Public Property Csd_invkey() As String
        Get
            Return mCsd_invkey
        End Get
        Set(ByVal value As String)
            mCsd_invkey = value
        End Set
    End Property

    Public Property Csd_applgr() As String
        Get
            Return mCsd_applgr
        End Get
        Set(ByVal value As String)
            mCsd_applgr = value
        End Set
    End Property

    Public Property Csd_arkey() As String
        Get
            Return mCsd_arkey
        End Get
        Set(ByVal value As String)
            mCsd_arkey = value
        End Set
    End Property

    Public Property Csd_cdate() As Nullable(Of DateTime)
        Get
            Return mCsd_cdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCsd_cdate = value
        End Set
    End Property

    Public Property Csd_today() As Nullable(Of DateTime)
        Get
            Return mCsd_today
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCsd_today = value
        End Set
    End Property

    Public Property Csd_suppid() As String
        Get
            Return mCsd_suppid
        End Get
        Set(ByVal value As String)
            mCsd_suppid = value
        End Set
    End Property

    Public Property Csd_prdkey() As String
        Get
            Return mCsd_prdkey
        End Get
        Set(ByVal value As String)
            mCsd_prdkey = value
        End Set
    End Property

    Public Property Csd_ok() As String
        Get
            Return mCsd_ok
        End Get
        Set(ByVal value As String)
            mCsd_ok = value
        End Set
    End Property

    Public Property Csd_final() As String
        Get
            Return mCsd_final
        End Get
        Set(ByVal value As String)
            mCsd_final = value
        End Set
    End Property

    Public Property Csd_note() As String
        Get
            Return mCsd_note
        End Get
        Set(ByVal value As String)
            mCsd_note = value
        End Set
    End Property

    Public Property Csd_locked() As String
        Get
            Return mCsd_locked
        End Get
        Set(ByVal value As String)
            mCsd_locked = value
        End Set
    End Property

    Public Property Csd_status() As String
        Get
            Return mCsd_status
        End Get
        Set(ByVal value As String)
            mCsd_status = value
        End Set
    End Property

    Public Property Csd_who() As String
        Get
            Return mCsd_who
        End Get
        Set(ByVal value As String)
            mCsd_who = value
        End Set
    End Property

    Public Property Csd_ukey() As String
        Get
            Return mCsd_ukey
        End Get
        Set(ByVal value As String)
            mCsd_ukey = value
        End Set
    End Property

    Public Property Chksum() As String
        Get
            Return mChksum
        End Get
        Set(ByVal value As String)
            mChksum = value
        End Set
    End Property

    Public Property Datecreated() As Nullable(Of DateTime)
        Get
            Return mDatecreated
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mDatecreated = value
        End Set
    End Property

    Private Shared Function makeBOSScashFromRow( _
            ByVal r As IDataReader _
        ) As BOSScash
        Return New BOSScash( _
                 clsUseful.notInteger(r.Item("cashID")), _
                 clsUseful.notString(r.Item("csd_key")), _
                 clsUseful.notString(r.Item("csd_appid")), _
                 clsUseful.notString(r.Item("csd_to")), _
                 clsUseful.notString(r.Item("csd_applyd")), _
                 clsUseful.notString(r.Item("csd_appkey")), _
                 clsUseful.notString(r.Item("csd_invkey")), _
                 clsUseful.notString(r.Item("csd_applgr")), _
                 clsUseful.notString(r.Item("csd_arkey")), _
                toNullableDate(r.Item("csd_cdate")), _
                toNullableDate(r.Item("csd_today")), _
                 clsUseful.notString(r.Item("csd_suppid")), _
                 clsUseful.notString(r.Item("csd_prdkey")), _
                 clsUseful.notString(r.Item("csd_ok")), _
                 clsUseful.notString(r.Item("csd_final")), _
                 clsUseful.notString(r.Item("csd_note")), _
                 clsUseful.notString(r.Item("csd_locked")), _
                 clsUseful.notString(r.Item("csd_status")), _
                 clsUseful.notString(r.Item("csd_who")), _
                 clsUseful.notString(r.Item("csd_ukey")), _
                 clsUseful.notString(r.Item("chksum")), _
                toNullableDate(r.Item("datecreated")))
    End Function

    Public Shared Function [get]( _
            ByVal pCashID As Integer _
        ) As BOSScash
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Using r As IDataReader = dbh.callSP("BOSScash_get", "@cashID", pCashID)
                If Not r.Read() Then
                    Throw New Exception("No BOSScash with id " & pCashID)
                End If
                Dim ret As BOSScash = makeBOSScashFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSScash)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As New List(Of BOSScash)()
            Using r As IDataReader = dbh.callSP("BOSScash_list")
                While r.Read()
                    ret.Add(makeBOSScashFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim strRet As String = clsUseful.notString(dbh.callSPSingleValue("BOSScash_save", "@CashID", mCashID, "@Csd_key", mCsd_key, "@Csd_appid", mCsd_appid, "@Csd_to", mCsd_to, "@Csd_applyd", mCsd_applyd, _
                                                 "@Csd_appkey", mCsd_appkey, "@Csd_invkey", mCsd_invkey, "@Csd_applgr", mCsd_applgr, "@Csd_arkey", mCsd_arkey, "@Csd_cdate", mCsd_cdate, _
                                                 "@Csd_today", mCsd_today, "@Csd_suppid", mCsd_suppid, "@Csd_prdkey", mCsd_prdkey, "@Csd_ok", mCsd_ok, "@Csd_final", mCsd_final, "@Csd_note", _
                                                 mCsd_note, "@Csd_locked", mCsd_locked, "@Csd_status", mCsd_status, "@Csd_who", mCsd_who, "@Csd_ukey", mCsd_ukey, "@Chksum", mChksum, "@Datecreated", mDatecreated))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete(ByVal pCashID As Integer, ByVal pCsd_ukey As String)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            dbh.callNonQuerySP("BOSScash_delete", "@CashID", pCashID, "@Csd_ukey", pCsd_ukey)
        End Using
    End Sub

End Class
