Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Web.Script.Services
Imports DatabaseObjects

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<ScriptService()> _
Public Class AjaxService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function venueNameSearch(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsVenue)
        vs = clsVenue.venueNameFind(prefixText)

        Dim items As New List(Of String)

        For Each v As clsVenue In vs
            items.Add(v.itemvalue)
        Next
        Return items.ToArray
    End Function

    <WebMethod()> _
Public Function venueInvoiceSearch(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsVenue)
        vs = clsVenue.venueInvoiceFind(prefixText)

        Dim items As New List(Of String)

        For Each v As clsVenue In vs
            items.Add(v.itemvalue)
        Next
        Return items.ToArray
    End Function

    <WebMethod()> _
Public Function guestPNRSearch(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsVenue)
        vs = clsVenue.guestPNRFind(prefixText)

        Dim items As New List(Of String)

        For Each v As clsVenue In vs
            items.Add(v.itemvalue)
        Next
        Return items.ToArray
    End Function

    <WebMethod()> _
Public Function guestNameSearch(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsVenue)
        vs = clsVenue.guestNameFind(prefixText)

        Dim items As New List(Of String)

        For Each v As clsVenue In vs
            items.Add(v.itemvalue)
        Next
        Return items.ToArray
    End Function
    'R15 CR
    <WebMethod()> _
    Public Function poCodeSearch_LV(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim vs As List(Of FeedImportData)
        vs = FeedImportData.poCodeFind(prefixText, contextKey, "lv=")

        Dim items As New List(Of String)
        For Each v As FeedImportData In vs
            items.Add(v.AICol6)
        Next

        Return items.ToArray
    End Function
    'R15 CR
    <WebMethod()> _
    Public Function poCodeSearch_HerefordshireCouncil(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim vs As List(Of FeedImportData)
        vs = FeedImportData.poCodeFind(prefixText, contextKey, "herefordshire council")

        Dim items As New List(Of String)
        For Each v As FeedImportData In vs
            items.Add(v.AICol6)
        Next

        Return items.ToArray
    End Function
End Class