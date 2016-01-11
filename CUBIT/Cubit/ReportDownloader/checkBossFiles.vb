Option Explicit On
Option Strict On

Imports DatabaseObjects
Imports System.Web
Imports EvoUtilities.ConfigUtils

<Assembly: CLSCompliant(True)> 
''' <summary>
''' Class checkBossFiles - used to check a specific directory for returned 
''' text files from the BOSS application
''' </summary>
''' <remarks>
''' Created 13/04/2009 Nick Massarella
''' </remarks>
Public Class checkBossFiles

    Public Function checker() As Boolean
        Dim ofile As System.IO.StreamReader
        'Dim ofiler As System.IO.StreamWriter
        Dim strJustFile As String
        Dim strRepName As String
        Dim strpath As String = getConfig("BossFilesToImport")
        Dim strmovepath As String = getConfig("BossFilesToImportOK") '& "BOSS_INV_FILES_CONFERMA/Imported/"
        Dim strerrorpath As String = getConfig("BossFilesToImportError") '& "BOSS_INV_FILES_CONFERMA/Error/"
        Dim intErrorCount As Integer = 0

        If IO.Directory.Exists(strpath) Then
            For Each strfile As String In IO.Directory.GetFiles(strpath)
                strJustFile = New System.IO.FileInfo(strfile).Name

                ofile = New System.IO.StreamReader(strpath & strJustFile)
                strRepName = ofile.ReadToEnd
                ofile.Close()
                strRepName = strRepName.Replace("c", "C")
                If strRepName <> "" And strRepName.Contains("C") Then
                    strRepName = Mid(strRepName, 1, Len(strRepName) - 2)
                    Dim indexofC As Integer = strRepName.ToUpper.IndexOf("C")
                    Dim indexofSpace As Integer = strRepName.IndexOf(" ")
                    Dim indexofFirstComma As Integer = strRepName.IndexOf(",")
                    Dim indexofLastComma As Integer = strRepName.LastIndexOf(",")

                    'CR
                    ' FIX: found if last comma is also first comma then exception is thrown
                    If indexofFirstComma = indexofLastComma Then
                        indexofLastComma = strRepName.Length
                    End If

                    Dim strboss As String = Mid(strRepName, 1, indexofC)
                    Dim strinvoice As String = Mid(strRepName, indexofC + 1, indexofSpace - indexofC)
                    Dim strvalue As String = Mid(strRepName, indexofSpace + 1, _
                                        indexofFirstComma - indexofSpace)
                    strvalue = strvalue.Trim

                    Dim strcommnett As String = Mid(strRepName, indexofFirstComma + 2, _
                                        indexofLastComma - (indexofFirstComma + 1))
                    Dim strcommvat As String = Mid(strRepName, indexofLastComma + 2)
                    'strcommvat = strcommvat.ToLower.ToLower.Replace("m", "").Replace("e", "").Replace("g", "").Replace("a", "").Replace("s", "").Replace("w", "")
                    strinvoice = strinvoice.Replace("C", "").Replace("c", "")
                    Try
                        Dim intInvoiceID As Integer = CInt(strinvoice)
                        'Dim dbltest As Double = CDbl(strvalue)
                        'Dim dbltest2 As Double = CDbl(strcommnett)
                        'Dim dbltest3 As Double = CDbl(strcommvat)
                        'POPULATE SAVED VALUES
                        Dim ts As FeedInvoice
                        ts = FeedInvoice.bookingTotalValue(0, intInvoiceID)
                        'now save values to DB

                        'CR
                        If clsFeedTransactionInvoice.saveTrans(ts.transactionnumber, intInvoiceID, strboss, _
                                                           clsStuff.notDouble(strvalue), clsStuff.notDouble(strcommnett), _
                                                           clsStuff.notDouble(strcommvat), False) > 0 Then
                            'check values are ok
                            Dim dbldiff As Double = clsStuff.notDouble(ts.dataitemgross) + clsStuff.notDouble(ts.transactionfee) - clsStuff.notDouble(strvalue)
                            dbldiff = Math.Abs(dbldiff)
                            If CDbl(dbldiff.ToString("N2")) >= 0 And CDbl(dbldiff.ToString("N2")) <= 0.02 Then
                                SetBookingIDtoExportStatus(ts.transactionnumber, True)
                                IO.File.Move(strpath & strJustFile, strmovepath & strJustFile)
                            Else
                                intErrorCount += 1
                                SetBookingIDtoExportStatus(ts.transactionnumber, False)
                                IO.File.Move(strpath & strJustFile, strerrorpath & strJustFile)
                            End If
                        Else
                            intErrorCount += 1
                            IO.File.Move(strpath & strJustFile, strerrorpath & strJustFile)
                            SetBookingIDtoExportStatus(ts.transactionnumber, False)
                        End If
                    Catch ex As Exception
                        intErrorCount += 1
                        IO.File.Move(strpath & strJustFile, strerrorpath & strJustFile)
                    End Try
                End If
            Next
        End If

        If intErrorCount > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Sub SetBookingIDtoExportStatus - Updates FeedData record status on import 
    ''' from BOSS setting correct status if failure or not occurs
    ''' </summary>
    ''' <param name="pinttransactionnumber"></param>
    ''' <param name="pblnOk"></param>
    ''' <remarks>
    ''' Created 27/03/2009 Nick Massarella
    ''' </remarks>
    Private Sub SetBookingIDtoExportStatus(ByVal pinttransactionnumber As Integer, ByVal pblnOk As Boolean)
        If pblnOk Then
            FeedImportData.SetBookingIDtoExportStatus(pinttransactionnumber, FeedStatus.getStatusID("Completed"))
        Else
            FeedImportData.SetBookingIDtoExportStatus(pinttransactionnumber, FeedStatus.getStatusID("Incomplete"))
        End If
    End Sub
End Class
