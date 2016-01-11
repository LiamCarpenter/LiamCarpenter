Option Strict On
Option Explicit On

Imports system.text
Imports EvoUtilities.Utils

Public Class CollectionUtils

    Public Shared Function makeList(Of T)(ByVal ParamArray e As T()) As List(Of T)
        Return New List(Of T)(e)
    End Function

    Public Shared Function toArray(Of T)(ByVal l As IList(Of T)) As T()
        Dim ret(l.Count) As T
        For i As Integer = 0 To l.Count - 1
            ret(i) = l(i)
        Next
        Return ret
    End Function

    Public Shared Function getLastNUniqueitems(Of T)(ByVal n As Integer, _
        ByVal l As IList(Of T)) As IList(Of T)
        Dim ret As New List(Of T)
        If l.Count > 0 Then
            For i As Integer = l.Count - 1 To 0 Step -1
                If ret.Contains(l(i)) Then
                    ret.Remove(l(i))
                End If
                ret.Insert(0, l(i))
                If ret.Count = n Then
                    Exit For
                End If
            Next
        End If
        Return ret
    End Function

    Public Shared Function join(ByVal l As ICollection(Of String), ByVal sep As String) As String
        Select Case l.Count
            Case 0
                Return ""
            Case 1
                For Each s As String In l
                    Return s
                Next
                'unreachable
                Return Nothing
            Case Else
                Dim ret As New System.Text.StringBuilder
                For Each s As String In l
                    ret.Append(s)
                    ret.Append(sep)
                Next
                TextUtils.removeFromEnd(ret, sep)
                Return ret.ToString
        End Select
    End Function

    Public Shared Function join1(ByVal l As IList, ByVal sep As String) As String
        Select Case l.Count
            Case 0
                Return ""
            Case 1
                For Each s As String In l
                    Return s
                Next
                'unreachable
                Return Nothing
            Case Else
                Dim ret As New System.Text.StringBuilder
                For Each s As String In l
                    ret.Append(s)
                    ret.Append(sep)
                Next
                TextUtils.removeFromEnd(ret, sep)
                Return ret.ToString
        End Select
    End Function


    Public Shared Function join(Of T)(ByVal l As IList(Of T), ByVal sep As String) As String
        Select Case l.Count
            Case 0
                Return ""
            Case 1
                Return l.Item(0).ToString
            Case Else
                Dim ret As New System.Text.StringBuilder
                For i As Integer = 0 To l.Count - 2
                    ret.Append(l.Item(i))
                    ret.Append(sep)
                Next
                ret.Append(l.Item(l.Count - 1))
                Return ret.ToString
        End Select
    End Function

    Public Shared Function ilistToList(Of T)(ByVal l As IList(Of T)) As List(Of T)
        Dim r1 As List(Of T) = TryCast(l, List(Of T))
        If r1 Is Nothing Then
            Return New List(Of T)(l)
        Else
            Return r1
        End If
    End Function

    Public Shared Function makeUnique(Of T)(ByVal l As IList(Of T)) As IList(Of T)
        Dim ret As New List(Of T)
        For Each s As T In l
            If Not ret.Contains(s) Then
                ret.Add(s)
            End If
        Next
        Return ret
    End Function

    Private Class ListContainsImpl(Of T)
        Private l As IList(Of T)
        Public Sub New(ByVal l As IList(Of T))
            Me.l = l
        End Sub

        Public Function contains(ByVal item As T) As Boolean
            Return l.Contains(item)
        End Function
    End Class

    Private Class ListMatchesImpl(Of T)
        Private l As IList(Of T)
        Private eqf As Eq(Of T)
        Public Sub New(ByVal l As IList(Of T), ByVal eqf As Eq(Of T))
            Me.l = l
            Me.eqf = eqf
        End Sub
        Public Function matches(ByVal item As T) As Boolean
            For Each t1 As T In l
                If (eqf(t1, item)) Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class

    Public Shared Function listContains(Of T)(ByVal l As IList(Of T)) As Predicate(Of T)
        Return AddressOf (New ListContainsImpl(Of T)(l)).contains
    End Function

    Public Shared Function listMatches(Of T)(ByVal l As IList(Of T), ByVal eqf As Eq(Of T)) As Predicate(Of T)
        Return AddressOf (New ListMatchesImpl(Of T)(l, eqf)).matches
    End Function

    Public Shared Function itemOrNothing(Of Key, Value As Class)(ByVal d As Dictionary(Of Key, Value), ByVal k As Key) As Value
        If d.ContainsKey(k) Then
            Return d.Item(k)
        Else
            Return Nothing
        End If
    End Function

    Private Class NotImpl(Of T)
        Private p As Predicate(Of T)
        Public Sub New(ByVal p As Predicate(Of T))
            Me.p = p
        End Sub
        Public Function [not](ByVal t1 As T) As Boolean
            Return Not p(t1)
        End Function
    End Class

    Public Shared Function [not](Of T)(ByVal p As Predicate(Of T)) As Predicate(Of T)
        Return AddressOf (New NotImpl(Of T)(p)).not
    End Function

    Public Shared Function removeAll(Of T)(ByVal l1 As IList(Of T), ByVal l2 As IList(Of T)) As IList(Of T)
        Dim l3 As New List(Of T)(l1)
        'Dim lc As New ListContains(Of T)(l2)
        l3.RemoveAll(listContains(l2))
        Return l3
    End Function

    Public Shared Function symmetricDiff(Of T)(ByVal l1 As IList(Of T), ByVal l2 As IList(Of T)) _
         As KeyValuePair(Of IList(Of T), IList(Of T))
        Dim ret As New KeyValuePair(Of IList(Of T), IList(Of T))( _
            removeAll(l1, l2), _
            removeAll(l2, l1))
        Return ret
    End Function

    Public Shared Function findAll(Of T)(ByVal l As IList(Of T), ByVal p As Predicate(Of T)) As IList(Of T)
        Dim ret As New List(Of T)
        For Each t1 As T In l
            If p(t1) Then
                ret.Add(t1)
            End If
        Next
        Return ret
    End Function

    Public Delegate Function ObjectPredicate(ByVal o As Object) As Boolean

    Public Shared Function findAllObj(Of T)(ByVal l As IList, ByVal p As ObjectPredicate) As IList(Of T)
        Dim ret As New List(Of T)
        For Each t1 As T In l
            If p(t1) Then
                ret.Add(t1)
            End If
        Next
        Return ret
    End Function


    Public Shared Function concatLists(Of T)(ByVal ParamArray lists() As IList(Of T)) As IList(Of T)
        'Return foldL(lists, New List(Of T), AddressOf addRange)
        Dim ret As New List(Of T)
        For Each l As List(Of T) In lists
            ret.AddRange(l)
        Next
        Return ret
    End Function

    Public Shared Function addRange(Of T)(ByVal l As List(Of T), ByVal m As List(Of T)) As List(Of T)
        l.AddRange(m)
        Return l
    End Function

    Public Delegate Function Eq(Of T)(ByVal t1 As T, ByVal t2 As T) As Boolean

    Private Class NotMatchList(Of T)
        Private ts As IList(Of T)
        Private eqf As Eq(Of T)

        Public Sub New(ByVal ts As IList(Of T), ByVal eqf As Eq(Of T))
            Me.ts = ts
            Me.eqf = eqf
        End Sub
        Public Function notMatch(ByVal t1 As T) As Boolean
            For Each t2 As T In ts
                If eqf(t1, t2) Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Class

    Public Delegate Function Stringeriser(Of T)(ByVal o As T) As String

    'take a list of T, a seperator, and a function T -> String
    'and join by converting each T to string using the function
    'and putting sep inbetween 'em
    Public Shared Function joinWith(Of T)(ByVal l As List(Of T), _
        ByVal sep As String, ByVal toS As Stringeriser(Of T)) As String
        Select Case l.Count
            Case 0
                Return ""
            Case 1
                Return toS(l.Item(0))
            Case Else
                Dim ret As New System.Text.StringBuilder
                For i As Integer = 0 To l.Count - 2
                    ret.Append(toS(l.Item(i)))
                    ret.Append(sep)
                Next
                ret.Append(toS(l.Item(l.Count - 1)))
                Return ret.ToString
        End Select
    End Function

    Public Shared Function split(ByVal source As String, ByVal sep As String) As List(Of String)
        Return New List(Of String)(Microsoft.VisualBasic.Strings.Split(source, sep))
    End Function

    Public Shared Function inList(Of T)(ByVal item As T, _
        ByVal ParamArray list As T()) As Boolean
        For Each s As T In list
            If item.Equals(s) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Shared Function filterByType(Of SourceType, TargetType) _
        (ByVal l As IList(Of SourceType)) As List(Of TargetType)
        Dim ret As New List(Of TargetType)
        For Each e As SourceType In l
            Dim e1 As TargetType = TryCast2(Of TargetType)(e)
            If e1 IsNot Nothing Then
                ret.Add(e1)
            End If
        Next
        Return ret
    End Function

    'really fucking like the way I have not only write tedious trivial
    '.equals for every fucking class, but I have to write it for 
    'fucking microsoft's code as well
    Public Shared Function listsEqual(Of T)(ByVal list1 As IList(Of T), ByVal list2 As IList(Of T)) As Boolean
        If list1 Is Nothing And list2 Is Nothing Then
            Return True
        ElseIf list1 Is Nothing Or list2 Is Nothing Then
            Return False
        ElseIf list1.Count <> list2.Count Then
            Return False
        Else
            For i As Integer = 0 To list1.Count - 1
                If Not list1.Item(i).Equals(list2.Item(i)) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Shared Function listsEqualNoOrder(Of T)(ByVal list1 As IList(Of T), ByVal list2 As IList(Of T)) As Boolean
        If list1 Is Nothing And list2 Is Nothing Then
            Return True
        ElseIf list1 Is Nothing Or list2 Is Nothing Then
            Return False
        ElseIf list1.Count <> list2.Count Then
            Return False
        Else
            For i As Integer = 0 To list1.Count - 1
                If list2.Contains(list1.Item(i)) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Shared Sub setsEqualDebug(Of T)(ByVal l1 As IList(Of T), ByVal l2 As IList(Of T))
        If Not listsEqualNoOrder(l1, l2) Then
            Dim inA As New List(Of T)
            Dim inB As New List(Of T)
            For Each te As T In l1
                If Not l2.Contains(te) Then
                    inA.Add(te)
                End If
            Next
            For Each te As T In l2
                If Not l1.Contains(te) Then
                    inB.Add(te)
                End If
            Next
            If inA.Count > 0 Or inB.Count > 0 Then
                Throw New Exception("Lists different, extra in a: " & join(inA, "," & vbNewLine) & _
                    ", extra in b: " & join(inB, "," & vbNewLine))
            End If
        End If
    End Sub

    Public Shared Function intersect(Of T)(ByVal l1 As IList(Of T), ByVal l2 As IList(Of T)) As IList(Of T)
        Dim ret As New List(Of T)
        For Each e1 As T In l1
            If l2.Contains(e1) Then
                ret.Add(e1)
            End If
        Next
        Return ret
    End Function


    Public Delegate Function MapElement(Of [In], Out)(ByVal [in] As [In]) As Out

    Public Shared Function map(Of T, U)(ByVal l As ICollection(Of T), ByVal fn As MapElement(Of T, U)) As IList(Of U)
        Dim ret As New List(Of U)
        For Each elem As T In l
            ret.Add(fn(elem))
        Next
        Return ret
    End Function

    Public Shared Function mapObj(Of U)(ByVal l As ICollection, ByVal fn As MapElement(Of Object, U)) As IList(Of U)
        Dim ret As New List(Of U)
        For Each elem As Object In l
            ret.Add(fn(elem))
        Next
        Return ret
    End Function

    Public Delegate Function FoldBit(Of T, U)(ByVal total As U, ByVal t As T) As U

    Public Shared Function foldL(Of T, U)(ByVal l As IList(Of T), _
        ByVal initialValue As U, ByVal accum As FoldBit(Of T, U)) As U
        Dim total As U = initialValue
        For Each elem As T In l
            total = accum(total, elem)
        Next
        Return total
    End Function

    'array version
    Public Shared Function foldL(Of T, U)(ByVal l() As T, _
        ByVal initialValue As U, ByVal accum As FoldBit(Of T, U)) As U
        Dim total As U = initialValue
        For Each elem As T In l
            total = accum(total, elem)
        Next
        Return total
    End Function


    'Dim t As String = foldL(s, New StringBuilder, AddressOf concatenate).ToString
    'Private Shared Function concatenate(ByVal total As StringBuilder, ByVal s As String) As StringBuilder
    '    total.Append(s)
    '    Return total
    'End Function

    Public Delegate Function Test(Of T)(ByVal arg As T) As Boolean

    Public Shared Function thereExists(Of T)(ByVal l As List(Of T), ByVal fn As Test(Of T)) As Boolean
        For Each lt As T In l
            If fn(lt) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Shared Function forAll(Of T)(ByVal l As List(Of T), ByVal fn As Test(Of T)) As Boolean
        For Each lt As T In l
            If Not fn(lt) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function forAll(Of T)(ByVal l As T(), ByVal fn As Test(Of T)) As Boolean
        For Each lt As T In l
            If Not fn(lt) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function castList(Of T As Class, U As Class)(ByVal l As IList(Of T)) As IList(Of U)
        Dim ret As New List(Of U)
        For Each t1 As T In l
            ret.Add(castIt(Of U)(t1))
        Next
        Return ret
    End Function

    Public Shared Function castIt(Of U)(ByVal t1 As Object) As U
        Return DirectCast(t1, U)
    End Function


    'support for quicker syntax for typesafe dictionary constructor
    'use like this:
    'makeDictionary(Of YesNo, String)( _
    '   p(YesNo.Yes, "Yes"), _
    '    p(YesNo.No, "No"))
    Public Shared Function p(Of a, b)(ByVal av As a, ByVal bv As b) _
        As KeyValuePair(Of a, b)
        Return New KeyValuePair(Of a, b)(av, bv)
    End Function

    Public Shared Function makeDictionary(Of KeyType, ValueType)( _
        ByVal ParamArray args As KeyValuePair(Of KeyType, ValueType)()) _
        As Dictionary(Of KeyType, ValueType)
        Dim ret As New Dictionary(Of KeyType, ValueType)
        For Each k As KeyValuePair(Of KeyType, ValueType) In args
            ret.Item(k.Key) = k.Value
        Next
        Return ret
    End Function

    Public Shared Function dictionariesEqual(Of K, V)( _
        ByVal d1 As Dictionary(Of K, V), _
        ByVal d2 As Dictionary(Of K, V)) As Boolean
        If d2.Count <> d1.Count Then
            Return False
        End If
        For Each kvp As KeyValuePair(Of K, V) In d1
            If Not d2.Item(kvp.Key).Equals(kvp.Value) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function dictionariesEqualDebug(Of K, V)( _
           ByVal d1 As Dictionary(Of K, V), _
           ByVal d2 As Dictionary(Of K, V)) As Boolean
        If d2.Count <> d1.Count Then
            Throw New Exception("key count not equal:" & vbNewLine & _
                join(New List(Of K)(d1.Keys), ", ") & vbNewLine & _
                join(New List(Of K)(d2.Keys), ", "))
        End If
        For Each kvp As KeyValuePair(Of K, V) In d1
            If Not d2.Item(kvp.Key).Equals(kvp.Value) Then
                Throw New Exception("A." & kvp.Key.ToString & "(" & kvp.Value.ToString & ") <> B." & _
                    kvp.Key.ToString & "(" & d2.Item(kvp.Key).ToString & ")")
            End If
        Next
        Return True
    End Function

End Class
