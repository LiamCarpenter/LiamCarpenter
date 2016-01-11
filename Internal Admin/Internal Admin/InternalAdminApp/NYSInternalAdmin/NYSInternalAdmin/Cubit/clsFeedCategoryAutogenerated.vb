Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

''' <summary>
''' Class FeedCategory - Accesses the database to add/edit/list records in FeedCategory table
''' </summary>
''' <remarks>
''' Created 12/03/2009 Nick Massarella
''' </remarks>
Partial Public Class FeedCategory

    ''' <summary>
    ''' Instantiate class with set parameters
    ''' </summary>
    ''' <param name="pCategoryid"></param>
    ''' <param name="pCategorycode"></param>
    ''' <param name="pCategoryname"></param>
    ''' <param name="pCategorybosscode"></param>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal pCategoryid As Integer, _
        ByVal pCategorycode As Nullable(Of Integer), _
        ByVal pCategoryname As String, _
        ByVal pCategorybosscode As String)
        mCategoryid = pCategoryid
        mCategorycode = pCategorycode
        mCategoryname = pCategoryname
        mCategorybosscode = pCategorybosscode
    End Sub

    ''' <summary>
    ''' Instantiate class
    ''' </summary>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Sub New( _
)
    End Sub

    Private mCategoryid As Integer
    Private mCategorycode As Nullable(Of Integer)
    Private mCategoryname As String
    Private mCategorybosscode As String
    
    Public Property Categoryid() As Integer
        Get
            Return mCategoryid
        End Get
        Set(ByVal value As Integer)
            mCategoryid = value
        End Set
    End Property

    Public Property Categorycode() As Nullable(Of Integer)
        Get
            Return mCategorycode
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mCategorycode = value
        End Set
    End Property

    Public Property Categoryname() As String
        Get
            Return mCategoryname
        End Get
        Set(ByVal value As String)
            mCategoryname = value
        End Set
    End Property

    Public Property Categorybosscode() As String
        Get
            Return mCategorybosscode
        End Get
        Set(ByVal value As String)
            mCategorybosscode = value
        End Set
    End Property

    ''' <summary>
    ''' Function makeFeedCategoryFromRow - creates a FeedCategory list
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns>
    ''' FeedAmendment table row as a single record
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Private Shared Function makeFeedCategoryFromRow( _
            ByVal r As IDataReader _
        ) As FeedCategory
        Return New FeedCategory( _
                clsStuff.notWholeNumber(r.Item("categoryid")), _
                toNullableInteger(r.Item("categorycode")), _
                clsStuff.notString(r.Item("categoryname")), _
                clsStuff.notString(r.Item("categorybosscode")))
    End Function

    ''' <summary>
    '''Function [get]- retrieves a FeedCategory record from the database
    ''' </summary>
    ''' <param name="pCategoryid"></param>
    ''' <returns>
    ''' A single record from FeedCategory table
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function [get]( _
            ByVal pCategoryid As Integer _
        ) As FeedCategory
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedCategory_get", "@categoryid", pCategoryid)
                Dim ret As New FeedCategory
                If r.Read() Then
                    ret = makeFeedCategoryFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function list- retrieves a list FeedCategory records from the database
    ''' </summary>
    ''' <returns>
    ''' A list of records from FeedAmendment table
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function list() As List(Of FeedCategory)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedCategory)()
            Using r As IDataReader = dbh.callSP("FeedCategory_list")
                While r.Read()
                    ret.Add(makeFeedCategoryFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' Function save - saves a FeedCategory record to the database
    ''' </summary>
    ''' <returns>
    ''' Newly added or amended FeedAmendmentID
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mCategoryid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedCategory_save", _
                                                     "@Categoryid", mCategoryid, _
                                                     "@Categorycode", mCategorycode, _
                                                     "@Categoryname", mCategoryname, _
                                                     "@Categorybosscode", mCategorybosscode))
            Return mCategoryid
        End Using
    End Function

    ''' <summary>
    ''' Sub delete - Deletes selected FeedCategory record from database 
    ''' </summary>
    ''' <param name="pCategoryid"></param>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Sub delete( _
            ByVal pCategoryid As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedCategory_delete", "@Categoryid", pCategoryid)
        End Using
    End Sub

    ''' <summary>
    ''' Function getCatergoryIDFromName
    ''' </summary>
    ''' <param name="pCatergoryname"></param>
    ''' <returns>The unique ID for the passed in value</returns>
    ''' <remarks>Created 12/03/2009 Nick Massarella</remarks>
    Public Shared Function getCatergoryIDFromName( _
            ByVal pCatergoryname As String _
        ) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intID As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("category_getIDfromName", _
                                                            "@Categoryname", pCatergoryname))
            Return intID
        End Using
    End Function

End Class
