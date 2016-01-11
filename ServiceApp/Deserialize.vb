Imports System.Xml
Imports System.Xml.Serialization
'element.CreateReader().Deserialize(Of object)()
Module Deserialize

    <System.Runtime.CompilerServices.Extension> _
    Public Function Deserialize(Of T)(xml As XmlReader) As T

        If xml Is Nothing Then
            Return Nothing
        End If

        Dim serializer = New XmlSerializer(GetType(T))

        Dim settings = New XmlReaderSettings()
        ' No settings need modifying here

        Return DirectCast(serializer.Deserialize(xml), T)

    End Function

End Module