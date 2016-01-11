Imports System
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Module EncryptModule

    Public Class EncryptSimple

        'Public Shared Function EncryptParam(ByVal pEncodedKey As String, ByVal pLoginNme As String) As String

        '    Dim crypt As New Chilkat.Crypt2()
        '    Dim success As Boolean
        '    success = crypt.UnlockComponent("NYSGROCrypt_pafhxYhyTVKZ")
        '    If (success <> True) Then
        '        'MsgBox("Crypt component unlock failed")
        '        'Exit Function
        '    End If

        '    crypt.CipherMode = "ecb"
        '    crypt.KeyLength = 256
        '    crypt.PaddingScheme = 3
        '    crypt.SetEncodedKey(pEncodedKey, "hex")
        '    crypt.EncodingMode = "hex"

        '    Dim encText As String
        '    Dim timer As String = Format(Now.AddHours(-1), "yyyyMMddHHmmss")
        '    encText = crypt.EncryptStringENC("ENC_TIME=" & timer & "&LOGINNAME=" & pLoginNme)

        '    Return encText
        'End Function

        'Public Shared Function DecryptParam(ByVal pEncodedKey As String, ByVal pToken As String) As String

        '    Dim crypt As New Chilkat.Crypt2()
        '    Dim success As Boolean
        '    success = crypt.UnlockComponent("NYSGROCrypt_pafhxYhyTVKZ")
        '    If (success <> True) Then
        '        'MsgBox("Crypt component unlock failed")
        '        'Exit Function
        '    End If

        '    crypt.CipherMode = "ecb"
        '    crypt.KeyLength = 256
        '    crypt.PaddingScheme = 3
        '    crypt.SetEncodedKey(pEncodedKey, "hex")
        '    crypt.EncodingMode = "hex"

        '    Dim encText As String

        '    encText = crypt.DecryptStringENC(pToken)

        '    Return encText
        'End Function

        Public Shared Function getKey(ByVal pstrPlainKey As String) As String
            'use below to create Key, this will be sent to Amadeus
            Dim bytes_Input As Byte()
            bytes_Input = CreateKey(pstrPlainKey) 'ETravelKey926 - used for test site - ETravelKeyNSG640 for NSG
            Return BytesToHexString(bytes_Input)
        End Function

        Public Shared Function BytesToHexString(ByVal bytes As Byte()) As String
            Dim hexString As StringBuilder = New StringBuilder(256)
            Dim counter As Integer

            For counter = 0 To bytes.Length - 1
                hexString.Append(String.Format("{0:X2}", bytes(counter)))
            Next

            Return hexString.ToString()
        End Function

        Public Shared Function CreateKey(ByVal strPassword As String) As Byte()
            'Convert strPassword to an array and store in chrData.
            Dim chrData() As Char = strPassword.ToCharArray
            'Use intLength to get strPassword size.
            Dim intLength As Integer = chrData.GetUpperBound(0)
            'Declare bytDataToHash and make it the same size as chrData.
            Dim bytDataToHash(intLength) As Byte

            'Use For Next to convert and store chrData into bytDataToHash.
            For i As Integer = 0 To chrData.GetUpperBound(0)
                bytDataToHash(i) = CByte(Asc(chrData(i)))
            Next

            'Declare what hash to use.
            Dim SHA512 As New System.Security.Cryptography.SHA512Managed
            'Declare bytResult, Hash bytDataToHash and store it in bytResult.
            Dim bytResult As Byte() = SHA512.ComputeHash(bytDataToHash)
            'Declare bytKey(31).  It will hold 256 bits.
            Dim bytKey(31) As Byte

            'Use For Next to put a specific size (256 bits) of 
            'bytResult into bytKey. The 0 To 31 will put the first 256 bits
            'of 512 bits into bytKey.
            For i As Integer = 0 To 31
                bytKey(i) = bytResult(i)
            Next

            Return bytKey 'Return the key.
        End Function
    End Class

End Module
