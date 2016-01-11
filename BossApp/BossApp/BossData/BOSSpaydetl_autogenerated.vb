Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSpaydetl

    Public Sub New( _
        ByVal pPaydetlID As Integer, _
        ByVal pPyd_key As String, _
        ByVal pPyd_line As String, _
        ByVal pPyd_ctrl As Nullable(Of Boolean), _
        ByVal pPyd_cdate As String, _
        ByVal pPyd_today As String, _
        ByVal pPyd_branch As String, _
        ByVal pPyd_ledger As String, _
        ByVal pPyd_applyd As String, _
        ByVal pPyd_dr As Decimal, _
        ByVal pPyd_cr As Decimal, _
        ByVal pPyd_apkey As String, _
        ByVal pPyd_ukey As String, _
        ByVal pPyd_locked As Nullable(Of Boolean), _
        ByVal pPyd_recon As Nullable(Of Boolean), _
        ByVal pPyd_recdte As String, _
        ByVal pPyd_ok As Nullable(Of Boolean), _
        ByVal pPyd_who As String)
        mPaydetlID = pPaydetlID
        mPyd_key = pPyd_key
        mPyd_line = pPyd_line
        mPyd_ctrl = pPyd_ctrl
        mPyd_cdate = pPyd_cdate
        mPyd_today = pPyd_today
        mPyd_branch = pPyd_branch
        mPyd_ledger = pPyd_ledger
        mPyd_applyd = pPyd_applyd
        mPyd_dr = pPyd_dr
        mPyd_cr = pPyd_cr
        mPyd_apkey = pPyd_apkey
        mPyd_ukey = pPyd_ukey
        mPyd_locked = pPyd_locked
        mPyd_recon = pPyd_recon
        mPyd_recdte = pPyd_recdte
        mPyd_ok = pPyd_ok
        mPyd_who = pPyd_who
    End Sub

    Public Sub New(ByVal pPaydetlID As Integer, _
       ByVal pPyd_key As String, _
        ByVal pPyd_apkey As String, _
        ByVal pPyd_ukey As String)
        mPaydetlID = pPaydetlID
        mPyd_key = pPyd_key
        mPyd_apkey = pPyd_apkey
        mPyd_ukey = pPyd_ukey
    End Sub

    Public Sub New( _
)
    End Sub

    Private mPaydetlID As Integer
    Private mPyd_key As String
    Private mPyd_line As String
    Private mPyd_ctrl As Nullable(Of Boolean)
    Private mPyd_cdate As String
    Private mPyd_today As String
    Private mPyd_branch As String
    Private mPyd_ledger As String
    Private mPyd_applyd As String
    Private mPyd_dr As Decimal
    Private mPyd_cr As Decimal
    Private mPyd_apkey As String
    Private mPyd_ukey As String
    Private mPyd_locked As Nullable(Of Boolean)
    Private mPyd_recon As Nullable(Of Boolean)
    Private mPyd_recdte As String
    Private mPyd_ok As Nullable(Of Boolean)
    Private mPyd_who As String

    Public Property PaydetlID() As Integer
        Get
            Return mPaydetlID
        End Get
        Set(ByVal value As Integer)
            mPaydetlID = value
        End Set
    End Property

    Public Property Pyd_key() As String
        Get
            Return mPyd_key
        End Get
        Set(ByVal value As String)
            mPyd_key = value
        End Set
    End Property

    Public Property Pyd_line() As String
        Get
            Return mPyd_line
        End Get
        Set(ByVal value As String)
            mPyd_line = value
        End Set
    End Property

    Public Property Pyd_ctrl() As Nullable(Of Boolean)
        Get
            Return mPyd_ctrl
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPyd_ctrl = value
        End Set
    End Property

    Public Property Pyd_cdate() As String
        Get
            Return mPyd_cdate
        End Get
        Set(ByVal value As String)
            mPyd_cdate = value
        End Set
    End Property

    Public Property Pyd_today() As String
        Get
            Return mPyd_today
        End Get
        Set(ByVal value As String)
            mPyd_today = value
        End Set
    End Property

    Public Property Pyd_branch() As String
        Get
            Return mPyd_branch
        End Get
        Set(ByVal value As String)
            mPyd_branch = value
        End Set
    End Property

    Public Property Pyd_ledger() As String
        Get
            Return mPyd_ledger
        End Get
        Set(ByVal value As String)
            mPyd_ledger = value
        End Set
    End Property

    Public Property Pyd_applyd() As String
        Get
            Return mPyd_applyd
        End Get
        Set(ByVal value As String)
            mPyd_applyd = value
        End Set
    End Property

    Public Property Pyd_dr() As Decimal
        Get
            Return mPyd_dr
        End Get
        Set(ByVal value As Decimal)
            mPyd_dr = value
        End Set
    End Property

    Public Property Pyd_cr() As Decimal
        Get
            Return mPyd_cr
        End Get
        Set(ByVal value As Decimal)
            mPyd_cr = value
        End Set
    End Property

    Public Property Pyd_apkey() As String
        Get
            Return mPyd_apkey
        End Get
        Set(ByVal value As String)
            mPyd_apkey = value
        End Set
    End Property

    Public Property Pyd_ukey() As String
        Get
            Return mPyd_ukey
        End Get
        Set(ByVal value As String)
            mPyd_ukey = value
        End Set
    End Property

    Public Property Pyd_locked() As Nullable(Of Boolean)
        Get
            Return mPyd_locked
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPyd_locked = value
        End Set
    End Property

    Public Property Pyd_recon() As Nullable(Of Boolean)
        Get
            Return mPyd_recon
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPyd_recon = value
        End Set
    End Property

    Public Property Pyd_recdte() As String
        Get
            Return mPyd_recdte
        End Get
        Set(ByVal value As String)
            mPyd_recdte = value
        End Set
    End Property

    Public Property Pyd_ok() As Nullable(Of Boolean)
        Get
            Return mPyd_ok
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPyd_ok = value
        End Set
    End Property

    Public Property Pyd_who() As String
        Get
            Return mPyd_who
        End Get
        Set(ByVal value As String)
            mPyd_who = value
        End Set
    End Property

    Private Shared Function makeBOSSpaydetlFromRow( _
            ByVal r As IDataReader _
        ) As BOSSpaydetl
        Return New BOSSpaydetl( _
                clsUseful.notInteger(r.Item("paydetlID")), _
                clsUseful.notString(r.Item("pyd_key")), _
                clsUseful.notString(r.Item("pyd_line")), _
                toNullableBoolean(r.Item("pyd_ctrl")), _
                clsUseful.notString(r.Item("pyd_cdate")), _
                clsUseful.notString(r.Item("pyd_today")), _
                clsUseful.notString(r.Item("pyd_branch")), _
                clsUseful.notString(r.Item("pyd_ledger")), _
                clsUseful.notString(r.Item("pyd_applyd")), _
                toNullableFloat(r.Item("pyd_dr")), _
                toNullableFloat(r.Item("pyd_cr")), _
                clsUseful.notString(r.Item("pyd_apkey")), _
                clsUseful.notString(r.Item("pyd_ukey")), _
                toNullableBoolean(r.Item("pyd_locked")), _
                toNullableBoolean(r.Item("pyd_recon")), _
                clsUseful.notString(r.Item("pyd_recdte")), _
                toNullableBoolean(r.Item("pyd_ok")), _
                clsUseful.notString(r.Item("pyd_who")))
    End Function

    Private Shared Function makeBOSSpaydetlIdsFromRow( _
            ByVal r As IDataReader _
        ) As BOSSpaydetl
        Return New BOSSpaydetl(clsUseful.notInteger(r.Item("paydetlID")), _
                                clsUseful.notString(r.Item("pyd_key")), _
                               clsUseful.notString(r.Item("pyd_apkey")), _
                               clsUseful.notString(r.Item("pyd_ukey")))
    End Function

    Public Shared Function [get]( _
            ByVal pPaydetlID As Integer _
        ) As BOSSpaydetl
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSSpaydetl_get", "@paydetlID", pPaydetlID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSpaydetl with id " & pPaydetlID)
                End If
                Dim ret As BOSSpaydetl = makeBOSSpaydetlFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSpaydetl)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSpaydetl)()
            Using r As IDataReader = dbh.callSP("BOSSpaydetl_list")
                While r.Read()
                    ret.Add(makeBOSSpaydetlFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function listIds() As List(Of BOSSpaydetl)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSpaydetl)()
            Using r As IDataReader = dbh.callSP("BOSSpaydetl_listIds")
                While r.Read()
                    ret.Add(makeBOSSpaydetlIdsFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mPaydetlID = CInt(dbh.callSPSingleValue("BOSSpaydetl_save", "@PaydetlID", mPaydetlID, "@Pyd_key", mPyd_key, "@Pyd_line", mPyd_line, "@Pyd_ctrl", mPyd_ctrl, _
                                                    "@Pyd_cdate", mPyd_cdate, "@Pyd_today", mPyd_today, "@Pyd_branch", mPyd_branch, "@Pyd_ledger", mPyd_ledger, "@Pyd_applyd", _
                                                    mPyd_applyd, "@Pyd_dr", mPyd_dr, "@Pyd_cr", mPyd_cr, "@Pyd_apkey", mPyd_apkey, "@Pyd_ukey", mPyd_ukey, "@Pyd_locked", mPyd_locked, _
                                                    "@Pyd_recon", mPyd_recon, "@Pyd_recdte", mPyd_recdte, "@Pyd_ok", mPyd_ok, "@Pyd_who", mPyd_who))
            Return mPaydetlID
        End Using
    End Function

    Public Shared Sub delete(ByVal pPaydetlID As Integer, ByVal pPyd_key As String, ByVal pPyd_applyd As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSSpaydetl_delete", "@PaydetlID", pPaydetlID, "@Pyd_key", pPyd_key, "@Pyd_applyd", pPyd_applyd)
        End Using
    End Sub

End Class
