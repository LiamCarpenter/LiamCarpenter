Imports System.Collections
Imports System.Collections.Generic
Imports System.Reflection

Namespace Utility
    
    ''' <summary>
    ''' Sort order enumeration
    ''' </summary>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Public Enum SortOrder
        Ascending
        Descending
    End Enum

    ''' <summary>
    ''' This instance class is used to sort a generic collection of object instances.
    ''' It automatically fetches the type and performs the necessary comparison(s) to sort.
    ''' 
    ''' To use, instantiate this class, set the sort string property, and pass this
    ''' instance to the internal Sort() function of your generic collection.
    ''' 
    ''' Example:
    '''     Dim MyList As List(Of MyClassType) = 'Populate the list somehow
    '''     Dim Sorter As New Sorter(Of MyClassType)    '''     Sorter.SortString = "Field1 DESC, Field2"
    '''     MyList.Sort(Sorter) 'After this call, the list is sorted
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Public Class Sorter(Of T)
        Implements IComparer(Of T)

        Private _Sort As String

        ''' <summary>
        ''' Instantiate the class.
        ''' </summary>
        ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Instantiate the class, setting the sort string.
        ''' Example: "LastName DESC, FirstName"
        ''' </summary>
        ''' <param name="SortValue"></param>
        ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
        Public Sub New(ByVal SortValue As String)
            _Sort = SortValue
        End Sub

        ''' <summary>
        ''' The sort string used to perform the sort. Can sort on multiple fields.
        ''' Use the property names of the class and basic SQL Syntax.
        ''' 
        ''' Example: "LastName DESC, FirstName"
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
        Public Property SortString() As String
            Get
                If _Sort IsNot Nothing Then
                    Return _Sort.Trim()
                End If

                Return Nothing
            End Get
            Set(ByVal value As String)
                _Sort = value
            End Set
        End Property

        ''' <summary>
        ''' This is an implementation of IComparer(Of T).Compare
        ''' Can sort on multiple fields, or just one.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns></returns>
        ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
        Public Function Compare(ByVal x As T, ByVal y As T) As Integer Implements IComparer(Of T).Compare
            If Not String.IsNullOrEmpty(Me.SortString) Then
                Const ERR As String = "The property ""{0}"" does not exist in type ""{1}"""
                Dim Type As Type = GetType(T)
                Dim Comp As Comparer = Comparer.DefaultInvariant
                Dim Info As PropertyInfo

                For Each Expr As String In Me.SortString.Split(","c)
                    Dim Dir As SortOrder = SortOrder.Ascending
                    Dim Field As String

                    Expr = Expr.Trim()

                    If Expr.EndsWith(" DESC", StringComparison.Ordinal) Then
                        Field = Expr.Replace(" DESC", String.Empty).Trim()
                        Dir = SortOrder.Descending
                    Else
                        Field = Expr.Replace(" ASC", String.Empty).Trim()
                    End If

                    Info = Type.GetProperty(Field)

                    If Info Is Nothing Then
                        Throw New MissingFieldException(String.Format(ERR, Field, Type.ToString()))
                    Else
                        Dim Result As Integer = Comp.Compare(Info.GetValue(x, Nothing), Info.GetValue(y, Nothing))

                        If Result <> 0 Then
                            If Dir = SortOrder.Descending Then
                                Return Result * -1
                            Else
                                Return Result
                            End If
                        End If
                    End If
                Next
            End If

            Return 0
        End Function
    End Class
End Namespace
