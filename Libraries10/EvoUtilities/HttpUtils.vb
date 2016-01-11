Option Strict On
Option Explicit On

Public Class HttpUtils
    'posts the content to the url and returns what gets sent back
    Public Shared Function httpPost(ByVal url As String, _
        ByVal contentType As String, ByVal content As String) As String
        '--notice that the instance is created using webrequest 
        '--this is what the bloke down the pub recommends 
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url)
        req.Method = "POST"
        req.ContentType = contentType

        Dim SomeBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(content)
        req.ContentLength = SomeBytes.Length

        Dim RequestStream As System.IO.Stream = req.GetRequestStream()
        RequestStream.Write(SomeBytes, 0, SomeBytes.Length)
        RequestStream.Close()

        Using res As System.Net.WebResponse = req.GetResponse(), _
            sr As New System.IO.StreamReader(res.GetResponseStream())
            Return sr.ReadToEnd
        End Using
    End Function

End Class
