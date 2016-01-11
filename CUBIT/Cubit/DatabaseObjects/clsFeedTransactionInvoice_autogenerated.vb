Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
'R17 CR
Partial Public Class clsFeedTransactionInvoice

    Public Sub New( _
        ByVal pTransactionId As Integer, _
        ByVal pInvoiceId As Integer, _
        ByVal pBOSSId As Nullable(Of Integer), _
        ByVal pBOSSTotal As Nullable(Of Double), _
        ByVal pBOSSCommNett As Nullable(Of Double), _
        ByVal pBOSSCommVAT As Nullable(Of Double))
        mTransactionId = pTransactionId
        mInvoiceId = pInvoiceId
        mBOSSId = pBOSSId
        mBOSSTotal = pBOSSTotal
        mBOSSCommNett = pBOSSCommNett
        mBOSSCommVAT = pBOSSCommVAT
    End Sub

    Public Sub New( _
)
    End Sub

    Private mTransactionId As Integer
    Private mInvoiceId As Integer
    Private mBOSSId As Nullable(Of Integer)
    Private mBOSSTotal As Nullable(Of Double)
    Private mBOSSCommNett As Nullable(Of Double)
    Private mBOSSCommVAT As Nullable(Of Double)

    Public Property TransactionId() As Integer
        Get
            Return mTransactionId
        End Get
        Set(ByVal value As Integer)
            mTransactionId = value
        End Set
    End Property

    Public Property InvoiceId() As Integer
        Get
            Return mInvoiceId
        End Get
        Set(ByVal value As Integer)
            mInvoiceId = value
        End Set
    End Property

    Public Property BOSSId() As Nullable(Of Integer)
        Get
            Return mBOSSId
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mBOSSId = value
        End Set
    End Property

    Public Property BOSSTotal() As Nullable(Of Double)
        Get
            Return mBOSSTotal
        End Get
        Set(ByVal value As Nullable(Of Double))
            mBOSSTotal = value
        End Set
    End Property

    Public Property BOSSCommNett() As Nullable(Of Double)
        Get
            Return mBOSSCommNett
        End Get
        Set(ByVal value As Nullable(Of Double))
            mBOSSCommNett = value
        End Set
    End Property

    Public Property BOSSCommVAT() As Nullable(Of Double)
        Get
            Return mBOSSCommVAT
        End Get
        Set(ByVal value As Nullable(Of Double))
            mBOSSCommVAT = value
        End Set
    End Property

    Private Shared Function makeFeedTransactionInvoiceFromRow( _
            ByVal r As IDataReader _
        ) As clsFeedTransactionInvoice
        Return New clsFeedTransactionInvoice( _
                clsStuff.notWholeNumber(r.Item("TransactionId")), _
                clsStuff.notWholeNumber(r.Item("InvoiceId")), _
                toNullableInteger(r.Item("BOSSId")), _
                toNullableFloat(r.Item("BOSSTotal")), _
                toNullableFloat(r.Item("BOSSCommNett")), _
                toNullableFloat(r.Item("BOSSCommVAT")))
    End Function

    Public Shared Function getTrans( _
            ByVal pTransactionId As Integer, _
            ByVal pInvoiceId As Integer _
        ) As clsFeedTransactionInvoice
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedTransactionInvoice_get", "@TransactionId", pTransactionId, "@InvoiceId", pInvoiceId)
                If Not r.Read() Then
                    Throw New Exception("No FeedTransactionInvoice with id " & pTransactionId)
                End If
                Dim ret As clsFeedTransactionInvoice = makeFeedTransactionInvoiceFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function listTrans() As List(Of clsFeedTransactionInvoice)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsFeedTransactionInvoice)()
            Using r As IDataReader = dbh.callSP("FeedTransactionInvoice_list")
                While r.Read()
                    ret.Add(makeFeedTransactionInvoiceFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function saveTrans(ByVal pintTransactionID As Integer, _
                                ByVal pintInvoiceID As Integer, _
                                ByVal pstrBOSSId As String, _
                                ByVal pdblBOSSTotal As Double, _
                                ByVal pdblBOSSCommNett As Double, _
                                ByVal pdblBOSSCommVAT As Double, _
                                ByVal pblnAllowUpdate As Boolean) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer
            ret = CInt(dbh.callSPSingleValue("FeedTransactionInvoice_save", _
                                                        "@TransactionId", pintTransactionID, _
                                                        "@InvoiceId", pintInvoiceID, _
                                                        "@BOSSId", pstrBOSSId, _
                                                        "@BOSSTotal", pdblBOSSTotal, _
                                                        "@BOSSCommNett", pdblBOSSCommNett, _
                                                        "@BOSSCommVAT", pdblBOSSCommVAT, _
                                                        "@AllowUpdate", pblnAllowUpdate))
            Return ret
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pTransactionId As Integer, _
            ByVal pInvoiceId As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedTransactionInvoice_delete", "@TransactionId", pTransactionId, "@InvoiceId", pInvoiceId)
        End Using
    End Sub

    Public Sub delete()
        delete(mTransactionId, mInvoiceId)
    End Sub

End Class
