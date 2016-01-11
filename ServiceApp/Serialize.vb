Imports System.IO
Imports System.Text
Imports System.Xml.Linq
Imports System.Xml.Serialization
'XElement element = myClass.ToXElement<MyClass>();
Public Module Serialize

    <System.Runtime.CompilerServices.Extension> _
    Public Function ToXElement(Of T)(obj As Object) As XElement
        Using memoryStream = New MemoryStream()
            Using streamWriter As TextWriter = New StreamWriter(memoryStream)
                Dim ns As New XmlSerializerNamespaces()
                ns.Add("", "")
                Dim xmlSerializer As New XmlSerializer(GetType(T))
                xmlSerializer.Serialize(streamWriter, obj, ns)

                Return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()))
            End Using
        End Using
    End Function
End Module