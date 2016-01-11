Option Strict On
Option Explicit On

Imports system.collections
Imports system.io
Imports system.text
Imports system.text.RegularExpressions
Imports EvoUtilities.CollectionUtils

Public Class TextUtils

    'replace \n and \t with newline and tab
    Public Shared Function unEscape(ByVal text As String) As String
        Return text.Replace("\n", vbNewLine).Replace("\t", vbTab)
    End Function

    'replace < > & ' " with entities in text
    Public Shared Function escapeXml(ByVal text As String) As String
        Return text.Replace("&", "&amp;"). _
            Replace("<", "&lt;"). _
            Replace(">", "&gt;"). _
            Replace("'", "&apos;"). _
            Replace("""", "&quot;")
    End Function

    'undo replace < > & ' " with entities in text
    Public Shared Function unescapeXml(ByVal text As String) As String
        Return text.Replace("&lt;", "<"). _
        Replace("&gt;", ">"). _
        Replace("&apos;", "'"). _
        Replace("&quot;", """"). _
        Replace("&amp;", "&")
    End Function

    Private Shared SMSEscapes As Dictionary(Of String, String) = makeDictionary( _
        p(vbCr, "%0D"), _
        p(vbLf, "%0A"), _
        p("#", "%23"), _
        p("%", "%25"), _
        p("&", "%26"), _
        p(",", "%2C"), _
        p(".", "%2E"), _
        p("/", "%2F"), _
        p(":", "%3A"), _
        p(";", "%3B"), _
        p("<", "%3C"), _
        p("=", "%3D"), _
        p(">", "%3E"), _
        p("?", "%3F"), _
        p("¡", "%A1"), _
        p("£", "%A3"), _
        p("#", "%A4"), _
        p("¥", "%A5"), _
        p("§", "%A7"), _
        p("Ä", "%C4"), _
        p("Å", "%C5"), _
        p("à", "%E0"), _
        p("ä", "%E4"), _
        p("å", "%E5"), _
        p("Æ", "%C6"), _
        p("Ç", "%C7"), _
        p("É", "%C9"), _
        p("è", "%E8"), _
        p("é", "%E9"), _
        p("ì", "%EC"), _
        p("Ñ", "%D1"), _
        p("ñ", "%F1"), _
        p("ò", "%F2"), _
        p("ö", "%F6"), _
        p("Ø", "%D8"), _
        p("Ö", "%D6"), _
        p("Ü", "%DC"), _
        p("ù", "%F9"), _
        p("ü", "%FC"), _
        p("ß", "%DF"))

    Public Shared Function fixCharactersForSMS(ByVal s As String) As String
        'see http://www.csoft.co.uk/sms/character_sets/encoding.htm
        For Each p As KeyValuePair(Of String, String) In SMSEscapes
            s = s.Replace(p.Key, p.Value)
        Next
        Return s
    End Function

    Public Shared Function getGroup(ByVal input As String, ByVal reg As String, ByVal groupName As String) As String
        Dim m As Match = Regex.Match(input, reg, RegexOptions.IgnoreCase)
        If Not m.Success Then
            Throw New Exception("match failure: " & input & " " & reg)
        End If
        Return m.Groups.Item(groupName).Value
    End Function

    Public Shared Function getGroupNoThrow(ByVal input As String, ByVal reg As String, ByVal groupName As String) As String
        Dim m As Match = Regex.Match(Input, reg, RegexOptions.IgnoreCase)
        If Not m.Success Then
            Return Nothing
        End If
        Return m.Groups.Item(groupName).Value
    End Function

    Public Shared Function capitalise(ByVal s As String) As String
        If s.Length = 0 Then
            Return s
        End If
        Dim chars() As Char = s.ToCharArray
        chars(0) = Char.ToUpper(chars(0))
        For i As Integer = 1 To chars.Length - 1
            If Char.IsWhiteSpace(chars(i)) And i < chars.Length - 2 _
                AndAlso Char.IsLetter(chars(i + 1)) Then
                chars(i + 1) = Char.ToUpper(chars(i + 1))
            End If
        Next
        Return New String(chars)
    End Function

    Public Shared Function capitaliseFirst(ByVal s As String) As String
        If s.Length = 0 Then
            Return s
        End If
        Dim chars() As Char = s.ToCharArray
        chars(0) = Char.ToUpper(chars(0))
        Return New String(chars)
    End Function

    Public Shared Sub removeFromEnd(ByVal sb As StringBuilder, ByVal s As String)
        If Not sb.ToString.EndsWith(s) Then
            Throw New Exception("cannot remove " & s & " from end since the sb doesn't end with this")
        End If
        sb.Remove(sb.Length - s.Length, s.Length)
    End Sub

    Public Shared Function substringFromEnd(ByVal s As String, ByVal n As Integer) As String
        Return s.Substring(0, s.Length - n)
    End Function


    Public Class SplitIntoLinesClass
        Implements IEnumerable

        Public Shared Function SplitIntoLines(ByVal s As String) As SplitIntoLinesClass
            Return New SplitIntoLinesClass(s)
        End Function

        Private mString As String
        Public Sub New(ByVal s As String)
            mString = s
        End Sub

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return New SplitIntoLinesEnumerator(mString)
        End Function

        Private Class SplitIntoLinesEnumerator
            Implements IEnumerator

            Private nextLine As String
            Private mString As String
            Private mStringReader As StringReader

            Public Sub New(ByVal s As String)
                mString = s
                mStringReader = New StringReader(mString)
            End Sub

            Public ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
                Get
                    Return nextLine
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                nextLine = mStringReader.ReadLine
                Return nextLine IsNot Nothing
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                mStringReader = New StringReader(mString)
            End Sub
        End Class

    End Class

    Public Shared ReadOnly CR As Char = vbCr.Chars(0)
    Public Shared ReadOnly LF As Char = vbLf.Chars(0)
    Public Const SPACE As Char = " "c
    Public Shared ReadOnly TAB As Char = vbTab.Chars(0)

    Public Shared Function isWhitespace(ByVal c As Char) As Boolean
        Select Case c
            Case CR, LF, SPACE, TAB
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Shared Function stringsEqualIgnoreWhitespace(ByVal s1 As String, ByVal s2 As String) As Boolean
        Dim p1 As Integer = 0
        Dim p2 As Integer = 0
        While p1 < s1.Length AndAlso p2 < s2.Length
            While p1 < s1.Length AndAlso isWhitespace(s1.Chars(p1))
                p1 += 1
            End While
            While p2 < s2.Length AndAlso isWhitespace(s2.Chars(p2))
                p2 += 1
            End While
            If p1 >= s1.Length Then
                'check the end of p2 is whitespace
                Return isEndWhitespace(s2, p2)
            End If
            If p2 >= s2.Length Then
                'check the end of p1 is whitespace
                Return isEndWhitespace(s1, p1)
            End If
            If s1.Chars(p1) <> s2.Chars(p2) Then
                Return False
            End If
            p1 += 1
            p2 += 1
        End While
        'check the end of p2 is whitespace
        Return isEndWhitespace(s2, p2)
    End Function

    Private Shared Function isEndWhitespace(ByVal s As String, ByVal p As Integer) As Boolean
        Return forAll(s.Substring(p).ToCharArray, AddressOf isWhitespace)
    End Function

    Public Shared Function humanReadableBytes(ByVal b As Long) As String
        If b > 1024L * 1024 * 1024 * 10 Then
            Return (b / 1024 / 1024 / 1024).ToString("N2") + " gigabytes"
        ElseIf b > 1024 * 1024 * 10 Then
            Return (b / 1024 / 1024).ToString("N2") + " megabytes"
        ElseIf b > 1024 * 10 Then
            Return (b / 1024).ToString("N2") + " kilobytes"
        Else
            Return b.ToString("N2") + " bytes"
        End If
    End Function

    <CLSCompliant(False)> _
    Public Shared Function humanReadableBytes(ByVal b As ULong) As String
        If b > 1024L * 1024 * 1024 * 10 Then
            Return (b / 1024 / 1024 / 1024).ToString("N2") + " gigabytes"
        ElseIf b > 1024 * 1024 * 10 Then
            Return (b / 1024 / 1024).ToString("N2") + " megabytes"
        ElseIf b > 1024 * 10 Then
            Return (b / 1024).ToString("N2") + " kilobytes"
        Else
            Return b.ToString("N2") + " bytes"
        End If
    End Function



    Public Shared Function maxLength(ByVal s As String, ByVal l As Integer) As String
        If s Is Nothing Then
            Return Nothing
        End If
        If s.Length > l Then
            Return s.Substring(0, l)
        Else
            Return s
        End If
    End Function

    Public Shared Function empty(ByVal s As String) As Boolean
        Return s Is Nothing OrElse s.Trim() = ""
    End Function

    Public Shared Function toCSVCell(ByVal s As String) As String
        If s.IndexOf(",") <> -1 Or _
            s.IndexOf("""") <> -1 Or _
            s.IndexOf(vbCr) <> -1 Or _
            s.IndexOf(vbLf) <> -1 Then
            Return """" & s.Replace("""", """""") & """"
        Else
            Return s
        End If
    End Function

End Class
