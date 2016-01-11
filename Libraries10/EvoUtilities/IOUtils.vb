Option Strict On
Option Explicit On

Imports system.io
Imports system.diagnostics.debug
Imports System.Text.RegularExpressions

Public Class IOUtils

    Public Shared ReadOnly filenameBadChars95 As String = "|\?*<"":>+[]/"
    Public Shared ReadOnly filenameBadCharsNT As String = "|\?*<"":>/"

    Public Shared Sub clearFolder(ByVal folderName As String, Optional ByVal recurse As Boolean = False)
        For Each f As String In dirList(folderName)
            If File.Exists(f) Then
                File.Delete(f)
            ElseIf recurse And Directory.Exists(f) Then
                Directory.Delete(f, True)
            End If
        Next
    End Sub

    'return the path relative to the root, only works if path is a subdir of root
    Public Shared Function getRelativePath(ByVal root As String, ByVal path As String) As String
        If root.EndsWith("\") Then
            root = root.Substring(0, root.Length - 1)
        End If
        If Not path.StartsWith(root) Then
            Throw New Exception("tried to get path of " & path & " relative to " & root & _
                ", but the path is not a subdir of the root")
        End If
        Return path.Substring(root.Length + 1)
    End Function

    'recursively get a list of all the files (but not directories) in a set of rootdirs
    Public Shared Function recurseAllFiles(ByVal roots As IList(Of String)) As IList(Of String)
        Dim ret As New List(Of String)
        For Each f As String In roots
            If File.Exists(f) Then
                ret.Add(f)
            ElseIf Directory.Exists(f) Then
                ret.AddRange(recurseAllFiles(dirList(f)))
            Else
                Throw New Exception("expecting file or folder but was something else(?): " & f)
            End If
        Next
        Return ret
    End Function

    'list all the files and directories in a folder
    Public Shared Function dirList(ByVal folder As String) As IList(Of String)
        Dim ret As New List(Of String)
        For Each f As DirectoryInfo In New DirectoryInfo(folder).GetDirectories
            ret.Add(f.FullName)
        Next
        For Each f As FileSystemInfo In New DirectoryInfo(folder).GetFiles
            ret.Add(f.FullName)
        Next
        Return ret
    End Function

    Public Shared Function sizeOfFile(ByVal f As String) As Long
        Return New FileInfo(f).Length
    End Function

    Public Shared Function getDirectorySizes(ByVal dirs As IList(Of String)) As Long
        Dim files As IList(Of String) = getFolderContentsRecursive(dirs)
        Dim ret As Long = 0
        For Each file As String In files
            ret = ret + sizeOfFile(file)
        Next
        Return ret
    End Function

    Private Shared Function getFolderContentsRecursive(ByVal paths As IList(Of String)) As List(Of String)
        Dim ret As New List(Of String)
        For Each p As String In paths
            getFolderContentsRecursive1(p, ret)
        Next
        Return ret
    End Function

    Private Shared Sub getFolderContentsRecursive1(ByVal path As String, ByVal l As List(Of String))
        For Each f As String In IO.Directory.GetFiles(path, "*", IO.SearchOption.AllDirectories)
            l.Add(f)
        Next
    End Sub


    Public Shared Function changeExtension(ByVal filename As String, ByVal extension As String) As String
        If filename = "" Then
            Throw New Exception("cannot change extension of empty string")
        End If
        Dim f As String = Path.GetFileName(filename)
        If f.LastIndexOf(".") > -1 Then
            f = f.Substring(0, f.LastIndexOf("."))
        End If
        f &= "." & extension
        If Path.GetDirectoryName(filename) <> "" Then
            Return Path.GetDirectoryName(filename) & "\" & f
        Else
            Return f
        End If
    End Function

    Private Shared ReadOnly badChars As New System.Text.RegularExpressions.Regex( _
    "[^A-Za-z0-9 \._\-]")
    'get rid of all chars not in the regex. Make sure the filename has at
    'least one character in the base part
    Public Shared Function makeFilenameSafe(ByVal filename As String) As String
        Dim ret As String = badChars.Replace(filename, "")
        Dim baseName As String = Path.GetFileNameWithoutExtension(ret)
        If baseName = "" Then
            Return "a" & Path.GetExtension(ret)
        Else
            Return ret
        End If
    End Function

    Public Shared Sub makeFolderExist(ByVal folder As String)
        If Not Directory.Exists(folder) Then
            Directory.CreateDirectory(folder)
        End If
    End Sub

    Public Shared Sub deleteFileIfExists(ByVal filename As String)
        If File.Exists(filename) Then
            File.Delete(filename)
        End If
    End Sub

    Public Shared Sub deleteDirIfExists(ByVal dirname As String)
        If Directory.Exists(dirname) Then
            Directory.Delete(dirname, True)
        End If
    End Sub

    Private Shared ReadOnly numberedFilenameRegex As New Regex( _
        "^(?<start>.*\.)(?<num>\d+)(?<end>\.[^.]*)$")

    Public Shared Sub moveToUnique(ByVal sourceFileName As String, ByVal targetFileName As String)
        While File.Exists(targetFileName)
            Dim m As Match = numberedFilenameRegex.Match(targetFileName)
            If m.Success Then
                targetFileName = m.Groups.Item("start").Value & _
                    CInt(m.Groups.Item("num").Value) + 1 & _
                    m.Groups.Item("end").Value
            Else
                targetFileName = Path.GetDirectoryName(targetFileName) & "\" & _
                    Path.GetFileNameWithoutExtension(targetFileName) & _
                    ".0" & Path.GetExtension(targetFileName)
            End If
        End While
        File.Move(sourceFileName, targetFileName)
    End Sub

    'Public Shared Sub directoryCopy(ByVal source As String, ByVal dest As String)
    '    If Not dest.EndsWith("/") Then
    '        dest = dest & "/"
    '    End If
    '    makeFolderExist(dest)
    '    For Each filename As String In Directory.GetFiles(source)
    '        File.Copy(filename, dest & Path.GetFileName(filename))
    '    Next
    '    For Each dirname As String In Directory.GetDirectories(source)
    '        dim target as string  = dirname.substring(source.
    '        directoryCopy(dirname, dest & 
    '    Next
    'End Sub

End Class
