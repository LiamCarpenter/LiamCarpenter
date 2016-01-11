Option Strict On
Option Explicit On 

Imports NUnit.framework
Imports System.io
Imports System.text
Imports CSVParser.CSVParser
Imports EvoUtilities.CollectionUtils
Imports CSVParser

<TestFixture()> _
Public Class CSVParserTests

    Private Shared Function tryCast2(Of T)(ByVal a As Object) As T
        If TypeOf a Is T Then
            Return CType(a, T)
        Else
            Return Nothing
        End If
    End Function

    Private Function unescapeIfString(Of T)(ByVal a As T) As T
        Dim a1 As String = TryCast(a, String) '(Of String)(a)
        If a1 IsNot Nothing Then
            Return tryCast2(Of T)(n(a1))
        Else
            Return a
        End If
    End Function

    Private Function a(Of T)(ByVal ParamArray elems() As T) As List(Of T)
        Dim ret As New List(Of T)
        For Each s As T In elems
            ret.Add(unescapeIfString(s))
        Next
        Return ret
    End Function

    'expands \n to vbnewline, and converts ' to " so string containing embedded
    Private Function n(ByVal t As String) As String
        Return t.Replace("\n", vbNewLine).Replace("'", """")
    End Function

    Private Sub assertArraysEqual(ByVal expected As List(Of String), _
        ByVal got As List(Of String), ByVal message As String)
        If expected.Count <> got.Count Then
            Throw New Exception(message & " - expected " & join(expected, ",") & "<> got " & join(got, ","))
        End If

        For j As Integer = 0 To expected.Count - 1
            Assert.AreEqual(expected.Item(j), got.Item(j), message)
        Next
    End Sub

    Private Sub testParsesAs(ByVal csv As String, ByVal data As List(Of List(Of String)))
        Dim i As Integer = 0
        For Each pline As List(Of String) In CSVParser.CSVParser.parseCSV(n(csv))
            assertArraysEqual(data.Item(i), pline, "line " & i)
            i += 1
        Next
    End Sub

    Private ReadOnly mt As New List(Of String)
    Private ReadOnly dee As List(Of String) = makeList("")

    <Test()> Public Sub testEmptyFile()
        testParsesAs("", a(Of List(Of String))())
    End Sub

    <Test()> Public Sub testSingleNewLine()
        testParsesAs("\n", a(a("")))
    End Sub

    <Test(), ExpectedException(GetType(NullReferenceException))> _
    Public Sub testNullCSV() 'don't
        testParsesAs(Nothing, a(Of List(Of String))())
    End Sub

    <Test()> Public Sub testTwoNewLines()
        testParsesAs("\n\n", a(dee, dee))
    End Sub

    <Test()> Public Sub testSingleField()
        testParsesAs("value", a(a("value")))
    End Sub

    <Test()> Public Sub testTwoFields()
        testParsesAs("value,another value", a(a("value", "another value")))
    End Sub

    <Test()> Public Sub testTwoRows()
        testParsesAs("value\nvalue2row", a(a("value"), a("value2row")))
    End Sub

    <Test()> Public Sub testDifferentLengthRows()
        testParsesAs("value,value2\nvalue3", a(a("value", "value2"), a("value3")))
    End Sub

    <Test()> Public Sub testTrailingComma()
        testParsesAs("value,", a(a("value", "")))
    End Sub

    <Test()> Public Sub testTrailingCommas1()
        testParsesAs(",", a(a("", "")))
    End Sub

    <Test()> Public Sub testTrailingCommas2()
        testParsesAs(",,", a(a("", "", "")))
    End Sub

    <Test()> Public Sub testTrailingCommas3()
        testParsesAs(",,,", a(a("", "", "", "")))
    End Sub

    <Test()> Public Sub testTrailingCommas4()
        testParsesAs(",,,,", a(a("", "", "", "", "")))
    End Sub

    <Test()> Public Sub testTrailingCommas24()
        testParsesAs("one,two,,,,", a(a("one", "two", "", "", "", "")))
    End Sub

    <Test()> Public Sub testMultilineTrailingCommas()
        testParsesAs("one,two,,,,\nthree,four,,\n,", _
            a(a("one", "two", "", "", "", ""), _
            a("three", "four", "", ""), _
            a("", "")))
    End Sub

    <Test()> Public Sub testMultilineTrailingCommasSmall()
        testParsesAs("one,\n", _
            a(a("one", "")))
    End Sub

    <Test()> Public Sub testMultilineTrailingCommasTiny()
        testParsesAs(",\n", _
            a(a("", "")))
    End Sub

    <Test()> Public Sub testInsideEmptyFields()
        testParsesAs("one,,two", a(a("one", "", "two")))
    End Sub

    'test blank lines
    <Test()> Public Sub testBlankLineMiddle()
        testParsesAs("one\n\nthree", a(a("one"), dee, a("three")))
    End Sub

    <Test()> Public Sub testBlankLineMix()
        testParsesAs("one\n\nthree\n\nfive\n\n", _
            a(a("one"), dee, a("three"), dee, a("five"), dee))
    End Sub

    <Test()> Public Sub testBlankLinesMiddle()
        testParsesAs("one\n\n\nthree", a(a("one"), dee, dee, a("three")))
    End Sub

    <Test()> Public Sub testBlankLinesEnd()
        testParsesAs("one\n\n\n", a(a("one"), dee, dee))
    End Sub

    <Test()> Public Sub commaInField()
        testParsesAs("'test,test'", a(a("test,test")))
    End Sub

    'leading and trailing spaces ignored
    <Test()> Public Sub ignoreLeadSpace()
        testParsesAs(" test", a(a("test")))
        testParsesAs(" te st", a(a("te st")))
        testParsesAs(" test1, test2", a(a("test1", "test2")))
    End Sub

    <Test()> Public Sub ignoreTrailSpace()
        testParsesAs("test ", a(a("test")))
        testParsesAs("te st ", a(a("te st")))
        testParsesAs("test1 ,test2 ", a(a("test1", "test2")))
    End Sub

    <Test()> Public Sub ignoreLeadTrailSpace()
        testParsesAs(" test ", a(a("test")))
        testParsesAs(" te st ", a(a("te st")))
        testParsesAs(" test1 , test2 ", a(a("test1", "test2")))
    End Sub

    <Test()> Public Sub ignoreLeadSpaceQuote()
        testParsesAs(" 'test'", a(a("test")))
        testParsesAs(" 'te st'", a(a("te st")))
        testParsesAs(" 'te st', 'test2'", a(a("te st", "test2")))
    End Sub

    <Test()> Public Sub ignoreTrailSpaceQuote()
        testParsesAs("'test' ", a(a("test")))
    End Sub

    <Test()> Public Sub ignoreLeadTrailSpaceQuote()
        testParsesAs(" 'test' ", a(a("test")))
    End Sub

    <Test()> Public Sub quotesInField()
        testParsesAs("'''test'''", a(a("'test'")))
        testParsesAs("'te''st'", a(a("te'st")))
    End Sub
    ' "
    ' ""
    ' """
    ' """"
    ' " a"
    ' "" b"
    ' """ c"
    ' """" d"
    'line breaks in field
    <Test()> Public Sub lineBreaksInField()
        testParsesAs("'te\nst'", a(a("te\nst")))
    End Sub

    <Test()> Public Sub leadingAndTrailingSpacesInQuotes()
        testParsesAs("' lead'  , 'trail ' \n  '  both   '  , nowt  ", _
            a(a(" lead", "trail "), a("  both   ", "nowt")))
    End Sub

    <Test()> Public Sub unneccessaryQuotesIgnored()
        testParsesAs("'a','b','c'\n'd','e','f'", _
            a(a("a", "b", "c"), a("d", "e", "f")))
    End Sub
    'bad data: quotes used after non whitespace
    <Test(), ExpectedException(GetType(SyntaxException))> _
    Public Sub quotesUsedAfterNonWhiteSpace()
        testParsesAs("a 'a' , c", a(Of List(Of String)))
    End Sub

    <Test(), ExpectedException(GetType(SyntaxException))> _
    Public Sub quotesUsedAfterNonWhiteSpace2()
        testParsesAs("a ''a , c", a(Of List(Of String)))
    End Sub

    'bad data: mismatched quotes
    <Test(), ExpectedException(GetType(SyntaxException))> _
    Public Sub mismatchedQuotes()
        testParsesAs("a,b,'c", a(Of List(Of String)))
    End Sub

    'bad data: junk after closing quote before comma, eol, eof
    <Test(), ExpectedException(GetType(SyntaxException))> _
    Public Sub junkAfterQuoteBeforeComma()
        testParsesAs("'a' b, c", a(Of List(Of String))())
    End Sub

    <Test(), ExpectedException(GetType(SyntaxException))> _
    Public Sub junkAfterQuoteBeforeEol()
        testParsesAs("a,b,'c' d\ne,f", a(Of List(Of String))())
    End Sub

    <Test(), ExpectedException(GetType(SyntaxException))> _
    Public Sub junkAfterQuoteBeforeEof()
        testParsesAs("e,f\na,b,'c' d", a(a("e", "f")))
    End Sub

    <Test()> Public Sub csvInAField()
        testParsesAs("'''a'',''c''','b'", a( _
            a("'a','c'", "b")))
    End Sub

    <Test()> Public Sub testABigAssFile()
        Dim parser As New CSVParser.CSVParser(My.Computer.FileSystem.ReadAllText("..\..\customerexport.csv"))
    End Sub

    Private Sub syntaxError(ByVal csv As String, ByVal lineNo As Integer)
        Try
            For Each line As String In CSVParser.CSVParser.parseCSV(n(csv))
            Next
            Assert.Fail("parse should raise syntax error but didn't")
        Catch ex As SyntaxException
            Assert.AreEqual(lineNo, ex.mLineNo)
        End Try
    End Sub

    <Test()> Public Sub testLineNumber0InError()
        syntaxError("'a','b'x,'c'\n'd','e','f'", 0)
    End Sub

    <Test()> Public Sub testLineNumber1InError()
        syntaxError("'a','b','c'\n'd','e'x,'f'", 1)
    End Sub

    <Test()> Public Sub testLineNumber3InError()
        syntaxError("'a','b','c'\n'd','e\nf\ng','f'x", 3)
    End Sub

End Class
