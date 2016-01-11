Imports System.Text

Namespace NSMd5Hasher
    ''' <summary>
    ''' MD5 hash provider
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Md5Hasher
        ''' <summary>
        ''' Calculate the MD5 hash of a given string 
        ''' </summary>
        ''' <param name="target">String to be hashed</param>
        ''' <returns>32 character string of the hash</returns>
        ''' <remarks></remarks>
        Public Function Hash(ByVal target As String) As String
            Dim objMD5 As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim arrData() As Byte
            Dim arrHash() As Byte

            ' first convert the string to bytes (using UTF8 encoding for unicode characters)
            arrData = Encoding.UTF8.GetBytes(target)

            ' hash contents of this byte array
            arrHash = objMD5.ComputeHash(arrData)

            ' thanks objects
            objMD5 = Nothing

            ' return formatted hash
            Return ByteArrayToString(arrHash).ToUpper()
        End Function

        ''' <summary>
        ''' utility function to convert a byte array into a hex string
        ''' </summary>
        ''' <param name="arrInput"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ByteArrayToString(ByVal arrInput() As Byte) As String

            Dim strOutput As New System.Text.StringBuilder(arrInput.Length)

            For i As Integer = 0 To arrInput.Length - 1
                strOutput.Append(arrInput(i).ToString("X2"))
            Next

            Return strOutput.ToString().ToLower
        End Function
    End Class
End Namespace