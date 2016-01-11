Option Strict On
Option Explicit On

Imports System.Reflection
Imports System.Collections.Generic
Imports System
Imports System.Text.RegularExpressions
Imports System.Web
Imports Microsoft.VisualBasic
Imports System.Web.SessionState
Imports System.Web.UI.WebControls

Namespace NSUtils
    <Serializable()> _
    Public Module Utils

        Private ReadOnly IntegerRegex As New Regex("^\s*\d+\s*$")
        Private ReadOnly FloatRegex As New Regex("^\s*\d+(\.\d+)?\s*$")

        Public Function isInteger(ByVal s As String) As Boolean
            Return IntegerRegex.IsMatch(s)
        End Function

        Public Function isFloat(ByVal s As String) As Boolean
            Return FloatRegex.IsMatch(s)
        End Function

        Public Function startOfThisMonth() As String
            Dim d As Date = Date.Now
            Return New Date(d.Year, d.Month, 1).ToString("dd/MM/yyyy")
        End Function

        Public Function endOfThisMonth() As String
            Dim d As Date = Date.Now
            Return New Date(d.Year, d.Month, Date.DaysInMonth(d.Year, d.Month)).ToString("dd/MM/yyyy")
        End Function

        Public Function startOfLastMonth() As String
            Dim d As Date = Date.Now
            Dim d1 As Date = New Date(d.Year, d.Month, 1)
            Return d1.AddMonths(-1).ToString("dd/MM/yyyy")
        End Function

        Public Function endOfLastMonth() As String
            Dim d As Date = Date.Now
            Dim d1 As Date = New Date(d.Year, d.Month, 1)
            Return d1.AddDays(-1).ToString("dd/MM/yyyy")
        End Function

        Public Function startOfNextMonth() As String
            Dim d As Date = Date.Now
            If d.Month = 12 Then
                Return New DateTime(d.Year + 1, 1, 1).ToString("dd/MM/yyyy")
            Else
                Return New DateTime(d.Year, d.Month + 1, 1).ToString("dd/MM/yyyy")
            End If
        End Function

        Public Function toEndOfDay(ByVal dt As DateTime) As DateTime
            'move the date to the start of the day
            'then add 1 day less 1 millisecond
            Return New DateTime(dt.Year, dt.Month, dt.Day).AddDays(1).AddMinutes(-1)
        End Function

        Public Function getSelectedValue(ByVal dd As DropDownList) As Integer
            If dd.SelectedIndex = -1 Then
                Return -1
            Else
                Return CInt(dd.SelectedValue)
            End If
        End Function

        Private Function getNonNullableType(ByVal t As Type) As Type
            If t.IsGenericType AndAlso t.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
                Return Nullable.GetUnderlyingType(t)
            Else
                Return t
            End If
        End Function

        Public Function GetTable(Of T)(ByVal objectCollection As List(Of T)) As Data.DataTable
            Dim table As New Data.DataTable
            Dim objectProperties As PropertyInfo() = GetType(T).GetProperties
            'create a column for each property in the class
            For Each propertyItem As PropertyInfo In objectProperties
                table.Columns.Add(New Data.DataColumn(propertyItem.Name, _
                    getNonNullableType(propertyItem.PropertyType)))
            Next
            For Each item As T In objectCollection
                'create a new row based on the table structure we just created
                Dim row As Data.DataRow = table.NewRow()
                'copy object data to the datarow
                For Each propertyItem As PropertyInfo In objectProperties
                    Dim o As Object = propertyItem.GetValue(item, Nothing)
                    If o Is Nothing Then
                        row(propertyItem.Name) = DBNull.Value
                    Else
                        row(propertyItem.Name) = o
                    End If
                Next
                'add row to the table
                table.Rows.Add(row)
            Next
            Return table
        End Function

        Public Function startOfThisWeek() As DateTime
            Dim dt As DateTime = DateTime.Now
            'add one to get the most recent monday
            dt -= New TimeSpan(dt.DayOfWeek - 1, 0, 0, 0)
            Return dt
        End Function

        Public Function endOfThisWeek() As DateTime
            Dim dt As DateTime = DateTime.Now
            'add six to get the next sunday after the most recent monday
            dt -= New TimeSpan(dt.DayOfWeek - 7, 0, 0, 0)
            Return dt
        End Function

        Public Function parseDate(ByVal s As String) As DateTime
            Return DateTime.ParseExact(s, "dd/MM/yyyy", Nothing)
        End Function

        'Public Function getPMState(ByVal session As HttpSessionState) As ProcessManagementState
        '    If session.Item("processManagementState") Is Nothing Then
        '        Return New ProcessManagementState
        '    Else
        '        Return DirectCast(session.Item("processManagementState"), ProcessManagementState)
        '    End If
        'End Function

        Public Sub sendCSVToClient(ByVal response As HttpResponse, ByVal FileName As String, ByVal csvData As String)
            response.AddHeader("Content-Type", "application/csv")
            response.AddHeader("content-disposition", "attachment; filename=" & FileName)
            response.Charset = "iso-8859-1"
            response.Write(csvData)
            response.End()
        End Sub

        Public Function toCSVCell(ByVal s As String) As String
            If s.IndexOf(",") <> -1 Or _
                s.IndexOf("""") <> -1 Or _
                s.IndexOf(vbCr) <> -1 Or _
                s.IndexOf(vbLf) <> -1 Then
                Return """" & s.Replace("""", """""") & """"
            Else
                Return s
            End If
        End Function

        Public Function nint(ByVal s As String) As Integer
            If s IsNot Nothing AndAlso isInteger(s) Then
                Return CInt(s)
            Else
                Return 0
            End If
        End Function


    End Module
End Namespace