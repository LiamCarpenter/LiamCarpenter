Option Strict On
Option Explicit On

Imports system.collections.generic

Namespace Collections

    Public Class InsertOrderSortedDictionary(Of Key, Value)
        Implements IDictionary(Of Key, Value), _
        IEnumerable(Of KeyValuePair(Of Key, Value)), _
        ICollection(Of KeyValuePair(Of Key, Value))

        'works just like a hashtable, except you can get keys and values out in the
        'order in which they are inserted
        'and the enumerator returns keys, values in the order they were inserted
        Private mMap As New Dictionary(Of Key, Value)
        Private mKeys As New List(Of Key)
        Private mValues As New List(Of Value)

        Public Sub Add1(ByVal item As KeyValuePair(Of Key, Value)) Implements _
            ICollection(Of KeyValuePair(Of Key, Value)).Add
            mMap.Add(item.Key, item.Value)
            mKeys.Add(item.Key)
            mValues.Add(item.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of Key, Value)).Clear
            mMap.Clear()
            mKeys.Clear()
            mValues.Clear()
        End Sub

        Public Function Contains(ByVal item As KeyValuePair(Of Key, Value)) As _
            Boolean Implements ICollection(Of KeyValuePair(Of Key, Value)).Contains
            Throw New Exception("not implemented")
        End Function

        Public Sub CopyTo(ByVal array() As KeyValuePair(Of Key, Value), _
            ByVal arrayIndex As Integer) Implements _
            ICollection(Of KeyValuePair(Of Key, Value)).CopyTo
            Throw New Exception("not implemented")
        End Sub

        Public ReadOnly Property Count() As Integer Implements ICollection( _
            Of KeyValuePair(Of Key, Value)).Count
            Get
                Return mKeys.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly() As Boolean _
            Implements ICollection(Of KeyValuePair(Of Key, Value)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove1(ByVal item As KeyValuePair(Of Key, Value)) As Boolean _
            Implements ICollection(Of KeyValuePair(Of Key, Value)).Remove
            Throw New Exception("not implemented")
        End Function

        Public Sub Add(ByVal key As Key, ByVal value As Value) _
            Implements IDictionary(Of Key, Value).Add
            mMap.Add(key, value)
            mKeys.Add(key)
            mValues.Add(value)
        End Sub

        Public Function ContainsKey(ByVal key As Key) As Boolean _
            Implements IDictionary(Of Key, Value).ContainsKey
            Return mMap.ContainsKey(key)
        End Function

        Default Public Property Item(ByVal key As Key) As Value _
            Implements IDictionary(Of Key, Value).Item
            Get
                Return mMap.Item(key)
            End Get
            Set(ByVal value As Value)
                mMap.Item(key) = value
            End Set
        End Property

        Public ReadOnly Property Keys() As ICollection(Of Key) _
            Implements IDictionary(Of Key, Value).Keys
            Get
                Return mKeys
            End Get
        End Property

        Public Function Remove(ByVal key As Key) As Boolean _
            Implements IDictionary(Of Key, Value).Remove
            mValues.Remove(mMap.Item(key))
            mKeys.Remove(key)
            mMap.Remove(key)
        End Function

        Public Function TryGetValue(ByVal key As Key, ByRef value As Value) As Boolean _
            Implements IDictionary(Of Key, Value).TryGetValue
            Return mMap.TryGetValue(key, value)
        End Function

        Public ReadOnly Property Values() As ICollection(Of Value) _
            Implements IDictionary(Of Key, Value).Values
            Get
                Return mValues
            End Get
        End Property

        Public Function GetEnumerator() As IEnumerator( _
            Of KeyValuePair(Of Key, Value)) _
            Implements IEnumerable( _
                Of KeyValuePair(Of Key, Value)).GetEnumerator
            Return New InsertOrderSortedDictionaryEnumerator(Of Key, Value)(Me)
        End Function

        Private Class InsertOrderSortedDictionaryEnumerator(Of Key1, Value1)
            Implements IEnumerator(Of KeyValuePair(Of Key, Value))

            Private mCurrentIndex As Integer = 0
            Private mDic As InsertOrderSortedDictionary(Of Key, Value)

            Public Sub New(ByVal c As InsertOrderSortedDictionary(Of Key, Value))
                mDic = c
            End Sub

            Public ReadOnly Property Current() As KeyValuePair(Of Key, Value) _
                Implements IEnumerator(Of KeyValuePair(Of Key, Value)).Current
                Get
                    Return New KeyValuePair(Of Key, Value)(mDic.mKeys.Item(mCurrentIndex), _
                        mDic.mValues.Item(mCurrentIndex))
                End Get
            End Property

            Public ReadOnly Property Current1() As Object Implements System.Collections.IEnumerator.Current
                Get
                    Return Current
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                If mCurrentIndex > mDic.Count - 1 Then
                    Return False
                End If
                mCurrentIndex = mCurrentIndex + 1
                Return True
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                mCurrentIndex = 0
            End Sub

            Private disposedValue As Boolean = False        ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: free unmanaged resources when explicitly called
                    End If

                    ' TODO: free shared unmanaged resources
                End If
                Me.disposedValue = True
            End Sub

#Region " IDisposable Support "
            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region
        End Class

        Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function


    End Class


End Namespace