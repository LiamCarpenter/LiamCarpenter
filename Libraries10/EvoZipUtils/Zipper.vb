Option Strict On
Option Explicit On

Imports system.io
Imports EvoUtilities.IOUtils
Imports system.diagnostics.debug
Imports evoutilities

Public Class Zipper
    'creates a zip file containing all the files and directories recursively in the files argument
    'paths are created relative to the root argument
    Public Shared Sub createZipFile(ByVal filename As String, _
        ByVal files As List(Of String), ByVal root As String)
        Using f As FileStream = File.Create(filename), _
            s As New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(f)
            Dim buffer(4096) As Byte
            s.SetLevel(9) ' 0 - store only to 9 - means best compression

            Dim list As ICollection(Of String) = recurseAllFiles(files)
            WriteLine(vbNewLine & "file list" & vbNewLine)
            'For Each t As String In list
            'WriteLine(t)
            'Next
            For Each file As String In list
                Dim entryFileName As String = getRelativePath(root, file)
                WriteLine(entryFileName)
                Dim entry As New ICSharpCode.SharpZipLib.Zip.ZipEntry(entryFileName)
                s.PutNextEntry(entry)
                Using fs As FileStream = System.IO.File.OpenRead(file)
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(fs, s, buffer)
                End Using
            Next
            s.Finish()
            s.Close()
        End Using
    End Sub

    'create a zip file containing one file only
    Public Shared Sub createZipFile(ByVal filename As String, ByVal fileToZip As String)
        createZipFile(filename, CollectionUtils.makeList(fileToZip), _
            Path.GetDirectoryName(fileToZip))
    End Sub

    'list contents of a zip file
    Public Shared Function listZipFile(ByVal filename As String) As List(Of String)
        Using f As FileStream = File.Open(filename, FileMode.Open), _
            istr As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(f)
            Dim ret As New List(Of String)
            While True
                Dim entry As ICSharpCode.SharpZipLib.Zip.ZipEntry = istr.GetNextEntry()
                If entry Is Nothing Then
                    Exit While
                End If
                ret.Add(entry.Name)
            End While
            Return ret
        End Using
    End Function

    Public Shared Sub extractZipFile(ByVal zipFile As String, ByVal targetDir As String)
        Dim fz As New ICSharpCode.SharpZipLib.Zip.FastZip()
        fz.ExtractZip(zipFile, targetDir, "")
    End Sub

End Class
