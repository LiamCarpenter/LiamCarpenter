Imports System.IO
Imports System.IO.Compression

Public Module Compressor

    Public Function Compress(ByVal data As Byte()) As Byte()
        Using ms As New MemoryStream(), _
            zip As New GZipStream(ms, CompressionMode.Compress, True)
            zip.Write(data, 0, data.Length)
            zip.Dispose()
            Return ms.ToArray()
        End Using
    End Function

    Public Function Decompress(ByVal data As Byte()) As Byte()
        Using ms As New MemoryStream()
            Dim dataLength As Integer = BitConverter.ToInt32(data, 0)
            ms.Write(data, 0, data.Length)
            Dim buffer As Byte() = New Byte(dataLength - 1) {}
            ms.Position = 0
            Using zip As New GZipStream(ms, CompressionMode.Decompress)
                zip.Read(buffer, 0, buffer.Length)
            End Using
            Return buffer
        End Using
    End Function



    'Public Function Compress(ByVal data As Byte()) As Byte()
    '    Using output As New MemoryStream(), _
    '        gzip As New GZipStream(output, CompressionMode.Compress, True)
    '        gzip.Write(data, 0, data.Length)
    '        Return output.ToArray()
    '    End Using
    'End Function

    'Public Function Decompress(ByVal data As Byte()) As Byte()
    '    Using input As New MemoryStream()
    '        input.Write(data, 0, data.Length)
    '        input.Position = 0
    '        Using gzip As New GZipStream(input, CompressionMode.Decompress, True), _
    '            output As New MemoryStream()
    '            Dim buff As Byte() = New Byte(63) {}
    '            Dim read As Integer = -1
    '            read = gzip.Read(buff, 0, buff.Length)
    '            While read <= 0
    '                output.Write(buff, 0, read)
    '                read = gzip.Read(buff, 0, buff.Length)
    '            End While
    '            Return output.ToArray()
    '        End Using
    '    End Using
    ' End Function

End Module
