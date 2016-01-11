Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

''' <summary>
''' Class clsSystemNYSUser
''' </summary>
''' <remarks>
''' Created 12/03/2009 Nick Massarella
''' Class retrieves currently logged in user details from their Windows login
'''  to allow access to application and record against amendments of records
''' </remarks>
<Serializable()> _
Partial Public Class clsSystemNYSUser

    Public Sub New( _
        ByVal pSystemnysuserid As Integer, _
        ByVal pSystemnysuserfirstname As String, _
        ByVal pSystemnysuserlastname As String, _
        ByVal pSystemnysuserloginname As String, _
        ByVal pSystemnysuserinitials As String, _
        ByVal pSystemnysusertel As String, _
        ByVal pSystemnysuseremail As String, _
        ByVal pSystemnysusergroup As String, _
        ByVal pSystemnysuserInactive As Nullable(Of Boolean))
        mSystemnysuserid = pSystemnysuserid
        mSystemnysuserfirstname = pSystemnysuserfirstname
        mSystemnysuserlastname = pSystemnysuserlastname
        mSystemnysuserloginname = pSystemnysuserloginname
        mSystemnysuserinitials = pSystemnysuserinitials
        mSystemnysusertel = pSystemnysusertel
        mSystemnysuseremail = pSystemnysuseremail
        mSystemnysusergroup = pSystemnysusergroup
        mSystemnysuserInactive = pSystemnysuserInactive
    End Sub

    Public Sub New( _
)
    End Sub

    Private mSystemnysuserid As Integer
    Private mSystemnysuserfirstname As String
    Private mSystemnysuserlastname As String
    Private mSystemnysuserloginname As String
    Private mSystemnysuserinitials As String
    Private mSystemnysusertel As String
    Private mSystemnysuseremail As String
    Private mSystemnysusergroup As String
    Private mSystemnysuserInactive As Nullable(Of Boolean)

    Public Property Systemnysuserid() As Integer
        Get
            Return mSystemnysuserid
        End Get
        Set(ByVal value As Integer)
            mSystemnysuserid = value
        End Set
    End Property

    Public Property Systemnysuserfirstname() As String
        Get
            Return mSystemnysuserfirstname
        End Get
        Set(ByVal value As String)
            mSystemnysuserfirstname = value
        End Set
    End Property

    Public Property Systemnysuserlastname() As String
        Get
            Return mSystemnysuserlastname
        End Get
        Set(ByVal value As String)
            mSystemnysuserlastname = value
        End Set
    End Property

    Public Property Systemnysuserloginname() As String
        Get
            Return mSystemnysuserloginname
        End Get
        Set(ByVal value As String)
            mSystemnysuserloginname = value
        End Set
    End Property

    Public Property Systemnysuserinitials() As String
        Get
            Return mSystemnysuserinitials
        End Get
        Set(ByVal value As String)
            mSystemnysuserinitials = value
        End Set
    End Property

    Public Property Systemnysusertel() As String
        Get
            Return mSystemnysusertel
        End Get
        Set(ByVal value As String)
            mSystemnysusertel = value
        End Set
    End Property

    Public Property Systemnysuseremail() As String
        Get
            Return mSystemnysuseremail
        End Get
        Set(ByVal value As String)
            mSystemnysuseremail = value
        End Set
    End Property

    Public Property Systemnysusergroup() As String
        Get
            Return mSystemnysusergroup
        End Get
        Set(ByVal value As String)
            mSystemnysusergroup = value
        End Set
    End Property

    Public Property SystemnysuserInactive() As Nullable(Of Boolean)
        Get
            Return mSystemnysuserInactive
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSystemnysuserInactive = value
        End Set
    End Property

    ''' <summary>
    ''' Function makeSystemnysuserFromRow
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns>
    ''' Current logged in user details as a single record
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Private Shared Function makeSystemnysuserFromRow( _
            ByVal r As IDataReader _
        ) As clsSystemNYSUser
        Return New clsSystemNYSUser( _
                clsStuff.notWholeNumber(r.Item("systemnysuserid")), _
                clsStuff.notString(r.Item("systemnysuserfirstname")), _
                clsStuff.notString(r.Item("systemnysuserlastname")), _
                clsStuff.notString(r.Item("systemnysuserloginname")), _
                clsStuff.notString(r.Item("systemnysuserinitials")), _
                clsStuff.notString(r.Item("systemnysusertel")), _
                clsStuff.notString(r.Item("systemnysuseremail")), _
                clsStuff.notString(r.Item("systemnysusergroup")), _
                toNullableBoolean(r.Item("systemnysuserInactive")))
    End Function

    ''' <summary>
    ''' Function Populate
    ''' </summary>
    ''' <param name="pSystemnysuserloginname"></param>
    ''' <returns>
    ''' All logged in user details
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function Populate(ByVal pSystemnysuserloginname As String) As List(Of clsSystemNYSUser)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsSystemNYSUser)()
            Using r As IDataReader = dbh.callSP("systemnysuser_get", _
                                                "@systemnysuserloginname", pSystemnysuserloginname, _
                                                "@databasename", getConfig("databasename"))
                While r.Read()
                    ret.Add(makeSystemnysuserFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

End Class

