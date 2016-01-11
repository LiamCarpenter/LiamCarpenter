Option Strict On
Option Explicit On 






Public Class XmlIterator
    Private oProcessor As clsXMLProcessor
    Private oIterator As System.Xml.XPath.XPathNodeIterator
    Public oNode As System.Xml.XmlNode

    Public Sub New(ByVal xml As String, ByVal path As String)
        oProcessor = New clsXMLProcessor
        oProcessor.loadXML(xml, path)
        oIterator = oProcessor.getIterator()
    End Sub

    Public Function nodeName() As String
        Return oNode.Name
    End Function

    Public Function MoveNext() As Boolean
        Dim ret As Boolean = oIterator.MoveNext
        oNode = CType(oIterator.Current, System.Xml.IHasXmlNode).GetNode()
        Return ret
    End Function

    Public Function innerText(ByVal xpath As String) As String
        If oNode.SelectSingleNode(xpath) Is Nothing Then
            Return ""
        Else
            Return oNode.SelectSingleNode(xpath).InnerText
        End If
    End Function
    Public Function innerTextOrNothing(ByVal xpath As String) As String
        If oNode.SelectSingleNode(xpath) Is Nothing Then
            Return Nothing
        Else
            Return oNode.SelectSingleNode(xpath).InnerText
        End If
    End Function
    Public Function innerIntOrNothing(ByVal xpath As String) As Integer
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return Nothing
        Else
            Return CInt(t)
        End If
    End Function
    Public Function innerStringOrNothing(ByVal xpath As String) As String
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return Nothing
        Else
            Return t
        End If
    End Function
    Public Function innerDateTimeOrNothing(ByVal xpath As String) As DateTime
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return Nothing
        Else
            Return DateTime.Parse(t)
        End If
    End Function
    Public Function innerBoolOrNothing(ByVal xpath As String) As Boolean
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return Nothing
        Else
            Return CBool(t)
        End If
    End Function
    Public Function innerSingleOrNothing(ByVal xpath As String) As Single
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return Nothing
        Else
            Return CSng(t)
        End If
    End Function

    Public Function innerInt(ByVal xpath As String) As Integer
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return 0
        Else
            Return CInt(t)
        End If
    End Function

    Public Function innerLong(ByVal xpath As String) As Long
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return 0
        Else
            Return CLng(t)
        End If
    End Function

    Public Function innerBool(ByVal xpath As String) As Boolean
        Dim t As String = innerText(xpath)
        If t = "" Then
            Return False
        Else
            Return CBool(t)
        End If
    End Function
End Class

Public Class clsXMLProcessor

    Dim iterator, innerIterator As System.Xml.XPath.XPathNodeIterator
    Dim nav, innerNav As System.Xml.XPath.XPathNavigator
    Dim exp, innerExp As System.Xml.XPath.XPathExpression

    Dim oXML As System.Xml.XmlDocument

    Public Function loadXML(ByVal pstsXML As String, ByVal pstrXPath As String) As Boolean

        oXML = New System.Xml.XmlDocument
        oXML.LoadXml(pstsXML)
        nav = oXML.CreateNavigator()
        innerExp = nav.Compile(pstrXPath)
        Return True
    End Function

    Public Function loadXML(ByVal pstsXML As String, ByVal pstrXPath As String, ByVal pstrSearch As String) As Boolean

        oXML = New System.Xml.XmlDocument
        oXML.LoadXml(pstsXML)
        nav = oXML.CreateNavigator()
        innerExp = nav.Compile(pstrXPath)
        innerExp.AddSort(pstrSearch, System.Collections.Comparer.Default)

        Return True
    End Function

    Public Function getIterator() As System.Xml.XPath.XPathNodeIterator
        Return nav.Select(innerExp)
    End Function

    Public Function getXMLObject() As System.Xml.XmlDocument
        Return oXML
    End Function

    Protected Overrides Sub Finalize()
        'close objects
        oXML = Nothing
        nav = Nothing
        innerExp = Nothing
        MyBase.Finalize()
    End Sub
End Class

