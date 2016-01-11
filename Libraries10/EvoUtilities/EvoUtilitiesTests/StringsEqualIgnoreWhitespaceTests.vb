Imports nunit.framework
Imports EvoUtilities.TextUtils

<TestFixture()> _
Public Class StringsEqualIgnoreWhitespaceTests

    Private Sub checkEqual(ByVal s1 As String, ByVal s2 As String)
        Assert.IsTrue(stringsEqualIgnoreWhitespace(s1, s2))
        Assert.IsTrue(stringsEqualIgnoreWhitespace(s2, s1))
    End Sub

    Private Sub checkFailure(ByVal s1 As String, ByVal s2 As String)
        Assert.IsFalse(stringsEqualIgnoreWhitespace(s1, s2))
        Assert.IsFalse(stringsEqualIgnoreWhitespace(s2, s1))
    End Sub

    <Test()> _
    Public Sub basicOK()
        checkEqual("this is a test", "thisisatest")
        checkEqual("    this is a test", "thisisatest    ")
    End Sub

    <Test()> _
    Public Sub basicFail()
        checkFailure("this is a test", "thisisatestx")
        checkFailure("    this is a test", "thisisatest    x")
    End Sub

End Class
