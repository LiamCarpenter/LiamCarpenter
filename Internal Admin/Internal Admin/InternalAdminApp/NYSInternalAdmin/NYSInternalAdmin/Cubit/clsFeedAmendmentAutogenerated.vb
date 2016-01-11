Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

<Assembly: CLSCompliant(True)> 
''' <summary>
''' Class FeedAmendment - Accesses database to add/edit/list records in FeedAmendment table
''' </summary>
''' <remarks>
''' Created 16/03/2009 Nick Massarella
''' </remarks>
Partial Public Class FeedAmendment

    ''' <summary>
    ''' Instantiate class with set parameters
    ''' </summary>
    ''' <param name="pAmendmentid"></param>
    ''' <param name="pSystemnysuserid"></param>
    ''' <param name="pDataid"></param>
    ''' <param name="pAmendmentdate"></param>
    ''' <remarks>Created 16/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal pAmendmentid As Integer, _
        ByVal pSystemnysuserid As Nullable(Of Integer), _
        ByVal pDataid As Nullable(Of Integer), _
        ByVal pAmendmentdate As Nullable(Of DateTime))
        mAmendmentid = pAmendmentid
        mSystemnysuserid = pSystemnysuserid
        mDataid = pDataid
        mAmendmentdate = pAmendmentdate
    End Sub

    ''' <summary>
    ''' Instantiate class
    ''' </summary>
    ''' <remarks>Created 16/03/2009 Nick Massarella</remarks>
    Public Sub New( _
)
    End Sub

    Private mAmendmentid As Integer
    Private mSystemnysuserid As Nullable(Of Integer)
    Private mDataid As Nullable(Of Integer)
    Private mAmendmentdate As Nullable(Of DateTime)
    
    Public Property Amendmentid() As Integer
        Get
            Return mAmendmentid
        End Get
        Set(ByVal value As Integer)
            mAmendmentid = value
        End Set
    End Property

    Public Property Systemnysuserid() As Nullable(Of Integer)
        Get
            Return mSystemnysuserid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mSystemnysuserid = value
        End Set
    End Property

    Public Property Dataid() As Nullable(Of Integer)
        Get
            Return mDataid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mDataid = value
        End Set
    End Property

    Public Property Amendmentdate() As Nullable(Of DateTime)
        Get
            Return mAmendmentdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mAmendmentdate = value
        End Set
    End Property

    ''' <summary>
    ''' Function makeFeedAmendmentFromRow - creates a collection of FeedAmendment records
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns>
    ''' FeedAmendment table row as a single record
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Private Shared Function makeFeedAmendmentFromRow( _
            ByVal r As IDataReader _
        ) As FeedAmendment
        Return New FeedAmendment( _
                clsStuff.notWholeNumber(r.Item("amendmentid")), _
                toNullableInteger(r.Item("systemnysuserid")), _
                toNullableInteger(r.Item("dataid")), _
                toNullableDate(r.Item("amendmentdate")))
    End Function

    ''' <summary>
    ''' Function [get] - retrieves a FeedAmendment record from the database
    ''' </summary>
    ''' <param name="pAmendmentid"></param>
    ''' <returns>
    ''' A single record from FeedAmendment table
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function [get]( _
            ByVal pAmendmentid As Integer _
        ) As FeedAmendment
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedAmendment_get", "@amendmentid", pAmendmentid)
                Dim ret As New FeedAmendment
                If r.Read() Then
                    ret = makeFeedAmendmentFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function list- retrieves a list of FeedAmendment records from the database
    ''' </summary>
    ''' <returns>
    ''' A list of records from FeedAmendment table
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function list() As List(Of FeedAmendment)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedAmendment)()
            Using r As IDataReader = dbh.callSP("FeedAmendment_list")
                While r.Read()
                    ret.Add(makeFeedAmendmentFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' Function save - saves a FeedAmendment record to the databse
    ''' </summary>
    ''' <returns>
    ''' Newly added or amended FeedAmendmentID
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mAmendmentid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedAmendment_save", _
                                                      "@Amendmentid", mAmendmentid, _
                                                      "@Systemnysuserid", mSystemnysuserid, _
                                                      "@Dataid", mDataid, _
                                                      "@Amendmentdate", mAmendmentdate))
            Return mAmendmentid
        End Using
    End Function

End Class
