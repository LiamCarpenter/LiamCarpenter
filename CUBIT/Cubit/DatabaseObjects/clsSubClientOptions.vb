Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Public Class clsSubClientOptions

    Public Sub New( _
)
    End Sub

    Public Sub New(ByVal pSubClientID As Integer, _
                    ByVal pSubClientName As String, _
                    ByVal pSubClientBossCode As String, _
                    ByVal pGroupID As Integer,
                    ByVal pSubClientFee As Decimal, _
                    ByVal pSubClientActive As Boolean)
        mSubClientID = pSubClientID
        mSubClientName = pSubClientName
        mSubClientBossCode = pSubClientBossCode
        mGroupID = pGroupID
        mSubClientFee = pSubClientFee
        mSubClientActive = pSubClientActive
    End Sub

    Private mSubClientID As Integer
    Private mSubClientName As String
    Private mSubClientBossCode As String
    Private mGroupID As Integer
    Private mSubClientFee As Decimal
    Private mSubClientActive As Boolean

    Public Property SubClientID As Integer
        Get
            Return mSubClientID
        End Get
        Set(ByVal value As Integer)
            mSubClientID = value
        End Set
    End Property

    Public Property SubClientName As String
        Get
            Return mSubClientName
        End Get
        Set(ByVal value As String)
            mSubClientName = value
        End Set
    End Property

    Public Property SubClientBossCode As String
        Get
            Return mSubClientBossCode
        End Get
        Set(ByVal value As String)
            mSubClientBossCode = value
        End Set
    End Property

    Public Property GroupID As Integer
        Get
            Return mGroupID
        End Get
        Set(ByVal value As Integer)
            mGroupID = value
        End Set
    End Property

    Public Property SubClientFee As Decimal
        Get
            Return mSubClientFee
        End Get
        Set(ByVal value As Decimal)
            mSubClientFee = value
        End Set
    End Property

    Public Property SubClientActive As Boolean
        Get
            Return mSubClientActive
        End Get
        Set(ByVal value As Boolean)
            mSubClientActive = value
        End Set
    End Property

    Private Shared Function makeSubClientOptionFromRow( _
            ByVal r As IDataReader _
        ) As clsSubClientOptions
        Return New clsSubClientOptions( _
                CInt(r.Item("SubClientID")), _
                clsStuff.notString(r.Item("SubClientName")), _
                clsStuff.notString(r.Item("SubClientBossCode")), _
                CInt(r.Item("GroupID")), _
                CDec(r.Item("SubClientFee")), _
                clsStuff.notBoolean(r.Item("SubClientActive")))
    End Function

    Public Shared Function list(ByVal pGroupID As Integer) As List(Of clsSubClientOptions)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsSubClientOptions)()
            Using r As IDataReader = dbh.callSP("SubClientOptions_list", "@GroupID", pGroupID)
                While r.Read()
                    ret.Add(makeSubClientOptionFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function [get](ByVal pSubClientID As Integer) As clsSubClientOptions
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("SubClientOptions_Get", "@SubClientID", pSubClientID)
                Dim ret As New clsSubClientOptions
                If r.Read() Then
                    ret = makeSubClientOptionFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function getSubClientFee(ByVal pstrBossCode As String) As Decimal
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim dRet As Decimal = clsStuff.notDecimal(dbh.callSPSingleValueCanReturnNothing("SubClientOptions_GetFee", _
                                                                       "@SubClientBossCode", pstrBossCode))
            Return dRet
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mSubClientID = clsStuff.notWholeNumber(dbh.callSPSingleValue("SubClientOptions_Save", _
                                                          "@SubClientID", mSubClientID, _
                                                          "@SubClientName", mSubClientName, _
                                                          "@SubClientBossCode", mSubClientBossCode, _
                                                          "@GroupID", mGroupID, _
                                                          "@SubClientFee", mSubClientFee,
                                                          "@SubClientActive", mSubClientActive))
            Return mSubClientID
        End Using
    End Function

End Class
