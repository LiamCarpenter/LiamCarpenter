Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class FeedParameter

    Public Sub New( _
        ByVal pParameterid As Integer, _
        ByVal pGroupid As Nullable(Of Integer), _
        ByVal pParameterstart As Nullable(Of DateTime), _
        ByVal pParameterend As Nullable(Of DateTime), _
        ByVal pTransactionid As Nullable(Of Integer))
        mParameterid = pParameterid
        mGroupid = pGroupid
        mParameterstart = pParameterstart
        mParameterend = pParameterend
        mTransactionid = pTransactionid
    End Sub

    Public Sub New( _
        ByVal pParameterid As Integer, _
        ByVal pGroupid As Nullable(Of Integer), _
        ByVal pParameterstart As Nullable(Of DateTime), _
        ByVal pParameterend As Nullable(Of DateTime), _
        ByVal pTransactionid As Nullable(Of Integer), _
        ByVal pTransactioncode As String, _
        ByVal pTransactionvalue As Nullable(Of Double), _
        ByVal pTransactiontype As String)
        mParameterid = pParameterid
        mGroupid = pGroupid
        mParameterstart = pParameterstart
        mParameterend = pParameterend
        mTransactionid = pTransactionid
        mTransactioncode = pTransactioncode
        mTransactionvalue = pTransactionvalue
        mTransactiontype = pTransactiontype
    End Sub

    Public Sub New( _
        ByVal pExtrasCharges As Integer)
        mExtrasCharges = pExtrasCharges
    End Sub

    Public Sub New( _
)
    End Sub

    Private mParameterid As Integer
    Private mExtrasCharges As Integer
    Private mGroupid As Nullable(Of Integer)
    Private mParameterstart As Nullable(Of DateTime)
    Private mParameterend As Nullable(Of DateTime)
    Private mTransactionid As Nullable(Of Integer)
    Private mTransactioncode As String
    Private mTransactiontype As String
    Private mTransactionvalue As Nullable(Of Double)
    
    Public Property ExtrasCharges() As Integer
        Get
            Return mExtrasCharges
        End Get
        Set(ByVal value As Integer)
            mExtrasCharges = value
        End Set
    End Property

    Public Property Parameterid() As Integer
        Get
            Return mParameterid
        End Get
        Set(ByVal value As Integer)
            mParameterid = value
        End Set
    End Property

    Public Property Groupid() As Nullable(Of Integer)
        Get
            Return mGroupid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mGroupid = value
        End Set
    End Property

    Public Property Parameterstart() As Nullable(Of DateTime)
        Get
            Return mParameterstart
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mParameterstart = value
        End Set
    End Property

    Public Property Parameterend() As Nullable(Of DateTime)
        Get
            Return mParameterend
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mParameterend = value
        End Set
    End Property

    Public Property Transactionid() As Nullable(Of Integer)
        Get
            Return mTransactionid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTransactionid = value
        End Set
    End Property

    Public Property Transactiontype() As String
        Get
            Return mTransactiontype
        End Get
        Set(ByVal value As String)
            mTransactiontype = value
        End Set
    End Property

    Public Property Transactioncode() As String
        Get
            Return mTransactioncode
        End Get
        Set(ByVal value As String)
            mTransactioncode = value
        End Set
    End Property

    Public Property Transactionvalue() As Nullable(Of Double)
        Get
            Return mTransactionvalue
        End Get
        Set(ByVal value As Nullable(Of Double))
            mTransactionvalue = value
        End Set
    End Property

    Private Shared Function makeFeedParameterFromRow( _
            ByVal r As IDataReader _
        ) As FeedParameter
        Return New FeedParameter( _
                clsStuff.notWholeNumber(r.Item("parameterid")), _
                toNullableInteger(r.Item("groupid")), _
                toNullableDate(r.Item("parameterstart")), _
                toNullableDate(r.Item("parameterend")), _
                toNullableInteger(r.Item("transactionid")))
    End Function

    Private Shared Function makeFeedParameterFromRow2( _
            ByVal r As IDataReader _
        ) As FeedParameter
        Return New FeedParameter( _
                clsStuff.notWholeNumber(r.Item("parameterid")), _
                toNullableInteger(r.Item("groupid")), _
                toNullableDate(r.Item("parameterstart")), _
                toNullableDate(r.Item("parameterend")), _
                toNullableInteger(r.Item("transactionid")), _
                clsStuff.notString(r.Item("transactioncode")), _
                toNullableFloat(r.Item("transactionvalue")), _
                clsStuff.notString(r.Item("transactiontype")))
    End Function

    Private Shared Function makeFeedParameterFromRowCheck( _
            ByVal r As IDataReader _
        ) As FeedParameter
        Return New FeedParameter( _
                clsStuff.notWholeNumber(r.Item("ExtrasCharges")))
    End Function

    Public Shared Function parameterCheck(ByVal pgroupid As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("FeedParameter_check", _
                                                "@groupid", pgroupid))
            Return intid
        End Using
    End Function

    Public Shared Function listforgroup(ByVal pgroupid As Integer) As List(Of FeedParameter)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedParameter)()
            Using r As IDataReader = dbh.callSP("FeedParameter_grouplist", "@groupid", pgroupid)
                While r.Read()
                    ret.Add(makeFeedParameterFromRow2(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function [get]( _
            ByVal pParameterid As Integer _
        ) As FeedParameter
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedParameter_get", "@parameterid", pParameterid)
                Dim ret As New FeedParameter
                If r.Read() Then
                    ret = makeFeedParameterFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of FeedParameter)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedParameter)()
            Using r As IDataReader = dbh.callSP("FeedParameter_list")
                While r.Read()
                    ret.Add(makeFeedParameterFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mParameterid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedParameter_save", _
                                                      "@Parameterid", mParameterid, _
                                                      "@Groupid", mGroupid, _
                                                      "@Parameterstart", mParameterstart, _
                                                      "@Parameterend", mParameterend, _
                                                      "@Transactionid", mTransactionid))
            Return mParameterid
        End Using
    End Function

    Public Shared Function parameterValueGet(ByVal pstrtype As String, ByVal pstrcode As String, _
                                             ByVal pdtcheckindate As Date, _
                                             ByVal pintgroupid As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("parameterValue_get", _
                                                 "@transactiontype", pstrtype, _
                                                 "@transactioncode", pstrcode, _
                                                 "@checkindate", pdtcheckindate, _
                                                 "@groupid", pintgroupid))
            Return intid
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pParameterid As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedParameter_delete", "@Parameterid", pParameterid)
        End Using
    End Sub

End Class
