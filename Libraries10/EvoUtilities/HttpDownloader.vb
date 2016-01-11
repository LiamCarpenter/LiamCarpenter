Option Strict On
Option Explicit On

Imports System.IO


Public Class HttpDownloader


    Public Shared Function readStream(ByVal str As Stream) As String
        Using sr As New System.IO.StreamReader(str)
            Return sr.ReadToEnd
        End Using
    End Function

    'gets content from a url
    Public Shared Function [get](ByVal url As String) As String
        '--notice that the instance is created using webrequest 
        '--this is what the bloke down the pub recommends 
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url)
        req.Method = "GET"

        'Dim SomeBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(content)
        'req.ContentLength = SomeBytes.Length
        'writeStream(req.GetRequestStream(), SomeBytes)

        Using res As System.Net.WebResponse = req.GetResponse()
            'read in the page 
            Return readStream(res.GetResponseStream())
        End Using
    End Function

End Class

