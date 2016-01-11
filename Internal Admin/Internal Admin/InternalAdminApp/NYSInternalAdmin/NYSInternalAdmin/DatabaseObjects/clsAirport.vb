Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
'Imports MySql.Data.MySqlClient

Partial Public Class clsAirport

    Public Sub New( _
    ByVal pAirportCodeID As Integer, _
    ByVal pAirportCode As String, _
    ByVal pAirportName As String, _
    ByVal pCountryName As String, _
    ByVal pAirportCity As String)
        mAirportCodeID = pAirportCodeID
        mAirportCode = pAirportCode
        mAirportName = pAirportName
        mCountryName = pCountryName
        mAirportCity = pAirportCity
    End Sub

    Public Sub New( _
        ByVal pairlineCodeID As Integer, _
        ByVal pIATACode As String, _
        ByVal pairlineName As String, _
        ByVal pairlineCountry As String)
        mairlineCodeID = pairlineCodeID
        mIATACode = pIATACode
        mairlineName = pairlineName
        mairlineCountry = pairlineCountry
    End Sub

    Public Sub New( _
    ByVal pproductID As Integer, _
    ByVal pproductDesc As String)
        mproductID = pproductID
        mproductDesc = pproductDesc
    End Sub

    Public Sub New( _
)
    End Sub

    Private mairlineCodeID As Integer
    Private mIATACode As String
    Private mairlineName As String
    Private mairlineCountry As String
    Private mproductID As Integer
    Private mproductDesc As String
    Private mAirportCodeID As Integer
    Private mAirportCode As String
    Private mAirportName As String
    Private mCountryName As String
    Private mAirportCity As String

    Public Property IATACode() As String
        Get
            Return mIATACode
        End Get
        Set(ByVal value As String)
            mIATACode = value
        End Set
    End Property

    Public Property airlineName() As String
        Get
            Return mairlineName
        End Get
        Set(ByVal value As String)
            mairlineName = value
        End Set
    End Property

    Public Property airlineCountry() As String
        Get
            Return mairlineCountry
        End Get
        Set(ByVal value As String)
            mairlineCountry = value
        End Set
    End Property

    Public Property airlineCodeID() As Integer
        Get
            Return mairlineCodeID
        End Get
        Set(ByVal value As Integer)
            mairlineCodeID = value
        End Set
    End Property

    Public Property productID() As Integer
        Get
            Return mproductID
        End Get
        Set(ByVal value As Integer)
            mproductID = value
        End Set
    End Property

    Public Property AirportCity() As String
        Get
            Return mAirportCity
        End Get
        Set(ByVal value As String)
            mAirportCity = value
        End Set
    End Property

    Public Property productDesc() As String
        Get
            Return mproductDesc
        End Get
        Set(ByVal value As String)
            mproductDesc = value
        End Set
    End Property

    Public Property AirportCodeID() As Integer
        Get
            Return mAirportCodeID
        End Get
        Set(ByVal value As Integer)
            mAirportCodeID = value
        End Set
    End Property

    Public Property AirportCode() As String
        Get
            Return mAirportCode
        End Get
        Set(ByVal value As String)
            mAirportCode = value
        End Set
    End Property

    Public Property AirportName() As String
        Get
            Return mAirportName
        End Get
        Set(ByVal value As String)
            mAirportName = value
        End Set
    End Property

    Public Property CountryName() As String
        Get
            Return mCountryName
        End Get
        Set(ByVal value As String)
            mCountryName = value
        End Set
    End Property

    Private Shared Function makeSummaryFromRow( _
           ByVal r As IDataReader _
           ) As clsAirport
        Return New clsAirport( _
            CInt(clsNYS.notNumber(r.Item("AirportCodeID"))), _
            clsNYS.notString(r.Item("AirportCode")), _
            clsNYS.notString(r.Item("AirportName")), _
            clsNYS.notString(r.Item("CountryName")), _
            clsNYS.notString(r.Item("AirportCity")))
    End Function

    Private Shared Function makeSummaryAirLineFromRow( _
           ByVal r As IDataReader _
           ) As clsAirport
        Return New clsAirport( _
            CInt(clsNYS.notNumber(r.Item("airlineCodeID"))), _
            clsNYS.notString(r.Item("IATACode")), _
            clsNYS.notString(r.Item("airlineName")), _
            clsNYS.notString(r.Item("airlineCountry")))
    End Function

    Private Shared Function makeSummaryFromRow2( _
           ByVal r As IDataReader _
           ) As clsAirport
        Return New clsAirport( _
            CInt(clsNYS.notNumber(r.Item("productID"))), _
            clsNYS.notString(r.Item("productDesc")))
    End Function


    'Public Shared Function tester(ByVal plocationname As String) As List(Of clsAirport)
    '    Dim conn As New MySqlConnection
    '    Dim myCommand As New MySqlCommand
    '    Dim myAdapter As New MySqlDataAdapter
    '    Dim myData As New DataTable

    '    conn.ConnectionString = "server=localhost;user id=root;password=Evosnozz123;database=test"
    '    conn.Open()
    '    myCommand.Connection = conn
    '    myCommand.CommandText = "select * from products"

    '    Dim drCommandResults As MySqlDataReader = myCommand.ExecuteReader()
    '    Dim ret As New List(Of clsAirport)()
    '    ' Should be 1 and only 1 record returned
    '    While drCommandResults.Read
    '        ret.Add(makeSummaryFromRow2(drCommandResults))
    '        ' Get the new ID
    '        ' intTable1ID = Convert.ToInt32(drCommandResults.Item(0))
    '    End While
    '    Return ret
    'End Function

    'Public Shared Function testerUpload() As Integer
    '    Dim conn As New MySqlConnection
    '    Dim myCommand As New MySqlCommand
    '    Dim myAdapter As New MySqlDataAdapter
    '    Dim myData As New DataTable

    '    conn.ConnectionString = "server=localhost;user id=root;password=Evosnozz123;database=test"
    '    conn.Open()
    '    myCommand.Connection = conn
    '    myCommand.CommandText = "insert products values(0,'rabbit biscuits')"
    '    myCommand.ExecuteNonQuery()
    '    'Dim drCommandResults As MySqlDataReader = myCommand.ExecuteReader()
    '    'Dim ret As New List(Of clsAirport)()
    '    '' Should be 1 and only 1 record returned
    '    'If drCommandResults.Read Then
    '    '    ret.Add(makeSummaryFromRow2(drCommandResults))
    '    '    ' Get the new ID
    '    '    ' intTable1ID = Convert.ToInt32(drCommandResults.Item(0))
    '    'End If
    '    'Return ret
    'End Function

    'Public Shared Function testerUpdate() As Integer
    '    Dim conn As New MySqlConnection
    '    Dim myCommand As New MySqlCommand
    '    Dim myAdapter As New MySqlDataAdapter
    '    Dim myData As New DataTable

    '    conn.ConnectionString = "server=localhost;user id=root;password=Evosnozz123;database=test"
    '    conn.Open()
    '    myCommand.Connection = conn
    '    myCommand.CommandText = "update products set productDesc = 'rabbit biscuits' where productID =1"
    '    myCommand.ExecuteNonQuery()
    '    'Dim drCommandResults As MySqlDataReader = myCommand.ExecuteReader()
    '    'Dim ret As New List(Of clsAirport)()
    '    '' Should be 1 and only 1 record returned
    '    'If drCommandResults.Read Then
    '    '    ret.Add(makeSummaryFromRow2(drCommandResults))
    '    '    ' Get the new ID
    '    '    ' intTable1ID = Convert.ToInt32(drCommandResults.Item(0))
    '    'End If
    '    'Return ret
    'End Function

    Public Shared Function airLineFind(ByVal pairline As String) As List(Of clsAirport)
        Using dbh As New SqlDatabaseHandle(getGIDSConnection)
            Dim ret As New List(Of clsAirport)()
            Using r As IDataReader = dbh.callSP("airline_list", _
                                                "@airline", pairline)
                While r.Read()
                    ret.Add(makeSummaryAirLineFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function airportFind(ByVal plocationname As String) As List(Of clsAirport)
        Using dbh As New SqlDatabaseHandle(getGIDSConnection)
            Dim ret As New List(Of clsAirport)()
            Using r As IDataReader = dbh.callSP("airport_list", _
                                                "@airportname", plocationname)
                While r.Read()
                    ret.Add(makeSummaryFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function airportFindInt(ByVal plocationname As String) As List(Of clsAirport)
        Using dbh As New SqlDatabaseHandle(getGIDSConnection)
            Dim ret As New List(Of clsAirport)()
            Using r As IDataReader = dbh.callSP("airportInt_list", _
                                                "@airportname", plocationname)
                While r.Read()
                    ret.Add(makeSummaryFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function
End Class

