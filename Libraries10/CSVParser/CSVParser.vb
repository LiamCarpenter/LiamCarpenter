Option Strict On
Option Explicit On 

Imports System.io
Imports System.diagnostics.Debug
Imports System.text
Imports System.text.regularexpressions
Imports EvoUtilities.CollectionUtils

Public Class CSVParser

    Public Sub New(ByVal s As String)
        content = parseCSVRet(s)
    End Sub
    Private content As IList(Of IList(Of String))
    Private mCurrentLine As Integer = 0
    Public Function readLine() As IList(Of String)
        If mCurrentLine >= content.Count Then
            Return Nothing
        Else
            mCurrentLine += 1
            Return content.Item(mCurrentLine - 1)
        End If
    End Function

    Private Class StrPtr
        Public Sub New(ByVal s As String)
            str = s
            ptr = 0
        End Sub

        Public str As String
        Public ptr As Integer

        Public lineNo As Integer

        Private Shared ReadOnly LF As Char = vbLf.Chars(0)
        Private Shared ReadOnly CR As Char = vbCr.Chars(0)
        Private Const DQ As Char = """"c
        Private Const COMMA As Char = ","c

        Public Function isEof() As Boolean
            Return ptr >= str.Length
        End Function

        'if the next char/ chars is a lf or crlf, then advance ptr
        'and return true else return false
        Public Function readNewline() As Boolean
            Select Case str.Chars(ptr)
                Case LF
                    ptr += 1
                    lineNo += 1
                    Return True
                Case CR
                    ptr += 1
                    If str.Chars(ptr) = LF Then
                        ptr += 1
                    End If
                    lineNo += 1
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        Public Function substrFrom(ByVal startPos As Integer) As String
            Return str.Substring(startPos, ptr - startPos)
        End Function

        'called to get the quoted string out:
        'the last char is the closing quote so strip that off
        'and replace all pairs of quotes with one quote
        Public Function substrFromFixDQDQ(ByVal startPos As Integer) As String

            Return str.Substring(startPos, ptr - startPos - 1).Replace("""", "")
            ' Return str.Substring(startPos, ptr - startPos - 1).Replace("""""", """")
        End Function

        Public Function readField() As readFieldRet
            'options are: 
            readWs()
            If isEof() Then
                Return New readFieldRet("", True)
            End If
            'EOL: return "", true
            '',' return "", false
            ', read quoted string, return s,false
            If readNewline() Then
                Return New readFieldRet("", True)
            End If
            'str = str.Replace("""", "")
            Select Case str.Chars(ptr)
                Case """"c
                    'read quoted string
                    ptr += 1
                    Dim startPos As Integer = ptr
                    While Not isEof()
                        If str.Chars(ptr) = DQ Then
                            'if the next character is also quote then keep going
                            'otherwise we've found the end of the field
                            'check for eol or eof after quote
                            ptr += 1
                            If isEof() Then 'last quote in file
                                Return New readFieldRet( _
                                    substrFromFixDQDQ(startPos), True)
                            End If
                            If str.Chars(ptr) = DQ Then
                                'double quote so continue reading field
                                ptr += 1
                            Else
                                'we've got to the end of the field
                                'check for whitespace followed by new line or eof
                                Dim field As String = substrFromFixDQDQ(startPos)
                                readWs()
                                If isEof() OrElse readNewline() Then
                                    Return New readFieldRet(field, True)
                                Else
                                    'expect a comma
                                    If Not str.Chars(ptr) = COMMA Then
                                        Throw New SyntaxException(lineNo, "Junk after quoted field before comma")
                                    End If
                                    ptr += 1
                                    Return New readFieldRet(field, False)
                                End If
                            End If
                        Else
                            If str.Chars(ptr) = LF Then
                                lineNo += 1
                            End If
                            ptr += 1
                        End If
                    End While
                    'can only get to here if we started a quoted field
                    'which wasn't closed before the end of the file
                    Throw New SyntaxException(lineNo, "parse error: unclosed quoted field")
                Case Else
                    'keep going till eof, eol or comma
                    Dim startPos As Integer = ptr
                    While True
                        If isEof() OrElse readNewline() Then
                            'eol or eof, treat the same: return all the text
                            ' to this point
                            'plus true to say the record has finished
                            'we've already stripped the whitespace
                            'from the start of the field, since this wasn't 
                            'quoted, we need to strip the whitespace
                            'from the end of the line, trim will
                            'do the job
                            Return New readFieldRet( _
                                substrFrom(startPos).Trim, True)
                        ElseIf str.Chars(ptr) = COMMA Then
                            'found a comma - return chars plus false
                            'to say not the end of the record
                            Dim field As String = substrFrom(startPos).Trim
                            ptr += 1
                            Return New readFieldRet(field, False)
                        ElseIf str.Chars(ptr) = DQ Then
                            Throw New SyntaxException(lineNo, "quotes used in non quoted field")
                        End If
                        ptr += 1
                    End While
            End Select
            'don't know how it's possible to get to here
            Throw New SyntaxException(lineNo, "unknown parse error")
        End Function

        Public Sub readWs()
            While Not isEof() AndAlso _
                inList(str.Chars(ptr), vbTab.Chars(0), " "c)
                ptr += 1
            End While
        End Sub

    End Class

    Public Shared Function parseCSVRet(ByVal csv As String) As IList(Of IList(Of String))
        Dim ret As New List(Of IList(Of String))
        If csv.Length = 0 Then
            Return ret
        End If
        Dim sp As New StrPtr(csv)
        Dim line As New List(Of String)
        While Not sp.isEof
            Dim rf As readFieldRet = sp.readField()
            line.Add(rf.fieldValue)
            If rf.endOfLine Or sp.isEof Then
                ret.Add(line)
                line = New List(Of String)
            End If
        End While
        'hack: if the last character was a comma then add a final field
        If sp.str.Chars(sp.str.Length - 1) = ","c Then
            ret.Item(ret.Count - 1).Add("")
        End If
        Return ret
    End Function

    Public Shared Function parseCSV(ByVal csv As String) As IEnumerable
        Return New ParseCsvEnumerable(New CSVParser(csv))
    End Function


    Private Class ParseCsvEnumerable
        Implements IEnumerable

        Private mcsvparser As CSVParser
        Public Sub New(ByVal m As CSVParser)
            mcsvparser = m
        End Sub
        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New ParseCsvEnumerator(mcsvparser)
        End Function
    End Class

    Private Class ParseCsvEnumerator
        Implements IEnumerator

        Private mcsvparser As CSVParser
        Public Sub New(ByVal m As CSVParser)
            mcsvparser = m
        End Sub

        Private mCurrentLine As IList(Of String)

        Public ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
            Get
                Return mCurrentLine
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
            mCurrentLine = mcsvparser.readLine
            Return mCurrentLine IsNot Nothing
        End Function

        Public Sub Reset() Implements System.Collections.IEnumerator.Reset
            Throw New Exception("no reset")
        End Sub
    End Class

    'returns next field, if this is the last field on the line then set endof line to true
    Private Structure readFieldRet
        Public Sub New(ByVal fv As String, ByVal eol As Boolean)
            fieldValue = fv
            endOfLine = eol
        End Sub
        Public fieldValue As String
        Public endOfLine As Boolean
    End Structure

End Class
