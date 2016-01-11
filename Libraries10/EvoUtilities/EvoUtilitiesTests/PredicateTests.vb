Imports NUnit.Framework
Imports EvoUtilities.CollectionUtils

<TestFixture()> _
Public Class PredicateTests

    Private Function isEvenImpl(ByVal i As Integer) As Boolean
        Return i Mod 2 = 0
    End Function

    Private Function isEven() As Predicate(Of Integer)
        Return AddressOf isEvenImpl
    End Function

    <Test()> Public Sub tNot()
        Dim l As List(Of Integer) = makeList(1, 2, 3, 4, 5)
        Assert.IsTrue(listsEqual(makeList(1, 3, 5), findAll(l, [not](isEven))))
    End Sub

    <Test()> Public Sub tlistContains()
        Dim l As List(Of Integer) = makeList(1, 2, 3, 4, 5)
        Dim cl As List(Of Integer) = makeList(-3, 1, 4, 5, 6)
        Assert.IsTrue(listsEqual(makeList(1, 4, 5), findAll(l, listContains(cl))))
    End Sub

    <Test()> Public Sub notListContains()
        Dim l As List(Of Integer) = makeList(1, 2, 3, 4, 5)
        Dim cl As List(Of Integer) = makeList(-3, 1, 4, 5, 6)
        Assert.IsTrue(listsEqual(makeList(2, 3), findAll(l, [not](listContains(cl)))))
    End Sub

End Class
