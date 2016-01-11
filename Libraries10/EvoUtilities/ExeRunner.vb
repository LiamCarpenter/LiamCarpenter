Public Class ExeRunner

    Public Shared Function runExe(ByVal filename As String, ByVal arguments As String) As String
        Dim pi As New ProcessStartInfo
        pi.RedirectStandardOutput = True
        Dim p As New Process()
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.RedirectStandardError = True
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.FileName = filename
        'p.StartInfo.Arguments = """" & arguments & """"
        p.StartInfo.Arguments = arguments
        p.Start()
        '// Do not wait for the child process to exit before
        '// reading to the end of its redirected stream.
        '// Read the output stream first and then wait.
        Dim output As String = p.StandardOutput.ReadToEnd()
        Dim errors As String = p.StandardError.ReadToEnd()
        p.WaitForExit()
        If p.ExitCode <> 0 Then
            Throw New Exception("error running " & filename & " - " & errors)
        End If
        Return output
    End Function

End Class
