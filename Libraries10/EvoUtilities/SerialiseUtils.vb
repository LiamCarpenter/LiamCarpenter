Imports System.Xml.Serialization
Imports system.io

Public Class SerialiseUtils

    Public Shared Function serialiseToXml(ByVal o As Object) As String
        Dim x As New XmlSerializer(o.GetType)
        Dim s As New StringWriter
        x.Serialize(s, o)
        Return s.ToString
    End Function

    Public Shared Function deserialiseFromXml(Of T As New)(ByVal xml As String) As T
        Dim ret As New T
        Dim x As New XmlSerializer(ret.GetType)
        Dim s As New StringReader(xml)
        ret = DirectCast(x.Deserialize(s), T)
        Return ret
    End Function


End Class
