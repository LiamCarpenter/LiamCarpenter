Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports NysDat
Imports System.Web.Script.Services

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<ScriptService()> _
Public Class AjaxService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function locationSearch(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsLocation)
        vs = clsLocation.locationFind(prefixText)

        Dim items As New List(Of String)

        For Each v As clsLocation In vs
            items.Add(v.LocationName)
        Next
        Return items.ToArray
    End Function

    <WebMethod()> _
     Public Function airportSearchInt(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsAirport)
        vs = clsAirport.airportFindInt(prefixText)

        Dim items As New List(Of String)

        For Each v As clsAirport In vs
            items.Add(v.AirportName)
        Next
        Return items.ToArray
    End Function

    <WebMethod()> _
    Public Function airportSearch(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsAirport)
        vs = clsAirport.airportFind(prefixText)

        Dim items As New List(Of String)

        For Each v As clsAirport In vs
            If v.AirportCode = "" Then
                If v.AirportCity <> "" Then
                    items.Add(v.AirportCity & ":" & v.CountryName)
                End If
            ElseIf v.AirportCode = "a" Then
                items.Add(v.CountryName)
            Else
                items.Add(v.AirportName & "(" & v.AirportCode & ")" & "-" & v.AirportCity & ":" & v.CountryName)
            End If

        Next
        Return items.ToArray
    End Function

    <WebMethod()> _
    Public Function airlineSearch(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim vs As List(Of clsAirport)
        vs = clsAirport.airLineFind(prefixText)

        Dim items As New List(Of String)

        For Each v As clsAirport In vs
            items.Add(v.airlineName)
        Next
        Return items.ToArray
    End Function

End Class