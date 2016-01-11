Option Strict On
Option Explicit On

Imports System.Xml

Public Module XmlUtils

    Public Function selectText(ByVal xml As String, ByVal xpath As String) As String

        Dim oXML As New System.Xml.XmlDocument
        oXML.LoadXml(xml)

        Dim xn As XmlNode = oXML.SelectSingleNode(xpath)
        If xn Is Nothing Then
            Throw New Exception("Xpath not found: " & xpath)
        End If
        Return xn.InnerText
    End Function

    Public Function selectXmlNodeList(ByVal xml As String, ByVal xpath As String) As XmlNodeList
        Dim oXML As New System.Xml.XmlDocument
        oXML.LoadXml(xml)
        Return oXML.SelectNodes(xpath)
    End Function

End Module
