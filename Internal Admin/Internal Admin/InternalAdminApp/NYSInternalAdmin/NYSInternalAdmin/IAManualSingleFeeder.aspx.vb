Imports NysDat
Partial Public Class IASingleFeeder
    Inherits clsNYS

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnCreateFile_Click(sender As Object, e As EventArgs) Handles btnCreateFile.Click
        Try
            FeederFileCreation.createSingleFileCIMA(txtInvoiceNumber.Text)


            'Dim strDateFolder As String = ""

            ''submit the file
            'Dim strError As New StringBuilder
            'Dim strPathX As String = getConfig("XMLFilePath") & "\CIMA\CIMA\" & Format(Now, "dd-MM-yyyy") & "\"
            'Dim strPathXMove As String = getConfig("XMLFilePath") & "\CIMA\CIMA\" & Format(Now, "dd-MM-yyyy") & "\SENT\"
            'Dim strPathCEL As String = getConfig("XMLFilePath") & "\CIMA\CEL\" & Format(Now, "dd-MM-yyyy") & "\"
            'Dim strPathCELMove As String = getConfig("XMLFilePath") & "\CIMA\CEL\" & Format(Now, "dd-MM-yyyy") & "\SENT\"
            'Dim strPathCREDITS As String = getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\"
            'Dim strPathCREDITSMove As String = getConfig("XMLFilePath") & "\CIMA\CREDITS\" & Format(Now, "dd-MM-yyyy") & "\SENT\"


            'Dim strFilePath As String = ""
            'Dim strMoveFolder As String = ""
            'If System.IO.File.Exists(strPathX & "NYS" & txtInvoiceNumber.Text) Then
            '    strFilePath = strPathX & "NYS" & txtInvoiceNumber.Text
            '    strMoveFolder = strPathXMove
            'ElseIf System.IO.File.Exists(strPathCEL & "NYS" & txtInvoiceNumber.Text) Then
            '    strFilePath = strPathCEL & "NYS" & txtInvoiceNumber.Text
            '    strMoveFolder = strPathCELMove
            'ElseIf System.IO.File.Exists(strPathCREDITS & "NYS" & txtInvoiceNumber.Text) Then
            '    strFilePath = strPathCREDITS & "NYS" & txtInvoiceNumber.Text
            '    strMoveFolder = strPathCREDITSMove
            'End If


            'Dim strJustFileName As String = New System.IO.FileInfo(strFilePath).Name

            'Dim oFile As System.IO.StreamReader
            'oFile = New System.IO.StreamReader(strFilePath)

            'Dim strRepName As String = oFile.ReadToEnd
            'oFile.Close()

            'strRepName = strRepName.Trim
            'If strRepName <> "" Then
            '    Dim strRet As String = ""
            '    If getConfig("BatchTest") = "true" Then
            '        strRet = "Status code=""200"" text=""OK"""
            '    Else
            '        strRet = IAFeederFile.sendCimaXml(strRepName, False)
            '    End If

            '    If Not strRet.Contains("Status code=""200"" text=""OK""") Then
            '        strError.Append(strJustFileName & " - " & strRet & vbCrLf)
            '    Else
            '        IO.File.Move(strFilePath, strMoveFolder & strJustFileName)

            '        'R2.5 CR
            '        'Save invoice to db table for records
            '        Dim oCIMAInvoice As New clsCIMASentInvoice
            '        oCIMAInvoice.CIMASentInvoiceID = 0
            '        oCIMAInvoice.DateSent = Date.Now().ToString("dd/MM/yyyy")
            '        oCIMAInvoice.InvoiceRef = strJustFileName.Replace("NYS", "").Replace(".xml", "")
            '        oCIMAInvoice.save()
            '        oCIMAInvoice = Nothing
            '    End If
            'End If

            'lblError.Text = strError.ToString
        Catch ex As Exception
            lblError.Text = "An Error occurred: " & ex.Message
        End Try

    End Sub
End Class