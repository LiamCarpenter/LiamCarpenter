Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class clsPO_New

    Public Sub New( _
     ByVal pRequesterName As String)
        mRequesterName = pRequesterName
    End Sub

    Public Sub New( _
)
    End Sub

    Private mRequesterName As String

    Public Property RequesterName() As String
        Get
            Return mRequesterName
        End Get
        Set(ByVal value As String)
            mRequesterName = value
        End Set
    End Property

    Private Shared Function makeSummaryFromRow( _
           ByVal r As IDataReader _
           ) As clsPO_New
        Return New clsPO_New( _
            clsNYS.notString(r.Item("RequesterName")))
    End Function

    Public Shared Function RequesterNameList() As List(Of clsPO_New)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsPO_New)()
            Using r As IDataReader = dbh.callSP("O2RequesterName_List")
                While r.Read()
                    ret.Add(makeSummaryFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

End Class


