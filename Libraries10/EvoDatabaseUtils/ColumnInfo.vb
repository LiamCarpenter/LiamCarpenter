Imports EvoUtilities.Utils
Imports EvoUtilities.CollectionUtils

Public Structure ColumnInfo

    Public Enum ColumnType
        Bit
        [Char]
        DateTime
        Float
        Image
        NVarChar
        SysName
        TimeStamp
        VarChar
        Int
        [Decimal]
        Numeric
    End Enum

    Public name As String
    Public type As ColumnType
    Public length As Integer
    Public isNullable As Boolean
    Public isIdentity As Boolean

    Public Sub New(ByVal pName As String, ByVal pType As ColumnType, ByVal plength As Integer, _
        Optional ByVal pIsNullable As Boolean = True, Optional ByVal pIsIdentity As Boolean = False)
        name = pName
        type = pType
        length = plength
        isNullable = pIsNullable
        isIdentity = pIsIdentity
    End Sub

    Public Shared Function isLengthType(ByVal type As ColumnType) As Boolean
        Return inList(type, ColumnType.Bit, ColumnType.Char, ColumnType.Image, ColumnType.NVarChar, _
                        ColumnType.SysName, ColumnType.VarChar)
    End Function

    Public Function dbType() As String
        'return the type with the length iff sql server wants to see the length
        If isLengthType(type) Then
            Return type.ToString.ToLowerInvariant() & "(" & length & ")" & _
                giif(isNullable, "", " not null")
        Else
            Return type.ToString.ToLowerInvariant & _
                              giif(isNullable, "", " not null")
        End If
    End Function

    Private Function nullableString() As String
        Return giif(isNullable, "null", "not null")
    End Function

    Public Overrides Function equals(ByVal obj As Object) As Boolean
        If obj.GetType Is GetType(ColumnInfo) Then
            Dim o1 As ColumnInfo = DirectCast(obj, ColumnInfo)
            Return o1.name = name And o1.type = type And _
                o1.length = length And o1.isNullable = isNullable _
                And o1.isIdentity = isIdentity
        End If
        Return False
    End Function

    Public Overrides Function toString() As String
        Return "ColumnInfo: " & name & " " & type & "(" & length & ")" & _
            giif(isNullable, "", " not null") & giif(isIdentity, " identity", "")
    End Function

End Structure
